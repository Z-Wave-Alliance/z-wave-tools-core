using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZIP_PORTAL
    {
        public const byte ID = 0x61;
        public const byte VERSION = 1;
        public partial class GATEWAY_CONFIGURATION_SET
        {
            public const byte ID = 0x01;
            public const byte lanIpv6AddressBytesCount = 16;
            public byte[] lanIpv6Address = new byte[lanIpv6AddressBytesCount];
            public ByteValue lanIpv6PrefixLength = 0;
            public const byte portalIpv6PrefixBytesCount = 16;
            public byte[] portalIpv6Prefix = new byte[portalIpv6PrefixBytesCount];
            public ByteValue portalIpv6PrefixLength = 0;
            public const byte defaultGatewayIpv6AddressBytesCount = 16;
            public byte[] defaultGatewayIpv6Address = new byte[defaultGatewayIpv6AddressBytesCount];
            public const byte panIpv6PrefixBytesCount = 16;
            public byte[] panIpv6Prefix = new byte[panIpv6PrefixBytesCount];
            public static implicit operator GATEWAY_CONFIGURATION_SET(byte[] data)
            {
                GATEWAY_CONFIGURATION_SET ret = new GATEWAY_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.lanIpv6Address = (data.Length - index) >= lanIpv6AddressBytesCount ? new byte[lanIpv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.lanIpv6Address[0] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[1] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[2] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[3] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[4] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[5] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[6] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[7] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[8] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[9] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[10] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[11] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[12] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[13] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[14] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[15] = data[index++];
                    ret.lanIpv6PrefixLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.portalIpv6Prefix = (data.Length - index) >= portalIpv6PrefixBytesCount ? new byte[portalIpv6PrefixBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.portalIpv6Prefix[0] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[1] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[2] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[3] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[4] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[5] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[6] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[7] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[8] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[9] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[10] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[11] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[12] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[13] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[14] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[15] = data[index++];
                    ret.portalIpv6PrefixLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.defaultGatewayIpv6Address = (data.Length - index) >= defaultGatewayIpv6AddressBytesCount ? new byte[defaultGatewayIpv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[0] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[1] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[2] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[3] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[4] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[5] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[6] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[7] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[8] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[9] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[10] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[11] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[12] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[13] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[14] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[15] = data[index++];
                    ret.panIpv6Prefix = (data.Length - index) >= panIpv6PrefixBytesCount ? new byte[panIpv6PrefixBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.panIpv6Prefix[0] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[1] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[2] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[3] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[4] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[5] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[6] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[7] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[8] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[9] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[10] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[11] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[12] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[13] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[14] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_PORTAL.ID);
                ret.Add(ID);
                if (command.lanIpv6Address != null)
                {
                    foreach (var tmp in command.lanIpv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.lanIpv6PrefixLength.HasValue) ret.Add(command.lanIpv6PrefixLength);
                if (command.portalIpv6Prefix != null)
                {
                    foreach (var tmp in command.portalIpv6Prefix)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.portalIpv6PrefixLength.HasValue) ret.Add(command.portalIpv6PrefixLength);
                if (command.defaultGatewayIpv6Address != null)
                {
                    foreach (var tmp in command.defaultGatewayIpv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.panIpv6Prefix != null)
                {
                    foreach (var tmp in command.panIpv6Prefix)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_CONFIGURATION_STATUS
        {
            public const byte ID = 0x02;
            public ByteValue status = 0;
            public static implicit operator GATEWAY_CONFIGURATION_STATUS(byte[] data)
            {
                GATEWAY_CONFIGURATION_STATUS ret = new GATEWAY_CONFIGURATION_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_CONFIGURATION_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_PORTAL.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_CONFIGURATION_GET
        {
            public const byte ID = 0x03;
            public static implicit operator GATEWAY_CONFIGURATION_GET(byte[] data)
            {
                GATEWAY_CONFIGURATION_GET ret = new GATEWAY_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](GATEWAY_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_PORTAL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_CONFIGURATION_REPORT
        {
            public const byte ID = 0x04;
            public const byte lanIpv6AddressBytesCount = 16;
            public byte[] lanIpv6Address = new byte[lanIpv6AddressBytesCount];
            public ByteValue lanIpv6PrefixLength = 0;
            public const byte portalIpv6PrefixBytesCount = 16;
            public byte[] portalIpv6Prefix = new byte[portalIpv6PrefixBytesCount];
            public ByteValue portalIpv6PrefixLength = 0;
            public const byte defaultGatewayIpv6AddressBytesCount = 16;
            public byte[] defaultGatewayIpv6Address = new byte[defaultGatewayIpv6AddressBytesCount];
            public const byte panIpv6PrefixBytesCount = 16;
            public byte[] panIpv6Prefix = new byte[panIpv6PrefixBytesCount];
            public static implicit operator GATEWAY_CONFIGURATION_REPORT(byte[] data)
            {
                GATEWAY_CONFIGURATION_REPORT ret = new GATEWAY_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.lanIpv6Address = (data.Length - index) >= lanIpv6AddressBytesCount ? new byte[lanIpv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.lanIpv6Address[0] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[1] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[2] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[3] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[4] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[5] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[6] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[7] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[8] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[9] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[10] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[11] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[12] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[13] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[14] = data[index++];
                    if (data.Length > index) ret.lanIpv6Address[15] = data[index++];
                    ret.lanIpv6PrefixLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.portalIpv6Prefix = (data.Length - index) >= portalIpv6PrefixBytesCount ? new byte[portalIpv6PrefixBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.portalIpv6Prefix[0] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[1] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[2] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[3] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[4] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[5] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[6] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[7] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[8] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[9] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[10] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[11] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[12] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[13] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[14] = data[index++];
                    if (data.Length > index) ret.portalIpv6Prefix[15] = data[index++];
                    ret.portalIpv6PrefixLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.defaultGatewayIpv6Address = (data.Length - index) >= defaultGatewayIpv6AddressBytesCount ? new byte[defaultGatewayIpv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[0] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[1] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[2] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[3] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[4] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[5] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[6] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[7] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[8] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[9] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[10] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[11] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[12] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[13] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[14] = data[index++];
                    if (data.Length > index) ret.defaultGatewayIpv6Address[15] = data[index++];
                    ret.panIpv6Prefix = (data.Length - index) >= panIpv6PrefixBytesCount ? new byte[panIpv6PrefixBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.panIpv6Prefix[0] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[1] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[2] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[3] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[4] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[5] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[6] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[7] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[8] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[9] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[10] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[11] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[12] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[13] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[14] = data[index++];
                    if (data.Length > index) ret.panIpv6Prefix[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_PORTAL.ID);
                ret.Add(ID);
                if (command.lanIpv6Address != null)
                {
                    foreach (var tmp in command.lanIpv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.lanIpv6PrefixLength.HasValue) ret.Add(command.lanIpv6PrefixLength);
                if (command.portalIpv6Prefix != null)
                {
                    foreach (var tmp in command.portalIpv6Prefix)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.portalIpv6PrefixLength.HasValue) ret.Add(command.portalIpv6PrefixLength);
                if (command.defaultGatewayIpv6Address != null)
                {
                    foreach (var tmp in command.defaultGatewayIpv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.panIpv6Prefix != null)
                {
                    foreach (var tmp in command.panIpv6Prefix)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_UNREGISTER
        {
            public const byte ID = 0x05;
            public static implicit operator GATEWAY_UNREGISTER(byte[] data)
            {
                GATEWAY_UNREGISTER ret = new GATEWAY_UNREGISTER();
                return ret;
            }
            public static implicit operator byte[](GATEWAY_UNREGISTER command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_PORTAL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

