/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Utils.UI.Threading
{
    public class ActionDispatcher<T>
    {
        private Thread _workerThread;
        private bool _isCancelling;
        private readonly AutoResetEvent _signal = new AutoResetEvent(false);
        private readonly object _locker = new object();
        private Queue<Action<T>> _tempQueue;
        private Queue<Action<T>> _storeQueue;
        private Queue<Action<T>> _activeQueue;
        public bool IsRunning { get; set; }
        public ActionDispatcher()
        {
            _storeQueue = new Queue<Action<T>>();
            _activeQueue = new Queue<Action<T>>();
        }

        private void DoWork()
        {
            IsRunning = true;
            while (!_isCancelling)
            {
                _signal.WaitOne();
                lock (_locker)
                {
                    _tempQueue = _activeQueue;
                    _activeQueue = _storeQueue;
                    _storeQueue = _tempQueue;
                }
                while (_activeQueue.Count > 0)
                {
                    _activeQueue.Dequeue();
                }
            }
            IsRunning = false;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                _isCancelling = false;
                _workerThread = new Thread(DoWork)
                {
                    Name = "Action Dispatcher",
                    IsBackground = true
                };
                _workerThread.Start();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _isCancelling = true;
                _signal.Set();
                if (Thread.CurrentThread.ManagedThreadId != _workerThread.ManagedThreadId)
                    _workerThread.Join();
                _workerThread = null;
            }
        }


        public void BeginInvoke(Action<T> action)
        {
            lock (_locker)
            {
                _storeQueue.Enqueue(action);
            }
            _signal.Set();
        }
    }
}
