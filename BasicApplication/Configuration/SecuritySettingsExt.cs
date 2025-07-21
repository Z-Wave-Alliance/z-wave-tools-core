/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ZWave.Enums;
using ZWave.Devices;
using System.Collections.ObjectModel;
using Utils;
using Utils.UI;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Security;
using Utils.UI.Wrappers;

namespace ZWave.Configuration
{
    public partial class NetworkKey
    {
        public NetworkKey(byte[] networkKey)
        {
            Value = networkKey;
        }
    }

    public partial class Node
    {
        public Node()
        {
            SecurityExtension = new Collection<SecurityExtension>();
        }

        public Node(NodeTag deviceInfo)
            : this()
        {
            Id = deviceInfo.Id;
            EndPointId = deviceInfo.EndPointId;
            EndPointIdSpecified = deviceInfo.EndPointId > 0;
        }

        public NodeTag NodeTag
        {
            get { return new NodeTag(Id, EndPointId); }
        }
    }

    public partial class SecurityExtension
    {
        [XmlIgnore]
        public NetworkKeyS2Flags KeysValue
        {
            get
            {
                if (Keys != null)
                    return (NetworkKeyS2Flags)Tools.GetByte(Keys);
                return 0;
            }
            set
            {
                Keys = Tools.GetHex((byte)value, true);
                RaisePropertyChanged("KeysValue");
            }
        }
        public SecurityExtension()
        {
        }

        public SecurityExtension(NetworkKeyS2Flags keyMask, byte[] commandClasses)
        {
            KeysValue = keyMask;
            CommandClasses = commandClasses;
        }

        public static SecurityClasses GetSecurityClass(SecuritySchemes scheme)
        {
            SecurityClasses ret = SecurityClasses.None;
            switch (scheme)
            {
                case SecuritySchemes.S2_UNAUTHENTICATED:
                    ret = SecurityClasses.S2Class0;
                    break;
                case SecuritySchemes.S2_AUTHENTICATED:
                    ret = SecurityClasses.S2Class1;
                    break;
                case SecuritySchemes.S2_ACCESS:
                    ret = SecurityClasses.S2Class2;
                    break;
                case SecuritySchemes.S0:
                    ret = SecurityClasses.S0;
                    break;
            }
            return ret;
        }

        public static SecuritySchemes GetSecurityScheme(SecurityClasses securityClass)
        {
            SecuritySchemes ret = SecuritySchemes.NONE;
            switch (securityClass)
            {
                case SecurityClasses.S2Class0:
                    ret = SecuritySchemes.S2_UNAUTHENTICATED;
                    break;
                case SecurityClasses.S2Class1:
                    ret = SecuritySchemes.S2_AUTHENTICATED;
                    break;
                case SecurityClasses.S2Class2:
                    ret = SecuritySchemes.S2_ACCESS;
                    break;
                case SecurityClasses.S0:
                    ret = SecuritySchemes.S0;
                    break;
            }
            return ret;
        }
    }

    public partial class TestParametersS2Settings
    {
        [XmlIgnore]
        public ParameterS2Type ParameterTypeV
        {
            get { return (ParameterS2Type)Enum.Parse(typeof(ParameterS2Type), ParameterType); }
            set { ParameterType = value.ToString(); }
        }
    }

    public partial class TestFrameS2Settings
    {
        [XmlIgnore]
        public SecurityS2TestFrames FrameTypeV
        {
            get { return (SecurityS2TestFrames)Enum.Parse(typeof(SecurityS2TestFrames), FrameType); }
            set { FrameType = value.ToString(); }
        }
    }

    public static class ExtensionMethods
    {
        public static void ApplyTestTestSettings(this IList<TestExtensionS2Settings> settingsList, List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList,
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList)
        {
            var toAddList = settingsList.Where(ext => ext.ActionV == ExtensionAppliedActions.Add).ToList();
            if (toAddList.Count > 0)
            {
                foreach (var ext in toAddList)
                {
                    if (!ext.NumOfUsageSpecified || ext.Counter.Value < ext.NumOfUsage)
                    {
                        ext.AddExtension(extensionsList, encryptedExtensionsList);
                        ext.Counter.Value++;
                    }
                }
            }
        }

