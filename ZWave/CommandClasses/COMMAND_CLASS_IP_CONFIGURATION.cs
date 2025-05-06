using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_IP_CONFIGURATION
    {
        public const byte ID = 0x9A;
        public const byte VERSION = 1;
        public partial class IP_CONFIGURATION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator IP_CONFIGURATION_GET(byte[] data)
            {
                IP_CONFIGURATION_GET ret = new IP_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](IP_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_CONFIGURATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IP_CONFIGURATION_RELEASE
        {
            public const byte ID = 0x04;
            public static implicit operator IP_CONFIGURATION_RELEASE(byte[] data)
            {
                IP_CONFIGURATION_RELEASE ret = new IP_CONFIGURATION_RELEASE();
                return ret;
            }
            public static implicit operator byte[](IP_CONFIGURATION_RELEASE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_CONFIGURATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IP_CONFIGURATION_RENEW
        {
            public const byte ID = 0x05;
            public static implicit operator IP_CONFIGURATION_RENEW(byte[] data)
            {
                IP_CONFIGURATION_RENEW ret = new IP_CONFIGURATION_RENEW();
                return ret;
            }
            public static implicit operator byte[](IP_CONFIGURATION_RENEW command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_CONFIGURATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IP_CONFIGURATION_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte autoDns
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte autoIp
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
            public const byte ipAddressBytesCount = 4;
            public byte[] ipAddress = new byte[ipAddressBytesCount];
            public const byte subnetMaskBytesCount = 4;
            public byte[] subnetMask = new byte[subnetMaskBytesCount];
            public const byte gatewayBytesCount = 4;
            public byte[] gateway = new byte[gatewayBytesCount];
            public const byte dns1BytesCount = 4;
            public byte[] dns1 = new byte[dns1BytesCount];
            public const byte dns2BytesCount = 4;
            public byte[] dns2 = new byte[dns2BytesCount];
            public const byte leasetimeBytesCount = 4;
            public byte[] leasetime = new byte[leasetimeBytesCount];
            public static implicit operator IP_CONFIGURATION_REPORT(byte[] data)
            {
                IP_CONFIGURATION_REPORT ret = new IP_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.ipAddress = (data.Length - index) >= ipAddressBytesCount ? new byte[ipAddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.ipAddress[0] = data[index++];
                    if (data.Length > index) ret.ipAddress[1] = data[index++];
                    if (data.Length > index) ret.ipAddress[2] = data[index++];
                    if (data.Length > index) ret.ipAddress[3] = data[index++];
                    ret.subnetMask = (data.Length - index) >= subnetMaskBytesCount ? new byte[subnetMaskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.subnetMask[0] = data[index++];
                    if (data.Length > index) ret.subnetMask[1] = data[index++];
                    if (data.Length > index) ret.subnetMask[2] = data[index++];
                    if (data.Length > index) ret.subnetMask[3] = data[index++];
                    ret.gateway = (data.Length - index) >= gatewayBytesCount ? new byte[gatewayBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.gateway[0] = data[index++];
                    if (data.Length > index) ret.gateway[1] = data[index++];
                    if (data.Length > index) ret.gateway[2] = data[index++];
                    if (data.Length > index) ret.gateway[3] = data[index++];
                    ret.dns1 = (data.Length - index) >= dns1BytesCount ? new byte[dns1BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dns1[0] = data[index++];
                    if (data.Length > index) ret.dns1[1] = data[index++];
                    if (data.Length > index) ret.dns1[2] = data[index++];
                    if (data.Length > index) ret.dns1[3] = data[index++];
                    ret.dns2 = (data.Length - index) >= dns2BytesCount ? new byte[dns2BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dns2[0] = data[index++];
                    if (data.Length > index) ret.dns2[1] = data[index++];
                    if (data.Length > index) ret.dns2[2] = data[index++];
                    if (data.Length > index) ret.dns2[3] = data[index++];
                    ret.leasetime = (data.Length - index) >= leasetimeBytesCount ? new byte[leasetimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.leasetime[0] = data[index++];
                    if (data.Length > index) ret.leasetime[1] = data[index++];
                    if (data.Length > index) ret.leasetime[2] = data[index++];
                    if (data.Length > index) ret.leasetime[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](IP_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_CONFIGURATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.ipAddress != null)
                {
                    foreach (var tmp in command.ipAddress)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.subnetMask != null)
                {
                    foreach (var tmp in command.subnetMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.gateway != null)
                {
                    foreach (var tmp in command.gateway)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dns1 != null)
                {
                    foreach (var tmp in command.dns1)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dns2 != null)
                {
                    foreach (var tmp in command.dns2)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.leasetime != null)
                {
                    foreach (var tmp in command.leasetime)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IP_CONFIGURATION_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte autoDns
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte autoIp
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
            public const byte ipAddressBytesCount = 4;
            public byte[] ipAddress = new byte[ipAddressBytesCount];
            public const byte subnetMaskBytesCount = 4;
            public byte[] subnetMask = new byte[subnetMaskBytesCount];
            public const byte gatewayBytesCount = 4;
            public byte[] gateway = new byte[gatewayBytesCount];
            public const byte dns1BytesCount = 4;
            public byte[] dns1 = new byte[dns1BytesCount];
            public const byte dns2BytesCount = 4;
            public byte[] dns2 = new byte[dns2BytesCount];
            public static implicit operator IP_CONFIGURATION_SET(byte[] data)
            {
                IP_CONFIGURATION_SET ret = new IP_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.ipAddress = (data.Length - index) >= ipAddressBytesCount ? new byte[ipAddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.ipAddress[0] = data[index++];
                    if (data.Length > index) ret.ipAddress[1] = data[index++];
                    if (data.Length > index) ret.ipAddress[2] = data[index++];
                    if (data.Length > index) ret.ipAddress[3] = data[index++];
                    ret.subnetMask = (data.Length - index) >= subnetMaskBytesCount ? new byte[subnetMaskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.subnetMask[0] = data[index++];
                    if (data.Length > index) ret.subnetMask[1] = data[index++];
                    if (data.Length > index) ret.subnetMask[2] = data[index++];
                    if (data.Length > index) ret.subnetMask[3] = data[index++];
                    ret.gateway = (data.Length - index) >= gatewayBytesCount ? new byte[gatewayBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.gateway[0] = data[index++];
                    if (data.Length > index) ret.gateway[1] = data[index++];
                    if (data.Length > index) ret.gateway[2] = data[index++];
                    if (data.Length > index) ret.gateway[3] = data[index++];
                    ret.dns1 = (data.Length - index) >= dns1BytesCount ? new byte[dns1BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dns1[0] = data[index++];
                    if (data.Length > index) ret.dns1[1] = data[index++];
                    if (data.Length > index) ret.dns1[2] = data[index++];
                    if (data.Length > index) ret.dns1[3] = data[index++];
                    ret.dns2 = (data.Length - index) >= dns2BytesCount ? new byte[dns2BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dns2[0] = data[index++];
                    if (data.Length > index) ret.dns2[1] = data[index++];
                    if (data.Length > index) ret.dns2[2] = data[index++];
                    if (data.Length > index) ret.dns2[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](IP_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_CONFIGURATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.ipAddress != null)
                {
                    foreach (var tmp in command.ipAddress)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.subnetMask != null)
                {
                    foreach (var tmp in command.subnetMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.gateway != null)
                {
                    foreach (var tmp in command.gateway)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dns1 != null)
                {
                    foreach (var tmp in command.dns1)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dns2 != null)
                {
                    foreach (var tmp in command.dns2)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

