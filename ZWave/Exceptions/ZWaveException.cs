/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Reflection;
using System.Runtime.Serialization;
using Utils;

namespace ZWave.Exceptions
{
    [Serializable]
    public class ZWaveException : Exception
    {
        public ZWaveException() : base() { }
        public ZWaveException(string message) : base(message) { }
        public ZWaveException(string message, Exception innerException) : base(message, innerException) { }
        protected ZWaveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public static string GetMethodName(int showCallingPointShifter)
        {
            string methodName = "N/A";
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            if (st.FrameCount - 1 > showCallingPointShifter)
            {
                System.Diagnostics.StackFrame sf = st.GetFrame(1 + showCallingPointShifter);
                MethodBase mb = sf.GetMethod();
                methodName = Tools.FormatStr("{0}.{1}", mb.DeclaringType.Name, mb.Name);
            }
            return methodName;
        }
    }
}
