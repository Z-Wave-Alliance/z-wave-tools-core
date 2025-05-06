/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Threading;
using System.Threading.Tasks;
using Utils;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;

namespace ZWave.BasicApplication.Operations
{
    public class DelayedResponseOperation : ApiAchOperation
    {
        public DelayedResponseOperation(NetworkViewPoint network, NodeTag dstNode, NodeTag srcNode, params ByteIndex[] compareData)
            : base(network, dstNode, srcNode, compareData)
        {
        }

        public DelayedResponseOperation(NetworkViewPoint network, NodeTag dstNode, NodeTag srcNode, byte[] data, int bytesToCompare)
            : base(network, dstNode, srcNode, data, bytesToCompare)
        {
        }

        public DelayedResponseOperation(NetworkViewPoint network, NodeTag dstNode, NodeTag srcNode, byte[] data, int bytesToCompare, ExtensionTypes[] extensionTypes)
            : base(network, dstNode, srcNode, data, bytesToCompare, extensionTypes)
        {
        }

        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            OnHandledDelayed(ou);
            //supervision delay processed by supervision manager
            if (_network.DelayResponseMs > 0 &&
                !ou.DataFrame.SubstituteIncomingFlags.HasFlag(SubstituteIncomingFlags.Supervision) &&
                ou.ActionItems != null && ou.ActionItems.Count > 0)
            {
                if (ou.ActionItems.Count == 1 && ou.ActionItems[0] is ActionSerialGroup)
                {
                    var groupWithDelay = new ActionSerialGroup(new DelayOperation(_network.DelayResponseMs));
                    groupWithDelay.AddActions((ActionSerialGroup)ou.ActionItems[0]);
                    ou.ActionItems[0] = groupWithDelay;
                }
                else
                {
                    var groupWithDelay = new ActionSerialGroup(new DelayOperation(_network.DelayResponseMs));
                    groupWithDelay.AddActions((ActionBase)ou.ActionItems[0]);
                    ou.ActionItems[0] = groupWithDelay;
                }
            }
        }

        protected virtual void OnHandledDelayed(DataReceivedUnit ou)
        {
        }
    }
}
