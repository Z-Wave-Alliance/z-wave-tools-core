/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ZWave.Layers
{
    public abstract class UdpDevicesDiscoveryService<TInfo> : IUdpDevicesDiscoveryService<TInfo> where TInfo : IUdpDiscoverInfo
    {
        protected int REQUEST_PORT_NO = 0;
        protected int RESPONSE_PORT_NO = 0;
        protected int UDP_RECEIVE_TIMEOUT = 1;

        public abstract TInfo[] DiscoverTcpDevices();
        public abstract Task DiscoverTcpDevicesAsync(IPAddress address);

        protected void PerformUdpDiscovery(IPAddress localIpAddress, byte[] discoverFrame, IPAddress targetAddress, Func<byte[], IPEndPoint, bool> answerValidationCallback)
        {
            if (localIpAddress == null && targetAddress == null)
                return;

            var udpClient = new UdpClient();
            try
            {
                udpClient.Client.ReceiveTimeout = UDP_RECEIVE_TIMEOUT;
                if (localIpAddress != null)
                {
                    var localEndPoint = new IPEndPoint(localIpAddress, RESPONSE_PORT_NO);
                    udpClient.Client.Bind(localEndPoint);
                }
                else
                {
                    udpClient.Send(discoverFrame.ToArray(), discoverFrame.Length, targetAddress.ToString(), REQUEST_PORT_NO);
                }

                var receiveTask = Task.Run(() =>
                {
                    do
                    {
                        try
                        {
                            var endPoint = new IPEndPoint(IPAddress.Any, RESPONSE_PORT_NO);
                            var bufferAnswer = udpClient.Receive(ref endPoint);
                            while (bufferAnswer != null && bufferAnswer.Length > 0)
                            {
                                answerValidationCallback.Invoke(bufferAnswer, endPoint);
                                try
                                {
                                    bufferAnswer = udpClient.Receive(ref endPoint);
                                }
                                catch (SocketException sex)
                                {
                                    sex.Message._DLOG();
                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            break; // No answer.
                        }
                    }
                    while (udpClient.Client.ReceiveTimeout != 0);
                });
                if (localIpAddress != null)
                {
                    udpClient.Send(discoverFrame.ToArray(), discoverFrame.Length, new IPEndPoint(IPAddress.Broadcast, REQUEST_PORT_NO));
                }
                receiveTask.Wait();
            }
            catch (Exception e)
            {
                $"Couldn't perform TCP clients discovery via UDP. Exception message: {e.Message}"._DLOG();
            }
            finally
            {
                udpClient.Close();
            }
        }

        protected void PerformUdpDiscoveryTask(IPAddress localIpAddress, byte[] discoverFrame, Func<byte[], IPEndPoint, bool> answerValidationCallback)
        {
            if (localIpAddress == null)
                return;
            using (UdpClient udpClient = new UdpClient())
            {
                try
                {
                    var localEndPoint = new IPEndPoint(localIpAddress, RESPONSE_PORT_NO);
                    udpClient.Client.Bind(localEndPoint);
                }
                catch (SocketException ex)
                {
                    return;
                }

                bool isClosing = false;
                var receiveTask = new Task(async () =>
                {
                    while (!isClosing)
                    {
                        try
                        {
                            if (udpClient.Client != null)
                            {
                                var rec = await udpClient.ReceiveAsync();
                                if (rec.Buffer != null && rec.Buffer.Length > 0)
                                {
                                    answerValidationCallback.Invoke(rec.Buffer, rec.RemoteEndPoint);
                                }
                            }
                        }
                        catch (NullReferenceException nex)
                        {
                            isClosing = true;
                        }
                        catch (ObjectDisposedException dex)
                        {
                            isClosing = true;
                        }
                    }
                });
                receiveTask.Start();
                udpClient.Send(discoverFrame.ToArray(), discoverFrame.Length, new IPEndPoint(IPAddress.Broadcast, REQUEST_PORT_NO));
                Task.Delay(500).Wait();
                udpClient.Send(discoverFrame.ToArray(), discoverFrame.Length, new IPEndPoint(IPAddress.Broadcast, REQUEST_PORT_NO));
                Task.Delay(500).Wait();
                udpClient.Send(discoverFrame.ToArray(), discoverFrame.Length, new IPEndPoint(IPAddress.Broadcast, REQUEST_PORT_NO));
                Task.Delay(500).Wait();
                lock (receiveTask)
                {
                    isClosing = true;
                }
                udpClient.Close();
                receiveTask.Wait();
            }
        }
    }
}
