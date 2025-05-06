/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;
using Utils;
using Utils.UI.Bind;

namespace ZWave.Xml.Application
{
    public partial class Define
    {
        private DefineSet _parentField;
        [XmlIgnore]
        public DefineSet Parent
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

        private Define _parentDefineField;
        [XmlIgnore]
        public Define ParentDefine
        {
            get
            {
                return _parentDefineField;
            }
            set
            {
                _parentDefineField = value;
                RaisePropertyChanged("ParentDefine");
            }
        }

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

        public virtual Define Clone(ISubscribeCollectionFactory iSubscribeCollectionFactory)
        {
            Define ret = (Define)MemberwiseClone();
            ret.Define1 = null;
            if (Define1 != null)
            {
                ret.Define1 = iSubscribeCollectionFactory.CreateCollection<Define>();
                foreach (var item in Define1)
                {
                    Define c = item.Clone(iSubscribeCollectionFactory);
                    c.ParentDefine = ret;
                    c.Parent = ret.Parent;
                    ret.Define1.Add(c);
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
