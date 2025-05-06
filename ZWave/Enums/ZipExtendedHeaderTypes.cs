/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    /// <summary>
    /// Transmit Statuses enumeration.
    /// </summary>
    public enum ZipExtendedHeaderTypes
    {
        ExpectedDelay = 0x01,
        InstallationAndMaintenanceGet = 0x02,
        InstallationAndMaintenanceReport = 0x03,
        EncapsulationFormatInformation = 0x04,
        ReceivedViaMulticast = 0x05,
        ExtendedZipPacketHeaderLength = 0x06,
        MulticastDestinationAddresses = 0x07,
        MulticastAcknowledgement = 0x08
    }
}
