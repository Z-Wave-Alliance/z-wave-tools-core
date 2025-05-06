/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWave.ZipApplication.Data
{
    public class DataFrame : CustomDataFrame
    {
        private const int MAX_LENGTH = 4000;
        public DataFrame(ushort sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp)
            : base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
            ApiType = ApiTypes.Zip;
        }

        protected override int GetMaxLength()
        {
            return MAX_LENGTH;
        }

        /// <summary>
        /// returns buffer without headerExt for ZIP_PACKET, returns buffer for others.
        /// </summary>
        /// <returns></returns>
        protected override byte[] RefreshData()
        {
            byte[] ret = Buffer;
            //if (Buffer.Length > 3)
            //{
            //    if (Buffer[0] == COMMAND_CLASS_ZIP_V2.ID && Buffer[1] == COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.ID)
            //    {
            //        COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET packet = Buffer;
            //        if (packet.zWaveCommand != null)
            //        {
            //            ret = new byte[7 + packet.zWaveCommand.Count];
            //            Array.Copy(Buffer, 0, ret, 0, 7);
            //            Array.Copy(packet.zWaveCommand.ToArray(), 0, ret, 7, ret.Length - 7);
            //        }
            //        else
            //        {
            //            ret = new byte[7];
            //            Array.Copy(Buffer, 0, ret, 0, 7);
            //        }
            //    }
            //    else
            //    {
            //        ret = Buffer;
            //    }
            //}
            return ret;
        }

        /// <summary>
        /// returns command with cmd_class and cmd keys
        /// </summary>
        /// <returns></returns>
        protected override byte[] RefreshPayload()
        {
            byte[] ret = null;
            if (Buffer.Length > 3)
            {
                if (Buffer[0] == COMMAND_CLASS_ZIP_V2.ID && Buffer[1] == COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.ID)
                {
                    COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET packet = Buffer;
                    if (packet.zWaveCommand != null)
                        ret = ((COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET)Buffer).zWaveCommand.ToArray();
                }
                else
                {
                    ret = Buffer;
                }
            }
            return ret;
        }
    }
}
