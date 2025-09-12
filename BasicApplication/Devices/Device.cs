/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: 2025 Z-Wave Alliance https://z-wavealliance.org
using System;
using System.Linq;
using System.Threading;
using Utils;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Tasks;
using ZWave.Devices;
using ZWave.Exceptions;
using System.Collections.Generic;
using ZWave.Security;
using Utils.Threading;
using System.Collections.ObjectModel;

namespace ZWave.BasicApplication.Devices
{
    public abstract class Device : ApplicationClient, IDevice
    {
        #region Properties
        protected NetworkViewPoint _network;
        public NetworkViewPoint Network
        {
            get { return _network; }
            set
            {
                _network = value;
                Notify("Network");
            }
        }

        private byte[] _dsk;
        public byte[] DSK
        {
            get { return _dsk; }
            set { _dsk = value; }
        }

        // hardcoded prk for devkit without NVR stored prk
        private byte[] _prk = new byte[]
        {
            0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
            0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
        };
        public byte[] PRK
        {
            get { return _prk; }
            set { _prk = value; }
        }

        public ushort Id
        {
            get { return _network.NodeTag.Id; }
            set { _network.NodeTag = new NodeTag(value); }
        }

        /// <summary>
        /// Returns 'HomeId' parameter of the MemoryGetId operation result
        /// </summary>
        public byte[] HomeId
        {
            get { return _network.HomeId; }
            set { _network.HomeId = value; }
        }

        /// <summary>
        /// Returns 'SUCNodeID' parameter of the GetSucNodeId operation result
        /// </summary>
        public ushort SucNodeId
        {
            get { return _network.SucNodeId; }
            set { _network.SucNodeId = value; }
        }

        /// <summary>
        /// Returns 'library type' parameter of the Version operation result or TypeLibrary operation result
        /// </summary>
        public new Libraries Library
        {
            get { return _network.Library; }
            set { _network.Library = value; }
        }

        /// <summary>
        ///  Returns 'capabilities' parameter of the SerialApiGetInitData operation result
        /// </summary>
        public byte SerialApiCapability
        {
            get { return _network.SerialApiCapability; }
            set { _network.SerialApiCapability = value; }
        }

        /// <summary>
        /// Returns 'ver' parameter of the SerialApiGetInitData operation result
        /// </summary>
        public byte SerialApiVersion
        {
            get { return _network.SerialApiVersion; }
            set { _network.SerialApiVersion = value; }
        }

        /// <summary>
        /// Returns 'SERIAL_APPL_VERSION' parameter of the SerialApiGetCapabilities operation result
        /// </summary>
        public byte SerialApplicationVersion
        {
            get { return _network.SerialApplicationVersion; }
            set { _network.SerialApplicationVersion = value; }
        }

        /// <summary>
        /// Returns 'SERIAL_APPL_REVISION' parameter of the SerialApiGetCapabilities operation result
        /// </summary>
        public byte SerialApplicationRevision
        {
            get { return _network.SerialApplicationRevision; }
            set { _network.SerialApplicationRevision = value; }
        }

        /// <summary>
        /// Return Hardware Version from NVR of device. Sets on device open.
        /// </summary>
        public byte HardwareVersion
        {
            get { return _network.HardwareVersion; }
            set { _network.HardwareVersion = value; }
        }

        private Dictionary<string, ActionToken> _runningTasks = new Dictionary<string, ActionToken>();

        #endregion Properties
        internal Device(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc, bool preInitNodes)
            : base(ApiTypes.Basic, sessionId, sc, fc, tc)
        {
            _network = new NetworkViewPoint(Notify, preInitNodes: preInitNodes);
            SessionClient.PostSubstituteAction = action =>
            {
                ActionBase ret = null;
                var type = action.GetType();
                if (type == typeof(SendDataExOperation))
                {
                    if (SupportedSerialApiCommands != null && !SupportedSerialApiCommands.Contains((byte)CommandTypes.CmdZWaveSendDataEx))
                    {
                        var completedCallback = action.CompletedCallback;
                        var token = action.Token;
                        var result = action.Result;
                        var parent = action.ParentAction;
                        var actionId = action.Id;

                        var sendDataExAction = (SendDataExOperation)action;
                        var sendDataSchemeRequested = sendDataExAction.SecurityScheme;
                        var node = sendDataExAction.DstNode;
                        var data = sendDataExAction.Data;
                        var dataDelay = sendDataExAction.DataDelay;
                        var newAction = new SendDataOperation(Network, node, data, sendDataExAction.TxOptions) { DataDelay = dataDelay };
                        newAction.SubstituteSettings = sendDataExAction.SubstituteSettings;

                        ret = newAction;
                        ret.Id = actionId;
                        ret.ParentAction = parent;
                        ret.Token = token;
                        if (ret.CompletedCallback == null)
                        {
                            ret.CompletedCallback = completedCallback;
                        }
                        else
                        {
                            var newCompletedCallback = ret.CompletedCallback;
                            ret.CompletedCallback = (x) =>
                            {
                                newCompletedCallback(x);
                                completedCallback(x);
                            };
                        }
                    }
                }
                return ret;
            };
            SessionClient.ActionStartedCallback = (action) =>
            {
                if (action is DelayedResponseOperation)
                {
                    var responseOperation = (DelayedResponseOperation)action;
                    var key = GenerateDelayedResponseOperationKey(responseOperation);
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (_runningTasks.ContainsKey(key))
                            _runningTasks.Remove(key);
                        _runningTasks.Add(key, responseOperation.Token);
                    }
                }
            };
            SessionClient.ActionCompletedCallback = (action) =>
            {
                if (action is DelayedResponseOperation)
                {
                    var key = GenerateDelayedResponseOperationKey((DelayedResponseOperation)action);
                    if (!string.IsNullOrEmpty(key))
                        _runningTasks.Remove(key);
                }
            };
        }

        public ActionToken GetRunningActionToken(params byte[] bytesToCompare)
        {
            var keySourceOperation = new DelayedResponseOperation(Network, new NodeTag(), new NodeTag(), bytesToCompare, bytesToCompare.Length);
            var key = GenerateDelayedResponseOperationKey(keySourceOperation);
            if (_runningTasks.ContainsKey(key))
            {
                return _runningTasks[key];
            }
            return null;
        }

        private string GenerateDelayedResponseOperationKey(DelayedResponseOperation responseTask)
        {
            var key = string.Empty;
            if (responseTask.DataToCompare?.Length > 0)
                for (int i = 0; i < responseTask.DataToCompare.Length; i++)
                    key += responseTask.DataToCompare[i].ToString();
            return key;
        }

        public bool IsPrimaryController
        {
            get
            {
                return (SerialApiCapability & 0x04) == 0; //Bit 2
            }
        }

        public bool IsEndDeviceApi
        {
            get
            {
                return (SerialApiCapability & 0x01) > 0; //Bit 0
            }
        }

        public bool IsTimerFunctionsFupported
        {
            get
            {
                return (SerialApiCapability & 0x02) > 0; //Bit 1
            }
        }

        /// <summary>
        /// Returns 'background RSSI' parameter of the GetBackgroundRSSI operation result
        /// </summary>
        public byte[] BackgroundRSSILevels { get; set; }
        /// <summary>
        /// Returns 'SERIALAPI_MANUFACTURER_ID' parameter of the SerialApiGetCapabilities operation result
        /// </summary>
        public ushort ManufacturerId { get; set; }
        /// <summary>
        /// Returns 'SERIALAPI_MANUFACTURER_PRODUCT_TYPE' parameter of the SerialApiGetCapabilities operation result
        /// </summary>
        public ushort ManufacturerProductType { get; set; }
        /// <summary>
        /// Returns 'SERIALAPI_MANUFACTURER_PRODUCT_ID' parameter of the SerialApiGetCapabilities operation result
        /// </summary>
        public ushort ManufacturerProductId { get; set; }
        /// <summary>
        /// Returns 'FUNCID_SUPPORTED_BITMASK' parameter of the SerialApiGetCapabilities operation result. Bitmask converted to list.
        /// </summary>
        public byte[] SupportedSerialApiCommands { get; set; }
        /// <summary>
        /// Extended Z-Wave API Setup Supported Sub Commands
        /// </summary>
        public byte[] ExtendedSetupSupportedSubCommands { get; set; }

