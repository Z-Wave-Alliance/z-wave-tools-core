/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Tasks;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public class SmartStartSupport : ApiOperation
    {
        private readonly Action<NodeStatuses> _setNodeStatusSignal;
        private readonly Func<byte, byte[], NodeProvision?> _dskNeededCallback;
        private readonly Action<bool, byte[], ActionResult> _busyCallback;
        private readonly int _timeoutMs;
        private readonly int _delayMs;
        private readonly SetupNodeLifelineSettings _setupNodeLifelineSettings = SetupNodeLifelineSettings.Default;
        public SmartStartSupport(NetworkViewPoint network, Action<NodeStatuses> setNodeStatusSignal, Func<byte, byte[], NodeProvision?> dskNeededCallback, Action<bool, byte[], ActionResult> busyCallback,
            int delayBeforeStartMs, int timeoutMs, SetupNodeLifelineSettings setupNodeLifelineSettings = SetupNodeLifelineSettings.Default)
            : base(false, null, false)
        {
            _network = network;
            _busyCallback = busyCallback;
            _setNodeStatusSignal = setNodeStatusSignal;
            _dskNeededCallback = dskNeededCallback;
            _delayMs = delayBeforeStartMs;
            _timeoutMs = timeoutMs;
            _setupNodeLifelineSettings = setupNodeLifelineSettings;
        }

        private ApiHandler _handler;
        private ApiHandler _handlerLR;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(_handler, OnReceived));
            ActionUnits.Add(new DataReceivedUnit(_handlerLR, OnReceived));
        }

        private bool _isRunning = false;
        private void OnReceived(DataReceivedUnit unit)
        {
            var payload = ((ApiHandler)unit.ActionHandler).DataFrame.Payload;
            var updateStateNodeInfo = payload[0];
            var _homeId = new byte[] { payload[3], payload[4], payload[5], payload[6] };
            if (_network.IsNodeIdBaseTypeLR)
            {
                _homeId = new byte[] { payload[4], payload[5], payload[6], payload[7] };
            }
            var nodeProvision = _dskNeededCallback(updateStateNodeInfo, _homeId);

            if (!_isRunning && nodeProvision != null)
            {
                SetRunning();
                byte[] dskPart = new byte[8];
                Array.Copy(nodeProvision.Value.DSK, 8, dskPart, 0, 8);
                IAddRemoveNode addNode = new AddNodeOperation(_network, (nodeProvision.Value.NodeOptions | Modes.NodeHomeId), _setNodeStatusSignal, _timeoutMs, dskPart)
                {
                    SequenceNumber = SequenceNumber,
                    DskValue = nodeProvision.Value.DSK,
                    GrantSchemesValue = nodeProvision.Value.GrantedSchemes
                };
                var action = new InclusionTask(_network, addNode, true, _setupNodeLifelineSettings);
                var actionGroup = new ActionSerialGroup(action, new SetSmartStartAction(true, _delayMs));
                actionGroup.CompletedCallback = (x) =>
                {
                    var act = x as ActionSerialGroup;
                    if (act != null && act.Actions != null && act.Actions.Length > 0)
                    {
                        _busyCallback(false, nodeProvision.Value.DSK, act.Actions[0]?.Result);
                    }
                    ReleaseRunning();
                };
                unit.SetNextActionItems(actionGroup);
                _busyCallback(true, null, null);
            }
        }
        private void SetRunning()
        {
            _isRunning = true;
        }

        private void ReleaseRunning()
        {
            _isRunning = false;
        }

        protected override void CreateInstance()
        {
            _handler = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
            _handler.AddConditions(new ByteIndex(0x85)); // UPDATE_STATE_NODE_INFO_SMARTSTART_HOMEID_RECEIVED
            _handler.AddConditions(ByteIndex.AnyValue);
            _handler.AddConditions(ByteIndex.AnyValue);
            _handler.AddConditions(ByteIndex.AnyValue);
            _handler.AddConditions(ByteIndex.AnyValue);
            _handler.AddConditions(ByteIndex.AnyValue);
            _handler.AddConditions(ByteIndex.AnyValue);

            _handlerLR = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
            _handlerLR.AddConditions(new ByteIndex(0x87)); // UPDATE_STATE_NODE_INFO_SMARTSTART_HOMEID_LR_RECEIVED
            _handlerLR.AddConditions(ByteIndex.AnyValue);
            _handlerLR.AddConditions(ByteIndex.AnyValue);
            _handlerLR.AddConditions(ByteIndex.AnyValue);
            _handlerLR.AddConditions(ByteIndex.AnyValue);
            _handlerLR.AddConditions(ByteIndex.AnyValue);
            _handlerLR.AddConditions(ByteIndex.AnyValue);
        }
    }
}
