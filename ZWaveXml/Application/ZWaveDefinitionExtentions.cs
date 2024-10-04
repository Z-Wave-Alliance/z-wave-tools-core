/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Linq;
using Utils;

namespace ZWave.Xml.Application
{
    public static class ZWaveDefinitionExtentions
    {
        public static CommandClass FindCommandClass(this ZWaveDefinition zWaveDefinition, string name, byte version)
        {
            return zWaveDefinition.CommandClasses.FirstOrDefault(ret => ret.Name == name && ret.Version == version);
        }

        public static CommandClass FindCommandClass(this ZWaveDefinition zWaveDefinition, byte key, byte version)
        {
            return zWaveDefinition.CommandClasses.FirstOrDefault(ret => ret.KeyId == key && ret.Version == version);
        }

        public static List<CommandClass> FindCommandClasses(this ZWaveDefinition zWaveDefinition, byte key)
        {
            return zWaveDefinition.CommandClasses.Where(ret => ret.KeyId == key).ToList();
        }

        public static Command FindCommand(this ZWaveDefinition zWaveDefinition, CommandClass commandClass, byte key)
        {
            Command ret = null;
            if (commandClass.Command != null)
                foreach (Command var in commandClass.Command)
                {
                    if (var.Bits > 0 && var.Bits < 8)
                    {
                        if (var.KeyId == (key & Tools.GetMaskFromBits(var.Bits, (byte)(8 - var.Bits))))
                        {
                            ret = var;
                            break;
                        }
                    }
                    else if (var.KeyId == key)
                    {
                        ret = var;
                        break;
                    }
                }
            return ret;
        }

        public static Command FindCommand(this ZWaveDefinition zWaveDefinition, CommandClass commandClass, string name)
        {
            return commandClass.Command.FirstOrDefault(ret => ret.Name == name);
        }

        public static Command FindCommand(this ZWaveDefinition zWaveDefinition, string commandClassName, byte commandClassVersion, string name)
        {
            CommandClass cmdClass = FindCommandClass(zWaveDefinition, commandClassName, commandClassVersion);
            return cmdClass.Command.FirstOrDefault(ret => ret.Name == name);
        }
    }
}
