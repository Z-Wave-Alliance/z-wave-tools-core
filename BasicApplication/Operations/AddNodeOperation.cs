/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using ZWave.Enums;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class AddNodeOperation : ApiOperation, IAddRemoveNode
    {
        public bool IsLegacySisAssign { get; set; }
        public static int TIMEOUT = 60000;
        private bool IsModeStopEnabled { get; set; }
        public NodeTag[] NodesBefore { get; set; }
        public byte[] DskValue { get; set; }
        public NetworkKeyS2Flags GrantSchemesValue { get; set; }
        public byte[] Args { get; set; }
        internal Modes InitMode { get; private set; }
        internal int TimeoutMs { get; set; }
        public Action<NodeStatuses> NodeStatusCallback { get; set; }
        public AddNodeOperation(NetworkViewPoint network, Modes mode, Action<NodeStatuses> nodeStatusCallback, int timeoutMs, params byte[] args)
            : this(network, mode, nodeStatusCallback, true, timeoutMs, args)
        { }
        public AddNodeOperation(NetworkViewPoint network, Modes mode, Action<NodeStatuses> nodeStatusCallback, bool isNodeStopEnabled, int timeoutMs, params byte[] args)
           : base(true, CommandTypes.CmdZWaveAddNodeToNetwork, true)
        {
            _network = network;
            IsModeStopEnabled = isNodeStopEnabled;
            IsExclusive = IsModeStopEnabled;
            Args = args;
            InitMode = mode;
            TimeoutMs = timeoutMs;
            NodeStatusCallback = nodeStatusCallback;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        public NodeStatuses NodeStatus { get; set; }

        private SetSucNodeIdOperation actionSetSis;
        private SetSucSelfOperation actionSetSelfSis;

        private ApiMessage messageStart;
        private ApiMessage messageStop;
        private ApiMessage messageStopFailed;
        private ApiMessage messageStopDone;

        private ApiHandler handlerLearnReady;
        private ApiHandler handlerFailed;              // code=7
        private ApiHandler handlerNodeFound;
        protected ApiHandler handlerAddingController;
        protected ApiHandler handlerAddingEndDevice;
        private ApiHandler handlerProtocolDone;        // code=5
        protected ApiHandler handlerDone;                // code=6 the end devices return this callback for messageStop and for messageStopDone
        private ApiHandler handlerNotPrimary;                // code=23

        protected override void CreateWorkflow()
        {
            if ((InitMode & Modes.NodeModeMask) != Modes.NodeStop || (InitMode & Modes.NodeModeMask) != Modes.NodeStopFailed)
            {
                ActionUnits.Add(new StartActionUnit(OnStart, TimeoutMs, messageStart));
                ActionUnits.Add(new DataReceivedUnit(handlerLearnReady, OnLearnReady));
                ActionUnits.Add(new DataReceivedUnit(handlerNodeFound, OnNodeFound));
                ActionUnits.Add(new DataReceivedUnit(handlerAddingController, OnAddingController));
                ActionUnits.Add(new DataReceivedUnit(handlerAddingEndDevice, OnAddingEndDevice));
                if (IsModeStopEnabled)
                {
                    ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnFailed, messageStopDone));
                    ActionUnits.Add(new DataReceivedUnit(handlerProtocolDone, OnProtocolDone,
                        new ActionSerialGroup(actionSetSis, new MessageOperation(messageStop)),
                        new ActionSerialGroup(actionSetSelfSis, new MessageOperation(messageStop)),
                        messageStop));
                    ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone, messageStopDone));
                    ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnNotPrimary, messageStopDone));

                    StopActionUnit = new StopActionUnit(messageStopDone);
                }
                else
                {
                    ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnFailed));
                    ActionUnits.Add(new DataReceivedUnit(handlerProtocolDone, OnDone));
                    ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnNotPrimary));
                }
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, messageStart));
            }
        }

        protected override void CreateInstance()
        {
            if (Args != null && Args.Length > 0)
            {
                byte[] args = new byte[Args.Length + 2];
                args[0] = (byte)InitMode;
                Array.Copy(Args, 0, args, 2, Args.Length);
                messageStart = new ApiMessage(SerialApiCommands[0], args);
                messageStart.SequenceNumberCustomIndex = 3;
            }
            else
            {
                messageStart = new ApiMessage(SerialApiCommands[0], (byte)InitMode);
            }

            if ((InitMode & Modes.NodeModeMask) != Modes.NodeStop || (InitMode & Modes.NodeModeMask) != Modes.NodeStopFailed)
            {
                messageStart.SetSequenceNumber(SequenceNumber);
            }
            else
            {
                messageStart.SetSequenceNumber(0);
            }

            messageStop = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Modes.NodeStop });
            messageStop.SetSequenceNumber(SequenceNumber);

            messageStopFailed = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Modes.NodeStopFailed });
            messageStopFailed.SetSequenceNumber(0); //NULL funcID = 0

            messageStopDone = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Modes.NodeStop });
            messageStopDone.SetSequenceNumber(0); //NULL funcID = 0

            handlerProtocolDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerProtocolDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.ProtocolDone));

            handlerDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Done));

            handlerNodeFound = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNodeFound.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.NodeFound));

            handlerAddingController = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerAddingController.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.AddingRemovingController));

            handlerAddingEndDevice = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerAddingEndDevice.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.AddingRemovingEndDevice));

            handlerLearnReady = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerLearnReady.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.LearnReady));

            handlerFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Failed));

            handlerNotPrimary = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNotPrimary.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.NotPrimary));

            actionSetSis = new SetSucNodeIdOperation(_network, new NodeTag(0x00), 0x01, false, 0x01);
            actionSetSis.IsExclusive = false;
            actionSetSelfSis = new SetSucSelfOperation(_network, new NodeTag(0x01), 0x01, false, 0x01);
            actionSetSelfSis.IsExclusive = false;
        }

        private void OnStart(StartActionUnit ou)
        {
            //Controller.ResetNodeStatusSignals();
        }

        private void OnNodeFound(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnNotPrimary(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateFailing(ou);
        }

        private void OnFailed(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateFailing(ou);
        }

        private void OnAddingController(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnAddingEndDevice(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnLearnReady(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private bool? OnProtocolDone(DataReceivedUnit ou)
        {
            bool? ret = null;
            ParseHandler(ou);
            actionSetSis.Node = new NodeTag(SpecificResult.Id);
            if (IsLegacySisAssign)
            {
                ret = !SpecificResult.IsEndDevice && SpecificResult.Basic == 0x02; // BASIC_TYPE_STATIC_CONTROLLER
            }
            return ret;
        }

        protected virtual void OnDone(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateCompleting(ou);
        }

        protected void ParseHandler(DataReceivedUnit ou)
        {
            byte[] res = ou.DataFrame.Payload;
            SequenceNumber = res[0];
            NodeStatus = (NodeStatuses)res[1];
            if (NodeStatus == NodeStatuses.AddingRemovingController || NodeStatus == NodeStatuses.AddingRemovingEndDevice)
            {
                SpecificResult.FillFromRaw(res, _network.IsNodeIdBaseTypeLR);
                if (NodesBefore != null && NodesBefore.Contains(SpecificResult.Node))
                {
                    SpecificResult.AddRemoveNodeStatus = AddRemoveNodeStatuses.Replicated;
                }
                else
                {
                    SpecificResult.AddRemoveNodeStatus = AddRemoveNodeStatuses.Added;
                }
            }
            NodeStatusCallback(NodeStatus);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}", SpecificResult.Id);
        }

        public AddRemoveNodeResult SpecificResult
        {
            get { return (AddRemoveNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AddRemoveNodeResult();
        }
    }

    public class AddRemoveNodeResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public ushort Id { get { return Node.Id; } }
        public byte Basic { get; set; }
        public byte Generic { get; set; }
        public byte Specific { get; set; }
        public byte[] CommandClasses { get; set; }
        public bool IsEndDevice { get; set; }
        public SubstituteStatuses SubstituteStatus { get; set; }
        public BootstrapingStatuses BootstrapingStatuses { get; set; }
        public byte RoleType { get; set; }
        public byte NodeType { get; set; }
        public byte ZWavePlusVersion { get; set; }
        public AddRemoveNodeStatuses AddRemoveNodeStatus { get; set; }
        public SecuritySchemes[] SecuritySchemes { get; set; }
        public byte[] DSK { get; set; }
        internal bool tmpSkipS0 { get; set; }

        /// <summary>
        /// Fill Specific Result of used operation from the operation call result.
        /// </summary>
        /// <param name="rawData">Raw data array returned by the operation call result (DataFrame.Payload).</param>
        /// <param name="IsNodeIdBaseTypeLR">Is Device with enabled LongRange option.</param>
        public void FillFromRaw(byte[] rawData, bool IsNodeIdBaseTypeLR)
        {
            var NodeStatus = (NodeStatuses)rawData[1];
            if (NodeStatus == NodeStatuses.AddingRemovingEndDevice)
                this.IsEndDevice = true; //set isController bit
            if (IsNodeIdBaseTypeLR)
            {
                if (rawData.Length > 3)
                    this.Node = new NodeTag((ushort)((rawData[2] << 8) + rawData[3]));
                // TODO WA: 
                //if (res.Length > 3)
                //    SpecificResult.Node = new NodeTag((ushort)((0x100) + res[3]));
                if (rawData.Length > 5)
                    this.Basic = rawData[5];
                if (rawData.Length > 6)
                    this.Generic = rawData[6];
                if (rawData.Length > 7)
                    this.Specific = rawData[7];
                if (rawData.Length > 8)
                    this.CommandClasses = rawData.Skip(8).TakeWhile(x => x != 0xEF).ToArray();
            }
            else
            {
                if (rawData.Length > 2)
                    this.Node = new NodeTag(rawData[2]);
                if (rawData.Length > 4)
                    this.Basic = rawData[4];
                if (rawData.Length > 5)
                    this.Generic = rawData[5];
                if (rawData.Length > 6)
                    this.Specific = rawData[6];
                if (rawData.Length > 7)
                    this.CommandClasses = rawData.Skip(7).TakeWhile(x => x != 0xEF).ToArray();
            }
        }
    }

    public interface IAddRemoveNode
    {
        byte[] DskValue { get; set; }
        NetworkKeyS2Flags GrantSchemesValue { get; set; }
        AddRemoveNodeResult SpecificResult { get; }
    }
}
