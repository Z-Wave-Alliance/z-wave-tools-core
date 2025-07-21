/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.Security
{
    public class ZWaveSecurityAuthData
    {
        /// <summary>
        /// Initial vector
        /// </summary>
        public byte[] IV;
        /// <summary>
        /// The security header is only the command #
        /// </summary>
        public byte SH;
        /// <summary>
        /// Source node ID
        /// </summary>
        public byte SenderNodeId;
        /// <summary>
        /// Destination node ID
        /// </summary>
        public byte ReceiverNodeId;
        /// <summary>
        /// Payload length
        /// </summary>
        public byte PayloadLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZWaveSecurityAuthData"/> class.
        /// </summary>
        public ZWaveSecurityAuthData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZWaveSecurityAuthData"/> class.
        /// </summary>
        /// <param name="pIV">The p IV.</param>
        /// <param name="pNonces">The p nonces.</param>
        /// <param name="pSH">The p SH.</param>
        /// <param name="pSenderNodeId">The p sender node id.</param>
        /// <param name="pReceiverNodeId">The p receiver node id.</param>
        /// <param name="pPayloadLength">Length of the p payload.</param>
        public ZWaveSecurityAuthData(byte[] pIV, byte[] pNonces, byte pSH, byte pSenderNodeId, byte pReceiverNodeId, byte pPayloadLength)
        {
            IV = new byte[16];
            for (int i = 0; i < 8; i++)
            {
                IV[i] = pIV[i];
            }
            for (int i = 8; i < 16; i++)
            {
                IV[i] = pNonces[i - 8];
            }
            SH = pSH;
            SenderNodeId = pSenderNodeId;
            ReceiverNodeId = pReceiverNodeId;
            PayloadLength = pPayloadLength;
        }

        /// <summary>
        /// Toes the byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            byte[] tempByteArray = new byte[20];

            int i = 0;

            for (int j = 0; j < IV.Length; j++)
            {
                tempByteArray[i++] = IV[j];
            }

            tempByteArray[i++] = SH;
            tempByteArray[i++] = SenderNodeId;
            tempByteArray[i++] = ReceiverNodeId;
            tempByteArray[i++] = PayloadLength;

            return tempByteArray;
        }
    }
}
