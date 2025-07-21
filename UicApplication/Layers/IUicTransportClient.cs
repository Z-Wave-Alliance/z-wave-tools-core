/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.UicApplication.Layers
{
    public interface IUicTransportClient
    {
        int PublishMessage(string topic, string payload, bool retain=false);
        int SubscribeTopic(string topic);
    }
}
