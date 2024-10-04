/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace Utils.Threading
{
    public interface IConsumerQueue<T> : IDisposable where T : class 
    {
        bool IsOpen { get; }
        string Name { get; }
        void Start(Action<T> consumerCallback);
        void Start(string name, Action<T> consumerCallback);
        void Stop();
        void Stop(Action<T> stopCurrentCallback, Action<T> stopPendingCallback);
        void Add(T item);
        void Reset();
    }

    public interface IConsumerThread<T> : IDisposable where T : class
    {
        ushort SessionId { get; }
        string Name { get; }
        void Start(ushort sessionId, string name, Action<T> consumerCallback);
        void Add(T item);
    }
}
