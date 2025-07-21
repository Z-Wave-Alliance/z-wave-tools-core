/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using ZWave.CommandClasses;

namespace ZWaveTests.CommandClassesProperties
{
    enum ByteSpecifiedTestEnum
    {
        Val1 = 1,
        Val2
    }

    [TestFixture]
    public class ByteSpecifiedStructTests
    {
        [Test]
        public void BaseOperationWithByteSpecified_AllInOne()
        {
            byte expected = 35;

            ByteValue actual = expected;
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected, actual.Value);

            //optioanl, operations with value:
            Assert.IsTrue(expected == actual);
            Assert.IsTrue(0x01 != actual);
            Assert.IsTrue(0x01 < actual);
            Assert.IsFalse(0x01 > actual);

            var a1 = (byte)(actual + 1);
            actual--;
            byte a2 = actual;

            Assert.AreEqual(expected + 1, a1);
            Assert.AreEqual(expected - 1, a2);
            Assert.AreNotSame(0x01, actual);

            actual = 0;
            Assert.IsTrue(actual.HasValue);
            actual = ByteValue.Empty;
            Assert.IsFalse(actual.HasValue);

            //if type changed check on build error:
            Assert.IsNotNull(actual);
            Assert.IsTrue(null != actual);
            Assert.IsFalse(null == actual);

            var cp = (byte)0x02;
            var enm = (ByteSpecifiedTestEnum)actual.Value;
            var val = true ? (ByteValue)cp : ByteValue.Empty;

            var ar1 = new[] { actual };
            var ar2 = new byte[] { actual };
            byte[] ar4 = new byte[] { actual };
            //byte[] ar3 = new[] { actual };

            //build errors:
            //var vala = true ? new ByteSpecified(cp) : ByteSpecified.Empty;
            //var enmF = (ByteSpecifiedTestEnum)actual;
            //var valF = true ? cp : ByteSpecified.Empty;
        }

    }
}
