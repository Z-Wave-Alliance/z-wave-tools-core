/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.ZipApplication
{
    public interface IUnsolicitedClientsFactory
    {
        IUnsolicitedClient CreatePrimatyClient(string address, ushort portNo);
        IUnsolicitedReceiveClient CreateSecondaryClient(string address, ushort portNo);
    }
}
