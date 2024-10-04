/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿
using System;
using Utils;
using NUnit.Framework;
using System.Collections.Generic;

namespace UtilsTests
{
    [TestFixture]
    public class SizeLimitedTableTests
    {
        [Test]
        public void ContainsKey_ItemWasAdded_ReturnsTrue()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(1);

            // Act.
            table.Add(1, "item");

            // Assert.
            Assert.IsTrue(table.ContainsKey(1));
        }

        [Test]
        public void ContainsKey_ItemWasntAdded_ReturnsFalse()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(1);

            // Act.
            table.Add(1, "item");

            // Assert.
            Assert.IsFalse(table.ContainsKey(2));
        }

        [Test]
        public void Add_AddItem_ItemWithSpecifiedKeyExists()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(2);

            // Act.
            table.Add(1, "item");
            table.Add(3, "item3");

            // Assert.
            Assert.AreEqual("item", table[1]);
            Assert.AreEqual("item3", table[3]);
        }

        [Test]
        public void Add_ItemWithSpecifiedKeyExists_ThrowsException()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(1);

            // Act.
            table.Add(1, "item");
            Assert.That(() => table.Add(1, "item"), Throws.TypeOf<ArgumentException>());

            // Assert.
        }

        [Test]
        public void Add_AddItemManyTimes_SizeLimitedBySpecifiedValueInConstructor()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(10);

            // Act.
            for (int i = 1; i <= 100; i++)
            {
                table.Add(i, "item" + i);
            }

            // Assert.
            Assert.AreEqual(10, table.Count);
            Assert.IsFalse(table.ContainsKey(90));
            Assert.AreEqual("item91", table[91]);
            Assert.AreEqual("item100", table[100]);
        }

        [Test]
        public void Indexer_NoItemWithSpecifiedKey_ThrowsKeyNotFoundException()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(2);

            // Act.
            table.Add(1, "item");
            table.Add(3, "item3");

            // Assert.
            string val;
            Assert.That(() => val = table[2], Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void Remove_HasItemWithSpecifiedKey_RemovesItemFromTable()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(2);

            // Act.
            table.Add(1, "item");
            table.Add(3, "item3");
            var res = table.Remove(1);

            // Assert.
            Assert.IsTrue(res);
            Assert.AreEqual(1, table.Count);
            Assert.AreEqual("item3", table[3]);
        }

        [Test]
        public void Remove_NoItemWithSpecifiedKey_NothingToRemoveFromTable()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(2);

            // Act.
            table.Add(1, "item");
            table.Add(3, "item3");
            var res = table.Remove(2);

            // Assert.
            Assert.IsFalse(res);
            Assert.AreEqual(2, table.Count);
        }

        [Test]
        public void Clear_HasItemsInTable_RemovesAllItemsFromTable()
        {
            // Arrange.
            var table = new SizeLimitedTable<int, string>(5);

            // Act.
            table.Add(1, "item");
            table.Add(2, "item2");
            table.Add(0, "item0");
            table.Add(4, "item4");
            table.Clear();

            // Assert.
            Assert.AreEqual(0, table.Count);
        }
    }
}
