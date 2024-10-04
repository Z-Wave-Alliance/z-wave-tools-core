/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SetLearnModeControllerOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;

        internal LearnModes Mode { get; set; }
        internal int TimeoutMs { get; set; }
        public NodeStatuses LearnStatus { get; set; }
        internal Action<NodeStatuses> NodeStatusCallback { get; set; }
        public SetLearnModeControllerOperation(NetworkViewPoint network, LearnModes mode, Action<NodeStatuses> nodeStatusCallback, int timeoutMs)
            : base(true, CommandTypes.CmdZWaveSetLearnMode, true)
        {
            _network = network;
            Mode = mode;
            TimeoutMs = timeoutMs;
            NodeStatusCallback = nodeStatusCallback;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        private ApiMessage _setLearnModeStart;
        private ApiMessage _exploreRequestInclusion;
        private ApiMessage _exploreRequestExclusion;
        protected ApiMessage _setLearnModeStop;

        private ApiHandler _setLearnModeStarted;
        private ApiHandler _setLearnModeDone;
        private ApiHandler _setLearnModeFailed;
        private ITimeoutItem _timeoutItem;

        protected override void CreateWorkflow()
        {
            Action<StartActionUnit> comletedFunc = null;
            if (Mode == LearnModes.LearnModeDisable)
            {
                ActionUnits.Add(new StartActionUnit(SetStateCompleting, TimeoutMs, _setLearnModeStop));
            }
            else if ((Mode & (LearnModes.LearnModeSmartStart | LearnModes.NetworkMask)) == (LearnModes.LearnModeSmartStart | LearnModes.NetworkMask))
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart));
            }
            else if ((Mode & LearnModes.NetworkMask) == LearnModes.NetworkMask && (Mode & LearnModes.LearnModeNWE) == LearnModes.LearnModeNWE) // Smart Start LR exclusion.
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart));
            }
            else if ((Mode & LearnModes.LearnModeNWE) == LearnModes.LearnModeNWE)
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _exploreRequestExclusion, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _exploreRequestExclusion, _timeoutItem));
            }
            else if ((Mode & LearnModes.LearnModeNWI) == LearnModes.LearnModeNWI)
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart, _exploreRequestInclusion, _timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, null, 0, _exploreRequestInclusion, _timeoutItem));
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(comletedFunc, TimeoutMs, _setLearnModeStart));
            }
            ActionUnits.Add(new DataReceivedUnit(_setLearnModeStarted, OnStarted));
            ActionUnits.Add(new DataReceivedUnit(_setLearnModeFailed, SetStateFailing, _setLearnModeStop));
            ActionUnits.Add(new DataReceivedUnit(_setLearnModeDone, OnDone, _setLearnModeStop));

            StopActionUnit = new StopActionUnit(_setLearnModeStop);
        }

        protected override void CreateInstance()
        {
            _setLearnModeStart = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Mode });
            _setLearnModeStart.SetSequenceNumber(SequenceNumber);

            _setLearnModeStop = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)LearnModes.LearnModeDisable });
            _setLearnModeStop.SetSequenceNumber(SequenceNumber);

            _exploreRequestInclusion = new ApiMessage(CommandTypes.CmdZWaveExploreRequestInclusion);
            _exploreRequestInclusion.SetSequenceNumber(SequenceNumber);

            _exploreRequestExclusion = new ApiMessage(CommandTypes.CmdZWaveExploreRequestExclusion);
            _exploreRequestExclusion.SetSequenceNumber(SequenceNumber);

            _setLearnModeStarted = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _setLearnModeStarted.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.LearnReady));

            _setLearnModeDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _setLearnModeDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Done));

            _setLearnModeFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _setLearnModeFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Failed));

            _timeoutItem = new RandomTimeInterval(GetNextCounter(), Id, 2000, 4000);
        }

        private void OnStarted(DataReceivedUnit ou)
        {
            LearnStatus = (NodeStatuses)ou.DataFrame.Payload[1];
            NodeStatusCallback(LearnStatus);
        }

        private void OnDone(DataReceivedUnit ou)
        {
            LearnStatus = (NodeStatuses)ou.DataFrame.Payload[1];
            NodeStatusCallback(LearnStatus);
            if (ou.DataFrame.Payload.Length > 2)
            {
                SpecificResult.Node = new NodeTag(ou.DataFrame.Payload[2]);
                if (_network.IsNodeIdBaseTypeLR)
                {
                    if (ou.DataFrame.Payload.Length > 3)
                    {
                        SpecificResult.Node = new NodeTag((byte)((ou.DataFrame.Payload[2] << 8) + ou.DataFrame.Payload[3]));
                    }
                }
                _network.NodeTag = SpecificResult.Node;
            }
            base.SetStateCompleting(ou);
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
}