        public static void ApplyTestSettings(this IList<TestExtensionS2Settings> settingsList, List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList,
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList)
        {
            var toRemoveList = settingsList.Where(ext => ext.ActionV == ExtensionAppliedActions.Delete).ToList();
            foreach (var ext in toRemoveList)
            {
                if (!ext.NumOfUsageSpecified || ext.Counter.Value < ext.NumOfUsage)
                {
                    ext.RemoveExtension(extensionsList, encryptedExtensionsList);
                    ext.Counter.Value++;
                }
            }
            var toModifyList = settingsList.Where(ext => ext.ActionV != ExtensionAppliedActions.Delete).ToList();
            foreach (var ext in toModifyList)
            {
                if (!ext.NumOfUsageSpecified || ext.Counter.Value < ext.NumOfUsage)
                {
                    switch (ext.ActionV)
                    {
                        case ExtensionAppliedActions.ModifyIfExists:
                            {
                                ext.ModifyExtension(extensionsList, encryptedExtensionsList);
                                ext.Counter.Value++;
                            }
                            break;
                        case ExtensionAppliedActions.AddOrModify:
                            {
                                if (!ext.ModifyExtension(extensionsList, encryptedExtensionsList))
                                {
                                    ext.AddExtension(extensionsList, encryptedExtensionsList);
                                }
                                ext.Counter.Value++;
                            }
                            break;
                    }
                }
            }
        }
    }

    public partial class TestExtensionS2Settings
    {
        public TestExtensionS2Settings(MessageTypes msgType, ExtensionTypes extensionType, ExtensionAppliedActions appliedAction)
            : this()
        {
            MessageTypeV = msgType;
            ExtensionTypeV = extensionType;
            ActionV = appliedAction;
            Counter = new ValueEntity<int>();
        }

        [XmlIgnore]
        public ExtensionAppliedActions ActionV
        {
            get { return (ExtensionAppliedActions)Enum.Parse(typeof(ExtensionAppliedActions), Action); }
            set { Action = value.ToString(); }
        }
        [XmlIgnore]
        public ExtensionTypes ExtensionTypeV
        {
            get { return (ExtensionTypes)Enum.Parse(typeof(ExtensionTypes), ExtensionType); }
            set { ExtensionType = value.ToString(); }
        }
        [XmlIgnore]
        public MessageTypes MessageTypeV
        {
            get { return (MessageTypes)Enum.Parse(typeof(MessageTypes), MessageType); }
            set { MessageType = value.ToString(); }
        }

        [XmlIgnore]
        public bool ValueSpecified { get; set; }