        SignalSlim _controllerUpdateStatusSignal = new SignalSlim(false);

        SignalSlim _nodeReplaceSignal = new SignalSlim(false);

        SignalSlim nsLearnReadySignal = new SignalSlim(false);
        SignalSlim nodeFoundSignal = new SignalSlim(false);
        SignalSlim addingRemovingEndDeviceSignal = new SignalSlim(false);
        SignalSlim addingRemovingControllerSignal = new SignalSlim(false);
        SignalSlim protocolDoneSignal = new SignalSlim(false);
        SignalSlim doneSignal = new SignalSlim(false);
        SignalSlim failedSignal = new SignalSlim(false);
        SignalSlim notPrimarySignal = new SignalSlim(false);

        SignalSlim assignCompleteSignal = new SignalSlim(false);
        SignalSlim assignNodeIdDoneSignal = new SignalSlim(false);
        SignalSlim assignRangeInfoUpdateSignal = new SignalSlim(false);

        volatile ControllerUpdateStatuses _lastControllerUpdateStatusValue;
        public bool WaitControllerUpdateSignal(ControllerUpdateStatuses updateStatus, int timeout)
        {
            bool ret = false;
            ret = _controllerUpdateStatusSignal.WaitOne(timeout);
            _controllerUpdateStatusSignal.Reset();
            while (_lastControllerUpdateStatusValue != updateStatus)
            {
                ret = _controllerUpdateStatusSignal.WaitOne(timeout);
                _controllerUpdateStatusSignal.Reset();
            }
            return ret;
        }

        public void SetNodeStatusSignal(ControllerUpdateStatuses updateStatus)
        {
            _lastControllerUpdateStatusValue = updateStatus;
            _controllerUpdateStatusSignal.Set();
        }

        public bool WaitFailedNodeStatusSignal(FailedNodeStatuses failedNodeStatus, int timeout)
        {
            bool ret = false;
            switch (failedNodeStatus)
            {
                case FailedNodeStatuses.NodeOk:
                    break;
                case FailedNodeStatuses.NodeReplace:
                    ret = _nodeReplaceSignal.WaitOne(timeout);
                    _nodeReplaceSignal.Reset();
                    break;
                default:
                    break;
            }
            return ret;
        }

        public bool WaitNodeStatusSignal(NodeStatuses nodeStatus, int timeout)
        {
            bool ret = false;
            switch (nodeStatus)
            {
                case NodeStatuses.Unknown:
                    break;
                case NodeStatuses.LearnReady:
                    ret = nsLearnReadySignal.WaitOne(timeout);
                    nsLearnReadySignal.Reset();
                    break;
                case NodeStatuses.NodeFound:
                    ret = nodeFoundSignal.WaitOne(timeout);
                    nodeFoundSignal.Reset();
                    break;
                case NodeStatuses.AddingRemovingEndDevice:
                    ret = addingRemovingEndDeviceSignal.WaitOne(timeout);
                    addingRemovingEndDeviceSignal.Reset();
                    break;
                case NodeStatuses.AddingRemovingController:
                    ret = addingRemovingControllerSignal.WaitOne(timeout);
                    addingRemovingControllerSignal.Reset();
                    break;
                case NodeStatuses.ProtocolDone:
                    ret = protocolDoneSignal.WaitOne(timeout);
                    protocolDoneSignal.Reset();
                    break;
                case NodeStatuses.Done:
                    ret = doneSignal.WaitOne(timeout);
                    doneSignal.Reset();
                    break;
                case NodeStatuses.Failed:
                    ret = failedSignal.WaitOne(timeout);
                    failedSignal.Reset();
                    break;
                case NodeStatuses.NotPrimary:
                    ret = notPrimarySignal.WaitOne(timeout);
                    notPrimarySignal.Reset();
                    break;
                default:
                    break;
            }
            return ret;
        }

        public bool WaitAssignStatusSignal(AssignStatuses assignStatus, int timeout)
        {
            bool ret = false;
            switch (assignStatus)
            {
                case AssignStatuses.AssignComplete:
                    ret = assignCompleteSignal.WaitOne(timeout);
                    assignCompleteSignal.Reset();
                    break;
                case AssignStatuses.AssignNodeIdDone:
                    ret = assignNodeIdDoneSignal.WaitOne(timeout);
                    assignNodeIdDoneSignal.Reset();
                    break;
                case AssignStatuses.AssignRangeInfoUpdate:
                    ret = assignRangeInfoUpdateSignal.WaitOne(timeout);
                    assignRangeInfoUpdateSignal.Reset();
                    break;
                default:
                    break;
            }
            return ret;
        }

        public void SetFailedNodeStatusSignal(FailedNodeStatuses failedNodeStatus)
        {
            switch (failedNodeStatus)
            {
                case FailedNodeStatuses.NodeOk:
                    break;
                case FailedNodeStatuses.NodeReplace:
                    _nodeReplaceSignal.Set();
                    break;
                default:
                    break;
            }
        }

        public void SetNodeStatusSignal(NodeStatuses nodeStatus)
        {
            switch (nodeStatus)
            {
                case NodeStatuses.Unknown:
                    break;
                case NodeStatuses.LearnReady:
                    nsLearnReadySignal.Set();
                    break;
                case NodeStatuses.NodeFound:
                    nodeFoundSignal.Set();
                    break;
                case NodeStatuses.AddingRemovingEndDevice:
                    addingRemovingEndDeviceSignal.Set();
                    break;
                case NodeStatuses.AddingRemovingController:
                    addingRemovingControllerSignal.Set();
                    break;
                case NodeStatuses.ProtocolDone:
                    protocolDoneSignal.Set();
                    break;
                case NodeStatuses.Done:
                    doneSignal.Set();
                    break;
                case NodeStatuses.Failed:
                    failedSignal.Set();
                    break;
                case NodeStatuses.NotPrimary:
                    notPrimarySignal.Set();
                    break;
                default:
                    break;
            }
        }

        public void SetAssignStatusSignal(AssignStatuses assignStatus)
        {
            switch (assignStatus)
            {
                case AssignStatuses.AssignComplete:
                    assignCompleteSignal.Set();
                    break;
                case AssignStatuses.AssignNodeIdDone:
                    assignNodeIdDoneSignal.Set();
                    break;
                case AssignStatuses.AssignRangeInfoUpdate:
                    assignRangeInfoUpdateSignal.Set();
                    break;
                default:
                    break;
            }
        }

        protected void ResetFailedNodeStatusSignals()
        {
            _nodeReplaceSignal.Reset();
        }

        protected void ResetNodeStatusSignals()
        {
            nsLearnReadySignal.Reset();
            nodeFoundSignal.Reset();
            addingRemovingEndDeviceSignal.Reset();
            addingRemovingControllerSignal.Reset();
            protocolDoneSignal.Reset();
            doneSignal.Reset();
            failedSignal.Reset();
            notPrimarySignal.Reset();
        }

        public void ResetAssignStatusSignals()
        {
            assignCompleteSignal.Reset();
            assignNodeIdDoneSignal.Reset();
            assignRangeInfoUpdateSignal.Reset();
        }

        protected void BeforeExecute(ActionBase operation)
        {
            ApiOperation op = (ApiOperation)operation;
            if (SupportedSerialApiCommands != null && op.SerialApiCommands != null)
            {
                foreach (var item in op.SerialApiCommands)
                {
                    if (!SupportedSerialApiCommands.Contains((byte)item))
                        OperationException.Throw(item + "=0x" + ((byte)item).ToString("X2") + " not supported");
                }
            }
        }

