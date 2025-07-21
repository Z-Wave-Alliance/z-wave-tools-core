/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Utils;
using ZWave.Xml.Application;

namespace ZWave.Xml.HeaderGenerator
{
    public class CHeaderGeneratorEx
    {
        public const string BasicDeviceClassHeaderName = "ZW_BASIC_DEVICE_CLASS";
        public const string GenericDeviceClassHeaderName = "ZW_GENERIC_DEVICE_CLASS";
        public CHeaderGeneratorEx(string selectedFolderName)
        {
            SelectedFolderName = selectedFolderName;
        }
        public string SelectedFolderName { get; set; }

        public void Generate(IList<BasicDevice> bDl, IList<GenericDevice> gDl, IList<CommandClass> cCl)
        {
            string bDlFileName = "";
            string gDlFileName = "";
            List<string> cClFileName = null;
            List<string> cmdStructNames = null;
            if (bDl != null)
            {
                bDlFileName = GenerateBasicDeviceClassIdentifiers(bDl);
            }
            if (gDl != null)
            {
                gDlFileName = GenerateGenericDeviceClassIdentifiers(gDl);
            }
            if (cCl != null)
            {
                var deprecatedList = new List<string>(new[]
                {
                    "COMMAND_CLASS_LOCK"
                });
                var cmdClasses = cCl.Where(x => !deprecatedList.Contains(x.Name)).ToList();
                var cmdClassFileName = GenerateCommandClassesEnum(cmdClasses);
                cClFileName = GenerateCommandClassesIdentifiers(cmdClasses, cmdClassFileName, out cmdStructNames);
            }

            string resultFileName = Path.Combine(SelectedFolderName, "ZW_CMDCLASS_COMMON.h");
            StreamWriter sw = File.CreateText(resultFileName);
            sw.WriteLine(GetTitle("ZW_CMDCLASS_COMMON.h"));
            sw.WriteLine("#ifndef _{0}_H_", "ZW_CMDCLASS_COMMON");
            sw.WriteLine("#define _{0}_H_", "ZW_CMDCLASS_COMMON");
            sw.WriteLine("");
            sw.WriteLine("/********* Z-Wave Command Class Common identifiers ***********/");
            if (!string.IsNullOrEmpty(bDlFileName))
            {
                sw.WriteLine("#include \"{0}\"", Path.GetFileName(bDlFileName));
            }
            if (!string.IsNullOrEmpty(gDlFileName))
            {
                sw.WriteLine("#include \"{0}\"", Path.GetFileName(gDlFileName));
            }
            if (cClFileName != null)
            {
                foreach (string s in cClFileName)
                {
                    sw.WriteLine("#include \"{0}\"", Path.GetFileName(s));
                }
                sw.WriteLine("");
                sw.WriteLine("/************************************************************/");
                sw.WriteLine("/* Common for all command classes                           */");
                sw.WriteLine("/************************************************************/");
                sw.WriteLine("typedef struct _ZW_COMMON_FRAME_");
                sw.WriteLine("{");
                sw.WriteLine("  BYTE        cmdClass;  /* The command class */");
                sw.WriteLine("  BYTE        cmd;       /* The command */");
                sw.WriteLine("} ZW_COMMON_FRAME;");
                sw.WriteLine("");

                sw.WriteLine("/* Unknown command class commands */");
                sw.WriteLine("#define UNKNOWN_VERSION                                                   0x00");


                sw.WriteLine("typedef union _ZW_APPLICATION_TX_BUFFER_");
                sw.WriteLine("{");
                sw.WriteLine(Tools.FormatStr("  {0,-60} {1};", "ZW_COMMON_FRAME", "ZW_Common"));
                foreach (string structName in cmdStructNames)
                {
                    string structNameMix = Tools.UpperUnderscoreToMixedUpperLower(structName, "_");
                    structNameMix = structNameMix.Replace("Zw_", "ZW@").Replace("_", "").Replace("@", "_");
                    sw.WriteLine(Tools.FormatStr("  {0,-60} {1};", structName, structNameMix));
                }
                sw.WriteLine("} ZW_APPLICATION_TX_BUFFER;");
                sw.WriteLine("");
                #region Hardcoded data
                sw.WriteLine("/************* Z-Wave+ Role Type identifiers **************/");
                sw.WriteLine("typedef enum _ZWAVE_PLUS_ROLE_TYPES_");
                sw.WriteLine("{");
                sw.WriteLine("  ROLE_TYPE_CONTROLLER_CENTRAL_STATIC                                             = 0x00,");
                sw.WriteLine("  ROLE_TYPE_CONTROLLER_SUB_STATIC                                                 = 0x01,");
                sw.WriteLine("  ROLE_TYPE_CONTROLLER_PORTABLE                                                   = 0x02,");
                sw.WriteLine("  ROLE_TYPE_CONTROLLER_PORTABLE_REPORTING                                         = 0x03,");
                sw.WriteLine("  ROLE_TYPE_END_NODE_PORTABLE                                                        = 0x04,");
                sw.WriteLine("  ROLE_TYPE_END_NODE_ALWAYS_ON                                                       = 0x05,");
                sw.WriteLine("  ROLE_TYPE_END_NODE_SLEEPING_REPORTING                                              = 0x06,");
                sw.WriteLine("  ROLE_TYPE_END_NODE_SLEEPING_LISTENING                                              = 0x07,");
                sw.WriteLine("} ZWAVE_PLUS_ROLE_TYPES;");
                sw.WriteLine("");
                sw.WriteLine("/************* Z-Wave+ Icon Type identifiers **************/");
                sw.WriteLine("/* The Z-Wave+ Icon Types defined in this section is the  */");
                sw.WriteLine("/* work of the Z-Wave Alliance.                           */");
                sw.WriteLine("/* The most current list of Z-Wave+ Icon Types may be     */");
                sw.WriteLine("/* found at Z-Wave Alliance web site along with           */");
                sw.WriteLine("/* sample icons.                                          */");
                sw.WriteLine("/* New Z-Wave+ Icon Types may be requested from the       */");
                sw.WriteLine("/* Z-Wave Alliance.                                       */");
                sw.WriteLine("/**********************************************************/");
                sw.WriteLine("typedef enum _ZWAVE_PLUS_ICON_TYPES_");
                sw.WriteLine("{");
                sw.WriteLine("  ICON_TYPE_UNASSIGNED                                                = 0x0000,  //MUST NOT be used by any product");
                sw.WriteLine("  ICON_TYPE_GENERIC_CENTRAL_CONTROLLER                                = 0x0100,  //Central Controller Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_DISPLAY_SIMPLE                                    = 0x0200,  //Display Simple Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_DOOR_LOCK_KEYPAD                                  = 0x0300,  //Door Lock Keypad  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_FAN_SWITCH                                        = 0x0400,  //Fan Switch  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_GATEWAY                                           = 0x0500,  //Gateway  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_LIGHT_DIMMER_SWITCH                               = 0x0600,  //Light Dimmer Switch  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_ON_OFF_POWER_SWITCH                               = 0x0700,  //On/Off Power Switch  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_POWER_STRIP                                       = 0x0800,  //Power Strip  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_REMOTE_CONTROL_AV                                 = 0x0900,  //Remove Control AV  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_REMOTE_CONTROL_MULTI_PURPOSE                      = 0x0A00,  //Remote Control Multi Purpose Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_REMOTE_CONTROL_SIMPLE                             = 0x0B00,  //Remote Control Simple Device Type");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_REMOTE_CONTROL_SIMPLE_KEYFOB                     = 0x0B01,  //Remote Control Simple Device Type (Key fob)");
                sw.WriteLine("  ICON_TYPE_GENERIC_SENSOR_NOTIFICATION                               = 0x0C00,  //Sensor Notification Device Type");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_SMOKE_ALARM                  = 0x0C01,  //Sensor Notification Device Type (Notification type Smoke Alarm)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_CO_ALARM                     = 0x0C02,  //Sensor Notification Device Type (Notification type CO Alarm)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_CO2_ALARM                    = 0x0C03,  //Sensor Notification Device Type (Notification type CO2 Alarm)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_HEAT_ALARM                   = 0x0C04,  //Sensor Notification Device Type (Notification type Heat Alarm)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_WATER_ALARM                  = 0x0C05,  //Sensor Notification Device Type (Notification type Water Alarm)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_ACCESS_CONTROL               = 0x0C06,  //Sensor Notification Device Type (Notification type Access Control)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_HOME_SECURITY                = 0x0C07,  //Sensor Notification Device Type (Notification type Home Security)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_POWER_MANAGEMENT             = 0x0C08,  //Sensor Notification Device Type (Notification type Power Management)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_SYSTEM                       = 0x0C09,  //Sensor Notification Device Type (Notification type System)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_EMERGENCY_ALARM              = 0x0C0A,  //Sensor Notification Device Type (Notification type Emergency Alarm)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_CLOCK                        = 0x0C0B,  //Sensor Notification Device Type (Notification type Clock)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_MULTIDEVICE                  = 0x0CFF,  //Sensor Notification Device Type (Bundled Notification functions)");
                sw.WriteLine("  ICON_TYPE_GENERIC_SENSOR_MULTILEVEL                                 = 0x0D00,  //Sensor Multilevel Device Type");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_AIR_TEMPERATURE                = 0x0D01,  //Sensor Multilevel Device Type (Sensor type Air Temperature)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_GENERAL_PURPOSE_VALUE          = 0x0D02,  //Sensor Multilevel Device Type (Sensor type General Purpose Value)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_LUMINANCE                      = 0x0D03,  //Sensor Multilevel Device Type (Sensor type Luminance)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_POWER                          = 0x0D04,  //Sensor Multilevel Device Type (Sensor type Power)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_HUMIDITY                       = 0x0D05,  //Sensor Multilevel Device Type (Sensor type Humidity)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_VELOCITY                       = 0x0D06,  //Sensor Multilevel Device Type (Sensor type Velocity)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_DIRECTION                      = 0x0D07,  //Sensor Multilevel Device Type (Sensor type Direction)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ATMOSPHERIC_PRESSURE           = 0x0D08,  //Sensor Multilevel Device Type (Sensor type Atmospheric Pressure)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_BAROMETRIC_PRESSURE            = 0x0D09,  //Sensor Multilevel Device Type (Sensor type Barometric Pressure)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SOLOR_RADIATION                = 0x0D0A,  //Sensor Multilevel Device Type (Sensor type Solar Radiation)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_DEW_POINT                      = 0x0D0B,  //Sensor Multilevel Device Type (Sensor type Dew Point)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_RAIN_RATE                      = 0x0D0C,  //Sensor Multilevel Device Type (Sensor type Rain Rate)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TIDE_LEVEL                     = 0x0D0D,  //Sensor Multilevel Device Type (Sensor type Tide Level)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_WEIGHT                         = 0x0D0E,  //Sensor Multilevel Device Type (Sensor type Weight)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_VOLTAGE                        = 0x0D0F,  //Sensor Multilevel Device Type (Sensor type Voltage)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_CURRENT                        = 0x0D10,  //Sensor Multilevel Device Type (Sensor type Current)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_CO2_LEVEL                      = 0x0D11,  //Sensor Multilevel Device Type (Sensor type CO2 Level)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_AIR_FLOW                       = 0x0D12,  //Sensor Multilevel Device Type (Sensor type Air Flow)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TANK_CAPACITY                  = 0x0D13,  //Sensor Multilevel Device Type (Sensor type Tank Capacity)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_DISTANCE                       = 0x0D14,  //Sensor Multilevel Device Type (Sensor type Distance)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ANGLE_POSITION                 = 0x0D15,  //Sensor Multilevel Device Type (Sensor type Angle Position)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ROTATION                       = 0x0D16,  //Sensor Multilevel Device Type (Sensor type Rotation)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_WATER_TEMPERATURE              = 0x0D17,  //Sensor Multilevel Device Type (Sensor type Water Temperature)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SOIL_TEMPERATURE               = 0x0D18,  //Sensor Multilevel Device Type (Sensor type Soil Temperature)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SEISMIC_INTENSITY              = 0x0D19,  //Sensor Multilevel Device Type (Sensor type Seismic Intensity)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SEISMIC_MAGNITUDE              = 0x0D1A,  //Sensor Multilevel Device Type (Sensor type Seismic Magnitude)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ULTRAVIOLET                    = 0x0D1B,  //Sensor Multilevel Device Type (Sensor type Ultraviolet)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ELECTRICAL_RESISTIVITY         = 0x0D1C,  //Sensor Multilevel Device Type (Sensor type Electrical Resistivity)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ELECTRICAL_CONDUCTIVITY        = 0x0D1D,  //Sensor Multilevel Device Type (Sensor type Electrical Conductivity)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_LOUDNESS                       = 0x0D1E,  //Sensor Multilevel Device Type (Sensor type Loudness)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_MOISTURE                       = 0x0D1F,  //Sensor Multilevel Device Type (Sensor type Moisture)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_FREQUENCY                      = 0x0D20,  //Sensor Multilevel Device Type (Sensor type Frequency)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TIME                           = 0x0D21,  //Sensor Multilevel Device Type (Sensor type Time )");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TARGET_TEMPERATURE             = 0x0D22,  //Sensor Multilevel Device Type (Sensor type Target Temperature)");
                sw.WriteLine("  ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_MULTIDEVICE                    = 0x0DFF,  //Sensor Multilevel Device Type (Bundled Sensor functions)");
                sw.WriteLine("  ICON_TYPE_GENERIC_SET_TOP_BOX                                       = 0x0E00,  //Set Top Box Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_SIREN                                             = 0x0F00,  //Siren Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_SUB_ENERGY_METER                                  = 0x1000,  //Sub Energy Meter Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_SUB_SYSTEM_CONTROLLER                             = 0x1100,  //Sub System Controller Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_THERMOSTAT_HVAC                                   = 0x1200,  //Thermostat HVAC Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_THERMOSTAT_SETBACK                                = 0x1300,  //Thermostat Setback Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_TV                                                = 0x1400,  //TV Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_VALVE_OPEN_CLOSE                                  = 0x1500,  //Valve Open/Close Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_WALL_CONTROLLER                                   = 0x1600,  //Wall Controller Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_WHOLE_HOME_METER_SIMPLE                           = 0x1700,  //Whole Home Meter Simple Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_WINDOW_COVERING_NO_POSITION_ENDPOINT              = 0x1800,  //Window Covering No Position/Endpoint  Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_WINDOW_COVERING_ENDPOINT_AWARE                    = 0x1900,  //Window Covering Endpoint Aware Device Type");
                sw.WriteLine("  ICON_TYPE_GENERIC_WINDOW_COVERING_POSITION_ENDPOINT_AWARE           = 0x1A00,  //Window Covering Position/Endpoint Aware Device Type");
                sw.WriteLine("} ZWAVE_PLUS_ICON_TYPES;");
                sw.WriteLine("");
                sw.WriteLine("/************* Manufacturer ID identifiers **************/");
                sw.WriteLine("typedef enum _ZWAVE_MANUFACTURER_IDS_");
                sw.WriteLine("{");
                sw.WriteLine("  MFG_ID_NOT_DEFINED_OR_UNDEFINED                           = 0xFFFF,  //Not defined or un-defined");
                sw.WriteLine("  MFG_ID_2B_ELECTRONICS                                     = 0x0028,  //2B Electronics");
                sw.WriteLine("  MFG_ID_2GIG_TECHNOLOGIES_INC                              = 0x009B,  //2gig Technologies Inc.");
                sw.WriteLine("  MFG_ID_3E_TECHNOLOGIES                                    = 0x002A,  //3e Technologies");
                sw.WriteLine("  MFG_ID_A1_COMPONENTS                                      = 0x0022,  //A-1 Components");
                sw.WriteLine("  MFG_ID_ABILIA                                             = 0x0117,  //Abilia");
                sw.WriteLine("  MFG_ID_ACT_ADVANCED_CONTROL_TECHNOLOGIES                  = 0x0001,  //ACT - Advanced Control Technologies");
                sw.WriteLine("  MFG_ID_AEON_LABS                                          = 0x0086,  //AEON Labs");
                sw.WriteLine("  MFG_ID_AIRLINE_MECHANICAL_CO_LTD                          = 0x0111,  //Airline Mechanical Co., Ltd.");
                sw.WriteLine("  MFG_ID_ALARMCOM                                           = 0x0094,  //Alarm.com");
                sw.WriteLine("  MFG_ID_ASIA_HEADING                                       = 0x0029,  //Asia Heading");
                sw.WriteLine("  MFG_ID_ATECH                                              = 0x002B,  //Atech");
                sw.WriteLine("  MFG_ID_BALBOA_INSTRUMENTS                                 = 0x0018,  //Balboa Instruments");
                sw.WriteLine("  MFG_ID_BENEXT                                             = 0x008A,  //BeNext");
                sw.WriteLine("  MFG_ID_BESAFER                                            = 0x002C,  //BeSafer");
                sw.WriteLine("  MFG_ID_BFT_SPA                                            = 0x014B,  //BFT S.p.A.");
                sw.WriteLine("  MFG_ID_BOCA_DEVICES                                       = 0x0023,  //Boca Devices");
                sw.WriteLine("  MFG_ID_BROADBAND_ENERGY_NETWORKS_INC                      = 0x002D,  //Broadband Energy Networks Inc.");
                sw.WriteLine("  MFG_ID_BULOGICS                                           = 0x0026,  //BuLogics");
                sw.WriteLine("  MFG_ID_CAMEO_COMMUNICATIONS_INC                           = 0x009C,  //Cameo Communications Inc.");
                sw.WriteLine("  MFG_ID_CARRIER                                            = 0x002E,  //Carrier");
                sw.WriteLine("  MFG_ID_CASAWORKS                                          = 0x000B,  //CasaWorks");
                sw.WriteLine("  MFG_ID_CHECKIT_SOLUTIONS_INC                              = 0x014E,  //Check-It Solutions Inc.");
                sw.WriteLine("  MFG_ID_CHROMAGIC_TECHNOLOGIES_CORPORATION                 = 0x0116,  //Chromagic Technologies Corporation");
                sw.WriteLine("  MFG_ID_COLOR_KINETICS_INCORPORATED                        = 0x002F,  //Color Kinetics Incorporated");
                sw.WriteLine("  MFG_ID_COMPUTIME                                          = 0x0140,  //Computime");
                sw.WriteLine("  MFG_ID_CONNECTED_OBJECT                                   = 0x011B,  //Connected Object");
                sw.WriteLine("  MFG_ID_CONTROLTHINK_LC                                    = 0x0019,  //ControlThink LC");
                sw.WriteLine("  MFG_ID_CONVERGEX_LTD                                      = 0x000F,  //ConvergeX Ltd.");
                sw.WriteLine("  MFG_ID_COOPER_LIGHTING                                    = 0x0079,  //Cooper Lighting");
                sw.WriteLine("  MFG_ID_COOPER_WIRING_DEVICES                              = 0x001A,  //Cooper Wiring Devices");
                sw.WriteLine("  MFG_ID_CORNUCOPIA_CORP                                    = 0x012D,  //Cornucopia Corp");
                sw.WriteLine("  MFG_ID_COVENTIVE_TECHNOLOGIES_INC                         = 0x009D,  //Coventive Technologies Inc.");
                sw.WriteLine("  MFG_ID_CYBERHOUSE                                         = 0x0014,  //Cyberhouse");
                sw.WriteLine("  MFG_ID_CYBERTAN_TECHNOLOGY_INC                            = 0x0067,  //CyberTAN Technology, Inc.");
                sw.WriteLine("  MFG_ID_CYTECH_TECHNOLOGY_PRE_LTD                          = 0x0030,  //Cytech Technology Pre Ltd.");
                sw.WriteLine("  MFG_ID_DANFOSS                                            = 0x0002,  //Danfoss");
                sw.WriteLine("  MFG_ID_DEFACONTROLS_BV                                    = 0x013F,  //Defacontrols BV");
                sw.WriteLine("  MFG_ID_DESTINY_NETWORKS                                   = 0x0031,  //Destiny Networks");
                sw.WriteLine("  MFG_ID_DIEHL_AKO                                          = 0x0103,  //Diehl AKO");
                sw.WriteLine("  MFG_ID_DIGITAL_5_INC                                      = 0x0032,  //Digital 5, Inc.");
                sw.WriteLine("  MFG_ID_DYNAQUIP_CONTROLS                                  = 0x0132,  //DynaQuip Controls");
                sw.WriteLine("  MFG_ID_ECOLINK                                            = 0x014A,  //Ecolink");
                sw.WriteLine("  MFG_ID_EKA_SYSTEMS                                        = 0x0087,  //Eka Systems");
                sw.WriteLine("  MFG_ID_ELECTRONIC_SOLUTIONS                               = 0x0033,  //Electronic Solutions");
                sw.WriteLine("  MFG_ID_ELGEV_ELECTRONICS_LTD                              = 0x0034,  //El-Gev Electronics LTD");
                sw.WriteLine("  MFG_ID_ELK_PRODUCTS_INC                                   = 0x001B,  //ELK Products, Inc.");
                sw.WriteLine("  MFG_ID_EMBEDIT_AS                                         = 0x0035,  //Embedit A/S");
                sw.WriteLine("  MFG_ID_ENBLINK_CO_LTD                                     = 0x014D,  //Enblink Co. Ltd");
                sw.WriteLine("  MFG_ID_EUROTRONICS                                        = 0x0148,  //Eurotronics");
                sw.WriteLine("  MFG_ID_EVERSPRING                                         = 0x0060,  //Everspring");
                sw.WriteLine("  MFG_ID_EVOLVE                                             = 0x0113,  //Evolve");
                sw.WriteLine("  MFG_ID_EXCEPTIONAL_INNOVATIONS                            = 0x0036,  //Exceptional Innovations");
                sw.WriteLine("  MFG_ID_EXHAUSTO                                           = 0x0004,  //Exhausto");
                sw.WriteLine("  MFG_ID_EXIGENT_SENSORS                                    = 0x009F,  //Exigent Sensors");
                sw.WriteLine("  MFG_ID_EXPRESS_CONTROLS                                   = 0x001E,  //Express Controls (former Ryherd Ventures)");
                sw.WriteLine("  MFG_ID_FAKRO                                              = 0x0085,  //Fakro");
                sw.WriteLine("  MFG_ID_FIBARGROUP                                         = 0x010F,  //Fibargroup");
                sw.WriteLine("  MFG_ID_FOARD_SYSTEMS                                      = 0x0037,  //Foard Systems");
                sw.WriteLine("  MFG_ID_FOLLOWGOOD_TECHNOLOGY_COMPANY_LTD                  = 0x0137,  //FollowGood Technology Company Ltd.");
                sw.WriteLine("  MFG_ID_FORTREZZ_LLC                                       = 0x0084,  //FortrezZ LLC");
                sw.WriteLine("  MFG_ID_FOXCONN                                            = 0x011D,  //Foxconn");
                sw.WriteLine("  MFG_ID_FROSTDALE                                          = 0x0110,  //Frostdale");
                sw.WriteLine("  MFG_ID_GOOD_WAY_TECHNOLOGY_CO_LTD                         = 0x0068,  //Good Way Technology Co., Ltd");
                sw.WriteLine("  MFG_ID_GREENWAVE_REALITY_INC                              = 0x0099,  //GreenWave Reality Inc.");
                sw.WriteLine("  MFG_ID_HITECH_AUTOMATION                                  = 0x0017,  //HiTech Automation");
                sw.WriteLine("  MFG_ID_HOLTEC_ELECTRONICS_BV                              = 0x013E,  //Holtec Electronics BV");
                sw.WriteLine("  MFG_ID_HOME_AUTOMATED_INC                                 = 0x005B,  //Home Automated Inc.");
                sw.WriteLine("  MFG_ID_HOME_AUTOMATED_LIVING                              = 0x000D,  //Home Automated Living");
                sw.WriteLine("  MFG_ID_HOME_AUTOMATION_EUROPE                             = 0x009A,  //Home Automation Europe");
                sw.WriteLine("  MFG_ID_HOME_DIRECTOR                                      = 0x0038,  //Home Director");
                sw.WriteLine("  MFG_ID_HOMEMANAGEABLES_INC                                = 0x0070,  //Homemanageables, Inc.");
                sw.WriteLine("  MFG_ID_HOMEPRO                                            = 0x0050,  //Homepro");
                sw.WriteLine("  MFG_ID_HOMESCENARIO                                       = 0x0162,  //HomeScenario");
                sw.WriteLine("  MFG_ID_HOMESEER_TECHNOLOGIES                              = 0x000C,  //HomeSeer Technologies");
                sw.WriteLine("  MFG_ID_HONEYWELL                                          = 0x0039,  //Honeywell");
                sw.WriteLine("  MFG_ID_HORSTMANN_CONTROLS_LIMITED                         = 0x0059,  //Horstmann Controls Limited");
                sw.WriteLine("  MFG_ID_ICOM_TECHNOLOGY_BV                                 = 0x0011,  //iCOM Technology b.v.");
                sw.WriteLine("  MFG_ID_INGERSOLL_RAND_SCHLAGE                             = 0x006C,  //Ingersoll Rand (Schlage)");
                sw.WriteLine("  MFG_ID_INGERSOLL_RAND_ECOLINK                             = 0x011F,  //Ingersoll Rand (Former Ecolink)");
                sw.WriteLine("  MFG_ID_INLON_SRL                                          = 0x003A,  //Inlon Srl");
                sw.WriteLine("  MFG_ID_INNOBAND_TECHNOLOGIES_INC                          = 0x0141,  //Innoband Technologies, Inc");
                sw.WriteLine("  MFG_ID_INNOVUS                                            = 0x0077,  //INNOVUS");
                sw.WriteLine("  MFG_ID_INTEL                                              = 0x0006,  //Intel");
                sw.WriteLine("  MFG_ID_INTELLICON                                         = 0x001C,  //IntelliCon");
                sw.WriteLine("  MFG_ID_INTERMATIC                                         = 0x0005,  //Intermatic");
                sw.WriteLine("  MFG_ID_INTERNET_DOM                                       = 0x0013,  //Internet Dom");
                sw.WriteLine("  MFG_ID_IR_SEC_SAFETY                                      = 0x003B,  //IR Sec. & Safety");
                sw.WriteLine("  MFG_ID_IWATSU                                             = 0x0123,  //IWATSU");
                sw.WriteLine("  MFG_ID_JASCO_PRODUCTS                                     = 0x0063,  //Jasco Products");
                sw.WriteLine("  MFG_ID_KAMSTRUP_AS                                        = 0x0091,  //Kamstrup A/S");
                sw.WriteLine("  MFG_ID_LAGOTEK_CORPORATION                                = 0x0051,  //Lagotek Corporation");
                sw.WriteLine("  MFG_ID_LEVITON                                            = 0x001D,  //Leviton");
                sw.WriteLine("  MFG_ID_LIFESTYLE_NETWORKS                                 = 0x003C,  //Lifestyle Networks");
                sw.WriteLine("  MFG_ID_LINEAR_CORP                                        = 0x014F,  //Linear Corp");
                sw.WriteLine("  MFG_ID_LIVING_STYLE_ENTERPRISES_LTD                       = 0x013A,  //Living Style Enterprises, Ltd.");
                sw.WriteLine("  MFG_ID_LOGITECH                                           = 0x007F,  //Logitech");
                sw.WriteLine("  MFG_ID_LOUDWATER_TECHNOLOGIES_LLC                         = 0x0025,  //Loudwater Technologies, LLC");
                sw.WriteLine("  MFG_ID_LS_CONTROL                                         = 0x0071,  //LS Control");
                sw.WriteLine("  MFG_ID_MARMITEK_BV                                        = 0x003D,  //Marmitek BV");
                sw.WriteLine("  MFG_ID_MARTEC_ACCESS_PRODUCTS                             = 0x003E,  //Martec Access Products");
                sw.WriteLine("  MFG_ID_MB_TURN_KEY_DESIGN                                 = 0x008F,  //MB Turn Key Design");
                sw.WriteLine("  MFG_ID_MERTEN                                             = 0x007A,  //Merten");
                sw.WriteLine("  MFG_ID_MITSUMI                                            = 0x0112,  //MITSUMI");
                sw.WriteLine("  MFG_ID_MONSTER_CABLE                                      = 0x007E,  //Monster Cable");
                sw.WriteLine("  MFG_ID_MOTOROLA                                           = 0x003F,  //Motorola");
                sw.WriteLine("  MFG_ID_MTC_MAINTRONIC_GERMANY                             = 0x0083,  //MTC Maintronic Germany");
                sw.WriteLine("  MFG_ID_NAPCO_SECURITY_TECHNOLOGIES_INC                    = 0x0121,  //Napco Security Technologies, Inc.");
                sw.WriteLine("  MFG_ID_NORTHQ                                             = 0x0096,  //NorthQ");
                sw.WriteLine("  MFG_ID_NOVAR_ELECTRICAL_DEVICES_AND_SYSTEMS_EDS           = 0x0040,  //Novar Electrical Devices and Systems (EDS)");
                sw.WriteLine("  MFG_ID_OMNIMA_LIMITED                                     = 0x0119,  //Omnima Limited");
                sw.WriteLine("  MFG_ID_ONSITE_PRO                                         = 0x014C,  //OnSite Pro");
                sw.WriteLine("  MFG_ID_OPENPEAK_INC                                       = 0x0041,  //OpenPeak Inc.");
                sw.WriteLine("  MFG_ID_PHILIO_TECHNOLOGY_CORP                             = 0x013C,  //Philio Technology Corp");
                sw.WriteLine("  MFG_ID_POLYCONTROL                                        = 0x010E,  //Poly-control");
                sw.WriteLine("  MFG_ID_POWERLYNX                                          = 0x0016,  //PowerLynx");
                sw.WriteLine("  MFG_ID_PRAGMATIC_CONSULTING_INC                           = 0x0042,  //Pragmatic Consulting Inc.");
                sw.WriteLine("  MFG_ID_PULSE_TECHNOLOGIES_ASPALIS                         = 0x005D,  //Pulse Technologies (Aspalis)");
                sw.WriteLine("  MFG_ID_QEES                                               = 0x0095,  //Qees");
                sw.WriteLine("  MFG_ID_QUBY                                               = 0x0130,  //Quby");
                sw.WriteLine("  MFG_ID_RADIO_THERMOSTAT_COMPANY_OF_AMERICA_RTC            = 0x0098,  //Radio Thermostat Company of America (RTC)");
                sw.WriteLine("  MFG_ID_RARITAN                                            = 0x008E,  //Raritan");
                sw.WriteLine("  MFG_ID_REITZGROUPDE                                       = 0x0064,  //Reitz-Group.de");
                sw.WriteLine("  MFG_ID_REMOTEC_TECHNOLOGY_LTD                             = 0x5254,  //Remotec Technology Ltd");
                sw.WriteLine("  MFG_ID_RESIDENTIAL_CONTROL_SYSTEMS_INC_RCS                = 0x0010,  //Residential Control Systems, Inc. (RCS)");
                sw.WriteLine("  MFG_ID_RIMPORT_LTD                                        = 0x0147,  //R-import Ltd.");
                sw.WriteLine("  MFG_ID_RS_SCENE_AUTOMATION                                = 0x0065,  //RS Scene Automation");
                sw.WriteLine("  MFG_ID_SAECO                                              = 0x0139,  //Saeco");
                sw.WriteLine("  MFG_ID_SAN_SHIH_ELECTRICAL_ENTERPRISE_CO_LTD              = 0x0093,  //San Shih Electrical Enterprise Co., Ltd.");
                sw.WriteLine("  MFG_ID_SANAV                                              = 0x012C,  //SANAV");
                sw.WriteLine("  MFG_ID_SCIENTIA_TECHNOLOGIES_INC                          = 0x001F,  //Scientia Technologies, Inc.");
                sw.WriteLine("  MFG_ID_SECURE_WIRELESS                                    = 0x011E,  //Secure Wireless");
                sw.WriteLine("  MFG_ID_SELUXIT                                            = 0x0069,  //Seluxit");
                sw.WriteLine("  MFG_ID_SENMATIC_AS                                        = 0x0043,  //Senmatic A/S");
                sw.WriteLine("  MFG_ID_SEQUOIA_TECHNOLOGY_LTD                             = 0x0044,  //Sequoia Technology LTD");
                sw.WriteLine("  MFG_ID_SIGMA_DESIGNS                                      = 0x0000,  //Sigma Designs");
                sw.WriteLine("  MFG_ID_SINE_WIRELESS                                      = 0x0045,  //Sine Wireless");
                sw.WriteLine("  MFG_ID_SMART_PRODUCTS_INC                                 = 0x0046,  //Smart Products, Inc.");
                sw.WriteLine("  MFG_ID_SMK_MANUFACTURING_INC                              = 0x0102,  //SMK Manufacturing Inc.");
                sw.WriteLine("  MFG_ID_SOMFY                                              = 0x0047,  //Somfy");
                sw.WriteLine("  MFG_ID_SYLVANIA                                           = 0x0009,  //Sylvania");
                sw.WriteLine("  MFG_ID_SYSTECH_CORPORATION                                = 0x0136,  //Systech Corporation");
                sw.WriteLine("  MFG_ID_TEAM_PRECISION_PCL                                 = 0x0089,  //Team Precision PCL");
                sw.WriteLine("  MFG_ID_TECHNIKU                                           = 0x000A,  //Techniku");
                sw.WriteLine("  MFG_ID_TELL_IT_ONLINE                                     = 0x0012,  //Tell It Online");
                sw.WriteLine("  MFG_ID_TELSEY                                             = 0x0048,  //Telsey");
                sw.WriteLine("  MFG_ID_THERE_CORPORATION                                  = 0x010C,  //There Corporation");
                sw.WriteLine("  MFG_ID_TKB_HOME                                           = 0x0118,  //TKB Home");
                sw.WriteLine("  MFG_ID_TKH_GROUP_EMINENT                                  = 0x011C,  //TKH Group / Eminent");
                sw.WriteLine("  MFG_ID_TRANE_CORPORATION                                  = 0x008B,  //Trane Corporation");
                sw.WriteLine("  MFG_ID_TRICKLESTAR                                        = 0x0066,  //TrickleStar");
                sw.WriteLine("  MFG_ID_TRICKLESTAR_LTD_EMPOWER_CONTROLS_LTD               = 0x006B,  //Tricklestar Ltd. (former Empower Controls Ltd.)");
                sw.WriteLine("  MFG_ID_TRIDIUM                                            = 0x0055,  //Tridium");
                sw.WriteLine("  MFG_ID_TWISTHINK                                          = 0x0049,  //Twisthink");
                sw.WriteLine("  MFG_ID_UNIVERSAL_ELECTRONICS_INC                          = 0x0020,  //Universal Electronics Inc.");
                sw.WriteLine("  MFG_ID_VDA                                                = 0x010A,  //VDA");
                sw.WriteLine("  MFG_ID_VERO_DUCO                                          = 0x0080,  //Vero Duco");
                sw.WriteLine("  MFG_ID_VIEWSONIC_CORPORATION                              = 0x005E,  //ViewSonic Corporation");
                sw.WriteLine("  MFG_ID_VIMAR_CRS                                          = 0x0007,  //Vimar CRS");
                sw.WriteLine("  MFG_ID_VISION_SECURITY                                    = 0x0109,  //Vision Security");
                sw.WriteLine("  MFG_ID_VISUALIZE                                          = 0x004A,  //Visualize");
                sw.WriteLine("  MFG_ID_WATT_STOPPER                                       = 0x004B,  //Watt Stopper");
                sw.WriteLine("  MFG_ID_WAYNE_DALTON                                       = 0x0008,  //Wayne Dalton");
                sw.WriteLine("  MFG_ID_WENZHOU_MTLC_ELECTRIC_APPLIANCES_COLTD             = 0x011A,  //Wenzhou MTLC Electric Appliances Co.,Ltd.");
                sw.WriteLine("  MFG_ID_WIDOM                                              = 0x0149,  //wiDom");
                sw.WriteLine("  MFG_ID_WILSHINE_HOLDING_CO_LTD                            = 0x012D,  //Wilshine Holding Co., Ltd");
                sw.WriteLine("  MFG_ID_WINTOP                                             = 0x0097,  //Wintop");
                sw.WriteLine("  MFG_ID_WOODWARD_LABS                                      = 0x004C,  //Woodward Labs");
                sw.WriteLine("  MFG_ID_WRAP                                               = 0x0003,  //Wrap");
                sw.WriteLine("  MFG_ID_WUHAN_NWD_TECHNOLOGY_CO_LTD                        = 0x012E,  //Wuhan NWD Technology Co., Ltd.");
                sw.WriteLine("  MFG_ID_XANBOO                                             = 0x004D,  //Xanboo");
                sw.WriteLine("  MFG_ID_ZDATA_LLC                                          = 0x004E,  //Zdata, LLC.");
                sw.WriteLine("  MFG_ID_ZIPATO                                             = 0x0131,  //Zipato");
                sw.WriteLine("  MFG_ID_ZONOFF                                             = 0x0120,  //Zonoff");
                sw.WriteLine("  MFG_ID_ZWAVE_TECHNOLOGIA                                  = 0x004F,  //Z-Wave Technologia");
                sw.WriteLine("  MFG_ID_ZWAVEME                                            = 0x0115,  //Z-Wave.Me");
                sw.WriteLine("  MFG_ID_ZYKRONIX                                           = 0x0021,  //Zykronix");
                sw.WriteLine("  MFG_ID_ZYXEL                                              = 0x0135,  //ZyXEL");
                sw.WriteLine("} ZWAVE_MANUFACTURER_IDS;");
                #endregion
            }
            sw.WriteLine("");
            sw.WriteLine("#endif");
            sw.Close();
        }

