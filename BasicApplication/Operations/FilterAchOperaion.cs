/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class FilterAchOperation : ApiAchOperation
    {
        private NodeTag _filterNode;
        private NodeTag _filterSucNode;
        public FilterAchOperation(NetworkViewPoint network)
            : base(network, NodeTag.Empty, NodeTag.Empty, null)
        {
            IsFirstPriority = true;
        }

        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            if (_filterNode.Id > 0)
            {
                if (ReceivedAchData.SrcNode == _filterNode ||
                     (_filterSucNode.Id > 0 && ReceivedAchData.SrcNode == _filterSucNode))
                {
                }
                else
                {
                    ou.DataFrame.IsHandled = true;
                    "DENIED FilterNodeId: {0}"._DLOG(ReceivedAchData.SrcNode.Id);
                }
            }
        }

        public void SetFilterNodeId(NodeTag node)
        {
            _filterNode = node;
        }

        public void SetFilterSucNodeId(NodeTag sucNode)
        {
            _filterSucNode = sucNode;
        }
    }
}
