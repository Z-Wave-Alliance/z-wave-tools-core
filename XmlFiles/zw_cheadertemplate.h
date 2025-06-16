/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: 2025 Trident IoT, LLC <https://www.tridentiot.com>
﻿<!GENERATING_INFO!>
 % The XML editor inserts here information about the automatic generation:
 % Ex:
 %  //Generated on: 1:50:46 PM, Tuesday, June 28, 2005
 %  //ZW_classcmd.h from protocol release:1_65
 /**
 * @file
 <!GENERATING_VERSION!>
 * Device and command class types and definitions.
 * 
 * @copyright 2022 Silicon Laboratories Inc.
 */
#ifndef _ZW_CLASSCMD_H_
#define _ZW_CLASSCMD_H_

 /****************************************************************************/
 /*                              INCLUDE FILES                               */
 /****************************************************************************/
#include <stdint.h>

/****************************************************************************
 *    TYPES and DEFINITIONS
 ***************************************************************************/

<!FRAME_MACRO!>
 % For all commands insert a collection macro. First a comment giving the
 % command class, then a line for each command:
 %  /* Command class Xxxx */\
 %  ZW_YYYY_FRAME      ZW_YyyyFrame;\
 % Ex:
 %  /* Thermostat Mode command class */\
 %  ZW_THERMOSTAT_MODE_SET_FRAME                          ZW_ThermostatModeSetFrame;\
 %  ZW_THERMOSTAT_MODE_GET_FRAME                          ZW_ThermostatModeGetFrame;\
 %  ZW_THERMOSTAT_MODE_REPORT_FRAME                       ZW_ThermostatModeReportFrame;\
 %  ZW_THERMOSTAT_MODE_SUPPORTED_GET_FRAME                ZW_ThermostatModeSupportedGetFrame;\
 %  ZW_THERMOSTAT_MODE_SUPPORTED_REPORT_FRAME             ZW_ThermostatModeSupportedReportFrame;
 % (All lines except last line ends with a '\')


/************ Basic Device Class identifiers **************/
<!BASIC_DEVICE_DEF!>
 % Defines for basic types. One line for each define:
 %  #define BASIC_TYPE_xxxx              0xHH /* comment */
 % Ex:
 %  #define BASIC_TYPE_CONTROLLER                           0x01 /* comment */


/***** Generic and Specific Device Class identifiers ******/
<!GEN_SPEC_DEVICE_DEF!>
 % Defines for generic and specific types. One line for each define:
 %  /* Device class xxxx */
 %  #define GENERIC_TYPE_xxxx            0xHH /* comment */
 %  #define SPECIFIC_TYPE_yyyy           0xHH /* comment */
 % Ex:
 %  /* Device class Switch Toggle */
 %  #define GENERIC_TYPE_SWITCH_TOGGLE                      0x13
 %  #define SPECIFIC_TYPE_SWITCH_TOGGLE_BINARY              0x01
 %  #define SPECIFIC_TYPE_SWITCH_TOGGLE_MULTILEVEL          0x02


/************* Z-Wave+ Role Type identifiers **************/
#define ROLE_TYPE_CONTROLLER_CENTRAL_STATIC                                              0x00
#define ROLE_TYPE_CONTROLLER_SUB_STATIC                                                  0x01
#define ROLE_TYPE_CONTROLLER_PORTABLE                                                    0x02
#define ROLE_TYPE_CONTROLLER_PORTABLE_REPORTING                                          0x03
#define ROLE_TYPE_END_NODE_PORTABLE                                                      0x04
#define ROLE_TYPE_END_NODE_ALWAYS_ON                                                     0x05
#define ROLE_TYPE_END_NODE_SLEEPING_REPORTING                                            0x06
#define ROLE_TYPE_END_NODE_SLEEPING_LISTENING                                            0x07
#define ROLE_TYPE_END_NODE_NETWORK_AWARE                                                 0x08


/************* Z-Wave+ Icon Type identifiers **************/
/* The Z-Wave+ Icon Types defined in this section is the  */
/* work of the Z-Wave Alliance.                           */
/* The most current list of Z-Wave+ Icon Types may be     */
/* found at Z-Wave Alliance web site along with           */
/* sample icons.                                          */
/* New Z-Wave+ Icon Types may be requested from the       */
/* Z-Wave Alliance.                                       */
/**********************************************************/
#define ICON_TYPE_UNASSIGNED                                                 0x0000   //MUST NOT be used by any product
                                                                           
#define ICON_TYPE_GENERIC_CENTRAL_CONTROLLER                                 0x0100   //Central Controller Device Type
                                                                           
#define ICON_TYPE_GENERIC_DISPLAY_SIMPLE                                     0x0200   //Display Simple Device Type
                                                                           
#define ICON_TYPE_GENERIC_DOOR_LOCK_KEYPAD                                   0x0300   //Door Lock Keypad  Device Type
                                                                           
#define ICON_TYPE_GENERIC_FAN_SWITCH                                         0x0400   //Fan Switch  Device Type
                                                                           
#define ICON_TYPE_GENERIC_GATEWAY                                            0x0500   //Gateway  Device Type
                                                                           
#define ICON_TYPE_GENERIC_LIGHT_DIMMER_SWITCH                                0x0600   //Light Dimmer Switch  Device Type
#define ICON_TYPE_SPECIFIC_LIGHT_DIMMER_SWITCH_PLUGIN                        0x0601	  //Light Dimmer, implemented as a plugin device 
#define ICON_TYPE_SPECIFIC_LIGHT_DIMMER_SWITCH_WALL_OUTLET	                 0x0602	  //Light Dimmer, implemented as a wall outlet
#define ICON_TYPE_SPECIFIC_LIGHT_DIMMER_SWITCH_CEILING_OUTLET	             0x0603	  //Light Dimmer, implemented as a ceiling outlet
#define ICON_TYPE_SPECIFIC_LIGHT_DIMMER_SWITCH_WALL_LAMP     	             0x0604	  //Relay device, implemented as a wall mounted lamp
#define ICON_TYPE_SPECIFIC_LIGHT_DIMMER_SWITCH_LAMP_POST_HIGH	             0x0605	  //Relay device, implemented as a ceiling outlet
#define ICON_TYPE_SPECIFIC_LIGHT_DIMMER_SWITCH_LAMP_POST_LOW	             0x0606	  //Relay device, implemented as a ceiling outlet
                                                                           
#define ICON_TYPE_GENERIC_ON_OFF_POWER_SWITCH                                0x0700   //On/Off Power Switch  Device Type
#define ICON_TYPE_SPECIFIC_ON_OFF_POWER_SWITCH_PLUGIN	                     0x0701	  //Relay device, implemented as a plugin device
#define ICON_TYPE_SPECIFIC_ON_OFF_POWER_SWITCH_WALL_OUTLET	                 0x0702	  //Relay device, implemented as a wall outlet
#define ICON_TYPE_SPECIFIC_ON_OFF_POWER_SWITCH_CEILING_OUTLET	             0x0703	  //Relay device, implemented as a ceiling outlet
#define ICON_TYPE_SPECIFIC_ON_OFF_POWER_SWITCH_WALL_LAMP	                 0x0704	  //Relay device, implemented as a wall mounted lamp
#define ICON_TYPE_SPECIFIC_ON_OFF_POWER_SWITCH_LAMP_POST_HIGH	             0x0705	  //Relay device, implemented as a ceiling outlet
#define ICON_TYPE_SPECIFIC_ON_OFF_POWER_SWITCH_LAMP_POST_LOW	             0x0706	  //Relay device, implemented as a ceiling outlet
                                                                           
#define ICON_TYPE_GENERIC_POWER_STRIP                                        0x0800   //Power Strip  Device Type
#define ICON_TYPE_SPECIFIC_POWER_STRIP_INDIVIDUAL_OUTLET	                 0x08FF	  //Individual outlet of a power strip for showing outlets in exploded view
                                                                           
#define ICON_TYPE_GENERIC_REMOTE_CONTROL_AV                                  0x0900   //Remote Control AV  Device Type
                                                                           
#define ICON_TYPE_GENERIC_REMOTE_CONTROL_MULTI_PURPOSE                       0x0A00   //Remote Control Multi Purpose Device Type
                                                                           
#define ICON_TYPE_GENERIC_REMOTE_CONTROL_SIMPLE                              0x0B00   //Remote Control Simple Device Type
#define ICON_TYPE_SPECIFIC_REMOTE_CONTROL_SIMPLE_KEYFOB                      0x0B01   //Remote Control Simple Device Type (Key fob)
                                                                           
#define ICON_TYPE_GENERIC_SENSOR_NOTIFICATION                                0x0C00   //Sensor Notification Device Type
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_SMOKE_ALARM                   0x0C01   //Sensor Notification Device Type (Notification type Smoke Alarm)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_CO_ALARM                      0x0C02   //Sensor Notification Device Type (Notification type CO Alarm)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_CO2_ALARM                     0x0C03   //Sensor Notification Device Type (Notification type CO2 Alarm)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_HEAT_ALARM                    0x0C04   //Sensor Notification Device Type (Notification type Heat Alarm)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_WATER_ALARM                   0x0C05   //Sensor Notification Device Type (Notification type Water Alarm)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_ACCESS_CONTROL                0x0C06   //Sensor Notification Device Type (Notification type Access Control)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_HOME_SECURITY                 0x0C07   //Sensor Notification Device Type (Notification type Home Security)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_POWER_MANAGEMENT              0x0C08   //Sensor Notification Device Type (Notification type Power Management)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_SYSTEM                        0x0C09   //Sensor Notification Device Type (Notification type System)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_EMERGENCY_ALARM               0x0C0A   //Sensor Notification Device Type (Notification type Emergency Alarm)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_CLOCK                         0x0C0B   //Sensor Notification Device Type (Notification type Clock)
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_APPLIANCE                     0x0C0C
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_HOME_HEALTH                   0x0C0D
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_SIREN                         0x0C0E
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_WATER_VALVE                   0x0C0F
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_WEATHER_ALARM                 0x0C10
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_IRRIGATION                    0x0C11
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_GAS_ALARM                     0x0C12
#define ICON_TYPE_SPECIFIC_SENSOR_NOTIFICATION_MULTIDEVICE                   0x0CFF   //Sensor Notification Device Type (Bundled Notification functions)
                                                                           
#define ICON_TYPE_GENERIC_SENSOR_MULTILEVEL                                  0x0D00   //Sensor Multilevel Device Type
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_AIR_TEMPERATURE                 0x0D01   //Sensor Multilevel Device Type (Sensor type Air Temperature)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_GENERAL_PURPOSE_VALUE           0x0D02   //Sensor Multilevel Device Type (Sensor type General Purpose Value)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_LUMINANCE                       0x0D03   //Sensor Multilevel Device Type (Sensor type Luminance)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_POWER                           0x0D04   //Sensor Multilevel Device Type (Sensor type Power)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_HUMIDITY                        0x0D05   //Sensor Multilevel Device Type (Sensor type Humidity)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_VELOCITY                        0x0D06   //Sensor Multilevel Device Type (Sensor type Velocity)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_DIRECTION                       0x0D07   //Sensor Multilevel Device Type (Sensor type Direction)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ATMOSPHERIC_PRESSURE            0x0D08   //Sensor Multilevel Device Type (Sensor type Atmospheric Pressure)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_BAROMETRIC_PRESSURE             0x0D09   //Sensor Multilevel Device Type (Sensor type Barometric Pressure)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SOLOR_RADIATION                 0x0D0A   //Sensor Multilevel Device Type (Sensor type Solar Radiation)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_DEW_POINT                       0x0D0B   //Sensor Multilevel Device Type (Sensor type Dew Point)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_RAIN_RATE                       0x0D0C   //Sensor Multilevel Device Type (Sensor type Rain Rate)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TIDE_LEVEL                      0x0D0D   //Sensor Multilevel Device Type (Sensor type Tide Level)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_WEIGHT                          0x0D0E   //Sensor Multilevel Device Type (Sensor type Weight)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_VOLTAGE                         0x0D0F   //Sensor Multilevel Device Type (Sensor type Voltage)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_CURRENT                         0x0D10   //Sensor Multilevel Device Type (Sensor type Current)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_CO2_LEVEL                       0x0D11   //Sensor Multilevel Device Type (Sensor type CO2 Level)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_AIR_FLOW                        0x0D12   //Sensor Multilevel Device Type (Sensor type Air Flow)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TANK_CAPACITY                   0x0D13   //Sensor Multilevel Device Type (Sensor type Tank Capacity)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_DISTANCE                        0x0D14   //Sensor Multilevel Device Type (Sensor type Distance)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ANGLE_POSITION                  0x0D15   //Sensor Multilevel Device Type (Sensor type Angle Position)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ROTATION                        0x0D16   //Sensor Multilevel Device Type (Sensor type Rotation)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_WATER_TEMPERATURE               0x0D17   //Sensor Multilevel Device Type (Sensor type Water Temperature)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SOIL_TEMPERATURE                0x0D18   //Sensor Multilevel Device Type (Sensor type Soil Temperature)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SEISMIC_INTENSITY               0x0D19   //Sensor Multilevel Device Type (Sensor type Seismic Intensity)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_SEISMIC_MAGNITUDE               0x0D1A   //Sensor Multilevel Device Type (Sensor type Seismic Magnitude)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ULTRAVIOLET                     0x0D1B   //Sensor Multilevel Device Type (Sensor type Ultraviolet)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ELECTRICAL_RESISTIVITY          0x0D1C   //Sensor Multilevel Device Type (Sensor type Electrical Resistivity)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_ELECTRICAL_CONDUCTIVITY         0x0D1D   //Sensor Multilevel Device Type (Sensor type Electrical Conductivity)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_LOUDNESS                        0x0D1E   //Sensor Multilevel Device Type (Sensor type Loudness)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_MOISTURE                        0x0D1F   //Sensor Multilevel Device Type (Sensor type Moisture)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_FREQUENCY                       0x0D20   //Sensor Multilevel Device Type (Sensor type Frequency)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TIME                            0x0D21   //Sensor Multilevel Device Type (Sensor type Time )
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_TARGET_TEMPERATURE              0x0D22   //Sensor Multilevel Device Type (Sensor type Target Temperature)
#define ICON_TYPE_SPECIFIC_SENSOR_MULTILEVEL_MULTIDEVICE                     0x0DFF   //Sensor Multilevel Device Type (Bundled Sensor functions)
                                                                           
#define ICON_TYPE_GENERIC_SET_TOP_BOX                                        0x0E00   //Set Top Box Device Type
                                                                           
#define ICON_TYPE_GENERIC_SIREN                                              0x0F00   //Siren Device Type
                                                                           
#define ICON_TYPE_GENERIC_SUB_ENERGY_METER                                   0x1000   //Sub Energy Meter Device Type
                                                                           
#define ICON_TYPE_GENERIC_SUB_SYSTEM_CONTROLLER                              0x1100   //Sub System Controller Device Type
                                                                           
#define ICON_TYPE_GENERIC_THERMOSTAT                                         0x1200   //Thermostat Device Type
#define ICON_TYPE_SPECIFIC_THERMOSTAT_LINE_VOLTAGE                           0x1201   //Thermostat Line Voltage Device Type
#define ICON_TYPE_SPECIFIC_THERMOSTAT_SETBACK                                0x1202   //Thermostat Setback Device Type
                                                                           
#define ICON_TYPE_GENERIC_THERMOSTAT_SETBACK_OBSOLETED                       0x1300   //Thermostat Setback [Obsoleted] Device Type
                                                                           
#define ICON_TYPE_GENERIC_TV                                                 0x1400   //TV Device Type
                                                                           
#define ICON_TYPE_GENERIC_VALVE_OPEN_CLOSE                                   0x1500   //Valve Open/Close Device Type
                                                                           
#define ICON_TYPE_GENERIC_WALL_CONTROLLER                                    0x1600   //Wall Controller Device Type
                                                                           
#define ICON_TYPE_GENERIC_WHOLE_HOME_METER_SIMPLE                            0x1700   //Whole Home Meter Simple Device Type
                                                                           
#define ICON_TYPE_GENERIC_WINDOW_COVERING_NO_POSITION_ENDPOINT               0x1800   //Window Covering No Position/Endpoint  Device Type
                                                                           
#define ICON_TYPE_GENERIC_WINDOW_COVERING_ENDPOINT_AWARE                     0x1900   //Window Covering Endpoint Aware Device Type
                                                                           
#define ICON_TYPE_GENERIC_WINDOW_COVERING_POSITION_ENDPOINT_AWARE            0x1A00   //Window Covering Position/Endpoint Aware Device Type

#define ICON_TYPE_GENERIC_REPEATER                                           0x1B00   //Repeater Device Type 

#define ICON_TYPE_GENERIC_DIMMER_WALL_SWITCH	                             0x1C00	  //Wall Switch
#define ICON_TYPE_SPECIFIC_DIMMER_WALL_SWITCH_ONE_BUTTON	                 0x1C01	  //Wall Switch, 1 button
#define ICON_TYPE_SPECIFIC_DIMMER_WALL_SWITCH_TWO_BUTTONS	                 0x1C02	  //Wall Switch, 2 buttons
#define ICON_TYPE_SPECIFIC_DIMMER_WALL_SWITCH_THREE_BUTTONS	                 0x1C03	  //Wall Switch, 3 buttons
#define ICON_TYPE_SPECIFIC_DIMMER_WALL_SWITCH_FOUR_BUTTONS	                 0x1C04	  //Wall Switch, 4 buttons
#define ICON_TYPE_SPECIFIC_DIMMER_WALL_SWITCH_ONE_ROTARY	                 0x1CF1	  //Wall Switch, 1 rotary knob

#define ICON_TYPE_GENERIC_ON_OFF_WALL_SWITCH	                             0x1D00	  //Wall Switch
#define ICON_TYPE_SPECIFIC_ON_OFF_WALL_SWITCH_ONE_BUTTON	                 0x1D01	  //Wall Switch, 1 button
#define ICON_TYPE_SPECIFIC_ON_OFF_WALL_SWITCH_TWO_BUTTONS	                 0x1D02	  //Wall Switch, 2 buttons
#define ICON_TYPE_SPECIFIC_ON_OFF_WALL_SWITCH_THREE_BUTTONS	                 0x1D03	  //Wall Switch, 3 buttons
#define ICON_TYPE_SPECIFIC_ON_OFF_WALL_SWITCH_FOUR_BUTTONS	                 0x1D04	  //Wall Switch, 4 buttons
#define ICON_TYPE_SPECIFIC_ON_OFF_WALL_SWITCH_DOOR_BELL                      0x1DE1   //Door Bell (button)
#define ICON_TYPE_SPECIFIC_ON_OFF_WALL_SWITCH_ONE_ROTARY	                 0x1DF1	  //Wall Switch, 1 rotary knob

#define ICON_TYPE_GENERIC_BARRIER                                            0x1E00   //Barrier

#define ICON_TYPE_GENERIC_IRRIGATION                                         0x1F00   //Irrigation

#define ICON_TYPE_GENERIC_ENTRY_CONTROL                                      0x2000   //Entry Control
#define ICON_TYPE_SPECIFIC_ENTRY_CONTROL_KEYPAD_0_9                          0x2001   //Entry Control Keypad 0-9
#define ICON_TYPE_SPECIFIC_ENTRY_CONTROL_RFID_TAG_READER_NO_BUTTON           0x2002   //Entry Control RFID tag reader, no button


/************* Manufacturer ID identifiers **************/
#define MFG_ID_NOT_DEFINED                                         0xFFFF   //Not defined
#define MFG_ID_2B_ELECTRONICS                                      0x0028   //2B Electronics
#define MFG_ID_2GIG_TECHNOLOGIES_INC                               0x009B   //2gig Technologies Inc.
#define MFG_ID_3E_TECHNOLOGIES                                     0x002A   //3e Technologies
#define MFG_ID_A1_COMPONENTS                                       0x0022   //A-1 Components
#define MFG_ID_ABILIA                                              0x0117   //Abilia
#define MFG_ID_ACROCOMM_CORP                                       0x034A   //AcroComm Corp.
#define MFG_ID_ACT_ADVANCED_CONTROL_TECHNOLOGIES                   0x0001   //ACT - Advanced Control Technologies
#define MFG_ID_ADMOBILIZE_LLC                                      0x0297   //AdMobilize, LLC
#define MFG_ID_ADOX_INC                                            0x0101   //ADOX, Inc.
#define MFG_ID_ADTRUSTMEDIA_LLC_DBA_EZLO                           0x025B   //AdTrustMedia LLC  dba: eZLO
#define MFG_ID_ADVANCED_OPTRONIC_DEVICES_CO_LTD                    0x016C   //Advanced Optronic Devices Co.,Ltd
#define MFG_ID_ADVENTURE_INTERACTIVE                               0x009E   //Adventure Interactive
#define MFG_ID_AEINNOVATION_AEI                                    0x035B   //Aeinnovation (AEI)
#define MFG_ID_AENSYS_INFORMATICS_LTD                              0x0350   //AENSys Informatics Ltd.
#define MFG_ID_AEON_LABS                                           0x0086   //AEON Labs
#define MFG_ID_AEOTEC_LIMITED                                      0x0371   //Aeotec Limited
#define MFG_ID_AIRVENT_SAM_SPA                                     0x0088   //Airvent SAM S.p.A.
#define MFG_ID_ALARMCOM                                            0x0094   //Alarm.com
#define MFG_ID_ALERTME                                             0x0126   //Alertme
#define MFG_ID_ALLEATO                                             0x0372   //ALLEATO
#define MFG_ID_ALLEGION                                            0x003B   //Allegion
#define MFG_ID_ALPHANETWORKS                                       0x028E   //Alphanetworks
#define MFG_ID_ALPHONSUS_TECH                                      0x0230   //Alphonsus Tech
#define MFG_ID_AMADAS_CO_LTD                                       0x029F   //AMADAS Co., LTD 
#define MFG_ID_AMDOCS                                              0x019C   //Amdocs
#define MFG_ID_AMERICAN_GRID_INC                                   0x005A   //American Grid, Inc.
#define MFG_ID_AMETA_INTERNATIONAL_CO_LTD                          0x0384   //Ameta International Co. Ltd.
#define MFG_ID_ANCHOR_TECH                                         0x032B   //Anchor Tech 
#define MFG_ID_ANIMUS_HOME_AB                                      0x0392   //Animus Home AB
#define MFG_ID_ANTIK_TECHNOLOGY_LTD                                0x026D   //Antik Technology Ltd.
#define MFG_ID_ANYCOMM_CORPORATION                                 0x0078   //anyCOMM Corporation
#define MFG_ID_APPLIED_MICRO_ELECTRONICS_AME_BV                    0x0144   //Applied Micro Electronics "AME" BV
#define MFG_ID_ARKEA                                               0x0291   //Arkea
#define MFG_ID_ASIA_HEADING                                        0x0029   //Asia Heading
#define MFG_ID_ASITEQ                                              0x0231   //ASITEQ
#define MFG_ID_ASKEY_COMPUTER_CORP                                 0x028A   //Askey Computer Corp.
#define MFG_ID_ASSA_ABLOY                                          0x0129   //ASSA ABLOY
#define MFG_ID_ASTRALINK                                           0x013B   //AstraLink
#define MFG_ID_ATT                                                 0x0134   //AT&T
#define MFG_ID_ATECH                                               0x002B   //Atech
#define MFG_ID_ATHOM_BV                                            0x0244   //Athom BV
#define MFG_ID_AUCEAN_TECHNOLOGY_INC                               0x032A   //AUCEAN TECHNOLOGY. INC
#define MFG_ID_AVADESIGN_TECHNOLOGY_CO_                            0x025D   //Avadesign Technology Co.,
#define MFG_ID_AVADESIGN_TECHNOLOGY_CO_LTD                         0x0155   //Avadesign Technology Co., Ltd.
#define MFG_ID_AXESSTEL_INC                                        0x0146   //Axesstel Inc
#define MFG_ID_BALBOA_INSTRUMENTS                                  0x0018   //Balboa Instruments
#define MFG_ID_BANDI_COMM_TECH_INC                                 0x0236   //Bandi Comm Tech Inc.
#define MFG_ID_BEIJING_SINOAMERICAN_BOYI_SOFTWARE_DEVELOPMENT_CO_L 0x0204   //Beijing Sino-American Boyi Software Development Co., Ltd
#define MFG_ID_BEIJING_UNIVERSAL_ENERGY_HUAXIA_TECHNOLOGY_CO_LTD   0x0251   //Beijing Universal Energy Huaxia Technology Co.,Ltd
#define MFG_ID_BELLATRIX_SYSTEMS_INC                               0x0196   //Bellatrix Systems, Inc.
#define MFG_ID_BENETEK                                             0x032D   //Benetek
#define MFG_ID_BENEXT                                              0x008A   //BeNext
#define MFG_ID_BESAFER                                             0x002C   //BeSafer
#define MFG_ID_BFT_SPA                                             0x014B   //BFT S.p.A.
#define MFG_ID_BIT7_INC                                            0x0052   //Bit7 Inc.
#define MFG_ID_BLAZE_AUTOMATION                                    0x0311   //Blaze Automation
#define MFG_ID_BMS_EVLER_LTD                                       0x0213   //BMS Evler LTD
#define MFG_ID_BOCA_DEVICES                                        0x0023   //Boca Devices
#define MFG_ID_BONIG_UND_KALLENBACH_OHG                            0x0169   //Bönig und Kallenbach oHG
#define MFG_ID_BOSCH_SECURITY_SYSTEMS_INC                          0x015C   //Bosch Security Systems, Inc
#define MFG_ID_BRK_BRANDS_INC                                      0x0138   //BRK Brands, Inc.
#define MFG_ID_BROADBAND_ENERGY_NETWORKS_INC                       0x002D   //Broadband Energy Networks Inc.
#define MFG_ID_BTSTAR_HK_TECHNOLOGY_COMPANY_LIMITED                0x024A   //BTSTAR(HK) TECHNOLOGY COMPANY LIMITED
#define MFG_ID_BUFFALO_INC                                         0x0145   //Buffalo Inc.
#define MFG_ID_BUILDING_36_TECHNOLOGIES                            0x0190   //Building 36 Technologies
#define MFG_ID_BULCRAFT_CONTROL                                    0x0396   //Bulcraft Control
#define MFG_ID_BULOGICS                                            0x0026   //BuLogics
#define MFG_ID_BONIG_UND_KALLENBACH_OHG                            0x0169   //Bönig und Kallenbach oHG
#define MFG_ID_CALIX                                               0x0398   //Calix
#define MFG_ID_CAMEO_COMMUNICATIONS_INC                            0x009C   //Cameo Communications Inc.
#define MFG_ID_CARRIER                                             0x002E   //Carrier
#define MFG_ID_CASAWORKS                                           0x000B   //CasaWorks
#define MFG_ID_CASENIO_AG                                          0x0243   //casenio AG
#define MFG_ID_CBCC_DOMOTIQUE_SAS                                  0x0166   //CBCC Domotique SAS
#define MFG_ID_CENTRALITE_SYSTEMS_INC                              0x0246   //CentraLite Systems, Inc
#define MFG_ID_CENTURYLINK                                         0x033C   //Centurylink
#define MFG_ID_CHAMBERLAIN_GROUP                                   0x0359   //Chamberlain Group
#define MFG_ID_CHECKIT_SOLUTIONS_INC                               0x014E   //Check-It Solutions Inc.
#define MFG_ID_CHENGPUTECH                                         0x038F   //CHENGPUTECH
#define MFG_ID_CHINA_SECURITY_FIRE_IOT_SENSING_CO_LTD              0x0320   //China Security & Fire IOT Sensing CO., LTD 
#define MFG_ID_CHROMAGIC_TECHNOLOGIES_CORPORATION                  0x0116   //Chromagic Technologies Corporation
#define MFG_ID_CHUANGO_SECURITY_TECHNOLOGY_CORPORATION             0x0280   //Chuango Security Technology Corporation
#define MFG_ID_CISCO_CONSUMER_BUSINESS_GROUP                       0x0082   //Cisco Consumer Business Group
#define MFG_ID_CLARE_CONTROLS                                      0x0194   //Clare Controls
#define MFG_ID_CLIMAX_TECHNOLOGY_LTD                               0x018E   //Climax Technology, Ltd.
#define MFG_ID_CLOUD_MEDIA                                         0x0200   //Cloud Media
#define MFG_ID_COLOR_KINETICS_INCORPORATED                         0x002F   //Color Kinetics Incorporated
#define MFG_ID_COMAP                                               0x0329   //COMAP
#define MFG_ID_COMFORTABILITY                                      0x0309   //Comfortability
#define MFG_ID_COMPUTIME                                           0x0140   //Computime
#define MFG_ID_CONNECTED_OBJECT                                    0x011B   //Connected Object
#define MFG_ID_CONNECTHOME                                         0x0179   //ConnectHome
#define MFG_ID_CONNECTION_TECHNOLOGY_SYSTEMS                       0x0285   //CONNECTION TECHNOLOGY SYSTEMS 
#define MFG_ID_CONTEC_INTELLIGENT_HOUSING                          0x025D   //Contec intelligent housing 
#define MFG_ID_CONTROL4_CORPORATION                                0x023F   //Control4 Corporation
#define MFG_ID_CONTROLTHINK_LC                                     0x0019   //ControlThink LC
#define MFG_ID_CONVERGEX_LTD                                       0x000F   //ConvergeX Ltd.
#define MFG_ID_COOLGUARD                                           0x007D   //CoolGuard
#define MFG_ID_COOPER_LIGHTING                                     0x0079   //Cooper Lighting
#define MFG_ID_COOPER_WIRING_DEVICES                               0x001A   //Cooper Wiring Devices
#define MFG_ID_COVENTIVE_TECHNOLOGIES_INC                          0x009D   //Coventive Technologies Inc.
#define MFG_ID_CPRO                                                0x0379   //CPRO
#define MFG_ID_CRIBOS                                              0x0383   //CribOS
#define MFG_ID_CVNET                                               0x0328   //Cvnet
#define MFG_ID_CYBERHOUSE                                          0x0014   //Cyberhouse
#define MFG_ID_CYBERTAN_TECHNOLOGY_INC                             0x0067   //CyberTAN Technology, Inc.
#define MFG_ID_CYTECH_TECHNOLOGY_PRE_LTD                           0x0030   //Cytech Technology Pre Ltd.
#define MFG_ID_D3_TECHNOLOGY_CO_LTD                                0x0294   //D-3 Technology Co. Ltd
#define MFG_ID_DANFOSS                                             0x0002   //Danfoss
#define MFG_ID_DAWON_DNS                                           0x018C   //Dawon DNS
#define MFG_ID_DECORIS_INTELLIGENT_SYSTEM_LIMITED                  0x020A   //Decoris Intelligent System Limited
#define MFG_ID_DEFACONTROLS_BV                                     0x013F   //Defacontrols BV
#define MFG_ID_DEFARO                                              0x032E   //DEFARO
#define MFG_ID_DESTINY_NETWORKS                                    0x0031   //Destiny Networks
#define MFG_ID_DEVOLO                                              0x0175   //Devolo
#define MFG_ID_DIEHL_AKO                                           0x0103   //Diehl AKO
#define MFG_ID_DIGITAL_5_INC                                       0x0032   //Digital 5, Inc.
#define MFG_ID_DIGITAL_WATCHDOG                                    0x035D   //Digital Watchdog
#define MFG_ID_DIGITALZONE                                         0x0228   //DigitalZone
#define MFG_ID_DLINK                                               0x0108   //D-Link
#define MFG_ID_DMP_DIGITAL_MONITORING_PRODUCTS                     0x0127   //DMP (Digital Monitoring Products)
#define MFG_ID_DOMINO_SISTEMI_DOO                                  0x0177   //Domino sistemi d.o.o.
#define MFG_ID_DOMITECH_PRODUCTS_LLC                               0x020E   //Domitech Products, LLC
#define MFG_ID_DONGGUAN_ZHOU_DA_ELECTRONICS_CO_LTD                 0x020C   //Dongguan Zhou Da Electronics Co.,Ltd
#define MFG_ID_DRACOR_INC                                          0x017D   //DRACOR Inc.
#define MFG_ID_DRAGON_TECH_INDUSTRIAL_LTD                          0x0184   //Dragon Tech Industrial, Ltd.
#define MFG_ID_DTV_RESEARCH_UNIPESSOAL_LDA                         0x0223   //DTV Research Unipessoal, Lda
#define MFG_ID_DUNEHD                                              0x0272   //Dune-HD
#define MFG_ID_DUSUN_ELECTRON_LTD                                  0x0334   //Dusun Electron Ltd.
#define MFG_ID_DVACO_GROUP                                         0x031B   //DVACO GROUP
#define MFG_ID_DYNAQUIP_CONTROLS                                   0x0132   //DynaQuip Controls
#define MFG_ID_EASY_SAVER_CO_INC                                   0x0247   //EASY SAVER Co., Inc
#define MFG_ID_EATON                                               0x001A   //Eaton
#define MFG_ID_EBV                                                 0x017C   //EbV
#define MFG_ID_ECHOSTAR                                            0x016B   //Echostar
#define MFG_ID_ECO_AUTOMATION                                      0x028F   //Eco Automation
#define MFG_ID_ECO_LIFE_ENGINEERING_CO_LTD                         0x037C   //Eco Life Engineering Co., Ltd.
#define MFG_ID_ECOLINK                                             0x014A   //Ecolink
#define MFG_ID_ECONET_CONTROLS                                     0x0157   //EcoNet Controls
#define MFG_ID_ECS                                                 0x039D   //ECS
#define MFG_ID_EELECTRON_SPA                                       0x031F   //Eelectron SpA
#define MFG_ID_EHOME_AUTOMATION                                    0x010D   //e-Home AUTOMATION
#define MFG_ID_EI_ELECTRONICS                                      0x026B   //Ei Electronics 
#define MFG_ID_EKA_SYSTEMS                                         0x0087   //Eka Systems
#define MFG_ID_ELEAR_SOLUTIONS_TECH_PVT_LTD                        0x0389   //Elear Solutions Tech Pvt. Ltd.
#define MFG_ID_ELECTRONIC_SOLUTIONS                                0x0033   //Electronic Solutions
#define MFG_ID_ELEXA_CONSUMER_PRODUCTS_INC                         0x021F   //Elexa Consumer Products Inc.
#define MFG_ID_ELGEV_ELECTRONICS_LTD                               0x0034   //El-Gev Electronics LTD
#define MFG_ID_ELK_PRODUCTS_INC                                    0x001B   //ELK Products, Inc.
#define MFG_ID_EMBEDDED_DATA_SYSTEMS                               0x0393   //Embedded Data Systems
#define MFG_ID_EMBEDDED_SYSTEM_DESIGN_LIMITED                      0x020B   //Embedded System Design Limited
#define MFG_ID_EMBEDIT_AS                                          0x0035   //Embedit A/S
#define MFG_ID_EMPERS_TECH_CO_LTD                                  0x0284   //Empers Tech Co., Ltd.
#define MFG_ID_EMT_CONTROLS                                        0x0336   //EMT Controls
#define MFG_ID_ENBLINK_CO_LTD                                      0x014D   //Enblink Co. Ltd
#define MFG_ID_ENWOX_TECHNOLOGIES_SRO                              0x0219   //Enwox Technologies s.r.o.
#define MFG_ID_ERONE                                               0x006F   //Erone
#define MFG_ID_ESSENCE_SECURITY                                    0x0160   //Essence Security
#define MFG_ID_ESSENTIAL_TECHNOLOGIES_INC                          0x029B   //ESSENTIAL TECHNOLOGIES INC.
#define MFG_ID_EUROTRONICS                                         0x0148   //Eurotronics
#define MFG_ID_EVERSPRING                                          0x0060   //Everspring
#define MFG_ID_EVOLVE                                              0x0113   //Evolve
#define MFG_ID_EVOLVERE_SPA                                        0x036F   //Evolvere SpA 
#define MFG_ID_EXCEPTIONAL_INNOVATIONS                             0x0036   //Exceptional Innovations
#define MFG_ID_EXHAUSTO                                            0x0004   //Exhausto
#define MFG_ID_EXIGENT_SENSORS                                     0x009F   //Exigent Sensors
#define MFG_ID_EXPRESS_CONTROLS                                    0x001E   //Express Controls
#define MFG_ID_EZEX_CORPORATION                                    0x0233   //eZEX Corporation
#define MFG_ID_FAKRO                                               0x0085   //Fakro
#define MFG_ID_FANTEM                                              0x016A   //Fantem
#define MFG_ID_FIBARGROUP                                          0x010F   //Fibargroup
#define MFG_ID_FIFTHPLAY_NV                                        0x0295   //fifthplay nv
#define MFG_ID_FLEX_AUTOMATION                                     0x002C   //Flex Automation
#define MFG_ID_FLEX_AUTOMATION_2                                   0x004F   //Flex Automation
#define MFG_ID_FLEXTRONICS                                         0x018D   //Flextronics
#define MFG_ID_FLUE_SENTINEL                                       0x0024   //Flue Sentinel
#define MFG_ID_FOARD_SYSTEMS                                       0x0037   //Foard Systems
#define MFG_ID_FOCAL_POINT_LIMITED                                 0x018F   //Focal Point Limited
#define MFG_ID_FOLLOWGOOD_TECHNOLOGY_COMPANY_LTD                   0x0137   //FollowGood Technology Company Ltd.
#define MFG_ID_FOREST_GROUP_NEDERLAND_BV                           0x0207   //Forest Group Nederland B.V
#define MFG_ID_FORTREZZ_LLC                                        0x0084   //FortrezZ LLC
#define MFG_ID_FOXCONN                                             0x011D   //Foxconn
#define MFG_ID_FOXCONN_INDUSTRIAL_INTERNET                         0x039C   //Foxconn Industrial Internet
#define MFG_ID_FROSTDALE                                           0x0110   //Frostdale
#define MFG_ID_FUTURE_HOME_AS                                      0x0305   //Future Home AS
#define MFG_ID_GE                                                  0x033E   //GE
#define MFG_ID_GES                                                 0x025A   //GES
#define MFG_ID_GKB_SECURITY_CORPORATION                            0x022B   //GKB Security Corporation
#define MFG_ID_GLOBALCHINATECH                                     0x018A   //Globalchina-Tech
#define MFG_ID_GOAP                                                0x0159   //Goap
#define MFG_ID_GOGGIN_RESEARCH                                     0x0076   //Goggin Research
#define MFG_ID_GOOD_WAY_TECHNOLOGY_CO_LTD                          0x0068   //Good Way Technology Co., Ltd
#define MFG_ID_GREENWAVE_REALITY_INC                               0x0099   //GreenWave Reality Inc.
#define MFG_ID_GRIB                                                0x018B   //Grib
#define MFG_ID_GUANGDONG_PHNIX_ECOENERGY_SOLUTION_LTD              0x0391   //GUANGDONG PHNIX ECO-ENERGY SOLUTION LTD.
#define MFG_ID_GUANGZHOU_RUIXIANG_ME_CO_LTD                        0x016D   //Guangzhou Ruixiang M&E Co., Ltd
#define MFG_ID_GUANGZHOU_ZEEWAVE_INFORMATION_TECHNOLOGY_CO_LTD     0x0158   //GuangZhou Zeewave Information Technology Co., Ltd.
#define MFG_ID_GUANGZHOU_SIMT_LIMITED                              0x0395   //Guangzhou_SIMT  Limited
#define MFG_ID_GUARDTEC_INC                                        0x037B   //Guardtec Inc.
#define MFG_ID_HAB_HOME_INTELLIGENCE_LLC                           0x0287   //HAB Home Intelligence, LLC
#define MFG_ID_HAMPOO                                              0x030D   //Hampoo
#define MFG_ID_HANGZHOU_IMAGIC_TECHNOLOGY_CO_LTD                   0x0387   //HangZhou iMagic Technology Co., Ltd
#define MFG_ID_HANK_ELECTRONICS_LTD                                0x0208   //HANK Electronics Ltd
#define MFG_ID_HANKOOK_GAS_KIKI_CO_LTD                             0x024C   //Hankook Gas Kiki CO.,LTD. 
#define MFG_ID_HAUPPAUGE                                           0x025C   //Hauppauge
#define MFG_ID_HAWKING_TECHNOLOGIES_INC                            0x0073   //Hawking Technologies Inc.
#define MFG_ID_HELTUN                                              0x0344   //HELTUN
#define MFG_ID_HERALD_DATANETICS_LIMITED                           0x020F   //Herald Datanetics Limited
#define MFG_ID_HITECH_AUTOMATION                                   0x0017   //HiTech Automation
#define MFG_ID_HOLION_ELECTRONIC_ENGINEERING_CO_LTD                0x0181   //Holion Electronic Engineering Co., Ltd
#define MFG_ID_HOLTEC_ELECTRONICS_BV                               0x013E   //Holtec Electronics BV
#define MFG_ID_HOME_AUTOMATED_LIVING                               0x000D   //Home Automated Living
#define MFG_ID_HOME_AUTOMATION_EUROPE                              0x009A   //Home Automation Europe
#define MFG_ID_HOME_AUTOMATION_INC                                 0x005B   //Home Automation Inc.
#define MFG_ID_HOME_CONTROLS                                       0x0293   //Home controls
#define MFG_ID_HOME_DIRECTOR                                       0x0038   //Home Director
#define MFG_ID_HOMEMANAGEABLES_INC                                 0x0070   //Homemanageables, Inc.
#define MFG_ID_HOMEPRO                                             0x0050   //Homepro
#define MFG_ID_HOMESCENARIO                                        0x0162   //HomeScenario
#define MFG_ID_HOMESEER_TECHNOLOGIES                               0x000C   //HomeSeer Technologies
#define MFG_ID_HONEST_TECHNOLOGY                                   0x0275   //Honest Technology
#define MFG_ID_HONEST_TECHNOLOGY_CO_LTD                            0x023D   //Honest Technology Co., Ltd.
#define MFG_ID_HONEYWELL                                           0x0039   //Honeywell
#define MFG_ID_HOPPE                                               0x0313   //Hoppe
#define MFG_ID_HORNBACH_BAUMARKT_AG                                0x0377   //HORNBACH Baumarkt AG
#define MFG_ID_HORUS_SMART_CONTROL                                 0x0298   //Horus Smart Control
#define MFG_ID_HOSEOTELNET                                         0x0221   //HOSEOTELNET
#define MFG_ID_HUAPIN_INFORMATION_TECHNOLOGY_CO_LTD                0x0180   //Huapin Information Technology Co.,Ltd
#define MFG_ID_HUAWEI_DEVICE_CO_LTD                                0x025F   //Huawei Device Co., Ltd. 
#define MFG_ID_HUAWEI_TECHNOLOGIES_CO_LTD                          0x024B   //Huawei Technologies Co., Ltd.
#define MFG_ID_HUNTER_DOUGLAS                                      0x007C   //Hunter Douglas
#define MFG_ID_HYUNDAI_TELECOM                                     0x0374   //Hyundai Telecom
#define MFG_ID_IAMSMART                                            0x038A   //iamsmart
#define MFG_ID_IAUTOMADE_PTE_LTD                                   0x0218   //iAutomade Pte Ltd
#define MFG_ID_ICOM_TECHNOLOGY_BV                                  0x0011   //iCOM Technology b.v.
#define MFG_ID_ICONTROL                                            0x0106   //iControl
#define MFG_ID_ICONTROL_NETWORKS                                   0x0106   //Icontrol Networks
#define MFG_ID_ID_LOCK_AS                                          0x0373   //ID Lock AS
#define MFG_ID_IDRF                                                0x0165   //ID-RF
#define MFG_ID_IEXERGY_GMBH                                        0x019E   //iEXERGY GmbH
#define MFG_ID_ILEVIA_SRL                                          0x031C   //Ilevia srl
#define MFG_ID_IMPACT_TECHNOLOGIES_AND_PRODUCTS                    0x0056   //Impact Technologies and Products
#define MFG_ID_IMPACT_TECHNOLOGIES_BV                              0x0061   //Impact Technologies BV
#define MFG_ID_INERGY_SYSTEMS_LLC                                  0x0385   //Inergy Systems LLC
#define MFG_ID_INFUSION_DEVELOPMENT                                0x012B   //Infusion Development
#define MFG_ID_INGERSOLL_RAND_SCHLAGE                              0x006C   //Ingersoll Rand (Schlage)
#define MFG_ID_INGERSOLL_RAND_ECOLINK                              0x011F   //Ingersoll Rand (was Ecolink)
#define MFG_ID_INKEL_CORP                                          0x0256   //Inkel Corp.
#define MFG_ID_INLON_SRL                                           0x003A   //Inlon Srl
#define MFG_ID_INNOBAND_TECHNOLOGIES_INC                           0x0141   //Innoband Technologies, Inc
#define MFG_ID_INNOPIA_TECHNOLOGIES_INC                            0x0382   //INNOPIA Technologies, Inc.
#define MFG_ID_INNOVUS                                             0x0077   //INNOVUS
#define MFG_ID_INOVELLI                                            0x031E   //Inovelli
#define MFG_ID_INSIGNIA                                            0x0100   //Insignia
#define MFG_ID_INTEL                                               0x0006   //Intel
#define MFG_ID_INTELLICON                                          0x001C   //IntelliCon
#define MFG_ID_INTERACTIVE_ELECTRONICS_SYSTEMS_IES                 0x0072   //Interactive Electronics Systems (IES)
#define MFG_ID_INTERMATIC                                          0x0005   //Intermatic
#define MFG_ID_INTERNATIONAL_INTEGRATED_SYSTEMS_INC_IISI           0x0338   //International Integrated Systems, Inc. (IISI)
#define MFG_ID_INTERNET_DOM                                        0x0013   //Internet Dom
#define MFG_ID_INTERSOFT                                           0x0288   //INTERSOFT
#define MFG_ID_INVALANCE                                           0x039E   //Invalance
#define MFG_ID_INVENTEC                                            0x0278   //Inventec
#define MFG_ID_IOOOTA                                              0x0368   //IOOOTA
#define MFG_ID_IQGROUP                                             0x005F   //IQ-Group
#define MFG_ID_IREVO                                               0x0212   //iRevo
#define MFG_ID_IUNGONL_BV                                          0x0253   //iungo.nl B.V.
#define MFG_ID_IWATSU                                              0x0123   //IWATSU
#define MFG_ID_JASCO_PRODUCTS                                      0x0063   //Jasco Products
#define MFG_ID_JIN_TAO_BAO                                         0x015A   //Jin Tao Bao
#define MFG_ID_JLABS_CORPORATION                                   0x039F   //JLabs Corporation
#define MFG_ID_JSW_PACIFIC_CORPORATION                             0x0164   //JSW Pacific Corporation
#define MFG_ID_KAIPULE_TECHNOLOGY_CO_LTD                           0x0214   //Kaipule Technology Co., Ltd.
#define MFG_ID_KAMSTRUP_AS                                         0x0091   //Kamstrup A/S
#define MFG_ID_KELLENDONK_ELEKTRONIK                               0x006A   //Kellendonk Elektronik
#define MFG_ID_KICHLER                                             0x0114   //Kichler
#define MFG_ID_KIWILAB                                             0x035F   //KIWILAB
#define MFG_ID_KLICKH_PVT_LTD                                      0x0139   //KlickH Pvt Ltd.
#define MFG_ID_KOOL_KONCEPTS                                       0x0261   //KOOL KONCEPTS
#define MFG_ID_KOPERA_DEVELOPMENT_INC                              0x0174   //Kopera Development Inc.
#define MFG_ID_KUMHO_ELECTRIC_INC                                  0x023A   //KUMHO ELECTRIC, INC
#define MFG_ID_LAGOTEK_CORPORATION                                 0x0051   //Lagotek Corporation
#define MFG_ID_LEAK_INTELLIGENCE_LLC                               0x0173   //Leak Intelligence, LLC
#define MFG_ID_LEEDARSON_LIGHTING_CO_LTD                           0x0300   //LEEDARSON LIGHTING CO., LTD.
#define MFG_ID_LEVION_TECHNOLOGIES_GMBH                            0x0187   //LEVION Technologies GmbH
#define MFG_ID_LEVITON                                             0x001D   //Leviton
#define MFG_ID_LEXEL                                               0x0015   //Lexel
#define MFG_ID_LG_ELECTRONICS                                      0x015B   //LG Electronics
#define MFG_ID_LIAONING_YOUWANG_LIGHTING_AND_ELECTRONIC_TECHNOLOGY 0x0362   //Liaoning Youwang Lighting and Electronic Technology Pty Ltd
#define MFG_ID_LIFESHIELD_LLC                                      0x0224   //LifeShield, LLC
#define MFG_ID_LIFESTYLE_NETWORKS                                  0x003C   //Lifestyle Networks
#define MFG_ID_LIGHT_ENGINE_LIMITED                                0x0210   //Light Engine Limited
#define MFG_ID_LIMEI                                               0x0342   //LIMEI
#define MFG_ID_LINK_ELECTRONICS_CO_LTD                             0x035C   //LINK ELECTRONICS Co., Ltd.
#define MFG_ID_LITE_AUTOMATION                                     0x0316   //Lite Automation
#define MFG_ID_LIVEGUARD_LTD                                       0x017A   //Liveguard Ltd.
#define MFG_ID_LIVING_STYLE_ENTERPRISES_LTD                        0x013A   //Living Style Enterprises, Ltd.
#define MFG_ID_LOCSTAR_TECHNOLOGY_CO_LTD                           0x015E   //Locstar Technology Co., Ltd
#define MFG_ID_LOGIC_GROUP                                         0x0234   //Logic Group
#define MFG_ID_LOGITECH                                            0x007F   //Logitech
#define MFG_ID_LOUDWATER_TECHNOLOGIES_LLC                          0x0025   //Loudwater Technologies, LLC
#define MFG_ID_LOWES                                               0x038C   //Lowes
#define MFG_ID_LS_CONTROL                                          0x0071   //LS Control
#define MFG_ID_LUFFANET_CO_LTE                                     0x036D   //Luffanet Co. Lte. 
#define MFG_ID_LUXEASY_TECHNOLOGY_COMPANY_LTD                      0x025E   //LUXEASY technology company LTD.
#define MFG_ID_LVI_PRODUKTER_AB                                    0x0062   //LVI Produkter AB
#define MFG_ID_M2M_SOLUTION                                        0x0192   //m2m Solution
#define MFG_ID_M2M_SOLUTION_2                                      0x0195   //M2M Solution
#define MFG_ID_MANODO_KTC                                          0x006E   //Manodo / KTC
#define MFG_ID_MARMITEK_BV                                         0x003D   //Marmitek BV
#define MFG_ID_MARTEC_ACCESS_PRODUCTS                              0x003E   //Martec Access Products
#define MFG_ID_MARTIN_RENZ_GMBH                                    0x0092   //Martin Renz GmbH
#define MFG_ID_MB_TURN_KEY_DESIGN                                  0x008F   //MB Turn Key Design
#define MFG_ID_MCOHOME_TECHNOLOGY_CO_LTD                           0x015F   //McoHome Technology Co., Ltd
#define MFG_ID_MCT_CO_LTD                                          0x0222   //MCT CO., LTD
#define MFG_ID_MEEDIO_LLC                                          0x0027   //Meedio, LLC
#define MFG_ID_MEGACHIPS                                           0x0107   //MegaChips
#define MFG_ID_MERCURY_CORPORATION                                 0x022D   //Mercury Corporation
#define MFG_ID_MERTEN                                              0x007A   //Merten
#define MFG_ID_MILANITY_INC                                        0x0238   //Milanity, Inc.
#define MFG_ID_MITSUMI                                             0x0112   //MITSUMI
#define MFG_ID_MOBILUS_MOTOR_SPOLKA_Z_OO                           0x019D   //MOBILUS MOTOR Spólka z o.o. 
#define MFG_ID_MODACOM_CO_LTD                                      0x0232   //MODACOM CO., LTD.
#define MFG_ID_MODSTROM                                            0x008D   //Modstrøm
#define MFG_ID_MOHITO_NETWORKS                                     0x000E   //Mohito Networks
#define MFG_ID_MONOPRICE                                           0x0202   //Monoprice
#define MFG_ID_MONSTER_CABLE                                       0x007E   //Monster Cable
#define MFG_ID_MOTION_CONTROL_SYSTEMS                              0x0125   //Motion Control Systems
#define MFG_ID_MOTOROLA                                            0x003F   //Motorola
#define MFG_ID_MSK_MIYAKAWA_SEISAKUSHO                             0x0122   //MSK - Miyakawa Seisakusho
#define MFG_ID_MTC_MAINTRONIC_GERMANY                              0x0083   //MTC Maintronic Germany
#define MFG_ID_MYSTROM                                             0x0143   //myStrom
#define MFG_ID_NANJING_EASTHOUSE_ELECTRICAL_CO_LTD                 0x016E   //Nanjing Easthouse Electrical Co., Ltd.
#define MFG_ID_NAPCO_SECURITY_TECHNOLOGIES_INC                     0x0121   //Napco Security Technologies, Inc.
#define MFG_ID_NCUBE                                               0x038D   //nCube
#define MFG_ID_NEC_PLATFORMS_LTD                                   0x036B   //NEC Platforms Ltd
#define MFG_ID_NEEO_AG                                             0x0241   //NEEO AG
#define MFG_ID_NEFIT                                               0x006D   //Nefit
#define MFG_ID_NEOCONTROL_US_LLC                                   0x0351   //NEOCONTROL US LLC
#define MFG_ID_NESS_CORPORATION_PTY_LTD                            0x0189   //Ness Corporation Pty Ltd
#define MFG_ID_NETGEAR                                             0x0133   //Netgear
#define MFG_ID_NEUSTA_NEXT_GMBH_CO_KG                              0x0248   //neusta next GmbH & Co. KG
#define MFG_ID_NEWLAND_COMMUNICATION_SCIENCE_TECHNOLOGY_CO_LTD     0x0203   //Newland Communication Science Technology Co., Ltd.
#define MFG_ID_NEXA_TRADING_AB                                     0x0268   //Nexa Trading AB
#define MFG_ID_NEXIA_HOME_INTELLIGENCE                             0x0178   //Nexia Home Intelligence
#define MFG_ID_NEXTENERGY                                          0x0075   //NextEnergy
#define MFG_ID_NHN_ENTERTAINMENT                                   0x0361   //NHN Entertainment
#define MFG_ID_NIE_TECHNOLOGY_CO_LTD                               0x0312   //NIE Technology Co., Ltd
#define MFG_ID_NINGBO_SENTEK_ELECTRONICS_CO_LTD                    0x0185   //Ningbo Sentek Electronics Co., Ltd
#define MFG_ID_NORTEK_SECURITY_CONTROL_LLC                         0x014F   //Nortek Security & Control LLC 
#define MFG_ID_NORTH_CHINA_UNIVERSITY_OF_TECHNOLOGY                0x0252   //North China University of Technology
#define MFG_ID_NORTHQ                                              0x0096   //NorthQ
#define MFG_ID_NOVAR_ELECTRICAL_DEVICES_AND_SYSTEMS_EDS            0x0040   //Novar Electrical Devices and Systems (EDS)
#define MFG_ID_NOVATEQNI_HK_LTD                                    0x020D   //Novateqni HK Ltd
#define MFG_ID_OBLO_LIVING_LLC                                     0x0296   //OBLO LIVING LLC
#define MFG_ID_OMNIMA_LIMITED                                      0x0119   //Omnima Limited
#define MFG_ID_ONSITE_PRO                                          0x014C   //OnSite Pro
#define MFG_ID_OPENPEAK_INC                                        0x0041   //OpenPeak Inc.
#define MFG_ID_OREGON_AUTOMATION                                   0x027D   //Oregon Automation 
#define MFG_ID_PANASONIC_ELECTRIC_WORKS_CO_LTD                     0x0104   //Panasonic Electric Works Co., Ltd.
#define MFG_ID_PANASONIC_ES_SHIN_DONGA_CO_LTD                      0x031A   //Panasonic ES Shin Dong-A Co., Ltd
#define MFG_ID_PANODIC_ELECTRIC_SHENZHEN_LIMITED                   0x028D   //Panodic Electric (Shenzhen) Limited
#define MFG_ID_PARATECH                                            0x0257   //PARATECH
#define MFG_ID_PASSIVSYSTEMS_LIMITED                               0x0172   //PassivSystems Limited
#define MFG_ID_PAXTON_ACCESS_LTD                                   0x0322   //Paxton Access Ltd
#define MFG_ID_PC_PARTNER                                          0x0281   //PC Partner
#define MFG_ID_PELLA                                               0x013D   //Pella
#define MFG_ID_PERMUNDO_GMBH                                       0x0245   //permundo GmbH
#define MFG_ID_PHILIA_TECHNOLOGY_CO_LTD                            0x0366   //PHILIA TECHNOLOGY Co., Ltd.
#define MFG_ID_PHILIO_TECHNOLOGY_CORP                              0x013C   //Philio Technology Corp
#define MFG_ID_PIXELA_CORPORATION                                  0x0277   //Pixela Corporation 
#define MFG_ID_POLYCONTROL                                         0x010E   //Poly-control
#define MFG_ID_POPP_CO                                             0x0154   //Popp & Co
#define MFG_ID_POWERHOUSE_DYNAMICS                                 0x0170   //Powerhouse Dynamics
#define MFG_ID_POWERLINX                                           0x0074   //PowerLinx
#define MFG_ID_POWERLYNX                                           0x0016   //PowerLynx
#define MFG_ID_PRAGMATIC_CONSULTING_INC                            0x0042   //Pragmatic Consulting Inc.
#define MFG_ID_PRODEA                                              0x0341   //Prodea
#define MFG_ID_PRODRIVE_TECHNOLOGIES                               0x0128   //Prodrive Technologies
#define MFG_ID_PROMIXIS_LLC                                        0x0161   //Promixis, LLC
#define MFG_ID_PULSE_TECHNOLOGIES_ASPALIS                          0x005D   //Pulse Technologies (Aspalis)
#define MFG_ID_PYTRONIC_AB                                         0x0376   //Pytronic AB 
#define MFG_ID_QEES                                                0x0095   //Qees
#define MFG_ID_QINGDAO_HONGYU_CLES_AIR_CONDITIONING_CO_LTD         0x0355   //Qingdao hongyu cles air conditioning co.,ltd.
#define MFG_ID_QOLSYS                                              0x012A   //Qolsys
#define MFG_ID_QUBY                                                0x0130   //Quby
#define MFG_ID_QUEENLOCK_IND_CO_LTD                                0x0163   //Queenlock Ind. Co., Ltd.
#define MFG_ID_RADEMACHER_GERATEELEKTRONIK_GMBH_CO_KG              0x0142   //Rademacher Geräte-Elektronik GmbH & Co. KG
#define MFG_ID_RADIO_THERMOSTAT_COMPANY_OF_AMERICA_RTC             0x0098   //Radio Thermostat Company of America (RTC)
#define MFG_ID_RAONIX_CO_LTD                                       0x0314   //Raonix Co., Ltd.
#define MFG_ID_RARITAN                                             0x008E   //Raritan
#define MFG_ID_RAYLIOS                                             0x0370   //Raylios
#define MFG_ID_RED_BEE_CO_LTD                                      0x021E   //Red Bee Co. Ltd
#define MFG_ID_REITZGROUPDE                                        0x0064   //Reitz-Group.de
#define MFG_ID_REMOTE_SOLUTION                                     0x022C   //Remote Solution
#define MFG_ID_REMOTE_TECHNOLOGIES_INCORPORATED                    0x0255   //Remote Technologies Incorporated
#define MFG_ID_REMOTEC                                             0x5254   //Remotec
#define MFG_ID_REPLY_SPA                                           0x039B   //Reply S.p.A.
#define MFG_ID_RESIDENTIAL_CONTROL_SYSTEMS_INC_RCS                 0x0010   //Residential Control Systems, Inc. (RCS)
#define MFG_ID_RET_NANJING_INTELLIGENCE_SYSTEM_CO_LTD              0x0216   //RET Nanjing Intelligence System CO.,Ltd
#define MFG_ID_REVOLV_INC                                          0x0153   //Revolv Inc
#define MFG_ID_RIMPORT_LTD                                         0x0147   //R-import Ltd.
#define MFG_ID_RISCO_GROUP                                         0x035E   //RISCO Group
#define MFG_ID_ROCCONNECT_INC                                      0x023B   //ROC-Connect, Inc.
#define MFG_ID_RPE_AJAX_LLC_DBS_SECUR_LTD                          0x0197   //RPE Ajax LLC (dbs Secur Ltd)
#define MFG_ID_RS_SCENE_AUTOMATION                                 0x0065   //RS Scene Automation
#define MFG_ID_RUBETEK                                             0x029D   //Rubetek
#define MFG_ID_S1                                                  0x0290   //S1
#define MFG_ID_SAFETECH_PRODUCTS                                   0x023C   //SafeTech Products
#define MFG_ID_SAMSUNG_ELECTRONICS_CO_LTD                          0x0201   //Samsung Electronics Co., Ltd.
#define MFG_ID_SAMSUNG_SDS                                         0x022E   //Samsung SDS
#define MFG_ID_SAN_SHIH_ELECTRICAL_ENTERPRISE_CO_LTD               0x0093   //San Shih Electrical Enterprise Co., Ltd.
#define MFG_ID_SANAV                                               0x012C   //SANAV
#define MFG_ID_SATCO_PRODUCTS_INC                                  0x0307   //SATCO Products, Inc. 
#define MFG_ID_SBCK_CORP                                           0x0318   //SBCK Corp. 
#define MFG_ID_SCIENTIA_TECHNOLOGIES_INC                           0x001F   //Scientia Technologies, Inc.
#define MFG_ID_SCOUT_ALARM                                         0x029A   //Scout Alarm
#define MFG_ID_SECURE_METERS_UK_LTD                                0x0059   //Secure Meters (UK) Ltd
#define MFG_ID_SECURE_WIRELESS                                     0x011E   //Secure Wireless
#define MFG_ID_SECURENET_TECHNOLOGIES                              0x0167   //SecureNet Technologies
#define MFG_ID_SECURIFI_LTD                                        0x0182   //Securifi Ltd.
#define MFG_ID_SELUXIT                                             0x0069   //Seluxit
#define MFG_ID_SENMATIC_AS                                         0x0043   //Senmatic A/S
#define MFG_ID_SENSATIVE_AB                                        0x019A   //Sensative AB
#define MFG_ID_SEQUOIA_TECHNOLOGY_LTD                              0x0044   //Sequoia Technology LTD
#define MFG_ID_SERCOMM_CORP                                        0x0151   //Sercomm Corp
#define MFG_ID_SHANDONG_BITTEL_INTELLIGENT_TECHNOLOGY_CO_LTD       0x0378   //Shandong Bittel Intelligent Technology Co., Ltd
#define MFG_ID_SHANDONG_SMART_LIFE_DATA_SYSTEM_CO_LTD              0x030B   //Shandong Smart Life Data System Co .LTD
#define MFG_ID_SHANGDONG_SMART_LIFE_DATA_SYSTEM_CO_LTD             0x0215   //Shangdong Smart Life Data System Co.,Ltd
#define MFG_ID_SHANGHAI_DORLINK_INTELLIGENT_TECHNOLOGIES_CO_LTD    0x023E   //Shanghai Dorlink Intelligent Technologies Co.,Ltd
#define MFG_ID_SHANGHAI_LONGCHUANG_ECOENERGY_SYSTEMS_CO_LTD        0x0205   //Shanghai Longchuang Eco-energy Systems Co., Ltd
#define MFG_ID_SHARP                                               0x010B   //Sharp
#define MFG_ID_SHENZHEN_3NOD_ACOUSTICLINK_CO_LTD                   0x0357   //Shenzhen 3nod Acousticlink Co., LTD
#define MFG_ID_SHENZHEN_AOYA_INDUSTRY_CO_LTD                       0x021A   //SHENZHEN AOYA INDUSTRY CO. LTD
#define MFG_ID_SHENZHEN_EASYHOME_TECHNOLOGY_CO_LTD                 0x0286   //Shenzhen Easyhome Technology Co., Ltd.
#define MFG_ID_SHENZHEN_ISURPASS_TECHNOLOGY_CO_LTD                 0x021C   //Shenzhen iSurpass Technology Co. ,Ltd
#define MFG_ID_SHENZHEN_JBT_SMART_LIGHTING_CO_LTD                  0x037A   //Shenzhen JBT Smart Lighting Co., Ltd
#define MFG_ID_SHENZHEN_KAADAS_INTELLIGENT_TECHNOLOGY_CO_LTD       0x021D   //Shenzhen Kaadas Intelligent Technology Co., Ltd
#define MFG_ID_SHENZHEN_LIAO_WANG_TONG_DA_TECHNOLOGY_LTD           0x0211   //Shenzhen Liao Wang Tong Da Technology Ltd
#define MFG_ID_SHENZHEN_NEO_ELECTRONICS_CO_LTD                     0x0258   //Shenzhen Neo Electronics Co., Ltd
#define MFG_ID_SHENZHEN_SEN5_TECHNOLOGY_CO_LTD                     0x036C   //Shenzhen Sen5 Technology Co., Ltd.
#define MFG_ID_SHENZHEN_THINGSVIEW_TECH                            0x0381   //Shenzhen Thingsview Tech
#define MFG_ID_SHENZHEN_TRIPATH_DIGITAL_AUDIO_EQUIPMENT_CO_LTD     0x0250   //Shenzhen Tripath Digital Audio Equipment Co.,Ltd
#define MFG_ID_SHENZHEN_ZHIQU_TECHNOLOGY_LIMITED                   0x0356   //Shenzhen ZHIQU Technology Limited
#define MFG_ID_SHENZHEN_HEIMAN_TECHNOLOGY_CO_LTD                   0x0260   //Shenzhen Heiman Technology Co., Ltd
#define MFG_ID_SHENZHEN_SAYKEY_TECHNOLOGY_CO_LTD                   0x032C   //Shenzhen Saykey Technology Co., Ltd 
#define MFG_ID_SIEGENIAAUBI_KG                                     0x0081   //SIEGENIA-AUBI KG
#define MFG_ID_ZWAVE                                               0x0000   //Z-Wave
#define MFG_ID_SIMONTECH_SLU                                       0x0267   //SimonTech S.L.U
#define MFG_ID_SINE_WIRELESS                                       0x0045   //Sine Wireless
#define MFG_ID_SITERWELL_TECHNOLOGY_HK_CO_LTD                      0x0266   //Siterwell Technology HK Co., LTD 
#define MFG_ID_SMART_ELECTRONIC_INDUSTRIAL_DONGGUAN_CO_LIMITED     0x0282   //Smart Electronic Industrial (Dongguan) Co., Limited
#define MFG_ID_SMART_PRODUCTS_INC                                  0x0046   //Smart Products, Inc.
#define MFG_ID_SMARTALL_INC                                        0x026A   //SmartAll Inc.
#define MFG_ID_SMARTHOME_PARTNER_GMBH                              0x0323   //SmartHome Partner GmbH
#define MFG_ID_SMARTLY_AS                                          0x024F   //Smartly AS
#define MFG_ID_SMARTTHINGS_INC                                     0x0150   //SmartThings, Inc.
#define MFG_ID_SMK_MANUFACTURING_INC                               0x0102   //SMK Manufacturing Inc.
#define MFG_ID_SOFTATHOME                                          0x029C   //SoftAtHome
#define MFG_ID_SOMFY                                               0x0047   //Somfy
#define MFG_ID_SONG_JIANG_YUNAN_TECHNOLOGY_CO_LTD                  0x0394   //SONG JIANG YUN-AN TECHNOLOGY CO., LTD.
#define MFG_ID_SOOSAN_HOMETECH                                     0x0274   //Soosan Hometech
#define MFG_ID_SOREL_GMBH                                          0x035A   //SOREL GmbH 
#define MFG_ID_SPECTRUM_BRANDS                                     0x0090   //Spectrum Brands
#define MFG_ID_SPRINGS_WINDOW_FASHIONS                             0x026E   //Springs Window Fashions
#define MFG_ID_SPRUE_SAFETY_PRODUCTS_LTD                           0x026F   //Sprue Safety Products Ltd
#define MFG_ID_SQUARE_CONNECT                                      0x0124   //Square Connect
#define MFG_ID_STT_ELECTRIC_CORPORATION                            0x021B   //ST&T Electric Corporation
#define MFG_ID_STAR_AUTOMATION                                     0x0358   //Star Automation
#define MFG_ID_STARKOFF                                            0x0259   //Starkoff
#define MFG_ID_STARVEDIA                                           0x0265   //StarVedia
#define MFG_ID_STEINEL_GMBH                                        0x0271   //STEINEL GmbH 
#define MFG_ID_STELPRO                                             0x0239   //Stelpro
#define MFG_ID_STRATTEC_ADVANCED_LOGIC_LLC                         0x0217   //Strattec Advanced Logic,LLC
#define MFG_ID_STRATTEC_SECURITY_CORPORATION                       0x0168   //STRATTEC Security Corporation
#define MFG_ID_SUMITOMO                                            0x0105   //Sumitomo
#define MFG_ID_SUNJET_COMPONENTS_CORP                              0x028B   //Sunjet Components Corp.
#define MFG_ID_SUPERNA                                             0x0054   //Superna
#define MFG_ID_SWANN_COMMUNICATIONS_PTY_LTD                        0x0191   //Swann Communications Pty Ltd
#define MFG_ID_SWYCS                                               0x0339   //SWYCS
#define MFG_ID_SYLVANIA                                            0x0009   //Sylvania
#define MFG_ID_SYSTECH_CORPORATION                                 0x0136   //Systech Corporation
#define MFG_ID_SYSTEMAIR_SVERIGE_AB                                0x0276   //Systemair Sverige AB
#define MFG_ID_TW_SHENZHEN_GONGJIN_ELECTRONICS_CO_LTD              0x0375   //T&W(SHENZHEN GONGJIN ELECTRONICS CO.,LTD)
#define MFG_ID_TAEWON_LIGHTING_CO_LTD                              0x0235   //TAEWON Lighting Co., Ltd.
#define MFG_ID_TAIWAN_FU_HSING_INDUSTRIAL_CO_LTD                   0x0262   //Taiwan Fu Hsing Industrial Co., Ltd.
#define MFG_ID_TAIWAN_ICATCH_INC                                   0x0264   //Taiwan iCATCH Inc.
#define MFG_ID_TEAM_DIGITAL_LIMITED                                0x0186   //Team Digital Limited
#define MFG_ID_TEAM_PRECISION_PCL                                  0x0089   //Team Precision PCL
#define MFG_ID_TECHNICOLOR                                         0x0240   //Technicolor
#define MFG_ID_TECHNIKU                                            0x000A   //Techniku
#define MFG_ID_TECOM_CO_LTD                                        0x012F   //Tecom Co., Ltd.
#define MFG_ID_TELL_IT_ONLINE                                      0x0012   //Tell It Online
#define MFG_ID_TELLDUS_TECHNOLOGIES_AB                             0x0176   //Telldus Technologies AB
#define MFG_ID_TELSEY                                              0x0048   //Telsey
#define MFG_ID_TELULAR                                             0x017E   //Telular
#define MFG_ID_TEPTRON_AB                                          0x0397   //Teptron AB 
#define MFG_ID_TERRA_OPTIMA_BV_PRIMAIR_SERVICES                    0x005C   //Terra Optima B.V. (tidligere Primair Services)
#define MFG_ID_THERE_CORPORATION                                   0x010C   //There Corporation
#define MFG_ID_THERMOFLOOR                                         0x019B   //ThermoFloor
#define MFG_ID_THINK_SIMPLE_SRL                                    0x0317   //Think Simple srl
#define MFG_ID_TIMEVALVE_INC                                       0x022A   //TIMEVALVE, Inc.
#define MFG_ID_TINGCORE_AB_INFO24_AB                               0x033B   //Tingcore AB  (Info24 AB)
#define MFG_ID_TKB_HOME                                            0x0118   //TKB Home
#define MFG_ID_TKH_GROUP_EMINENT                                   0x011C   //TKH Group / Eminent
#define MFG_ID_TMC_TECHNOLOGY_LTD                                  0x0327   //TMC Technology Ltd.
#define MFG_ID_TOLEDO_CO_INC                                       0x0319   //Toledo & Co., Inc.
#define MFG_ID_TONG_LUNG_METAL_INDUSTRY_CO_LTD                     0x034F   //TONG LUNG METAL INDUSTRY CO., LTD.
#define MFG_ID_TOSHIBA_VISUAL_SOLUTION                             0x0333   //Toshiba Visual Solution
#define MFG_ID_TPLINK_TECHNOLOGIES_CO_LTD                          0x0283   //TP-Link Technologies Co., Ltd.
#define MFG_ID_TRANE_CORPORATION                                   0x008B   //Trane Corporation
#define MFG_ID_TRICKLESTAR                                         0x0066   //TrickleStar
#define MFG_ID_TRICKLESTAR_LTD_EMPOWER_CONTROLS_LTD                0x006B   //Tricklestar Ltd. (former Empower Controls Ltd.)
#define MFG_ID_TRIDIUM                                             0x0055   //Tridium
#define MFG_ID_TRONICO_TECHNOLOGY_CO_LTD                           0x0111   //Tronico Technology Co. Ltd.
#define MFG_ID_TWISTHINK                                           0x0049   //Twisthink
#define MFG_ID_UBITECH                                             0x0270   //Ubitech
#define MFG_ID_UFAIRY_GR_TECH                                      0x0152   //UFairy G.R. Tech
#define MFG_ID_UNIVERSAL_DEVICES_INC                               0x0193   //Universal Devices, Inc
#define MFG_ID_UNIVERSAL_ELECTRONICS_INC                           0x0020   //Universal Electronics Inc.
#define MFG_ID_UNIVERSE_FUTURE                                     0x0183   //Universe Future
#define MFG_ID_UTC_FIRE_AND_SECURITY_AMERICAS_CORP                 0x0209   //UTC Fire and Security Americas Corp
#define MFG_ID_VATES                                               0x0388   //Vates
#define MFG_ID_VDA                                                 0x010A   //VDA
#define MFG_ID_VEMMIO                                              0x030F   //Vemmio
#define MFG_ID_VENSTAR_INC                                         0x0198   //Venstar Inc.
#define MFG_ID_VERA_CONTROL                                        0x008C   //Vera Control
#define MFG_ID_VERO_DUCO                                           0x0080   //Vero Duco
#define MFG_ID_VESTEL_ELEKTRONIK_TICARET_VE_SANAYI_AS              0x0237   //Vestel Elektronik Ticaret ve Sanayi A.S. 
#define MFG_ID_VIEWSONIC                                           0x0053   //Viewsonic
#define MFG_ID_VIEWSONIC_CORPORATION                               0x005E   //ViewSonic Corporation
#define MFG_ID_VIMAR_CRS                                           0x0007   //Vimar CRS
#define MFG_ID_VIPASTAR                                            0x0188   //Vipa-Star
#define MFG_ID_VISION_SECURITY                                     0x0109   //Vision Security
#define MFG_ID_VISUALIZE                                           0x004A   //Visualize
#define MFG_ID_VITELEC                                             0x0058   //Vitelec
#define MFG_ID_VIVA_LABS_AS                                        0x0263   //Viva Labs AS
#define MFG_ID_VIVINT                                              0x0156   //Vivint
#define MFG_ID_VSSAFETY_AS                                         0x017B   //Vs-Safety AS
#define MFG_ID_WATT_STOPPER                                        0x004B   //Watt Stopper
#define MFG_ID_WAYNE_DALTON                                        0x0008   //Wayne Dalton
#define MFG_ID_WEBEE_LIFE                                          0x019F   //Webee Life
#define MFG_ID_WEBEHOME_AB                                         0x0171   //WeBeHome AB
#define MFG_ID_WENZHOU_MTLC_ELECTRIC_APPLIANCES_CO_LTD             0x011A   //Wenzhou MTLC Electric Appliances Co.,Ltd.
#define MFG_ID_WESTCONTROL_AS                                      0x026C   //Westcontrol AS
#define MFG_ID_WHIRLPOOL                                           0x0057   //Whirlpool
#define MFG_ID_WHITE_RABBIT                                        0x027B   //White Rabbit
#define MFG_ID_WIDOM                                               0x0149   //wiDom
#define MFG_ID_WILLIS_ELECTRIC_CO_LTD                              0x015D   //Willis Electric Co., Ltd.
#define MFG_ID_WILSHINE_HOLDING_CO_LTD                             0x012D   //Wilshine Holding Co., Ltd
#define MFG_ID_WINK_INC                                            0x017F   //Wink Inc.
#define MFG_ID_WINTOP                                              0x0097   //Wintop
#define MFG_ID_WINYTECHNOLOGY                                      0x0242   //Winytechnology
#define MFG_ID_WIRELESS_MAINGATE_AB                                0x0199   //Wireless Maingate AB
#define MFG_ID_WOODWARD_LABS                                       0x004C   //Woodward Labs
#define MFG_ID_WOOREE_LIGHTING_CO_LTD                              0x0269   //WOOREE Lighting Co.,Ltd.
#define MFG_ID_WRAP                                                0x0003   //Wr@p
#define MFG_ID_WRT_INTELLIGENT_TECHNOLOGY_CO_LTD                   0x022F   //WRT Intelligent Technology CO., LTD.
#define MFG_ID_WUHAN_NWD_TECHNOLOGY_CO_LTD                         0x012E   //Wuhan NWD Technology Co., Ltd.
#define MFG_ID_XANBOO                                              0x004D   //Xanboo
#define MFG_ID_XIAMEN_ACTEC_ELECTRONICS_CO_LTD                     0x037D   //Xiamen AcTEC Electronics Co., Ltd.
#define MFG_ID_ZCONNECT                                            0x024E   //zConnect
#define MFG_ID_ZDATA_LLC                                           0x004E   //Zdata, LLC.
#define MFG_ID_ZHEJIANG_JIUXING_ELECTRIC_CO_LTD                    0x016F   //Zhejiang Jiuxing Electric Co Ltd
#define MFG_ID_ZINNO                                               0x036E   //Zinno
#define MFG_ID_ZIPATO                                              0x0131   //Zipato
#define MFG_ID_ZONOFF                                              0x0120   //Zonoff
#define MFG_ID_ZOOZ                                                0x027A   //Zooz
#define MFG_ID_ZWAVE_ALLIANCE                                      0x031D   //Z-Wave Alliance
#define MFG_ID_ZWAVE_TECHNOLOGIA                                   0x004F   //Z-Wave Technologia
#define MFG_ID_ZWAVEME                                             0x0115   //Z-Wave.Me
#define MFG_ID_ZWAVEPRODUCTSCOM                                    0x0315   //zwaveproducts.com
#define MFG_ID_ZWORKS_INC                                          0x024D   //Z-works Inc.
#define MFG_ID_ZYKRONIX                                            0x0021   //Zykronix
#define MFG_ID_ZYXEL                                               0x0135   //ZyXEL


/*************** command class identifiers ****************/
<!COMMAND_CLASS_DEF!>
 % Defines for command classes. One line for each define:
 %  #define COMMAND_CLASS_xxxx           0xHH /* comment */
 % Ex:
 %  #define COMMAND_CLASS_NO_OPERATION                      0x00
 %  #define COMMAND_CLASS_BASIC                             0x20
 %  #define COMMAND_CLASS_CONTROLLER_REPLICATION            0x21

/*************** command class extended identifiers ****************/
#define COMMAND_CLASS_SECURITY_SCHEME0_MARK                                              0xF100

/* Unknown command class commands */
#define UNKNOWN_VERSION                                                                  0x00

<!COMMAND_DEF!>
 % Defines for commands. This includes also necessary defines
 % for masks, shift, etc.:
 % ???? Naming of mask, shift, etc. defines
 %  /* xxxx command class commands */
 %  #define XXXX_VERSION   0xHH               (all command classes have a version)
 %  #define YYYY           0xHH /* comment */ (this is a command)
 %  /* Values used for command class xxxx */
 %  #define ZZZZ           0xHH /* comment */
 % Ex:
 %  /* Time command class commands */
 %  #define TIME_VERSION                                    0x01
 %  #define TIME_GET                                        0x01
 %  #define TIME_REPORT                                     0x02
 %  #define DATE_GET                                        0x03
 %  #define DATE_REPORT                                     0x04
 %  #define TIME_OFFSET_SET                                 0x05
 %  #define TIME_OFFSET_GET                                 0x06
 %  #define TIME_OFFSET_REPORT                              0x07
 % Ex:
 %  /* Clock command class commands */
 %  #define CLOCK_VERSION                                   0x01
 %  #define CLOCK_SET                                       0x04
 %  #define CLOCK_GET                                       0x05
 %  #define CLOCK_REPORT                                    0x06
 %  /* Values used for command class Clock */
 %  #define CLOCK_WEEKDAY_MASK                              0xE0
 %  #define CLOCK_WEEKDAY_SHIFT                             0x05
 %  #define CLOCK_HOUR_MASK                                 0x1F

/* Max. frame size to allow routing over 4 hops */
#define META_DATA_MAX_DATA_SIZE                      48

/* Max frame that can be transmitted */
#define TX_DATA_MAX_DATA_SIZE                        170

/************************************************************/
/* Structs and unions that can be used by the application   */
/* to construct the frames to be sent                       */
/************************************************************/

/************************************************************/
/* Common for all command classes                           */
/************************************************************/
typedef struct _ZW_COMMON_FRAME_
{
  uint8_t        cmdClass;  /* The command class */
  uint8_t        cmd;       /* The command */
} ZW_COMMON_FRAME;

<!COMMAND_STRUCTS!>
 % structs for commands:
 % ???? Naming of struct fields
 %  /************************************************************/
 %  /* xxxx command class structs                               */
 %  /************************************************************/
 %  typedef struct _ZW_XXXX_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %    uint8_t        yyyy;      /* comment */
 %  } ZW_XXXX_FRAME;
 % Ex:
 %  /************************************************************/
 %  /* Time command class structs                               */
 %  /************************************************************/
 %  typedef struct _ZW_TIME_GET_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %  } ZW_TIME_GET_FRAME;
 %  typedef struct _ZW_TIME_REPORT_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %    uint8_t        hourLocalTime;
 %    uint8_t        minuteLocalTime;
 %    uint8_t        secondLocalTime;
 %  } ZW_TIME_REPORT_FRAME;
 %  typedef struct _ZW_DATE_GET_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %  } ZW_DATE_GET_FRAME;
 %  typedef struct _ZW_DATE_REPORT_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %    uint8_t        year1;
 %    uint8_t        year2;
 %    uint8_t        month;
 %    uint8_t        day;
 %  } ZW_DATE_REPORT_FRAME;
 %  typedef struct _ZW_TIME_OFFSET_SET_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %    uint8_t        hourTZO;   /* Bit 7 = Sign of TZO */
 %    uint8_t        minuteTZO;
 %    uint8_t        minuteOffsetDST; /* Bit 7 = Sign of offset DST */
 %    uint8_t        monthStartDST;
 %    uint8_t        dayStartDST;
 %    uint8_t        hourStartDST;
 %    uint8_t        monthEndDST;
 %    uint8_t        dayEndDST;
 %    uint8_t        hourEndDST;
 %  } ZW_TIME_OFFSET_SET_FRAME;
 %  typedef struct _ZW_TIME_OFFSET_GET_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %  } ZW_TIME_OFFSET_GET_FRAME;
 %  typedef struct _ZW_TIME_OFFSET_REPORT_FRAME_
 %  {
 %    uint8_t        cmdClass;  /* The command class */
 %    uint8_t        cmd;       /* The command */
 %    uint8_t        hourTZO;   /* Bit 7 = Sign of TZO */
 %    uint8_t        minuteTZO;
 %    uint8_t        minuteOffsetDST; /* Bit 7 = Sign of offset DST */
 %    uint8_t        monthStartDST;
 %    uint8_t        dayStartDST;
 %    uint8_t        hourStartDST;
 %    uint8_t        monthEndDST;
 %    uint8_t        dayEndDST;
 %    uint8_t        hourEndDST;
 %  } ZW_TIME_OFFSET_REPORT_FRAME;


