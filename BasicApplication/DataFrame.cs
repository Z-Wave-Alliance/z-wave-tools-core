/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.Enums;
using ZWave.Layers.Frame;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication
{
    public class DataFrame : CustomDataFrame
    {
        private const int MAX_LENGTH = 255;

        public DataFrame(ushort sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp)
            : base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
            ApiType = ApiTypes.Basic;
        }

        protected override int GetMaxLength()
        {
            return MAX_LENGTH;
        }

        protected override byte[] RefreshData()
        {
            byte[] ret = null;
            if (Buffer.Length > 2)
            {
                ret = new byte[Buffer.Length - 3];
                Array.Copy(Buffer, 2, ret, 0, Buffer.Length - 3);
            }
            return ret;
        }

        protected override byte[] RefreshPayload()
        {
            byte[] ret = null;
            if (Data.Length > 1)
            {
                ret = new byte[Data.Length - 2];
                Array.Copy(Data, 2, ret, 0, Data.Length - 2);
            }
            return ret;
        }

        public static byte[] CreateFrameBuffer(byte[] data)
        {
            int frameLength = data.Length + 1;
            byte[] ret = new byte[data.Length + 3];
            ret[0] = (byte)HeaderTypes.Sof;
            ret[1] = (byte)frameLength;
            Array.Copy(data, 0, ret, 2, data.Length);

            if (ret.Length > 3 && ret[3] == (byte)CommandTypes.CmdTestInterface)
            {
                ret[3] = 0;
                ret[2] = ret[1];
                ret[1] = 0;

                data[1] = 0;
            }
            else if (ret.Length > 3 && ret[2] == (byte)CommandTypes.CmdTestInterface)
            {
                ret[1] = (byte)(frameLength >> 8);
                ret[2] = (byte)frameLength;
                data[0] = 0;
            }
            ret[data.Length + 2] = CalculateChecksum(data);
            return ret;
        }

        private static byte CalculateChecksum(byte[] data)
        {
            byte calcChksum = 0xFF;
            calcChksum ^= (byte)(data.Length + 1); // Length
            for (int i = 0; i < data.Length; i++)
            {
                calcChksum ^= data[i];      // Data
            }
            return calcChksum;
        }

        public bool IsChecksumValid(byte checksum)
        {
            return CalculateChecksum(Data) == checksum;
        }

        public static DataFrame CreateDataFrame(CustomDataFrame packet, int lenIndex, byte[] cmd,
          byte lastUsedSecurityScheme,
          bool hasActiveKeyForNode,
          byte activeSecuritySchemeForNode)
        {
            byte[] frameData = packet.Data;
            byte rssi = 0;
            int multiDestsOffsetNodeMaskLen = 0;
            byte[] multiDestsNodeMask = new byte[0];
            byte[] tail = new byte[2];

            if (frameData[1] == (byte)CommandTypes.CmdApplicationCommandHandler_Bridge)
            {
                int multiDestsOffsetNodeMaskLenIndex = lenIndex + 1 + frameData[lenIndex];
                if (frameData.Length > multiDestsOffsetNodeMaskLenIndex)
                {
                    multiDestsOffsetNodeMaskLen = frameData[multiDestsOffsetNodeMaskLenIndex];
                }
                if (multiDestsOffsetNodeMaskLen > 0 &&
                   frameData.Length > multiDestsOffsetNodeMaskLenIndex + 1 + multiDestsOffsetNodeMaskLen)
                {
                    multiDestsNodeMask = frameData.Skip(multiDestsOffsetNodeMaskLenIndex + 1).Take(multiDestsOffsetNodeMaskLen).ToArray();
                }
                if (frameData.Length > multiDestsOffsetNodeMaskLenIndex + 1 + multiDestsOffsetNodeMaskLen)
                {
                    rssi = frameData[multiDestsOffsetNodeMaskLenIndex + 1 + multiDestsOffsetNodeMaskLen];
                }
                if (multiDestsOffsetNodeMaskLen == 0)
                {
                    tail = new byte[3];
                }
                else
                {
                    tail = new byte[multiDestsOffsetNodeMaskLen + 1 + tail.Length];
                    Array.Copy(multiDestsNodeMask, 0, tail, 1, multiDestsOffsetNodeMaskLen);
                }
                tail[0] = (byte)multiDestsOffsetNodeMaskLen;
            }
            else
            {
                if (frameData.Length > lenIndex + 1 + frameData[lenIndex])
                {
                    rssi = frameData[lenIndex + 1 + frameData[lenIndex]];
                }
            }
            tail[tail.Length - 2] = rssi;
            tail[tail.Length - 1] = lastUsedSecurityScheme;

            "set scheme {0}"._DLOG(lastUsedSecurityScheme);

            byte[] payload = new byte[lenIndex + 1 + cmd.Length + tail.Length];
            for (int i = 0; i < lenIndex + 2; i++)
            {
                payload[i] = frameData[i];
            }
            payload[lenIndex] = (byte)cmd.Length;
            Array.Copy(cmd, 0, payload, lenIndex + 1, cmd.Length);
            Array.Copy(tail, 0, payload, lenIndex + 1 + cmd.Length, tail.Length);
            if (hasActiveKeyForNode)
            {
                payload[payload.Length - 1] = activeSecuritySchemeForNode;
            }
            DataFrame df = new DataFrame(packet.SessionId, packet.DataFrameType, packet.IsHandled, packet.IsOutcome,
                packet.SystemTimeStamp)
            { IsSubstituted = true };
            byte[] t = DataFrame.CreateFrameBuffer(payload);
            df.SetBuffer(t, 0, t.Length);
            var currentFlags = packet.SubstituteIncomingFlags;
            df.SubstituteIncomingFlags = (currentFlags | SubstituteIncomingFlags.Security);
            return df;
        }
    }
}
