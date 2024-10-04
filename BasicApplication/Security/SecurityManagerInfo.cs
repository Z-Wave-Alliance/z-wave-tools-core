/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Utils;
using Utils.UI;
using ZWave.CommandClasses;
using ZWave.Configuration;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;
using ZWave.Security.S2;

namespace ZWave.BasicApplication.Security
{
    public class SecurityManagerInfo
    {
        public SecurityManagerInfo() { }

        private const int MAX_DELAY = 60000;

        #region Test Fields
        internal byte[] TestNetworkKeyS2Temp;
        internal byte[] TestSecretKeyS2;
        public Dictionary<SecurityS2TestFrames, TestFrameS2Settings> TestFramesS2 = new Dictionary<SecurityS2TestFrames, TestFrameS2Settings>();
        public List<TestExtensionS2Settings> TestExtensionsS2 = new List<TestExtensionS2Settings>();
        public AutoResetEvent TempKeyActivationResetEvent = new AutoResetEvent(true);

        public bool IsTempKeyActivatedOnInclusion { get; protected set; }
        private bool _isInclusion;
        public bool IsInclusion
        {
            get { return _isInclusion; }
            set
            {
                _isInclusion = value;
                if (_isInclusion)
                {
                    IsTempKeyActivatedOnInclusion = false;
                }
            }
        }

        //internal byte[] TestNetworkKeyS0;
        internal byte[] TestNetworkKeyS0Temp;
        internal byte[] TestNetworkKeyS0InSet;
        internal byte? TestSchemeS0;
        internal byte? TestSchemeS0InGet;
        internal byte? TestSchemeS0InReport;
        internal byte? TestSchemeS0InInherit;
        internal byte? TestSchemeS0InReportEncrypted;
        //pass this in message encapsulation frame already encrypted with valid internal and external nonces.
        internal byte[] TestMacS0;
        internal byte? TestPropertyByteS0;
        internal byte? TestPropertyByteS0SecondFrame;
        internal byte[] TestReceiverNonceS0;
        //pass this in message encapsulation frame already encrypted with valid internal and external nonces.
        internal byte? TestReceiverNonceS0Id;
        //pass this in message encapsulation frame already encrypted with valid internal and external nonces.
        internal byte[] TestSenderNonceS0;
        internal bool TestFirstFragmentedPartDisabled;
        internal bool TestSecondFragmentedPartDisabled;
        internal bool TestReuseReceiverNonceS0InSecondFragment;

        internal Dictionary<SecurityS0Delays, int> DelaysS0 = new Dictionary<SecurityS0Delays, int>();
        internal TransmitOptions TxOptions = TransmitOptions.TransmitOptionAcknowledge | TransmitOptions.TransmitOptionAutoRoute | TransmitOptions.TransmitOptionExplore;

        #endregion

        public NetworkViewPoint Network { get; set; }
        public MpanTable MpanTable { get; set; }
        public SpanTable SpanTable { get; set; }
        public ConcurrentDictionary<InvariantPeerNodeId, SinglecastKey> ScKeys { get; set; }
        public Dictionary<NodeGroupId, MulticastKey> McKeys { get; set; }

        public bool CheckIfSupportSecurityCC
        {
            get { return Network.CheckIfSupportSecurityCC; }
            set { Network.CheckIfSupportSecurityCC = value; }
        }
        public bool IsActive { get; set; }

        private Func<byte, SecuritySchemes> _getActiveKeyMultiCallback;
        public Func<byte, SecuritySchemes> GetActiveKeyMultiCallback
        {
            set
            {
                _getActiveKeyMultiCallback = value;
            }
        }

        private SecuritySchemes _defaultActiveNetworkKeyType = SecuritySchemes.S2_UNAUTHENTICATED;

        public SecuritySchemes GetActiveKeyTypeForGroup(byte groupId)
        {
            if (_getActiveKeyMultiCallback != null)
            {
                return _getActiveKeyMultiCallback(groupId);
            }
            return _defaultActiveNetworkKeyType;
        }

        public DSKRequestStatuses DSKRequestStatus { get; set; }
        private Func<NodeTag, byte[], byte[]> _DSKNeededCallback;
        public Func<NodeTag, byte[], byte[]> DSKNeededCallback
        {
            get
            {
                return _DSKNeededCallback;
            }
            set
            {
                _DSKNeededCallback = value;
            }
        }

        private Func<IEnumerable<SecuritySchemes>, bool, KEXSetConfirmResult> _KEXSetConfirmCallback;
        public Func<IEnumerable<SecuritySchemes>, bool, KEXSetConfirmResult> KEXSetConfirmCallback
        {
            get
            {
                return _KEXSetConfirmCallback;
            }
            set
            {
                _KEXSetConfirmCallback = value;
            }
        }

        private Action _csaPinCallback;
        public Action CsaPinCallback
        {
            get
            {
                return _csaPinCallback;
            }
            set
            {
                _csaPinCallback = value;
            }
        }

        private Action _dskPinCallback;
        public Action DskPinCallback
        {
            get
            {
                return _dskPinCallback;
            }
            set
            {
                _dskPinCallback = value;
            }
        }

        private Func<byte[]> _DSKVerificationOnReceiverCallback;
        public Func<byte[]> DSKVerificationOnReceiverCallback
        {
            get
            {
                return _DSKVerificationOnReceiverCallback;
            }
            set
            {
                _DSKVerificationOnReceiverCallback = value;
            }
        }

