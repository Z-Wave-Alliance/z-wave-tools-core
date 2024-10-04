/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using ZWave.CommandClasses;

namespace ZWave.ZnifferApplication
{
    public class ExtensionsFilterObject : FilterObject
    {
        public byte ExtType { get; set; }

        public bool IsEncrypted { get; set; }

        protected override bool ApplyFilterState(DataItem ditem, SecurityManager securityManager)
        {
            if (ditem.Payload != null && base.ApplyFilterState(ditem, securityManager))
            {
                var payload = ditem.CarryPayload.FirstOrDefault(x => x != null && x.Length > 1 && x[0] == COMMAND_CLASS_SECURITY_2.ID && x[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID);
                if (payload != null)
                {
                    var cmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)payload;
                    // Catch any extension.
                    if (ExtType == 0 && (cmd.properties1.extension == 0x01 || cmd.properties1.encryptedExtension == 0x01))
                    {
                        return true;
                    }
                    // Catch specified extension.
                    byte encFlag = (byte)(IsEncrypted ? 0x01 : 0x00);
                    if (IsEncrypted && cmd.properties1.encryptedExtension == encFlag)
                    {
                        var type = ditem.Payload[1] & 0x3F;
                        if (type == ExtType)
                        {
                            return true;
                        }
                    }
                    else if (cmd.properties1.extension == 0x01)
                    {
                        foreach (var vg1 in cmd.vg1)
                        {
                            if (vg1.properties1.type == ExtType)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
