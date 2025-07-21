/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Net;
using System.Threading.Tasks;

namespace ZWave.Layers
{
    public interface IUdpDevicesDiscoveryService<TInfo> where TInfo : IUdpDiscoverInfo
    {
        TInfo[] DiscoverTcpDevices();
        Task DiscoverTcpDevicesAsync(IPAddress address);
    }

    public interface IUdpDiscoverInfo
    {
        IPAddress IPAddress { get; set; }
        string SerialNo { get; set; }
    }
}