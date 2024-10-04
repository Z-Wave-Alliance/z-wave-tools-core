/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4
    {
        public const byte ID = 0x67;
        public const byte VERSION = 4;
        public partial class PRIORITY_ROUTE_SET
        {
            public const byte ID = 0x01;
            public ByteValue nodeid = 0;
            public ByteValue repeater1 = 0;
            public ByteValue repeater2 = 0;
            public ByteValue repeater3 = 0;
            public ByteValue repeater4 = 0;
            public ByteValue speed = 0;
            public static implicit operator PRIORITY_ROUTE_SET(byte[] data)
            {
                PRIORITY_ROUTE_SET ret = new PRIORITY_ROUTE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater1 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater3 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater4 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.speed = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PRIORITY_ROUTE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.repeater1.HasValue) ret.Add(command.repeater1);
                if (command.repeater2.HasValue) ret.Add(command.repeater2);
                if (command.repeater3.HasValue) ret.Add(command.repeater3);
                if (command.repeater4.HasValue) ret.Add(command.repeater4);
                if (command.speed.HasValue) ret.Add(command.speed);
                return ret.ToArray();
            }
        }
        public partial class PRIORITY_ROUTE_GET
        {
            public const byte ID = 0x02;
            public ByteValue nodeid = 0;
            public static implicit operator PRIORITY_ROUTE_GET(byte[] data)
            {
                PRIORITY_ROUTE_GET ret = new PRIORITY_ROUTE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PRIORITY_ROUTE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                return ret.ToArray();
            }
        }
        public partial class PRIORITY_ROUTE_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue nodeid = 0;
            public ByteValue type = 0;
            public ByteValue repeater1 = 0;
            public ByteValue repeater2 = 0;
            public ByteValue repeater3 = 0;
            public ByteValue repeater4 = 0;
            public ByteValue speed = 0;
            public static implicit operator PRIORITY_ROUTE_REPORT(byte[] data)
            {
                PRIORITY_ROUTE_REPORT ret = new PRIORITY_ROUTE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.type = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater1 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater3 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.repeater4 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.speed = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PRIORITY_ROUTE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.type.HasValue) ret.Add(command.type);
                if (command.repeater1.HasValue) ret.Add(command.repeater1);
                if (command.repeater2.HasValue) ret.Add(command.repeater2);
                if (command.repeater3.HasValue) ret.Add(command.repeater3);
                if (command.repeater4.HasValue) ret.Add(command.repeater4);
                if (command.speed.HasValue) ret.Add(command.speed);
                return ret.ToArray();
            }
        }
        public partial class STATISTICS_GET
        {
            public const byte ID = 0x04;
            public ByteValue nodeid = 0;
            public static implicit operator STATISTICS_GET(byte[] data)
            {
                STATISTICS_GET ret = new STATISTICS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](STATISTICS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                return ret.ToArray();
            }
        }
        public partial class STATISTICS_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue nodeid = 0;
            public class TSTATISTICS
            {
                public ByteValue type = 0;
                public ByteValue length = 0;
                public IList<byte> value = new List<byte>();
            }
            public List<TSTATISTICS> statistics = new List<TSTATISTICS>();
            public static implicit operator STATISTICS_REPORT(byte[] data)
            {
                STATISTICS_REPORT ret = new STATISTICS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.statistics = new List<TSTATISTICS>();
                    while (data.Length - 0 > index)
                    {
                        TSTATISTICS tmp = new TSTATISTICS();
                        tmp.type = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.length = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = new List<byte>();
                        for (int i = 0; i < tmp.length; i++)
                        {
                            if (data.Length > index) tmp.value.Add(data[index++]);
                        }
                        ret.statistics.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](STATISTICS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.statistics != null)
                {
                    foreach (var item in command.statistics)
                    {
                        if (item.type.HasValue) ret.Add(item.type);
                        if (item.length.HasValue) ret.Add(item.length);
                        if (item.value != null)
                        {
                            foreach (var tmp in item.value)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class STATISTICS_CLEAR
        {
            public const byte ID = 0x06;
            public static implicit operator STATISTICS_CLEAR(byte[] data)
            {
                STATISTICS_CLEAR ret = new STATISTICS_CLEAR();
                return ret;
            }
            public static implicit operator byte[](STATISTICS_CLEAR command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class RSSI_GET
        {
            public const byte ID = 0x07;
            public static implicit operator RSSI_GET(byte[] data)
            {
                RSSI_GET ret = new RSSI_GET();
                return ret;
            }
            public static implicit operator byte[](RSSI_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class RSSI_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue channel1Rssi = 0;
            public ByteValue channel2Rssi = 0;
            public ByteValue channel3Rssi = 0;
            public static implicit operator RSSI_REPORT(byte[] data)
            {
                RSSI_REPORT ret = new RSSI_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.channel1Rssi = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.channel2Rssi = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.channel3Rssi = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RSSI_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.channel1Rssi.HasValue) ret.Add(command.channel1Rssi);
                if (command.channel2Rssi.HasValue) ret.Add(command.channel2Rssi);
                if (command.channel3Rssi.HasValue) ret.Add(command.channel3Rssi);
                return ret.ToArray();
            }
        }
        public partial class S2_RESYNCHRONIZATION_EVENT
        {
            public const byte ID = 0x09;
            public ByteValue nodeid = 0;
            public ByteValue reason = 0;
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator S2_RESYNCHRONIZATION_EVENT(byte[] data)
            {
                S2_RESYNCHRONIZATION_EVENT ret = new S2_RESYNCHRONIZATION_EVENT();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reason = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](S2_RESYNCHRONIZATION_EVENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.reason.HasValue) ret.Add(command.reason);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class EXTENDED_STATISTICS_GET
        {
            public const byte ID = 0x0B;
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator EXTENDED_STATISTICS_GET(byte[] data)
            {
                EXTENDED_STATISTICS_GET ret = new EXTENDED_STATISTICS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](EXTENDED_STATISTICS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class EXTENDED_STATISTICS_REPORT
        {
            public const byte ID = 0x0C;
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public class TSTATISTICS
            {
                public ByteValue type = 0;
                public ByteValue length = 0;
                public IList<byte> value = new List<byte>();
            }
            public List<TSTATISTICS> statistics = new List<TSTATISTICS>();
            public static implicit operator EXTENDED_STATISTICS_REPORT(byte[] data)
            {
                EXTENDED_STATISTICS_REPORT ret = new EXTENDED_STATISTICS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                    ret.statistics = new List<TSTATISTICS>();
                    while (data.Length - 0 > index)
                    {
                        TSTATISTICS tmp = new TSTATISTICS();
                        tmp.type = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.length = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = new List<byte>();
                        for (int i = 0; i < tmp.length; i++)
                        {
                            if (data.Length > index) tmp.value.Add(data[index++]);
                        }
                        ret.statistics.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](EXTENDED_STATISTICS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.statistics != null)
                {
                    foreach (var item in command.statistics)
                    {
                        if (item.type.HasValue) ret.Add(item.type);
                        if (item.length.HasValue) ret.Add(item.length);
                        if (item.value != null)
                        {
                            foreach (var tmp in item.value)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CHANNEL_CONFIGURATION_SET
        {
            public const byte ID = 0x0A;
            public ByteValue zWaveLongRangeChannel = 0;
            public static implicit operator ZWAVE_LR_CHANNEL_CONFIGURATION_SET(byte[] data)
            {
                ZWAVE_LR_CHANNEL_CONFIGURATION_SET ret = new ZWAVE_LR_CHANNEL_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveLongRangeChannel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CHANNEL_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.zWaveLongRangeChannel.HasValue) ret.Add(command.zWaveLongRangeChannel);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CHANNEL_CONFIGURATION_GET
        {
            public const byte ID = 0x0D;
            public static implicit operator ZWAVE_LR_CHANNEL_CONFIGURATION_GET(byte[] data)
            {
                ZWAVE_LR_CHANNEL_CONFIGURATION_GET ret = new ZWAVE_LR_CHANNEL_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CHANNEL_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CHANNEL_CONFIGURATION_REPORT
        {
            public const byte ID = 0x0E;
            public ByteValue zWaveLongRangeChannel = 0;
            public static implicit operator ZWAVE_LR_CHANNEL_CONFIGURATION_REPORT(byte[] data)
            {
                ZWAVE_LR_CHANNEL_CONFIGURATION_REPORT ret = new ZWAVE_LR_CHANNEL_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveLongRangeChannel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CHANNEL_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INSTALLATION_MAINTENANCE_V4.ID);
                ret.Add(ID);
                if (command.zWaveLongRangeChannel.HasValue) ret.Add(command.zWaveLongRangeChannel);
                return ret.ToArray();
            }
        }
    }
}

