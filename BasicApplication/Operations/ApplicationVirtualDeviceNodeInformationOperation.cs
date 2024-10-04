/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xA0 | destNode | listening | genericType | specificType | parmLength | nodeParm[ ]
    /// </summary>
    public class ApplicationVirtualDeviceNodeInformationOperation : ControlNApiOperation
    {
        private NodeTag DestNode { get; set; }
        private bool IsListening { get; set; }
        private byte Generic { get; set; }
        private byte Specific { get; set; }
        private byte[] NodeParameters { get; set; }
        public ApplicationVirtualDeviceNodeInformationOperation(NetworkViewPoint network, NodeTag destNode, bool isListening, byte generic, byte specific, byte[] nodeParameters)
            : base(CommandTypes.CmdSerialApiVirtualDeviceNodeInfo)
        {
            _network = network;
            DestNode = destNode;
            IsListening = isListening;
            Generic = generic;
            Specific = specific;
            NodeParameters = nodeParameters;
        }

        protected override byte[] CreateInputParameters()
        {

            byte[] ret = new byte[NodeParameters.Length + 5];
            ret[0] = (byte)DestNode.Id;
            if (IsListening)
            {
                ret[1] = 0x01;
            }
            else
            {
                ret[1] = 0x00;
            }
            ret[2] = Generic;
            ret[3] = Specific;
            ret[4] = (byte)NodeParameters.Length;
            for (int i = 0; i < NodeParameters.Length; i++)
            {
                ret[i + 5] = NodeParameters[i];
            }
            return ret;
        }
    }
}
