/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class SizeLimitedTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public int MaxSize { get; set; }

        private readonly List<KeyValuePair<TKey, TValue>> _table = new List<KeyValuePair<TKey, TValue>>();

        public int Count { get { return _table.Count; } }

        public TValue this[TKey key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException("The property is retrieved and key does not exist in the collection");
                }
                return _table[FindValueIndex(key)].Value;
            }

            set
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException("The property is retrieved and key does not exist in the collection");
                }
                _table[FindValueIndex(key)] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public SizeLimitedTable(int maxSize)
        {
            MaxSize = maxSize;
        }

        public bool ContainsKey(TKey key)
        {
            var val = _table.Where(pair => EqualityComparer<TKey>.Default.Equals(key, pair.Key));
            var count = val.Count(); 
            if (count == 0)
            {
                return false;
            }
            if (count != 1)
            {
                throw new ArgumentException("More than one element exists with the same key");
            }
            return true;
        }

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists");
            }
            if (_table.Count == MaxSize)
            {
                _table.RemoveAt(0);
            }
            _table.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                _table.RemoveAt(FindValueIndex(key));
                return true;
            }
            return false;
        }

        private int FindValueIndex(TKey key)
        {
            for (int i = 0; i < MaxSize; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(key, _table[i].Key))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Clear()
        {
            _table.Clear();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _table.GetEnumerator();
        }
    }
}
