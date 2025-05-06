/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.CommandClasses
{
    public class SwitchBinarySupport : DelayedResponseOperation
    {
        //This is missing the duration handling!!!!
        public byte Duration { get; set; }
        public byte TargetValue { get; set; }
        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }

        public SwitchBinarySupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_SWITCH_BINARY_V2.ID))
        {
            TxOptions = txOptions;
            TxOptions2 = TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE;
            TxSecOptions = TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY;
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
                    if (command[1] == COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_GET.ID)
                    {
                        ApiOperation sendData = null;
                        var data = new COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_REPORT() { targetValue = TargetValue };
                        sendData = new SendDataExOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                        ou.SetNextActionItems(sendData);
                    }
                    else if (command[1] == COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_SET.ID)
                    {
                        var SetCommand = (COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_SET)command;
                        TargetValue = SetCommand.targetValue;
                    }
                }
            }
        }
    }
}