        private Func<NodeTag, byte[], bool> _sendDataSubstitutionCallback;
        public Func<NodeTag, byte[], bool> SendDataSubstitutionCallback
        {
            get
            {
                return _sendDataSubstitutionCallback;
            }
            internal set
            {
                _sendDataSubstitutionCallback = value;
            }
        }

        private Action<NodeTag, byte[], bool> _receiveDataSubstitutionCallback;
        public Action<NodeTag, byte[], bool> ReceiveDataSubstitutionCallback
        {
            get
            {
                return _receiveDataSubstitutionCallback;
            }
            internal set
            {
                _receiveDataSubstitutionCallback = value;
            }
        }

        private byte[] _secretKeyS2;
        public byte[] SecretKeyS2
        {
            get
            {
                if (TestSecretKeyS2 != null)
                {
                    return TestSecretKeyS2;
                }
                return _secretKeyS2;
            }
            set
            {
                _secretKeyS2 = value;
            }
        }

        private readonly byte[] _nvrSecretKeyS2;
        public byte[] JoinSecretKeyS2
        {
            get
            {
                return TestSecretKeyS2 ?? _nvrSecretKeyS2;
            }
        }

        public static int GetNetworkKeyIndex(byte scheme, bool isLongRange)
        {
            return GetNetworkKeyIndex((SecuritySchemes)scheme, isLongRange);
        }

        /// <summary>
        /// Returns index of the Network key of the specified security scheme 
        /// in the network keys array
        /// </summary>
        /// <param name="scheme">scheme</param>
        /// <returns>index</returns>
        public static int GetNetworkKeyIndex(SecuritySchemes scheme, bool isLongRange)
        {
            int ret = -1;
            switch (scheme)
            {
                case SecuritySchemes.S2_UNAUTHENTICATED:
                    ret = NetworkKeyS2Flags.S2Class0.GetByteFlagIndex();
                    break;
                case SecuritySchemes.S2_AUTHENTICATED:
                    ret = isLongRange ? 3 : NetworkKeyS2Flags.S2Class1.GetByteFlagIndex();
                    break;
                case SecuritySchemes.S2_ACCESS:
                    ret = isLongRange ? 4 : NetworkKeyS2Flags.S2Class2.GetByteFlagIndex();
                    break;
                case SecuritySchemes.S0:
                    ret = NetworkKeyS2Flags.S0.GetByteFlagIndex();
                    break;
            }
            return ret;
        }

        public static SecuritySchemes[] ConvertToSecuritySchemes(NetworkKeyS2Flags keysMask)
        {
            List<SecuritySchemes> ret = new List<SecuritySchemes>();
            if ((keysMask & NetworkKeyS2Flags.S0) > 0)
            {
                ret.Add(SecuritySchemes.S0);
            }
            if ((keysMask & NetworkKeyS2Flags.S2Class2) > 0)
            {
                ret.Add(SecuritySchemes.S2_ACCESS);
            }
            if ((keysMask & NetworkKeyS2Flags.S2Class1) > 0)
            {
                ret.Add(SecuritySchemes.S2_AUTHENTICATED);
            }
            if ((keysMask & NetworkKeyS2Flags.S2Class0) > 0)
            {
                ret.Add(SecuritySchemes.S2_UNAUTHENTICATED);
            }
            return ret.ToArray();
        }

        public static NetworkKeyS2Flags ConvertToNetworkKeyMask(SecuritySchemes[] securitySchemes)
        {
            NetworkKeyS2Flags ret = NetworkKeyS2Flags.None;
            if (securitySchemes != null)
            {
                if (securitySchemes.Contains(SecuritySchemes.S0))
                {
                    ret |= NetworkKeyS2Flags.S0;
                }
                if (securitySchemes.Contains(SecuritySchemes.S2_ACCESS))
                {
                    ret |= NetworkKeyS2Flags.S2Class2;
                }
                if (securitySchemes.Contains(SecuritySchemes.S2_AUTHENTICATED))
                {
                    ret |= NetworkKeyS2Flags.S2Class1;
                }
                if (securitySchemes.Contains(SecuritySchemes.S2_UNAUTHENTICATED))
                {
                    ret |= NetworkKeyS2Flags.S2Class0;
                }
            }
            return ret;
        }

        internal static SecuritySchemes ConvertToSecurityScheme(NetworkKeyS2Flags verifyKey)
        {
            SecuritySchemes ret = SecuritySchemes.NONE;
            switch (verifyKey)
            {
                case NetworkKeyS2Flags.S2Class0:
                    ret = SecuritySchemes.S2_UNAUTHENTICATED;
                    break;
                case NetworkKeyS2Flags.S2Class1:
                    ret = SecuritySchemes.S2_AUTHENTICATED;
                    break;
                case NetworkKeyS2Flags.S2Class2:
                    ret = SecuritySchemes.S2_ACCESS;
                    break;
                case NetworkKeyS2Flags.S0:
                    ret = SecuritySchemes.S0;
                    break;
            }
            return ret;
        }

        public virtual byte[] GetPublicKeyS2()
        {
            byte[] ret = new byte[32];
            SecurityS2Utils.CryptoScalarmultCurve25519Base(ret, SecretKeyS2);
            return ret;
        }

        public byte[] GetSharedSecretS2(byte[] receiverPublicKey)
        {
            byte[] ret = new byte[32];
            SecurityS2Utils.CryptoScalarmultCurve25519(ret, SecretKeyS2, receiverPublicKey);
            return ret;
        }

