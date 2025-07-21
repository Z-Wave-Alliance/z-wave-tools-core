/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZWave.BasicApplication.Operations;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    public class FirmwareUpdateMdSupportBasic : FirmwareUpdateMdSupport
    {
        private int _firmwareOffset;
        private FirmwareUpdateNvmSetNewImageOperation _firmwareUpdateNvmSetNewImageTrue;
        private FirmwareUpdateNvmSetNewImageOperation _firmwareUpdateNvmSetNewImageFalse;
        private FirmwareUpdateNvmInitOperation _firmwareUpdateNvmInit;

        public FirmwareUpdateMdSupportBasic(NetworkViewPoint network, TransmitOptions txOptions, Action<bool> SetNewImageCompletedCallback) :
            base(network, txOptions, SetNewImageCompletedCallback)
        {
        }

        protected override void CreateInstance()
        {
            _firmwareUpdateNvmSetNewImageTrue = new FirmwareUpdateNvmSetNewImageOperation(true);
            _firmwareUpdateNvmSetNewImageFalse = new FirmwareUpdateNvmSetNewImageOperation(false);
            _firmwareUpdateNvmInit = new FirmwareUpdateNvmInitOperation();
            _firmwareUpdateLoaded = _firmwareUpdateNvmSetNewImageTrue;

            base.CreateInstance();
        }

        protected override void FirmareUpdateStart(DataReceivedUnit ou)
        {
            _firmwareOffset = 0;
            _firmwareUpdateNvmInit.NewToken();
            ou.AddNextActionItems(_firmwareUpdateNvmInit);
            _firmwareUpdateNvmSetNewImageFalse.NewToken();
            ou.AddNextActionItems(_firmwareUpdateNvmSetNewImageFalse);
        }

        protected override void FirmwareUpdateAddPacket(DataReceivedUnit ou, IList<byte> data)
        {
            ushort dataLen = (ushort)data.Count;
            var _firmwareUpdateNvmWrite = new FirmwareUpdateNvmWriteOperation(_firmwareOffset, dataLen, data.ToArray());
            ou.SetNextActionItems(_firmwareUpdateNvmWrite);
            _firmwareOffset += dataLen;
        }

        protected override void FirmwareUpdateLoad(DataReceivedUnit ou)
        {
            _firmwareUpdateLoaded = _firmwareUpdateNvmSetNewImageTrue;
            _firmwareUpdateLoaded.NewToken();
            ou.AddNextActionItems(_firmwareUpdateLoaded);
            ReportStatusResult = RESULT_FAILED_STATUS;
        }

        protected override void OnSetNewImageCompleted(ActionCompletedUnit ou)
        {
            ReportStatusResult = RESULT_FAILED_STATUS;
            if (ou.Action.Result is FirmwareUpdateNvmSetNewImageResult)
            {
                var res = (FirmwareUpdateNvmSetNewImageResult)ou.Action.Result;
                if (res.IsSet)
                {
                    ReportStatusResult = RESULT_SUCCUESSFULLY_COMPLETED_STATUS;
                }
            }
            ou.AddNextActionItems(SendFwuMdStatusReport());
            ThreadPool.QueueUserWorkItem((q) =>
            {
                _setNewImageCompletedCallback(ReportStatusResult == RESULT_SUCCUESSFULLY_COMPLETED_STATUS);
            });
        }
    }
}
