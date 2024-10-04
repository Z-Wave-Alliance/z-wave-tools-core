/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using System.Text;

namespace ZWave
{
    public class ActionParallelGroup : ActionGroup
    {
        public ActionBase[] Actions { get; set; }
        protected bool IsWaitingForAllActions { get; set; }
        public ActionParallelGroup(bool isWaitingForAllActions, params ActionBase[] actions)
        {
            Actions = actions;
            IsWaitingForAllActions = isWaitingForAllActions;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, Actions));
            for (int i = 0; i < Actions.Length; i++)
            {
                ActionUnits.Add(new ActionCompletedUnit(Actions[i], OnCompleted));
            }
        }

        private int CompletedOoperations = 0;
        private void OnCompleted(ActionCompletedUnit ou)
        {
            Result.InnerResults.Add(ou.Action.Result);
            if (IsWaitingForAllActions)
            {
                CompletedOoperations++;
                if (CompletedOoperations == Actions.Length)
                {
                    if (ou.Action.Result.State == ActionStates.Failed)
                    {
                        SetStateFailed(ou);
                    }
                    else if (ou.Action.Result.State == ActionStates.Expired)
                    {
                        SetStateExpired(ou);
                    }
                    else
                    {
                        SetStateCompleted(ou);
                    }
                }
            }
            else
            {
                if (ou.Action.Result.State == ActionStates.Failed)
                {
                    SetStateFailed(ou);
                }
                else if (ou.Action.Result.State == ActionStates.Expired)
                {
                    SetStateExpired(ou);
                }
                else
                {
                    SetStateCompleted(ou);
                }
            }
        }

        protected override void CreateInstance()
        {
            foreach (var item in Actions)
            {
                item.ParentAction = this;
            }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
