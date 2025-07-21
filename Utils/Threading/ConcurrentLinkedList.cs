/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;

namespace Utils.Threading
{
    /// <summary>
    /// ThreadSafe Linked List. 
    /// Once foreach started all items added with AddFirst will be skipped during foreach 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentLinkedList<T>
    {
        public int MaxCapacity { get; set; }
        private readonly LinkedList<T> _innerList;
        private readonly object _lockObject = new object();

        public ConcurrentLinkedList() :
            this(100000)
        { }

        public ConcurrentLinkedList(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            _innerList = new LinkedList<T>();
        }

        public int Count
        {
            get
            {
                lock (_lockObject)
                {
                    return _innerList.Count;
                }
            }
        }

        public void AddFirst(T item)
        {
            lock (_lockObject)
            {
                _innerList.AddFirst(item);
            }
        }

        public void AddLast(T item)
        {
            lock (_lockObject)
            {
                _innerList.AddLast(item);
            }
        }

        public bool Remove(T item)
        {
            bool ret = false;
            lock (_lockObject)
            {
                if (_innerList.Contains(item))
                {
                    _innerList.Remove(item);
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsAddAllowed
        {
            get
            {
                lock (_lockObject)
                {
                    return _innerList.Count < MaxCapacity;
                }
            }
        }

        public void ForEach(Func<T, bool> action)
        {
            LinkedListNode<T> node;
            lock (_lockObject)
            {
                node = _innerList.First;
            }
            while (node != null)
            {
                LinkedListNode<T> current = node;
                lock (_lockObject)
                {
                    node = node.Next;
                }
                bool isRemove = action(current.Value);
                lock (_lockObject)
                {
                    if (isRemove)
                    {
                        _innerList.Remove(current);
                    }
                }
            }
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _innerList.Clear();
            }
        }

        public ConcurrentLinkedList<T> ReturnAndReset()
        {
            lock (_lockObject)
            {
                ConcurrentLinkedList<T> ret = new ConcurrentLinkedList<T>();
                ret._innerList.AddRange(_innerList);
                _innerList.Clear();
                return ret;
            }
        }
    }
}