        private string GenerateCommandClassesEnum(IEnumerable<CommandClass> cCl)
        {
            string resultFileName = Path.Combine(SelectedFolderName, "ZW_COMMAND_CLASSES.h");
            StreamWriter sw = File.CreateText(resultFileName);
            sw.WriteLine(GetTitle("ZW_COMMAND_CLASSES.h"));
            sw.WriteLine("#ifndef _{0}_H_", "ZW_COMMAND_CLASSES");
            sw.WriteLine("#define _{0}_H_", "ZW_COMMAND_CLASSES");
            sw.WriteLine("");

            sw.WriteLine("/********* Z-Wave Command Classes ************************/");

            sw.WriteLine("typedef enum _COMMAND_CLASSES_");
            sw.WriteLine("{");
            foreach (CommandClass cc in cCl)
            {
                string versionPostfix = "";
                if (cc.Version > 1) versionPostfix = "_V" + cc.Version;
                string cmdClassName = cc.Name + versionPostfix;
                sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/", cmdClassName, cc.KeyId.ToString("X2"), string.IsNullOrEmpty(cc.Comment) ? cc.Text : cc.Comment));
            }
            sw.WriteLine("} COMMAND_CLASSES;");
            sw.WriteLine("");

            sw.WriteLine("");
            sw.WriteLine("#endif");
            sw.Close();

            return resultFileName;
        }

