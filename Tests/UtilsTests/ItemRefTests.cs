/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;
using Utils.UI;
using Utils;
using System.Collections;

namespace UtilsTests
{
    [TestFixture]
    public class ItemRefTests
    {
        [Test]
        public void MemorySizeOnlyOne()
        {
            int number = 100000;
            Thread.MemoryBarrier();
            GC.Collect();
            long memoryBefore = GC.GetTotalMemory(true);
            List<IntRef> list = new List<IntRef>(number);
            GC.Collect();
            long memory1 = GC.GetTotalMemory(false);
            for (int i = 0; i < number; i++)
            {
                var current = new IntRef(int.MaxValue);
                list.Add(current);
            }
            GC.Collect();
            long memoryAfter = GC.GetTotalMemory(false);
            Thread.MemoryBarrier();

            var size = Math.Round((float)((memoryAfter - memoryBefore) / (double)number));
            Console.Out.WriteLine("Total of {0} items: {1} bytes container: {2}, per item {3}",
                number,
                memoryAfter - memory1,
                memory1 - memoryBefore,
                size);
        }

        [Test]
        public void MemorySizeOnlyOne2()
        {
            int number = 1000;
            Thread.MemoryBarrier();
            GC.Collect();
            long memoryBefore = GC.GetTotalMemory(true);
            List<BitArray> list = new List<BitArray>(number);
            GC.Collect();
            long memory1 = GC.GetTotalMemory(false);
            for (int i = 0; i < number; i++)
            {
                var current = new BitArray(1000000);
                list.Add(current);
            }
            GC.Collect();
            long memoryAfter = GC.GetTotalMemory(false);
            Thread.MemoryBarrier();

            var size = Math.Round((float)((memoryAfter - memoryBefore) / (double)number));
            Console.Out.WriteLine("Total of {0} items: {1} bytes container: {2}, per item {3}",
                number,
                memoryAfter - memory1,
                memory1 - memoryBefore,
                size);
        }

        [Test]
        public void MemorySize()
        {
            int number = 100000;
            Thread.MemoryBarrier();
            GC.Collect();
            long memoryBefore = GC.GetTotalMemory(true);
            List<ItemRefBase> list = new List<ItemRefBase>(number);
            GC.Collect();
            long memory1 = GC.GetTotalMemory(false);
            for (int i = 0; i < number; i++)
            {
                ItemRefBase current;
                if (i % 3 == 0)
                    current = new LinkedItemRef();
                else if (i % 5 == 0)
                    current = new RoutedItemRef();
                else if (i % 7 == 0)
                    current = new RoutedLinkedItemRef();
                else if (i % 8 == 0)
                    current = new GroupItemRef();
                else
                    current = new ItemRef();
                list.Add(current);
            }
            GC.Collect();
            long memoryAfter = GC.GetTotalMemory(false);
            Thread.MemoryBarrier();


            Console.Out.WriteLine("Total of {0} items: {1} bytes container: {2}, per item {3}",
                number,
                memoryAfter - memory1,
                memory1 - memoryBefore,
                Math.Round((float)((memoryAfter - memoryBefore) / number)));
        }

        [Test]
        public void NewList_GroupItems_NoCrashes()
        {
            int number = 100000;
            List<ItemRefBase> list = new List<ItemRefBase>(number);
            for (int i = 0; i < number; i++)
            {
                ItemRefBase current;
                current = new ItemRef();
                list.Add(current);
            }

            GroupItemRef group = new GroupItemRef(list[1], list[3], list[5]);
            list.Insert(1, group);
        }

    }

    public class GroupItemRef : ItemRefBase
    {
        public ItemRefBase[] Items { get; set; }
        public GroupItemInfo Info { get; set; }
        public GroupItemRef(params ItemRefBase[] items)
        {
            Items = items;
            if (Items != null)
                foreach (var item in Items)
                {
                    //item.
                }
        }
    }

    public class PositionRef
    {
        public int Position { get; set; }
    }

    public class LinkedItemRef : ItemRef
    {
        public ItemRef Parent { get; set; }
    }

    public class RoutedLinkedItemRef : RoutedItemRef
    {
        public ItemRef Parent { get; set; }
    }

    public class ItemRef : ItemRefBase
    {
        public int Position { get; set; }
        public ItemInfo Info { get; set; }
    }

    public class RoutedItemRef : ItemRef
    {
        public RoutedItemInfo RoutedInfo { get; set; }
    }

    public class ItemRefBase : EntityBase
    {
        public bool IsSelected
        {
            get { return false; }
            set
            {
                Notify("IsSelected");
            }
        }

        public bool IsVisible
        {
            get { return false; }
            set
            {
                Notify("IsVisible");
            }
        }
    }

    public struct ItemInfo
    {
        /// <summary>
        /// selected,     hidden,       shortHeaderType(3bits),        api(3bits)
        /// </summary>
        public byte Properties { get; set; }
        public byte HomeTag { get; set; }           // do not display it explicitly, this is overlapping search index
        public byte RouteTag { get; set; }          // do not display it explicitly, this is overlapping search index
        public byte DataTag { get; set; }           // do not display it explicitly, this is overlapping search index
    }

    public struct GroupItemInfo
    {
        public byte Properties { get; set; }
        public byte HomeTag { get; set; }           // do not display it explicitly, this is overlapping search index  
        public byte GroupRouteTag { get; set; }     // do not display it explicitly, this is overlapping search index
        public byte GroupType { get; set; }
    }

    public struct RoutedItemInfo
    {
        public byte AA { get; set; }
        public byte BB { get; set; }
        public byte CC { get; set; }
        public byte DD { get; set; }
    }
}
