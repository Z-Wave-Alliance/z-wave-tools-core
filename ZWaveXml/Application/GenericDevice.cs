/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;
using Utils;
using Utils.UI.Bind;

namespace ZWave.Xml.Application
{
    public partial class GenericDevice
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
        public virtual GenericDevice Clone(ISubscribeCollectionFactory iSubscribeCollectionFactory)
        {
            GenericDevice ret = (GenericDevice)MemberwiseClone();

            ret.SpecificDevice = null;
            if (SpecificDevice != null)
            {
                ret.SpecificDevice = iSubscribeCollectionFactory.CreateCollection<SpecificDevice>();
                foreach (var item in SpecificDevice)
                {
                    SpecificDevice c = item.Clone();
                    c.Parent = ret;
                    ret.SpecificDevice.Add(c);
                }
            }
            return ret;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
