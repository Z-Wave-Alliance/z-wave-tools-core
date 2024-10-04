/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers.Frame;
using ZWave.Enums;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication
{
    public class DataFrame : CustomDataFrame
    {
        private const int MAX_LENGTH = 255;
        public DataFrame(ushort sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp)
            : base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
            ApiType = ApiTypes.Programmer;
        }

        protected override int GetMaxLength()
        {
            return MAX_LENGTH;
        }

        protected override byte[] RefreshData()
        {
            byte[] ret = null;
            if (Buffer.Length > 0)
            {
                ret = new byte[Buffer.Length];
                Array.Copy(Buffer, 0, ret, 0, Buffer.Length);
            }
            return ret;
        }

        protected override byte[] RefreshPayload()
        {
            byte[] ret = null;
            if (Data.Length > 0)
            {
                ret = new byte[Data.Length];
                Array.Copy(Data, 0, ret, 0, Data.Length);
            }
            return ret;
        }

        public static byte[] CreateFrameBuffer(byte[] data)
        {
            byte[] ret = new byte[data.Length];
            Array.Copy(data, 0, ret, 0, data.Length);
            return ret;
        }
    }
}
