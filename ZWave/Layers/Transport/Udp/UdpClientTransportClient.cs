/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net.Sockets;
using Utils;
using System.Net;
using ZWave.Enums;
using System.IO;

namespace ZWave.Layers.Transport
{
    public class UdpClientTransportClient : TransportClientBase
    {
        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;

        private const int BUFFER_LENGTH = 1024 * 1024;

        public UdpClient _port;

        private Thread _threadWorker;
        private readonly object _lockObject = new object();
        private volatile bool _isStoped = true;
        private SocketDataSource _dataSource { get; set; }
        private IPEndPoint _remoteEndPoint;
        private readonly Action<DataChunk> _transmitCallback;
        public Func<byte[], int> DataWriteSubstitute;

        public override bool IsOpen
        {
            get { return _port.Client.Connected; }
        }

        public UdpClientTransportClient(Action<DataChunk> transmitCallback)
        {
            _transmitCallback = transmitCallback;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource ds)
        {
            InnerDisconnect();
            lock (_lockObject)
            {
                CommunicationStatuses ret = CommunicationStatuses.Busy;
                DataSource = ds;

                if (ds != null &&
                    ds is SocketDataSource &&
                    IPAddress.TryParse(ds.SourceName, out IPAddress ipAddress)
                    )
                {
                    _dataSource = (SocketDataSource)ds;
                    _remoteEndPoint = new IPEndPoint(IPAddress.Parse(ds.SourceName), _dataSource.Port);
                    if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        if (Socket.OSSupportsIPv6)
                        {
                            int _localPort = GetAvailablePort(4000);
                            //IPEndPoint DsEnpoint = 
                            //    new IPEndPoint(IPAddress.Parse("fd00:1111::10"), _localPort); //LAN IP address connected to the portal -- how to not hadcode this?

                            IPAddress RemoteIP = IPAddress.Parse(ds.SourceName);
                            IPEndPoint RemoteEndPoint = new IPEndPoint(RemoteIP, 4123);

                            Socket socket = null;
                            if (RemoteIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                socket = new Socket(
                                    AddressFamily.InterNetworkV6,
                                    SocketType.Dgram,
                                    ProtocolType.Udp);
                            }
                            else
                            {
                                socket = new Socket(
                                    AddressFamily.InterNetwork,
                                    SocketType.Dgram,
                                    ProtocolType.Udp);
                            }
                            IPEndPoint localEndPoint = QueryRoutingInterface(socket, RemoteEndPoint);

                            IPEndPoint DsEnpoint =
                                new IPEndPoint(localEndPoint.Address, _localPort); //LAN IP address connected to the portal -- how to not hadcode this?

                            try
                            {
                                _port = new UdpClient(AddressFamily.InterNetworkV6);
                                _port.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, true);
                                _port.ExclusiveAddressUse = true;
                                _port.Client.Bind(DsEnpoint);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                    else
                    {
                        _port = new UdpClient(AddressFamily.InterNetwork);
                    }
                    "{0:X2} {1} {2}@{3} {4}"._DLOG(SessionId, ApiType, _dataSource.SourceName, _dataSource.Port, _port.Client.Connected);
                    try
                    {
                        int attempts = 10;
                        while (!_port.Client.Connected && attempts > 0)
                        {
                            attempts--;
                            Thread.Sleep(100);
                            _port.Connect(_dataSource.SourceName, 4123); //harcoded again!!!!!!
                            "{0:X2} {1} {2}@{3} {4}"._DLOG(SessionId, ApiType, _dataSource.SourceName, _dataSource.Port, _port.Client.Connected);
                        }
                        if (_port.Client.Connected)
                        {
                            ret = CommunicationStatuses.Done;
                            _isStoped = false;
                            Connected?.Invoke(this);
                            _threadWorker = new Thread(DoWork);
                            _threadWorker.Name = "Udp Client";
                            _threadWorker.IsBackground = true;
                            _threadWorker.Start();
                        }
                    }
                    catch (IOException ex)
                    {
                        ex.Message._EXLOG();
                    }
                }
                return ret;
            }
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
            catch(Exception e)
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

        public static int GetAvailablePort(int startingPort)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (int i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }

        protected override void InnerDisconnect()
        {
            lock (_lockObject)
            {
                if (_isStoped)
                    return;

                _isStoped = true;
                if (_port != null)
                {
                    _port.Close();
                    _port = new UdpClient();
                }

                //if (_threadWorker.ThreadState != ThreadState.Unstarted && Thread.CurrentThread.ManagedThreadId != _threadWorker.ManagedThreadId)
                //{
                //    _threadWorker.Join();
                //}
                Disconnected?.Invoke(this);
                "{0:X2} {1} {2}@{3}"._DLOG(SessionId, ApiType, _dataSource.SourceName, _dataSource.Port);
            }
        }

        protected override int InnerWriteData(byte[] data)
        {
            if (DataWriteSubstitute != null)
            {
                return DataWriteSubstitute(data);
            }
            else
            {
                lock (_lockObject)
                {
                    if (_isStoped)
                        return -1;

                    if (!SuppressDebugOutput)
                    {
                        "{0} >> {1}"._DLOG(DataSource.SourceName, Tools.GetHex(data));
                    }
                    DataChunk dc = new DataChunk(data, SessionId, true, ApiType);
                    _transmitCallback?.Invoke(dc);
                    int ret = _port.Send(data, data.Length);
                    if (ret != data.Length)
                    {
                        "SEND FAILED"._DLOG();
                    }
                    return ret;
                }
            }
        }


        private void DoWork()
        {
            while (!_isStoped && _port.Client.Connected)
            {
                try
                {
                    byte[] data = _port.Receive(ref _remoteEndPoint);
                    if (!SuppressDebugOutput)
                        "{0} << {1}"._DLOG(DataSource.SourceName, Tools.GetHex(data));
                    DataChunk dc = new DataChunk(data, SessionId, false, ApiType);
                    _transmitCallback?.Invoke(dc);
                    ReceiveDataCallback?.Invoke(dc, false);
                }
                catch (SocketException)
                {
                    if (_port.Client != null)
                    {
                        if (!_port.Client.Connected)
                        {
                            InnerDisconnect();
                        }
                    }
                    else
                    {
                        InnerConnect(_dataSource);
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Socket was closed.
                }
                Thread.Sleep(100);
            }
        }

        protected override void InnerDispose()
        {
            InnerDisconnect();
        }
    }
}
