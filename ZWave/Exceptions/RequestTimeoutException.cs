/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Runtime.Serialization;

namespace ZWave.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the time allotted for a reaqest has expired.
    /// </summary>
    [Serializable]
    public sealed class RequestTimeoutException : ZWaveException
    {
        public RequestTimeoutException() : base() { }
        public RequestTimeoutException(string message, Exception innerException) : base(message, innerException) { }
        public RequestTimeoutException(string message) : base(message) { }
        private RequestTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public static void Throw()
        {
            string message = GetMethodName(1) + " Failed";
            throw new RequestTimeoutException(message);
        }
    }
}
