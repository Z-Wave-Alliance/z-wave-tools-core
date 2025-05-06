/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication
{
    public class XModemDataFrame
    {
        //XmodemCRC Packet Format
        //Byte 1           |Byte 2         |Byte 3 Bytes       |4-131          |Bytes 132-133 
        //Start of Header  |Packet Number  |(Packet Number)    |Packet Data    |16-bit CRC
        public const int FrameLength = 133;
        public const int HeaderLength = 3;
        public const int CrcChecksumLength = 2;
        public const int PayloadSize = FrameLength - HeaderLength - CrcChecksumLength;

        ApiTypes ApiType { get { return ApiTypes.XModem; } }
        public byte[] Payload { get; private set; }
        public byte[] Buffer { get; private set; }

        public XModemDataFrame(byte packetNo, byte[] data)
        {
            if (FrameLength - data.Length - (HeaderLength + CrcChecksumLength) < 0)
            {
                throw new ArgumentException("Not valid data size");
            }
            Payload = data;
            Buffer = CreateFrameBuffer(packetNo, Payload);
        }

        private byte[] CreateFrameBuffer(byte packetNo, byte[] packetData)
        {
            if (packetData.Length < PayloadSize)
            {
                Array.Resize(ref packetData, PayloadSize);
            }
            var crc = CalculateChecksum(packetData);
            var bufferLengh = packetData.Length + HeaderLength + CrcChecksumLength;
            byte[] ret = new byte[bufferLengh];
            ret[0] = (byte)XModemRecieverTransmisionStatuses.SOH;
            ret[1] = packetNo;
            ret[2] = (byte)~packetNo;
            Array.Copy(packetData, 0, ret, 3, packetData.Length);
            ret[bufferLengh - 2] = crc[0];
            ret[bufferLengh - 1] = crc[1];
            return ret;
        }

        private static byte[] CalculateChecksum(byte[] data)
        {
            int crc = 0;
            for (int i = 0; i < data.Length; i++)
            {
                crc = crc ^ ((int)data[i]) << 8;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                    {
                        crc = crc << 1 ^ 0x1021;
                    }
                    else
                    {
                        crc = crc << 1;
                    }
                }
            }
            //ushort crc = Tools.ZW_CheckCrc16(0, data, (ushort)data.Length);
            return new byte[] { (byte)(crc >> 8), (byte)(crc) };
        }

        public static bool IsChecksumValid(byte[] payload, byte[] crc)
        {
            return CalculateChecksum(payload).SequenceEqual(crc);
        }

        public static implicit operator byte[](XModemDataFrame dataFrame)
        {
            return dataFrame.Buffer;
        }
    }
}
