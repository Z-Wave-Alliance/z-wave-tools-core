/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ZWave.CommandClasses;

namespace ZWave.ZipApplication
{
    public class ZipControllerDiscoverService
    {
        const int LOCAL_PORT = 51230; // Any port we choose.
        private List<string> _zipControllerIps = new List<string>();
        private readonly string _ipv4RemoteAddress = null;
        private readonly string _ipv6RemoteAddress = null;
        private readonly int _port;

        #region DllImports

        private bool Is64Bit { get { return Environment.Is64BitProcess; } }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void OnZipDetectedDelegate([In, MarshalAs(UnmanagedType.LPStr)] string pszAddress,
            IntPtr pResponse,
            [In, MarshalAs(UnmanagedType.U4)] uint responseSize);

        [DllImport("ziptrans32", EntryPoint = "DiscoverZip", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void DiscoverZip_32([In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pDiscoverFrame,
            [In, MarshalAs(UnmanagedType.U4)] uint frameSize,
            [In, MarshalAs(UnmanagedType.LPStr)] string ipv6DiscoveryAddress,
            [In, MarshalAs(UnmanagedType.LPStr)] string ipv4DiscoveryAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] OnZipDetectedDelegate onZipDetected);

        [DllImport("ziptrans64", EntryPoint = "DiscoverZip", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void DiscoverZip_64([In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pDiscoverFrame,
            [In, MarshalAs(UnmanagedType.U4)] uint frameSize,
            [In, MarshalAs(UnmanagedType.LPStr)] string ipv6DiscoveryAddress,
            [In, MarshalAs(UnmanagedType.LPStr)] string ipv4DiscoveryAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] OnZipDetectedDelegate onZipDetected);

        private void DiscoverZip(byte[] pDiscoverFrame, uint frameSize, string ipv6DiscoveryAddress, string ipv4DiscoveryAddress, ushort portNo, OnZipDetectedDelegate onZipDetected)
        {
            if (Is64Bit)
            {
                DiscoverZip_64(pDiscoverFrame, frameSize, ipv6DiscoveryAddress, ipv4DiscoveryAddress, portNo, onZipDetected);
            }
            else
            {
                DiscoverZip_32(pDiscoverFrame, frameSize, ipv6DiscoveryAddress, ipv4DiscoveryAddress, portNo, onZipDetected);
            }
        }

        #endregion

        public ZipControllerDiscoverService(string ipv6Address, string ipv4Address, int port)
        {
            _port = port;
            _ipv4RemoteAddress = ipv4Address;
            _ipv6RemoteAddress = ipv6Address;
        }

        public string[] Discover()
        {
            _zipControllerIps.Clear();
            // Compose Node Info Cached Get request.
            var zipPacket = new COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET();
            zipPacket.zWaveCommand = new List<byte>((byte[])new COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V2.NODE_INFO_CACHED_GET());
            zipPacket.properties2.zWaveCmdIncluded = 1;
            zipPacket.properties2.secureOrigin = 1;
            zipPacket.seqNo = 1;
            var discoveryFrame = (byte[])zipPacket;
            var onZipDetected = new OnZipDetectedDelegate(OnZipDetected);
            DiscoverZip(discoveryFrame, (uint)discoveryFrame.Length, _ipv6RemoteAddress, _ipv4RemoteAddress, (ushort)_port, onZipDetected);
            return _zipControllerIps.Distinct().ToArray();
        }

        private void OnZipDetected(string pszAddress, IntPtr pResponse, uint responseSize)
        {
            if (responseSize < 2)
                return;
            var response = new byte[responseSize];
            Marshal.Copy(pResponse, response, 0, (int)responseSize);
            if (response[0] == COMMAND_CLASS_ZIP_V2.ID && response[1] == COMMAND_CLASS_ZIP_V2.VERSION)
            {
                var zipPacket = (COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET)response;
                if (zipPacket.zWaveCommand.Count > 1)
                {
                    var zCommand = zipPacket.zWaveCommand.ToArray();
                    if ((zCommand[0] == COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V2.ID &&
                        zCommand[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V2.NODE_INFO_CACHED_REPORT.ID) ||
                        (zCommand[0] == COMMAND_CLASS_APPLICATION_STATUS.ID &&
                        zCommand[1] == COMMAND_CLASS_APPLICATION_STATUS.APPLICATION_BUSY.ID))
                    {
                        if (!string.IsNullOrEmpty(pszAddress))
                            _zipControllerIps.Add(pszAddress);
                    }
                }
            }
        }
    }
}
