/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Newtonsoft.Json;

namespace UicApplication.Clusters
{
	public class OccupancySensingCluster
    {		
	    public const string ClusterName = "OccupancySensing";
        public const string ClusterDesired = "Desired";
        public const string ClusterReported = "Reported";
        public const string ClusterSupportedCommands = "SupportedCommands";
        public const string ClusterAttributes = "Attributes";

        #region Cluster Attributes

        public class ATT_Occupancy
             {
                public const string Occupancy_name = "Occupancy";
                public class Occupancy_Attributes
                {
                    public static implicit operator string(Occupancy_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_Occupancy command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_Occupancy.Occupancy_name);
                }
                public class Occupancy_Desired
                {
                     public struct Value
                     {
                        public bool SensedOccupancy { get; set; }
                     }

                     public Value value;
                    
                     public static implicit operator Occupancy_Desired(string options)
                     {
                        return JsonConvert.DeserializeObject<Occupancy_Desired>(options);
                     }

                     public static implicit operator string(Occupancy_Desired command)
                     {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_Occupancy.Occupancy_name, ClusterDesired);
                     }
                }
                public class Occupancy_Report
                {

                    public struct Value
                    {
                        public bool SensedOccupancy { get; set; }
                    }

                    public Value value;

                public static implicit operator Occupancy_Report(string options)
                {
                    return JsonConvert.DeserializeObject<Occupancy_Report>(options);
                }
                public static implicit operator string(Occupancy_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_Occupancy.Occupancy_name, ClusterReported);
                    }
                }
                public class Occupancy_SupportCommands
                {
                    public static implicit operator string(Occupancy_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                public class Bitmap
                                   {
                                                                            public class SensedOccupancy
                                        {
                                            public const string SensedOccupancy_name = "SensedOccupancy";                                            
                                        }
                                                                           }
                                     }/*End of ATT class*/          
        public class ATT_OccupancySensorType
             {
                public const string OccupancySensorType_name = "OccupancySensorType";
                public class OccupancySensorType_Attributes
                {
                    public static implicit operator string(OccupancySensorType_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_OccupancySensorType command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_OccupancySensorType.OccupancySensorType_name);
                }
                public class OccupancySensorType_Desired
                {
                    public static implicit operator string(OccupancySensorType_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_OccupancySensorType.OccupancySensorType_name, ClusterDesired);
                    }
                }
                public class OccupancySensorType_Report
                {
                    public static implicit operator string(OccupancySensorType_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_OccupancySensorType.OccupancySensorType_name, ClusterReported);
                    }
                }
                public class OccupancySensorType_SupportCommands
                {
                    public static implicit operator string(OccupancySensorType_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                                        public class Restriction
                            {  
                                public class PIR
                                {
                                    public const string PIR_value = "00";
                                    public const string PIR_name = "PIR";
                                }
                                  
                                public class Ultrasonic
                                {
                                    public const string Ultrasonic_value = "01";
                                    public const string Ultrasonic_name = "Ultrasonic";
                                }
                                  
                                public class PIRAndUltrasonic
                                {
                                    public const string PIRAndUltrasonic_value = "02";
                                    public const string PIRAndUltrasonic_name = "PIRAndUltrasonic";
                                }
                                  
                                public class PhysicalContact
                                {
                                    public const string PhysicalContact_value = "03";
                                    public const string PhysicalContact_name = "PhysicalContact";
                                }
                                                        }/*End of Restriction*/                                
                                     }/*End of ATT class*/          
        public class ATT_OccupancySensorTypeBitmap
             {
                public const string OccupancySensorTypeBitmap_name = "OccupancySensorTypeBitmap";
                public class OccupancySensorTypeBitmap_Attributes
                {
                    public static implicit operator string(OccupancySensorTypeBitmap_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_OccupancySensorTypeBitmap command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_OccupancySensorTypeBitmap.OccupancySensorTypeBitmap_name);
                }
                public class OccupancySensorTypeBitmap_Desired
                {
                    public static implicit operator string(OccupancySensorTypeBitmap_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_OccupancySensorTypeBitmap.OccupancySensorTypeBitmap_name, ClusterDesired);
                    }
                }
                public class OccupancySensorTypeBitmap_Report
                {
                    public static implicit operator string(OccupancySensorTypeBitmap_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_OccupancySensorTypeBitmap.OccupancySensorTypeBitmap_name, ClusterReported);
                    }
                }
                public class OccupancySensorTypeBitmap_SupportCommands
                {
                    public static implicit operator string(OccupancySensorTypeBitmap_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                public class Bitmap
                                   {
                                                                            public class PIR
                                        {
                                            public const string PIR_name = "PIR";                                            
                                        }
                                                                                public class Ultrasonic
                                        {
                                            public const string Ultrasonic_name = "Ultrasonic";                                            
                                        }
                                                                                public class PhysicalContact
                                        {
                                            public const string PhysicalContact_name = "PhysicalContact";                                            
                                        }
                                                                           }
                                     }/*End of ATT class*/          
        public class ATT_PIROccupiedToUnoccupiedDelay
             {
                public const string PIROccupiedToUnoccupiedDelay_name = "PIROccupiedToUnoccupiedDelay";
                public class PIROccupiedToUnoccupiedDelay_Attributes
                {
                    public static implicit operator string(PIROccupiedToUnoccupiedDelay_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_PIROccupiedToUnoccupiedDelay command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIROccupiedToUnoccupiedDelay.PIROccupiedToUnoccupiedDelay_name);
                }
                public class PIROccupiedToUnoccupiedDelay_Desired
                {
                    public static implicit operator string(PIROccupiedToUnoccupiedDelay_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIROccupiedToUnoccupiedDelay.PIROccupiedToUnoccupiedDelay_name, ClusterDesired);
                    }
                }
                public class PIROccupiedToUnoccupiedDelay_Report
                {
                    public static implicit operator string(PIROccupiedToUnoccupiedDelay_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIROccupiedToUnoccupiedDelay.PIROccupiedToUnoccupiedDelay_name, ClusterReported);
                    }
                }
                public class PIROccupiedToUnoccupiedDelay_SupportCommands
                {
                    public static implicit operator string(PIROccupiedToUnoccupiedDelay_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_PIRUnoccupiedToOccupiedDelay
             {
                public const string PIRUnoccupiedToOccupiedDelay_name = "PIRUnoccupiedToOccupiedDelay";
                public class PIRUnoccupiedToOccupiedDelay_Attributes
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedDelay_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_PIRUnoccupiedToOccupiedDelay command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIRUnoccupiedToOccupiedDelay.PIRUnoccupiedToOccupiedDelay_name);
                }
                public class PIRUnoccupiedToOccupiedDelay_Desired
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedDelay_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIRUnoccupiedToOccupiedDelay.PIRUnoccupiedToOccupiedDelay_name, ClusterDesired);
                    }
                }
                public class PIRUnoccupiedToOccupiedDelay_Report
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedDelay_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIRUnoccupiedToOccupiedDelay.PIRUnoccupiedToOccupiedDelay_name, ClusterReported);
                    }
                }
                public class PIRUnoccupiedToOccupiedDelay_SupportCommands
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedDelay_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_PIRUnoccupiedToOccupiedThreshold
             {
                public const string PIRUnoccupiedToOccupiedThreshold_name = "PIRUnoccupiedToOccupiedThreshold";
                public class PIRUnoccupiedToOccupiedThreshold_Attributes
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedThreshold_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_PIRUnoccupiedToOccupiedThreshold command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIRUnoccupiedToOccupiedThreshold.PIRUnoccupiedToOccupiedThreshold_name);
                }
                public class PIRUnoccupiedToOccupiedThreshold_Desired
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedThreshold_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIRUnoccupiedToOccupiedThreshold.PIRUnoccupiedToOccupiedThreshold_name, ClusterDesired);
                    }
                }
                public class PIRUnoccupiedToOccupiedThreshold_Report
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedThreshold_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PIRUnoccupiedToOccupiedThreshold.PIRUnoccupiedToOccupiedThreshold_name, ClusterReported);
                    }
                }
                public class PIRUnoccupiedToOccupiedThreshold_SupportCommands
                {
                    public static implicit operator string(PIRUnoccupiedToOccupiedThreshold_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_UltrasonicOccupiedToUnoccupiedDelay
             {
                public const string UltrasonicOccupiedToUnoccupiedDelay_name = "UltrasonicOccupiedToUnoccupiedDelay";
                public class UltrasonicOccupiedToUnoccupiedDelay_Attributes
                {
                    public static implicit operator string(UltrasonicOccupiedToUnoccupiedDelay_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_UltrasonicOccupiedToUnoccupiedDelay command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicOccupiedToUnoccupiedDelay.UltrasonicOccupiedToUnoccupiedDelay_name);
                }
                public class UltrasonicOccupiedToUnoccupiedDelay_Desired
                {
                    public static implicit operator string(UltrasonicOccupiedToUnoccupiedDelay_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicOccupiedToUnoccupiedDelay.UltrasonicOccupiedToUnoccupiedDelay_name, ClusterDesired);
                    }
                }
                public class UltrasonicOccupiedToUnoccupiedDelay_Report
                {
                    public static implicit operator string(UltrasonicOccupiedToUnoccupiedDelay_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicOccupiedToUnoccupiedDelay.UltrasonicOccupiedToUnoccupiedDelay_name, ClusterReported);
                    }
                }
                public class UltrasonicOccupiedToUnoccupiedDelay_SupportCommands
                {
                    public static implicit operator string(UltrasonicOccupiedToUnoccupiedDelay_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_UltrasonicUnoccupiedToOccupiedDelay
             {
                public const string UltrasonicUnoccupiedToOccupiedDelay_name = "UltrasonicUnoccupiedToOccupiedDelay";
                public class UltrasonicUnoccupiedToOccupiedDelay_Attributes
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedDelay_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_UltrasonicUnoccupiedToOccupiedDelay command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicUnoccupiedToOccupiedDelay.UltrasonicUnoccupiedToOccupiedDelay_name);
                }
                public class UltrasonicUnoccupiedToOccupiedDelay_Desired
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedDelay_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicUnoccupiedToOccupiedDelay.UltrasonicUnoccupiedToOccupiedDelay_name, ClusterDesired);
                    }
                }
                public class UltrasonicUnoccupiedToOccupiedDelay_Report
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedDelay_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicUnoccupiedToOccupiedDelay.UltrasonicUnoccupiedToOccupiedDelay_name, ClusterReported);
                    }
                }
                public class UltrasonicUnoccupiedToOccupiedDelay_SupportCommands
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedDelay_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_UltrasonicUnoccupiedToOccupiedThreshold
             {
                public const string UltrasonicUnoccupiedToOccupiedThreshold_name = "UltrasonicUnoccupiedToOccupiedThreshold";
                public class UltrasonicUnoccupiedToOccupiedThreshold_Attributes
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedThreshold_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_UltrasonicUnoccupiedToOccupiedThreshold command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicUnoccupiedToOccupiedThreshold.UltrasonicUnoccupiedToOccupiedThreshold_name);
                }
                public class UltrasonicUnoccupiedToOccupiedThreshold_Desired
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedThreshold_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicUnoccupiedToOccupiedThreshold.UltrasonicUnoccupiedToOccupiedThreshold_name, ClusterDesired);
                    }
                }
                public class UltrasonicUnoccupiedToOccupiedThreshold_Report
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedThreshold_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_UltrasonicUnoccupiedToOccupiedThreshold.UltrasonicUnoccupiedToOccupiedThreshold_name, ClusterReported);
                    }
                }
                public class UltrasonicUnoccupiedToOccupiedThreshold_SupportCommands
                {
                    public static implicit operator string(UltrasonicUnoccupiedToOccupiedThreshold_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_PhysicalContactOccupiedToUnoccupiedDelay
             {
                public const string PhysicalContactOccupiedToUnoccupiedDelay_name = "PhysicalContactOccupiedToUnoccupiedDelay";
                public class PhysicalContactOccupiedToUnoccupiedDelay_Attributes
                {
                    public static implicit operator string(PhysicalContactOccupiedToUnoccupiedDelay_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_PhysicalContactOccupiedToUnoccupiedDelay command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactOccupiedToUnoccupiedDelay.PhysicalContactOccupiedToUnoccupiedDelay_name);
                }
                public class PhysicalContactOccupiedToUnoccupiedDelay_Desired
                {
                    public static implicit operator string(PhysicalContactOccupiedToUnoccupiedDelay_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactOccupiedToUnoccupiedDelay.PhysicalContactOccupiedToUnoccupiedDelay_name, ClusterDesired);
                    }
                }
                public class PhysicalContactOccupiedToUnoccupiedDelay_Report
                {
                    public static implicit operator string(PhysicalContactOccupiedToUnoccupiedDelay_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactOccupiedToUnoccupiedDelay.PhysicalContactOccupiedToUnoccupiedDelay_name, ClusterReported);
                    }
                }
                public class PhysicalContactOccupiedToUnoccupiedDelay_SupportCommands
                {
                    public static implicit operator string(PhysicalContactOccupiedToUnoccupiedDelay_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_PhysicalContactUnoccupiedToOccupiedDelay
             {
                public const string PhysicalContactUnoccupiedToOccupiedDelay_name = "PhysicalContactUnoccupiedToOccupiedDelay";
                public class PhysicalContactUnoccupiedToOccupiedDelay_Attributes
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedDelay_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_PhysicalContactUnoccupiedToOccupiedDelay command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactUnoccupiedToOccupiedDelay.PhysicalContactUnoccupiedToOccupiedDelay_name);
                }
                public class PhysicalContactUnoccupiedToOccupiedDelay_Desired
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedDelay_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactUnoccupiedToOccupiedDelay.PhysicalContactUnoccupiedToOccupiedDelay_name, ClusterDesired);
                    }
                }
                public class PhysicalContactUnoccupiedToOccupiedDelay_Report
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedDelay_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactUnoccupiedToOccupiedDelay.PhysicalContactUnoccupiedToOccupiedDelay_name, ClusterReported);
                    }
                }
                public class PhysicalContactUnoccupiedToOccupiedDelay_SupportCommands
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedDelay_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
        public class ATT_PhysicalContactUnoccupiedToOccupiedThreshold
             {
                public const string PhysicalContactUnoccupiedToOccupiedThreshold_name = "PhysicalContactUnoccupiedToOccupiedThreshold";
                public class PhysicalContactUnoccupiedToOccupiedThreshold_Attributes
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedThreshold_Attributes command)
                    {
                        return SumAllAtt(OccupancySensingCluster.ClusterName,ClusterAttributes);
                    }
                }                
                public static implicit operator string(ATT_PhysicalContactUnoccupiedToOccupiedThreshold command)
                {
                    return SumATTStr(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactUnoccupiedToOccupiedThreshold.PhysicalContactUnoccupiedToOccupiedThreshold_name);
                }
                public class PhysicalContactUnoccupiedToOccupiedThreshold_Desired
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedThreshold_Desired command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactUnoccupiedToOccupiedThreshold.PhysicalContactUnoccupiedToOccupiedThreshold_name, ClusterDesired);
                    }
                }
                public class PhysicalContactUnoccupiedToOccupiedThreshold_Report
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedThreshold_Report command)
                    {                    
                        return SumATTStr_WithSubValue(OccupancySensingCluster.ClusterName, OccupancySensingCluster.ATT_PhysicalContactUnoccupiedToOccupiedThreshold.PhysicalContactUnoccupiedToOccupiedThreshold_name, ClusterReported);
                    }
                }
                public class PhysicalContactUnoccupiedToOccupiedThreshold_SupportCommands
                {
                    public static implicit operator string(PhysicalContactUnoccupiedToOccupiedThreshold_SupportCommands command)
                    {
                        return SumSupCMD(OccupancySensingCluster.ClusterName, ClusterSupportedCommands);
                    }
                }
                             }/*End of ATT class*/          
                #endregion

        #region Cluster Commands
        
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
        public struct Payload_Occupancy_Value
        {
            public class Value
            {
                public bool SensedOccupancy { get; set; }
            }

            public Value _value { get; set; }

            public Payload_Occupancy_Value(Value value)
            {
                _value = value;
            }

            public static implicit operator Payload_Occupancy_Value(string options)
            {
                Value deserialized = JsonConvert.DeserializeObject<Value>(options);
                return new Payload_Occupancy_Value(deserialized);
            }
        }

        public static implicit operator OccupancySensingCluster(string options)
        {
            return JsonConvert.DeserializeObject<OccupancySensingCluster>(options);
        }
        public static implicit operator string(OccupancySensingCluster command)
        {
            return OccupancySensingCluster.ClusterName;
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
            string SumStr = SumATTStr(ClusterName,AttName) + "/" + SubValue;
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
