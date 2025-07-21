/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Layers.Frame;

namespace ZWave
{
    public class DataReceivedUnit : ActionUnit<DataReceivedUnit>
    {
        public CustomDataFrame DataFrame
        {
            get { return ((CommandHandler)ActionHandler).DataFrame; }
        }

        public DataReceivedUnit(CommandHandler handler, Action<DataReceivedUnit> func, int timeoutMs, params IActionItem[] items)
            : base(handler, func, timeoutMs, items)
        {
        }

        public DataReceivedUnit(CommandHandler handler, Action<DataReceivedUnit> func, params IActionItem[] items) :
            this(handler, func, 0, items)
        {

        }

        public DataReceivedUnit(CommandHandler handler, Func<DataReceivedUnit, bool?> predicate, IActionItem item, IActionItem falseItem, IActionItem undefinedItem) :
           base(handler, predicate, 0, item, falseItem, undefinedItem)
        {

        }
    }
}
