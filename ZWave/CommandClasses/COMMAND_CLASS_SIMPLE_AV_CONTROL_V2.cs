/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SIMPLE_AV_CONTROL_V2
    {
        public const byte ID = 0x94;
        public const byte VERSION = 2;
        public partial class SIMPLE_AV_CONTROL_GET
        {
            public const byte ID = 0x02;
            public static implicit operator SIMPLE_AV_CONTROL_GET(byte[] data)
            {
                SIMPLE_AV_CONTROL_GET ret = new SIMPLE_AV_CONTROL_GET();
                return ret;
            }
            public static implicit operator byte[](SIMPLE_AV_CONTROL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SIMPLE_AV_CONTROL_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SIMPLE_AV_CONTROL_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue numberOfReports = 0;
            public static implicit operator SIMPLE_AV_CONTROL_REPORT(byte[] data)
            {
                SIMPLE_AV_CONTROL_REPORT ret = new SIMPLE_AV_CONTROL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfReports = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SIMPLE_AV_CONTROL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SIMPLE_AV_CONTROL_V2.ID);
                ret.Add(ID);
                if (command.numberOfReports.HasValue) ret.Add(command.numberOfReports);
                return ret.ToArray();
            }
        }
        public partial class SIMPLE_AV_CONTROL_SET
        {
            public const byte ID = 0x01;
            public ByteValue sequenceNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte keyAttributes
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public const byte reserved2BytesCount = 2;
            public byte[] reserved2 = new byte[reserved2BytesCount];
            public class TVG
            {
                public const byte commandBytesCount = 2;
                public byte[] command = new byte[commandBytesCount];
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator SIMPLE_AV_CONTROL_SET(byte[] data)
            {
                SIMPLE_AV_CONTROL_SET ret = new SIMPLE_AV_CONTROL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.reserved2 = (data.Length - index) >= reserved2BytesCount ? new byte[reserved2BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.reserved2[0] = data[index++];
                    if (data.Length > index) ret.reserved2[1] = data[index++];
                    ret.vg = new List<TVG>();
                    while (data.Length - 0 > index)
                    {
                        TVG tmp = new TVG();
                        tmp.command = (data.Length - index) >= TVG.commandBytesCount ? new byte[TVG.commandBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.command[0] = data[index++];
                        if (data.Length > index) tmp.command[1] = data[index++];
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SIMPLE_AV_CONTROL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SIMPLE_AV_CONTROL_V2.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.reserved2 != null)
                {
                    foreach (var tmp in command.reserved2)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.command != null)
                        {
                            foreach (var tmp in item.command)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SIMPLE_AV_CONTROL_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public ByteValue reportNo = 0;
            public static implicit operator SIMPLE_AV_CONTROL_SUPPORTED_GET(byte[] data)
            {
                SIMPLE_AV_CONTROL_SUPPORTED_GET ret = new SIMPLE_AV_CONTROL_SUPPORTED_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.reportNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SIMPLE_AV_CONTROL_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SIMPLE_AV_CONTROL_V2.ID);
                ret.Add(ID);
                if (command.reportNo.HasValue) ret.Add(command.reportNo);
                return ret.ToArray();
            }
        }
        public partial class SIMPLE_AV_CONTROL_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue reportNo = 0;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator SIMPLE_AV_CONTROL_SUPPORTED_REPORT(byte[] data)
            {
                SIMPLE_AV_CONTROL_SUPPORTED_REPORT ret = new SIMPLE_AV_CONTROL_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SIMPLE_AV_CONTROL_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SIMPLE_AV_CONTROL_V2.ID);
                ret.Add(ID);
                if (command.reportNo.HasValue) ret.Add(command.reportNo);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

