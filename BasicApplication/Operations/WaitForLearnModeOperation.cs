/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    public class WaitForLearnModeOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;
        internal int TimeoutMs { get; private set; }
        public AssignStatuses AssignStatus { get; set; }
        internal Action<AssignStatuses> AssignStatusCallback { get; set; }
        public WaitForLearnModeOperation(Action<AssignStatuses> assignStatusCallback, int timeoutMs)
            : base(true, CommandTypes.CmdZWaveSetLearnMode, true)
        {
            TimeoutMs = timeoutMs;
            AssignStatusCallback = assignStatusCallback;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        private ApiMessage _setLearnModeStart;
        private ApiMessage _setLearnModeStop;
        private ITimeoutItem _stopDelay;

        private ApiHandler _learnReady;
        private ApiHandler _assignComplete;
        private ApiHandler _assignNodeIdDone;
        private ApiHandler _assignRangeInfoUpdate;

        private ApiMessage _exploreRequestInclusion;
        private ApiMessage _exploreRequestExclusion;

        private ApiMessage _sendNodeInfo;
        private ApiHandler _sendNodeInfoRetFail;
        private ApiHandler _sendNodeInfoCallback;

        private bool _isAssignCompleteReceived;

        protected override void CreateWorkflow()
        {

            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            ActionUnits.Add(new DataReceivedUnit(_assignComplete, OnAssignComplete, _setLearnModeStop, _stopDelay));
            ActionUnits.Add(new TimeElapsedUnit(_stopDelay, SetStateCompleted, 0, null));
            ActionUnits.Add(new DataReceivedUnit(_assignRangeInfoUpdate, OnRangeInfoUpdate));

        }

        protected override void CreateInstance()
        {
            _isAssignCompleteReceived = false;

            _setLearnModeStop = new ApiMessage(CommandTypes.CmdZWaveSetLearnMode, new byte[] { (byte)LearnModes.LearnModeDisable });
            _setLearnModeStop.SetSequenceNumber(0);

            _stopDelay = new TimeInterval(GetNextCounter(), Id, 200);

            _learnReady = new ApiHandler(FrameTypes.Response, CommandTypes.CmdZWaveSetLearnMode);
            _learnReady.AddConditions(new ByteIndex(0x01));

            _assignComplete = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSetLearnMode);
            _assignComplete.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignComplete));

            _assignNodeIdDone = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSetLearnMode);
            _assignNodeIdDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignNodeIdDone));

            _assignRangeInfoUpdate = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSetLearnMode);
            _assignRangeInfoUpdate.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignRangeInfoUpdate));

            _exploreRequestInclusion = new ApiMessage(CommandTypes.CmdZWaveExploreRequestInclusion);
            _exploreRequestInclusion.SetSequenceNumber(SequenceNumber);
            _exploreRequestInclusion.IsNoAck = true;

            _exploreRequestExclusion = new ApiMessage(CommandTypes.CmdZWaveExploreRequestExclusion);
            _exploreRequestExclusion.SetSequenceNumber(SequenceNumber);
            _exploreRequestExclusion.IsNoAck = true;

            _sendNodeInfo = new ApiMessage(CommandTypes.CmdZWaveSendNodeInformation, 0xFF, (byte)TransmitOptions.TransmitOptionNone);
            _sendNodeInfo.SetSequenceNumber(SequenceNumber);

            _sendNodeInfoRetFail = new ApiHandler(FrameTypes.Response, CommandTypes.CmdZWaveSendNodeInformation);
            _sendNodeInfoRetFail.AddConditions(new ByteIndex(0x00));

            _sendNodeInfoCallback = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveSendNodeInformation);
            _sendNodeInfoCallback.AddConditions(ByteIndex.AnyValue, ByteIndex.AnyValue);
        }

        private void OnLearnReady(IActionUnit ou)
        {
            if (_isAssignCompleteReceived)
            {
                SetStateCompleted(ou);
            }
        }

        private void OnNodeIdDone(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload.Length > 2)
            {
                //SpecificResult.NodeId = ou.DataFrame.Payload[2];
            }
            AssignStatus = (AssignStatuses)ou.DataFrame.Payload[1];
            AssignStatusCallback(AssignStatus);
        }

        private void OnRangeInfoUpdate(DataReceivedUnit ou)
        {
            AssignStatus = (AssignStatuses)ou.DataFrame.Payload[1];
            AssignStatusCallback(AssignStatus);
        }

        private void OnAssignComplete(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload.Length > 2)
            {
                //SpecificResult.NodeId = ou.DataFrame.Payload[2];
            }
            AssignStatus = (AssignStatuses)ou.DataFrame.Payload[1];
            AssignStatusCallback(AssignStatus);
            _isAssignCompleteReceived = true;
        }

        public override string AboutMe()
        {
            return string.Format("Id=null");
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
}
