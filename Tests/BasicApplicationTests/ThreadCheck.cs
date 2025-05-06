using NUnit.Framework;
using System.Diagnostics;
using System.Threading;

namespace BasicApplicationTests
{
    [TestFixture]
    public class ThreadCheck
    {
        [Test]
        public void Test1()
        {
            AutoResetEvent signal = new AutoResetEvent(false);
            var sw = new Stopwatch();
            sw.Start();
            Thread t = new Thread(() => 
            {
                Thread.Sleep(20);
                signal.Set();
            });
            t.Start();
            var ret = signal.WaitOne(200);
            sw.Stop();
            Assert.IsTrue(ret);
            Assert.IsTrue(sw.ElapsedMilliseconds < 100);
            Assert.IsTrue(sw.ElapsedMilliseconds >= 20);
        }
    }
}