        public byte[] GetJoinPublicKeyS2()
        {
            byte[] ret = new byte[32];
            SecurityS2Utils.CryptoScalarmultCurve25519Base(ret, JoinSecretKeyS2);
            return ret;
        }

        public byte[] GetJoinSharedSecretS2(byte[] receiverPublicKey)
        {
            byte[] ret = new byte[32];
            SecurityS2Utils.CryptoScalarmultCurve25519(ret, JoinSecretKeyS2, receiverPublicKey);
            return ret;
        }

        public byte[] CalculateTempNetworkKeyS2(byte[] receiverPublicKey, bool isKeySender)
        {
            byte[] authTag = new byte[64];
            byte[] ret = new byte[16];

            if (isKeySender)
            {
                Array.Copy(GetPublicKeyS2(), 0, authTag, 0, GetPublicKeyS2().Length);
                Array.Copy(receiverPublicKey, 0, authTag, GetPublicKeyS2().Length, receiverPublicKey.Length);
                SecurityS2Utils.TempKeyExtract(GetSharedSecretS2(receiverPublicKey), authTag, ret);
            }
            else
            {
                Array.Copy(receiverPublicKey, 0, authTag, 0, receiverPublicKey.Length);
                Array.Copy(GetJoinPublicKeyS2(), 0, authTag, receiverPublicKey.Length, GetJoinPublicKeyS2().Length);
                SecurityS2Utils.TempKeyExtract(GetJoinSharedSecretS2(receiverPublicKey), authTag, ret);
            }
            return ret;
        }

        public const int NETWORK_KEYS_COUNT = 8;
        private readonly NetworkKey[] _networkKeys = new NetworkKey[NETWORK_KEYS_COUNT];
        /// <summary>
        /// Returns table of network keys
        /// </summary>
        public NetworkKey[] NetworkKeys { get { return _networkKeys; } }

        /// <summary>
        /// Returns current network key for the specified scheme if no test network key added.
        /// Returns added test network for the specified scheme if test network key added.
        /// </summary>
        /// <param name="scheme">scheme</param>
        /// <param name="isLongRange">is Long Range</param>
        /// <returns>key</returns>
        public byte[] GetActualNetworkKey(SecuritySchemes scheme, bool isLongRange)
        {
            var index = GetNetworkKeyIndex(scheme, isLongRange);
            return _networkKeys[index].TestValue ?? _networkKeys[index].Value;
        }

        ///// <summary>
        ///// Fills network key S2 table with specified keys (Without activating any of them)
        ///// </summary>
        ///// <param name="networkKeys">Keys table</param>
        //public void SetNetworkKeys(NetworkKey[] networkKeys)
        //{
        //    NetworkKey[] keys = networkKeys ?? new NetworkKey[SecurityManagerInfo.NETWORK_KEYS_COUNT];
        //    if (keys.Length != NETWORK_KEYS_COUNT)
        //        throw new ArgumentException("invalid keys table length");


        //    Array.Copy(keys, _networkKeys, NETWORK_KEYS_COUNT);
        //}

        /// <summary>
        /// Sests network key for specified scheme (wihout activating it)
        /// </summary>
        /// <param name="key">Network key</param>
        /// <param name="scheme">scheme</param>
        public void SetNetworkKey(byte[] key, SecuritySchemes scheme, bool isLongRange)
        {
            var index = GetNetworkKeyIndex(scheme, isLongRange);
            if (_networkKeys[index] == null)
            {
                _networkKeys[index] = new NetworkKey();
            }
            _networkKeys[index].Value = key;
        }

        //private byte[][] _testNetworkKeys = new byte[NETWORK_KEYS_COUNT][];
        ///// <summary>
        ///// Returns table of test network keys
        ///// </summary>
        //public byte[][] TestNetworkKeys { get { return _testNetworkKeys; } }

        /// <summary>
        /// Returns test network key for specified scheme 
        /// </summary>
        /// <param name="scheme">scheme</param>
        /// <returns>test key</returns>
        public byte[] GetTestNetworkKey(SecuritySchemes scheme, bool isLongRange)
        {
            return _networkKeys[GetNetworkKeyIndex(scheme, isLongRange)].TestValue;
        }

        /// <summary>
        /// Returns test network key for specified scheme 
        /// </summary>
        /// <param name="scheme">scheme</param>
        /// <returns>test key</returns>
        public byte[] GetNetworkKey(SecuritySchemes scheme, bool isLongRange)
        {
            return _networkKeys[GetNetworkKeyIndex(scheme, isLongRange)].Value;
        }

        /// <summary>
        /// Sests test network key for specified security class (wihout activating it)
        /// </summary>
        /// <param name="key">Test network key</param>
        /// <param name="scheme">scheme</param>
        public void SetTestNetworkKey(byte[] key, SecuritySchemes scheme, bool isLongRange)
        {
            _networkKeys[GetNetworkKeyIndex(scheme, isLongRange)].TestValue = key;
        }

        public void SetTestNetworkKeyS0InSet(byte[] key)
        {
            TestNetworkKeyS0InSet = key;
        }

        private byte[] _networkKeyS2Temp = new byte[16];
        /// <summary>
        /// Returns temp network key if no test temp network key specified.
        /// Returns test temp network key if test temp network key specified.
        /// </summary>
        /// <returns>temp key</returns>
        public virtual byte[] GetActualNetworkKeyS2Temp()
        {
            return TestNetworkKeyS2Temp ?? _networkKeyS2Temp;
        }

