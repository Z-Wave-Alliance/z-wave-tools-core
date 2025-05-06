/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils
{
    public class Win32PnPEntityClass : BaseWin32DeviceClass
    {
        public string SerialPortName { get; set; }

        public string ClassGuid { get; set; }

        public bool? ErrorCleared { get; set; }

        public string ErrorDescription { get; set; }

        public DateTime? InstallDate { get; set; }

        public int? LastErrorCode { get; set; }

        public string Manufacturer { get; set; }

        public string Service { get; set; }
    }
}
