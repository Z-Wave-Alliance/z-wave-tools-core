/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Net;
using ZWave.CommandClasses;
using ZWave.Devices;

namespace ZWave.ZipApplication.Operations
{
    public class GetIpAddressOperation : RequestDataOperation
    {
        private NodeTag Node { get; set; }
        public GetIpAddressOperation(NodeTag node)
            : base(null, null, COMMAND_CLASS_ZIP_ND_V2.ID, COMMAND_CLASS_ZIP_ND_V2.ZIP_NODE_ADVERTISEMENT.ID, 2000)
        {
            Node = node;
            if(node.Id > 0xFF)
            {
                Data = new COMMAND_CLASS_ZIP_ND_V2.ZIP_INV_NODE_SOLICITATION() { nodeId = 0xFF, extendedNodeid = 
                    Enumerable.Reverse(BitConverter.GetBytes(Node.Id)).ToArray() };
            }
            else
            {
                Data = new COMMAND_CLASS_ZIP_ND.ZIP_INV_NODE_SOLICITATION() { nodeId = (byte)Node.Id };
            }
            
            IsNoAck = true;
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_ZIP_ND.ZIP_NODE_ADVERTISEMENT packet = (COMMAND_CLASS_ZIP_ND.ZIP_NODE_ADVERTISEMENT)ou.DataFrame.Payload;
            SpecificResult.NodeId = packet.nodeId;
            SpecificResult.HomeId = packet.homeId.ToArray();
            SpecificResult.IpAddress = GetIpAddressBytes(packet.ipv6Address.ToArray());
            base.SetStateCompleted(ou);
        }

        private static IPAddress GetIpAddressBytes(byte[] ipAddress)
        {
            byte[] ipAddr = ipAddress;

            if (ipAddress[0] == 0 &&
                ipAddress[1] == 0 &&
                ipAddress[2] == 0 &&
                ipAddress[3] == 0 &&
                ipAddress[4] == 0 &&
                ipAddress[5] == 0 &&
                ipAddress[6] == 0 &&
                ipAddress[7] == 0 &&
                ipAddress[8] == 0 &&
                ipAddress[9] == 0 &&
                ipAddress[10] == 0xFF &&
                ipAddress[11] == 0xFF)
            {
                ipAddr = new byte[4];
                Array.Copy(ipAddress, 12, ipAddr, 0, 4);
            }
            return new IPAddress(ipAddr);
        }

        public override string AboutMe()
        {
            return string.Format("IP={0}", SpecificResult.IpAddress == null ? "" : SpecificResult.IpAddress.ToString());
        }

        public new GetIpAddressResult SpecificResult
        {
            get { return (GetIpAddressResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetIpAddressResult();
        }
    }

    public class GetIpAddressResult : ActionResult
    {
        public byte NodeId { get; set; }
        public byte[] HomeId { get; set; }
        public IPAddress IpAddress { get; set; }
    }
}
