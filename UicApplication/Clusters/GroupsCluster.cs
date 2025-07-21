/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Newtonsoft.Json;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class GroupsCluster
    {
        public const string ClusterName = "Groups";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_NameSupport
        {
            public const string NameSupport_name = "NameSupport";
            public class NameSupport_Attributes
            {
                public static implicit operator string(NameSupport_Attributes command)
                {
                    return SumAllAtt(GroupsCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_NameSupport command)
            {
                return SumATTStr(GroupsCluster.ClusterName, GroupsCluster.ATT_NameSupport.NameSupport_name);
            }
            public class NameSupport_Desired
            {
                public static implicit operator string(NameSupport_Desired command)
                {
                    return SumATTStr_WithSubValue(GroupsCluster.ClusterName, GroupsCluster.ATT_NameSupport.NameSupport_name, ClusterDesired);
                }
            }
            public class NameSupport_Report
            {
                public static implicit operator string(NameSupport_Report command)
                {
                    return SumATTStr_WithSubValue(GroupsCluster.ClusterName, GroupsCluster.ATT_NameSupport.NameSupport_name, ClusterReported);
                }
            }
            public class NameSupport_SupportCommands
            {
                public static implicit operator string(NameSupport_SupportCommands command)
                {
                    return SumSupCMD(GroupsCluster.ClusterName, ClusterSupportedCommands);
                }
            }
            public class Bitmap
            {
                public class Supported
                {
                    public const string Supported_name = "Supported";
                }
            }
        }/*End of ATT class*/
        public class ATT_GroupList
        {
            public const string GroupList_name = "GroupList";
            public class GroupList_Attributes
            {
                public static implicit operator string(GroupList_Attributes command)
                {
                    return SumAllAtt(GroupsCluster.ClusterName, ClusterAttributes);
                }
            }
            public static implicit operator string(ATT_GroupList command)
            {
                return SumATTStr(GroupsCluster.ClusterName, GroupsCluster.ATT_GroupList.GroupList_name);
            }
            public class GroupList_Desired
            {
                public static implicit operator string(GroupList_Desired command)
                {
                    return SumATTStr_WithSubValue(GroupsCluster.ClusterName, GroupsCluster.ATT_GroupList.GroupList_name, ClusterDesired);
                }
            }
            public class GroupList_Report
            {
                public static implicit operator string(GroupList_Report command)
                {
                    return SumATTStr_WithSubValue(GroupsCluster.ClusterName, GroupsCluster.ATT_GroupList.GroupList_name, ClusterReported);
                }
            }            
        }/*End of ATT class*/
           
        #endregion

        #region Cluster Commands

        public class CMD_AddGroup
        {
            public const string AddGroup_name = "AddGroup";
            public static implicit operator string(CMD_AddGroup command)
            {
                return SumCMDStr(GroupsCluster.ClusterName, GroupsCluster.CMD_AddGroup.AddGroup_name);
            }
            public class CMD_AddGroup_Payload
            {
                public string GroupId { get; set; }
                public string GroupName { get; set; }
                /*Start of public Payload*/
                public CMD_AddGroup_Payload(string my_GroupId, string my_GroupName)
                {
                    GroupId = my_GroupId;
                    GroupName = my_GroupName;

                }
                /*End of public Payload*/
                public static implicit operator CMD_AddGroup_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_AddGroup_Payload>(command);
                }
                public static implicit operator string(CMD_AddGroup_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
            public const string AddressAssignmentMode_refValue = "AddressAssignmentMode";
            public const string MulticastIPv6Address_refValue = "MulticastIPv6Address";
            public const string GroupPort_refValue = "GroupPort";
        }/*End of CMD class with Childnodes*/
        public class CMD_ViewGroup
        {
            public const string ViewGroup_name = "ViewGroup";
            public static implicit operator string(CMD_ViewGroup command)
            {
                return SumCMDStr(GroupsCluster.ClusterName, GroupsCluster.CMD_ViewGroup.ViewGroup_name);
            }
            public class CMD_ViewGroup_Payload
            {
                public int GroupId { get; set; }
                /*Start of public Payload*/
                public CMD_ViewGroup_Payload(int my_GroupId)
                {
                    GroupId = my_GroupId;

                }
                /*End of public Payload*/
                public static implicit operator CMD_ViewGroup_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_ViewGroup_Payload>(command);
                }
                public static implicit operator string(CMD_ViewGroup_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_GetGroupMembership
        {
            public const string GetGroupMembership_name = "GetGroupMembership";
            public static implicit operator string(CMD_GetGroupMembership command)
            {
                return SumCMDStr(GroupsCluster.ClusterName, GroupsCluster.CMD_GetGroupMembership.GetGroupMembership_name);
            }
            public class CMD_GetGroupMembership_Payload
            {
                public string GroupList { get; set; }
                /*Start of public Payload*/
                public CMD_GetGroupMembership_Payload(string my_GroupList)
                {
                    GroupList = my_GroupList;

                }
                /*End of public Payload*/
                public static implicit operator CMD_GetGroupMembership_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_GetGroupMembership_Payload>(command);
                }
                public static implicit operator string(CMD_GetGroupMembership_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_RemoveGroup
        {
            public const string RemoveGroup_name = "RemoveGroup";
            public static implicit operator string(CMD_RemoveGroup command)
            {
                return SumCMDStr(GroupsCluster.ClusterName, GroupsCluster.CMD_RemoveGroup.RemoveGroup_name);
            }
            public class CMD_RemoveGroup_Payload
            {
                public string GroupId { get; set; }
                /*Start of public Payload*/
                public CMD_RemoveGroup_Payload(string my_GroupId)
                {
                    GroupId = my_GroupId;

                }
                /*End of public Payload*/
                public static implicit operator CMD_RemoveGroup_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_RemoveGroup_Payload>(command);
                }
                public static implicit operator string(CMD_RemoveGroup_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/
        public class CMD_RemoveAllGroups
        {
            public const string RemoveAllGroups_name = "RemoveAllGroups";
            public static implicit operator string(CMD_RemoveAllGroups command)
            {
                return SumCMDStr(GroupsCluster.ClusterName, GroupsCluster.CMD_RemoveAllGroups.RemoveAllGroups_name);
            }
        }/*End of CMD class*/
        public class CMD_AddGroupIfIdentifying
        {
            public const string AddGroupIfIdentifying_name = "AddGroupIfIdentifying";
            public static implicit operator string(CMD_AddGroupIfIdentifying command)
            {
                return SumCMDStr(GroupsCluster.ClusterName, GroupsCluster.CMD_AddGroupIfIdentifying.AddGroupIfIdentifying_name);
            }
            public class CMD_AddGroupIfIdentifying_Payload
            {
                public string GroupId { get; set; }
                public string GroupName { get; set; }
                /*Start of public Payload*/
                public CMD_AddGroupIfIdentifying_Payload(string my_GroupId, string my_GroupName)
                {
                    GroupId = my_GroupId;
                    GroupName = my_GroupName;

                }
                /*End of public Payload*/
                public static implicit operator CMD_AddGroupIfIdentifying_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_AddGroupIfIdentifying_Payload>(command);
                }
                public static implicit operator string(CMD_AddGroupIfIdentifying_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
            public const string AddressAssignmentMode_refValue = "AddressAssignmentMode";
            public const string MulticastIPv6Address_refValue = "MulticastIPv6Address";
            public const string GroupPort_refValue = "GroupPort";
        }/*End of CMD class with Childnodes*/
        #endregion

        #region Cluster Operations

        public class Payload_Att_SupportedCommands
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
        public struct Payload_Att_GroupList
        {
            //public string[] GroupList;

            //public static implicit operator Payload_Att_GroupList(string options)
            //{
            //    var payloadGroupList = new Payload_Att_GroupList();
            //    payloadGroupList.GroupList = JsonConvert.DeserializeObject<string[]>(options);
            //    return payloadGroupList;
            //}

            public class Value
            {
                public List<string> value { get; set; }
            }

            public Value _value { get; set; }

            public Payload_Att_GroupList(Value value)
            {
                _value = value;
            }

            public static implicit operator Payload_Att_GroupList(string options)
            {
                Value deserialized = JsonConvert.DeserializeObject<Value>(options);
                return new Payload_Att_GroupList(deserialized);
            }
        }      

        public static implicit operator GroupsCluster(string options)
        {
            return JsonConvert.DeserializeObject<GroupsCluster>(options);
        }
        public static implicit operator string(GroupsCluster command)
        {
            return GroupsCluster.ClusterName;
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

        public static string SumATTStr(string ClusterName, string AttName)
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
