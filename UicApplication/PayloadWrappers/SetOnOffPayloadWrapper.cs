/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.UicApplication.PayloadWrappers.Enums;

namespace ZWave.UicApplication.PayloadWrappers
{
    public class SetOnOffPayloadWrapper
    {
        public string Unid { get; set; }
        public OnOffStatuses Status { get; set; }
        public string Ep { get; set; }

    }
}
