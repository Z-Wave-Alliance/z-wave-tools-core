/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;

namespace ZWave.Xml.FrameHeader
{
    public partial class ItemReference
    {
        [XmlIgnore]
        public DataIndex IndexOfOpt { get; set; }
        [XmlIgnore]
        public DataIndex Index { get; set; }

        public ItemReference()
        {
        }

        public ItemReference(string reference)
        {
            Ref = reference;
        }
    }
}
