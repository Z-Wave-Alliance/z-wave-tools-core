/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class BASE_TOPIC
    {
        public const string uicTopic = "/ucl";

        public const string byUnId = "/by-unid";

        public string UnId;
    }

    public struct mqttMessage
    {
        public string topic { get; set; }
        public string payload { get; set; }
    }
}
