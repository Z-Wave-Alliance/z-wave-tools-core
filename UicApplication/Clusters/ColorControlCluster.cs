/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Newtonsoft.Json;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class ColorControlCluster
    {
        public const string ClusterName = "ColorControl";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_CurrentHue
        {
            public const string CurrentHue_name = "CurrentHue";
            public class CurrentHue_Attributes
            {
                public static implicit operator string(CurrentHue_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_CurrentHue command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentHue.CurrentHue_name);
            }
            public class CHue_Desired
            {
                public static implicit operator string(CHue_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentHue.CurrentHue_name, ClusterDesired);
                }
            }
            public class CHue_Report
            {
                public static implicit operator string(CHue_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentHue.CurrentHue_name, ClusterReported);
                }
            }
            public class CHue_SupportCommands
            {
                public static implicit operator string(CHue_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_CurrentSaturation
        {
            public const string CurrentSaturation_name = "CurrentSaturation";
            public class CurrentSaturation_Attributes
            {
                public static implicit operator string(CurrentSaturation_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_CurrentSaturation command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentSaturation.CurrentSaturation_name);
            }
            public class CurrentSaturation_Desired
            {
                public static implicit operator string(CurrentSaturation_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentSaturation.CurrentSaturation_name, ClusterDesired);
                }
            }
            public class CurrentSaturation_Report
            {
                public static implicit operator string(CurrentSaturation_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentSaturation.CurrentSaturation_name, ClusterReported);
                }
            }
            public class CurrentSaturation_SupportCommands
            {
                public static implicit operator string(CurrentSaturation_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_RemainingTime
        {
            public const string RemainingTime_name = "RemainingTime";
            public class RemainingTime_Attributes
            {
                public static implicit operator string(RemainingTime_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RemainingTime command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_RemainingTime.RemainingTime_name);
            }
            public class RemainingTime_Desired
            {
                public static implicit operator string(RemainingTime_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_RemainingTime.RemainingTime_name, ClusterDesired);
                }
            }
            public class RemainingTime_Report
            {
                public static implicit operator string(RemainingTime_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_RemainingTime.RemainingTime_name, ClusterReported);
                }
            }
            public class RemainingTime_SupportCommands
            {
                public static implicit operator string(RemainingTime_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_CurrentX
        {
            public const string CurrentX_name = "CurrentX";
            public class CurrentX_Attributes
            {
                public static implicit operator string(CurrentX_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_CurrentX command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentX.CurrentX_name);
            }
            public class CurrentX_Desired
            {
                public static implicit operator string(CurrentX_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentX.CurrentX_name, ClusterDesired);
                }
            }
            public class CurrentX_Report
            {
                public static implicit operator string(CurrentX_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentX.CurrentX_name, ClusterReported);
                }
            }
            public class CurrentX_SupportCommands
            {
                public static implicit operator string(CurrentX_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_CurrentY
        {
            public const string CurrentY_name = "CurrentX";
            public class CurrentY_Attributes
            {
                public static implicit operator string(CurrentY_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_CurrentY command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentY.CurrentY_name);
            }
            public class CurrentY_Desired
            {
                public static implicit operator string(CurrentY_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentY.CurrentY_name, ClusterDesired);
                }
            }
            public class CurrentY_Report
            {
                public static implicit operator string(CurrentY_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_CurrentY.CurrentY_name, ClusterReported);
                }
            }
            public class CurrentY_SupportCommands
            {
                public static implicit operator string(CurrentY_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_ColorMode
        {
            public const string ColorMode_name = "ColorMode";
            public class ColorMode_Attributes
            {
                public static implicit operator string(ColorMode_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ColorMode command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_ColorMode.ColorMode_name);
            }
            public class ColorMode_Desired
            {
                public static implicit operator string(ColorMode_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_ColorMode.ColorMode_name, ClusterDesired);
                }
            }
            public class ColorMode_Report
            {
                public static implicit operator string(ColorMode_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_ColorMode.ColorMode_name, ClusterReported);
                }
            }
            public class ColorMode_SupportCommands
            {
                public static implicit operator string(ColorMode_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_Options
        {
            public const string Options_name = "Options";
            public class Options_Attributes
            {
                public static implicit operator string(Options_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_Options command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_Options.Options_name);
            }
            public class Options_Desired
            {
                public static implicit operator string(Options_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_Options.Options_name, ClusterDesired);
                }
            }
            public class Options_Report
            {
                public static implicit operator string(Options_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_Options.Options_name, ClusterReported);
                }
            }
            public class Options_SupportCommands
            {
                public static implicit operator string(Options_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_ColorCapabilities
        {
            public const string ColorCapabilities_name = "ColorCapabilities";
            public class ColorCapabilities_Attributes
            {
                public static implicit operator string(ColorCapabilities_Attributes command)
                {
                    return SumAllAtt(ColorControlCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_ColorCapabilities command)
            {
                return SumATTStr(ColorControlCluster.ClusterName, ColorControlCluster.ATT_ColorCapabilities.ColorCapabilities_name);
            }
            public class ColorCapabilities_Desired
            {
                public static implicit operator string(ColorCapabilities_Desired command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_ColorCapabilities.ColorCapabilities_name, ClusterDesired);
                }
            }
            public class ColorCapabilities_Report
            {
                public static implicit operator string(ColorCapabilities_Report command)
                {
                    return SumATTStr_WithSubValue(ColorControlCluster.ClusterName, ColorControlCluster.ATT_ColorCapabilities.ColorCapabilities_name, ClusterReported);
                }
            }
            public class ColorCapabilities_SupportCommands
            {
                public static implicit operator string(ColorCapabilities_SupportCommands command)
                {
                    return SumSupCMD(ColorControlCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        #endregion

        #region Cluster Commands

        /*CLASSES AND ENUMS FOR COMMANDS*/
        public enum Direction
        {
            Up,
            Down,
            ShortestDistance,
            LongestDistance
        }
        public enum MoveMode
        {
            Stop,
            Up,
            Down
        }
        public enum StepMode
        {
            Up,
            Down
        }
        public class CCColorOptions
        {
            public bool ExecuteIfOff { get; set; }
        }
        /*SUPPORTED COMMANDS*/
        public class CMD_MoveToHue
        {
            public const string MoveToHue_name = "MoveToHue";
            public static implicit operator string(CMD_MoveToHue command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveToHue.MoveToHue_name);
            }
            public class CMD_MoveToHue_Payload
            {
                public int Hue { get; set; }
                [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
                public Direction Direction { get; set; }
                public int TransitionTime { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_MoveToHue_Payload(int my_Hue, Direction my_direction, int my_TransitionTime, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    Hue = my_Hue;
                    Direction = my_direction;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveToHue_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToHue_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToHue_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveHue
        {
            public const string MoveHue_name = "MoveHue";
            public static implicit operator string(CMD_MoveHue command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveHue.MoveHue_name);
            }
            public class CMD_MoveHue_Payload
            {
                [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
                public MoveMode MoveMode { get; set; }
                public int Rate { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }

                /*Start of public Payload*/
                public CMD_MoveHue_Payload(MoveMode my_MoveMode, int my_Rate, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    MoveMode = my_MoveMode;
                    Rate = my_Rate;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveHue_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveHue_Payload>(command);
                }
                public static implicit operator string(CMD_MoveHue_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StepHue
        {
            public const string StepHue_name = "StepHue";
            public static implicit operator string(CMD_StepHue command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_StepHue.StepHue_name);
            }
            public class CMD_StepHue_Payload
            {
                public StepMode StepMode { get; set; }
                public int StepSize { get; set; }
                public int TransitionTime { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }

                /*Start of public Payload*/
                public CMD_StepHue_Payload(StepMode my_StepMode, int my_StepSize, int my_TransitionTime, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    StepMode = my_StepMode;
                    StepSize = my_StepSize;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_StepHue_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StepHue_Payload>(command);
                }
                public static implicit operator string(CMD_StepHue_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveToSaturation
        {
            public const string MoveToSaturation_name = "MoveToSaturation";
            public static implicit operator string(CMD_MoveToSaturation command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveToSaturation.MoveToSaturation_name);
            }
            public class CMD_MoveToSaturation_Payload
            {
                public int Saturation { get; set; }
                public int TransitionTime { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }

                /*Start of public Payload*/
                public CMD_MoveToSaturation_Payload(int my_Saturation, int my_TransitionTime, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    Saturation = my_Saturation;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveToSaturation_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToSaturation_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToSaturation_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveSaturation
        {
            public const string MoveSaturation_name = "MoveSaturation";
            public static implicit operator string(CMD_MoveSaturation command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveSaturation.MoveSaturation_name);
            }
            public class CMD_MoveSaturation_Payload
            {
                public MoveMode MoveMode { get; set; }
                public int Rate { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }

                /*Start of public Payload*/
                public CMD_MoveSaturation_Payload(MoveMode my_MoveMode, int my_Rate, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    MoveMode = my_MoveMode;
                    Rate = my_Rate;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveSaturation_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveSaturation_Payload>(command);
                }
                public static implicit operator string(CMD_MoveSaturation_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StepSaturation
        {
            public const string StepSaturation_name = "StepSaturation";
            public static implicit operator string(CMD_StepSaturation command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_StepSaturation.StepSaturation_name);
            }
            public class CMD_StepSaturation_Payload
            {
                public StepMode StepMode { get; set; }
                public int StepSize { get; set; }
                public int TransitionTime { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }

                /*Start of public Payload*/
                public CMD_StepSaturation_Payload(StepMode my_StepMode, int my_StepSize, int my_TransitionTime, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    StepMode = my_StepMode;
                    StepSize = my_StepSize;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_StepSaturation_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StepSaturation_Payload>(command);
                }
                public static implicit operator string(CMD_StepSaturation_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveToHueAndSaturation
        {
            public const string MoveToHueAndSaturation_name = "MoveToHueAndSaturation";
            public static implicit operator string(CMD_MoveToHueAndSaturation command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveToHueAndSaturation.MoveToHueAndSaturation_name);
            }
            public class CMD_MoveToHueAndSaturation_Payload
            {
                public int Hue { get; set; }
                public int Saturation { get; set; }
                public int TransitionTime { get; set; }
                public CCColorOptions OptionsMask { get; set; }
                public CCColorOptions OptionsOverride { get; set; }

                /*Start of public Payload*/
                public CMD_MoveToHueAndSaturation_Payload(int my_Hue, int my_Saturation, int my_TransitionTime, CCColorOptions my_OptionsMask, CCColorOptions my_OptionsOverride)
                {
                    Hue = my_Hue;
                    Saturation = my_Saturation;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveToHueAndSaturation_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToHueAndSaturation_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToHueAndSaturation_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        /*NOT SUPPORTED COMMANDS - NOT COMPLETED*/
        public class CMD_MoveToColor
        {
            public const string MoveToColor_name = "MoveToColor";
            public static implicit operator string(CMD_MoveToColor command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveToColor.MoveToColor_name);
            }

            public class CMD_MoveToColor_Payload
            {
                public int ColorX { get; set; }
                public int ColorY { get; set; }
                public int TransitionTime { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_MoveToColor_Payload(int my_ColorX, int my_ColorY, int my_TransitionTime, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    ColorX = my_ColorX;
                    ColorY = my_ColorY;
                    TransitionTime = my_TransitionTime;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveToColor_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToColor_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToColor_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveColor
        {
            public const string MoveColor_name = "MoveColor";
            public static implicit operator string(CMD_MoveColor command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveColor.MoveColor_name);
            }

            public class CMD_MoveColor_Payload
            {
                public int RateX { get; set; }
                public int RateY { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_MoveColor_Payload(int my_RateX, int my_RateY, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    RateX = my_RateX;
                    RateY = my_RateY;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveColor_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveColor_Payload>(command);
                }
                public static implicit operator string(CMD_MoveColor_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StepColor
        {
            public const string StepColor_name = "StepColor";
            public static implicit operator string(CMD_StepColor command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_StepColor.StepColor_name);
            }

            public class CMD_StepColor_Payload
            {
                public int StepX { get; set; }
                public int StepY { get; set; }
                public int TransitionTime { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_StepColor_Payload(int my_StepX, int my_StepY, int my_TransitionTime, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    StepX = my_StepX;
                    StepY = my_StepY;
                    TransitionTime = my_TransitionTime;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_StepColor_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StepColor_Payload>(command);
                }
                public static implicit operator string(CMD_StepColor_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveToColorTemperature
        {
            public const string MoveToColorTemperature_name = "MoveToColorTemperature";
            public static implicit operator string(CMD_MoveToColorTemperature command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveToColorTemperature.MoveToColorTemperature_name);
            }

            public class CMD_MoveToColorTemperature_Payload
            {
                public int ColorTemperatureMireds { get; set; }
                public int TransitionTime { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_MoveToColorTemperature_Payload(int my_ColorTemperatureMireds, int my_TransitionTime, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    ColorTemperatureMireds = my_ColorTemperatureMireds;
                    TransitionTime = my_TransitionTime;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveToColorTemperature_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToColorTemperature_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToColorTemperature_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_EnhancedMoveToHue
        {
            public const string EnhancedMoveToHue_name = "EnhancedMoveToHue";
            public static implicit operator string(CMD_EnhancedMoveToHue command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_EnhancedMoveToHue.EnhancedMoveToHue_name);
            }

            public class CMD_EnhancedMoveToHue_Payload
            {
                public int EnhancedHue { get; set; }
                public struct Direction_ShortestDistance
                {
                    public string ShortestDistance { get; set; }
                }
                public struct Direction_LongestDistance
                {
                    public string LongestDistance { get; set; }
                }
                public struct Direction_Up
                {
                    public string Up { get; set; }
                }
                public struct Direction_Down
                {
                    public string Down { get; set; }
                }
                public class Direction
                {
                    public Direction_ShortestDistance my_ShortestDistance { get; set; }
                    public Direction_LongestDistance my_LongestDistance { get; set; }
                    public Direction_Up my_Up { get; set; }
                    public Direction_Down my_Down { get; set; }
                }
                public int TransitionTime { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_EnhancedMoveToHue_Payload(int my_EnhancedHue, Direction my_direction, int my_TransitionTime, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    EnhancedHue = my_EnhancedHue;
                    Direction Direction_payload = my_direction;
                    TransitionTime = my_TransitionTime;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_EnhancedMoveToHue_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_EnhancedMoveToHue_Payload>(command);
                }
                public static implicit operator string(CMD_EnhancedMoveToHue_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_EnhancedMoveHue
        {
            public const string EnhancedMoveHue_name = "EnhancedMoveHue";
            public static implicit operator string(CMD_EnhancedMoveHue command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_EnhancedMoveHue.EnhancedMoveHue_name);
            }

            public class CMD_EnhancedMoveHue_Payload
            {
                public struct MoveMode_Stop
                {
                    public string Stop { get; set; }
                }
                public struct MoveMode_Up
                {
                    public string Up { get; set; }
                }
                public struct MoveMode_Down
                {
                    public string Down { get; set; }
                }
                public class MoveMode
                {
                    public MoveMode_Stop my_Stop { get; set; }
                    public MoveMode_Up my_Up { get; set; }
                    public MoveMode_Down my_Down { get; set; }
                }
                public int Rate { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_EnhancedMoveHue_Payload(MoveMode my_MoveMode, int my_Rate, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    MoveMode MoveMode_payload = my_MoveMode;
                    Rate = my_Rate;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_EnhancedMoveHue_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_EnhancedMoveHue_Payload>(command);
                }
                public static implicit operator string(CMD_EnhancedMoveHue_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_EnhancedStepHue
        {
            public const string EnhancedStepHue_name = "EnhancedStepHue";
            public static implicit operator string(CMD_EnhancedStepHue command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_EnhancedStepHue.EnhancedStepHue_name);
            }

            public class CMD_EnhancedStepHue_Payload
            {
                public struct StepMode_Up
                {
                    public string Up { get; set; }
                }
                public struct StepMode_Down
                {
                    public string Down { get; set; }
                }
                public class StepMode
                {
                    public StepMode_Up my_Up { get; set; }
                    public StepMode_Down my_Down { get; set; }
                }
                public int StepSize { get; set; }
                public int TransitionTime { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_EnhancedStepHue_Payload(StepMode my_StepMode, int my_StepSize, int my_TransitionTime, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    StepMode MoveMode_payload = my_StepMode;
                    StepSize = my_StepSize;
                    TransitionTime = my_TransitionTime;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_EnhancedStepHue_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_EnhancedStepHue_Payload>(command);
                }
                public static implicit operator string(CMD_EnhancedStepHue_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_EnhancedMoveToHueAndSaturation
        {
            public const string EnhancedMoveToHueAndSaturation_name = "EnhancedMoveToHueAndSaturation";
            public static implicit operator string(CMD_EnhancedMoveToHueAndSaturation command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_EnhancedMoveToHueAndSaturation.EnhancedMoveToHueAndSaturation_name);
            }

            public class CMD_EnhancedMoveToHueAndSaturation_Payload
            {
                public int EnhancedHue { get; set; }
                public int Saturation { get; set; }
                public int TransitionTime { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_EnhancedMoveToHueAndSaturation_Payload(int my_EnhancedHue, int my_Saturation, int my_TransitionTime, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    EnhancedHue = my_EnhancedHue;
                    Saturation = my_Saturation;
                    TransitionTime = my_TransitionTime;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_EnhancedMoveToHueAndSaturation_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_EnhancedMoveToHueAndSaturation_Payload>(command);
                }
                public static implicit operator string(CMD_EnhancedMoveToHueAndSaturation_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_ColorLoopSet
        {
            public const string ColorLoopSet_name = "ColorLoopSet";
            public static implicit operator string(CMD_ColorLoopSet command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_ColorLoopSet.ColorLoopSet_name);
            }

            public class CMD_ColorLoopSet_Payload
            {
                public struct UpdateFlags_UpdateAction
                {
                    public string UpdateAction { get; set; }
                }
                public struct UpdateFlags_UpdateDirection
                {
                    public string UpdateDirection { get; set; }
                }
                public struct UpdateFlags_UpdateTime
                {
                    public string UpdateDirection { get; set; }
                }
                public struct UpdateFlags_UpdateStartHue
                {
                    public string UpdateStartHue { get; set; }
                }
                public class UpdateFlags
                {
                    public UpdateFlags_UpdateAction my_UpdateAction { get; set; }
                    public UpdateFlags_UpdateDirection my_UpdateDirection { get; set; }
                    public UpdateFlags_UpdateTime my_UpdateTime { get; set; }
                    public UpdateFlags_UpdateStartHue my_UpdateStartHue { get; set; }
                }
                public struct Action_DeactivateColorLoop
                {
                    public string DeactivateColorLoop { get; set; }
                }
                public struct Action_ActivateColorLoopFromColorLoopStartEnhancedHue
                {
                    public string ActivateColorLoopFromColorLoopStartEnhancedHue { get; set; }
                }
                public struct Action_ActivateColorLoopFromEnhancedCurrentHue
                {
                    public string ActivateColorLoopFromEnhancedCurrentHue { get; set; }
                }
                public class Action
                {
                    public Action_DeactivateColorLoop my_DeactivateColorLoop { get; set; }
                    public Action_ActivateColorLoopFromColorLoopStartEnhancedHue my_ActivateColorLoopFromColorLoopStartEnhancedHue { get; set; }
                    public Action_ActivateColorLoopFromEnhancedCurrentHue my_ActivateColorLoopFromEnhancedCurrentHue { get; set; }

                }
                public struct Direction_DecrementEnhancedCurrentHue
                {
                    public string DecrementEnhancedCurrentHue { get; set; }
                }
                public struct Direction_IncrementEnhancedCurrentHue
                {
                    public string IncrementEnhancedCurrentHue { get; set; }
                }
                public class Direction
                {
                    public Direction_DecrementEnhancedCurrentHue my_DecrementEnhancedCurrentHue { get; set; }
                    public Direction_IncrementEnhancedCurrentHue my_IncrementEnhancedCurrentHue { get; set; }

                }
                public int Time { get; set; }
                public int StartHue { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_ColorLoopSet_Payload(UpdateFlags my_UpdateFlags, Action my_Action, Direction my_Direction, int my_Time, int my_StartHue, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    UpdateFlags UpdateFlags_payload = my_UpdateFlags;
                    Action Action_payload = my_Action;
                    Direction Direction_payload = my_Direction;
                    Time = my_Time;
                    StartHue = my_StartHue;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_ColorLoopSet_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ColorLoopSet_Payload>(command);
                }
                public static implicit operator string(CMD_ColorLoopSet_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StopMoveStep
        {
            public const string StopMoveStep_name = "StopMoveStep";
            public static implicit operator string(CMD_StopMoveStep command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_StopMoveStep.StopMoveStep_name);
            }

            public class CMD_StopMoveStep_Payload
            {
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_StopMoveStep_Payload(OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_StopMoveStep_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StopMoveStep_Payload>(command);
                }
                public static implicit operator string(CMD_StopMoveStep_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveColorTemperature
        {
            public const string MoveColorTemperature_name = "MoveColorTemperature";
            public static implicit operator string(CMD_MoveColorTemperature command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_MoveColorTemperature.MoveColorTemperature_name);
            }

            public class CMD_MoveColorTemperature_Payload
            {
                public struct MoveMode_Stop
                {
                    public string Stop { get; set; }
                }
                public struct MoveMode_Up
                {
                    public string Up { get; set; }
                }
                public struct MoveMode_Down
                {
                    public string Down { get; set; }
                }
                public class MoveMode
                {
                    public MoveMode_Stop my_Stop { get; set; }
                    public MoveMode_Up my_Up { get; set; }
                    public MoveMode_Down my_Down { get; set; }
                }
                public int Rate { get; set; }
                public int ColorTemperatureMinimumMireds { get; set; }
                public int ColorTemperatureMaximumMireds { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_MoveColorTemperature_Payload(MoveMode my_MoveMode, int my_Rate, int my_ColorTemperatureMinimumMireds, int my_ColorTemperatureMaximumMireds, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    MoveMode ModeMode_payload = my_MoveMode;
                    ColorTemperatureMinimumMireds = my_ColorTemperatureMinimumMireds;
                    ColorTemperatureMaximumMireds = my_ColorTemperatureMaximumMireds;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_MoveColorTemperature_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveColorTemperature_Payload>(command);
                }
                public static implicit operator string(CMD_MoveColorTemperature_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StepColorTemperature
        {
            public const string StepColorTemperature_name = "StepColorTemperature";
            public static implicit operator string(CMD_StepColorTemperature command)
            {
                return SumCMDStr(ColorControlCluster.ClusterName, ColorControlCluster.CMD_StepColorTemperature.StepColorTemperature_name);
            }

            public class CMD_StepColorTemperature_Payload
            {
                public struct StepMode_Up
                {
                    public string Up { get; set; }
                }
                public struct StepMode_Down
                {
                    public string Down { get; set; }
                }
                public class StepMode
                {
                    public StepMode_Up my_Up { get; set; }
                    public StepMode_Down my_Down { get; set; }
                }
                public int StepSize { get; set; }
                public int TransitionTime { get; set; }
                public int ColorTemperatureMinimumMireds { get; set; }
                public int ColorTemperatureMaximumMireds { get; set; }
                public struct CCColOp_ExecuteIfOff
                {
                    public string ExecuteIfOff { get; set; }
                }
                public class OptionsMask
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }
                public class OptionsOverride
                {
                    public CCColOp_ExecuteIfOff my_ExecuteIfOff { get; set; }

                }

                /*Start of public Payload*/
                public CMD_StepColorTemperature_Payload(StepMode my_StepMode, int my_StepSize, int my_TransitionTime, int my_ColorTemperatureMinimumMireds, int my_ColorTemperatureMaximumMireds, OptionsMask my_OptionsMask, OptionsOverride my_OptionsOverride)
                {
                    StepMode MoveMode_payload = my_StepMode;
                    StepSize = my_StepSize;
                    TransitionTime = my_TransitionTime;
                    ColorTemperatureMinimumMireds = my_ColorTemperatureMinimumMireds;
                    ColorTemperatureMaximumMireds = my_ColorTemperatureMaximumMireds;
                    OptionsMask OptionsMask_payload = my_OptionsMask;
                    OptionsOverride OptionsOverride_payload = my_OptionsOverride;
                }/*End of public Payload*/
                public static implicit operator CMD_StepColorTemperature_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StepColorTemperature_Payload>(command);
                }
                public static implicit operator string(CMD_StepColorTemperature_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/

        #endregion

        #region Cluster Operations

        public class Payload_Att_SupportedCommands
        {
            public class Payload
            {
                public List<string> value { get; set; }
            }

            public Payload _payload { get; set; }

            public Payload_Att_SupportedCommands(Payload payload)
            {
                _payload = payload;
            }

            public Payload_Att_SupportedCommands()
            {
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

        public static implicit operator ColorControlCluster(string options)
        {
            return JsonConvert.DeserializeObject<ColorControlCluster>(options);
        }
        public static implicit operator string(ColorControlCluster command)
        {
            return ColorControlCluster.ClusterName;
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
