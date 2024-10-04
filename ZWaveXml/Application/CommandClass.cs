/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using System.Xml.Serialization;
using Utils;
using Utils.UI.Bind;

namespace ZWave.Xml.Application
{
    public partial class CommandClass
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

        public Command FindCommand(string cmdName)
        {
            Command ret = Command.First(x => x.Name == cmdName);
            ret.Parent = this;
            return ret;
        }

        public Command FindCommand(byte key)
        {
            Command ret = Command.First(x => Tools.GetByte(x.Key) == key);
            ret.Parent = this;
            return ret;
        }

        public virtual CommandClass Clone(ISubscribeCollectionFactory iSubscribeCollectionFactory)
        {
            CommandClass ret = (CommandClass)MemberwiseClone();

            ret.Command = null;
            if (Command != null)
            {
                ret.Command = iSubscribeCollectionFactory.CreateCollection<Command>();
                foreach (var item in Command)
                {
                    Command c = item.Clone(iSubscribeCollectionFactory);
                    c.Parent = ret;
                    ret.Command.Add(c);
                }
            }

            ret.DefineSet = null;
            if (DefineSet != null)
            {
                ret.DefineSet = iSubscribeCollectionFactory.CreateCollection<DefineSet>();
                foreach (var item in DefineSet)
                {
                    DefineSet c = item.Clone(iSubscribeCollectionFactory);
                    c.Parent = ret;
                    ret.DefineSet.Add(c);
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
