/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Extensions
{
    public static class TaskExtensions
    {
        public static void DoForEach<T>(this IEnumerable<T> source, Action<T> func)
        {
            source.ThrowIfNull("source");
            var exceptions = new ConcurrentQueue<Exception>();
            Parallel.ForEach(source, (x) =>
            {
                try
                {
                    func(x);
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
            });
            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
            else
            {
                exceptions = null;
            }
        }
    }
}