        private List<string> GenerateCommandClassesIdentifiers(IEnumerable<CommandClass> cCl, string includeFile, out List<string> cmdStructNames)
        {
            cmdStructNames = new List<string>();
            List<string> resultFileNames = new List<string>();
            foreach (CommandClass cc in cCl)
            {
                if (cc.Version > 1)
                {
                }
                resultFileNames.Add(GenerateCommandClassIdentifiers(cc, includeFile, out List<string> names));
                if (names != null)
                {
                    cmdStructNames.AddRange(names);
                }

            }
            return resultFileNames;
        }

        private const string StructFieldFormat = "{0}{1,-45}{2,-20}{3}";
        private const string Tab1 = "  ";

        private string GenerateCommandClassIdentifiers(CommandClass cc, string includeFile, out List<string> structNames)
        {
            structNames = new List<string>();
            string versionPostfix = "";
            if (cc.Version > 1) versionPostfix = "_V" + cc.Version;
            string cmdClassName = cc.Name + versionPostfix;
            string resultFileName = Path.Combine(SelectedFolderName, cmdClassName + ".h");

            StreamWriter sw = File.CreateText(resultFileName);
            sw.WriteLine(GetTitle(cmdClassName + ".h"));
            sw.WriteLine("#ifndef _{0}_H_", cmdClassName);
            sw.WriteLine("#define _{0}_H_", cmdClassName);
            sw.WriteLine("");
            sw.WriteLine("#include \"{0}\"", Path.GetFileName(includeFile));
            sw.WriteLine("");
            sw.WriteLine(@"/*  {0}/", (cmdClassName + "  ").PadRight(75, '*'));
            //sw.WriteLine("typedef enum _COMMAND_CLASSES_");
            //sw.WriteLine("{");
            //sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/", cmdClassName, cc.KeyId.ToString("X2"), (String.IsNullOrEmpty(cc.Comment)) ? cc.Text : cc.Comment));
            //sw.WriteLine("} COMMAND_CLASSES;");
            //sw.WriteLine("");

            if (cc.DefineSet != null)
            {
                foreach (DefineSet ds in cc.DefineSet)
                {
                    GenerateDefines(ds, sw, versionPostfix);
                }
            }
            if (cc.Command != null)
            {
                sw.WriteLine("/* {0} commands*/", string.IsNullOrEmpty(cc.Comment) ? cc.Text : cc.Comment);
                sw.WriteLine("typedef enum _{0}_COMMANDS_", Tools.CutString(cmdClassName, "COMMAND_CLASS_"));
                sw.WriteLine("{");
                sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/", cmdClassName + "_VERSION", cc.Version.ToString("X2"), ""));
                foreach (Command c in cc.Command)
                {
                    string cmdName = c.Name + versionPostfix;
                    sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/", cmdName, c.KeyId.ToString("X2"), string.IsNullOrEmpty(c.Comment) ? c.Text : c.Comment));
                }
                sw.WriteLine("} " + Tools.CutString(cmdClassName, "COMMAND_CLASS_") + "_COMMANDS;");
                sw.WriteLine("");
                sw.WriteLine("");

                foreach (Command cmd in cc.Command)
                {
                    string cmdName = cmd.Name + versionPostfix;

                    if (cmd.Param != null)
                    {
                        foreach (Param param in cmd.Param)
                        {
                            if (param.Param1 != null && param.Param1.Count > 0 && (param.Size > 1 || param.SizeReference != null))
                            {
                                GenerateVariantGroup(sw, param);
                            }
                            else if (param.Mode == ParamModes.Property)
                            {
                                GenerateBitFieldStruct(sw, param, "");
                            }
                        }
                    }
                    sw.WriteLine("/************************************************************/");
                    sw.WriteLine("/* {0} command structs */", string.IsNullOrEmpty(cmd.Comment) ? cmd.Text : cmd.Comment);
                    sw.WriteLine("/************************************************************/");
                    sw.WriteLine("typedef struct _ZW_{0}_FRAME", cmdName);
                    sw.WriteLine("{");
                    #region foreach params
                    sw.WriteLine(StructFieldFormat, Tab1, "COMMAND_CLASSES", " cmdClass;", "/* The command class */");
                    sw.WriteLine(StructFieldFormat, Tab1, string.Format("{0}_COMMANDS", Tools.CutString(cmdClassName, "COMMAND_CLASS_")), " cmd;", "/* The command */");

                    //IList<Param> parameters = c != null ? c.Param : vgParam.Param1;
                    if (cmd.Param != null)
                    {
                        foreach (Param param in cmd.Param)
                        {
                            GenerateParamString(cmd, param, sw, versionPostfix);
                        }
                    }

                    #endregion
                    string structName = "ZW_" + cmdName + "_FRAME";
                    structNames.Add(structName);
                    sw.WriteLine("} " + structName + ";");
                    sw.WriteLine("");
                }
            }
            sw.WriteLine("#endif");
            sw.Close();
            return resultFileName;
        }

