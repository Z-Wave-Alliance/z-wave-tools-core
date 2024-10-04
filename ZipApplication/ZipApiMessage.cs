/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.ZipApplication
{
    public class ZipApiMessage : CommandMessage
    {
        public byte[] HeaderExtension { get; set; }
        public ZipApiMessage(byte[] headerExtension, byte[] data)
        {
            HeaderExtension = headerExtension;
            AddData(data);
        }
        public ZipApiMessage(byte[] headerExtension, byte[] data, bool isNoAck)
        {
            HeaderExtension = headerExtension;
            AddData(data);
            IsNoAck = IsNoAck;
        }
    }
}
