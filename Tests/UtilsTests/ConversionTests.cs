/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Utils;
using System.Diagnostics;

namespace UtilsTests
{
    [TestFixture]
    public class ConversionTests
    {
        [Test]
        public void GetInt32_FromByteArray_ExpectedResult()
        {
            Assert.AreEqual(1234, Tools.GetInt32(new byte[] { 0x04, 0xD2 }));
            Assert.AreEqual(12345, Tools.GetInt32(new byte[] { 0x30, 0x39 }));
            Assert.AreEqual(123456, Tools.GetInt32(new byte[] { 0x01, 0xE2, 0x40 }));
            Assert.AreEqual(1234567, Tools.GetInt32(new byte[] { 0x12, 0xD6, 0x87 }));
            Assert.AreEqual(12345678, Tools.GetInt32(new byte[] { 0xBC, 0x61, 0x4E }));
            Assert.AreEqual(123456789, Tools.GetInt32(new byte[] { 0x07, 0x5B, 0xCD, 0x15 }));
        }

        [Test]
        public void GetBytes_FromInt32_ExpectedResult()
        {
            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x7B }, Tools.GetBytes(123));
            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x04, 0xD2 }, Tools.GetBytes(1234));
            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x30, 0x39 }, Tools.GetBytes(12345));
            Assert.AreEqual(new byte[] { 0x00, 0x01, 0xE2, 0x40 }, Tools.GetBytes(123456));
            Assert.AreEqual(new byte[] { 0x00, 0x12, 0xD6, 0x87 }, Tools.GetBytes(1234567));
            Assert.AreEqual(new byte[] { 0x00, 0xBC, 0x61, 0x4E }, Tools.GetBytes(12345678));
            Assert.AreEqual(new byte[] { 0x07, 0x5B, 0xCD, 0x15 }, Tools.GetBytes(123456789));
        }

        [Test]
        public void Convert_ToAndFromInt32_ReturnTheSameResult()
        {
            int exp_value = 123456789;
            byte[] exp_bytes = new byte[] { 0x07, 0x5B, 0xCD, 0x15 };
            //1.from int to byte[]
            byte[] act_bytes = Tools.GetBytes(exp_value);
            Assert.AreEqual(exp_bytes, act_bytes);
            //2.Back from byte[] to int
            int act_value = Tools.GetInt32(act_bytes);
            Assert.AreEqual(exp_value, act_value);
            //3.Again from int to byte[]
            act_bytes = Tools.GetBytes(act_value);
            Assert.AreEqual(exp_bytes, act_bytes);
        }


        [TestCase(new byte[] { 0x21, 0x01 }, 0, 4, new byte[] { 0x02 })]
        [TestCase(new byte[] { 0x21, 0x31 }, 4, 12, new byte[] { 0x01, 0x31 })]
        [TestCase(new byte[] { 0x21, 0x3F, 0xFF }, 0, 12, new byte[] { 0x02, 0x13 })]
        [TestCase(new byte[] { 0x10, 0x1F, 0xFF }, 12, 12, new byte[] { 0x0F, 0xFF })]
        public void GetBytesFromByteArray_tst(byte[] array, byte offset, byte lenght, byte[] expectedResult)
        {
            var opres = Tools.GetBytesFromByteArray(array, offset, lenght);
            Assert.AreEqual(expectedResult, opres);
        }

    }
}