        private static void GenerateBitFieldStruct(TextWriter sw, Param param, string prefix)
        {
            if (param.ParentCmd.Parent.Version > 1)
            {
            }

            sw.WriteLine("/************************************************************/");
            var line = string.Format("/* {0} property bitfield structs */", prefix +
                                                                               Tools.CutUpperUnderscoreToMixedUpperLower(param.ParentCmd.Name + "_V" + param.ParentCmd.Parent.Version, "COMMAND_CLASS_", " "));
            sw.WriteLine(line);
            sw.WriteLine("/************************************************************/");
            line = string.Format("typedef struct _{0}_{1}_", prefix + Tools.CutString(param.ParentCmd.Name + "_V" + param.ParentCmd.Parent.Version, "COMMAND_CLASS_"), param.Name.ToUpper());
            sw.WriteLine(line);
            sw.WriteLine("{");
            #region foreach params
            const string pFormat = "{0}{1,-20}{2,-20} : {3}{4}";
            if (param.Param1 != null)
            {
                foreach (Param p in param.Param1)
                {
                    sw.WriteLine(pFormat, Tab1, "BYTE", " " + Tools.MakeLegalMixCaseIdentifier(p.Text), p.Bits + ";", p.Comment);
                }
            }

            #endregion
            sw.Write("}");
            sw.WriteLine(" {0}_{1};", prefix + Tools.CutString(param.ParentCmd.Name + "_V" + param.ParentCmd.Parent.Version, "COMMAND_CLASS_"), param.Name.ToUpper());
            sw.WriteLine("");
        }

