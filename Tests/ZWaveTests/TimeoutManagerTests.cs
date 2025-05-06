using NUnit.Framework;
using ZWave;
using ZWave.Layers;
using System.Threading;
using System.Collections.Generic;

namespace ZWaveTests
{
    //[TestFixture]
    public class TimeoutManagerTests
    {
        private ITimeoutManager _timeoutManager;

        [SetUp]
        public void SetUp()
        {
            _timeoutManager = new TimeoutManager();
        }

        [TearDown]
        public void TearDown()
        {
            _timeoutManager.Dispose();
        }

        //[Test]
        //public void AddTimer_ActionTokenBecomesExpired_CallsSessionClientHandleTimeElapsed()
        //{
        //    // Arrange.
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeInterval = new TimeInterval(0, 0, 200);

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    _timeoutManager.AddTimer(timeInterval);
        //    Thread.Sleep(400);

        //    // Assert.
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeInterval));
        //}

        [Test]
        public void AddTimer_ActionTokenWithoutTimeout_ActionTokenAdded()
        {
            // Arrange.
            int expectedCountOfTokens = 1;
            var timeInterval = new TimeInterval(0, 0, 0);

            // Act.
            _timeoutManager.AddTimer(timeInterval);

            // Assert.
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        //[Test]
        //public void StartTimer_ActionTokenWithoutTimeout_ActionTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 0;
        //    var sessionClientMock = Substitute.For<ISessionClient>();

        //    var timeInterval = new TimeInterval(0, 0, 0);

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    _timeoutManager.AddTimer(timeInterval);

        //    // Assert.
        //    Thread.Sleep(100);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeInterval));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_ActionTokenExpired_ActionTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 0;
        //    var sessionClientMock = Substitute.For<ISessionClient>();

        //    var timeInterval = new TimeInterval(0, 0, 100);

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    _timeoutManager.AddTimer(timeInterval);

        //    // Assert.
        //    Thread.Sleep(200);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeInterval));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_ActionTokenNotExpired_ActionTokenNotRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 1;
        //    var sessionClientMock = Substitute.For<ISessionClient>();

        //    var timeInterval = new TimeInterval(0, 0, 200);

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    _timeoutManager.AddTimer(timeInterval);

        //    // Assert.
        //    Thread.Sleep(100);
        //    sessionClientMock.Received(0).HandleActionCase(Arg.Is(timeInterval));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_MultipleActionTokenNotExpired_ActionTokensNotRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 10;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, nonExpiringTimeout),
        //        new TimeInterval(0, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, nonExpiringTimeout),
        //        new TimeInterval(0, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, nonExpiringTimeout),
        //        new TimeInterval(0, 6, nonExpiringTimeout),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, nonExpiringTimeout),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var timeInterval in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(timeInterval);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(0).HandleActionCase(Arg.Any<TimeInterval>());
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_MultipleTokensFirstExpired_ExpiredTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 9;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, 50),
        //        new TimeInterval(0, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, nonExpiringTimeout),
        //        new TimeInterval(0, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, nonExpiringTimeout),
        //        new TimeInterval(0, 6, nonExpiringTimeout),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, nonExpiringTimeout),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var timeInterval in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(timeInterval);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Any<TimeInterval>());
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}


        //[Test]
        //public void StartTimer_MultipleTokensFifthExpired_ExpiredTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 9;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, nonExpiringTimeout),
        //        new TimeInterval(0, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, nonExpiringTimeout),
        //        new TimeInterval(0, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, 50),
        //        new TimeInterval(0, 6, nonExpiringTimeout),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, nonExpiringTimeout),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var timeInterval in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(timeInterval);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[4]));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_MultipleTokensTwoExpired_ExpiredTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 8;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, nonExpiringTimeout),
        //        new TimeInterval(0, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, nonExpiringTimeout),
        //        new TimeInterval(0, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, 50),
        //        new TimeInterval(0, 6, 50),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, nonExpiringTimeout),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var timeInterval in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(timeInterval);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[4]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[5]));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_MultipleTokensFiveExpired_ExpiredTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 5;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, 50),
        //        new TimeInterval(0, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, 51),
        //        new TimeInterval(0, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, 52),
        //        new TimeInterval(0, 6, 51),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, 50),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var actionToken in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(actionToken);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[0]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[2]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[4]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[5]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[8]));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_MultipleTokensFiveZero_ExpiredTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 5;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, 0),
        //        new TimeInterval(0, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, 0),
        //        new TimeInterval(0, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, 0),
        //        new TimeInterval(0, 6, 0),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, 0),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var timeInterval in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(timeInterval);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[0]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[2]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[4]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[5]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervalList[8]));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        ///// <summary>
        ///// Carefully this test is an emulation to real world scenario. Adds token logic to reset.
        ///// Nothing to do with TimeoutManager.
        ///// </summary>
        //[Test]
        //public void ResetToken_MessUpOrderedActionTokensList_ExpiredTokenRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfTokens = 9;
        //    int nonExpiringTimeout = 40000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var actionTokenToReset1 = new TimeInterval(0, 1, 2000);
        //    var actionTokenToReset2 = new TimeInterval(0, 2, 2000);
        //    var actionTokenToReset3 = new TimeInterval(0, 3, 3000);
        //    var actionTokenExpired = new TimeInterval(0, 4, 3000);

        //    var timeIntervalList = new List<TimeInterval>
        //    {
        //        actionTokenToReset1,
        //        actionTokenToReset2,
        //        actionTokenToReset3,
        //        actionTokenExpired,
        //        new TimeInterval(0, 5, nonExpiringTimeout),
        //        new TimeInterval(0, 6, nonExpiringTimeout),
        //        new TimeInterval(0, 7, nonExpiringTimeout),
        //        new TimeInterval(0, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, nonExpiringTimeout),
        //        new TimeInterval(0, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var actionToken in timeIntervalList)
        //    {
        //        _timeoutManager.AddTimer(actionToken);
        //    }

        //    Thread.Sleep(1500);
        //    _timeoutManager.AddTimer(new TimeInterval(0, 1, nonExpiringTimeout));
        //    _timeoutManager.AddTimer(new TimeInterval(0, 2, nonExpiringTimeout));
        //    _timeoutManager.AddTimer(new TimeInterval(0, 3, nonExpiringTimeout));

        //    // Assert.
        //    Thread.Sleep(2000);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(actionTokenExpired));
        //    Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        //}

        [Test]
        public void AddTimer_TimeIntervalWithoutTimeout_TimeIntervalAdded()
        {
            // Arrange.
            int expectedCountOfIntervals = 1;
            var timeInterval = new TimeInterval(0, 0, 0);

            // Act.
            _timeoutManager.AddTimer(timeInterval);

            // Assert.
            Assert.AreEqual(expectedCountOfIntervals, _timeoutManager.GetTimeoutsCount());
        }

        //[Test]
        //public void StartTimer_TimeIntervalWithoutTimeout_TimeIntervalRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfIntervals = 0;
        //    var timeInterval = new TimeInterval(0, 0, 0);
        //    var sessionClientMock = Substitute.For<ISessionClient>();

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    _timeoutManager.AddTimer(timeInterval);
        //    Thread.Sleep(300);

        //    // Assert.
        //    Assert.AreEqual(expectedCountOfIntervals, _timeoutManager.GetTimeoutsCount());
        //}

        //[Test]
        //public void StartTimer_TimeIntervalBecomesExpired_CallsSessionClientHandleTimeElapsed()
        //{
        //    // Arrange.
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeInterval = new TimeInterval(0, 0, 200);

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    _timeoutManager.AddTimer(timeInterval);
        //    Thread.Sleep(300);

        //    // Assert.
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeInterval));
        //}

        //[Test]
        //public void StartTimer_MultipleTimeIntervalsFiveZero_ExpiredTimeIntervalsRemoved()
        //{
        //    // Arrange.
        //    int expectedCountOfIntervals = 5;
        //    int nonExpiringTimeout = 3000;
        //    var sessionClientMock = Substitute.For<ISessionClient>();
        //    var timeIntervals = new List<TimeInterval>
        //    {
        //        new TimeInterval(0, 1, 0),
        //        new TimeInterval(1, 2, nonExpiringTimeout),
        //        new TimeInterval(0, 3, 0),
        //        new TimeInterval(1, 4, nonExpiringTimeout),
        //        new TimeInterval(0, 5, 0),
        //        new TimeInterval(0, 6, 0),
        //        new TimeInterval(1, 7, nonExpiringTimeout),
        //        new TimeInterval(1, 8, nonExpiringTimeout),
        //        new TimeInterval(0, 9, 0),
        //        new TimeInterval(1, 10, nonExpiringTimeout)
        //    };

        //    // Act.
        //    _timeoutManager.Start(sessionClientMock);
        //    foreach (var timeInterval in timeIntervals)
        //    {
        //        _timeoutManager.AddTimer(timeInterval);
        //    }

        //    // Assert.
        //    Thread.Sleep(150);
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervals[0]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervals[2]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervals[4]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervals[5]));
        //    sessionClientMock.Received(1).HandleActionCase(Arg.Is(timeIntervals[8]));
        //    Assert.AreEqual(expectedCountOfIntervals, _timeoutManager.GetTimeoutsCount());
        //}
    }
}