        /// <summary>
        /// Returns temp network key.
        /// </summary>
        /// <returns>key</returns>
        public byte[] GetOriginalNetworkKeyS2Temp()
        {
            return _networkKeyS2Temp;
        }

        /// <summary>
        /// Sets secified temp test key for specified node (without activating it)
        /// </summary>
        /// <param name="tempKey">Temp key</param>
        public void SetTestNetworkKeyS2Temp(byte[] tempKey)
        {
            TestNetworkKeyS2Temp = tempKey;
        }

        /// <summary>
        /// Sets secified temp key for specified node (without activating it)
        /// </summary>
        /// <param name="tempKey">Temp key</param>
        public void SetNetworkKeyS2Temp(byte[] tempKey)
        {
            _networkKeyS2Temp = tempKey;
        }

        private NodeTag _initializingNodeId;
        public NodeTag InitializingNodeId
        {
            get { return _initializingNodeId; }
            set { _initializingNodeId = value; }
        }

        public ConcurrentDictionary<InvariantPeerNodeId, RetransmissionRecord> RetransmissionTableS2 = new ConcurrentDictionary<InvariantPeerNodeId, RetransmissionRecord>();

        private byte[] _networkKeyS0Temp = new byte[16];
        public byte[] NetworkKeyS0Temp
        {
            get
            {
                return TestNetworkKeyS0Temp ?? _networkKeyS0Temp;
            }
            internal set
            {
                _networkKeyS0Temp = value;
            }
        }

        private byte _securitySchemeInGetS0;
        public byte SecuritySchemeInGetS0
        {
            get
            {
                if (TestSchemeS0InGet != null)
                    return (byte)TestSchemeS0InGet;
                else if (TestSchemeS0 != null)
                    return (byte)TestSchemeS0;
                else
                    return _securitySchemeInGetS0;
            }
            internal set
            {
                _securitySchemeInGetS0 = value;
            }
        }

        private byte _securitySchemeInReportS0;
        public byte SecuritySchemeInReportS0
        {
            get
            {
                if (TestSchemeS0InReport != null)
                    return (byte)TestSchemeS0InReport;
                else if (TestSchemeS0 != null)
                    return (byte)TestSchemeS0;
                else
                    return _securitySchemeInReportS0;
            }
            internal set
            {
                _securitySchemeInReportS0 = value;
            }
        }

        private byte _securitySchemeInReportEncryptedS0;
        public byte SecuritySchemeInReportEncryptedS0
        {
            get
            {
                if (TestSchemeS0InReportEncrypted != null)
                    return (byte)TestSchemeS0InReportEncrypted;
                else if (TestSchemeS0 != null)
                    return (byte)TestSchemeS0;
                else
                    return _securitySchemeInReportEncryptedS0;
            }
            internal set
            {
                _securitySchemeInReportEncryptedS0 = value;
            }
        }

        private byte _securitySchemeInInheritS0;
        public byte SecuritySchemeInInheritS0
        {
            get
            {
                if (TestSchemeS0InInherit != null)
                    return (byte)TestSchemeS0InInherit;
                else if (TestSchemeS0 != null)
                    return (byte)TestSchemeS0;
                else
                    return _securitySchemeInInheritS0;
            }
            internal set
            {
                _securitySchemeInInheritS0 = value;
            }
        }

        public event Action<byte[], bool> NetworkKeyS0Changed;
        private void FireNetworkKeyS0Changed(bool isRealKey)
        {
            byte[] networkKey;
            if (isRealKey)
            {
                networkKey = GetActualNetworkKey(SecuritySchemes.S0, false);
            }
            else
            {
                networkKey = NetworkKeyS0Temp;
            }
            NetworkKeyS0Changed?.Invoke(networkKey, !isRealKey);
        }

        public event Action<InvariantPeerNodeId, byte[], SecuritySchemes, bool, bool> NetworkKeyS2Changed;
        private void FireNetworkKeyS2Changed(InvariantPeerNodeId peerNodeId, byte[] networkKey, SecuritySchemes securityScheme)
        {
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            if (securityScheme == SecuritySchemes.S2_TEMP)
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
                        if (!item.Value.CcmKey.SequenceEqual(ccmKey) && item.Value.SecurityScheme == securityScheme)
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
                    ScKeys.Clear();
                    MpanTable.ClearMpanTable();
                    SpanTable.ClearNonceTable();
                    break;
                default:
                    bool isSameValue = false;
                    ScKeys.AddOrUpdate(peerNodeId, new SinglecastKey(networkKey[0], ccmKey, personalization, securityScheme), (x, y) =>
                    {
                        y.Update(networkKey[0], ccmKey, personalization, securityScheme);
                        return y;
                    });

                    if (!isSameValue && SpanTable.GetSpanState(peerNodeId) != SpanStates.ReceiversNonce)
                    {
                        SpanTable.SetNonceFree(peerNodeId);
                    }
                    break;
            }

            if (IsInclusion && securityScheme == SecuritySchemes.S2_TEMP)
            {
                IsTempKeyActivatedOnInclusion = true;
            }

