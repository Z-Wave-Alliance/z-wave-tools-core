/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;
using Utils;

namespace ZWave.Xml.Application
{
    public partial class BasicDevice
    {
        [XmlIgnore]
        public byte KeyId
        {
            get
            {
                if (Key != null)
                    return Tools.GetByte(Key);
                return 1;
            }
            set
            {
                Key = Tools.GetHex(value, true);
                RaisePropertyChanged("KeyId");
            }
        }
        public virtual BasicDevice Clone()
        {
            return (BasicDevice)MemberwiseClone();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
