/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x10
    /// ZW->HOST: RES | 0x0B | 0x10 | maxPayloadSize
    /// ZW->HOST: RES | 0x0B | 0x00 | 0x10
    /// </summary>
    public class GetMaxPayloadSizeOperation : SerialApiSetupOperation
    {
        public const byte SERIAL_API_SETUP_CMD_TX_GET_MAX_PAYLOAD_SIZE = 0x10;

        public GetMaxPayloadSizeOperation(NetworkViewPoint network)
            : base(SERIAL_API_SETUP_CMD_TX_GET_MAX_PAYLOAD_SIZE)
        {
            _network = network;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            var res = base.SpecificResult.ByteArray;
            if (res != null && res.Length > 1)
            {
                if (res[0] == 0x10) // if the subfunction is supported
                {
                    (SpecificResult as GetMaxPayloadSizeResult).MaxPayloadSize = base.SpecificResult.ByteArray[1];
                    _network.TransportServiceMaxSegmentSize = base.SpecificResult.ByteArray[1];
                    _network.S0MaxBytesPerFrameSize = base.SpecificResult.ByteArray[1] - 20; // S0 message overhead = 20 bytes
                }
                else
                {
                    // SERIAL_API_SETUP_CMD_SUPPORTED
                    (SpecificResult as GetMaxPayloadSizeResult).MaxPayloadSize = 0;
                }
            }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetMaxPayloadSizeResult();
        }
    }

    public class GetMaxPayloadSizeResult : ReturnValueResult
    {
        public byte MaxPayloadSize { get; set; }
    }
}
