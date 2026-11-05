// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Silicon Laboratories Inc. <https://www.silabs.com>
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Security;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x20
    /// ZW->HOST: RES | 0x20 | HomeId(4) | NodeId(1 or 2)
    /// A Serial API End Device does the security bootstrapping itself and holds the keys, while the NodeTag setter grants
    /// the own node every enabled scheme with the NodeId. For such a library the operation therefore reads the keys with
    /// Security Setup Get Security Keys after the NodeId and completes with them.
    /// </summary>
    public class MemoryGetIdOperation : RequestApiOperation
    {
        public MemoryGetIdOperation(NetworkViewPoint network)
            : base(CommandTypes.CmdMemoryGetId, false)
        {
            _network = network;
        }

        private ApiMessage _getSecurityKeys;
        private ApiHandler _securityKeys;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, message));
            if (_network.Library.IsSerialApiEndDevice())
            {
                ActionUnits.Add(new DataReceivedUnit(handler, OnMemoryGetId, _getSecurityKeys));
                ActionUnits.Add(new DataReceivedUnit(_securityKeys, OnSecurityKeys));
            }
            else
            {
                ActionUnits.Add(new DataReceivedUnit(handler, OnMemoryGetIdCompleted));
            }
        }

        protected override void CreateInstance()
        {
            base.CreateInstance();

            // HOST->ZW: REQ | 0x9C | 0 (Security Setup, Get Security Keys)
            _getSecurityKeys = new ApiMessage(CommandTypes.CmdZWaveSecuritySetup, 0x00);

            _securityKeys = new ApiHandler(FrameTypes.Response, CommandTypes.CmdZWaveSecuritySetup);
            _securityKeys.AddConditions(new ByteIndex(0x00));
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        /// <summary>
        /// Stores the HomeId and NodeId the chip reported in the network view and in the result.
        /// </summary>
        private void OnMemoryGetId(DataReceivedUnit ou)
        {
            byte[] res = ou.DataFrame.Payload;
            SpecificResult.HomeId = new byte[] { res[0], res[1], res[2], res[3] };
            SpecificResult.Node = new NodeTag(res[4]);
            if (_network.IsNodeIdBaseTypeLR && res.Length > 5)
            {
                SpecificResult.Node = new NodeTag((ushort)((res[4] << 8) + res[5]));
            }
            _network.HomeId = SpecificResult.HomeId;
            _network.NodeTag = SpecificResult.Node;
        }

        private void OnMemoryGetIdCompleted(DataReceivedUnit ou)
        {
            OnMemoryGetId(ou);
            SetStateCompleted(ou);
        }

        /// <summary>
        /// Stores the keys the chip reported as the own security schemes of the network view and in the result.
        /// </summary>
        private void OnSecurityKeys(DataReceivedUnit ou)
        {
            // ZW->HOST: RES | 0x9C | 0 | securityKeys_bitmaskLen(1) | securityKeys_bitmask
            byte[] res = ou.DataFrame.Payload;
            if (res != null && res.Length > 2)
            {
                SpecificResult.SecuritySchemes = SecurityManagerInfo.ConvertToSecuritySchemes((NetworkKeyS2Flags)res[2]);
                _network.SetSecuritySchemes(SpecificResult.SecuritySchemes);
            }
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, HomeId={1}", SpecificResult.Node.Id, SpecificResult.HomeId == null ? "" : Tools.GetHexShort(SpecificResult.HomeId));
        }

        public MemoryGetIdResult SpecificResult
        {
            get { return (MemoryGetIdResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new MemoryGetIdResult();
        }
    }

    public class MemoryGetIdResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public byte[] HomeId { get; set; }
        /// <summary>
        /// The keys of a Serial API End Device library, null for every other library.
        /// </summary>
        public SecuritySchemes[] SecuritySchemes { get; set; }
    }
}
