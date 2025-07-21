/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Utils;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.Security
{
    public class SecurityS2CryptoProviderBase
    {
        protected RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public static int MaxMpanIterations = 5;
        public byte LastSentMulticastGroupId { get; protected set; }

        protected byte[] GetEntropyInput()
        {
            byte[] entropy_input = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            _rngCryptoServiceProvider.GetBytes(entropy_input);
            return entropy_input;
        }

        public static bool DecryptSinglecastFrame(
            SpanContainer spanContainer,
            SinglecastKey singlecastKey,
            NodeTag sourceNodeId,
            NodeTag destNodeId,
            byte[] homeId,
            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION cmd,
            out byte[] data,
            out Extensions extensions)
        {
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
            extensions = null;
            data = null;
            bool ret = false;
            bool isDecryptSucceeded = false;
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId(destNodeId, sourceNodeId);
            extensionsList = cmd.vg1;
            if (spanContainer != null && singlecastKey != null)
            {
                spanContainer.RxSequenceNumber = cmd.sequenceNumber;
                if (spanContainer.SpanState == SpanStates.ReceiversNonce)
                {
                    #region SpanStates.ReceiversNonce
                    // Establish nonce synchronization.
                    byte[] senderEntropyInput = null;
                    if (cmd.properties1.extension == 1)
                    {
                        foreach (var extData in cmd.vg1)
                        {
                            if (extData.properties1.type == (byte)ExtensionTypes.Span &&
                                extData.extensionLength == 18 &&
                                extData.extension.Count == 16)
                            {
                                senderEntropyInput = extData.extension.ToArray();
                                break;
                            }
                        }
                    }

                    if (senderEntropyInput != null)
                    {
                        var receiverEntropyInput = spanContainer.ReceiversNonce;
                        spanContainer.InstantiateWithSenderNonce(senderEntropyInput, singlecastKey.Personalization);
                        spanContainer.NextNonce();
                        isDecryptSucceeded = DecryptS2(singlecastKey.CcmKey,
                            spanContainer.Span,
                            sourceNodeId,
                            destNodeId,
                            homeId,
                            cmd,
                            out byte[] plainData,
                            out encryptedExtensionsList);
                        if (isDecryptSucceeded)
                        {
                            extensions = new Extensions
                            {
                                ExtensionsList = extensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>(),
                                EncryptedExtensionsList = encryptedExtensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>()
                            };
                            ret = true;
                            data = plainData;
                        }
                        else
                        {
                            extensions = new Extensions
                            {
                                ExtensionsList = extensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>(),
                                EncryptedExtensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>()
                            };
                            spanContainer.SetReceiversNonceState();
                        }
                    }

                    #endregion
                }
                else if (spanContainer.SpanState == SpanStates.Span)
                {
                    #region SpanStates.Span
                    // Check nonce synchronization.
                    int attemptsCount = 5;
                    byte[] plainData = null;
                    while (!isDecryptSucceeded && attemptsCount > 0)
                    {
                        spanContainer.NextNonce();
                        attemptsCount--;
                        if (spanContainer != null)
                        {
                            isDecryptSucceeded = DecryptS2(singlecastKey.CcmKey,
                                spanContainer.Span,
                                sourceNodeId,
                                destNodeId,
                                homeId,
                                cmd,
                                out plainData,
                                out encryptedExtensionsList);
                        }
                    }
                    if (isDecryptSucceeded)
                    {
                        extensions = new Extensions
                        {
                            ExtensionsList = extensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>(),
                            EncryptedExtensionsList = encryptedExtensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>()
                        };
                        ret = true;
                        data = plainData;
                    }
                    #endregion
                }
                else
                {
                    throw new InvalidOperationException("Unexpected nonce state");
                }
            }
            return ret;
        }

        public static bool DecryptMulticastFrame(MpanTable mpanTable, MulticastKey multicastKey, NodeIdBaseTypes nodeIdBaseType, NodeGroupId nodeGroupId, byte[] homeId, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION cmd, out byte[] data, out Extensions extensions)
        {
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
            extensions = null;
            data = null;
            bool ret = false;
            extensionsList = cmd.vg1;

            if (mpanTable.CheckMpanExists(nodeGroupId) && !mpanTable.IsRecordInMOSState(nodeGroupId))
            {
                // Backup mpan.
                byte seqNo = mpanTable.GetContainer(nodeGroupId).SequenceNumber;
                byte[] mpanBackup = new byte[mpanTable.GetContainer(nodeGroupId).MpanState.Length];
                Array.Copy(mpanTable.GetContainer(nodeGroupId).MpanState, mpanBackup, mpanTable.GetContainer(nodeGroupId).MpanState.Length);
                // Try to decrypt.
                bool decryptSucceeded = false;
                int attempts = 0;
                byte[] plainData = null;
                while (!decryptSucceeded && attempts < MaxMpanIterations)
                {
                    var mpan = new byte[16];
                    if (IncrementMpan(mpanTable, multicastKey, nodeGroupId, mpan))
                    {
                        decryptSucceeded = DecryptS2(multicastKey.CcmKey,
                            mpan,
                            nodeGroupId.Node,
                            new NodeTag(nodeGroupId.GroupId),
                            homeId,
                            cmd,
                            out plainData,
                            out encryptedExtensionsList);
                    }
                    attempts++;
                }
                if (decryptSucceeded)
                {
                    extensions = new Extensions
                    {
                        ExtensionsList = extensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>(),
                        EncryptedExtensionsList = encryptedExtensionsList ?? new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>()
                    };
                    ret = true;
                    data = plainData;
                }
                else
                {
                    var restoredMpanContainer = mpanTable.AddOrReplace(nodeGroupId, seqNo, null, mpanBackup);
                }
            }
            return ret;
        }

        public static byte[] EncryptPayload(NodeTag senderId, NodeTag receiverId, byte[] homeId, byte sequenceNumber, byte[] receiverNonce,
            byte[] senderNonce, byte[] networkKey, byte[] textToEncrypt, int generationCount, bool isRealKey)
        {
            var ret = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION { sequenceNumber = sequenceNumber };
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId();
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            if (isRealKey)
            {
                SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);
            }
            else
            {
                SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);
            }
            SpanTable spanTable = new SpanTable();
            spanTable.Add(peerNodeId, receiverNonce, 0, 0);
            SpanContainer spanContainer = spanTable.GetContainer(peerNodeId);
            spanContainer.InstantiateWithSenderNonce(senderNonce, personalization);
            for (int i = 0; i < generationCount; i++)
            {
                spanContainer.NextNonce();
            }

            var aad = new AAD
            {
                SenderNodeId = senderId,
                ReceiverNodeId = receiverId,
                HomeId = homeId,
                PayloadLength = (ushort)(textToEncrypt.Length + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH),
                SequenceNumber = sequenceNumber
            };
            aad.PayloadLength += (ushort)((byte[])ret).Length;
            var cipherData = SecurityS2Utils.CcmEncryptAndAuth(ccmKey, spanContainer.Span, aad, textToEncrypt);
            if (cipherData != null && cipherData.Length > 0)
            {
                ret.ccmCiphertextObject = new List<byte>(cipherData);
            }
            else
            {
                ret = null;
            }
            return ret;
        }

        public static byte[] DecryptPayload(NodeTag senderId, NodeTag receiverId, byte[] homeId, byte sequenceNumber, byte[] receiverNonce,
            byte[] senderNonce, byte[] networkKey, int generationCount, bool isRealKey, byte[] fullMessageS2, out int currentGenerationCount)
        {
            byte[] ret = null;
            currentGenerationCount = 0;
            if (fullMessageS2 != null && fullMessageS2.Length > 2)
            {
                var msgEncap = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)fullMessageS2;

                var peerNodeId = new InvariantPeerNodeId();
                var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
                var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
                var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
                if (isRealKey)
                {
                    SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);
                }
                else
                {
                    SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);
                }
                SpanTable spanTable = new SpanTable();
                spanTable.Add(peerNodeId, receiverNonce, 0, 0);
                SpanContainer spanContainer = spanTable.GetContainer(peerNodeId);
                spanContainer.InstantiateWithSenderNonce(senderNonce, personalization);


                var messageLength = (ushort)fullMessageS2.Length;
                var aad = new AAD
                {
                    SenderNodeId = senderId,
                    ReceiverNodeId = receiverId,
                    HomeId = homeId,
                    PayloadLength = messageLength,
                    SequenceNumber = sequenceNumber,
                    StatusByte = msgEncap.properties1
                };

                if (msgEncap.properties1.extension == 1)
                {
                    var dataList = new List<byte>();
                    foreach (var vg1 in msgEncap.vg1)
                    {
                        dataList.AddRange(new byte[] { vg1.extensionLength, vg1.properties1 });
                        dataList.AddRange(vg1.extension);
                    }
                    aad.ExtensionData = dataList.ToArray();
                }

                for (int i = 0; i < generationCount; i++)
                {
                    spanContainer.NextNonce();
                    ret = SecurityS2Utils.CcmDecryptAndAuth(ccmKey, spanContainer.Span, aad, msgEncap.ccmCiphertextObject.ToArray());
                    if (ret != null && ret.Length > 0)
                    {
                        currentGenerationCount = i + 1;
                        break;
                    }
                }
            }
            return ret;
        }

        protected static byte[] EncryptS2Internal(byte[] key, byte[] iv, NodeTag senderNodeId, NodeTag receiverNodeId,
            byte[] homeId,
            ushort payloadLength, byte sequenceNumber, byte statusByte, byte[] extensionData, byte[] data)
        {
            byte[] aad = new AAD
            {
                SenderNodeId = senderNodeId,
                ReceiverNodeId = receiverNodeId,
                HomeId = homeId,
                SequenceNumber = sequenceNumber,
                StatusByte = statusByte,
                PayloadLength = payloadLength,
                ExtensionData = extensionData
            };
            return SecurityS2Utils.CcmEncryptAndAuth(key, iv, aad, data);
        }

        private static byte[] DecryptS2Internal(byte[] key, byte[] iv, NodeTag senderNodeId, NodeTag receiverNodeId,
            byte[] homeId,
            ushort payloadLength, byte sequenceNumber, byte statusByte, byte[] extensionData, byte[] data)
        {
            byte[] aad = new AAD
            {
                SenderNodeId = senderNodeId,
                ReceiverNodeId = receiverNodeId,
                HomeId = homeId,
                PayloadLength = payloadLength,
                SequenceNumber = sequenceNumber,
                StatusByte = statusByte,
                ExtensionData = extensionData
            };
            return SecurityS2Utils.CcmDecryptAndAuth(key, iv, aad, data);
        }

        public byte[] EncryptMulticastCommand(MulticastKey mckey, MpanTable mpanTable, NodeIdBaseTypes nodeIdBaseType, NodeGroupId nodeGroupId, byte[] homeId, byte[] plainData)
        {
            byte[] encryptedData = null;
            var mpan = new byte[16];
            if (IncrementMpan(mpanTable, mckey, nodeGroupId, mpan)) // Spec [CC:009F.01.00.11.028]
            {
                var iv = GetPropertyIV(mpan);
                var sequenceNumber = GetPropertySequenceNo(mpanTable.GetContainer(nodeGroupId).SequenceNumber);
                var reserved = GetPropertyReserved(0);

                var extentions = new Extensions();
                extentions.AddMpanGrpExtension(nodeGroupId.GroupId);
                FillExtensions(extentions, false);
                encryptedData = EncryptS2(mckey.CcmKey,
                    sequenceNumber,
                    plainData,
                    nodeGroupId.Node,
                    new NodeTag(nodeGroupId.GroupId),
                    homeId,
                    iv,
                    reserved,
                    extentions);
                LastSentMulticastGroupId = nodeGroupId.GroupId;
            }
            return encryptedData;
        }

        public byte[] EncryptSinglecastCommand(SinglecastKey sckey, SpanTable spanTable, NodeTag senderNodeId, NodeTag receiverNodeId, byte[] homeId, byte[] plainData, Extensions extentions, SubstituteSettings substituteSettings)
        {
            byte[] encryptedData = null;
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId(senderNodeId, receiverNodeId);
            var spanContainer = spanTable.GetContainer(peerNodeId);
            if (spanContainer != null)
            {
                encryptedData = EncryptSinglecastCommandInternal(sckey, spanTable, senderNodeId, receiverNodeId, homeId, plainData, extentions, substituteSettings);
            }
            else
            {
            }
            return encryptedData;
        }

        protected byte[] EncryptSinglecastCommandInternal(SinglecastKey sckey, SpanTable spanTable, NodeTag senderNodeId, NodeTag receiverNodeId, byte[] homeId, byte[] plainData, Extensions extentions, SubstituteSettings substituteSettings)
        {
            if (plainData.GetHex() == "9F 0C 02")
            {

            }
            Extensions ext = extentions ?? new Extensions();
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId(senderNodeId, receiverNodeId);
            var spanContainer = spanTable.GetContainer(peerNodeId);
            var spanExtension = GetPersonalizationArray(sckey, spanContainer);
            if (spanExtension != null)
            {
                ext.AddSpanExtension(spanExtension);
            }
            spanContainer.NextNonce();
            spanTable.UpdateTxSequenceNumber(peerNodeId);

            var iv = GetPropertyIV(spanContainer.Span);
            var sequenceNumber = GetPropertySequenceNo(spanContainer.TxSequenceNumber);
            var reserved = GetPropertyReserved(0);
            FillExtensions(ext, true);
            byte[] encryptedData = EncryptS2(sckey.CcmKey,
                sequenceNumber,
                plainData,
                senderNodeId,
                receiverNodeId,
                homeId,
                iv,
                reserved,
                ext);
            if (plainData != null && encryptedData != null)
            {
                SaveDataForRetransmission(peerNodeId, plainData, substituteSettings, sckey.SecurityScheme);
            }
            return encryptedData;
        }

        protected static byte[] EncryptS2(byte[] key, byte sequenceNumber, byte[] cmdData, NodeTag senderNodeId, NodeTag receiverNodeId,
            byte[] homeId, byte[] iv, byte reserved, Extensions extensions)
        {
            var dataToEncrypt = new List<byte>();

            var ret = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION
            {
                sequenceNumber = sequenceNumber,
                vg1 = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>()
            };

            if (extensions != null)
            {
                if (!extensions.ExtensionsList.IsNullOrEmpty())
                {
                    ret.properties1.extension = 1;
                    ret.vg1.AddRange(extensions.ExtensionsList);
                }
                if (!extensions.EncryptedExtensionsList.IsNullOrEmpty())
                {
                    ret.properties1.encryptedExtension = 1;
                    foreach (var encExt in extensions.EncryptedExtensionsList)
                    {
                        dataToEncrypt.Add(encExt.extensionLength);
                        dataToEncrypt.Add(encExt.properties1);
                        if (encExt.extension != null && encExt.extension.Count > 0)
                        {
                            dataToEncrypt.AddRange(encExt.extension);
                        }
                    }
                }
            }
            if (!cmdData.IsNullOrEmpty())
            {
                dataToEncrypt.AddRange(cmdData);
            }

            ret.properties1.reserved = reserved;
            byte[] extensionData = new byte[0];
            if (ret.properties1.extension == 1)
            {
                var tempExtDataList = new List<byte>();
                foreach (var ext in ret.vg1)
                {
                    tempExtDataList.AddRange(new byte[] { ext.extensionLength, ext.properties1 });
                    if (ext.extension != null)
                    {
                        tempExtDataList.AddRange(ext.extension);
                    }
                }
                extensionData = tempExtDataList.ToArray();
            }

            var payloadLength = (ushort)(4 + extensionData.Length + dataToEncrypt.Count + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH);

            var cipherData = EncryptS2Internal(key, iv, senderNodeId, receiverNodeId, homeId, payloadLength, ret.sequenceNumber, ret.properties1, extensionData, dataToEncrypt.ToArray());
            ret.ccmCiphertextObject = new List<byte>(cipherData);
            return ret;
        }

        public static SinglecastKey GetSinglecastKey(byte[] networkKey, bool isTemp)
        {
            SinglecastKey ret;
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            if (isTemp)
            {
                SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);
                ret = new SinglecastKey(networkKey[0], ccmKey, personalization, ZWave.Enums.SecuritySchemes.S2_TEMP);
            }
            else
            {
                SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);
                ret = new SinglecastKey(networkKey[0], ccmKey, personalization, ZWave.Enums.SecuritySchemes.S2_ACCESS);
            }
            return ret;
        }

        public static bool DecryptS2(
            byte[] key,
            byte[] iv,
            NodeTag senderNodeId,
            NodeTag receiverNodeId,
            byte[] homeId,
            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION cmd,
            out byte[] decryptedPayload,
            out List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> decryptedExtensions)
        {
            var cipherData = cmd.ccmCiphertextObject.ToArray();
            decryptedPayload = new byte[0];
            decryptedExtensions = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
            if (cipherData.Length == SecurityS2Utils.AUTH_DATA_HEADER_LENGTH)
            {
                return true;
            }

            var payloadLength = (ushort)((byte[])cmd).Length;
            var sequenceNumber = cmd.sequenceNumber;
            var statusByte = cmd.properties1;
            byte[] extensionData = new byte[0];

            if (cmd.properties1.extension == 1)
            {
                var dataList = new List<byte>();
                foreach (var vg1 in cmd.vg1)
                {
                    dataList.AddRange(new byte[] { vg1.extensionLength, vg1.properties1 });
                    dataList.AddRange(vg1.extension);
                }
                extensionData = dataList.ToArray();
            }

            var decryptedData = DecryptS2Internal(key, iv, senderNodeId, receiverNodeId, homeId, payloadLength,
                sequenceNumber, statusByte, extensionData, cipherData);
            var isDecryptSucceeded = decryptedData.Length > 0;
            if (isDecryptSucceeded)
            {
                if (cmd.properties1.encryptedExtension == 1)
                {
                    try
                    {
                        int ind = 0;
                        while (ind + 2 <= decryptedData.Length && ind + decryptedData[ind] <= decryptedData.Length)
                        {
                            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1 ext =
                                new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1
                                {
                                    extensionLength = decryptedData[ind],
                                    properties1 = decryptedData[ind + 1]
                                };

                            if (ind + ext.extensionLength <= decryptedData.Length)
                            {
                                ext.extension = decryptedData.Skip(ind + 2).Take(ext.extensionLength - 2).ToList();
                            }
                            else
                            {
                                ext.extension = new List<byte>(0);
                            }

                            decryptedExtensions.Add(ext);
                            ind += ext.extensionLength;

                            if (ext.properties1.moreToFollow == 0)
                            {
                                break;
                            }
                        }
                        if (ind < decryptedData.Length) // Has payload.
                        {
                            decryptedPayload = decryptedData.Skip(ind).ToArray();
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                else
                {
                    decryptedPayload = decryptedData;
                }
            }
            return isDecryptSucceeded;
        }

        /// <summary>
        /// Gets 16 bytes of pseudo random data
        /// </summary>
        /// <returns>16 bytes of pseudo random data</returns>
        public byte[] GetRandomData()
        {
            var ctx = new CTR_DRBG_CTX();
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.AesCtrDrbgInstantiate(ref ctx, GetEntropyInput(), personalization);
            var ret = new byte[SecurityS2Utils.KEY_SIZE];
            SecurityS2Utils.AesCtrDrbgGenerate(ref ctx, ret);
            return ret;
        }

        public byte[] GenerateNonceReport(SpanTable spanTable, InvariantPeerNodeId peerNodeId, byte txSequenceNumber, byte rxSequenceNumber,
            bool isSos, bool isMos)
        {
            byte[] receiverNonce = GetRandomData();
            if (isSos)
            {
                if (spanTable.CheckNonceExists(peerNodeId))
                {
                    spanTable.SetNonceFree(peerNodeId);
                }
                spanTable.Add(peerNodeId, receiverNonce, txSequenceNumber, rxSequenceNumber);
            }
            var nonceReportCmd = new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT
            {
                sequenceNumber = txSequenceNumber,
                properties1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.Tproperties1
                {
                    sos = (byte)(isSos ? 1 : 0),
                    mos = (byte)(isMos ? 1 : 0)
                },
                receiversEntropyInput = receiverNonce
            };
            return nonceReportCmd;
        }

        //public bool HasActiveKeyForNode(InvariantPeerNodeId peerNodeId)
        //{
        //    return ScKeys.ContainsKey(peerNodeId);
        //}

        //public SecuritySchemes GetActiveSecuritySchemeForNode(InvariantPeerNodeId peerNodeId)
        //{
        //    return ScKeys[peerNodeId].SecurityScheme;
        //}

        //public SecuritySchemes GetActiveSecuritySchemeForGroup(NodeGroupId peerGroupId)
        //{
        //    return McKeys[peerGroupId].SecurityScheme;
        //}

        //public bool HasActiveKeyForGroupId(byte groupId, byte ownerId)
        //{

        //    return McKeys.ContainsKey(new NodeGroupId(ownerId, groupId));
        //}

        public bool CheckMpanMosForOwnerNode(MpanTable mpanTable, NodeTag ownerNode)
        {
            var groupIds = mpanTable.SelectGroupIds(ownerNode);
            bool isMos = false;
            foreach (var groupId in groupIds)
            {
                var nodeGroupId = new NodeGroupId(ownerNode, groupId);
                if (mpanTable.IsRecordInMOSState(nodeGroupId))
                {
                    // If found several goups leave the first one and remove another.
                    if (!isMos)
                    {
                        isMos = true;
                    }
                    else
                    {
                        mpanTable.RemoveRecord(nodeGroupId);
                    }
                }
            }
            return isMos;
        }

        public static bool DecrementMpan(MpanTable mpanTable, MulticastKey multicastKey, NodeGroupId nodeGroupId, byte[] outMpan)
        {
            if (mpanTable.CheckMpanExists(nodeGroupId) && !mpanTable.IsRecordInMOSState(nodeGroupId))
            {
                var container = mpanTable.GetContainer(nodeGroupId);
                SecurityS2Utils.Aes128EcbEncrypt(container.MpanState, multicastKey.MpanKey, outMpan);
                container.DecrementMpanState();
                return true;
            }
            return false;
        }

        public static bool IncrementMpan(MpanTable mpanTable, MulticastKey multicastKey, NodeGroupId nodeGroupId, byte[] outMpan)
        {
            if (mpanTable.CheckMpanExists(nodeGroupId) && !mpanTable.IsRecordInMOSState(nodeGroupId))
            {
                var container = mpanTable.GetContainer(nodeGroupId);
                SecurityS2Utils.Aes128EcbEncrypt(container.MpanState, multicastKey.MpanKey, outMpan);
                container.IncrementMpanState();
                container.UpdateSequenceNumber();
                return true;
            }
            return false;
        }

        //public void Reset(SpanTable spanTable, MpanTable mpanTable)
        //{
        //    spanTable.ClearNonceTable();
        //    mpanTable.ClearMpanTable();
        //    ScKeys.Clear();
        //    McKeys.Clear();
        //}

        protected virtual void FillExtensions(Extensions extensions, bool isSinglecast)
        {
        }

        protected virtual byte GetPropertyReserved(byte reservedByte)
        {
            return reservedByte;
        }

        protected virtual byte GetPropertySequenceNo(byte sequenceNo)
        {
            return sequenceNo;
        }

        protected virtual byte[] GetPropertyIV(byte[] iv)
        {
            byte[] ret = new byte[16];
            Array.Copy(iv, ret, iv.Length);
            return ret;
        }

        protected virtual byte[] GetPersonalizationArray(SinglecastKey sckey, SpanContainer spanContainer)
        {
            byte[] senderNonce = null;
            if (spanContainer.SpanState == SpanStates.ReceiversNonce)
            {
                senderNonce = GetRandomData(); // Generate sender nonce.
                spanContainer.InstantiateWithSenderNonce(senderNonce, sckey.Personalization);
            }
            return senderNonce;
        }

        protected virtual void SaveDataForRetransmission(InvariantPeerNodeId peerNodeId, byte[] plainData, SubstituteSettings substituteSettings, SecuritySchemes scheme)
        {
        }
    }

    public class SinglecastKey
    {
        public byte KeyId { get; private set; }
        public byte[] CcmKey { get; private set; }
        public byte[] Personalization { get; private set; }
        public SecuritySchemes SecurityScheme { get; private set; }
        public SinglecastKey(byte keyId, byte[] ccmKey, byte[] personalization, SecuritySchemes scheme)
        {
            Update(keyId, ccmKey, personalization, scheme);
        }

        public void Update(byte keyId, byte[] ccmKey, byte[] personalization, SecuritySchemes scheme)
        {
            KeyId = keyId;
            CcmKey = ccmKey;
            Personalization = personalization;
            SecurityScheme = scheme;
        }
    }

    public class MulticastKey
    {
        public byte[] CcmKey { get; set; }
        public byte[] MpanKey { get; set; }
        public SecuritySchemes SecurityScheme { get; set; }
    }
}