            NetworkKeyS2Changed?.Invoke(peerNodeId, networkKey, securityScheme, IsInclusion, securityScheme == SecuritySchemes.S2_TEMP);
            if (peerNodeId.NodeId2.Id == 0)
            {
                RetransmissionTableS2.Clear();
            }
            else if (peerNodeId.NodeId2.Id == 0xFF)
            {
                RetransmissionTableS2.Clear();
            }
            else
            {
                if (RetransmissionTableS2.TryRemove(peerNodeId, out RetransmissionRecord rr))
                {
                }
            }
        }

        private void FireNetworkKeyS2ChangedMulti(byte groupId, NodeTag owner, byte[] networkKey, SecuritySchemes securityScheme)
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

        private NodeTag[] _nodesMask;
        private void Network_S2SchemeSettingsChanged(SecuritySchemes scheme, bool isEnabled)
        {
            if (SecuritySchemeSet.ALLS2.Contains(scheme))
            {
                var nodes = _nodesMask.Where(x => x != Network.NodeTag && Network.HasSecurityScheme(x, scheme)).ToList();
                var selfNode = Network.NodeTag;

                foreach (var node in nodes)
                {
                    var key = new InvariantPeerNodeId(selfNode, node);
                    if (ScKeys.ContainsKey(key))
                    {
                        if ((int)scheme >= (int)ScKeys[key].SecurityScheme)
                        {
                            ScKeys.TryRemove(key, out SinglecastKey scKey);
                        }
                    }
                }
                if (MpanTable != null)
                {
                    var groups = MpanTable.SelectGroupIds(selfNode).Where(
                        groupId =>
                        {
                            var container = MpanTable.GetContainer(new NodeGroupId(selfNode, groupId));
                            if (container != null)
                            {
                                return container.ReceiverGroupHandle.Intersect(nodes).Any();
                            }
                            return false;
                        });

                    foreach (var groupId in groups)
                    {
                        var peerGroupId = new NodeGroupId(selfNode, groupId);
                        McKeys.Remove(peerGroupId);
                    }
                }
            }
        }

        internal SecurityManagerInfo(NetworkViewPoint network, NetworkKey[] networkKeys, byte[] privateKey)
        {
            _nvrSecretKeyS2 = privateKey;
            Network = network;
            _nodesMask = new NodeTag[NetworkViewPoint.MAX_NODES];
            for (int i = 0; i < _nodesMask.Length; i++)
            {
                _nodesMask[i] = new NodeTag((ushort)(i));
            }
            Network.EnableSecuritySchemeSettingsChanged += Network_S2SchemeSettingsChanged;
            _networkKeys = networkKeys;
            MpanTable = new MpanTable();
            SpanTable = new SpanTable();
            ScKeys = new ConcurrentDictionary<InvariantPeerNodeId, SinglecastKey>();
            McKeys = new Dictionary<NodeGroupId, MulticastKey>();
        }

        #region Test_S0

        public void ClearTestOverridesS0()
        {
            TestPropertyByteS0 = null;
            TestPropertyByteS0SecondFrame = null;
            TestMacS0 = null;
            TestNetworkKeyS0Temp = null;
            TestNetworkKeyS0InSet = null;
            TestReceiverNonceS0 = null;
            TestReceiverNonceS0Id = null;
            TestSenderNonceS0 = null;
            TestFirstFragmentedPartDisabled = false;
            TestSecondFragmentedPartDisabled = false;
            TestSchemeS0 = null;
            TestSchemeS0InGet = null;
            TestSchemeS0InReport = null;
            TestSchemeS0InReportEncrypted = null;
            TestSchemeS0InInherit = null;
            IsSenderNonceS0Disabled = false;
        }

        public void SetTestMacS0(byte[] code)
        {
            TestMacS0 = code;
        }

        public void SetTestPropertyByteS0(byte value)
        {
            TestPropertyByteS0 = value;
        }

        public void SetTestPropertyByteSecondFrameS0(byte value)
        {
            TestPropertyByteS0SecondFrame = value;
        }

        public void SetTestReceiverNonceS0(byte[] nonceData)
        {
            TestReceiverNonceS0 = nonceData;
        }

        public void SetTestReceiverNonceIdS0(byte? value)
        {
            TestReceiverNonceS0Id = value;
        }

        public void SetTestSenderNonceS0(byte[] nonceData)
        {
            TestSenderNonceS0 = nonceData;
        }

        public void SetTestReuseReceiverNonceS0InSecondFragment(bool reuse)
        {
            TestReuseReceiverNonceS0InSecondFragment = reuse;
        }

        public bool IsSenderNonceS0Disabled { get; set; }

        public void SetTestFragmentedPartDisabled(bool isFirstPartDisabled, bool isSecondPartDisabled)
        {
            TestFirstFragmentedPartDisabled = isFirstPartDisabled;
            TestSecondFragmentedPartDisabled = isSecondPartDisabled;
        }

        public void ActivateNetworkKeyS0()
        {
            byte[] networkKey = GetActualNetworkKey(SecuritySchemes.S0, false);
            if (networkKey != null)
                FireNetworkKeyS0Changed(true);
        }

        public void ActivateNetworkKeyS0Temp()
        {
            FireNetworkKeyS0Changed(false);
        }

        public void SetTestSecuritySchemeS0(byte securityScheme)
        {
            TestSchemeS0 = securityScheme;
        }

        public void SetTestSecuritySchemeInGetS0(byte securityScheme)
        {
            TestSchemeS0InGet = securityScheme;
        }

        public void SetTestSecuritySchemeInReportS0(byte securityScheme)
        {
            TestSchemeS0InReport = securityScheme;
        }

