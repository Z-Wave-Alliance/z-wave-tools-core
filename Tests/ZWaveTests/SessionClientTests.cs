/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Threading;
using System.Collections.Generic;
using NUnit.Framework;
using ZWave;
using ZWave.Layers;
using ZWave.Layers.Session;
using ZWave.TextApplication.Operations;
using Utils.Threading;
using Moq;

namespace ZWaveTests
{
    [TestFixture]
    public class SessionClientTests
    {
        private SessionClient _sessionClient;
        private Mock<ITimeoutManager> _timeoutManager;
        private Mock<ISubstituteManager> _substituteManagerMock;

        [SetUp]
        public void SetUp()
        {
            _timeoutManager = new Mock<ITimeoutManager>();
            _sessionClient = new SessionClient(null)
            {
                SuppressDebugOutput = true
            };
            _sessionClient.RunComponents(_timeoutManager.Object, new ConsumerThread<IActionCase>());
            _substituteManagerMock = new Mock<ISubstituteManager>();
            _substituteManagerMock.Setup(x => x.SubstituteAction(It.IsAny<ActionBase>())).
                Returns<ActionBase>(x => x);
            _sessionClient.AddSubstituteManager(_substituteManagerMock.Object, null);
        }

        [TearDown]
        public void TearDown()
        {
            _sessionClient.Dispose();
        }

        [Test]
        public void ExecuteAsync_NoNextOperations_CallTimerManager_AddToCallbackBufferBlock_TokenReady()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, null, 100, new CommandMessage() { Data = new byte[2] });

            _timeoutManager.Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
                .Callback<ITimeoutItem>(x =>
                {
                    _sessionClient.HandleActionCase(x);
                });

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            var res = token.WaitCompletedSignal();

            // Call timeout manager.
            _timeoutManager.Verify(x => x.AddTimer(It.IsAny<ITimeoutItem>()), Times.Once);

