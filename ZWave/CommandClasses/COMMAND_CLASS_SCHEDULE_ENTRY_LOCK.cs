/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCHEDULE_ENTRY_LOCK
    {
        public const byte ID = 0x4E;
        public const byte VERSION = 1;
        public partial class SCHEDULE_ENTRY_LOCK_ENABLE_ALL_SET
        {
            public const byte ID = 0x02;
            public ByteValue enabled = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_ENABLE_ALL_SET(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_ENABLE_ALL_SET ret = new SCHEDULE_ENTRY_LOCK_ENABLE_ALL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.enabled = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_ENABLE_ALL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.enabled.HasValue) ret.Add(command.enabled);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_ENABLE_SET
        {
            public const byte ID = 0x01;
            public ByteValue userIdentifier = 0;
            public ByteValue enabled = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_ENABLE_SET(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_ENABLE_SET ret = new SCHEDULE_ENTRY_LOCK_ENABLE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.enabled = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_ENABLE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.enabled.HasValue) ret.Add(command.enabled);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_WEEK_DAY_GET
        {
            public const byte ID = 0x04;
            public ByteValue userIdentifier = 0;
            public ByteValue scheduleSlotId = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_WEEK_DAY_GET(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_WEEK_DAY_GET ret = new SCHEDULE_ENTRY_LOCK_WEEK_DAY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleSlotId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_WEEK_DAY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.scheduleSlotId.HasValue) ret.Add(command.scheduleSlotId);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_WEEK_DAY_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue userIdentifier = 0;
            public ByteValue scheduleSlotId = 0;
            public ByteValue dayOfWeek = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public ByteValue stopHour = 0;
            public ByteValue stopMinute = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_WEEK_DAY_REPORT(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_WEEK_DAY_REPORT ret = new SCHEDULE_ENTRY_LOCK_WEEK_DAY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleSlotId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dayOfWeek = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_WEEK_DAY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.scheduleSlotId.HasValue) ret.Add(command.scheduleSlotId);
                if (command.dayOfWeek.HasValue) ret.Add(command.dayOfWeek);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.stopHour.HasValue) ret.Add(command.stopHour);
                if (command.stopMinute.HasValue) ret.Add(command.stopMinute);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_WEEK_DAY_SET
        {
            public const byte ID = 0x03;
            public ByteValue setAction = 0;
            public ByteValue userIdentifier = 0;
            public ByteValue scheduleSlotId = 0;
            public ByteValue dayOfWeek = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public ByteValue stopHour = 0;
            public ByteValue stopMinute = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_WEEK_DAY_SET(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_WEEK_DAY_SET ret = new SCHEDULE_ENTRY_LOCK_WEEK_DAY_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.setAction = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleSlotId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dayOfWeek = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_WEEK_DAY_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.setAction.HasValue) ret.Add(command.setAction);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.scheduleSlotId.HasValue) ret.Add(command.scheduleSlotId);
                if (command.dayOfWeek.HasValue) ret.Add(command.dayOfWeek);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.stopHour.HasValue) ret.Add(command.stopHour);
                if (command.stopMinute.HasValue) ret.Add(command.stopMinute);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_YEAR_DAY_GET
        {
            public const byte ID = 0x07;
            public ByteValue userIdentifier = 0;
            public ByteValue scheduleSlotId = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_YEAR_DAY_GET(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_YEAR_DAY_GET ret = new SCHEDULE_ENTRY_LOCK_YEAR_DAY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleSlotId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_YEAR_DAY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.scheduleSlotId.HasValue) ret.Add(command.scheduleSlotId);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_YEAR_DAY_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue userIdentifier = 0;
            public ByteValue scheduleSlotId = 0;
            public ByteValue startYear = 0;
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public ByteValue stopYear = 0;
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHour = 0;
            public ByteValue stopMinute = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_YEAR_DAY_REPORT(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_YEAR_DAY_REPORT ret = new SCHEDULE_ENTRY_LOCK_YEAR_DAY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleSlotId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startYear = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_YEAR_DAY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.scheduleSlotId.HasValue) ret.Add(command.scheduleSlotId);
                if (command.startYear.HasValue) ret.Add(command.startYear);
                if (command.startMonth.HasValue) ret.Add(command.startMonth);
                if (command.startDay.HasValue) ret.Add(command.startDay);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.stopYear.HasValue) ret.Add(command.stopYear);
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHour.HasValue) ret.Add(command.stopHour);
                if (command.stopMinute.HasValue) ret.Add(command.stopMinute);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_LOCK_YEAR_DAY_SET
        {
            public const byte ID = 0x06;
            public ByteValue setAction = 0;
            public ByteValue userIdentifier = 0;
            public ByteValue scheduleSlotId = 0;
            public ByteValue startYear = 0;
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public ByteValue stopYear = 0;
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHour = 0;
            public ByteValue stopMinute = 0;
            public static implicit operator SCHEDULE_ENTRY_LOCK_YEAR_DAY_SET(byte[] data)
            {
                SCHEDULE_ENTRY_LOCK_YEAR_DAY_SET ret = new SCHEDULE_ENTRY_LOCK_YEAR_DAY_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.setAction = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleSlotId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startYear = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_LOCK_YEAR_DAY_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.setAction.HasValue) ret.Add(command.setAction);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.scheduleSlotId.HasValue) ret.Add(command.scheduleSlotId);
                if (command.startYear.HasValue) ret.Add(command.startYear);
                if (command.startMonth.HasValue) ret.Add(command.startMonth);
                if (command.startDay.HasValue) ret.Add(command.startDay);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.stopYear.HasValue) ret.Add(command.stopYear);
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHour.HasValue) ret.Add(command.stopHour);
                if (command.stopMinute.HasValue) ret.Add(command.stopMinute);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_TYPE_SUPPORTED_GET
        {
            public const byte ID = 0x09;
            public static implicit operator SCHEDULE_ENTRY_TYPE_SUPPORTED_GET(byte[] data)
            {
                SCHEDULE_ENTRY_TYPE_SUPPORTED_GET ret = new SCHEDULE_ENTRY_TYPE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_TYPE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_ENTRY_TYPE_SUPPORTED_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue numberOfSlotsWeekDay = 0;
            public ByteValue numberOfSlotsYearDay = 0;
            public static implicit operator SCHEDULE_ENTRY_TYPE_SUPPORTED_REPORT(byte[] data)
            {
                SCHEDULE_ENTRY_TYPE_SUPPORTED_REPORT ret = new SCHEDULE_ENTRY_TYPE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSlotsWeekDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfSlotsYearDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_ENTRY_TYPE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_ENTRY_LOCK.ID);
                ret.Add(ID);
                if (command.numberOfSlotsWeekDay.HasValue) ret.Add(command.numberOfSlotsWeekDay);
                if (command.numberOfSlotsYearDay.HasValue) ret.Add(command.numberOfSlotsYearDay);
                return ret.ToArray();
            }
        }
    }
}

