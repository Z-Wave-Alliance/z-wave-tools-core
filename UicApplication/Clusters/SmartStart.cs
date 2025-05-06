/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class SmartStart
    {
        public const string MainTopic = "ucl";

        public const string ID = "/SmartStart";

        public class List
        {
            public const string ID = "/List";
            public class SmartStartList
            {
                public string DSK { get; set; }
                public string Unid { get; set; }
                public bool Include { get; set; }
                public string ProtocolControllerUnid { get; set; }
                public string[] PreferredProtocols { get; set; }
            }

            public struct Value
            {
                public List<SmartStartList> SmartStartList { get; set; }
            }

            public struct Payload
            {
                public string DSK { get; set; }
                public string Unid { get; set; }
                public bool Include { get; set; }
                public string ProtocolControllerUnid { get; set; }
                public string[] PreferredProtocols { get; set; }

                public static implicit operator Payload(string data)
                {
                    return JsonConvert.DeserializeObject<Payload>(data);
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

            public struct SmartStartListPayload
            {

                //public List<SmartStartList> SmartStartList { get; set;}                
                public Value value;

                public static implicit operator SmartStartListPayload(string data)
                {
                    Value localList = JsonConvert.DeserializeObject<Value>(data);
                    return new SmartStartListPayload() { value = localList };
                }

                public static implicit operator string(SmartStartListPayload data)
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

            /// <summary>
            ///  Create the cluster network SmartStart/List/ command with the chossen payload opetions
            /// </summary>
            /// <param name="options"></param>
            public static implicit operator List(string options)
            {
                return JsonConvert.DeserializeObject<List>(options);
            }
            public static implicit operator string(List command)
            {
                mqttMessage message = new mqttMessage();

                message.topic = MainTopic + SmartStart.ID + ID;

                return message.topic;
            }

            public class Update
            {
                public Payload payload;
                public string Topic
                {
                    get
                    {
                        return MainTopic + SmartStart.ID + List.ID + Update.ID;
                    }
                    set
                    {
                        Topic = value;
                    }
                }
                public const string ID = "/Update";
                /// <summary>
                ///  Create the cluster network SmartStart/List/Update command with the chossen payload opetions
                /// </summary>
                /// <param name="options"></param>
                public static implicit operator Update(string options)
                {
                    return JsonConvert.DeserializeObject<Update>(options);
                }
                public static implicit operator string(Update command)
                {
                    mqttMessage message = new mqttMessage();
                    if (command.payload == null)
                    {
                        message.topic = MainTopic + SmartStart.ID + List.ID + ID;
                    }
                    else
                    {
                        message.payload = JsonConvert.SerializeObject(command.payload, Formatting.Indented);
                        message.topic = MainTopic + SmartStart.ID + List.ID + ID;
                    }

                    return message.topic;
                }
            }

            public class Remove
            {
                public Payload payload;

                public struct Payload
                {
                    public string DSK { get; set; }

                    public static implicit operator Payload(string data)
                    {
                        return JsonConvert.DeserializeObject<Payload>(data);
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

                public string Topic
                {
                    get
                    {
                        return MainTopic + SmartStart.ID + List.ID + ID;
                    }
                    set
                    {
                        Topic = value;
                    }
                }

                public const string ID = "/Remove";
                /// <summary>
                ///  Create the cluster network SmartStart/List/Update command with the chossen payload opetions
                /// </summary>
                /// <param name="options"></param>
                public static implicit operator Remove(string options)
                {
                    return JsonConvert.DeserializeObject<Remove>(options);
                }
                public static implicit operator string(Remove command)
                {
                    mqttMessage message = new mqttMessage();
                    if (command.payload == null)
                    {
                        message.topic = MainTopic + SmartStart.ID + List.ID + ID;
                    }
                    else
                    {
                        message.payload = JsonConvert.SerializeObject(command.payload, Formatting.Indented);
                        message.topic = MainTopic + SmartStart.ID + List.ID + ID;
                    }

                    return message.topic;
                }
            }
        }
            
    }
}
