/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Runtime.Serialization;
using Utils;

namespace ZWave.Exceptions
{
    [Serializable]
    public sealed class OperationException : ZWaveException
    {
        public OperationException() : base() { }
        public OperationException(string message, Exception innerException) : base(message, innerException) { }
        public OperationException(string message) : base(message) { }
        private OperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void Throw(string message)
        {
            string msg = Tools.FormatStr("{0} {1}", GetMethodName(1), message);
            throw new OperationException(msg);
        }

        public static void Throw(string message, int methodStackIndex)
        {
            string msg = Tools.FormatStr("{0} {1}", GetMethodName(1 + methodStackIndex), message);
            throw new OperationException(msg);
        }
    }
}
