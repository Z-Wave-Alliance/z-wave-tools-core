using System.Linq;
using NUnit.Framework;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Devices;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave;
using System.Collections.Generic;
using Utils;
using ZWave.BasicApplication.Security;
using ZWave.BasicApplication.Tasks;
using ZWave.BasicApplication.Enums;
using System.Threading;
using ZWave.Security.S2;
using ZWave.Devices;

namespace BasicApplicationTests.RequestNodeInfo
{
    public class NodeInfoTests : TestBase
    {
        public int SUPPORTED_TIMEOUT;

        public byte[] cmdS2SupportedGet = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET();
        public byte[] cmdS2SupportedReport = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT();
        public byte[] cmdS0SupportedGet = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET();
        public byte[] cmdS0SupportedReport = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT();

        private SecurityManagerInfo _smiFirst;
        private SecurityManagerInfo _smiSecond;

        [SetUp]
        public void SetUp()
        {
            var oneSecTimeout = 334;
            _ctrlFirst.Network.RequestTimeoutMs = oneSecTimeout;
            _ctrlSecond.Network.RequestTimeoutMs = oneSecTimeout;
            _ctrlThird.Network.RequestTimeoutMs = oneSecTimeout;

            SUPPORTED_TIMEOUT = 8 * oneSecTimeout;

            _ctrlFirst.SerialApiGetCapabilities();
            _ctrlSecond.SerialApiGetCapabilities();

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
            _smiSecond = ((SecurityManager)_ctrlSecond.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo;
        }

        protected InclusionResult IncludeSecondary()
        {
            var actionTokenAdd = _ctrlFirst.IncludeNode(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var actionTokenLearn = _ctrlSecond.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);

            var includeRes = actionTokenAdd.WaitCompletedSignal() as InclusionResult;
            var learnRes = actionTokenLearn.WaitCompletedSignal() as SetLearnModeResult;

            Assert.IsTrue(includeRes);
            Assert.IsTrue(learnRes);
            Assert.AreEqual(SubstituteStatuses.Done, includeRes.AddRemoveNode.SubstituteStatus);
            Assert.AreEqual(SubstituteStatuses.Done, learnRes.SubstituteStatus);

            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();
            return includeRes;
        }

        [Test]
        public void ReProbingEachKey_InCorrectOrder()
        {
            //Arrange
            IncludeSecondary();
            var collector = ListenDataCollector.Create(_ctrlFirst, _ctrlSecond);

            //Action
            collector.Start(true);
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();
            Thread.Sleep(500);
            var logs = collector.Stop();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);

            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);

            Assert.AreEqual(SecuritySchemeSet.ALL.Length * 2, logs.Count);
            int i = 0;
            foreach (var item in SecuritySchemeSet.ALL)
            {
                Assert.AreEqual(logs[i].First, item);
                Assert.AreEqual(logs[++i].First, item);
                i++;
            }

