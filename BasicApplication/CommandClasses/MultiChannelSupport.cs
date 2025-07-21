/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using Utils;
using System.Text;
using ZWave.Devices;

namespace ZWave.BasicApplication.CommandClasses
{
    public class MultiChannelSupport : DelayedResponseOperation
    {
        private const byte END_POINTS_COUNT = 3;
        private const byte GENERIC_DEVICE_CLASS = 0x18; //0x10 TODO:check
        private const byte SPECIFIC_DEVICE_CLASS = 0x00;
        private const byte GENERIC_TYPE_NON_INTEROPERABLE = 0xFF;
        private byte[] ENDPOINTS_COMMAND_CLASSES { get; set; } = {
                COMMAND_CLASS_SWITCH_BINARY.ID,
                COMMAND_CLASS_SWITCH_MULTILEVEL_V4.ID,
                COMMAND_CLASS_ASSOCIATION.ID,
                COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ID,
                COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.ID,
                COMMAND_CLASS_NOTIFICATION_V8.ID,
                COMMAND_CLASS_SUPERVISION.ID,
        };

        #region endpointStatus variables

        private byte binarySwithStatus { get; set; } = 0x00;

        #endregion

        private byte _groupId { get; set; }
        private string _groupName { get; set; }
        private List<byte>[] _associatedNodeIds { get; set; }
        private byte _maxNodesSupported { get; set; }


        public TransmitOptions TxOptions { get; set; }

