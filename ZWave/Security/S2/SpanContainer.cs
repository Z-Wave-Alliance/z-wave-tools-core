/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Security
{
    public class SpanContainer
    {
        private CTR_DRBG_CTX _state;
        public CTR_DRBG_CTX Context
        {
            get { return _state; }
            set { _state = value; }
        }
        public byte TxSequenceNumber { get; set; }
        public byte RxSequenceNumber { get; set; }
        public SpanStates SpanState { get; private set; }
        private readonly byte[] _receiversNonce;
        public byte[] ReceiversNonce
        {
            get
            {
                if (SpanState != SpanStates.ReceiversNonce)
                    throw new InvalidOperationException("Invalid container type");

                return _receiversNonce;
            }
        }

        public void SetNonceFree()
        {
            SpanState = SpanStates.None;
        }

        public void SetReceiversNonceState()
        {
            SpanState = SpanStates.ReceiversNonce;
        }

        public void SetReceiversNonceState(byte[] rNonce)
        {
            SpanState = SpanStates.ReceiversNonce;
            Array.Copy(rNonce, _receiversNonce, 16);
        }

        private readonly byte[] _span;
        public byte[] Span
        {
            get
            {
                if (SpanState != SpanStates.Span)
                    throw new InvalidOperationException("Invalid container type");

                return _span;
            }
        }

        public SpanContainer(CTR_DRBG_CTX state, byte txSequenceNumber, byte rxSequenceNumber, byte[] nonce)
            : this(txSequenceNumber, rxSequenceNumber)
        {
            this._state = state;
            SpanState = SpanStates.Span;
            Array.Copy(nonce, _span, 16);
        }

        public SpanContainer(byte[] receiverNonce, byte txSequenceNumber, byte rxSequenceNumber)
            : this(txSequenceNumber, rxSequenceNumber)
        {
            SetReceiversNonceState(receiverNonce);
        }

        private SpanContainer(byte txSequenceNumber, byte rxSequenceNumber)
        {
            TxSequenceNumber = txSequenceNumber;
            RxSequenceNumber = rxSequenceNumber;
            _span = new byte[16];
            _receiversNonce = new byte[16];
        }

        public void InstantiateWithSenderNonce(byte[] senderNonce, byte[] personalization)
        {
            SecurityS2Utils.NextNonceInstantiate(ref _state, senderNonce, ReceiversNonce, personalization);
            SpanState = SpanStates.Span;
            Array.Copy(senderNonce, _span, 16);
        }

        public void NextNonce()
        {
            if (SpanState != SpanStates.None)
            {
                SecurityS2Utils.NextNonceGenerate(ref _state, _span);
                SpanState = SpanStates.Span;
            }
        }

        public void NextNonce(int iterations)
        {
            if (SpanState != SpanStates.None)
            {
                SecurityS2Utils.NextNonceGenerate(ref _state, _span, iterations);
                SpanState = SpanStates.Span;
            }
        }
    }
}
