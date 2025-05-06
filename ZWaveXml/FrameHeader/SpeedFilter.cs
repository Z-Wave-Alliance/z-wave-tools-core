/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Xml.FrameHeader
{
    public class SpeedFilter
    {
        public string Text { get; set; }
        public byte[] Values { get; set; }
        public override bool Equals(object obj)
        {
            var speedFilter = obj as SpeedFilter;
            if (speedFilter != null)
            {
                bool ret = false;
                SpeedFilter sobj = speedFilter;
                if (Values == null && sobj.Values == null)
                {
                    ret = true;
                }
                else if (Values != null && sobj.Values != null && Values.Length == sobj.Values.Length)
                {
                    ret = true;
                    for (int i = 0; i < Values.Length; i++)
                    {
                        ret &= Values[i] == sobj.Values[i];
                        if (!ret)
                        {
                            break;
                        }
                    }
                }
                return ret;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
