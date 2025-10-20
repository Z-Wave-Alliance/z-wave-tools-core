/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ZWave.Layers.Transport
{
#if NETCOREAPP
    public class UzbProviderUnix : SafeSerialSourceAccess<int>
    {
        #region DllImports

        private static bool Is64Bit { get { return Environment.Is64BitProcess; } }

        [DllImport("basictrans32", EntryPoint = "SetLogLevel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void SetLogLevel32(byte logLevel);

        [DllImport("basictrans64", EntryPoint = "SetLogLevel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void SetLogLevel64(byte logLevel);

        public static void SetLogLevel(byte logLevel)
        {
            if (Is64Bit)
            {
                SetLogLevel64(logLevel);
            }
            else
            {
                SetLogLevel32(logLevel);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct USB_DEVICE_ADDRESS
        {
            public byte busNo;
            public byte deviceAddress;
        }

        [DllImport("basictrans32", EntryPoint = "EnumUZBSticks", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.U4)]
        private extern static uint EnumUZBSticks32([In, Out] USB_DEVICE_ADDRESS[] lpDevices);

        [DllImport("basictrans64", EntryPoint = "EnumUZBSticks", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.U4)]
        private extern static uint EnumUZBSticks64([In, Out] USB_DEVICE_ADDRESS[] lpDevices);

        private static uint EnumUZBSticks(USB_DEVICE_ADDRESS[] lpDevices)
        {
            return Is64Bit ? EnumUZBSticks64(lpDevices) : EnumUZBSticks32(lpDevices);
        }

        [DllImport("basictrans32", EntryPoint = "OpenSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool OpenSerialPort32(byte busNo, byte addressNo);

        [DllImport("basictrans64", EntryPoint = "OpenSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool OpenSerialPort64(byte busNo, byte addressNo);

        private static bool OpenSerialPort(byte busNo, byte addressNo)
        {
            return Is64Bit ?
                OpenSerialPort64(busNo, addressNo) :
                OpenSerialPort32(busNo, addressNo);
        }

        [DllImport("basictrans32", EntryPoint = "CloseSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void CloseSerialPort32(byte busNo, byte addressNo);

        [DllImport("basictrans64", EntryPoint = "CloseSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void CloseSerialPort64(byte busNo, byte addressNo);

        private static void CloseSerialPort(byte busNo, byte addressNo)
        {
            if (Is64Bit)
            {
                CloseSerialPort64(busNo, addressNo);
            }
            else
            {
                CloseSerialPort32(busNo, addressNo);
            }
        }

        [DllImport("basictrans32", EntryPoint = "SerialPortRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SerialPortRead32(byte busNo,
            byte addressNo,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            uint dataLen,
            uint timeout);

        [DllImport("basictrans64", EntryPoint = "SerialPortRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SerialPortRead64(byte busNo,
            byte addressNo,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            uint dataLen,
            uint timeout);

        private static int SerialPortRead(byte busNo, byte addressNo, byte[] pData, uint dataLen, uint timeout)
        {
            return Is64Bit ?
                SerialPortRead64(busNo, addressNo, pData, dataLen, timeout) :
                SerialPortRead32(busNo, addressNo, pData, dataLen, timeout);
        }

        [DllImport("basictrans32", EntryPoint = "SerialPortWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SerialPortWrite32(byte busNo,
            byte addressNo,
            byte[] pData,
            uint dataLen,
            uint timeout);

        [DllImport("basictrans64", EntryPoint = "SerialPortWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SerialPortWrite64(byte busNo,
            byte addressNo,
            byte[] pData,
            uint dataLen,
            uint timeout);

        private static int SerialPortWrite(byte busNo, byte addressNo, byte[] pData, uint dataLen, uint timeout)
        {
            return Is64Bit ?
                SerialPortWrite64(busNo, addressNo, pData, dataLen, timeout) :
                SerialPortWrite32(busNo, addressNo, pData, dataLen, timeout);
        }

        [DllImport("basictrans32", EntryPoint = "SerialPortIsOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool SerialPortIsOpen32(byte busNo, byte addressNo);

        [DllImport("basictrans64", EntryPoint = "SerialPortIsOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool SerialPortIsOpen64(byte busNo, byte addressNo);

        private static bool SerialPortIsOpen(byte busNo, byte addressNo)
        {
            return Is64Bit ? SerialPortIsOpen64(busNo, addressNo) : SerialPortIsOpen32(busNo, addressNo);
        }

        #endregion

        private byte _busNo;
        private byte _addressNo;

        protected override bool CreateKey(string sourceName, out int key)
        {
            var nums = Regex.Matches(sourceName, @"\d{3}");
            if (nums.Count == 2 &&
                byte.TryParse(nums[0].Value, out _busNo) &&
                byte.TryParse(nums[1].Value, out _addressNo)
                )
            {
                key = (_addressNo << 8) | _busNo;
                return true;
            }
            key = -1;
            return false;
        }


        protected override bool IsOpenInternal()
        {
            return SerialPortIsOpen(_busNo, _addressNo);
        }


        protected override void CloseInternal()
        {
            CloseSerialPort(_busNo, _addressNo);
        }

        protected override bool OpenInternal(int baudRate)
        {
            return OpenSerialPort(_busNo, _addressNo);
        }

        protected override int ReadInternal(byte[] buffer, int bufferLen)
        {
            return SerialPortRead(_busNo, _addressNo, buffer, (uint)bufferLen, 0);
        }

        protected override byte[] ReadExistingInternal()
        {
            var buffer = new byte[512];
            var readLen = SerialPortRead(_busNo, _addressNo, buffer, (uint)buffer.Length, 500);
            if (readLen != -1)
            {
                return buffer.Take(readLen).ToArray();
            }
            return null;
        }

        protected override int WriteInternal(byte[] buffer, int bufferLen)
        {
            return SerialPortWrite(_busNo, _addressNo, buffer, (uint)bufferLen, 0);
        }

        public static string[] EnumConnectedUZBSticks()
        {
            var count = EnumUZBSticks(null);
            if (count > 0)
            {
                var devices = new USB_DEVICE_ADDRESS[count];
                EnumUZBSticks(devices);
                return devices.Select(addr => $"{addr.busNo:D3}_{addr.deviceAddress:D3}").ToArray();
            }
            return new string[] { };
        }
    }
#endif
}
