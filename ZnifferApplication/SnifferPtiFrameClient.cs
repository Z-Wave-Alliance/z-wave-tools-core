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
        const byte FRAME_START = 0x5B; // '['
        const byte FRAME_END = 0x5D;   // ']'
        const int MAX_FRAMED_LENGTH = ushort.MaxValue + 3;

        public SnifferPtiFrameClient(Action<IDataFrame> transmitCallback, FrameDefinition frameDefinition)
        {
            this.transmitCallback = transmitCallback;
            this.frameDefinition = frameDefinition;
        }

        public ushort SessionId { get; set; }
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }
        public void HandleData(DataChunk dc, bool isFromFile)
        {
            if (dc.ApiType == ApiTypes.Pti)
            {
                byte[] tmpData = dc.GetDataBuffer();
                if (tmpData != null && tmpData.Length > 0)
                {
                    // Prepend leftover bytes from the previous chunk
                    if (receivingBuffer.Count > 0)
                    {
                        receivingBuffer.AddRange(tmpData);
                        tmpData = receivingBuffer.ToArray();
                    }

                    var index = 1;
                    var desyncCount = 0;
                    var firstDesyncIndex = -1;
                    var hasFrameStart = true;
                    // Parse all complete frames
                    while (index + 1 < tmpData.Length)
                    {
                        // Frame format: "[", <16 bit LE length>, <data>, "]"
                        int frameLength = tmpData[index] | (tmpData[index + 1] << 8);
                        var isDesynchronized = frameLength < 3 || tmpData[index - 1] != FRAME_START;
                        if (!isDesynchronized)
                        {
                            if (index + frameLength >= tmpData.Length)
                            {
                                // Wait only while the visible bytes still look like a real frame start
                                if (IsPlausibleFrameStart(tmpData, index))
                                    break;
                                isDesynchronized = true;
                            }
                            else if (tmpData[index + frameLength] != FRAME_END)
                            {
                                isDesynchronized = true;
                            }
                        }
                        if (isDesynchronized)
                        {
                            if (firstDesyncIndex < 0)
                                firstDesyncIndex = index;
                            desyncCount++;
                            index = ScanForNextFrame(tmpData, index, out hasFrameStart);
                            continue;
                        }
                        var data = new byte[frameLength - 2];
                        Array.Copy(tmpData, index + 2, data, 0, data.Length);
                        var dataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, false, DateTime.Now);
                        var dataItem = PtiFrameParser.GetDataItem(ApiTypes.Pti, DateTime.Now, frameDefinition, SessionId, data);
                        if (dataItem != null)
                        {
                            dataFrame.ApiType = dataItem.ApiType;
                            dataFrame.SetBuffer(data, data.Length);
                            dataFrame.DataItem = dataItem;
                            OnFrameReceived(dataFrame);
                        }
                        index += frameLength + 2;
                    }

                    if (desyncCount > 0)
                    {
                        $"!!PTI desync at index {firstDesyncIndex} of {tmpData.Length}, {desyncCount} candidate(s) skipped"._DLOG();
                    }

                    // Retain the unconsumed tail for the next chunk
                    receivingBuffer.Clear();
                    var remaining = tmpData.Length - (index - 1);
                    if (hasFrameStart && remaining > 0 && remaining <= MAX_FRAMED_LENGTH)
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
        /// Returns whether the visible prefix still looks like a valid DCH frame start.
        /// Bytes beyond the buffer are treated as plausible.
        /// </summary>
        private static bool IsPlausibleFrameStart(byte[] tmpData, int index)
        {
            var versionIndex = index + 2;
            if (versionIndex >= tmpData.Length)
                return true;
            return tmpData[versionIndex] == 2 || tmpData[versionIndex] == 3;
        }

        /// <summary>
        /// Scans forward from <paramref name="index"/> to find the next 0x5B frame
        /// start marker and returns the position of the length field after it.
        /// Sets <paramref name="hasFrameStart"/> to false and returns
        /// <c>tmpData.Length</c> if no marker is found, which ends the loop.
        /// </summary>
        private static int ScanForNextFrame(byte[] tmpData, int index, out bool hasFrameStart)
        {
            for (int i = index; i < tmpData.Length; i++)
            {
                if (tmpData[i] == FRAME_START)
                {
                    hasFrameStart = true;
                    return i + 1;
                }
            }
            hasFrameStart = false;
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
