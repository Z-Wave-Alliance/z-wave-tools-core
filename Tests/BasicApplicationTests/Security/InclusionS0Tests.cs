/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using NUnit.Framework;
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave;

namespace BasicApplicationTests.Security.Inclusion
{
    [TestFixture]
    public class InclusionS0Tests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_BASIC.ID, COMMAND_CLASS_SECURITY.ID });
            _ctrlSecond.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_BASIC.ID, COMMAND_CLASS_SECURITY.ID });

            SetUpSecurity(_ctrlFirst);
            SetUpSecurity(_ctrlSecond);

            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = false;
        }

        [Test]

        public void AddNode_SecureInclusion_NodeIncludedSameHomeIdNewNodeId()
        {
            // Arange.
            // Act.
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));

            var addToken = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var learnToken = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            var addRes = (AddRemoveNodeResult)addToken.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)learnToken.WaitCompletedSignal();

            // Assert.
            Assert.AreEqual(ActionStates.Completed, addRes.State);
            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.AreEqual(SubstituteStatuses.Done, addRes.SubstituteStatus);
            Assert.AreEqual(SubstituteStatuses.Done, learnRes.SubstituteStatus);
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.S0));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.S0));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));
        }
    }
}
