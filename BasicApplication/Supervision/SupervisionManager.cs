/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Layers.Frame;
using ZWave.BasicApplication.Operations;
using ZWave.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public class SupervisionManager : SubstituteManagerBase
    {
        protected override SubstituteIncomingFlags GetId()
        {
            return SubstituteIncomingFlags.Supervision;
        }

        public TransmitOptions TxOptions = TransmitOptions.TransmitOptionAcknowledge | TransmitOptions.TransmitOptionAutoRoute | TransmitOptions.TransmitOptionExplore;
        private static byte _sessionId = 0x01;

        private Func<NodeTag, byte[], bool> mSendDataSubstitutionCallback;
        public Func<NodeTag, byte[], bool> SendDataSubstitutionCallback
        {
            get
            {
                return mSendDataSubstitutionCallback;
            }
            internal set
            {
                mSendDataSubstitutionCallback = value;
            }
        }

        public Action SetWakeupDelayed { get; set; }
        public Action UnSetWakeupDelayed { get; set; }
        public Action ResponseDelay { get; set; }

        public SupervisionManager(NetworkViewPoint network, Func<NodeTag, byte[], bool> sendDataSubstitutionCallback)
                : base(network)
        {
            SendDataSubstitutionCallback = sendDataSubstitutionCallback;
        }

        public override bool OnIncomingSubstituted(CustomDataFrame dataFrameOri, CustomDataFrame dataFrameSub, List<ActionHandlerResult> ahResults, out ActionBase additionalAction)
        {
            additionalAction = null;
            bool ret = false;
            if (IsActive)
            {
                if (TryParseCommand(dataFrameOri, out NodeTag destNode, out NodeTag srcNode, out int lenIndex, out byte[] cmdData))
                {
                    if (cmdData[0] == COMMAND_CLASS_SUPERVISION.ID && cmdData[1] == COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.ID)
                    {
                        ResponseDelay?.Invoke();
                        UnSetWakeupDelayed?.Invoke();
                    }
                }
            }
            return ret;
        }

        protected override CustomDataFrame SubstituteIncomingInternal(CustomDataFrame packet, NodeTag destNode, NodeTag srcNode, byte[] cmdData, int lenIndex, out ActionBase additionalAction, out ActionBase completeAction)
        {
            CustomDataFrame ret = packet;
            additionalAction = null;
            completeAction = null;
            if (IsActive)
            {
                if (cmdData.Length > 4 && cmdData[0] == COMMAND_CLASS_SUPERVISION.ID && cmdData[1] == COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.ID)
                {
                    SetWakeupDelayed?.Invoke();
                    COMMAND_CLASS_SUPERVISION.SUPERVISION_GET cmd = cmdData;
                    if (cmd.encapsulatedCommand != null && cmd.encapsulatedCommand.Count > 0)
                    {
                        byte[] newFrameData = new byte[packet.Data.Length - 4];
                        Array.Copy(packet.Data, 0, newFrameData, 0, lenIndex);
                        newFrameData[lenIndex] = (byte)(cmdData.Length - 4);
                        Array.Copy(cmdData, 4, newFrameData, lenIndex + 1, cmdData.Length - 4);
                        Array.Copy(packet.Data, lenIndex + 1 + cmdData.Length, newFrameData, lenIndex + 1 + cmdData.Length - 4,
                            packet.Data.Length - lenIndex - 1 - cmdData.Length);
                        ret = CreateNewFrame(packet, newFrameData);
                    }
                }
            }
            return ret;
        }

        public override ActionBase SubstituteActionInternal(ApiOperation action)
        {
            ActionBase ret = null;
            if (IsActive)
            {
                var sendDataAction = action as SendDataOperation;
                if (sendDataAction != null && sendDataAction.IsFollowup)
                {
                    if (IsSupervisionSupported(sendDataAction.DstNode, sendDataAction.Data))
                    {
                        if (sendDataAction.Data.Length >= 2 && (sendDataAction.Data[0] != COMMAND_CLASS_SUPERVISION.ID || sendDataAction.Data[1] != COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.ID))
                        {
                            if (sendDataAction.Data[0] == COMMAND_CLASS_MULTI_CHANNEL_V4.ID && sendDataAction.Data[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP.ID)
                            {
                                COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP multiChannelCommand = sendDataAction.Data;
                                List<byte> innerdata = new List<byte>();
                                innerdata.Add(multiChannelCommand.commandClass);
                                innerdata.Add(multiChannelCommand.command);
                                if (multiChannelCommand.parameter != null)
                                {
                                    innerdata.AddRange(multiChannelCommand.parameter);
                                }
                                var supervisionGetCmd = new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET()
                                {
                                    properties1 = new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.Tproperties1()
                                    {
                                        sessionId = _sessionId++
                                    },
                                    encapsulatedCommandLength = (byte)innerdata.Count,
                                    encapsulatedCommand = innerdata.ToArray()
                                };
                                byte[] supervisionGetCmdData = supervisionGetCmd;
                                multiChannelCommand.commandClass = supervisionGetCmdData[0];
                                multiChannelCommand.command = supervisionGetCmdData[1];
                                if (supervisionGetCmdData.Length > 2)
                                {
                                    var tmp = new byte[supervisionGetCmdData.Length - 2];
                                    Array.Copy(supervisionGetCmdData, 2, tmp, 0, tmp.Length);
                                    multiChannelCommand.parameter = tmp;
                                }
                                sendDataAction.Data = multiChannelCommand;
                                ret = sendDataAction;
                            }
                            else
                            {
                                var substitutedData = new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET()
                                {
                                    properties1 = new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.Tproperties1()
                                    {
                                        sessionId = _sessionId++
                                    },
                                    encapsulatedCommandLength = (byte)sendDataAction.Data.Length,
                                    encapsulatedCommand = sendDataAction.Data
                                };
                                sendDataAction.Data = substitutedData;
                                ret = sendDataAction;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private bool IsSupervisionSupported(NodeTag node, byte[] data)
        {
            bool ret = false;
            if (SendDataSubstitutionCallback != null)
            {
                ret = SendDataSubstitutionCallback(node, data);
            }
            return ret;
        }
    }
}
