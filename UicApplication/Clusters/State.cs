/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class State
    {
        public const string ID = "State";

        public struct Payload
        {
            
            public string NetworkStatus { get; set; }
            public string Security { get; set; }
            public string MaximumCommandDelay { get; set; }            

            public static implicit operator Payload(string data)
            {

                if (data != "" && data != null)
                {
                    return JsonConvert.DeserializeObject<Payload>(data);
                }
                else
                {
                    return new Payload();
                }

            }

            public static implicit operator string(Payload data)
            {
                string serializedData = JsonConvert.SerializeObject(data,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                serializedData = serializedData.Replace("\n", String.Empty);
                serializedData = serializedData.Replace("\r", String.Empty);
                return serializedData;
            }
        }
      
        public class CMD_RemoveOffline
        {
            public const string RemoveOffline_name = "RemoveOffline";
            public static implicit operator string(CMD_RemoveOffline command)
            {
                return SumCMDStr(State.ID, State.CMD_RemoveOffline.RemoveOffline_name);
            }
            public class CMD_RemoveOffline_Payload
            {               
                /*Start of public Payload*/
                public CMD_RemoveOffline_Payload()
                {                  

                }
                /*End of public Payload*/
                public static implicit operator CMD_RemoveOffline_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_RemoveOffline_Payload>(command);
                }
                public static implicit operator string(CMD_RemoveOffline_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/            
        }/*End of CMD class with Childnodes*/       

        public class CMD_Remove
        {
            public const string Remove_name = "Remove";
            public static implicit operator string(CMD_Remove command)
            {
                return SumCMDStr(State.ID, State.CMD_Remove.Remove_name);
            }
            public class CMD_Remove_Payload
            {
                /*Start of public Payload*/
                public CMD_Remove_Payload()
                {

                }
                /*End of public Payload*/
                public static implicit operator CMD_Remove_Payload(string command)
                {
                    return JsonConvert.DeserializeObject<CMD_Remove_Payload>(command);
                }
                public static implicit operator string(CMD_Remove_Payload command)
                {
                    return JsonConvert.SerializeObject(command);
                }
            }/*End Payload class*/
        }/*End of CMD class with Childnodes*/

        private static string SumCMDStr(string ClusterName, string CmdName)
        {
            string SumStr = "/" + ClusterName + "/" + "Commands" + "/" + CmdName;
            return SumStr;
        }

    }
}
