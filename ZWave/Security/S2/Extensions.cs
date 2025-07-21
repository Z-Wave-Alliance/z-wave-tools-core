/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.CommandClasses;

namespace ZWave.Security
{
    public class Extensions
    {
        public Extensions()
        {
            ExtensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
            EncryptedExtensionsList = new List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1>();
        }

        public List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> ExtensionsList { get; set; }
        public List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> EncryptedExtensionsList { get; set; }

        //private bool? _prevTestFrameIsMoreToFollow = null;

        public void MergeWith(Extensions extensions)
        {
            if (extensions.ExtensionsList.Count > 0)
            {
                if (ExtensionsList.Count > 0)
                {
                    var last = ExtensionsList[ExtensionsList.Count - 1];
                    if (last.properties1.type != (byte)ExtensionTypes.Test)
                    {
                        last.properties1.moreToFollow = 1;
                    }
                }
                ExtensionsList.AddRange(extensions.ExtensionsList);
            }
            if (extensions.EncryptedExtensionsList.Count > 0)
            {
                if (EncryptedExtensionsList.Count > 0)
                {
                    var last = EncryptedExtensionsList[EncryptedExtensionsList.Count - 1];
                    if (last.properties1.type != (byte)ExtensionTypes.Test)
                    {
                        last.properties1.moreToFollow = 1;
                    }
                }
                EncryptedExtensionsList.AddRange(extensions.EncryptedExtensionsList);
            }
        }

        public static byte GetLengthByExtensionType(ExtensionTypes extensionType)
        {
            byte extensionLength = 2;
            switch (extensionType)
            {
                case ExtensionTypes.Mos:
                    extensionLength = 2;
                    break;
                case ExtensionTypes.Mpan:
                    extensionLength = 19;
                    break;
                case ExtensionTypes.MpanGrp:
                    extensionLength = 3;
                    break;
                case ExtensionTypes.Span:
                    extensionLength = 18;
                    break;
            }
            return extensionLength;
        }

        public void AddSpanExtension(byte[] senderEntropyInput)
        {
            if (ExtensionsList.Count > 0)
            {
                ExtensionsList[ExtensionsList.Count - 1].properties1.moreToFollow = 1;
            }

            ExtensionsList.Add(
                new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1
                {
                    extensionLength = GetLengthByExtensionType(ExtensionTypes.Span),
                    properties1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1.Tproperties1
                    {
                        moreToFollow = 0,
                        critical = 1,
                        type = (byte)ExtensionTypes.Span
                    },
                    extension = new List<byte>(senderEntropyInput)
                });
        }

        public void AddMpanExtension(byte[] mpan, byte groupId)
        {
            if (EncryptedExtensionsList.Count > 0)
            {
                EncryptedExtensionsList[EncryptedExtensionsList.Count - 1].properties1.moreToFollow = 1;
            }

            var mpanExt = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1
            {
                extensionLength = GetLengthByExtensionType(ExtensionTypes.Mpan),
                properties1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1.Tproperties1
                {
                    moreToFollow = 0,
                    critical = 1,
                    type = (byte)ExtensionTypes.Mpan
                }
            };
            var extData = new List<byte>();
            extData.Add(groupId);
            extData.AddRange(mpan);
            mpanExt.extension = extData;
            EncryptedExtensionsList.Add(mpanExt);
        }

        public void AddMpanGrpExtension(byte groupId)
        {
            if (ExtensionsList.Count > 0)
            {
                ExtensionsList[ExtensionsList.Count - 1].properties1.moreToFollow = 1;
            }

            ExtensionsList.Add(
                new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1
                {
                    extensionLength = GetLengthByExtensionType(ExtensionTypes.MpanGrp),
                    properties1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1.Tproperties1
                    {
                        moreToFollow = 0,
                        critical = 1,
                        type = (byte)ExtensionTypes.MpanGrp
                    },
                    extension = new List<byte>(new[] { groupId })
                });
        }

        public void AddMosExtension()
        {
            if (ExtensionsList.Count > 0)
            {
                ExtensionsList[ExtensionsList.Count - 1].properties1.moreToFollow = 1;
            }

            ExtensionsList.Add(
                new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1
                {
                    extensionLength = GetLengthByExtensionType(ExtensionTypes.Mos),
                    properties1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1.Tproperties1
                    {
                        moreToFollow = 0,
                        critical = 0,
                        type = (byte)ExtensionTypes.Mos
                    }
                });
        }
    }
}
