/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;
using System.Collections.Generic;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// 
    /// only use 8 bit for nodeID (regardless of basetype)
    /// </summary>
    public class SetVirtualDeviceLearnModeOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;

        internal VirtualDeviceLearnModes Mode { get; private set; }
        internal NodeTag Node { get; private set; }
        private readonly byte _txOptions;
        internal int TimeoutMs { get; private set; }
        internal Action<AssignStatuses> AssignStatusCallback { get; private set; }

        public SetVirtualDeviceLearnModeOperation(NetworkViewPoint network, NodeTag node, VirtualDeviceLearnModes mode, Action<AssignStatuses> assignStatusCallback, TransmitOptions txOptions, int timeoutMs)
            : base(true, new CommandTypes[] { CommandTypes.CmdZWaveSetVirtualDeviceLearnMode, CommandTypes.CmdZWaveSendVirtualDeviceNodeInfo }, true)
        {
            _network = network;
            Node = node;
            Mode = mode;
            AssignStatusCallback = assignStatusCallback;
            _txOptions = (byte)txOptions;
            TimeoutMs = timeoutMs;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        public SetVirtualDeviceLearnModeOperation(NodeTag node, VirtualDeviceLearnModes mode, Action<AssignStatuses> assignStatusCallback, int timeoutMs)
            : this(null, node, mode, assignStatusCallback, TransmitOptions.TransmitOptionNone, timeoutMs)
        {
        }

        public SetVirtualDeviceLearnModeOperation(NodeTag node, VirtualDeviceLearnModes mode, Action<AssignStatuses> assignStatusCallback, TransmitOptions txOptions, int timeoutMs)
            : this(null, node, mode, assignStatusCallback, txOptions, timeoutMs)
        {
        }

        public SetVirtualDeviceLearnModeOperation(NetworkViewPoint network, NodeTag node, VirtualDeviceLearnModes mode, Action<AssignStatuses> assignStatusCallback, int timeoutMs)
            : this(network, node, mode, assignStatusCallback, TransmitOptions.TransmitOptionNone, timeoutMs)
        {
        }

        private ApiMessage _cmStart;
        private ApiMessage _cmStop;
        private ApiHandler _chOk;
        private ApiHandler _chFailed;

        private ApiHandler _chAssignComplete;
        private ApiHandler _chAssignNodeIdDone;
        private ApiHandler _chAssignRangeInfoUpdate;

        private ApiMessage cmEndDeviceNodeInfo;

        protected override void CreateWorkflow()
        {
            if (Mode == VirtualDeviceLearnModes.Add || Mode == VirtualDeviceLearnModes.Remove)
            {
                ActionUnits.Add(new StartActionUnit(OnStart, 5000, _cmStart));
                ActionUnits.Add(new DataReceivedUnit(_chOk, null, TimeoutMs));
                ActionUnits.Add(new DataReceivedUnit(_chFailed, SetStateFailed));
                ActionUnits.Add(new DataReceivedUnit(_chAssignNodeIdDone, OnAssignComplete));
            }
            else if (Mode == VirtualDeviceLearnModes.Disable)
            {
                ActionUnits.Add(new StartActionUnit(null, 5000, _cmStop));
                ActionUnits.Add(new DataReceivedUnit(_chOk, SetStateCompleted));
                ActionUnits.Add(new DataReceivedUnit(_chFailed, SetStateFailed));
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(OnStart, 5000, _cmStart));
                ActionUnits.Add(new DataReceivedUnit(_chOk, null, TimeoutMs, cmEndDeviceNodeInfo));
                ActionUnits.Add(new DataReceivedUnit(_chFailed, SetStateFailed));
                ActionUnits.Add(new DataReceivedUnit(_chAssignComplete, OnAssignComplete));
                ActionUnits.Add(new DataReceivedUnit(_chAssignNodeIdDone, OnAssignNodeIdDone));
            }
        }

        protected override void CreateInstance()
        {
            var inputParams = new List<byte>(new byte[] { (byte)Node.Id });

            _cmStart = new ApiMessage(SerialApiCommands[0], inputParams.Concat(new[] { (byte)Mode }).ToArray());
            _cmStart.SetSequenceNumber(SequenceNumber);

            _cmStop = new ApiMessage(SerialApiCommands[0], inputParams.Concat(new[] { (byte)VirtualDeviceLearnModes.Disable }).ToArray());
            _cmStop.SetSequenceNumber(SequenceNumber);

            cmEndDeviceNodeInfo = new ApiMessage(SerialApiCommands[1], inputParams.Concat(new[] { (byte)0xFF, _txOptions }).ToArray());
            cmEndDeviceNodeInfo.SetSequenceNumber(SequenceNumber);

            _chOk = new ApiHandler(SerialApiCommands[0]);
            _chOk.AddConditions(new ByteIndex(0x01));

            _chFailed = new ApiHandler(SerialApiCommands[0]);
            _chFailed.AddConditions(new ByteIndex(0x00));

            _chAssignComplete = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _chAssignComplete.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignComplete));

            _chAssignNodeIdDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _chAssignNodeIdDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignNodeIdDone));

            _chAssignRangeInfoUpdate = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            _chAssignRangeInfoUpdate.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)AssignStatuses.AssignRangeInfoUpdate));
        }

        private void OnStart(StartActionUnit ou)
        {

        }

        private void OnAssignNodeIdDone(DataReceivedUnit ou)
        {
            AssignNodeTag(ou.DataFrame.Payload, 3);
        }

        private void OnAssignComplete(DataReceivedUnit ou)
        {
            AssignNodeTag(ou.DataFrame.Payload, 3);
            SetStateCompleted(ou);
        }

        private void AssignNodeTag(byte[] payload, int index)
        {
            SpecificResult.Node = new NodeTag(payload[index]);
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