            //From second
            collector.Start(true);
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            getS0Token = _ctrlFirst.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS0Token = _ctrlSecond.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();
            logs = collector.Stop();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);

            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);

            Assert.AreEqual(SecuritySchemeSet.ALL.Length * 2, logs.Count);
            i = 0;
            foreach (var item in SecuritySchemeSet.ALL)
            {
                Assert.AreEqual(logs[i].First, item);
                Assert.AreEqual(logs[++i].First, item);
                i++;
            }
        }

        [Test]
        public void DisableHighestAfterInclusion_OnSecondary_RequestFromPrimary_UsesHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;

            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme, "Probes Highest");
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme, "but reports on next high available");


            //2.Enable Key Again
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;

            //Act.
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableHighestAfterInclusion_OnSecondary_RequestFromSecondary_UsesHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;

            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme, "Probes Highest");
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2.Enable Key Again
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;

            //Act.
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableHighestAfterInclusion_OnPrimary_RequestFromPrimary_UsesHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            ((SecurityManager)_ctrlSecond.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo
                .SetNetworkKey(NKEY1_S2_C2, SecuritySchemes.S2_ACCESS, false);

            //Action
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableHighestAfterInclusion_OnPrimary_RequestFromSecondary_UsesHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;

            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;

            //Action
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableAllS2AfterInclusion_OnPrimary_RequestFromPrimary_RestoresHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            _ctrlFirst.Network.IsEnabledS0 = true;

            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_UNAUTHENTICATED));

            //2. Restore Key

            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS0 = true;

            //Action
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_ACCESS));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_UNAUTHENTICATED));
        }

        [Test]
        public void DisableAllS2AfterInclusion_OnPrimary_RequestFromSecondary_RestoresHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            _ctrlFirst.Network.IsEnabledS0 = true;

            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlFirst.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlSecond.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            //check nonce get? 
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_UNAUTHENTICATED));

            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS0 = true;

            //Action
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_ACCESS));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_UNAUTHENTICATED));
        }

        [Test]
        public void DisableAllS2AfterInclusion_OnSecondary_RequestFromPrimary_RestoresHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            _ctrlSecond.Network.IsEnabledS0 = true;
            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command, "Tries get with highest");
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));


            //2. Restore Key
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS0 = true;

            //Action
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_ACCESS));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_UNAUTHENTICATED));
        }

        [Test]
        public void DisableAllS2AfterInclusion_OnSecondary_RequestFromSecondary_RestoresHighestAvailable()
        {
            //Arrange
            IncludeSecondary();
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            _ctrlSecond.Network.IsEnabledS0 = true;
            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlFirst.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlSecond.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));


            //2. Restore Key
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS0 = true;

            //Action
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_ACCESS));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsTrue(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_UNAUTHENTICATED));
        }

        [TestCase(SecuritySchemes.S2_ACCESS)]
        [TestCase(SecuritySchemes.S2_AUTHENTICATED)]
        [TestCase(SecuritySchemes.S2_UNAUTHENTICATED)]
        [TestCase(SecuritySchemes.S2_TEMP)]
        [TestCase(SecuritySchemes.S0)]
        [TestCase(SecuritySchemes.NONE)]
        public void SetActiveScheme_Restores_HighestAvailable(SecuritySchemes testScheme)
        {
            //Arrange
            IncludeSecondary();
            _ctrlFirst.Network.SetCurrentSecurityScheme(NODE_ID_2, testScheme);

            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void KEXConfirm_GrantOnlyS0_AffterIncluion_UseOnlyS0()
        {
            // Arrange
            var sm = (SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager));
            sm.SecurityManagerInfo.KEXSetConfirmCallback = ((requestedSchemes, isClientSideAuthRequested) =>
                {
                    var ret = new KEXSetConfirmResult();
                    ret.GrantedSchemes = new System.Collections.Generic.List<SecuritySchemes>();
                    ret.GrantedSchemes.Add(SecuritySchemes.S0);
                    ret.IsConfirmed = true;
                    return ret;
                });
            IncludeSecondary();

            // Action1
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);
            _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            // Assert
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S0));

            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_SET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_SET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            Assert.AreEqual(SecuritySchemes.S0, expectRes.SecurityScheme);
        }

        [Test]
        public void KEXConfirm_GrantOnlyS2All_AffterIncluion_UseS2Access()
        {
            // Arrange
            var sm = (SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager));
            sm.SecurityManagerInfo.KEXSetConfirmCallback = ((requestedSchemes, isClientSideAuthRequested) =>
            {
                var ret = new KEXSetConfirmResult();
                ret.GrantedSchemes = new System.Collections.Generic.List<SecuritySchemes>();
                ret.GrantedSchemes.AddRange(SecuritySchemeSet.ALLS2);
                ret.IsConfirmed = true;
                return ret;
            });
            IncludeSecondary();
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S0));

            // Action1
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);
            _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S0));

            Assert.Null(actualGetS0Res.Command);
            Assert.Null(actualReportS0Res.Command);
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_ACCESS, actualReportS2Res.SecurityScheme);

            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_SET(), SUPPORTED_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_SET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            Assert.AreEqual(SecuritySchemes.S2_ACCESS, expectRes.SecurityScheme);
        }

        [TestCase(SecuritySchemes.S2_ACCESS)]
        [TestCase(SecuritySchemes.S2_AUTHENTICATED)]
        [TestCase(SecuritySchemes.S2_UNAUTHENTICATED)]
        public void KEXConfirm_GrantOnlyOneS2_AffterIncluion_UseOnlyTestScheme(SecuritySchemes testScheme)
        {
            // Arrange
            var sm = (SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager));
            sm.SecurityManagerInfo.KEXSetConfirmCallback = ((requestedSchemes, isClientSideAuthRequested) =>
            {
                var ret = new KEXSetConfirmResult();
                ret.GrantedSchemes = new List<SecuritySchemes>();
                ret.GrantedSchemes.Add(testScheme);
                ret.IsConfirmed = true;
                return ret;
            });
            IncludeSecondary();

            // Action1
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);
            _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, testScheme));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S0));

            Assert.Null(actualGetS0Res.Command);
            Assert.Null(actualReportS0Res.Command);
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(testScheme, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(testScheme, actualReportS2Res.SecurityScheme);

            _ctrlSecond.RequestNodeInfo(NODE_ID_1);
            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_SET(), EXPECT_TIMEOUT, null);
            var sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_SET(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            Assert.AreEqual(testScheme, expectRes.SecurityScheme);

            _ctrlFirst.RequestNodeInfo(NODE_ID_2);
            expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_SET(), EXPECT_TIMEOUT, null);
            sendRes = _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), new COMMAND_CLASS_BASIC.BASIC_SET(), TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            Assert.AreEqual(testScheme, expectRes.SecurityScheme);
        }

        [Test]
        public void RequestSupportedCommandClassFromSecondary_AllKeys_ReturnsOnlyForHighest()
        {
            // Arrange
            IncludeSecondary();
            var secureCCs = _ctrlSecond.Network.GetSecureCommandClasses();

            // Act
            var collector = ListenDataCollector.Create(_ctrlFirst, _ctrlSecond);
            collector.Start(true);
            _ctrlFirst.RequestNodeInfo(NODE_ID_2);
            Thread.Sleep(500);
            var results = collector.Stop();

            // Assert
            // Access Key
            var frames = results.Where(i => i.First == SecuritySchemes.S2_ACCESS).Select(i => i.Second).ToArray();
            Assert.AreEqual(2, frames.Length);
            var request = frames[0];
            Assert.AreEqual(2, request.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.ID, request[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET.ID, request[1]);
            var response = frames[1];
            Assert.AreNotEqual(2, response.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.ID, response[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID, response[1]);
            Assert.IsTrue(secureCCs.SequenceEqual(response.Skip(2)));

            // Authenticated Key
            frames = results.Where(i => i.First == SecuritySchemes.S2_AUTHENTICATED).Select(i => i.Second).ToArray();
            Assert.AreEqual(2, frames.Length);
            request = frames[0];
            Assert.AreEqual(2, request.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.ID, request[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET.ID, request[1]);
            response = frames[1];
            Assert.AreEqual(2, response.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.ID, response[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID, response[1]);

            // Anauthenticated Key
            frames = results.Where(i => i.First == SecuritySchemes.S2_UNAUTHENTICATED).Select(i => i.Second).ToArray();
            Assert.AreEqual(2, frames.Length);
            request = frames[0];
            Assert.AreEqual(2, request.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.ID, request[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET.ID, request[1]);
            response = frames[1];
            Assert.AreEqual(2, response.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.ID, response[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID, response[1]);

            // S0 Key
            frames = results.Where(i => i.First == SecuritySchemes.S0).Select(i => i.Second).ToArray();
            Assert.AreEqual(2, frames.Length);
            request = frames[0];
            Assert.AreEqual(2, request.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY.ID, request[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET.ID, request[1]);
            response = frames[1];
            Assert.AreEqual(3, response.Length);
            Assert.AreEqual(COMMAND_CLASS_SECURITY.ID, response[0]);
            Assert.AreEqual(COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT.ID, response[1]);
            Assert.AreEqual(0, response[2]);
        }

        [Test]
        public void DisableHighest_BeforeInclusion_OnSecondary_RequestFromPrimary_PrimaryTriesS2_AccessWithoutResponce()
        {
            //Arrange
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            IncludeSecondary();

            //Action
            byte[] s2MEC = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION();
            var s2mecToken = _ctrlSecond.ExpectData(s2MEC, SUPPORTED_TIMEOUT, null);
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualS2mecRes = (ExpectDataResult)s2mecToken.WaitCompletedSignal();
            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualS2mecRes.Command, "not decrypted frame - S2_Access Supported Get");
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2.Enable Key Again
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;

            //Act.
            s2mecToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION(), SUPPORTED_TIMEOUT, null);
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualS2mecRes = (ExpectDataResult)s2mecToken.WaitCompletedSignal();
            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualS2mecRes.Command, "not decrypted frame - S2_Access Supported Get");
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableHighest_BeforeInclusion_OnSecondary_RequestFromSecondary_NotUsesHighestAvailable()
        {
            //Arrange
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            IncludeSecondary();

            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2.Enable Key Again
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;

            //Act.
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableHighest_BeforeInclusion_OnPrimary_RequestFromPrimary_NotUsesHighestAvailable()
        {
            //Arrange
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            IncludeSecondary();

            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;

            //Action
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableHighest_BeforeInclusion_OnPrimary_RequestFromSecondary_NotUsesHighestAvailable()
        {
            //Arrange
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            IncludeSecondary();

            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);


            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;

            //Action
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.NotNull(actualGetS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualGetS2Res.SecurityScheme);
            Assert.NotNull(actualReportS2Res.Command);
            Assert.AreEqual(SecuritySchemes.S2_AUTHENTICATED, actualReportS2Res.SecurityScheme);
        }

        [Test]
        public void DisableAllS2_BeforeInclusion_OnPrimary_RequestFromPrimary_UseOnlyS0()
        {
            //Arrange
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_BASIC.ID });

            IncludeSecondary();

            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_UNAUTHENTICATED));

            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS0 = true;

            //Action
            _smiFirst.CheckIfSupportSecurityCC = false;
            _smiSecond.CheckIfSupportSecurityCC = false;

            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemes.S2_UNAUTHENTICATED));
        }

        [Test]
        public void DisableAllS2_BeforeInclusion_OnPrimary_RequestFromSecondary_UseOnlyS0()
        {
            //Arrange
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = false;
            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_BASIC.ID });

            IncludeSecondary();

            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlFirst.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlSecond.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_UNAUTHENTICATED));

            //2. Restore Key
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS0 = true;

            //Action
            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S2_UNAUTHENTICATED));
        }

        [Test]
        public void DisableAllS2_BeforeInclusion_OnSecondary_RequestFromPrimary_UseOnlyS0()
        {
            //Arrange
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_BASIC.ID });

            IncludeSecondary();
            //Action
            var getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlSecond.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlFirst.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));


            //2. Restore Key
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS0 = true;

            //Action
            getS2Token = _ctrlSecond.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlFirst.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlFirst.RequestNodeInfo(NODE_ID_2);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlFirst.Network.HasSecurityScheme(NODE_ID_2, SecuritySchemeSet.ALLS2));
        }

        [Test]
        public void DisableAllS2_BeforeInclusion_OnSecondary_RequestFromSecondary_UseOnlyS0()
        {
            //Arrange
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = false;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = false;
            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_BASIC.ID });

            IncludeSecondary();
            //Action
            var getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            var getS0Token = _ctrlFirst.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            var reportS0Token = _ctrlSecond.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            var r1 = _ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S0);
            var res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);
            var r2 = _ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemes.S0);

            var actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            var actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            var actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            var actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));


            //2. Restore Key
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS0 = true;

            //Action
            _smiFirst.CheckIfSupportSecurityCC = false;
            _smiSecond.CheckIfSupportSecurityCC = false;

            getS2Token = _ctrlFirst.ExpectData(cmdS2SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS2Token = _ctrlSecond.ExpectData(cmdS2SupportedReport, SUPPORTED_TIMEOUT, null);
            getS0Token = _ctrlFirst.ExpectData(cmdS0SupportedGet, SUPPORTED_TIMEOUT, null);
            reportS0Token = _ctrlSecond.ExpectData(cmdS0SupportedReport, SUPPORTED_TIMEOUT, null);

            res = _ctrlSecond.RequestNodeInfo(NODE_ID_1);

            actualGetS2Res = (ExpectDataResult)getS2Token.WaitCompletedSignal();
            actualReportS2Res = (ExpectDataResult)reportS2Token.WaitCompletedSignal();
            actualGetS0Res = (ExpectDataResult)getS0Token.WaitCompletedSignal();
            actualReportS0Res = (ExpectDataResult)reportS0Token.WaitCompletedSignal();

            //Assert
            Assert.Null(actualGetS2Res.Command);
            Assert.Null(actualReportS2Res.Command);
            Assert.NotNull(actualGetS0Res.Command);
            Assert.NotNull(actualReportS0Res.Command);
            Assert.AreEqual(SecuritySchemes.S0, actualGetS0Res.SecurityScheme);
            Assert.AreEqual(SecuritySchemes.S0, actualReportS0Res.SecurityScheme);
            Assert.IsFalse(_ctrlSecond.Network.HasSecurityScheme(NODE_ID_1, SecuritySchemeSet.ALLS2));
        }

        [Test]
        public void NifResponseDisabled_DuringInclusion_SkipSetupLifeLine()
        {
            //Arrange
            _transport.SetNifResponseEnabled(_ctrlSecond.SessionId, false);
            var expected = ActionStates.Completed;

            //Action
            var res = IncludeSecondary();

            //Assert
            Assert.Null(res.NodeInfo.RequestNodeInfo.NodeInfo);
            Assert.AreNotEqual(expected, res.NodeInfo.RequestNodeInfo.State);
            Assert.IsTrue(res.SetupLifelineResult.IsSetupLifelineCancelled);
        }

        [Test]
        public void NifResponseEnabled_DuringInclusion_SetupLifeLineCompleted()
        {
            //Arrange
            var expected = ActionStates.Completed;

            //Action
            var res = IncludeSecondary();

            //Assert
            Assert.NotNull(res.NodeInfo.RequestNodeInfo.NodeInfo);
            Assert.AreEqual(expected, res.NodeInfo.RequestNodeInfo.State);
            Assert.IsFalse(res.SetupLifelineResult.IsSetupLifelineCancelled);
        }
    }

    public class ListenDataCollector
    {
        private static ListenDataCollector _instance;
        private List<Pair<SecuritySchemes, byte[]>> _collectedItems;
        private Controller _primary;
        private Controller _secondary;
        private ActionToken _primaryCollectToken = null;
        private ActionToken _secondaryCollectToken = null;

        private ListenDataCollector() { }

        public byte[] filterOnSecondary1;
        public byte[] filterOnPrimary1;
        public byte[] filterOnSecondary2;
        public byte[] filterOnPrimary2;

        public static ListenDataCollector Create(Controller primary, Controller secondary)
        {
            if (_instance == null)
            {
                _instance = new ListenDataCollector();
            }
            _instance._collectedItems = new List<Pair<SecuritySchemes, byte[]>>();
            _instance._primary = primary;
            _instance._secondary = secondary;
            _instance.filterOnPrimary1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT();
            _instance.filterOnPrimary2 = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT();
            _instance.filterOnSecondary1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET();
            _instance.filterOnSecondary2 = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET();
            return _instance;
        }

        public void Start(bool isEnabledFilter)
        {
            _instance._primaryCollectToken = _primary.ListenData((x) =>
            {
                if (x.Command != null && x.Command.Length > 1)
                {
                    if (isEnabledFilter)
                    {
                        if (x.Command.Take(2).ToArray().SequenceEqual(filterOnPrimary1.Take(2).ToArray()) ||
                            x.Command.Take(2).ToArray().SequenceEqual(filterOnPrimary2.Take(2).ToArray()))
                        {

                            _instance._collectedItems.Add(new Pair<SecuritySchemes, byte[]>((SecuritySchemes)x.SecurityScheme, x.Command));
                        }
                    }
                    else
                    {
                        _instance._collectedItems.Add(new Pair<SecuritySchemes, byte[]>((SecuritySchemes)x.SecurityScheme, x.Command));
                    }
                }
            });
            _instance._secondaryCollectToken = _secondary.ListenData((x) =>
            {
                if (x.Command != null && x.Command.Length > 1)
                {
                    if (isEnabledFilter)
                    {
                        if (x.Command.Take(2).ToArray().SequenceEqual(filterOnSecondary1.Take(2).ToArray()) ||
                            x.Command.Take(2).ToArray().SequenceEqual(filterOnSecondary2.Take(2).ToArray()))
                        {
                            _instance._collectedItems.Add(new Pair<SecuritySchemes, byte[]>((SecuritySchemes)x.SecurityScheme, x.Command));
                        }
                    }
                    else
                    {
                        _instance._collectedItems.Add(new Pair<SecuritySchemes, byte[]>((SecuritySchemes)x.SecurityScheme, x.Command));
                    }
                }
            });
        }

        public List<Pair<SecuritySchemes, byte[]>> Stop()
        {
            _instance._primary.Cancel(_instance._primaryCollectToken);
            _instance._primaryCollectToken.WaitCompletedSignal();
            _instance._secondary.Cancel(_instance._secondaryCollectToken);
            _instance._secondaryCollectToken.WaitCompletedSignal();
            return new List<Pair<SecuritySchemes, byte[]>>(_instance._collectedItems);
        }
    }
}