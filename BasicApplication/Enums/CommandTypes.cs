/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    /// <summary>
    /// Command Types enumeration.
    /// </summary>
    public enum CommandTypes : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Command to initialize Zerial API data.
        /// </summary>
        CmdSerialApiGetInitData = 0x02,
        /// <summary>
        /// Command to get LR Nodes.
        /// </summary>
        CmdSerialApiGetLRNodes = 0xDA,
        /// <summary>
        /// Command to generate the Node Information frame and to save information about Node capabilities.
        /// </summary>
        CmdSerialApiApplNodeInformation = 0x03,
        CmdSerialApiApplNodeInformationCmdClasses = 0x0C,
        /// <summary>
        /// Command to handle Z-Wave protocol ApplicationCommandHandler function.
        /// </summary>
        CmdApplicationCommandHandler = 0x04,
        /// <summary>
        /// Command to handle Z-Wave protocol ApplicationCommandHandler function.
        /// </summary>
        CmdApplicationCommandHandlerPMode = 0xD1,
        /// <summary>
        /// Command to get bitmask containing the capabilities of the controller.
        /// </summary>
        CmdZWaveGetControllerCapabilities = 0x05,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdSerialApiSetTimeouts = 0x06,
        /// <summary>
        /// Command to get Serial API capabilities.
        /// </summary>
        CmdSerialApiGetCapabilities = 0x07,
        /// <summary>
        /// Command to make the Z-Wave module do a software reset.
        /// </summary>
        CmdSerialApiSoftReset = 0x08,
        /// <summary>
        /// Command to power down the RF when not in use e.g. expects nothing to be received.
        /// </summary>
        CmdZWaveSetRFReceiveMode = 0x10,
        /// <summary>
        /// Command to set the Z-Wave module's CPU in sleep mode until woken by an interrupt.
        /// </summary>
        CmdZWaveSetSleepMode = 0x11,
        CmdZWaveSetWutTimeout = 0xB4,
        /// <summary>
        /// Command to create and transmit a "Node Information" frame.
        /// </summary>
        CmdZWaveSendNodeInformation = 0x12,
        /// <summary>
        /// Command to transmit the data buffer to a single Z-Wave Node or all Z-Wave Nodes (broadcast).
        /// </summary>
        CmdZWaveSendData = 0x13,
        /// <summary>
        /// Command to transmit the data buffer to a single Z-Wave Node. SecurityS2.
        /// </summary>
        CmdZWaveSendDataEx = 0x0E,
        /// <summary>
        /// Command to transmit the data buffer to a single Z-Wave Node or all Z-Wave Nodes (broadcast).
        /// </summary>
        CmdZWaveSendData_Bridge = 0xA9,
        /// <summary>
        /// Command to transmit the data buffer to a single Z-Wave Node or all Z-Wave Nodes (broadcast).
        /// </summary>
        CmdZWaveSendDataMeta_Bridge = 0xAA,
        /// <summary>
        /// Command to transmit the data buffer to a list of Z-Wave Nodes (multicast frame).
        /// <br/>NOTE: This function is implemented in Z-Wave Controller APIs, Z-Wave Routing End Device API and Z-Wave Enhanced End Device API only.
        /// </summary>
        CmdZWaveSendDataMulti = 0x14,
        /// <summary>
        /// Command to transmit the data buffer to a list of Z-Wave Nodes (multicast frame).
        /// </summary>
        CmdZWaveSendDataMulti_Bridge = 0xAB,
        /// <summary>
        /// Command to get the Z-Wave basis API library version.
        /// </summary>
        CmdZWaveGetVersion = 0x15,
        /// <summary>
        /// Command to abort the ongoing transmit started with <see cref="CmdZWaveSendData"/> or <see cref="CmdZWaveSendDataMulti"/>.
        /// </summary>
        CmdZWaveSendDataAbort = 0x16,
        /// <summary>
        /// Command to set the power level used in RF transmitting.
        /// </summary>
        CmdZWaveRFPowerLevelSet = 0x17,
        /// <summary>
        /// Get the current power level used in RF transmitting.
        /// </summary>
        CmdZWaveRFPowerLevelGet = 0xBA,
        /// <summary>
        /// Set the power level locally in the node when finding neighbors
        /// </summary>
        CmdZWaveRFPowerlevelRediscoverySet = 0x1E,
        /// <summary>
        /// Command to transmit streaming or bulk data in the Z-Wave network.
        /// <br/>NOTE: This function is only available in Z-Wave 0201 libraries.
        /// </summary>
        CmdZWaveSendDataMeta = 0x18,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveSendDataMR = 0x19,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveSendDataMetaMR = 0x1A,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveSetRoutingInfo = 0x1B,
        /// <summary>
        /// Command to set the maxiomum number of source routing attempts before the next mechanism kick-in
        /// </summary>
        CmdZWaveSetRoutingMAX = 0xD4,
        /// <summary>
        /// Send Test Frame Command.
        /// </summary>
        CmdZWaveSendTestFrame = 0xBE,
        /// <summary>
        /// Command to copy the Home Id and Node Id from the non-volatile memory to the specified RAM addresses.
        /// </summary>
        CmdMemoryGetId = 0x20,
        /// <summary>
        /// Command to read one byte from the non-volatile memory.
        /// </summary>
        CmdMemoryGetByte = 0x21,
        /// <summary>
        /// Command to write one byte to the application area of the non-volatile memory.
        /// </summary>
        CmdMemoryPutByte = 0x22,
        /// <summary>
        /// Command to read a number of bytes from the application area of the EEPROM to a RAM buffer.
        /// </summary>
        CmdMemoryGetBuffer = 0x23,
        /// <summary>
        /// Command to write a number of bytes from the application area of the EEPROM to a RAM buffer.
        /// </summary>
        CmdMemoryPutBuffer = 0x24,
        /// <summary>
        /// Command to write the specified time to the current Real Time Clock.
        /// </summary>
        CmdClockSet = 0x30,
        /// <summary>
        /// Command to copy the current Real Time Clock time to the specified time buffer.
        /// </summary>
        CmdClockGet = 0x31,
        /// <summary>
        /// Command to compare a specified time against the current Real Time Clock time.
        /// </summary>
        CmdClockCompare = 0x32,
        /// <summary>
        /// Command to create a new timer element.
        /// </summary>
        CmdRtcTimerCreate = 0x33,
        /// <summary>
        /// Command to Search in the RTC list and if a used element found, copy the timer element from the RTC timer list to the specified memory buffer.
        /// </summary>
        CmdRtcTimerRead = 0x34,
        /// <summary>
        /// Command to remove the specified timer element from the RTC timer list.
        /// </summary>
        CmdRtcTimerDelete = 0x35,
        /// <summary>
        /// Commad to execute callback function if it specified in <see cref="CmdRtcTimerCreate"/>.
        /// </summary>
        CmdRtcTimerCall = 0x36,
        /// <summary>
        /// Command to get the Node Information frame without command classes from the EEPROM memory.
        /// </summary>
        CmdZWaveGetNodeProtocolInfo = 0x41,
        /// <summary>
        /// Command to remove all Nodes, routing information, assigned homeID/nodeID and RTC timers from the EEPROM memory. This function set the Controller back to the factory default state.
        /// </summary>
        CmdZWaveSetDefault = 0x42,
        /// <summary>
        /// Command to send command completed to sending controller.
        /// </summary>
        CmdZWaveReplicationCommandComplete = 0x44,
        /// <summary>
        /// Command to send the payload and expects the receiver to respond with a command complete message.
        /// </summary>
        CmdZWaveReplicationSendData = 0x45,
        /// <summary>
        /// Command to assign static return routes (up to 4) to a Routing End Device node or Enhanced End Device node.
        /// </summary>
        CmdZWaveAssignReturnRoute = 0x46,
        /// <summary>
        /// Command to delete all static return routes from a Routing End Device node or Enhanced End Device node.
        /// </summary>
        CmdZWaveDeleteReturnRoute = 0x47,
        /// <summary>
        /// Command to get the neighbors from the specified node.
        /// </summary>
        CmdZWaveRequestNodeNeighborUpdate = 0x48,
        /// <summary>
        /// Command to handle "Node Information" frame. See <see cref="CmdZWaveSendNodeInformation"/>.
        /// </summary>
        CmdApplicationControllerUpdate = 0x49,
        /// <summary>
        /// Command to add any nodes to the Z-Wave network.
        /// </summary>
        CmdZWaveAddNodeToNetwork = 0x4a,
        /// <summary>
        /// Command to remove any nodes from the Z-Wave network.
        /// </summary>
        CmdZWaveRemoveNodeFromNetwork = 0x4b,
        /// <summary>
        /// Command to remove specific node from the Z-Wave network.
        /// </summary>
        CmdZWaveRemoveNodeIdFromNetwork = 0x3f,
        /// <summary>
        /// Command to add a controller to the Z-Wave network as a replacement for the old primary controller.
        /// </summary>
        CmdZWaveCreateNewPrimary = 0x4c,
        /// <summary>
        /// Command to add a controller to the Z-Wave network and transfer the role as primary controller to it.
        /// </summary>
        CmdZWaveControllerChange = 0x4d,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveRequestNodeNeighborUpdateMR = 0x4E,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveAssignReturnRouteMR = 0x4F,
        /// <summary>
        /// Command to instruct the controller to allow it to be added or removed from the network.
        /// </summary>
        CmdZWaveSetLearnMode = 0x50,
        /// <summary>
        /// Command to assign the return routes of the SUC node from a Routing End Device node or Enhanced End Device node.
        /// </summary>
        CmdZWaveAssignSucReturnRoute = 0x51,
        /// <summary>
        /// Command to enable/disable assignment of the SUC/SIS functionality in the controller.
        /// </summary>
        CmdZWaveEnableSuc = 0x52,
        /// <summary>
        /// Command to request network topology updates from the SUC/SIS node.
        /// </summary>
        CmdZWaveRequestNetworkUpdate = 0x53,
        /// <summary>
        /// Command to configure a static/bridge controller to be a SUC/SIS node or not.
        /// </summary>
        CmdZWaveSetSucNodeId = 0x54,
        /// <summary>
        /// Command to delete the return routes of the SUC node from a Routing End Device node or Enhanced End Device node.
        /// </summary>
        CmdZWaveDeleteSucReturnRoute = 0x55,
        /// <summary>
        /// Command to get the currently registered SUC node ID.
        /// </summary>
        CmdZWaveGetSucNodeId = 0x56,
        /// <summary>
        /// Command to transmit SUC node Id from a primary controller or static controller to the controller node Id specified.
        /// </summary>
        CmdZWaveSendSucId = 0x57,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        /* CmdZWaveAssignSucReturnRouteMR = 0x111, // Obsoleted
        /// <summary>
        /// Command to handle rediscovery needed call.
        /// </summary> */
        CmdZWaveAssignPrioritySucReturnRoute = 0x58,
        /// <summary>
        /// Command to assign the specified Priority SUC Return Route and also assign up to three return routes calculated from the routing table.
        /// </summary>
        CmdZWaveAssignPriorityReturnRoute = 0x4F,
        /// <summary>
        /// Command to assign the specified Priority Return Route and also assign up to three return routes calculated from the routing table.
        /// </summary>
        CmdZWaveRediscoveryNeeded = 0x96, // Obsoleted
        /// <summary>
        /// Command to request the node information frame from a controller based node in the network.
        /// </summary>
        CmdZWaveRequestNodeInfo = 0x60,
        /// <summary>
        /// Command to remove a non-responding node from the routing table in the requesting controller.
        /// </summary>
        CmdZWaveRemoveFailedNodeId = 0x61,
        /// <summary>
        /// Command to test if a node ID is stored in the failed node ID list.
        /// </summary>
        CmdZWaveIsFailedNode = 0x62,
        /// <summary>
        /// Command to replace a non-responding node with a new one in the requesting controller.
        /// </summary>
        CmdZWaveReplaceFailedNode = 0x63,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdTimerStart = 0x70,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdTimerRestart = 0x71,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdTimerCancel = 0x72,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdTimerCall = 0x73,
        /// <summary>
        /// Command to read out neighbor information from the protocol.
        /// </summary>
        CmdGetRoutingTableLine = 0x80,
        /// <summary>
        /// Command to get a variable that returns the number of transmits that the protocol has done since last reset of the variable.
        /// </summary>
        CmdGetTXCounter = 0x81,
        /// <summary>
        /// Command to reset a variable that returns the number of transmits 
        /// </summary>
        CmdResetTXCounter = 0x82,
        /// <summary>
        /// Command to restore protocol node information from a backup or the like.
        /// </summary>
        CmdStoreNodeInfo = 0x83,
        /// <summary>
        /// Command to restore HomeID and NodeID information from a backup.
        /// </summary>
        CmdStoreHomeId = 0x84,
        /// <summary>
        /// Command to assure that the response route don’t disappear when transmitting a sequence of frames to the same node Id.
        /// </summary>
        CmdLockRouteResponse = 0x90,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveSendDataRouteDemo = 0x91,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdSerialApiTest = 0x95,
        /// <summary>
        /// Command to request application Virtual Device Node information.
        /// </summary>
        CmdSerialApiVirtualDeviceNodeInfo = 0xa0,
        /// <summary>
        /// Command to handle Z-Wave protocol ApplicationEndDeviceCommandHandler function.
        /// </summary>
        CmdApplicationVirtualDeviceCommandHandler = 0xa1,
        /// <summary>
        /// Command to create and transmit a Virtual Device node "Node Information" frame from source Virtual End Device node.
        /// </summary>
        CmdZWaveSendVirtualDeviceNodeInfo = 0xa2,
        /// <summary>
        /// Command to transmit the data buffer to a single Z-Wave Node or all Z-Wave Nodes (NODE_BROADCAST) from the Virtual Device node.
        /// </summary>
        CmdZWaveSendVirtualDeviceData = 0xa3,
        /// <summary>
        /// Command to enables the possibility for enabling or disabling “End Device Learn Mode”, which when enabled makes it possible for other controllers (primary or inclusion controllers) to add or remove a Virtual Device Node to the Z-Wave network.
        /// </summary>
        CmdZWaveSetVirtualDeviceLearnMode = 0xa4,
        /// <summary>
        /// Command to request a buffer containing available Virtual Device nodes in the Z-Wave network.
        /// </summary>
        CmdZWaveGetVirtualDeviceNodes = 0xa5,
        /// <summary>
        /// Command to check if node is a Virtual Device node.
        /// </summary>
        CmdZWaveIsVirtualDeviceNode = 0xa6,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveReservedSSD = 0xA7,
        /// <summary>
        /// Command to handle Z-Wave protocol ApplicationCommandHandler_Bridge function.
        /// </summary>
        CmdApplicationCommandHandler_Bridge = 0xA8,
        /// <summary>
        /// Reserved Command.
        /// </summary>
        CmdZWaveGetRandom = 0x1c,
        /// <summary>
        /// Command to enable/disable the promiscuous mode.
        /// </summary>
        CmdZWaveSetPromiscuousMode = 0xd0,
        /// <summary>
        /// This function sends out an explorer frame requesting inclusion into a network
        /// </summary>
        CmdZWaveExploreRequestInclusion = 0x5E,
        /// <summary>
        /// This function sends out an explorer frame requesting inclusion into a network
        /// </summary>
        CmdZWaveExploreRequestExclusion = 0x5F,
        /// <summary>
        /// This function returns a mask telling which protocol function is currently running
        /// </summary>
        CmdZWaveGetProtocolStatus = 0xBF,
        /// <summary>
        /// A pseudo-random number generator that generates a sequence of numbers, the elements of which are
        /// approximately independent of each other
        /// </summary>
        CmdZWaveRandom = 0x1D,
        /// <summary>
        /// Set the trigger level for external interrupt 0 or 1.
        /// </summary>
        CmdZWaveSetExtIntLevel = 0xB9,
        /// <summary>
        /// Get the Z-Wave library type.
        /// </summary>
        CmdZWaveTypeLibrary = 0xBD,
        /// <summary>
        /// Enables the 400 Series built-in watchdog.
        /// </summary>
        CmdZWaveWatchDogEnable = 0xB6,
        /// <summary>
        /// Disable the 400 Series built in watchdog.
        /// </summary>
        CmdZWaveWatchDogDisable = 0xB7,
        /// <summary>
        /// Kicks the 400 Series built in watchdog.
        /// </summary>
        CmdZWaveWatchDogKick = 0xB8,
        CmdZWaveWatchDogStart = 0xD2,
        CmdZWaveWatchDogStop = 0xD3,
        /// <summary>
        /// Used check if two nodes are marked as being within direct range of each other
        /// </summary>
        CmdAreNodesNeighbours = 0xBC,
        /// <summary>
        /// Used to get the number of neighbors the specified node has registered.
        /// </summary>
        CmdZWaveGetNeighborCount = 0xBB,
        /// <summary>
        /// Check if the supplied nodeID is marked as being within direct range in any of the existing return routes
        /// </summary>
        CmdZWaveIsNodeWithinDirectRange = 0x5D,
        /// <summary>
        /// Used to request new return route destinations from the SUC/SIS node
        /// </summary>
        CmdZWaveRequestNewRouteDestinations = 0x5C,
        /// <summary>
        /// Command to Firmware Update Locally.
        /// </summary>
        CmdZWaveFirmwareUpdateNvm = 0x78,
        /// <summary>
        /// Command to NVM Backup/Restore.
        /// </summary>
        CmdZWaveNVMBackupRestore = 0x2E,

        /// <summary>
        /// /* Allocated for NUNIT test */
        ///FUNC_ID_ZW_NUNIT_CMD                            0xE0
        ///FUNC_ID_ZW_NUNIT_INIT                           0xE1
        ///FUNC_ID_ZW_NUNIT_LIST                           0xE2
        ///FUNC_ID_ZW_NUNIT_RUN                            0xE3
        ///FUNC_ID_ZW_NUNIT_END                            0xE4
        ///FUNC_ID_IO_PORT_STATUS                          0xE5
        ///FUNC_ID_IO_PORT                                 0xE6
        /// </summary>
        CmdZWaveNUnitCmd = 0xE0,
        CmdZWaveNUnitInit = 0xE1,
        CmdZWaveNUnitList = 0xE2,
        CmdZWaveNUnitRun = 0xE3,
        CmdZWaveNUnitEnd = 0xE4,

        CmdZWaveIoPortStatus = 0xE5,
        CmdZWaveIoPort = 0xE6,
        /// <summary>
        /// Used to get Setup Serial API for a selected device
        /// </summary>
        CmdSerialApiSetup = 0x0B,
        /// <summary>
        /// Used to get Setup Serial API for a selected device
        /// </summary>
        CmdSerialApiTestSetup = 0xF0,
        /// <summary>
        /// ZW_GetPriorityRoute superceeds the ZW_GetLastWorkingRoute functionality, which is obsoleted
        /// </summary>
        CmdZWaveGetPriorityRoute = 0x92,
        /// <summary>
        /// CmdZWaveSetPriorityRoute superceeds the ZW_SetLastWorkingRoute functionality, which is obsoleted
        /// </summary>
        CmdZWaveSetPriorityRoute = 0x93,
        /// <summary>
        /// ZW_SECURITY_SETUP  function returns a bitmask of security keys the node posses. 
        /// </summary>
        CmdZWaveSecuritySetup = 0x9C,
        /* Allocated for proprietary serial API commands TestInterface*/
        CmdTestInterface = 0xEF,
        /// <summary>
        ///ZW_NVRGetValue read a value from the NVR Flash memory area.
        /// </summary>
        CmdZWaveNVRGetValue = 0x28,
        CmdSetListenBeforeTalkThreshold = 0x3C,
        CmdSendDataMultiEx = 0x0F,
        CmdDebugOutput = 0x26,
        /// <summary>
        /// This function can be used to set the maximum interval between SmartStart inclusion requests.
        /// </summary>
        CmdZWaveSetMaxInclusionRequestIntervals = 0xD6,
        /// <summary>
        /// 
        /// </summary>
        CmdPowerMgmtStayAwake = 0xD7,
        /// <summary>
        /// 
        /// </summary>
        CmdPowerMgmtCancel = 0xD8,
        /// <summary>
        /// This function gets the protocols internal tx timer for the specified channel
        /// </summary>
        CmdGetTxTimer = 0x38,
        /// <summary>
        /// This function clears the protocols internal tx timers
        /// </summary>
        CmdClearTxTimer = 0x37,
        /// <summary>
        /// This function clears the current Network Statistics collected by the Z-Wave protocol
        /// </summary>
        CmdClearNetworkStats = 0x39,
        /// <summary>
        /// This function retrieves the current Network Statistics as collected by the Z-Wave protocol
        /// </summary>
        CmdGetNetworkStats = 0x3A,
        /// <summary>
        /// Used to get Background RSSI for a selected device
        /// </summary>
        CmdGetBackgroundRSSI = 0x3B,
        /// <summary>
        /// Initiate graceful shutdown. The API will stops the radio and cancel all timers and power locks.
        /// </summary>
        CmdZWaveInitiateShutdown = 0xD9,
        /// <summary>
        /// Command to get the active Long Range rf-channel.  Introduced in Serial API version 9.
        /// </summary>
        CmdGetLRChannel = 0xDB,
        /// <summary>
        /// Command to set the active Long Range rf-channel.  Introduced in Serial API version 9.
        /// </summary>
        CmdSetLRChannel = 0xDC,
        /// <summary>
        /// Command to get current configuration of the DCDC.  Introduced in Serial API version 9.
        /// </summary>
        CmdGetDcdcMode = 0xDE,
        /// <summary>
        /// Command to set current configuration of the DCDC.  Introduced in Serial API version 9.
        /// </summary>
        CmdSetDcdcMode = 0xDF,
        /// <summary>
        /// Command to Enable/Disable PTI support
        /// </summary>
        CmdEnableRadioPTI = 0xE7,
        /// <summary>
        /// Command to get is enabled Radio PTI state
        /// </summary>
        CmdGetRadioPTI = 0xE8,
        /// <summary>
        /// Command to get the neighbors from the specified node.
        /// An alternative for CmdZWaveRequestNodeNeighborUpdate.
        /// </summary>
        CmdZWaveRequestNodeNeighborDiscovery = 0x68,
    }
}
