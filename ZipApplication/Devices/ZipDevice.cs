/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Net;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.ZipApplication.Operations;

namespace ZWave.ZipApplication.Devices
{
    public class ZipDevice : ApplicationClient, IDevice
    {
        public delegate List<byte[]> ResponseExDataDelegate(ReceiveStatuses options, NodeTag destNodeId, NodeTag srcNodeId, byte[] data);
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

        public bool IsUnsolicited { get; set; }

        public byte[] DSK { get; set; }

        public string AppVersion { get; set; }

        public IPAddress IpAddress
        {
            get
            {
                var ds = DataSource as SocketDataSource;
                if (ds != null && IPAddress.TryParse(ds.SourceName, out IPAddress ret))
                {
                    return ret;
                }
                return null;
            }
            set { (DataSource as SocketDataSource).SourceName = value.ToString(); }
        }

        public int Port
        {
            get { return (DataSource as SocketDataSource).Port; }
            set { (DataSource as SocketDataSource).Port = value; }
        }

        public ushort Id
        {
            get { return _network.NodeTag.Id; }
            set { _network.NodeTag = new NodeTag(value); }
        }

        public byte ControllerId { get; set; }

        public byte[] HomeId
        {
            get { return _network.HomeId; }
            set { _network.HomeId = value; }
        }

        public NodeTag[] IncludedNodes { get; set; }

        public byte LastCommandStatus { get; set; }

        public ushort SucNodeId { get; set; }

