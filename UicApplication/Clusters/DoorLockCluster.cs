/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class DoorLockCluster
    {
        public const string ClusterName = "DoorLock";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_LockState
        {
            public const string LockState_name = "LockState";
            public class LockState_Attributes
            {
                public static implicit operator string(LockState_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_LockState command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LockState.LockState_name);
            }
            public class LockState_Desired
            {
                public static implicit operator string(LockState_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LockState.LockState_name, ClusterDesired);
                }
            }
            public class LockState_Report
            {
                public static implicit operator string(LockState_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LockState.LockState_name, ClusterReported);
                }
            }
            public class LockState_SupportCommands
            {
                public static implicit operator string(LockState_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class NotFullyLocked
                {
                    public const string NotFullyLocked_value = "00";
                    public const string NotFullyLocked_name = "NotFullyLocked";
                }

                public class Locked
                {
                    public const string Locked_value = "01";
                    public const string Locked_name = "Locked";
                }

                public class Unlocked
                {
                    public const string Unlocked_value = "02";
                    public const string Unlocked_name = "Unlocked";
                }

                public class Undefined
                {
                    public const string Undefined_value = "FF";
                    public const string Undefined_name = "Undefined";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_LockType
        {
            public const string LockType_name = "LockType";
            public class LockType_Attributes
            {
                public static implicit operator string(LockType_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_LockType command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LockType.LockType_name);
            }
            public class LockType_Desired
            {
                public static implicit operator string(LockType_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LockType.LockType_name, ClusterDesired);
                }
            }
            public class LockType_Report
            {
                public static implicit operator string(LockType_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LockType.LockType_name, ClusterReported);
                }
            }
            public class LockType_SupportCommands
            {
                public static implicit operator string(LockType_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class DeadBolt
                {
                    public const string DeadBolt_value = "00";
                    public const string DeadBolt_name = "DeadBolt";
                }

                public class Magnetic
                {
                    public const string Magnetic_value = "01";
                    public const string Magnetic_name = "Magnetic";
                }

                public class Other
                {
                    public const string Other_value = "02";
                    public const string Other_name = "Other";
                }

                public class Mortise
                {
                    public const string Mortise_value = "03";
                    public const string Mortise_name = "Mortise";
                }

                public class Rim
                {
                    public const string Rim_value = "04";
                    public const string Rim_name = "Rim";
                }

                public class LatchBolt
                {
                    public const string LatchBolt_value = "05";
                    public const string LatchBolt_name = "LatchBolt";
                }

                public class CylindricalLock
                {
                    public const string CylindricalLock_value = "06";
                    public const string CylindricalLock_name = "CylindricalLock";
                }

                public class TubularLock
                {
                    public const string TubularLock_value = "07";
                    public const string TubularLock_name = "TubularLock";
                }

                public class InterconnectedLock
                {
                    public const string InterconnectedLock_value = "08";
                    public const string InterconnectedLock_name = "InterconnectedLock";
                }

                public class DeadLatch
                {
                    public const string DeadLatch_value = "09";
                    public const string DeadLatch_name = "DeadLatch";
                }

                public class DoorFurniture
                {
                    public const string DoorFurniture_value = "0A";
                    public const string DoorFurniture_name = "DoorFurniture";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_ActuatorEnabled
        {
            public const string ActuatorEnabled_name = "ActuatorEnabled";
            public class ActuatorEnabled_Attributes
            {
                public static implicit operator string(ActuatorEnabled_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ActuatorEnabled command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_ActuatorEnabled.ActuatorEnabled_name);
            }
            public class ActuatorEnabled_Desired
            {
                public static implicit operator string(ActuatorEnabled_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_ActuatorEnabled.ActuatorEnabled_name, ClusterDesired);
                }
            }
            public class ActuatorEnabled_Report
            {
                public static implicit operator string(ActuatorEnabled_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_ActuatorEnabled.ActuatorEnabled_name, ClusterReported);
                }
            }
            public class ActuatorEnabled_SupportCommands
            {
                public static implicit operator string(ActuatorEnabled_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_DoorState
        {
            public const string DoorState_name = "DoorState";
            public class DoorState_Attributes
            {
                public static implicit operator string(DoorState_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_DoorState command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorState.DoorState_name);
            }
            public class DoorState_Desired
            {
                public static implicit operator string(DoorState_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorState.DoorState_name, ClusterDesired);
                }
            }
            public class DoorState_Report
            {
                public static implicit operator string(DoorState_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorState.DoorState_name, ClusterReported);
                }
            }
            public class DoorState_SupportCommands
            {
                public static implicit operator string(DoorState_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Open
                {
                    public const string Open_value = "00";
                    public const string Open_name = "Open";
                }

                public class Closed
                {
                    public const string Closed_value = "01";
                    public const string Closed_name = "Closed";
                }

                public class ErrorJammed
                {
                    public const string ErrorJammed_value = "02";
                    public const string ErrorJammed_name = "ErrorJammed";
                }

                public class ErrorForcedOpen
                {
                    public const string ErrorForcedOpen_value = "03";
                    public const string ErrorForcedOpen_name = "ErrorForcedOpen";
                }

                public class ErrorUnspecified
                {
                    public const string ErrorUnspecified_value = "04";
                    public const string ErrorUnspecified_name = "ErrorUnspecified";
                }

                public class Undefined
                {
                    public const string Undefined_value = "FF";
                    public const string Undefined_name = "Undefined";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_DoorOpenEvents
        {
            public const string DoorOpenEvents_name = "DoorOpenEvents";
            public class DoorOpenEvents_Attributes
            {
                public static implicit operator string(DoorOpenEvents_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_DoorOpenEvents command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorOpenEvents.DoorOpenEvents_name);
            }
            public class DoorOpenEvents_Desired
            {
                public static implicit operator string(DoorOpenEvents_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorOpenEvents.DoorOpenEvents_name, ClusterDesired);
                }
            }
            public class DoorOpenEvents_Report
            {
                public static implicit operator string(DoorOpenEvents_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorOpenEvents.DoorOpenEvents_name, ClusterReported);
                }
            }
            public class DoorOpenEvents_SupportCommands
            {
                public static implicit operator string(DoorOpenEvents_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_DoorClosedEvents
        {
            public const string DoorClosedEvents_name = "DoorClosedEvents";
            public class DoorClosedEvents_Attributes
            {
                public static implicit operator string(DoorClosedEvents_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_DoorClosedEvents command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorClosedEvents.DoorClosedEvents_name);
            }
            public class DoorClosedEvents_Desired
            {
                public static implicit operator string(DoorClosedEvents_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorClosedEvents.DoorClosedEvents_name, ClusterDesired);
                }
            }
            public class DoorClosedEvents_Report
            {
                public static implicit operator string(DoorClosedEvents_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DoorClosedEvents.DoorClosedEvents_name, ClusterReported);
                }
            }
            public class DoorClosedEvents_SupportCommands
            {
                public static implicit operator string(DoorClosedEvents_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OpenPeriod
        {
            public const string OpenPeriod_name = "OpenPeriod";
            public class OpenPeriod_Attributes
            {
                public static implicit operator string(OpenPeriod_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OpenPeriod command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_OpenPeriod.OpenPeriod_name);
            }
            public class OpenPeriod_Desired
            {
                public static implicit operator string(OpenPeriod_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_OpenPeriod.OpenPeriod_name, ClusterDesired);
                }
            }
            public class OpenPeriod_Report
            {
                public static implicit operator string(OpenPeriod_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_OpenPeriod.OpenPeriod_name, ClusterReported);
                }
            }
            public class OpenPeriod_SupportCommands
            {
                public static implicit operator string(OpenPeriod_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfLogRecordsSupported
        {
            public const string NumberOfLogRecordsSupported_name = "NumberOfLogRecordsSupported";
            public class NumberOfLogRecordsSupported_Attributes
            {
                public static implicit operator string(NumberOfLogRecordsSupported_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfLogRecordsSupported command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfLogRecordsSupported.NumberOfLogRecordsSupported_name);
            }
            public class NumberOfLogRecordsSupported_Desired
            {
                public static implicit operator string(NumberOfLogRecordsSupported_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfLogRecordsSupported.NumberOfLogRecordsSupported_name, ClusterDesired);
                }
            }
            public class NumberOfLogRecordsSupported_Report
            {
                public static implicit operator string(NumberOfLogRecordsSupported_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfLogRecordsSupported.NumberOfLogRecordsSupported_name, ClusterReported);
                }
            }
            public class NumberOfLogRecordsSupported_SupportCommands
            {
                public static implicit operator string(NumberOfLogRecordsSupported_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfTotalUsersSupported
        {
            public const string NumberOfTotalUsersSupported_name = "NumberOfTotalUsersSupported";
            public class NumberOfTotalUsersSupported_Attributes
            {
                public static implicit operator string(NumberOfTotalUsersSupported_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfTotalUsersSupported command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfTotalUsersSupported.NumberOfTotalUsersSupported_name);
            }
            public class NumberOfTotalUsersSupported_Desired
            {
                public static implicit operator string(NumberOfTotalUsersSupported_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfTotalUsersSupported.NumberOfTotalUsersSupported_name, ClusterDesired);
                }
            }
            public class NumberOfTotalUsersSupported_Report
            {
                public static implicit operator string(NumberOfTotalUsersSupported_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfTotalUsersSupported.NumberOfTotalUsersSupported_name, ClusterReported);
                }
            }
            public class NumberOfTotalUsersSupported_SupportCommands
            {
                public static implicit operator string(NumberOfTotalUsersSupported_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            { }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_NumberOfPINUsersSupported
        {
            public const string NumberOfPINUsersSupported_name = "NumberOfPINUsersSupported";
            public class NumberOfPINUsersSupported_Attributes
            {
                public static implicit operator string(NumberOfPINUsersSupported_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfPINUsersSupported command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfPINUsersSupported.NumberOfPINUsersSupported_name);
            }
            public class NumberOfPINUsersSupported_Desired
            {
                public static implicit operator string(NumberOfPINUsersSupported_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfPINUsersSupported.NumberOfPINUsersSupported_name, ClusterDesired);
                }
            }
            public class NumberOfPINUsersSupported_Report
            {
                public static implicit operator string(NumberOfPINUsersSupported_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfPINUsersSupported.NumberOfPINUsersSupported_name, ClusterReported);
                }
            }
            public class NumberOfPINUsersSupported_SupportCommands
            {
                public static implicit operator string(NumberOfPINUsersSupported_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfRFIDUsersSupported
        {
            public const string NumberOfRFIDUsersSupported_name = "NumberOfRFIDUsersSupported";
            public class NumberOfRFIDUsersSupported_Attributes
            {
                public static implicit operator string(NumberOfRFIDUsersSupported_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfRFIDUsersSupported command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfRFIDUsersSupported.NumberOfRFIDUsersSupported_name);
            }
            public class NumberOfRFIDUsersSupported_Desired
            {
                public static implicit operator string(NumberOfRFIDUsersSupported_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfRFIDUsersSupported.NumberOfRFIDUsersSupported_name, ClusterDesired);
                }
            }
            public class NumberOfRFIDUsersSupported_Report
            {
                public static implicit operator string(NumberOfRFIDUsersSupported_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfRFIDUsersSupported.NumberOfRFIDUsersSupported_name, ClusterReported);
                }
            }
            public class NumberOfRFIDUsersSupported_SupportCommands
            {
                public static implicit operator string(NumberOfRFIDUsersSupported_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfWeekDaySchedulesSupportedPerUser
        {
            public const string NumberOfWeekDaySchedulesSupportedPerUser_name = "NumberOfWeekDaySchedulesSupportedPerUser";
            public class NumberOfWeekDaySchedulesSupportedPerUser_Attributes
            {
                public static implicit operator string(NumberOfWeekDaySchedulesSupportedPerUser_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfWeekDaySchedulesSupportedPerUser command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfWeekDaySchedulesSupportedPerUser.NumberOfWeekDaySchedulesSupportedPerUser_name);
            }
            public class NumberOfWeekDaySchedulesSupportedPerUser_Desired
            {
                public static implicit operator string(NumberOfWeekDaySchedulesSupportedPerUser_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfWeekDaySchedulesSupportedPerUser.NumberOfWeekDaySchedulesSupportedPerUser_name, ClusterDesired);
                }
            }
            public class NumberOfWeekDaySchedulesSupportedPerUser_Report
            {
                public static implicit operator string(NumberOfWeekDaySchedulesSupportedPerUser_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfWeekDaySchedulesSupportedPerUser.NumberOfWeekDaySchedulesSupportedPerUser_name, ClusterReported);
                }
            }
            public class NumberOfWeekDaySchedulesSupportedPerUser_SupportCommands
            {
                public static implicit operator string(NumberOfWeekDaySchedulesSupportedPerUser_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfYearDaySchedulesSupportedPerUser
        {
            public const string NumberOfYearDaySchedulesSupportedPerUser_name = "NumberOfYearDaySchedulesSupportedPerUser";
            public class NumberOfYearDaySchedulesSupportedPerUser_Attributes
            {
                public static implicit operator string(NumberOfYearDaySchedulesSupportedPerUser_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfYearDaySchedulesSupportedPerUser command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfYearDaySchedulesSupportedPerUser.NumberOfYearDaySchedulesSupportedPerUser_name);
            }
            public class NumberOfYearDaySchedulesSupportedPerUser_Desired
            {
                public static implicit operator string(NumberOfYearDaySchedulesSupportedPerUser_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfYearDaySchedulesSupportedPerUser.NumberOfYearDaySchedulesSupportedPerUser_name, ClusterDesired);
                }
            }
            public class NumberOfYearDaySchedulesSupportedPerUser_Report
            {
                public static implicit operator string(NumberOfYearDaySchedulesSupportedPerUser_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfYearDaySchedulesSupportedPerUser.NumberOfYearDaySchedulesSupportedPerUser_name, ClusterReported);
                }
            }
            public class NumberOfYearDaySchedulesSupportedPerUser_SupportCommands
            {
                public static implicit operator string(NumberOfYearDaySchedulesSupportedPerUser_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfHolidaySchedulesSupported
        {
            public const string NumberOfHolidaySchedulesSupported_name = "NumberOfHolidaySchedulesSupported";
            public class NumberOfHolidaySchedulesSupported_Attributes
            {
                public static implicit operator string(NumberOfHolidaySchedulesSupported_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfHolidaySchedulesSupported command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfHolidaySchedulesSupported.NumberOfHolidaySchedulesSupported_name);
            }
            public class NumberOfHolidaySchedulesSupported_Desired
            {
                public static implicit operator string(NumberOfHolidaySchedulesSupported_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfHolidaySchedulesSupported.NumberOfHolidaySchedulesSupported_name, ClusterDesired);
                }
            }
            public class NumberOfHolidaySchedulesSupported_Report
            {
                public static implicit operator string(NumberOfHolidaySchedulesSupported_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_NumberOfHolidaySchedulesSupported.NumberOfHolidaySchedulesSupported_name, ClusterReported);
                }
            }
            public class NumberOfHolidaySchedulesSupported_SupportCommands
            {
                public static implicit operator string(NumberOfHolidaySchedulesSupported_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MaxPINCodeLength
        {
            public const string MaxPINCodeLength_name = "MaxPINCodeLength";
            public class MaxPINCodeLength_Attributes
            {
                public static implicit operator string(MaxPINCodeLength_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MaxPINCodeLength command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MaxPINCodeLength.MaxPINCodeLength_name);
            }
            public class MaxPINCodeLength_Desired
            {
                public static implicit operator string(MaxPINCodeLength_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MaxPINCodeLength.MaxPINCodeLength_name, ClusterDesired);
                }
            }
            public class MaxPINCodeLength_Report
            {
                public static implicit operator string(MaxPINCodeLength_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MaxPINCodeLength.MaxPINCodeLength_name, ClusterReported);
                }
            }
            public class MaxPINCodeLength_SupportCommands
            {
                public static implicit operator string(MaxPINCodeLength_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MinPINCodeLength
        {
            public const string MinPINCodeLength_name = "MinPINCodeLength";
            public class MinPINCodeLength_Attributes
            {
                public static implicit operator string(MinPINCodeLength_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinPINCodeLength command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MinPINCodeLength.MinPINCodeLength_name);
            }
            public class MinPINCodeLength_Desired
            {
                public static implicit operator string(MinPINCodeLength_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MinPINCodeLength.MinPINCodeLength_name, ClusterDesired);
                }
            }
            public class MinPINCodeLength_Report
            {
                public static implicit operator string(MinPINCodeLength_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MinPINCodeLength.MinPINCodeLength_name, ClusterReported);
                }
            }
            public class MinPINCodeLength_SupportCommands
            {
                public static implicit operator string(MinPINCodeLength_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MaxRFIDCodeLength
        {
            public const string MaxRFIDCodeLength_name = "MaxRFIDCodeLength";
            public class MaxRFIDCodeLength_Attributes
            {
                public static implicit operator string(MaxRFIDCodeLength_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MaxRFIDCodeLength command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MaxRFIDCodeLength.MaxRFIDCodeLength_name);
            }
            public class MaxRFIDCodeLength_Desired
            {
                public static implicit operator string(MaxRFIDCodeLength_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MaxRFIDCodeLength.MaxRFIDCodeLength_name, ClusterDesired);
                }
            }
            public class MaxRFIDCodeLength_Report
            {
                public static implicit operator string(MaxRFIDCodeLength_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MaxRFIDCodeLength.MaxRFIDCodeLength_name, ClusterReported);
                }
            }
            public class MaxRFIDCodeLength_SupportCommands
            {
                public static implicit operator string(MaxRFIDCodeLength_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MinRFIDCodeLength
        {
            public const string MinRFIDCodeLength_name = "MinRFIDCodeLength";
            public class MinRFIDCodeLength_Attributes
            {
                public static implicit operator string(MinRFIDCodeLength_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinRFIDCodeLength command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MinRFIDCodeLength.MinRFIDCodeLength_name);
            }
            public class MinRFIDCodeLength_Desired
            {
                public static implicit operator string(MinRFIDCodeLength_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MinRFIDCodeLength.MinRFIDCodeLength_name, ClusterDesired);
                }
            }
            public class MinRFIDCodeLength_Report
            {
                public static implicit operator string(MinRFIDCodeLength_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_MinRFIDCodeLength.MinRFIDCodeLength_name, ClusterReported);
                }
            }
            public class MinRFIDCodeLength_SupportCommands
            {
                public static implicit operator string(MinRFIDCodeLength_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_EnableLogging
        {
            public const string EnableLogging_name = "EnableLogging";
            public class EnableLogging_Attributes
            {
                public static implicit operator string(EnableLogging_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_EnableLogging command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableLogging.EnableLogging_name);
            }
            public class EnableLogging_Desired
            {
                public static implicit operator string(EnableLogging_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableLogging.EnableLogging_name, ClusterDesired);
                }
            }
            public class EnableLogging_Report
            {
                public static implicit operator string(EnableLogging_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableLogging.EnableLogging_name, ClusterReported);
                }
            }
            public class EnableLogging_SupportCommands
            {
                public static implicit operator string(EnableLogging_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_Language
        {
            public const string Language_name = "Language";
            public class Language_Attributes
            {
                public static implicit operator string(Language_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_Language command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_Language.Language_name);
            }
            public class Language_Desired
            {
                public static implicit operator string(Language_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_Language.Language_name, ClusterDesired);
                }
            }
            public class Language_Report
            {
                public static implicit operator string(Language_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_Language.Language_name, ClusterReported);
                }
            }
            public class Language_SupportCommands
            {
                public static implicit operator string(Language_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            { }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_LEDSettings
        {
            public const string LEDSettings_name = "LEDSettings";
            public class LEDSettings_Attributes
            {
                public static implicit operator string(LEDSettings_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_LEDSettings command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LEDSettings.LEDSettings_name);
            }
            public class LEDSettings_Desired
            {
                public static implicit operator string(LEDSettings_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LEDSettings.LEDSettings_name, ClusterDesired);
                }
            }
            public class LEDSettings_Report
            {
                public static implicit operator string(LEDSettings_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_LEDSettings.LEDSettings_name, ClusterReported);
                }
            }
            public class LEDSettings_SupportCommands
            {
                public static implicit operator string(LEDSettings_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class NeverUseLED
                {
                    public const string NeverUseLED_value = "00";
                    public const string NeverUseLED_name = "NeverUseLED";
                }

                public class UseLEDExceptForAccessAllowed
                {
                    public const string UseLEDExceptForAccessAllowed_value = "01";
                    public const string UseLEDExceptForAccessAllowed_name = "UseLEDExceptForAccessAllowed";
                }

                public class UseLEDForAllEvents
                {
                    public const string UseLEDForAllEvents_value = "02";
                    public const string UseLEDForAllEvents_name = "UseLEDForAllEvents";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_AutoRelockTime
        {
            public const string AutoRelockTime_name = "AutoRelockTime";
            public class AutoRelockTime_Attributes
            {
                public static implicit operator string(AutoRelockTime_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AutoRelockTime command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_AutoRelockTime.AutoRelockTime_name);
            }
            public class AutoRelockTime_Desired
            {
                public static implicit operator string(AutoRelockTime_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_AutoRelockTime.AutoRelockTime_name, ClusterDesired);
                }
            }
            public class AutoRelockTime_Report
            {
                public static implicit operator string(AutoRelockTime_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_AutoRelockTime.AutoRelockTime_name, ClusterReported);
                }
            }
            public class AutoRelockTime_SupportCommands
            {
                public static implicit operator string(AutoRelockTime_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Disabled
                {
                    public const string Disabled_value = "0";
                    public const string Disabled_name = "Disabled";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_SoundVolume
        {
            public const string SoundVolume_name = "SoundVolume";
            public class SoundVolume_Attributes
            {
                public static implicit operator string(SoundVolume_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SoundVolume command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SoundVolume.SoundVolume_name);
            }
            public class SoundVolume_Desired
            {
                public static implicit operator string(SoundVolume_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SoundVolume.SoundVolume_name, ClusterDesired);
                }
            }
            public class SoundVolume_Report
            {
                public static implicit operator string(SoundVolume_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SoundVolume.SoundVolume_name, ClusterReported);
                }
            }
            public class SoundVolume_SupportCommands
            {
                public static implicit operator string(SoundVolume_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class SilentMode
                {
                    public const string SilentMode_value = "00";
                    public const string SilentMode_name = "SilentMode";
                }

                public class LowVolume
                {
                    public const string LowVolume_value = "01";
                    public const string LowVolume_name = "LowVolume";
                }

                public class HighVolume
                {
                    public const string HighVolume_value = "02";
                    public const string HighVolume_name = "HighVolume";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_OperatingMode
        {
            public const string OperatingMode_name = "OperatingMode";
            public class OperatingMode_Attributes
            {
                public static implicit operator string(OperatingMode_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OperatingMode command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_OperatingMode.OperatingMode_name);
            }
            public class OperatingMode_Desired
            {
                public static implicit operator string(OperatingMode_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_OperatingMode.OperatingMode_name, ClusterDesired);
                }
            }
            public class OperatingMode_Report
            {
                public static implicit operator string(OperatingMode_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_OperatingMode.OperatingMode_name, ClusterReported);
                }
            }
            public class OperatingMode_SupportCommands
            {
                public static implicit operator string(OperatingMode_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_SupportedOperatingModes
        {
            public const string SupportedOperatingModes_name = "SupportedOperatingModes";
            public class SupportedOperatingModes_Attributes
            {
                public static implicit operator string(SupportedOperatingModes_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SupportedOperatingModes command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SupportedOperatingModes.SupportedOperatingModes_name);
            }
            public class SupportedOperatingModes_Desired
            {
                public static implicit operator string(SupportedOperatingModes_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SupportedOperatingModes.SupportedOperatingModes_name, ClusterDesired);
                }
            }
            public class SupportedOperatingModes_Report
            {
                public static implicit operator string(SupportedOperatingModes_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SupportedOperatingModes.SupportedOperatingModes_name, ClusterReported);
                }
            }
            public class SupportedOperatingModes_SupportCommands
            {
                public static implicit operator string(SupportedOperatingModes_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class NormalModeSupported
                {
                    public const string NormalModeSupported_name = "NormalModeSupported";
                }
                public class VacationModeSupported
                {
                    public const string VacationModeSupported_name = "VacationModeSupported";
                }
                public class PrivacyModeSupported
                {
                    public const string PrivacyModeSupported_name = "PrivacyModeSupported";
                }
                public class NoRFLockOrUnlockModeSupported
                {
                    public const string NoRFLockOrUnlockModeSupported_name = "NoRFLockOrUnlockModeSupported";
                }
                public class PassageModeSupported
                {
                    public const string PassageModeSupported_name = "PassageModeSupported";
                }
            }
        }/*End of ATT class*/
        public class ATT_DefaultConfigurationRegister
        {
            public const string DefaultConfigurationRegister_name = "DefaultConfigurationRegister";
            public class DefaultConfigurationRegister_Attributes
            {
                public static implicit operator string(DefaultConfigurationRegister_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_DefaultConfigurationRegister command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DefaultConfigurationRegister.DefaultConfigurationRegister_name);
            }
            public class DefaultConfigurationRegister_Desired
            {
                public static implicit operator string(DefaultConfigurationRegister_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DefaultConfigurationRegister.DefaultConfigurationRegister_name, ClusterDesired);
                }
            }
            public class DefaultConfigurationRegister_Report
            {
                public static implicit operator string(DefaultConfigurationRegister_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_DefaultConfigurationRegister.DefaultConfigurationRegister_name, ClusterReported);
                }
            }
            public class DefaultConfigurationRegister_SupportCommands
            {
                public static implicit operator string(DefaultConfigurationRegister_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class DefaultEnableLocalProgrammingAttributeIsEnabled
                {
                    public const string DefaultEnableLocalProgrammingAttributeIsEnabled_name = "DefaultEnableLocalProgrammingAttributeIsEnabled";
                }
                public class DefaultKeypadInterfaceIsEnabled
                {
                    public const string DefaultKeypadInterfaceIsEnabled_name = "DefaultKeypadInterfaceIsEnabled";
                }
                public class DefaultRFInterfaceIsEnabled
                {
                    public const string DefaultRFInterfaceIsEnabled_name = "DefaultRFInterfaceIsEnabled";
                }
                public class DefaultSoundVolumeIsEnabled
                {
                    public const string DefaultSoundVolumeIsEnabled_name = "DefaultSoundVolumeIsEnabled";
                }
                public class DefaultAutoRelockTimeIsEnabled
                {
                    public const string DefaultAutoRelockTimeIsEnabled_name = "DefaultAutoRelockTimeIsEnabled";
                }
                public class DefaultLEDSettingsIsEnabled
                {
                    public const string DefaultLEDSettingsIsEnabled_name = "DefaultLEDSettingsIsEnabled";
                }
            }
        }/*End of ATT class*/
        public class ATT_EnableLocalProgramming
        {
            public const string EnableLocalProgramming_name = "EnableLocalProgramming";
            public class EnableLocalProgramming_Attributes
            {
                public static implicit operator string(EnableLocalProgramming_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_EnableLocalProgramming command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableLocalProgramming.EnableLocalProgramming_name);
            }
            public class EnableLocalProgramming_Desired
            {
                public static implicit operator string(EnableLocalProgramming_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableLocalProgramming.EnableLocalProgramming_name, ClusterDesired);
                }
            }
            public class EnableLocalProgramming_Report
            {
                public static implicit operator string(EnableLocalProgramming_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableLocalProgramming.EnableLocalProgramming_name, ClusterReported);
                }
            }
            public class EnableLocalProgramming_SupportCommands
            {
                public static implicit operator string(EnableLocalProgramming_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_EnableOneTouchLocking
        {
            public const string EnableOneTouchLocking_name = "EnableOneTouchLocking";
            public class EnableOneTouchLocking_Attributes
            {
                public static implicit operator string(EnableOneTouchLocking_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_EnableOneTouchLocking command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableOneTouchLocking.EnableOneTouchLocking_name);
            }
            public class EnableOneTouchLocking_Desired
            {
                public static implicit operator string(EnableOneTouchLocking_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableOneTouchLocking.EnableOneTouchLocking_name, ClusterDesired);
                }
            }
            public class EnableOneTouchLocking_Report
            {
                public static implicit operator string(EnableOneTouchLocking_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableOneTouchLocking.EnableOneTouchLocking_name, ClusterReported);
                }
            }
            public class EnableOneTouchLocking_SupportCommands
            {
                public static implicit operator string(EnableOneTouchLocking_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_EnableInsideStatusLED
        {
            public const string EnableInsideStatusLED_name = "EnableInsideStatusLED";
            public class EnableInsideStatusLED_Attributes
            {
                public static implicit operator string(EnableInsideStatusLED_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_EnableInsideStatusLED command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableInsideStatusLED.EnableInsideStatusLED_name);
            }
            public class EnableInsideStatusLED_Desired
            {
                public static implicit operator string(EnableInsideStatusLED_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableInsideStatusLED.EnableInsideStatusLED_name, ClusterDesired);
                }
            }
            public class EnableInsideStatusLED_Report
            {
                public static implicit operator string(EnableInsideStatusLED_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnableInsideStatusLED.EnableInsideStatusLED_name, ClusterReported);
                }
            }
            public class EnableInsideStatusLED_SupportCommands
            {
                public static implicit operator string(EnableInsideStatusLED_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_EnablePrivacyModeButton
        {
            public const string EnablePrivacyModeButton_name = "EnablePrivacyModeButton";
            public class EnablePrivacyModeButton_Attributes
            {
                public static implicit operator string(EnablePrivacyModeButton_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_EnablePrivacyModeButton command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnablePrivacyModeButton.EnablePrivacyModeButton_name);
            }
            public class EnablePrivacyModeButton_Desired
            {
                public static implicit operator string(EnablePrivacyModeButton_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnablePrivacyModeButton.EnablePrivacyModeButton_name, ClusterDesired);
                }
            }
            public class EnablePrivacyModeButton_Report
            {
                public static implicit operator string(EnablePrivacyModeButton_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_EnablePrivacyModeButton.EnablePrivacyModeButton_name, ClusterReported);
                }
            }
            public class EnablePrivacyModeButton_SupportCommands
            {
                public static implicit operator string(EnablePrivacyModeButton_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_WrongCodeEntryLimit
        {
            public const string WrongCodeEntryLimit_name = "WrongCodeEntryLimit";
            public class WrongCodeEntryLimit_Attributes
            {
                public static implicit operator string(WrongCodeEntryLimit_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_WrongCodeEntryLimit command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_WrongCodeEntryLimit.WrongCodeEntryLimit_name);
            }
            public class WrongCodeEntryLimit_Desired
            {
                public static implicit operator string(WrongCodeEntryLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_WrongCodeEntryLimit.WrongCodeEntryLimit_name, ClusterDesired);
                }
            }
            public class WrongCodeEntryLimit_Report
            {
                public static implicit operator string(WrongCodeEntryLimit_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_WrongCodeEntryLimit.WrongCodeEntryLimit_name, ClusterReported);
                }
            }
            public class WrongCodeEntryLimit_SupportCommands
            {
                public static implicit operator string(WrongCodeEntryLimit_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_UserCodeTemporaryDisableTime
        {
            public const string UserCodeTemporaryDisableTime_name = "UserCodeTemporaryDisableTime";
            public class UserCodeTemporaryDisableTime_Attributes
            {
                public static implicit operator string(UserCodeTemporaryDisableTime_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_UserCodeTemporaryDisableTime command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_UserCodeTemporaryDisableTime.UserCodeTemporaryDisableTime_name);
            }
            public class UserCodeTemporaryDisableTime_Desired
            {
                public static implicit operator string(UserCodeTemporaryDisableTime_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_UserCodeTemporaryDisableTime.UserCodeTemporaryDisableTime_name, ClusterDesired);
                }
            }
            public class UserCodeTemporaryDisableTime_Report
            {
                public static implicit operator string(UserCodeTemporaryDisableTime_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_UserCodeTemporaryDisableTime.UserCodeTemporaryDisableTime_name, ClusterReported);
                }
            }
            public class UserCodeTemporaryDisableTime_SupportCommands
            {
                public static implicit operator string(UserCodeTemporaryDisableTime_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_SendPINOverTheAir
        {
            public const string SendPINOverTheAir_name = "SendPINOverTheAir";
            public class SendPINOverTheAir_Attributes
            {
                public static implicit operator string(SendPINOverTheAir_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SendPINOverTheAir command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SendPINOverTheAir.SendPINOverTheAir_name);
            }
            public class SendPINOverTheAir_Desired
            {
                public static implicit operator string(SendPINOverTheAir_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SendPINOverTheAir.SendPINOverTheAir_name, ClusterDesired);
                }
            }
            public class SendPINOverTheAir_Report
            {
                public static implicit operator string(SendPINOverTheAir_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SendPINOverTheAir.SendPINOverTheAir_name, ClusterReported);
                }
            }
            public class SendPINOverTheAir_SupportCommands
            {
                public static implicit operator string(SendPINOverTheAir_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_RequirePINforRFOperation
        {
            public const string RequirePINforRFOperation_name = "RequirePINforRFOperation";
            public class RequirePINforRFOperation_Attributes
            {
                public static implicit operator string(RequirePINforRFOperation_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RequirePINforRFOperation command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RequirePINforRFOperation.RequirePINforRFOperation_name);
            }
            public class RequirePINforRFOperation_Desired
            {
                public static implicit operator string(RequirePINforRFOperation_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RequirePINforRFOperation.RequirePINforRFOperation_name, ClusterDesired);
                }
            }
            public class RequirePINforRFOperation_Report
            {
                public static implicit operator string(RequirePINforRFOperation_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RequirePINforRFOperation.RequirePINforRFOperation_name, ClusterReported);
                }
            }
            public class RequirePINforRFOperation_SupportCommands
            {
                public static implicit operator string(RequirePINforRFOperation_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_SecurityLevel
        {
            public const string SecurityLevel_name = "SecurityLevel";
            public class SecurityLevel_Attributes
            {
                public static implicit operator string(SecurityLevel_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SecurityLevel command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SecurityLevel.SecurityLevel_name);
            }
            public class SecurityLevel_Desired
            {
                public static implicit operator string(SecurityLevel_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SecurityLevel.SecurityLevel_name, ClusterDesired);
                }
            }
            public class SecurityLevel_Report
            {
                public static implicit operator string(SecurityLevel_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_SecurityLevel.SecurityLevel_name, ClusterReported);
                }
            }
            public class SecurityLevel_SupportCommands
            {
                public static implicit operator string(SecurityLevel_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Network
                {
                    public const string Network_value = "00";
                    public const string Network_name = "Network";
                }

                public class APS
                {
                    public const string APS_value = "01";
                    public const string APS_name = "APS";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_AlarmMask
        {
            public const string AlarmMask_name = "AlarmMask";
            public class AlarmMask_Attributes
            {
                public static implicit operator string(AlarmMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AlarmMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_AlarmMask.AlarmMask_name);
            }
            public class AlarmMask_Desired
            {
                public static implicit operator string(AlarmMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_AlarmMask.AlarmMask_name, ClusterDesired);
                }
            }
            public class AlarmMask_Report
            {
                public static implicit operator string(AlarmMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_AlarmMask.AlarmMask_name, ClusterReported);
                }
            }
            public class AlarmMask_SupportCommands
            {
                public static implicit operator string(AlarmMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class DeadboltJammed
                {
                    public const string DeadboltJammed_name = "DeadboltJammed";
                }
                public class LockResetToFactoryDefaults
                {
                    public const string LockResetToFactoryDefaults_name = "LockResetToFactoryDefaults";
                }
                public class RFPowerModuleCycled
                {
                    public const string RFPowerModuleCycled_name = "RFPowerModuleCycled";
                }
                public class TamperAlarmWrongCodeEntryLimit
                {
                    public const string TamperAlarmWrongCodeEntryLimit_name = "TamperAlarmWrongCodeEntryLimit";
                }
                public class TamperAlarmFrontEscutcheonRemovedFromMain
                {
                    public const string TamperAlarmFrontEscutcheonRemovedFromMain_name = "TamperAlarmFrontEscutcheonRemovedFromMain";
                }
                public class ForcedDoorOpenUnderDoorLockedCondition
                {
                    public const string ForcedDoorOpenUnderDoorLockedCondition_name = "ForcedDoorOpenUnderDoorLockedCondition";
                }
            }
        }/*End of ATT class*/
        public class ATT_KeypadOperationEventMask
        {
            public const string KeypadOperationEventMask_name = "KeypadOperationEventMask";
            public class KeypadOperationEventMask_Attributes
            {
                public static implicit operator string(KeypadOperationEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_KeypadOperationEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_KeypadOperationEventMask.KeypadOperationEventMask_name);
            }
            public class KeypadOperationEventMask_Desired
            {
                public static implicit operator string(KeypadOperationEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_KeypadOperationEventMask.KeypadOperationEventMask_name, ClusterDesired);
                }
            }
            public class KeypadOperationEventMask_Report
            {
                public static implicit operator string(KeypadOperationEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_KeypadOperationEventMask.KeypadOperationEventMask_name, ClusterReported);
                }
            }
            public class KeypadOperationEventMask_SupportCommands
            {
                public static implicit operator string(KeypadOperationEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class KeypadOpUnknownOrMS
                {
                    public const string KeypadOpUnknownOrMS_name = "KeypadOpUnknownOrMS";
                }
                public class KeypadOpLock
                {
                    public const string KeypadOpLock_name = "KeypadOpLock";
                }
                public class KeypadOpUnlock
                {
                    public const string KeypadOpUnlock_name = "KeypadOpUnlock";
                }
                public class KeypadOpLockErrorInvalidPIN
                {
                    public const string KeypadOpLockErrorInvalidPIN_name = "KeypadOpLockErrorInvalidPIN";
                }
                public class KeypadOpLockErrorInvalidSchedule
                {
                    public const string KeypadOpLockErrorInvalidSchedule_name = "KeypadOpLockErrorInvalidSchedule";
                }
                public class KeypadOpUnlockInvalidPIN
                {
                    public const string KeypadOpUnlockInvalidPIN_name = "KeypadOpUnlockInvalidPIN";
                }
                public class KeypadOpUnlockInvalidSchedule
                {
                    public const string KeypadOpUnlockInvalidSchedule_name = "KeypadOpUnlockInvalidSchedule";
                }
                public class KeypadOpNonAccessUser
                {
                    public const string KeypadOpNonAccessUser_name = "KeypadOpNonAccessUser";
                }
            }
        }/*End of ATT class*/
        public class ATT_RFOperationEventMask
        {
            public const string RFOperationEventMask_name = "RFOperationEventMask";
            public class RFOperationEventMask_Attributes
            {
                public static implicit operator string(RFOperationEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RFOperationEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFOperationEventMask.RFOperationEventMask_name);
            }
            public class RFOperationEventMask_Desired
            {
                public static implicit operator string(RFOperationEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFOperationEventMask.RFOperationEventMask_name, ClusterDesired);
                }
            }
            public class RFOperationEventMask_Report
            {
                public static implicit operator string(RFOperationEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFOperationEventMask.RFOperationEventMask_name, ClusterReported);
                }
            }
            public class RFOperationEventMask_SupportCommands
            {
                public static implicit operator string(RFOperationEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class RFOpUnknownOrMS
                {
                    public const string RFOpUnknownOrMS_name = "RFOpUnknownOrMS";
                }
                public class RFOpLock
                {
                    public const string RFOpLock_name = "RFOpLock";
                }
                public class RFOpUnlock
                {
                    public const string RFOpUnlock_name = "RFOpUnlock";
                }
                public class RFOpLockErrorInvalidCode
                {
                    public const string RFOpLockErrorInvalidCode_name = "RFOpLockErrorInvalidCode";
                }
                public class RFOpLockErrorInvalidSchedule
                {
                    public const string RFOpLockErrorInvalidSchedule_name = "RFOpLockErrorInvalidSchedule";
                }
                public class RFOpUnlockInvalidCode
                {
                    public const string RFOpUnlockInvalidCode_name = "RFOpUnlockInvalidCode";
                }
                public class RFOpUnlockInvalidSchedule
                {
                    public const string RFOpUnlockInvalidSchedule_name = "RFOpUnlockInvalidSchedule";
                }
            }
        }/*End of ATT class*/
        public class ATT_ManualOperationEventMask
        {
            public const string ManualOperationEventMask_name = "ManualOperationEventMask";
            public class ManualOperationEventMask_Attributes
            {
                public static implicit operator string(ManualOperationEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ManualOperationEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_ManualOperationEventMask.ManualOperationEventMask_name);
            }
            public class ManualOperationEventMask_Desired
            {
                public static implicit operator string(ManualOperationEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_ManualOperationEventMask.ManualOperationEventMask_name, ClusterDesired);
                }
            }
            public class ManualOperationEventMask_Report
            {
                public static implicit operator string(ManualOperationEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_ManualOperationEventMask.ManualOperationEventMask_name, ClusterReported);
                }
            }
            public class ManualOperationEventMask_SupportCommands
            {
                public static implicit operator string(ManualOperationEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class ManualOpUnknownOrMS
                {
                    public const string ManualOpUnknownOrMS_name = "ManualOpUnknownOrMS";
                }
                public class ManualOpThumbturnLock
                {
                    public const string ManualOpThumbturnLock_name = "ManualOpThumbturnLock";
                }
                public class ManualOpThumbturnUnlock
                {
                    public const string ManualOpThumbturnUnlock_name = "ManualOpThumbturnUnlock";
                }
                public class ManualOpOneTouchLock
                {
                    public const string ManualOpOneTouchLock_name = "ManualOpOneTouchLock";
                }
                public class ManualOpKeyLock
                {
                    public const string ManualOpKeyLock_name = "ManualOpKeyLock";
                }
                public class ManualOpKeyUnlock
                {
                    public const string ManualOpKeyUnlock_name = "ManualOpKeyUnlock";
                }
                public class ManualOpAutoLock
                {
                    public const string ManualOpAutoLock_name = "ManualOpAutoLock";
                }
                public class ManualOpScheduleLock
                {
                    public const string ManualOpScheduleLock_name = "ManualOpScheduleLock";
                }
                public class ManualOpScheduleUnlock
                {
                    public const string ManualOpScheduleUnlock_name = "ManualOpScheduleUnlock";
                }
                public class ManualOpLock
                {
                    public const string ManualOpLock_name = "ManualOpLock";
                }
                public class ManualOpUnlock
                {
                    public const string ManualOpUnlock_name = "ManualOpUnlock";
                }
            }
        }/*End of ATT class*/
        public class ATT_RFIDOperationEventMask
        {
            public const string RFIDOperationEventMask_name = "RFIDOperationEventMask";
            public class RFIDOperationEventMask_Attributes
            {
                public static implicit operator string(RFIDOperationEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RFIDOperationEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFIDOperationEventMask.RFIDOperationEventMask_name);
            }
            public class RFIDOperationEventMask_Desired
            {
                public static implicit operator string(RFIDOperationEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFIDOperationEventMask.RFIDOperationEventMask_name, ClusterDesired);
                }
            }
            public class RFIDOperationEventMask_Report
            {
                public static implicit operator string(RFIDOperationEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFIDOperationEventMask.RFIDOperationEventMask_name, ClusterReported);
                }
            }
            public class RFIDOperationEventMask_SupportCommands
            {
                public static implicit operator string(RFIDOperationEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class RFIDOpUnknownOrMS
                {
                    public const string RFIDOpUnknownOrMS_name = "RFIDOpUnknownOrMS";
                }
                public class RFIDOpLock
                {
                    public const string RFIDOpLock_name = "RFIDOpLock";
                }
                public class RFIDOpUnlock
                {
                    public const string RFIDOpUnlock_name = "RFIDOpUnlock";
                }
                public class RFIDOpLockErrorInvalidRFID
                {
                    public const string RFIDOpLockErrorInvalidRFID_name = "RFIDOpLockErrorInvalidRFID";
                }
                public class RFIDOpLockErrorInvalidSchedule
                {
                    public const string RFIDOpLockErrorInvalidSchedule_name = "RFIDOpLockErrorInvalidSchedule";
                }
                public class RFIDOpUnlockErrorInvalidRFID
                {
                    public const string RFIDOpUnlockErrorInvalidRFID_name = "RFIDOpUnlockErrorInvalidRFID";
                }
                public class RFIDOpUnlockErrorInvalidSchedule
                {
                    public const string RFIDOpUnlockErrorInvalidSchedule_name = "RFIDOpUnlockErrorInvalidSchedule";
                }
            }
        }/*End of ATT class*/
        public class ATT_KeypadProgrammingEventMask
        {
            public const string KeypadProgrammingEventMask_name = "KeypadProgrammingEventMask";
            public class KeypadProgrammingEventMask_Attributes
            {
                public static implicit operator string(KeypadProgrammingEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_KeypadProgrammingEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_KeypadProgrammingEventMask.KeypadProgrammingEventMask_name);
            }
            public class KeypadProgrammingEventMask_Desired
            {
                public static implicit operator string(KeypadProgrammingEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_KeypadProgrammingEventMask.KeypadProgrammingEventMask_name, ClusterDesired);
                }
            }
            public class KeypadProgrammingEventMask_Report
            {
                public static implicit operator string(KeypadProgrammingEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_KeypadProgrammingEventMask.KeypadProgrammingEventMask_name, ClusterReported);
                }
            }
            public class KeypadProgrammingEventMask_SupportCommands
            {
                public static implicit operator string(KeypadProgrammingEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class KeypadProgUnknownOrMS
                {
                    public const string KeypadProgUnknownOrMS_name = "KeypadProgUnknownOrMS";
                }
                public class KeypadProgMasterCodeChanged
                {
                    public const string KeypadProgMasterCodeChanged_name = "KeypadProgMasterCodeChanged";
                }
                public class KeypadProgPINAdded
                {
                    public const string KeypadProgPINAdded_name = "KeypadProgPINAdded";
                }
                public class KeypadProgPINDeleted
                {
                    public const string KeypadProgPINDeleted_name = "KeypadProgPINDeleted";
                }
                public class KeypadProgPINChanged
                {
                    public const string KeypadProgPINChanged_name = "KeypadProgPINChanged";
                }
            }
        }/*End of ATT class*/
        public class ATT_RFProgrammingEventMask
        {
            public const string RFProgrammingEventMask_name = "RFProgrammingEventMask";
            public class RFProgrammingEventMask_Attributes
            {
                public static implicit operator string(RFProgrammingEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RFProgrammingEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFProgrammingEventMask.RFProgrammingEventMask_name);
            }
            public class RFProgrammingEventMask_Desired
            {
                public static implicit operator string(RFProgrammingEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFProgrammingEventMask.RFProgrammingEventMask_name, ClusterDesired);
                }
            }
            public class RFProgrammingEventMask_Report
            {
                public static implicit operator string(RFProgrammingEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFProgrammingEventMask.RFProgrammingEventMask_name, ClusterReported);
                }
            }
            public class RFProgrammingEventMask_SupportCommands
            {
                public static implicit operator string(RFProgrammingEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class RFProgUnknownOrMS
                {
                    public const string RFProgUnknownOrMS_name = "RFProgUnknownOrMS";
                }
                public class RFProgPINAdded
                {
                    public const string RFProgPINAdded_name = "RFProgPINAdded";
                }
                public class RFProgPINDeleted
                {
                    public const string RFProgPINDeleted_name = "RFProgPINDeleted";
                }
                public class RFProgPINChanged
                {
                    public const string RFProgPINChanged_name = "RFProgPINChanged";
                }
                public class RFProgRFIDAdded
                {
                    public const string RFProgRFIDAdded_name = "RFProgRFIDAdded";
                }
                public class RFProgRFIDDeleted
                {
                    public const string RFProgRFIDDeleted_name = "RFProgRFIDDeleted";
                }
            }
        }/*End of ATT class*/
        public class ATT_RFIDProgrammingEventMask
        {
            public const string RFIDProgrammingEventMask_name = "RFIDProgrammingEventMask";
            public class RFIDProgrammingEventMask_Attributes
            {
                public static implicit operator string(RFIDProgrammingEventMask_Attributes command)
                {
                    return SumAllAtt(DoorLockCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RFIDProgrammingEventMask command)
            {
                return SumATTStr(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFIDProgrammingEventMask.RFIDProgrammingEventMask_name);
            }
            public class RFIDProgrammingEventMask_Desired
            {
                public static implicit operator string(RFIDProgrammingEventMask_Desired command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFIDProgrammingEventMask.RFIDProgrammingEventMask_name, ClusterDesired);
                }
            }
            public class RFIDProgrammingEventMask_Report
            {
                public static implicit operator string(RFIDProgrammingEventMask_Report command)
                {
                    return SumATTStr_WithSubValue(DoorLockCluster.ClusterName, DoorLockCluster.ATT_RFIDProgrammingEventMask.RFIDProgrammingEventMask_name, ClusterReported);
                }
            }
            public class RFIDProgrammingEventMask_SupportCommands
            {
                public static implicit operator string(RFIDProgrammingEventMask_SupportCommands command)
                {
                    return SumSupCMD(DoorLockCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class RFIDProgUnknownOrMS
                {
                    public const string RFIDProgUnknownOrMS_name = "RFIDProgUnknownOrMS";
                }
                public class RFIDProgRFIDAdded
                {
                    public const string RFIDProgRFIDAdded_name = "RFIDProgRFIDAdded";
                }
                public class RFIDProgRFIDDeleted
                {
                    public const string RFIDProgRFIDDeleted_name = "RFIDProgRFIDDeleted";
                }
            }
        }/*End of ATT class*/
        #endregion

        #region Cluster Commands

        public class CMD_LockDoor
        {
            public const string LockDoor_name = "LockDoor";
            public static implicit operator string(CMD_LockDoor command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_LockDoor.LockDoor_name);
            }
            public class CMD_LockDoor_Payload
            {
                public string PINOrRFIDCode { get; set; }
                /*Start of public Payload*/
                public CMD_LockDoor_Payload(string my_PINOrRFIDCode)
                {
                    PINOrRFIDCode = my_PINOrRFIDCode;

                }
                /*End of public Payload*/
                public static implicit operator CMD_LockDoor_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_LockDoor_Payload>(command);
                }
                public static implicit operator string(CMD_LockDoor_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_UnlockDoor
        {
            public const string UnlockDoor_name = "UnlockDoor";
            public static implicit operator string(CMD_UnlockDoor command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_UnlockDoor.UnlockDoor_name);
            }
            public class CMD_UnlockDoor_Payload
            {
                public string PINOrRFIDCode { get; set; }
                /*Start of public Payload*/
                public CMD_UnlockDoor_Payload(string my_PINOrRFIDCode)
                {
                    PINOrRFIDCode = my_PINOrRFIDCode;

                }
                /*End of public Payload*/
                public static implicit operator CMD_UnlockDoor_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_UnlockDoor_Payload>(command);
                }
                public static implicit operator string(CMD_UnlockDoor_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_Toggle
        {
            public const string Toggle_name = "Toggle";
            public static implicit operator string(CMD_Toggle command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_Toggle.Toggle_name);
            }
            public class CMD_Toggle_Payload
            {
                public string PINOrRFIDCode { get; set; }
                /*Start of public Payload*/
                public CMD_Toggle_Payload(string my_PINOrRFIDCode)
                {
                    PINOrRFIDCode = my_PINOrRFIDCode;

                }
                /*End of public Payload*/
                public static implicit operator CMD_Toggle_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_Toggle_Payload>(command);
                }
                public static implicit operator string(CMD_Toggle_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_UnlockWithTimeout
        {
            public const string UnlockWithTimeout_name = "UnlockWithTimeout";
            public static implicit operator string(CMD_UnlockWithTimeout command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_UnlockWithTimeout.UnlockWithTimeout_name);
            }
            public class CMD_UnlockWithTimeout_Payload
            {
                public string TimeoutInSeconds { get; set; }
                public string PINOrRFIDCode { get; set; }
                /*Start of public Payload*/
                public CMD_UnlockWithTimeout_Payload(string my_TimeoutInSeconds, string my_PINOrRFIDCode)
                {
                    TimeoutInSeconds = my_TimeoutInSeconds;
                    PINOrRFIDCode = my_PINOrRFIDCode;

                }
                /*End of public Payload*/
                public static implicit operator CMD_UnlockWithTimeout_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_UnlockWithTimeout_Payload>(command);
                }
                public static implicit operator string(CMD_UnlockWithTimeout_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetLogRecord
        {
            public const string GetLogRecord_name = "GetLogRecord";
            public static implicit operator string(CMD_GetLogRecord command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetLogRecord.GetLogRecord_name);
            }

            public class CMD_GetLogRecord_Payload
            {
                public struct LogIndex_MostRecent
                {
                    public string MostRecent { get; set; }
                }

                public class LogIndex
                {
                    public LogIndex_MostRecent my_MostRecent { get; set; }

                }

                /*Start of public Payload*/
                public CMD_GetLogRecord_Payload(LogIndex my_LogIndex)
                {
                    LogIndex LogIndex_payload = my_LogIndex;
                }/*End of public Payload*/
                public static implicit operator CMD_GetLogRecord_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetLogRecord_Payload>(command);
                }
                public static implicit operator string(CMD_GetLogRecord_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetPINCode
        {
            public const string SetPINCode_name = "SetPINCode";
            public static implicit operator string(CMD_SetPINCode command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetPINCode.SetPINCode_name);
            }
            public class CMD_SetPINCode_Payload
            {
                public string UserID { get; set; }
                public string UserStatus { get; set; }
                public string UserType { get; set; }
                public string PIN { get; set; }
                /*Start of public Payload*/
                public CMD_SetPINCode_Payload(string my_UserID, string my_UserStatus, string my_UserType, string my_PIN)
                {
                    UserID = my_UserID;
                    UserStatus = my_UserStatus;
                    UserType = my_UserType;
                    PIN = my_PIN;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetPINCode_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetPINCode_Payload>(command);
                }
                public static implicit operator string(CMD_SetPINCode_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetPINCode
        {
            public const string GetPINCode_name = "GetPINCode";
            public static implicit operator string(CMD_GetPINCode command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetPINCode.GetPINCode_name);
            }
            public class CMD_GetPINCode_Payload
            {
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_GetPINCode_Payload(string my_UserID)
                {
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetPINCode_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetPINCode_Payload>(command);
                }
                public static implicit operator string(CMD_GetPINCode_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearPINCode
        {
            public const string ClearPINCode_name = "ClearPINCode";
            public static implicit operator string(CMD_ClearPINCode command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearPINCode.ClearPINCode_name);
            }
            public class CMD_ClearPINCode_Payload
            {
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_ClearPINCode_Payload(string my_UserID)
                {
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_ClearPINCode_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ClearPINCode_Payload>(command);
                }
                public static implicit operator string(CMD_ClearPINCode_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearAllPINCodes
        {
            public const string ClearAllPINCodes_name = "ClearAllPINCodes";
            public static implicit operator string(CMD_ClearAllPINCodes command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearAllPINCodes.ClearAllPINCodes_name);
            }
        }/*End of CMD class*/
        public class CMD_SetUserStatus
        {
            public const string SetUserStatus_name = "SetUserStatus";
            public static implicit operator string(CMD_SetUserStatus command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetUserStatus.SetUserStatus_name);
            }
            public class CMD_SetUserStatus_Payload
            {
                public string UserID { get; set; }
                public string UserStatus { get; set; }
                /*Start of public Payload*/
                public CMD_SetUserStatus_Payload(string my_UserID, string my_UserStatus)
                {
                    UserID = my_UserID;
                    UserStatus = my_UserStatus;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetUserStatus_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetUserStatus_Payload>(command);
                }
                public static implicit operator string(CMD_SetUserStatus_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetUserStatus
        {
            public const string GetUserStatus_name = "GetUserStatus";
            public static implicit operator string(CMD_GetUserStatus command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetUserStatus.GetUserStatus_name);
            }
            public class CMD_GetUserStatus_Payload
            {
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_GetUserStatus_Payload(string my_UserID)
                {
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetUserStatus_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetUserStatus_Payload>(command);
                }
                public static implicit operator string(CMD_GetUserStatus_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetWeekdaySchedule
        {
            public const string SetWeekdaySchedule_name = "SetWeekdaySchedule";
            public static implicit operator string(CMD_SetWeekdaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetWeekdaySchedule.SetWeekdaySchedule_name);
            }




            public class CMD_SetWeekdaySchedule_Payload
            {
                public string ScheduleID { get; set; }
                public string UserID { get; set; }
                public string DaysMask { get; set; }
                public string StartHour { get; set; }
                public string StartMinute { get; set; }
                public string EndHour { get; set; }
                public string EndMinute { get; set; }
                /*Start of public Payload*/
                public CMD_SetWeekdaySchedule_Payload(string my_ScheduleID, string my_UserID, string my_DaysMask, string my_StartHour, string my_StartMinute, string my_EndHour, string my_EndMinute)
                {
                    ScheduleID = my_ScheduleID;
                    UserID = my_UserID;
                    DaysMask = my_DaysMask;
                    StartHour = my_StartHour;
                    StartMinute = my_StartMinute;
                    EndHour = my_EndHour;
                    EndMinute = my_EndMinute;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetWeekdaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetWeekdaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_SetWeekdaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetWeekdaySchedule
        {
            public const string GetWeekdaySchedule_name = "GetWeekdaySchedule";
            public static implicit operator string(CMD_GetWeekdaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetWeekdaySchedule.GetWeekdaySchedule_name);
            }
            public class CMD_GetWeekdaySchedule_Payload
            {
                public string ScheduleID { get; set; }
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_GetWeekdaySchedule_Payload(string my_ScheduleID, string my_UserID)
                {
                    ScheduleID = my_ScheduleID;
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetWeekdaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetWeekdaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_GetWeekdaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearWeekdaySchedule
        {
            public const string ClearWeekdaySchedule_name = "ClearWeekdaySchedule";
            public static implicit operator string(CMD_ClearWeekdaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearWeekdaySchedule.ClearWeekdaySchedule_name);
            }
            public class CMD_ClearWeekdaySchedule_Payload
            {
                public string ScheduleID { get; set; }
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_ClearWeekdaySchedule_Payload(string my_ScheduleID, string my_UserID)
                {
                    ScheduleID = my_ScheduleID;
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_ClearWeekdaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ClearWeekdaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_ClearWeekdaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetYearDaySchedule
        {
            public const string SetYearDaySchedule_name = "SetYearDaySchedule";
            public static implicit operator string(CMD_SetYearDaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetYearDaySchedule.SetYearDaySchedule_name);
            }

            public class CMD_SetYearDaySchedule_Payload
            {
                public string ScheduleID { get; set; }
                public string UserID { get; set; }
                public string LocalStartTime { get; set; }
                public string LocalEndTime { get; set; }
                /*Start of public Payload*/
                public CMD_SetYearDaySchedule_Payload(string my_ScheduleID, string my_UserID, string my_LocalStartTime, string my_LocalEndTime)
                {
                    ScheduleID = my_ScheduleID;
                    UserID = my_UserID;
                    LocalStartTime = my_LocalStartTime;
                    LocalEndTime = my_LocalEndTime;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetYearDaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetYearDaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_SetYearDaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetYearDaySchedule
        {
            public const string GetYearDaySchedule_name = "GetYearDaySchedule";
            public static implicit operator string(CMD_GetYearDaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetYearDaySchedule.GetYearDaySchedule_name);
            }
            public class CMD_GetYearDaySchedule_Payload
            {
                public string ScheduleID { get; set; }
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_GetYearDaySchedule_Payload(string my_ScheduleID, string my_UserID)
                {
                    ScheduleID = my_ScheduleID;
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetYearDaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetYearDaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_GetYearDaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearYearDaySchedule
        {
            public const string ClearYearDaySchedule_name = "ClearYearDaySchedule";
            public static implicit operator string(CMD_ClearYearDaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearYearDaySchedule.ClearYearDaySchedule_name);
            }
            public class CMD_ClearYearDaySchedule_Payload
            {
                public string ScheduleID { get; set; }
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_ClearYearDaySchedule_Payload(string my_ScheduleID, string my_UserID)
                {
                    ScheduleID = my_ScheduleID;
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_ClearYearDaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ClearYearDaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_ClearYearDaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetHolidaySchedule
        {
            public const string SetHolidaySchedule_name = "SetHolidaySchedule";
            public static implicit operator string(CMD_SetHolidaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetHolidaySchedule.SetHolidaySchedule_name);
            }

            public class CMD_SetHolidaySchedule_Payload
            {
                public string HolidayScheduleID { get; set; }
                public string LocalStartTime { get; set; }
                public string LocalEndTime { get; set; }
                public string OperatingModeDuringHoliday { get; set; }
                /*Start of public Payload*/
                public CMD_SetHolidaySchedule_Payload(string my_HolidayScheduleID, string my_LocalStartTime, string my_LocalEndTime, string my_OperatingModeDuringHoliday)
                {
                    HolidayScheduleID = my_HolidayScheduleID;
                    LocalStartTime = my_LocalStartTime;
                    LocalEndTime = my_LocalEndTime;
                    OperatingModeDuringHoliday = my_OperatingModeDuringHoliday;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetHolidaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetHolidaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_SetHolidaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetHolidaySchedule
        {
            public const string GetHolidaySchedule_name = "GetHolidaySchedule";
            public static implicit operator string(CMD_GetHolidaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetHolidaySchedule.GetHolidaySchedule_name);
            }
            public class CMD_GetHolidaySchedule_Payload
            {
                public string HolidayScheduleID { get; set; }
                /*Start of public Payload*/
                public CMD_GetHolidaySchedule_Payload(string my_HolidayScheduleID)
                {
                    HolidayScheduleID = my_HolidayScheduleID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetHolidaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetHolidaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_GetHolidaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearHolidaySchedule
        {
            public const string ClearHolidaySchedule_name = "ClearHolidaySchedule";
            public static implicit operator string(CMD_ClearHolidaySchedule command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearHolidaySchedule.ClearHolidaySchedule_name);
            }
            public class CMD_ClearHolidaySchedule_Payload
            {
                public string HolidayScheduleID { get; set; }
                /*Start of public Payload*/
                public CMD_ClearHolidaySchedule_Payload(string my_HolidayScheduleID)
                {
                    HolidayScheduleID = my_HolidayScheduleID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_ClearHolidaySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ClearHolidaySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_ClearHolidaySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetUserType
        {
            public const string SetUserType_name = "SetUserType";
            public static implicit operator string(CMD_SetUserType command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetUserType.SetUserType_name);
            }
            public class CMD_SetUserType_Payload
            {
                public string UserID { get; set; }
                public string UserType { get; set; }
                /*Start of public Payload*/
                public CMD_SetUserType_Payload(string my_UserID, string my_UserType)
                {
                    UserID = my_UserID;
                    UserType = my_UserType;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetUserType_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetUserType_Payload>(command);
                }
                public static implicit operator string(CMD_SetUserType_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetUserType
        {
            public const string GetUserType_name = "GetUserType";
            public static implicit operator string(CMD_GetUserType command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetUserType.GetUserType_name);
            }
            public class CMD_GetUserType_Payload
            {
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_GetUserType_Payload(string my_UserID)
                {
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetUserType_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetUserType_Payload>(command);
                }
                public static implicit operator string(CMD_GetUserType_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetRFIDCode
        {
            public const string SetRFIDCode_name = "SetRFIDCode";
            public static implicit operator string(CMD_SetRFIDCode command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_SetRFIDCode.SetRFIDCode_name);
            }
            public class CMD_SetRFIDCode_Payload
            {
                public string UserID { get; set; }
                public string UserStatus { get; set; }
                public string UserType { get; set; }
                public string RFIDCode { get; set; }
                /*Start of public Payload*/
                public CMD_SetRFIDCode_Payload(string my_UserID, string my_UserStatus, string my_UserType, string my_RFIDCode)
                {
                    UserID = my_UserID;
                    UserStatus = my_UserStatus;
                    UserType = my_UserType;
                    RFIDCode = my_RFIDCode;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetRFIDCode_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetRFIDCode_Payload>(command);
                }
                public static implicit operator string(CMD_SetRFIDCode_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetRFIDCode
        {
            public const string GetRFIDCode_name = "GetRFIDCode";
            public static implicit operator string(CMD_GetRFIDCode command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_GetRFIDCode.GetRFIDCode_name);
            }
            public class CMD_GetRFIDCode_Payload
            {
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_GetRFIDCode_Payload(string my_UserID)
                {
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetRFIDCode_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetRFIDCode_Payload>(command);
                }
                public static implicit operator string(CMD_GetRFIDCode_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearRFIDCode
        {
            public const string ClearRFIDCode_name = "ClearRFIDCode";
            public static implicit operator string(CMD_ClearRFIDCode command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearRFIDCode.ClearRFIDCode_name);
            }
            public class CMD_ClearRFIDCode_Payload
            {
                public string UserID { get; set; }
                /*Start of public Payload*/
                public CMD_ClearRFIDCode_Payload(string my_UserID)
                {
                    UserID = my_UserID;

                }
                /*End of public Payload*/
                public static implicit operator CMD_ClearRFIDCode_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ClearRFIDCode_Payload>(command);
                }
                public static implicit operator string(CMD_ClearRFIDCode_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearAllRFIDCodes
        {
            public const string ClearAllRFIDCodes_name = "ClearAllRFIDCodes";
            public static implicit operator string(CMD_ClearAllRFIDCodes command)
            {
                return SumCMDStr(DoorLockCluster.ClusterName, DoorLockCluster.CMD_ClearAllRFIDCodes.ClearAllRFIDCodes_name);
            }
        }/*End of CMD class*/
        #endregion

        #region Cluster Operations

        public struct Payload_Att_SupportedCommands
        {
            public class Payload
            {
                public List<string> value { get; set; }
            }

            public Payload _payload;

            public Payload_Att_SupportedCommands(Payload payload)
            {
                _payload = payload;
            }
            public static implicit operator Payload_Att_SupportedCommands(string options)
            {
                Payload deserialized = JsonConvert.DeserializeObject<Payload>(options);
                return new Payload_Att_SupportedCommands(deserialized);
            }
        }

        public struct Payload_Att_Value
        {
            public string value;

            public static implicit operator Payload_Att_Value(string options)
            {
                return JsonConvert.DeserializeObject<Payload_Att_Value>(options);
            }
        }

        public static implicit operator DoorLockCluster(string options)
        {
            return JsonConvert.DeserializeObject<DoorLockCluster>(options);
        }
        public static implicit operator string(DoorLockCluster command)
        {
            return DoorLockCluster.ClusterName;
        }
        private static string SumAllAtt(string AttClusterName, string Att)
        {
            string SumStr = "/" + AttClusterName + "/" + Att + "/" + "#";
            return SumStr;
        }
        private static string SumCMDStr(string ClusterName, string CmdName)
        {
            string SumStr = "/" + ClusterName + "/" + "Commands" + "/" + CmdName;
            return SumStr;
        }

        private static string SumATTStr(string ClusterName, string AttName)
        {
            string SumStr = "/" + ClusterName + "/" + "Attributes" + "/" + AttName;
            return SumStr;
        }
        private static string SumATTStr_WithSubValue(string ClusterName, string AttName, string SubValue)
        {
            string SumStr = SumATTStr(ClusterName, AttName) + "/" + SubValue;
            return SumStr;
        }
        private static string SumSupCMD(string ClusterName, string SupCMD)
        {
            string SumStr = "/" + ClusterName + "/" + SupCMD;
            return SumStr;
        }
        #endregion
    }
}
