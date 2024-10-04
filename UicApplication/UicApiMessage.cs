/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave;

namespace UicApplication
{
    public class UicApiMessage : CommandMessage
    {
        public UicApiMessage(string unid, string cluster, string payload, bool isPublish, bool isRetain)
        {
            Topic = unid + cluster;
            IsPublish = isPublish;
            IsRetain = isRetain;
            if (payload != null && !IsRetain)
            {
                Payload = payload;
            }
            else if (payload == null && IsRetain)
            {
                payload = "";
            }
            
        }

        public UicApiMessage(string unid, string cluster, string payload, bool isPublish)
        {
            Topic = unid + cluster;
            IsPublish = isPublish;
            if (payload != null)
            {
                Payload = payload;
            }
        }

        public UicApiMessage(string unid, string cluster, string payload) : this(unid, cluster, payload, true)
        {
        }

        public UicApiMessage(string unid, string cluster) : this(unid, cluster, null, false)
        {
        }

        public bool IsPublish { get; set; }

        public string Topic { get; set; }

        public string Payload { get; set; }

        public bool IsRetain { get; set; }

    }
}