        public virtual ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            var actionBase = actionItem as ActionBase;
            if (actionBase != null)
            {
                if (actionBase is SetDefaultOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            ActionResult res = action.Result;
                            if (res)
                            {
                                SessionClient.ResetSubstituteManagers();
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is RequestNodeInfoOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            RequestNodeInfoResult res = (RequestNodeInfoResult)action.Result;
                            if (res)
                            {
                                Network.SetCommandClasses(res.Node, res.CommandClasses);
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is MemoryGetIdOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            MemoryGetIdResult res = (MemoryGetIdResult)action.Result;
                            if (res)
                            {
                                var prevId = Id;
                                var prevHomeId = new byte[4];
                                if (HomeId != null)
                                {
                                    prevHomeId = new byte[HomeId.Length];
                                    Array.Copy(HomeId, prevHomeId, HomeId.Length);
                                }
                                Network.NodeTag = new NodeTag(res.Node.Id);
                                HomeId = res.HomeId;

                                if (prevId != Id || !prevHomeId.SequenceEqual(HomeId))
                                {
                                    SessionClient.ResetSubstituteManagers();
                                }
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is SerialApiGetCapabilitiesOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            SerialApiGetCapabilitiesResult res = (SerialApiGetCapabilitiesResult)action.Result;
                            if (res)
                            {
                                SerialApplicationVersion = res.SerialApplicationVersion;
                                SerialApplicationRevision = res.SerialApplicationRevision;
                                ManufacturerId = res.ManufacturerId;
                                ManufacturerProductType = res.ManufacturerProductType;
                                ManufacturerProductId = res.ManufacturerProductId;
                                SupportedSerialApiCommands = res.SupportedSerialApiCommands;
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is SerialApiGetInitDataOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            SerialApiGetInitDataResult res = (SerialApiGetInitDataResult)action.Result;
                            if (res)
                            {
                                SerialApiVersion = res.SerialApiVersion;
                                SerialApiCapability = res.SerialApiCapability;
                                ChipType = res.ChipType;
                                ChipRevision = res.ChipRevision;
                                if (this.GetType() == typeof(Controller))
                                {
                                    ((Controller)this).IncludedNodes = res.IncludedNodes;
                                }
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is SerialApiGetLRNodesOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            var res = (SerialApiGetLRNodesResult)action.Result;
                            if (res)
                            {
                                if (this.GetType() == typeof(Controller) && res.IncludedNodes != null && res.IncludedNodes.Length > 0)
                                {
                                    ((Controller)this).IncludedNodes = ((Controller)this).IncludedNodes.Concat(res.IncludedNodes).ToArray();
                                }
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is VersionOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            VersionResult res = (VersionResult)action.Result;
                            if (res)
                            {
                                Library = res.Library;
                                Version = res.Version;
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is TypeLibraryOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            TypeLibraryResult res = (TypeLibraryResult)action.Result;
                            if (res)
                            {
                                Library = res.Library;
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is GetBackgroundRssiOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            GetBackgroundRssiResult res = (GetBackgroundRssiResult)action.Result;
                            if (res)
                            {
                                BackgroundRSSILevels = res.BackgroundRSSILevels;
                            }
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else
                {
                    actionBase.CompletedCallback = completedCallback;
                }
                actionBase.Token.LogEntryPointCategory = "Basic";
                actionBase.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            return SessionClient.ExecuteAsync(actionBase);
        }

        public virtual ActionResult Execute(IActionItem actionItem)
        {
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Basic";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            ActionToken token = SessionClient.ExecuteAsync(action);
            ActionResult ret = WaitCompletedSignal(token);

            if (actionItem is SetDefaultOperation)
            {
                ActionResult res = ret;
                if (res)
                {
                    SessionClient.ResetSubstituteManagers();
                }
            }
            else if (actionItem is RequestNodeInfoOperation)
            {
                RequestNodeInfoResult res = (RequestNodeInfoResult)ret;
                if (res)
                {
                    Network.SetCommandClasses(res.Node, res.CommandClasses);
                }
            }
            else if (actionItem is MemoryGetIdOperation)
            {
                MemoryGetIdResult res = (MemoryGetIdResult)ret;
                if (res)
                {
                    var prevId = Id;
                    Network.NodeTag = new NodeTag(res.Node.Id);
                    HomeId = res.HomeId;

                    if (prevId != Id)
                    {
                        SessionClient.ResetSubstituteManagers();
                    }
                }
            }
            else if (actionItem is SerialApiGetCapabilitiesOperation)
            {
                SerialApiGetCapabilitiesResult res = (SerialApiGetCapabilitiesResult)ret;
                if (res)
                {
                    SerialApplicationVersion = res.SerialApplicationVersion;
                    SerialApplicationRevision = res.SerialApplicationRevision;
                    ManufacturerId = res.ManufacturerId;
                    ManufacturerProductType = res.ManufacturerProductType;
                    ManufacturerProductId = res.ManufacturerProductId;
                    SupportedSerialApiCommands = res.SupportedSerialApiCommands;
                }
            }
            else if (actionItem is SerialApiGetInitDataOperation)
            {
                SerialApiGetInitDataResult res = (SerialApiGetInitDataResult)ret;
                if (res)
                {
                    SerialApiVersion = res.SerialApiVersion;
                    SerialApiCapability = res.SerialApiCapability;
                    ChipType = res.ChipType;
                    ChipRevision = res.ChipRevision;
                    if (this is Controller)
                    {
                        ((Controller)this).IncludedNodes = res.IncludedNodes;
                    }
                }
            }
            else if (actionItem is SerialApiGetLRNodesOperation)
            {
                var res = (SerialApiGetLRNodesResult)ret;
                if (res)
                {
                    if (this is Controller && res.IncludedNodes != null && res.IncludedNodes.Length > 0)
                    {
                        ((Controller)this).IncludedNodes = ((Controller)this).IncludedNodes.Concat(res.IncludedNodes).ToArray();
                    }
                }
            }
            else if (actionItem is VersionOperation)
            {
                VersionResult res = (VersionResult)ret;
                if (res)
                {
                    Library = res.Library;
                    Version = res.Version;
                }
            }
            else if (actionItem is TypeLibraryOperation)
            {
                TypeLibraryResult res = (TypeLibraryResult)ret;
                if (res)
                {
                    Library = res.Library;
                }
            }
            else if (actionItem is GetBackgroundRssiOperation)
            {
                GetBackgroundRssiResult res = (GetBackgroundRssiResult)ret;
                if (res)
                {
                    BackgroundRSSILevels = res.BackgroundRSSILevels;
                }
            }
            return ret;
        }

        /// <summary>
        /// Cancels all actionTokens in RunningActions for specified Type
        /// Wrapper around Stop as its name is not clear
        /// </summary>
        /// <param name="type">Action type to cancel</param>
        public void CancelRunningActionsByType(Type type)
        {
            Stop(type);
        }

        /// <summary>
        /// TODO: Remove if unused
        /// </summary>
        /// <param name="type"></param>
        public void Stop(Type type)
        {
            SessionClient.Cancel(type);
        }

        public ActionResult SerialApiSetNodeIdBaseType(byte version)
        {
            if (ChipType > ChipTypes.ZW050x || ChipType == ChipTypes.UNKNOWN)
            {
                return Execute(new SerialApiSetNodeIdBaseTypeOperation(Network, version));
            }
            else
            {
                return null;
            }
        }

        #region SetLearnMode
        public ApiOperation learnModeOperation = null;
        public abstract SetLearnModeResult SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs);
        public abstract ActionToken SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs, Action<IActionItem> completedCallback);
        public abstract SetLearnModeResult SetLearnMode(LearnModes mode, int timeoutMs);
        public abstract ActionToken SetLearnMode(LearnModes mode, int timeoutMs, Action<IActionItem> completedCallback);
        public ActionToken WaitForLearnMode(int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            WaitForLearnModeOperation oper = new WaitForLearnModeOperation(SetAssignStatusSignal, timeoutMs);
            learnModeOperation = oper;
            ret = ExecuteAsync(oper, completedCallback);
            return ret;
        }
        #endregion

        #region SerialApiGetCapabilities

        public SerialApiGetCapabilitiesResult SerialApiGetCapabilities()
        {
            return (SerialApiGetCapabilitiesResult)Execute(new SerialApiGetCapabilitiesOperation(Network));
        }

        public ActionToken SerialApiGetCapabilities(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SerialApiGetCapabilitiesOperation(Network), completedCallback);
        }

        #endregion

        #region SetDefault
        public ActionResult SetDefault()
        {
            return Execute(new SetDefaultOperation());
        }

        public ActionToken SetDefault(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SetDefaultOperation(), completedCallback);
        }
        #endregion

        #region GetPRK

        public NVRGetValueResult GetPRK()
        {
            var ret = (NVRGetValueResult)Execute(new NVRGetValueOperation(0x43, 32));
            if (ret && ret.NVRValue.Length == 32 && !Enumerable.Repeat<byte>(0xFF, ret.NVRValue.Length).SequenceEqual(ret.NVRValue))
            {
                PRK = ret.NVRValue;
            }
            return ret;
        }

        #endregion

        #region MemoryGetId

        public MemoryGetIdResult MemoryGetId()
        {
            return (MemoryGetIdResult)Execute(new MemoryGetIdOperation(Network));
        }

        public ActionToken MemoryGetId(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new MemoryGetIdOperation(Network), completedCallback);
        }

        #endregion

        #region SendDataEx

        public SendDataResult SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, SecuritySchemes scheme)
        {
            return (SendDataResult)Execute(new SendDataExOperation(Network, node, data, txOptions, TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, scheme, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE));
        }

        public SendDataResult SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2)
        {
            return (SendDataResult)Execute(new SendDataExOperation(Network, node, data, txOptions, txSecOptions, scheme, txOptions2));
        }

        public SendDataResult SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2)
        {
            return (SendDataResult)Execute(new SendDataExOperation(Network, node, data, txOptions, txSecOptions, scheme, txOptions2) { SubstituteSettings = substituteSettings });
        }

        public SendDataResult SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, int Timeout)
        {
            return (SendDataResult)Execute(new SendDataExOperation(Network, node, data, txOptions, txSecOptions, scheme, txOptions2, Timeout));
        }

