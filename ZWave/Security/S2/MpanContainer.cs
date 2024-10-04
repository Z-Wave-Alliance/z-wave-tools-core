/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.Devices;

namespace ZWave.Security
{
    public enum MpanUsageStates
    {
        Used = 0,
        Mos,
        MosReported
    }

    public class MpanContainer
    {
        private MpanUsageStates _mpanUsageState;
        public bool IsMosState { get { return _mpanUsageState == MpanUsageStates.Mos; } }

        public bool IsMosReportedState { get { return _mpanUsageState == MpanUsageStates.MosReported; } }

        public NodeGroupId NodeGroupId { get; private set; }

        public byte SequenceNumber { get; private set; }

        private HashSet<NodeTag> _receiverGroupHandle;
        public NodeTag[] ReceiverGroupHandle
        {
            get { return _receiverGroupHandle.ToArray(); }
        }

        public bool IsLatestFromOwner { get; set; }

        public void SetReceiverGroupHandle(NodeTag[] receivers)
        {
            _receiverGroupHandle = new HashSet<NodeTag>(receivers);
        }

        private BigInteger _mpanState;
        public byte[] MpanState { get { return _mpanState.GetBytes(); } }

        public MpanContainer(NodeGroupId nodeGroupId, byte[] mpanState, byte sequenceNumber, NodeTag[] receiverGroupHandle)
        {
            SequenceNumber = sequenceNumber;
            NodeGroupId = nodeGroupId;
            SetMpanState(mpanState ?? new byte[16]);
            if (!receiverGroupHandle.IsNullOrEmpty())
                _receiverGroupHandle = new HashSet<NodeTag>(receiverGroupHandle);
            else
                _receiverGroupHandle = new HashSet<NodeTag>();
        }

        public void IncrementMpanState()
        {
            if (IsMosState)
                throw new InvalidOperationException("container is in MOS state");

            _mpanState.Increment();
        }

        public void DecrementMpanState()
        {
            if (IsMosState)
                throw new InvalidOperationException("container is in MOS state");

            _mpanState.Decrement();
        }

        public void SetMpanState(byte[] mpanState)
        {
            if (IsMosState)
                throw new InvalidOperationException("container is in MOS state");

            _mpanState = new BigInteger(mpanState);
            _mpanUsageState = MpanUsageStates.Used;
        }

        public byte UpdateSequenceNumber()
        {
            if (IsMosState)
                throw new InvalidOperationException("container is in MOS state");

            return ++SequenceNumber;
        }

        public void SetSequenceNumber(byte sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }

        public void SetMosState(bool isMos)
        {
            _mpanUsageState = isMos ? MpanUsageStates.Mos : MpanUsageStates.Used;
        }

        public void SetMosStateReported()
        {
            if (_mpanUsageState != MpanUsageStates.Mos)
                throw new InvalidOperationException("container must be in MOS state");

            _mpanUsageState = MpanUsageStates.MosReported;
        }

        public bool DestNodesEquals(NodeTag[] nodeIds)
        {
            return _receiverGroupHandle != null && _receiverGroupHandle.SetEquals(nodeIds);
        }
    }
}
