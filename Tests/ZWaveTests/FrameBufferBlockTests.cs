/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Utils;
using ZWave;
using ZWave.Layers.Frame;

namespace ZWaveTests
{
    [TestFixture]
    public class FrameBufferBlockTests
    {
        private List<byte> _writtenData;
        private FrameBufferBlock _frameBufferBlock;
        private AutoResetEvent _writeSignal;
        private AutoResetEvent _extSignal;
        private const int ACK_TIMEOUT = 20;
        [SetUp]
        public void SetUp()
        {
            _writtenData = new List<byte>();
            _frameBufferBlock = new FrameBufferBlock(ACK_TIMEOUT, WriteData, CreateFrameBuffer);
            _writeSignal = new AutoResetEvent(false);
            _extSignal = new AutoResetEvent(false);
        }

        [TearDown]
        public void TearDown()
        {
            _writeSignal.Close();
            _extSignal.Close();
        }

        [Test]
        public void AddFrames_WithACK_CorrectlyWriteAllData()
        {
            // Arrange.
            var rawData = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5 };
            var command = new CommandMessage { IsNoAck = false };
            command.AddData(rawData);
            var transmitSucceeded = false;
            var frame = new ActionHandlerResult(new FakeAction());
            frame.NextFramesCompletedCallback =
                succeeded =>
                {
                    transmitSucceeded = succeeded;
                };
            frame.NextActions.AddRange(new CommandMessage[] { command, command, command });

            // Act.
            bool closing = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                _extSignal.Set();
                while (!closing)
                {
                    _writeSignal.WaitOne();
                    if (!closing)
                    {
                        _frameBufferBlock.Acknowledge(true);
                    }
                }
            }));

            _extSignal.WaitOne();
            _frameBufferBlock.Send(frame);
            closing = true;
            _writeSignal.Set();


            // Assert.
            Assert.IsTrue(_writtenData.ToArray().SequenceEqual(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5 }));
            Assert.IsTrue(transmitSucceeded);
        }

        [Test]
        public void AddFrames_NoACK_MakesThreeAttemptsToWriteData()
        {
            // Arrange.
            var rawData = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5 };
            var command = new CommandMessage { IsNoAck = false };
            command.AddData(rawData);
            var transmitSucceeded = true;
            var frame = new ActionHandlerResult(new FakeAction());
            frame.NextFramesCompletedCallback =
                succeeded =>
                {
                    transmitSucceeded = succeeded;
                };
            frame.NextActions.AddRange(new CommandMessage[] { command });

            // Act.
            _frameBufferBlock.Send(frame);
            Thread.Sleep(ACK_TIMEOUT * 3);

            // Assert.
            Assert.IsTrue(_writtenData.SequenceEqual(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5 }));
            Assert.IsFalse(transmitSucceeded);
        }

        [Test]
        public void AddFrames_WithCAN_MakesElevenAttemptsToWriteData()
        {
            // Arrange.
            var rawData = new byte[] { 0x2, 0x3 };
            var command = new CommandMessage { IsNoAck = false };
            command.AddData(rawData);
            var transmitSucceeded = true;
            var frame = new ActionHandlerResult(new FakeAction());
            frame.NextFramesCompletedCallback =
                succeeded =>
                {
                    transmitSucceeded = succeeded;
                };
            frame.NextActions.AddRange(new CommandMessage[] { command });

            // Act.
            bool closing = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                _extSignal.Set();
                while (!closing)
                {
                    _writeSignal.WaitOne();
                    if (!closing)
                    {
                        _frameBufferBlock.Acknowledge(false);
                    }
                }
            }));

            _extSignal.WaitOne();
            _frameBufferBlock.Send(frame);
            closing = true;
            _writeSignal.Set();

            // Assert.
            Assert.IsTrue(_writtenData.SequenceEqual(new byte[] { 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3, 0x2, 0x3 }));
            Assert.IsFalse(transmitSucceeded);
        }

        private int WriteData(byte[] data)
        {
            _writtenData.AddRange(data);
            _writeSignal.Set();
            return data.Length;
        }

        private byte[] CreateFrameBuffer(CommandMessage message)
        {
            return message.Data;
        }
    }
}
