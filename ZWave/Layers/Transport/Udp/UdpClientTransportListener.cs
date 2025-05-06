/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Utils;

namespace ZWave.Layers.Transport
{
    public class StartListenParams : IStartListenParams
    {
        public string IpAddress { get; set; }
        public ushort PortNo { get; set; }

        public string InterfaceName { get; set; }
    }

    internal class BeginReceiveState
    {
        public Socket ListenSocket { get; set; }
        public byte[] Data { get; set; }
        public EndPoint RemotePeer;
    }

    public class UdpClientTransportListener : ISocketListener
    {
        private const int IPV6_V6ONLY = 27;
        private const int MAX_DATA_BUFFER_LENGTH = 10000;

        private Socket _socket;
        private readonly object _socketAccessLock = new object();

        public event Action<ReceivedDataArgs> DataReceived;

        public event Action<string, ushort> ConnectionCreated;
        public event Action<string, ushort> ConnectionClosed;

        private HashSet<EndPoint> _clientsTable = new HashSet<EndPoint>();

        public bool SuppressDebugOutput { get; set; }
        public IStartListenParams ListenParams { get; private set; }

        private volatile bool _isListening;
        public bool IsListening { get { return _isListening; } }

        public bool Listen(IStartListenParams listenParams)
        {
            if (!_isListening)
            {
                try
                {
                    lock (_socketAccessLock)
                    {
                        IPAddress listenAddress = null;
                        if (string.IsNullOrEmpty(listenParams.IpAddress))
                            listenAddress = IPAddress.IPv6Any;
                        else if (!IPAddress.TryParse(listenParams.IpAddress, out listenAddress))
                            return false;
                        _socket = new Socket(listenAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); // Enable using the same address by different clients.
                        if (listenAddress.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            int ipv6Only = 0; // Disable treat wildcard bind as AF_INET6-only.
#if NETCOREAPP
                            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX)) 
                                ipv6Only = 1; // OSX doesn't support dual mode.
#endif
                            _socket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)IPV6_V6ONLY, ipv6Only);
                        }
                        IPEndPoint localEndPoint = QueryRoutingInterface(_socket, 
                            new IPEndPoint(listenAddress, listenParams.PortNo));
                        localEndPoint.Port = listenParams.PortNo;
                        _socket.Bind(localEndPoint);
                        "Start listening @{0}"._DLOG(listenParams.PortNo);
                        _isListening = true;
                        ListenParams = listenParams;
                        var state = new BeginReceiveState
                        {
                            ListenSocket = _socket,
                            Data = new byte[MAX_DATA_BUFFER_LENGTH],
                            RemotePeer = new IPEndPoint(((IPEndPoint)_socket.LocalEndPoint).Address, IPEndPoint.MinPort)
                        };
                        var bufferSize = _socket.BeginReceiveFrom(state.Data, 0, state.Data.Length,
                            SocketFlags.None,
                            ref state.RemotePeer,
                            ReceiveCallback,
                            state);
                    }
                }
                catch (SocketException ex)
                {
                    ex.Message._DLOG();
                    CloseSocket();
                }
                catch (System.Security.SecurityException ex)
                {
                    ex.Message._DLOG();
                    CloseSocket();
                }
            }
            return IsListening;
        }

        public int ResponseTo(byte[] data, string address, ushort portNo)
        {
            if (_isListening)
            {
                if (IPAddress.TryParse(address, out IPAddress ipAddress))
                    return _socket.SendTo(data, new IPEndPoint(ipAddress, portNo));
            }
            return -1;
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var state = asyncResult.AsyncState as BeginReceiveState;
            if (state == null)
                return;

            int bytesRecieved = 0;
            if (state.ListenSocket != null)
            {
                lock (_socketAccessLock)
                {
                    if (state.ListenSocket != null)
                    {
                        try
                        {
                            bytesRecieved = state.ListenSocket.EndReceiveFrom(asyncResult, ref state.RemotePeer);
                            var bufferSize = state.ListenSocket.BeginReceiveFrom(state.Data, 0, state.Data.Length,
                                SocketFlags.None,
                                ref state.RemotePeer,
                                ReceiveCallback,
                                state);
                        }
                        catch (ObjectDisposedException) // Socket has been closed.
                        {
                            if (_clientsTable.Contains(state.RemotePeer))
                            {
                                _clientsTable.Remove(state.RemotePeer);
                                var remoteRawAddress = ((IPEndPoint)state.RemotePeer).Address;
                                var ipAddress = remoteRawAddress.IsIPv4MappedToIPv6 ? remoteRawAddress.MapToIPv4() : remoteRawAddress;
                                ConnectionClosed?.Invoke(ipAddress.ToString(), (ushort)((IPEndPoint)state.RemotePeer).Port);
                            }
                            return;
                        }
                        catch (SocketException ex) // An error occurred when attempting to access the socket.
                        {
                            ex.Message._DLOG();
                            CloseSocket();
                            return;
                        }
                        catch (InvalidCastException ex)
                        {
                            ex.Message._DLOG();
                        }
                    }
                }
            }

            if (bytesRecieved > 0)
            {
                var remoteRawAddress = ((IPEndPoint)state.RemotePeer).Address;
                var portNo = (ushort)((IPEndPoint)state.RemotePeer).Port;
                var ipAddress = remoteRawAddress.IsIPv4MappedToIPv6 ? remoteRawAddress.MapToIPv4() : remoteRawAddress;
                if (!_clientsTable.Contains(state.RemotePeer))
                {
                    _clientsTable.Add(state.RemotePeer);
                    ConnectionCreated?.Invoke(ipAddress.ToString(), (ushort)((IPEndPoint)state.RemotePeer).Port);
                }

                // Compose callback data.
                var receivedData = new ReceivedDataArgs { SourceName = ipAddress.ToString(), SourcePort = portNo };
                receivedData.Data = new byte[bytesRecieved];
                Array.Copy(state.Data, receivedData.Data, bytesRecieved);
                DataReceived?.Invoke(receivedData);
                if (!SuppressDebugOutput)
                {
                    "Listener accepted {0} << {1}"._DLOG(receivedData.SourceName, Tools.GetHex(receivedData.Data));
                }
            }
        }

        private void CloseSocket()
        {
            if (_socket != null)
            {
                lock (_socketAccessLock)
                {
                    if (_socket != null)
                    {
                        _socket.Close();
                        _socket = null;
                        "Stop listening @{0}"._DLOG(ListenParams?.PortNo);
                    }
                }
            }
        }

        public void Stop()
        {
            if (!_isListening)
                return;

            _isListening = false;
            CloseSocket();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        private static IPEndPoint QueryRoutingInterface(
          Socket socket,
          IPEndPoint remoteEndPoint)
        {
            SocketAddress address = remoteEndPoint.Serialize();

            byte[] remoteAddrBytes = new byte[address.Size];
            for (int i = 0; i < address.Size; i++)
            {
                remoteAddrBytes[i] = address[i];
            }

            byte[] outBytes = new byte[remoteAddrBytes.Length];
            try
            {
                socket.IOControl(
                            IOControlCode.RoutingInterfaceQuery,
                            remoteAddrBytes,
                            outBytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            for (int i = 0; i < address.Size; i++)
            {
                address[i] = outBytes[i];
            }

            EndPoint ep = remoteEndPoint.Create(address);
            return (IPEndPoint)ep;
        }

        #endregion
    }
}
