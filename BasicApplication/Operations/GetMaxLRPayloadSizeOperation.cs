/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x11
    /// ZW->HOST: RES | 0x0B | 0x11 | maxPayloadSize
    /// ZW->HOST: RES | 0x0B | 0x00 | 0x10
    /// </summary>
    public class GetMaxLRPayloadSizeOperation : SerialApiSetupOperation
    {
        public const byte SERIAL_API_SETUP_CMD_TX_GET_MAX_LR_PAYLOAD_SIZE = 0x11;

        public GetMaxLRPayloadSizeOperation(NetworkViewPoint network)
            : base(SERIAL_API_SETUP_CMD_TX_GET_MAX_LR_PAYLOAD_SIZE)
        {
            _network = network;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            var res = base.SpecificResult.ByteArray;
            if (res != null && res.Length > 1)
            {
                if (res[0] == SERIAL_API_SETUP_CMD_TX_GET_MAX_LR_PAYLOAD_SIZE) // if the subfunction is supported
                {
                    (SpecificResult as GetMaxPayloadSizeResult).MaxPayloadSize = base.SpecificResult.ByteArray[1];
                    _network.TransportServiceMaxLRSegmentSize = base.SpecificResult.ByteArray[1];
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
}
