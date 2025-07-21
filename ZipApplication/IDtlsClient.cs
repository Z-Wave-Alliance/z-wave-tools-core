/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.ZipApplication
{
    /// <summary>
    /// Provides mechanism for client connection via DTLS
    /// </summary>
    public interface IDtlsClient
    {
        /// <summary>
        /// Occurs when client successfuly connected to specified endpoint
        /// </summary>
        event Action<IDtlsClient> Connected;
        /// <summary>
        /// Occurs when client connection has been closed
        /// </summary>
        event Action<IDtlsClient> Closed;

        /// <summary>
        /// Returns a value indicating whether client is connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// If client is connected returns local endpoint it's bound to
        /// </summary>
        Tuple<string, ushort> LocalEndpoint { get; }

        /// <summary>
        /// Connects to specified endpoint
        /// </summary>
        /// <param name="psk">Psk key</param>
        /// <param name="address">IP address to connect to</param>
        /// <param name="port">Destinated port number</param>
        /// <returns>Returns true if succeeded or false otherwise </returns>
        bool Connect(string psk, string address, ushort port);
        /// <summary>
        /// Connects to specified endpoint
        /// </summary>
        /// <param name="psk">Psk key</param>
        /// <param name="address">Local IP address from wich to connect</param>
        /// <param name="port">Local port number</param>
        /// <param name="address">IP address to connect to</param>
        /// <param name="port">Destinated port number</param>
        /// <returns>Returns true if succeeded or false otherwise </returns>
        bool Connect(string psk, string localAddress, ushort localPort, string destAddress, ushort destPortNo);
        /// <summary>
        /// Closes client connection
        /// </summary>
        void Close();
        /// <summary>
        /// Sends data to established connection
        /// </summary>
        /// <param name="data">Data to be sent</param>
        /// <returns>Returns number of bytes successfuly sent or -1 otherwise</returns>
        int Send(byte[] data);
        /// <summary>
        /// Receives data from established connection. Methos is synchronous i.e. blocks current thread.
        /// </summary>
        /// <param name="data">Data buffer to receive data into</param>
        /// <returns>Returns number of bytes successfuly received or -1 otherwise</returns>
        int Receive(byte[] data);
    }
}
