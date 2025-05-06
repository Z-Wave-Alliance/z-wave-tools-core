/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.UicApplication.PayloadWrappers.Enums;

namespace ZWave.UicApplication.PayloadWrappers
{
    public class PairPayloadWrapper
    {
        public string DeviceDsk { get; set; }
        
        public object GetDeviceDskInMqttFormat()
        {
            return new
            {
                DSK = DeviceDsk,
                Include = true
            };
        }

    }
    public class PairStatusPayloadWrapper
    {
        public string Unid { get; set; }
        public PairStatus Status { get; set; } // enum? 
    }
}
