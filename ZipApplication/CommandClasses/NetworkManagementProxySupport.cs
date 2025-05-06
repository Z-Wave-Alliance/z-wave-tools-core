/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.CommandClasses;
using ZWave.ZipApplication.Operations;

namespace ZWave.ZipApplication.CommandClasses
{
    public class NetworkManagementProxySupport : ResponseDataOperation
    {
        byte[] _commandsToSupport;
        readonly Action<GetNodeListResult> _updateControllerCallback;


        public NetworkManagementProxySupport(byte[] commandsToSupport, Action<GetNodeListResult> updateControllerCallback)
            : base(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID)
        {
            ReceiveCallback = OnReceiveCallback;
            _commandsToSupport = commandsToSupport;
            _updateControllerCallback = updateControllerCallback;
            _isHeaderExtensionSpecified = true;
        }

        public byte[] OnReceiveCallback(byte[] headerExtension, byte[] payload)
        {
            if (payload != null && payload.Length > 2)
            {
                if (payload[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_LIST_REPORT.ID && _commandsToSupport.Contains(payload[1]))
                {
                    var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_LIST_REPORT)payload;
                    var result = new GetNodeListResult();
                    result.ControllerId = packet.nodeListControllerId;
                    var nodes = GetNodeListResult.ParseNodesList(packet.nodeListData);
                    if (packet.extendedNodeListLength != null && packet.extendedNodeListLength.Length == 2)
                    {
                        var length = (packet.extendedNodeListLength[0] << 8) + packet.extendedNodeListLength[1];
                        if (length > 0)
                            nodes.AddRange(GetNodeListResult.ParseNodesList(packet.extendedNodeList, true));
                    }
                    result.Nodes = nodes.ToArray();
                    result.Status = packet.status;
                    _updateControllerCallback?.Invoke(result);
                }
            }
            return null;
        }
    }
}
