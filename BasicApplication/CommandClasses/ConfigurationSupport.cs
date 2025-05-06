/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    /// <summary>
    /// Support Task handles response on requests of COMMAND_CLASS_CONFIGURATION.
    /// Note: Will split responses if a sender does not support Transport Service.
    /// </summary>
    public class ConfigurationSupport : DelayedResponseOperation
    {
        private TransmitOptions _txOptions;
        private string[] paramsTxt = new string[]
        {
            "Test Param",
            "Signed Int Test Param",
            "Unsigned Int Test Param",
            "Radio Buttons Test Param",
            "Check Boxes Test Param",
        };

        public ConfigurationSupport(NetworkViewPoint network, TransmitOptions txOptions) 
            : base(network, NodeTag.Empty, NodeTag.Empty, COMMAND_CLASS_CONFIGURATION_V4.ID)
        {
            _txOptions = txOptions;
        }

        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();
            if (DstNode.EndPointId == ReceivedAchData.DstNode.EndPointId)
            {
                var node = ReceivedAchData.SrcNode;
                byte[] command = ReceivedAchData.Command;
                var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
                bool isSupportedScheme = IsSupportedScheme(_network, node, command, scheme);

                byte[] response = null;
                ApiOperation sendData = null;

                if (command != null && command.Length > 1 && isSupportedScheme)
                {
                    switch (command[1])
                    {
                        case (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET.ID):
                            {
                                var hRes = HandleConfigurationNameGet(command, node, scheme);
                                ou.SetNextActionItems(hRes);
                            }
                            break;
                        case (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_PROPERTIES_GET.ID):
                            {
                                response = ConfigurationPropertiesGetReport(command);
                            }
                            break;
                        case (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_INFO_GET.ID):
                            {
                                var hRes = HandleConfigurationInfoGet(command, node, scheme);
                                ou.SetNextActionItems(hRes);
                            }
                            break;
                        case (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_GET.ID):
                            {
                                response = ConfigurationGetReport(command);
                            }
                            break;
                        default:
                            {
                                //Unhandled data like reports
                            }
                            break;
                    }
                }
                if (response != null)
                {
                    sendData = new SendDataExOperation(_network, node, response, _txOptions, scheme);
                    ou.SetNextActionItems(sendData);
                }
            }
        }

        private byte[] ConfigurationGetReport(COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_GET command)
        {
            var pn = command.parameterNumber;
            COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_REPORT rt = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_REPORT();
            rt.properties1.size = 0x01;
            rt.configurationValue = new byte[] { 0x01 };
            rt.parameterNumber = 0x01;
            return rt;
        }

        private byte[] ConfigurationPropertiesGetReport(COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_PROPERTIES_GET cmd)
        {
            COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_PROPERTIES_REPORT rt = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_PROPERTIES_REPORT();
            if (cmd.parameterNumber.SequenceEqual(new byte[] { 0x00, 0x01 }))
            {
                rt.parameterNumber = new byte[] { 0x00, 0x01 };
                rt.properties1.size = 2;
                rt.properties1.mreadonly = 1;
                rt.properties1.format = 0;
                rt.defaultValue = new byte[] { 0x00, 0x01 };
                rt.minValue = new byte[] { 0xFF, 0xFF };
                rt.maxValue = new byte[] { 0x7F, 0xFF };
                rt.nextParameterNumber = new byte[] { 0x00, 0x02 };
                return rt;
            }
            else if (cmd.parameterNumber.SequenceEqual(new byte[] { 0x00, 0x02 }))
            {
                rt.parameterNumber = new byte[] { 0x00, 0x02 };
                rt.properties1.size = 2;
                rt.properties1.mreadonly = 1;
                rt.properties1.format = 1;
                rt.defaultValue = new byte[] { 0x00, 0x01 };
                rt.minValue = new byte[] { 0x00, 0x00 };
                rt.maxValue = new byte[] { 0xFF, 0xFF };
                rt.nextParameterNumber = new byte[] { 0x00, 0x03 };
                return rt;
            }
            else if (cmd.parameterNumber.SequenceEqual(new byte[] { 0x00, 0x03 }))
            {
                rt.parameterNumber = new byte[] { 0x00, 0x03 };
                rt.properties1.size = 2;
                rt.properties1.mreadonly = 1;
                rt.properties1.format = 2;
                rt.defaultValue = new byte[] { 0x00, 0x01 };
                rt.minValue = new byte[] { 0x00, 0x01 };
                rt.maxValue = new byte[] { 0x00, 0x05 };
                rt.nextParameterNumber = new byte[] { 0x00, 0x04 };
                return rt;
            }
            else if (cmd.parameterNumber.SequenceEqual(new byte[] { 0x00, 0x04 }))
            {
                rt.parameterNumber = new byte[] { 0x00, 0x04 };
                rt.properties1.size = 2;
                rt.properties1.mreadonly = 1;
                rt.properties1.format = 3;
                rt.defaultValue = new byte[] { 0x00, 0x01 };
                rt.minValue = new byte[] { 0x00, 0x01 };
                rt.maxValue = new byte[] { 0x00, 0x04 };
                rt.nextParameterNumber = new byte[] { 0x00, 0x0A };
                return rt;
            }
            else if (cmd.parameterNumber.SequenceEqual(new byte[] { 0x00, 0x0A }))
            {
                rt.parameterNumber = new byte[] { 0x00, 0x0A };
                rt.properties1.size = 2;
                rt.properties1.mreadonly = 1;
                rt.properties1.format = 3;
                rt.defaultValue = new byte[] { 0x00, 0x01 };
                rt.minValue = new byte[] { 0x00, 0x01 };
                rt.maxValue = new byte[] { 0x00, 0x04 };
                rt.nextParameterNumber = new byte[] { 0x00, 0x00 };
                return rt;
            }
            return null;
        }

        private IActionItem[] HandleConfigurationInfoGet(COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_INFO_GET infoGetCmd, NodeTag node, SecuritySchemes scheme)
        {
            var ret = new List<IActionItem>();
            var rptInfo = paramsTxt[0];
            bool isSplitEnabled = true;
            if (infoGetCmd.parameterNumber[0] == 0 && infoGetCmd.parameterNumber[1].IsInRange(0, (byte)paramsTxt.Length))
            {
                rptInfo = paramsTxt[infoGetCmd.parameterNumber[1]];
                isSplitEnabled = !_network.HasCommandClass(node, COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
            }
            rptInfo += " Information";

            if (isSplitEnabled)
            {
                var items = rptInfo.Split(' ');
                var rc = items.Length;
                while (rc > 0)
                {
                    rc--;
                    var rpt = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_INFO_REPORT();
                    rpt.parameterNumber = infoGetCmd.parameterNumber;
                    rpt.info = Encoding.UTF8.GetBytes(items[rc]);
                    rpt.reportsToFollow = (byte)(rc);
                    var sendData = new SendDataExOperation(_network, node, rpt, _txOptions, scheme);
                    ret.Add(sendData);
                }
            }
            else
            {
                var rpt = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_INFO_REPORT();
                rpt.reportsToFollow = 0;
                rpt.parameterNumber = infoGetCmd.parameterNumber;
                rpt.info = Encoding.UTF8.GetBytes(rptInfo);
                var sendData = new SendDataExOperation(_network, node, rpt, _txOptions, scheme);
                ret.Add(sendData);
            }
            return ret.ToArray();
        }

        private IActionItem[] HandleConfigurationNameGet(COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET nameGetCmd, NodeTag node, SecuritySchemes scheme)
        {
            var ret = new List<IActionItem>();
            var rptName = paramsTxt[0];
            bool isSplitEnabled = true;
            if (nameGetCmd.parameterNumber[0] == 0 && nameGetCmd.parameterNumber[1].IsInRange(0, (byte)paramsTxt.Length))
            {
                rptName = paramsTxt[nameGetCmd.parameterNumber[1]];
                isSplitEnabled = !_network.HasCommandClass(node, COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
            }

            if (isSplitEnabled)
            {
                var items = rptName.Split(' ');
                var rc = items.Length;
                while (rc > 0)
                {
                    rc--;
                    var rpt = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_REPORT();
                    rpt.parameterNumber = nameGetCmd.parameterNumber;
                    rpt.name = Encoding.UTF8.GetBytes(items[rc]);
                    rpt.reportsToFollow = (byte)(rc);
                    var sendData = new SendDataExOperation(_network, node, rpt, _txOptions, scheme);
                    ret.Add(sendData);
                }
            }
            else
            {
                var rpt = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_REPORT();
                rpt.reportsToFollow = 0;
                rpt.parameterNumber = nameGetCmd.parameterNumber;
                rpt.name = Encoding.UTF8.GetBytes(rptName);
                var sendData = new SendDataExOperation(_network, node, rpt, _txOptions, scheme);
                ret.Add(sendData);
            }

            return ret.ToArray();
        }
    }
}
