/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.ZipApplication
{
    /// <summary>
    /// Provides mechanism for listening incoming DTLS connections
    /// </summary>
    public interface IDtlsListener
    {
        /// <summary>
        /// Occurs when listener receives data from client connection
        /// </summary>
        event Action<byte[], string, ushort> DataReceived;
        /// <summary>
        /// Occurs when new client has been connected
        /// </summary>
        event Action<string, ushort> ClientConnected;
        /// <summary>
        /// Occurs when client connection has been closed
        /// </summary>
        event Action<string, ushort> ClientClosed;

        /// <summary>
        /// Gets a value indicating whether listening is enabled
        /// </summary>
        bool Listening { get; }

        /// <summary>
        /// Starts listening to specified endpoint
        /// </summary>
        /// <param name="psk">Psk key</param>
        /// <param name="localAddress">Local IP address to start listening to</param>
        /// <param name="portNo">Listening port number</param>
        /// <returns>Returns 0 if succeeded or error code otherwise</returns>
        int Start(string psk, string localAddress, ushort portNo);
        /// <summary>
        /// Responses to client specified by endpoint
        /// </summary>
        /// <param name="data">Data to be responded</param>
        /// <param name="address">IP address to response to</param>
        /// <param name="portNo">Destinated port number</param>
        /// <returns>Returns number of bytes successfuly sent or -1 otherwise</returns>
		int ResponseTo(byte[] data, string address, ushort portNo);
        /// <summary>
        /// Stops listening
        /// </summary>
        void Stop();
    }
}
