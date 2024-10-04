/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZWave.Layers
{
    public class UdpMeshRouterDiscoveryService : UdpDevicesDiscoveryService<MeshRouterDeviceInfo>
    {
        private IReadOnlyList<byte> _discoverFrame => Array.AsReadOnly(Encoding.ASCII.GetBytes("MCL_MULTI_CHAN_CONTROLLER?"));

        public override MeshRouterDeviceInfo[] DiscoverTcpDevices()
        {
            REQUEST_PORT_NO = 4950;
            RESPONSE_PORT_NO = 4951;
            UDP_RECEIVE_TIMEOUT = 300;
            var hostName = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(hostName);
            var localIps = ipEntry.AddressList.Where(address => address.AddressFamily == AddressFamily.InterNetwork).ToArray();

            var foundEndPoints = new List<MeshRouterDeviceInfo>();

            foreach (var address in localIps)
            {
                PerformUdpDiscovery(address, _discoverFrame.ToArray(), null, (bufferAnswer, endPoint) =>
                {
                    var response = Encoding.ASCII.GetString(bufferAnswer);
                    if (MeshRouterDeviceInfo.TryParse(response, out MeshRouterDeviceInfo info))
                    {
                        foundEndPoints.Add(info);
                        return true;
                    }
                    return false;
                });
            }
            return foundEndPoints.ToArray();
        }

        public async override Task DiscoverTcpDevicesAsync(IPAddress address)
        {
            throw new NotImplementedException();
        }
    }

    public class MeshRouterDeviceInfo : IUdpDiscoverInfo
    {
        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public string SerialNo { get; set; }
        public string ModelName { get; set; }
        public string SubnetMask { get; set; }
        public string NetworkGateway { get; set; }
        public string MACAddress { get; set; }

        public static bool TryParse(string deviceRawResponse, out MeshRouterDeviceInfo info)
        {
            info = new MeshRouterDeviceInfo();
            if (string.IsNullOrEmpty(deviceRawResponse))
                return false;
            var infoFields = new List<string>(deviceRawResponse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
            try
            {
                int sepInd = infoFields[2].IndexOf("Port:");
                var addressStr = infoFields[2].Substring(0, sepInd);
                var portsArray = infoFields[2].Substring(sepInd).Split();
                var portStr = $"{portsArray[0]} {portsArray[1]}";
                infoFields.RemoveAt(2);
                infoFields.Insert(2, addressStr);
                infoFields.Insert(3, portStr);
                info.ModelName = infoFields[0].Remove(0, "Model Name: ".Length).Trim();
                info.SerialNo = infoFields[1].Remove(0, "Serial Number: ".Length).Trim();
                info.IPAddress = IPAddress.Parse(infoFields[2].Remove(0, "IP Address=".Length).Trim());
                info.Port = int.Parse(infoFields[3].Remove(0, "Port: ".Length));
                info.SubnetMask = infoFields[4].Remove(0, "Subnet Mask=".Length).Trim();
                info.NetworkGateway = infoFields[5].Remove(0, "Network Gateway=".Length).Trim();
                info.MACAddress = infoFields[6].Remove(0, "Mac Address=".Length).Trim();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
