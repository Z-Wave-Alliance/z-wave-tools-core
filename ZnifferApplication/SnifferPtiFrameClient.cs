/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Frame;
using ZWave.Xml.FrameHeader;

namespace ZWave.ZnifferApplication
{
    public class SnifferPtiFrameClient : IFrameClient
    {
        private Action<IDataFrame> transmitCallback;
        private FrameDefinition frameDefinition;
        private readonly List<byte> receivingBuffer = new List<byte>();
        const int MAX_FRAMED_LENGTH = ushort.MaxValue + 3;

        public SnifferPtiFrameClient(Action<IDataFrame> transmitCallback, FrameDefinition frameDefinition)
        {
            this.transmitCallback = transmitCallback;
            this.frameDefinition = frameDefinition;
        }

        public ushort SessionId { get; set; }
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }
        const int DCH_LENGTH_VER2 = 11;
        const int DCH_LENGTH_VER3 = 18;
        public void HandleData(DataChunk dc, bool isFromFile)
        {
            if (dc.ApiType == ApiTypes.Pti)
            {
                byte[] tmpData = dc.GetDataBuffer();
                if (tmpData != null && tmpData.Length > 0)
                {
                    // A frame can be split over several chunks, so parse what is left
                    // of the previous chunk together with the new data.
                    if (receivingBuffer.Count > 0)
                    {
                        receivingBuffer.AddRange(tmpData);
                        tmpData = receivingBuffer.ToArray();
                    }

                    var index = 1;
                    // Parse all complete frames
                    while (index + 1 < tmpData.Length)
                    {
                        // Frame format: "[", <16 bit LE length>, <data>, "]"
                        int frameLength = tmpData[index] | (tmpData[index + 1] << 8);
                        if (frameLength < 3 || tmpData[index - 1] != 0x5B)
                        {
                            index = ScanForNextFrame(tmpData, index);
                            continue;
                        }
                        if (index + frameLength >= tmpData.Length)
                        {
                            // The frame is not complete yet, wait for the next chunk.
                            break;
                        }
                        if (tmpData[index + frameLength] != 0x5D)
                        {
                            index = ScanForNextFrame(tmpData, index);
                            continue;
                        }
                        var data = new byte[frameLength - 2];
                        Array.Copy(tmpData, index + 2, data, 0, data.Length);
                        var dchLength = data[0] == 2 ? DCH_LENGTH_VER2 : (data[0] == 3 ? DCH_LENGTH_VER3 : 0);
                        var apiType = ApiTypes.PtiDiagnostic;
                        // check that data is big enough to contain preamble and postamble
                        if (dchLength > 0 && data.Length > dchLength + 6)
                        {
                            if ((data[dchLength] == 0xF8 && data[data.Length - 6] == 0xF9)
                             || (data[dchLength] == 0xFC && data[data.Length - 5] == 0xFD))
                            {
                                apiType = ApiTypes.Pti;
                            }
                            else if ((data[dchLength] == 0xF8 && data[12] == 0x55) //Beam Tag
                                || (data[dchLength] == 0xFC && data[12] == 0x55))
                            {
                                apiType = ApiTypes.Pti;
                            }
                        }
                        var dataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, false, DateTime.Now);
                        var dataItem = PtiFrameParser.GetDataItem(apiType, DateTime.Now, frameDefinition, SessionId, data);
                        if (dataItem != null)
                        {
                            dataFrame.ApiType = dataItem.ApiType;
                            dataFrame.SetBuffer(data, data.Length);
                            dataFrame.DataItem = dataItem;
                            OnFrameReceived(dataFrame);
                        }
                        index += frameLength + 2;
                    }

                    // Everything before index - 1 was consumed by complete frames,
                    // keep the rest until the chunk carrying its tail arrives.
                    receivingBuffer.Clear();
                    var remaining = tmpData.Length - (index - 1);
                    if (remaining > 0 && remaining <= MAX_FRAMED_LENGTH)
                    {
                        receivingBuffer.AddRange(new ArraySegment<byte>(tmpData, index - 1, remaining));
                    }
                }
                else
                {
                    $"!!{tmpData.GetHex()}"._DLOG();
                }
            }
        }

        /// <summary>
        /// Scans forward from <paramref name="index"/> to find the next 0x5B frame
        /// start marker and returns the position of the length field after it.
        /// Returns <c>tmpData.Length</c> if no marker is found, which ends the loop.
        /// </summary>
        private static int ScanForNextFrame(byte[] tmpData, int index)
        {
            $"!!PTI desync at index {index}: {tmpData.GetHex()}"._DLOG();
            for (int i = index; i < tmpData.Length; i++)
            {
                if (tmpData[i] == 0x5B)
                    return i + 1;
            }
            return tmpData.Length;
        }

        private void OnFrameReceived(DataFrame dataFrame)
        {
            if (dataFrame != null)
            {
                if (transmitCallback != null && dataFrame.DataFrameType == DataFrameTypes.Data)
                    transmitCallback(dataFrame);
                ReceiveFrameCallback?.Invoke(dataFrame);
            }
        }

        public void ResetParser()
        {
            receivingBuffer.Clear();
        }

        public bool SendFrames(ActionHandlerResult frameData)
        {
            return false;
        }

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}
