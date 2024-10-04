/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using System.Collections.Generic;
using ZWave.Security;
using ZWave.CommandClasses;
using ZWave.Configuration;
using ZWave.Enums;
using System;
using Utils;

namespace ZWave.BasicApplication.Security
{
    public class SecurityS2CryptoProvider : SecurityS2CryptoProviderBase
    {
        private readonly SecurityManagerInfo _securityManagerInfo;

        public SecurityS2CryptoProvider(SecurityManagerInfo securityManagerInfo)
        {
            _securityManagerInfo = securityManagerInfo;
        }

        protected override byte[] GetPersonalizationArray(SinglecastKey sckey, SpanContainer spanContainer)
        {
            byte[] senderNonce = null;
            if (_securityManagerInfo.TestReceiverEntropyS2 != null)
            {
                senderNonce = GetRandomData();
                spanContainer.SetReceiversNonceState(_securityManagerInfo.TestReceiverEntropyS2);
                spanContainer.InstantiateWithSenderNonce(senderNonce, sckey.Personalization);
                if (_securityManagerInfo.NumOfTestReceiverEntropyUsage > 0)
                {
                    _securityManagerInfo.NumOfTestReceiverEntropyUsage--;
                    if (_securityManagerInfo.NumOfTestReceiverEntropyUsage == 0)
                        _securityManagerInfo.SetTestReceiverEntropyS2(null);
                }
            }
            else
            {
                senderNonce = base.GetPersonalizationArray(sckey, spanContainer);
            }
            return senderNonce;
        }

        protected override void SaveDataForRetransmission(InvariantPeerNodeId peerNodeId, byte[] plainData, SubstituteSettings substituteSettings, SecuritySchemes scheme)
        {
            /* Delete when counter:
             * 0 - item was retransmitted sucessfully (only one Nonce Report)
             * 2 - item sent from first try sucessfully (no Nonce Reports were received)
            */
            if (_securityManagerInfo.RetransmissionTableS2.TryGetValue(peerNodeId, out RetransmissionRecord rrec))
            {
                if (rrec.Counter == 0 || rrec.Counter == 2)
                {
                    if (_securityManagerInfo.RetransmissionTableS2.TryRemove(peerNodeId, out RetransmissionRecord rr1))
                    {
                    }
                }
            }

            _securityManagerInfo.RetransmissionTableS2.AddOrUpdate(peerNodeId,
                new RetransmissionRecord(plainData) { SubstituteSettings = substituteSettings, SecurityScheme = scheme, Counter = 2 },
                (x, y) =>
                {
                    y.Data = plainData;
                    y.Counter--;
                    return y;
                });

            if (_securityManagerInfo.RetransmissionTableS2.TryGetValue(peerNodeId, out RetransmissionRecord rr2))
            {
            }
        }

        protected override byte[] GetPropertyIV(byte[] iv)
        {
            byte[] ret = null; ;
            if (_securityManagerInfo.TestSpanS2 != null)
            {
                if (_securityManagerInfo.NumOfTestSpanUsage > 0)
                {
                    _securityManagerInfo.NumOfTestSpanUsage--;
                    ret = _securityManagerInfo.TestSpanS2;

                    if (_securityManagerInfo.NumOfTestSpanUsage == 0)
                        _securityManagerInfo.SetTestSpanS2(null);
                }
                else
                {
                    ret = _securityManagerInfo.TestSpanS2;
                }
            }
            else
            {
                ret = base.GetPropertyIV(iv);
            }
            return ret;
        }

        protected override byte GetPropertySequenceNo(byte sequenceNo)
        {
            if (_securityManagerInfo.TestSequenceNumberS2 != null)
            {
                return (byte)_securityManagerInfo.TestSequenceNumberS2;
            }
            else
            {
                return base.GetPropertySequenceNo(sequenceNo);
            }
        }

        protected override byte GetPropertyReserved(byte reservedByte)
        {
            if (_securityManagerInfo.TestReservedFieldS2 != null)
            {
                return (byte)_securityManagerInfo.TestReservedFieldS2;
            }
            else
            {
                return base.GetPropertyReserved(reservedByte);
            }
        }

