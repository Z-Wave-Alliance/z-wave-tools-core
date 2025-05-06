/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.UicApplication.PayloadWrappers
{
    public class NetworkInfoStatusPayloadWrapper
    {
        public NetworkInfoStatusItem[] NetworkInfoItems { get; set; }
    }

    public class NetworkInfoStatusItem
    {
        public string Unid { get; set; }
        public string Status { get; set; }
    }
}
