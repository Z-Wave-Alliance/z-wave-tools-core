/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;

namespace ZWave
{
    public interface IActionUnit
    {
        string Name { get; set; }
        int TimeoutMs { get; }
        IActionHandler ActionHandler { get; set; }
        Action<IActionUnit> Func { get; }

        void Reset(int timeoutMs);
        void SetParentAction(ActionBase action);
        bool TryHandle(IActionCase actionCase);
        int CopyActionsItemsTo(List<IActionItem> nextActions);
    }

    public class ActionUnit<T> : IActionUnit where T : IActionUnit
    {
        public string Name { get; set; }
        public int TimeoutMs { get; protected set; }
        public IActionHandler ActionHandler { get; set; }
        public Action<IActionUnit> Func { get; set; }
        public List<IActionItem> ActionItems { get; protected set; }

        public ActionUnit(IActionHandler handler, Action<T> func, int timeoutMs, params IActionItem[] items)
        {
            ActionItems = new List<IActionItem>();
            ActionHandler = handler;
            if (func != null)
            {
                Func = x => func((T)x);
            }
            TimeoutMs = timeoutMs;
            if (items != null && items.Length > 0)
            {
                ActionItems.AddRange(items);
            }
        }

        public ActionUnit(IActionItem actionItem)
           : this(null, null, 0, actionItem)
        {
        }

        public ActionUnit(IActionHandler handler, Func<T, bool?> predicate, int timeoutMs, IActionItem item, IActionItem falseItem, IActionItem undefinedItem)
            : this(handler, null, timeoutMs, item)
        {
            if (predicate != null)
            {
                Func = x =>
                {
                    bool? res = predicate((T)x);
                    ActionItems = new List<IActionItem>();
                    if (res == null)
                    {
                        ActionItems.Add(undefinedItem);
                    }
                    else if (res.Value)
                    {
                        ActionItems.Add(item);
                    }
                    else if (!res.Value)
                    {
                        ActionItems.Add(falseItem);
                    }
                };
            }
        }

        public void Reset(int timeoutMs)
        {
            TimeoutMs = timeoutMs;
        }

        public int CopyActionsItemsTo(List<IActionItem> nextActions)
        {
            int ret = 0;
            lock (this)
            {
                if (nextActions != null && ActionItems != null && ActionItems.Count > 0)
                {
                    nextActions.AddRange(ActionItems);
                    ret = ActionItems.Count;
                }
            }
            return ret;
        }

        public void SetParentAction(ActionBase action)
        {
            lock (this)
            {
                if (ActionItems != null)
                {
                    foreach (var item in ActionItems)
                    {
                        item.ParentAction = action;
                    }
                }
            }
        }

        public void SetNextActionItems(params IActionItem[] actionItems)
        {
            lock (this)
            {
                if (actionItems != null && actionItems.Length > 0)
                {
                    ActionItems = new List<IActionItem>(actionItems);
                }
                else if (ActionItems != null)
                {
                    ActionItems.Clear();
                }
            }
        }

        public void AddNextActionItems(params IActionItem[] actionItems)
        {
            lock (this)
            {
                if (ActionItems == null)
                {
                    SetNextActionItems(actionItems);
                }
                else
                {
                    ActionItems.AddRange(actionItems);
                }
            }
        }

        public void AddFirstNextActionItems(params IActionItem[] actionItems)
        {
            lock (this)
            {
                if (ActionItems == null)
                {
                    SetNextActionItems(actionItems);
                }
                else
                {
                    ActionItems.InsertRange(0, actionItems);
                }
            }
        }

        public virtual bool TryHandle(IActionCase actionCase)
        {
            return ActionHandler != null && ActionHandler.State != HandlerStates.Handled && ActionHandler.WaitingFor(actionCase);
        }
    }
}