        private static void GenerateParamString(Command cmd, Param param, TextWriter sw, string versionPostfix)
        {
            if (param.SkipField)
                return;
            string paramText = param.Text;
            if (string.Equals(paramText, "data", StringComparison.CurrentCultureIgnoreCase))
            {
                paramText = param.Text + "Buffer";
            }
            if (cmd != null && cmd.Bits > 0 && cmd.Bits < 8 && cmd.Param.IndexOf(param) == 0 && param.Param1 != null && param.Param1.Count > 0)
            {
            }
            else if (param.Param1 != null && param.Param1.Count > 0 && (param.Size > 1 || param.SizeReference != null))
            {
                //Reference to fariant group
                sw.WriteLine(
                    StructFieldFormat,
                    Tab1,
                    Tools.FormatStr("{0}_{1}_{2} ", "VG", Tools.CutString(param.ParentCmd.Name + "_V" + param.ParentCmd.Parent.Version, "COMMAND_CLASS"), "VG"),
                    " " + Tools.MakeLegalMixCaseIdentifier("variantGroup") + ";", "/*" + param.Comment + "*/");
            }
            else if (param.Mode == ParamModes.Property)
            {
                //Reference to property
                if (param.ParentParam != null && param.ParentParam.Mode == ParamModes.VariantGroup)
                {
                    sw.WriteLine(
                        StructFieldFormat,
                        Tab1,
                        Tools.FormatStr("{0}_{1}", "_VG_" + Tools.CutString(param.ParentCmd.Name + "_V" + param.ParentCmd.Parent.Version, "COMMAND_CLASS_"), param.Name.ToUpper()),
                        " " + Tools.MakeLegalMixCaseIdentifier(param.Text) + ";", "/*" + param.Comment + "*/");
                }
                else
                {
                    sw.WriteLine(
                        StructFieldFormat,
                        Tab1,
                        Tools.FormatStr("{0}_{1}", Tools.CutString(param.ParentCmd.Name + "_V" + param.ParentCmd.Parent.Version, "COMMAND_CLASS_"), param.Name.ToUpper()),
                        " " + Tools.MakeLegalMixCaseIdentifier(param.Text) + ";", "/*" + param.Comment + "*/");
                }
            }
            else
            {
                string comment = "/*" + param.Comment + "*/";
                if (param.Param1 != null && param.Param1.Count > 0 && param.Bits > 1)
                {
                    comment = "/* masked byte */";
                }
                int byteFieldCount = 0;
                if (param.Type == zwParamType.BITMASK && param.Size != 1 || param.SizeReference != null)
                {
                    byteFieldCount = 4; //Default array size
                }

                else if (param.Bits % 8 == 0)
                {
                    if (param.Bits == 8 && param.Size <= 1)
                    {
                        byteFieldCount = 0;
                        if (param.Defines != null)
                        {
                            string dsName = param.ParentCmd.Parent.Name.Replace("COMMAND_CLASS_", "") + "_" + param.Defines.Replace("zwave", "");
                            sw.WriteLine(StructFieldFormat, Tab1,
                                Tools.MakeLegalUpperCaseIdentifier(dsName) + versionPostfix,
                                " " + Tools.MakeLegalMixCaseIdentifier(paramText) + ";", comment);
                        }
                        else
                        {
                            sw.WriteLine(StructFieldFormat, Tab1, "BYTE", " " + Tools.MakeLegalMixCaseIdentifier(paramText) + ";", comment);
                        }
                    }
                    else
                    {
                        byteFieldCount = param.Size <= 1 ? param.Bits / 8 : param.Size * (param.Bits / 8);
                    }
                }
                comment = "";
                if (byteFieldCount > 0)
                {
                    sw.WriteLine(StructFieldFormat, Tab1, "BYTE", " " + Tools.MakeLegalMixCaseIdentifier(paramText) + "[" + byteFieldCount + "];", comment);
                }
            }
        }

