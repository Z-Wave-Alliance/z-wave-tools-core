/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace UtilsTests
{
    [TestFixture]
    class ComparerTests
    {
        [Test]
        public void OrderComparer_Set1_Test()
        {
            var order = new[] { 2, 1, 5, 3, 8, 7, 9 };
            var input = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var ascComparer = new OrderComparer<int>(order);
            var descComparer = new OrderComparer<int>(order, true);

            var ascRes = input.OrderBy(x => x, ascComparer).ToArray();
            var descRes = input.OrderBy(x => x, descComparer).ToArray();

            Assert.That(ascRes, Is.EquivalentTo(input));
            Assert.That(ascRes, Is.EqualTo(order.Intersect(input).Concat(input).Distinct()));

            Assert.That(descRes, Is.EquivalentTo(input));
            Assert.That(descRes, Is.EqualTo(order.Reverse().Intersect(input).Concat(input).Distinct()));
        }

        [Test]
        public void OrderComparer_Set2_Test()
        {
            var order = new[] { 2, 1, 5, 66 };
            var input = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var ascComparer = new OrderComparer<int>(order);
            var descComparer = new OrderComparer<int>(order, true);

            var ascRes = input.OrderBy(x => x, ascComparer).ToArray();
            var descRes = input.OrderBy(x => x, descComparer).ToArray();

            Assert.That(ascRes, Is.EquivalentTo(input));
            Assert.That(ascRes, Is.EqualTo(order.Intersect(input).Concat(input).Distinct()));

            Assert.That(descRes, Is.EquivalentTo(input));
            Assert.That(descRes, Is.EqualTo(order.Reverse().Intersect(input).Concat(input).Distinct()));
        }

        [Test]
        public void OrderComparer_Set3_Test()
        {
            var order = new[] { 32, 41, 55, 66 };
            var input = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var ascComparer = new OrderComparer<int>(order);
            var descComparer = new OrderComparer<int>(order, true);

            var ascRes = input.OrderBy(x => x, ascComparer).ToArray();
            var descRes = input.OrderBy(x => x, descComparer).ToArray();

            Assert.That(ascRes, Is.EquivalentTo(input));
            Assert.That(ascRes, Is.EqualTo(order.Intersect(input).Concat(input).Distinct()));

            Assert.That(descRes, Is.EquivalentTo(input));
            Assert.That(descRes, Is.EqualTo(order.Reverse().Intersect(input).Concat(input).Distinct()));
        }

        [Test]
        public void OrderComparer_Set4_Test()
        {
            var order = new int[0];
            var input = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var ascComparer = new OrderComparer<int>(order);
            var descComparer = new OrderComparer<int>(order, true);

            var ascRes = input.OrderBy(x => x, ascComparer).ToArray();
            var descRes = input.OrderBy(x => x, descComparer).ToArray();

            Assert.That(ascRes, Is.EquivalentTo(input));
            Assert.That(ascRes, Is.EqualTo(order.Intersect(input).Concat(input).Distinct()));

            Assert.That(descRes, Is.EquivalentTo(input));
            Assert.That(descRes, Is.EqualTo(order.Reverse().Intersect(input).Concat(input).Distinct()));
        }

        [Test]
        public void OrderComparer_Set5_Test()
        {
            var order = new[] { 32, 41, 55, 66 };
            var input = new int[0];

            var ascComparer = new OrderComparer<int>(order);
            var descComparer = new OrderComparer<int>(order, true);

            var ascRes = input.OrderBy(x => x, ascComparer).ToArray();
            var descRes = input.OrderBy(x => x, descComparer).ToArray();

            Assert.That(ascRes, Is.EquivalentTo(input));
            Assert.That(ascRes, Is.EqualTo(order.Intersect(input).Concat(input).Distinct()));

            Assert.That(descRes, Is.EquivalentTo(input));
            Assert.That(descRes, Is.EqualTo(order.Reverse().Intersect(input).Concat(input).Distinct()));
        }
    }
}
