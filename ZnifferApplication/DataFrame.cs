/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers;
using Utils;
using ZWave.ZnifferApplication.Enums;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWave.ZnifferApplication
{
    public class DataFrame : CustomDataFrame
    {
        private const int MAX_LENGTH = 255;
        #region CONST
        private const byte ZNIFFER_3X_DATA_FRAME_SOF = 0x3B;
        private const byte ZNIFFER_3X_DATA_MARKER = 0x23;
        private const byte ZNIFFER_3X_DATA_SEPARATOR = 0x2C;
        private const byte ZNIFFER_3X_DATA_FRAME_EOF = 0x2E;
        private const byte ZNIFFER_3X_COMMAND_FRAME_SOF = 0x26;
        private const byte ZNIFFER_3X_COMMAND_49_FRAME_SOF = 0x49;
        private const byte ZNIFFER_3X_COMMAND_46_FRAME_SOF = 0x46;
        private const byte ZNIFFER_3X_COMMAND_PAYLOAD_LENGTH = 6;
        private const byte ZNIFFER_3X_IS_WAKEUP_DATA = 0x55;
        private const byte ZNIFFER_3X_IS_WAKEUP_MARKER = 0x57;
        private const byte ZNIFFER_3X_COMMAND_FRAME_SET_FREQUENCY_DONE = 0x2E;

        private const byte ZNIFFER_4X_DATA_FRAME_SOF = 0x21;
        private const byte ZNIFFER_4X_COMMAND_FRAME_SOF = 0x23;
        private const byte ZNIFFER_4X_DATA_FRAME_CHANNEL_SHIFT = 5;
        private const byte ZNIFFER_4X_DATA_FRAME_SPEED_MASK = 0x1F;
        private const byte ZNIFFER_4X_IS_WAKEUP_DATA_MARKER = 0x02;
        private const byte ZNIFFER_4X_IS_SOF_MARKER = 0x01;
        private const byte ZNIFFER_4X_IS_DATA_MARKER = 0x03;
        #endregion
        public DataFrame(ushort sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp)
            : base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
            ApiType = ApiTypes.Zniffer;
            FrameData = new byte[] { (byte)CommandTypes.DataHandler };
            DataItem = new DataItem()
            {
                CreatedAt = timeStamp
            };
        }

        public DataItem DataItem { get; set; }
        public byte[] FrameData { get; set; }
        public bool IsZnifferDataFrame
        {
            get
            {
                return
                    Buffer != null &&
                    Buffer.Length > 0 &&
                    (Buffer[0] == ZNIFFER_3X_DATA_FRAME_SOF || Buffer[0] == ZNIFFER_4X_DATA_FRAME_SOF);
            }
        }

        protected override int GetMaxLength()
        {
            return MAX_LENGTH;
        }

        protected override byte[] RefreshData()
        {
            byte[] ret = null;
            if (Buffer.Length > 0)
            {
                if (ApiType == ApiTypes.Text)
                {
                    ret = Buffer;
                }
                else if (ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                {
                    ret = Buffer;
                }
                else
                {
                    switch (Buffer[0])
                    {
                        case ZNIFFER_3X_DATA_FRAME_SOF:
                            ret = FrameData;
                            break;
                        case ZNIFFER_4X_DATA_FRAME_SOF:
                            ret = FrameData;
                            break;


                        case (byte)CommandTypes.GetDeviceInfo3x:
                        case (byte)CommandTypes.SetFrequency3x:
                        case (byte)CommandTypes.GetDeviceInfo3xResponse:
                        case (byte)CommandTypes.SetFrequency3xResponse:
                            ret = Buffer;
                            break;

                        case ZNIFFER_4X_COMMAND_FRAME_SOF:
                            ret = new byte[Buffer.Length - 1];
                            Array.Copy(Buffer, 1, ret, 0, Buffer.Length - 1);
                            break;

                        default:
                            ret = Buffer;
                            break;
                    }
                }
            }
            return ret;
        }

        protected override byte[] RefreshPayload()
        {
            byte[] ret = null;
            if (Buffer.Length > 0)
            {
                if (ApiType == ApiTypes.Text)
                {
                    ret = Buffer;
                }
                else if (ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                {
                    ret = Buffer;
                }
                else
                {
                    switch (Buffer[0])
                    {
                        case ZNIFFER_3X_DATA_FRAME_SOF:
                        case ZNIFFER_4X_DATA_FRAME_SOF:
                            if (FrameData.Length > 0)
                            {
                                ret = new byte[FrameData.Length - 1];
                                Array.Copy(FrameData, 1, ret, 0, FrameData.Length - 1);
                            }
                            break;
                        case (byte)CommandTypes.GetDeviceInfo3x:
                            break;
                        case (byte)CommandTypes.SetFrequency3x:
                            if (Buffer.Length > 2)
                                ret = new byte[] { Buffer[2] };
                            break;
                        case (byte)CommandTypes.GetDeviceInfo3xResponse:
                        case (byte)CommandTypes.SetFrequency3xResponse:
                            ret = new byte[Buffer.Length - 1];
                            Array.Copy(Buffer, 1, ret, 0, Buffer.Length - 1);
                            break;

                        case ZNIFFER_4X_COMMAND_FRAME_SOF:
                            if (Buffer.Length > 2)
                            {
                                ret = new byte[Buffer.Length - 3];
                                Array.Copy(Buffer, 3, ret, 0, Buffer.Length - 3);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return ret;
        }

        public static byte[] CreateFrameBuffer(byte[] data)
        {
            byte[] ret;
            byte command = data[0];
            switch (command)
            {
                case (byte)CommandTypes.GetDeviceInfo3x:
                case (byte)CommandTypes.SetFrequency3x:
                case (byte)CommandTypes.GetDeviceInfo3xResponse:
                case (byte)CommandTypes.SetFrequency3xResponse:
                    ret = data;
                    break;
                case (byte)CommandTypes.GetVersion4x:
                case (byte)CommandTypes.SetFrequency4x:
                case (byte)CommandTypes.Start4x:
                case (byte)CommandTypes.Stop4x:
                case (byte)CommandTypes.GetFrequencies4x:
                case (byte)CommandTypes.SetBaudRate4x:
                case (byte)CommandTypes.GetFrequencyStr4x:
                case (byte)CommandTypes.GetLRChConfigs:
                case (byte)CommandTypes.GetLRChConfigStr:
                case (byte)CommandTypes.SetLRChConfig:
                case (byte)CommandTypes.GetLRRegions:
                    ret = new byte[1 + data.Length];
                    ret[0] = ZNIFFER_4X_COMMAND_FRAME_SOF;
                    Array.Copy(data, 0, ret, 1, data.Length);
                    break;
                default:
                    ret = data;
                    break;
            }
            return ret;
        }

        public int AddData(byte[] buffer, int offset, int count)
        {
            int ret = 0;
            if (buffer != null)
            {
                if (count + FrameData.Length < GetMaxLength())
                {
                    if (FrameData != null)
                    {
                        byte[] originData = FrameData;
                        FrameData = new byte[originData.Length + count];
                        Array.Copy(originData, 0, FrameData, 0, originData.Length);
                        Array.Copy(buffer, offset, FrameData, originData.Length, count);
                    }
                    else
                    {
                        FrameData = new byte[count];
                        Array.Copy(buffer, offset, FrameData, 0, count);
                    }
                    ret = count;
                }
            }
            _payload = RefreshPayload();
            _data = RefreshData();
            DataItem.SetData(Payload);
            return ret;
        }
    }
}
