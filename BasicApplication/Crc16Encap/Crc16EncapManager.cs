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
    public class Crc16EncapManager : SubstituteManagerBase
    {
        public Crc16EncapManager(NetworkViewPoint network)
            : base(network)
        { }

        protected override SubstituteIncomingFlags GetId()
        {
            return SubstituteIncomingFlags.Crc16Encap;
        }

        protected override CustomDataFrame SubstituteIncomingInternal(CustomDataFrame packet, NodeTag destNode, NodeTag srcNode, byte[] cmdData, int lenIndex, out ActionBase additionalAction, out ActionBase completeAction)
        {
            CustomDataFrame ret = packet;
            additionalAction = null;
            completeAction = null;
            if (IsActive)
            {
                if (cmdData.Length > 4 && cmdData[0] == COMMAND_CLASS_CRC_16_ENCAP.ID && cmdData[1] == COMMAND_CLASS_CRC_16_ENCAP.CRC_16_ENCAP.ID)
                {
                    COMMAND_CLASS_CRC_16_ENCAP.CRC_16_ENCAP cmd = cmdData;
                    if (Tools.CalculateCrc16Array(cmdData, 0, cmdData.Length - 2).SequenceEqual(cmd.checksum))
                    {
                        byte[] newFrameData = new byte[packet.Data.Length - 4];
                        Array.Copy(packet.Data, 0, newFrameData, 0, lenIndex);
                        newFrameData[lenIndex] = (byte)(cmdData.Length - 4);
                        Array.Copy(cmdData, 2, newFrameData, lenIndex + 1, cmdData.Length - 4);
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
            if (IsActive && (action is SendDataOperation || action is SendDataExOperation) &&
                action.SubstituteSettings.HasFlag(SubstituteFlags.UseCrc16Encap))
            {
                byte[] data = null;
                if (action is SendDataOperation)
                {
                    data = ((SendDataOperation)action).Data;
                }
                else
                {
                    data = ((SendDataExOperation)action).Data;
                }
                if (data.Length > 1)
                {
                    if (data[0] != COMMAND_CLASS_CRC_16_ENCAP.ID)
                    {
                        var substitutedData = new COMMAND_CLASS_CRC_16_ENCAP.CRC_16_ENCAP();
                        substitutedData.commandClass = data[0];
                        substitutedData.command = data[1];
                        substitutedData.data = new List<byte>();

                        for (int i = 2; i < data.Length; i++)
                        {
                            substitutedData.data.Add(data[i]);
                        }

                        substitutedData.checksum = new byte[] { 0, 0 };
                        byte[] tmp = substitutedData;
                        ushort crc = Utils.Tools.ZW_CreateCrc16(null, 0, tmp, (byte)(tmp.Length - 2));
                        substitutedData.checksum = new[] { (byte)(crc >> 8), (byte)crc };

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