        public void SetTestSecuritySchemeInReportEncryptedS0(byte securityScheme)
        {
            TestSchemeS0InReportEncrypted = securityScheme;
        }

        public void SetTestSecuritySchemeInInheritS0(byte securityScheme)
        {
            TestSchemeS0InInherit = securityScheme;
        }

        public void SetDelayS0(SecurityS0Delays delay, int timeoutMs)
        {
            if (timeoutMs > 0)
            {
                int t = timeoutMs > MAX_DELAY ? MAX_DELAY : timeoutMs;
                if (!DelaysS0.ContainsKey(delay))
                    DelaysS0.Add(delay, t);
                else
                    DelaysS0[delay] = t;
            }
            else
            {
                if (DelaysS0.ContainsKey(delay))
                    DelaysS0.Remove(delay);
            }
        }

        public void ClearDelaysS0()
        {
            DelaysS0.Clear();
        }

        #endregion

        #region Test_S2

        public byte[] TestOverrideMessageEncapsulation(SinglecastKey sckey, SpanTable spanTable, SecurityS2CryptoProvider _securityS2CryptoProvider, SubstituteSettings substituteSettings, NodeTag node, byte[] data, InvariantPeerNodeId peerNodeId, Extensions extensions, byte[] encryptedMsg, byte[] initData)
        {
            byte[] ret = initData;
            if (TestFramesS2.ContainsKey(SecurityS2TestFrames.MessageEncapsulation))
            {
                var testData = data;
                var testFrame = TestFramesS2[SecurityS2TestFrames.MessageEncapsulation];
                if (testFrame.DelaySpecified && testFrame.Delay > 0)
                {
                    Thread.Sleep(testFrame.Delay);
                }
                if (testFrame.Command != null)
                {
                    testData = testFrame.Command;
                }
                if (!testFrame.IsEncryptedSpecified || testFrame.IsEncrypted)
                {
                    if (testFrame.IsTemp)
                    {
                        if (testFrame.NetworkKey != null)
                        {
                            ActivateNetworkKeyS2CustomForNode(peerNodeId, testFrame.IsTemp, testFrame.NetworkKey);
                        }
                        else
                        {
                            ActivateNetworkKeyS2CustomForNode(peerNodeId, testFrame.IsTemp, GetActualNetworkKeyS2Temp());
                        }
                    }
                    else
                    {
                        if (testFrame.NetworkKey != null)
                        {
                            ActivateNetworkKeyS2CustomForNode(peerNodeId, testFrame.IsTemp, testFrame.NetworkKey);
                        }
                    }
                    //ret = _securityS2CryptoProvider.EncryptSinglecastCommand(sckey, spanTable, Network.NodeTag, node, Network.HomeId, testData, extensions, substituteSettings);
                    //if (ret == null) {

                    //}
                }
                else
                {
                    var msgEncapCryptedData = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)encryptedMsg;
                    msgEncapCryptedData.ccmCiphertextObject = new List<byte>(testData);
                    ret = msgEncapCryptedData;
                }
            }
            return ret;
        }


        public void SetTestSecretKeyS2(byte[] secretKey)
        {
            TestSecretKeyS2 = secretKey;
        }

        /// <summary>
        /// Activates network key from with specified security class for specified nodes
        /// </summary>
        /// <param name="peerNodeId">MSB is local NodeId, LSB is remote NodeId</param>
        /// <param name="scheme">scheme</param>
        public void ActivateNetworkKeyS2ForNode(InvariantPeerNodeId peerNodeId, SecuritySchemes scheme, bool isLRScheme)
        {
            byte[] networkKey = null;
            if (scheme == SecuritySchemes.S2_TEMP)
            {
                networkKey = GetActualNetworkKeyS2Temp();
            }
            else
            {
                networkKey = GetActualNetworkKey(scheme, isLRScheme);
            }

            if (peerNodeId.IsEmpty)
            {
                _defaultActiveNetworkKeyType = scheme;
            }
            $"Activate Network Key ({scheme}): {networkKey.GetHex()}"._DLOG();
            FireNetworkKeyS2Changed(peerNodeId, networkKey, scheme);
        }

        /// <summary>
        /// Activates temp key for specified node
        /// </summary>
        /// <param name="peerNodeId">MSB is local NodeId, LSB is remote NodeId</param>
        public void ActivateNetworkKeyS2TempForNode(InvariantPeerNodeId peerNodeId)
        {
            byte[] networkKey = GetActualNetworkKeyS2Temp();
            FireNetworkKeyS2Changed(peerNodeId, networkKey, SecuritySchemes.S2_TEMP);
        }

        /// <summary>
        /// Activates custom key for specified node
        /// </summary>
        /// <param name="nodeId">Node id</param>
        /// <param name="isTemp">Indicates if temp inclusion key will be activated</param>
        /// <param name="networkKey">Custom network key</param>
        public void ActivateNetworkKeyS2CustomForNode(InvariantPeerNodeId peerNodeId, bool isTemp, byte[] networkKey)
        {
            byte[] nk = new byte[16];
            if (networkKey != null && networkKey.Length > 0)
            {
                Array.Copy(networkKey, nk, networkKey.Length > nk.Length ? nk.Length : networkKey.Length);
            }
            FireNetworkKeyS2Changed(peerNodeId, nk, isTemp ? SecuritySchemes.S2_TEMP : SecuritySchemes.S2_ACCESS);
        }

        /// <summary>
        /// Activates network key for multicast for specified group ID
        /// </summary>
        /// <param name="groupId">Multicast group ID</param>
        /// <param name="ownerId">Multicast group owner ID</param>
        /// <param name="scheme">scheme</param>
        public void ActivateNetworkKeyS2Multi(byte groupId, NodeTag owner, SecuritySchemes scheme, bool isLRScheme)
        {
            FireNetworkKeyS2ChangedMulti(groupId, owner, GetActualNetworkKey(scheme, isLRScheme), scheme);
        }

        /// <summary>
        /// Activates custom network key for multicast for specified group ID
        /// </summary>
        /// <param name="groupId">Multicast group ID</param>
        /// <param name="ownerId">Multicast group owner ID</param>
        /// <param name="networkKey">Custom network key</param>
        public void ActivateNetworkKeyS2CustomMulti(byte groupId, NodeTag owner, byte[] networkKey)
        {
            FireNetworkKeyS2ChangedMulti(groupId, owner, networkKey, SecuritySchemes.S2_ACCESS);
        }

        public void ClearTestOverridesS2()
        {
            TestNetworkKeyS2Temp = null;
            //for (int i = 0; i < NETWORK_KEYS_COUNT; i++)
            //{
            //    if (_networkKeys[i] != null)
            //    {
            //        _networkKeys[i].TestValue = null;
            //    }
            //}
            _testSenderEntropyInputS2 = null;
            _testSpanS2 = null;
            _testSequenceNumberS2 = null;
            _testReservedFieldS2 = null;
            TestEnableClientSideAuthS2 = false;
            ClearTestFramesS2();
            ClearTestExtensionsS2();
        }

        public int NumOfTestSenderEntropyInputUsage { get; set; }

        private byte[] _testSenderEntropyInputS2;
        public byte[] TestSenderEntropyInputS2
        {
            get { return _testSenderEntropyInputS2; }
        }

        public void SetTestSenderEntropyInputS2(byte[] senderEntropy)
        {
            SetTestSenderEntropyInputS2(senderEntropy, 0);
        }

        public void SetTestSenderEntropyInputS2(byte[] senderEntropy, int numOfSenderEntropyInputUsage)
        {
            _testSenderEntropyInputS2 = senderEntropy;
            NumOfTestSenderEntropyInputUsage = numOfSenderEntropyInputUsage;
        }

        public bool TestEnableClientSideAuthS2 { get; set; }

        public int NumOfTestSpanUsage { get; set; }

        private byte[] _testSpanS2;
        public byte[] TestSpanS2
        {
            get { return _testSpanS2; }
        }

        public int NumOfTestReceiverEntropyUsage { get; set; }

        private byte[] _testReceiverEntropyS2;
        public byte[] TestReceiverEntropyS2
        {
            get { return _testReceiverEntropyS2; }
        }

        private byte? _testSequenceNumberS2;
        public byte? TestSequenceNumberS2
        {
            get { return _testSequenceNumberS2; }
        }

        private byte? _testReservedFieldS2;
        public byte? TestReservedFieldS2
        {
            get { return _testReservedFieldS2; }
        }

        public void SetTestSpanS2(byte[] span)
        {
            SetTestSpanS2(span, 0);
        }

        public void SetTestSpanS2(byte[] span, int numOfTestSpanUsage)
        {
            _testSpanS2 = span;
            NumOfTestSpanUsage = numOfTestSpanUsage;
        }

        public void SetTestReceiverEntropyS2(byte[] receiverEntropy)
        {
            SetTestReceiverEntropyS2(receiverEntropy, 0);
        }

        public void SetTestReceiverEntropyS2(byte[] receiverEntropy, int numOfTestReceiverEntropyUsage)
        {
            _testReceiverEntropyS2 = receiverEntropy;
            NumOfTestReceiverEntropyUsage = numOfTestReceiverEntropyUsage;
        }

        public void SetTestSequenceNumberS2(byte? sequenceNumber)
        {
            _testSequenceNumberS2 = sequenceNumber;
        }

        public void SetTestReservedFieldS2(byte? reserved)
        {
            _testReservedFieldS2 = reserved;
        }

        #region TestFramesS2
        public void SetTestFrameCommandS2(SecurityS2TestFrames testFrame, byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                if (!TestFramesS2.ContainsKey(testFrame))
                    TestFramesS2.Add(testFrame, new TestFrameS2Settings
                    {
                        FrameTypeV = testFrame,
                        Command = data
                    });
                else
                {
                    TestFramesS2[testFrame].Command = data;
                }
            }
            else
            {
                if (TestFramesS2.ContainsKey(testFrame))
                {
                    TestFramesS2[testFrame].Command = null;
                }
            }
        }

        public void SetTestFrameDelayS2(SecurityS2TestFrames testFrame, int delay)
        {
            if (delay > 0)
            {
                if (!TestFramesS2.ContainsKey(testFrame))
                    TestFramesS2.Add(testFrame, new TestFrameS2Settings
                    {
                        FrameTypeV = testFrame,
                        Delay = delay,
                        DelaySpecified = true
                    });
                else
                {
                    TestFramesS2[testFrame].Delay = delay;
                    TestFramesS2[testFrame].DelaySpecified = true;
                }
            }
            else
            {
                if (TestFramesS2.ContainsKey(testFrame))
                {
                    TestFramesS2[testFrame].Delay = 0;
                    TestFramesS2[testFrame].DelaySpecified = false;
                }
            }
        }

        public void SetTestFrameNetworkKeyS2(SecurityS2TestFrames testFrame, byte[] networkKey, bool isTemp)
        {
            if (!TestFramesS2.ContainsKey(testFrame))
                TestFramesS2.Add(testFrame, new TestFrameS2Settings
                {
                    FrameTypeV = testFrame,
                    NetworkKey = networkKey,
                    IsTemp = isTemp
                });
            else
            {
                TestFramesS2[testFrame].NetworkKey = networkKey;
                TestFramesS2[testFrame].IsTemp = isTemp;
            }
        }

        public void SetTestFrameEncryptedS2(SecurityS2TestFrames testFrame, bool? isEncrypted)
        {
            if (isEncrypted != null)
            {
                if (!TestFramesS2.ContainsKey(testFrame))
                    TestFramesS2.Add(testFrame, new TestFrameS2Settings
                    {
                        FrameTypeV = testFrame,
                        IsEncrypted = isEncrypted.Value,
                        IsEncryptedSpecified = true
                    });
                else
                {
                    TestFramesS2[testFrame].IsEncrypted = isEncrypted.Value;
                    TestFramesS2[testFrame].IsEncryptedSpecified = true;
                }
            }
            else
            {
                if (TestFramesS2.ContainsKey(testFrame))
                {
                    TestFramesS2[testFrame].IsEncrypted = false;
                    TestFramesS2[testFrame].IsEncryptedSpecified = false;
                }
            }
        }

        public void SetTestFrameMulticastS2(SecurityS2TestFrames testFrame, bool? isMulticast)
        {
            if (isMulticast != null)
            {
                if (!TestFramesS2.ContainsKey(testFrame))
                    TestFramesS2.Add(testFrame, new TestFrameS2Settings
                    {
                        FrameTypeV = testFrame,
                        IsMulticast = isMulticast.Value,
                        IsMulticastSpecified = true
                    });
                else
                {
                    TestFramesS2[testFrame].IsMulticast = isMulticast.Value;
                    TestFramesS2[testFrame].IsMulticastSpecified = true;
                }
            }
            else
            {
                if (TestFramesS2.ContainsKey(testFrame))
                {
                    TestFramesS2[testFrame].IsMulticast = false;
                    TestFramesS2[testFrame].IsMulticastSpecified = false;
                }
            }
        }

        public void SetTestFrameBroadcastS2(SecurityS2TestFrames testFrame, bool? isBroadcast)
        {
            if (isBroadcast != null)
            {
                if (!TestFramesS2.ContainsKey(testFrame))
                    TestFramesS2.Add(testFrame, new TestFrameS2Settings
                    {
                        FrameTypeV = testFrame,
                        IsBroadcast = isBroadcast.Value,
                        IsBroadcastSpecified = true
                    });
                else
                {
                    TestFramesS2[testFrame].IsBroadcast = isBroadcast.Value;
                    TestFramesS2[testFrame].IsBroadcastSpecified = true;
                }
            }
            else
            {
                if (TestFramesS2.ContainsKey(testFrame))
                {
                    TestFramesS2[testFrame].IsBroadcast = false;
                    TestFramesS2[testFrame].IsBroadcastSpecified = false;
                }
            }
        }
        private byte? _initialS2SequenceNumber = null;
        public void SetInitialS2SequenceNumber(byte? sequenceNumber)
        {
            _initialS2SequenceNumber = sequenceNumber;
        }

        internal byte? GetInitialS2SequenceNumber()
        {
            return _initialS2SequenceNumber;
        }

        public void ClearTestFramesS2()
        {
            TestFramesS2.Clear();
        }
        #endregion

        #region TestExtensionsS2

        public void AddTestExtensionS2(TestExtensionS2Settings ext)
        {
            TestExtensionsS2.Add(ext);
        }

        public void ClearTestExtensionsS2()
        {
            TestExtensionsS2.Clear();
        }

        #endregion

        #endregion

        /// <summary>
        /// Returns network key for the specified scheme
        /// </summary>
        /// <param name="scheme">scheme</param>
        /// <returns>key</returns>
        public byte[] GetOriginalNetworkKey(SecuritySchemes scheme, bool isLongRange)
        {
            return _networkKeys[GetNetworkKeyIndex(scheme, isLongRange)].Value;
        }

        public byte[] LastSendDataBuffer { get; set; }

    }

    public enum ParameterS2Type
    {
        [Description("Test Span")]
        Span,
        [Description("Test Sender Entropy Input S2")]
        Sender_EI,
        [Description("Test Secret Key S2")]
        SecretKey,
        [Description("Test Sequence Number S2")]
        SequenceNo,
        [Description("Test Reserved field S2")]
        ReservedField
    }

    public enum DSKRequestStatuses
    {
        None,
        Started,
        Completed,
        Cancelled,
    }

    public class ValueSwitch<T> : EntityBase
    {
        public ValueSwitch()
        {

        }

        public ValueSwitch(bool isSet, T value)
        {
            Set(isSet, value);
        }

        private T mValue;
        public T Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
                Notify("Value");
            }
        }

        private bool mIsSet;
        public bool IsSet
        {
            get { return mIsSet; }
            set
            {
                mIsSet = value;
                Notify("IsSet");
            }
        }

        public void Clear()
        {
            IsSet = false;
            Value = default(T);
        }

        public void Set(bool isSet, T value)
        {
            IsSet = isSet;
            Value = value;
        }
    }
}
