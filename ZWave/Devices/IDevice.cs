/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using ZWave.Layers;

namespace ZWave.Devices
{
    public interface IDevice : IApplicationClient
    {
        ushort Id { get; set; }
        byte[] HomeId { get; set; }
        ushort SucNodeId { get; set; }
        NetworkViewPoint Network { get; set; }
    }
}
