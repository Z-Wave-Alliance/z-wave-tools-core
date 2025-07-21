/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave;
using ZWave.Layers;
using ZWave.Layers.Frame;
using UicApplication.Data;
using System.Text;

namespace UicApplication
{
    /// <summary>
    /// Needs renaming
    /// </summary>
    public class UicFrameClient : IFrameClient
    {
        public ushort SessionId { get; set; }
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }
        public Func<string, string, bool, int> mqttSendDataCallback_Pub { get; set; }
        public Func<string, int> mqttSendDataCallback_Sub { get; set; }

        public void Dispose()
        {
        }

        public void HandleData(DataChunk dataChunk, bool isFromFile)
        {
            if(dataChunk.ApiType == ApiTypes.Uic)
            {
                string topic = dataChunk.MqttData.topic;
                DataFrame frame = new DataFrame(DataFrameTypes.Publish, dataChunk.TimeStamp);
                if (dataChunk.MqttData.payload != null)
                {
                    string payload = Encoding.UTF8.GetString(dataChunk.MqttData.payload);
                    frame.MqttPayload = payload.Replace("- payload: ", "").Trim(' ');                    
                }
                
                frame.MqttTopic = topic;
                mTransmitCallback?.Invoke(frame);
                ReceiveFrameCallback?.Invoke(frame);
            }
        }

        public void ResetParser()
        {
            throw new NotImplementedException();
        }

        private readonly Action<DataFrame> mTransmitCallback;
        public UicFrameClient(Action<DataFrame> transmitCallback)
        {
            mTransmitCallback = transmitCallback;
        }
        public bool SendFrames(ActionHandlerResult frameData)
        {
            bool ret = false;
            if (frameData != null && frameData.NextActions != null)
            {
                var sendFrames = frameData.NextActions.FindAll(x => x is UicApiMessage);
                if (sendFrames.Any())
                {
                    ret = true;
                    foreach (var frame in sendFrames)
                    {
                        UicApiMessage mqttFrame = (UicApiMessage)frame;
                        if(mqttFrame.IsPublish)
                        {
                            DataFrame dataFrame = new DataFrame(DataFrameTypes.Publish, DateTime.Now);
                            dataFrame.MqttTopic = mqttFrame.Topic;
                            dataFrame.MqttPayload = mqttFrame.Payload;
                            dataFrame.SetBuffer(new byte[] { }, 0);
                            mTransmitCallback(dataFrame);
                            //if (SendDataCallback != null)
                            //    return mqttSendDataCallback(data);
                            mqttSendDataCallback_Pub(mqttFrame.Topic, mqttFrame.Payload, mqttFrame.IsRetain);
                        }
                        else
                        {
                            DataFrame dataFrame = new DataFrame(DataFrameTypes.Subscribe, DateTime.Now);
                            dataFrame.MqttTopic = mqttFrame.Topic;
                            dataFrame.SetBuffer(new byte[] { }, 0);
                            mTransmitCallback(dataFrame);
                            //if (SendDataCallback != null)
                            //    return mqttSendDataCallback(data);
                            mqttSendDataCallback_Sub(mqttFrame.Topic);
                        }
                        frameData.Parent.AddTraceLogItem(DateTime.Now, null, true);
                    }
                    frameData.NextFramesCompletedCallback?.Invoke(ret);
                }
            }
            return ret;
        }

    }
}