        public MultiChannelSupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_MULTI_CHANNEL_V4.ID))
        {
            _groupId = 0x01;
            _groupName = "Lifeline";
            _associatedNodeIds = new List<byte>[END_POINTS_COUNT];
            for (int i = 0; i < END_POINTS_COUNT; i++)
            {
                _associatedNodeIds[i] = new List<byte>();
            }
            _maxNodesSupported = 0xE8;
            TxOptions = txOptions;
        }

        public MultiChannelSupport(NetworkViewPoint network, TransmitOptions txOptions, byte[] encpointCommandClasses)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_MULTI_CHANNEL_V4.ID))
        {
            _groupId = 0x01;
            _groupName = "Lifeline";
            _associatedNodeIds = new List<byte>[END_POINTS_COUNT];
            for (int i = 0; i < END_POINTS_COUNT; i++)
            {
                _associatedNodeIds[i] = new List<byte>();
            }
            _maxNodesSupported = 0xE8;
            TxOptions = txOptions;
            ENDPOINTS_COMMAND_CLASSES = encpointCommandClasses;
        }

        //TODO:
        //MULTI_CHANNEL_END_POINT_FIND
        //MULTI_INSTANCE_CMD_ENCAP
        //MULTI_INSTANCE_GET
        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();
            var node = ReceivedAchData.SrcNode;
            byte[] command = ReceivedAchData.Command;
            var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
            bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);
            var substituteFlags = ReceivedAchData.SubstituteIncomingFlags;
            var sendData = new SendDataExOperation(_network, node, null, TxOptions, scheme);
            if (substituteFlags.HasFlag(SubstituteIncomingFlags.Crc16Encap))
            {
                sendData.SubstituteSettings.SetFlag(SubstituteFlags.UseCrc16Encap);
            }

            if (command != null && command.Length > 1 && isSuportedScheme)
            {
                if (command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_GET.ID)
                {
                    var cmd = (COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_GET)command;
                    var rpt = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT()
                    {
                        properties1 = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT.Tproperties1()
                        {
                            res1 = 0x00,
                            identical = 0x00,
                            dynamic = 0x00,
                        },
                        properties2 = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT.Tproperties2()
                        {
                            individualEndPoints = END_POINTS_COUNT,
                            res2 = 0x00
                        },
                        properties3 = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT.Tproperties3()
                        {
                        }
                    };
                    sendData.Data = rpt;
                    ou.SetNextActionItems(sendData);
                }
                else if (command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_GET.ID)
                {
                    var cmd = (COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_GET)command;
                    if (cmd.properties1.endPoint > 0 &&
                        cmd.properties1.endPoint <= END_POINTS_COUNT)
                    {

                        var rpt = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_REPORT()
                        {
                            genericDeviceClass = GENERIC_DEVICE_CLASS,
                            specificDeviceClass = SPECIFIC_DEVICE_CLASS,
                            properties1 =
                            {
                                dynamic = 0,
                                endPoint = cmd.properties1.endPoint
                            },
                            commandClass = GetCapability(cmd.properties1.endPoint)
                        };
                        sendData.Data = rpt;
                        ou.SetNextActionItems(sendData);
                    }
                }
                else if (command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_AGGREGATED_MEMBERS_GET.ID)
                {
                    var cmd = (COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_AGGREGATED_MEMBERS_GET)command;
                    if (cmd.properties1.aggregatedEndPoint > 0 &&
                        cmd.properties1.aggregatedEndPoint <= END_POINTS_COUNT)
                    {
                        var rpt = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_AGGREGATED_MEMBERS_REPORT()
                        {
                            properties1 =
                            {
                                aggregatedEndPoint = cmd.properties1.aggregatedEndPoint,
                                res = 0
                            },
                            numberOfBitMasks = 0x01
                        };
                        rpt.aggregatedMembersBitMask.Add(0x03);
                        sendData.Data = rpt;
                        ou.SetNextActionItems(sendData);
                    }
                }
                else if (command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP.ID)
                {
                    var data = UnboxAndGetResponse(command);
                    if (data != null && data.Length > 0)
                    {
                        sendData.Data = data;
                        ou.SetNextActionItems(sendData);
                    }
                }
                else if (command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_FIND.ID)
                {
                    var cmd = (COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_FIND)command;
                    var GDC = cmd.genericDeviceClass;
                    var SDC = cmd.specificDeviceClass;

                    var rpt = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_FIND_REPORT();
                    rpt.reportsToFollow = 0;
                    rpt.genericDeviceClass = GDC;
                    rpt.specificDeviceClass = SDC;
                    rpt.vg = new List<COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_FIND_REPORT.TVG>();
                    if ((GDC == GENERIC_TYPE_NON_INTEROPERABLE || SDC == GENERIC_DEVICE_CLASS) && // The value 0xFF MUST indicate that all existing End Points MUST be returned
                        (SDC == 0xFF || SDC == SPECIFIC_DEVICE_CLASS))
                    {
                        for (int i = 0; i < END_POINTS_COUNT; i++)
                        {
                            var tVG = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_FIND_REPORT.TVG();
                            tVG.properties1.endPoint = (byte)(i + 1);
                            rpt.vg.Add(tVG);
                        }
                    }
                    else
                    {
                        var tVG = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_FIND_REPORT.TVG() { properties1 = 0 };
                        rpt.vg.Add(tVG);
                    }
                    sendData.Data = rpt;
                    ou.SetNextActionItems(sendData);
                }                
            }
        }

        private byte[] UnboxAndGetResponse(byte[] encapsulatedCommand)
        {
            byte[] data = null;
            var encapCmd = (COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP)encapsulatedCommand;
            var destEPId = encapCmd.properties2.destinationEndPoint;
            if (destEPId > 0 && destEPId <= END_POINTS_COUNT)
            {
                var command = new List<byte>();
                command.Add(encapCmd.commandClass);
                command.Add(encapCmd.command);
                command.AddRange(encapCmd.parameter);

                #region Security and Capability
                if (encapCmd.commandClass == COMMAND_CLASS_SECURITY_2.ID &&
                    encapCmd.command == COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET.ID)
                {
                    List<byte> secureClasses = new List<byte>(_network.GetSecureFilteredCommandClasses(ENDPOINTS_COMMAND_CLASSES, true));
                    var rpt = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT()
                    {
                        commandClass = secureClasses
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_SECURITY.ID &&
                    encapCmd.command == COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT()
                    {
                        commandClassSupport = new List<byte>(_network.GetSecureFilteredCommandClasses(ENDPOINTS_COMMAND_CLASSES, true))
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_MULTI_CHANNEL_V4.ID &&
                    encapCmd.command == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_REPORT()
                    {
                        genericDeviceClass = GENERIC_DEVICE_CLASS,
                        specificDeviceClass = SPECIFIC_DEVICE_CLASS,
                        properties1 = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_REPORT.Tproperties1()
                        {
                            endPoint = destEPId
                        },
                        commandClass = GetCapability(destEPId)
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                #endregion

                #region Association
                else if (encapCmd.commandClass == COMMAND_CLASS_ASSOCIATION.ID)
                {
                    if (encapCmd.command == COMMAND_CLASS_ASSOCIATION.ASSOCIATION_GROUPINGS_GET.ID)
                    {
                        var unboxedCmd = (COMMAND_CLASS_ASSOCIATION.ASSOCIATION_GROUPINGS_GET)command.ToArray();

                        var rpt = new COMMAND_CLASS_ASSOCIATION.ASSOCIATION_GROUPINGS_REPORT()
                        {
                            supportedGroupings = 0x01
                        };
                        data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);

                    }
                    else if (encapCmd.command == COMMAND_CLASS_ASSOCIATION.ASSOCIATION_GET.ID)
                    {
                        var unboxedCmd = (COMMAND_CLASS_ASSOCIATION.ASSOCIATION_GET)command.ToArray();

                        var rpt = new COMMAND_CLASS_ASSOCIATION.ASSOCIATION_REPORT()
                        {
                            groupingIdentifier = 0x01,
                            maxNodesSupported = 0x01,
                            nodeid = new List<byte>() { 0x01 },
                            reportsToFollow = 0x00
                        };
                        data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);

                    }
                }
                #endregion

                #region MULTI CHANNEL ASSOCIATIONS
                else if (encapCmd.commandClass == COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID)
                {
                    if (encapCmd.command == COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.MULTI_CHANNEL_ASSOCIATION_GET.ID)
                    {
                        var unboxedCmd = (COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.MULTI_CHANNEL_ASSOCIATION_GET)command.ToArray();
                        if (unboxedCmd.groupingIdentifier == _groupId)
                        {
                            var rpt = new COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.MULTI_CHANNEL_ASSOCIATION_REPORT()
                            {
                                groupingIdentifier = _groupId,
                                nodeId = _associatedNodeIds[destEPId - 1],
                                maxNodesSupported = _maxNodesSupported
                            };
                            data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                        }
                    }
                    else if (encapCmd.command == COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.MULTI_CHANNEL_ASSOCIATION_GROUPINGS_GET.ID)
                    {
                        var rpt = new COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.MULTI_CHANNEL_ASSOCIATION_GROUPINGS_REPORT()
                        {
                            supportedGroupings = 0x02
                        };
                        data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                    }
                    else if (encapCmd.command == COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.MULTI_CHANNEL_ASSOCIATION_REMOVE.ID)
                    {
                        var unboxedCmd = (COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.MULTI_CHANNEL_ASSOCIATION_REMOVE)command.ToArray();
                        if (unboxedCmd.groupingIdentifier == _groupId)
                        {
                            foreach (var associateNodeId in unboxedCmd.nodeId)
                            {
                                if (!_associatedNodeIds[destEPId - 1].Contains(associateNodeId))
                                {
                                    _associatedNodeIds[destEPId - 1].Remove(associateNodeId);
                                }
                            }
                        }
                    }
                    else if (encapCmd.command == COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.MULTI_CHANNEL_ASSOCIATION_SET.ID)
                    {
                        var unboxedCmd = (COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.MULTI_CHANNEL_ASSOCIATION_SET)command.ToArray();
                        if (unboxedCmd.groupingIdentifier == _groupId)
                        {
                            foreach (var associateNodeId in unboxedCmd.nodeId)
                            {
                                if (!_associatedNodeIds[destEPId - 1].Contains(associateNodeId))
                                {
                                    _associatedNodeIds[destEPId - 1].Add(associateNodeId);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Association GRP
                else if (encapCmd.commandClass == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ID)
                {
                    if (encapCmd.command == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_NAME_GET.ID)
                    {
                        var associationNameGetCmd = (COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_NAME_GET)command.ToArray();
                        var requestedGroupId = associationNameGetCmd.groupingIdentifier;
                        if (requestedGroupId == _groupId)
                        {
                            var rpt = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_NAME_REPORT()
                            {
                                groupingIdentifier = _groupId,
                                lengthOfName = (byte)Encoding.UTF8.GetByteCount(_groupName),
                                name = Encoding.UTF8.GetBytes(_groupName)
                            };
                            data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                        }
                    }
                    else if (encapCmd.command == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_GET.ID)
                    {
                        var associationInfoGetCmd = (COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_GET)command.ToArray();
                        var requestedGroupId = associationInfoGetCmd.groupingIdentifier;
                        var rpt = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_INFO_REPORT()
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
                                    groupingIdentifier = _groupId,
                                    mode = 0,
                                    profile1 = 0,
                                    profile2 = 1
                                }
                            }
                        };
                        data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                    }
                    else if (encapCmd.command == COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_COMMAND_LIST_GET.ID)
                    {
                        var associationCommandListGetCmd = (COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_COMMAND_LIST_GET)command.ToArray();
                        var requestedGroupId = associationCommandListGetCmd.groupingIdentifier;
                        var rpt = new COMMAND_CLASS_ASSOCIATION_GRP_INFO_V3.ASSOCIATION_GROUP_COMMAND_LIST_REPORT()
                        {
                            groupingIdentifier = _groupId,
                            listLength = 0x02,
                            command = new List<byte>()
                        {
                            COMMAND_CLASS_BASIC.ID,
                            COMMAND_CLASS_BASIC.BASIC_GET.ID,
                        }
                        };
                        data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                    }
                }
                #endregion

                #region Multichannel Association
                else if (encapCmd.commandClass == COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.ID)
                {

                }
                #endregion

                #region Application responses 
                //Added:   Binary switch
                //Missing: Notification, Thermostat and Doorlock
                else
                {
                    if (encapCmd.commandClass == COMMAND_CLASS_SWITCH_BINARY_V2.ID)
                    {
                        if (encapCmd.command == COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_GET.ID)
                        {
                            var rpt = new COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_REPORT()
                            {
                                currentValue = binarySwithStatus,
                                targetValue = binarySwithStatus,
                                duration = 0x00
                            };
                            data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                        }
                        if (encapCmd.command == COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_SET.ID)
                        {
                            var switchBinarySetCmd = (COMMAND_CLASS_SWITCH_BINARY_V2.SWITCH_BINARY_SET)command.ToArray();

                            binarySwithStatus = switchBinarySetCmd.targetValue;
                        }
                    }
                }
                #endregion

                #region Interview responses
                if (encapCmd.commandClass == COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID &&
                        encapCmd.command == COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_REPORT()
                    {
                        zWaveVersion = 0x02,
                        installerIconType = new byte[] { 0x07, 0x00 },
                        userIconType = new byte[] { 0x07, 0x00 },
                        roleType = 0x05,
                        nodeType = 0x00
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if(encapCmd.commandClass == COMMAND_CLASS_VERSION.ID &&
                        encapCmd.command == COMMAND_CLASS_VERSION.VERSION_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_VERSION.VERSION_REPORT()
                    {
                        applicationSubVersion = 0x00,
                        applicationVersion = 0x01,
                        zWaveLibraryType = 0x01,
                        zWaveProtocolSubVersion = 0x00,
                        zWaveProtocolVersion = 0x01
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_VERSION.ID &&
                        encapCmd.command == COMMAND_CLASS_VERSION_V3.VERSION_CAPABILITIES_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_VERSION_V3.VERSION_CAPABILITIES_REPORT()
                    {
                        properties1 = { version = 0x03, commandClass = 0x01, zWaveSoftware = 0x01 }
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_VERSION.ID &&
                        encapCmd.command == COMMAND_CLASS_VERSION_V3.VERSION_ZWAVE_SOFTWARE_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_VERSION_V3.VERSION_ZWAVE_SOFTWARE_REPORT()
                    {
                        
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.ID &&
                        encapCmd.command == COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.MANUFACTURER_SPECIFIC_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.MANUFACTURER_SPECIFIC_REPORT()
                    {
                        productId = new byte[] { 0x00, 0x02 },
                        productTypeId = new byte[] { 0x00, 0x03 }
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.ID &&
                        encapCmd.command == COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.DEVICE_SPECIFIC_REPORT()
                    {
                        deviceIdData = new List<byte>(),
                        properties1 = { deviceIdType = 0}                        
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_BATTERY_V3.ID &&
                        encapCmd.command == COMMAND_CLASS_BATTERY_V3.BATTERY_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_BATTERY_V3.BATTERY_REPORT()
                    {
                        batteryLevel = 0,
                        properties1 = 0,
                        properties2 = 0
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_NOTIFICATION_V5.ID &&
                        encapCmd.command == COMMAND_CLASS_NOTIFICATION_V5.EVENT_SUPPORTED_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_NOTIFICATION_V5.EVENT_SUPPORTED_REPORT()
                    {
                        notificationType = 0x08,
                        properties1 = new COMMAND_CLASS_NOTIFICATION_V5.EVENT_SUPPORTED_REPORT.Tproperties1()
                        {
                            numberOfBitMasks = 0x02,
                            reserved = 0x00
                        },
                        bitMask = new byte[] { 0x00, 0x01 }
                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                else if (encapCmd.commandClass == COMMAND_CLASS_BASIC_V2.ID &&
                        encapCmd.command == COMMAND_CLASS_BASIC_V2.BASIC_GET.ID)
                {
                    var rpt = new COMMAND_CLASS_BASIC_V2.BASIC_REPORT()
                    {

                    };
                    data = EncapsulateData(rpt, destEPId, encapCmd.properties1.sourceEndPoint);
                }
                #endregion
            }
            return data;
        }

        private IList<byte> GetCapability(int endpointId)
        {
            var ret = new List<byte>();
            ret.Add(COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID);
            if (endpointId != 2) // exclude secure classes for testing
            {
                if (_network.HasSecurityScheme(SecuritySchemes.S0))
                {
                    ret.Add(COMMAND_CLASS_SECURITY.ID);
                }
                if (_network.HasSecurityScheme(SecuritySchemeSet.ALLS2))
                {
                    ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                }
            }
            if (!_network.HasSecurityScheme(SecuritySchemeSet.ALL))
            {
                ret.AddRange(ENDPOINTS_COMMAND_CLASSES);
            }
            else
            {
                ret.AddRange(_network.GetSecureFilteredCommandClasses(ENDPOINTS_COMMAND_CLASSES, false));
            }
            return ret;
        }

        private byte[] EncapsulateData(byte[] data, byte destinationEndPoint)
        {
            return EncapsulateData(data, 0, destinationEndPoint);
        }

        private byte[] EncapsulateData(byte[] data, byte sourceEndPoint, byte destinationEndPoint)
        {
            COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP multiChannelCmd = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP();
            multiChannelCmd.commandClass = data[0];
            multiChannelCmd.command = data[1];
            multiChannelCmd.parameter = new List<byte>();
            for (int i = 2; i < data.Length; i++)
            {
                multiChannelCmd.parameter.Add(data[i]);
            }
            multiChannelCmd.properties1.res = 0;
            multiChannelCmd.properties1.sourceEndPoint = sourceEndPoint;
            multiChannelCmd.properties2.bitAddress = 0;
            multiChannelCmd.properties2.destinationEndPoint = destinationEndPoint;
            data = multiChannelCmd;
            return data;
        }

    }
}
