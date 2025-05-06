using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_USER_CODE_V2
    {
        public const byte ID = 0x63;
        public const byte VERSION = 2;
        public partial class USER_CODE_GET
        {
            public const byte ID = 0x02;
            public ByteValue userIdentifier = 0;
            public static implicit operator USER_CODE_GET(byte[] data)
            {
                USER_CODE_GET ret = new USER_CODE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue userIdentifier = 0;
            public ByteValue userIdStatus = 0;
            public IList<byte> userCode = new List<byte>();
            public static implicit operator USER_CODE_REPORT(byte[] data)
            {
                USER_CODE_REPORT ret = new USER_CODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userCode = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.userCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.userIdStatus.HasValue) ret.Add(command.userIdStatus);
                if (command.userCode != null)
                {
                    foreach (var tmp in command.userCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_SET
        {
            public const byte ID = 0x01;
            public ByteValue userIdentifier = 0;
            public ByteValue userIdStatus = 0;
            public IList<byte> userCode = new List<byte>();
            public static implicit operator USER_CODE_SET(byte[] data)
            {
                USER_CODE_SET ret = new USER_CODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userCode = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.userCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.userIdStatus.HasValue) ret.Add(command.userIdStatus);
                if (command.userCode != null)
                {
                    foreach (var tmp in command.userCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USERS_NUMBER_GET
        {
            public const byte ID = 0x04;
            public static implicit operator USERS_NUMBER_GET(byte[] data)
            {
                USERS_NUMBER_GET ret = new USERS_NUMBER_GET();
                return ret;
            }
            public static implicit operator byte[](USERS_NUMBER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class USERS_NUMBER_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue supportedUsers = 0;
            public const byte extendedSupportedUsersBytesCount = 2;
            public byte[] extendedSupportedUsers = new byte[extendedSupportedUsersBytesCount];
            public static implicit operator USERS_NUMBER_REPORT(byte[] data)
            {
                USERS_NUMBER_REPORT ret = new USERS_NUMBER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedUsers = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.extendedSupportedUsers = (data.Length - index) >= extendedSupportedUsersBytesCount ? new byte[extendedSupportedUsersBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedSupportedUsers[0] = data[index++];
                    if (data.Length > index) ret.extendedSupportedUsers[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USERS_NUMBER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.supportedUsers.HasValue) ret.Add(command.supportedUsers);
                if (command.extendedSupportedUsers != null)
                {
                    foreach (var tmp in command.extendedSupportedUsers)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class EXTENDED_USER_CODE_SET
        {
            public const byte ID = 0x0B;
            public ByteValue numberOfUserCodes = 0;
            public class TVG1
            {
                public const byte userIdentifierBytesCount = 2;
                public byte[] userIdentifier = new byte[userIdentifierBytesCount];
                public ByteValue userIdStatus = 0;
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte userCodeLength
                    {
                        get { return (byte)(_value >> 0 & 0x0F); }
                        set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                    }
                    public byte reserved
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
                public IList<byte> userCode = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator EXTENDED_USER_CODE_SET(byte[] data)
            {
                EXTENDED_USER_CODE_SET ret = new EXTENDED_USER_CODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfUserCodes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfUserCodes; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.userIdentifier = (data.Length - index) >= TVG1.userIdentifierBytesCount ? new byte[TVG1.userIdentifierBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.userIdentifier[0] = data[index++];
                        if (data.Length > index) tmp.userIdentifier[1] = data[index++];
                        tmp.userIdStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.userCode = new List<byte>();
                        for (int i = 0; i < tmp.properties1.userCodeLength; i++)
                        {
                            if (data.Length > index) tmp.userCode.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](EXTENDED_USER_CODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.numberOfUserCodes.HasValue) ret.Add(command.numberOfUserCodes);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.userIdentifier != null)
                        {
                            foreach (var tmp in item.userIdentifier)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.userIdStatus.HasValue) ret.Add(item.userIdStatus);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.userCode != null)
                        {
                            foreach (var tmp in item.userCode)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class EXTENDED_USER_CODE_GET
        {
            public const byte ID = 0x0C;
            public const byte userIdentifierBytesCount = 2;
            public byte[] userIdentifier = new byte[userIdentifierBytesCount];
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
            public static implicit operator EXTENDED_USER_CODE_GET(byte[] data)
            {
                EXTENDED_USER_CODE_GET ret = new EXTENDED_USER_CODE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = (data.Length - index) >= userIdentifierBytesCount ? new byte[userIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userIdentifier[0] = data[index++];
                    if (data.Length > index) ret.userIdentifier[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](EXTENDED_USER_CODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.userIdentifier != null)
                {
                    foreach (var tmp in command.userIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class EXTENDED_USER_CODE_REPORT
        {
            public const byte ID = 0x0D;
            public ByteValue numberOfUserCodes = 0;
            public class TVG1
            {
                public const byte userIdentifierBytesCount = 2;
                public byte[] userIdentifier = new byte[userIdentifierBytesCount];
                public ByteValue userIdStatus = 0;
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte userCodeLength
                    {
                        get { return (byte)(_value >> 0 & 0x0F); }
                        set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                    }
                    public byte reserved
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
                public IList<byte> userCode = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public const byte nextUserIdentifierBytesCount = 2;
            public byte[] nextUserIdentifier = new byte[nextUserIdentifierBytesCount];
            public static implicit operator EXTENDED_USER_CODE_REPORT(byte[] data)
            {
                EXTENDED_USER_CODE_REPORT ret = new EXTENDED_USER_CODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfUserCodes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.numberOfUserCodes > 0)
                    {
                        ret.vg1 = new List<TVG1>();
                        for (int j = 0; j < ret.numberOfUserCodes; j++)
                        {
                            TVG1 tmp = new TVG1();
                            tmp.userIdentifier = (data.Length - index) >= TVG1.userIdentifierBytesCount ? new byte[TVG1.userIdentifierBytesCount] : new byte[data.Length - index];
                            if (data.Length > index) tmp.userIdentifier[0] = data[index++];
                            if (data.Length > index) tmp.userIdentifier[1] = data[index++];
                            tmp.userIdStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                            tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                            tmp.userCode = new List<byte>();
                            for (int i = 0; i < tmp.properties1.userCodeLength; i++)
                            {
                                if (data.Length > index) tmp.userCode.Add(data[index++]);
                            }
                            ret.vg1.Add(tmp);
                        }
                    }
                    ret.nextUserIdentifier = (data.Length - index) >= nextUserIdentifierBytesCount ? new byte[nextUserIdentifierBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextUserIdentifier[0] = data[index++];
                    if (data.Length > index) ret.nextUserIdentifier[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](EXTENDED_USER_CODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.numberOfUserCodes.HasValue) ret.Add(command.numberOfUserCodes);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.userIdentifier != null)
                        {
                            foreach (var tmp in item.userIdentifier)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.userIdStatus.HasValue) ret.Add(item.userIdStatus);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.userCode != null)
                        {
                            foreach (var tmp in item.userCode)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                if (command.nextUserIdentifier != null)
                {
                    foreach (var tmp in command.nextUserIdentifier)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_CAPABILITIES_GET
        {
            public const byte ID = 0x06;
            public static implicit operator USER_CODE_CAPABILITIES_GET(byte[] data)
            {
                USER_CODE_CAPABILITIES_GET ret = new USER_CODE_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](USER_CODE_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_CAPABILITIES_REPORT
        {
            public const byte ID = 0x07;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte supportedUserIdStatusBitMaskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte acdSupport
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte acSupport
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
            public IList<byte> supportedUserIdStatusBitMask = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte supportedKeypadModesBitMaskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte mucsSupport
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte mucrSupport
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte uccSupport
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public IList<byte> supportedKeypadModesBitMask = new List<byte>();
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte supportedKeysBitMaskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> supportedKeysBitMask = new List<byte>();
            public static implicit operator USER_CODE_CAPABILITIES_REPORT(byte[] data)
            {
                USER_CODE_CAPABILITIES_REPORT ret = new USER_CODE_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.supportedUserIdStatusBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.supportedUserIdStatusBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.supportedUserIdStatusBitMask.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.supportedKeypadModesBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties2.supportedKeypadModesBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.supportedKeypadModesBitMask.Add(data[index++]);
                    }
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.supportedKeysBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties3.supportedKeysBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.supportedKeysBitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.supportedUserIdStatusBitMask != null)
                {
                    foreach (var tmp in command.supportedUserIdStatusBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.supportedKeypadModesBitMask != null)
                {
                    foreach (var tmp in command.supportedKeypadModesBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.supportedKeysBitMask != null)
                {
                    foreach (var tmp in command.supportedKeysBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_KEYPAD_MODE_SET
        {
            public const byte ID = 0x08;
            public ByteValue keypadMode = 0;
            public static implicit operator USER_CODE_KEYPAD_MODE_SET(byte[] data)
            {
                USER_CODE_KEYPAD_MODE_SET ret = new USER_CODE_KEYPAD_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.keypadMode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_KEYPAD_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.keypadMode.HasValue) ret.Add(command.keypadMode);
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_KEYPAD_MODE_GET
        {
            public const byte ID = 0x09;
            public static implicit operator USER_CODE_KEYPAD_MODE_GET(byte[] data)
            {
                USER_CODE_KEYPAD_MODE_GET ret = new USER_CODE_KEYPAD_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](USER_CODE_KEYPAD_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_KEYPAD_MODE_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue keypadMode = 0;
            public static implicit operator USER_CODE_KEYPAD_MODE_REPORT(byte[] data)
            {
                USER_CODE_KEYPAD_MODE_REPORT ret = new USER_CODE_KEYPAD_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.keypadMode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_KEYPAD_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.keypadMode.HasValue) ret.Add(command.keypadMode);
                return ret.ToArray();
            }
        }
        public partial class ADMIN_CODE_SET
        {
            public const byte ID = 0x0E;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte adminCodeLength
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
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
            public IList<byte> adminCode = new List<byte>();
            public static implicit operator ADMIN_CODE_SET(byte[] data)
            {
                ADMIN_CODE_SET ret = new ADMIN_CODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.adminCode = new List<byte>();
                    for (int i = 0; i < ret.properties1.adminCodeLength; i++)
                    {
                        if (data.Length > index) ret.adminCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ADMIN_CODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.adminCode != null)
                {
                    foreach (var tmp in command.adminCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ADMIN_CODE_GET
        {
            public const byte ID = 0x0F;
            public static implicit operator ADMIN_CODE_GET(byte[] data)
            {
                ADMIN_CODE_GET ret = new ADMIN_CODE_GET();
                return ret;
            }
            public static implicit operator byte[](ADMIN_CODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ADMIN_CODE_REPORT
        {
            public const byte ID = 0x10;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte adminCodeLength
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
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
            public IList<byte> adminCode = new List<byte>();
            public static implicit operator ADMIN_CODE_REPORT(byte[] data)
            {
                ADMIN_CODE_REPORT ret = new ADMIN_CODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.adminCode = new List<byte>();
                    for (int i = 0; i < ret.properties1.adminCodeLength; i++)
                    {
                        if (data.Length > index) ret.adminCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ADMIN_CODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.adminCode != null)
                {
                    foreach (var tmp in command.adminCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_CHECKSUM_GET
        {
            public const byte ID = 0x11;
            public static implicit operator USER_CODE_CHECKSUM_GET(byte[] data)
            {
                USER_CODE_CHECKSUM_GET ret = new USER_CODE_CHECKSUM_GET();
                return ret;
            }
            public static implicit operator byte[](USER_CODE_CHECKSUM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_CHECKSUM_REPORT
        {
            public const byte ID = 0x12;
            public const byte userCodeChecksumBytesCount = 2;
            public byte[] userCodeChecksum = new byte[userCodeChecksumBytesCount];
            public static implicit operator USER_CODE_CHECKSUM_REPORT(byte[] data)
            {
                USER_CODE_CHECKSUM_REPORT ret = new USER_CODE_CHECKSUM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userCodeChecksum = (data.Length - index) >= userCodeChecksumBytesCount ? new byte[userCodeChecksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userCodeChecksum[0] = data[index++];
                    if (data.Length > index) ret.userCodeChecksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_CHECKSUM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE_V2.ID);
                ret.Add(ID);
                if (command.userCodeChecksum != null)
                {
                    foreach (var tmp in command.userCodeChecksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

