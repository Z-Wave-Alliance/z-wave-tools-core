/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.CommandClasses;
using ZWave.BasicApplication.Operations;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.CommandClasses
{
    public class PowerLevelSupport : DelayedResponseOperation
    {
        public byte Value { get; set; }
        public PowerLevelSupport(NetworkViewPoint network)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_POWERLEVEL.ID))
        {
        }

        SendTestFrameOperation _sendTest;
        RFPowerLevelGetOperation _powerLevelGet;
        RFPowerLevelSetOperation _powerLevelSet;
        SendDataExOperation _sendPowerLevelTestNodeReport;
        SendDataExOperation _sendPowerLevelReport;
        int _testIteration = 0;
        int _failIterations = 0;
        int _testFrameCount = 0;
        NodeTag _testFromNode = NodeTag.Empty;

        protected override void CreateWorkflow()
        {
            base.CreateWorkflow();
            ActionUnits.Add(new ActionCompletedUnit(_powerLevelGet, OnPowerLevelGet, _sendPowerLevelReport));
            ActionUnits.Add(new ActionCompletedUnit(_sendTest, OnTestCompleted));
        }

        protected override void CreateInstance()
        {
            _powerLevelGet = new RFPowerLevelGetOperation();
            _powerLevelSet = new RFPowerLevelSetOperation(0);
            _sendTest = new SendTestFrameOperation(_network, new NodeTag(0), 0x06);
            _sendPowerLevelReport = new SendDataExOperation(_network, NodeTag.Empty, null, TransmitOptions.TransmitOptionAcknowledge, SecuritySchemes.NONE);
            _sendPowerLevelReport.SubstituteSettings.SetFlag(SubstituteFlags.DenySupervision);
            _sendPowerLevelTestNodeReport = new SendDataExOperation(_network, NodeTag.Empty, null, TransmitOptions.TransmitOptionAcknowledge, SecuritySchemes.NONE);
            _sendPowerLevelTestNodeReport.SubstituteSettings.SetFlag(SubstituteFlags.DenySupervision);

            base.CreateInstance();
        }

        private void OnPowerLevelGet(ActionCompletedUnit ou)
        {
            _sendPowerLevelReport.Data = new COMMAND_CLASS_POWERLEVEL.POWERLEVEL_REPORT()
            {
                powerLevel = _powerLevelGet.SpecificResult.PowerLevel
            };
        }

        private void OnTestCompleted(ActionCompletedUnit ou)
        {
            _testIteration++;
            if (_sendTest.SpecificResult.TransmitStatus != TransmitStatuses.CompleteOk)
            {
                _failIterations++;
            }
            if (_testIteration >= _testFrameCount)
            {
                _sendPowerLevelTestNodeReport.NewToken();
                _sendPowerLevelTestNodeReport.DstNode = _testFromNode;
                _sendPowerLevelTestNodeReport.Data = new COMMAND_CLASS_POWERLEVEL.POWERLEVEL_TEST_NODE_REPORT()
                {
                    statusOfOperation = _failIterations == 0 ? (byte)0x01 : (byte)0x00,
                    testFrameCount = new byte[] { (byte)(_testIteration << 8), (byte)_testIteration },
                    testNodeid = (byte)_sendTest.Node.Id
                };
                ou.SetNextActionItems(_sendPowerLevelTestNodeReport);
            }
            else
            {
                _sendTest.NewToken();
                ou.SetNextActionItems(_sendTest);
            }
        }

        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();
            if (DstNode.EndPointId == ReceivedAchData.DstNode.EndPointId)
            {
                var node = ReceivedAchData.SrcNode;
                byte[] command = ReceivedAchData.Command;
                var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
                bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);
                if (command != null && command.Length > 1 && isSuportedScheme)
                {
                    if (command[1] == COMMAND_CLASS_POWERLEVEL.POWERLEVEL_GET.ID)
                    {
                        _powerLevelGet.NewToken();
                        _sendPowerLevelReport.NewToken();
                        _sendPowerLevelReport.DstNode = node;
                        _sendPowerLevelReport.SecurityScheme = scheme;
                        ou.SetNextActionItems(_powerLevelGet);
                    }
                    if (command[1] == COMMAND_CLASS_POWERLEVEL.POWERLEVEL_SET.ID)
                    {
                        COMMAND_CLASS_POWERLEVEL.POWERLEVEL_SET cmd = command;
                        _powerLevelSet.NewToken();
                        _powerLevelSet.PowerLevel = cmd.powerLevel;
                        ou.SetNextActionItems(_powerLevelSet);
                    }
                    else if (command[1] == COMMAND_CLASS_POWERLEVEL.POWERLEVEL_TEST_NODE_SET.ID)
                    {
                        COMMAND_CLASS_POWERLEVEL.POWERLEVEL_TEST_NODE_SET cmd = command;
                        _sendTest.NewToken();
                        _sendTest.Node = new NodeTag(cmd.testNodeid);
                        _sendTest.PowerLevel = cmd.powerLevel;

                        _testFromNode = node;
                        _testFrameCount = Tools.GetInt32(cmd.testFrameCount);
                        _testIteration = 0;
                        _failIterations = 0;
                        ou.SetNextActionItems(_sendTest);
                    }
                    else if (command[1] == COMMAND_CLASS_POWERLEVEL.POWERLEVEL_TEST_NODE_GET.ID)
                    {
                        COMMAND_CLASS_POWERLEVEL.POWERLEVEL_TEST_NODE_GET cmd = command;
                        _sendPowerLevelTestNodeReport.NewToken();
                        _sendPowerLevelTestNodeReport.DstNode = node;
                        _sendPowerLevelTestNodeReport.SecurityScheme = scheme;
                        _sendPowerLevelTestNodeReport.Data = new COMMAND_CLASS_POWERLEVEL.POWERLEVEL_TEST_NODE_REPORT()
                        {
                            statusOfOperation = _failIterations == 0 ? (byte)0x01 : (byte)0x00,
                            testFrameCount = new byte[] { (byte)(_testIteration << 8), (byte)_testIteration },
                            testNodeid = (byte)_sendTest.Node.Id
                        };
                        ou.SetNextActionItems(_sendPowerLevelTestNodeReport);
                    }
                }
            }
        }
    }
}
