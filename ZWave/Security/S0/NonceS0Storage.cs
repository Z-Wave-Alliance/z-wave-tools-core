/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils;

namespace ZWave.Security
{
    public class NonceS0Storage
    {
        private SizeLimitedTable<OrdinalPeerNodeId, NonceContainer> _table;
        public NonceS0Storage(int capacity)
        {
            _table = new SizeLimitedTable<OrdinalPeerNodeId, NonceContainer>(capacity);
        }

        public NonceContainer this[OrdinalPeerNodeId peerNodeId]
        {
            get
            {
                if (!_table.ContainsKey(peerNodeId))
                    throw new IndexOutOfRangeException("nonce container for such peer id doesn't exist");

                return _table[peerNodeId];
            }
        }

        public void RegisterInternal(OrdinalPeerNodeId peerNodeId, byte[] nonceData)
        {
            NonceContainer n = new NonceContainer(nonceData);
            if (!_table.ContainsKey(peerNodeId))
            {
                _table.Add(peerNodeId, n);
            }
            else
            {
                _table[peerNodeId] = n;
            }
        }

        public byte[] Find(OrdinalPeerNodeId peerNodeId, byte nonceId)
        {
            byte[] ret = null;
            if (_table.ContainsKey(peerNodeId) && _table[peerNodeId].NonceId == nonceId)
            {
                ret = _table[peerNodeId].Nonce;
            }
            else
            {
                "=none"._DLOG();
            }
            return ret;
        }
    }

    public class NonceContainer
    {
        private readonly byte[] _nonce = new byte[8];
        public byte[] Nonce
        {
            get
            {
                return _nonce;
            }
        }

        public byte NonceId
        {
            get
            {
                return (_nonce ?? new byte[1])[0];
            }
        }

        public void SetNonce(byte[] nonce)
        {
            Array.Copy(nonce, _nonce, 8);
        }

        public NonceContainer(byte[] nonce)
        {
            SetNonce(nonce);
        }

        public override string ToString()
        {
            if (Nonce != null)
            {
                return Nonce.GetHex();
            }
            return "N/A";
        }
    }
}
