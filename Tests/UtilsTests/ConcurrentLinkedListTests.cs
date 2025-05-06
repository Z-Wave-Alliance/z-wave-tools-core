using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Utils.Threading;
using Utils;
using System.Threading;

namespace UtilsTests
{
    [TestFixture]
    public class ConcurrentLinkedListTests
    {
        [Test]
        public void AddNewItems_CountIsValid()
        {
            // Arrange.
            var consumerQueue = new ConcurrentLinkedList<IntRef>();

            // Act.
            consumerQueue.AddLast(new IntRef(0xFE));
            consumerQueue.AddFirst(new IntRef(0x01));
            consumerQueue.AddLast(new IntRef(0xFF));

            // Assert.
            Assert.AreEqual(3, consumerQueue.Count);
        }

        [Test]
        public void Add_ForeachCheck_CountDecreased()
        {
            // Arrange.
            var consumerQueue = new ConcurrentLinkedList<IntRef>();
            consumerQueue.AddLast(new IntRef(0xFE));
            consumerQueue.AddFirst(new IntRef(0x01));
            consumerQueue.AddLast(new IntRef(0xFF));

            // Act.
            consumerQueue.ForEach(x => x.Value == 0x01);

            // Assert.
            Assert.AreEqual(2, consumerQueue.Count);
        }


        [Test]
        public void Add_Multithread_NoException()
        {
            // Arrange.
            var list = new ConcurrentLinkedList<IntRef>();
            int count = 1000000;

            // Act.
            var producerFirstWork = new Thread(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    list.AddFirst(new IntRef(0x01));
                }
            });

            var producerLastWork = new Thread(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    list.AddLast(new IntRef(0xFF));
                }
            });

            var consumerWork = new Thread(() =>
            {
                int itemFFcount = 0;
                while (itemFFcount != count)
                {
                    itemFFcount = 0;
                    list.ForEach((x) =>
                    {
                        if (x.Value == 0x01)
                        {
                            return true;
                        }
                        else
                        {
                            itemFFcount++;
                            return false;
                        }
                    });
                }
            });

            producerFirstWork.Start();
            consumerWork.Start();
            producerLastWork.Start();
            producerFirstWork.Join();
            producerLastWork.Join();
            consumerWork.Join();
            
            list.ForEach(x => x.Value == 0x01);

            // Assert.
            Assert.AreEqual(count, list.Count);
        }
    }
}
