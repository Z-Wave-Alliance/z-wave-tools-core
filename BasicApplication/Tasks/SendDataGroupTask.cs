/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.BasicApplication.Operations;

namespace ZWave.BasicApplication.Tasks
{
    public class SendDataGroupTask : ActionSerialGroup
    {
        public bool IsStopOnNak { get; private set; }
        public SendDataGroupTask(bool isStopOnNak, params ActionBase[] actions) :
            base(actions)
        {
            IsStopOnNak = isStopOnNak;
        }

        protected override void OnCompletedInternal(ActionCompletedUnit ou)
        {
            if (IsStopOnNak)
            {
                SendDataResult sdr = ou.Action.Result as SendDataResult;
                if (sdr != null)
                {
                    if (!sdr || sdr.TransmitStatus != ZWave.Enums.TransmitStatuses.CompleteOk)
                    {
                        SetStateFailed(ou);
                    }
                }
            }
        }
    }
}
