/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using System.Linq;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Security;
using ZWave.BasicApplication.Tasks;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace BasicApplicationTests.Security.Inclusion
{
    [TestFixture]
    public class InclusionControllerTests : TestBase
    {
        private const int TEN_SECONDS_TIMOUT = 1000;

        private SecurityManagerInfo _smiFirst;

        [SetUp]
        public void SetUp()
        {
            byte[] commandClasses = new byte[]
            {
                COMMAND_CLASS_BASIC.ID,
                COMMAND_CLASS_VERSION.ID,
                COMMAND_CLASS_ASSOCIATION.ID,
                COMMAND_CLASS_INCLUSION_CONTROLLER.ID, // Enable Inclusion support
                COMMAND_CLASS_CONTROLLER_REPLICATION.ID,
                COMMAND_CLASS_SECURITY.ID,
                COMMAND_CLASS_SECURITY_2.ID
            };

            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, commandClasses);
            _ctrlSecond.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_BASIC.ID });
            _ctrlThird.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_BASIC.ID });

            _ctrlFirst.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));
            _ctrlSecond.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));
            _ctrlThird.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));

            SetUpSecurity(_ctrlFirst, NKEY1_S2_C0, NKEY1_S2_C1, NKEY1_S2_C2, NKEY1_S0);
            SetUpSecurity(_ctrlSecond, NKEY2_S2_C0, NKEY2_S2_C1, NKEY2_S2_C2, NKEY2_S0);
            SetUpSecurity(_ctrlThird, NKEY3_S2_C0, NKEY3_S2_C1, NKEY3_S2_C2, NKEY3_S0);

            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;

            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;

            _ctrlThird.Network.IsEnabledS0 = true;
            _ctrlThird.Network.IsEnabledS2_ACCESS = true;
            _ctrlThird.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlThird.Network.IsEnabledS2_UNAUTHENTICATED = true;

            _smiFirst = ((SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo;
        }

        [Test]
        public void AddNode_InclusionController_NodeIncludedSameHomeIdNewNodeId()
        {
            // Arange.
            // Act. Assert.
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlSecond.HomeId));
            Assert.IsFalse(_ctrlFirst.HomeId.SequenceEqual(_ctrlThird.HomeId));
            Assert.IsFalse(_ctrlSecond.HomeId.SequenceEqual(_ctrlThird.HomeId));

            var res = _ctrlFirst.SetSucNodeID(_ctrlFirst.Network.NodeTag, true, false, 0x01);
            var actionTokenAdd = _ctrlFirst.IncludeNode(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, 200);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);
            var includeRes = (InclusionResult)actionTokenAdd.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)actionTokenLearn.WaitCompletedSignal();

            Assert.IsTrue(includeRes.AddRemoveNode.SubstituteStatus == SubstituteStatuses.Done);
            Assert.IsTrue(learnRes.SubstituteStatus == SubstituteStatuses.Done);
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));

            actionTokenAdd = _ctrlSecond.IncludeNode(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlSecond.WaitNodeStatusSignal(NodeStatuses.LearnReady, 200);
            actionTokenLearn = _ctrlThird.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);
            includeRes = (InclusionResult)actionTokenAdd.WaitCompletedSignal();
            learnRes = (SetLearnModeResult)actionTokenLearn.WaitCompletedSignal();

            Assert.IsTrue(includeRes.AddRemoveNode.SubstituteStatus == SubstituteStatuses.Done);
            Assert.IsTrue(learnRes.SubstituteStatus == SubstituteStatuses.Done);
            Assert.IsTrue(_ctrlThird.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
        }
    }
}