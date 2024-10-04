/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System;
using System.Linq;
using ZWave;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Devices;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Security;
using ZWave.BasicApplication.Tasks;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;
using ZWave.Security.S2;

namespace BasicApplicationTests.Security.Inclusion
{
    [TestFixture]
    public class InclusionS2Tests : TestBase
    {
        private const int TEN_SECONDS_TIMOUT = 1000;

        private SecurityManagerInfo _smiFirst;

        [SetUp]
        public void SetUp()
        {
            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_BASIC.ID });
            _ctrlSecond.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_BASIC.ID });

            _ctrlFirst.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));
            _ctrlSecond.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));

            SetUpSecurity(_ctrlFirst, NKEY1_S2_C0, NKEY1_S2_C1, NKEY1_S2_C2, NKEY1_S0);
            SetUpSecurity(_ctrlSecond, NKEY2_S2_C0, NKEY2_S2_C1, NKEY2_S2_C2, NKEY2_S0);

            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;

            _smiFirst = ((SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo;
        }

        [Test]
        public void AddNode_SecureInclusion_NodeIncludedSameHomeIdNewNodeId()
        {
            // Arange.
            // Act. Assert.
            Add2Controllers(AssertInclusionSecureDone);

            // Assert.
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S0));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S0));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));

            AssertInclusionSequenceWithAllKeys(_ctrlFirst.SessionId, NODE_ID_2);
        }

        [Test]
        public void AddNode_SecureInclusionWithCSA_NodeIncludedSameHomeIdNewNodeId()
        {
            // Arange.
            // Act. Assert.
            Add2Controllers(AssertInclusionSecureDone);

            // Assert.
            // Joining node supports Security S0 CC and requested S0 network key
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S0));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S0));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));

            AssertInclusionSequenceWithAllKeys(_ctrlFirst.SessionId, NODE_ID_2);
        }

        private void AssertInclusionSequenceWithAllKeys(ushort sessionId, NodeTag node)
        {
            AssertCmdSequence(sessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.KEX_GET.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.KEX_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.KEX_SET.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.PUBLIC_KEY_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.PUBLIC_KEY_REPORT.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // KEX_SET
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // KEX_REPORT
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_GET
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_REPORT
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_VERIFY
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_TRANSFER_END
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_GET
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_REPORT
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_VERIFY
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_TRANSFER_END
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_GET
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_REPORT
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_VERIFY
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_TRANSFER_END
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_GET
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_REPORT
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_NETWORK_KEY_VERIFY
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, node, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // SECURITY_2_TRANSFER_END
                FrameLogRecord.Create(node, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID) // SECURITY_2_TRANSFER_END
            });
        }

        [Test]
        public void AddNode_SecureInclusionKEXGetWrongCommand_SecurityInclusionFails()
        {
            // Arange.
            // Set wrong NK Verify
            _smiFirst.SetTestFrameCommandS2(SecurityS2TestFrames.KEXGet, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_GET());

            // Act. Assert.
            Add2Controllers(AssertInclusionSecureFailed);

            // Assert.
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_GET.ID)
            });
        }

        [Test]
        public void AddNode_SecureInclusionKexSetSentMulticast_SecurityInclusionFails()
        {
            // Arange.
            // Set multicast Kex Set
            _smiFirst.SetTestFrameMulticastS2(SecurityS2TestFrames.KEXSet, true);

            // Act. Assert.
            Add2Controllers(AssertInclusionSecureFailed);
        }

        #region PrimaryS2S0

        [Test]
        public void AddNode_PrimaryS2S0_SecondarySchemeS2S0_NodeInfoS2S0_CheckCCTrue_S0CommunicationPossbible()
        {
            // Arange.
            // Act. Assert.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.AreEqual(SecuritySchemes.S0, expectRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS2S0_SecondarySchemeS2_NodeInfoNone_CheckCCFalse_S0CommunicationNotPossbible()
        {
            // Arange.

            // Set Security Scheme on Secondary
            _ctrlSecond.Network.IsEnabledS0 = false;

            // Act. Assert.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            Assert.IsFalse(expectRes);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS2S0_SecondarySchemeS2_NodeInfoS2S0_CheckCCTrue_S0CommunicationNotPossbible()
        {
            // Arange.
            // Set Security Scheme on Secondary
            _ctrlSecond.Network.IsEnabledS0 = false;

            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS2S0_SecondarySchemeS2S0_NodeInfoNone_CheckCCFalse_S0CommunicationPossbible()
        {
            // Arange.
            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecuritySchemes.S0 == (SecuritySchemes)expectRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS2S0_SecondarySchemeS2S0_NodeInfoS2_CheckCCTrue_S0CommunicationPossbible()
        {
            // Arange.
            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecuritySchemes.S0 == (SecuritySchemes)expectRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS2S0_SecondarySchemeS2_NodeInfoS2_CheckCCTrue_S0CommunicationNotPossbible()
        {
            // Arange.
            // Set Security Scheme on Secondary
            _ctrlSecond.Network.IsEnabledS0 = false;

            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID)
            });
        }

        [Test]

        public void AddNode_PrimaryS2S0_SecondarySchemeS2_ZeroKeysGranted_S2ProbingNotPerformed()
        {
            // Arange.
            _ctrlSecond.Network.IsEnabledS0 = false;
            // Grant zero keys to secondary.
            _smiFirst.KEXSetConfirmCallback = (schemes, isClientSide) =>
                {
                    return new KEXSetConfirmResult(false, false, false, false, false);
                };

            // Act + Assert.
            Include2Controllers(AssertInclusionOkAndProbingNotDone);
        }
        #endregion

        #region PrimaryS0
        [Test]
        public void AddNode_PrimaryS0_SecondarySchemeS2S0_NodeInfoS2S0_CheckCCTrue_S0CommunicationPossbible()
        {
            // Arange.
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();


            Assert.IsTrue(SecuritySchemes.S0 == (SecuritySchemes)expectRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS0_SecondarySchemeS2_NodeInfoNone_CheckCCFalse_S0CommunicationNotPossbible()
        {
            // Arange.

            // Set Security Scheme on Secondary
            _ctrlSecond.Network.IsEnabledS0 = false;

            // Set Security Scheme on Primary
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            // Act.
            Add2Controllers(AssertInclusionSecureFailed);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS0_SecondarySchemeS2_NodeInfoS2S0_CheckCCTrue_S0CommunicationNotPossbible()
        {
            // Arange.
            // Set Security Scheme on Secondary
            _ctrlSecond.Network.IsEnabledS0 = false;

            // Set Security Scheme on Primary
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            // Act.
            Add2Controllers(AssertInclusionSecureFailed);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS0_SecondarySchemeS2S0_NodeInfoNone_CheckCCFalse_S0CommunicationPossbible()
        {
            // Arange.

            // Set Security Scheme on Primary
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecuritySchemes.S0 == (SecuritySchemes)expectRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void AddNode_PrimaryS0_SecondarySchemeS2S0_NodeInfoS2_CheckCCTrue_S0CommunicationNotPossbible()
        {
            // Arange.
            // Set Security Scheme on Primary
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes);
        }

        [Test]
        public void AddNode_PrimaryS0_SecondarySchemeS2_NodeInfoS2_CheckCCTrue_S0CommunicationNotPossbible()
        {
            // Arange.
            // Set Security Scheme on Secondary
            _ctrlSecond.Network.IsEnabledS0 = false;

            // Set Security Scheme on Primary
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            // Act.
            Add2Controllers(AssertInclusionSecureFailed);

            //Set Security Scheme S0
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID)
            });
        }
        #endregion

        #region Bridge Tests

        //[Test]
        [Ignore(@"PhysicalDeviceFake.OnGetDataFromClient added implementation of End Device Api functions (0xA4, 0xA2), 
        but still needed implementation mechnism of binding between Virtual-Bridge-Ctrl
        INCLUSION ENDS WITH SECURITY FAILED")]
        public void AddNodeBridgeEndDevice_InclusionSuccessfull()
        {
            // Arange.
            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_BASIC.ID });
            _ctrlSecond.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_BASIC.ID });

            // Act.
            ActionToken actionTokenAdd;
            ActionToken actionTokenLearn;

            // Act.
            Assert.IsFalse(_ctrlSecond.Network.HomeId.SequenceEqual(_ctrlFirst.Network.HomeId));
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();
            #endregion

            actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT,
                (action) =>
                {
                });
            actionTokenLearn = (_ctrlSecond as BridgeController).SetVirtualDeviceLearnMode(NodeTag.Empty, VirtualDeviceLearnModes.Enable, INCLUSION_TIMEOUT,
                (action) =>
                {
                });

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();


            // Assert.
            Assert.IsTrue(actionTokenAdd.Result.State == ActionStates.Completed);
            Assert.IsTrue(actionTokenLearn.Result.State == ActionStates.Completed);
            //THIS ASSERTS DOESN"T WORKS -> NO SECURTIY -> IGNORE! 
            Assert.IsTrue(SubstituteStatuses.Done == ((AddRemoveNodeResult)actionTokenAdd.Result).SubstituteStatus);
            Assert.IsTrue(SubstituteStatuses.Done == ((SetLearnModeResult)actionTokenLearn.Result).SubstituteStatus);

            AssertInclusionSequenceWithAllKeys(_ctrlFirst.SessionId, _ctrlSecond.Network.NodeTag);

            //SendDataBridge
            var expectToken = _ctrlFirst.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = (_ctrlSecond as BridgeController).SendDataBridge(new NodeTag(_ctrlSecond.Id), new NodeTag(_ctrlFirst.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            Assert.IsTrue(SecuritySchemes.S2_ACCESS == (SecuritySchemes)expectRes.SecurityScheme);
        }

        [Test]
        public void AddNode_PrimaryS2_SecondarySchemeS2S0_NodeInfoS2S0_CheckCCTrue_S0CommunicationNotPossbible()
        {
            // Arange.
            // Act.
            _ctrlFirst.Network.SetSecuritySchemes(SecuritySchemeSet.ALLS2); // Turn off S0.
            Add2Controllers(AssertInclusionSecureDone);

            // Set Security Scheme S0.
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);

            _framesLoggerProvider.ClearLog();

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecuritySchemes.NONE == (SecuritySchemes)expectRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_BASIC.BASIC_GET.ID)
            });
        }

        [TestCase(SecuritySchemes.S2_ACCESS)]
        [TestCase(SecuritySchemes.S2_AUTHENTICATED)]
        [TestCase(SecuritySchemes.S2_UNAUTHENTICATED)]
        [TestCase(SecuritySchemes.S0)]
        public void AddNode_SendWithSpecificKey_NodeRespondsWithSameKey(SecuritySchemes activeSecurityScheme)
        {
            // Arange.
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId(NODE_ID_1, NODE_ID_2);
            // Act.
            Add2Controllers(AssertInclusionSecureDone);

            _framesLoggerProvider.ClearLog();

            _ctrlSecond.ResponseData(new COMMAND_CLASS_BASIC.BASIC_REPORT(), TXO, new COMMAND_CLASS_BASIC.BASIC_GET());

            _smiFirst.ActivateNetworkKeyS2ForNode(peerNodeId, activeSecurityScheme, false);

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            var expectPrimaryToken = _ctrlFirst.ExpectData(new COMMAND_CLASS_BASIC.BASIC_REPORT(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            var expectPrimaryRes = (ExpectDataResult)expectPrimaryToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes);
            Assert.IsTrue(expectPrimaryRes);
            Assert.IsTrue(activeSecurityScheme == (SecuritySchemes)expectRes.SecurityScheme);
            Assert.IsTrue(activeSecurityScheme == (SecuritySchemes)expectPrimaryRes.SecurityScheme);

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void AddNode_KexGetDelay_FailS2Inclusion()
        {
            // Arrange
            _smiFirst.SetTestFrameDelayS2(SecurityS2TestFrames.KEXGet, DefaultTimeouts.SECURITY_S2_KEX_GET_TIMEOUT + DefaultTimeouts.EXPIRED_EXTRA_TIMEOUT);

            // Act.
            Assert.IsFalse(_ctrlSecond.Network.HomeId.SequenceEqual(_ctrlFirst.Network.HomeId));
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            // Assert
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }
        [Test]
        public void AddNode_PublicKeyReportADelay_FailS2Inclusion()
        {
            // Arrange
            _smiFirst.SetTestFrameDelayS2(SecurityS2TestFrames.PublicKeyReportA, TEN_SECONDS_TIMOUT);

            // Act.
            Assert.IsFalse(_ctrlSecond.Network.HomeId.SequenceEqual(_ctrlFirst.Network.HomeId));
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            // Assert
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }

        [Test]
        public void AddNode_KexSetDelay_FailS2Inclusion()
        {
            // Arrange
            _smiFirst.SetTestFrameDelayS2(SecurityS2TestFrames.KEXSet, DefaultTimeouts.SECURITY_S2_KEX_SET_TIMEOUT + DefaultTimeouts.EXPIRED_EXTRA_TIMEOUT);

            // Act.
            Assert.IsFalse(_ctrlSecond.Network.HomeId.SequenceEqual(_ctrlFirst.Network.HomeId));
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            // Assert
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }

        [Test]
        public void AddNode_NetworkKeyReportDelay_FailsS2Inclusion()
        {
            // Arrange
            _smiFirst.SetTestFrameDelayS2(SecurityS2TestFrames.NetworkKeyReport_S2Access, TEN_SECONDS_TIMOUT);

            // Act.
            Assert.IsFalse(_ctrlSecond.Network.HomeId.SequenceEqual(_ctrlFirst.Network.HomeId));
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            // Assert
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }

        [Test]
        public void AddNode_TransferEndDelay_FailsS2Inclusion()
        {
            // Arrange
            _smiFirst.SetTestFrameDelayS2(SecurityS2TestFrames.TransferEndA_S2Access, TEN_SECONDS_TIMOUT);

            // Act. 
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            actionTokenAdd.WaitCompletedSignal();
            actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            // Assert
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }

        private void AssertInclusionSecureDone(AddRemoveNodeResult includeRes, SetLearnModeResult learnRes)
        {
            Assert.AreEqual(NODE_ID_2.Id, _ctrlSecond.Id);
            Assert.IsTrue(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            Assert.AreEqual(ActionStates.Completed, includeRes.State);
            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.AreEqual(SubstituteStatuses.Done, includeRes.SubstituteStatus);
            Assert.AreEqual(SubstituteStatuses.Done, learnRes.SubstituteStatus);
            Assert.AreEqual(NODE_ID_2.Id, _ctrlSecond.Network.NodeTag.Id);
            Assert.IsTrue(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.Network.HomeId));
        }

        private void AssertInclusionSecureFailed(AddRemoveNodeResult includeRes, SetLearnModeResult learnRes)
        {
            Assert.AreEqual(NODE_ID_2.Id, _ctrlSecond.Id);
            Assert.IsTrue(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            Assert.AreEqual(ActionStates.Completed, includeRes.State);
            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.AreEqual(SubstituteStatuses.Failed, includeRes.SubstituteStatus);
            Assert.AreNotEqual(SubstituteStatuses.Done, learnRes.SubstituteStatus);

            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.S0));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.S0));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }

        private void Add2Controllers(Action<AddRemoveNodeResult, SetLearnModeResult> assertAction)
        {
            // Act.
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, 200);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            var includeRes = (AddRemoveNodeResult)actionTokenAdd.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            assertAction(includeRes, learnRes);
        }

        private void Include2Controllers(Action<InclusionResult, SetLearnModeResult> assertAction)
        {
            // Act.
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var actionTokenAdd = _ctrlFirst.IncludeNode(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, 200);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            var includeRes = (InclusionResult)actionTokenAdd.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)actionTokenLearn.WaitCompletedSignal();

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            assertAction(includeRes, learnRes);
        }

        private void AssertInclusionOkAndProbingNotDone(InclusionResult includeRes, SetLearnModeResult learnRes)
        {
            Assert.AreEqual(NODE_ID_2.Id, _ctrlSecond.Id);
            Assert.IsTrue(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            Assert.AreEqual(ActionStates.Completed, includeRes.State);
            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.AreEqual(BootstrapingStatuses.Completed, includeRes.AddRemoveNode.BootstrapingStatuses);
            Assert.AreEqual(SubstituteStatuses.None, includeRes.AddRemoveNode.SubstituteStatus);
            Assert.IsNull(includeRes.NodeInfo.RequestNodeInfo.SecuritySchemes);
            // Check that NO probing was done. Request data operations are not added to ActionGroup.
            // Only _delayBeforeStart + _nodeInfo operations MUST be performed.
            Assert.AreEqual(2, includeRes.NodeInfo.RequestNodeInfo.InnerResults.Count);
            Assert.AreEqual(SubstituteStatuses.None, learnRes.SubstituteStatus);
            Assert.AreEqual(NODE_ID_2.Id, _ctrlSecond.Network.NodeTag.Id);
            Assert.IsTrue(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.Network.HomeId));
        }
    }
}