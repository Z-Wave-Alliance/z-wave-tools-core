using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Utils;
using System.Diagnostics;

namespace UtilsTests
{
    [TestFixture]
    public class BigIntegerTests
    {
        [Test]
        public void Increment_AllFFsIncrementOnce_NumberIsZero()
        {
            // Arrange.
            BigInteger number = BigInteger.MaxValue;

            // Act.
            BigInteger res = number.Increment();

            // Assert.
            Assert.IsTrue(res == BigInteger.Zero);
        }

        [Test]
        public void Increment_AllZeroesIncrement100Times_NumberIs100()
        {
            // Arrange.
            BigInteger number = BigInteger.Zero;

            // Act.
            for (int i = 0; i < 100; i++)
                number.Increment();
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100 }));
        }

        [Test]
        public void Increment_AllZeroesIncrement256Times_SetOneToTheNextByte()
        {
            // Arrange.
            BigInteger number = BigInteger.Zero;

            // Act.
            for (int i = 0; i < 256; i++)
                number.Increment();
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }));
        }

        [Test]
        public void Decrement_NumberOneDecrementOnce_NumberIsZero()
        {
            // Arrange.
            BigInteger number = new BigInteger(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });

            // Act.
            number.Decrement();
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(BigInteger.Zero.GetBytes()));
        }

        [Test]
        public void Decrement_AllZeroDecrementOnce_NumberAllFFs()
        {
            // Arrange.
            BigInteger number = BigInteger.Zero;

            // Act.
            number.Decrement();
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(BigInteger.MaxValue.GetBytes()));
        }

        [Test]
        public void Decrement_LastByteIsZero_DecrementsNextByteLastIsFF()
        {
            // Arrange.
            BigInteger number = new BigInteger(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 37, 0 });

            // Act.
            number.Decrement();
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 36, 255 }));
        }

        [Test]
        public void GetBytes_InitializedWithLessThan16BytesArray_ReturnsCorrect16BytesArray()
        {
            // Arrange.
            BigInteger number = new BigInteger(new byte[] { 1, 0, 1 });

            // Act.
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }));
        }

        [Test]
        public void GetBytes_InitializedWithMoreThan16BytesArray_ReturnsCorrect16BytesArray()
        {
            // Arrange.
            BigInteger number = new BigInteger(new byte[] { 1, 1, 1, 1, 0x14, 0xa8, 0x26, 0x24, 0x15, 0x21, 0x3e, 0xfc, 0x65, 0xea, 0x50, 0xf2, 0x0a, 0x2f, 0x70, 0x68 });

            // Act.
            var bytes = number.GetBytes();

            // Assert.
            Assert.IsTrue(bytes.SequenceEqual(new byte[] { 0x14, 0xa8, 0x26, 0x24, 0x15, 0x21, 0x3e, 0xfc, 0x65, 0xea, 0x50, 0xf2, 0x0a, 0x2f, 0x70, 0x68 }));
        }

        [Test]
        public void Performance_IncrementedFor1Second_ValueNoMoreThan40timesLess()
        {
            var stopwatch = new Stopwatch();
            var bInt = BigInteger.Zero;
            ulong nb = 0;
            const int iterations = 100000;

            // Act.
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                bInt.Increment();
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

            ulong na = BitConverter.ToUInt64(bInt.GetBytes().Skip(8).Take(8).Reverse().ToArray(), 0);
            Assert.AreEqual(na, nb);
            Assert.Less(naTicks, nbTicks);
        }
    }
}
