/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Runtime.InteropServices;
using Utils;

namespace ZWave.ZipApplication
{
    /// <summary>
    /// Implements mechanism for client connection via DTLS
    /// </summary>
    public class DtlsClient : IDtlsClient
    {
        private bool Is64Bit { get { return Environment.Is64BitProcess; } }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void ClientConnectedDelegate([MarshalAs(UnmanagedType.LPStr)] string pszDestAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portDestNo,
            [MarshalAs(UnmanagedType.LPStr)] string pszLocalAddress,
            [MarshalAs(UnmanagedType.U2)] ushort localPortNo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void ClientClosedDelegate([MarshalAs(UnmanagedType.LPStr)] string pszAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo);

        #region DllImports

        [DllImport("ziptrans32", EntryPoint = "DtlsClientConnect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private extern static UIntPtr DtlsClientConnect_32([In, MarshalAs(UnmanagedType.LPStr)] string psk,
            [In, MarshalAs(UnmanagedType.LPStr)] string pszAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientConnectedDelegate connectedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientClosedDelegate closedProc);

        [DllImport("ziptrans64", EntryPoint = "DtlsClientConnect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private extern static UIntPtr DtlsClientConnect_64([In, MarshalAs(UnmanagedType.LPStr)] string psk,
            [In, MarshalAs(UnmanagedType.LPStr)] string pszAddress,
            [MarshalAs(UnmanagedType.U2)] ushort portNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientConnectedDelegate connectedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientClosedDelegate closedProc);

        private UIntPtr DtlsClientConnect(string psk, string pszAddress, ushort portNo, ClientConnectedDelegate connectedProc, ClientClosedDelegate closedProc)
        {
            return Is64Bit ? DtlsClientConnect_64(psk, pszAddress, portNo, connectedProc, closedProc) :
                DtlsClientConnect_32(psk, pszAddress, portNo, connectedProc, closedProc);
        }

        [DllImport("ziptrans32", EntryPoint = "DtlsClientConnectFrom", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private extern static UIntPtr DtlsClientConnectFrom_32([In, MarshalAs(UnmanagedType.LPStr)] string psk,
            [In, MarshalAs(UnmanagedType.LPStr)] string localAddress,
            [MarshalAs(UnmanagedType.U2)] ushort localPortNo,
            [In, MarshalAs(UnmanagedType.LPStr)] string destAddress,
            [MarshalAs(UnmanagedType.U2)] ushort destPortNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientConnectedDelegate connectedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientClosedDelegate closedProc);

        [DllImport("ziptrans64", EntryPoint = "DtlsClientConnectFrom", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private extern static UIntPtr DtlsClientConnectFrom_64([In, MarshalAs(UnmanagedType.LPStr)] string psk,
            [In, MarshalAs(UnmanagedType.LPStr)] string localAddress,
            [MarshalAs(UnmanagedType.U2)] ushort localPortNo,
            [In, MarshalAs(UnmanagedType.LPStr)] string destAddress,
            [MarshalAs(UnmanagedType.U2)] ushort destPortNo,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientConnectedDelegate connectedProc,
            [MarshalAs(UnmanagedType.FunctionPtr)] ClientClosedDelegate closedProc);

        private UIntPtr DtlsClientConnectFrom(string psk, string localAddress, ushort localPortNo, string destAddress, ushort destPortNo, ClientConnectedDelegate connectedProc, ClientClosedDelegate closedProc)
        {
            return Is64Bit ? DtlsClientConnectFrom_64(psk, localAddress, localPortNo, destAddress, destPortNo, connectedProc, closedProc) :
                    DtlsClientConnectFrom_32(psk, localAddress, localPortNo, destAddress, destPortNo, connectedProc, closedProc);
        }

        [DllImport("ziptrans32", EntryPoint = "DtlsClientClose", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void DtlsClientClose_32([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id);

        [DllImport("ziptrans64", EntryPoint = "DtlsClientClose", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void DtlsClientClose_64([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id);

        private void DtlsClientClose(UIntPtr id)
        {
            if (Is64Bit)
            {
                try
                {
                    DtlsClientClose_64(id);
                }
                catch(Exception ex)
                {
                    $"{ex.Message}"._DLOG();
                    Console.WriteLine("DTLS Closing Threw and exception");
                }
            }
            else
            {
                try
                {
                    DtlsClientClose_32(id);
                }
                catch (Exception ex)
                {
                    $"{ex.Message}"._DLOG();
                    Console.WriteLine("DTLS Closing Threw and exception");
                }                
            }
        }

        [DllImport("ziptrans32", EntryPoint = "DtlsClientSend", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int DtlsClientSend_32([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen);

        [DllImport("ziptrans64", EntryPoint = "DtlsClientSend", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int DtlsClientSend_64([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen);

        private int DtlsClientSend(UIntPtr id, byte[] pData, uint dataLen)
        {
            return Is64Bit ? DtlsClientSend_64(id, pData, dataLen) :
                DtlsClientSend_32(id, pData, dataLen);
        }

        [DllImport("ziptrans32", EntryPoint = "DtlsClientReceive", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int DtlsClientReceive_32([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen);

        [DllImport("ziptrans64", EntryPoint = "DtlsClientReceive", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int DtlsClientReceive_64([In, MarshalAs(UnmanagedType.SysUInt)] UIntPtr id,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] pData,
            [MarshalAs(UnmanagedType.U4)] uint dataLen);

        private int DtlsClientReceive(UIntPtr id, byte[] pData, uint dataLen)
        {
            return Is64Bit ? DtlsClientReceive_64(id, pData, dataLen) :
                DtlsClientReceive_32(id, pData, dataLen);
        }

        #endregion

        #region IDtlsClient Members

        public event Action<IDtlsClient> Connected;
        public event Action<IDtlsClient> Closed;

        private ClientConnectedDelegate _clientConnectedDelegate;
        private ClientClosedDelegate _clientClosedDelegate;

        public Tuple<string, ushort>  LocalEndpoint { get; private set; }

        private UIntPtr _id;

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
        }

        public bool Connect(string psk, string address, ushort port)
        {
            return ConnectInternal(psk, null, 0, address, port);
        }

        public bool Connect(string psk, string localAddress, ushort localPort, string destAddress, ushort destPortNo)
        {
            return ConnectInternal(psk, localAddress, localPort, destAddress, destPortNo);
        }

        private bool ConnectInternal(string psk, string localAddress, ushort localPort, string destAddress, ushort destPortNo)
        {
            _clientConnectedDelegate = new ClientConnectedDelegate(ClientConnected);
            _clientClosedDelegate = new ClientClosedDelegate(ClientClosed);
            _id = (!string.IsNullOrEmpty(localAddress) && localPort > 0) ?
                DtlsClientConnectFrom(psk, localAddress, localPort, destAddress, destPortNo, _clientConnectedDelegate, _clientClosedDelegate) :
                DtlsClientConnect(psk, destAddress, destPortNo, _clientConnectedDelegate, _clientClosedDelegate);
            if (_id == UIntPtr.Zero)
            {
                _clientClosedDelegate = null;
                _clientConnectedDelegate = null;
            }
            return _id != UIntPtr.Zero;
        }

        public void Close()
        {
            if (_isConnected)
            {
                try
                {
                    DtlsClientClose(_id);
                }
                catch
                {
                    Console.WriteLine("DTLS pointer already closed");
                }
            }
        }

        public int Send(byte[] data)
        {
            return DtlsClientSend(_id, data, (uint)data.Length);
        }

        public int Receive(byte[] data)
        {
            return DtlsClientReceive(_id, data, (uint)data.Length);
        }

        #endregion

        private void ClientConnected(string destAddress, ushort destPortNo, string localAddress, ushort localPortNo)
        {
            _isConnected = true;
            LocalEndpoint = !string.IsNullOrEmpty(localAddress) && localPortNo > 0 ? new Tuple<string, ushort>(localAddress, localPortNo) : null;
            Connected?.Invoke(this);
        }

        private void ClientClosed(string address, ushort portNo)
        {
            _clientConnectedDelegate = null;
            _clientClosedDelegate = null;
            _isConnected = false;
            Closed?.Invoke(this);
        }
    }
}
