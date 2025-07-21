/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils.Events
{
    public delegate void EventDelegate<T>(T e) where T : EventArgs;
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; set; }
        public EventArgs(T value)
        {
            Value = value;
        }
    }

    public class EventArgs<T1, T2> : EventArgs
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public EventArgs(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }

    public class EventArgs<T1, T2, T3> : EventArgs
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public EventArgs(T1 value1, T2 value2, T3 value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
    }
}
