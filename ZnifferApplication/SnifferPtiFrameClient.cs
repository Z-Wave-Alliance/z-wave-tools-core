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
                if (tmpData != null && tmpData.Length > 4)
                {
                    var index = 1;
                    //check data starts with '[' and ends with ']'
                    while (index > 0 && index < tmpData.Length
                        && index + tmpData[index] < tmpData.Length
                        && tmpData[index - 1] == 0x5B && tmpData[index + tmpData[index]] == 0x5D)
                    {
                        var data = new byte[tmpData[index] - 2];
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
                        index += tmpData[index] + 2;
                    }
                }
                else
                {
                    $"!!{tmpData.GetHex()}"._DLOG();
                }
            }
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
