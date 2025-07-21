/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using NUnit.Framework;
using ZWave.Security;

namespace ZWaveTests.Security
{
    [TestFixture]
    public class SpanContainerTests
    {
        private readonly byte[] _fakeSpanNonce = new byte[] { 0x7f, 0xef, 0x6c, 0x43, 0xec, 0x5a, 0x19, 0x99, 0x51, 0x2e, 0x34, 0xb8, 0x5f, 0xea, 0x7d, 0x4f };
        private byte[] _fakeReceiverNonce = new byte[] { 0x9d, 0x8e, 0x49, 0xab, 0x44, 0xae, 0x00, 0x9c, 0xba, 0x48, 0x2b, 0xcf, 0x6d, 0x33, 0x98, 0x53 };

        private const byte SEQUENCE_NUMBER = 1;

        [Test]
        public void SetReceiversNonce_ValidNonce_HasCorrectReceiversNonce()
        {
            // Arrange.
            var nonceContainer = new SpanContainer(new CTR_DRBG_CTX(), SEQUENCE_NUMBER, SEQUENCE_NUMBER, _fakeSpanNonce);

            // Act.
            nonceContainer.SetReceiversNonceState(_fakeReceiverNonce);

            // Assert.
            Assert.IsTrue(_fakeReceiverNonce.SequenceEqual(nonceContainer.ReceiversNonce));
        }

        [Test]
        public void GetReceiversNonce_TypeIsSpan_ThrowsInvalidOperationException()
        {
            // Arrange.
            var nonceContainer = new SpanContainer(new CTR_DRBG_CTX(), SEQUENCE_NUMBER, SEQUENCE_NUMBER, _fakeSpanNonce);

            // Act.
            byte[] rNonce;
            Assert.That(() => rNonce = nonceContainer.ReceiversNonce, Throws.TypeOf<InvalidOperationException>());

            // Assert.
        }

        [Test]
        public void GetSpanNonce_TypeIsReceiver_ThrowsInvalidOperationException()
        {
            // Arrange.
            var nonceContainer = new SpanContainer(_fakeReceiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);

            // Act.
            byte[] spanNonce;
            Assert.That(() => spanNonce = nonceContainer.Span, Throws.TypeOf<InvalidOperationException>());

            // Assert.
        }
    }
}
