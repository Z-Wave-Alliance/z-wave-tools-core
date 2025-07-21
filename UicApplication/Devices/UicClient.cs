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

namespace UicApplication.Devices
{
    /// <summary>
    /// "Unified IoT Controller" client. 
    /// Will hold the infrastructure to perform all the Zip Controller functions over MQTT
    /// </summary>
    public class UicClient : ApplicationClient, IDevice
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

        public List<string> SubscribedTopics = new List<string>();

        internal UicClient(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
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

        public ActionResult Execute(ActionBase action)
        {
            action.Token.LogEntryPointCategory = "UIC";
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            SessionClient.ExecuteAsync(action);
            action.Token.WaitCompletedSignal();
            ActionResult ret = action.Token.Result;
            return ret;
        }
        public void ExecuteAsync(ActionBase action, Action<IActionItem> completedCallback)
        {
            action.CompletedCallback = completedCallback;
            action.Token.LogEntryPointCategory = "UIC";
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            SessionClient.ExecuteAsync(action);
        }

        #region Publish

        public ActionToken Publish(string cluster, string payload,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            PublishOperation op = new PublishOperation(cluster, payload);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken Publish(string cluster, string payload, bool isRetain,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            PublishOperation op = new PublishOperation(cluster, payload, isRetain);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken PublishByUNID(string unid, string cluster, string payload,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            PublishOperation op = new PublishOperation(unid, cluster, payload);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken PublishByUNIDandEndPoint(string unid, string cluster, string payload, string endPoint,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            PublishOperation op = new PublishOperation(unid, endPoint, cluster, payload);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken PublishByGroup(string unid, string cluster, string payload, int group,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            PublishOperation op = new PublishOperation(unid, cluster, payload, group);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken PublishByMachineId(string machId, string cluster, string payload,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            PublishOperation op = new PublishOperation(machId, cluster, payload);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        #endregion

        #region SubsCribe
        public ActionToken SubscribeByUNID(string cluster,
            Action<IActionItem> completedCallback)
        {
            if (!IsSubscribedToTopic(cluster))
            {
                SubscribedTopics.Add(cluster);
            }
            ActionToken ret = null;
            SubscribeOperation op = new SubscribeOperation(UnId, cluster);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }
        public ActionToken SubscribeByUNID(string cluster, string unId,
            Action<IActionItem> completedCallback)
        {
            if (!IsSubscribedToTopic(cluster))
            {
                SubscribedTopics.Add(cluster);
            }
            ActionToken ret = null;
            SubscribeOperation op = new SubscribeOperation(unId, cluster);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken SubscribeByUNIDandEndPoint(string cluster, string unId, string endPoint,
            Action<IActionItem> completedCallback)
        {
            if (!IsSubscribedToTopic(cluster))
            {
                SubscribedTopics.Add(cluster);
            }
            ActionToken ret = null;
            SubscribeOperation op = new SubscribeOperation(unId, cluster, endPoint);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        public ActionToken Subscribe(string cluster,
            Action<IActionItem> completedCallback)
        {
            if (!IsSubscribedToTopic(cluster))
            {
                SubscribedTopics.Add(cluster);
            }
            ActionToken ret = null;
            SubscribeOperation op = new SubscribeOperation(cluster);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);

            return ret;
        }

        #endregion

        #region ExpectData

        public ExpectDataResult ExpectData(string topic, int timeoutMs)
        {
            return (ExpectDataResult)Execute(new ExpectDataOperation(topic, timeoutMs));
        }

        public ExpectDataResult ExpectDataByUNID(string unid, string topic, int timeoutMs)
        {
            return (ExpectDataResult)Execute(new ExpectDataOperation(unid, topic, timeoutMs));
        }

        public ActionToken ExpectData(string topic, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(topic, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ExpectData(string topic, int timeoutMs, int count, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(topic, timeoutMs, count);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ExpectDataByUNID(string unid, string topic, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(unid, topic, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ExpectDataByUNIDAndEp(string unid, string topic, string ep, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(unid, ep, topic, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ExpectDataByEp(string topic, string ep, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(ep, topic, timeoutMs, 1);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ExpectDataByUNIDAndEp(string unid, string topic, string ep, int timeoutMs, int count, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(unid, ep, topic, timeoutMs, count);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ExpectDataByMachineId(string machineId, string topic, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(machineId, topic);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }
        public ActionToken ExpectDataByMqttClient(string mqttclient, string topic, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation("by-mqtt-client", mqttclient, topic);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken ListenData(ListenDataDelegate listenCallback,  string topic, int timeoutMs)
        {
            ActionToken ret = null;
            ListenDataOperation op = new ListenDataOperation(topic, timeoutMs, listenCallback);
            ret = op.Token;
            ExecuteAsync(op, null);
            return ret;
        }

        #endregion

        #region Initialize

        public GetUnIdResult GetUnId()
        {
            GetUnIdResult getUnidRes = (GetUnIdResult)Execute(new GetUnIdOperation());
            return getUnidRes;
        }

        #endregion

        #region NetworkInfo
        public GetStatusResult GetStatus(string unId)
        {
            var getUnidRes = (GetStatusResult)Execute(new GetStatusOperation(unId));
            return getUnidRes;
        }

        public GetStatusResult GetNetworkInfo()
        {
            var getUnidRes = (GetStatusResult)Execute(new GetStatusOperation(null));
            return getUnidRes;
        }
        #endregion

        #region AddNode

        public InclusionResult IncludeNode(string payload, int timeoutMs)
        {
            return (InclusionResult)Execute(new IncludeOperation(payload, timeoutMs));
        }

        public ActionToken IncludeNode(string payload, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            IncludeOperation op = new IncludeOperation(payload, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public InclusionResult IncludeNodeClassic(int timeoutMs)
        {
            return (InclusionResult)Execute(new IncludeClassicOperation(UnId, timeoutMs));
        }

        public ActionToken IncludeNodeClassic(int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            IncludeClassicOperation op = new IncludeClassicOperation(UnId, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public InclusionResult InputPin(string dsk, int timeoutMs)
        {
            return (InclusionResult)Execute(new InputPinOperation(UnId, dsk, timeoutMs));
        }

        public ActionToken InputPin(string dsk, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            InputPinOperation op = new InputPinOperation(UnId, dsk, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        #endregion

        #region ExcludeNode
        public ExclusionResult ExcludNode(string unId, List<string> payload, int timeoutMs)
        {
            return (ExclusionResult)Execute(new ExclusionOperation(unId, payload, timeoutMs));
        }

        public ActionToken ExcludNode(string unId, List<string> removeUnId, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExclusionOperation op = new ExclusionOperation(unId, removeUnId, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }
        #endregion

        #region Request

        public RequestDataResult RequestData(string cluster, string payload, string exptectTopic)
        {
            return (RequestDataResult)Execute(new RequestDataOperation(UnId, cluster, payload, exptectTopic));
        }

        public RequestDataResult RequestData(string unId, string cluster, string payload, string exptectTopic)
        {
            return (RequestDataResult)Execute(new RequestDataOperation(unId, cluster, payload, exptectTopic));
        }

        public ActionToken RequestData(string unId, string cluster, string payload, string exptectTopic, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            RequestDataOperation op = new RequestDataOperation(unId, cluster, payload, exptectTopic);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken RequestData(string unId, string cluster, string exptectTopic, Action<IActionItem> completedCallback)
        {
            return RequestData(unId, cluster, exptectTopic, 0, completedCallback);
        }

        public ActionToken RequestData(string unId, string cluster, string exptectTopic, int timeout, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            RequestDataOperation op = new RequestDataOperation(unId, cluster, exptectTopic);
            if (timeout > 0)
            {
                op._timeout = timeout;
            }
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken RequestPlainData(string unId, string cluster, string payload, string expectedTopic, int timeout, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            RequestPlainDataOperation op = new RequestPlainDataOperation(unId, cluster, payload, expectedTopic, timeout);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        #endregion

        #region Failed Remove

        public ExclusionResult FailedRemove(string unId, string payload, int timeoutMs)
        {
            return (ExclusionResult)Execute(new FailedRemoveOperation(unId, payload, timeoutMs));
        }

        public ActionToken FailedRemove(string unId, string removeUnId, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            FailedRemoveOperation op = new FailedRemoveOperation(unId, removeUnId, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        #endregion

        public bool IsSubscribedToTopic(string topic)
        {
            return SubscribedTopics.Contains(topic);
        }

        public SetIdleResult SetIdle(string unId, int timeoutMs)
        {
            return (SetIdleResult)Execute(new SetIdleOperation(unId, timeoutMs));
        }
        public SetDefaultResult SetDefault(string unId, int timeoutMs)
        {
            return (SetDefaultResult)Execute(new SetDefaultOperation(unId, timeoutMs));
        }
        public ActionToken SetDefault(string unid, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SetDefaultOperation op = new SetDefaultOperation(unid, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }
        public GetNodesResult GetNodes(int timeoutMs)
        {
            return (GetNodesResult)Execute(new GetNodesOperation(UnId, timeoutMs));
        }
        public ActionToken GetNodes(int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            GetNodesOperation op = new GetNodesOperation(UnId, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }
    }
}
