/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using Utils;
using ZWave.Enums;
using ZWave.Layers.Transport;

namespace ZWave.Layers
{
    public partial class SerialPortDataSource : DataSourceBase
    {
        public BaudRates BaudRate { get; set; }
        public PInvokeStopBits StopBits { get; set; }
        public bool IsSigmaDesignsUsbProgrammingDriver { get; set; }
        public bool IsUzbDriver { get; set; }
        public SerialPortDataSource()
        {
            StopBits = PInvokeStopBits.One;
            BaudRate = BaudRates.Rate_115200;
        }

        public SerialPortDataSource(string sourceName)
            : this(sourceName, BaudRates.Rate_115200, PInvokeStopBits.One, null, null)
        {
        }

        public SerialPortDataSource(string sourceName, BaudRates baudRate)
            : this(sourceName, baudRate, PInvokeStopBits.One, null, null)
        {
        }

        public SerialPortDataSource(string sourceName, BaudRates baudRate, PInvokeStopBits stopBits)
            : this(sourceName, baudRate, stopBits, null, null)
        {
        }

        public SerialPortDataSource(string sourceName, BaudRates baudRate, string alias, string args)
           : this(sourceName, baudRate, PInvokeStopBits.One, alias, args)
        {
        }


        public SerialPortDataSource(string sourceName, BaudRates baudRate, PInvokeStopBits stopBits, string alias, string args)
        {
            SourceName = sourceName;
            BaudRate = baudRate;
            StopBits = stopBits;
            Alias = alias;
            Args = args;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(SourceName);
        }

        public static AutoProgDeviceTypes GetAutoProgType(string portName)
        {
            AutoProgDeviceTypes ret = AutoProgDeviceTypes.NONE;
            Win32PnPEntityClass portInfo = null;
#if !NETCOREAPP
            try
            {
                portInfo = ComputerSystemHardwareHelper.GetWin32PnPEntityClassSerialPortDevice(portName);
            }
            catch (Exception)
            {
                "can't get port list from System.Management"._DLOG();
            }
#endif
            if (portInfo != null)
            {
                ret = AutoProgDeviceTypes.UART;
                if (portInfo.Description.ToUpper().Contains("Sigma Designs ZWave programming interface".ToUpper()))
                {
                    if (portInfo.HardwareId == "0001")
                        ret = AutoProgDeviceTypes.SD_USB_0001;
                    else
                        ret = AutoProgDeviceTypes.SD_USB_0000;
                }
                else
                {
                    if (portInfo.Description.ToUpper().Contains("UZB") || portInfo.Description.ToUpper().Contains("ZCOM"))
                    {
                        if (portInfo.HardwareId == "0001")
                            ret = AutoProgDeviceTypes.UZB_0001;
                        else
                            ret = AutoProgDeviceTypes.UZB_0000;
                    }
                }
            }
            ret.ToString()._DLOG();
            return ret;
        }

        public override string ToString()
        {
            return SourceName;
        }

        protected override string GenerateSourceId()
        {
            if (SourceName[0] == '/')
            {
                try
                {
                    var splitted = SourceName.Substring(1).Split('/');
                    if (splitted.Length == 2)
                    {
                        return $"{splitted[0]}_{splitted[1]}";
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                }
            }
            return SourceName;
        }
    }

    public enum AutoProgDeviceTypes
    {
        NONE,
        UART,
        UZB_0000,
        UZB_0001,
        SD_USB_0000,
        SD_USB_0001
    }


    public class SerialPortProgrammerDataSource : SerialPortDataSource
    {
        public SerialPortProgrammerDataSource(string sourceName)
            : base(sourceName, BaudRates.Rate_115200)
        {
            Win32PnPEntityClass device = null;
#if !NETCOREAPP
            try
            {
                device = ComputerSystemHardwareHelper.GetWin32PnPEntityClassSerialPortDevice(sourceName);
            }
            catch (Exception)
            {
                "can't get port list from System.Management"._DLOG();
            }
#endif
            if (device != null && device.Caption != null && device.Caption.Contains("Sigma Designs"))
            {
                StopBits = PInvokeStopBits.One;
                IsSigmaDesignsUsbProgrammingDriver = true;
            }
            else if (device != null && device.Caption != null && (device.Caption.Contains("UZB") || device.Caption.Contains("ZCOM")))
            {
                IsUzbDriver = true;
            }
            else
            {
                StopBits = PInvokeStopBits.Two; //UART
            }
        }
    }

}
