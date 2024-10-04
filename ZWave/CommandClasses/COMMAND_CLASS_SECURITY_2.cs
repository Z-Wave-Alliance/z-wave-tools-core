/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SECURITY_2
    {
        public const byte ID = 0x9F;
        public const byte VERSION = 1;
        public partial class SECURITY_2_NONCE_GET
        {
            public const byte ID = 0x01;
            public ByteValue sequenceNumber = 0;
            public static implicit operator SECURITY_2_NONCE_GET(byte[] data)
            {
                SECURITY_2_NONCE_GET ret = new SECURITY_2_NONCE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_NONCE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_NONCE_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue sequenceNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte sos
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte mos
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public IList<byte> receiversEntropyInput = new List<byte>();
            public static implicit operator SECURITY_2_NONCE_REPORT(byte[] data)
            {
                SECURITY_2_NONCE_REPORT ret = new SECURITY_2_NONCE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    if (ret.properties1.sos > 0)
                    {
                        ret.receiversEntropyInput = new List<byte>();
                        while (data.Length - 0 > index)
                        {
                            if (data.Length > index) ret.receiversEntropyInput.Add(data[index++]);
                        }
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_NONCE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties1.sos > 0)
                {
                    if (command.receiversEntropyInput != null)
                    {
                        foreach (var tmp in command.receiversEntropyInput)
                        {
                            ret.Add(tmp);
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_MESSAGE_ENCAPSULATION
        {
            public const byte ID = 0x03;
            public ByteValue sequenceNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte extension
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte encryptedExtension
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public class TVG1
            {
                public ByteValue extensionLength = 0;
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte type
                    {
                        get { return (byte)(_value >> 0 & 0x3F); }
                        set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                    }
                    public byte critical
                    {
                        get { return (byte)(_value >> 6 & 0x01); }
                        set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                    }
                    public byte moreToFollow
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
                public IList<byte> extension = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public IList<byte> ccmCiphertextObject = new List<byte>();
            public static implicit operator SECURITY_2_MESSAGE_ENCAPSULATION(byte[] data)
            {
                SECURITY_2_MESSAGE_ENCAPSULATION ret = new SECURITY_2_MESSAGE_ENCAPSULATION();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    if (ret.properties1.extension > 0)
                    {
                        ret.vg1 = new List<TVG1>();
                        while (data.Length - 0 > index)
                        {
                            TVG1 tmp = new TVG1();
                            tmp.extensionLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                            tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                            tmp.extension = new List<byte>();
                            for (int i = 0; i < tmp.extensionLength - 2; i++)
                            {
                                if (data.Length > index) tmp.extension.Add(data[index++]);
                            }
                            ret.vg1.Add(tmp);
                            if (tmp.properties1.moreToFollow == 0)
                                break;
                        }
                    }
                    ret.ccmCiphertextObject = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.ccmCiphertextObject.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_MESSAGE_ENCAPSULATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.extensionLength.HasValue) ret.Add(item.extensionLength);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.extension != null)
                        {
                            foreach (var tmp in item.extension)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                if (command.ccmCiphertextObject != null)
                {
                    foreach (var tmp in command.ccmCiphertextObject)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class KEX_GET
        {
            public const byte ID = 0x04;
            public static implicit operator KEX_GET(byte[] data)
            {
                KEX_GET ret = new KEX_GET();
                return ret;
            }
            public static implicit operator byte[](KEX_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class KEX_REPORT
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte echo
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte requestCsa
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public ByteValue supportedKexSchemes = 0;
            public ByteValue supportedEcdhProfiles = 0;
            public ByteValue requestedKeys = 0;
            public static implicit operator KEX_REPORT(byte[] data)
            {
                KEX_REPORT ret = new KEX_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.supportedKexSchemes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.supportedEcdhProfiles = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.requestedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](KEX_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.supportedKexSchemes.HasValue) ret.Add(command.supportedKexSchemes);
                if (command.supportedEcdhProfiles.HasValue) ret.Add(command.supportedEcdhProfiles);
                if (command.requestedKeys.HasValue) ret.Add(command.requestedKeys);
                return ret.ToArray();
            }
        }
        public partial class KEX_SET
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte echo
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte requestCsa
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public ByteValue selectedKexScheme = 0;
            public ByteValue selectedEcdhProfile = 0;
            public ByteValue grantedKeys = 0;
            public static implicit operator KEX_SET(byte[] data)
            {
                KEX_SET ret = new KEX_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.selectedKexScheme = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.selectedEcdhProfile = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.grantedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](KEX_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.selectedKexScheme.HasValue) ret.Add(command.selectedKexScheme);
                if (command.selectedEcdhProfile.HasValue) ret.Add(command.selectedEcdhProfile);
                if (command.grantedKeys.HasValue) ret.Add(command.grantedKeys);
                return ret.ToArray();
            }
        }
        public partial class KEX_FAIL
        {
            public const byte ID = 0x07;
            public ByteValue kexFailType = 0;
            public static implicit operator KEX_FAIL(byte[] data)
            {
                KEX_FAIL ret = new KEX_FAIL();
                if (data != null)
                {
                    int index = 2;
                    ret.kexFailType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](KEX_FAIL command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.kexFailType.HasValue) ret.Add(command.kexFailType);
                return ret.ToArray();
            }
        }
        public partial class PUBLIC_KEY_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte includingNode
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
            public IList<byte> ecdhPublicKey = new List<byte>();
            public static implicit operator PUBLIC_KEY_REPORT(byte[] data)
            {
                PUBLIC_KEY_REPORT ret = new PUBLIC_KEY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.ecdhPublicKey = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.ecdhPublicKey.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](PUBLIC_KEY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.ecdhPublicKey != null)
                {
                    foreach (var tmp in command.ecdhPublicKey)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_NETWORK_KEY_GET
        {
            public const byte ID = 0x09;
            public ByteValue requestedKey = 0;
            public static implicit operator SECURITY_2_NETWORK_KEY_GET(byte[] data)
            {
                SECURITY_2_NETWORK_KEY_GET ret = new SECURITY_2_NETWORK_KEY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.requestedKey = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_NETWORK_KEY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.requestedKey.HasValue) ret.Add(command.requestedKey);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_NETWORK_KEY_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue grantedKey = 0;
            public const byte networkKeyBytesCount = 16;
            public byte[] networkKey = new byte[networkKeyBytesCount];
            public static implicit operator SECURITY_2_NETWORK_KEY_REPORT(byte[] data)
            {
                SECURITY_2_NETWORK_KEY_REPORT ret = new SECURITY_2_NETWORK_KEY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.grantedKey = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.networkKey = (data.Length - index) >= networkKeyBytesCount ? new byte[networkKeyBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.networkKey[0] = data[index++];
                    if (data.Length > index) ret.networkKey[1] = data[index++];
                    if (data.Length > index) ret.networkKey[2] = data[index++];
                    if (data.Length > index) ret.networkKey[3] = data[index++];
                    if (data.Length > index) ret.networkKey[4] = data[index++];
                    if (data.Length > index) ret.networkKey[5] = data[index++];
                    if (data.Length > index) ret.networkKey[6] = data[index++];
                    if (data.Length > index) ret.networkKey[7] = data[index++];
                    if (data.Length > index) ret.networkKey[8] = data[index++];
                    if (data.Length > index) ret.networkKey[9] = data[index++];
                    if (data.Length > index) ret.networkKey[10] = data[index++];
                    if (data.Length > index) ret.networkKey[11] = data[index++];
                    if (data.Length > index) ret.networkKey[12] = data[index++];
                    if (data.Length > index) ret.networkKey[13] = data[index++];
                    if (data.Length > index) ret.networkKey[14] = data[index++];
                    if (data.Length > index) ret.networkKey[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_NETWORK_KEY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.grantedKey.HasValue) ret.Add(command.grantedKey);
                if (command.networkKey != null)
                {
                    foreach (var tmp in command.networkKey)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_NETWORK_KEY_VERIFY
        {
            public const byte ID = 0x0B;
            public static implicit operator SECURITY_2_NETWORK_KEY_VERIFY(byte[] data)
            {
                SECURITY_2_NETWORK_KEY_VERIFY ret = new SECURITY_2_NETWORK_KEY_VERIFY();
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_NETWORK_KEY_VERIFY command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_TRANSFER_END
        {
            public const byte ID = 0x0C;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte keyRequestComplete
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte keyVerified
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public static implicit operator SECURITY_2_TRANSFER_END(byte[] data)
            {
                SECURITY_2_TRANSFER_END ret = new SECURITY_2_TRANSFER_END();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_TRANSFER_END command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_COMMANDS_SUPPORTED_GET
        {
            public const byte ID = 0x0D;
            public static implicit operator SECURITY_2_COMMANDS_SUPPORTED_GET(byte[] data)
            {
                SECURITY_2_COMMANDS_SUPPORTED_GET ret = new SECURITY_2_COMMANDS_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_COMMANDS_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_2_COMMANDS_SUPPORTED_REPORT
        {
            public const byte ID = 0x0E;
            public IList<byte> commandClass = new List<byte>();
            public static implicit operator SECURITY_2_COMMANDS_SUPPORTED_REPORT(byte[] data)
            {
                SECURITY_2_COMMANDS_SUPPORTED_REPORT ret = new SECURITY_2_COMMANDS_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.commandClass = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClass.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_2_COMMANDS_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_2.ID);
                ret.Add(ID);
                if (command.commandClass != null)
                {
                    foreach (var tmp in command.commandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