        public ActionToken SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, SecuritySchemes scheme, Action<IActionItem> completedCallback)
        {
            SendDataExOperation operation = new SendDataExOperation(Network, node, data, txOptions, TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, scheme, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionToken SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, Action<IActionItem> completedCallback)
        {
            SendDataExOperation operation = new SendDataExOperation(Network, node, data, txOptions, txSecOptions, scheme, txOptions2);
            return ExecuteAsync(operation, completedCallback);
        }

        public SendDataResult SendDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, out ActionToken token)
        {
            token = SendDataEx(node, data, txOptions, txSecOptions, scheme, txOptions2, null);
            return (SendDataResult)WaitCompletedSignal(token);
        }

        #endregion

        #region SendData

        public SendDataResult SendData(NodeTag node, byte[] data, TransmitOptions txOptions)
        {
            return SendData(node, data, txOptions, new SubstituteSettings());
        }

        public SendDataResult SendData(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, bool isSinglecastFollowup)
        {
            return (SendDataResult)Execute(new SendDataOperation(Network, node, data, txOptions)
            {
                SubstituteSettings = substituteSettings ?? new SubstituteSettings(),
                IsFollowup = isSinglecastFollowup
            });
        }

        public SendDataResult SendData(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings)
        {
            return (SendDataResult)Execute(new SendDataOperation(Network, node, data, txOptions)
            {
                SubstituteSettings = substituteSettings ?? new SubstituteSettings()
            });
        }

        public SendDataResult SendData(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, out ActionToken token)
        {
            var operation = new SendDataOperation(Network, node, data, txOptions)
            {
                SubstituteSettings = substituteSettings ?? new SubstituteSettings()
            };
            token = ExecuteAsync(operation, null);
            return (SendDataResult)WaitCompletedSignal(token);
        }

        public ActionToken SendData(NodeTag node, byte[] data, TransmitOptions txOptions, Action<IActionItem> completedCallback)
        {
            return SendData(node, data, txOptions, null, completedCallback);
        }

        public ActionToken SendData(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, Action<IActionItem> completedCallback)
        {
            SendDataOperation operation = new SendDataOperation(Network, node, data, txOptions)
            {
                SubstituteSettings = substituteSettings ?? new SubstituteSettings()
            };
            return ExecuteAsync(operation, completedCallback);
        }

        #endregion

        #region SendDataMulti

        public TransmitResult SendDataMulti(NodeTag[] nodes, byte[] data, TransmitOptions txOptions)
        {
            return (TransmitResult)(_network.IsBridgeController ?
                Execute(new SendDataMultiBridgeOperation(Network, new NodeTag(Id), nodes, data, txOptions)) :
                Execute(new SendDataMultiOperation(Network, nodes, data, txOptions)));
        }

        public TransmitResult SendDataMulti(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings)
        {
            var token = SendDataMulti(nodes, data, txOptions, substituteSettings, null);
            return (TransmitResult)WaitCompletedSignal(token);
        }

        public TransmitResult SendDataMulti(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, out ActionToken token)
        {
            return SendDataMulti(nodes, data, txOptions, new SubstituteSettings(), out token);
        }

        public TransmitResult SendDataMulti(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, out ActionToken token)
        {
            token = SendDataMulti(nodes, data, txOptions, substituteSettings, null);
            return (TransmitResult)WaitCompletedSignal(token);
        }

        public ActionToken SendDataMulti(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, Action<IActionItem> completedCallback)
        {
            return SendDataMulti(nodes, data, txOptions, new SubstituteSettings(), completedCallback);
        }

        public ActionToken SendDataMulti(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, Action<IActionItem> completedCallback)
        {
            IActionItem sendDataMultiOperation = (_network.IsBridgeController ?
                new SendDataMultiBridgeOperation(Network, new NodeTag(Id), nodes, data, txOptions) { SubstituteSettings = substituteSettings } :
                (IActionItem)new SendDataMultiOperation(Network, nodes, data, txOptions) { SubstituteSettings = substituteSettings });
            return ExecuteAsync(sendDataMultiOperation, completedCallback);
        }

        #endregion

        #region SendDataMultiEx

        public SendDataResult SendDataMultiEx(byte[] data, TransmitOptions txOptions, SecuritySchemes scheme, byte groupId)
        {
            return (SendDataResult)Execute(new SendDataMultiExOperation(data, txOptions, scheme, groupId));
        }

        public SendDataResult SendDataMultiEx(byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, SecuritySchemes scheme, byte groupId)
        {
            return (SendDataResult)Execute(new SendDataMultiExOperation(data, txOptions, scheme, groupId) { SubstituteSettings = substituteSettings });
        }

        public ActionToken SendDataMultiEx(byte[] data, TransmitOptions txOptions, SecuritySchemes scheme, byte groupId, Action<IActionItem> completedCallback)
        {
            SendDataMultiExOperation operation = new SendDataMultiExOperation(data, txOptions, scheme, groupId);
            return ExecuteAsync(operation, completedCallback);
        }

        public SendDataResult SendDataMultiEx(byte[] data, TransmitOptions txOptions, SecuritySchemes scheme, byte groupId, out ActionToken token)
        {
            token = SendDataMultiEx(data, txOptions, scheme, groupId, null);
            return (SendDataResult)WaitCompletedSignal(token);
        }

        #endregion

        #region SendNodeInformation

        public TransmitResult SendNodeInformation(NodeTag destination, TransmitOptions txOptions)
        {
            return (TransmitResult)Execute(new SendNodeInformationOperation(_network, destination, txOptions));
        }

        public ActionToken SendNodeInformation(NodeTag destination, TransmitOptions txOptions, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SendNodeInformationOperation(_network, destination, txOptions), completedCallback);
        }

        #endregion

        #region SetPromiscuousMode

        public ActionResult SetPromiscuousMode(bool state)
        {
            return Execute(new SetPromiscuousModeOperation(state));
        }

        public ActionToken SetPromiscuousMode(bool state, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SetPromiscuousModeOperation(state), completedCallback);
        }

        #endregion

        #region ExploreRequestInclusion

        public ActionResult ExploreRequestInclusion()
        {
            byte learnFuncId = 0;
            if (learnModeOperation != null)
                learnFuncId = learnModeOperation.SequenceNumber;
            return Execute(new ExploreRequestInclusionOperation(learnFuncId));
        }
        public ActionResult ExploreRequestExclusion()
        {
            byte learnFuncId = 0;
            if (learnModeOperation != null)
                learnFuncId = learnModeOperation.SequenceNumber;
            return Execute(new ExploreRequestExclusionOperation(learnFuncId));
        }

        #endregion

        public ActionResult SerialApiSoftReset()
        {
            return Execute(new SerialApiSoftResetOperation());
        }

        public ActionResult SerialApiSetTimeouts(byte rxAckTimeout, byte rxByteTimeout)
        {
            return Execute(new SerialApiSetTimeoutsOperation(rxAckTimeout, rxByteTimeout));
        }

        public ReturnValueResult SetMaxLrTxPower(short value)
        {
            return (ReturnValueResult)Execute(new SetMaxLrTxPowerOperation(Network, value));
        }

        public GetMaxLrTxPowerResult GetMaxLrTxPower()
        {
            return (GetMaxLrTxPowerResult)Execute(new GetMaxLrTxPowerOperation(Network));
        }

        public GetProtocolStatusResult GetProtocolStatus()
        {
            return (GetProtocolStatusResult)Execute(new GetProtocolStatusOperation());
        }

        public GetRandomWordResult GetRandomWord(byte count)
        {
            return (GetRandomWordResult)Execute(new GetRandomWordOperation(count));
        }

        public MemoryGetBufferResult MemoryGetBuffer(ushort offset, byte length)
        {
            return (MemoryGetBufferResult)Execute(new MemoryGetBufferOperation(offset, length));
        }

        public MemoryGetByteResult MemoryGetByte(ushort offset)
        {
            return (MemoryGetByteResult)Execute(new MemoryGetByteOperation(offset));
        }

        public ActionResult MemoryPutBuffer(ushort offset, ushort length, byte[] data)
        {
            return Execute(new MemoryPutBufferOperation(offset, length, data));
        }

        public ActionResult MemoryPutByte(ushort offset, byte data)
        {
            return Execute(new MemoryPutByteOperation(offset, data));
        }

        public RandomResult RandomByte()
        {
            return (RandomResult)Execute(new RandomOperation());
        }

        public ActionToken RequestNetworkUpdate(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RequestNetworkUpdateOperation(), completedCallback);
        }

        public RequestNetworkUpdateResult RequestNetworkUpdate()
        {
            return (RequestNetworkUpdateResult)Execute(new RequestNetworkUpdateOperation());
        }

        public RequestNodeInfoResult RequestNodeInfo(NodeTag node)
        {
            return (RequestNodeInfoResult)Execute(new RequestNodeInfoOperation(_network, node));
        }

        public ActionToken RequestNodeInfo(NodeTag node, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RequestNodeInfoOperation(_network, node), completedCallback);
        }

        public RequestNodeInfoResult RequestNodeInfo(NodeTag node, out ActionToken token)
        {
            token = RequestNodeInfo(node, null);
            return (RequestNodeInfoResult)WaitCompletedSignal(token);
        }

        public NodeInfoResult NodeInfo(NodeTag node)
        {
            return (NodeInfoResult)Execute(new NodeInfoTask(Network, node));
        }

        public ActionToken NodeInfo(NodeTag node, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new NodeInfoTask(Network, node), completedCallback);
        }

        public NodeInfoResult NodeInfo(NodeTag node, out ActionToken token)
        {
            token = NodeInfo(node, null);
            return (NodeInfoResult)WaitCompletedSignal(token);
        }

        public RFPowerLevelGetResult RFPowerLevelGet()
        {
            return (RFPowerLevelGetResult)Execute(new RFPowerLevelGetOperation());
        }

        public ActionResult RFPowerlevelRediscoverySet(byte powerLevel)
        {
            return Execute(new RFPowerlevelRediscoverySetOperation(powerLevel));
        }

        public ActionResult RFPowerLevelSet(byte powerLevel)
        {
            return Execute(new RFPowerLevelSetOperation(powerLevel));
        }

        public ActionResult SendDataAbort()
        {
            return Execute(new SendDataAbortOperation());
        }

        public TransmitResult SendTestFrame(NodeTag nodeId, byte powerLevel)
        {
            return (TransmitResult)Execute(new SendTestFrameOperation(Network, nodeId, powerLevel));
        }

        public ActionToken SendTestFrame(NodeTag nodeId, byte powerLevel, Action<IActionItem> completedCallback)
        {
            SendTestFrameOperation operation = new SendTestFrameOperation(Network, nodeId, powerLevel);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionResult SendTestFrame(NodeTag nodeId, byte powerLevel, out ActionToken token)
        {
            token = SendTestFrame(nodeId, powerLevel, null);
            return WaitCompletedSignal(token);
        }

        public ActionResult ApplicationNodeInformationCmdClasses(byte[] nodeParameters, byte[] unsecureNodeParameters, byte[] secureNodeParameters)
        {
            return Execute(new ApplicationNodeInformationCmdClassesOperation(nodeParameters, unsecureNodeParameters, secureNodeParameters));
            //return Execute(new ActionSerialGroup(new ApplicationNodeInformationCmdClassesOperation(nodeParameters, unsecureNodeParameters, secureNodeParameters),
            //    new DelayOperation(200))
            //{ Name = "ApplicationNodeInformationCmdClassesOperation" });
        }

        public ActionResult ApplicationNodeInformation(bool isListening, byte generic, byte specific, byte[] cmdClasses)
        {
            return Execute(new ApplicationNodeInformationOperation(isListening ? DeviceOptions.Listening : DeviceOptions.NoneListening, generic, specific, cmdClasses));
            //var deviceOptionsListening = isListening ? DeviceOptions.Listening : DeviceOptions.NoneListening;
            //return Execute(new ActionSerialGroup(new ApplicationNodeInformationOperation(deviceOptionsListening, generic, specific, cmdClasses),
            //    new DelayOperation(200))
            //{ Name = "ApplicationNodeInformation" });
        }

        public ActionResult ApplicationNodeInformation(DeviceOptions deviceOptions, byte generic, byte specific, byte[] cmdClasses)
        {
            return Execute(new ApplicationNodeInformationOperation(deviceOptions, generic, specific, cmdClasses));
            //return Execute(new ActionSerialGroup(new ApplicationNodeInformationOperation(deviceOptions, generic, specific, cmdClasses),
            //    new DelayOperation(200))
            //{ Name = "ApplicationNodeInformation" });
        }

        public ActionResult ApplicationUserInput(byte[] UserInputId, byte[] UserInputLocal)
        {
            return Execute(new ApplicationUserInputOperation(UserInputId, UserInputLocal));
        }

        public ActionResult ApplicationGetUserInput()
        {
            return Execute(new ApplicationGetUserInputOperation());
        }

        public SerialApiGetInitDataResult SerialApiGetInitData()
        {
            return (SerialApiGetInitDataResult)Execute(new SerialApiGetInitDataOperation());
        }

        public SerialApiGetInitDataResult SerialApiGetInitData(bool isNoAck)
        {
            return (SerialApiGetInitDataResult)Execute(new SerialApiGetInitDataOperation(isNoAck));
        }

        public ActionToken SerialApiGetInitData(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SerialApiGetInitDataOperation(), completedCallback);
        }

        public SerialApiGetLRNodesResult SerialApiGetLRNodes()
        {
            return (SerialApiGetLRNodesResult)Execute(new SerialApiGetLRNodesOperation(Network));
        }

        public ActionToken SerialApiGetLRNodes(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SerialApiGetLRNodesOperation(Network), completedCallback);
        }

        public ActionResult SetExtIntLevel(byte intSrc, bool triggerLevel)
        {
            return Execute(new SetExtIntLevelOperation(intSrc, triggerLevel));
        }

        public ActionResult SetMaxInclusionRequestIntervals(byte requestintervalSec)
        {
            return Execute(new SetMaxInclusionRequestIntervalsOperation(requestintervalSec));
        }

        #region SetRFReceiveMode
        public ReturnValueResult SetRFReceiveMode(byte mode)
        {
            return (ReturnValueResult)Execute(new SetRFReceiveModeOperation(mode));
        }

        public ActionToken SetRFReceiveMode(byte mode, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SetRFReceiveModeOperation(mode), completedCallback);
        }

        #endregion

        public ActionResult SetSleepMode(SleepModes sleepModes, byte intEnable)
        {
            return Execute(new SetSleepModeOperation(sleepModes, intEnable));
        }

        public ActionResult SetWutTimeout(byte timeoutSec)
        {
            return Execute(new SetWutTimeoutOperation(timeoutSec));
        }

        public ActionResult PowerMgmtStayAwake(byte powerLockType, int awakeTimeout, int wakeupTimeout)
        {
            return Execute(new PowerMgmtStayAwakeOperation(powerLockType, awakeTimeout, wakeupTimeout));
        }

        public ActionResult PowerMgmtCancel(byte powerLockType)
        {
            return Execute(new PowerMgmtCancelOperation(powerLockType));
        }

        public VersionResult GetVersion()
        {
            return (VersionResult)Execute(new VersionOperation(false));
        }

        public VersionResult GetVersion(bool isNoAck)
        {
            return (VersionResult)Execute(new VersionOperation(isNoAck));
        }

        public VersionResult GetVersion(bool isNoAck, int timeoutMs)
        {
            return (VersionResult)Execute(new VersionOperation(isNoAck, timeoutMs));
        }

        public ActionToken GetVersion(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new VersionOperation(), completedCallback);
        }

        public TypeLibraryResult GetTypeLibrary()
        {
            return (TypeLibraryResult)Execute(new TypeLibraryOperation());
        }

        public ActionResult WatchDogDisable()
        {
            return Execute(new WatchDogDisableOperation());
        }

        public ActionResult WatchDogEnable()
        {
            return Execute(new WatchDogEnableOperation());
        }

        public ActionResult WatchDogKick()
        {
            return Execute(new WatchDogKickOperation());
        }

        public ActionResult WatchDogStart()
        {
            return Execute(new WatchDogStartOperation());
        }

        public ActionResult WatchDogStop()
        {
            return Execute(new WatchDogStopOperation());
        }

        public ActionToken SerialApiTest(Action<SerialApiTestResult> receiveCallback, byte testCmd, ushort testDelay, byte testPayloadLength, ushort testCount, TransmitOptions txOptions, NodeTag[] nodes, bool isStopOnErrors, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SerialApiTestOperation(receiveCallback, testCmd, testDelay, testPayloadLength, testCount, txOptions, nodes, isStopOnErrors), completedCallback);
        }

        public SerialApiTestResult SerialApiTest(Action<SerialApiTestResult> receiveCallback, byte testCmd, ushort testDelay, byte testPayloadLength, ushort testCount, TransmitOptions txOptions, NodeTag[] nodes, bool isStopOnErrors)
        {
            return (SerialApiTestResult)Execute(new SerialApiTestOperation(receiveCallback, testCmd, testDelay, testPayloadLength, testCount, txOptions, nodes, isStopOnErrors));
        }

        public ReturnValueResult SerialApiSetup(params byte[] args)
        {
            return (ReturnValueResult)Execute(new SerialApiSetupOperation(args));
        }

        public GetMaxPayloadSizeResult GetMaxPayloadSize()
        {
            return (GetMaxPayloadSizeResult)Execute(new GetMaxPayloadSizeOperation(Network));
        }

        public GetMaxPayloadSizeResult GetMaxLRPayloadSize()
        {
            return (GetMaxPayloadSizeResult)Execute(new GetMaxLRPayloadSizeOperation(Network));
        }

        public GetBackgroundRssiResult GetBackgroundRSSI()
        {
            return (GetBackgroundRssiResult)Execute(new GetBackgroundRssiOperation());
        }

        public ActionToken ExpectControllerUpdate(ControllerUpdateStatuses updateStatus, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectControllerUpdateOperation operation = new ExpectControllerUpdateOperation(Network, updateStatus, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public ExpectDataResult ExpectData(byte[] data, int timeoutMs)
        {
            ExpectDataOperation operation = new ExpectDataOperation(Network, NodeTag.Empty, NodeTag.Empty, data, 2, timeoutMs);
            return (ExpectDataResult)Execute(operation);
        }

        public ActionToken ExpectData(byte[] data, int numberOfBytesToCompare, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectDataOperation operation = new ExpectDataOperation(Network, NodeTag.Empty, NodeTag.Empty, data, numberOfBytesToCompare, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionToken ExpectData(byte[] data, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectDataOperation operation = new ExpectDataOperation(Network, NodeTag.Empty, NodeTag.Empty, data, 2, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionToken ExpectData(NodeTag node, byte[] data, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectDataOperation operation = new ExpectDataOperation(Network, NodeTag.Empty, node, data, 2, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionToken ExpectData(NodeTag node, byte[] data, ExtensionTypes[] extensionTypes, int timeoutMs, Action<IActionItem> completedCallback)
        {
            var dataLen = (data ?? new byte[0]).Length;
            if (dataLen > 2)
            {
                dataLen = 2;
            }
            ExpectDataOperation operation = new ExpectDataOperation(Network, NodeTag.Empty, node, data, dataLen, extensionTypes, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public RequestDataResult RequestData(NodeTag node, byte[] data, TransmitOptions txOptions, byte[] expectData, int timeoutMs)
        {
            return RequestData(NodeTag.Empty, node, data, txOptions, expectData, timeoutMs);
        }

        public RequestDataResult RequestData(NodeTag srcNode, NodeTag node, byte[] data, TransmitOptions txOptions, byte[] expectData, int timeoutMs)
        {
            RequestDataOperation operation = new RequestDataOperation(Network, srcNode, node, data, txOptions, expectData, 2, timeoutMs);
            return (RequestDataResult)Execute(operation);
        }

        public RequestDataResult RequestData(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, byte[] expectData, int timeoutMs)
        {
            return RequestData(NodeTag.Empty, node, data, txOptions, substituteSettings, expectData, timeoutMs);
        }

        public RequestDataResult RequestData(NodeTag srcNode, NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, byte[] expectData, int timeoutMs)
        {
            RequestDataOperation operation = new RequestDataOperation(Network, srcNode, node, data, txOptions, expectData, 2, timeoutMs);
            operation.SubstituteSettings = substituteSettings;
            return (RequestDataResult)Execute(operation);
        }

        public ActionToken RequestData(NodeTag node, byte[] data, TransmitOptions txOptions, byte[] expectData, int timeoutMs, Action<IActionItem> completedCallback)
        {
            RequestDataOperation operation = new RequestDataOperation(Network, NodeTag.Empty, node, data, txOptions, expectData, 2, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public RequestDataResult RequestData(NodeTag node, byte[] data, TransmitOptions txOptions, byte[] expectData, int timeoutMs, out ActionToken token)
        {
            token = RequestData(node, data, txOptions, expectData, timeoutMs, null);
            return (RequestDataResult)WaitCompletedSignal(token);
        }

        public RequestDataResult RequestDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData, int timeoutMs)
        {
            RequestDataExOperation operation = new RequestDataExOperation(Network, NodeTag.Empty, node, data, txOptions, txSecOptions, scheme, txOptions2, expectData[0], expectData[1], timeoutMs);
            return (RequestDataResult)Execute(operation);
        }

        public RequestDataResult RequestDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData, int timeoutMs)
        {
            RequestDataExOperation operation = new RequestDataExOperation(Network, NodeTag.Empty, node, data, txOptions, txSecOptions, scheme, txOptions2, expectData[0], expectData[1], timeoutMs);
            operation.SubstituteSettings = substituteSettings;
            return (RequestDataResult)Execute(operation);
        }

        public ActionToken RequestDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData, int timeoutMs, Action<IActionItem> completedCallback)
        {
            RequestDataExOperation operation = new RequestDataExOperation(Network, NodeTag.Empty, node, data, txOptions, txSecOptions, scheme, txOptions2, expectData[0], expectData[1], timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public RequestDataResult RequestDataEx(NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData, int timeoutMs, out ActionToken token)
        {
            token = RequestDataEx(node, data, txOptions, txSecOptions, scheme, txOptions2, expectData, timeoutMs, null);
            return (RequestDataResult)WaitCompletedSignal(token);
        }

        public ActionToken HandleControllerUpdate(Action<ApplicationControllerUpdateResult> updateCallback)
        {
            return ExecuteAsync(new ApplicationControllerUpdateOperation(Network, updateCallback), null);
        }

        public ActionToken ListenData(ListenDataDelegate listenCallback)
        {
            return ListenData(listenCallback, null);
        }

        public ActionToken ListenData(ListenDataDelegate listenCallback, ByteIndex[] compareData)
        {
            ListenDataOperation operation = new ListenDataOperation(Network, listenCallback, compareData);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ListenDebugData(ListenDebugDataDelegate listenCallback)
        {
            var operation = new ListenDebugDataOperation(listenCallback);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseData(NodeTag dstNode, byte[] data, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(Network, dstNode, data, txOptions, NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseData(byte[] data, TransmitOptions txOptions, byte[] expectData)
        {
            return ResponseData(NodeTag.Empty, data, txOptions, expectData);
        }

        public ActionToken ResponseData(ResponseDataDelegate receiveCallback, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(Network, receiveCallback, txOptions, NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseDataEx(NodeTag dstNode, byte[] data, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, data, txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE,
                dstNode, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseDataEx(byte[] data, TransmitOptions txOptions, byte[] expectData)
        {
            return ResponseDataEx(NodeTag.Empty, data, txOptions, expectData);
        }

        public ActionToken ResponseDataEx(byte[] data, TransmitOptions txOptions, byte[] expectData, int NumBytesToCompare)
        {
            return ResponseDataEx(NodeTag.Empty, data, txOptions, expectData, NumBytesToCompare);
        }

        public ActionToken ResponseDataEx(NodeTag dstNode, byte[] data, TransmitOptions txOptions, byte[] expectData, int NumBytesToCompare)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, data, txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE,
                dstNode, NumBytesToCompare, expectData[0], expectData);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseDataEx(NodeTag dstNode, byte[] data, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, data, txOptions,
                txSecOptions, txOptions2,
                dstNode, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseDataEx(byte[] data, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, byte[] expectData)
        {
            return ResponseDataEx(NodeTag.Empty, data, txOptions, txSecOptions, txOptions2, expectData);
        }

        public ActionToken ResponseDataEx(NodeTag dstNode, byte[] data, TransmitOptions txOptions,
        TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, data, txOptions,
                txSecOptions, scheme, txOptions2,
                dstNode, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseDataEx(byte[] data, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData)
        {
            return ResponseDataEx(NodeTag.Empty, data, txOptions, txSecOptions, scheme, txOptions2, expectData);
        }

        public ActionToken ResponseDataEx(ResponseDataDelegate receiveCallback, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, receiveCallback, txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE,
                NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseDataEx(ResponseDataDelegate receiveCallback, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, receiveCallback, txOptions,
                txSecOptions, scheme, txOptions2,
                NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseMultiDataEx(ResponseExDataDelegate receiveCallback, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, receiveCallback, txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE,
                NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseMultiDataEx(ResponseExDataDelegate receiveCallback, TransmitOptions txOptions, bool isStopOnNak, byte[] expectData)
        {
            ResponseDataExOperation operation = new ResponseDataExOperation(Network, receiveCallback, txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE,
                NodeTag.Empty, isStopOnNak, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseMultiData(ResponseExDataDelegate receiveCallback, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(Network, receiveCallback, txOptions, NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken ResponseData(ResponseAchDataDelegate receiveCallback, TransmitOptions txOptions, byte[] expectData)
        {
            ResponseDataOperation operation = new ResponseDataOperation(Network, receiveCallback, txOptions, NodeTag.Empty, expectData[0], expectData[1]);
            return ExecuteAsync(operation, null);
        }

        public ActionToken NoiseData(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, byte[] expectData, Action<RequestDataResult> requestCallback, int intervalMs, int timeoutMs)
        {
            NoiseDataOperation operation = new NoiseDataOperation(Network, nodes, data, txOptions, expectData[0], expectData[1], requestCallback, intervalMs, timeoutMs);
            return ExecuteAsync(operation, null);
        }

        public ActionToken NoiseData(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, Action<SendDataResult> sendCallback, int intervalMs, int timeoutMs)
        {
            NoiseDataOperation operation = new NoiseDataOperation(Network, nodes, data, txOptions, sendCallback, intervalMs, timeoutMs);
            return ExecuteAsync(operation, null);
        }

        public ActionToken NoiseDataEx(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, byte[] expectData, int intervalMs, int timeoutMs, SecuritySchemes securityScheme, TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2)
        {
            NoiseDataExOperation operation = null;
            if (expectData != null)
                operation = new NoiseDataExOperation(Network, nodes, data, txOptions, expectData[0], expectData[1],
                    intervalMs, timeoutMs, securityScheme, txSecOptions, txOptions2);
            else
                operation = new NoiseDataExOperation(Network, nodes, data, txOptions, intervalMs, timeoutMs, securityScheme, txSecOptions, txOptions2);
            return ExecuteAsync(operation, null);
        }

        public ActionToken NoiseDataEx(NodeTag[] nodes, byte[] data, TransmitOptions txOptions, byte[] expectData, int intervalMs, int timeoutMs,
            SecuritySchemes securityScheme, TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, bool isPoisson)
        {
            NoiseDataExOperation operation = null;
            if (expectData != null)
                operation = new NoiseDataExOperation(Network, nodes, data, txOptions, expectData[0], expectData[1],
                    intervalMs, timeoutMs, securityScheme, txSecOptions, txOptions2, isPoisson);
            else
                operation = new NoiseDataExOperation(Network, nodes, data, txOptions, intervalMs, timeoutMs, securityScheme, txSecOptions, txOptions2, isPoisson);
            return ExecuteAsync(operation, null);
        }

        #region FirmwareUpdateLocal / OTW

        public FirmwareUpdateNvmInitResult FirmwareUpdateNvmInit()
        {
            return (FirmwareUpdateNvmInitResult)Execute(new FirmwareUpdateNvmInitOperation());
        }

        public FirmwareUpdateNvmSetNewImageResult FirmwareUpdateNvmSetNewImage(bool isNewImage)
        {
            return (FirmwareUpdateNvmSetNewImageResult)Execute(new FirmwareUpdateNvmSetNewImageOperation(isNewImage));
        }

        public FirmwareUpdateNvmGetNewImageResult FirmwareUpdateNvmGetNewImage()
        {
            return (FirmwareUpdateNvmGetNewImageResult)Execute(new FirmwareUpdateNvmGetNewImageOperation());
        }

        public FirmwareUpdateNvmUpdateCrc16Result FirmwareUpdateNvmUpdateCrc16(int offset, ushort length, ushort seed)
        {
            return (FirmwareUpdateNvmUpdateCrc16Result)Execute(new FirmwareUpdateNvmUpdateCrc16Operation(offset, length, seed));
        }

        public FirmwareUpdateNvmIsValidCrc16Result FirmwareUpdateNvmIsValidCrc16()
        {
            return (FirmwareUpdateNvmIsValidCrc16Result)Execute(new FirmwareUpdateNvmIsValidCrc16Operation());
        }

        public FirmwareUpdateNvmWriteResult FirmwareUpdateNvmWrite(int offset, ushort length, byte[] buffer)
        {
            return (FirmwareUpdateNvmWriteResult)Execute(new FirmwareUpdateNvmWriteOperation(offset, length, buffer));
        }

        #endregion

        #region NVM Backup/Restore

        public NvmBackupRestoreOpenResult NvmBackupRestoreOpen()
        {
            return (NvmBackupRestoreOpenResult)Execute(new NvmBackupRestoreOpenOperation());
        }

        public NvmBackupRestoreReadResult NvmBackupRestoreRead(byte length, ushort offset)
        {
            return (NvmBackupRestoreReadResult)Execute(new NvmBackupRestoreReadOperation(length, offset));
        }

        public NvmBackupRestoreWriteResult NvmBackupRestoreWrite(byte length, int offset, byte[] data)
        {
            return (NvmBackupRestoreWriteResult)Execute(new NvmBackupRestoreWriteOperation(length, offset, data));
        }

        public ActionResult NvmBackupRestoreClose()
        {
            return Execute(new NvmBackupRestoreCloseOperation());
        }

        #endregion

        #region SoftReset
        public ActionResult SoftReset()
        {
            return Execute(new SoftResetOperation());
        }

        public ActionToken SoftReset(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new SoftResetOperation(), completedCallback);
        }
        #endregion

        public ActionResult SetRoutingMAX(byte maxRouteTries)
        {
            return Execute(new SetRoutingMAXOperation(maxRouteTries));
        }

        public NVRGetValueResult NVRGetValue(byte offset, byte lenght)
        {
            return (NVRGetValueResult)Execute(new NVRGetValueOperation(offset, lenght));
        }

        public ReturnValueResult SetListenBeforeTalkThreshold(byte channel, byte threshhold)
        {
            return (ReturnValueResult)Execute(new SetListenBeforeTalkThresholdOperation(channel, threshhold));
        }

        public ClearNetworkStatsResult ClearNetworkStats()
        {
            return (ClearNetworkStatsResult)Execute(new ClearNetworkStatsOperation());
        }

        public GetNetworkStatsResult GetNetworkStats()
        {
            return (GetNetworkStatsResult)Execute(new GetNetworkStatsOperation());
        }

        public GetTxTimerResult GetTxTimer()
        {
            return (GetTxTimerResult)Execute(new GetTxTimerOperation());
        }

        public ActionResult ClearTxTimers()
        {
            return (ActionResult)Execute(new ClearTxTimersOperation());
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}^{SessionId:00} ({Library})";
        }

        public ActionToken ExpectZW(IEnumerable<ByteIndex[]> filter, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectZWOperation operation = new ExpectZWOperation(filter, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionResult WriteZW(byte[] data)
        {
            WriteZWOperation operation = new WriteZWOperation(data);
            return Execute(operation);
        }

        public ExpectZWResult RequestZW(byte[] data, ByteIndex[] mask, int timeoutMs)
        {
            return (ExpectZWResult)Execute(new RequestZWOperation(data, mask, timeoutMs));
        }

        public GetSupportedSetupSubCommandsResult GetSupportedSetupSubCommands()
        {
            return (GetSupportedSetupSubCommandsResult)Execute(new GetSupportedSetupSubCommandsOperation(Network));
        }

        public DefaultTxPowerLevelGetResult GetDefaultTxPowerLevel()
        {
            return (DefaultTxPowerLevelGetResult)Execute(new GetDefaultTxPowerLevelOperation());
        }

        public DefaultTxPowerLevelGetResult GetDefaultTxPowerLevel16Bit()
        {
            return (DefaultTxPowerLevelGetResult)Execute(new GetDefaultTxPowerLevel16BitOperation());
        }

        public bool SetDefaultTxPowerLevel(sbyte normalTxPower, sbyte measured0dBmPower)
        {
            return Execute(new SetDefaultTxPowerLevelOperation(normalTxPower, measured0dBmPower));
        }

        public bool SetDefaultTxPowerLevel16Bit(short normalTxPower, short measured0dBmPower)
        {
            return Execute(new SetDefaultTxPowerLevel16BitOperation(normalTxPower, measured0dBmPower));
        }

        public RfRegionGetResult GetRfRegion()
        {
            return (RfRegionGetResult)Execute(new GetRfRegionOperation());
        }

        public bool SetRfRegion(RfRegions rfRegion)
        {
            return Execute(new SetRfRegionOperation(rfRegion));
        }

        public DcdcModeGetResult GetDcdcMode()
        {
            return (DcdcModeGetResult)Execute(new GetDcdcModeOperation());
        }

        public ActionResult SetDcdcMode(DcdcModes dcdcMode)
        {
            return Execute(new SetDcdcModeOperation(dcdcMode));
        }

        public ReturnValueResult InitiateShutdown()
        {
            return (ReturnValueResult)Execute(new InitiateShutdownOperation());
        }

        public GetLRChannelResult GetLRChannel()
        {
            return (GetLRChannelResult)Execute(new GetLRChannelOperation());
        }

        public ActionResult SetLRChannel(LongRangeChannels channel)
        {
            return Execute(new SetLRChannelOperation(channel));
        }

        /// <summary>
        /// Not finished yet.
        /// FUNC_ID_GET_RADIO_PTI  0xE8.
        /// </summary>
        /// <returns>true if Radio PTI enabled, otherwise -false.</returns>
        public GetRadioPTIResult IsRadioPTI()
        {
            return (GetRadioPTIResult)Execute(new GetRadioPTIOperation());
        }

        /// <summary>
        /// Execute EnableRadioPTIOperation to change Radio PTI support state.
        /// </summary>
        /// <param name="isEnabled">true to enable Radio PTI support; otherwise, false.</param>
        /// <returns>Operation execution state</returns>
        public ActionResult EnableRadioPTI(bool isEnabled)
        {
            return Execute(new EnableRadioPTIOperation(isEnabled));
        }

        #region Only end device libraries

        public GetSecurityKeysResult GetSecurityKeys()
        {
            return (GetSecurityKeysResult)Execute(new GetSecurityKeysOperation(_network));
        }

        public GetSecurityS2PublicDSKResult GetSecurityS2PublicDSK()
        {
            return (GetSecurityS2PublicDSKResult)Execute(new GetSecurityS2PublicDSKOperation());
        }

        public ReturnValueResult SetSecurityInclusionRequestedKeys(SecuritySchemes[] securitySchemes)
        {
            var operation = new SetSecurityInclusionRequestedKeysOperation(securitySchemes);
            return (ReturnValueResult)Execute(operation);
        }

        public ReturnValueResult SetSecurityInclusionRequestedAuthentication(SecurityAuthentications securityAuthentication)
        {
            var operation = new SetSecurityInclusionRequestedAuthenticationOperation(securityAuthentication);
            return (ReturnValueResult)Execute(operation);
        }

        #endregion
    }
}
