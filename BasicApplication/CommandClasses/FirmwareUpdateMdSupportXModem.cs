/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    public class FirmwareUpdateMdSupportXModem : FirmwareUpdateMdSupport
    {
        private List<byte[]> _firmwareData;
        private ActionBase _delay;
        private Func<byte[], FirmwareUpdateXModemResult> _firmwareUpdateXModemCallback;
        public FirmwareUpdateMdSupportXModem(NetworkViewPoint network, TransmitOptions txOptions, Action<bool> SetNewImageCompletedCallback, Func<byte[], FirmwareUpdateXModemResult> XModemUpdateCallback) :
            base(network, txOptions, SetNewImageCompletedCallback)
        {
            _firmwareUpdateXModemCallback = XModemUpdateCallback;
        }

        protected override void CreateInstance()
        {
            _firmwareData = new List<byte[]>();
            _delay = new DelayOperation(2000);
            _firmwareUpdateLoaded = _delay;
            base.CreateInstance();
        }
        protected override void FirmareUpdateStart(DataReceivedUnit ou)
        {
            _firmwareData.Clear();
        }

        protected override void FirmwareUpdateAddPacket(DataReceivedUnit ou, IList<byte> data)
        {
            _firmwareData.Add(data.ToArray());
        }

        protected override void FirmwareUpdateLoad(DataReceivedUnit ou)
        {
            var hexData = _firmwareData.SelectMany(x => x).ToArray();
            var receivedFileFirmwareChecksum = Tools.CalculateCrc16Array(hexData);
            var isMatchedCRC = _fwuMdRequestGetChecksum.SequenceEqual(receivedFileFirmwareChecksum);
            if (isMatchedCRC && _firmwareUpdateXModemCallback != null)
            {
                var res = _firmwareUpdateXModemCallback(hexData);
                if (res.UpdateStatus)
                {
                    ReportStatusResult = RESULT_SUCCUESSFULLY_COMPLETED_STATUS;
                }
                else if (res.IsKeyValidationFailed)
                {
                    ReportStatusResult = RESULT_FAILED_KEYS_STATUS;
                }
            }
            else
            {
                ReportStatusResult = RESULT_FAILED_DATA_STATUS;
            }
            _firmwareUpdateLoaded = _delay;
            _firmwareUpdateLoaded.NewToken();
            ou.AddNextActionItems(_firmwareUpdateLoaded);
            ou.AddNextActionItems(SendFwuMdStatusReport());
        }

        protected override void OnSetNewImageCompleted(ActionCompletedUnit ou)
        {
            if (ReportStatusResult == RESULT_SUCCUESSFULLY_COMPLETED_STATUS || ReportStatusResult == RESULT_FAILED_KEYS_STATUS)
            {
                ThreadPool.QueueUserWorkItem((q) =>
                {
                    _setNewImageCompletedCallback(ReportStatusResult == RESULT_SUCCUESSFULLY_COMPLETED_STATUS);
                });
            }
        }
    }

    public class FirmwareUpdateXModemResult
    {
        public bool IsKeyValidationFailed { get; set; }
        public bool UpdateStatus { get; set; }
    }
}
