/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.CommandClasses
{
    public class ManufacturerSpecificSupport : DelayedResponseOperation
    {
        private ushort _manufacturerId;
        private ushort _productId;
        private ushort _productTypeId;

        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }

        public ManufacturerSpecificSupport(NetworkViewPoint network, TransmitOptions txOptions, ushort manufacturerId, ushort productId, ushort productTypeId)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_MANUFACTURER_SPECIFIC.ID))
        {
            _manufacturerId = manufacturerId;
            _productId = productId;
            _productTypeId = productTypeId;

            TxOptions = txOptions;
            TxOptions2 = TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE;
            TxSecOptions = TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY;
        }

        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();

            var node = ReceivedAchData.SrcNode;
            byte[] command = ReceivedAchData.Command;
            var receiveStatus = ReceivedAchData.Options;
            var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;

            // SDS13782: CC:0072.01.00.41.004:
            bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);
            //SDS13782: CC:0072.01.04.11.003 | CC:0072.02.06.11.003
            var isMustIgnore = receiveStatus.HasFlag(ReceiveStatuses.TypeBroad) || receiveStatus.HasFlag(ReceiveStatuses.TypeMulti) || ReceivedAchData.DstNode.EndPointId > 0;

            if (command != null && command.Length > 1 && isSuportedScheme && !isMustIgnore)
            {
                if (command[1] == COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.MANUFACTURER_SPECIFIC_GET.ID)
                {
                    var data = new COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.MANUFACTURER_SPECIFIC_REPORT();
                    data.manufacturerId = new byte[] { (byte)(_manufacturerId >> 8), (byte)_manufacturerId };
                    data.productId = new byte[] { (byte)(_productId >> 8), (byte)_productId };
                    data.productTypeId = new byte[] { (byte)(_productTypeId >> 8), (byte)_productTypeId };

                    var sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
                else if (command[1] == COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_GET.ID)
                {
                    byte len = 0;
                    var cmd = (COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_GET)command;
                    var data = new COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_REPORT();
                    data.deviceIdData = new byte[len];
                    data.properties1 = new COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_REPORT.Tproperties1()
                    {
                        deviceIdType = cmd.properties1.deviceIdType
                    };
                    data.properties2 = new COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_REPORT.Tproperties2()
                    {
                        deviceIdDataFormat = 0x00, // UTF-8
                        deviceIdDataLengthIndicator = len
                    };

                    var sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
            }
        }
    }
}
