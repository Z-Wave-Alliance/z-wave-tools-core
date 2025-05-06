/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace Utils
{
    public class Win32SerialPortClass : BaseWin32DeviceClass
    {
        public bool? Binary { get; set; }

        public uint? MaxBaudRate { get; set; }

        public uint? MaximumInputBufferSize { get; set; }

        public uint? MaximumOutputBufferSize { get; set; }

        public bool? OsAutoDiscovered { get; set; }

        public string ProviderType { get; set; }

        public bool? SettableBaudRate { get; set; }

        public bool? SettableDataBits { get; set; }

        public bool? SettableFlowControl { get; set; }

        public bool? SettableParity { get; set; }

        public bool? SettableParityCheck { get; set; }

        public bool? SettableRlsd { get; set; }

        public bool? SettableStopBits { get; set; }

        public bool? Supports16BitMode { get; set; }

        public bool? SupportsDtrdsr { get; set; }

        public bool? SupportsElapsedTimeouts { get; set; }

        public bool? SupportsIntTimeouts { get; set; }

        public bool? SupportsParityCheck { get; set; }

        public bool? SupportsRlsd { get; set; }

        public bool? SupportsRtscts { get; set; }

        public bool? SupportsSpecialCharacters { get; set; }

        public bool? SupportsXonXOff { get; set; }

        public bool? SupportsXonXOffSet { get; set; }
    }
}
