/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com

//#define MORE_DEBUG_LOGS

using System;
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SetLearnModeEndDeviceOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;

        internal LearnModes Mode { get; private set; }
        internal int TimeoutMs { get; private set; }
        public AssignStatuses AssignStatus { get; set; }
        internal Action<AssignStatuses> AssignStatusCallback { get; set; }
        public SetLearnModeEndDeviceOperation(NetworkViewPoint network, LearnModes mode, Action<AssignStatuses> assignStatusCallback, int timeoutMs)
            : base(false, CommandTypes.CmdZWaveSetLearnMode, true)
        {
            _network = network;
            Mode = mode;
            TimeoutMs = timeoutMs;
            AssignStatusCallback = assignStatusCallback;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        private ApiMessage _setLearnModeStart;
        private ApiMessage _setLearnModeStop;
        private ITimeoutItem _stopCompletedDelay;
        private ITimeoutItem _stopFailedDelay;
        private ITimeoutItem _timeoutItem;

        private ApiHandler _learnReady;
        private ApiHandler _learnNotReady;
        private ApiHandler _assignComplete;
        private ApiHandler _assignNodeIdDone;
        private ApiHandler _assignRangeInfoUpdate;
        private ApiHandler _assignLearnModeCompletedFailed;
        private ApiHandler _assignLearnModeCompletedTimeout;

        private ApiMessage _exploreRequestInclusion;
        private ApiMessage _exploreRequestExclusion;

        private ApiMessage _sendNodeInfo;
        private ApiHandler _sendNodeInfoRetFail;
        private ApiHandler _sendNodeInfoCallback;

        private bool _isAssignCompleteReceived;

#if MORE_DEBUG_LOGS
        private void PrintTimeStampAndName(string method)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " SetLearnModeEndDeviceOperation: " +  method);
        }
