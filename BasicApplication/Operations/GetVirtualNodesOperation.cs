/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class GetVirtualNodesOperation : RequestApiOperation
    {
        public GetVirtualNodesOperation()
            : base(CommandTypes.CmdZWaveGetVirtualDeviceNodes, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.NodeIds = new List<byte>();
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null)
            {
                for (int i = 0; i < res.Length; i++)
                {

                    byte maskByte = res[i];
                    if (maskByte == 0)
                    {
                        continue;
                    }
                    byte bitMask = 0x01;
                    byte bitOffset = 0x01;//nodes starting from 1 in mask bytes array
                    for (int j = 0; j < 8; j++)
                    {
                        if ((bitMask & maskByte) != 0)
                        {
                            byte nodeID = (byte)(((i * 8) + j) + bitOffset);
                            SpecificResult.NodeIds.Add(nodeID);
                        }
                        bitMask <<= 1;
                    }
                }
            }
            base.SetStateCompleted(ou);
        }

        public GetVirtualNodesResult SpecificResult
        {
            get { return (GetVirtualNodesResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new GetVirtualNodesResult();
        }
    }

    public class GetVirtualNodesResult : ActionResult
    {
        public List<byte> NodeIds { get; set; }
    }
}
