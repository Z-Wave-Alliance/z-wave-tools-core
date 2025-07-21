/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace Utils
{
    public class BiDictionary<T1, T2>
    {
        private readonly Dictionary<T1, T2> _keys = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _values = new Dictionary<T2, T1>();

        public bool ContainsKey(T1 key)
        {
            return _keys.ContainsKey(key);
        }

        public bool Containsvalue(T2 value)
        {
            return _values.ContainsKey(value);
        }

        public T2 GetValue(T1 key)
        {
            return _keys[key];
        }

        public T1 GetKey(T2 value)
        {
            return _values[value];
        }

        public bool Bind(T1 key, T2 value, bool isOverWrite)
        {
            bool ret = false;
            if (isOverWrite)
            {
                if (_keys.ContainsKey(key))
                {
                    if (_values.ContainsKey(_keys[key]))
                        _values.Remove(_keys[key]);
                    _keys[key] = value;
                }
                else
                {
                    _keys.Add(key, value);
                }

                if (_values.ContainsKey(value))
                {
                    if (_keys.ContainsKey(_values[value]))
                        _keys.Remove(_values[value]);
                    _values[value] = key;
                }
                else
                {
                    _values.Add(value, key);
                }
                ret = true;
            }
            else
            {
                if (_keys.ContainsKey(key))
                {
                    if (!_values.ContainsKey(value))
                    {
                        if (_values.ContainsKey(_keys[key]))
                            _values.Remove(_keys[key]);
                        _keys[key] = value;
                        _values.Add(value, key);
                        ret = true;
                    }
                }
                else
                {
                    if (!_values.ContainsKey(value))
                    {
                        _keys.Add(key, value);
                        _values.Add(value, key);
                        ret = true;
                    }
                }
            }
            return ret;
        }

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public int Count
        {
            get
            {
                return _keys.Count;
            }
        }
    }
}
