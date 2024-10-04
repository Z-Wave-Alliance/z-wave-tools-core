/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Threading
{
    public class ConsumerQueue1<T> : IConsumerQueue<T>
        where T : class
    {
        #region privates

        private Queue<T> _innerQueue;
        private T _innerCurrent;
        private Action<T> _consumerCallback;
        private Task _task;

        #endregion privates

        public string Name { get; set; }

        private volatile bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
        }

        public void Stop()
        {
            Stop(null, null);
        }

        public void Stop(Action<T> stopCurrentCallback, Action<T> stopPendingCallback)
        {
            if (!_isOpen)
            {
                return;
            }

            _isOpen = false;

            if (stopCurrentCallback != null && _innerCurrent != null)
            {
                stopCurrentCallback(_innerCurrent);
            }

            if (stopPendingCallback != null)
            {
                var item = _innerQueue.Dequeue();
                while (item != default(T))
                {
                    stopPendingCallback(item);
                    item = _innerQueue.Dequeue();
                }
            }
            else
            {
                _innerQueue.Clear();
            }
            if (_task != null)
            {
                _task.Wait();
            }
        }

        public void Start(Action<T> consumerCallback)
        {
            Start("", consumerCallback);
        }

        public void Start(string name, Action<T> consumerCallback)
        {
            Name = name;
            _innerCurrent = null;
            if (_isOpen)
            {
                return;
            }

            _isOpen = true;
            _innerQueue = new Queue<T>();
            _consumerCallback = consumerCallback;
        }

        private int state;
        private readonly object locker = new object();
        public void Add(T item)
        {
            if (_isOpen)
            {
                lock (locker)
                {
                    _innerQueue.Enqueue(item);
                }
                if (Interlocked.Exchange(ref state, 1) == 0)
                {
                    _task = Task.Factory.StartNew(() => DoWork(), TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning);
                }
            }
        }

        private void DoWork()
        {
            while (_isOpen)
            {
                lock (locker)
                {
                    if (_innerQueue.Count > 0)
                    {
                        _innerCurrent = _innerQueue.Dequeue();
                    }
                    else
                    {
                        state = 0;
                        break;
                    }
                }
                _consumerCallback(_innerCurrent);
            }
        }

        public void Reset()
        {
            lock (locker)
            {
                _innerQueue.Clear();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
