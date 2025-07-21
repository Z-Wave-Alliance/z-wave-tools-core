/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.BasicApplication.Enums;
using System.Collections.Generic;
using System;
using ZWave.Devices;

namespace BasicApplicationTests
{
    public class FramesLoggerProvider
    {
        public  Dictionary<ushort, List<FrameLogRecord>> FramesLog { get; } = new Dictionary<ushort, List<FrameLogRecord>>();
        private object _syncObject = new object();
        public void ClearLog()
        {
            lock (_syncObject)
            {
                FramesLog.Clear();
            }
        }

        public void FrameLayer_FrameTransmitted(object sender, Utils.Events.EventArgs<IDataFrame> e)
        {
            lock (_syncObject)
            {
                if (e.Value.Data.Length < 7)
                    return;

                if (e.Value.Data[1] == (byte)CommandTypes.CmdZWaveSendData)
                {
                    if (!FramesLog.ContainsKey(e.Value.SessionId))
                    {
                        FramesLog.Add(e.Value.SessionId, new List<FrameLogRecord>());
                    }
                    FramesLog[e.Value.SessionId].Add(FrameLogRecord.Create(new NodeTag(0), new NodeTag(e.Value.Data[2]), e.Value.Data[5]));
                }
                else if (e.Value.Data[1] == (byte)CommandTypes.CmdApplicationCommandHandler)
                {
                    if (!FramesLog.ContainsKey(e.Value.SessionId))
                    {
                        FramesLog.Add(e.Value.SessionId, new List<FrameLogRecord>());
                    }
                    FramesLog[e.Value.SessionId].Add(FrameLogRecord.Create(new NodeTag(e.Value.Data[3]), new NodeTag(0), e.Value.Data[6]));
                }
            }
        }

        public void FrameLayer_FrameTransmittedToConsole(object sender, Utils.Events.EventArgs<IDataFrame> e)
        {
            //ReportToConsole(e.Value);
        }

        private void ReportToConsole(IDataFrame dataFrame)
        {
            Console.Out.WriteLine("{0:HH:mm:ss.fff} {1:000} {2} {3}",
                dataFrame.SystemTimeStamp,
                dataFrame.SessionId,
                dataFrame.IsOutcome ? ">>" : "<<",
                dataFrame);
        }
    }
}
