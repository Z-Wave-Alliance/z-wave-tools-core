/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave
{
    public class ActionSerialGroup : ActionGroup
    {
        protected Action<ActionBase, ActionResult> _onActionCompleted;
        protected Action<ActionBase, ActionBase> _onActionCompletedNew;
        public ActionBase[] Actions { get; protected set; }
        private bool _hasFailed;
        protected bool _allowFailed;
        public ActionSerialGroup(params ActionBase[] actions)
        {
            Actions = actions;
        }

        public ActionSerialGroup(Action<ActionBase, ActionResult> onActionCompleted, params ActionBase[] actions)
            : this(actions)
        {
            _onActionCompleted = onActionCompleted;
        }

        public ActionSerialGroup(Action<ActionBase, ActionBase> onActionCompletedNew, params ActionBase[] actions)
            : this(actions)
        {
            _onActionCompletedNew = onActionCompletedNew;
        }

        public void AddActions(params ActionBase[] actions)
        {
            if (actions != null && actions.Length > 0)
            {
                if (Actions == null)
                    Actions = actions;
                else
                {
                    var tmp = new ActionBase[Actions.Length + actions.Length];
                    Array.Copy(Actions, 0, tmp, 0, Actions.Length);
                    Array.Copy(actions, 0, tmp, Actions.Length, actions.Length);
                    Actions = tmp;
                }
            }
        }

        protected override void CreateWorkflow()
        {
            if (Actions.Length > 0)
            {
                for (int i = 0; i < Actions.Length + 1; i++)
                {
                    if (i == 0)
                    {
                        ActionUnits.Add(new StartActionUnit(OnStart, 0, Actions[i]));
                    }
                    else if (i == Actions.Length)
                    {
                        ActionUnits.Add(new ActionCompletedUnit(Actions[i - 1], OnLastCompleted, null));
                    }
                    else
                    {
                        ActionUnits.Add(new ActionCompletedUnit(Actions[i - 1], OnCompleted, Actions[i]));
                    }
                }
            }
            else
                ActionUnits.Add(new StartActionUnit(SetStateCompleted, 0));
        }

        private void OnStart(StartActionUnit ou)
        {
        }

        protected virtual void OnCompletedInternal(ActionCompletedUnit ou)
        {
        }

        private void OnCompleted(ActionCompletedUnit ou)
        {
            if (ou.Action.Result.State == ActionStates.Failed)
            {
                _hasFailed = true;
            }

            if (_onActionCompleted != null && ou.ActionItems.Count > 0)
            {
                _onActionCompleted((ActionBase)ou.ActionItems[0], ou.Action.Result);
            }
            if (_onActionCompletedNew != null && ou.ActionItems.Count > 0)
            {
                _onActionCompletedNew(this, ou.Action);
            }
            Result.InnerResults.Add(ou.Action.Result);
            OnCompletedInternal(ou);
        }

        private void OnLastCompleted(ActionCompletedUnit ou)
        {
            OnCompleted(ou);
            //foreach (var action in Actions)
            //{
            //    Result.InnerResults.Add(action.Result);
            //}
            if (!_allowFailed && _hasFailed)
            {
                SetStateFailed(ou);
            }
            else
            {
                SetStateCompleted(ou);
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
