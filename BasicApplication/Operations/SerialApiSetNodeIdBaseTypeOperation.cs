/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiSetNodeIdBaseTypeOperation : SerialApiSetupOperation
    {
        public const byte SERIAL_API_SETUP_CMD_NODEID_BASETYPE_SET = (1 << 7);

        public SerialApiSetNodeIdBaseTypeOperation(NetworkViewPoint network, byte version)
            : base(SERIAL_API_SETUP_CMD_NODEID_BASETYPE_SET, version)
        {
            _network = network;
        }

        protected override void CreateWorkflow()
        {
            base.CreateWorkflow();
            StopActionUnit = new StopActionUnit(OnStop, 0, null);
        }

        private void OnStop(StopActionUnit ou)
        {
            SetStateCompleted(ou);
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            var res = base.SpecificResult.ByteArray;
            if (res != null && res.Length > 1)
            {
                if (res[1] == 1)
                {
                    _network.NodeIdBaseType = NodeIdBaseTypes.Base2;
                }
            }
        }
    }
}
