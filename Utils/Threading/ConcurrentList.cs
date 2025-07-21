/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace Utils.Threading
{
    public class ConcurrentList<T>
    {
        public int MaxCapacity { get; set; }
        private readonly List<T> _innerList;
        private readonly object _mLockObject = new object();

        public ConcurrentList() :
            this(100000)
        { }

        public ConcurrentList(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            _innerList = new List<T>(MaxCapacity);
        }

        public int Count
        {
            get
            {
                lock (_mLockObject)
                {
                    return _innerList.Count;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_mLockObject)
                {
                    if (_innerList.Count > index)
                        return _innerList[index];
                    return default(T);
                }
            }
        }

        public void Add(T item)
        {
            lock (_mLockObject)
            {
                _innerList.Add(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_mLockObject)
            {
                if (_innerList.Count > index)
                    _innerList.RemoveAt(index);
            }
        }

        public bool IsAddAllowed
        {
            get
            {
                lock (_mLockObject)
                {
                    return _innerList.Count < MaxCapacity;
                }
            }
        }

        public void Clear()
        {
            lock (_mLockObject)
            {
                _innerList.Clear();
            }
        }

        public T[] ToArray()
        {
            lock (_mLockObject)
            {
                return _innerList.ToArray();
            }
        }

        public ConcurrentList<T> ReturnAndReset()
        {
            lock (_mLockObject)
            {
                ConcurrentList<T> ret = new ConcurrentList<T>();
                ret._innerList.AddRange(_innerList);
                _innerList.Clear();
                return ret;
            }
        }
    }
}
