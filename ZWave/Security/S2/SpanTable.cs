/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils;
using ZWave.Devices;

namespace ZWave.Security
{
    public class SpanTable
    {
        public const int MAX_RECORDS_COUNT = 232;
        public const int INIT_SEQ_NO = 0x55;
        private SizeLimitedTable<InvariantPeerNodeId, SpanContainer> _table;
        private SizeLimitedTable<InvariantPeerNodeId, byte> _txSequenceNumberTable;
        private readonly object _synchObject = new object();

        /// <summary>
        /// Creates span table with default SPAN records capacity of 10
        /// </summary>
        public SpanTable()
            : this(MAX_RECORDS_COUNT)
        {
        }

        /// <summary>
        /// Creates span table with specified SPAN records capacity
        /// </summary>
        /// <param name="maxRecordsCount">SPAN records capacity</param>
        public SpanTable(int maxRecordsCount)
        {
            _table = new SizeLimitedTable<InvariantPeerNodeId, SpanContainer>(maxRecordsCount);
            _txSequenceNumberTable = new SizeLimitedTable<InvariantPeerNodeId, byte>(maxRecordsCount);
        }

        /// <summary>
        /// Adds new nonce with Receiver state to table
        /// </summary>
        /// <param name="peerNodeId">Sender node id</param>
        /// <param name="receiverNonce">16 bytes receiver nonce</param>
        /// <param name="sequenceNumber">Sequence number</param>
        public void Add(InvariantPeerNodeId peerNodeId, byte[] receiverNonce, byte txSequenceNumber, byte rxSequenceNumber)
        {
            lock (_synchObject)
            {
                if (!_table.ContainsKey(peerNodeId))
                {
                    _table.Add(peerNodeId, new SpanContainer(receiverNonce, txSequenceNumber, rxSequenceNumber));
                }
                else if (_table.ContainsKey(peerNodeId) && _table[peerNodeId].SpanState == SpanStates.None)
                {
                    _table[peerNodeId].SetReceiversNonceState(receiverNonce);
                    _table[peerNodeId].TxSequenceNumber = txSequenceNumber;
                    _table[peerNodeId].RxSequenceNumber = rxSequenceNumber;
                }
                else
                {
                    throw new InvalidOperationException("Nonce with specified peerNodeId already exists");
                }

                BackupTxSequenceNumber(peerNodeId);
            }
        }

        /// <summary>
        /// Remove if exists and adds new nonce with Receiver state to table. 
        /// </summary>
        /// <param name="peerNodeId">Sender node id</param>
        /// <param name="receiverNonce">16 bytes receiver nonce</param>
        public void AddOrReplace(InvariantPeerNodeId peerNodeId, byte[] receiverNonce, byte txSequenceNumber, byte rxSequenceNumber)
        {
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId))
                {
                    _table[peerNodeId].SetReceiversNonceState(receiverNonce);
                    _table[peerNodeId].RxSequenceNumber = rxSequenceNumber;
                }
                else
                {
                    _table.Add(peerNodeId, new SpanContainer(receiverNonce, txSequenceNumber, rxSequenceNumber));
                }
                BackupTxSequenceNumber(peerNodeId);
            }
        }

        public bool CheckNonceExists(InvariantPeerNodeId peerNodeId)
        {
            lock (_synchObject)
            {
                return _table.ContainsKey(peerNodeId) && _table[peerNodeId].SpanState != SpanStates.None;
            }
        }

        public bool SetNonceFree(InvariantPeerNodeId peerNodeId)
        {
            bool ret = false;
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId))
                {
                    ret = true;
                    _table[peerNodeId].SetNonceFree();
                }
            }
            return ret;
        }

        public void ClearNonceTable()
        {
            lock (_synchObject)
            {
                _table.Clear();
            }
        }

        public SpanContainer GetContainer(InvariantPeerNodeId peerNodeId)
        {
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId) && _table[peerNodeId].SpanState != SpanStates.None)
                {
                    return _table[peerNodeId];
                }
                return null;
            }
        }

        public SpanStates GetSpanState(InvariantPeerNodeId peerNodeId)
        {
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId) && _table[peerNodeId].SpanState != SpanStates.None)
                {
                    return _table[peerNodeId].SpanState;
                }
                return SpanStates.None;
            }
        }

        public bool IsValidRxSequenceNumber(InvariantPeerNodeId peerNodeId, byte rxSequenceNumber)
        {
            bool ret = false;
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId))
                {
                    ret = _table[peerNodeId].RxSequenceNumber != rxSequenceNumber;
                }
                else
                {
                    ret = true;
                }
            }
            return ret;
        }

        //public void UpdateRxSequenceNumber(InvariantPeerNodeId peerNodeId, byte rxSequenceNumber)
        //{
        //    lock (_synchObject)
        //    {
        //        if (_table.ContainsKey(peerNodeId))
        //        {
        //            _table[peerNodeId].RxSequenceNumber = rxSequenceNumber;
        //        }
        //    }
        //}

        public byte GetRxSequenceNumber(InvariantPeerNodeId peerNodeId)
        {
            byte ret = 0;
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId))
                {
                    ret = _table[peerNodeId].RxSequenceNumber;
                }
            }
            return ret;
        }

        public byte GetTxSequenceNumber(InvariantPeerNodeId peerNodeId)
        {
            return GetTxSequenceNumber(peerNodeId, null);
        }

        public byte GetTxSequenceNumber(InvariantPeerNodeId peerNodeId, byte? initialSequenceNumber)
        {
            byte ret = 0;
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId))
                {
                    ret = _table[peerNodeId].TxSequenceNumber;
                }
                else if (_txSequenceNumberTable.ContainsKey(peerNodeId))
                {
                    ret = ++_txSequenceNumberTable[peerNodeId];
                }
                else
                {
                    ret = initialSequenceNumber ?? INIT_SEQ_NO;
                    _txSequenceNumberTable.Add(peerNodeId, ret);
                }
            }
            return ret;
        }

        public byte UpdateTxSequenceNumber(InvariantPeerNodeId peerNodeId)
        {
            byte ret = 0;
            lock (_synchObject)
            {
                if (_table.ContainsKey(peerNodeId))
                {
                    _table[peerNodeId].TxSequenceNumber++;
                    ret = _table[peerNodeId].TxSequenceNumber;
                }
                BackupTxSequenceNumber(peerNodeId);
            }
            return ret;
        }

        private void BackupTxSequenceNumber(InvariantPeerNodeId peerNodeId)
        {
            if (_table.ContainsKey(peerNodeId))
            {
                if (_txSequenceNumberTable.ContainsKey(peerNodeId))
                {
                    _txSequenceNumberTable[peerNodeId] = _table[peerNodeId].TxSequenceNumber;
                }
                else
                {
                    _txSequenceNumberTable.Add(peerNodeId, _table[peerNodeId].TxSequenceNumber);
                }
            }
        }
    }
}
