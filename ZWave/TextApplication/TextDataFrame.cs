/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Text;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWave.TextApplication
{
    public class TextDataFrame : CustomDataFrame
    {
        private const int MAX_LENGTH = 4000;

        public TextDataFrame(ushort sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp)
            : base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
            ApiType = ApiTypes.Text;
        }

        protected override int GetMaxLength()
        {
            return MAX_LENGTH;
        }

        /// <summary>
        /// Returns buffer
        /// </summary>
        /// <returns></returns>
        protected override byte[] RefreshData()
        {
            return Buffer;
        }

        /// <summary>
        /// Returns payload
        /// </summary>
        /// <returns></returns>
        protected override byte[] RefreshPayload()
        {
            return Buffer;
        }

        public override String ToString()
        {
            var ret = Encoding.ASCII.GetString(Buffer);
            return ret;
        }
    }
}
