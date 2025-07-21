/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiGetCapabilitiesOperation : RequestApiOperation
    {
        public SerialApiGetCapabilitiesOperation(NetworkViewPoint network)
            : base(CommandTypes.CmdSerialApiGetCapabilities, 2000)
        {
            _network = network;
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.SerialApplicationVersion = res[0];
            SpecificResult.SerialApplicationRevision = res[1];
            SpecificResult.ManufacturerId = (ushort)((res[2] << 8) + res[3]);
            SpecificResult.ManufacturerProductType = (ushort)((res[4] << 8) + res[5]);
            SpecificResult.ManufacturerProductId = (ushort)((res[6] << 8) + res[7]);
            byte funcIdx = 0;
            List<byte> SupportedFuncIds = new List<byte>();
            var tmp = new BitArray(res.Skip(8).ToArray());
            _network.SupportedSerialApiCommands = new BitArray(256);
            for (int j = 8; j < res.Length; j++)
            {
                byte availabilityMask = res[j];
                for (byte bit = 0; bit < 8; bit++)
                {
                    funcIdx++;
                    if ((availabilityMask & (1 << bit)) > 0)
                    {
                        SupportedFuncIds.Add(funcIdx);
                        _network.SupportedSerialApiCommands[funcIdx] = true;
                    }
                }
            }
            SpecificResult.SupportedSerialApiCommands = SupportedFuncIds.ToArray();
            base.SetStateCompleted(ou);
        }

        public SerialApiGetCapabilitiesResult SpecificResult
        {
            get { return (SerialApiGetCapabilitiesResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SerialApiGetCapabilitiesResult();
        }
    }

    public class SerialApiGetCapabilitiesResult : ActionResult
    {
        public byte SerialApplicationVersion { get; set; }
        public byte SerialApplicationRevision { get; set; }
        public ushort ManufacturerId { get; set; }
        public ushort ManufacturerProductType { get; set; }
        public ushort ManufacturerProductId { get; set; }
        public byte[] SupportedSerialApiCommands { get; set; }
    }
}
