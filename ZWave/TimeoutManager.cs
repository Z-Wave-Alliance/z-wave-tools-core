/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Utils;
using ZWave.Layers;

namespace ZWave
{
    public sealed class TimeoutManager : ITimeoutManager
    {
        private ISessionClient _sessionClient;
        private LinkedList<ITimeoutItem> _timeouts = new LinkedList<ITimeoutItem>();
        private AutoResetEvent _signalAdd = new AutoResetEvent(false);
        private AutoResetEvent _signalInterval = new AutoResetEvent(false);
        private ManualResetEventSlim _signalStart = new ManualResetEventSlim(false);
        private Thread _worker;
        public void Stop()
        {
            Dispose();
        }

        public void Start(ISessionClient sessionClient)
        {
            _sessionClient = sessionClient;
            _worker = new Thread(DoWork);
            _worker.IsBackground = true;
            _worker.Start();
            _signalStart.Wait();
        }

        //private string GetInfo(ITimeoutItem timeoutItem)
        //{
        //    return $"{timeoutItem.ActionId} [{timeoutItem.TimeoutMs}] ms to {timeoutItem.ExpireDateTime:mm:ss.fff}";
        //}

        public void ClearTimers(int actionId)
        {
            if (isDisposing)
                return;

            var dt = DateTime.Now;

            lock (_lockObject)
            {
                var prevItems = _timeouts.Where(x => x.ActionId == actionId);
                if (prevItems != null && prevItems.Any())
                {
                    var firstItem = _timeouts.First.Value;
                    bool isInProgress = false;
                    foreach (var prevItem in prevItems.ToArray())
                    {
                        if (firstItem == prevItem)
                        {
                            isInProgress = true;
                        }
                        _timeouts.Remove(prevItem);
                    }
                    if (isInProgress)
                    {
                        _signalInterval.Set(); // interrupt ongoing and spin worker 
                    }
                }
            }
        }

        public void AddTimer(ITimeoutItem timeoutItem)
        {
            if (isDisposing)
                return;
            if (timeoutItem == null)
            {
                throw new Exceptions.OperationException();
            }
            timeoutItem.Reset();
            var dt = DateTime.Now;
            lock (_lockObject)
            {
                var prevItems = _timeouts.Where(x => x.Id == timeoutItem.Id && x.ActionId == timeoutItem.ActionId);
                if (prevItems != null && prevItems.Any())
                {
                    var firstItem = _timeouts.First.Value;
                    bool isInProgress = false;
                    foreach (var prevItem in prevItems.ToArray())
                    {
                        if (firstItem == prevItem)
                        {
                            isInProgress = true;
                        }
                        _timeouts.Remove(prevItem);
                    }
                    if (isInProgress)
                    {
                        _signalInterval.Set(); // interrupt ongoing and spin worker 
                    }
                }
                var ti = _timeouts.Last;
                while (ti != null)
                {
                    if (timeoutItem.ExpireDateTime > ti.Value.ExpireDateTime)
                    {
                        break;
                    }
                    else
                    {
                        ti = ti.Previous;
                    }
                }
                if (ti == null)
                {
                    timeoutItem.Initialize();
                    _timeouts.AddFirst(timeoutItem);
                    if (_timeouts.Count > 1)
                    {
                        _signalInterval.Set(); // interrupt ongoing and spin worker 
                    }
                }
                else
                {
                    _timeouts.AddAfter(ti, timeoutItem);
                }
            }
            _signalAdd.Set();
        }

        private void DoWork(object state)
        {
            _signalStart.Set();
            while (!isDisposing)
            {
                LinkedListNode<ITimeoutItem> timeoutItemNode = null;
                lock (_lockObject)
                {
                    timeoutItemNode = _timeouts.First;
                }
                if (timeoutItemNode == null)
                {
                    _signalAdd.WaitOne();
                }
                else
                {
                    Interlocked.MemoryBarrier();
                    var dt = DateTime.Now;
                    var nextTick = (int)(timeoutItemNode.Value.ExpireDateTime - DateTime.Now).TotalMilliseconds;
                    if (nextTick > 0)
                    {
                        var signalRes = _signalInterval.WaitOne(nextTick);
                    }
                    Interlocked.MemoryBarrier();
                    if (nextTick <= 0)
                    {
                        bool isDiscarded = false;
                        lock (_lockObject)
                        {
                            if (_timeouts.Contains(timeoutItemNode.Value))
                            {
                                _timeouts.Remove(timeoutItemNode);
                            }
                            else
                            {
                                isDiscarded = true;
                            }
                        }
                        if (!isDisposing && !isDiscarded)
                        {
                            _sessionClient.HandleActionCase(timeoutItemNode.Value);
                        }
                    }
                }
            }
        }

        public int GetTimeoutsCount()
        {
            return _timeouts.Count;
        }

        #region IDisposable Members
        private readonly object _lockObject = new object();
        private volatile bool isDisposing = false;
        public void Dispose()
        {
            isDisposing = true;
            if (!_signalAdd.SafeWaitHandle.IsClosed)
            {
                _signalAdd.Set();
            }
            if (!_signalInterval.SafeWaitHandle.IsClosed)
            {
                _signalInterval.Set();
            }
            _worker?.Join();
            _signalAdd.Close();
            _signalInterval.Close();
            _signalStart.Dispose();
        }

        #endregion
    }
}
