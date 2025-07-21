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
    public class SetSucNodeIdOperation : CallbackApiOperation
    {
        public NodeTag Node { get; set; }
        private byte SucState { get; set; }
        private bool IsLowPower { get; set; }
        private byte Capabilities { get; set; }
        public SetSucNodeIdOperation(NetworkViewPoint network, NodeTag node, byte sucState, bool isLowPower, byte capabilities)
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

        protected override void OnCallback(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.RetVal = (SetSucReturnValues)ou.DataFrame.Payload[1];
                if (isHandled && (SpecificResult.RetVal == SetSucReturnValues.SucSetSucceeded))
                    SetStateCompleted(ou);
                else
                    SetStateFailed(ou);
            }
            else
                SetStateFailed(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Status={0}", SpecificResult.RetVal);
        }

        public SetSucNodeIdResult SpecificResult
        {
            get { return (SetSucNodeIdResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetSucNodeIdResult();
        }
    }

    public class SetSucNodeIdResult : ActionResult
    {
        public SetSucNodeIdResult()
            : base()
        {

        }
        public SetSucNodeIdResult(ActionResult result)
            : base(result)
        {

        }
        public SetSucReturnValues RetVal { get; set; }
    }
}
