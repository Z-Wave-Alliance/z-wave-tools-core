using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using ZWave.CommandClasses;
using ZWave.BasicApplication.Operations;
using ZWave;
using ZWave.BasicApplication;
using ZWave.Devices;

namespace BasicApplicationTests.Supervision
{
    [TestFixture]
    public class SendDataSupervisionTests : TestBase
    {
        #region Hardcodes

        private const int SUPERVISION_REPORT_TIMEOUT = 222;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _transport.SetUpModulesNetwork(_ctrlFirst.SessionId, _ctrlSecond.SessionId);
            _ctrlFirst.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true), new SupervisionReportTask(_ctrlFirst.Network, TXO));
            _ctrlSecond.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true), new SupervisionReportTask(_ctrlSecond.Network, TXO));
            _ctrlFirst.SessionClient.AddSubstituteManager(new SecurityManager(_ctrlFirst.Network, null, null));
            _ctrlSecond.SessionClient.AddSubstituteManager(new SecurityManager(_ctrlSecond.Network, null, null));
        }

        [Test]
        public void A_SendData_WrappedWithSupervisionGetCommand_SupervisionReportIsReceived()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            byte[] supervisionWrappedCmd = CreateSupervisionCmd(primaryCmd);

            // Act.
            var expectSupervisionReportToken = _ctrlFirst.ExpectData(new COMMAND_CLASS_SUPERVISION.SUPERVISION_REPORT(), SUPERVISION_REPORT_TIMEOUT, null);
            var expectPrimaryCmdToken = _ctrlSecond.ExpectData(primaryCmd, SUPERVISION_REPORT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, supervisionWrappedCmd, TXO);
            var expectPrimaryCmdRes = (ExpectDataResult)expectPrimaryCmdToken.WaitCompletedSignal();
            var expectSupervisionReportCmdRes = (ExpectDataResult)expectSupervisionReportToken.WaitCompletedSignal();

            // Assert.
            Assert.AreEqual(expectPrimaryCmdRes.State, ActionStates.Completed);
            Assert.IsTrue(expectPrimaryCmdRes.Command.SequenceEqual(primaryCmd));
            Assert.AreEqual(expectSupervisionReportCmdRes.State, ActionStates.Completed);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SUPERVISION.SUPERVISION_REPORT.ID)
            });
        }

        [Test]
        public void SendData_FrameIsFollowup_WrappedWithSupervisionGetCommand()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectSupervisionReportToken = _ctrlFirst.ExpectData(new COMMAND_CLASS_SUPERVISION.SUPERVISION_REPORT(), SUPERVISION_REPORT_TIMEOUT, null);
            var expectPrimaryCmdToken = _ctrlSecond.ExpectData(primaryCmd, SUPERVISION_REPORT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO, null, true /*Frame is Followup*/);
            var expectPrimaryCmdRes = (ExpectDataResult)expectPrimaryCmdToken.WaitCompletedSignal();
            var expectSupervisionReportCmdRes = (ExpectDataResult)expectSupervisionReportToken.WaitCompletedSignal();

            // Assert.
            Assert.AreEqual(expectPrimaryCmdRes.State, ActionStates.Completed);
            Assert.IsTrue(expectPrimaryCmdRes.Command.SequenceEqual(primaryCmd));
            Assert.AreEqual(expectSupervisionReportCmdRes.State, ActionStates.Completed);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SUPERVISION.SUPERVISION_REPORT.ID)
            });
        }

        private byte[] CreateSupervisionCmd(byte[] data)
        {
            var ret = new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET();
            ret.encapsulatedCommandLength = (byte)data.Length;
            ret.encapsulatedCommand = new List<byte>(data);
            return ret;
        }
    }
}
