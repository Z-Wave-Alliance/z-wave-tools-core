/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Net;
using ZWave.Layers;
using Utils;
using System.Net.Sockets;

namespace ZWave.ZipApplication
{
    public class Dtls1ClientTransportListener : ISocketListener
    {
        private bool _disposed = false;
        private readonly object _listenerLock = new object();

        public event Action<ReceivedDataArgs> DataReceived;
        public event Action<string, ushort> ConnectionCreated;
        public event Action<string, ushort> ConnectionClosed;

        public bool SuppressDebugOutput { get; set; }
        private IDtlsListener _listener;
        public IDtlsListener Listener
        {
            get { return _listener ?? (_listener = new DtlsListener()); }
            set { _listener = value; }
        }

        public bool IsListening { get { return Listener.Listening; } }

        public IStartListenParams ListenParams { get; private set; }

        public bool Listen(IStartListenParams listenParams)
        {
            lock (_listenerLock)
            {
                if (!Listener.Listening)
                {
                    var dtlsListenParams = listenParams as IDtlsStartListenParams;
                    if (dtlsListenParams == null)
                    {
                        throw new ArgumentException("Invalid listenParams.");
                    }

                    if (dtlsListenParams.PortNo == 0)
                    {
                        throw new ArgumentException("Zero port number.");
                    }

                    if (string.IsNullOrEmpty(dtlsListenParams.PskKey))
                    {
                        throw new ArgumentException("PSK key can't be null or empty.");
                    }
                    
                    Listener.ClientConnected += Listener_ConnectionCreated;
                    Listener.ClientClosed += Listener_ConnectionClosed;
                    Listener.DataReceived += Listener_DataReceived;
#if !NETCOREAPP
                    IPAddress listenIpAddress;
                    if (string.IsNullOrEmpty(dtlsListenParams.IpAddress))
                        listenIpAddress = IPAddress.IPv6Any;
                    else if (!IPAddress.TryParse(dtlsListenParams.IpAddress, out listenIpAddress))
                        return false;

                    IPEndPoint localEndPoint = QueryRoutingInterface(new Socket(listenIpAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp), 
                        new IPEndPoint(listenIpAddress, dtlsListenParams.PortNo));

                    if (!localEndPoint.Address.Equals(listenIpAddress))
                        dtlsListenParams.IpAddress = localEndPoint.Address.ToString();
#endif

                    var retCode = Listener.Start(dtlsListenParams.PskKey, dtlsListenParams.IpAddress.ToString(), dtlsListenParams.PortNo);
                    if (retCode != 0)
                    {
                        "DTLSv1 Listening failed @ {0} : {1} Error code: {2}"._DLOG(dtlsListenParams.IpAddress, dtlsListenParams.PortNo, retCode.ToString());
                        Listener.ClientConnected -= Listener_ConnectionCreated;
                        Listener.ClientClosed -= Listener_ConnectionClosed;
                        Listener.DataReceived -= Listener_DataReceived;
                    }
                    else
                    {
                        "DTLSv1 Listening started @ {0} : {1}"._DLOG(dtlsListenParams.IpAddress, dtlsListenParams.PortNo);
                        ListenParams = dtlsListenParams;
                        _disposed = false;
                    }
                }
            }
            return IsListening;
        }

        void Listener_ConnectionCreated(string address, ushort portNo)
        {
            "DTLSv1 Listener accepted new connection @ {0} : {1}"._DLOG(address, portNo);
            ConnectionCreated?.Invoke(address, portNo);
        }

        void Listener_ConnectionClosed(string address, ushort portNo)
        {
            "DTLSv1 Listener connection closed @ {0} : {1}"._DLOG(address, portNo);
            ConnectionClosed?.Invoke(address, portNo);
        }

        public void Stop()
        {
            lock (_listenerLock)
            {
                if (Listener.Listening)
                {
                    Listener.ClientConnected -= Listener_ConnectionCreated;
                    Listener.ClientClosed -= Listener_ConnectionClosed;
                    Listener.DataReceived -= Listener_DataReceived;
                    Listener.Stop();
                    "DTLSv1 Listening stoped @{0}"._DLOG(ListenParams?.PortNo);
                }
            }
        }

        private void Listener_DataReceived(byte[] bytes, string sourceName, ushort sourcePortNo)
        {
            if (!SuppressDebugOutput)
            {
                "Listener accepted {0} >> {1}"._DLOG(sourceName, Tools.GetHex(bytes));
            }
            DataReceived?.Invoke(new ReceivedDataArgs { Data = bytes, SourceName = sourceName, SourcePort = sourcePortNo, ListenerPort = ListenParams.PortNo });
        }

        public int ResponseTo(byte[] data, string address, ushort portNo)
        {
            if (!SuppressDebugOutput)
            {
                "{0}:{1} >> {2}"._DLOG(address, portNo.ToString(), Tools.GetHex(data));
            }
            return Listener.ResponseTo(data, address, portNo);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Protect from being called multiple times.
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Clean up all managed resources.
            }

            // Clean up all unmanaged resources.
            if (Listener != null)
            {
                Stop();
            }

            _disposed = true;
        }

        private static IPEndPoint QueryRoutingInterface(Socket socket, IPEndPoint remoteEndPoint)
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

        ~Dtls1ClientTransportListener()
        {
            Dispose(false);
        }
    }

    public class Dtls1StartListenParams : IDtlsStartListenParams
    {
        public string IpAddress { get; set; }
        public ushort PortNo { get; set; }
        public string PskKey { get; set; }
        public string InterfaceName { get; set; }
    }
}
