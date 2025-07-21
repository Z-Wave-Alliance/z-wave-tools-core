/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Linq;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class ListenDataOperation : ApiAchOperation
    {
        private readonly ListenDataDelegate _listenCallback;
        public ListenDataOperation(NetworkViewPoint network, ListenDataDelegate listenCallback)
            : base(network, NodeTag.Empty, NodeTag.Empty, null)
        {
            _listenCallback = listenCallback;
        }

        public ListenDataOperation(NetworkViewPoint network, ListenDataDelegate listenCallback, ByteIndex[] compareData)
            : base(network, NodeTag.Empty, NodeTag.Empty, compareData)
        {
            _listenCallback = listenCallback;
        }

        static readonly byte[] emptyArray = new byte[0];
        NodeTag handlingRequestFromNode = NodeTag.Empty;
        byte[] handlingRequest = emptyArray;
        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            var node = ReceivedAchData.SrcNode;
            byte[] cmd = ReceivedAchData.Command;
            if ((cmd != null && cmd.Length > 1) || ReceivedAchData.Extensions != null)
            {
                if (handlingRequestFromNode != node || !handlingRequest.SequenceEqual(cmd))
                {
                    SpecificResult.TotalCount++;
                    handlingRequestFromNode = node;
                    handlingRequest = cmd;
                    _listenCallback?.Invoke(ReceivedAchData);
                    handlingRequestFromNode = NodeTag.Empty;
                    handlingRequest = emptyArray;
                }
            }
        }

        public ListenDataResult SpecificResult
        {
            get { return (ListenDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ListenDataResult();
        }
    }

    public class ListenDataResult : ActionResult
    {
        public int TotalCount { get; set; }
    }

    public delegate void ListenDataDelegate(AchData appCmdHandlerData);
}
