using Utils.Threading;
using NUnit.Framework;
using System.Threading;

namespace UtilsTests
{
    class CalledCount
    {
        public int Value { get; private set; }

        public int Increment()
        {
            return Value++;
        }
    }

    [TestFixture]
    public class ConsumerQueueTests
    {
        [Test]
        public void Add_WithoutTimeout_CallsActionFromQueue()
        {
            // Arrange.
            var consumerQueue = new ConsumerThread<CalledCount>();
            var calledCount = new CalledCount();

            // Act.
            consumerQueue.Start(0, "Test", ConsumerQueueCallback);
            for (int i = 0; i < 5; i++)
            {
                consumerQueue.Add(calledCount);
            }
            Thread.Sleep(400);
            consumerQueue.Dispose();

            // Assert.
            Assert.AreEqual(5, calledCount.Value);
        }

        //[Test]
        //public void StartAndStop_IsOpenReturnsCorrectValues()
        //{
        //    // Arrange.
        //    var consumerQueue = new ConsumerQueue<CalledCount>();

        //    // Act. // Assert.
        //    Assert.IsFalse(consumerQueue.IsOpen);
        //    consumerQueue.Start(ConsumerQueueCallback);
        //    Assert.IsTrue(consumerQueue.IsOpen);
        //    consumerQueue.Stop();
        //    Assert.IsFalse(consumerQueue.IsOpen);
        //}

        private void ConsumerQueueCallback(CalledCount calledCount)
        {
            calledCount.Increment();
        }

        private void ConsumerQueueCallbackWithTimeout(CalledCount calledCount)
        {
            Thread.Sleep(400);
            calledCount.Increment();
        }
    }
}
