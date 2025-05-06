/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.Threading;
using System.Threading;
using System.Linq;
using Utils;

namespace ZWave.Layers.Frame
{
    public class FrameBufferBlock : IDisposable
    {
        private const int ACK_TIME = 2000;
        private readonly Func<byte[], int> _writeData;
        private readonly Func<CommandMessage, byte[]> _createFrameBuffer;
        private EventWaitHandle _ackSignal = new ManualResetEvent(false);
        private volatile bool _isTransmitted = false;
        private int _ackTimeout = ACK_TIME;
        public FrameBufferBlock(Func<byte[], int> writeData, Func<CommandMessage, byte[]> createFrameBuffer)
            : this(ACK_TIME, writeData, createFrameBuffer)
        {
        }

        public FrameBufferBlock(int ackTimeout, Func<byte[], int> writeData, Func<CommandMessage, byte[]> createFrameBuffer)
        {
            _ackTimeout = ackTimeout;
            _writeData = writeData;
            _createFrameBuffer = createFrameBuffer;
        }

        public void Acknowledge(bool isTransmitted)
        {
            _isTransmitted = isTransmitted;
            _ackSignal.Set();
        }

        public bool Send(ActionHandlerResult ahResult)
        {
            bool ret = false;
            if (ahResult != null && ahResult.NextActions != null)
            {
                var sendFrames = ahResult.NextActions.Where(x => x is CommandMessage);
                if (sendFrames.Any())
                {
                    ret = true;
                    foreach (CommandMessage frame in sendFrames)
                    {
                        byte[] tmp = _createFrameBuffer(frame);
                        byte[] dataTmp = frame.Data;
                        if (frame.IsSequenceNumberRequired)
                        {
                            dataTmp = new byte[frame.Data.Length + 1];
                            Array.Copy(frame.Data, 0, dataTmp, 0, frame.Data.Length);
                            dataTmp[dataTmp.Length - 1] = frame.SequenceNumber;
                        }
                        _isTransmitted = false;
                        _ackSignal.Reset();
                        _writeData(tmp);
                        ahResult.Parent.AddTraceLogItem(DateTime.Now, dataTmp, true);
                        if (!frame.IsNoAck)
                        {
                            int countCAN = 0;
                            int countNAK = 0;
                            while (countCAN < 10 && countNAK < 2 && !_isTransmitted /*&& _queue.IsOpen*/)
                            {
                                bool res = _ackSignal.WaitOne(_ackTimeout);
                                if (!_isTransmitted)
                                {
                                    if (res)
                                    {
                                        countCAN++;
                                        Thread.Sleep(countCAN * (_ackTimeout / 10)); //wait after CAN received (for example ctrl is busy with smart start)
                                        $"CAN RECEIVED {countCAN}"._DLOG();
                                    }
                                    else
                                    {
                                        countNAK++;
                                        $"ACK MISSING {countCAN}"._DLOG();
                                    }
                                    _ackSignal.Reset();
                                    _writeData(tmp);
                                    ahResult.Parent.AddTraceLogItem(DateTime.Now, dataTmp, true);
                                }
                            }
                        }
                        else
                        {
                            _isTransmitted = true;
                        }
                        ret &= _isTransmitted;
                    }
                    ahResult.NextFramesCompletedCallback?.Invoke(ret);
                }
            }
            return ret;
        }

        public void Dispose()
        {
            _ackSignal.Close();
        }
    }
}
