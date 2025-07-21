/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace Utils.UI.Logging
{
    public class LogRawData
    {
        public ushort SourceId { get; set; }
        public byte SecuritySchemes { get; set; }
        public byte[] RawData { get; set; }
    }
}
