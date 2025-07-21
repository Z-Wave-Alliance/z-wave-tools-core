/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    /// <summary>
    /// Time Parameters Command Class Support Task
    /// </summary>
    public class TimeParametersSupport : DelayedResponseOperation
    {
        public TransmitOptions TxOptions { get; set; }

        public TimeParametersSupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_TIME_PARAMETERS.ID))
        {
            TxOptions = txOptions;
        }

        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();

            var node = ReceivedAchData.SrcNode;
            byte[] command = ReceivedAchData.Command;
            var receiveStatus = ReceivedAchData.Options;
            //// SDS13782: CC:0072.01.00.41.004:
            //var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
            //bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);

            var isMustIgnore = receiveStatus.HasFlag(ReceiveStatuses.TypeBroad) || receiveStatus.HasFlag(ReceiveStatuses.TypeMulti) || ReceivedAchData.DstNode.EndPointId > 0;

            if (command != null && command.Length > 1 && !isMustIgnore)
            {
                if (command[1] == COMMAND_CLASS_TIME_PARAMETERS.TIME_PARAMETERS_GET.ID)
                {
                    var data = new COMMAND_CLASS_TIME_PARAMETERS.TIME_PARAMETERS_REPORT();
                    data.year = new byte[2];
                    data.year[0] = (byte)(DateTime.UtcNow.Year >> 8);
                    data.year[1] = (byte)DateTime.UtcNow.Year;
                    data.month = (byte)DateTime.UtcNow.Month;
                    data.day = (byte)DateTime.UtcNow.Day;
                    data.hourUtc = (byte)DateTime.UtcNow.Hour;
                    data.minuteUtc = (byte)DateTime.UtcNow.Minute;
                    data.secondUtc = (byte)DateTime.UtcNow.Second;

                    var sendData = new SendDataOperation(_network, node, data, TxOptions);
                    ou.SetNextActionItems(sendData);
                }
            }
        }
    }
}
