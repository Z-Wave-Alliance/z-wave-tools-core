/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
namespace ZWave.Enums
{
    [Flags]
    public enum SubstituteFlags : int
    {
        None = 0x0000,
        UseTransportService = 0x00000001,
        DenyTransportService = 0x00000002,
        UseSecurity = 0x00000004,
        DenySecurity = 0x00000008,
        UseSupervision = 0x00000010,
        DenySupervision = 0x00000020,
        UseCrc16Encap = 0x00000040,
        DenyCrc16Encap = 0x00000080,
        UseMulticast = 0x00000100,
        DenyMulticast = 0x00000200,
        UseBroadcast = 0x00000400,
        DenyBroadcast = 0x00000800,
        UseFollowup = 0x00001000,
        DenyFollowup = 0x00002000,
        UseMultiChannel = 0x00004000,
        DenyMultiChannel = 0x00008000,
        zReserved41 = 0x00010000,
        zReserved42 = 0x00020000,
        zReserved44 = 0x00040000,
        zReserved48 = 0x00080000,
    }

    [Flags]
    public enum SubstituteIncomingFlags : int
    {
        None = 0x0000,
        Operations = 0x00000001,
        TransportService = 0x00000002,
        Security = 0x00000004,
        Supervision = 0x00000008,
        Crc16Encap = 0x00000010,
        Multicast = 0x00000020,
        SecurityFailed = 0x00000040,
        MultiChannel = 0x00000080,
        Settings = 0x00000100,
    }
}
