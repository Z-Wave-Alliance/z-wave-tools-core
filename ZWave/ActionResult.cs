/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Text;
using Utils;
using Utils.Threading;

namespace ZWave
{
    public class ActionResult
    {
        public ActionStates State { get; internal set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime StopTimestamp { get; set; }
        public ConcurrentList<TraceLogItem> TraceLog { get; internal set; }
        public int RetryCount { get; set; }
        public List<ActionResult> InnerResults { get; set; }
        public ActionResult()
        {
            //TraceLog = new ConcurrentList<TraceLogItem>();
            InnerResults = new List<ActionResult>();
        }

        public ActionResult(ActionResult res)
        {
            State = res.State;
            TraceLog = res.TraceLog;
            RetryCount = res.RetryCount;
            InnerResults = new List<ActionResult>();
        }

        public int ElapsedMs
        {
            get
            {
                return (int)(StopTimestamp - StartTimestamp).TotalMilliseconds;
            }
        }

        public bool IsStateCompleted
        {
            get { return State == ActionStates.Completed; }
        }

        public static implicit operator bool(ActionResult val)
        {
            return val != null && val.IsStateCompleted;
        }

        public T FindFirstInnerResult<T>() where T : ActionResult
        {
            T ret = default(T);
            foreach (var item in InnerResults)
            {
                if (item is T)
                {
                    ret = (T)item;
                }
                else
                {
                    ret = item.FindFirstInnerResult<T>();
                }
                if (ret != null)
                    break;
            }
            return ret;
        }

        public List<T> FindInnerResults<T>() where T : ActionResult
        {
            List<T> ret = new List<T>();
            foreach (var item in InnerResults)
            {
                if (item is T)
                {
                    ret.Add((T)item);
                }
                ret.AddRange(item.FindInnerResults<T>());
            }
            return ret;
        }

        internal void AddTraceLogItem(DateTime dt, byte[] data, bool isOutcome)
        {
            if (data != null && data.Length > 0 && TraceLog != null)
            {
                if (TraceLog.Count < 10)
                {
                    mTraceLogString = null;
                    TraceLog.Add(new TraceLogItem(dt, data, isOutcome));
                }
                else if (!mIsLogSealed)
                {
                    mTraceLogString += "too many log items" + Environment.NewLine;
                    mIsLogSealed = true;
                }
            }
        }

        private bool mIsLogSealed = false;
        private string mTraceLogString = null;
        public string TraceLogString
        {
            get
            {
                if (mTraceLogString == null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (TraceLog != null)
                    {
                        foreach (var item in TraceLog.ToArray())
                        {
                            sb.AppendLine(string.Format("{0:HH:mm:ss.fff} {1} {2}", item.TimeStamp, item.IsOutcome ? " >> " : " << ", Tools.GetHex(item.Data)));
                        }
                    }
                    mTraceLogString = sb.ToString();
                }
                return mTraceLogString;
            }
        }
    }

    public class ReturnValueResult : ActionResult
    {
        public byte ByteValue
        {
            get { return ByteArray != null && ByteArray.Length > 0 ? ByteArray[0] : (byte)0; }
        }

        public byte[] ByteArray { get; set; }
    }

    public class TraceLogItem
    {
        public DateTime TimeStamp { get; set; }
        public byte[] Data { get; set; }
        public byte Flags { get; set; }
        public bool IsOutcome
        {
            get { return (Flags & 0x01) > 0; }
            set
            {
                Flags &= 0xFF - 0x01;
                if (value)
                    Flags += 0x01;
            }
        }

        public TraceLogItem(DateTime dt, byte[] data, bool isOutcome)
        {
            TimeStamp = dt;
            Data = data;
            IsOutcome = isOutcome;
        }
    }
}
