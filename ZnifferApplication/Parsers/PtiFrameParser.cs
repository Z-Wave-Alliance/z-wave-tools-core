/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Enums;
using ZWave.Xml.FrameHeader;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication
{
    public static class PtiFrameParser
    {
        private static readonly int MIN_FRAME_SIZE = 3;
        private static readonly int MAX_FRAME_SIZE = byte.MaxValue;

        public static DataItem GetDataItem(ApiTypes apiType, DateTime timeStamp, FrameDefinition frameDefinition, ushort sessionId, byte[] data)
        {
            DataItem dataItem = CreateDataItem(apiType, timeStamp, frameDefinition, sessionId, data);
            return dataItem;
        }

        private static bool tmp = false;
        private static readonly int LengthIndexInHeader = 7;
        private static readonly byte RegionIdMask = 0x0F;
        private static readonly byte ChannelMask = 0x0F;
        private static readonly byte ProtocolMask = 0x0F;
        private static readonly byte ZWaveProtocol = 0x06;

        // These defines are created from the rail_zwave_region_defines.h file.
        private static readonly byte[] RegionsBase1 = new byte[]
        {
            0x07, // Japan
            0x0A, // Korea
        };

        /*
         * In these byte array we are storing information about the available frequencies which is presented
         * in the ITU-T G.9959 specification (table A.1, A.2). The high nible stores the channel number and the
         * low nible stores the region ID which can be found in the rail_zwave.h e.g RAIL_ZWAVE_REGIONID_EU.
         */
        private static readonly ushort[] ChannelRegionsBaudLR = new ushort[]
        {
            (0x03 << 8) + 0x0C, // CH3 + US_LR1
            (0x03 << 8) + 0x0D, // CH3 + US_LR2
            (0x00 << 8) + 0x0E, // CH0 + US_LR3
            (0x01 << 8) + 0x0E, // CH1 + US_LR3

            (0x03 << 8) + 0x0F, // CH3 + EU_LR1
            (0x03 << 8) + 0x10, // CH3 + EU_LR2
            (0x00 << 8) + 0x11, // CH0 + EU_LR3
            (0x01 << 8) + 0x11, // CH1 + EU_LR3
        };

        private static readonly ushort[] ChannelRegionsBaud9600 = new ushort[]
        {
            (0x02 << 8) + 0x01, // European Union
            (0x02 << 8) + 0x02, // United States
            (0x02 << 8) + 0x03, // Australia/New Zealand
            (0x02 << 8) + 0x04, // Hong Kong
            (0x02 << 8) + 0x05, // Malaysia
            (0x02 << 8) + 0x06, // India
            (0x02 << 8) + 0x08, // Russian Federation
            (0x02 << 8) + 0x09, // Israel
            (0x02 << 8) + 0x0B, // China
            (0x02 << 8) + 0x0C, // United States, Long Range 1
            (0x02 << 8) + 0x0D, // United States, Long Range 2
            (0x02 << 8) + 0x0F, // European Union, Long Range 1
            (0x02 << 8) + 0x10, // European Union, Long Range 2
        };

        private static readonly ushort[] ChannelRegionsBaud40K = new ushort[]
        {
            (0x01 << 8) + 0x01, // European Union
            (0x01 << 8) + 0x02, // United States
            (0x01 << 8) + 0x03, // Australia/New Zealand
            (0x01 << 8) + 0x04, // Hong Kong
            (0x01 << 8) + 0x05, // Malaysia
            (0x01 << 8) + 0x06, // India
            (0x01 << 8) + 0x08, // Russian Federation
            (0x01 << 8) + 0x09, // Israel
            (0x01 << 8) + 0x0B, // China
            (0x01 << 8) + 0x0C, // United States, Long Range 1
            (0x01 << 8) + 0x0D, // United States, Long Range 2
            (0x01 << 8) + 0x0F, // European Union, Long Range 1
            (0x01 << 8) + 0x10, // European Union, Long Range 2
        };

        private static readonly ushort[] ChannelRegionsBaud100K = new ushort[]
        {
            (0x00 << 8) + 0x01, // European Union
            (0x00 << 8) + 0x02, // United States
            (0x00 << 8) + 0x03, // Australia/New Zealand
            (0x00 << 8) + 0x04, // Hong Kong
            (0x00 << 8) + 0x05, // Malaysia
            (0x00 << 8) + 0x06, // India
            (0x00 << 8) + 0x07, // Japan
            (0x01 << 8) + 0x07, // Japan
            (0x02 << 8) + 0x07, // Japan
            (0x00 << 8) + 0x08, // Russian Federation
            (0x00 << 8) + 0x09, // Israel
            (0x00 << 8) + 0x0A, // Korea
            (0x01 << 8) + 0x0A, // Korea
            (0x02 << 8) + 0x0A, // Korea
            (0x00 << 8) + 0x0B, // China
            (0x00 << 8) + 0x0C, // United States, Long Range 1
            (0x00 << 8) + 0x0D, // United States, Long Range 2
            (0x00 << 8) + 0x0F, // European Union, Long Range 1
            (0x00 << 8) + 0x10, // European Union, Long Range 2
        };

        private const byte HWRxStart = 0xF8;   // Rx Start
        private const byte HWTxStart = 0xFC;   // Tx Start
        private const byte HWRxSuccess = 0xF9; // Rx Success
        private const byte HWTxSuccess = 0xFD; // Tx Success
        private const byte HWRxAbort = 0xFA;   // Rx Abort
        private const byte HWTxAbort = 0xFE;   // Tx Abort
        private static DataItem CreateDataItem(ApiTypes dcApiType, DateTime timeStamp, FrameDefinition frameDefinition, ushort sessionId, byte[] data)
        {
            DataItem dataItem = null;
            var apiType = ApiTypes.PtiDiagnostic;
            int RssiOffset = 1;
            int BeforeDataLength = 12;
            int AfterDataLength = 5;
            if (data[0] == 2)
            {
                BeforeDataLength = 12;
            }
            else if (data[0] == 3)
            {
                BeforeDataLength = 19;
            }
            if (data.Length > BeforeDataLength && BeforeDataLength > 0)
            {
                switch (data[BeforeDataLength - 1])
                {
                    case HWRxStart:
                        AfterDataLength = 6;
                        RssiOffset = 1;
                        if (data.Length > BeforeDataLength + AfterDataLength
                            && data[data.Length - AfterDataLength] == HWRxSuccess)
                        {
                            apiType = ApiTypes.Pti;
                        }
                        break;
                    case HWTxStart:
                        AfterDataLength = 5;
                        RssiOffset = 0;
                        if (data.Length > BeforeDataLength + AfterDataLength
                           && data[data.Length - AfterDataLength] == HWTxSuccess)
                        {
                            apiType = ApiTypes.Pti;
                        }
                        break;
                    default:
                        BeforeDataLength = -1;
                        AfterDataLength = -1;
                        break;
                }
            }

            if (BeforeDataLength == -1 && AfterDataLength == -1)
            {
                dataItem = new DataItem();
                ParseHeaderWithCrc(frameDefinition, dataItem);
                dataItem.HeaderType = HeaderStore.H_UNKNOWN;
            }
            else if (AfterDataLength > 0 && data != null && data.Length > BeforeDataLength + AfterDataLength)
            {
                int RegionIdOffset = RssiOffset + 1;
                int ChannelOffset = RssiOffset + 2;
                int ProtocolOffset = RssiOffset + 3;

                var protocol = data[data.Length - AfterDataLength + ProtocolOffset] & ProtocolMask;
                byte region = (byte)(data[data.Length - AfterDataLength + RegionIdOffset] & RegionIdMask);
                var channel = (byte)(data[data.Length - AfterDataLength + ChannelOffset] & ChannelMask);
                var rssi = (byte)(RssiOffset > 0 ? data[data.Length - AfterDataLength + RssiOffset] : 0);
                ushort key = (ushort)((channel << 8) + region);
                var packetLength = data.Length - BeforeDataLength - AfterDataLength;
                if (protocol == ZWaveProtocol) // && rssi != 0xF9 && rssi != 0xFA (ZNF-389)
                {
                    dataItem = new DataItem();
                    dataItem.Channel = channel;
                    dataItem.Rssi = rssi;
                    dataItem.ApiType = apiType;
                    dataItem.Speed = 0;
                    if (ChannelRegionsBaud9600.Contains(key))
                    {
                        dataItem.Speed = 0;
                    }
                    else if (ChannelRegionsBaud40K.Contains(key))
                    {
                        dataItem.Speed = 1;
                    }
                    else if (ChannelRegionsBaud100K.Contains(key))
                    {
                        dataItem.Speed = 2;
                    }
                    else if (ChannelRegionsBaudLR.Contains(key))
                    {
                        dataItem.Speed = 3;
                    }
                    dataItem.CreatedAt = timeStamp;
                    dataItem.Frequency = region;
                    if (data[BeforeDataLength] != 0x55)
                    {
                        if (apiType == ApiTypes.Pti && LengthIndexInHeader < packetLength
                            && data[BeforeDataLength + LengthIndexInHeader] <= packetLength)
                        {
                            dataItem.SetData(data, BeforeDataLength, data[BeforeDataLength + LengthIndexInHeader]);
                        }
                        else
                        {
                            dataItem.SetData(data, BeforeDataLength, packetLength);
                        }
                    }
                    else
                    {
                        dataItem.ApiType = ApiTypes.Pti; // (ZNF-389)
                        dataItem.WakeUpBeamType = WakeUpBeamTypes.Impulse;
                        dataItem.SetData(data, BeforeDataLength, data.Length - BeforeDataLength - AfterDataLength);
                        dataItem.WakeupCounter = ParseWakeUpBeamsCounter((int)((dataItem.Speed == 3) ? 4 : 3),
                            data, BeforeDataLength, data.Length - BeforeDataLength - AfterDataLength);
                    }
                    ParseHeaderWithCrc(frameDefinition, dataItem);
                }
            }
            return dataItem;
        }

        private static ushort ParseWakeUpBeamsCounter(int frameLength, byte[] data, int dataOffset, int dataLength)
        {
            ushort ret = 1;

            // make sure we not exceed length in case of missing home id hash
            if (frameLength > dataLength)
            {
                frameLength = dataLength;
            }

            // check if data contains more beams
            int length = dataLength - frameLength;
            int offset = dataOffset + frameLength;
            while (length - frameLength >= 0)
            {
                // align
                if (data[offset] != 0x55)
                {
                    offset++;
                    length--;
                }
                else
                {
                    // verify
                    if (AreArraysEquals(data, dataOffset, frameLength, data, offset))
                    {
                        ret++;
                        offset += frameLength;
                        length -= frameLength;
                    }
                    // shift next
                    else
                    {
                        offset++;
                        length--;
                    }
                }
            }
            return ret;
        }

        private static bool AreArraysEquals(byte[] array1, int offset1, int length, byte[] array2, int offset2)
        {
            bool ret = false;
            if (array1.Length >= offset1 + length && array2.Length >= offset2 + length)
            {
                ret = true;
                for (int i = 0; i < length && ret; i++)
                {
                    ret = array1[offset1 + i] == array2[offset2 + i];
                }
            }
            return ret;
        }

        private static void ParseHeaderWithCrc(FrameDefinition frameDefinition, DataItem dataItem)
        {
            if (frameDefinition != null)
            {
                if (dataItem != null && dataItem.WakeupCounter == 0)
                {
                    bool isCrcOk = false;
                    Header header = null;
                    byte crcBytes = (byte)(dataItem.Speed > 1 ? 2 : 1);
                    byte baseHeaderKey = 0;
                    if (dataItem.Speed == 3)
                    {
                        baseHeaderKey = 2;
                    }
                    else if (RegionsBase1.Contains(dataItem.Frequency))
                    {
                        baseHeaderKey = 1;
                    }
                    isCrcOk = frameDefinition.ParseHeaderWithCrc(dataItem.DataBuffer, crcBytes, baseHeaderKey, out header);
                    if (!isCrcOk)
                    {
                        dataItem.HeaderType = HeaderStore.H_CRC_FALSE;
                    }
                    else
                    {
                        if (header != null && dataItem.ApiType == ApiTypes.Pti)
                        {
                            dataItem.HeaderType = header.Key;
                        }
                        else
                        {
                            dataItem.HeaderType = HeaderStore.H_UNKNOWN;
                        }
                    }
                }
                else
                {
                    if (dataItem.WakeUpBeamType == WakeUpBeamTypes.Start)
                    {
                        dataItem.HeaderType = HeaderStore.H_WAKEUP_START;
                    }
                    else if (dataItem.WakeUpBeamType == WakeUpBeamTypes.Stop)
                    {
                        dataItem.HeaderType = HeaderStore.H_WAKEUP_STOP;
                    }
                    else
                    {
                        dataItem.HeaderType = dataItem.Speed == 3 ? HeaderStore.H_WAKEUP_LR : HeaderStore.H_WAKEUP;
                    }
                }
            }
        }
    }
}
