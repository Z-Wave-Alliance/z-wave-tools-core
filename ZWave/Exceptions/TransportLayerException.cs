/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Runtime.Serialization;

namespace ZWave.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a error occured in <see cref="ITransportLayer"></see>.
    /// </summary>
    [Serializable]
    public sealed class TransportLayerException : ZWaveException
    {
        public TransportLayerException() : base() { }
        public TransportLayerException(string message, Exception innerException) : base(message, innerException) { }
        public TransportLayerException(string message) : base(message) { }
        private TransportLayerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public static void Throw()
        {
            string message = GetMethodName(1) + " Failed";
            throw new TransportLayerException(message);
        }
    }
}
