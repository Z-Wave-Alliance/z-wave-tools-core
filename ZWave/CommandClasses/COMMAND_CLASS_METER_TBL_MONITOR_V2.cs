/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_METER_TBL_MONITOR_V2
    {
        public const byte ID = 0x3D;
        public const byte VERSION = 2;
        public partial class METER_TBL_STATUS_REPORT
        {
            public const byte ID = 0x0B;
            public ByteValue reportsToFollow = 0;
            public const byte currentOperatingStatusBytesCount = 3;
            public byte[] currentOperatingStatus = new byte[currentOperatingStatusBytesCount];
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte operatingStatusEventId
                    {
                        get { return (byte)(_value >> 0 & 0x1F); }
                        set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                    }
                    public byte reserved
                    {
                        get { return (byte)(_value >> 5 & 0x03); }
                        set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                    }
                    public byte type
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
                public const byte yearBytesCount = 2;
                public byte[] year = new byte[yearBytesCount];
                public ByteValue month = 0;
                public ByteValue day = 0;
                public ByteValue hourLocalTime = 0;
                public ByteValue minuteLocalTime = 0;
                public ByteValue secondLocalTime = 0;
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator METER_TBL_STATUS_REPORT(byte[] data)
            {
                METER_TBL_STATUS_REPORT ret = new METER_TBL_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.currentOperatingStatus = (data.Length - index) >= currentOperatingStatusBytesCount ? new byte[currentOperatingStatusBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.currentOperatingStatus[0] = data[index++];
                    if (data.Length > index) ret.currentOperatingStatus[1] = data[index++];
                    if (data.Length > index) ret.currentOperatingStatus[2] = data[index++];
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.reportsToFollow; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        tmp.year = (data.Length - index) >= TVG.yearBytesCount ? new byte[TVG.yearBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.year[0] = data[index++];
                        if (data.Length > index) tmp.year[1] = data[index++];
                        tmp.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.currentOperatingStatus != null)
                {
                    foreach (var tmp in command.currentOperatingStatus)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.year != null)
                        {
                            foreach (var tmp in item.year)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.month.HasValue) ret.Add(item.month);
                        if (item.day.HasValue) ret.Add(item.day);
                        if (item.hourLocalTime.HasValue) ret.Add(item.hourLocalTime);
                        if (item.minuteLocalTime.HasValue) ret.Add(item.minuteLocalTime);
                        if (item.secondLocalTime.HasValue) ret.Add(item.secondLocalTime);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_STATUS_DATE_GET
        {
            public const byte ID = 0x0A;
            public ByteValue maximumReports = 0;
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public ByteValue startSecondLocalTime = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHourLocalTime = 0;
            public ByteValue stopMinuteLocalTime = 0;
            public ByteValue stopSecondLocalTime = 0;
            public static implicit operator METER_TBL_STATUS_DATE_GET(byte[] data)
            {
                METER_TBL_STATUS_DATE_GET ret = new METER_TBL_STATUS_DATE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.maximumReports = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_STATUS_DATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.maximumReports.HasValue) ret.Add(command.maximumReports);
                if (command.startYear != null)
                {
                    foreach (var tmp in command.startYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startMonth.HasValue) ret.Add(command.startMonth);
                if (command.startDay.HasValue) ret.Add(command.startDay);
                if (command.startHourLocalTime.HasValue) ret.Add(command.startHourLocalTime);
                if (command.startMinuteLocalTime.HasValue) ret.Add(command.startMinuteLocalTime);
                if (command.startSecondLocalTime.HasValue) ret.Add(command.startSecondLocalTime);
                if (command.stopYear != null)
                {
                    foreach (var tmp in command.stopYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHourLocalTime.HasValue) ret.Add(command.stopHourLocalTime);
                if (command.stopMinuteLocalTime.HasValue) ret.Add(command.stopMinuteLocalTime);
                if (command.stopSecondLocalTime.HasValue) ret.Add(command.stopSecondLocalTime);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_STATUS_DEPTH_GET
        {
            public const byte ID = 0x09;
            public ByteValue statusEventLogDepth = 0;
            public static implicit operator METER_TBL_STATUS_DEPTH_GET(byte[] data)
            {
                METER_TBL_STATUS_DEPTH_GET ret = new METER_TBL_STATUS_DEPTH_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.statusEventLogDepth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_STATUS_DEPTH_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.statusEventLogDepth.HasValue) ret.Add(command.statusEventLogDepth);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_STATUS_SUPPORTED_GET
        {
            public const byte ID = 0x07;
            public static implicit operator METER_TBL_STATUS_SUPPORTED_GET(byte[] data)
            {
                METER_TBL_STATUS_SUPPORTED_GET ret = new METER_TBL_STATUS_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](METER_TBL_STATUS_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_STATUS_SUPPORTED_REPORT
        {
            public const byte ID = 0x08;
            public const byte supportedOperatingStatusBytesCount = 3;
            public byte[] supportedOperatingStatus = new byte[supportedOperatingStatusBytesCount];
            public ByteValue statusEventLogDepth = 0;
            public static implicit operator METER_TBL_STATUS_SUPPORTED_REPORT(byte[] data)
            {
                METER_TBL_STATUS_SUPPORTED_REPORT ret = new METER_TBL_STATUS_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedOperatingStatus = (data.Length - index) >= supportedOperatingStatusBytesCount ? new byte[supportedOperatingStatusBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.supportedOperatingStatus[0] = data[index++];
                    if (data.Length > index) ret.supportedOperatingStatus[1] = data[index++];
                    if (data.Length > index) ret.supportedOperatingStatus[2] = data[index++];
                    ret.statusEventLogDepth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_STATUS_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.supportedOperatingStatus != null)
                {
                    foreach (var tmp in command.supportedOperatingStatus)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.statusEventLogDepth.HasValue) ret.Add(command.statusEventLogDepth);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_CURRENT_DATA_GET
        {
            public const byte ID = 0x0C;
            public const byte datasetRequestedBytesCount = 3;
            public byte[] datasetRequested = new byte[datasetRequestedBytesCount];
            public static implicit operator METER_TBL_CURRENT_DATA_GET(byte[] data)
            {
                METER_TBL_CURRENT_DATA_GET ret = new METER_TBL_CURRENT_DATA_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.datasetRequested = (data.Length - index) >= datasetRequestedBytesCount ? new byte[datasetRequestedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.datasetRequested[0] = data[index++];
                    if (data.Length > index) ret.datasetRequested[1] = data[index++];
                    if (data.Length > index) ret.datasetRequested[2] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_CURRENT_DATA_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.datasetRequested != null)
                {
                    foreach (var tmp in command.datasetRequested)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_CURRENT_DATA_REPORT
        {
            public const byte ID = 0x0D;
            public ByteValue reportsToFollow = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte rateType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x7C; _value += (byte)(value << 2 & 0x7C); }
                }
                public byte operatingStatusIndication
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
            public const byte datasetBytesCount = 3;
            public byte[] dataset = new byte[datasetBytesCount];
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte currentScale
                    {
                        get { return (byte)(_value >> 0 & 0x1F); }
                        set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                    }
                    public byte currentPrecision
                    {
                        get { return (byte)(_value >> 5 & 0x07); }
                        set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
                public const byte currentValueBytesCount = 4;
                public byte[] currentValue = new byte[currentValueBytesCount];
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator METER_TBL_CURRENT_DATA_REPORT(byte[] data)
            {
                METER_TBL_CURRENT_DATA_REPORT ret = new METER_TBL_CURRENT_DATA_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dataset = (data.Length - index) >= datasetBytesCount ? new byte[datasetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dataset[0] = data[index++];
                    if (data.Length > index) ret.dataset[1] = data[index++];
                    if (data.Length > index) ret.dataset[2] = data[index++];
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.reportsToFollow; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        tmp.currentValue = (data.Length - index) >= TVG.currentValueBytesCount ? new byte[TVG.currentValueBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.currentValue[0] = data[index++];
                        if (data.Length > index) tmp.currentValue[1] = data[index++];
                        if (data.Length > index) tmp.currentValue[2] = data[index++];
                        if (data.Length > index) tmp.currentValue[3] = data[index++];
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_CURRENT_DATA_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dataset != null)
                {
                    foreach (var tmp in command.dataset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.hourLocalTime.HasValue) ret.Add(command.hourLocalTime);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.currentValue != null)
                        {
                            foreach (var tmp in item.currentValue)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_HISTORICAL_DATA_GET
        {
            public const byte ID = 0x0E;
            public ByteValue maximumReports = 0;
            public const byte historicalDatasetRequestedBytesCount = 3;
            public byte[] historicalDatasetRequested = new byte[historicalDatasetRequestedBytesCount];
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public ByteValue startSecondLocalTime = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHourLocalTime = 0;
            public ByteValue stopMinuteLocalTime = 0;
            public ByteValue stopSecondLocalTime = 0;
            public static implicit operator METER_TBL_HISTORICAL_DATA_GET(byte[] data)
            {
                METER_TBL_HISTORICAL_DATA_GET ret = new METER_TBL_HISTORICAL_DATA_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.maximumReports = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.historicalDatasetRequested = (data.Length - index) >= historicalDatasetRequestedBytesCount ? new byte[historicalDatasetRequestedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.historicalDatasetRequested[0] = data[index++];
                    if (data.Length > index) ret.historicalDatasetRequested[1] = data[index++];
                    if (data.Length > index) ret.historicalDatasetRequested[2] = data[index++];
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_HISTORICAL_DATA_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.maximumReports.HasValue) ret.Add(command.maximumReports);
                if (command.historicalDatasetRequested != null)
                {
                    foreach (var tmp in command.historicalDatasetRequested)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startYear != null)
                {
                    foreach (var tmp in command.startYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startMonth.HasValue) ret.Add(command.startMonth);
                if (command.startDay.HasValue) ret.Add(command.startDay);
                if (command.startHourLocalTime.HasValue) ret.Add(command.startHourLocalTime);
                if (command.startMinuteLocalTime.HasValue) ret.Add(command.startMinuteLocalTime);
                if (command.startSecondLocalTime.HasValue) ret.Add(command.startSecondLocalTime);
                if (command.stopYear != null)
                {
                    foreach (var tmp in command.stopYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHourLocalTime.HasValue) ret.Add(command.stopHourLocalTime);
                if (command.stopMinuteLocalTime.HasValue) ret.Add(command.stopMinuteLocalTime);
                if (command.stopSecondLocalTime.HasValue) ret.Add(command.stopSecondLocalTime);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_HISTORICAL_DATA_REPORT
        {
            public const byte ID = 0x0F;
            public ByteValue reportsToFollow = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte rateType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x7C; _value += (byte)(value << 2 & 0x7C); }
                }
                public byte operatingStatusIndication
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
            public const byte datasetBytesCount = 3;
            public byte[] dataset = new byte[datasetBytesCount];
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte historicalScale
                    {
                        get { return (byte)(_value >> 0 & 0x1F); }
                        set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                    }
                    public byte historicalPrecision
                    {
                        get { return (byte)(_value >> 5 & 0x07); }
                        set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
                public const byte historicalValueBytesCount = 4;
                public byte[] historicalValue = new byte[historicalValueBytesCount];
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator METER_TBL_HISTORICAL_DATA_REPORT(byte[] data)
            {
                METER_TBL_HISTORICAL_DATA_REPORT ret = new METER_TBL_HISTORICAL_DATA_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dataset = (data.Length - index) >= datasetBytesCount ? new byte[datasetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dataset[0] = data[index++];
                    if (data.Length > index) ret.dataset[1] = data[index++];
                    if (data.Length > index) ret.dataset[2] = data[index++];
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.reportsToFollow; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        tmp.historicalValue = (data.Length - index) >= TVG.historicalValueBytesCount ? new byte[TVG.historicalValueBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.historicalValue[0] = data[index++];
                        if (data.Length > index) tmp.historicalValue[1] = data[index++];
                        if (data.Length > index) tmp.historicalValue[2] = data[index++];
                        if (data.Length > index) tmp.historicalValue[3] = data[index++];
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_HISTORICAL_DATA_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dataset != null)
                {
                    foreach (var tmp in command.dataset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.hourLocalTime.HasValue) ret.Add(command.hourLocalTime);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.historicalValue != null)
                        {
                            foreach (var tmp in item.historicalValue)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_TABLE_CAPABILITY_REPORT
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte meterType
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte rateType
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte payMeter
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public const byte datasetSupportedBytesCount = 3;
            public byte[] datasetSupported = new byte[datasetSupportedBytesCount];
            public const byte datasetHistorySupportedBytesCount = 3;
            public byte[] datasetHistorySupported = new byte[datasetHistorySupportedBytesCount];
            public const byte dataHistorySupportedBytesCount = 3;
            public byte[] dataHistorySupported = new byte[dataHistorySupportedBytesCount];
            public static implicit operator METER_TBL_TABLE_CAPABILITY_REPORT(byte[] data)
            {
                METER_TBL_TABLE_CAPABILITY_REPORT ret = new METER_TBL_TABLE_CAPABILITY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.datasetSupported = (data.Length - index) >= datasetSupportedBytesCount ? new byte[datasetSupportedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.datasetSupported[0] = data[index++];
                    if (data.Length > index) ret.datasetSupported[1] = data[index++];
                    if (data.Length > index) ret.datasetSupported[2] = data[index++];
                    ret.datasetHistorySupported = (data.Length - index) >= datasetHistorySupportedBytesCount ? new byte[datasetHistorySupportedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.datasetHistorySupported[0] = data[index++];
                    if (data.Length > index) ret.datasetHistorySupported[1] = data[index++];
                    if (data.Length > index) ret.datasetHistorySupported[2] = data[index++];
                    ret.dataHistorySupported = (data.Length - index) >= dataHistorySupportedBytesCount ? new byte[dataHistorySupportedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dataHistorySupported[0] = data[index++];
                    if (data.Length > index) ret.dataHistorySupported[1] = data[index++];
                    if (data.Length > index) ret.dataHistorySupported[2] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_TABLE_CAPABILITY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.datasetSupported != null)
                {
                    foreach (var tmp in command.datasetSupported)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.datasetHistorySupported != null)
                {
                    foreach (var tmp in command.datasetHistorySupported)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dataHistorySupported != null)
                {
                    foreach (var tmp in command.dataHistorySupported)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_TABLE_CAPABILITY_GET
        {
            public const byte ID = 0x05;
            public static implicit operator METER_TBL_TABLE_CAPABILITY_GET(byte[] data)
            {
                METER_TBL_TABLE_CAPABILITY_GET ret = new METER_TBL_TABLE_CAPABILITY_GET();
                return ret;
            }
            public static implicit operator byte[](METER_TBL_TABLE_CAPABILITY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_TABLE_ID_GET
        {
            public const byte ID = 0x03;
            public static implicit operator METER_TBL_TABLE_ID_GET(byte[] data)
            {
                METER_TBL_TABLE_ID_GET ret = new METER_TBL_TABLE_ID_GET();
                return ret;
            }
            public static implicit operator byte[](METER_TBL_TABLE_ID_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_TABLE_ID_REPORT
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfCharacters
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> meterIdCharacter = new List<byte>();
            public static implicit operator METER_TBL_TABLE_ID_REPORT(byte[] data)
            {
                METER_TBL_TABLE_ID_REPORT ret = new METER_TBL_TABLE_ID_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.meterIdCharacter = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfCharacters; i++)
                    {
                        if (data.Length > index) ret.meterIdCharacter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_TABLE_ID_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.meterIdCharacter != null)
                {
                    foreach (var tmp in command.meterIdCharacter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_TABLE_POINT_ADM_NO_GET
        {
            public const byte ID = 0x01;
            public static implicit operator METER_TBL_TABLE_POINT_ADM_NO_GET(byte[] data)
            {
                METER_TBL_TABLE_POINT_ADM_NO_GET ret = new METER_TBL_TABLE_POINT_ADM_NO_GET();
                return ret;
            }
            public static implicit operator byte[](METER_TBL_TABLE_POINT_ADM_NO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_TABLE_POINT_ADM_NO_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfCharacters
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> meterPointAdmNumberCharacter = new List<byte>();
            public static implicit operator METER_TBL_TABLE_POINT_ADM_NO_REPORT(byte[] data)
            {
                METER_TBL_TABLE_POINT_ADM_NO_REPORT ret = new METER_TBL_TABLE_POINT_ADM_NO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.meterPointAdmNumberCharacter = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfCharacters; i++)
                    {
                        if (data.Length > index) ret.meterPointAdmNumberCharacter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_TABLE_POINT_ADM_NO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_MONITOR_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.meterPointAdmNumberCharacter != null)
                {
                    foreach (var tmp in command.meterPointAdmNumberCharacter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

