/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.UicApplication.PayloadWrappers.Enums;

namespace ZWave.UicApplication.PayloadWrappers
{
    public class RemovePayloadWrapper
    {
        public string Unid { get; set; }
    }
    public class RemoveStatusPayloadWrapper
    {
        public RemoveStatus Status { get; set; }
    }
}