        private static void GenerateVariantGroup(TextWriter sw, Param vgParam)
        {
            string versionPostfix = "";
            if (vgParam.ParentCmd.Parent.Version > 1) versionPostfix = "_V" + vgParam.ParentCmd.Parent.Version;

            if (vgParam.Param1 != null)
            {
                foreach (Param param in vgParam.Param1)
                {
                    if (param.Mode == ParamModes.Property)
                    {
                        GenerateBitFieldStruct(sw, param, "_VG_");
                    }
                }
            }

            sw.WriteLine("/************************************************************/");
            sw.WriteLine("/* {0} variant group structs */", Tools.CutUpperUnderscoreToMixedUpperLower(vgParam.ParentCmd.Name + "_V" + vgParam.ParentCmd.Parent.Version, "COMMAND_CLASS", " "));
            sw.WriteLine("/************************************************************/");
            sw.WriteLine("typedef struct _{0}_{1}_{2}_", "VG", Tools.CutString(vgParam.ParentCmd.Name + "_V" + vgParam.ParentCmd.Parent.Version, "COMMAND_CLASS"), "VG");
            sw.WriteLine("{");
            #region foreach params

            if (vgParam.Param1 != null)
            {
                foreach (Param param in vgParam.Param1)
                {
                    GenerateParamString(vgParam.ParentCmd, param, sw, versionPostfix);
                }
            }

            #endregion
            sw.Write("}");
            sw.WriteLine(" {0}_{1}_{2};", "VG", Tools.CutString(vgParam.ParentCmd.Name + "_V" + vgParam.ParentCmd.Parent.Version, "COMMAND_CLASS"), "VG");
            sw.WriteLine("");
        }

