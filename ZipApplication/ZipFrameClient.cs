/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Frame;
using ZWave.ZipApplication.Data;

namespace ZWave.ZipApplication
{
    public class ZipFrameClient : IFrameClient
    {
        public ushort SessionId { get; set; }

        public bool IsNoAck = false;
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }

        public void ResetParser()
        { }

        private readonly Action<DataFrame> mTransmitCallback;
        public ZipFrameClient(Action<DataFrame> transmitCallback)
        {
            mTransmitCallback = transmitCallback;
        }

        private byte[] CreateBuffer(ZipApiMessage frame)
        {
            byte[] tmp = CreateBufferInner(frame.SequenceNumber, frame.HeaderExtension, frame.Data);
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
            if (dataChunk.ApiType == ApiTypes.Zip)
            {
                byte[] data = dataChunk.GetDataBuffer();
                ParseRawData(data, dataChunk.IsOutcome, dataChunk.TimeStamp, isFromFile);
            }
        }

        private byte[] CreateACK(byte seqNo)
        {
            COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET packet = new COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET();
            packet.properties1.ackResponse = 1;
            packet.seqNo = seqNo;
            return packet;
        }

        internal void ParseRawData(byte[] data, bool isOutcome, DateTime timeStamp, bool isFromFile)
        {
            if (data != null && data.Length > 2)
            {
                DataFrame frame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                frame.SetBuffer(data, 0, data.Length);
                if (data[0] == COMMAND_CLASS_ZIP_V2.ID && data[1] == COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.ID)
                {
                    COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET packet = data;
                    if (!isFromFile && packet.properties1.ackRequest == 1)
                        WriteData(CreateACK(packet.seqNo));

                }
                mTransmitCallback?.Invoke(frame);
                ReceiveFrameCallback?.Invoke(frame);
            }
        }

        #region IFrameClient Members

        public bool SendFrames(ActionHandlerResult frameData)
        {
            bool ret = false;
            if (frameData != null && frameData.NextActions != null)
            {
                var sendFrames = frameData.NextActions.Where(x => x is ZipApiMessage);
                if (sendFrames.Any())
                {
                    ret = true;
                    foreach (var frame in sendFrames)
                    {
                        byte[] tmp = CreateBuffer((ZipApiMessage)frame);
                        int writtenBytes = WriteData(tmp);
                        if (ret && writtenBytes != tmp.Length)
                            ret = false;
                        frameData.Parent.AddTraceLogItem(DateTime.Now, tmp, true);
                    }
                    frameData.NextFramesCompletedCallback?.Invoke(ret);
                }
            }
            return ret;
        }

        #endregion

        private byte[] CreateBufferInner(byte seqNo, byte[] headerExtension, byte[] data)
        {
            byte[] ret = null;
            if (data[0] == COMMAND_CLASS_ZIP_ND.ID)
            {
                ret = data;
            }
            else if (data[0] == COMMAND_CLASS_ZIP_V2.ID && data[1] == COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.ID)
            {
                ret = data;
            }
            else if (data[0] == COMMAND_CLASS_ZIP_V4.ID && data[1] == COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_KEEP_ALIVE.ID)
            {
                ret = data;
            }
            else
            {
                COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET packet = new COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET();
                packet.properties1.ackRequest = 1;               
                packet.properties2.zWaveCmdIncluded = 1;
                packet.properties2.secureOrigin = 1; // Secure origin bit in fifth position
                packet.properties3.sourceEndPoint = 0;
                packet.properties4.destinationEndPoint = 0;
                packet.seqNo = seqNo;
                if (headerExtension != null && headerExtension.Length > 0)
                {
                    packet.properties2.headerExtIncluded = 1;
                    packet.headerLength = (byte)(headerExtension.Length + 1);
                    packet.headerExtension = new List<byte>(headerExtension);
                }
                packet.zWaveCommand = new List<byte>(data);
                ret = packet;
            }
            return ret;
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
