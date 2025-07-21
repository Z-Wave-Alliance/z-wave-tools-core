/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using ZWave.Layers;

namespace ZWave
{
    public interface ITimeoutManager : IDisposable
    {
        void Start(ISessionClient sessionClient);
        void AddTimer(ITimeoutItem timeoutItem);
        void ClearTimers(int actionId);
        int GetTimeoutsCount();
    }

    public interface ITimeoutItem : IActionCase, IActionItem
    {
        int Id { get;  }
        int ActionId { get; }
        int TimeoutMs { get; set; }
        DateTime ExpireDateTime { get; }
        void Reset();
        void Initialize();
    }
}
