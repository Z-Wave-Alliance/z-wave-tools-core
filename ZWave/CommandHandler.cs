/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using Utils;
using ZWave.Layers.Frame;

namespace ZWave
{
    public class CommandHandler : ActionHandler
    {
        public BoolFlag Substituted { get; set; }
        public CustomDataFrame DataFrame { get; set; }

        protected ByteIndex[] mMask;
        public ByteIndex[] Mask
        {
            get { return mMask; }
            set { mMask = value; }
        }

        public override bool WaitingFor(IActionCase actionCase)
        {
            bool ret = false;
            if (actionCase is CustomDataFrame)
            {
                CustomDataFrame receivedValue = (CustomDataFrame)actionCase;
                if (Substituted == BoolFlag.NotSpecified ||
                    Substituted == BoolFlag.True && receivedValue.IsSubstituted ||
                    Substituted == BoolFlag.False && !receivedValue.IsSubstituted)
                {
                    byte[] payload = receivedValue.Data;
                    ret = IsExpectedData(payload);
                    if (ret)
                    {
                        DataFrame = receivedValue;
                    }
                }
            }
            //"{0}: {1} - {2}"._DLOG(ret, actionCase.ToString(), mMask.GetHex());
            return ret;
        }

        protected virtual bool IsExpectedData(IList<byte> data)
        {
            bool ret = false;
            for (int i = 0; i < mMask.Length; i++)
            {
                if (i < data.Count)
                {
                    if (mMask[i].Presence != Presence.AnyValue)
                    {
                        if (mMask[i].MaskInData != 0xFF)
                        {
                            ret = (data[i] & mMask[i].MaskInData) == (mMask[i].Value & mMask[i].MaskInData);
                        }
                        else
                        {
                            ret = data[i] == mMask[i].Value;
                        }
                        if (mMask[i].Presence == Presence.Value && !ret ||
                            mMask[i].Presence == Presence.ExceptValue && ret)
                        {
                            break;
                        }
                    }
                    else
                    {
                        ret = true;
                    }
                }
                else
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
        public void AddConditions(params ByteIndex[] conditions)
        {
            if (mMask != null)
            {
                ByteIndex[] originMask = mMask;
                mMask = new ByteIndex[originMask.Length + conditions.Length];
                Array.Copy(originMask, 0, mMask, 0, originMask.Length);
                Array.Copy(conditions, 0, mMask, originMask.Length, conditions.Length);
            }
            else
            {
                mMask = new ByteIndex[conditions.Length];
                Array.Copy(conditions, 0, mMask, 0, conditions.Length);
            }
        }
    }
}
