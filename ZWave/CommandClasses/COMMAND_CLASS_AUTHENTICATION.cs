/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_AUTHENTICATION
    {
        public const byte ID = 0xA1;
        public const byte VERSION = 1;
        public partial class AUTHENTICATION_CAPABILITY_GET
        {
            public const byte ID = 0x01;
            public static implicit operator AUTHENTICATION_CAPABILITY_GET(byte[] data)
            {
                AUTHENTICATION_CAPABILITY_GET ret = new AUTHENTICATION_CAPABILITY_GET();
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_CAPABILITY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_CAPABILITY_REPORT
        {
            public const byte ID = 0x02;
            public const byte supportedDataIdEntriesBytesCount = 2;
            public byte[] supportedDataIdEntries = new byte[supportedDataIdEntriesBytesCount];
            public const byte supportedAuthenticationIdEntriesBytesCount = 2;
            public byte[] supportedAuthenticationIdEntries = new byte[supportedAuthenticationIdEntriesBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte supportedAuthenticationTechnologyTypeBitMaskLength
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte or
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte madr
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte mar
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
            public IList<byte> supportedAuthenticationTechnologyTypeBitMask = new List<byte>();
            public ByteValue supportedChecksumTypeBitMask = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte supportedFallbackStatusBitMaskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> supportedFallbackStatusBitMask = new List<byte>();
            public static implicit operator AUTHENTICATION_CAPABILITY_REPORT(byte[] data)
            {
                AUTHENTICATION_CAPABILITY_REPORT ret = new AUTHENTICATION_CAPABILITY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedDataIdEntries = (data.Length - index) >= supportedDataIdEntriesBytesCount ? new byte[supportedDataIdEntriesBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.supportedDataIdEntries[0] = data[index++];
                    if (data.Length > index) ret.supportedDataIdEntries[1] = data[index++];
                    ret.supportedAuthenticationIdEntries = (data.Length - index) >= supportedAuthenticationIdEntriesBytesCount ? new byte[supportedAuthenticationIdEntriesBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.supportedAuthenticationIdEntries[0] = data[index++];
                    if (data.Length > index) ret.supportedAuthenticationIdEntries[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.supportedAuthenticationTechnologyTypeBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.supportedAuthenticationTechnologyTypeBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.supportedAuthenticationTechnologyTypeBitMask.Add(data[index++]);
                    }
                    ret.supportedChecksumTypeBitMask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.supportedFallbackStatusBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties2.supportedFallbackStatusBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.supportedFallbackStatusBitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_CAPABILITY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.supportedDataIdEntries != null)
                {
                    foreach (var tmp in command.supportedDataIdEntries)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.supportedAuthenticationIdEntries != null)
                {
                    foreach (var tmp in command.supportedAuthenticationIdEntries)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.supportedAuthenticationTechnologyTypeBitMask != null)
                {
                    foreach (var tmp in command.supportedAuthenticationTechnologyTypeBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.supportedChecksumTypeBitMask.HasValue) ret.Add(command.supportedChecksumTypeBitMask);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.supportedFallbackStatusBitMask != null)
                {
                    foreach (var tmp in command.supportedFallbackStatusBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_DATA_SET
        {
            public const byte ID = 0x03;
            public const byte authenticationDataIdBytesCount = 2;
            public byte[] authenticationDataId = new byte[authenticationDataIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte authenticationTechnologyType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public ByteValue authenticationDataLength = 0;
            public IList<byte> authenticationData = new List<byte>();
            public static implicit operator AUTHENTICATION_DATA_SET(byte[] data)
            {
                AUTHENTICATION_DATA_SET ret = new AUTHENTICATION_DATA_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.authenticationDataId = (data.Length - index) >= authenticationDataIdBytesCount ? new byte[authenticationDataIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.authenticationDataId[0] = data[index++];
                    if (data.Length > index) ret.authenticationDataId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.authenticationDataLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.authenticationData = new List<byte>();
                    for (int i = 0; i < ret.authenticationDataLength; i++)
                    {
                        if (data.Length > index) ret.authenticationData.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_DATA_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.authenticationDataId != null)
                {
                    foreach (var tmp in command.authenticationDataId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.authenticationDataLength.HasValue) ret.Add(command.authenticationDataLength);
                if (command.authenticationData != null)
                {
                    foreach (var tmp in command.authenticationData)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_DATA_GET
        {
            public const byte ID = 0x04;
            public const byte authenticationDataIdBytesCount = 2;
            public byte[] authenticationDataId = new byte[authenticationDataIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportMore
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved1
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
            public static implicit operator AUTHENTICATION_DATA_GET(byte[] data)
            {
                AUTHENTICATION_DATA_GET ret = new AUTHENTICATION_DATA_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.authenticationDataId = (data.Length - index) >= authenticationDataIdBytesCount ? new byte[authenticationDataIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.authenticationDataId[0] = data[index++];
                    if (data.Length > index) ret.authenticationDataId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_DATA_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.authenticationDataId != null)
                {
                    foreach (var tmp in command.authenticationDataId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_DATA_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue numberOfAuthenticationDataIdBlocks = 0;
            public class TVG1
            {
                public const byte authenticationDataIdBytesCount = 2;
                public byte[] authenticationDataId = new byte[authenticationDataIdBytesCount];
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte authenticationTechnologyType
                    {
                        get { return (byte)(_value >> 0 & 0x0F); }
                        set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                    }
                    public byte reserved1
                    {
                        get { return (byte)(_value >> 4 & 0x0F); }
                        set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
                public ByteValue authenticationDataLength = 0;
                public IList<byte> authenticationData = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public const byte nextAuthenticationDataIdBytesCount = 2;
            public byte[] nextAuthenticationDataId = new byte[nextAuthenticationDataIdBytesCount];
            public static implicit operator AUTHENTICATION_DATA_REPORT(byte[] data)
            {
                AUTHENTICATION_DATA_REPORT ret = new AUTHENTICATION_DATA_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfAuthenticationDataIdBlocks = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfAuthenticationDataIdBlocks; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.authenticationDataId = (data.Length - index) >= TVG1.authenticationDataIdBytesCount ? new byte[TVG1.authenticationDataIdBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.authenticationDataId[0] = data[index++];
                        if (data.Length > index) tmp.authenticationDataId[1] = data[index++];
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.authenticationDataLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.authenticationData = new List<byte>();
                        for (int i = 0; i < tmp.authenticationDataLength; i++)
                        {
                            if (data.Length > index) tmp.authenticationData.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                    ret.nextAuthenticationDataId = (data.Length - index) >= nextAuthenticationDataIdBytesCount ? new byte[nextAuthenticationDataIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextAuthenticationDataId[0] = data[index++];
                    if (data.Length > index) ret.nextAuthenticationDataId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_DATA_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.numberOfAuthenticationDataIdBlocks.HasValue) ret.Add(command.numberOfAuthenticationDataIdBlocks);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.authenticationDataId != null)
                        {
                            foreach (var tmp in item.authenticationDataId)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.authenticationDataLength.HasValue) ret.Add(item.authenticationDataLength);
                        if (item.authenticationData != null)
                        {
                            foreach (var tmp in item.authenticationData)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                if (command.nextAuthenticationDataId != null)
                {
                    foreach (var tmp in command.nextAuthenticationDataId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_TECHNOLOGIES_COMBINATION_SET
        {
            public const byte ID = 0x06;
            public const byte authenticationIdBytesCount = 2;
            public byte[] authenticationId = new byte[authenticationIdBytesCount];
            public ByteValue fallbackStatus = 0;
            public const byte userIdentifierBytesCount = 2;
            public byte[] userIdentifier = new byte[userIdentifierBytesCount];
            public const byte scheduleIdBytesCount = 2;
            public byte[] scheduleId = new byte[scheduleIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfAuthenticationDataIds
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte orLogic
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
            public class TVG1
            {
                public const byte authenticationDataIdBytesCount = 2;
                public byte[] authenticationDataId = new byte[authenticationDataIdBytesCount];
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator AUTHENTICATION_TECHNOLOGIES_COMBINATION_SET(byte[] data)
            {
                AUTHENTICATION_TECHNOLOGIES_COMBINATION_SET ret = new AUTHENTICATION_TECHNOLOGIES_COMBINATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.authenticationId = (data.Length - index) >= authenticationIdBytesCount ? new byte[authenticationIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.authenticationId[0] = data[index++];
                    if (data.Length > index) ret.authenticationId[1] = data[index++];
                    ret.fallbackStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdentifier = (data.Length - index) >= userIdentifierBytesCount ? new byte[userIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userIdentifier[1] = data[index++];
                    ret.scheduleId = (data.Length - index) >= scheduleIdBytesCount ? new byte[scheduleIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleId[0] = data[index++];
                    if (data.Length > index) ret.scheduleId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.numberOfAuthenticationDataIds; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.authenticationDataId = (data.Length - index) >= TVG1.authenticationDataIdBytesCount ? new byte[TVG1.authenticationDataIdBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.authenticationDataId[0] = data[index++];
                        if (data.Length > index) tmp.authenticationDataId[1] = data[index++];
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_TECHNOLOGIES_COMBINATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.authenticationId != null)
                {
                    foreach (var tmp in command.authenticationId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.fallbackStatus.HasValue) ret.Add(command.fallbackStatus);
                if (command.userIdentifier != null)
                {
                    foreach (var tmp in command.userIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleId != null)
                {
                    foreach (var tmp in command.scheduleId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.authenticationDataId != null)
                        {
                            foreach (var tmp in item.authenticationDataId)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_TECHNOLOGIES_COMBINATION_GET
        {
            public const byte ID = 0x07;
            public const byte authenticationIdBytesCount = 2;
            public byte[] authenticationId = new byte[authenticationIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportMore
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved1
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
            public static implicit operator AUTHENTICATION_TECHNOLOGIES_COMBINATION_GET(byte[] data)
            {
                AUTHENTICATION_TECHNOLOGIES_COMBINATION_GET ret = new AUTHENTICATION_TECHNOLOGIES_COMBINATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.authenticationId = (data.Length - index) >= authenticationIdBytesCount ? new byte[authenticationIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.authenticationId[0] = data[index++];
                    if (data.Length > index) ret.authenticationId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_TECHNOLOGIES_COMBINATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.authenticationId != null)
                {
                    foreach (var tmp in command.authenticationId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_TECHNOLOGIES_COMBINATION_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue numberOfAuthenticationIdBlocks = 0;
            public class TVG1
            {
                public ByteValue authenticationIdBlockLength = 0;
                public const byte authenticationIdBytesCount = 2;
                public byte[] authenticationId = new byte[authenticationIdBytesCount];
                public ByteValue fallbackStatus = 0;
                public const byte userIdentifierBytesCount = 2;
                public byte[] userIdentifier = new byte[userIdentifierBytesCount];
                public const byte scheduleIdBytesCount = 2;
                public byte[] scheduleId = new byte[scheduleIdBytesCount];
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte numberOfAuthenticationDataIds
                    {
                        get { return (byte)(_value >> 0 & 0x7F); }
                        set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                    }
                    public byte orLogic
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
                public struct Tvg2
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tvg2 Empty { get { return new Tvg2() { _value = 0, HasValue = false }; } }
                    public byte authenticationDataId
                    {
                        get { return (byte)(_value >> 0 & 0x00); }
                        set { HasValue = true; _value &= 0xFF - 0x00; _value += (byte)(value << 0 & 0x00); }
                    }
                    public static implicit operator Tvg2(byte data)
                    {
                        Tvg2 ret = new Tvg2();
                        ret._value = data;
                        ret.HasValue = true;
                        return ret;
                    }
                    public static implicit operator byte(Tvg2 prm)
                    {
                        return prm._value;
                    }
                }
                public Tvg2 vg2 = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public const byte nextAuthenticationIdBytesCount = 2;
            public byte[] nextAuthenticationId = new byte[nextAuthenticationIdBytesCount];
            public static implicit operator AUTHENTICATION_TECHNOLOGIES_COMBINATION_REPORT(byte[] data)
            {
                AUTHENTICATION_TECHNOLOGIES_COMBINATION_REPORT ret = new AUTHENTICATION_TECHNOLOGIES_COMBINATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfAuthenticationIdBlocks = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfAuthenticationIdBlocks; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.authenticationIdBlockLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.authenticationId = (data.Length - index) >= TVG1.authenticationIdBytesCount ? new byte[TVG1.authenticationIdBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.authenticationId[0] = data[index++];
                        if (data.Length > index) tmp.authenticationId[1] = data[index++];
                        tmp.fallbackStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.userIdentifier = (data.Length - index) >= TVG1.userIdentifierBytesCount ? new byte[TVG1.userIdentifierBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.userIdentifier[0] = data[index++];
                        if (data.Length > index) tmp.userIdentifier[1] = data[index++];
                        tmp.scheduleId = (data.Length - index) >= TVG1.scheduleIdBytesCount ? new byte[TVG1.scheduleIdBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.scheduleId[0] = data[index++];
                        if (data.Length > index) tmp.scheduleId[1] = data[index++];
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.vg2 = data.Length > index ? (TVG1.Tvg2)data[index++] : TVG1.Tvg2.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.nextAuthenticationId = (data.Length - index) >= nextAuthenticationIdBytesCount ? new byte[nextAuthenticationIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextAuthenticationId[0] = data[index++];
                    if (data.Length > index) ret.nextAuthenticationId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_TECHNOLOGIES_COMBINATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.numberOfAuthenticationIdBlocks.HasValue) ret.Add(command.numberOfAuthenticationIdBlocks);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.authenticationIdBlockLength.HasValue) ret.Add(item.authenticationIdBlockLength);
                        if (item.authenticationId != null)
                        {
                            foreach (var tmp in item.authenticationId)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.fallbackStatus.HasValue) ret.Add(item.fallbackStatus);
                        if (item.userIdentifier != null)
                        {
                            foreach (var tmp in item.userIdentifier)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.scheduleId != null)
                        {
                            foreach (var tmp in item.scheduleId)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.vg2.HasValue) ret.Add(item.vg2);
                    }
                }
                if (command.nextAuthenticationId != null)
                {
                    foreach (var tmp in command.nextAuthenticationId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_CHECKSUM_GET
        {
            public const byte ID = 0x09;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte authenticationTechnologyType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte checksumType
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte reserved1
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
            public static implicit operator AUTHENTICATION_CHECKSUM_GET(byte[] data)
            {
                AUTHENTICATION_CHECKSUM_GET ret = new AUTHENTICATION_CHECKSUM_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_CHECKSUM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_CHECKSUM_REPORT
        {
            public const byte ID = 0x0A;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte authenticationTechnologyType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte checksumType
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte reserved1
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
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator AUTHENTICATION_CHECKSUM_REPORT(byte[] data)
            {
                AUTHENTICATION_CHECKSUM_REPORT ret = new AUTHENTICATION_CHECKSUM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_CHECKSUM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.checksum != null)
                {
                    foreach (var tmp in command.checksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

