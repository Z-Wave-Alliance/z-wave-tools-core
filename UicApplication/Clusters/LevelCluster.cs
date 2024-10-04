/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Newtonsoft.Json;

namespace UicApplication.Clusters
{
    public class LevelCluster
    {
        public const string ClusterName = "Level";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_CurrentLevel
        {
            public const string CurrentLevel_name = "CurrentLevel";
            public class CurrentLevel_Attributes
            {
                public static implicit operator string(CurrentLevel_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_CurrentLevel command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_CurrentLevel.CurrentLevel_name);
            }
            public class CurrentLevel_Desired
            {
                public static implicit operator string(CurrentLevel_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_CurrentLevel.CurrentLevel_name, ClusterDesired);
                }
            }
            public class CurrentLevel_Report
            {
                public static implicit operator string(CurrentLevel_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_CurrentLevel.CurrentLevel_name, ClusterReported);
                }
            }
            public class CurrentLevel_SupportCommands
            {
                public static implicit operator string(CurrentLevel_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinLevel_ref = "MinLevel";

                public const string MaxLevel_ref = "MaxLevel";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_RemainingTime
        {
            public const string RemainingTime_name = "RemainingTime";
            public class RemainingTime_Attributes
            {
                public static implicit operator string(RemainingTime_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_RemainingTime command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_RemainingTime.RemainingTime_name);
            }
            public class RemainingTime_Desired
            {
                public static implicit operator string(RemainingTime_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_RemainingTime.RemainingTime_name, ClusterDesired);
                }
            }
            public class RemainingTime_Report
            {
                public static implicit operator string(RemainingTime_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_RemainingTime.RemainingTime_name, ClusterReported);
                }
            }
            public class RemainingTime_SupportCommands
            {
                public static implicit operator string(RemainingTime_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_MinLevel
        {
            public const string MinLevel_name = "MinLevel";
            public class MinLevel_Attributes
            {
                public static implicit operator string(MinLevel_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinLevel command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_MinLevel.MinLevel_name);
            }
            public class MinLevel_Desired
            {
                public static implicit operator string(MinLevel_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MinLevel.MinLevel_name, ClusterDesired);
                }
            }
            public class MinLevel_Report
            {
                public static implicit operator string(MinLevel_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MinLevel.MinLevel_name, ClusterReported);
                }
            }
            public class MinLevel_SupportCommands
            {
                public static implicit operator string(MinLevel_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MaxLevel_ref = "MaxLevel";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_MaxLevel
        {
            public const string MaxLevel_name = "MaxLevel";
            public class MaxLevel_Attributes
            {
                public static implicit operator string(MaxLevel_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MaxLevel command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_MaxLevel.MaxLevel_name);
            }
            public class MaxLevel_Desired
            {
                public static implicit operator string(MaxLevel_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MaxLevel.MaxLevel_name, ClusterDesired);
                }
            }
            public class MaxLevel_Report
            {
                public static implicit operator string(MaxLevel_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MaxLevel.MaxLevel_name, ClusterReported);
                }
            }
            public class MaxLevel_SupportCommands
            {
                public static implicit operator string(MaxLevel_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinLevel_ref = "MinLevel";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_CurrentFrequency
        {
            public const string CurrentFrequency_name = "CurrentFrequency";
            public class CurrentFrequency_Attributes
            {
                public static implicit operator string(CurrentFrequency_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_CurrentFrequency command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_CurrentFrequency.CurrentFrequency_name);
            }
            public class CurrentFrequency_Desired
            {
                public static implicit operator string(CurrentFrequency_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_CurrentFrequency.CurrentFrequency_name, ClusterDesired);
                }
            }
            public class CurrentFrequency_Report
            {
                public static implicit operator string(CurrentFrequency_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_CurrentFrequency.CurrentFrequency_name, ClusterReported);
                }
            }
            public class CurrentFrequency_SupportCommands
            {
                public static implicit operator string(CurrentFrequency_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinFrequency_ref = "MinFrequency";

                public const string MaxFrequency_ref = "MaxFrequency";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_MinFrequency
        {
            public const string MinFrequency_name = "MinFrequency";
            public class MinFrequency_Attributes
            {
                public static implicit operator string(MinFrequency_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MinFrequency command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_MinFrequency.MinFrequency_name);
            }
            public class MinFrequency_Desired
            {
                public static implicit operator string(MinFrequency_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MinFrequency.MinFrequency_name, ClusterDesired);
                }
            }
            public class MinFrequency_Report
            {
                public static implicit operator string(MinFrequency_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MinFrequency.MinFrequency_name, ClusterReported);
                }
            }
            public class MinFrequency_SupportCommands
            {
                public static implicit operator string(MinFrequency_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MaxFrequency_ref = "MaxFrequency";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_MaxFrequency
        {
            public const string MaxFrequency_name = "MaxFrequency";
            public class MaxFrequency_Attributes
            {
                public static implicit operator string(MaxFrequency_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_MaxFrequency command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_MaxFrequency.MaxFrequency_name);
            }
            public class MaxFrequency_Desired
            {
                public static implicit operator string(MaxFrequency_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MaxFrequency.MaxFrequency_name, ClusterDesired);
                }
            }
            public class MaxFrequency_Report
            {
                public static implicit operator string(MaxFrequency_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_MaxFrequency.MaxFrequency_name, ClusterReported);
                }
            }
            public class MaxFrequency_SupportCommands
            {
                public static implicit operator string(MaxFrequency_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinFrequency_ref = "MinFrequency";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_OnOffTransitionTime
        {
            public const string OnOffTransitionTime_name = "OnOffTransitionTime";
            public class OnOffTransitionTime_Attributes
            {
                public static implicit operator string(OnOffTransitionTime_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OnOffTransitionTime command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_OnOffTransitionTime.OnOffTransitionTime_name);
            }
            public class OnOffTransitionTime_Desired
            {
                public static implicit operator string(OnOffTransitionTime_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OnOffTransitionTime.OnOffTransitionTime_name, ClusterDesired);
                }
            }
            public class OnOffTransitionTime_Report
            {
                public static implicit operator string(OnOffTransitionTime_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OnOffTransitionTime.OnOffTransitionTime_name, ClusterReported);
                }
            }
            public class OnOffTransitionTime_SupportCommands
            {
                public static implicit operator string(OnOffTransitionTime_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OnLevel
        {
            public const string OnLevel_name = "OnLevel";
            public class OnLevel_Attributes
            {
                public static implicit operator string(OnLevel_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OnLevel command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_OnLevel.OnLevel_name);
            }
            public class OnLevel_Desired
            {
                public static implicit operator string(OnLevel_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OnLevel.OnLevel_name, ClusterDesired);
                }
            }
            public class OnLevel_Report
            {
                public static implicit operator string(OnLevel_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OnLevel.OnLevel_name, ClusterReported);
                }
            }
            public class OnLevel_SupportCommands
            {
                public static implicit operator string(OnLevel_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public const string MinLevel_ref = "MinLevel";

                public const string MaxLevel_ref = "MaxLevel";
            }/*End of Restriction*/
        }/*End of ATT class*/
        public class ATT_OnTransitionTime
        {
            public const string OnTransitionTime_name = "OnTransitionTime";
            public class OnTransitionTime_Attributes
            {
                public static implicit operator string(OnTransitionTime_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OnTransitionTime command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_OnTransitionTime.OnTransitionTime_name);
            }
            public class OnTransitionTime_Desired
            {
                public static implicit operator string(OnTransitionTime_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OnTransitionTime.OnTransitionTime_name, ClusterDesired);
                }
            }
            public class OnTransitionTime_Report
            {
                public static implicit operator string(OnTransitionTime_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OnTransitionTime.OnTransitionTime_name, ClusterReported);
                }
            }
            public class OnTransitionTime_SupportCommands
            {
                public static implicit operator string(OnTransitionTime_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OffTransitionTime
        {
            public const string OffTransitionTime_name = "OffTransitionTime";
            public class OffTransitionTime_Attributes
            {
                public static implicit operator string(OffTransitionTime_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OffTransitionTime command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_OffTransitionTime.OffTransitionTime_name);
            }
            public class OffTransitionTime_Desired
            {
                public static implicit operator string(OffTransitionTime_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OffTransitionTime.OffTransitionTime_name, ClusterDesired);
                }
            }
            public class OffTransitionTime_Report
            {
                public static implicit operator string(OffTransitionTime_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_OffTransitionTime.OffTransitionTime_name, ClusterReported);
                }
            }
            public class OffTransitionTime_SupportCommands
            {
                public static implicit operator string(OffTransitionTime_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_DefaultMoveRate
        {
            public const string DefaultMoveRate_name = "DefaultMoveRate";
            public class DefaultMoveRate_Attributes
            {
                public static implicit operator string(DefaultMoveRate_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_DefaultMoveRate command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_DefaultMoveRate.DefaultMoveRate_name);
            }
            public class DefaultMoveRate_Desired
            {
                public static implicit operator string(DefaultMoveRate_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_DefaultMoveRate.DefaultMoveRate_name, ClusterDesired);
                }
            }
            public class DefaultMoveRate_Report
            {
                public static implicit operator string(DefaultMoveRate_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_DefaultMoveRate.DefaultMoveRate_name, ClusterReported);
                }
            }
            public class DefaultMoveRate_SupportCommands
            {
                public static implicit operator string(DefaultMoveRate_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
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
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_Options command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_Options.Options_name);
            }
            public class Options_Desired
            {
                public static implicit operator string(Options_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_Options.Options_name, ClusterDesired);
                }
            }
            public class Options_Report
            {
                public static implicit operator string(Options_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_Options.Options_name, ClusterReported);
                }
            }
            public class Options_SupportCommands
            {
                public static implicit operator string(Options_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_StartUpCurrentLevel
        {
            public const string StartUpCurrentLevel_name = "StartUpCurrentLevel";
            public class StartUpCurrentLevel_Attributes
            {
                public static implicit operator string(StartUpCurrentLevel_Attributes command)
                {
                    return SumAllAtt(LevelCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_StartUpCurrentLevel command)
            {
                return SumATTStr(LevelCluster.ClusterName, LevelCluster.ATT_StartUpCurrentLevel.StartUpCurrentLevel_name);
            }
            public class StartUpCurrentLevel_Desired
            {
                public static implicit operator string(StartUpCurrentLevel_Desired command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_StartUpCurrentLevel.StartUpCurrentLevel_name, ClusterDesired);
                }
            }
            public class StartUpCurrentLevel_Report
            {
                public static implicit operator string(StartUpCurrentLevel_Report command)
                {
                    return SumATTStr_WithSubValue(LevelCluster.ClusterName, LevelCluster.ATT_StartUpCurrentLevel.StartUpCurrentLevel_name, ClusterReported);
                }
            }
            public class StartUpCurrentLevel_SupportCommands
            {
                public static implicit operator string(StartUpCurrentLevel_SupportCommands command)
                {
                    return SumSupCMD(LevelCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class MinimumDeviceValuePermitted
                {
                    public const string MinimumDeviceValuePermitted_value = "00";
                    public const string MinimumDeviceValuePermitted_name = "MinimumDeviceValuePermitted";
                }

                public class SetToPreviousValue
                {
                    public const string SetToPreviousValue_value = "ff";
                    public const string SetToPreviousValue_name = "SetToPreviousValue";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        #endregion

        public struct LevelOptions
        {
            public bool ExecuteIfOff { get; set; }
            public bool CoupleColorTempToLevel { get; set; }
        }

        #region Cluster Commands

        public class CMD_MoveToLevel
        {
            public const string MoveToLevel_name = "MoveToLevel";
            public static implicit operator string(CMD_MoveToLevel command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_MoveToLevel.MoveToLevel_name);
            }
            public class CMD_MoveToLevel_Payload
            {
                public int Level { get; set; }
                public int TransitionTime { get; set; }
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_MoveToLevel_Payload(int my_Level, int my_TransitionTime, int my_OptionsMask, int my_OptionsOverride)
                {
                    Level = my_Level;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_MoveToLevel_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToLevel_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToLevel_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_Move
        {
            public const string Move_name = "Move";
            public static implicit operator string(CMD_Move command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_Move.Move_name);
            }
            public class CMD_Move_Payload
            {
                public int MoveMode { get; set; }
                public int Rate { get; set; }
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_Move_Payload(int my_MoveMode, int my_Rate, int my_OptionsMask, int my_OptionsOverride)
                {
                    MoveMode = my_MoveMode;
                    Rate = my_Rate;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_Move_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_Move_Payload>(command);
                }
                public static implicit operator string(CMD_Move_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_Step
        {
            public const string Step_name = "Step";
            public static implicit operator string(CMD_Step command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_Step.Step_name);
            }
            public class CMD_Step_Payload
            {
                public int StepMode { get; set; }
                public int StepSize { get; set; }
                public int TransitionTime { get; set; }
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_Step_Payload(int my_StepMode, int my_StepSize, int my_TransitionTime, int my_OptionsMask, int my_OptionsOverride)
                {
                    StepMode = my_StepMode;
                    StepSize = my_StepSize;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_Step_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_Step_Payload>(command);
                }
                public static implicit operator string(CMD_Step_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_Stop
        {
            public const string Stop_name = "Stop";
            public static implicit operator string(CMD_Stop command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_Stop.Stop_name);
            }
            public class CMD_Stop_Payload
            {
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_Stop_Payload(int my_OptionsMask, int my_OptionsOverride)
                {
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_Stop_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_Stop_Payload>(command);
                }
                public static implicit operator string(CMD_Stop_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveToLevelWithOnOff
        {
            public const string MoveToLevelWithOnOff_name = "MoveToLevelWithOnOff";
            public static implicit operator string(CMD_MoveToLevelWithOnOff command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_MoveToLevelWithOnOff.MoveToLevelWithOnOff_name);
            }
            public class CMD_MoveToLevelWithOnOff_Payload
            {
                public int Level { get; set; }
                public int TransitionTime { get; set; }
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_MoveToLevelWithOnOff_Payload(int my_Level, int my_TransitionTime, int my_OptionsMask, int my_OptionsOverride)
                {
                    Level = my_Level;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_MoveToLevelWithOnOff_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToLevelWithOnOff_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToLevelWithOnOff_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveWithOnOff
        {
            public const string MoveWithOnOff_name = "MoveWithOnOff";
            public static implicit operator string(CMD_MoveWithOnOff command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_MoveWithOnOff.MoveWithOnOff_name);
            }
            public class CMD_MoveWithOnOff_Payload
            {
                public int MoveMode { get; set; }
                public int Rate { get; set; }
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_MoveWithOnOff_Payload(int my_MoveMode, int my_Rate, int my_OptionsMask, int my_OptionsOverride)
                {
                    MoveMode = my_MoveMode;
                    Rate = my_Rate;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_MoveWithOnOff_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveWithOnOff_Payload>(command);
                }
                public static implicit operator string(CMD_MoveWithOnOff_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StepWithOnOff
        {
            public const string StepWithOnOff_name = "StepWithOnOff";
            public static implicit operator string(CMD_StepWithOnOff command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_StepWithOnOff.StepWithOnOff_name);
            }
            public class CMD_StepWithOnOff_Payload
            {
                public int StepMode { get; set; }
                public int StepSize { get; set; }
                public int TransitionTime { get; set; }
                public int OptionsMask { get; set; }
                public int OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_StepWithOnOff_Payload(int my_StepMode, int my_StepSize, int my_TransitionTime, int my_OptionsMask, int my_OptionsOverride)
                {
                    StepMode = my_StepMode;
                    StepSize = my_StepSize;
                    TransitionTime = my_TransitionTime;
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_StepWithOnOff_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StepWithOnOff_Payload>(command);
                }
                public static implicit operator string(CMD_StepWithOnOff_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_StopWithOnOff
        {
            public const string StopWithOnOff_name = "StopWithOnOff";
            public static implicit operator string(CMD_StopWithOnOff command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_StopWithOnOff.StopWithOnOff_name);
            }
            public class CMD_StopWithOnOff_Payload
            {
                public string OptionsMask { get; set; }
                public string OptionsOverride { get; set; }
                /*Start of public Payload*/
                public CMD_StopWithOnOff_Payload(string my_OptionsMask, string my_OptionsOverride)
                {
                    OptionsMask = my_OptionsMask;
                    OptionsOverride = my_OptionsOverride;

                }
                /*End of public Payload*/
                public static implicit operator CMD_StopWithOnOff_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_StopWithOnOff_Payload>(command);
                }
                public static implicit operator string(CMD_StopWithOnOff_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_MoveToClosestFrequency
        {
            public const string MoveToClosestFrequency_name = "MoveToClosestFrequency";
            public static implicit operator string(CMD_MoveToClosestFrequency command)
            {
                return SumCMDStr(LevelCluster.ClusterName, LevelCluster.CMD_MoveToClosestFrequency.MoveToClosestFrequency_name);
            }
            public class CMD_MoveToClosestFrequency_Payload
            {
                public string Frequency { get; set; }
                /*Start of public Payload*/
                public CMD_MoveToClosestFrequency_Payload(string my_Frequency)
                {
                    Frequency = my_Frequency;

                }
                /*End of public Payload*/
                public static implicit operator CMD_MoveToClosestFrequency_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_MoveToClosestFrequency_Payload>(command);
                }
                public static implicit operator string(CMD_MoveToClosestFrequency_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
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

        public static implicit operator LevelCluster(string options)
        {
            return JsonConvert.DeserializeObject<LevelCluster>(options);
        }
        public static implicit operator string(LevelCluster command)
        {
            return LevelCluster.ClusterName;
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
