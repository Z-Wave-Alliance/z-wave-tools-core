/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Threading
{
    public class ConsumerThread<T> : IConsumerThread<T>
        where T : class
    {
        #region privates
        private ConcurrentQueue<T> _innerQueue = new ConcurrentQueue<T>();
        private Action<T> _consumerCallback;
        private Thread _worker;
        private AutoResetEvent _signalAdd = new AutoResetEvent(false);
        private ManualResetEventSlim _signalStart = new ManualResetEventSlim(false);
        private Stopwatch _callbackSW = new Stopwatch();
        #endregion privates

        public ushort SessionId { get; set; }
        public string Name { get; set; }

        public void Start(ushort sessionId, string name, Action<T> consumerCallback)
        {
            SessionId = sessionId;
            Name = name;
            _consumerCallback = consumerCallback;
            _worker = new Thread(DoWork);
            _worker.IsBackground = true;
            _worker.Start();
            _signalStart.Wait();
            $"{SessionId:X2} ConsumerThread {Name} started"._DLOG();
        }

        private string GetInfo(T item)
        {
            return item.ToString();
        }

        public void Add(T item)
        {
            if (isClosing)
                return;
            if (item == null)
            {
                throw new ArgumentException();
            }

            _innerQueue.Enqueue(item);
            if (!isClosing)
            {
                try
                {
                    _signalAdd.Set();
                }
                catch (ObjectDisposedException)
                { }
            }
        }

        private void DoWork()
        {
            _signalStart.Set();
            while (!isClosing)
            {
                if (_innerQueue.TryDequeue(out T item))
                {
                    _callbackSW.Restart();
                    _consumerCallback(item);
                    _callbackSW.Stop();
                    if (_callbackSW.ElapsedMilliseconds > 1000)
                    {
                        "WARNING - long running consumer callback"._DLOG();
                    }
                }
                else
                {
                    _signalAdd.WaitOne();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;
        private volatile bool isClosing = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    isClosing = true;
                    _signalAdd.Set();
                    _worker?.Join();
                    _signalAdd.Close();
                    _signalStart.Dispose();
                    _callbackSW.Stop();
                    $"{SessionId:X2} ConsumerThread {Name} disposed"._DLOG();
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

    public class ConsumerBlockingCollection1<T> : IConsumerThread<T>
     where T : class
    {
        #region privates
        private BlockingCollection<T> _innerQueue = new BlockingCollection<T>();
        private Action<T> _consumerCallback;
        private Thread _worker;
        private ManualResetEventSlim _signalStart = new ManualResetEventSlim(false);

        #endregion privates

        public ushort SessionId { get; set; }
        public string Name { get; set; }

        public void Start(ushort sessionId, string name, Action<T> consumerCallback)
        {
            SessionId = sessionId;
            Name = name;
            _consumerCallback = consumerCallback;
            _worker = new Thread(DoWork);
            _worker.Start();
            _signalStart.Wait();
        }

        private string GetInfo(T item)
        {
            return item.ToString();
        }

        public void Add(T item)
        {
            if (isClosing)
                return;
            if (item == null)
            {
                throw new ArgumentException();
            }
            _innerQueue.TryAdd(item);
        }

        private void DoWork()
        {
            _signalStart.Set();
            while (!isClosing)
            {
                bool hasItem = false;
                T item = null;
                try
                {
                    hasItem = _innerQueue.TryTake(out item);
                }
                catch (Exception ex)
                {
                    ex.Message._EXLOG();
                }
                if (hasItem)
                {
                    _consumerCallback(item);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;
        private volatile bool isClosing = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    isClosing = true;
                    _innerQueue.Dispose();
                    _worker?.Join();
                    _signalStart.Dispose();
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
