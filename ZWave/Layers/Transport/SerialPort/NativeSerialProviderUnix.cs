/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace ZWave.Layers.Transport
{
#if NETCOREAPP
    public class NativeSerialProviderUnix : SafeSerialSourceAccess<string>
    {
        #region DllImports

        private const int MAX_ENUM_BUFF = 64;
        private const int MAX_PORT_NAME = 64;

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

        [DllImport("basictrans64", EntryPoint = "EnumNativeSerialPorts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.U4)]
        private extern static uint EnumNativeSerialPorts64([In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPTStr, SizeConst = MAX_ENUM_BUFF)] IntPtr[] ppSerialPorts, int size);

        [DllImport("basictrans32", EntryPoint = "EnumNativeSerialPorts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.U4)]
        private extern static uint EnumNativeSerialPorts32([In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPTStr, SizeConst = MAX_ENUM_BUFF)] IntPtr[] ppSerialPorts, int size);

        private static uint EnumNativeSerialPorts(IntPtr[] ppSerialPorts, int size)
        {
            return Is64Bit ? EnumNativeSerialPorts64(ppSerialPorts, size) : EnumNativeSerialPorts32(ppSerialPorts, size);
        }
        
        [DllImport("basictrans64", EntryPoint = "OpenNativeSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool OpenNativeSerialPort64([In, MarshalAs(UnmanagedType.LPStr)] string portName, int baudRate);

        [DllImport("basictrans32", EntryPoint = "OpenNativeSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool OpenNativeSerialPort32([In, MarshalAs(UnmanagedType.LPStr)] string portName, int baudRate);

        private static bool OpenNativeSerialPort(string portName, int baudRate)
        {
            return Is64Bit ? OpenNativeSerialPort64(portName, baudRate) : OpenNativeSerialPort32(portName, baudRate);
        }

        [DllImport("basictrans64", EntryPoint = "NativeSerialPortRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int NativeSerialPortRead64([In, MarshalAs(UnmanagedType.LPStr)] string portName,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            uint dataLen,
            int timeout);

        [DllImport("basictrans32", EntryPoint = "NativeSerialPortRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int NativeSerialPortRead32([In, MarshalAs(UnmanagedType.LPStr)] string portName,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            uint dataLen,
            int timeout);

        private static int NativeSerialPortRead(string portName, byte[] pData, uint dataLen, int timeout)
        {
            return Is64Bit ? NativeSerialPortRead64(portName, pData, dataLen, timeout) : NativeSerialPortRead32(portName, pData, dataLen, timeout);
        }


        [DllImport("basictrans64", EntryPoint = "NativeSerialPortWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int NativeSerialPortWrite64([In, MarshalAs(UnmanagedType.LPStr)] string portName,
            byte[] pData,
            uint dataLen);

        [DllImport("basictrans32", EntryPoint = "NativeSerialPortWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int NativeSerialPortWrite32([In, MarshalAs(UnmanagedType.LPStr)] string portName,
            byte[] pData,
            uint dataLen);

        private static int NativeSerialPortWrite(string portName, byte[] pData, uint dataLen)
        {
            return Is64Bit ? NativeSerialPortWrite64(portName, pData, dataLen) : NativeSerialPortWrite32(portName, pData, dataLen);
        }

        [DllImport("basictrans64", EntryPoint = "CloseNativeSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void CloseNativeSerialPort64([In, MarshalAs(UnmanagedType.LPStr)] string portName);

        [DllImport("basictrans32", EntryPoint = "CloseNativeSerialPort", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void CloseNativeSerialPort32([In, MarshalAs(UnmanagedType.LPStr)] string portName);

        private static void CloseNativeSerialPort(string portName)
        {
            if (Is64Bit)
            {
                CloseNativeSerialPort64(portName);
            }
            else
            {
                CloseNativeSerialPort32(portName);
            }
        }

        [DllImport("basictrans64", EntryPoint = "NativeSerialPortIsOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool NativeSerialPortIsOpen64([In, MarshalAs(UnmanagedType.LPStr)] string portName);

        [DllImport("basictrans32", EntryPoint = "NativeSerialPortIsOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool NativeSerialPortIsOpen32([In, MarshalAs(UnmanagedType.LPStr)] string portName);

        private static bool NativeSerialPortIsOpen([In, MarshalAs(UnmanagedType.LPStr)] string portName)
        {
            return Is64Bit ? NativeSerialPortIsOpen64(portName) : NativeSerialPortIsOpen32(portName);
        }

        #endregion

        private string _serialPortName;

        protected override void CloseInternal()
        {
            CloseNativeSerialPort(_serialPortName);
        }

        protected override bool CreateKey(string sourceName, out string key)
        {
            if (!string.IsNullOrEmpty(sourceName))
            {
                _serialPortName = sourceName;
                key = _serialPortName;
                return true;
            }
            key = null;
            return false;
        }

        protected override bool IsOpenInternal()
        {
            return NativeSerialPortIsOpen(_serialPortName);
        }

        protected override bool OpenInternal(int baudRate)
        {
            return OpenNativeSerialPort(_serialPortName, baudRate);
        }

        protected override byte[] ReadExistingInternal()
        {
            var buffer = new byte[512];
            int readLen = NativeSerialPortRead(_serialPortName, buffer, (uint)buffer.Length, 500);
            if (readLen != -1)
            {
                return buffer.Take(readLen).ToArray();
            }
            return null;
        }

        protected override int ReadInternal(byte[] buffer, int bufferLen)
        {
            return NativeSerialPortRead(_serialPortName, buffer, (uint)bufferLen, -1);
        }

        protected override int WriteInternal(byte[] buffer, int bufferLen)
        {
            return NativeSerialPortWrite(_serialPortName, buffer, (uint)bufferLen);
        }

        public static string[] EnumNativeSerialPorts()
        {
            var serialPorts = new IntPtr[MAX_ENUM_BUFF];
            for (int i = 0; i < MAX_ENUM_BUFF; i++)
            {
                serialPorts[i] = Marshal.AllocHGlobal(MAX_PORT_NAME);
                for (int j = 0; j < MAX_PORT_NAME; j++)
                    Marshal.WriteByte(serialPorts[i], j, 0x00);
            }
            int serialPortsCount = (int)EnumNativeSerialPorts(serialPorts, MAX_ENUM_BUFF);
            string[] ports = null;
            if (serialPortsCount > 0)
            {
                ports = serialPorts.Take(serialPortsCount).Select(x => Marshal.PtrToStringAnsi(x)).ToArray();
            }
            for (int i = 0; i < MAX_ENUM_BUFF; i++)
            {
                Marshal.FreeHGlobal(serialPorts[i]);
            }
            return ports;
        }
    }
#endif
}
