/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x54 | nodeID | SUCState | bTxOption | capabilities | funcID
    /// ZW->HOST: RES | 0x54 | RetVal
    /// ZW->HOST: REQ | 0x54 | funcID | txStatus
    /// 
    /// In case ZW_SetSUCNodeID is called locally with the controllers own node ID then only the response is returned. 
    /// In case true is returned in the response then it can be interpreted as the command is now executed successfully.
    /// </summary>
    public class SetSucSelfOperation : ControlApiOperation
    {
        internal NodeTag Node { get; set; }
        private byte SucState { get; set; }
        private bool IsLowPower { get; set; }
        private byte Capabilities { get; set; }
        public SetSucSelfOperation(NetworkViewPoint network, NodeTag node, byte sucState, bool isLowPower, byte capabilities)
            : base(CommandTypes.CmdZWaveSetSucNodeId)
        {
            _network = network;
            Node = node;
            SucState = sucState;
            IsLowPower = isLowPower;
            Capabilities = capabilities;
        }

        protected override byte[] CreateInputParameters()
        {
            List<byte> ret = new List<byte>();
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret.Add((byte)(Node.Id >> 8));
            }
            ret.Add((byte)Node.Id);
            ret.Add(SucState);
            ret.Add((byte)(IsLowPower ? 1 : 0));
            ret.Add(Capabilities);
            return ret.ToArray();
        }
    }
}
