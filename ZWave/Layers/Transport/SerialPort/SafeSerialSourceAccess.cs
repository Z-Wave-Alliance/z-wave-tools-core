/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;

namespace ZWave.Layers.Transport
{
#if NETCOREAPP
    public abstract class SafeSerialSourceAccess<T> : ISerialPortProvider
    {
        private Guid _id = Guid.NewGuid();

        protected static object _access = new object();
        protected static Dictionary<T, Guid> _sourcesMap = new Dictionary<T, Guid>();

        protected static bool AccessCallLocked(T key, Guid id, Action actionToCall)
        {
            lock (_access)
            {
                if (!EqualityComparer<T>.Default.Equals(key, default(T)) && _sourcesMap.ContainsKey(key) && _sourcesMap[key].Equals(id))
                {
                    actionToCall.Invoke();
                    return true;
                }
                return false;
            }
        }

        protected T _key;
        protected abstract bool CreateKey(string sourceName, out T key);

        protected string _portName;
        public string PortName { get { return _portName; } }

        public bool IsOpen
        {
            get
            {
                return AccessCallLocked(_key, _id, () => IsOpenInternal());
            }
        }
        protected abstract bool IsOpenInternal();


        public void Close()
        {
            AccessCallLocked(_key, _id, () =>
            {
                if (IsOpenInternal())
                {
                    CloseInternal();
                }
                _sourcesMap.Remove(_key);
                _portName = null;
            });
        }
        protected abstract void CloseInternal();

        public void Dispose()
        {
            Close();
        }

        public bool Open(string portName, int baudRate, PInvokeParity parity, int dataBits, PInvokeStopBits stopBits)
        {
            if (CreateKey(portName, out _key))
            {
                _portName = portName;
                lock (_access)
                {
                    if (!_sourcesMap.ContainsKey(_key) && OpenInternal(baudRate))
                    {
                        _sourcesMap.Add(_key, _id);
                        return true;
                    }
                }
            }
            return false;
        }
        protected abstract bool OpenInternal(int baudRate);

        public int Read(byte[] buffer, int bufferLen)
        {
            if (IsOpen)
            {
                return ReadInternal(buffer, bufferLen);
            }
            return -1;
        }
        protected abstract int ReadInternal(byte[] buffer, int bufferLen);

        public byte[] ReadExisting()
        {
            if (IsOpen)
            {
                return ReadExistingInternal();
            }
            return null;
        }
        protected abstract byte[] ReadExistingInternal();

        public int Write(byte[] buffer, int bufferLen)
        {
            if (IsOpen)
            {
                return WriteInternal(buffer, bufferLen);
            }
            return -1;
        }
        protected abstract int WriteInternal(byte[] buffer, int bufferLen);
    }
#endif
}