/**********************************************************************/
/* Command class structs use to encapsulating other commands          */
/* Do not define these commands in ZW_FRAME_COLLECTION_MACRO          */
/* Do not include commands defined in ZW_FRAME_COLLECTION_MACRO below */
/**********************************************************************/
typedef union _ALL_EXCEPT_ENCAP
{
  <!FRAME_MACRO_EX!>
} ALL_EXCEPT_ENCAP;

typedef struct _ZW_MULTI_COMMAND_ENCAP_FRAME_
{
  uint8_t        cmdClass;            /* The command class */
  uint8_t        cmd;                 /* The command */
  uint8_t        numberOfCommands;
  uint8_t        commandLength;
  ALL_EXCEPT_ENCAP     encapFrame;
} ZW_MULTI_COMMAND_ENCAP_FRAME;

typedef struct _ZW_COMPOSITE_CMD_ENCAP_FRAME_
{
  uint8_t        cmdClass;            /* The command class */
  uint8_t        cmd;                 /* The command */
  uint8_t        endPointMask1;       /* End point mask 1 */
  uint8_t        endPointMask2;       /* End point mask 2 */
  ALL_EXCEPT_ENCAP     encapFrame;
} ZW_COMPOSITE_CMD_ENCAP_FRAME;

typedef struct _ZW_COMPOSITE_REPLY_ENCAP_FRAME_
{
  uint8_t        cmdClass;            /* The command class */
  uint8_t        cmd;                 /* The command */
  uint8_t        endPoint;            /* Bit7-5=Reserved and Bit0-4=End Point */
  ALL_EXCEPT_ENCAP     encapFrame;
} ZW_COMPOSITE_REPLY_ENCAP_FRAME;

