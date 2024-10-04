/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Runtime.InteropServices;

namespace ZWave.ZipApplication
{
    /// <summary>
    /// implements mechanism for listening incoming DTLS connections
    /// </summary>
    class DtlsListener : IDtlsListener
    {
        private bool Is64Bit { get { return Environment.Is64BitProcess; } }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void ListenerDataReceivedDelegate(IntPtr pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen,
            [MarshalAs(UnmanagedType.LPStr)] string addressPtr,
            [MarshalAs(UnmanagedType.U2)] ushort portNo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void ListenerClientConnectedDelegate([MarshalAs(UnmanagedType.LPStr)] string pszAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void ListenerClientClosedDelegate([MarshalAs(UnmanagedType.LPStr)] string pszAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo);

        #region DllImports

        [DllImport("ziptrans32", EntryPoint = "DtlsListenerStart", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private extern static UIntPtr DtlsListenerStart_32([In, MarshalAs(UnmanagedType.LPStr)] string psk,
            [In, MarshalAs(UnmanagedType.LPStr)] string address,
            [MarshalAs(UnmanagedType.U2)] ushort portNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] ListenerDataReceivedDelegate dataReceivedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ListenerClientConnectedDelegate connectedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ListenerClientClosedDelegate closedProc);

        [DllImport("ziptrans64", EntryPoint = "DtlsListenerStart", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private extern static UIntPtr DtlsListenerStart_64([In, MarshalAs(UnmanagedType.LPStr)] string psk,
            [In, MarshalAs(UnmanagedType.LPStr)] string address,
            [MarshalAs(UnmanagedType.U2)] ushort portNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] ListenerDataReceivedDelegate dataReceivedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ListenerClientConnectedDelegate connectedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ListenerClientClosedDelegate closedProc);

        private UIntPtr DtlsListenerStart(string psk, string address, ushort portNo, ListenerDataReceivedDelegate dataReceivedProc,
            ListenerClientConnectedDelegate connectedProc, ListenerClientClosedDelegate closedProc)
        {
            return Is64Bit ? DtlsListenerStart_64(psk, address, portNo, dataReceivedProc, connectedProc, closedProc) :
                DtlsListenerStart_32(psk, address, portNo, dataReceivedProc, connectedProc, closedProc);
        }

        [DllImport("ziptrans32", EntryPoint = "DtlsListenerStop", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void DtlsListenerStop_32([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id);

        [DllImport("ziptrans64", EntryPoint = "DtlsListenerStop", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void DtlsListenerStop_64([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id);

        private void DtlsListenerStop(UIntPtr id)
        {
            if (Is64Bit)
            {
                try
                {
                    DtlsListenerStop_64(id);
                }
                catch(SEHException)
                {
                }
            }
            else
            {
                DtlsListenerStop_32(id);
            }
        }
        
        [DllImport("ziptrans32", EntryPoint = "DtlsListenerResponseTo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int DtlsListenerResponseTo_32([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen,
            [In, MarshalAs(UnmanagedType.LPStr)] string address,
            [MarshalAs(UnmanagedType.U2)] ushort portNo);

        [DllImport("ziptrans64", EntryPoint = "DtlsListenerResponseTo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int DtlsListenerResponseTo_64([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen,
            [In, MarshalAs(UnmanagedType.LPStr)] string address,
            [MarshalAs(UnmanagedType.U2)] ushort portNo);

        private int DtlsListenerResponseTo(UIntPtr id, byte[] pData, uint dataLen, string address, ushort portNo)
        {
            return Is64Bit ? DtlsListenerResponseTo_64(id, pData, dataLen, address, portNo) :
                DtlsListenerResponseTo_32(id, pData, dataLen, address, portNo);
        }

        #endregion

        #region IDtlsListener Members

        public event Action<byte[], string, ushort> DataReceived;
        public event Action<string, ushort> ClientConnected;
        public event Action<string, ushort> ClientClosed;

        private ListenerDataReceivedDelegate _dataReceivedDelegate;
        private ListenerClientConnectedDelegate _clientConnectedDelegate;
        private ListenerClientClosedDelegate _clientClosedDelegate;

        private UIntPtr _id;

        private bool _listening;
        public bool Listening
        {
            get { return _listening; }
        }

        public int Start(string psk, string localAddress, ushort portNo)
        {
            if (_listening)
            {
                return -1;
            }
            _dataReceivedDelegate = new ListenerDataReceivedDelegate(OnDataReceived);
            _clientConnectedDelegate = new ListenerClientConnectedDelegate(OnClientConnected);
            _clientClosedDelegate = new ListenerClientClosedDelegate(OnClientClosed);
            _id = DtlsListenerStart(psk, localAddress, portNo, _dataReceivedDelegate, _clientConnectedDelegate, _clientClosedDelegate);
            _listening = _id != UIntPtr.Zero;
            if (!_listening)
            {
                _dataReceivedDelegate = null;
                _clientConnectedDelegate = null;
                _clientClosedDelegate = null;
            }
            return _listening ? 0 : -1;
        }

        public int ResponseTo(byte[] data, string address, ushort portNo)
        {
            return _listening ? DtlsListenerResponseTo(_id, data, (uint)data.Length, address, portNo) : -1;
        }

        public void Stop()
        {
            if (_listening)
            {
                _listening = false;
                DtlsListenerStop(_id);
                _dataReceivedDelegate = null;
                _clientConnectedDelegate = null;
                _clientClosedDelegate = null;
            }
        }

        #endregion

        private void OnDataReceived(IntPtr pData, uint dataLen, string addressPtr, ushort portNo)
        {
            byte[] data = new byte[dataLen];
            Marshal.Copy(pData, data, 0, (int)dataLen);
            DataReceived?.Invoke(data, addressPtr, portNo);
        }

        private void OnClientConnected(string pszAddress, ushort portNo)
        {
            ClientConnected?.Invoke(pszAddress, portNo);
        }

        private void OnClientClosed(string pszAddress, ushort portNo)
        {
            ClientClosed?.Invoke(pszAddress, portNo);
        }
    }
}