        private ValueEntity<int> _counter = new ValueEntity<int>();
        [XmlIgnore]
        public ValueEntity<int> Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                RaisePropertyChanged("Counter");
            }
        }

        public void RemoveExtension(List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList,
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList)
        {
            switch (ExtensionTypeV)
            {
                case ExtensionTypes.Mpan:
                    RemoveExtensionFromList(ref encryptedExtensionsList, ExtensionTypeV);
                    break;
                case ExtensionTypes.Test:
                    RemoveExtensionFromList(ref encryptedExtensionsList, ExtensionTypeV);
                    RemoveExtensionFromList(ref extensionsList, ExtensionTypeV);
                    break;
                default:
                    RemoveExtensionFromList(ref extensionsList, ExtensionTypeV);
                    break;
            }
        }

        private static void RemoveExtensionFromList(ref List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> list, ExtensionTypes extensionType)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].properties1.type == (byte)extensionType)
                {
                    list.RemoveAt(i);
                    if (i > 0)
                    {
                        list[i - 1].properties1.moreToFollow = 0;
                    }
                }
            }
        }

        public bool ModifyExtension(List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList,
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList)
        {
            bool ret = false;
            var isEncrypted = ExtensionTypeV == ExtensionTypes.Mpan;
            var extList = isEncrypted ? encryptedExtensionsList : extensionsList;
            if (extList == null || extList.Count == 0)
            {
                return ret;
            }
            bool isEncryptionStateChanged = IsEncryptedSpecified && IsEncrypted != isEncrypted;
            int i = 0;
            for (; i < extList.Count; i++)
            {
                if (extList[i].properties1.type == (byte)ExtensionTypeV)
                {
                    ret = true;
                    if (ValueSpecified && Value != null)
                    {
                        extList[i].extension = new List<byte>(Value);
                    }
                    if (ExtensionLengthSpecified)
                    {
                        extList[i].extensionLength = ExtensionLength;
                    }
                    else
                    {
                        extList[i].extensionLength = (byte)(extList[i].extension.Count + 2);
                    }
                    if (IsMoreToFollowSpecified)
                    {
                        extList[i].properties1.moreToFollow = (byte)(IsMoreToFollow ? 0x01 : 0x00);
                    }
                    if (IsCriticalSpecified)
                    {
                        extList[i].properties1.critical = (byte)(IsCritical ? 0x01 : 0x00);
                    }
                    if (isEncryptionStateChanged)
                    {
                        var from = isEncrypted ? encryptedExtensionsList : extensionsList;
                        var to = isEncrypted ? extensionsList : encryptedExtensionsList;
                        to.Add(from[i]);
                        from.RemoveAt(i);
                    }
                    break;
                }
            }
            return ret;
        }

        public static void ResetTracker()
        {
            isPreviousExtensionMoreToFollowDisabled = false;
            isPreviousEncryptedExtensionMoreToFollowDisabled = false;
        }

        static bool isPreviousExtensionMoreToFollowDisabled = false;
        static bool isPreviousEncryptedExtensionMoreToFollowDisabled = false;

        public void AddExtension(List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> extensionsList,
            List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> encryptedExtensionsList)
        {
            byte extensionActualLength = 0;
            if (!ValueSpecified || Value == null)
            {
                extensionActualLength = Extensions.GetLengthByExtensionType(ExtensionTypeV);
                byte valueLength = (byte)(extensionActualLength - 2);
                if (valueLength > 0)
                {
                    Value = new byte[valueLength];
                }
            }
            else
            {
                extensionActualLength = (byte)(Value.Length + 2);
            }

            bool isEncryptedValue = IsEncryptedSpecified
                ? IsEncrypted
                : ExtensionTypeV == ExtensionTypes.Mpan;
            bool isCriticalValue = IsCriticalSpecified
                ? IsCritical
                : ExtensionTypeV != ExtensionTypes.Mos;
            var testExt = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1
            {
                extensionLength = ExtensionLengthSpecified ? ExtensionLength : extensionActualLength,
                properties1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1.Tproperties1
                {
                    moreToFollow = (byte)((IsMoreToFollowSpecified && IsMoreToFollow) ? 0x01 : 0x00),
                    critical = (byte)(isCriticalValue ? 0x01 : 0x00),
                    type = (byte)ExtensionTypeV
                },
                extension = Value
            };

            if (isEncryptedValue)
            {
                if (encryptedExtensionsList.Count > 0 && !isPreviousEncryptedExtensionMoreToFollowDisabled)
                {
                    encryptedExtensionsList[encryptedExtensionsList.Count - 1].properties1.moreToFollow = 1;
                }
                encryptedExtensionsList.Add(testExt);
                isPreviousEncryptedExtensionMoreToFollowDisabled = IsMoreToFollowSpecified ? !IsMoreToFollow : false;
            }
            else
            {
                if (extensionsList.Count > 0 && !isPreviousExtensionMoreToFollowDisabled)
                {
                    extensionsList[extensionsList.Count - 1].properties1.moreToFollow = 1;
                }
                extensionsList.Add(testExt);
                isPreviousExtensionMoreToFollowDisabled = IsMoreToFollowSpecified ? !IsMoreToFollow : false;
            }
        }
    }
}
