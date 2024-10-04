/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Utils;

namespace UtilsTests
{
    [TestFixture]
    public class BitArrayTests
    {
        private int _initialCapacity = 1000;

        [Test]
        public void VariantBitArray_SetValue_Resize()
        {
            // Arrange.
            VariantBitArray array = new VariantBitArray(_initialCapacity);

            // Act.
            for (int i = 0; i < _initialCapacity; i++)
            {
                array.SetPosition(i);
            }
            array.SetPosition(_initialCapacity);

            // Assert.
            for (int i = 1; i < _initialCapacity; i++)
            {
                var pi = array.GetPrevious(i);
                Assert.AreEqual(i - 1, pi);
            }

            var ppi = array.GetPrevious(_initialCapacity);
            Assert.AreEqual(_initialCapacity - 1, ppi);

            for (int i = 0; i < _initialCapacity - 1; i++)
            {
                var pi = array.GetNext(i);
                Assert.AreEqual(i + 1, pi);
            }

            var npi = array.GetNext(_initialCapacity);
            Assert.AreEqual(-1, npi);
        }

        [Test]
        public void VariantBitArray_SetValue_Clear()
        {
            // Arrange.
            VariantBitArray array = new VariantBitArray(_initialCapacity);

            // Act.
            for (int i = 0; i < _initialCapacity; i++)
            {
                array.SetPosition(i);
            }
            array.Clear();

            // Assert.
            for (int i = 1; i < _initialCapacity; i++)
            {
                var pi = array.GetPrevious(i);
                Assert.AreEqual(-1, pi);
            }

            for (int i = 0; i < _initialCapacity - 1; i++)
            {
                var pi = array.GetNext(i);
                Assert.AreEqual(-1, pi);
            }
        }

        [Test]
        public void VariantBitArray_SetValue_GetPreviousValue()
        {
            // Arrange.
            VariantBitArray array = new VariantBitArray(_initialCapacity);

            // Act.
            for (int i = 0; i < _initialCapacity; i++)
            {
                array.SetPosition(i);
            }

            // Assert.
            for (int i = 1; i < _initialCapacity; i++)
            {
                var pi = array.GetPrevious(i);
                Assert.AreEqual(i - 1, pi);
            }

            var fpi = array.GetPrevious(0);
            Assert.AreEqual(-1, fpi);

            fpi = array.GetPrevious(-1);
            Assert.AreEqual(-1, fpi);

            fpi = array.GetPrevious(_initialCapacity + 1);
            Assert.AreEqual(_initialCapacity - 1, fpi);
        }

        [Test]
        public void VariantBitArray_SetValue_GetNextValue()
        {
            // Arrange.
            VariantBitArray array = new VariantBitArray(_initialCapacity);

            // Act.
            for (int i = 0; i < _initialCapacity; i++)
            {
                array.SetPosition(i);
            }

            // Assert.
            for (int i = 0; i < _initialCapacity - 1; i++)
            {
                var pi = array.GetNext(i);
                Assert.AreEqual(i + 1, pi);
            }

            var fpi = array.GetNext(_initialCapacity);
            Assert.AreEqual(-1, fpi);

            fpi = array.GetNext(_initialCapacity + 1);
            Assert.AreEqual(-1, fpi);

            fpi = array.GetNext(0 - 1);
            Assert.AreEqual(0, fpi);
        }

        [Test]
        public void BitArray256_SetValueClearValue_GetValue()
        {
            BitArray256 array = new BitArray256();
            for (int i = 0; i < BitArray256.MaxCount; i++)
            {
                array[i] = true;
                Assert.IsTrue(array[i]);
                Assert.AreEqual(1, array.GetValuesCount());

                array[i] = false;
                Assert.IsFalse(array[i]);
                Assert.AreEqual(0, array.GetValuesCount());
            }
        }

        [Test]
        public void BitArray256_AllSetSetValue_GetValue()
        {
            BitArray256 array = new BitArray256();
            for (int i = 0; i < BitArray256.MaxCount; i++)
            {
                array.SetAll(false);
                array[i] = true;
                Assert.IsTrue(array[i]);
                Assert.AreEqual(1, array.GetValuesCount());

                array.SetAll(true);
                array[i] = false;
                Assert.IsFalse(array[i]);
                Assert.AreEqual(BitArray256.MaxCount - 1, array.GetValuesCount());
            }
        }

        [Test]
        public void BitArray256_SetValue_GetValue()
        {
            BitArray256 array = new BitArray256();
            for (int i = 0; i < BitArray256.MaxCount; i++)
            {
                array[i] = true;
                Assert.IsTrue(array[i]);
                Assert.AreEqual(i + 1, array.GetValuesCount());
            }

            for (int i = 0; i < BitArray256.MaxCount; i++)
            {
                array[i] = false;
                Assert.IsFalse(array[i]);
                Assert.AreEqual(BitArray256.MaxCount - 1 - i, array.GetValuesCount());
            }
        }

        [Test]
        public void BitArray8_SetValueClearValue_GetValue()
        {
            BitArray8 array = new BitArray8();
            for (int i = 0; i < BitArray8.MaxCount; i++)
            {
                array[i] = true;
                Assert.IsTrue(array[i]);
                Assert.AreEqual(1, array.GetValuesCount());

                array[i] = false;
                Assert.IsFalse(array[i]);
                Assert.AreEqual(0, array.GetValuesCount());
            }
        }

        [Test]
        public void BitArray8_AllSetSetValue_GetValue()
        {
            BitArray8 array = new BitArray8();
            for (int i = 0; i < BitArray8.MaxCount; i++)
            {
                array.SetAll(false);
                array[i] = true;
                Assert.IsTrue(array[i]);
                Assert.AreEqual(1, array.GetValuesCount());

                array.SetAll(true);
                array[i] = false;
                Assert.IsFalse(array[i]);
                Assert.AreEqual(BitArray8.MaxCount - 1, array.GetValuesCount());
            }
        }

        [Test]
        public void BitArray8_SetValue_GetValue()
        {
            BitArray8 array = new BitArray8();
            for (int i = 0; i < BitArray8.MaxCount; i++)
            {
                array[i] = true;
                Assert.IsTrue(array[i]);
                Assert.AreEqual(i + 1, array.GetValuesCount());
            }

            for (int i = 0; i < BitArray8.MaxCount; i++)
            {
                array[i] = false;
                Assert.IsFalse(array[i]);
                Assert.AreEqual(BitArray8.MaxCount - 1 - i, array.GetValuesCount());
            }
        }
    }
}