        private static void GenerateDefines(DefineSet ds, TextWriter sw, string versionPostfix)
        {
            string dsName = ds.Parent.Name.Replace("COMMAND_CLASS_", "") + "_" + ds.Name.Replace("zwave", "");
            sw.WriteLine("typedef enum _{0}_", Tools.MakeLegalUpperCaseIdentifier(dsName) + versionPostfix);
            sw.WriteLine("{");
            foreach (Define d in ds.Define)
            {
                sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/",
                    Tools.MakeLegalUpperCaseIdentifier(dsName) + versionPostfix + "_" +
                    Tools.MakeLegalUpperCaseIdentifier(d.Name), d.KeyId.ToString("X2"), d.Text));
            }
            sw.Write("}");
            sw.WriteLine(" {0};", Tools.MakeLegalUpperCaseIdentifier(dsName) + versionPostfix);
            sw.WriteLine("");
        }
        private string GenerateBasicDeviceClassIdentifiers(IEnumerable<BasicDevice> bDl)
        {
            string resultFileName = Path.Combine(SelectedFolderName, BasicDeviceClassHeaderName + ".h");
            StreamWriter sw = File.CreateText(resultFileName);
            sw.WriteLine(GetTitle(BasicDeviceClassHeaderName + ".h"));
            sw.WriteLine("#ifndef _{0}_H_", BasicDeviceClassHeaderName);
            sw.WriteLine("#define _{0}_H_", BasicDeviceClassHeaderName);
            sw.WriteLine("");
            sw.WriteLine("/************ Basic Device Class identifiers **************/");
            sw.WriteLine("typedef enum _BASIC_DEVICE_CLASSES_");
            sw.WriteLine("{");
            foreach (BasicDevice bd in bDl)
            {
                sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/", bd.Name, bd.KeyId.ToString("X2"), string.IsNullOrEmpty(bd.Comment) ? bd.Text : bd.Comment));
            }
            sw.WriteLine("} BASIC_DEVICE_CLASSES;");
            sw.WriteLine("");
            sw.WriteLine("#endif");
            sw.Close();
            return resultFileName;
        }
        private string GenerateGenericDeviceClassIdentifiers(IEnumerable<GenericDevice> gDl)
        {
            string resultFileName = Path.Combine(SelectedFolderName, GenericDeviceClassHeaderName + ".h");
            StreamWriter sw = File.CreateText(resultFileName);
            sw.WriteLine(GetTitle(GenericDeviceClassHeaderName + ".h"));
            sw.WriteLine("#ifndef _{0}_H_", GenericDeviceClassHeaderName);
            sw.WriteLine("#define _{0}_H_", GenericDeviceClassHeaderName);
            sw.WriteLine("");
            sw.WriteLine("/************ Generic Device Class identifiers **************/");
            sw.WriteLine("typedef enum _GENERIC_DEVICE_CLASSES_");
            sw.WriteLine("{");
            sw.WriteLine("");
            Dictionary<string, string> specDeviceLines = new Dictionary<string, string>();
            foreach (GenericDevice gd in gDl)
            {
                sw.WriteLine(Tools.FormatStr("  {0,-35} = 0x{1}, /*{2}*/", gd.Name, gd.KeyId.ToString("X2"), string.IsNullOrEmpty(gd.Comment) ? gd.Text : gd.Comment));
                if (!specDeviceLines.ContainsKey(gd.Name))
                {
                    specDeviceLines.Add(gd.Name, string.Format("  /* Device class {0} */", string.IsNullOrEmpty(gd.Comment) ? gd.Text : gd.Comment));
                }
                else
                {
                    throw new ApplicationException(string.Format("Key already exists: {0}", gd.Name));
                }
                foreach (SpecificDevice sd in gd.SpecificDevice)
                {
                    if (!specDeviceLines.ContainsKey(sd.Name))
                    {
                        specDeviceLines.Add(sd.Name, Tools.FormatStr("  {0,-48} = 0x{1}, /*{2}*/", sd.Name, sd.KeyId.ToString("X2"), string.IsNullOrEmpty(sd.Comment) ? sd.Text : sd.Comment));
                    }
                    else
                    {
                        Debug.WriteLine(sd.Name);
                    }
                }
            }
            sw.WriteLine("} GENERIC_DEVICE_CLASSES;");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("/************ Specific Device Class identifiers **************/");
            sw.WriteLine("typedef enum _SPECIVIC_DEVICE_CLASSES_");
            sw.WriteLine("{");
            foreach (KeyValuePair<string, string> sd in specDeviceLines)
            {
                if (sd.Value.StartsWith("  /* Device class"))
                {
                    sw.WriteLine("");
                }
                sw.WriteLine(sd.Value);
            }
            sw.WriteLine("");
            sw.WriteLine("} SPECIVIC_DEVICE_CLASSES;");
            sw.WriteLine("");
            sw.WriteLine("#endif");
            sw.Close();
            return resultFileName;
        }

        private static string GetTitle(string titleName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(@"// This file is auto generated. Do not edit it manually!"));
            sb.AppendLine(string.Format(@"// Generated on: {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            sb.AppendLine(string.Format(@"/*  {0}", (titleName + "  ").PadRight(75, '*')));
            sb.AppendLine("*");
            sb.AppendLine("*          Z-Wave, the wireless language.");
            sb.AppendLine("*");
            sb.AppendLine("*");
            sb.AppendLine("*");
            sb.AppendLine("* Author: ");
            sb.AppendLine("*");
            sb.AppendLine("* Last Changed By:  $Author:  $");
            sb.AppendLine("* Revision:         $Revision: $");
            sb.AppendLine("* Last Changed:     $Date: $");
            sb.AppendLine("*");
            sb.AppendLine("*");
            sb.AppendLine("*");
            sb.AppendLine("*******************************************************************************/");
            return sb.ToString();

        }
    }
}
