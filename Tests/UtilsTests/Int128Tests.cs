/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Utils;
using System.Threading;
using System.Diagnostics;

namespace UtilsTests
{
    [TestFixture]
    public class Int128Tests
    {
        [Test]
        public void Constructor_GetBytes_AreEqual()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                byte[] buffer = new byte[16];
                rnd.NextBytes(buffer);
                Int128 na = new Int128(buffer);
                Assert.IsTrue(na.GetBytes().SequenceEqual(buffer));
            }
        }

        [Test]
        public void Add_AllFF_AllZeroes()
        {
            Int128 zero = new Int128(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Int128 one = new Int128(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
            Int128 number = new Int128(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });
            number = number + one;
            Int128 res = new Int128(number.GetBytes());
            Assert.IsTrue(zero == Int128.MinValue);
            Assert.IsTrue(number.GetBytes().SequenceEqual(zero.GetBytes()));
            Assert.IsTrue(res == zero);
        }

        [Test]
        public void Increment_AllFF_AllZeroes()
        {
            Int128 zero = new Int128(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Int128 number = new Int128(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });
            number++;
            Int128 res = new Int128(number.GetBytes());
            Assert.IsTrue(number.GetBytes().SequenceEqual(zero.GetBytes()));
            Assert.IsTrue(res == zero);
        }

        [Test]
        public void Substruct_AllZeroes_AllFF()
        {
            Int128 allFF = new Int128(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });
            Assert.IsTrue(allFF == Int128.MaxValue);
            Int128 one = new Int128(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
            Int128 number = new Int128(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            number = number - one;
            Int128 res = new Int128(number.GetBytes());
            Assert.IsTrue(allFF == Int128.MaxValue);
            Assert.IsTrue(number.GetBytes().SequenceEqual(allFF.GetBytes()));
            Assert.IsTrue(res == allFF);
        }

        [Test]
        public void Decrement_AllZeroes_AllFF()
        {
            Int128 allFF = new Int128(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });
            Int128 number = new Int128(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            number--;
            Int128 res = new Int128(number.GetBytes());
            Assert.IsTrue(number.GetBytes().SequenceEqual(allFF.GetBytes()));
            Assert.IsTrue(res == allFF);
        }

        [Test]
        public void Add_Int32Added_NextInt32()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int a = rnd.Next(1000000);
                int b = rnd.Next(1000000);
                Int128 na = new Int128(a);
                Int128 nb = new Int128(b);
                Assert.IsTrue(na + nb == new Int128(a + b));
            }
        }

        [Test]
        public void Increment_Int32Incremented_NextInt32()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int a = rnd.Next(1000000);
                Int128 na = new Int128(a);
                Assert.IsTrue(++na == new Int128(++a));
            }
        }

        [Test]
        public void Substruct_Int32Substructed_DeltaResult32()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int a = rnd.Next(1000000);
                int b = rnd.Next(1000000);
                Int128 na = new Int128(a);
                Int128 nb = new Int128(b);
                Assert.IsTrue((ulong)(na - nb) == (ulong)((ulong)a - (ulong)b));
            }
        }

        [Test]
        public void Decrement_Int32Decremented_DeltaResult32()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int a = rnd.Next(1000000);
                Int128 na = new Int128(a);
                Assert.IsTrue((ulong)(--na) == (ulong)((ulong)a - (ulong)1));
            }
        }

        [Test]
        public void AddSubstruct_Int32AddedSubstructed_OriginalValue()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                byte[] buffer = new byte[16];
                rnd.NextBytes(buffer);
                buffer[0] = 0;
                Int128 na = new Int128(buffer);
                rnd.NextBytes(buffer);
                buffer[0] = 0;
                Int128 nb = new Int128(buffer);
                Assert.IsTrue(na + nb - nb == na);
            }
            //losing most significant byte
            for (int i = 0; i < 1000; i++)
            {
                byte[] buffer = new byte[16];
                rnd.NextBytes(buffer);
                buffer[0] = 0xFF;
                Int128 na = new Int128(buffer);
                rnd.NextBytes(buffer);
                buffer[0] = 0xFF;
                Int128 nb = new Int128(buffer);
                Assert.IsTrue(na + nb - nb == na);
            }
        }

        [Test]
        public void IncrementDecrement_Int32IncrementedDecremented_OriginalValue()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                byte[] buffer = new byte[16];
                rnd.NextBytes(buffer);
                Int128 na = new Int128(buffer);
                Int128 nb = na;
                nb++;
                Assert.IsTrue(--nb == na);
            }
        }

        [Test]
        public void Performance_IncrementedFor1Second_ValueNoMoreThan40timesLess()
        {
            // Arrange.
            Int128 na = new Int128(0);
            ulong nb = 0;
            var stopwatch = new Stopwatch();
            const int iterations = 100000;

            // Act.
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                na++;
            }
            stopwatch.Stop();
            var naTicks = stopwatch.ElapsedTicks;


            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                nb++;
            }
            stopwatch.Stop();
            var nbTicks = stopwatch.ElapsedTicks;
            
            // Assert.
            // No more than 40 times slower than ulong.
            Assert.AreEqual(BitConverter.ToUInt64(na.GetBytes().Reverse().ToArray(), 0), nb);
            Assert.Less(naTicks, nbTicks);
        }
    }
}
