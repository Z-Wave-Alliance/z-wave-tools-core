/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.BasicApplication.Security;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Tasks;
using ZWave.BasicApplication.CommandClasses;
using System.Threading;
using ZWave.Security;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class InclusionControllerSecureSupport : DelayedResponseOperation
    {
        //Inclusion Controller Initiate Step ID
        const byte PROXY_INCLUSION = 0x01;
        const byte S0_INCLUSION = 0x02;
        const byte PROXY_INCLUSION_REPLACE = 0x03;
        //Inclusion Controller Complete Status
        const byte STEP_OK = 0x01;
        const byte STEP_USER_REJECTED = 0x02;
        const byte STEP_FAILED = 0x03;
        const byte STEP_NOT_SUPPORTED = 0x04;

        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }
        private SecurityManagerInfo _securityManagerInfo;
        private readonly Action<ActionResult> _updateCallback;
        private readonly Action<ActionToken, bool> _inclusionControllerStatusUpdateCallback;
        private InclusionController.Complete _sendDataComplete;
        private byte _stepId;
        public InclusionControllerSecureSupport(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, Action<ActionResult> updateCallback,
                Action<ActionToken, bool> inclusionControllerStatusUpdateCallback)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_INCLUSION_CONTROLLER.ID))
        {
            _updateCallback = updateCallback;
            _securityManagerInfo = securityManagerInfo;
            _inclusionControllerStatusUpdateCallback = inclusionControllerStatusUpdateCallback;
            TxOptions = _securityManagerInfo.TxOptions;
            TxOptions2 = TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE;
            TxSecOptions = TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY;
        }

        public InclusionControllerSecureSupport(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo)
            : this(network, securityManagerInfo, null, null)
        {
        }

        NodeTag nodeIdToInclude = NodeTag.Empty;
        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            var node = ReceivedAchData.SrcNode;
            byte[] command = ReceivedAchData.Command;
            if (command != null && command.Length > 1)
            {
                if (command[1] == COMMAND_CLASS_INCLUSION_CONTROLLER.INITIATE.ID && nodeIdToInclude == NodeTag.Empty)
                {
                    var initiateCommand = (COMMAND_CLASS_INCLUSION_CONTROLLER.INITIATE)command;

                    if (initiateCommand.stepId == PROXY_INCLUSION || initiateCommand.stepId == PROXY_INCLUSION_REPLACE)
                    {
                        //if we're not SIS then ignore
                        if (_securityManagerInfo.Network.SucNodeId == _securityManagerInfo.Network.NodeTag.Id)
                        {
                            //Start add node for example
                            nodeIdToInclude = new NodeTag(initiateCommand.nodeId);

                            var isVirtualNodeOperation = new IsVirtualNodeOperation(_network, nodeIdToInclude);
                            if (ReceivedAchData.CommandType != CommandTypes.CmdApplicationCommandHandler_Bridge)
                            {
                                isVirtualNodeOperation.SetCancelled();
                            }
                            _stepId = initiateCommand.stepId;
                            _sendDataComplete = new InclusionController.Complete(_network, NodeTag.Empty, node, TxOptions, 10000);
                            _sendDataComplete.SetCommandParameters(STEP_OK, _stepId);
                            var requestDataStep2 = new InclusionController.Initiate(_network, NodeTag.Empty, node, TxOptions, 20000);
                            requestDataStep2.SetCommandParameters(nodeIdToInclude, S0_INCLUSION);
                            var sendDataRejectComplete = new InclusionController.Complete(_network, NodeTag.Empty, node, TxOptions, 10000);
                            sendDataRejectComplete.SetCommandParameters(STEP_USER_REJECTED, _stepId);

                            var addNodeOperation = new AddNodeS2Operation(_network, _securityManagerInfo);
                            addNodeOperation.SetInclusionControllerInitiateParameters(nodeIdToInclude);

                            var setupNodeLifelineTask = new SetupNodeLifelineTask(_securityManagerInfo.Network);
                            setupNodeLifelineTask.Node = _securityManagerInfo.Network.NodeTag;
                            setupNodeLifelineTask.SucNode = _securityManagerInfo.Network.NodeTag;
                            setupNodeLifelineTask.TargetNode = nodeIdToInclude;
                            setupNodeLifelineTask.CompletedCallback = OnNodeInfoCompleted;
                            var serialGroup = new ActionSerialGroup(OnInitiateFlowActionCompleted, isVirtualNodeOperation,
                                new RequestNodeInfoOperation(_network, nodeIdToInclude)
                                {
                                    SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0)
                                },
                                addNodeOperation, requestDataStep2, setupNodeLifelineTask, _sendDataComplete);
                            serialGroup.StopActionUnit = new StopActionUnit(sendDataRejectComplete);
                            //hack
                            serialGroup.Token.Result = new AddRemoveNodeResult();
                            serialGroup.CompletedCallback = OnS2SerialGroupCompleted;
                            _inclusionControllerStatusUpdateCallback?.Invoke(serialGroup.Token, false);
                            ou.SetNextActionItems(serialGroup);
                        }
                    }
                    else if (initiateCommand.stepId == S0_INCLUSION)
                    {
                        //only if asked from SIS
                        if (_securityManagerInfo.Network.SucNodeId == node.Id)
                        {
                            InclusionController.Complete sendDataComplete = null;
                            //Start S0
                            nodeIdToInclude = new NodeTag(initiateCommand.nodeId);

                            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0))
                            {
                                sendDataComplete = new InclusionController.Complete(_network, NodeTag.Empty, node, TxOptions, 10000);
                                sendDataComplete.SetCommandParameters(STEP_OK, S0_INCLUSION);

                                var addNodeOperation = new AddNodeS0Operation(_network, _securityManagerInfo);
                                addNodeOperation.SetInclusionControllerInitiateParameters(nodeIdToInclude);

                                var nodeInfoOperation = new RequestNodeInfoOperation(_network, nodeIdToInclude);
                                nodeInfoOperation.CompletedCallback = OnNodeInfoCompleted;

                                var serialGroup = new ActionSerialGroup(new RequestNodeInfoOperation(_network, nodeIdToInclude)
                                {
                                    SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0)
                                },
                                    addNodeOperation, nodeInfoOperation, sendDataComplete);

                                //hack
                                serialGroup.Token.Result = new AddRemoveNodeResult();

                                ou.SetNextActionItems(serialGroup);
                            }
                            else
                            {
                                sendDataComplete = new InclusionController.Complete(_network, NodeTag.Empty, node, TxOptions, 10000);
                                sendDataComplete.SetCommandParameters(STEP_NOT_SUPPORTED, S0_INCLUSION);
                                ou.SetNextActionItems(sendDataComplete);
                            }
                        }
                    }
                }
                else if (command[1] == COMMAND_CLASS_INCLUSION_CONTROLLER.COMPLETE.ID)
                {

                }
            }
        }

        private void OnInitiateFlowActionCompleted(ActionBase actionGroup, ActionBase completedAction)
        {
            if (completedAction is RequestNodeInfoOperation)
            {
                if (completedAction.Result)
                {
                    var requestNodeInfoResult = (RequestNodeInfoResult)completedAction.Result;
                    bool isS2Supported = false;
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2) &&
                        requestNodeInfoResult.CommandClasses.Contains(COMMAND_CLASS_SECURITY_2.ID))
                    {
                        isS2Supported = true;
                        ((ActionSerialGroup)actionGroup).Actions[3].SetCancelled();
                    }
                    else
                    {
                        ((ActionSerialGroup)actionGroup).Actions[2].SetCancelled();
                    }

                    if (!isS2Supported && _securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0) &&
                        requestNodeInfoResult.CommandClasses.Contains(COMMAND_CLASS_SECURITY.ID))
                    {
                        ((ActionSerialGroup)actionGroup).Actions[2].SetCancelled();
                    }
                    else
                    {
                        ((ActionSerialGroup)actionGroup).Actions[3].SetCancelled();
                    }
                }
                else
                {
                    _sendDataComplete.SetCommandParameters(STEP_FAILED, _stepId);
                    ((ActionSerialGroup)actionGroup).Actions[2].SetCancelled();
                    ((ActionSerialGroup)actionGroup).Actions[3].SetCancelled();
                    ((ActionSerialGroup)actionGroup).Actions[4].SetCancelled();
                }
            }
            else if (completedAction.Result && completedAction is IsVirtualNodeOperation)
            {
                var isVirtualNodeResult = (IsVirtualNodeResult)completedAction.Result;
                if (ReceivedAchData.CommandType == CommandTypes.CmdApplicationCommandHandler_Bridge &&
                    isVirtualNodeResult && isVirtualNodeResult.RetValue)
                {
                    foreach (var item in ((ActionSerialGroup)actionGroup).Actions)
                    {
                        if (!(item is SendDataExOperation))
                        {
                            item.SetCancelled();
                        }
                    }
                }
            }

            else if ((completedAction is AddNodeS2Operation) && (completedAction as AddNodeS2Operation).SpecificResult.SubstituteStatus == SubstituteStatuses.Failed)
            {
                if ((completedAction as AddNodeS2Operation).SpecificResult.BootstrapingStatuses != BootstrapingStatuses.Completed)
                    _sendDataComplete.SetCommandParameters(STEP_USER_REJECTED, _sendDataComplete.StepId);
                else
                    _sendDataComplete.SetCommandParameters(STEP_FAILED, _sendDataComplete.StepId);
            }
        }

        private void OnNodeInfoCompleted(IActionItem actionItem)
        {
            nodeIdToInclude = NodeTag.Empty;
            if (_updateCallback != null)
            {
                var action = actionItem as ActionBase;
                if (action != null)
                {
                    _updateCallback(action.Result);
                }
            }
        }

        private void OnS2SerialGroupCompleted(IActionItem action)
        {
            nodeIdToInclude = NodeTag.Empty;
            _inclusionControllerStatusUpdateCallback?.Invoke(null, true);
        }
    }

    public class InclusionControllerSecure
    {
        public class Initiate : InclusionController.Initiate
        {
            private readonly SecurityManagerInfo _securityManagerInfo;
            private readonly ISecurityTestSettingsService _securityTestSettingsService;

            public Initiate(SecurityManagerInfo securityManagerInfo, NetworkViewPoint network, NodeTag srcNode, NodeTag destNode, TransmitOptions txOptions, int timeoutMs)
                : base(network, srcNode, destNode, txOptions, timeoutMs)
            {
                _securityManagerInfo = securityManagerInfo;
                _securityTestSettingsService = new SecurityTestSettingsService(_securityManagerInfo, true);
            }

            protected override void OnStart(StartActionUnit ou)
            {
                if (StepId == 0x01)
                {
                    _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.InclusionInititate1, this);
                }
                else if (StepId == 0x02)
                {
                    _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.InclusionInititate2, this);
                }
                base.OnStart(ou);
            }
        }
    }
}
