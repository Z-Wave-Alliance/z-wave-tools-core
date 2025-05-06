/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.IO.Ports;

namespace Utils.Extensions
{
    public static class SerialPortShouldBeNotNullExt
    {
        public static TResult SouldBeNotNull<TResult>(this SerialPort port, Func<SerialPort, TResult> func, string message)
        {
            if (port == null)
            {
                throw new InvalidOperationException(message);
            }
            return func(port);
        }

        public static void SouldBeNotNull(this SerialPort port, Action<SerialPort> action, string message)
        {
            if (port == null)
            {
                throw new InvalidOperationException(message);
            }
            action(port);
        }

        public static void SouldBeNotNull(this SerialPort port, Action action, string message)
        {
            if (port == null)
            {
                throw new InvalidOperationException(message);
            }
            action();
        }
    }
}
