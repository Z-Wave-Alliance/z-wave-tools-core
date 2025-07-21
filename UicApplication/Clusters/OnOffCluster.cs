/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Newtonsoft.Json;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class OnOffCluster
    {
        public const string ClusterName = "OnOff";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_OnOff
        {
            public const string OnOff_name = "OnOff";
            public class OnOff_Attributes
            {
                public static implicit operator string(OnOff_Attributes command)
                {
                    return SumAllAtt(OnOffCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OnOff command)
            {
                return SumATTStr(OnOffCluster.ClusterName, OnOffCluster.ATT_OnOff.OnOff_name);
            }
            public class OnOff_Desired
            {
                public static implicit operator string(OnOff_Desired command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_OnOff.OnOff_name, ClusterDesired);
                }
            }
            public class OnOff_Report
            {
                public static implicit operator string(OnOff_Report command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_OnOff.OnOff_name, ClusterReported);
                }
            }
            public class OnOff_SupportCommands
            {
                public static implicit operator string(OnOff_SupportCommands command)
                {
                    return SumSupCMD(OnOffCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_GlobalSceneControl
        {
            public const string GlobalSceneControl_name = "GlobalSceneControl";
            public class GlobalSceneControl_Attributes
            {
                public static implicit operator string(GlobalSceneControl_Attributes command)
                {
                    return SumAllAtt(OnOffCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_GlobalSceneControl command)
            {
                return SumATTStr(OnOffCluster.ClusterName, OnOffCluster.ATT_GlobalSceneControl.GlobalSceneControl_name);
            }
            public class GlobalSceneControl_Desired
            {
                public static implicit operator string(GlobalSceneControl_Desired command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_GlobalSceneControl.GlobalSceneControl_name, ClusterDesired);
                }
            }
            public class GlobalSceneControl_Report
            {
                public static implicit operator string(GlobalSceneControl_Report command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_GlobalSceneControl.GlobalSceneControl_name, ClusterReported);
                }
            }
            public class GlobalSceneControl_SupportCommands
            {
                public static implicit operator string(GlobalSceneControl_SupportCommands command)
                {
                    return SumSupCMD(OnOffCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OnTime
        {
            public const string OnTime_name = "OnTime";
            public class OnTime_Attributes
            {
                public static implicit operator string(OnTime_Attributes command)
                {
                    return SumAllAtt(OnOffCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OnTime command)
            {
                return SumATTStr(OnOffCluster.ClusterName, OnOffCluster.ATT_OnTime.OnTime_name);
            }
            public class OnTime_Desired
            {
                public static implicit operator string(OnTime_Desired command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_OnTime.OnTime_name, ClusterDesired);
                }
            }
            public class OnTime_Report
            {
                public static implicit operator string(OnTime_Report command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_OnTime.OnTime_name, ClusterReported);
                }
            }
            public class OnTime_SupportCommands
            {
                public static implicit operator string(OnTime_SupportCommands command)
                {
                    return SumSupCMD(OnOffCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_OffWaitTime
        {
            public const string OffWaitTime_name = "OffWaitTime";
            public class OffWaitTime_Attributes
            {
                public static implicit operator string(OffWaitTime_Attributes command)
                {
                    return SumAllAtt(OnOffCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_OffWaitTime command)
            {
                return SumATTStr(OnOffCluster.ClusterName, OnOffCluster.ATT_OffWaitTime.OffWaitTime_name);
            }
            public class OffWaitTime_Desired
            {
                public static implicit operator string(OffWaitTime_Desired command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_OffWaitTime.OffWaitTime_name, ClusterDesired);
                }
            }
            public class OffWaitTime_Report
            {
                public static implicit operator string(OffWaitTime_Report command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_OffWaitTime.OffWaitTime_name, ClusterReported);
                }
            }
            public class OffWaitTime_SupportCommands
            {
                public static implicit operator string(OffWaitTime_SupportCommands command)
                {
                    return SumSupCMD(OnOffCluster.ClusterName, ClusterSupportedCommands);
                }
            }
        }/*End of ATT class*/
        public class ATT_StartUpOnOff
        {
            public const string StartUpOnOff_name = "StartUpOnOff";
            public class StartUpOnOff_Attributes
            {
                public static implicit operator string(StartUpOnOff_Attributes command)
                {
                    return SumAllAtt(OnOffCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_StartUpOnOff command)
            {
                return SumATTStr(OnOffCluster.ClusterName, OnOffCluster.ATT_StartUpOnOff.StartUpOnOff_name);
            }
            public class StartUpOnOff_Desired
            {
                public static implicit operator string(StartUpOnOff_Desired command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_StartUpOnOff.StartUpOnOff_name, ClusterDesired);
                }
            }
            public class StartUpOnOff_Report
            {
                public static implicit operator string(StartUpOnOff_Report command)
                {
                    return SumATTStr_WithSubValue(OnOffCluster.ClusterName, OnOffCluster.ATT_StartUpOnOff.StartUpOnOff_name, ClusterReported);
                }
            }
            public class StartUpOnOff_SupportCommands
            {
                public static implicit operator string(StartUpOnOff_SupportCommands command)
                {
                    return SumSupCMD(OnOffCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Restriction
            {
                public class SetOnOffTo0
                {
                    public const string SetOnOffTo0_value = "00";
                    public const string SetOnOffTo0_name = "SetOnOffTo0";
                }

                public class SetOnOffTo1
                {
                    public const string SetOnOffTo1_value = "01";
                    public const string SetOnOffTo1_name = "SetOnOffTo1";
                }

                public class TogglePreviousOnOff
                {
                    public const string TogglePreviousOnOff_value = "02";
                    public const string TogglePreviousOnOff_name = "TogglePreviousOnOff";
                }

                public class SetPreviousOnOff
                {
                    public const string SetPreviousOnOff_value = "FF";
                    public const string SetPreviousOnOff_name = "SetPreviousOnOff";
                }
            }/*End of Restriction*/
        }/*End of ATT class*/
        #endregion

        #region Cluster Commands

        public class CMD_Off
        {
            public const string Off_name = "Off";
            public static implicit operator string(CMD_Off command)
            {
                return SumCMDStr(OnOffCluster.ClusterName, OnOffCluster.CMD_Off.Off_name);
            }
        }/*End of CMD class*/
        public class CMD_On
        {
            public const string On_name = "On";
            public static implicit operator string(CMD_On command)
            {
                return SumCMDStr(OnOffCluster.ClusterName, OnOffCluster.CMD_On.On_name);
            }
        }/*End of CMD class*/
        public class CMD_Toggle
        {
            public const string Toggle_name = "Toggle";
            public static implicit operator string(CMD_Toggle command)
            {
                return SumCMDStr(OnOffCluster.ClusterName, OnOffCluster.CMD_Toggle.Toggle_name);
            }
        }/*End of CMD class*/
        public class CMD_OffWithEffect
        {
            public const string OffWithEffect_name = "OffWithEffect";
            public static implicit operator string(CMD_OffWithEffect command)
            {
                return SumCMDStr(OnOffCluster.ClusterName, OnOffCluster.CMD_OffWithEffect.OffWithEffect_name);
            }

            public class CMD_OffWithEffect_Payload
            {
                public string EffectIdentifier { get; set; }
                public struct EffectVariant_DelayedAllOff
                {
                    public string DelayedAllOff { get; set; }
                }
                public struct EffectVariant_DyingLight
                {
                    public string DyingLight { get; set; }
                }

                public class EffectVariant
                {
                    public EffectVariant_DelayedAllOff my_DelayedAllOff { get; set; }
                    public EffectVariant_DyingLight my_DyingLight { get; set; }

                }

                /*Start of public Payload*/
                public CMD_OffWithEffect_Payload(string my_EffectIdentifier, EffectVariant my_EffectVariant)
                {
                    EffectIdentifier = my_EffectIdentifier;
                    EffectVariant EffectVariant_payload = my_EffectVariant;
                }/*End of public Payload*/
                public static implicit operator CMD_OffWithEffect_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_OffWithEffect_Payload>(command);
                }
                public static implicit operator string(CMD_OffWithEffect_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_OnWithRecallGlobalScene
        {
            public const string OnWithRecallGlobalScene_name = "OnWithRecallGlobalScene";
            public static implicit operator string(CMD_OnWithRecallGlobalScene command)
            {
                return SumCMDStr(OnOffCluster.ClusterName, OnOffCluster.CMD_OnWithRecallGlobalScene.OnWithRecallGlobalScene_name);
            }
        }/*End of CMD class*/
        public class CMD_OnWithTimedOff
        {
            public const string OnWithTimedOff_name = "OnWithTimedOff";
            public static implicit operator string(CMD_OnWithTimedOff command)
            {
                return SumCMDStr(OnOffCluster.ClusterName, OnOffCluster.CMD_OnWithTimedOff.OnWithTimedOff_name);
            }
            public class CMD_OnWithTimedOff_Payload
            {
                public string OnOffControl { get; set; }
                public string OnTime { get; set; }
                public struct OffWaitTime_AcceptOnlyWhenOn
                {
                    public string AcceptOnlyWhenOn { get; set; }
                }

                public class OffWaitTime
                {
                    public OffWaitTime_AcceptOnlyWhenOn my_AcceptOnlyWhenOn { get; set; }

                }

                /*Start of public Payload*/
                public CMD_OnWithTimedOff_Payload(string my_OnOffControl, string my_OnTime, OffWaitTime my_OffWaitTime)
                {
                    OnOffControl = my_OnOffControl;
                    OnTime = my_OnTime;
                    OffWaitTime OffWaitTime_payload = my_OffWaitTime;
                }/*End of public Payload*/
                public static implicit operator CMD_OnWithTimedOff_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_OnWithTimedOff_Payload>(command);
                }
                public static implicit operator string(CMD_OnWithTimedOff_Payload command)
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

        public static implicit operator OnOffCluster(string options)
        {
            return JsonConvert.DeserializeObject<OnOffCluster>(options);
        }
        public static implicit operator string(OnOffCluster command)
        {
            return OnOffCluster.ClusterName;
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