#endif

        protected override void CreateWorkflow()
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("CreateWorkflow");
#endif

            var isSysTestEndDevice = _network.Library == Libraries.EndDeviceSysTestLib;
            Action<StartActionUnit> comletedFunc = null;
            if (isSysTestEndDevice)
            {
                comletedFunc = SetStateCompleting;
            }
            if (Mode == LearnModes.LearnModeDisable)
            {
                ActionUnits.Add(new StartActionUnit(SetStateCompleting, TimeoutMs, _setLearnModeStop));
            }
            else if ((Mode & LearnModes.LearnModeSmartStart) == LearnModes.LearnModeSmartStart && (Mode & LearnModes.NetworkMask) == LearnModes.NetworkMask)
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart));
            }
            else if ((Mode & LearnModes.NetworkMask) == LearnModes.NetworkMask && (Mode & LearnModes.LearnModeNWE) == LearnModes.LearnModeNWE) // Smart Start LR exclusion.
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _sendNodeInfo));
            }
            else if ((Mode & LearnModes.LearnModeNWE) == LearnModes.LearnModeNWE)
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _sendNodeInfo, _exploreRequestExclusion, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _exploreRequestExclusion, _timeoutItem));
            }
            else if ((Mode & LearnModes.LearnModeNWI) == LearnModes.LearnModeNWI)
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _sendNodeInfo, _exploreRequestInclusion, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _exploreRequestInclusion, _timeoutItem));
            }
            else if ((Mode & LearnModes.LearnModeSmartStart) == LearnModes.LearnModeSmartStart)
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _exploreRequestInclusion, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _exploreRequestInclusion, _timeoutItem));
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _sendNodeInfo));
            }

            ActionUnits.Add(new DataReceivedUnit(_learnReady, OnLearnReady));
            ActionUnits.Add(new DataReceivedUnit(_assignNodeIdDone, OnNodeIdDone));
            ActionUnits.Add(new DataReceivedUnit(_assignComplete, OnAssignComplete, _setLearnModeStop, _stopCompletedDelay));
            ActionUnits.Add(new TimeElapsedUnit(_stopCompletedDelay, SetStateCompleted, 0, null));
            ActionUnits.Add(new TimeElapsedUnit(_stopFailedDelay, SetStateFailed, 0, null));
            ActionUnits.Add(new DataReceivedUnit(_assignRangeInfoUpdate, OnRangeInfoUpdate));
            ActionUnits.Add(new DataReceivedUnit(_learnNotReady, null, _setLearnModeStop, _stopFailedDelay));
            ActionUnits.Add(new DataReceivedUnit(_assignLearnModeCompletedFailed, null, _setLearnModeStop, _stopFailedDelay));
            ActionUnits.Add(new DataReceivedUnit(_assignLearnModeCompletedTimeout, null, _setLearnModeStop, _stopFailedDelay));

            StopActionUnit = new StopActionUnit(_setLearnModeStop);

        }

        protected override void CreateInstance()
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("CreateInstance");
#endif
            _isAssignCompleteReceived = false;

            Mode = Mode == LearnModes.NodeAnyS2 ? LearnModes.LearnModeClassic : Mode;
            _setLearnModeStart = new ApiMessage(CommandTypes.CmdZWaveSetLearnMode, new byte[] { (byte)Mode });
            _setLearnModeStart.SetSequenceNumber(SequenceNumber);

            _setLearnModeStop = new ApiMessage(CommandTypes.CmdZWaveSetLearnMode, new byte[] { (byte)LearnModes.LearnModeDisable });
            _setLearnModeStop.SetSequenceNumber(0);

            _stopCompletedDelay = new TimeInterval(GetNextCounter(), Id, 200);
            _stopFailedDelay = new TimeInterval(GetNextCounter(), Id, 200);

            _learnReady = new ApiHandler(FrameTypes.Response, CommandTypes.CmdZWaveSetLearnMode);
            _learnReady.AddConditions(new ByteIndex(0x01));

            _learnNotReady = new ApiHandler(FrameTypes.Response, CommandTypes.CmdZWaveSetLearnMode);
            _learnNotReady.AddConditions(new ByteIndex(0x00));

            _assignComplete = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSetLearnMode);
            _assignComplete.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignComplete));

            _assignNodeIdDone = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSetLearnMode);
            _assignNodeIdDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignNodeIdDone));

            _assignRangeInfoUpdate = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSetLearnMode);
            _assignRangeInfoUpdate.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignRangeInfoUpdate));

            _assignLearnModeCompletedFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _assignLearnModeCompletedFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignLearnModeCompletedFailed));

            _assignLearnModeCompletedTimeout = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _assignLearnModeCompletedTimeout.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignLearnModeCompletedTimeout));

            _exploreRequestInclusion = new ApiMessage(CommandTypes.CmdZWaveExploreRequestInclusion);
            _exploreRequestInclusion.SetSequenceNumber(SequenceNumber);
            _exploreRequestInclusion.IsNoAck = true;

            _exploreRequestExclusion = new ApiMessage(CommandTypes.CmdZWaveExploreRequestExclusion);
            _exploreRequestExclusion.SetSequenceNumber(SequenceNumber);
            _exploreRequestExclusion.IsNoAck = true;

            _sendNodeInfo = _network.IsNodeIdBaseTypeLR ?
                new ApiMessage(CommandTypes.CmdZWaveSendNodeInformation, 0x0F, 0xFF, (byte)TransmitOptions.TransmitOptionNone) :
                new ApiMessage(CommandTypes.CmdZWaveSendNodeInformation, 0xFF, (byte)TransmitOptions.TransmitOptionNone);
            _sendNodeInfo.SetSequenceNumber(SequenceNumber);

            _sendNodeInfoRetFail = new ApiHandler(FrameTypes.Response, CommandTypes.CmdZWaveSendNodeInformation);
            _sendNodeInfoRetFail.AddConditions(new ByteIndex(0x00));

            _sendNodeInfoCallback = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSendNodeInformation);
            _sendNodeInfoCallback.AddConditions(ByteIndex.AnyValue, ByteIndex.AnyValue);

            _timeoutItem = new RandomTimeInterval(GetNextCounter(), Id, 2000, 4000);
        }

        private void OnLearnReady(IActionUnit ou)
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("OnLearnReady");
#endif
            if (_isAssignCompleteReceived)
            {
                SetStateCompleted(ou);
            }
        }

        private void OnNodeIdDone(DataReceivedUnit ou)
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("OnNodeIdDone");
#endif
            if (ou.DataFrame.Payload.Length > 2)
            {
                SpecificResult.Node = new NodeTag(ou.DataFrame.Payload[2]);
                if (_network.IsNodeIdBaseTypeLR)
                {
                    if (ou.DataFrame.Payload.Length > 3)
                    {
                        SpecificResult.Node = new NodeTag((ushort)((ou.DataFrame.Payload[2] << 8) + ou.DataFrame.Payload[3]));
                    }
                }
                _network.NodeTag = SpecificResult.Node;
            }
            _network.NodeTag = SpecificResult.Node;
            AssignStatus = (AssignStatuses)ou.DataFrame.Payload[1];
            AssignStatusCallback(AssignStatus);
        }

        private void OnRangeInfoUpdate(DataReceivedUnit ou)
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("OnRangeInfoUpdate");
#endif
            AssignStatus = (AssignStatuses)ou.DataFrame.Payload[1];
            AssignStatusCallback(AssignStatus);
        }

        private void OnAssignComplete(DataReceivedUnit ou)
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("OnAssignComplete");
#endif
            if (ou.DataFrame.Payload.Length > 2)
            {
                SpecificResult.Node = new NodeTag(ou.DataFrame.Payload[2]);
                if (_network.IsNodeIdBaseTypeLR)
                {
                    if (ou.DataFrame.Payload.Length > 3)
                    {
                        SpecificResult.Node = new NodeTag((ushort)((ou.DataFrame.Payload[2] << 8) + ou.DataFrame.Payload[3]));
                    }
                }
                _network.NodeTag = SpecificResult.Node;
            }
            AssignStatus = (AssignStatuses)ou.DataFrame.Payload[1];
            AssignStatusCallback(AssignStatus);
            _isAssignCompleteReceived = true;
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}", SpecificResult.Node.Id);
        }

        public SetLearnModeResult SpecificResult
        {
            get { return (SetLearnModeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetLearnModeResult();
        }
    }

    public class SetLearnModeResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public SecuritySchemes[] SecuritySchemes { get; set; }
        public SubstituteStatuses SubstituteStatus { get; set; }
        public LearnModeStatuses LearnModeStatus { get; set; }
    }

    public enum LearnModeStatuses
    {
        None,
        Added,
        Removed,
        Changed,
        Replicated
    }
}
