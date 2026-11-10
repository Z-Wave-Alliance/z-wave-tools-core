// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// Runs the Learn Mode of a Serial API End Device and reads the keys of the chip with Security Setup Get Security Keys once
    /// the Learn Mode is complete. The chip does the security bootstrapping itself and holds the keys, while the NodeTag setter
    /// grants the own node every enabled scheme with the new NodeId. The key read has a short timeout and leaves the Learn Mode
    /// result as it is when it gets no answer, so a chip that restarts at the end of the inclusion does not fail the Learn Mode.
    /// </summary>
    public class SetLearnModeWithSecurityKeysOperation : ApiOperation
    {
        public static int SECURITY_KEYS_TIMEOUT = 5000;

        private readonly SetLearnModeEndDeviceOperation _learnMode;
        private GetSecurityKeysOperation _getSecurityKeys;

        public SetLearnModeWithSecurityKeysOperation(NetworkViewPoint network, SetLearnModeEndDeviceOperation learnMode)
            : base(false, learnMode.SerialApiCommands, false)
        {
            _network = network;
            _learnMode = learnMode;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, _learnMode));
            ActionUnits.Add(new ActionCompletedUnit(_learnMode, OnLearnModeCompleted));
            ActionUnits.Add(new ActionCompletedUnit(_getSecurityKeys, OnSecurityKeysCompleted));
        }

        protected override void CreateInstance()
        {
            _getSecurityKeys = new GetSecurityKeysOperation(_network, SECURITY_KEYS_TIMEOUT);
        }

        /// <summary>
        /// Takes the Learn Mode result over and starts the key read after a completed Learn Mode, or ends in the state of the
        /// Learn Mode otherwise.
        /// </summary>
        private void OnLearnModeCompleted(ActionCompletedUnit unit)
        {
            SetLearnModeResult res = _learnMode.SpecificResult;
            SpecificResult.Node = res.Node;
            SpecificResult.LearnModeStatus = res.LearnModeStatus;
            SpecificResult.SubstituteStatus = res.SubstituteStatus;
            switch (res.State)
            {
                case ActionStates.Completed:
                    unit.SetNextActionItems(_getSecurityKeys);
                    break;
                case ActionStates.Cancelled:
                    SetStateCancelled(unit);
                    break;
                case ActionStates.Expired:
                    SetStateExpired(unit);
                    break;
                default:
                    SetStateFailed(unit);
                    break;
            }
        }

        /// <summary>
        /// Stores the keys the chip reported in the result and completes, with or without an answer.
        /// </summary>
        private void OnSecurityKeysCompleted(ActionCompletedUnit unit)
        {
            if (_getSecurityKeys.Result)
            {
                SpecificResult.SecuritySchemes = _getSecurityKeys.SpecificResult.SecuritySchemes;
            }
            SetStateCompleted(unit);
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
