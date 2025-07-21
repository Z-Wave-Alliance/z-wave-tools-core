/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave
{
    public class RFrequency
    {
        public byte Channels { get; set; }
        public string Name { get; set; }
        public RFrequency(byte channels, string name)
        {
            Channels = channels;
            Name = name;
        }
    }
}
