/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils;
using ZWave.Devices;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    public class WakeupSupport : DelayedResponseOperation
    {
        private byte[] _wakeupDelay = new byte[2];
        private SetRFReceiveModeOperation _disableRFReceiveOperation;

        public TransmitOptions TxOptions { get; set; }

        public WakeupSupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_WAKE_UP_V2.ID))
        {
            _wakeupDelay = new byte[] { 0x00, 0x01, 0x2C }; //300 sec
            TxOptions = txOptions;
        }

        protected override void CreateInstance()
        {
            _disableRFReceiveOperation = new SetRFReceiveModeOperation(0x00);
            base.CreateInstance();
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
                if (isSuportedScheme && command != null && command.Length > 1)
                {
                    if (command[1] == COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_GET.ID)
                    {
                        var cmd = (COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_GET)command;
                        var rpt = new COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_REPORT()
                        {
                            minimumWakeUpIntervalSeconds = new byte[] { 0x00, 0x00, 0x3C },
                            maximumWakeUpIntervalSeconds = new byte[] { 0x01, 0x51, 0x80 },
                            defaultWakeUpIntervalSeconds = new byte[] { 0x00, 0x01, 0x2C },
                            wakeUpIntervalStepSeconds = new byte[] { 0x00, 0x00, 0x3C }
                        };
                        var sendData = new SendDataExOperation(_network, node, rpt, TxOptions, scheme);
                        ou.SetNextActionItems(sendData);
                    }
                    else if (command[1] == COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_GET.ID)
                    {
                        var cmd = (COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_GET)command;
                        var rpt = new COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_REPORT()
                        {
                            seconds = _wakeupDelay
                        };
                        var sendData = new SendDataExOperation(_network, node, rpt, TxOptions, scheme);
                        ou.SetNextActionItems(sendData);
                    }
                    else if (command[1] == COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_SET.ID)
                    {
                        var cmd = (COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_SET)command;
                        _wakeupDelay = cmd.seconds;
                    }
                    else if (command[1] == COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_NO_MORE_INFORMATION.ID)
                    {
                        _disableRFReceiveOperation.NewToken();
                        ou.SetNextActionItems(_disableRFReceiveOperation);
                    }
                }
            }
        }

    }
}
