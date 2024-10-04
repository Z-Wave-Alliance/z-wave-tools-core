/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_USER_CREDENTIAL
    {
        public const byte ID = 0x83;
        public const byte VERSION = 1;
        public partial class USER_CAPABILITIES_GET
        {
            public const byte ID = 0x01;
            public static implicit operator USER_CAPABILITIES_GET(byte[] data)
            {
                USER_CAPABILITIES_GET ret = new USER_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](USER_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class USER_CAPABILITIES_REPORT
        {
            public const byte ID = 0x02;
            public const byte numberOfSupportedUserUniqueIdentifiersBytesCount = 2;
            public byte[] numberOfSupportedUserUniqueIdentifiers = new byte[numberOfSupportedUserUniqueIdentifiersBytesCount];
            public ByteValue supportedCredentialRulesBitMask = 0;
            public ByteValue maxLengthOfUserName = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte userChecksumSupport
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte allUsersChecksumSupport
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte userScheduleSupport
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue supportedUserTypesBitMaskLength = 0;
            public class TVG1
            {
                public ByteValue supportedUserTypesBitMask = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator USER_CAPABILITIES_REPORT(byte[] data)
            {
                USER_CAPABILITIES_REPORT ret = new USER_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSupportedUserUniqueIdentifiers = (data.Length - index) >= numberOfSupportedUserUniqueIdentifiersBytesCount ? new byte[numberOfSupportedUserUniqueIdentifiersBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.numberOfSupportedUserUniqueIdentifiers[0] = data[index++];
                    if (data.Length > index) ret.numberOfSupportedUserUniqueIdentifiers[1] = data[index++];
                    ret.supportedCredentialRulesBitMask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.maxLengthOfUserName = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.supportedUserTypesBitMaskLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.supportedUserTypesBitMaskLength; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.supportedUserTypesBitMask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.numberOfSupportedUserUniqueIdentifiers != null)
                {
                    foreach (var tmp in command.numberOfSupportedUserUniqueIdentifiers)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.supportedCredentialRulesBitMask.HasValue) ret.Add(command.supportedCredentialRulesBitMask);
                if (command.maxLengthOfUserName.HasValue) ret.Add(command.maxLengthOfUserName);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.supportedUserTypesBitMaskLength.HasValue) ret.Add(command.supportedUserTypesBitMaskLength);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.supportedUserTypesBitMask.HasValue) ret.Add(item.supportedUserTypesBitMask);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_CAPABILITIES_GET
        {
            public const byte ID = 0x03;
            public static implicit operator CREDENTIAL_CAPABILITIES_GET(byte[] data)
            {
                CREDENTIAL_CAPABILITIES_GET ret = new CREDENTIAL_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_CAPABILITIES_REPORT
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte credentialChecksumSupport
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue numberOfSupportedCredentialTypes = 0;
            public class TVG1CREDENTIALTYPE
            {
                public ByteValue credentialType = 0;
            }
            public List<TVG1CREDENTIALTYPE> vg1CredentialType = new List<TVG1CREDENTIALTYPE>();
            public class TVG2PROPERTIES2PERCREDENTIALTYPE
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte reserved2
                    {
                        get { return (byte)(_value >> 0 & 0x7F); }
                        set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                    }
                    public byte clSupport
                    {
                        get { return (byte)(_value >> 7 & 0x01); }
                        set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                    }
                    public static implicit operator Tproperties1(byte data)
                    {
                        Tproperties1 ret = new Tproperties1();
                        ret._value = data;
                        ret.HasValue = true;
                        return ret;
                    }
                    public static implicit operator byte(Tproperties1 prm)
                    {
                        return prm._value;
                    }
                }
                public Tproperties1 properties1 = 0;
            }
            public List<TVG2PROPERTIES2PERCREDENTIALTYPE> vg2Properties2PerCredentialType = new List<TVG2PROPERTIES2PERCREDENTIALTYPE>();
            public class TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE
            {
                public const byte numberOfSupportedCredentialSlotsBytesCount = 2;
                public byte[] numberOfSupportedCredentialSlots = new byte[numberOfSupportedCredentialSlotsBytesCount];
            }
            public List<TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE> vg3NumberOfSupportedCredentialSlotsPerCredentialType = new List<TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE>();
            public class TVG4MINLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE
            {
                public ByteValue minLengthOfCredentialData = 0;
            }
            public List<TVG4MINLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE> vg4MinLengthOfCredentialDataPerCredentialType = new List<TVG4MINLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE>();
            public class TVG5MAXLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE
            {
                public ByteValue maxLengthOfCredentialData = 0;
            }
            public List<TVG5MAXLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE> vg5MaxLengthOfCredentialDataPerCredentialType = new List<TVG5MAXLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE>();
            public class TVG6CLRECOMMENDEDTIMEOUTPERCREDENTIALTYPE
            {
                public ByteValue clRecommendedTimeout = 0;
            }
            public List<TVG6CLRECOMMENDEDTIMEOUTPERCREDENTIALTYPE> vg6ClRecommendedTimeoutPerCredentialType = new List<TVG6CLRECOMMENDEDTIMEOUTPERCREDENTIALTYPE>();
            public class TVG7CLNUMBEROFSTEPSPERCREDENTIALTYPE
            {
                public ByteValue clNumberOfSteps = 0;
            }
            public List<TVG7CLNUMBEROFSTEPSPERCREDENTIALTYPE> vg7ClNumberOfStepsPerCredentialType = new List<TVG7CLNUMBEROFSTEPSPERCREDENTIALTYPE>();
            public static implicit operator CREDENTIAL_CAPABILITIES_REPORT(byte[] data)
            {
                CREDENTIAL_CAPABILITIES_REPORT ret = new CREDENTIAL_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.numberOfSupportedCredentialTypes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1CredentialType = new List<TVG1CREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG1CREDENTIALTYPE tmp = new TVG1CREDENTIALTYPE();
                        tmp.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1CredentialType.Add(tmp);
                    }
                    ret.vg2Properties2PerCredentialType = new List<TVG2PROPERTIES2PERCREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG2PROPERTIES2PERCREDENTIALTYPE tmp = new TVG2PROPERTIES2PERCREDENTIALTYPE();
                        tmp.properties1 = data.Length > index ? (TVG2PROPERTIES2PERCREDENTIALTYPE.Tproperties1)data[index++] : TVG2PROPERTIES2PERCREDENTIALTYPE.Tproperties1.Empty;
                        ret.vg2Properties2PerCredentialType.Add(tmp);
                    }
                    ret.vg3NumberOfSupportedCredentialSlotsPerCredentialType = new List<TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE tmp = new TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE();
                        tmp.numberOfSupportedCredentialSlots = (data.Length - index) >= TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE.numberOfSupportedCredentialSlotsBytesCount ? new byte[TVG3NUMBEROFSUPPORTEDCREDENTIALSLOTSPERCREDENTIALTYPE.numberOfSupportedCredentialSlotsBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.numberOfSupportedCredentialSlots[0] = data[index++];
                        if (data.Length > index) tmp.numberOfSupportedCredentialSlots[1] = data[index++];
                        ret.vg3NumberOfSupportedCredentialSlotsPerCredentialType.Add(tmp);
                    }
                    ret.vg4MinLengthOfCredentialDataPerCredentialType = new List<TVG4MINLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG4MINLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE tmp = new TVG4MINLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE();
                        tmp.minLengthOfCredentialData = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg4MinLengthOfCredentialDataPerCredentialType.Add(tmp);
                    }
                    ret.vg5MaxLengthOfCredentialDataPerCredentialType = new List<TVG5MAXLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG5MAXLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE tmp = new TVG5MAXLENGTHOFCREDENTIALDATAPERCREDENTIALTYPE();
                        tmp.maxLengthOfCredentialData = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg5MaxLengthOfCredentialDataPerCredentialType.Add(tmp);
                    }
                    ret.vg6ClRecommendedTimeoutPerCredentialType = new List<TVG6CLRECOMMENDEDTIMEOUTPERCREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG6CLRECOMMENDEDTIMEOUTPERCREDENTIALTYPE tmp = new TVG6CLRECOMMENDEDTIMEOUTPERCREDENTIALTYPE();
                        tmp.clRecommendedTimeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg6ClRecommendedTimeoutPerCredentialType.Add(tmp);
                    }
                    ret.vg7ClNumberOfStepsPerCredentialType = new List<TVG7CLNUMBEROFSTEPSPERCREDENTIALTYPE>();
                    for (int j = 0; j < ret.numberOfSupportedCredentialTypes; j++)
                    {
                        TVG7CLNUMBEROFSTEPSPERCREDENTIALTYPE tmp = new TVG7CLNUMBEROFSTEPSPERCREDENTIALTYPE();
                        tmp.clNumberOfSteps = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg7ClNumberOfStepsPerCredentialType.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.numberOfSupportedCredentialTypes.HasValue) ret.Add(command.numberOfSupportedCredentialTypes);
                if (command.vg1CredentialType != null)
                {
                    foreach (var item in command.vg1CredentialType)
                    {
                        if (item.credentialType.HasValue) ret.Add(item.credentialType);
                    }
                }
                if (command.vg2Properties2PerCredentialType != null)
                {
                    foreach (var item in command.vg2Properties2PerCredentialType)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                if (command.vg3NumberOfSupportedCredentialSlotsPerCredentialType != null)
                {
                    foreach (var item in command.vg3NumberOfSupportedCredentialSlotsPerCredentialType)
                    {
                        if (item.numberOfSupportedCredentialSlots != null)
                        {
                            foreach (var tmp in item.numberOfSupportedCredentialSlots)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                if (command.vg4MinLengthOfCredentialDataPerCredentialType != null)
                {
                    foreach (var item in command.vg4MinLengthOfCredentialDataPerCredentialType)
                    {
                        if (item.minLengthOfCredentialData.HasValue) ret.Add(item.minLengthOfCredentialData);
                    }
                }
                if (command.vg5MaxLengthOfCredentialDataPerCredentialType != null)
                {
                    foreach (var item in command.vg5MaxLengthOfCredentialDataPerCredentialType)
                    {
                        if (item.maxLengthOfCredentialData.HasValue) ret.Add(item.maxLengthOfCredentialData);
                    }
                }
                if (command.vg6ClRecommendedTimeoutPerCredentialType != null)
                {
                    foreach (var item in command.vg6ClRecommendedTimeoutPerCredentialType)
                    {
                        if (item.clRecommendedTimeout.HasValue) ret.Add(item.clRecommendedTimeout);
                    }
                }
                if (command.vg7ClNumberOfStepsPerCredentialType != null)
                {
                    foreach (var item in command.vg7ClNumberOfStepsPerCredentialType)
                    {
                        if (item.clNumberOfSteps.HasValue) ret.Add(item.clNumberOfSteps);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_SET
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operationType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue userType = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte userActiveState
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public ByteValue credentialRule = 0;
            public const byte expiringTimeoutMinutesBytesCount = 2;
            public byte[] expiringTimeoutMinutes = new byte[expiringTimeoutMinutesBytesCount];
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte userNameEncoding
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue userNameLength = 0;
            public IList<byte> userName = new List<byte>();
            public static implicit operator USER_SET(byte[] data)
            {
                USER_SET ret = new USER_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.userType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.credentialRule = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.expiringTimeoutMinutes = (data.Length - index) >= expiringTimeoutMinutesBytesCount ? new byte[expiringTimeoutMinutesBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.expiringTimeoutMinutes[0] = data[index++];
                    if (data.Length > index) ret.expiringTimeoutMinutes[1] = data[index++];
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.userNameLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userName = new List<byte>();
                    for (int i = 0; i < ret.userNameLength; i++)
                    {
                        if (data.Length > index) ret.userName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userType.HasValue) ret.Add(command.userType);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.credentialRule.HasValue) ret.Add(command.credentialRule);
                if (command.expiringTimeoutMinutes != null)
                {
                    foreach (var tmp in command.expiringTimeoutMinutes)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.userNameLength.HasValue) ret.Add(command.userNameLength);
                if (command.userName != null)
                {
                    foreach (var tmp in command.userName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_GET
        {
            public const byte ID = 0x06;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public static implicit operator USER_GET(byte[] data)
            {
                USER_GET ret = new USER_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_REPORT
        {
            public const byte ID = 0x07;
            public const byte nextUserUniqueIdentifierBytesCount = 2;
            public byte[] nextUserUniqueIdentifier = new byte[nextUserUniqueIdentifierBytesCount];
            public ByteValue userModifierType = 0;
            public const byte userModifierNodeIdBytesCount = 2;
            public byte[] userModifierNodeId = new byte[userModifierNodeIdBytesCount];
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue userType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte userActiveState
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialRule = 0;
            public const byte expiringTimeoutMinutesBytesCount = 2;
            public byte[] expiringTimeoutMinutes = new byte[expiringTimeoutMinutesBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte userNameEncoding
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public ByteValue userNameLength = 0;
            public IList<byte> userName = new List<byte>();
            public static implicit operator USER_REPORT(byte[] data)
            {
                USER_REPORT ret = new USER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.nextUserUniqueIdentifier = (data.Length - index) >= nextUserUniqueIdentifierBytesCount ? new byte[nextUserUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextUserUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.nextUserUniqueIdentifier[1] = data[index++];
                    ret.userModifierType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userModifierNodeId = (data.Length - index) >= userModifierNodeIdBytesCount ? new byte[userModifierNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userModifierNodeId[0] = data[index++];
                    if (data.Length > index) ret.userModifierNodeId[1] = data[index++];
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.userType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialRule = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.expiringTimeoutMinutes = (data.Length - index) >= expiringTimeoutMinutesBytesCount ? new byte[expiringTimeoutMinutesBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.expiringTimeoutMinutes[0] = data[index++];
                    if (data.Length > index) ret.expiringTimeoutMinutes[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.userNameLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userName = new List<byte>();
                    for (int i = 0; i < ret.userNameLength; i++)
                    {
                        if (data.Length > index) ret.userName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.nextUserUniqueIdentifier != null)
                {
                    foreach (var tmp in command.nextUserUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userModifierType.HasValue) ret.Add(command.userModifierType);
                if (command.userModifierNodeId != null)
                {
                    foreach (var tmp in command.userModifierNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userType.HasValue) ret.Add(command.userType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialRule.HasValue) ret.Add(command.credentialRule);
                if (command.expiringTimeoutMinutes != null)
                {
                    foreach (var tmp in command.expiringTimeoutMinutes)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.userNameLength.HasValue) ret.Add(command.userNameLength);
                if (command.userName != null)
                {
                    foreach (var tmp in command.userName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_SET_ERROR_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue userSetErrorType = 0;
            public ByteValue userModifierType = 0;
            public const byte userModifierNodeIdBytesCount = 2;
            public byte[] userModifierNodeId = new byte[userModifierNodeIdBytesCount];
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue userType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte userActiveState
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialRule = 0;
            public const byte expiringTimeoutMinutesBytesCount = 2;
            public byte[] expiringTimeoutMinutes = new byte[expiringTimeoutMinutesBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte userNameEncoding
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public ByteValue userNameLength = 0;
            public IList<byte> userName = new List<byte>();
            public static implicit operator USER_SET_ERROR_REPORT(byte[] data)
            {
                USER_SET_ERROR_REPORT ret = new USER_SET_ERROR_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userSetErrorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userModifierType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userModifierNodeId = (data.Length - index) >= userModifierNodeIdBytesCount ? new byte[userModifierNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userModifierNodeId[0] = data[index++];
                    if (data.Length > index) ret.userModifierNodeId[1] = data[index++];
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.userType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialRule = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.expiringTimeoutMinutes = (data.Length - index) >= expiringTimeoutMinutesBytesCount ? new byte[expiringTimeoutMinutesBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.expiringTimeoutMinutes[0] = data[index++];
                    if (data.Length > index) ret.expiringTimeoutMinutes[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.userNameLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userName = new List<byte>();
                    for (int i = 0; i < ret.userNameLength; i++)
                    {
                        if (data.Length > index) ret.userName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_SET_ERROR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userSetErrorType.HasValue) ret.Add(command.userSetErrorType);
                if (command.userModifierType.HasValue) ret.Add(command.userModifierType);
                if (command.userModifierNodeId != null)
                {
                    foreach (var tmp in command.userModifierNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userType.HasValue) ret.Add(command.userType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialRule.HasValue) ret.Add(command.credentialRule);
                if (command.expiringTimeoutMinutes != null)
                {
                    foreach (var tmp in command.expiringTimeoutMinutes)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.userNameLength.HasValue) ret.Add(command.userNameLength);
                if (command.userName != null)
                {
                    foreach (var tmp in command.userName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_NOTIFICATION_REPORT
        {
            public const byte ID = 0x09;
            public ByteValue userModifierType = 0;
            public const byte userModifierNodeIdBytesCount = 2;
            public byte[] userModifierNodeId = new byte[userModifierNodeIdBytesCount];
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue userType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte userActiveState
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialRule = 0;
            public const byte expiringTimeoutMinutesBytesCount = 2;
            public byte[] expiringTimeoutMinutes = new byte[expiringTimeoutMinutesBytesCount];
            public static implicit operator USER_NOTIFICATION_REPORT(byte[] data)
            {
                USER_NOTIFICATION_REPORT ret = new USER_NOTIFICATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userModifierType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userModifierNodeId = (data.Length - index) >= userModifierNodeIdBytesCount ? new byte[userModifierNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userModifierNodeId[0] = data[index++];
                    if (data.Length > index) ret.userModifierNodeId[1] = data[index++];
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.userType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialRule = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.expiringTimeoutMinutes = (data.Length - index) >= expiringTimeoutMinutesBytesCount ? new byte[expiringTimeoutMinutesBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.expiringTimeoutMinutes[0] = data[index++];
                    if (data.Length > index) ret.expiringTimeoutMinutes[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USER_NOTIFICATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userModifierType.HasValue) ret.Add(command.userModifierType);
                if (command.userModifierNodeId != null)
                {
                    foreach (var tmp in command.userModifierNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userType.HasValue) ret.Add(command.userType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialRule.HasValue) ret.Add(command.credentialRule);
                if (command.expiringTimeoutMinutes != null)
                {
                    foreach (var tmp in command.expiringTimeoutMinutes)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_SET
        {
            public const byte ID = 0x0A;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operationType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialLength = 0;
            public IList<byte> credentialData = new List<byte>();
            public static implicit operator CREDENTIAL_SET(byte[] data)
            {
                CREDENTIAL_SET ret = new CREDENTIAL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialData = new List<byte>();
                    for (int i = 0; i < ret.credentialLength; i++)
                    {
                        if (data.Length > index) ret.credentialData.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialLength.HasValue) ret.Add(command.credentialLength);
                if (command.credentialData != null)
                {
                    foreach (var tmp in command.credentialData)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_GET
        {
            public const byte ID = 0x0B;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public static implicit operator CREDENTIAL_GET(byte[] data)
            {
                CREDENTIAL_GET ret = new CREDENTIAL_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_REPORT
        {
            public const byte ID = 0x0C;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte crb
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialLength = 0;
            public IList<byte> credentialData = new List<byte>();
            public ByteValue credentialModifierType = 0;
            public const byte credentialModifierNodeIdBytesCount = 2;
            public byte[] credentialModifierNodeId = new byte[credentialModifierNodeIdBytesCount];
            public ByteValue nextCredentialType = 0;
            public const byte nextCredentialSlotBytesCount = 2;
            public byte[] nextCredentialSlot = new byte[nextCredentialSlotBytesCount];
            public static implicit operator CREDENTIAL_REPORT(byte[] data)
            {
                CREDENTIAL_REPORT ret = new CREDENTIAL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialData = new List<byte>();
                    for (int i = 0; i < ret.credentialLength; i++)
                    {
                        if (data.Length > index) ret.credentialData.Add(data[index++]);
                    }
                    ret.credentialModifierType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialModifierNodeId = (data.Length - index) >= credentialModifierNodeIdBytesCount ? new byte[credentialModifierNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialModifierNodeId[0] = data[index++];
                    if (data.Length > index) ret.credentialModifierNodeId[1] = data[index++];
                    ret.nextCredentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nextCredentialSlot = (data.Length - index) >= nextCredentialSlotBytesCount ? new byte[nextCredentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextCredentialSlot[0] = data[index++];
                    if (data.Length > index) ret.nextCredentialSlot[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialLength.HasValue) ret.Add(command.credentialLength);
                if (command.credentialData != null)
                {
                    foreach (var tmp in command.credentialData)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialModifierType.HasValue) ret.Add(command.credentialModifierType);
                if (command.credentialModifierNodeId != null)
                {
                    foreach (var tmp in command.credentialModifierNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nextCredentialType.HasValue) ret.Add(command.nextCredentialType);
                if (command.nextCredentialSlot != null)
                {
                    foreach (var tmp in command.nextCredentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_SET_ERROR_REPORT
        {
            public const byte ID = 0x0D;
            public ByteValue credentialSetErrorType = 0;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte crb
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialLength = 0;
            public IList<byte> credentialData = new List<byte>();
            public ByteValue credentialModifierType = 0;
            public const byte credentialModifierNodeIdBytesCount = 2;
            public byte[] credentialModifierNodeId = new byte[credentialModifierNodeIdBytesCount];
            public static implicit operator CREDENTIAL_SET_ERROR_REPORT(byte[] data)
            {
                CREDENTIAL_SET_ERROR_REPORT ret = new CREDENTIAL_SET_ERROR_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.credentialSetErrorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialData = new List<byte>();
                    for (int i = 0; i < ret.credentialLength; i++)
                    {
                        if (data.Length > index) ret.credentialData.Add(data[index++]);
                    }
                    ret.credentialModifierType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialModifierNodeId = (data.Length - index) >= credentialModifierNodeIdBytesCount ? new byte[credentialModifierNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialModifierNodeId[0] = data[index++];
                    if (data.Length > index) ret.credentialModifierNodeId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_SET_ERROR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.credentialSetErrorType.HasValue) ret.Add(command.credentialSetErrorType);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialLength.HasValue) ret.Add(command.credentialLength);
                if (command.credentialData != null)
                {
                    foreach (var tmp in command.credentialData)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialModifierType.HasValue) ret.Add(command.credentialModifierType);
                if (command.credentialModifierNodeId != null)
                {
                    foreach (var tmp in command.credentialModifierNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_NOTIFICATION_REPORT
        {
            public const byte ID = 0x0E;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte crb
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialLength = 0;
            public IList<byte> credentialData = new List<byte>();
            public ByteValue credentialModifierType = 0;
            public const byte credentialModifierNodeIdBytesCount = 2;
            public byte[] credentialModifierNodeId = new byte[credentialModifierNodeIdBytesCount];
            public static implicit operator CREDENTIAL_NOTIFICATION_REPORT(byte[] data)
            {
                CREDENTIAL_NOTIFICATION_REPORT ret = new CREDENTIAL_NOTIFICATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialData = new List<byte>();
                    for (int i = 0; i < ret.credentialLength; i++)
                    {
                        if (data.Length > index) ret.credentialData.Add(data[index++]);
                    }
                    ret.credentialModifierType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialModifierNodeId = (data.Length - index) >= credentialModifierNodeIdBytesCount ? new byte[credentialModifierNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialModifierNodeId[0] = data[index++];
                    if (data.Length > index) ret.credentialModifierNodeId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_NOTIFICATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialLength.HasValue) ret.Add(command.credentialLength);
                if (command.credentialData != null)
                {
                    foreach (var tmp in command.credentialData)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialModifierType.HasValue) ret.Add(command.credentialModifierType);
                if (command.credentialModifierNodeId != null)
                {
                    foreach (var tmp in command.credentialModifierNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_LEARN_START
        {
            public const byte ID = 0x0F;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operationType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue credentialLearnTimeout = 0;
            public static implicit operator CREDENTIAL_LEARN_START(byte[] data)
            {
                CREDENTIAL_LEARN_START ret = new CREDENTIAL_LEARN_START();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.credentialLearnTimeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_LEARN_START command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.credentialLearnTimeout.HasValue) ret.Add(command.credentialLearnTimeout);
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_LEARN_CANCEL
        {
            public const byte ID = 0x10;
            public static implicit operator CREDENTIAL_LEARN_CANCEL(byte[] data)
            {
                CREDENTIAL_LEARN_CANCEL ret = new CREDENTIAL_LEARN_CANCEL();
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_LEARN_CANCEL command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_LEARN_REPORT
        {
            public const byte ID = 0x11;
            public ByteValue credentialLearnStatus = 0;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public ByteValue credentialType = 0;
            public const byte credentialSlotBytesCount = 2;
            public byte[] credentialSlot = new byte[credentialSlotBytesCount];
            public ByteValue credentialLearnStepsRemaining = 0;
            public static implicit operator CREDENTIAL_LEARN_REPORT(byte[] data)
            {
                CREDENTIAL_LEARN_REPORT ret = new CREDENTIAL_LEARN_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.credentialLearnStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialSlot = (data.Length - index) >= credentialSlotBytesCount ? new byte[credentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialSlot[0] = data[index++];
                    if (data.Length > index) ret.credentialSlot[1] = data[index++];
                    ret.credentialLearnStepsRemaining = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_LEARN_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.credentialLearnStatus.HasValue) ret.Add(command.credentialLearnStatus);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialSlot != null)
                {
                    foreach (var tmp in command.credentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.credentialLearnStepsRemaining.HasValue) ret.Add(command.credentialLearnStepsRemaining);
                return ret.ToArray();
            }
        }
        public partial class USER_CREDENTIAL_ASSOCIATION_SET
        {
            public const byte ID = 0x12;
            public const byte sourceUserUniqueIdentifierBytesCount = 2;
            public byte[] sourceUserUniqueIdentifier = new byte[sourceUserUniqueIdentifierBytesCount];
            public ByteValue sourceCredentialType = 0;
            public const byte sourceCredentialSlotBytesCount = 2;
            public byte[] sourceCredentialSlot = new byte[sourceCredentialSlotBytesCount];
            public const byte destinationUserUniqueIdentifierBytesCount = 2;
            public byte[] destinationUserUniqueIdentifier = new byte[destinationUserUniqueIdentifierBytesCount];
            public const byte destinationCredentialSlotBytesCount = 2;
            public byte[] destinationCredentialSlot = new byte[destinationCredentialSlotBytesCount];
            public static implicit operator USER_CREDENTIAL_ASSOCIATION_SET(byte[] data)
            {
                USER_CREDENTIAL_ASSOCIATION_SET ret = new USER_CREDENTIAL_ASSOCIATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.sourceUserUniqueIdentifier = (data.Length - index) >= sourceUserUniqueIdentifierBytesCount ? new byte[sourceUserUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.sourceUserUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.sourceUserUniqueIdentifier[1] = data[index++];
                    ret.sourceCredentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sourceCredentialSlot = (data.Length - index) >= sourceCredentialSlotBytesCount ? new byte[sourceCredentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.sourceCredentialSlot[0] = data[index++];
                    if (data.Length > index) ret.sourceCredentialSlot[1] = data[index++];
                    ret.destinationUserUniqueIdentifier = (data.Length - index) >= destinationUserUniqueIdentifierBytesCount ? new byte[destinationUserUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.destinationUserUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.destinationUserUniqueIdentifier[1] = data[index++];
                    ret.destinationCredentialSlot = (data.Length - index) >= destinationCredentialSlotBytesCount ? new byte[destinationCredentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.destinationCredentialSlot[0] = data[index++];
                    if (data.Length > index) ret.destinationCredentialSlot[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USER_CREDENTIAL_ASSOCIATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.sourceUserUniqueIdentifier != null)
                {
                    foreach (var tmp in command.sourceUserUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sourceCredentialType.HasValue) ret.Add(command.sourceCredentialType);
                if (command.sourceCredentialSlot != null)
                {
                    foreach (var tmp in command.sourceCredentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.destinationUserUniqueIdentifier != null)
                {
                    foreach (var tmp in command.destinationUserUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.destinationCredentialSlot != null)
                {
                    foreach (var tmp in command.destinationCredentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CREDENTIAL_ASSOCIATION_REPORT
        {
            public const byte ID = 0x13;
            public const byte sourceUserUniqueIdentifierBytesCount = 2;
            public byte[] sourceUserUniqueIdentifier = new byte[sourceUserUniqueIdentifierBytesCount];
            public ByteValue sourceCredentialType = 0;
            public const byte sourceCredentialSlotBytesCount = 2;
            public byte[] sourceCredentialSlot = new byte[sourceCredentialSlotBytesCount];
            public const byte destinationUserUniqueIdentifierBytesCount = 2;
            public byte[] destinationUserUniqueIdentifier = new byte[destinationUserUniqueIdentifierBytesCount];
            public const byte destinationCredentialSlotBytesCount = 2;
            public byte[] destinationCredentialSlot = new byte[destinationCredentialSlotBytesCount];
            public ByteValue userCredentialAssociationStatus = 0;
            public static implicit operator USER_CREDENTIAL_ASSOCIATION_REPORT(byte[] data)
            {
                USER_CREDENTIAL_ASSOCIATION_REPORT ret = new USER_CREDENTIAL_ASSOCIATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sourceUserUniqueIdentifier = (data.Length - index) >= sourceUserUniqueIdentifierBytesCount ? new byte[sourceUserUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.sourceUserUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.sourceUserUniqueIdentifier[1] = data[index++];
                    ret.sourceCredentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sourceCredentialSlot = (data.Length - index) >= sourceCredentialSlotBytesCount ? new byte[sourceCredentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.sourceCredentialSlot[0] = data[index++];
                    if (data.Length > index) ret.sourceCredentialSlot[1] = data[index++];
                    ret.destinationUserUniqueIdentifier = (data.Length - index) >= destinationUserUniqueIdentifierBytesCount ? new byte[destinationUserUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.destinationUserUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.destinationUserUniqueIdentifier[1] = data[index++];
                    ret.destinationCredentialSlot = (data.Length - index) >= destinationCredentialSlotBytesCount ? new byte[destinationCredentialSlotBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.destinationCredentialSlot[0] = data[index++];
                    if (data.Length > index) ret.destinationCredentialSlot[1] = data[index++];
                    ret.userCredentialAssociationStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](USER_CREDENTIAL_ASSOCIATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.sourceUserUniqueIdentifier != null)
                {
                    foreach (var tmp in command.sourceUserUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sourceCredentialType.HasValue) ret.Add(command.sourceCredentialType);
                if (command.sourceCredentialSlot != null)
                {
                    foreach (var tmp in command.sourceCredentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.destinationUserUniqueIdentifier != null)
                {
                    foreach (var tmp in command.destinationUserUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.destinationCredentialSlot != null)
                {
                    foreach (var tmp in command.destinationCredentialSlot)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userCredentialAssociationStatus.HasValue) ret.Add(command.userCredentialAssociationStatus);
                return ret.ToArray();
            }
        }
        public partial class ALL_USERS_CHECKSUM_GET
        {
            public const byte ID = 0x14;
            public static implicit operator ALL_USERS_CHECKSUM_GET(byte[] data)
            {
                ALL_USERS_CHECKSUM_GET ret = new ALL_USERS_CHECKSUM_GET();
                return ret;
            }
            public static implicit operator byte[](ALL_USERS_CHECKSUM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ALL_USERS_CHECKSUM_REPORT
        {
            public const byte ID = 0x15;
            public const byte allUsersChecksumBytesCount = 2;
            public byte[] allUsersChecksum = new byte[allUsersChecksumBytesCount];
            public static implicit operator ALL_USERS_CHECKSUM_REPORT(byte[] data)
            {
                ALL_USERS_CHECKSUM_REPORT ret = new ALL_USERS_CHECKSUM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.allUsersChecksum = (data.Length - index) >= allUsersChecksumBytesCount ? new byte[allUsersChecksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.allUsersChecksum[0] = data[index++];
                    if (data.Length > index) ret.allUsersChecksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ALL_USERS_CHECKSUM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.allUsersChecksum != null)
                {
                    foreach (var tmp in command.allUsersChecksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CHECKSUM_GET
        {
            public const byte ID = 0x16;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public static implicit operator USER_CHECKSUM_GET(byte[] data)
            {
                USER_CHECKSUM_GET ret = new USER_CHECKSUM_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USER_CHECKSUM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CHECKSUM_REPORT
        {
            public const byte ID = 0x17;
            public const byte userUniqueIdentifierBytesCount = 2;
            public byte[] userUniqueIdentifier = new byte[userUniqueIdentifierBytesCount];
            public const byte userChecksumBytesCount = 2;
            public byte[] userChecksum = new byte[userChecksumBytesCount];
            public static implicit operator USER_CHECKSUM_REPORT(byte[] data)
            {
                USER_CHECKSUM_REPORT ret = new USER_CHECKSUM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userUniqueIdentifier = (data.Length - index) >= userUniqueIdentifierBytesCount ? new byte[userUniqueIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userUniqueIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userUniqueIdentifier[1] = data[index++];
                    ret.userChecksum = (data.Length - index) >= userChecksumBytesCount ? new byte[userChecksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userChecksum[0] = data[index++];
                    if (data.Length > index) ret.userChecksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USER_CHECKSUM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.userUniqueIdentifier != null)
                {
                    foreach (var tmp in command.userUniqueIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userChecksum != null)
                {
                    foreach (var tmp in command.userChecksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_CHECKSUM_GET
        {
            public const byte ID = 0x18;
            public ByteValue credentialType = 0;
            public static implicit operator CREDENTIAL_CHECKSUM_GET(byte[] data)
            {
                CREDENTIAL_CHECKSUM_GET ret = new CREDENTIAL_CHECKSUM_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_CHECKSUM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                return ret.ToArray();
            }
        }
        public partial class CREDENTIAL_CHECKSUM_REPORT
        {
            public const byte ID = 0x19;
            public ByteValue credentialType = 0;
            public const byte credentialChecksumBytesCount = 2;
            public byte[] credentialChecksum = new byte[credentialChecksumBytesCount];
            public static implicit operator CREDENTIAL_CHECKSUM_REPORT(byte[] data)
            {
                CREDENTIAL_CHECKSUM_REPORT ret = new CREDENTIAL_CHECKSUM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.credentialType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.credentialChecksum = (data.Length - index) >= credentialChecksumBytesCount ? new byte[credentialChecksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.credentialChecksum[0] = data[index++];
                    if (data.Length > index) ret.credentialChecksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CREDENTIAL_CHECKSUM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CREDENTIAL.ID);
                ret.Add(ID);
                if (command.credentialType.HasValue) ret.Add(command.credentialType);
                if (command.credentialChecksum != null)
                {
                    foreach (var tmp in command.credentialChecksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