typedef struct _ZW_MULTI_INSTANCE_CMD_ENCAP_FRAME_
{
  uint8_t        cmdClass;            /* The command class */
  uint8_t        cmd;                 /* The command */
  uint8_t        instance;            /* The instance to access */
  ALL_EXCEPT_ENCAP     encapFrame;
} ZW_MULTI_INSTANCE_CMD_ENCAP_FRAME;

typedef struct _ZW_MULTI_CHANNEL_CMD_ENCAP_V2_FRAME_
{
    uint8_t      cmdClass;                     /* The command class */
    uint8_t      cmd;                          /* The command */
    uint8_t      properties1;                  /* masked byte */
    uint8_t      properties2;                  /* masked byte */
  ALL_EXCEPT_ENCAP     encapFrame;
} ZW_MULTI_CHANNEL_CMD_ENCAP_V2_FRAME;

typedef struct _ZW_SECURITY_MESSAGE_ENCAP_FRAME_
{
  uint8_t        cmdClass;            /* The command class */
  uint8_t        cmd;                 /* The command */
  uint8_t        initVectorByte1;     /* The initialization vector byte 1 (MSB) */
  uint8_t        initVectorByte2;     /* The initialization vector byte 2  */
  uint8_t        initVectorByte3;     /* The initialization vector byte 3 */
  uint8_t        initVectorByte4;     /* The initialization vector byte 4 */
  uint8_t        initVectorByte5;     /* The initialization vector byte 5 */
  uint8_t        initVectorByte6;     /* The initialization vector byte 6 */
  uint8_t        initVectorByte7;     /* The initialization vector byte 7 */
  uint8_t        initVectorByte8;     /* The initialization vector byte 8 (LSB) */
  uint8_t        securityEncapMessage[29];
  uint8_t        receiverNonceIdent;
  uint8_t        messageAuthenticationCodeByte1; /* The Authentication code byte 1 (MSB) */
  uint8_t        messageAuthenticationCodeByte2; /* The Authentication code byte 2 */
  uint8_t        messageAuthenticationCodeByte3; /* The Authentication code byte 3 */
  uint8_t        messageAuthenticationCodeByte4; /* The Authentication code byte 4 */
  uint8_t        messageAuthenticationCodeByte5; /* The Authentication code byte 5 */
  uint8_t        messageAuthenticationCodeByte6; /* The Authentication code byte 6 */
  uint8_t        messageAuthenticationCodeByte7; /* The Authentication code byte 7 */
  uint8_t        messageAuthenticationCodeByte8; /* The Authentication code byte 8 (LSB) */
} ZW_SECURITY_MESSAGE_ENCAP_FRAME;

