/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Newtonsoft.Json;

namespace UicApplication.Clusters
{
    public class ThermostatCluster
    {
        public const string ClusterName = "Thermostat";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_LocalTemperature
        {
            public const string LocalTemperature_name = "LocalTemperature";
            public class LocalTemperature_Attributes
            {
                public static implicit operator string(LocalTemperature_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_LocalTemperature command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_LocalTemperature.LocalTemperature_name);
            }
            public class LocalTemperature_Desired
            {
                public static implicit operator string(LocalTemperature_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_LocalTemperature.LocalTemperature_name, ClusterDesired);
                }
            }
            public class LocalTemperature_Report
            {
                public static implicit operator string(LocalTemperature_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_LocalTemperature.LocalTemperature_name, ClusterReported);
                }
            }
            public class LocalTemperature_SupportCommands
            {
                public static implicit operator string(LocalTemperature_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OutdoorTemperature
        {
            public const string OutdoorTemperature_name = "OutdoorTemperature";
            public class OutdoorTemperature_Attributes
            {
                public static implicit operator string(OutdoorTemperature_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OutdoorTemperature command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OutdoorTemperature.OutdoorTemperature_name);
            }
            public class OutdoorTemperature_Desired
            {
                public static implicit operator string(OutdoorTemperature_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OutdoorTemperature.OutdoorTemperature_name, ClusterDesired);
                }
            }
            public class OutdoorTemperature_Report
            {
                public static implicit operator string(OutdoorTemperature_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OutdoorTemperature.OutdoorTemperature_name, ClusterReported);
                }
            }
            public class OutdoorTemperature_SupportCommands
            {
                public static implicit operator string(OutdoorTemperature_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_Occupancy
        {
            public const string Occupancy_name = "Occupancy";
            public class Occupancy_Attributes
            {
                public static implicit operator string(Occupancy_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_Occupancy command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_Occupancy.Occupancy_name);
            }
            public class Occupancy_Desired
            {
                public static implicit operator string(Occupancy_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_Occupancy.Occupancy_name, ClusterDesired);
                }
            }
            public class Occupancy_Report
            {
                public static implicit operator string(Occupancy_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_Occupancy.Occupancy_name, ClusterReported);
                }
            }
            public class Occupancy_SupportCommands
            {
                public static implicit operator string(Occupancy_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class Occupied
                {
                    public const string Occupied_name = "Occupied";
                }
            }
        }/*End of ATT class*/
        public class ATT_AbsMinHeatSetpointLimit
        {
            public const string AbsMinHeatSetpointLimit_name = "AbsMinHeatSetpointLimit";
            public class AbsMinHeatSetpointLimit_Attributes
            {
                public static implicit operator string(AbsMinHeatSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AbsMinHeatSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMinHeatSetpointLimit.AbsMinHeatSetpointLimit_name);
            }
            public class AbsMinHeatSetpointLimit_Desired
            {
                public static implicit operator string(AbsMinHeatSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMinHeatSetpointLimit.AbsMinHeatSetpointLimit_name, ClusterDesired);
                }
            }
            public class AbsMinHeatSetpointLimit_Report
            {
                public static implicit operator string(AbsMinHeatSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMinHeatSetpointLimit.AbsMinHeatSetpointLimit_name, ClusterReported);
                }
            }
            public class AbsMinHeatSetpointLimit_SupportCommands
            {
                public static implicit operator string(AbsMinHeatSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_AbsMaxHeatSetpointLimit
        {
            public const string AbsMaxHeatSetpointLimit_name = "AbsMaxHeatSetpointLimit";
            public class AbsMaxHeatSetpointLimit_Attributes
            {
                public static implicit operator string(AbsMaxHeatSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AbsMaxHeatSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMaxHeatSetpointLimit.AbsMaxHeatSetpointLimit_name);
            }
            public class AbsMaxHeatSetpointLimit_Desired
            {
                public static implicit operator string(AbsMaxHeatSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMaxHeatSetpointLimit.AbsMaxHeatSetpointLimit_name, ClusterDesired);
                }
            }
            public class AbsMaxHeatSetpointLimit_Report
            {
                public static implicit operator string(AbsMaxHeatSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMaxHeatSetpointLimit.AbsMaxHeatSetpointLimit_name, ClusterReported);
                }
            }
            public class AbsMaxHeatSetpointLimit_SupportCommands
            {
                public static implicit operator string(AbsMaxHeatSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_AbsMinCoolSetpointLimit
        {
            public const string AbsMinCoolSetpointLimit_name = "AbsMinCoolSetpointLimit";
            public class AbsMinCoolSetpointLimit_Attributes
            {
                public static implicit operator string(AbsMinCoolSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AbsMinCoolSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMinCoolSetpointLimit.AbsMinCoolSetpointLimit_name);
            }
            public class AbsMinCoolSetpointLimit_Desired
            {
                public static implicit operator string(AbsMinCoolSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMinCoolSetpointLimit.AbsMinCoolSetpointLimit_name, ClusterDesired);
                }
            }
            public class AbsMinCoolSetpointLimit_Report
            {
                public static implicit operator string(AbsMinCoolSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMinCoolSetpointLimit.AbsMinCoolSetpointLimit_name, ClusterReported);
                }
            }
            public class AbsMinCoolSetpointLimit_SupportCommands
            {
                public static implicit operator string(AbsMinCoolSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_AbsMaxCoolSetpointLimit
        {
            public const string AbsMaxCoolSetpointLimit_name = "AbsMaxCoolSetpointLimit";
            public class AbsMaxCoolSetpointLimit_Attributes
            {
                public static implicit operator string(AbsMaxCoolSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AbsMaxCoolSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMaxCoolSetpointLimit.AbsMaxCoolSetpointLimit_name);
            }
            public class AbsMaxCoolSetpointLimit_Desired
            {
                public static implicit operator string(AbsMaxCoolSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMaxCoolSetpointLimit.AbsMaxCoolSetpointLimit_name, ClusterDesired);
                }
            }
            public class AbsMaxCoolSetpointLimit_Report
            {
                public static implicit operator string(AbsMaxCoolSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AbsMaxCoolSetpointLimit.AbsMaxCoolSetpointLimit_name, ClusterReported);
                }
            }
            public class AbsMaxCoolSetpointLimit_SupportCommands
            {
                public static implicit operator string(AbsMaxCoolSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_PICoolingDemand
        {
            public const string PICoolingDemand_name = "PICoolingDemand";
            public class PICoolingDemand_Attributes
            {
                public static implicit operator string(PICoolingDemand_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_PICoolingDemand command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_PICoolingDemand.PICoolingDemand_name);
            }
            public class PICoolingDemand_Desired
            {
                public static implicit operator string(PICoolingDemand_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_PICoolingDemand.PICoolingDemand_name, ClusterDesired);
                }
            }
            public class PICoolingDemand_Report
            {
                public static implicit operator string(PICoolingDemand_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_PICoolingDemand.PICoolingDemand_name, ClusterReported);
                }
            }
            public class PICoolingDemand_SupportCommands
            {
                public static implicit operator string(PICoolingDemand_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_PIHeatingDemand
        {
            public const string PIHeatingDemand_name = "PIHeatingDemand";
            public class PIHeatingDemand_Attributes
            {
                public static implicit operator string(PIHeatingDemand_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_PIHeatingDemand command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_PIHeatingDemand.PIHeatingDemand_name);
            }
            public class PIHeatingDemand_Desired
            {
                public static implicit operator string(PIHeatingDemand_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_PIHeatingDemand.PIHeatingDemand_name, ClusterDesired);
                }
            }
            public class PIHeatingDemand_Report
            {
                public static implicit operator string(PIHeatingDemand_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_PIHeatingDemand.PIHeatingDemand_name, ClusterReported);
                }
            }
            public class PIHeatingDemand_SupportCommands
            {
                public static implicit operator string(PIHeatingDemand_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_HVACSystemTypeConfiguration
        {
            public const string HVACSystemTypeConfiguration_name = "HVACSystemTypeConfiguration";
            public class HVACSystemTypeConfiguration_Attributes
            {
                public static implicit operator string(HVACSystemTypeConfiguration_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_HVACSystemTypeConfiguration command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_HVACSystemTypeConfiguration.HVACSystemTypeConfiguration_name);
            }
            public class HVACSystemTypeConfiguration_Desired
            {
                public static implicit operator string(HVACSystemTypeConfiguration_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_HVACSystemTypeConfiguration.HVACSystemTypeConfiguration_name, ClusterDesired);
                }
            }
            public class HVACSystemTypeConfiguration_Report
            {
                public static implicit operator string(HVACSystemTypeConfiguration_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_HVACSystemTypeConfiguration.HVACSystemTypeConfiguration_name, ClusterReported);
                }
            }
            public class HVACSystemTypeConfiguration_SupportCommands
            {
                public static implicit operator string(HVACSystemTypeConfiguration_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class CoolingSystemStage
                {
                    public const string CoolingSystemStage_name = "CoolingSystemStage";
                }
                public class HeatingSystemStage
                {
                    public const string HeatingSystemStage_name = "HeatingSystemStage";
                }
                public class HeatingSystemType
                {
                    public const string HeatingSystemType_name = "HeatingSystemType";
                }
                public class HeatingFuelSource
                {
                    public const string HeatingFuelSource_name = "HeatingFuelSource";
                }
            }
        }/*End of ATT class*/
        public class ATT_LocalTemperatureCalibration
        {
            public const string LocalTemperatureCalibration_name = "LocalTemperatureCalibration";
            public class LocalTemperatureCalibration_Attributes
            {
                public static implicit operator string(LocalTemperatureCalibration_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_LocalTemperatureCalibration command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_LocalTemperatureCalibration.LocalTemperatureCalibration_name);
            }
            public class LocalTemperatureCalibration_Desired
            {
                public static implicit operator string(LocalTemperatureCalibration_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_LocalTemperatureCalibration.LocalTemperatureCalibration_name, ClusterDesired);
                }
            }
            public class LocalTemperatureCalibration_Report
            {
                public static implicit operator string(LocalTemperatureCalibration_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_LocalTemperatureCalibration.LocalTemperatureCalibration_name, ClusterReported);
                }
            }
            public class LocalTemperatureCalibration_SupportCommands
            {
                public static implicit operator string(LocalTemperatureCalibration_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OccupiedCoolingSetpoint
        {
            public const string OccupiedCoolingSetpoint_name = "OccupiedCoolingSetpoint";
            public class OccupiedCoolingSetpoint_Attributes
            {
                public static implicit operator string(OccupiedCoolingSetpoint_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OccupiedCoolingSetpoint command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedCoolingSetpoint.OccupiedCoolingSetpoint_name);
            }
            public class OccupiedCoolingSetpoint_Desired
            {
                public static implicit operator string(OccupiedCoolingSetpoint_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedCoolingSetpoint.OccupiedCoolingSetpoint_name, ClusterDesired);
                }
            }
            public class OccupiedCoolingSetpoint_Report
            {
                public static implicit operator string(OccupiedCoolingSetpoint_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedCoolingSetpoint.OccupiedCoolingSetpoint_name, ClusterReported);
                }
            }
            public class OccupiedCoolingSetpoint_SupportCommands
            {
                public static implicit operator string(OccupiedCoolingSetpoint_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinCoolSetpointLimit_ref = "MinCoolSetpointLimit";

                public const string MaxCoolSetpointLimit_ref = "MaxCoolSetpointLimit";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_OccupiedHeatingSetpoint
        {
            public const string OccupiedHeatingSetpoint_name = "OccupiedHeatingSetpoint";
            public class OccupiedHeatingSetpoint_Attributes
            {
                public static implicit operator string(OccupiedHeatingSetpoint_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OccupiedHeatingSetpoint command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedHeatingSetpoint.OccupiedHeatingSetpoint_name);
            }
            public class OccupiedHeatingSetpoint_Desired
            {
                public static implicit operator string(OccupiedHeatingSetpoint_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedHeatingSetpoint.OccupiedHeatingSetpoint_name, ClusterDesired);
                }
            }
            public class OccupiedHeatingSetpoint_Report
            {
                public static implicit operator string(OccupiedHeatingSetpoint_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedHeatingSetpoint.OccupiedHeatingSetpoint_name, ClusterReported);
                }
            }
            public class OccupiedHeatingSetpoint_SupportCommands
            {
                public static implicit operator string(OccupiedHeatingSetpoint_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinHeatSetpointLimit_ref = "MinHeatSetpointLimit";

                public const string MaxHeatSetpointLimit_ref = "MaxHeatSetpointLimit";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_UnoccupiedCoolingSetpoint
        {
            public const string UnoccupiedCoolingSetpoint_name = "UnoccupiedCoolingSetpoint";
            public class UnoccupiedCoolingSetpoint_Attributes
            {
                public static implicit operator string(UnoccupiedCoolingSetpoint_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_UnoccupiedCoolingSetpoint command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedCoolingSetpoint.UnoccupiedCoolingSetpoint_name);
            }
            public class UnoccupiedCoolingSetpoint_Desired
            {
                public static implicit operator string(UnoccupiedCoolingSetpoint_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedCoolingSetpoint.UnoccupiedCoolingSetpoint_name, ClusterDesired);
                }
            }
            public class UnoccupiedCoolingSetpoint_Report
            {
                public static implicit operator string(UnoccupiedCoolingSetpoint_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedCoolingSetpoint.UnoccupiedCoolingSetpoint_name, ClusterReported);
                }
            }
            public class UnoccupiedCoolingSetpoint_SupportCommands
            {
                public static implicit operator string(UnoccupiedCoolingSetpoint_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinCoolSetpointLimit_ref = "MinCoolSetpointLimit";

                public const string MaxCoolSetpointLimit_ref = "MaxCoolSetpointLimit";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_UnoccupiedHeatingSetpoint
        {
            public const string UnoccupiedHeatingSetpoint_name = "UnoccupiedHeatingSetpoint";
            public class UnoccupiedHeatingSetpoint_Attributes
            {
                public static implicit operator string(UnoccupiedHeatingSetpoint_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_UnoccupiedHeatingSetpoint command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedHeatingSetpoint.UnoccupiedHeatingSetpoint_name);
            }
            public class UnoccupiedHeatingSetpoint_Desired
            {
                public static implicit operator string(UnoccupiedHeatingSetpoint_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedHeatingSetpoint.UnoccupiedHeatingSetpoint_name, ClusterDesired);
                }
            }
            public class UnoccupiedHeatingSetpoint_Report
            {
                public static implicit operator string(UnoccupiedHeatingSetpoint_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedHeatingSetpoint.UnoccupiedHeatingSetpoint_name, ClusterReported);
                }
            }
            public class UnoccupiedHeatingSetpoint_SupportCommands
            {
                public static implicit operator string(UnoccupiedHeatingSetpoint_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinHeatSetpointLimit_ref = "MinHeatSetpointLimit";

                public const string MaxHeatSetpointLimit_ref = "MaxHeatSetpointLimit";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_MinHeatSetpointLimit
        {
            public const string MinHeatSetpointLimit_name = "MinHeatSetpointLimit";
            public class MinHeatSetpointLimit_Attributes
            {
                public static implicit operator string(MinHeatSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinHeatSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinHeatSetpointLimit.MinHeatSetpointLimit_name);
            }
            public class MinHeatSetpointLimit_Desired
            {
                public static implicit operator string(MinHeatSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinHeatSetpointLimit.MinHeatSetpointLimit_name, ClusterDesired);
                }
            }
            public class MinHeatSetpointLimit_Report
            {
                public static implicit operator string(MinHeatSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinHeatSetpointLimit.MinHeatSetpointLimit_name, ClusterReported);
                }
            }
            public class MinHeatSetpointLimit_SupportCommands
            {
                public static implicit operator string(MinHeatSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MaxHeatSetpointLimit
        {
            public const string MaxHeatSetpointLimit_name = "MaxHeatSetpointLimit";
            public class MaxHeatSetpointLimit_Attributes
            {
                public static implicit operator string(MaxHeatSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MaxHeatSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MaxHeatSetpointLimit.MaxHeatSetpointLimit_name);
            }
            public class MaxHeatSetpointLimit_Desired
            {
                public static implicit operator string(MaxHeatSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MaxHeatSetpointLimit.MaxHeatSetpointLimit_name, ClusterDesired);
                }
            }
            public class MaxHeatSetpointLimit_Report
            {
                public static implicit operator string(MaxHeatSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MaxHeatSetpointLimit.MaxHeatSetpointLimit_name, ClusterReported);
                }
            }
            public class MaxHeatSetpointLimit_SupportCommands
            {
                public static implicit operator string(MaxHeatSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MinCoolSetpointLimit
        {
            public const string MinCoolSetpointLimit_name = "MinCoolSetpointLimit";
            public class MinCoolSetpointLimit_Attributes
            {
                public static implicit operator string(MinCoolSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinCoolSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinCoolSetpointLimit.MinCoolSetpointLimit_name);
            }
            public class MinCoolSetpointLimit_Desired
            {
                public static implicit operator string(MinCoolSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinCoolSetpointLimit.MinCoolSetpointLimit_name, ClusterDesired);
                }
            }
            public class MinCoolSetpointLimit_Report
            {
                public static implicit operator string(MinCoolSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinCoolSetpointLimit.MinCoolSetpointLimit_name, ClusterReported);
                }
            }
            public class MinCoolSetpointLimit_SupportCommands
            {
                public static implicit operator string(MinCoolSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MaxCoolSetpointLimit
        {
            public const string MaxCoolSetpointLimit_name = "MaxCoolSetpointLimit";
            public class MaxCoolSetpointLimit_Attributes
            {
                public static implicit operator string(MaxCoolSetpointLimit_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MaxCoolSetpointLimit command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MaxCoolSetpointLimit.MaxCoolSetpointLimit_name);
            }
            public class MaxCoolSetpointLimit_Desired
            {
                public static implicit operator string(MaxCoolSetpointLimit_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MaxCoolSetpointLimit.MaxCoolSetpointLimit_name, ClusterDesired);
                }
            }
            public class MaxCoolSetpointLimit_Report
            {
                public static implicit operator string(MaxCoolSetpointLimit_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MaxCoolSetpointLimit.MaxCoolSetpointLimit_name, ClusterReported);
                }
            }
            public class MaxCoolSetpointLimit_SupportCommands
            {
                public static implicit operator string(MaxCoolSetpointLimit_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MinSetpointDeadBand
        {
            public const string MinSetpointDeadBand_name = "MinSetpointDeadBand";
            public class MinSetpointDeadBand_Attributes
            {
                public static implicit operator string(MinSetpointDeadBand_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinSetpointDeadBand command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinSetpointDeadBand.MinSetpointDeadBand_name);
            }
            public class MinSetpointDeadBand_Desired
            {
                public static implicit operator string(MinSetpointDeadBand_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinSetpointDeadBand.MinSetpointDeadBand_name, ClusterDesired);
                }
            }
            public class MinSetpointDeadBand_Report
            {
                public static implicit operator string(MinSetpointDeadBand_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_MinSetpointDeadBand.MinSetpointDeadBand_name, ClusterReported);
                }
            }
            public class MinSetpointDeadBand_SupportCommands
            {
                public static implicit operator string(MinSetpointDeadBand_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_RemoteSensing
        {
            public const string RemoteSensing_name = "RemoteSensing";
            public class RemoteSensing_Attributes
            {
                public static implicit operator string(RemoteSensing_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RemoteSensing command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_RemoteSensing.RemoteSensing_name);
            }
            public class RemoteSensing_Desired
            {
                public static implicit operator string(RemoteSensing_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_RemoteSensing.RemoteSensing_name, ClusterDesired);
                }
            }
            public class RemoteSensing_Report
            {
                public static implicit operator string(RemoteSensing_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_RemoteSensing.RemoteSensing_name, ClusterReported);
                }
            }
            public class RemoteSensing_SupportCommands
            {
                public static implicit operator string(RemoteSensing_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class LocalTemperatureRemote
                {
                    public const string LocalTemperatureRemote_name = "LocalTemperatureRemote";
                }
                public class OutdoorTemperatureRemote
                {
                    public const string OutdoorTemperatureRemote_name = "OutdoorTemperatureRemote";
                }
                public class OccupancyRemote
                {
                    public const string OccupancyRemote_name = "OccupancyRemote";
                }
            }
        }/*End of ATT class*/
        public class ATT_ControlSequenceOfOperation
        {
            public const string ControlSequenceOfOperation_name = "ControlSequenceOfOperation";
            public class ControlSequenceOfOperation_Attributes
            {
                public static implicit operator string(ControlSequenceOfOperation_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ControlSequenceOfOperation command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ControlSequenceOfOperation.ControlSequenceOfOperation_name);
            }
            public class ControlSequenceOfOperation_Desired
            {
                public static implicit operator string(ControlSequenceOfOperation_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ControlSequenceOfOperation.ControlSequenceOfOperation_name, ClusterDesired);
                }
            }
            public class ControlSequenceOfOperation_Report
            {
                public static implicit operator string(ControlSequenceOfOperation_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ControlSequenceOfOperation.ControlSequenceOfOperation_name, ClusterReported);
                }
            }
            public class ControlSequenceOfOperation_SupportCommands
            {
                public static implicit operator string(ControlSequenceOfOperation_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class CoolingOnly
                {
                    public const string CoolingOnly_value = "00";
                    public const string CoolingOnly_name = "CoolingOnly";
                }

                public class CoolingWithReheat
                {
                    public const string CoolingWithReheat_value = "01";
                    public const string CoolingWithReheat_name = "CoolingWithReheat";
                }

                public class HeatingOnly
                {
                    public const string HeatingOnly_value = "02";
                    public const string HeatingOnly_name = "HeatingOnly";
                }

                public class HeatingWithReheat
                {
                    public const string HeatingWithReheat_value = "03";
                    public const string HeatingWithReheat_name = "HeatingWithReheat";
                }

                public class CoolingAndHeating4Pipes
                {
                    public const string CoolingAndHeating4Pipes_value = "04";
                    public const string CoolingAndHeating4Pipes_name = "CoolingAndHeating4Pipes";
                }

                public class CoolingAndHeating4PipesWithReheat
                {
                    public const string CoolingAndHeating4PipesWithReheat_value = "05";
                    public const string CoolingAndHeating4PipesWithReheat_name = "CoolingAndHeating4PipesWithReheat";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_SystemMode
        {
            public const string SystemMode_name = "SystemMode";
            public class SystemMode_Attributes
            {
                public static implicit operator string(SystemMode_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SystemMode command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SystemMode.SystemMode_name);
            }
            public class SystemMode_Desired
            {
                public static implicit operator string(SystemMode_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SystemMode.SystemMode_name, ClusterDesired);
                }
            }
            public class SystemMode_Report
            {
                public static implicit operator string(SystemMode_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SystemMode.SystemMode_name, ClusterReported);
                }
            }
            public class SystemMode_SupportCommands
            {
                public static implicit operator string(SystemMode_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Off
                {
                    public const string Off_value = "00";
                    public const string Off_name = "Off";
                }

                public class Auto
                {
                    public const string Auto_value = "01";
                    public const string Auto_name = "Auto";
                }

                public class Cool
                {
                    public const string Cool_value = "03";
                    public const string Cool_name = "Cool";
                }

                public class Heat
                {
                    public const string Heat_value = "04";
                    public const string Heat_name = "Heat";
                }

                public class EmergencyHeating
                {
                    public const string EmergencyHeating_value = "05";
                    public const string EmergencyHeating_name = "EmergencyHeating";
                }

                public class Precooling
                {
                    public const string Precooling_value = "06";
                    public const string Precooling_name = "Precooling";
                }

                public class FanOnly
                {
                    public const string FanOnly_value = "07";
                    public const string FanOnly_name = "FanOnly";
                }

                public class Dry
                {
                    public const string Dry_value = "08";
                    public const string Dry_name = "Dry";
                }

                public class Sleep
                {
                    public const string Sleep_value = "09";
                    public const string Sleep_name = "Sleep";
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
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_AlarmMask command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AlarmMask.AlarmMask_name);
            }
            public class AlarmMask_Desired
            {
                public static implicit operator string(AlarmMask_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AlarmMask.AlarmMask_name, ClusterDesired);
                }
            }
            public class AlarmMask_Report
            {
                public static implicit operator string(AlarmMask_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_AlarmMask.AlarmMask_name, ClusterReported);
                }
            }
            public class AlarmMask_SupportCommands
            {
                public static implicit operator string(AlarmMask_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class InitializationFailure
                {
                    public const string InitializationFailure_name = "InitializationFailure";
                }
                public class HardwareFailure
                {
                    public const string HardwareFailure_name = "HardwareFailure";
                }
                public class SelfCalibrationFailure
                {
                    public const string SelfCalibrationFailure_name = "SelfCalibrationFailure";
                }
            }
        }/*End of ATT class*/
        public class ATT_ThermostatRunningMode
        {
            public const string ThermostatRunningMode_name = "ThermostatRunningMode";
            public class ThermostatRunningMode_Attributes
            {
                public static implicit operator string(ThermostatRunningMode_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ThermostatRunningMode command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatRunningMode.ThermostatRunningMode_name);
            }
            public class ThermostatRunningMode_Desired
            {
                public static implicit operator string(ThermostatRunningMode_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatRunningMode.ThermostatRunningMode_name, ClusterDesired);
                }
            }
            public class ThermostatRunningMode_Report
            {
                public static implicit operator string(ThermostatRunningMode_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatRunningMode.ThermostatRunningMode_name, ClusterReported);
                }
            }
            public class ThermostatRunningMode_SupportCommands
            {
                public static implicit operator string(ThermostatRunningMode_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Off
                {
                    public const string Off_value = "00";
                    public const string Off_name = "Off";
                }

                public class Cool
                {
                    public const string Cool_value = "03";
                    public const string Cool_name = "Cool";
                }

                public class Heat
                {
                    public const string Heat_value = "04";
                    public const string Heat_name = "Heat";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_StartOfWeek
        {
            public const string StartOfWeek_name = "StartOfWeek";
            public class StartOfWeek_Attributes
            {
                public static implicit operator string(StartOfWeek_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_StartOfWeek command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_StartOfWeek.StartOfWeek_name);
            }
            public class StartOfWeek_Desired
            {
                public static implicit operator string(StartOfWeek_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_StartOfWeek.StartOfWeek_name, ClusterDesired);
                }
            }
            public class StartOfWeek_Report
            {
                public static implicit operator string(StartOfWeek_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_StartOfWeek.StartOfWeek_name, ClusterReported);
                }
            }
            public class StartOfWeek_SupportCommands
            {
                public static implicit operator string(StartOfWeek_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Sunday
                {
                    public const string Sunday_value = "00";
                    public const string Sunday_name = "Sunday";
                }

                public class Monday
                {
                    public const string Monday_value = "01";
                    public const string Monday_name = "Monday";
                }

                public class Tuesday
                {
                    public const string Tuesday_value = "02";
                    public const string Tuesday_name = "Tuesday";
                }

                public class Wednesday
                {
                    public const string Wednesday_value = "03";
                    public const string Wednesday_name = "Wednesday";
                }

                public class Thursday
                {
                    public const string Thursday_value = "04";
                    public const string Thursday_name = "Thursday";
                }

                public class Friday
                {
                    public const string Friday_value = "05";
                    public const string Friday_name = "Friday";
                }

                public class Saturday
                {
                    public const string Saturday_value = "06";
                    public const string Saturday_name = "Saturday";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_NumberOfWeeklyTransitions
        {
            public const string NumberOfWeeklyTransitions_name = "NumberOfWeeklyTransitions";
            public class NumberOfWeeklyTransitions_Attributes
            {
                public static implicit operator string(NumberOfWeeklyTransitions_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfWeeklyTransitions command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_NumberOfWeeklyTransitions.NumberOfWeeklyTransitions_name);
            }
            public class NumberOfWeeklyTransitions_Desired
            {
                public static implicit operator string(NumberOfWeeklyTransitions_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_NumberOfWeeklyTransitions.NumberOfWeeklyTransitions_name, ClusterDesired);
                }
            }
            public class NumberOfWeeklyTransitions_Report
            {
                public static implicit operator string(NumberOfWeeklyTransitions_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_NumberOfWeeklyTransitions.NumberOfWeeklyTransitions_name, ClusterReported);
                }
            }
            public class NumberOfWeeklyTransitions_SupportCommands
            {
                public static implicit operator string(NumberOfWeeklyTransitions_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_NumberOfDailyTransitions
        {
            public const string NumberOfDailyTransitions_name = "NumberOfDailyTransitions";
            public class NumberOfDailyTransitions_Attributes
            {
                public static implicit operator string(NumberOfDailyTransitions_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NumberOfDailyTransitions command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_NumberOfDailyTransitions.NumberOfDailyTransitions_name);
            }
            public class NumberOfDailyTransitions_Desired
            {
                public static implicit operator string(NumberOfDailyTransitions_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_NumberOfDailyTransitions.NumberOfDailyTransitions_name, ClusterDesired);
                }
            }
            public class NumberOfDailyTransitions_Report
            {
                public static implicit operator string(NumberOfDailyTransitions_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_NumberOfDailyTransitions.NumberOfDailyTransitions_name, ClusterReported);
                }
            }
            public class NumberOfDailyTransitions_SupportCommands
            {
                public static implicit operator string(NumberOfDailyTransitions_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_TemperatureSetpointHold
        {
            public const string TemperatureSetpointHold_name = "TemperatureSetpointHold";
            public class TemperatureSetpointHold_Attributes
            {
                public static implicit operator string(TemperatureSetpointHold_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_TemperatureSetpointHold command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_TemperatureSetpointHold.TemperatureSetpointHold_name);
            }
            public class TemperatureSetpointHold_Desired
            {
                public static implicit operator string(TemperatureSetpointHold_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_TemperatureSetpointHold.TemperatureSetpointHold_name, ClusterDesired);
                }
            }
            public class TemperatureSetpointHold_Report
            {
                public static implicit operator string(TemperatureSetpointHold_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_TemperatureSetpointHold.TemperatureSetpointHold_name, ClusterReported);
                }
            }
            public class TemperatureSetpointHold_SupportCommands
            {
                public static implicit operator string(TemperatureSetpointHold_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class SetpointHoldOff
                {
                    public const string SetpointHoldOff_value = "00";
                    public const string SetpointHoldOff_name = "SetpointHoldOff";
                }

                public class SetpointHoldOn
                {
                    public const string SetpointHoldOn_value = "01";
                    public const string SetpointHoldOn_name = "SetpointHoldOn";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_TemperatureSetpointHoldDuration
        {
            public const string TemperatureSetpointHoldDuration_name = "TemperatureSetpointHoldDuration";
            public class TemperatureSetpointHoldDuration_Attributes
            {
                public static implicit operator string(TemperatureSetpointHoldDuration_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_TemperatureSetpointHoldDuration command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_TemperatureSetpointHoldDuration.TemperatureSetpointHoldDuration_name);
            }
            public class TemperatureSetpointHoldDuration_Desired
            {
                public static implicit operator string(TemperatureSetpointHoldDuration_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_TemperatureSetpointHoldDuration.TemperatureSetpointHoldDuration_name, ClusterDesired);
                }
            }
            public class TemperatureSetpointHoldDuration_Report
            {
                public static implicit operator string(TemperatureSetpointHoldDuration_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_TemperatureSetpointHoldDuration.TemperatureSetpointHoldDuration_name, ClusterReported);
                }
            }
            public class TemperatureSetpointHoldDuration_SupportCommands
            {
                public static implicit operator string(TemperatureSetpointHoldDuration_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_ThermostatProgrammingOperationMode
        {
            public const string ThermostatProgrammingOperationMode_name = "ThermostatProgrammingOperationMode";
            public class ThermostatProgrammingOperationMode_Attributes
            {
                public static implicit operator string(ThermostatProgrammingOperationMode_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ThermostatProgrammingOperationMode command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatProgrammingOperationMode.ThermostatProgrammingOperationMode_name);
            }
            public class ThermostatProgrammingOperationMode_Desired
            {
                public static implicit operator string(ThermostatProgrammingOperationMode_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatProgrammingOperationMode.ThermostatProgrammingOperationMode_name, ClusterDesired);
                }
            }
            public class ThermostatProgrammingOperationMode_Report
            {
                public static implicit operator string(ThermostatProgrammingOperationMode_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatProgrammingOperationMode.ThermostatProgrammingOperationMode_name, ClusterReported);
                }
            }
            public class ThermostatProgrammingOperationMode_SupportCommands
            {
                public static implicit operator string(ThermostatProgrammingOperationMode_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class ProgrammingMode
                {
                    public const string ProgrammingMode_name = "ProgrammingMode";
                }
                public class AutoOrRecovery
                {
                    public const string AutoOrRecovery_name = "AutoOrRecovery";
                }
                public class EconomyOrEnergyStar
                {
                    public const string EconomyOrEnergyStar_name = "EconomyOrEnergyStar";
                }
            }
        }/*End of ATT class*/
        public class ATT_ThermostatRunningState
        {
            public const string ThermostatRunningState_name = "ThermostatRunningState";
            public class ThermostatRunningState_Attributes
            {
                public static implicit operator string(ThermostatRunningState_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ThermostatRunningState command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatRunningState.ThermostatRunningState_name);
            }
            public class ThermostatRunningState_Desired
            {
                public static implicit operator string(ThermostatRunningState_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatRunningState.ThermostatRunningState_name, ClusterDesired);
                }
            }
            public class ThermostatRunningState_Report
            {
                public static implicit operator string(ThermostatRunningState_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ThermostatRunningState.ThermostatRunningState_name, ClusterReported);
                }
            }
            public class ThermostatRunningState_SupportCommands
            {
                public static implicit operator string(ThermostatRunningState_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class HeatOn
                {
                    public const string HeatOn_name = "HeatOn";
                }
                public class CoolOn
                {
                    public const string CoolOn_name = "CoolOn";
                }
                public class FanOn
                {
                    public const string FanOn_name = "FanOn";
                }
                public class HeatSecondStageOn
                {
                    public const string HeatSecondStageOn_name = "HeatSecondStageOn";
                }
                public class CoolSecondStageOn
                {
                    public const string CoolSecondStageOn_name = "CoolSecondStageOn";
                }
                public class FanSecondStageOn
                {
                    public const string FanSecondStageOn_name = "FanSecondStageOn";
                }
                public class FanThirdStageOn
                {
                    public const string FanThirdStageOn_name = "FanThirdStageOn";
                }
            }
        }/*End of ATT class*/
        public class ATT_SetpointChangeSource
        {
            public const string SetpointChangeSource_name = "SetpointChangeSource";
            public class SetpointChangeSource_Attributes
            {
                public static implicit operator string(SetpointChangeSource_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SetpointChangeSource command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeSource.SetpointChangeSource_name);
            }
            public class SetpointChangeSource_Desired
            {
                public static implicit operator string(SetpointChangeSource_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeSource.SetpointChangeSource_name, ClusterDesired);
                }
            }
            public class SetpointChangeSource_Report
            {
                public static implicit operator string(SetpointChangeSource_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeSource.SetpointChangeSource_name, ClusterReported);
                }
            }
            public class SetpointChangeSource_SupportCommands
            {
                public static implicit operator string(SetpointChangeSource_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class Manual
                {
                    public const string Manual_value = "00";
                    public const string Manual_name = "Manual";
                }

                public class ScheduleOrInternalProgramming
                {
                    public const string ScheduleOrInternalProgramming_value = "01";
                    public const string ScheduleOrInternalProgramming_name = "ScheduleOrInternalProgramming";
                }

                public class External
                {
                    public const string External_value = "02";
                    public const string External_name = "External";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_SetpointChangeAmount
        {
            public const string SetpointChangeAmount_name = "SetpointChangeAmount";
            public class SetpointChangeAmount_Attributes
            {
                public static implicit operator string(SetpointChangeAmount_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SetpointChangeAmount command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeAmount.SetpointChangeAmount_name);
            }
            public class SetpointChangeAmount_Desired
            {
                public static implicit operator string(SetpointChangeAmount_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeAmount.SetpointChangeAmount_name, ClusterDesired);
                }
            }
            public class SetpointChangeAmount_Report
            {
                public static implicit operator string(SetpointChangeAmount_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeAmount.SetpointChangeAmount_name, ClusterReported);
                }
            }
            public class SetpointChangeAmount_SupportCommands
            {
                public static implicit operator string(SetpointChangeAmount_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_SetpointChangeSourceTimestamp
        {
            public const string SetpointChangeSourceTimestamp_name = "SetpointChangeSourceTimestamp";
            public class SetpointChangeSourceTimestamp_Attributes
            {
                public static implicit operator string(SetpointChangeSourceTimestamp_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_SetpointChangeSourceTimestamp command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeSourceTimestamp.SetpointChangeSourceTimestamp_name);
            }
            public class SetpointChangeSourceTimestamp_Desired
            {
                public static implicit operator string(SetpointChangeSourceTimestamp_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeSourceTimestamp.SetpointChangeSourceTimestamp_name, ClusterDesired);
                }
            }
            public class SetpointChangeSourceTimestamp_Report
            {
                public static implicit operator string(SetpointChangeSourceTimestamp_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_SetpointChangeSourceTimestamp.SetpointChangeSourceTimestamp_name, ClusterReported);
                }
            }
            public class SetpointChangeSourceTimestamp_SupportCommands
            {
                public static implicit operator string(SetpointChangeSourceTimestamp_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OccupiedSetback
        {
            public const string OccupiedSetback_name = "OccupiedSetback";
            public class OccupiedSetback_Attributes
            {
                public static implicit operator string(OccupiedSetback_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OccupiedSetback command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetback.OccupiedSetback_name);
            }
            public class OccupiedSetback_Desired
            {
                public static implicit operator string(OccupiedSetback_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetback.OccupiedSetback_name, ClusterDesired);
                }
            }
            public class OccupiedSetback_Report
            {
                public static implicit operator string(OccupiedSetback_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetback.OccupiedSetback_name, ClusterReported);
                }
            }
            public class OccupiedSetback_SupportCommands
            {
                public static implicit operator string(OccupiedSetback_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string OccupiedSetbackMin_ref = "OccupiedSetbackMin";

                public const string OccupiedSetbackMax_ref = "OccupiedSetbackMax";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_OccupiedSetbackMin
        {
            public const string OccupiedSetbackMin_name = "OccupiedSetbackMin";
            public class OccupiedSetbackMin_Attributes
            {
                public static implicit operator string(OccupiedSetbackMin_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OccupiedSetbackMin command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetbackMin.OccupiedSetbackMin_name);
            }
            public class OccupiedSetbackMin_Desired
            {
                public static implicit operator string(OccupiedSetbackMin_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetbackMin.OccupiedSetbackMin_name, ClusterDesired);
                }
            }
            public class OccupiedSetbackMin_Report
            {
                public static implicit operator string(OccupiedSetbackMin_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetbackMin.OccupiedSetbackMin_name, ClusterReported);
                }
            }
            public class OccupiedSetbackMin_SupportCommands
            {
                public static implicit operator string(OccupiedSetbackMin_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string OccupiedSetbackMax_ref = "OccupiedSetbackMax";
                public const string OccupiedSetbackMax_value = "0";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_OccupiedSetbackMax
        {
            public const string OccupiedSetbackMax_name = "OccupiedSetbackMax";
            public class OccupiedSetbackMax_Attributes
            {
                public static implicit operator string(OccupiedSetbackMax_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OccupiedSetbackMax command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetbackMax.OccupiedSetbackMax_name);
            }
            public class OccupiedSetbackMax_Desired
            {
                public static implicit operator string(OccupiedSetbackMax_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetbackMax.OccupiedSetbackMax_name, ClusterDesired);
                }
            }
            public class OccupiedSetbackMax_Report
            {
                public static implicit operator string(OccupiedSetbackMax_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_OccupiedSetbackMax.OccupiedSetbackMax_name, ClusterReported);
                }
            }
            public class OccupiedSetbackMax_SupportCommands
            {
                public static implicit operator string(OccupiedSetbackMax_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string OccupiedSetbackMin_ref = "OccupiedSetbackMin";
                public const string OccupiedSetbackMin_value = "0";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_UnoccupiedSetback
        {
            public const string UnoccupiedSetback_name = "UnoccupiedSetback";
            public class UnoccupiedSetback_Attributes
            {
                public static implicit operator string(UnoccupiedSetback_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_UnoccupiedSetback command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetback.UnoccupiedSetback_name);
            }
            public class UnoccupiedSetback_Desired
            {
                public static implicit operator string(UnoccupiedSetback_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetback.UnoccupiedSetback_name, ClusterDesired);
                }
            }
            public class UnoccupiedSetback_Report
            {
                public static implicit operator string(UnoccupiedSetback_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetback.UnoccupiedSetback_name, ClusterReported);
                }
            }
            public class UnoccupiedSetback_SupportCommands
            {
                public static implicit operator string(UnoccupiedSetback_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string UnoccupiedSetbackMin_ref = "UnoccupiedSetbackMin";
                public const string UnoccupiedSetbackMin_value = "255";

                public const string UnoccupiedSetbackMax_ref = "UnoccupiedSetbackMax";
                public const string UnoccupiedSetbackMax_value = "255";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_UnoccupiedSetbackMin
        {
            public const string UnoccupiedSetbackMin_name = "UnoccupiedSetbackMin";
            public class UnoccupiedSetbackMin_Attributes
            {
                public static implicit operator string(UnoccupiedSetbackMin_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_UnoccupiedSetbackMin command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetbackMin.UnoccupiedSetbackMin_name);
            }
            public class UnoccupiedSetbackMin_Desired
            {
                public static implicit operator string(UnoccupiedSetbackMin_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetbackMin.UnoccupiedSetbackMin_name, ClusterDesired);
                }
            }
            public class UnoccupiedSetbackMin_Report
            {
                public static implicit operator string(UnoccupiedSetbackMin_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetbackMin.UnoccupiedSetbackMin_name, ClusterReported);
                }
            }
            public class UnoccupiedSetbackMin_SupportCommands
            {
                public static implicit operator string(UnoccupiedSetbackMin_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string UnoccupiedSetbackMax_ref = "UnoccupiedSetbackMax";
                public const string UnoccupiedSetbackMax_value = "0";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_UnoccupiedSetbackMax
        {
            public const string UnoccupiedSetbackMax_name = "UnoccupiedSetbackMax";
            public class UnoccupiedSetbackMax_Attributes
            {
                public static implicit operator string(UnoccupiedSetbackMax_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_UnoccupiedSetbackMax command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetbackMax.UnoccupiedSetbackMax_name);
            }
            public class UnoccupiedSetbackMax_Desired
            {
                public static implicit operator string(UnoccupiedSetbackMax_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetbackMax.UnoccupiedSetbackMax_name, ClusterDesired);
                }
            }
            public class UnoccupiedSetbackMax_Report
            {
                public static implicit operator string(UnoccupiedSetbackMax_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_UnoccupiedSetbackMax.UnoccupiedSetbackMax_name, ClusterReported);
                }
            }
            public class UnoccupiedSetbackMax_SupportCommands
            {
                public static implicit operator string(UnoccupiedSetbackMax_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string UnoccupiedSetbackMin_ref = "UnoccupiedSetbackMin";
                public const string UnoccupiedSetbackMin_value = "0";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_EmergencyHeatDelta
        {
            public const string EmergencyHeatDelta_name = "EmergencyHeatDelta";
            public class EmergencyHeatDelta_Attributes
            {
                public static implicit operator string(EmergencyHeatDelta_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_EmergencyHeatDelta command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_EmergencyHeatDelta.EmergencyHeatDelta_name);
            }
            public class EmergencyHeatDelta_Desired
            {
                public static implicit operator string(EmergencyHeatDelta_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_EmergencyHeatDelta.EmergencyHeatDelta_name, ClusterDesired);
                }
            }
            public class EmergencyHeatDelta_Report
            {
                public static implicit operator string(EmergencyHeatDelta_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_EmergencyHeatDelta.EmergencyHeatDelta_name, ClusterReported);
                }
            }
            public class EmergencyHeatDelta_SupportCommands
            {
                public static implicit operator string(EmergencyHeatDelta_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_ACType
        {
            public const string ACType_name = "ACType";
            public class ACType_Attributes
            {
                public static implicit operator string(ACType_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACType command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACType.ACType_name);
            }
            public class ACType_Desired
            {
                public static implicit operator string(ACType_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACType.ACType_name, ClusterDesired);
                }
            }
            public class ACType_Report
            {
                public static implicit operator string(ACType_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACType.ACType_name, ClusterReported);
                }
            }
            public class ACType_SupportCommands
            {
                public static implicit operator string(ACType_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class CoolingAndFixedSpeed
                {
                    public const string CoolingAndFixedSpeed_value = "01";
                    public const string CoolingAndFixedSpeed_name = "CoolingAndFixedSpeed";
                }

                public class HeatPumpAndFixedSpeed
                {
                    public const string HeatPumpAndFixedSpeed_value = "02";
                    public const string HeatPumpAndFixedSpeed_name = "HeatPumpAndFixedSpeed";
                }

                public class CoolingAndInverter
                {
                    public const string CoolingAndInverter_value = "03";
                    public const string CoolingAndInverter_name = "CoolingAndInverter";
                }

                public class HeatPumpAndInverter
                {
                    public const string HeatPumpAndInverter_value = "04";
                    public const string HeatPumpAndInverter_name = "HeatPumpAndInverter";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_ACCapacity
        {
            public const string ACCapacity_name = "ACCapacity";
            public class ACCapacity_Attributes
            {
                public static implicit operator string(ACCapacity_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACCapacity command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCapacity.ACCapacity_name);
            }
            public class ACCapacity_Desired
            {
                public static implicit operator string(ACCapacity_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCapacity.ACCapacity_name, ClusterDesired);
                }
            }
            public class ACCapacity_Report
            {
                public static implicit operator string(ACCapacity_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCapacity.ACCapacity_name, ClusterReported);
                }
            }
            public class ACCapacity_SupportCommands
            {
                public static implicit operator string(ACCapacity_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_ACRefrigerantType
        {
            public const string ACRefrigerantType_name = "ACRefrigerantType";
            public class ACRefrigerantType_Attributes
            {
                public static implicit operator string(ACRefrigerantType_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACRefrigerantType command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACRefrigerantType.ACRefrigerantType_name);
            }
            public class ACRefrigerantType_Desired
            {
                public static implicit operator string(ACRefrigerantType_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACRefrigerantType.ACRefrigerantType_name, ClusterDesired);
                }
            }
            public class ACRefrigerantType_Report
            {
                public static implicit operator string(ACRefrigerantType_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACRefrigerantType.ACRefrigerantType_name, ClusterReported);
                }
            }
            public class ACRefrigerantType_SupportCommands
            {
                public static implicit operator string(ACRefrigerantType_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class R22
                {
                    public const string R22_value = "01";
                    public const string R22_name = "R22";
                }

                public class R410a
                {
                    public const string R410a_value = "02";
                    public const string R410a_name = "R410a";
                }

                public class R407c
                {
                    public const string R407c_value = "03";
                    public const string R407c_name = "R407c";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_ACCompressorType
        {
            public const string ACCompressorType_name = "ACCompressorType";
            public class ACCompressorType_Attributes
            {
                public static implicit operator string(ACCompressorType_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACCompressorType command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCompressorType.ACCompressorType_name);
            }
            public class ACCompressorType_Desired
            {
                public static implicit operator string(ACCompressorType_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCompressorType.ACCompressorType_name, ClusterDesired);
                }
            }
            public class ACCompressorType_Report
            {
                public static implicit operator string(ACCompressorType_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCompressorType.ACCompressorType_name, ClusterReported);
                }
            }
            public class ACCompressorType_SupportCommands
            {
                public static implicit operator string(ACCompressorType_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class T1
                {
                    public const string T1_value = "01";
                    public const string T1_name = "T1";
                }

                public class T2
                {
                    public const string T2_value = "02";
                    public const string T2_name = "T2";
                }

                public class T3
                {
                    public const string T3_value = "03";
                    public const string T3_name = "T3";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_ACErrorCode
        {
            public const string ACErrorCode_name = "ACErrorCode";
            public class ACErrorCode_Attributes
            {
                public static implicit operator string(ACErrorCode_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACErrorCode command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACErrorCode.ACErrorCode_name);
            }
            public class ACErrorCode_Desired
            {
                public static implicit operator string(ACErrorCode_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACErrorCode.ACErrorCode_name, ClusterDesired);
                }
            }
            public class ACErrorCode_Report
            {
                public static implicit operator string(ACErrorCode_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACErrorCode.ACErrorCode_name, ClusterReported);
                }
            }
            public class ACErrorCode_SupportCommands
            {
                public static implicit operator string(ACErrorCode_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class CompressorFailureOrRefrigerantLeakage
                {
                    public const string CompressorFailureOrRefrigerantLeakage_name = "CompressorFailureOrRefrigerantLeakage";
                }
                public class RoomTemperatureSensorFailure
                {
                    public const string RoomTemperatureSensorFailure_name = "RoomTemperatureSensorFailure";
                }
                public class OutdoorTemperatureSensorFailure
                {
                    public const string OutdoorTemperatureSensorFailure_name = "OutdoorTemperatureSensorFailure";
                }
                public class IndoorCoilTemperatureSensorFailure
                {
                    public const string IndoorCoilTemperatureSensorFailure_name = "IndoorCoilTemperatureSensorFailure";
                }
                public class FanFailure
                {
                    public const string FanFailure_name = "FanFailure";
                }
            }
        }/*End of ATT class*/
        public class ATT_ACLouverPosition
        {
            public const string ACLouverPosition_name = "ACLouverPosition";
            public class ACLouverPosition_Attributes
            {
                public static implicit operator string(ACLouverPosition_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACLouverPosition command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACLouverPosition.ACLouverPosition_name);
            }
            public class ACLouverPosition_Desired
            {
                public static implicit operator string(ACLouverPosition_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACLouverPosition.ACLouverPosition_name, ClusterDesired);
                }
            }
            public class ACLouverPosition_Report
            {
                public static implicit operator string(ACLouverPosition_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACLouverPosition.ACLouverPosition_name, ClusterReported);
                }
            }
            public class ACLouverPosition_SupportCommands
            {
                public static implicit operator string(ACLouverPosition_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class FullyClosed
                {
                    public const string FullyClosed_value = "01";
                    public const string FullyClosed_name = "FullyClosed";
                }

                public class FullyOpen
                {
                    public const string FullyOpen_value = "02";
                    public const string FullyOpen_name = "FullyOpen";
                }

                public class QuarterOpen
                {
                    public const string QuarterOpen_value = "03";
                    public const string QuarterOpen_name = "QuarterOpen";
                }

                public class HalfOpen
                {
                    public const string HalfOpen_value = "04";
                    public const string HalfOpen_name = "HalfOpen";
                }

                public class ThreeQuartersOpen
                {
                    public const string ThreeQuartersOpen_value = "05";
                    public const string ThreeQuartersOpen_name = "ThreeQuartersOpen";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_ACCoilTemperature
        {
            public const string ACCoilTemperature_name = "ACCoilTemperature";
            public class ACCoilTemperature_Attributes
            {
                public static implicit operator string(ACCoilTemperature_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACCoilTemperature command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCoilTemperature.ACCoilTemperature_name);
            }
            public class ACCoilTemperature_Desired
            {
                public static implicit operator string(ACCoilTemperature_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCoilTemperature.ACCoilTemperature_name, ClusterDesired);
                }
            }
            public class ACCoilTemperature_Report
            {
                public static implicit operator string(ACCoilTemperature_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCoilTemperature.ACCoilTemperature_name, ClusterReported);
                }
            }
            public class ACCoilTemperature_SupportCommands
            {
                public static implicit operator string(ACCoilTemperature_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_ACCapacityFormat
        {
            public const string ACCapacityFormat_name = "ACCapacityFormat";
            public class ACCapacityFormat_Attributes
            {
                public static implicit operator string(ACCapacityFormat_Attributes command)
                {
                    return SumAllAtt(ThermostatCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ACCapacityFormat command)
            {
                return SumATTStr(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCapacityFormat.ACCapacityFormat_name);
            }
            public class ACCapacityFormat_Desired
            {
                public static implicit operator string(ACCapacityFormat_Desired command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCapacityFormat.ACCapacityFormat_name, ClusterDesired);
                }
            }
            public class ACCapacityFormat_Report
            {
                public static implicit operator string(ACCapacityFormat_Report command)
                {
                    return SumATTStr_WithSubValue(ThermostatCluster.ClusterName, ThermostatCluster.ATT_ACCapacityFormat.ACCapacityFormat_name, ClusterReported);
                }
            }
            public class ACCapacityFormat_SupportCommands
            {
                public static implicit operator string(ACCapacityFormat_SupportCommands command)
                {
                    return SumSupCMD(ThermostatCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class BTUh
                {
                    public const string BTUh_value = "00";
                    public const string BTUh_name = "BTUh";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        #endregion

        #region Cluster Commands

        public class CMD_SetpointRaiseOrLower
        {
            public const string SetpointRaiseOrLower_name = "SetpointRaiseOrLower";
            public static implicit operator string(CMD_SetpointRaiseOrLower command)
            {
                return SumCMDStr(ThermostatCluster.ClusterName, ThermostatCluster.CMD_SetpointRaiseOrLower.SetpointRaiseOrLower_name);
            }

            public class CMD_SetpointRaiseOrLower_Payload
            {
                public string Mode { get; set; }
                public string Amount { get; set; }
                public struct Amount_Heat
                {
                    public string Heat { get; set; }
                }
                public struct Amount_Cool
                {
                    public string Cool { get; set; }
                }
                public struct Amount_Both
                {
                    public string Both { get; set; }
                }

                //public class Amount
                //{
                //    public Amount_Heat my_Heat { get; set; }
                //    public Amount_Cool my_Cool { get; set; }
                //    public Amount_Both my_Both { get; set; }                    
                //}                

                /*Start of public Payload*/
                public CMD_SetpointRaiseOrLower_Payload(string my_Mode, string my_Amount)
                {
                    Mode = my_Mode;
                    Amount = my_Amount;
                }/*End of public Payload*/
                public static implicit operator CMD_SetpointRaiseOrLower_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetpointRaiseOrLower_Payload>(command);
                }
                public static implicit operator string(CMD_SetpointRaiseOrLower_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_SetWeeklySchedule
        {
            public const string SetWeeklySchedule_name = "SetWeeklySchedule";
            public static implicit operator string(CMD_SetWeeklySchedule command)
            {
                return SumCMDStr(ThermostatCluster.ClusterName, ThermostatCluster.CMD_SetWeeklySchedule.SetWeeklySchedule_name);
            }

            public class CMD_SetWeeklySchedule_Payload
            {
                public string NumberOfTransitions { get; set; }
                public string DayOfWeek { get; set; }
                public string Mode { get; set; }
                public string Transitions { get; set; }
                /*Start of public Payload*/
                public CMD_SetWeeklySchedule_Payload(string my_NumberOfTransitions, string my_DayOfWeek, string my_Mode, string my_Transitions)
                {
                    NumberOfTransitions = my_NumberOfTransitions;
                    DayOfWeek = my_DayOfWeek;
                    Mode = my_Mode;
                    Transitions = my_Transitions;

                }
                /*End of public Payload*/
                public static implicit operator CMD_SetWeeklySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_SetWeeklySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_SetWeeklySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetWeeklySchedule
        {
            public const string GetWeeklySchedule_name = "GetWeeklySchedule";
            public static implicit operator string(CMD_GetWeeklySchedule command)
            {
                return SumCMDStr(ThermostatCluster.ClusterName, ThermostatCluster.CMD_GetWeeklySchedule.GetWeeklySchedule_name);
            }
            public class CMD_GetWeeklySchedule_Payload
            {
                public string DaysToReturn { get; set; }
                public string ModeToReturn { get; set; }
                /*Start of public Payload*/
                public CMD_GetWeeklySchedule_Payload(string my_DaysToReturn, string my_ModeToReturn)
                {
                    DaysToReturn = my_DaysToReturn;
                    ModeToReturn = my_ModeToReturn;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetWeeklySchedule_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetWeeklySchedule_Payload>(command);
                }
                public static implicit operator string(CMD_GetWeeklySchedule_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ClearWeeklySchedule
        {
            public const string ClearWeeklySchedule_name = "ClearWeeklySchedule";
            public static implicit operator string(CMD_ClearWeeklySchedule command)
            {
                return SumCMDStr(ThermostatCluster.ClusterName, ThermostatCluster.CMD_ClearWeeklySchedule.ClearWeeklySchedule_name);
            }
        }/*End of CMD class*/
        public class CMD_GetRelayStatusLog
        {
            public const string GetRelayStatusLog_name = "GetRelayStatusLog";
            public static implicit operator string(CMD_GetRelayStatusLog command)
            {
                return SumCMDStr(ThermostatCluster.ClusterName, ThermostatCluster.CMD_GetRelayStatusLog.GetRelayStatusLog_name);
            }
        }/*End of CMD class*/
        #endregion

        #region Cluster Operations

        public struct Payload_Att_SupportedCommands
        {
            public string[] SupportedCommands;
            public Payload_Att_SupportedCommands(string[] newStrArray)
            {
                SupportedCommands = newStrArray;
            }
            public static implicit operator Payload_Att_SupportedCommands(string options)
            {
                return new Payload_Att_SupportedCommands(JsonConvert.DeserializeObject<string[]>(options));
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

        public static implicit operator ThermostatCluster(string options)
        {
            return JsonConvert.DeserializeObject<ThermostatCluster>(options);
        }
        public static implicit operator string(ThermostatCluster command)
        {
            return ThermostatCluster.ClusterName;
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
