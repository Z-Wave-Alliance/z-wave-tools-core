/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;
using Utils.UI.Bind;

namespace ZWave.Xml.Application
{
    public partial class DefineSet
    {
        private CommandClass _parentField;
        [XmlIgnore]
        public CommandClass Parent
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

        public virtual DefineSet Clone(ISubscribeCollectionFactory iSubscribeCollectionFactory)
        {
            DefineSet ret = (DefineSet)MemberwiseClone();

            ret.Define = null;
            if (Define != null)
            {
                ret.Define = iSubscribeCollectionFactory.CreateCollection<Define>();
                foreach (var item in Define)
                {
                    Define c = item.Clone(iSubscribeCollectionFactory);
                    c.Parent = ret;
                    ret.Define.Add(c);
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