        internal ZipDevice(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(ApiTypes.Zip, sessionId, sc, fc, tc)
        {
            _network = new NetworkViewPoint(Notify, true);
        }

        public RequestNodeNeighborUpdateResult RequestNodeNeighborUpdate(NodeTag node, int timeoutMs)
        {
            RequestNodeNeighborUpdateResult ret = null;
            RequestNodeNeighborUpdateOperation op = new RequestNodeNeighborUpdateOperation(node, timeoutMs);
            ret = (RequestNodeNeighborUpdateResult)Execute(op);
            return ret;
        }

        public ActionToken RequestNodeNeighborUpdate(NodeTag node, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            RequestNodeNeighborUpdateOperation op = new RequestNodeNeighborUpdateOperation(node, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public SendDataResult SendNodeInformation(byte[] headerExtension, byte nodeId, TransmitOptions txOptions)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NODE_INFORMATION_SEND cmd = new COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NODE_INFORMATION_SEND();
            cmd.txOptions = (byte)txOptions;
            cmd.destinationNodeId = nodeId;
            return SendData(headerExtension, cmd);
        }

        public ActionToken SendNodeInformation(byte[] headerExtension, byte nodeId, TransmitOptions txOptions, Action<IActionItem> completedCallback)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NODE_INFORMATION_SEND cmd = new COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NODE_INFORMATION_SEND();
            cmd.txOptions = (byte)txOptions;
            cmd.destinationNodeId = nodeId;
            return SendData(headerExtension, cmd, completedCallback);
        }

        public GetVersionResult GetVersion()
        {
            GetVersionResult ret = (GetVersionResult)Execute(new GetVersionOperation());
            Library = ret.Library;
            Version = ret.Firmware0Version;
            return ret;
        }

        public GetIdResult GetId()
        {
            GetIdResult ret = (GetIdResult)Execute(new GetIdOperation(IpAddress));
            Network.NodeTag = new NodeTag(ret.NodeId);
            HomeId = ret.HomeId;
            return ret;
        }

        public GetIdResult GetId(IPAddress ipAddress)
        {
            GetIdResult ret = (GetIdResult)Execute(new GetIdOperation(ipAddress));
            Network.NodeTag = new NodeTag(ret.NodeId);
            HomeId = ret.HomeId;
            return ret;
        }

        public GetIpAddressResult GetIpAddress(NodeTag node)
        {
            GetIpAddressResult ret = null;
            GetIpAddressOperation op = new GetIpAddressOperation(node);
            ret = (GetIpAddressResult)Execute(op);
            //IpAddress = ret.IpAddress;
            return ret;
        }

        public ActionToken SetDefault(int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SetDefaultOperation op = new SetDefaultOperation(timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public SetDefaultResult SetDefault(int timeoutMs)
        {
            SetDefaultResult ret = null;
            SetDefaultOperation op = new SetDefaultOperation(timeoutMs);
            ret = (SetDefaultResult)Execute(op);
            return ret;
        }

        #region SendData

        public ActionToken SendData(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo, int timeout,
            Action<IActionItem> completedCallback, bool isNoAck)
        {
            ActionToken ret = null;
            SendDataOperation op = new SendDataOperation(headerExtension, data, isValidateAckSeqNo, timeout, isNoAck);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ActionToken SendData(byte[] headerExtension, byte[] data, Action<IActionItem> completedCallback)
        {
            return SendData(headerExtension, data, true, null);
        }

        public ActionToken SendData(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo,
            Action<IActionItem> completedCallback)
        {
            return SendData(headerExtension, data, isValidateAckSeqNo, 0, null);
        }

        public ActionToken SendData(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo, int timeout,
            Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SendDataOperation op = new SendDataOperation(headerExtension, data, isValidateAckSeqNo, timeout);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public SendDataResult SendData(byte[] headerExtension, byte[] data)
        {
            return SendData(headerExtension, data, true);
        }

        public SendDataResult SendData(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo)
        {
            return SendData(headerExtension, data, isValidateAckSeqNo, 0);
        }

        public SendDataResult SendData(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo, int timeout)
        {
            SendDataResult ret = null;
            SendDataOperation op = new SendDataOperation(headerExtension, data, isValidateAckSeqNo, timeout);
            ret = (SendDataResult)Execute(op);
            return ret;
        }

        #endregion

        #region RequestData

        public RequestDataResult RequestData(byte[] headerExtension, byte[] data, byte[] expectedData, int timeoutMs)
        {
            return (RequestDataResult)Execute(new RequestDataOperation(headerExtension, data, expectedData[0], expectedData[1], timeoutMs));
        }

        public ActionToken RequestData(byte[] headerExtension, byte[] data, byte[] expectedData, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            RequestDataOperation op = new RequestDataOperation(headerExtension, data, expectedData[0], expectedData[1], timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }   

        #endregion

        #region ExpectData

        public ExpectDataResult ExpectData(byte[] expectedData, int timeoutMs)
        {
            return (ExpectDataResult)Execute(new ExpectDataOperation(expectedData[0], expectedData[1], timeoutMs));
        }

        public ActionToken ExpectData(byte[] expectedData, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExpectData(expectedData, timeoutMs, 0, completedCallback);
        }

        public ActionToken ExpectData(byte[] expectedData, int timeoutMs, int hitCount, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ExpectDataOperation op = new ExpectDataOperation(expectedData[0], expectedData[1], timeoutMs, hitCount);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        #endregion

        public void Stop(Type taskType)
        {
            SessionClient.Cancel(taskType);
        }

        public void ExecuteAsync(ActionBase action, Action<IActionItem> completedCallback)
        {
            action.CompletedCallback = completedCallback;
            action.Token.LogEntryPointCategory = "Z/IP";
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            SessionClient.ExecuteAsync(action);
        }

        public ActionResult Execute(ActionBase action)
        {
            action.Token.LogEntryPointCategory = "Z/IP";
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            SessionClient.ExecuteAsync(action);
            action.Token.WaitCompletedSignal();
            ActionResult ret = action.Token.Result;
            if (action is GetVersionOperation)
            {
                GetVersionResult res = (GetVersionResult)ret;
                if (res)
                {
                    Library = res.Library;
                    Version = res.Firmware0Version;
                }
            }
            else if (action is GetIdOperation)
            {
                GetIdResult res = (GetIdResult)ret;
                if (res)
                {
                    Network.NodeTag = new NodeTag(res.NodeId);
                    HomeId = res.HomeId;
                }
            }
            return ret;
        }

        public ActionToken ResponseData(byte[] headerExtension, byte[] data, byte[] expectedData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(headerExtension, data, expectedData[0], expectedData[1]);
            ExecuteAsync(operation, null);
            return operation.Token;
        }

        public ActionToken ResponseData(byte[] headerExtension, byte[] data, byte[] expectedData, int numBytesToCompare)
        {
            ResponseDataOperation operation = new ResponseDataOperation(headerExtension, data, expectedData, expectedData[0], expectedData[1], numBytesToCompare);
            ExecuteAsync(operation, null);
            return operation.Token;
        }

        public ActionToken ResponseData(byte[] headerExtension, Func<byte[], byte[], byte[]> receiveCallback, byte[] expectData, bool isNoAck)
        {
            ResponseDataOperation operation = new ResponseDataOperation(headerExtension, receiveCallback, expectData[0], expectData[1], isNoAck);
            ExecuteAsync(operation, null);
            return operation.Token;
        }

        public ActionToken ResponseData(byte[] headerExtension, Func<byte[], byte[], byte[]> receiveCallback, byte[] expectData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(headerExtension, receiveCallback, expectData[0], expectData[1]);
            ExecuteAsync(operation, null);
            return operation.Token;
        }

        public ActionToken ResponseMultiData(byte[] headerExtension, Func<ActionToken, byte[], byte[], List<byte[]>> receiveCallback, byte[] expectData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(headerExtension, receiveCallback, expectData[0], expectData[1]);
            ExecuteAsync(operation, null);
            return operation.Token;
        }       

        public ActionToken NoiseData(byte[] headerExtension, byte[] data, byte[] expectData, int intervalMs, int timeoutMs)
        {
            NoiseDataOperation operation = null;
            if (expectData != null)
                operation = new NoiseDataOperation(headerExtension, data, expectData[0], expectData[1], intervalMs, timeoutMs);
            else
                operation = new NoiseDataOperation(headerExtension, data, intervalMs);
            ExecuteAsync(operation, null);
            return operation.Token;
        }
    }
}
