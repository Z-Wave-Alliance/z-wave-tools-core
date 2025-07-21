/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class OrderComparer<T> : IComparer<T>
    {
        private IDictionary<T, int> _order = new Dictionary<T, int>();
        private bool _isDesc = false;
        public OrderComparer(IEnumerable<T> order)
            : this(order, false)
        {
        }

        public OrderComparer(IEnumerable<T> order, bool isDesc)
        {
            _isDesc = isDesc;
            int index = _isDesc ? order.Count() : 1;
            foreach (var item in order)
            {
                _order.Add(item, index);
                if (isDesc)
                {
                    index--;
                }
                else
                {
                    index++;
                }
            }
        }

        public int Compare(T x, T y)
        {
            if (!EqualityComparer<T>.Default.Equals(x, default(T)) && _order.ContainsKey(x))
            {
                if (!EqualityComparer<T>.Default.Equals(y, default(T)) && _order.ContainsKey(y))
                {
                    return _order[x].CompareTo(_order[y]);
                }
                else
                {
                    return -1;
                }
            }
            else if (!EqualityComparer<T>.Default.Equals(y, default(T)) && _order.ContainsKey(y))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
