/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Enums;
using ZWave.ZnifferApplication.Enums;
using Utils;

namespace ZWave.ZnifferApplication.Operations
{
    public class GetVersionOperation : ActionBase
    {
        private const int TIMEOUT_MS = 100;
        public GetVersionOperation()
            : this(ZnifferApiVersion.V4)
        {
        }

        public GetVersionOperation(ZnifferApiVersion apiVersion)
            : base(true)
        {
            SpecificResult.ApiVersion = apiVersion;
        }

        public ZnifferApiMessage GetVersionMessage { get; set; }
        public ZnifferApiHandler GetVersionHandler { get; set; }
        public ZnifferApiMessage Stop4xMessage { get; set; }
        public ZnifferApiHandler Stop4xHandler { get; set; }

        //public ZnifferApiHandler GetDeviceInfo3xHandler { get; set; }
        //public ZnifferApiMessage GetDeviceInfo3xMessage { get; set; }

        protected override void CreateWorkflow()
        {
            switch (SpecificResult.ApiVersion)
            {
                case ZnifferApiVersion.Unknown:
                    ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, Stop4xMessage/*, GetDeviceInfo3xMessage*/));
                    ActionUnits.Add(new DataReceivedUnit(Stop4xHandler, null, TIMEOUT_MS, GetVersionMessage));
                    ActionUnits.Add(new DataReceivedUnit(GetVersionHandler, OnDone4x));
                    //ActionUnits.Add(new DataReceivedUnit(GetDeviceInfo3xHandler, OnDone3x));
                    break;
                //case ZnifferApiVersion.V3:
                //    ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, GetDeviceInfo3xMessage));
                //    ActionUnits.Add(new DataReceivedUnit(GetDeviceInfo3xHandler, OnDone3x));
                //    break;
                case ZnifferApiVersion.V4:
                    ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, Stop4xMessage));
                    ActionUnits.Add(new DataReceivedUnit(Stop4xHandler, null, TIMEOUT_MS, GetVersionMessage));
                    ActionUnits.Add(new DataReceivedUnit(GetVersionHandler, OnDone4x));
                    break;
                default:
                    break;
            }
        }

        protected override void CreateInstance()
        {
            GetVersionMessage = new ZnifferApiMessage(CommandTypes.GetVersion4x, new byte[] { 0x00 });
            GetVersionHandler = new ZnifferApiHandler(CommandTypes.GetVersion4x);
            Stop4xMessage = new ZnifferApiMessage(CommandTypes.Stop4x, new byte[] { 0x00 });
            Stop4xHandler = new ZnifferApiHandler(CommandTypes.Stop4x);

            //GetDeviceInfo3xHandler = new ZnifferApiHandler(CommandTypes.GetDeviceInfo3xResponse);
            //GetDeviceInfo3xMessage = new ZnifferApiMessage(CommandTypes.GetDeviceInfo3x, new byte[] { 0x47 });
        }

        private void OnDone4x(DataReceivedUnit ou)
        {
            SpecificResult.ApiVersion = ZnifferApiVersion.V4;
            byte[] payload = ou.DataFrame.Payload;
            if (payload != null && payload.Length > 3)
            {
                SpecificResult.ChipType = payload[0];
                SpecificResult.ChipVersion = payload[1];
                SpecificResult.SnifferVersion = payload[2];
                SpecificResult.SnifferRevision = payload[3];
            }
            SetStateCompleted(ou);
        }

        private void OnDone3x(DataReceivedUnit ou)
        {
            SpecificResult.ApiVersion = ZnifferApiVersion.V3;
            byte[] payload = ou.DataFrame.Payload;
            if (payload != null && payload.Length > 5)
            {
                SpecificResult.CurrentFrequency = payload[0];
                if (SpecificResult.CurrentFrequency == 10) // for RU frequency
                    SpecificResult.CurrentFrequency = 26;
                if (SpecificResult.CurrentFrequency == 11) // for IL frequency
                    SpecificResult.CurrentFrequency = 27;
                SpecificResult.SupportedFrequencies = payload[1];
                SpecificResult.ChipType = payload[2];
                SpecificResult.ChipVersion = payload[3];
                SpecificResult.SnifferVersion = payload[4];
                SpecificResult.SnifferRevision = payload[5];
            }
            SetStateCompleted(ou);
        }

        public GetVersionResult SpecificResult
        {
            get { return (GetVersionResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetVersionResult();
        }
    }

    public class GetVersionResult : ActionResult
    {
        public ZnifferApiVersion ApiVersion { get; set; }
        public byte ChipType { get; set; }
        public byte ChipVersion { get; set; }
        public byte SnifferVersion { get; set; }
        public byte SnifferRevision { get; set; }

        public byte CurrentFrequency { get; set; }
        public byte SupportedFrequencies { get; set; }
    }
}
