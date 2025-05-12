/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers;
using ZWave.ZnifferApplication.Enums;
using ZWave.Exceptions;
using Utils.Events;
using ZWave.Xml.FrameHeader;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWave.ZnifferApplication
{
    public class ZnifferFrameClient : IFrameClient
    {
        #region CONST
        private const byte MAX_FRAME_SIZE = 255;
        private const byte MIN_FRAME_SIZE = 3;

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
        private const byte ZNIFFER_4X_IS_SOF_WAKEUP_START_MARKER = 0x04;
        private const byte ZNIFFER_4X_IS_SOF_WAKEUP_STOP_MARKER = 0x05;
        private const byte ZNIFFER_4X_IS_SOF_MARKER = 0x01;
        private const byte ZNIFFER_4X_IS_DATA_MARKER = 0x03;
        #endregion

        private readonly Action<DataFrame> mTransmitCallback;
        public ZnifferFrameClient(Action<DataFrame> transmitCallback, FrameDefinition frameDefinition)
        {
            mTransmitCallback = transmitCallback;
            FrameDefinition = frameDefinition;
        }

        public ushort SessionId { get; set; }

        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }

        public FrameDefinition FrameDefinition { get; set; }

        private Frame3xReceiveStates mParser3xState;
        private Frame4xReceiveStates mParser4xState;
        public int ApiVersion { get; set; } // 0 - unknown, 1 - V3, 2 - V4
        public ApiTypes ApiType { get; set; } = ApiTypes.Zniffer;

        readonly Type commandTypesType = typeof(CommandTypes);

        public DataFrame ReceivingDataFrame { get; set; }
        public int ReceivingDataLength { get; set; }
        private Header ReceivingDataFrameHeader;
        private int mReceivingDataLengthCounter;
        private int mReceivingBufferLengthCounter;
        private readonly byte[] mReceivingBuffer = new byte[MAX_FRAME_SIZE];
        private readonly byte[] mReceivingData = new byte[MAX_FRAME_SIZE];


        public void ResetParser()
        {
            mParser3xState = Frame3xReceiveStates.SOF_HUNT;
            mParser4xState = Frame4xReceiveStates.SOF_HUNT;
            ResetReceivingBuffer();
        }

        private int AddToReceivingBuffer(byte buffer)
        {
            if (mReceivingBufferLengthCounter < MAX_FRAME_SIZE)
            {
                mReceivingBuffer[mReceivingBufferLengthCounter] = buffer;
                mReceivingBufferLengthCounter++;
                return 1;
            }
            else return 0;
        }

        private int AddToReceivingData(byte buffer)
        {
            if (mReceivingDataLengthCounter < MAX_FRAME_SIZE)
            {
                mReceivingData[mReceivingDataLengthCounter] = buffer;
                mReceivingDataLengthCounter++;
                return 1;
            }
            else return 0;
        }

        private void ResetReceivingBuffer()
        {
            mReceivingDataLengthCounter = 0;
            mReceivingBufferLengthCounter = 0;
        }

        public bool SendFrames(ActionHandlerResult frameData)
        {
            if (frameData != null && frameData.NextActions != null)
            {
                var sendFrames = frameData.NextActions.Where(x => x is ZnifferApiMessage);
                if (sendFrames.Any())
                {
                    foreach (ZnifferApiMessage item in sendFrames)
                    {
                        byte[] data = CreateBuffer(item);
                        WriteData(data);
                        frameData.Parent.AddTraceLogItem(DateTime.Now, data, true);
                    }
                }
                frameData.NextFramesCompletedCallback?.Invoke(true);
            }
            return true;
        }

        private byte[] CreateBuffer(CommandMessage frame)
        {
            byte[] data = frame.Data;
            byte[] tmp = DataFrame.CreateFrameBuffer(data);
            return tmp;
        }

        private int WriteData(byte[] data)
        {
            if (mTransmitCallback != null)
            {
                DataFrame dataFrame = new DataFrame(SessionId, DataFrameTypes.Data, false, true, DateTime.Now);
                dataFrame.SetBuffer(data, 0, data.Length);
                mTransmitCallback(dataFrame);
            }
            if (SendDataCallback != null)
                return SendDataCallback(data);
            else
                return -1;
        }

        public void HandleData(DataChunk dataChunk, bool isFromFile)
        {
            if (dataChunk.ApiType == ApiTypes.Zniffer || dataChunk.ApiType == ApiTypes.TridentIoTZniffer )
            {
                byte[] data = dataChunk.GetDataBuffer();
                if (data != null && data.Length > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        ParseRawData(data[i], dataChunk.IsOutcome, dataChunk.TimeStamp, false);
                    }
                }
            }
        }

        internal byte[] ParseRawData(byte buffer, bool isOutcome, DateTime timeStamp, bool isFromFile)
        {
            if (AddToReceivingBuffer(buffer) > 0)
            {
                if (mParser3xState != Frame3xReceiveStates.SOF_HUNT && mParser4xState != Frame4xReceiveStates.SOF_HUNT)
                {
                    ResetParser();
                    throw new ZWaveException("Parser state error");
                }

                if (mParser4xState == Frame4xReceiveStates.SOF_HUNT)
                {
                    switch (mParser3xState)
                    {
                        case Frame3xReceiveStates.SOF_HUNT:
                            if (buffer == ZNIFFER_3X_DATA_FRAME_SOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParser3xState = Frame3xReceiveStates.TIME_STAMP_1;
                            }
                            else if (buffer == ZNIFFER_3X_COMMAND_FRAME_SOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParser3xState = Frame3xReceiveStates.CMD_DATA;
                            }
                            else if (buffer == ZNIFFER_3X_COMMAND_49_FRAME_SOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParser3xState = Frame3xReceiveStates.CMD_DATA_49;
                            }
                            else if (buffer == ZNIFFER_3X_COMMAND_46_FRAME_SOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParser3xState = Frame3xReceiveStates.CMD_DATA_46;
                            }
                            else if (buffer == ZNIFFER_3X_DATA_FRAME_EOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                OnFrameReceived();
                            }
                            break;
                        case Frame3xReceiveStates.TIME_STAMP_1:
                            ReceivingDataFrame.DataItem.Systime = (ushort)(buffer << 8);
                            mParser3xState = Frame3xReceiveStates.TIME_STAMP_2;
                            break;
                        case Frame3xReceiveStates.TIME_STAMP_2:
                            ReceivingDataFrame.DataItem.Systime += buffer;
                            mParser3xState = Frame3xReceiveStates.DATA_MARKER;
                            break;
                        case Frame3xReceiveStates.DATA_MARKER:
                            if (buffer == ZNIFFER_3X_DATA_MARKER)
                            {
                                ReceivingDataFrame.DataItem.Speed = buffer;
                                mParser3xState = Frame3xReceiveStates.SPEED;
                            }
                            else
                                mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            break;
                        case Frame3xReceiveStates.SPEED:
                            ReceivingDataFrame.DataItem.Speed = buffer;
                            mParser3xState = Frame3xReceiveStates.DATA_SEPARATOR;
                            break;
                        case Frame3xReceiveStates.DATA_SEPARATOR:
                            if (buffer == ZNIFFER_3X_DATA_SEPARATOR)
                            {
                                mParser3xState = Frame3xReceiveStates.DATA;
                            }
                            else if (buffer == ZNIFFER_3X_DATA_FRAME_EOF)
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                ReceivingDataFrame.AddData(mReceivingData, 0, mReceivingDataLengthCounter);
                                ParseHeaderWithCrc(timeStamp);
                                OnFrameReceived();
                                mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            }
                            else
                                mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            break;
                        case Frame3xReceiveStates.DATA:
                            if (AddToReceivingData(buffer) > 0)
                            {
                                if (buffer == ZNIFFER_3X_IS_WAKEUP_DATA)
                                {
                                    mParser3xState = Frame3xReceiveStates.WAKE_UP_MARKER;
                                }
                                else
                                    mParser3xState = Frame3xReceiveStates.DATA_SEPARATOR;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame3xReceiveStates.WAKE_UP_MARKER:
                            if (buffer == ZNIFFER_3X_IS_WAKEUP_MARKER)
                            {
                                AddToReceivingData(buffer);
                                ReceivingDataFrame.DataItem.WakeupCounter = 1;
                                mParser3xState = Frame3xReceiveStates.WAKE_UP_1;
                            }
                            else if (buffer == ZNIFFER_3X_DATA_SEPARATOR)
                            {
                                mParser3xState = Frame3xReceiveStates.DATA;
                            }
                            break;
                        case Frame3xReceiveStates.WAKE_UP_1:
                            AddToReceivingData(buffer);
                            mParser3xState = Frame3xReceiveStates.DATA_SEPARATOR;
                            break;
                        case Frame3xReceiveStates.CMD_DATA_49:
                            ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                            OnFrameReceived();
                            mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            break;
                        case Frame3xReceiveStates.CMD_DATA_46:
                            mParser3xState = Frame3xReceiveStates.CMD_DATA_46_LEN;
                            break;
                        case Frame3xReceiveStates.CMD_DATA_46_LEN:
                            ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                            OnFrameReceived();
                            mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            break;
                        case Frame3xReceiveStates.CMD_DATA:
                            if (mReceivingBufferLengthCounter == 7)
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                OnFrameReceived();
                                mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            }
                            else if (mReceivingBufferLengthCounter > 7)
                                mParser3xState = Frame3xReceiveStates.SOF_HUNT;
                            break;
                    }
                }
                if (mParser3xState == Frame3xReceiveStates.SOF_HUNT)
                {
                    switch (mParser4xState)
                    {
                        case Frame4xReceiveStates.SOF_HUNT:
                            if (buffer == ZNIFFER_4X_DATA_FRAME_SOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParser4xState = Frame4xReceiveStates.TYPE;
                            }
                            else if (buffer == ZNIFFER_4X_COMMAND_FRAME_SOF)
                            {
                                ResetReceivingBuffer();
                                AddToReceivingBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParser4xState = Frame4xReceiveStates.CMD_TYPE;
                            }
                            break;
                        case Frame4xReceiveStates.TYPE:
                            if (buffer == ZNIFFER_4X_IS_SOF_MARKER)
                                mParser4xState = Frame4xReceiveStates.TIME_STAMP_1;
                            else if (buffer == ZNIFFER_4X_IS_SOF_WAKEUP_START_MARKER)
                            {
                                ReceivingDataFrame.DataItem.WakeUpBeamType = WakeUpBeamTypes.Start;
                                mParser4xState = Frame4xReceiveStates.WAKE_UP_START_TIME_STAMP_1;
                            }
                            else if (buffer == ZNIFFER_4X_IS_SOF_WAKEUP_STOP_MARKER)
                            {
                                ReceivingDataFrame.DataItem.WakeUpBeamType = WakeUpBeamTypes.Stop;
                                mParser4xState = Frame4xReceiveStates.WAKE_UP_STOP_TIME_STAMP_1;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.TIME_STAMP_1:
                            ReceivingDataFrame.DataItem.Systime = (ushort)(buffer << 8);
                            mParser4xState = Frame4xReceiveStates.TIME_STAMP_2;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_TIME_STAMP_1:
                            ReceivingDataFrame.DataItem.Systime = (ushort)(buffer << 8);
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_TIME_STAMP_2;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_STOP_TIME_STAMP_1:
                            ReceivingDataFrame.DataItem.Systime = (ushort)(buffer << 8);
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_STOP_TIME_STAMP_2;
                            break;
                        case Frame4xReceiveStates.TIME_STAMP_2:
                            ReceivingDataFrame.DataItem.Systime += buffer;
                            mParser4xState = Frame4xReceiveStates.SPEED_CHANNEL;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_TIME_STAMP_2:
                            ReceivingDataFrame.DataItem.Systime += buffer;
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_SPEED_CHANNEL;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_STOP_TIME_STAMP_2:
                            ReceivingDataFrame.DataItem.Systime += buffer;
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_STOP_RSSI;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_STOP_RSSI:
                            ReceivingDataFrame.DataItem.Rssi = buffer;
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_STOP_COUNT_1;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_STOP_COUNT_1:
                            AddToReceivingData(buffer);
                            ReceivingDataFrame.DataItem.WakeupCounter = (ushort)(buffer << 8);
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_STOP_COUNT_2;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_STOP_COUNT_2:
                            AddToReceivingData(buffer);
                            ReceivingDataFrame.DataItem.WakeupCounter += buffer;
                            ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                            ReceivingDataFrame.AddData(mReceivingData, 0, mReceivingDataLengthCounter);
                            ParseHeaderWithCrc(timeStamp);//added SRO
                            OnFrameReceived();
                            mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.SPEED_CHANNEL:
                            ReceivingDataFrame.DataItem.Channel = (byte)(buffer >> ZNIFFER_4X_DATA_FRAME_CHANNEL_SHIFT);
                            ReceivingDataFrame.DataItem.Speed = (byte)(buffer & ZNIFFER_4X_DATA_FRAME_SPEED_MASK);
                            mParser4xState = Frame4xReceiveStates.CURRENT_FREQUENCY;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_SPEED_CHANNEL:
                            ReceivingDataFrame.DataItem.Channel = (byte)(buffer >> ZNIFFER_4X_DATA_FRAME_CHANNEL_SHIFT);
                            ReceivingDataFrame.DataItem.Speed = (byte)(buffer & ZNIFFER_4X_DATA_FRAME_SPEED_MASK);
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_CURRENT_FREQUENCY;
                            break;
                        case Frame4xReceiveStates.CURRENT_FREQUENCY:
                            ReceivingDataFrame.DataItem.Frequency = buffer;
                            mParser4xState = Frame4xReceiveStates.RSSI;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_CURRENT_FREQUENCY:
                            ReceivingDataFrame.DataItem.Frequency = buffer;
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_RSSI;
                            break;
                        case Frame4xReceiveStates.RSSI:
                            ReceivingDataFrame.DataItem.Rssi = buffer;
                            mParser4xState = Frame4xReceiveStates.DATA_MARKER;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_RSSI:
                            ReceivingDataFrame.DataItem.Rssi = buffer;
                            ReceivingDataFrame.DataItem.WakeupCounter = 1;
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_BEAM_TYPE;
                            break;
                        case Frame4xReceiveStates.DATA_MARKER:
                            if (buffer == ZNIFFER_4X_DATA_FRAME_SOF)
                            {
                                mParser4xState = Frame4xReceiveStates.SOF_DATA;
                            }
                            else if (ReceivingDataFrame.DataItem.Rssi == ZNIFFER_4X_DATA_FRAME_SOF && buffer == ZNIFFER_4X_IS_DATA_MARKER) //missing RSSI
                            {
                                ReceivingDataFrame.DataItem.Rssi = 0;
                            }
                            else if (ReceivingDataFrame.DataItem.Rssi == ZNIFFER_4X_DATA_FRAME_SOF && buffer == ZNIFFER_4X_IS_WAKEUP_DATA_MARKER) //missing RSSI
                            {
                                ReceivingDataFrame.DataItem.Rssi = 0;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.SOF_DATA:
                            if (buffer == ZNIFFER_4X_IS_DATA_MARKER)
                            {
                                mParser4xState = Frame4xReceiveStates.DATA_LENGTH;
                            }
                            else if (buffer == ZNIFFER_4X_IS_WAKEUP_DATA_MARKER)
                            {
                                mParser4xState = Frame4xReceiveStates.BEAM_TYPE;
                                ReceivingDataFrame.DataItem.WakeupCounter = 1;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.DATA_LENGTH:
                            ReceivingDataLength = buffer;
                            if (ReceivingDataLength == 0)
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                OnFrameReceived();
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.DATA;
                            break;
                        case Frame4xReceiveStates.DATA:
                            AddToReceivingData(buffer);
                            if (mReceivingDataLengthCounter == ReceivingDataLength)
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                ReceivingDataFrame.AddData(mReceivingData, 0, mReceivingDataLengthCounter);
                                ParseHeaderWithCrc(timeStamp);
                                OnFrameReceived();
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            }
                            break;
                        case Frame4xReceiveStates.BEAM_TYPE:
                            AddToReceivingData(buffer);
                            mParser4xState = Frame4xReceiveStates.BEAM_DESTINATION;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_BEAM_TYPE:
                            AddToReceivingData(buffer);
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_BEAM_DESTINATION;
                            break;
                        case Frame4xReceiveStates.BEAM_DESTINATION:
                            AddToReceivingData(buffer);
                            ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                            ReceivingDataFrame.AddData(mReceivingData, 0, mReceivingDataLengthCounter);
                            ParseHeaderWithCrc(timeStamp);
                            OnFrameReceived();
                            mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_BEAM_DESTINATION:
                            AddToReceivingData(buffer);
                            mParser4xState = Frame4xReceiveStates.WAKE_UP_START_BEAM_VERSION;
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_BEAM_VERSION:
                            AddToReceivingData(buffer);
                            if (buffer == 1)
                            {
                                mParser4xState = Frame4xReceiveStates.WAKE_UP_START_HOME_ID_HASH;
                            }
                            else
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                ReceivingDataFrame.AddData(mReceivingData, 0, mReceivingDataLengthCounter);
                                ParseHeaderWithCrc(timeStamp);//added SRO
                                OnFrameReceived();
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            }
                            break;
                        case Frame4xReceiveStates.WAKE_UP_START_HOME_ID_HASH:
                            AddToReceivingData(buffer);
                            ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                            ReceivingDataFrame.AddData(mReceivingData, 0, mReceivingDataLengthCounter);
                            ParseHeaderWithCrc(timeStamp);//added SRO
                            OnFrameReceived();
                            mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.CMD_TYPE:
                            if (Enum.IsDefined(commandTypesType, (int)buffer))
                            {
                                mParser4xState = Frame4xReceiveStates.CMD_LENGTH;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            break;
                        case Frame4xReceiveStates.CMD_LENGTH:
                            ReceivingDataLength = buffer;
                            if (ReceivingDataLength == 0)
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                OnFrameReceived();
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            }
                            else
                                mParser4xState = Frame4xReceiveStates.CMD_DATA;
                            break;
                        case Frame4xReceiveStates.CMD_DATA:
                            if (mReceivingBufferLengthCounter > ReceivingDataLength + 2)
                            {
                                ReceivingDataFrame.SetBuffer(mReceivingBuffer, 0, mReceivingBufferLengthCounter);
                                OnFrameReceived();
                                mParser4xState = Frame4xReceiveStates.SOF_HUNT;
                            }
                            break;
                    }
                }
            }
            else
            {
                ResetParser();
            }
            return null;
        }

        private void OnFrameReceived()
        {
            if (ReceivingDataFrame != null)
            {
                // At the moment, there is only two possible options of serial Zniffer
                // Hence we are only changing 
                if (ReceivingDataFrame.DataItem.ApiType == ApiTypes.Zniffer 
                    || ReceivingDataFrame.DataItem.ApiType == ApiTypes.TridentIoTZniffer)
                {
                    ReceivingDataFrame.ApiType = ApiType;
                    ReceivingDataFrame.DataItem.ApiType = ApiType;
                }
                if (mTransmitCallback != null && ReceivingDataFrame.DataFrameType == DataFrameTypes.Data)
                    mTransmitCallback(ReceivingDataFrame);
                ReceiveFrameCallback?.Invoke(ReceivingDataFrame);
            }
        }

        private void ParseHeaderWithCrc(DateTime timeStamp)
        {
            if (ReceivingDataFrame.IsZnifferDataFrame && FrameDefinition != null)
            {
                ReceivingDataFrame.DataItem.CreatedAt = timeStamp;
                if (ReceivingDataFrame.DataItem != null && ReceivingDataFrame.DataItem.WakeupCounter == 0)
                {
                    bool isCrcOk = false;
                    byte crcBytes = (byte)(ReceivingDataFrame.DataItem.Speed > 1 ? 2 : 1);
                    byte baseHeaderKey = 0;
                    if (ReceivingDataFrame.DataItem.Speed == 3)
                    {
                        baseHeaderKey = 2;
                    }
                    else if (FrameDefinition.FrequencyHeaders.ContainsKey(ReceivingDataFrame.DataItem.Frequency))
                    {
                        baseHeaderKey = FrameDefinition.FrequencyHeaders[ReceivingDataFrame.DataItem.Frequency];
                    }
                    isCrcOk = FrameDefinition.ParseHeaderWithCrc(ReceivingDataFrame.DataItem.DataBuffer, crcBytes, baseHeaderKey, out ReceivingDataFrameHeader);
                    if (!isCrcOk)
                    {
                        ReceivingDataFrame.DataItem.HeaderType = HeaderStore.H_CRC_FALSE;
                    }
                    else
                    {
                        if (ReceivingDataFrameHeader != null)
                        {
                            ReceivingDataFrame.DataItem.HeaderType = ReceivingDataFrameHeader.Key;
                        }
                        else
                        {
                            ReceivingDataFrame.DataItem.HeaderType = HeaderStore.H_UNKNOWN;
                        }
                    }
                }
                else
                {
                    if (ReceivingDataFrame.DataItem.WakeUpBeamType == WakeUpBeamTypes.Start)
                    {
                        ReceivingDataFrame.DataItem.HeaderType = HeaderStore.H_WAKEUP_START;
                    }
                    else if (ReceivingDataFrame.DataItem.WakeUpBeamType == WakeUpBeamTypes.Stop)
                    {
                        ReceivingDataFrame.DataItem.HeaderType = HeaderStore.H_WAKEUP_STOP;
                    }
                    else
                    {
                        ReceivingDataFrame.DataItem.HeaderType = HeaderStore.H_WAKEUP;
                    }
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}
