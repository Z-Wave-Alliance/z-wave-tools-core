/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using ZWave.Devices;

namespace ZWave.Security
{
    // Additional Authenticated Data.
    public class AAD
    {
        public NodeTag SenderNodeId { get; set; }
        public NodeTag ReceiverNodeId { get; set; }
        public byte[] HomeId { get; set; }
        public ushort PayloadLength { get; set; }
        public byte SequenceNumber { get; set; }
        public byte StatusByte { get; set; }
        public byte[] ExtensionData { get; set; }

        public static implicit operator byte[] (AAD aad)
        {
            var data = new List<byte>();
            if (aad.SenderNodeId.Id > 0xFF || aad.ReceiverNodeId.Id > 0xFF)
            {
                data.Add((byte)(aad.SenderNodeId.Id >> 8));
            }
            data.Add((byte)aad.SenderNodeId.Id);
            if (aad.SenderNodeId.Id > 0xFF || aad.ReceiverNodeId.Id > 0xFF)
            {
                data.Add((byte)(aad.ReceiverNodeId.Id >> 8));
            }
            data.Add((byte)aad.ReceiverNodeId.Id);
            data.AddRange(aad.HomeId);
            data.Add((byte)(aad.PayloadLength >> 8));
            data.Add((byte)aad.PayloadLength);
            data.Add(aad.SequenceNumber);
            data.Add(aad.StatusByte);
            if (aad.ExtensionData != null)
            {
                data.AddRange(aad.ExtensionData);
            }
            return data.ToArray();
        }
    }
}
