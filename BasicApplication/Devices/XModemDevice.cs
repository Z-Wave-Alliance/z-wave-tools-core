/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.Operations;

namespace ZWave.BasicApplication.Devices
{
    public class XModemDevice : ApplicationClient
    {
        public string ErrorMessage { get { return ((XModemFrameClient)FrameClient).CancelationMessage; } }

        public bool IsKeyValidationFailed { get { return ((XModemFrameClient)FrameClient).ErrorCode == (int)XModemErrorCodes.KeyValidation; } }

        public XModemDevice(ISessionClient sc, IFrameClient fc, ITransportClient tc) :
            base(ApiTypes.XModem, 0, sc, fc, tc)
        {
        }

        public ActionResult Execute(ActionBase action)
        {
            action.Token.LogEntryPointCategory = "XModem";
            action.Token.LogEntryPointSource = DataSource == null ? string.Empty : DataSource.SourceName;
            var token = SessionClient.ExecuteAsync(action);
            return WaitCompletedSignal(token);
        }

        public new bool Send(byte[] data)
        {
            bool ret = false;
            var result = Execute(new SendOperation(data) { IsSequenceNumberRequired = false });
            ret = result != null && result.IsStateCompleted;
            return ret;
        }

        public bool WaitUpdateSessionReady(int timeout)
        {
            return ((XModemFrameClient)FrameClient).WaitReady(timeout);
        }

        public bool ConfirmUpdate(int timeout)
        {
            return ((XModemFrameClient)FrameClient).CloseSession(timeout);
        }
    }
}
