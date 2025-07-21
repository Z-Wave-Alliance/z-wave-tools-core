/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Xml.Serialization;
using Utils;

namespace ZWave.Xml.Application
{
    public partial class SpecificDevice
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

        [XmlIgnore]
        public byte ScopeKeyId
        {
            get
            {
                return Tools.GetByte(ScopeKey);
            }
            set
            {
                ScopeKey = Tools.GetHex(value, true);
            }
        }

        private GenericDevice _parentField;
        [XmlIgnore]
        public GenericDevice Parent
        {
            get
            {
                return _parentField;
            }
            set
            {
                _parentField = value;
                RaisePropertyChanged("Parent");
            }
        }

        public virtual SpecificDevice Clone()
        {
            return (SpecificDevice)MemberwiseClone();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
