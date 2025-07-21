/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication
{
    public class SupervisionReportTask : DelayedResponseOperation
    {
        public Func<AchData, byte[]> ReceiveAchDataCallback { get; set; }
        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }

        public SupervisionReportTask(NetworkViewPoint network, TransmitOptions txOptions)
           : this(network, NodeTag.Empty, null, txOptions)
        {
        }

        public SupervisionReportTask(NetworkViewPoint network, NodeTag dstNode, TransmitOptions txOptions)
           : this(network, dstNode, null, txOptions)
        {
        }

        public SupervisionReportTask(NetworkViewPoint network, NodeTag dstNode, Func<AchData, byte[]> receiveAchDataCallback, TransmitOptions txOptions)
            : base(network, dstNode, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_SUPERVISION.ID))
        {
            ReceiveAchDataCallback = receiveAchDataCallback;
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
                    if (command[1] == COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.ID)
                    {
                        byte[] data = null;
                        if (ReceiveAchDataCallback != null)
                        {
                            data = ReceiveAchDataCallback(ReceivedAchData);
                        }
                        else
                        {
                            COMMAND_CLASS_SUPERVISION.SUPERVISION_GET cmd = command;
                            data = new COMMAND_CLASS_SUPERVISION.SUPERVISION_REPORT()
                            {
                                duration = (byte)_network.SupervisionDuration,
                                properties1 = new COMMAND_CLASS_SUPERVISION.SUPERVISION_REPORT.Tproperties1()
                                {
                                    sessionId = cmd.properties1.sessionId,
                                    moreStatusUpdates = (byte)_network.SupervisionMoreStatusUpdates
                                },
                                status = (byte)_network.SupervisionReportStatusResponse
                            };
                        }

                        if (data != null)
                        {
                            var sendData = new SendDataExOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                            ou.SetNextActionItems(sendData);
                        }
                        else
                        {
                            ou.SetNextActionItems();
                        }
                    }
                }
            }
        }
    }
}