            // Check action token state.
            Assert.IsFalse(res);
            Assert.AreEqual(ActionStates.Expired, res.State);
        }

        [Test]
        public void ExecuteAsync_WithNextOperations_CallTimerManager_AddToCallbackBufferBlock_TokenReady()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, null, 50, new FakeAction());

            _sessionClient.SendFramesCallback = ahResult =>
            {
                return true;
            };

            _timeoutManager.Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
               .Callback<ITimeoutItem>(x =>
               {
                   _sessionClient.HandleActionCase(x);
               });

            // Act.
            var res = _sessionClient.ExecuteAsync(fakeAction).WaitCompletedSignal();

            // Call timeout manager.
            _timeoutManager.Verify(x => x.AddTimer(It.Is<TimeInterval>(y => y.Id == 0 && y.ActionId == fakeAction.Id && y.TimeoutMs == 50)), Times.Once);
            // Check action token state.
            Assert.IsTrue(!res);
            Assert.AreEqual(ActionStates.Expired, res.State);
        }

        [Test]
        public void ExecuteAsync_WithNextTimeIntervals_AddIntervalsToTimerManager()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, (x, y) => y.SetCompleting(), 0,
                    new TimeInterval(0, 0, 50),
                    new TimeInterval(1, 0, 50),
                    new TimeInterval(3, 0, 50));
            _timeoutManager.Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
             .Callback<ITimeoutItem>(x =>
             {
                 _sessionClient.HandleActionCase(x);
             });

            // Act.
            var res = _sessionClient.ExecuteAsync(fakeAction).WaitCompletedSignal();

            // Assert.
            _timeoutManager.Verify(x => x.AddTimer(It.IsAny<TimeInterval>()), Times.Exactly(3));
        }

        [Test]
        public void ExecuteAsync_WithNextFramesHasNextTimeIntervals_AddIntervalsToTimerManager()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, (x, y) => y.SetCompleting(), 0,
                   new CommandMessage(),
                   new TimeInterval(1, 0, 100),
                   new TimeInterval(3, 0, 100));
            _timeoutManager.Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
           .Callback<ITimeoutItem>(x =>
           {
               _sessionClient.HandleActionCase(x);
           });

            // Act.
            var res = _sessionClient.ExecuteAsync(fakeAction).WaitCompletedSignal();

            // Assert.
            _timeoutManager.Verify(x => x.AddTimer(It.IsAny<TimeInterval>()), Times.Exactly(2));
        }

        [Test]
        public void ExecuteAsync_WithNextFrames_CallsSendFramesCallback()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, (x, y) => y.SetCompleting(), 50, new CommandMessage() { Data = new byte[2] });

            bool sendFramesCallbackCalled = false;
            _sessionClient.SendFramesCallback = ahResult => sendFramesCallbackCalled = true;

            // Act.
            _sessionClient.ExecuteAsync(fakeAction).WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendFramesCallbackCalled);
        }

        [Test]
        public void ExecuteAsync_WithFramesNotTransmited_TokenHasFailedState()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, null, 50, new CommandMessage() { Data = new byte[2] });

            _sessionClient.SendFramesCallback =
                ahResult =>
                {
                    return false;
                };

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            var res = token.WaitCompletedSignal();

            // Assert.
            Assert.AreEqual(ActionStates.Failed, res.State);
        }

        [Test]
        public void ExecuteAsync_WithFramesIsTransmited_CallTimerManager_AddToCallbackBufferBlock_TokenReady()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, (x, y) => y.SetCompleting(), 50, new CommandMessage() { Data = new byte[2] });

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            var res = token.WaitCompletedSignal();

            // Assert.
            // Call timeout manager.
            _timeoutManager.Verify(x => x.AddTimer(It.Is<TimeInterval>(y =>
                y.Id == 0 && y.ActionId == fakeAction.Id && y.TimeoutMs == 50
            )), Times.Once);
            // Check action token state.
            Assert.IsTrue(res);
        }

        [Test]
        public void Cancel_NoNextFrames_TokenIsCancelled_ActionRemovedFromRunning_AddToCallbackBufferBlock()
        {
            // Arrange.
            var fakeAction = new FakeAction();

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            Thread.Sleep(50);
            Assert.AreEqual(1, _sessionClient.RunningActions.Count);
            _sessionClient.Cancel(token);
            var res = token.WaitCompletedSignal();


            // Assert.
            // Token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res.State);
            // Action removed from running actions.
            Assert.AreEqual(0, _sessionClient.RunningActions.Count);
            // Check action token state.
            Assert.IsTrue(!res);
        }

        [Test]
        public void Cancel_WithParentAction_ParentActionsTokenIsCancelled_ChildActionsTokenIsCancelled()
        {
            // Arrange.
            var fakeParentAction = new FakeAction() { IsExclusive = false };
            var fakeAction = new FakeAction
            {
                ParentAction = fakeParentAction,
                IsExclusive = false
            };

            // Act.
            var tokenParent = _sessionClient.ExecuteAsync(fakeParentAction);
            var token = _sessionClient.ExecuteAsync(fakeAction);
            Thread.Sleep(50);
            Assert.AreEqual(2, _sessionClient.RunningActions.Count);
            _sessionClient.Cancel(tokenParent);
            var resParent = tokenParent.WaitCompletedSignal();
            var res = token.WaitCompletedSignal();

            // Assert.
            // Token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res.State);
            // Parent token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, resParent.State);
            // Action removed from running actions.
            Assert.AreEqual(0, _sessionClient.RunningActions.Count);
        }

        [Test]
        public void Cancel_WithParentAction_ParentActionsTokenIsCancelled_ChildActionStopActionIsCalled()
        {
            // Arrange.
            var fakeParentAction = new FakeAction() { IsExclusive = false };
            var fakeAction = new FakeAction
            {
                ParentAction = fakeParentAction,
                IsExclusive = false
            };
            var actionItem = new CommandMessage();
            fakeAction.StopActionUnit = new StopActionUnit(actionItem);

            var sendFramesCallbackCalled = false;
            _sessionClient.SendFramesCallback =
                ahResult =>
                {
                    if (ahResult?.NextActions != null && ahResult.NextActions.Contains(actionItem))
                    {
                        sendFramesCallbackCalled = true;
                    }
                    return true;
                };

            // Act.
            var tokenParent = _sessionClient.ExecuteAsync(fakeParentAction);
            var token = _sessionClient.ExecuteAsync(fakeAction);
            Thread.Sleep(50);
            Assert.AreEqual(2, _sessionClient.RunningActions.Count);
            _sessionClient.Cancel(tokenParent);
            var resParent = tokenParent.WaitCompletedSignal();
            var res = token.WaitCompletedSignal();

            // Assert.
            // Token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res.State);
            // Parent token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, resParent.State);
            // Action removed from running actions.
            Assert.AreEqual(0, _sessionClient.RunningActions.Count);
            // Stop Action Unit called
            Assert.IsTrue(sendFramesCallbackCalled);
        }

        [Test]
        public void Cancel_WithNextFrames_SendFramesCallbackCalled_TokenIsCancelled()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, null, 500, new CommandMessage() { Data = new byte[2] });
            var sendFramesCallbackCalled = false;
            _sessionClient.SendFramesCallback =
                ahResult =>
                {
                    sendFramesCallbackCalled = true;
                    return true;
                };

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            Thread.Sleep(50);
            _sessionClient.Cancel(token);
            var res = token.WaitCompletedSignal();

            // Assert.
            // Token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res.State);
            // SendFramesCallback called.
            Assert.IsTrue(sendFramesCallbackCalled);
        }

        [Test]
        public void Cancel_TokenIsCancelled()
        {
            // Arrange.
            var fakeAction = new FakeAction();

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            _sessionClient.Cancel(token);
            var res = token.WaitCompletedSignal();

            // Assert.
            // Token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res.State);
        }

        [Test]
        public void Cancel_ByActionsTypes_SpecifiedActionsTokensAreCancelled()
        {
            // Arrange.
            var fakeAction1 = new FakeAction() { IsExclusive = false };
            var fakeAction2 = new FakeAction() { IsExclusive = false };
            var fakeAction3 = new FakeAction() { IsExclusive = false };
            var sendOp1 = new AnotherFakeAction() { IsExclusive = false };
            var sendOp2 = new AnotherFakeAction() { IsExclusive = false };

            // Act.
            var token1 = _sessionClient.ExecuteAsync(fakeAction1);
            var token2 = _sessionClient.ExecuteAsync(fakeAction2);
            var token3 = _sessionClient.ExecuteAsync(fakeAction3);
            _sessionClient.ExecuteAsync(sendOp1);
            _sessionClient.ExecuteAsync(sendOp2);
            Thread.Sleep(50);
            Assert.AreEqual(5, _sessionClient.RunningActions.Count);
            _sessionClient.Cancel(typeof(FakeAction));
            var res1 = token1.WaitCompletedSignal();
            var res2 = token2.WaitCompletedSignal();
            var res3 = token3.WaitCompletedSignal();

            // Assert.
            // Tokens state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res1.State);
            Assert.AreEqual(ActionStates.Cancelled, res2.State);
            Assert.AreEqual(ActionStates.Cancelled, res3.State);
            // Only send operations left in running actions.
            Assert.AreEqual(2, _sessionClient.RunningActions.Count);
        }

        [Test]
        public void TokenExpired_NoNextFrames_TokenIsExpired_ActionRemovedFromRunning_AddToCallbackBufferBlock()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, null, 50, new CommandMessage() { Data = new byte[2] });
            _timeoutManager
                .Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
                .Callback<ITimeoutItem>(x => _sessionClient.HandleActionCase(x));

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            var res = token.WaitCompletedSignal();

            // Assert.
            // Token state is expired.
            Assert.AreEqual(ActionStates.Expired, res.State);
            // Action removed from running actions.
            Assert.AreEqual(0, _sessionClient.RunningActions.Count);
            // Check action token state.
            Assert.IsFalse(res);
        }

        [Test]
        public void TokenExpired_WithParentAction_ParentActionsTokenIsExpired_ChildActionsTokenIsCancelled()
        {
            // Arrange.
            var fakeAction = new FakeAction() { IsExclusive = false };
            var fakeParentAction = new FakeAction(false, null, 100, fakeAction);
            _timeoutManager
                .Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
                .Callback<ITimeoutItem>(x =>
                {
                    Thread.Sleep(x.TimeoutMs);
                    _sessionClient.HandleActionCase(x);
                });

            // Act.
            var tokenParent = _sessionClient.ExecuteAsync(fakeParentAction);
            var token = _sessionClient.ExecuteAsync(fakeAction);
            Thread.Sleep(50);
            Assert.AreEqual(2, _sessionClient.RunningActions.Count);
            var resParent = tokenParent.WaitCompletedSignal();
            var res = token.WaitCompletedSignal();

            // Assert.
            // Parent token state is cancelled.
            Assert.AreEqual(ActionStates.Expired, resParent.State);
            // Token state is cancelled.
            Assert.AreEqual(ActionStates.Cancelled, res.State);
            // Action removed from running actions.
            Assert.AreEqual(0, _sessionClient.RunningActions.Count);
        }

        [Test]
        public void TokenExpired_WithNextFrames_SendFramesCallbackCalled_TokenIsExpired()
        {
            // Arrange.
            var fakeAction = new FakeAction(true, null, 50, new CommandMessage() { Data = new byte[2] });
            _timeoutManager
               .Setup(x => x.AddTimer(It.IsAny<ITimeoutItem>()))
               .Callback<ITimeoutItem>(x => _sessionClient.HandleActionCase(x));

            var sendFramesCallbackCalled = false;
            _sessionClient.SendFramesCallback =
                ahResult =>
                {
                    sendFramesCallbackCalled = true;
                    return true;
                };

            // Act.
            var token = _sessionClient.ExecuteAsync(fakeAction);
            var res = token.WaitCompletedSignal();

            // Assert.
            // Token state is expired.
            Assert.AreEqual(ActionStates.Expired, res.State);
            // SendFramesCallback called.
            Assert.IsTrue(sendFramesCallbackCalled);
        }

        [Test]
        public void RunComponents_ComponentsIsReady_AllComponentsStarted()
        {
            // Arrange.

            // Act.
            var sessionClient = new SessionClient(null)
            {
                SuppressDebugOutput = true
            };
            sessionClient.RunComponents(_timeoutManager.Object, new ConsumerThread<IActionCase>());

            // Assert.
            _timeoutManager.Verify(x => x.Start(It.Is<ISessionClient>(y => y == sessionClient)), Times.Once);
        }
    }
}
