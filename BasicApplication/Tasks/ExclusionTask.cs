/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Tasks
{
    public class ExclusionTask : ActionParallelGroup
    {
        public int WAKE_UP_INTERVAL = 5 * 60; //seconds
        private readonly Modes _mode;
        private readonly int _timeoutMs;
        private readonly Action<NodeStatuses> _nodeStatusCallback;


        private FilterAchOperation _peerFilter;
        private RemoveNodeOperation _removeNode;

        public ExclusionTask(NetworkViewPoint network, Modes mode, Action<NodeStatuses> nodeStatusCallback, int timeoutMs)
            : base(false, null)
        {
            _mode = mode;
            _nodeStatusCallback = nodeStatusCallback;
            _timeoutMs = timeoutMs;

            _peerFilter = new FilterAchOperation(network);
            _peerFilter.SetFilterNodeId(new NodeTag(0xFF));
            _removeNode = new RemoveNodeOperation(network, _mode, OnNodeStatus, _timeoutMs);

            SpecificResult.AddRemoveNode = _removeNode.SpecificResult;

            Actions = new ActionBase[] 
            {
                _peerFilter,
                _removeNode
            };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
        }

        private void OnNodeStatus(NodeStatuses nodeStatus)
        {
            if (nodeStatus == NodeStatuses.AddingRemovingController || nodeStatus == NodeStatuses.AddingRemovingEndDevice)
            {
                _peerFilter.SetFilterNodeId(_removeNode.SpecificResult.Node);
            }
            _nodeStatusCallback?.Invoke(nodeStatus);
        }

        public ExclusionResult SpecificResult
        {
            get { return (ExclusionResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExclusionResult();
        }
    }

    public class ExclusionResult : ActionResult
    {
        public AddRemoveNodeResult AddRemoveNode { get; set; }
    }
}
