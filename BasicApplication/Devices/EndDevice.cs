/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Layers;
using ZWave.BasicApplication.Operations;
using ZWave.Enums;

namespace ZWave.BasicApplication.Devices
{
    public class EndDevice : Device
    {
        internal EndDevice(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc, bool preInitNodes = true)
            : base(sessionId, sc, fc, tc, preInitNodes)
        { }

        public override ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            var actionBase = actionItem as ActionBase;
            if (actionBase != null)
            {
                actionBase.CompletedCallback = completedCallback;
            }
            return base.ExecuteAsync(actionItem, actionItem.CompletedCallback);
        }

        public override ActionResult Execute(IActionItem action)
        {
            ActionResult ret = base.Execute(action);
            return ret;
        }

        #region SetLearnMode
        public override SetLearnModeResult SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs)
        {
            SetLearnModeResult res = null;
            res = (SetLearnModeResult)Execute(new SetLearnModeEndDeviceOperation(Network, mode, SetAssignStatusSignal, timeoutMs));
            return res;
        }

        public override SetLearnModeResult SetLearnMode(LearnModes mode, int timeoutMs)
        {
            return SetLearnMode(mode, false, timeoutMs);
        }

        public override ActionToken SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SetLearnModeEndDeviceOperation oper = new SetLearnModeEndDeviceOperation(Network, mode, SetAssignStatusSignal, timeoutMs);
            learnModeOperation = oper;
            ret = ExecuteAsync(oper, completedCallback);
            return ret;
        }

        public override ActionToken SetLearnMode(LearnModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return SetLearnMode(mode, false, timeoutMs, completedCallback);
        }
        #endregion

        public IsNodeWithinDirectRangeResult IsNodeWithinDirectRange(byte nodeId)
        {
            return (IsNodeWithinDirectRangeResult)Execute(new IsNodeWithinDirectRangeOperation(nodeId));
        }

        public ActionResult RediscoveryNeeded(byte nodeId)
        {
            return Execute(new RediscoveryNeededOperation(nodeId));
        }

        public ActionResult RequestNewRouteDestinations(byte[] destList)
        {
            return Execute(new RequestNewRouteDestinationsOperation(destList));
        }

        public SetLearnModeResult StopLearnMode()
        {
            return SetLearnMode(LearnModes.LearnModeDisable, 0);
        }
    }
}
