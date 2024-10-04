/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4
    {
        public const byte ID = 0x7A;
        public const byte VERSION = 4;
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
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_MD_REPORT
        {
            public const byte ID = 0x02;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte firmware0IdBytesCount = 2;
            public byte[] firmware0Id = new byte[firmware0IdBytesCount];
            public const byte firmware0ChecksumBytesCount = 2;
            public byte[] firmware0Checksum = new byte[firmware0ChecksumBytesCount];
            public ByteValue firmwareUpgradable = 0;
            public ByteValue numberOfFirmwareTargets = 0;
            public const byte maxFragmentSizeBytesCount = 2;
            public byte[] maxFragmentSize = new byte[maxFragmentSizeBytesCount];
            public class TVG1
            {
                public const byte firmwareIdBytesCount = 2;
                public byte[] firmwareId = new byte[firmwareIdBytesCount];
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator FIRMWARE_MD_REPORT(byte[] data)
            {
                FIRMWARE_MD_REPORT ret = new FIRMWARE_MD_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.firmware0Id = (data.Length - index) >= firmware0IdBytesCount ? new byte[firmware0IdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.firmware0Id[0] = data[index++];
                    if (data.Length > index) ret.firmware0Id[1] = data[index++];
                    ret.firmware0Checksum = (data.Length - index) >= firmware0ChecksumBytesCount ? new byte[firmware0ChecksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.firmware0Checksum[0] = data[index++];
                    if (data.Length > index) ret.firmware0Checksum[1] = data[index++];
                    ret.firmwareUpgradable = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfFirmwareTargets = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.maxFragmentSize = (data.Length - index) >= maxFragmentSizeBytesCount ? new byte[maxFragmentSizeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maxFragmentSize[0] = data[index++];
                    if (data.Length > index) ret.maxFragmentSize[1] = data[index++];
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfFirmwareTargets; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.firmwareId = (data.Length - index) >= TVG1.firmwareIdBytesCount ? new byte[TVG1.firmwareIdBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.firmwareId[0] = data[index++];
                        if (data.Length > index) tmp.firmwareId[1] = data[index++];
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
                ret.Add(ID);
                if (command.manufacturerId != null)
                {
                    foreach (var tmp in command.manufacturerId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.firmware0Id != null)
                {
                    foreach (var tmp in command.firmware0Id)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.firmware0Checksum != null)
                {
                    foreach (var tmp in command.firmware0Checksum)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.firmwareUpgradable.HasValue) ret.Add(command.firmwareUpgradable);
                if (command.numberOfFirmwareTargets.HasValue) ret.Add(command.numberOfFirmwareTargets);
                if (command.maxFragmentSize != null)
                {
                    foreach (var tmp in command.maxFragmentSize)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.firmwareId != null)
                        {
                            foreach (var tmp in item.firmwareId)
                            {
                                ret.Add(tmp);
                            }
                        }
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
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
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
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator FIRMWARE_UPDATE_MD_REPORT(byte[] data)
            {
                FIRMWARE_UPDATE_MD_REPORT ret = new FIRMWARE_UPDATE_MD_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.reportNumber2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.data = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
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
        public partial class FIRMWARE_UPDATE_MD_REQUEST_GET
        {
            public const byte ID = 0x03;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte firmwareIdBytesCount = 2;
            public byte[] firmwareId = new byte[firmwareIdBytesCount];
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public ByteValue firmwareTarget = 0;
            public const byte fragmentSizeBytesCount = 2;
            public byte[] fragmentSize = new byte[fragmentSizeBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte activation
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
                    ret.firmwareTarget = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.fragmentSize = (data.Length - index) >= fragmentSizeBytesCount ? new byte[fragmentSizeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.fragmentSize[0] = data[index++];
                    if (data.Length > index) ret.fragmentSize[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_REQUEST_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
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
                if (command.firmwareTarget.HasValue) ret.Add(command.firmwareTarget);
                if (command.fragmentSize != null)
                {
                    foreach (var tmp in command.fragmentSize)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
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
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_UPDATE_MD_STATUS_REPORT
        {
            public const byte ID = 0x07;
            public ByteValue status = 0;
            public const byte waittimeBytesCount = 2;
            public byte[] waittime = new byte[waittimeBytesCount];
            public static implicit operator FIRMWARE_UPDATE_MD_STATUS_REPORT(byte[] data)
            {
                FIRMWARE_UPDATE_MD_STATUS_REPORT ret = new FIRMWARE_UPDATE_MD_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.waittime = (data.Length - index) >= waittimeBytesCount ? new byte[waittimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.waittime[0] = data[index++];
                    if (data.Length > index) ret.waittime[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_MD_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.waittime != null)
                {
                    foreach (var tmp in command.waittime)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_UPDATE_ACTIVATION_SET
        {
            public const byte ID = 0x08;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte firmwareIdBytesCount = 2;
            public byte[] firmwareId = new byte[firmwareIdBytesCount];
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public ByteValue firmwareTarget = 0;
            public static implicit operator FIRMWARE_UPDATE_ACTIVATION_SET(byte[] data)
            {
                FIRMWARE_UPDATE_ACTIVATION_SET ret = new FIRMWARE_UPDATE_ACTIVATION_SET();
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
                    ret.firmwareTarget = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_ACTIVATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
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
                if (command.firmwareTarget.HasValue) ret.Add(command.firmwareTarget);
                return ret.ToArray();
            }
        }
        public partial class FIRMWARE_UPDATE_ACTIVATION_STATUS_REPORT
        {
            public const byte ID = 0x09;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte firmwareIdBytesCount = 2;
            public byte[] firmwareId = new byte[firmwareIdBytesCount];
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public ByteValue firmwareTarget = 0;
            public ByteValue firmwareUpdateStatus = 0;
            public static implicit operator FIRMWARE_UPDATE_ACTIVATION_STATUS_REPORT(byte[] data)
            {
                FIRMWARE_UPDATE_ACTIVATION_STATUS_REPORT ret = new FIRMWARE_UPDATE_ACTIVATION_STATUS_REPORT();
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
                    ret.firmwareTarget = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.firmwareUpdateStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIRMWARE_UPDATE_ACTIVATION_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V4.ID);
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
                if (command.firmwareTarget.HasValue) ret.Add(command.firmwareTarget);
                if (command.firmwareUpdateStatus.HasValue) ret.Add(command.firmwareUpdateStatus);
                return ret.ToArray();
            }
        }
    }
}

