using System;
using Utils;
using NUnit.Framework;

namespace UtilsTests
{
    [Flags]
    internal enum FakeByteFlags : byte
    {
        Flag1 = 0x01,
        Flag2 = 0x02,
        Flag3 = 0x04,
        Flag4 = 0x08,
        Flag5 = 0x10,
        Flag6 = 0x20,
        Flag7 = 0x40,
        Flag8 = 0x80
    }

    [Flags]
    internal enum FakeIntFlags : int
    {
        Flag1 = 0x01,
        Flag2 = 0x80,
        Flag3 = 0x8000,
        Flag4 = 0x800000
    }

    [Flags]
    internal enum FakeLongFlags : long
    {
        Flag1 = 0x8000000000,
        Flag2 = 0x80000000000,
        Flag3 = 0x800000000000
    }

    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetByteFlagIndex_ByteFlags_ReturnsCorrectIndexStartingFromZero()
        {
            Assert.AreEqual(0, FakeByteFlags.Flag1.GetByteFlagIndex());
            Assert.AreEqual(1, FakeByteFlags.Flag2.GetByteFlagIndex());
            Assert.AreEqual(2, FakeByteFlags.Flag3.GetByteFlagIndex());
            Assert.AreEqual(3, FakeByteFlags.Flag4.GetByteFlagIndex());
            Assert.AreEqual(4, FakeByteFlags.Flag5.GetByteFlagIndex());
            Assert.AreEqual(5, FakeByteFlags.Flag6.GetByteFlagIndex());
            Assert.AreEqual(6, FakeByteFlags.Flag7.GetByteFlagIndex());
            Assert.AreEqual(7, FakeByteFlags.Flag8.GetByteFlagIndex());
            Assert.AreNotEqual(7, FakeByteFlags.Flag5.GetByteFlagIndex());
        }

        [Ignore("")]
        [Test]
        public void GetByteFlagIndex_IntFlags_ReturnsCorrectIndexStartingFromZero()
        {
            Assert.AreEqual(0, FakeIntFlags.Flag1.GetByteFlagIndex());
            Assert.AreEqual(7, FakeIntFlags.Flag2.GetByteFlagIndex());
            Assert.AreEqual(15, FakeIntFlags.Flag3.GetByteFlagIndex());
            Assert.AreEqual(23, FakeIntFlags.Flag4.GetByteFlagIndex());
            Assert.AreNotEqual(0, FakeIntFlags.Flag4.GetByteFlagIndex());
        }

        [Test]
        public void GetByteFlagIndex_LongFlags_ThrowsExceptionAsLongIsNotSupported()
        {
            Assert.That(() => FakeLongFlags.Flag1.GetByteFlagIndex(), Throws.TypeOf<NotImplementedException>());
        }
        
        [Test]
        [Ignore("")]
        public void HasFlag_MakeAggregatedByteFlag_HasSpecificValue()
        {
            var aggFlag = FakeByteFlags.Flag1 | FakeByteFlags.Flag3 | FakeByteFlags.Flag5;
            Assert.IsTrue(aggFlag.HasFlag(FakeByteFlags.Flag1));
            Assert.IsTrue(aggFlag.HasFlag(FakeByteFlags.Flag3));
            Assert.IsTrue(aggFlag.HasFlag(FakeByteFlags.Flag5));
            Assert.IsFalse(aggFlag.HasFlag(FakeByteFlags.Flag4));
        }

        [Test]
        [Ignore("")]
        public void HasFlag_MakeAggregatedIntFlag_HasSpecificValue()
        {
            var aggFlag = FakeIntFlags.Flag1 | FakeIntFlags.Flag2 | FakeIntFlags.Flag4;
            Assert.IsTrue(aggFlag.HasFlag(FakeIntFlags.Flag1));
            Assert.IsTrue(aggFlag.HasFlag(FakeIntFlags.Flag2));
            Assert.IsTrue(aggFlag.HasFlag(FakeIntFlags.Flag4));
            Assert.IsFalse(aggFlag.HasFlag(FakeIntFlags.Flag3));
        }

        [Test]
        [Ignore("")]
        public void HasFlag_MakeAggregatedLongFlag_ThrowsExceptionAsLongIsNotSupported()
        {
            var aggFlag = FakeLongFlags.Flag1 | FakeLongFlags.Flag2;
            Assert.That(() => aggFlag.HasFlag(FakeLongFlags.Flag1), Throws.TypeOf<NotImplementedException>());
        }
    }
}
