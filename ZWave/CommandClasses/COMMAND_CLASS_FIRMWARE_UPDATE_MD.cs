using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_FIRMWARE_UPDATE_MD
    {
        public const byte ID = 0x7A;
        public const byte VERSION = 1;
        public partial class FIRMWARE_MD_GET
        {
            public const byte ID = 0x01;
            public static implicit operator FIRMWARE_MD_GET(byte[] data)
            {
                FIRMWARE_MD_GET ret = new FIRMWARE_MD_GET();
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_MD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_MD_REPORT
        {
            public const byte ID = 0x02;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte firmwareIdBytesCount = 2;
            public byte[] firmwareId = new byte[firmwareIdBytesCount];
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator FIRMWARE_MD_REPORT(byte[] data)
            {
                FIRMWARE_MD_REPORT ret = new FIRMWARE_MD_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.firmwareId = (data.Length - index) >= firmwareIdBytesCount ? new byte[firmwareIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.firmwareId[0] = data[index++];
                    if (data.Length > index) ret.firmwareId[1] = data[index++];
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                if (command.manufacturerId != null)
                {
                    foreach (var tmp in command.manufacturerId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.firmwareId != null)
                {
                    foreach (var tmp in command.firmwareId)
                    {
                        ret.Add(tmp);
                    }
                }
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
        public partial class FIRMWARE_UPDATE_MD_GET
        {
            public const byte ID = 0x05;
            public ByteValue numberOfReports = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportNumber1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte zero
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
            public ByteValue reportNumber2 = 0;
            public static implicit operator FIRMWARE_UPDATE_MD_GET(byte[] data)
            {
                FIRMWARE_UPDATE_MD_GET ret = new FIRMWARE_UPDATE_MD_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfReports = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.reportNumber2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                if (command.numberOfReports.HasValue) ret.Add(command.numberOfReports);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.reportNumber2.HasValue) ret.Add(command.reportNumber2);
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_UPDATE_MD_REPORT
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportNumber1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte last
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
            public ByteValue reportNumber2 = 0;
            public IList<byte> data = new List<byte>();
            public static implicit operator FIRMWARE_UPDATE_MD_REPORT(byte[] data)
            {
                FIRMWARE_UPDATE_MD_REPORT ret = new FIRMWARE_UPDATE_MD_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.reportNumber2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.data = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.reportNumber2.HasValue) ret.Add(command.reportNumber2);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_UPDATE_MD_REQUEST_GET
        {
            public const byte ID = 0x03;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte firmwareIdBytesCount = 2;
            public byte[] firmwareId = new byte[firmwareIdBytesCount];
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator FIRMWARE_UPDATE_MD_REQUEST_GET(byte[] data)
            {
                FIRMWARE_UPDATE_MD_REQUEST_GET ret = new FIRMWARE_UPDATE_MD_REQUEST_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.firmwareId = (data.Length - index) >= firmwareIdBytesCount ? new byte[firmwareIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.firmwareId[0] = data[index++];
                    if (data.Length > index) ret.firmwareId[1] = data[index++];
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_REQUEST_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                if (command.manufacturerId != null)
                {
                    foreach (var tmp in command.manufacturerId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.firmwareId != null)
                {
                    foreach (var tmp in command.firmwareId)
                    {
                        ret.Add(tmp);
                    }
                }
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
        public partial class FIRMWARE_UPDATE_MD_REQUEST_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue status = 0;
            public static implicit operator FIRMWARE_UPDATE_MD_REQUEST_REPORT(byte[] data)
            {
                FIRMWARE_UPDATE_MD_REQUEST_REPORT ret = new FIRMWARE_UPDATE_MD_REQUEST_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_REQUEST_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_UPDATE_MD_STATUS_REPORT
        {
            public const byte ID = 0x07;
            public ByteValue status = 0;
            public static implicit operator FIRMWARE_UPDATE_MD_STATUS_REPORT(byte[] data)
            {
                FIRMWARE_UPDATE_MD_STATUS_REPORT ret = new FIRMWARE_UPDATE_MD_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
    }
}

