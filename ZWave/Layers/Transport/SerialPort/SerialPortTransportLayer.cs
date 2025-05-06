/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.IO.Ports;
using System.Linq;
using Utils.Events;

namespace ZWave.Layers.Transport
{
    public class SerialPortTransportLayer : ITransportLayer
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput { get; set; }

        public ITransportClient CreateClient(ushort sessionId)
        {
            SerialPortTransportClient ret = new SerialPortTransportClient(dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)))
            {
                SuppressDebugOutput = SuppressDebugOutput,
                SessionId = sessionId
            };
            return ret;
        }

        public static string[] GetDeviceNames()
        {
            var ret = SerialPort.GetPortNames();
            if (ret != null)
            {
                ret = ret.Select(x =>
                {
                    var r = x;
                    int inx = x.IndexOf('\0');
                    if (inx > 0)
                    {
                        r = x.Substring(0, inx);
                    }
                    return r;
                }).ToArray();
            }
            return ret;
        }
    }
}
