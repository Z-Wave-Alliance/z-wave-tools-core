/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Net;
using ZWave;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.UicApplication;
using UicApplication.Operations;
using Utils;
using System;
using ZWave.UicApplication.Layers;
using ZWave.TextApplication;

namespace UicApplication.Devices
{
    /// <summary>
    /// This is a abstracted UIC end device type.
    /// I am considering to write it as a inheritable main type in the future.
    /// For now is just for ZigBee devices
    /// </summary>
    public class UicDevice : TextDevice
    {
        private NetworkViewPoint _network;
        public NetworkViewPoint Network
        {
            get { return _network; }
            set
            {
                _network = value;
                Notify("Network");
            }
        }

        public ushort Id
        {
            get { return _network.NodeTag.Id; }
            set { _network.NodeTag = new NodeTag(value); }
        }
        public byte[] HomeId
        {
            get { return _network.HomeId; }
            set { _network.HomeId = value; }
        }
        public ushort SucNodeId { get; set; }
        public string UnId { get; set; }

        //public List<string> SubscribedTopics = new List<string>();

        internal UicDevice(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(ApiTypes.Uic, sessionId, sc, fc, tc)
        {
            BindUicLayers(sc, fc, tc);
        }
        private void BindUicLayers(ISessionClient sc, IFrameClient fc, ITransportClient tc)
        {
            if (tc is IUicTransportClient)
            {
                (fc as UicFrameClient).mqttSendDataCallback_Pub = (tc as IUicTransportClient).PublishMessage;
                (fc as UicFrameClient).mqttSendDataCallback_Sub = (tc as IUicTransportClient).SubscribeTopic;
            }
        }
        public ActionToken SetDefault(Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SetDefaultDeviceOperation op = new SetDefaultDeviceOperation();
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }
        public ActionToken SetLearnMode(Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SetLearnMode op = new SetLearnMode();
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }
    }        
}
