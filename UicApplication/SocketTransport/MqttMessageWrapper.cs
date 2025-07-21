/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.UicApplication
{
    public class MqttMessageWrapper
    {
        public string Topic { get; set; }
        public string Payload { get; set; }
        public bool IsPublish { get; set; }
    }
}
