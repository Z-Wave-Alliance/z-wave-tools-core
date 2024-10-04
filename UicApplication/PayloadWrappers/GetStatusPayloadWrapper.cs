/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.UicApplication.PayloadWrappers
{
    public class GetStatusPayloadWrapper
    {
        public string Unid { get; set; }

    }
    public class GetStatusReportPayloadWrapper
    {
        public string Unid { get; set; }
        public string Status { get; set; }
    }
}
