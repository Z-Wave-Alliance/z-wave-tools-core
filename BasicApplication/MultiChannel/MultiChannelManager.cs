/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZWave.Layers.Frame;
using Utils;
using ZWave.CommandClasses;
using ZWave.BasicApplication.Operations;
using ZWave.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public class MultiChannelManager : SubstituteManagerBase
    {
        public MultiChannelManager(NetworkViewPoint network)
            : base(network)
        { }

        protected override SubstituteIncomingFlags GetId()
        {
            return SubstituteIncomingFlags.MultiChannel;
        }

        protected override CustomDataFrame SubstituteIncomingInternal(CustomDataFrame packet, NodeTag destNode, NodeTag srcNode, byte[] cmdData, int lenIndex, out ActionBase additionalAction, out ActionBase completeAction)
        {
            CustomDataFrame ret = packet;
            additionalAction = null;
            completeAction = null;
            if (IsActive)
            {
                if (cmdData.Length > 4 && cmdData[0] == COMMAND_CLASS_MULTI_CHANNEL_V2.ID && cmdData[1] == COMMAND_CLASS_MULTI_CHANNEL_V2.MULTI_CHANNEL_CMD_ENCAP.ID)
                {
                    COMMAND_CLASS_MULTI_CHANNEL_V2.MULTI_CHANNEL_CMD_ENCAP cmd = cmdData;

                    byte[] newFrameData = new byte[packet.Data.Length - 4];
                    Array.Copy(packet.Data, 0, newFrameData, 0, lenIndex);
                    newFrameData[lenIndex] = (byte)(cmdData.Length - 4);
                    Array.Copy(cmdData, 4, newFrameData, lenIndex + 1, cmdData.Length - 4);
                    Array.Copy(packet.Data, lenIndex + 1 + cmdData.Length, newFrameData, lenIndex + 1 + cmdData.Length - 4,
                        packet.Data.Length - lenIndex - 1 - cmdData.Length);
                    ret = CreateNewFrame(packet, newFrameData);
                    ret.SrcEndPoint = cmd.properties1.sourceEndPoint;
                    ret.DstEndPoint = cmd.properties2.destinationEndPoint;
                    ret.IsBitAdress = cmd.properties2.bitAddress > 0;
                }
            }
            return ret;
        }

        public override ActionBase SubstituteActionInternal(ApiOperation action)
        {
            ActionBase ret = null;
            if (IsActive && (action is SendDataOperation || action is SendDataExOperation))
            {
                byte[] data = null;
                byte srcEndPointId = 0;
                byte dstEndPointId = 0;
                bool isBitAdress = false;
                if (action is SendDataOperation)
                {
                    data = ((SendDataOperation)action).Data;
                    srcEndPointId = ((SendDataOperation)action).SrcNode.EndPointId;
                    dstEndPointId = ((SendDataOperation)action).DstNode.EndPointId;
                    isBitAdress = ((SendDataOperation)action).DstNode.IsBitAddress;
                }
                else if (action is SendDataExOperation)
                {
                    data = ((SendDataExOperation)action).Data;
                    srcEndPointId = ((SendDataExOperation)action).SrcNode.EndPointId;
                    dstEndPointId = ((SendDataExOperation)action).DstNode.EndPointId;
                    isBitAdress = ((SendDataExOperation)action).DstNode.IsBitAddress;
                }
                else if (action is SendDataBridgeOperation)
                {
                    data = ((SendDataBridgeOperation)action).Data;
                    srcEndPointId = ((SendDataBridgeOperation)action).SrcNode.EndPointId;
                    dstEndPointId = ((SendDataBridgeOperation)action).DstNode.EndPointId;
                    isBitAdress = ((SendDataBridgeOperation)action).DstNode.IsBitAddress;
                }

                if (action.SubstituteSettings.HasFlag(SubstituteFlags.UseMultiChannel))
                {
                    srcEndPointId = action.SubstituteSettings.SrcEndPoint;
                    dstEndPointId = action.SubstituteSettings.DstEndPoint;
                    isBitAdress = action.SubstituteSettings.IsBitAdress;
                }

                if (data.Length > 1 && (dstEndPointId > 0 || srcEndPointId > 0))
                {
                    if (data[0] != COMMAND_CLASS_MULTI_CHANNEL_V2.ID)
                    {
                        var substitutedData = new COMMAND_CLASS_MULTI_CHANNEL_V2.MULTI_CHANNEL_CMD_ENCAP();
                        substitutedData.commandClass = data[0];
                        substitutedData.command = data[1];
                        substitutedData.parameter = new List<byte>();
                        substitutedData.properties1.sourceEndPoint = srcEndPointId;
                        substitutedData.properties2.destinationEndPoint = dstEndPointId;
                        substitutedData.properties2.bitAddress = (byte)(isBitAdress ? 1 : 0);

                        for (int i = 2; i < data.Length; i++)
                        {
                            substitutedData.parameter.Add(data[i]);
                        }
                        data = substitutedData;
                        if (action is SendDataOperation)
                        {
                            ((SendDataOperation)action).Data = data;
                        }
                        else
                        {
                            ((SendDataExOperation)action).Data = data;
                        }
                        ret = action;
                    }
                }
            }
            return ret;
        }
    }
}
