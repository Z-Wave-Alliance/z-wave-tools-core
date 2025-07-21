/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class EnumerableExt
    {
        public static void CopyTo<T>(this ICollection<T> source, ICollection<T> destination) where T : class
        {
            source.ThrowIfNull("source");
            destination.ThrowIfNull("destination");
            destination.Clear();
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            collection.ThrowIfNull("collection");
            enumerable.ThrowIfNull("enumerable");
            foreach (var cur in enumerable)
            {
                collection.Add(cur);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
