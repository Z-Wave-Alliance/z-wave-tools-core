/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Xml.Application;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.ZipApplication.Operations
{
    public class GetCachedNodeInfoOperation : RequestDataOperation
    {
        private NodeTag _node;
        public GetCachedNodeInfoOperation(NetworkViewPoint network, NodeTag node, int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_INFO_CACHED_REPORT.ID,
            timeoutMs)
        {
            _node = node;
            var cachedGet = new COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_INFO_CACHED_GET()
            {
                seqNo = SequenceNumber
            };
            if (network.IsLongRangeEnabled(_node))
            {
                cachedGet.nodeId = 0xFF;
                cachedGet.extendedNodeid = new byte[] { (byte)(_node.Id >> 8), (byte)_node.Id };
            }
            else
            {
                cachedGet.nodeId = (byte)_node.Id;
                cachedGet.extendedNodeid = new byte[] { 0x00, (byte)_node.Id };
            }
            Data = cachedGet;
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_INFO_CACHED_REPORT packet = ou.DataFrame.Payload;
            SpecificResult.Node = _node;
            SpecificResult.NodeInfo = new NodeInfo
            {
                Basic = packet.basicDeviceClass,
                Generic = packet.genericDeviceClass,
                Properties1 = packet.properties1,
                Capability = packet.properties2,
                Security = packet.properties3,
                Specific = packet.specificDeviceClass
            };
            var grantedKeys = (NetworkKeyS2Flags)packet.grantedKeys.Value;
            List<SecuritySchemes> securitySchemes = new List<SecuritySchemes>();
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class2))
            {
                securitySchemes.Add(SecuritySchemes.S2_ACCESS);
            }
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class1))
            {
                securitySchemes.Add(SecuritySchemes.S2_AUTHENTICATED);
            }
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class0))
            {
                securitySchemes.Add(SecuritySchemes.S2_UNAUTHENTICATED);
            }
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S0))
            {
                securitySchemes.Add(SecuritySchemes.S0);
            }
            SpecificResult.SecuritySchemes = securitySchemes.ToArray();
            if (packet.commandClass != null)
            {
                ZWaveDefinition.TryParseCommandClassRef(packet.commandClass, out byte[] commandClasses, out byte[] secureCommandClasses);
                SpecificResult.CommandClasses = commandClasses;
                SpecificResult.SecureCommandClasses = secureCommandClasses;
            }
            base.SetStateCompleted(ou);
        }

        public new GetCachedNodeInfoResult SpecificResult
        {
            get { return (GetCachedNodeInfoResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetCachedNodeInfoResult();
        }
    }

    public class GetCachedNodeInfoResult : RequestDataResult
    {
        public NodeTag Node { get; set; }
        public NodeInfo NodeInfo { get; set; }
        public byte[] CommandClasses { get; set; }
        public byte[] SecureCommandClasses { get; set; }
        public SecuritySchemes[] SecuritySchemes { get; set; }
    }
}