        protected override void FillExtensions(Extensions extensions, bool isSinglecast)
        {
            List<MessageTypes> msgTypes = GetMessageTypes(extensions, isSinglecast);

            ApplyTestExtensionsSettings(msgTypes, ref extensions);
            if (extensions == null)
            {
                return;
            }

            if (_securityManagerInfo.TestSenderEntropyInputS2 != null)
            {

                if (_securityManagerInfo.NumOfTestSenderEntropyInputUsage > 0)
                {
                    _securityManagerInfo.NumOfTestSenderEntropyInputUsage--;
                    foreach (var ext in extensions.ExtensionsList)
                    {
                        if (ext.properties1.type == (byte)ExtensionTypes.Span)
                        {
                            ext.extension = new List<byte>(_securityManagerInfo.TestSenderEntropyInputS2);
                            break;
                        }
                    }

                    if (_securityManagerInfo.NumOfTestSenderEntropyInputUsage == 0)
                        _securityManagerInfo.SetTestSenderEntropyInputS2(null);
                }
                else
                {
                    foreach (var ext in extensions.ExtensionsList)
                    {
                        if (ext.properties1.type == (byte)ExtensionTypes.Span)
                        {
                            ext.extension = new List<byte>(_securityManagerInfo.TestSenderEntropyInputS2);
                            break;
                        }
                    }
                }
            }
        }

        private static List<MessageTypes> GetMessageTypes(Extensions extensions, bool isSinglecast)
        {
            List<MessageTypes> msgTypes = new List<MessageTypes>();

            // Prepare list of current extension types.
            if (extensions != null)
            {
                if (isSinglecast)
                {
                    if (extensions.EncryptedExtensionsList != null && extensions.EncryptedExtensionsList.Count > 0)
                    {
                        msgTypes.Add(MessageTypes.SinglecastWithMpan);
                    }
                    if (extensions.ExtensionsList != null)
                    {
                        foreach (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1 extension in extensions.ExtensionsList)
                        {
                            if (extension.properties1.type == (byte)ExtensionTypes.Span)
                            {
                                msgTypes.Add(MessageTypes.SinglecastWithSpan);
                            }
                            if (extension.properties1.type == (byte)ExtensionTypes.MpanGrp)
                            {
                                // Only followup Messages and Multicast contain MGRP extension.
                                msgTypes.Add(MessageTypes.SinglecastWithMpanGrp);
                            }
                            if (extension.properties1.type == (byte)ExtensionTypes.Mos)
                            {
                                msgTypes.Add(MessageTypes.SinglecastWithMos);
                            }
                        }
                    }
                }
                else
                {
                    msgTypes.Add(MessageTypes.MulticastAll);
                }
            }
            msgTypes.Add(isSinglecast ? MessageTypes.SinglecastAll : MessageTypes.MulticastAll);
            return msgTypes;
        }

        public void ApplyTestExtensionsSettings(List<MessageTypes> msgTypes, ref Extensions extensions)
        {
            if (_securityManagerInfo.TestExtensionsS2.Count > 0)
            {
                TestExtensionS2Settings.ResetTracker();
                var testExtsByMessageTypes = _securityManagerInfo.TestExtensionsS2.Where(ext => msgTypes.Contains(ext.MessageTypeV));
                if (testExtsByMessageTypes.Any())
                {
                    var testExtensions = new Extensions();
                    testExtsByMessageTypes.ToList().ApplyTestTestSettings(testExtensions.ExtensionsList, testExtensions.EncryptedExtensionsList);
                    if (extensions == null)
                    {
                        extensions = new Extensions();
                    }
                    testExtsByMessageTypes.ToList().ApplyTestSettings(extensions.ExtensionsList, extensions.EncryptedExtensionsList);
                    extensions.MergeWith(testExtensions);
                }
            }
        }

        internal byte[] GenerateSecretKey()
        {
            byte[] ret = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            var ctx = new CTR_DRBG_CTX();

            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.AesCtrDrbgInstantiate(ref ctx, GetEntropyInput(), personalization);

            var first_sequence = new byte[SecurityS2Utils.KEY_SIZE];
            SecurityS2Utils.AesCtrDrbgGenerate(ref ctx, first_sequence);
            var second_sequence = new byte[SecurityS2Utils.KEY_SIZE];
            SecurityS2Utils.AesCtrDrbgGenerate(ref ctx, second_sequence);

            Array.Copy(first_sequence, ret, 16);
            Array.Copy(second_sequence, 0, ret, 16, 16);
            return ret;
        }
    }
}
