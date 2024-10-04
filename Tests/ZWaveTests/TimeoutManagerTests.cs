/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using ZWave;
using ZWave.Layers;
using System.Threading;
using System.Collections.Generic;
using Moq;

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

        [Test]
        public void AddTimer_ActionTokenBecomesExpired_CallsSessionClientHandleTimeElapsed()
        {
            // Arrange.
            var sessionClientMock = new Mock<ISessionClient>();
            var timeInterval = new TimeInterval(0, 0, 200);

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            _timeoutManager.AddTimer(timeInterval);
            Thread.Sleep(400);

            // Assert.
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeInterval)), Times.Once);
        }

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

        [Test]
        public void StartTimer_ActionTokenWithoutTimeout_ActionTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 0;
            var sessionClientMock = new Mock<ISessionClient>();

            var timeInterval = new TimeInterval(0, 0, 0);

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            _timeoutManager.AddTimer(timeInterval);

            // Assert.
            Thread.Sleep(100);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeInterval)), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_ActionTokenExpired_ActionTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 0;
            var sessionClientMock = new Mock<ISessionClient>();

            var timeInterval = new TimeInterval(0, 0, 100);

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            _timeoutManager.AddTimer(timeInterval);

            // Assert.
            Thread.Sleep(200);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeInterval)), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_ActionTokenNotExpired_ActionTokenNotRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 1;
            var sessionClientMock = new Mock<ISessionClient>();

            var timeInterval = new TimeInterval(0, 0, 200);

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            _timeoutManager.AddTimer(timeInterval);

            // Assert.
            Thread.Sleep(100);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeInterval)), Times.Never);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_MultipleActionTokenNotExpired_ActionTokensNotRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 10;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervalList = new List<TimeInterval>
            {
                new TimeInterval(0, 1, nonExpiringTimeout),
                new TimeInterval(0, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, nonExpiringTimeout),
                new TimeInterval(0, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, nonExpiringTimeout),
                new TimeInterval(0, 6, nonExpiringTimeout),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, nonExpiringTimeout),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var timeInterval in timeIntervalList)
            {
                _timeoutManager.AddTimer(timeInterval);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.IsAny<TimeInterval>()), Times.Never);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_MultipleTokensFirstExpired_ExpiredTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 9;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervalList = new List<TimeInterval>
            {
                new TimeInterval(0, 1, 50),
                new TimeInterval(0, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, nonExpiringTimeout),
                new TimeInterval(0, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, nonExpiringTimeout),
                new TimeInterval(0, 6, nonExpiringTimeout),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, nonExpiringTimeout),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var timeInterval in timeIntervalList)
            {
                _timeoutManager.AddTimer(timeInterval);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.IsAny<TimeInterval>()), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }


        [Test]
        public void StartTimer_MultipleTokensFifthExpired_ExpiredTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 9;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervalList = new List<TimeInterval>
            {
                new TimeInterval(0, 1, nonExpiringTimeout),
                new TimeInterval(0, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, nonExpiringTimeout),
                new TimeInterval(0, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, 50),
                new TimeInterval(0, 6, nonExpiringTimeout),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, nonExpiringTimeout),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var timeInterval in timeIntervalList)
            {
                _timeoutManager.AddTimer(timeInterval);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[4])), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_MultipleTokensTwoExpired_ExpiredTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 8;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervalList = new List<TimeInterval>
            {
                new TimeInterval(0, 1, nonExpiringTimeout),
                new TimeInterval(0, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, nonExpiringTimeout),
                new TimeInterval(0, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, 50),
                new TimeInterval(0, 6, 50),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, nonExpiringTimeout),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var timeInterval in timeIntervalList)
            {
                _timeoutManager.AddTimer(timeInterval);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[4])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[5])), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_MultipleTokensFiveExpired_ExpiredTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 5;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervalList = new List<TimeInterval>
            {
                new TimeInterval(0, 1, 50),
                new TimeInterval(0, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, 51),
                new TimeInterval(0, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, 52),
                new TimeInterval(0, 6, 51),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, 50),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var actionToken in timeIntervalList)
            {
                _timeoutManager.AddTimer(actionToken);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[0])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[2])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[4])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[5])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[8])), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_MultipleTokensFiveZero_ExpiredTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 5;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervalList = new List<TimeInterval>
            {
                new TimeInterval(0, 1, 0),
                new TimeInterval(0, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, 0),
                new TimeInterval(0, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, 0),
                new TimeInterval(0, 6, 0),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, 0),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var timeInterval in timeIntervalList)
            {
                _timeoutManager.AddTimer(timeInterval);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[0])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[2])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[4])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[5])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervalList[8])), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

        /// <summary>
        /// Carefully this test is an emulation to real world scenario. Adds token logic to reset.
        /// Nothing to do with TimeoutManager.
        /// </summary>
        [Test]
        public void ResetToken_MessUpOrderedActionTokensList_ExpiredTokenRemoved()
        {
            // Arrange.
            int expectedCountOfTokens = 9;
            int nonExpiringTimeout = 4000;
            var sessionClientMock = new Mock<ISessionClient>();
            var actionTokenToReset1 = new TimeInterval(0, 1, 200);
            var actionTokenToReset2 = new TimeInterval(0, 2, 200);
            var actionTokenToReset3 = new TimeInterval(0, 3, 300);
            var actionTokenExpired = new TimeInterval(0, 4, 300);

            var timeIntervalList = new List<TimeInterval>
            {
                actionTokenToReset1,
                actionTokenToReset2,
                actionTokenToReset3,
                actionTokenExpired,
                new TimeInterval(0, 5, nonExpiringTimeout),
                new TimeInterval(0, 6, nonExpiringTimeout),
                new TimeInterval(0, 7, nonExpiringTimeout),
                new TimeInterval(0, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, nonExpiringTimeout),
                new TimeInterval(0, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var actionToken in timeIntervalList)
            {
                _timeoutManager.AddTimer(actionToken);
            }

            Thread.Sleep(150);
            _timeoutManager.AddTimer(new TimeInterval(0, 1, nonExpiringTimeout));
            _timeoutManager.AddTimer(new TimeInterval(0, 2, nonExpiringTimeout));
            _timeoutManager.AddTimer(new TimeInterval(0, 3, nonExpiringTimeout));

            // Assert.
            Thread.Sleep(200);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == actionTokenExpired)), Times.Once);
            Assert.AreEqual(expectedCountOfTokens, _timeoutManager.GetTimeoutsCount());
        }

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

        [Test]
        public void StartTimer_TimeIntervalWithoutTimeout_TimeIntervalRemoved()
        {
            // Arrange.
            int expectedCountOfIntervals = 0;
            var timeInterval = new TimeInterval(0, 0, 0);
            var sessionClientMock = new Mock<ISessionClient>();

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            _timeoutManager.AddTimer(timeInterval);
            Thread.Sleep(300);

            // Assert.
            Assert.AreEqual(expectedCountOfIntervals, _timeoutManager.GetTimeoutsCount());
        }

        [Test]
        public void StartTimer_TimeIntervalBecomesExpired_CallsSessionClientHandleTimeElapsed()
        {
            // Arrange.
            var sessionClientMock = new Mock<ISessionClient>();
            var timeInterval = new TimeInterval(0, 0, 200);

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            _timeoutManager.AddTimer(timeInterval);
            Thread.Sleep(300);

            // Assert.
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeInterval)), Times.Once);
        }

        [Test]
        public void StartTimer_MultipleTimeIntervalsFiveZero_ExpiredTimeIntervalsRemoved()
        {
            // Arrange.
            int expectedCountOfIntervals = 5;
            int nonExpiringTimeout = 3000;
            var sessionClientMock = new Mock<ISessionClient>();
            var timeIntervals = new List<TimeInterval>
            {
                new TimeInterval(0, 1, 0),
                new TimeInterval(1, 2, nonExpiringTimeout),
                new TimeInterval(0, 3, 0),
                new TimeInterval(1, 4, nonExpiringTimeout),
                new TimeInterval(0, 5, 0),
                new TimeInterval(0, 6, 0),
                new TimeInterval(1, 7, nonExpiringTimeout),
                new TimeInterval(1, 8, nonExpiringTimeout),
                new TimeInterval(0, 9, 0),
                new TimeInterval(1, 10, nonExpiringTimeout)
            };

            // Act.
            _timeoutManager.Start(sessionClientMock.Object);
            foreach (var timeInterval in timeIntervals)
            {
                _timeoutManager.AddTimer(timeInterval);
            }

            // Assert.
            Thread.Sleep(150);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervals[0])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervals[2])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervals[4])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervals[5])), Times.Once);
            sessionClientMock.Verify(x => x.HandleActionCase(It.Is<TimeInterval>(a => a == timeIntervals[8])), Times.Once);

            Assert.AreEqual(expectedCountOfIntervals, _timeoutManager.GetTimeoutsCount());
        }
    }
}
