/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Xml.Serialization;
using Utils;
using Utils.UI.Bind;

namespace ZWave.Xml.Application
{
    public partial class Command
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

        private CommandClass parentField;
        [XmlIgnore]
        public CommandClass Parent
        {
            get
            {
                return parentField;
            }
            set
            {
                parentField = value;
                RaisePropertyChanged("Parent");
            }
        }



        public byte[] FillPayload(params byte[] values)
        {
            if (values != null)
            {
                byte[] ret = new byte[values.Length + 2];
                ret[0] = Parent.KeyId;
                ret[1] = KeyId;
                for (int i = 0; i < values.Length; i++)
                {
                    ret[i + 2] = values[i];
                }
                return ret;
            }
            return new[] { Parent.KeyId, KeyId };
        }
        public virtual Command Clone(ISubscribeCollectionFactory iSubscribeCollectionFactory)
        {
            Command ret = (Command)MemberwiseClone();

            ret.Param = null;
            if (Param != null)
            {
                ret.Param = iSubscribeCollectionFactory.CreateCollection<Param>();
                foreach (var item in Param)
                {
                    Param c = item.Clone(iSubscribeCollectionFactory);
                    DoParamInner(c, ret);
                    ret.Param.Add(c);
                }
            }
            return ret;
        }

        private static void DoParamInner(Param prm, Command ret)
        {
            prm.ParentCmd = ret;
            if (prm.Param1 != null)
            {
                foreach (var p in prm.Param1)
                {
                    DoParamInner(p, ret);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
