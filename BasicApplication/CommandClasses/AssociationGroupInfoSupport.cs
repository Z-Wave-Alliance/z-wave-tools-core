/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Text;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.Devices;
using Utils;

namespace ZWave.BasicApplication.CommandClasses
{
    public class AssociationGroupInfoSupport : DelayedResponseOperation
    {
        #region Enums
        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }
        #endregion

        public byte GroupId { get; set; }
        public string GroupName { get; set; }
        public List<byte> AssociatedNodeIds { get; set; }
        public byte MaxNodesSupported { get; set; }

        public AssociationGroupInfoSupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ID))
        {
            GroupId = 0x01;
            GroupName = "Lifeline";
            AssociatedNodeIds = new List<byte>();
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
                    if (command[1] == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_NAME_GET.ID)
                    {
                        var associationNameGetCmd = (COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_NAME_GET)command;
                        var requestedGroupId = associationNameGetCmd.groupingIdentifier;
                        if (requestedGroupId == GroupId)
                        {
                            ApiOperation sendData = null;
                            var data = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_NAME_REPORT()
                            {
                                groupingIdentifier = GroupId,
                                lengthOfName = (byte)Encoding.UTF8.GetByteCount(GroupName),
                                name = Encoding.UTF8.GetBytes(GroupName)
                            };
                            sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                            ou.SetNextActionItems(sendData);
                        }
                    }
                    else if (command[1] == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_GET.ID)
                    {
                        var associationInfoGetCmd = (COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_GET)command;
                        var requestedGroupId = associationInfoGetCmd.groupingIdentifier;
                        ApiOperation sendData = null;
                        var data = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_REPORT()
                        {
                            properties1 = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_REPORT.Tproperties1()
                            {
                                listMode = associationInfoGetCmd.properties1.listMode,
                                groupCount = 0x01
                            },
                            vg1 = new List<COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_REPORT.TVG1>()
                        {
                            new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_REPORT.TVG1()
                            {
                                groupingIdentifier = GroupId,
                                mode = 0,
                                profile1 = 0,
                                profile2 = 1
                            }
                        }
                        };
                        sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                        ou.SetNextActionItems(sendData);
                    }
                    else if (command[1] == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_COMMAND_LIST_GET.ID)
                    {
                        var associationCommandListGetCmd = (COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_COMMAND_LIST_GET)command;
                        var requestedGroupId = associationCommandListGetCmd.groupingIdentifier;
                        ApiOperation sendData = null;
                        var data = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_COMMAND_LIST_REPORT()
                        {
                            groupingIdentifier = GroupId,
                            listLength = 0x02,
                            command = new List<byte>()
                        {
                            COMMAND_CLASS_BASIC.ID,
                            COMMAND_CLASS_BASIC.BASIC_GET.ID,
                        }
                        };
                        sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                        ou.SetNextActionItems(sendData);
                    }
                }
            }
        }
    }
}