/************************************************************/
/* Union of all command classes                             */
/************************************************************/
typedef union _ZW_APPLICATION_TX_BUFFER_
{
  ZW_MULTI_COMMAND_ENCAP_FRAME           ZW_MultiCommandEncapFrame;
  ZW_COMPOSITE_CMD_ENCAP_FRAME           ZW_CompositeCmdEncapFrame;
  ZW_COMPOSITE_REPLY_ENCAP_FRAME         ZW_CompositeReplyEncapFrame;
  ZW_MULTI_INSTANCE_CMD_ENCAP_FRAME      ZW_MultiInstanceCmdEncapFrame;
  ZW_MULTI_CHANNEL_CMD_ENCAP_V2_FRAME    ZW_MultiChannelCmdEncapV2Frame;
  <!FRAME_MACRO_EX!>
  uint8_t                                bPadding[TX_DATA_MAX_DATA_SIZE];
} ZW_APPLICATION_TX_BUFFER;

/************************************************************/
/* Union of all command classes with room for a full        */
/* meta data frame                                          */
/************************************************************/
typedef union _ZW_APPLICATION_META_TX_BUFFER_
{
  ZW_MULTI_COMMAND_ENCAP_FRAME           ZW_MultiCommandEncapFrame;
  ZW_COMPOSITE_CMD_ENCAP_FRAME           ZW_CompositeCmdEncapFrame;
  ZW_COMPOSITE_REPLY_ENCAP_FRAME         ZW_CompositeReplyEncapFrame;
  ZW_MULTI_INSTANCE_CMD_ENCAP_FRAME      ZW_MultiInstanceCmdEncapFrame;
  ZW_MULTI_CHANNEL_CMD_ENCAP_V2_FRAME    ZW_MultiChannelCmdEncapV2Frame;
  <!FRAME_MACRO_EX!>
  uint8_t                                bPadding[META_DATA_MAX_DATA_SIZE];
} ZW_APPLICATION_META_TX_BUFFER;


#endif