/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Security;
using ZWave.Xml.Application;

namespace ZWave.ZnifferApplication
{
    public class SecurityManager
    {
        private readonly object _locker = new object();
        private List<NetworkKey> _securityKeys;
        private List<NetworkKey> _tempSecurityKeys;

        private List<byte[]> _newSecurityKeys;
        private List<byte[]> _newTempSecurityKeys;

        private ConcurrentDictionary<int, SpanCache> _spanCacheDictionary;
        private ConcurrentDictionary<int, MpanCache> _mpanCacheDictionary;
        private ConcurrentDictionary<int, KeysCache> _nonceS0CacheDictionary;
        private SecurityS0CryptoProviderBase _securityS0 { get; set; }
        private SecurityS2CryptoProviderBase _securityS2 { get; set; }
        public MpanTable MpanTable { get; set; }
        public SpanTable SpanTable { get; set; }
        public Dictionary<InvariantPeerNodeId, SinglecastKey> ScKeys { get; set; }
        public Dictionary<NodeGroupId, MulticastKey> McKeys { get; set; }

        public SecurityManager()
        {
            _newTempSecurityKeys = new List<byte[]>();
            _newSecurityKeys = new List<byte[]>();
            _tempSecurityKeys = new List<NetworkKey>();
            _securityKeys = new List<NetworkKey>();
            AddPresetKeys();
            _spanCacheDictionary = new ConcurrentDictionary<int, SpanCache>();
            _mpanCacheDictionary = new ConcurrentDictionary<int, MpanCache>();
            _nonceS0CacheDictionary = new ConcurrentDictionary<int, KeysCache>();
            MpanTable = new MpanTable();
            SpanTable = new SpanTable();
            ScKeys = new Dictionary<InvariantPeerNodeId, SinglecastKey>();
            McKeys = new Dictionary<NodeGroupId, MulticastKey>();
            _securityS0 = new SecurityS0CryptoProviderBase();
            _securityS2 = new SecurityS2CryptoProviderBase();
        }

        public void Reset()
        {
            _newTempSecurityKeys.Clear();
            _newSecurityKeys.Clear();
            _tempSecurityKeys.Clear();
            _securityKeys.Clear();
            AddPresetKeys();
            _spanCacheDictionary.Clear();
            _mpanCacheDictionary.Clear();
            _syncIdCounter = 0;
            nonceS0Storage.Clear();
            nonceS0Part0Storage.Clear();
            encapS0Part0Storage.Clear();
            spanStorage.Clear();
            spanStorageBk.Clear();
            mpanValueStorage.Clear();
        }

        private void AddPresetKeys()
        {
            _securityKeys.AddRange(new[]
            {
                new NetworkKey("01010101010101010101010101010101".GetBytes()),
                new NetworkKey("C0000000000000000000000000000000".GetBytes()),
                new NetworkKey("C1000000000000000000000000000000".GetBytes()),
                new NetworkKey("C2000000000000000000000000000000".GetBytes()),
                new NetworkKey("C3000000000000000000000000000000".GetBytes()),
                new NetworkKey("C4000000000000000000000000000000".GetBytes()),
                new NetworkKey("00000000000000000000000000000000".GetBytes()),
            });
        }

        private Dictionary<int, Dictionary<OrdinalPeerNodeId, byte[]>> nonceS0Part0Storage = new Dictionary<int, Dictionary<OrdinalPeerNodeId, byte[]>>();
        private Dictionary<int, Dictionary<OrdinalPeerNodeId, byte[]>> encapS0Part0Storage = new Dictionary<int, Dictionary<OrdinalPeerNodeId, byte[]>>();
        private Dictionary<int, Dictionary<OrdinalPeerNodeId, NonceS0Table>> nonceS0Storage = new Dictionary<int, Dictionary<OrdinalPeerNodeId, NonceS0Table>>();
        private Dictionary<int, Dictionary<InvariantPeerNodeId, SpanRecord>> spanStorage = new Dictionary<int, Dictionary<InvariantPeerNodeId, SpanRecord>>();
        private Dictionary<int, Dictionary<InvariantPeerNodeId, SpanRecord>> spanStorageBk = new Dictionary<int, Dictionary<InvariantPeerNodeId, SpanRecord>>();
        private Dictionary<int, Dictionary<NodeGroupId, Dictionary<int, MpanValue>>> mpanValueStorage = new Dictionary<int, Dictionary<NodeGroupId, Dictionary<int, MpanValue>>>();

