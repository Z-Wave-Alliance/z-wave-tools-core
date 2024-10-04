/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils;

namespace ZWave
{
    public class TimeInterval : IActionCase, IActionItem, ITimeoutItem
    {
        public bool IsHandled { get; set; }
        public int Id { get; set; }
        public int ActionId { get; set; }
        public DateTime ExpireDateTime { get; protected set; }
        public Action<IActionItem> CompletedCallback { get; set; }
        public ActionBase ParentAction { get; set; }
        public int TimeoutMs { get; set; }
        public int DataDelay { get; set; }
        public TimeInterval(int id, int actionId, int intervalMs)
        {
            Id = id;
            ActionId = actionId;
            ExpireDateTime = DateTime.MinValue;
            TimeoutMs = intervalMs;
        }

        public void Reset()
        {
            ExpireDateTime = DateTime.Now + TimeSpan.FromMilliseconds(TimeoutMs);
        }

        public override string ToString()
        {
            return Tools.FormatStr("{0}; {1}ms", GetType().Name, TimeoutMs);
        }

        public virtual void Initialize()
        {
            
        }
    }

    public class RandomTimeInterval : TimeInterval
    {
        public int MinIntervalMs { get; set; }
        public int MaxIntervalMs { get; set; }
        private Random rnd = new Random();

        public RandomTimeInterval(int id, int actionId, int minIntervalMs, int maxIntervalMs)
            : base(id, actionId, 0)
        {
            MinIntervalMs = minIntervalMs;
            MaxIntervalMs = maxIntervalMs;
        }

        public int NextTimeoutMs()
        {
            TimeoutMs = rnd.Next(MinIntervalMs, MaxIntervalMs);
            return TimeoutMs;
        }

        public override void Initialize()
        {
            NextTimeoutMs();
        }
    }

    public class PoissonTimeInterval : TimeInterval
    {
        public int ArrivalRateMs { get; set; }

        private Random rnd = new Random();

        public PoissonTimeInterval(int id, int actionId, int arrivalRateMs)
            : base(id, actionId, 0)
        {
            ArrivalRateMs = arrivalRateMs;
        }

        public int NextTimeoutMs()
        {
            TimeoutMs = (int)(-Math.Log(rnd.NextDouble())* ArrivalRateMs);            
            return TimeoutMs;
        }
        public override void Initialize()
        {
            NextTimeoutMs();
        }
    }
}
