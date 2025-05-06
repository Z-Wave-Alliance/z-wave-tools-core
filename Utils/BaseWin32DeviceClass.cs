/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace Utils
{
    public class BaseWin32DeviceClass
    {
        public string HardwareId { get; set; }

        public ushort? Availability { get; set; }

        public string Caption { get; set; }

        public uint? ConfigManagerErrorCode { get; set; }

        public bool? ConfigManagerUserConfig { get; set; }

        public string CreationClassName { get; set; }

        public string Description { get; set; }

        public string DeviceId { get; set; }

        public string Name { get; set; }

        public string PnpDeviceId { get; set; }

        public ushort[] PowerManagementCapabilities { get; set; }

        public bool? PowerManagementSupported { get; set; }

        public string Status { get; set; }

        public ushort? StatusInfo { get; set; }

        public string SystemCreationClassName { get; set; }

        public string SystemName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
