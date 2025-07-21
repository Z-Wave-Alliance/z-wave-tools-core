/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Net;
using Utils;
using ZWave.CommandClasses;

namespace ZWave.ZipApplication.Operations
{
    public class GetIdOperation : RequestDataOperation
    {
        private IPAddress IpAddress { get; set; }
        public GetIdOperation(IPAddress ipAddress)
            : base(null, null, COMMAND_CLASS_ZIP_ND.ID, COMMAND_CLASS_ZIP_ND.ZIP_NODE_ADVERTISEMENT.ID, 2000)
        {
            IpAddress = ipAddress;
            byte[] ip = new byte[16];
            byte[] ipAddressBytes = IpAddress.GetAddressBytes();
            if (ipAddressBytes.Length == 4)
            {
                ip[10] = 0xFF;
                ip[11] = 0xFF;
                Array.Copy(ipAddressBytes, 0, ip, 12, 4);
            }
            else
                ip = ipAddressBytes;
            Data = new COMMAND_CLASS_ZIP_ND.ZIP_NODE_SOLICITATION() { ipv6Address = ip };
            IsNoAck = true;
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_ZIP_ND.ZIP_NODE_ADVERTISEMENT packet = (COMMAND_CLASS_ZIP_ND.ZIP_NODE_ADVERTISEMENT)ou.DataFrame.Payload;
            SpecificResult.NodeId = packet.nodeId;
            SpecificResult.HomeId = packet.homeId.ToArray();
            base.SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, HomeId={1}", SpecificResult.NodeId, SpecificResult.HomeId == null ? "" : Tools.GetHexShort(SpecificResult.HomeId));
        }

        public new GetIdResult SpecificResult
        {
            get { return (GetIdResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new GetIdResult();
        }
    }

    public class GetIdResult : ActionResult
    {
        public byte[] HomeId { get; set; }
        public byte NodeId { get; set; }
    }
}
