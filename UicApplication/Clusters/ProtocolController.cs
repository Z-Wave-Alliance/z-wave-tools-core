/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UicApplication.Clusters
{
    public class ProtocolController
    {
        public const string ID = "ProtocolController";

        public class StateParameters
        {
            public StateParameters()
            {
                UserAccept = true;
            }
            public string ProvisioningMode { get; set; }
            public bool UserAccept { get; set; }
            public string SecurityCode { get; set; }
            public string unid { get; set; }

            public static implicit operator StateParameters(string data)
            {
                return JsonConvert.DeserializeObject<StateParameters>(data);
            }

            public static implicit operator string(StateParameters data)
            {
                string serializedData = JsonConvert.SerializeObject(data,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                
                            });
                serializedData = serializedData.Replace("\n", String.Empty);
                serializedData = serializedData.Replace("\r", String.Empty);
                return serializedData;
            }
        }

        public struct Payload
        {
            public string State { get; set; }
            public IList<string> SupportedStateList { get; set; }
            public StateParameters StateParameters { get; set; }

            //public UInt16 Version { get; set; }

            public static implicit operator Payload(string data)
            {
                return JsonConvert.DeserializeObject<Payload>(data);
            }

            public static implicit operator string (Payload data)
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

        public struct TelemetryGeneratedCmd
        {
            public int AckChannel { get; set; }
            public int AckRSSI { get; set; }
            public int DestinationAckMeasuredNoiseFloor { get; set; }
            public int DestinationAckMeasuredRSSI { get; set; }
            public int DestinationAckTxPowerdBm { get; set; }
            public string DestinationUNID { get; set; }
            public int IncomingRSSIRepeaters { get; set; }
            public string LastRouteFailedLinkFunctionalUNID { get; set; }
            public string LastRouteFailedLinkNonFunctionalUNID { get; set; }
            public string LastRouteRepeaters { get; set; }
            public int MeasuredNoiseFloordBm { get; set; }
            public int NumberOfLastRouteRepeaters { get; set; }
            public bool RouteChanged { get; set; }
            public int RoutingAttempts { get; set; }
            public string SourceUNID { get; set; }
            public string TransmissionSpeed { get; set; }
            public bool TransmissionSuccessful { get; set; }
            public int TransmissionTimeMs { get; set; }
            public int TxPowerdBm { get; set; }
        }

        public class NetworkManagement
        {
            public const string ID = "NetworkManagement";

            public string topic
            {
                get
                {
                    if (payload.State == null)
                    {
                        return ProtocolController.ID + "/" + ID + "/";
                    }
                    else
                    {
                        return ProtocolController.ID + "/" + ID + "/Write";
                    }
                }
                set
                {
                    topic = value;
                }
            }

            public Payload payload;

            /// <summary>
            ///  Create the cluster network management command with the chossen payload opetions
            /// </summary>
            /// <param name="options"></param>
            public static implicit operator NetworkManagement( string options )
            {
                return JsonConvert.DeserializeObject<NetworkManagement>(options);                
            }

            /// <summary>
            /// Generates the string that will be used to publish NetworkManagement Commands
            /// </summary>
            /// <param name="command"></param>
            public static implicit operator string (NetworkManagement command)
            {
                mqttMessage message = new mqttMessage();
                if(command.payload.StateParameters == null && command.payload.State == null &&
                    command.payload.SupportedStateList == null)
                {
                    message.topic = "/" + ProtocolController.ID + "/" + NetworkManagement.ID;
                }
                else
                {
                    message.payload = JsonConvert.SerializeObject(command.payload, Formatting.Indented);
                    message.topic = "/" + ProtocolController.ID + "/" + NetworkManagement.ID + "/" +
                        "Write";
                }
                
                return message.topic;
            }

        }

        public class RFTelemetry
        {
            public const string ID = "RFTelemetry";

            #region RFTelemetry Commands
            public class CMD_WriteAttributes
            {
                public const string MoveToLevel_name = "WriteAttributes";
                public static implicit operator string(CMD_WriteAttributes command)
                {
                    return ProtocolController.ID + "/" + ID + "/" + RFTelemetry.CMD_WriteAttributes.MoveToLevel_name;
                }
                public class CMD_WriteAttributes_Payload
                {
                    public bool TxReportEnabled { get; set; }
                    /*End of public Payload*/
                    public static implicit operator CMD_WriteAttributes_Payload(string command)
                    {
                        return JsonConvert.DeserializeObject<CMD_WriteAttributes_Payload>(command);
                    }
                    public static implicit operator string(CMD_WriteAttributes_Payload command)
                    {
                        return JsonConvert.SerializeObject(command);
                    }
                }/*End Payload class*/
            }/*End of CMD class with Childnodes*/
            #endregion

            #region RFTelemetry Attributes
            //There are some, but does not seem necessary at this point.
            #endregion

            #region RFTelemetry GeneratedCommands

            public class GeneratedCommands
            {
                public const string GeneratedCommands_id = "GeneratedCommands";
                public string topic
                {
                    get
                    {
                        return ProtocolController.ID + "/" + ID + "/" +  GeneratedCommands_id;
                    }
                    set
                    {
                        topic = value;
                    }
                }

                public class TxReport
                {
                    public const string TxReport_id = "TxReport";
                    public TelemetryGeneratedCmd payload { get; set; }
                    public string topic
                    {
                        get
                        {
                            return ProtocolController.ID + "/" + ID + "/" + GeneratedCommands_id + "/" + TxReport_id;
                        }
                        set
                        {
                            topic = value;
                        }
                    }

                    public static implicit operator TxReport(string options)
                    {
                        return JsonConvert.DeserializeObject<TxReport>(options);
                    }
                    public static implicit operator string(TxReport command)
                    {

                        mqttMessage message = new mqttMessage();
                        message.payload = JsonConvert.SerializeObject(command.payload, Formatting.Indented);
                        message.topic = "/" + ProtocolController.ID + "/" + ID + "/" + GeneratedCommands_id + "/" + TxReport_id;

                        return message.topic;
                    }
                }
            }

            #endregion            

        }
    }
}
