// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Silicon Laboratories Inc. <https://www.silabs.com>
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
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

        /// <summary>
        /// Runs the action and, for a MemoryGetId of a Serial API End Device library, reads the keys of the chip before the
        /// callback runs. The token completes with the MemoryGetId, so a caller reads the keys in the callback.
        /// </summary>
        public override ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            var actionBase = actionItem as ActionBase;
            if (actionBase != null)
            {
                if (IsMemoryGetIdOfSerialApiEndDevice(actionItem))
                {
                    actionBase.CompletedCallback = x => ReadSecurityKeys(x, completedCallback);
                }
                else
                {
                    actionBase.CompletedCallback = completedCallback;
                }
            }
            return base.ExecuteAsync(actionItem, actionItem.CompletedCallback);
        }

        /// <summary>
        /// Runs the action and, for a MemoryGetId of a Serial API End Device library, reads the keys of the chip before it
        /// returns.
        /// </summary>
        public override ActionResult Execute(IActionItem action)
        {
            ActionResult ret = base.Execute(action);
            if (ret && IsMemoryGetIdOfSerialApiEndDevice(action))
            {
                base.Execute(new GetSecurityKeysOperation(Network));
            }
            return ret;
        }

        /// <summary>
        /// Whether the action is a MemoryGetId of a Serial API End Device library. Such a chip does the security bootstrapping
        /// itself and holds the keys, while the NodeTag setter grants the own node every enabled scheme with the NodeId.
        /// </summary>
        private bool IsMemoryGetIdOfSerialApiEndDevice(IActionItem action)
        {
            return action is MemoryGetIdOperation && Network.Library.IsSerialApiEndDevice();
        }

        /// <summary>
        /// Reads the keys of the chip after a completed MemoryGetId and runs the callback once they are in the network view.
        /// </summary>
        private void ReadSecurityKeys(IActionItem memoryGetId, Action<IActionItem> completedCallback)
        {
            if (((ActionBase)memoryGetId).Result)
            {
                ExecuteAsync(new GetSecurityKeysOperation(Network), x => completedCallback?.Invoke(memoryGetId));
            }
            else
            {
                completedCallback?.Invoke(memoryGetId);
            }
        }

        #region SetLearnMode
        public override SetLearnModeResult SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs)
        {
            return (SetLearnModeResult)Execute(CreateSetLearnMode(mode, timeoutMs));
        }

        public override SetLearnModeResult SetLearnMode(LearnModes mode, int timeoutMs)
        {
            return SetLearnMode(mode, false, timeoutMs);
        }

        public override ActionToken SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(CreateSetLearnMode(mode, timeoutMs), completedCallback);
        }

        public override ActionToken SetLearnMode(LearnModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return SetLearnMode(mode, false, timeoutMs, completedCallback);
        }

        /// <summary>
        /// Creates the Learn Mode operation. A Serial API End Device library gets the one that reads the keys of the chip once
        /// the Learn Mode is complete, except for a stop of the Learn Mode.
        /// </summary>
        private ApiOperation CreateSetLearnMode(LearnModes mode, int timeoutMs)
        {
            SetLearnModeEndDeviceOperation learnMode = new SetLearnModeEndDeviceOperation(Network, mode, SetAssignStatusSignal, timeoutMs);
            learnModeOperation = learnMode;
            if (mode == LearnModes.LearnModeDisable || !Network.Library.IsSerialApiEndDevice())
            {
                return learnMode;
            }
            return new SetLearnModeWithSecurityKeysOperation(Network, learnMode);
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
