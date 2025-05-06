/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;
using Utils.UI.Bind;

namespace ZWave.Xml.Application
{
    public partial class Param
    {
        private ParamModes _modeField;

        [XmlIgnore]
        public ParamModes Mode
        {
            get
            {
                return _modeField;
            }
            set
            {
                _modeField = value;
                RaisePropertyChanged("Mode");
            }
        }

        private Command _parentCmdField;

        [XmlIgnore]
        public Command ParentCmd
        {
            get
            {
                return _parentCmdField;
            }
            set
            {
                _parentCmdField = value;
                RaisePropertyChanged("ParentCmd");
            }
        }
        private Param _parentParamField;
        
        [XmlIgnore]
        public Param ParentParam
        {
            get
            {
                return _parentParamField;
            }
            set
            {
                _parentParamField = value;
                RaisePropertyChanged("ParentParam");
            }
        }

        public virtual Param Clone(ISubscribeCollectionFactory iSubscribeCollectionFactory)
        {
            Param ret = (Param)MemberwiseClone();

            ret.Param1 = null;
            if (Param1 != null && Param1.Count > 0)
            {
                ret.Param1 = iSubscribeCollectionFactory.CreateCollection<Param>();
                foreach (var item in Param1)
                {
                    Param c = item.Clone(iSubscribeCollectionFactory);
                    c.ParentCmd = null;
                    c.ParentParam = ret;
                    ret.Param1.Add(item.Clone(iSubscribeCollectionFactory));
                }
            }
            return ret;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public enum ParamModes
    { 
        Param,
        Property,
        VariantGroup
    }
}
