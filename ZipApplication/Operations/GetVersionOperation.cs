/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Enums;

namespace ZWave.ZipApplication.Operations
{
    public class GetVersionOperation : RequestDataOperation
    {
        public GetVersionOperation() :
            base(null, null, COMMAND_CLASS_VERSION_V2.ID, COMMAND_CLASS_VERSION_V2.VERSION_REPORT.ID, 2000)
        {
            Data = new COMMAND_CLASS_VERSION_V2.VERSION_GET();
            IsNoAck = true;
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_VERSION_V2.VERSION_REPORT packet = (COMMAND_CLASS_VERSION_V2.VERSION_REPORT)ou.DataFrame.Payload;
            SpecificResult.Firmware0Version = string.Format("{0}.{1}", packet.firmware0Version, packet.firmware0SubVersion);
            SpecificResult.HardwareVersion = packet.hardwareVersion.ToString();
            SpecificResult.NumberOfFirmwareTargets = packet.numberOfFirmwareTargets;
            SpecificResult.Library = (Libraries)packet.zWaveLibraryType.Value;
            SpecificResult.Firmware0Version = string.Format("{0}.{1}", packet.zWaveProtocolVersion, packet.zWaveProtocolSubVersion);
            if (packet.vg != null)
            {
                SpecificResult.FirmwareVersions = new List<string>();
                foreach (var item in packet.vg)
                {
                    SpecificResult.FirmwareVersions.Add(string.Format("{0}.{1}", item.firmwareVersion, item.firmwareSubVersion));
                }
            }
            base.SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("ver.{0}", SpecificResult.Firmware0Version);
        }

        public new GetVersionResult SpecificResult
        {
            get { return (GetVersionResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetVersionResult();
        }
    }

    public class GetVersionResult : RequestDataResult
    {
        public string Firmware0Version { get; set; }
        public string HardwareVersion { get; set; }
        public int NumberOfFirmwareTargets { get; set; }
        public Libraries Library { get; set; }
        public string ProtocolVersion { get; set; }
        public List<string> FirmwareVersions { get; set; }
    }

}
