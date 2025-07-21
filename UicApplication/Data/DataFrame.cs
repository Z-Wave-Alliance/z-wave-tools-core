/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace UicApplication.Data
{
    public class DataFrame : CustomDataFrame
    {
        private const int MAX_LENGTH = 4000;
        public DataFrame(DataFrameTypes type, DateTime timeStamp)
            : base(type, timeStamp)
        {
            ApiType = ApiTypes.Uic;
        }

        public string MqttTopic { get; set; }

        public string MqttPayload { get; set; }

        protected override int GetMaxLength()
        {
            throw new NotImplementedException();
        }

        protected override byte[] RefreshData()
        {
            throw new NotImplementedException();
        }

        protected override byte[] RefreshPayload()
        {
            throw new NotImplementedException();
        }
    }
}
