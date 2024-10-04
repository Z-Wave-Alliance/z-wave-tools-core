/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR
    {
        public const byte ID = 0x2F;
        public const byte VERSION = 1;
        public partial class COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR_INSTALLED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue zoneNumber = 0;
            public ByteValue numberOfSensors = 0;
            public static implicit operator COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR_INSTALLED_REPORT(byte[] data)
            {
                COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR_INSTALLED_REPORT ret = new COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR_INSTALLED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfSensors = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR_INSTALLED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.numberOfSensors.HasValue) ret.Add(command.numberOfSensors);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_SENSOR_TYPE_GET
        {
            public const byte ID = 0x03;
            public ByteValue zoneNumber = 0;
            public ByteValue sensorNumber = 0;
            public static implicit operator SECURITY_PANEL_ZONE_SENSOR_TYPE_GET(byte[] data)
            {
                SECURITY_PANEL_ZONE_SENSOR_TYPE_GET ret = new SECURITY_PANEL_ZONE_SENSOR_TYPE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_SENSOR_TYPE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.sensorNumber.HasValue) ret.Add(command.sensorNumber);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_SENSOR_TYPE_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue zoneNumber = 0;
            public ByteValue sensorNumber = 0;
            public ByteValue zwaveAlarmType = 0;
            public static implicit operator SECURITY_PANEL_ZONE_SENSOR_TYPE_REPORT(byte[] data)
            {
                SECURITY_PANEL_ZONE_SENSOR_TYPE_REPORT ret = new SECURITY_PANEL_ZONE_SENSOR_TYPE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_SENSOR_TYPE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.sensorNumber.HasValue) ret.Add(command.sensorNumber);
                if (command.zwaveAlarmType.HasValue) ret.Add(command.zwaveAlarmType);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_SENSOR_INSTALLED_GET
        {
            public const byte ID = 0x01;
            public ByteValue zoneNumber = 0;
            public static implicit operator SECURITY_PANEL_ZONE_SENSOR_INSTALLED_GET(byte[] data)
            {
                SECURITY_PANEL_ZONE_SENSOR_INSTALLED_GET ret = new SECURITY_PANEL_ZONE_SENSOR_INSTALLED_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_SENSOR_INSTALLED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_SENSOR_STATE_GET
        {
            public const byte ID = 0x05;
            public ByteValue zoneNumber = 0;
            public ByteValue sensorNumber = 0;
            public static implicit operator SECURITY_PANEL_ZONE_SENSOR_STATE_GET(byte[] data)
            {
                SECURITY_PANEL_ZONE_SENSOR_STATE_GET ret = new SECURITY_PANEL_ZONE_SENSOR_STATE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_SENSOR_STATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.sensorNumber.HasValue) ret.Add(command.sensorNumber);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_SENSOR_STATE_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue zoneNumber = 0;
            public ByteValue sensorNumber = 0;
            public ByteValue zwaveAlarmType = 0;
            public ByteValue zwaveAlarmEvent = 0;
            public ByteValue eventParameters = 0;
            public static implicit operator SECURITY_PANEL_ZONE_SENSOR_STATE_REPORT(byte[] data)
            {
                SECURITY_PANEL_ZONE_SENSOR_STATE_REPORT ret = new SECURITY_PANEL_ZONE_SENSOR_STATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmEvent = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.eventParameters = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_SENSOR_STATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.sensorNumber.HasValue) ret.Add(command.sensorNumber);
                if (command.zwaveAlarmType.HasValue) ret.Add(command.zwaveAlarmType);
                if (command.zwaveAlarmEvent.HasValue) ret.Add(command.zwaveAlarmEvent);
                if (command.eventParameters.HasValue) ret.Add(command.eventParameters);
                return ret.ToArray();
            }
        }
    }
}