        public int ParseSecurity(DataItem dataItem)
        {
            int ret = 0;
            if (dataItem.Destinations != null && dataItem.Destinations.Length > 0 && dataItem.LastPayload != null && dataItem.LastPayload.Length > 1 && dataItem.HomeId != null && dataItem.HomeId.Length == 4)
            {
                if (dataItem.LastPayload[0] == COMMAND_CLASS_SECURITY.ID)
                {
                    if (dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID)
                    {
                        var peerNodeId = new OrdinalPeerNodeId(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination));
                        var homeId = BitConverter.ToInt32(dataItem.HomeId, 0);
                        COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT cmd = dataItem.LastPayload;
                        if (!nonceS0Storage.ContainsKey(homeId))
                        {
                            nonceS0Storage.Add(homeId, new Dictionary<OrdinalPeerNodeId, NonceS0Table>());
                        }
                        if (!nonceS0Storage[homeId].ContainsKey(peerNodeId))
                        {
                            nonceS0Storage[homeId].Add(peerNodeId, new NonceS0Table());
                        }
                        nonceS0Storage[homeId][peerNodeId].PutNonce(cmd.nonceByte.ToArray());
                    }
                    else
                        if (dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID ||
                            dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION_NONCE_GET.ID)
                    {
                        var peerNodeId = new OrdinalPeerNodeId(new NodeTag(dataItem.Destination), new NodeTag(dataItem.Source));
                        var homeId = BitConverter.ToInt32(dataItem.HomeId, 0);
                        COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION cmd = dataItem.LastPayload;
                        byte[] nonce = null;
                        if (nonceS0Storage.ContainsKey(homeId))
                        {
                            if (nonceS0Storage[homeId].ContainsKey(peerNodeId))
                            {
                                nonce = nonceS0Storage[homeId][peerNodeId].GetNonce(cmd.receiversNonceIdentifier);
                            }
                        }
                        if (nonce != null)
                        {
                            dataItem.AddExtension(DataItemExtensionTypes.NonceS0, nonce);
                            if (dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
                            {
                                byte[] noncePart0 = null;
                                byte[] encapPart0 = null;
                                if (nonceS0Part0Storage.ContainsKey(homeId))
                                {
                                    if (nonceS0Part0Storage[homeId].ContainsKey(peerNodeId))
                                    {
                                        noncePart0 = nonceS0Part0Storage[homeId][peerNodeId];
                                    }
                                }
                                peerNodeId = new OrdinalPeerNodeId(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination));
                                if (encapS0Part0Storage.ContainsKey(homeId))
                                {
                                    if (encapS0Part0Storage[homeId].ContainsKey(peerNodeId))
                                    {
                                        encapPart0 = encapS0Part0Storage[homeId][peerNodeId];
                                    }
                                }

                                if (noncePart0 != null && encapPart0 != null)
                                {
                                    dataItem.AddExtension(DataItemExtensionTypes.NonceS0Part0, noncePart0);
                                    dataItem.AddExtension(DataItemExtensionTypes.EncapS0Part0, encapPart0);
                                }
                            }
                            else if (dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION_NONCE_GET.ID)
                            {
                                if (!nonceS0Part0Storage.ContainsKey(homeId))
                                {
                                    nonceS0Part0Storage.Add(homeId, new Dictionary<OrdinalPeerNodeId, byte[]>());
                                }
                                if (!nonceS0Part0Storage[homeId].ContainsKey(peerNodeId))
                                {
                                    nonceS0Part0Storage[homeId].Add(peerNodeId, nonce);
                                }
                                else
                                {
                                    nonceS0Part0Storage[homeId][peerNodeId] = nonce;
                                }

                                peerNodeId = new OrdinalPeerNodeId(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination));
                                if (!encapS0Part0Storage.ContainsKey(homeId))
                                {
                                    encapS0Part0Storage.Add(homeId, new Dictionary<OrdinalPeerNodeId, byte[]>());
                                }
                                if (!encapS0Part0Storage[homeId].ContainsKey(peerNodeId))
                                {
                                    encapS0Part0Storage[homeId].Add(peerNodeId, dataItem.LastPayload);
                                }
                                else
                                {
                                    encapS0Part0Storage[homeId][peerNodeId] = dataItem.LastPayload;
                                }
                            }
                        }
                    }
                }
                else if (dataItem.LastPayload[0] == COMMAND_CLASS_SECURITY_2.ID)
                {
                    if (dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
                    {
                        var homeId = BitConverter.ToInt32(dataItem.HomeId, 0);
                        COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT cmd = dataItem.LastPayload;
                        if (cmd.properties1.sos > 0 && cmd.receiversEntropyInput != null && cmd.receiversEntropyInput.Count == 16)
                        {
                            var peerNodeId = new InvariantPeerNodeId(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination));
                            if (!spanStorage.ContainsKey(homeId))
                            {
                                spanStorage.Add(homeId, new Dictionary<InvariantPeerNodeId, SpanRecord>());
                            }
                            if (!spanStorage[homeId].ContainsKey(peerNodeId))
                            {
                                spanStorage[homeId].Add(peerNodeId, null);
                            }
                            else
                            {
                                if (!spanStorageBk.ContainsKey(homeId))
                                {
                                    spanStorageBk.Add(homeId, new Dictionary<InvariantPeerNodeId, SpanRecord>());
                                }
                                if (!spanStorageBk[homeId].ContainsKey(peerNodeId))
                                {
                                    spanStorageBk[homeId].Add(peerNodeId, spanStorage[homeId][peerNodeId]);
                                }
                                else
                                {
                                    spanStorageBk[homeId][peerNodeId] = spanStorage[homeId][peerNodeId];
                                }
                            }
                            var span = new SpanRecord
                            {
                                SyncId = NextSyncId(),
                                Generation = 0,
                            };
                            ret = span.SyncId;
                            dataItem.AddExtension(DataItemExtensionTypes.Span, span);
                            span.ReceiverValue = cmd.receiversEntropyInput.ToArray();
                            spanStorage[homeId][peerNodeId] = span;
                        }
                    }
                    else if (dataItem.LastPayload[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
                    {
                        if (HeaderStore.GetIsSinglecast(dataItem.HeaderType) || HeaderStore.GetIsRoutedSinglecast(dataItem.HeaderType))
                        {
                            var homeId = BitConverter.ToInt32(dataItem.HomeId, 0);
                            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION cmd = dataItem.LastPayload;
                            var peerNodeId = new InvariantPeerNodeId(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination));
                            SpanRecord span = null;
                            if (spanStorage.ContainsKey(homeId))
                            {
                                if (spanStorage[homeId].ContainsKey(peerNodeId))
                                {
                                    span = spanStorage[homeId][peerNodeId];
                                }
                            }
                            if (span != null)
                            {
                                if (cmd.properties1.extension == 1)
                                {
                                    foreach (var extData in cmd.vg1)
                                    {
                                        switch (extData.properties1.type)
                                        {
                                            case (byte)ExtensionTypes.Span:
                                                if (extData.extensionLength == 18 && extData.extension.Count == 16)
                                                {
                                                    if (span.SenderValue == null || span.SenderValue.Sum(x => x) == 0)
                                                    {
                                                        span.SenderValue = extData.extension.ToArray();
                                                    }
                                                }
                                                break;
                                            case (byte)ExtensionTypes.MpanGrp:
                                                if (extData.extensionLength == 3 && extData.extension.Count == 1)
                                                {
                                                    //
                                                }
                                                break;
                                            case (byte)ExtensionTypes.Mos:
                                                if (extData.extensionLength == 2 && extData.extension.Count == 0)
                                                {
                                                    //
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                if (span.SenderValue == null)
                                {
                                    if (spanStorageBk.ContainsKey(homeId))
                                    {
                                        if (spanStorageBk[homeId].ContainsKey(peerNodeId))
                                        {
                                            span = spanStorageBk[homeId][peerNodeId];
                                        }
                                    }
                                }

                                if (span.SenderValue != null)
                                {
                                    if (span.IsValidSequenceNumber(dataItem.Source, dataItem.Destination, cmd.sequenceNumber))
                                    {
                                        span.SetSequenceNumber(dataItem.Source, dataItem.Destination, cmd.sequenceNumber);
                                        span.Generation++;
                                    }
                                    dataItem.AddExtension(DataItemExtensionTypes.Span, span);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private int _syncIdCounter = 0;
        private int NextSyncId()
        {
            var ret = ++_syncIdCounter;
            return ret;
        }

        public bool DecryptS0(DataItem dataItem, out byte[] decryptKey, out byte[] payload, out byte decryptedProperties)
        {
            bool isDecrypted = false;
            decryptedProperties = 0;
            payload = null;
            byte[] payloadPart0 = null;
            byte decryptedPropertiesPart0 = 0;
            decryptKey = null;
            if (dataItem.Destinations != null &&
                dataItem.Destinations.Length > 0 &&
                dataItem.LastPayload != null &&
                dataItem.LastPayload.Length > 1 &&
                dataItem.NonceS0 != null)
            {
                try
                {
                    int homeId = BitConverter.ToInt32(dataItem.HomeId, 0);
                    var s0Cache = _nonceS0CacheDictionary.GetOrAdd(homeId, new KeysCache());
                    if (_securityKeys != null)
                    {
                        var keyNode = s0Cache.GetLastUsedValue();
                        for (int i = 0; i < s0Cache.GetValuesCount(); i++)
                        {
                            var key = keyNode.Value;
                            lock (_locker)
                            {
                                _securityS0.OnNetworkKeyS0Changed(key, false);
                                isDecrypted = _securityS0.DecryptFrame(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination), dataItem.HomeId, dataItem.LastPayload, dataItem.NonceS0, dataItem.LastPayload[1], out payload, out decryptedProperties);
                            }
                            if (isDecrypted)
                            {
                                decryptKey = key;
                                s0Cache.SetLastUsedValue(keyNode);
                                if (dataItem.EncapS0Part0 != null && dataItem.NonceS0Part0 != null && decryptedProperties > 0)
                                {
                                    var isDecryptedPart0 = _securityS0.DecryptFrame(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination), dataItem.HomeId, dataItem.EncapS0Part0, dataItem.NonceS0Part0, dataItem.EncapS0Part0[1], out payloadPart0, out decryptedPropertiesPart0);
                                    if (isDecryptedPart0)
                                    {
                                        var dp0 = (COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.Tproperties1)decryptedPropertiesPart0;
                                        var dp1 = (COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.Tproperties1)decryptedProperties;
                                        isDecrypted = dp0.sequenceCounter == dp1.sequenceCounter;
                                    }
                                }
                                break;
                            }
                            keyNode = s0Cache.GetNextValue(keyNode);
                        }
                        if (!isDecrypted)
                        {
                            var startIndex = 0;
                            var endIndex = _securityKeys.Count;
                            for (int keyIndex = startIndex; keyIndex < endIndex; keyIndex++)
                            {
                                var key = _securityKeys[keyIndex];
                                _securityS0.OnNetworkKeyS0Changed(key.Value, false);
                                isDecrypted = _securityS0.DecryptFrame(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination), dataItem.HomeId, dataItem.LastPayload, dataItem.NonceS0, dataItem.LastPayload[1], out payload, out decryptedProperties);
                                if (isDecrypted)
                                {
                                    decryptKey = key.Value;
                                    s0Cache.AddValue(key.Value);
                                    if (dataItem.EncapS0Part0 != null && dataItem.NonceS0Part0 != null)
                                    {
                                        var isDecryptedPart0 = _securityS0.DecryptFrame(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination), dataItem.HomeId, dataItem.EncapS0Part0, dataItem.NonceS0Part0, dataItem.EncapS0Part0[1], out payloadPart0, out decryptedPropertiesPart0);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

            if (payload != null && payloadPart0 != null && payloadPart0.Length > 0)
            {
                payload = payloadPart0.Concat(payload).ToArray();
            }

            return isDecrypted;
        }

        private void FireNetworkKeyS2Changed(InvariantPeerNodeId peerNodeId, byte[] networkKey, ZWave.Enums.SecuritySchemes securityScheme)
        {
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            if (securityScheme == ZWave.Enums.SecuritySchemes.S2_TEMP)
            {
                SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);
            }
            else
            {
                SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);
            }

            switch (peerNodeId.NodeId2.Id)
            {
                case 0:
                    foreach (var item in ScKeys)
                    {
                        if (!item.Value.CcmKey.SequenceEqual(ccmKey) &&
                            item.Value.SecurityScheme == securityScheme)
                        {
                            item.Value.Update(networkKey[0], ccmKey, personalization, securityScheme);
                            if (SpanTable.GetSpanState(item.Key) != SpanStates.ReceiversNonce)
                            {
                                SpanTable.SetNonceFree(item.Key);
                            }
                        }
                    }
                    //for (int ii = 0; ii <= ushort.MaxValue; ii++)
                    //{
                    //    var index = new InvariantPeerNodeId(ii);
                    //    if (ScKeys.ContainsKey(index) && !ScKeys[index].CcmKey.SequenceEqual(ccmKey) &&
                    //        ScKeys[index].SecurityScheme == securityScheme)
                    //    {
                    //        ScKeys[index].CcmKey = ccmKey;
                    //        ScKeys[index].Personalization = personalization;
                    //        if (SpanTable.GetSpanState(index) != SpanStates.ReceiversNonce)
                    //        {
                    //            SpanTable.SetNonceFree(index);
                    //        }
                    //    }
                    //}
                    foreach (var item in McKeys)
                    {
                        if (!item.Value.CcmKey.SequenceEqual(ccmKey) && item.Value.SecurityScheme == securityScheme)
                        {
                            MpanTable.RemoveRecord(item.Key);
                        }
                    }
                    //for (int i = 0; i <= ushort.MaxValue; i++)
                    //{
                    //    var index = new NodeGroupId(i);
                    //    if (McKeys.ContainsKey(index) && !McKeys[index].CcmKey.SequenceEqual(ccmKey) &&
                    //        McKeys[index].SecurityScheme == securityScheme)
                    //    {
                    //        MpanTable.RemoveRecord(index);
                    //    }
                    //}
                    break;
                case 0xFF:
                    foreach (var item in ScKeys)
                    {
                        item.Value.Update(networkKey[0], ccmKey, personalization, securityScheme);
                    }
                    //for (int ii = 0; ii <= ushort.MaxValue; ii++)
                    //{
                    //    var i = new InvariantPeerNodeId(ii);
                    //    if (ScKeys.ContainsKey(i))
                    //    {
                    //        ScKeys[i].CcmKey = ccmKey;
                    //        ScKeys[i].Personalization = personalization;
                    //        ScKeys[i].SecurityScheme = securityScheme;
                    //    }
                    //}
                    MpanTable.ClearMpanTable();
                    SpanTable.ClearNonceTable();
                    break;
                default:
                    if (ScKeys.ContainsKey(peerNodeId))
                    {
                        ScKeys[peerNodeId].Update(networkKey[0], ccmKey, personalization, securityScheme);
                    }
                    else
                    {
                        ScKeys.Add(peerNodeId, new SinglecastKey(networkKey[0], ccmKey, personalization, securityScheme));
                    }

                    if (SpanTable.GetSpanState(peerNodeId) != SpanStates.ReceiversNonce)
                    {
                        SpanTable.SetNonceFree(peerNodeId);
                    }
                    break;
            }
        }

        private void FireNetworkKeyS2ChangedMulti(byte groupId, NodeTag owner, byte[] networkKey, ZWave.Enums.SecuritySchemes securityScheme)
        {
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];

            SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            var peerGroupId = new NodeGroupId(owner, groupId);
            if (groupId != 0)
            {
                if (McKeys.ContainsKey(peerGroupId))
                {
                    McKeys[peerGroupId].CcmKey = ccmKey;
                    McKeys[peerGroupId].MpanKey = mpanKey;
                    McKeys[peerGroupId].SecurityScheme = securityScheme;
                }
                else
                {
                    McKeys.Add(peerGroupId, new MulticastKey { CcmKey = ccmKey, MpanKey = mpanKey, SecurityScheme = securityScheme });
                }
            }
        }

        public bool DecryptS2(DataItem dataItem, out byte[] decryptKey, out byte[] payload, out Extensions extensions)
        {
            bool isDecrypted = false;
            decryptKey = null;
            payload = null;
            extensions = null;
            var homeId = BitConverter.ToInt32(dataItem.HomeId, 0);
            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION cmd = dataItem.LastPayload;
            try
            {
                if (cmd.vg1 != null && (HeaderStore.GetIsBroadcast(dataItem.HeaderType) || HeaderStore.GetIsMulticast(dataItem.HeaderType)))
                {
                    extensions = new Extensions();
                    extensions.ExtensionsList = cmd.vg1;
                    var mpanGrp = cmd.vg1.FirstOrDefault(x => x.properties1.type == (byte)ExtensionTypes.MpanGrp);
                    if (mpanGrp != null && mpanGrp.extension != null && mpanGrp.extension.Count == 1)
                    {
                        NodeGroupId nodeGroupId = new NodeGroupId(new NodeTag(dataItem.Source), mpanGrp.extension[0]);
                        var mpanValue = GetMpan(dataItem.Position, dataItem.HomeId, nodeGroupId);
                        if (mpanValue != null)
                        {
                            var key = _securityKeys[mpanValue.KeyIndex];
                            FireNetworkKeyS2ChangedMulti(nodeGroupId.GroupId, nodeGroupId.Node, key.Value, ZWave.Enums.SecuritySchemes.S2_ACCESS);
                            isDecrypted = DecryptS2MulticastWithCache(dataItem, nodeGroupId, mpanValue.Value, out payload, out extensions);
                            if (isDecrypted)
                            {
                            }
                        }
                    }
                }
                else if (dataItem.Destinations != null && dataItem.Destinations.Length > 0)
                {
                    var peerNodeId = new InvariantPeerNodeId(new NodeTag(dataItem.Source), new NodeTag(dataItem.Destination));

                    if (dataItem.Span != null)
                    {
                        SpanRecord span = dataItem.Span;
                        var s2Cache = _spanCacheDictionary.GetOrAdd(span.SyncId, new SpanCache());
                        if (_securityKeys != null)
                        {
                            var keyNode = s2Cache.NetworkKeys.GetLastUsedValue();
                            for (int i = 0; i < s2Cache.NetworkKeys.GetValuesCount(); i++)
                            {
                                var key = keyNode?.Value;
                                lock (_locker)
                                {
                                    FireNetworkKeyS2Changed(peerNodeId, key, ZWave.Enums.SecuritySchemes.S2_ACCESS);
                                    isDecrypted = DecryptS2SinglecastWithCache(dataItem, peerNodeId, span, s2Cache, out payload, out extensions);
                                }
                                if (isDecrypted)
                                {
                                    decryptKey = key;
                                    s2Cache.NetworkKeys.SetLastUsedValue(keyNode);
                                    var mpanext = extensions.EncryptedExtensionsList.FirstOrDefault(x => x.properties1.type == (byte)ExtensionTypes.Mpan);
                                    if (mpanext != null && mpanext.extension != null && mpanext.extension.Count == 17)
                                    {
                                        MpanValue mpanValue = new MpanValue();
                                        mpanValue.Position = dataItem.Position;
                                        mpanValue.Value = mpanext.extension.Skip(1).ToArray();
                                        //mpanValue.KeyIndex = keyIndex;
                                        RegisterMpan(dataItem.Position, dataItem.HomeId, new NodeGroupId(new NodeTag(dataItem.Source), mpanext.extension[0]), mpanValue);
                                    }
                                    break;
                                }
                                keyNode = s2Cache.NetworkKeys.GetNextValue(keyNode);
                            }
                            if (!isDecrypted)
                            {
                                var startIndex = 0;
                                var endIndex = _securityKeys.Count;
                                for (int keyIndex = startIndex; keyIndex < endIndex; keyIndex++)
                                {
                                    var key = _securityKeys[keyIndex];
                                    if (key.SyncIdBanlist.GetPosition(span.SyncId))
                                    {
                                        continue;
                                    }
                                    lock (_locker)
                                    {
                                        FireNetworkKeyS2Changed(peerNodeId, key.Value, ZWave.Enums.SecuritySchemes.S2_ACCESS);
                                        isDecrypted = DecryptS2SinglecastWithCache(dataItem, peerNodeId, span, s2Cache, out payload, out extensions);
                                    }
                                    if (isDecrypted)
                                    {
                                        decryptKey = key.Value;
                                        s2Cache.NetworkKeys.AddValue(key.Value);
                                        var mpanext = extensions.EncryptedExtensionsList.FirstOrDefault(x => x.properties1.type == (byte)ExtensionTypes.Mpan);
                                        if (mpanext != null && mpanext.extension != null && mpanext.extension.Count == 17)
                                        {
                                            MpanValue mpanValue = new MpanValue();
                                            mpanValue.Position = dataItem.Position;
                                            mpanValue.Value = mpanext.extension.Skip(1).ToArray();
                                            mpanValue.KeyIndex = keyIndex;
                                            RegisterMpan(dataItem.Position, dataItem.HomeId, new NodeGroupId(new NodeTag(dataItem.Source), mpanext.extension[0]), mpanValue);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        key.SyncIdBanlist.SetPosition(span.SyncId);
                                        s2Cache.CtxSpanCacheSlices.Clear();
                                    }
                                }
                            }
                        }
                        if (!isDecrypted && _tempSecurityKeys != null)
                        {
                            var keyNode = s2Cache.TempNetworkKeys.GetLastUsedValue();
                            for (int i = 0; i < s2Cache.TempNetworkKeys.GetValuesCount(); i++)
                            {
                                var key = keyNode?.Value;
                                lock (_locker)
                                {
                                    FireNetworkKeyS2Changed(peerNodeId, key, ZWave.Enums.SecuritySchemes.S2_TEMP);
                                    isDecrypted = DecryptS2SinglecastWithCache(dataItem, peerNodeId, span, s2Cache, out payload, out extensions);
                                }
                                if (isDecrypted)
                                {
                                    decryptKey = key;
                                    s2Cache.TempNetworkKeys.SetLastUsedValue(keyNode);
                                    break;
                                }
                                keyNode = s2Cache.TempNetworkKeys.GetNextValue(keyNode);
                            }
                            if (!isDecrypted)
                            {
                                var startIndex = 0;
                                var endIndex = _tempSecurityKeys.Count;
                                for (int keyIndex = startIndex; keyIndex < endIndex; keyIndex++)
                                {
                                    var key = _tempSecurityKeys[keyIndex];
                                    if (key.SyncIdBanlist.GetPosition(span.SyncId))
                                    {
                                        continue;
                                    }
                                    lock (_locker)
                                    {
                                        FireNetworkKeyS2Changed(peerNodeId, key.Value, ZWave.Enums.SecuritySchemes.S2_TEMP);
                                        isDecrypted = DecryptS2SinglecastWithCache(dataItem, peerNodeId, span, s2Cache, out payload, out extensions);
                                    }
                                    if (isDecrypted)
                                    {
                                        decryptKey = key.Value;
                                        s2Cache.TempNetworkKeys.AddValue(key.Value);
                                        break;
                                    }
                                    else
                                    {
                                        key.SyncIdBanlist.SetPosition(span.SyncId);
                                        s2Cache.CtxSpanCacheSlices.Clear();
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch
            {
                // ignored
            }
            if (extensions == null)
            {
                extensions = new Extensions();
                extensions.ExtensionsList = cmd.vg1;
            }
            return isDecrypted;
        }

        private MpanValue GetMpan(int position, byte[] homeId, NodeGroupId nodeGroupId)
        {
            MpanValue ret = null;
            var homeKey = BitConverter.ToInt32(homeId, 0);
            if (mpanValueStorage.ContainsKey(homeKey))
            {
                if (mpanValueStorage[homeKey].ContainsKey(nodeGroupId))
                {
                    var mpanValues = mpanValueStorage[homeKey][nodeGroupId];
                    var mpanValueKey = mpanValues.Keys;//.Keys.Where(x => x > position);
                    if (mpanValueKey.Count() > 0)
                    {
                        ret = mpanValues[mpanValueKey.First()];
                    }
                }
            }
            return ret;
        }

        internal void RegisterMpan(int position, byte[] homeId, NodeGroupId nodeGroupId, MpanValue mpan)
        {
            var homeKey = BitConverter.ToInt32(homeId, 0);
            if (!mpanValueStorage.ContainsKey(homeKey))
            {
                mpanValueStorage.Add(homeKey, new Dictionary<NodeGroupId, Dictionary<int, MpanValue>>());
            }
            if (!mpanValueStorage[homeKey].ContainsKey(nodeGroupId))
            {
                mpanValueStorage[homeKey].Add(nodeGroupId, new Dictionary<int, MpanValue>());
            }
            if (!mpanValueStorage[homeKey][nodeGroupId].ContainsKey(position))
            {
                mpanValueStorage[homeKey][nodeGroupId].Add(position, mpan);
            }
        }

        private bool DecryptS2MulticastWithCache(DataItem dataItem, NodeGroupId nodeGroupId, byte[] mpanValue, out byte[] payload, out Extensions extensions)
        {
            extensions = null;
            payload = null;
            var mpanContainer = MpanTable.AddOrReplace(nodeGroupId, dataItem.LastPayload[2], null, mpanValue);

            bool isDecrypted = false;
            if (McKeys.ContainsKey(nodeGroupId))
            {
                SecurityS2CryptoProviderBase.DecrementMpan(MpanTable, McKeys[nodeGroupId], nodeGroupId, new byte[16]);
                SecurityS2CryptoProviderBase.DecrementMpan(MpanTable, McKeys[nodeGroupId], nodeGroupId, new byte[16]);
                SecurityS2CryptoProviderBase.MaxMpanIterations = 1;
                for (int i = 0; i < 30; i++)
                {
                    isDecrypted = SecurityS2CryptoProviderBase.DecryptMulticastFrame(
                        MpanTable,
                        McKeys[nodeGroupId],
                        NodeIdBaseTypes.Base1,
                        nodeGroupId,
                        dataItem.HomeId,
                        dataItem.LastPayload,
                        out payload,
                        out extensions);

                    if (isDecrypted)
                    {
                        break;
                    }
                    SecurityS2CryptoProviderBase.IncrementMpan(MpanTable, McKeys[nodeGroupId], nodeGroupId, new byte[16]);
                }
            }
            return isDecrypted;
        }

        private bool DecryptS2SinglecastWithCache(DataItem dataItem, InvariantPeerNodeId peerNodeId, SpanRecord nonce, SpanCache spanCache, out byte[] payload, out Extensions extensions)
        {
            extensions = null;
            payload = null;
            SpanTable.Add(peerNodeId, nonce.ReceiverValue, 0, 0);
            var spanContainer = SpanTable.GetContainer(peerNodeId);
            spanContainer.InstantiateWithSenderNonce(nonce.SenderValue, ScKeys[peerNodeId].Personalization);
            int sliceindex = nonce.GetSliceIndex();
            var lastSliceIndex = spanCache.CtxSpanCacheSlices.Count - 1;
            CTR_DRBG_CTX ctx;
            if (sliceindex > lastSliceIndex)
            {
                if (spanCache.CtxSpanCacheSlices.Count == 0)
                {
                    ctx = SpanTable.GetContainer(peerNodeId).Context.Clone();
                    spanCache.CtxSpanCacheSlices.Add(ctx);
                    lastSliceIndex = 0;
                }
                else
                {
                    ctx = spanCache.CtxSpanCacheSlices[lastSliceIndex];
                    SpanTable.GetContainer(peerNodeId).Context = ctx.Clone();
                }
                for (int i = lastSliceIndex; i < sliceindex; i++)
                {
                    spanContainer.NextNonce(nonce.GetSliceSpan());
                    ctx = SpanTable.GetContainer(peerNodeId).Context.Clone();
                    spanCache.CtxSpanCacheSlices.Add(ctx);
                }
            }
            else
            {
                ctx = spanCache.CtxSpanCacheSlices[sliceindex];
                SpanTable.GetContainer(peerNodeId).Context = ctx.Clone();
            }
            var leftIterations = nonce.GetSliceLeftIterations();
            // Attempt to chase lost frames by formula 1 + 10%.
            var extra = 1 + nonce.Generation / 10;
            // Attempt to handle duplicated frames due to Retransmissions
            leftIterations -= extra;
            spanContainer.NextNonce(leftIterations - 1);
            bool isDecrypted = false;
            // One extra is for 'compensating' on leftIterations.
            // Another is for trying to chase current SPAN state
            // +5 is for fixing lost frame in first 10 generations.
            for (int i = 0; i < 5 + extra + extra; i += 5)
            {
                isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                    spanContainer,
                    ScKeys.ContainsKey(peerNodeId) ? ScKeys[peerNodeId] : null,
                    new NodeTag(dataItem.Source),
                    new NodeTag(dataItem.Destination),
                    dataItem.HomeId,
                    dataItem.LastPayload,
                    out payload,
                    out extensions);

                if (isDecrypted)
                {
                    break;
                }
            }
            return isDecrypted;
        }

        public NetworkKeysAttachment GetNetworkKeysAttachment()
        {
            NetworkKeysAttachment ret = null;
            if (_securityKeys != null || _tempSecurityKeys != null)
            {
                ret = new NetworkKeysAttachment();
                if (_securityKeys != null)
                {
                    ret.NetworkKeys.AddRange(_securityKeys.Select(x => x.Value));
                }
                if (_tempSecurityKeys != null)
                {
                    ret.TempNetworkKeys.AddRange(_tempSecurityKeys.Select(x => x.Value));
                }
            }
            return ret;
        }

        public NetworkKeysAttachment GetNewlyAddedNetworkKeysAttachment()
        {
            NetworkKeysAttachment ret = null;
            if ((_newSecurityKeys != null && _newSecurityKeys.Count > 0)
                || (_newTempSecurityKeys != null && _newTempSecurityKeys.Count > 0))
            {
                ret = new NetworkKeysAttachment();
                if (_newSecurityKeys != null)
                {
                    ret.NetworkKeys.AddRange(_newSecurityKeys);
                }
                if (_newTempSecurityKeys != null)
                {
                    ret.TempNetworkKeys.AddRange(_newTempSecurityKeys);
                }
            }
            return ret;
        }

        public bool AddNewSecurityKey(byte[] keyValue)
        {
            bool ret = false;
            if (!_securityKeys.Where(x => x.Value.SequenceEqual(keyValue)).Any())
            {
                _securityKeys.Add(new NetworkKey(keyValue));
                _newSecurityKeys.Add(keyValue);
                ret = true;
            }
            return ret;
        }

        public bool AddSecurityKeys(List<byte[]> networkKeys)
        {
            bool ret = false;
            if (networkKeys != null)
            {
                foreach (var keyValue in networkKeys)
                {
                    if (!_securityKeys.Where(x => x.Value.SequenceEqual(keyValue)).Any())
                    {
                        _securityKeys.Add(new NetworkKey(keyValue));
                        ret = true;
                    }
                }
            }
            return ret;
        }

        public bool AddNewTempSecurityKey(byte[] keyValue)
        {
            bool ret = false;
            if (!_tempSecurityKeys.Where(x => x.Value.SequenceEqual(keyValue)).Any())
            {
                _tempSecurityKeys.Add(new NetworkKey(keyValue));
                _newTempSecurityKeys.Add(keyValue);
                ret = true;
            }
            return ret;
        }

        public bool AddTempSecurityKeys(List<byte[]> tempNetworkKeys)
        {
            bool ret = false;
            if (tempNetworkKeys != null)
            {
                foreach (var keyValue in tempNetworkKeys)
                {
                    if (!_tempSecurityKeys.Where(x => x.Value.SequenceEqual(keyValue)).Any())
                    {
                        _tempSecurityKeys.Add(new NetworkKey(keyValue));
                        ret = true;
                    }
                }
            }
            return ret;
        }
    }

    public class NetworkKey
    {
        public byte[] Value { get; set; }
        public VariantBitArray SyncIdBanlist { get; set; }
        public NetworkKey(IEnumerable<byte> value)
        {
            Value = value.ToArray();
            SyncIdBanlist = new VariantBitArray(1000);
        }
    }

    public class KeysCache
    {
        private const int CAPACITY = 100;
        private LinkedList<byte[]> _networkKeys = new LinkedList<byte[]>();
        private LinkedListNode<byte[]> _lastUsedNetworkKey;

        public KeysCache()
        {
        }

        public void AddValue(byte[] value)
        {
            lock (this)
            {
                if (_networkKeys.Count > CAPACITY)
                {
                    _networkKeys.RemoveFirst();
                }
                _lastUsedNetworkKey = _networkKeys.AddLast(value);
            }
        }

        public LinkedListNode<byte[]> GetLastUsedValue()
        {
            return _lastUsedNetworkKey ?? _networkKeys.First;
        }

        internal LinkedListNode<byte[]> GetNextValue(LinkedListNode<byte[]> value)
        {
            return value?.Next ?? _networkKeys.First;
        }

        internal int GetValuesCount()
        {
            return _networkKeys.Count;
        }

        internal void SetLastUsedValue(LinkedListNode<byte[]> value)
        {
            _lastUsedNetworkKey = value;
        }
    }

    public class SpanCache
    {
        public KeysCache NetworkKeys { get; set; }
        public KeysCache TempNetworkKeys { get; set; }
        public SpanCache()
        {
            NetworkKeys = new KeysCache();
            TempNetworkKeys = new KeysCache();
            CtxSpanCacheSlices = new List<CTR_DRBG_CTX>();
        }

        //public int NetworkKeyIndex { get; set; }
        //public int ProbedNetworkKeyCount { get; set; }
        //public int TempNetworkKeyIndex { get; set; }
        //public int ProbedTempNetworkKeyCount { get; set; }

        /// <summary>
        /// Stored slices every 1K iteration
        /// </summary>
        public List<CTR_DRBG_CTX> CtxSpanCacheSlices { get; set; }
    }

    public class MpanCache
    {
        public MpanCache()
        {
            NetworkKeyIndex = -1;
            CtxMpanCacheSlices = new List<CTR_DRBG_CTX>();
        }

        public int NetworkKeyIndex { get; set; }
        public int ProbedNetworkKeyCount { get; set; }

        /// <summary>
        /// Stored slices every 1K iteration
        /// </summary>
        public List<CTR_DRBG_CTX> CtxMpanCacheSlices { get; set; }
    }

    public class NonceS0Table
    {
        static readonly int Capacity = 10;
        LinkedList<byte[]> nonces = new LinkedList<byte[]>();

        public byte[] GetNonce(byte id)
        {
            byte[] ret = null;
            ret = nonces.FirstOrDefault(x => x != null && x.Length > 0 && x[0] == id);
            return ret;
        }

        public void PutNonce(byte[] nonce)
        {
            if (nonce != null && nonce.Length == 8)
            {
                lock (this)
                {
                    nonces.AddFirst(nonce);
                    if (nonces.Count > Capacity)
                    {
                        nonces.RemoveLast();
                    }
                }
            }
        }

        public void Clear()
        {
            lock (this)
            {
                nonces.Clear();
            }
        }
    }

    public class SpanRecord
    {
        public int SyncId { get; set; }
        public int Generation { get; set; }
        public byte[] ReceiverValue { get; set; }
        public byte[] SenderValue { get; set; }
        private byte _sequenceNumberLeft; // seqNo from node with lesser source nodeId (not persistent) 
        private byte _sequenceNumberRight; // seqNo from node with grater source nodeId (not persistent)

        public static implicit operator SpanRecord(byte[] data)
        {
            SpanRecord ret = new SpanRecord();
            if (data != null)
            {
                if (data.Length == 4)
                {
                    ret.SyncId = BitConverter.ToInt32(data, 0);
                }
                else if (data.Length == 24)
                {
                    ret.SyncId = BitConverter.ToInt32(data, 0);
                    ret.Generation = BitConverter.ToInt32(data, 4);
                    ret.ReceiverValue = data.Skip(8).Take(16).ToArray();
                }
                else if (data.Length == 40)
                {
                    ret.SyncId = BitConverter.ToInt32(data, 0);
                    ret.Generation = BitConverter.ToInt32(data, 4);
                    ret.ReceiverValue = data.Skip(8).Take(16).ToArray();
                    ret.SenderValue = data.Skip(24).Take(16).ToArray();
                }
            }
            return ret;
        }

        public static implicit operator byte[](SpanRecord value)
        {
            List<byte> ret = new List<byte>();
            ret.AddRange(BitConverter.GetBytes(value.SyncId));
            if (value.ReceiverValue != null && value.ReceiverValue.Length == 16)
            {
                ret.AddRange(BitConverter.GetBytes(value.Generation));
                ret.AddRange(value.ReceiverValue);
                if (value.SenderValue != null && value.SenderValue.Length == 16)
                {
                    ret.AddRange(value.SenderValue);
                }
            }
            return ret.ToArray();
        }

        public bool IsValidSequenceNumber(ushort src, ushort dest, byte sequenceNumber)
        {
            bool ret = false;
            if (Generation == 0)
            {
                ret = true;
            }
            else
            {
                if (src > dest)
                {
                    ret = _sequenceNumberRight != sequenceNumber;
                }
                else
                {
                    ret = _sequenceNumberLeft != sequenceNumber;
                }
            }
            return ret;
        }

        public void SetSequenceNumber(ushort src, ushort dest, byte sequenceNumber)
        {
            if (src > dest)
            {
                _sequenceNumberRight = sequenceNumber;
            }
            else
            {
                _sequenceNumberLeft = sequenceNumber;
            }
        }

        public int GetSliceSpan()
        {
            return 1000;
        }

        public int GetSliceIndex()
        {
            return Generation / GetSliceSpan();
        }

        public int GetSliceLeftIterations()
        {
            return Generation % GetSliceSpan();
        }
    }

    public class MpanValue
    {
        public int Position { get; set; }
        public byte[] Value { get; set; }
        public int KeyIndex { get; set; }
    }
}
