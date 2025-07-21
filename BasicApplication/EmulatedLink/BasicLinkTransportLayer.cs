/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utils;
using Utils.Events;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;

namespace ZWave.BasicApplication.EmulatedLink
{
    public class BasicLinkTransportLayer : ITransportLayer
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput { get; set; }

        const byte rssiVal = 0x7e; // RSSI_MAX_POWER_SATURATED
        ConcurrentDictionary<ushort, BlockingCollection<byte[]>> _pool = new ConcurrentDictionary<ushort, BlockingCollection<byte[]>>();
        Dictionary<ushort, BasicLinkModuleMemory> _modules = new Dictionary<ushort, BasicLinkModuleMemory>();
        Dictionary<ushort, SendDataDelayedInfo> _sendDataDelays = new Dictionary<ushort, SendDataDelayedInfo>();

        private const int MAX_FRAME_SIZE = 144;
        public int? MaxFrameSize { get; set; }
        private int ActualMaxFrameSize => MaxFrameSize ?? MAX_FRAME_SIZE;

        public ITransportClient CreateClient(ushort sessionId)
        {
            BasicLinkTransportClient ret = new BasicLinkTransportClient(dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)),
                WriteData, ReadData, UnregisterModule)
            {
                SuppressDebugOutput = SuppressDebugOutput,
                SessionId = sessionId
            };
            RegisterModule(sessionId, new BasicLinkModuleMemory(0x01));
            return ret;
        }

        private void UnregisterModule(ushort sessionId)
        {
            _modules.Remove(sessionId);
            if (_pool.TryRemove(sessionId, out BlockingCollection<byte[]> value))
            {
                value.Dispose();
            }
        }

        private void RegisterModule(ushort sessionId, BasicLinkModuleMemory module)
        {
            _modules.Add(sessionId, module);
            _pool.TryAdd(sessionId, new BlockingCollection<byte[]>());
        }

        public void SetUpModulesNetwork(ushort sessionId, params ushort[] joinSessionIds)
        {
            foreach (var joinSessionId in joinSessionIds)
            {
                _modules[joinSessionId].NodeId = _modules[sessionId].SeedNextNodeId();
                _modules[joinSessionId].HomeId = _modules[sessionId].HomeId;
                _modules[joinSessionId].NodesList.Add(_modules[sessionId].NodeId);
                _modules[sessionId].NodesList.Add(_modules[joinSessionId].NodeId);
            }
        }

        public void SetNifResponseEnabled(ushort sessionId, bool value)
        {
            _modules[sessionId].IsNifResponseEnabled = value;
        }


        public void AddSendDataDelay(ushort sessionId, int framesCount, int delayMs)
        {
            if (_sendDataDelays.ContainsKey(sessionId))
                _sendDataDelays[sessionId] = new SendDataDelayedInfo { FramesCount = framesCount, DelayMs = delayMs };
            else
                _sendDataDelays.Add(sessionId, new SendDataDelayedInfo { FramesCount = framesCount, DelayMs = delayMs });
        }

        public void RemoveSendDataDelay(ushort sessionId, int framesCount, int delayMs)
        {
            if (_sendDataDelays.ContainsKey(sessionId))
                _sendDataDelays.Remove(sessionId);
        }

        private void CheckSendDataDelay(ushort sessionId)
        {
            if (_sendDataDelays.ContainsKey(sessionId) && _sendDataDelays[sessionId].FramesCount > 0)
            {
                System.Threading.Tasks.Task.Delay(_sendDataDelays[sessionId].DelayMs).Wait();
                _sendDataDelays[sessionId].FramesCount--;
            }
        }


        #region Helpers

        private byte[] ComposeAddNodeClientResponse(byte status, byte funcId, byte nodeToIncludeId, byte[] nodeSupportedCC, CommandTypes commandType)
        {
            var supportedCC = nodeSupportedCC ?? new byte[0];
            byte bLen = (byte)(3 /*basic | generic | specific length*/ + supportedCC.Length);
            return CreateFrame(new byte[]
            {
                (byte)FrameTypes.Request,
                (byte)commandType,
                funcId,
                status,
                nodeToIncludeId,
                bLen,
                1, 2, 1 // basic | generic | specific
            }.Concat(supportedCC));
        }

        private byte[] CreateFrame(IEnumerable<byte> data)
        {
            byte[] ret = new byte[data.Count() + 3];
            var index = 0;
            ret[index++] = (byte)HeaderTypes.Sof;
            ret[index++] = (byte)(data.Count() + 1);
            foreach (var b in data)
            {
                ret[index++] = b;
            }
            ret[index] = CalculateChecksum(data);
            return ret;
        }

        private byte CalculateChecksum(IEnumerable<byte> data)
        {
            byte calcChksum = 0xFF;
            calcChksum ^= (byte)(data.Count() + 1); // Length
            foreach (var b in data)
            {
                calcChksum ^= b; // Data
            }
            return calcChksum;
        }

        private byte[] GetFuncIdSupportedBitmask(ushort sessionId)
        {
            byte[] ret = new byte[32];
            var sapList = new List<CommandTypes>(_supported);
            sapList.Sort();
            foreach (var item in sapList)
            {
                var val = (byte)item - 1; // started from 1
                var index = val / 8;
                var bit = val % 8;
                ret[index] = (byte)(ret[index] ^ (1 << bit));
            }
            return ret;
        }

        #endregion

        #region Direct Link

        private int ReadData(ushort sessionId, byte[] buffer, CancellationToken ctoken)
        {
            int ret = 0;
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                var data = value.Take(ctoken);
                Array.Copy(data, buffer, data.Length);
                ret = data.Length;
            }
            return ret;
        }

        private readonly CommandTypes[] _supported = new[]
       {
            CommandTypes.CmdSerialApiGetCapabilities,
            CommandTypes.CmdZWaveAddNodeToNetwork,
            CommandTypes.CmdZWaveSetLearnMode,
            CommandTypes.CmdZWaveSendData,
            CommandTypes.CmdZWaveSendDataAbort,
            CommandTypes.CmdMemoryGetId,
            CommandTypes.CmdSerialApiGetInitData,
            CommandTypes.CmdSerialApiApplNodeInformation,
            CommandTypes.CmdZWaveRequestNodeInfo,
            CommandTypes.CmdZWaveGetSucNodeId,
            CommandTypes.CmdZWaveSendDataMulti,
            CommandTypes.CmdZWaveSetDefault,
            CommandTypes.CmdZWaveSetRFReceiveMode,
            CommandTypes.CmdZWaveGetVersion,
            CommandTypes.CmdZWaveNVRGetValue,
            CommandTypes.CmdSerialApiSoftReset,
            CommandTypes.CmdZWaveRemoveNodeFromNetwork,
            CommandTypes.CmdSerialApiSetup,
            CommandTypes.CmdZWaveSetSucNodeId,
            CommandTypes.CmdZWaveIsFailedNode,
            CommandTypes.CmdZWaveRequestNodeNeighborUpdate
        };

        private int WriteData(ushort sessionId, byte[] data)
        {
            if (data != null && data.Length > 1)
            {
                if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
                {
                    value.Add(new byte[] { 0x06 });
                    if (data[2] == (byte)FrameTypes.Request)
                    {
                        switch ((CommandTypes)data[3])
                        {
                            case CommandTypes.CmdSerialApiGetCapabilities:
                                CmdSerialApiGetCapabilities(sessionId);
                                break;
                            case CommandTypes.CmdZWaveAddNodeToNetwork:
                                CmdZWaveAddNodeToNetwork(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSetLearnMode:
                                CmdZWaveSetLearnMode(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSendData:
                                CheckSendDataDelay(sessionId);
                                CmdZWaveSendData(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSendDataAbort:
                                CmdZWaveSendDataAbort(sessionId);
                                break;
                            case CommandTypes.CmdMemoryGetId:
                                CmdMemoryGetId(sessionId);
                                break;
                            case CommandTypes.CmdSerialApiGetInitData:
                                CmdSerialApiGetInitData(sessionId, data);
                                break;
                            case CommandTypes.CmdSerialApiApplNodeInformation:
                                CmdSerialApiApplNodeInformation(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveRequestNodeInfo:
                                CmdZWaveRequestNodeInfo(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveGetSucNodeId:
                                CmdZWaveGetSucNodeId(sessionId);
                                break;
                            case CommandTypes.CmdZWaveSendDataMulti:
                                CmdZWaveSendDataMulti(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSetDefault:
                                CmdZWaveSetDefault(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSetRFReceiveMode:
                                CmdZWaveSetRFReceiveMode(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveGetVersion:
                                CmdZWaveGetVersion(sessionId);
                                break;
                            case CommandTypes.CmdZWaveNVRGetValue:
                                CmdZWaveNVRGetValue(sessionId, data);
                                break;
                            case CommandTypes.CmdSerialApiSoftReset:
                                CmdSerialApiSoftReset(sessionId);
                                break;
                            case CommandTypes.CmdZWaveRemoveNodeFromNetwork:
                                CmdZWaveRemoveNodeFromNetwork(sessionId, data);
                                break;
                            case CommandTypes.CmdSerialApiSetup:
                                CmdSerialApiSetup(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSetSucNodeId:
                                CmdZWaveSetSucNodeId(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveIsFailedNode:
                                CmdZWaveIsFailedNode(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveRequestNodeNeighborUpdate:
                                CmdZWaveRequestNodeNeighborUpdate(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveRemoveFailedNodeId:
                                CmdZWaveRemoveFailedNodeId(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveReplaceFailedNode:
                                CmdZWaveReplaceFailedNode(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveExploreRequestInclusion:
                                //CmdZWaveExploreRequestInclusion(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveControllerChange:
                                CmdZWaveControllerChange(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveGetControllerCapabilities:
                                CmdZWaveGetControllerCapabilities(sessionId);
                                break;
                            case CommandTypes.CmdZWaveWatchDogStart:
                                break;
                            case CommandTypes.CmdZWaveSendNodeInformation:
                                CmdZWaveSendNodeInformation(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveGetNodeProtocolInfo:
                                CmdZWaveGetNodeProtocolInfo(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveGetPriorityRoute:
                                CmdZWaveGetPriorityRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveSetPriorityRoute:
                                CmdZWaveSetPriorityRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveDeleteReturnRoute:
                                CmdZWaveDeleteReturnRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveDeleteSucReturnRoute:
                                CmdZWaveDeleteSucReturnRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveAssignReturnRoute:
                                CmdZWaveAssignReturnRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveAssignSucReturnRoute:
                                CmdZWaveAssignSucReturnRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveAssignPriorityReturnRoute:
                                CmdZWaveAssignPriorityReturnRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveAssignPrioritySucReturnRoute:
                                CmdZWaveAssignPrioritySucReturnRoute(sessionId, data);
                                break;
                            case CommandTypes.CmdGetRoutingTableLine:
                                CmdGetRoutingTableLine(sessionId, data);
                                break;
                            case CommandTypes.CmdZWaveNVMBackupRestore:
                                CmdZWaveNVMBackupRestore(sessionId, data);
                                break;
                            case CommandTypes.CmdSerialApiGetLRNodes:
                                break;
                            default:
                                throw new NotImplementedException("CommandType: " + (CommandTypes)data[3]);
                        }
                    }
                }
                return data.Length;
            }
            else
            {
                return 0;
            }
        }

        private void CmdZWaveNVMBackupRestore(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x2E | Operation | Length
                if (data[1] == 0x05)
                    value.TryAdd(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveNVMBackupRestore, 0x00, 0x00 }));
                else
                    value.TryAdd(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveNVMBackupRestore, 0xff, 0x01, 0x00, 0x00, 0xaa }));
            }
        }

        private void CmdGetRoutingTableLine(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x80 | bNodeID | bRemoveBad | bRemoveNonReps | funcID
                // ZW->HOST: RES | 0x80 | NodeMask[29]
                var nodesMask = new byte[29];
                if (data[4] == 0x01)
                    nodesMask[0] = 0x02;
                else nodesMask[0] = 0x01;
                var frameData = new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdGetRoutingTableLine };
                value.Add(CreateFrame(frameData.Concat(nodesMask)));
            }
        }

        private void CmdZWaveGetPriorityRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x92 | bNodeID
                // ZW->HOST: RES | 0x92 | bNodeID | retVal | repeater0 | repeater1 | repeater2 | repeater3 | routespeed
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveGetPriorityRoute, data[4], 1, 1, 0, 0, 0, 3 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveGetPriorityRoute, data[4] }));
            }
        }

        private void CmdZWaveSetPriorityRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x93 | bNodeID | repeater0 | repeater1 | repeater2 | repeater3 | routespeed
                // ZW->HOST: RES | 0x93 | bNodeID | retVal
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSetPriorityRoute, data[4], 1, 1, 0, 0, 0, 3 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSetPriorityRoute, data[4], 1 }));
            }
        }

        private void CmdZWaveDeleteReturnRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x47 | nodeID | funcID
                // ZW->HOST: RES | 0x47 | retVal
                // ZW->HOST: REQ | 0x47 | funcID | bStatus
                var funcId = data[5];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveDeleteReturnRoute, 1 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveDeleteReturnRoute, funcId, 1 }));
            }
        }

        private void CmdZWaveDeleteSucReturnRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x55 | nodeID | funcID
                // ZW->HOST: RES | 0x55 | retVal
                // ZW->HOST: REQ | 0x55 | funcID | bStatus
                var funcId = data[5];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveDeleteSucReturnRoute, 1 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveDeleteSucReturnRoute, funcId, 1 }));
            }
        }

        private void CmdZWaveAssignReturnRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x46 | bSrcNodeID | bDstNodeID | funcID
                // ZW->HOST: RES | 0x46 | retVal
                // ZW->HOST: REQ | 0x46 | funcID | bStatus
                var funcId = data[6];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveAssignReturnRoute, 1 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveAssignReturnRoute, funcId, 1 }));
            }
        }

        private void CmdZWaveAssignSucReturnRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x51 | bSrcNodeID | funcID | funcID
                // ZW->HOST: RES | 0x51 | retVal
                // ZW->HOST: REQ | 0x51 | funcID | bStatus
                var funcId = data[5];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveAssignSucReturnRoute, 1 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveAssignSucReturnRoute, funcId, 1 }));
            }
        }

        private void CmdZWaveAssignPriorityReturnRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x4F | bSrcNodeID | bDstNodeID | PriorityRoute | funcID
                // ZW->HOST: RES | 0x4F | retVal
                // ZW->HOST: REQ | 0x4F | funcID | bStatus
                var funcId = data[11];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveAssignPriorityReturnRoute, 1 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveAssignPriorityReturnRoute, funcId, 1 }));
            }
        }

        private void CmdZWaveAssignPrioritySucReturnRoute(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x58 | bSrcNodeID | PriorityRoute | funcID
                // ZW->HOST: RES | 0x58 | retVal
                // ZW->HOST: REQ | 0x58 | funcID | bStatus
                var funcId = data[10];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveAssignPrioritySucReturnRoute, 1 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveAssignPrioritySucReturnRoute, funcId, 1 }));
            }
        }

        private void CmdZWaveGetNodeProtocolInfo(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                //HOST->ZW: REQ | 0x41 | bNodeID
                //ZW->HOST: RES | 0x41 | nodeInfo
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveGetNodeProtocolInfo, _modules[sessionId].NodeInfoCapability, 0, 0, _modules[sessionId].Basic, _modules[sessionId].Generic, _modules[sessionId].Specific }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveGetNodeProtocolInfo, data[4] }));
            }
        }

        private void CmdZWaveControllerChange(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                //HOST->ZW: REQ | 0x4D | mode | funcID
                //ZW->HOST: REQ | 0x4D | funcID | bStatus | bSource | bLen | basic | generic | specific | cmdclasses[ ]
                _modules[sessionId].IsRFReceiveMode = true;
                var funcId = data[5];
                if (data[4] == (byte)ControllerChangeModes.Start)
                {
                    _modules[sessionId].IsShiftNode = true;
                    _modules[sessionId].FuncId = funcId;
                    _modules[sessionId].AddOrReplaceNodeId = data[5];
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveControllerChange, funcId, (byte)NodeStatuses.LearnReady, 0, 0 }));
                }
            }
        }

        private void CmdZWaveSendNodeInformation(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                //HOST->ZW: REQ | 0x12 | destNode | txOptions | funcID
                //ZW->HOST: RES | 0x12 | retVal
                //ZW->HOST: REQ | 0x12 | funcID | txStatus
                var funcId = data[6];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSendNodeInformation, 0x01 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSendNodeInformation, funcId, (byte)TransmitStatuses.CompleteOk }));
            }
        }

        private void CmdZWaveGetControllerCapabilities(ushort sessionId)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                //HOST->ZW: REQ | 0x05
                //ZW->HOST: RES | 0x05 | RetVal
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveGetControllerCapabilities, (byte)_modules[sessionId].ControllerCapability }));
            }
        }

        private void CmdZWaveRemoveFailedNodeId(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x61 | nodeID | funcID
                // ZW->HOST: RES | 0x61 | retVal
                // ZW->HOST: REQ | 0x61 | funcID | txStatus
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveRemoveFailedNodeId, (byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_STARTED }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRemoveFailedNodeId, data[5], (byte)FailedNodeStatuses.NodeRemoved }));
            }
        }

        private void CmdSerialApiSoftReset(ushort sessionId)
        {

        }

        private void CmdSerialApiSetup(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x0B | subCommand | bEnable
                // ZW->HOST: RES | 0x0B | subCommand | RetVal
                //HAS TO BE A SPECIFIC HANDLER FOR EACH subCommand CASE!
                var cmd = (byte)CommandTypes.CmdSerialApiSetup;
                var subCmd = data[4];
                switch (subCmd)
                {
                    case 0x10:
                        value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, cmd, subCmd, 46, }));
                        break;
                    default:
                        value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, cmd, subCmd, 0x01 }));
                        break;
                }
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdSerialApiSetup, data[4], 0x01 }));
            }
        }

        private void CmdZWaveIsFailedNode(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x62 | nodeID
                // ZW->HOST: RES | 0x62 | retVal
                var target = _modules.Where(x => x.Value.IsRFReceiveMode && x.Value.NodeId == data[4] && x.Value.HomeId.SequenceEqual(_modules[sessionId].HomeId));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveIsFailedNode, (byte)(target.Any() ? 0 : 1) }));
            }
        }

        private void CmdZWaveRequestNodeNeighborUpdate(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x48 | nodeID | funcID
                // ZW->HOST: REQ | 0x48 | funcID | bStatus
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRequestNodeNeighborUpdate, data[5], (byte)RequestNeighborUpdateStatuses.Started }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRequestNodeNeighborUpdate, data[5], (byte)RequestNeighborUpdateStatuses.Done }));
            }
        }

        private void CmdZWaveSetSucNodeId(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x54 | nodeID | SUCState | bTxOption | capabilities | funcID
                // ZW->HOST: RES | 0x54 | RetVal
                // ZW->HOST: REQ | 0x54 | funcID | txStatus
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSetSucNodeId, 0x01 }));
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSetSucNodeId, data[8], (byte)SetSucReturnValues.SucSetSucceeded }));

                var network = _modules.Where(x => x.Value.HomeId.SequenceEqual(_modules[sessionId].HomeId));
                foreach (var item in network)
                {
                    if (data[5] > 0)
                    {
                        item.Value.SucNodeId = data[4];
                    }
                    else
                    {
                        item.Value.SucNodeId = 0;
                    }
                }
                _modules[sessionId].ControllerCapability |= ControllerCapabilities.IS_SUC | ControllerCapabilities.NODEID_SERVER_PRESENT;
            }
        }

        private void CmdZWaveNVRGetValue(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x28 | offset | length
                // ZW->HOST: RES | 0x28 | NVRdata[]

                byte[] prk = "8DDD34AC7B136AFB666711FD05F4FEAD8A6D01685F4E49160C704A9D38BB9641".GetBytes();
                byte[] pub = "9DE45ED3AE44DC54FB54ED8DB93FB399D48FF461BC901C6AEEB94DFCE4EF7C59".GetBytes();

                byte[] nvr = new byte[256];
                Array.Copy(pub, 0, nvr, 35, 32);
                Array.Copy(pub, 0, nvr, 35 + 32, 32);

                var offset = data[4];
                var length = data[5];

                var nvrData = nvr.Skip(offset).Take(length).ToArray();

                value.Add(CreateFrame(new byte[]
                {
                (byte)FrameTypes.Response,
                (byte)CommandTypes.CmdZWaveNVRGetValue
                }.
                Concat(nvrData)));
            }
        }

        private void CmdZWaveGetVersion(ushort sessionId)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x15
                // ZW->HOST: RES | 0x15 | buffer(12 bytes) | library type
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveGetVersion }.Concat(new byte[12]).Concat(new byte[] { (byte)Libraries.ControllerStaticLib })));
            }
        }

        private void CmdSerialApiGetCapabilities(ushort sessionId)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x07
                // ZW->HOST: RES | 0x07 | version | revision | mfrId1 | mfrId2 | mfrProdType1 | mfrProdType2 | mfrProdId1 | mfrProdId2 | funcidSupportedBitmask[]
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdSerialApiGetCapabilities, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }.Concat(GetFuncIdSupportedBitmask(sessionId))));
            }
        }

        private void CmdZWaveSetRFReceiveMode(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                byte mode = data[4];
                _modules[sessionId].IsRFReceiveMode = mode > 0;
                // HOST->ZW: REQ | 0x10 | mode
                // ZW->HOST: RES | 0x10 | retVal            
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSetRFReceiveMode, 0x01 }));
            }
        }

        private void CmdZWaveSendDataMulti(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;

                // HOST->ZW: REQ | 0x14 | numberNodes | pNodeIDList[ ] | dataLength | pData[ ] | txOptions | funcID
                if (data.Length > ActualMaxFrameSize)
                {
                    // Response fail to client.
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSendDataMulti, 0x00 }));
                }
                else
                {
                    // Response to client.
                    var srcNodeId = _modules[sessionId].NodeId;
                    int index = 4; // numberNodes
                    byte numberNodes = data[index];
                    var nodes = new byte[numberNodes];
                    Array.Copy(data, index + 1, nodes, 0, numberNodes);
                    index += numberNodes + 1; // dataLength
                    byte dataLength = data[index];
                    var cmdData = new byte[dataLength];
                    Array.Copy(data, index + 1, cmdData, 0, dataLength);
                    index += dataLength + 1 + 1/*txOptions*/;
                    byte funcId = data[index];

                    // ZW->HOST: RES | 0x14 | RetVal
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSendDataMulti, 0x01 }));
                    // ZW->HOST: REQ | 0x14 | funcID | txStatus
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSendDataMulti, funcId, (byte)TransmitStatuses.CompleteNoAcknowledge }));

                    var destinations = _modules.Where(x => x.Value.IsRFReceiveMode && nodes.Contains(x.Value.NodeId) && x.Value.HomeId.SequenceEqual(_modules[sessionId].HomeId));
                    foreach (var destination in destinations)
                    {
                        // Create application command handler.
                        // ZW->PC: REQ | 0x04 | rxStatus | sourceNode | cmdLength | pCmd[] | rssiVal 
                        const byte rxStatus = 0x08; // RECEIVE_STATUS_TYPE_MULTI
                        if (_pool.TryGetValue(destination.Key, out BlockingCollection<byte[]> destValue))
                        {
                            destValue.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdApplicationCommandHandler, rxStatus, srcNodeId, dataLength }.Concat(cmdData).Concat(new[] { rssiVal })));
                        }
                    }
                }
            }
        }

        private void CmdZWaveGetSucNodeId(ushort sessionId)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;
                // HOST->ZW: REQ | 0x56
                // ZW->HOST: RES | 0x56 | SUCNodeID
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveGetSucNodeId, (byte)_modules[sessionId].SucNodeId }));
            }
        }

        private void CmdZWaveRequestNodeInfo(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;
                // HOST->ZW: REQ | 0x60 | NodeID
                // ZW->HOST: RES | 0x60 | retVal
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveRequestNodeInfo, 0x01 }));

                var nodeId = data[4];
                var requestedModules = _modules.Where(x => x.Value.IsRFReceiveMode && x.Value.IsNifResponseEnabled && x.Value.NodeId == nodeId);
                if (requestedModules.Any())
                {
                    var cmdClasses = requestedModules.First().Value.CmdClasses ?? new byte[0];
                    //ZW->HOST: REQ | 0x49 | bStatus | bNodeID | bLen | basic | generic | specific | commandclasses[ ]
                    byte bLen = (byte)(3 /*basic | generic | specific length*/ + cmdClasses.Length);
                    var reqCmd = new byte[bLen + 5];
                    reqCmd[0] = (byte)FrameTypes.Request;
                    reqCmd[1] = (byte)CommandTypes.CmdApplicationControllerUpdate;
                    reqCmd[2] = 0x84;
                    reqCmd[3] = data[4];
                    reqCmd[4] = bLen;
                    reqCmd[5] = 0x01;
                    reqCmd[6] = 0x02;
                    reqCmd[7] = 0x01;
                    Array.Copy(cmdClasses, 0, reqCmd, 8, cmdClasses.Length);
                    value.Add(CreateFrame(reqCmd));
                }
            }
        }

        private void CmdZWaveRemoveNodeFromNetwork(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;
                // HOST->ZW: REQ | 0x4B | bMode | funcID 
                // ZW->HOST: REQ | 0x4B | funcID | bStatus | bSource | bLen | basic | generic | specific | cmdclasses[ ]
                var funcId = data[5];
                if (((Modes)data[4]).HasFlag(Modes.NodeAny) || ((Modes)data[4]).HasFlag(Modes.NodeOptionNetworkWide))
                {
                    _modules[sessionId].IsRemovingNode = true;
                    _modules[sessionId].FuncId = funcId;
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRemoveNodeFromNetwork, funcId, (byte)NodeStatuses.LearnReady, 0, 0 }));
                }
                else if (data[4] == (byte)Modes.NodeStop)
                {
                    _modules[sessionId].IsAddingNode = false;
                    if (funcId != 0)
                    {
                        value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRemoveNodeFromNetwork, funcId, (byte)NodeStatuses.Done, 0, 0, 0 }));
                    }
                    else
                    {
                        // Assume node add completed and no response for that by design.
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void CmdZWaveReplaceFailedNode(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;
                // HOST->ZW: REQ | 0x63 | nodeID | funcID
                // ZW->HOST: RES | 0x63 | retVal
                // ZW->HOST: REQ | 0x63 | funcID | txStatus
                var funcId = data[5];
                _modules[sessionId].IsReplacingNode = true;
                _modules[sessionId].FuncId = funcId;
                _modules[sessionId].AddOrReplaceNodeId = data[4];
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveReplaceFailedNode, (byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_STARTED }));
            }
        }

        private void CmdZWaveAddNodeToNetwork(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;

                // HOST->ZW: REQ | 0x4A | mode | funcID
                // ZW->HOST: REQ | 0x4A | funcID | bStatus | bSource | bLen | basic | generic | specific | cmdclasses[]
                var funcId = data[5];
                if (data[4] == (byte)Modes.NodeAny || ((Modes)data[4]).HasFlag(Modes.NodeOptionNetworkWide) || data[4] == (byte)(Modes.NodeOptionNormalPower | Modes.NodeAny))
                {
                    _modules[sessionId].IsAddingNode = true;
                    _modules[sessionId].FuncId = funcId;
                    _modules[sessionId].AddOrReplaceNodeId = _modules[sessionId].SeedNextNodeId();
                    _modules[sessionId].ControllerCapability = ControllerCapabilities.IS_REAL_PRIMARY;

                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveAddNodeToNetwork, funcId, (byte)NodeStatuses.LearnReady, 0, 0 }));
                }
                else if (data[4] == (byte)Modes.NodeStop)
                {
                    _modules[sessionId].IsAddingNode = false;
                    if (funcId != 0)
                    {
                        value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveAddNodeToNetwork, funcId, (byte)NodeStatuses.Done, 0, 0, 0 }));
                    }
                    else
                    {
                        // Assume node add completed and no response for that by design.
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void CmdZWaveSetLearnMode(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;

                // HOST->ZW: REQ | 0x50 | mode | funcID
                // ZW->HOST: REQ | 0x50 | funcID | bStatus | bSource | bLen | pCmd[]
                var learnModeFuncId = data[5];
                if (data[4] == (byte)LearnModes.LearnModeClassic ||
                    data[4] == (byte)LearnModes.LearnModeNWE ||
                    data[4] == (byte)LearnModes.LearnModeNWI)
                {
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSetLearnMode, learnModeFuncId, (byte)NodeStatuses.LearnReady, 0, 0, 0 }));

                    var inclusionModules = _modules.Where(x => x.Value.IsAddingNode);
                    var exclusionModules = _modules.Where(x => x.Value.IsRemovingNode);
                    var replacingModules = _modules.Where(x => x.Value.IsReplacingNode);
                    var shiftModules = _modules.Where(x => x.Value.IsShiftNode);
                    if (inclusionModules.Any())
                    {
                        var inclusionModule = inclusionModules.First();
                        if (_pool.TryGetValue(inclusionModule.Key, out BlockingCollection<byte[]> inclusionValue))
                        {
                            if (!_modules[sessionId].HomeId.SequenceEqual(inclusionModule.Value.HomeId))
                            {
                                _modules[sessionId].NodeId = inclusionModule.Value.AddOrReplaceNodeId;
                                _modules[sessionId].HomeId = inclusionModule.Value.HomeId.Take(4).ToArray();
                                _modules[sessionId].SucNodeId = inclusionModule.Value.SucNodeId;
                                inclusionModule.Value.NodesList.Add(inclusionModule.Value.AddOrReplaceNodeId);
                                _modules[sessionId].NodesList = new List<byte> { _modules[sessionId].NodeId, inclusionModule.Value.NodeId };
                                _modules[sessionId].ControllerCapability = ControllerCapabilities.IS_SECONDARY | ControllerCapabilities.ON_OTHER_NETWORK;
                            }
                            inclusionValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.NodeFound, _modules[inclusionModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveAddNodeToNetwork));
                            inclusionValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.AddingRemovingController, _modules[inclusionModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveAddNodeToNetwork));
                            inclusionValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.ProtocolDone, _modules[inclusionModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveAddNodeToNetwork));

                            _modules[inclusionModule.Key].IsAddingNode = false;
                        }
                    }
                    if (shiftModules.Any())
                    {
                        var shiftModule = shiftModules.First();
                        if (_pool.TryGetValue(shiftModule.Key, out BlockingCollection<byte[]> shiftValue))
                        {
                            //_modules[sessionId].NodesList = new List<byte> { _modules[sessionId].NodeId, shiftModule.Value.NodeId };
                            var inclusionModule = shiftModules.First();
                            var capability = _modules[sessionId].ControllerCapability;
                            _modules[sessionId].ControllerCapability = inclusionModule.Value.ControllerCapability;
                            inclusionModule.Value.ControllerCapability = capability;

                            shiftValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.NodeFound, _modules[shiftModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveControllerChange));
                            shiftValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.AddingRemovingController, _modules[shiftModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveControllerChange));
                            shiftValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.ProtocolDone, _modules[shiftModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveControllerChange));
                            shiftValue.Add(ComposeAddNodeClientResponse((byte)NodeStatuses.Done, _modules[shiftModule.Key].FuncId, _modules[sessionId].NodeId, _modules[sessionId].CmdClasses, CommandTypes.CmdZWaveControllerChange));

                            _modules[shiftModule.Key].IsShiftNode = false;
                        }
                    }
                    if (replacingModules.Any())
                    {
                        var replacingModule = replacingModules.First();
                        if (_pool.TryGetValue(replacingModule.Key, out BlockingCollection<byte[]> replaceValue))
                        {
                            _modules[sessionId].NodeId = replacingModule.Value.AddOrReplaceNodeId;
                            _modules[sessionId].HomeId = replacingModule.Value.HomeId.Take(4).ToArray();
                            _modules[sessionId].SucNodeId = replacingModule.Value.SucNodeId;

                            replaceValue.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveReplaceFailedNode, _modules[replacingModule.Key].FuncId, (byte)FailedNodeStatuses.NodeReplace }));
                            replaceValue.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveReplaceFailedNode, _modules[replacingModule.Key].FuncId, (byte)FailedNodeStatuses.NodeReplaceDone }));

                            _modules[replacingModule.Key].IsReplacingNode = false;
                        }
                    }
                    else if (exclusionModules.Any())
                    {
                        var exclusionModule = exclusionModules.First();
                        if (_pool.TryGetValue(exclusionModule.Key, out BlockingCollection<byte[]> exclusionValue))
                        {
                            exclusionValue.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRemoveNodeFromNetwork, _modules[exclusionModule.Key].FuncId, (byte)NodeStatuses.NodeFound, 0, 0 }));
                            exclusionValue.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRemoveNodeFromNetwork, _modules[exclusionModule.Key].FuncId, (byte)NodeStatuses.AddingRemovingController, _modules[sessionId].NodeId, (byte)(3 + (_modules[sessionId].CmdClasses ?? new byte[0]).Length), _modules[sessionId].Basic, _modules[sessionId].Generic, _modules[sessionId].Specific }.Concat(_modules[sessionId].CmdClasses ?? new byte[0])));

                            ResetModule(sessionId);
                            _modules[exclusionModule.Key].NodesList.Remove(_modules[sessionId].NodeId);

                            exclusionValue.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveRemoveNodeFromNetwork, _modules[exclusionModule.Key].FuncId, (byte)NodeStatuses.Done, 0, 0 }));

                            _modules[exclusionModule.Key].IsRemovingNode = false;
                        }
                    }
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSetLearnMode, learnModeFuncId, (byte)NodeStatuses.Done, _modules[sessionId].NodeId, 0, 0 }));
                }
                else if (data[4] == (byte)LearnModes.LearnModeDisable)
                {
                    // Assume we disable learn mode and no response for that by design.
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void SetSucNodeId(ushort sessionId, NodeTag node)
        {
            _modules[sessionId].SucNodeId = node.Id;
        }

        private void CmdZWaveSendData(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                _modules[sessionId].IsRFReceiveMode = true;

                // HOST->ZW: REQ | 0x13 | nodeID | dataLength | pData[ ] | txOptions | funcID
                if (data.Length > ActualMaxFrameSize)
                {
                    // Response fail to client.
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSendData, 0x00 }));
                }
                else
                {
                    // Response to client.
                    var nodeId = data[4];
                    var dataLength = data[5];
                    var funcId = data[5 + 1/*cmd*/ + dataLength + 1/*txOptions*/];
                    var srcNodeId = _modules[sessionId].NodeId;
                    var cmdData = new byte[dataLength];
                    Array.Copy(data, 6, cmdData, 0, dataLength);

                    // ZW->HOST: RES | 0x13 | RetVal
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdZWaveSendData, 0x01 }));
                    // ZW->HOST: REQ | 0x13 | funcID | txStatus
                    value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSendData, funcId, (byte)TransmitStatuses.CompleteOk }));

                    // Send data to linked device.
                    if (nodeId == 0xFF)
                    {
                        var destinations = _modules.Where(x => x.Value.IsRFReceiveMode && x.Value.NodeId != srcNodeId && x.Value.HomeId.SequenceEqual(_modules[sessionId].HomeId));
                        foreach (var destination in destinations)
                        {
                            if (_pool.TryGetValue(destination.Key, out BlockingCollection<byte[]> destValue))
                            {
                                // Create application command handler.
                                // ZW->PC: REQ | 0x04 | rxStatus | sourceNode | cmdLength | pCmd[] | rssiVal 
                                byte rxStatus = 0x04;
                                var toClientDataList = new List<byte>();
                                toClientDataList.AddRange(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdApplicationCommandHandler, rxStatus, srcNodeId, dataLength });
                                toClientDataList.AddRange(cmdData);
                                toClientDataList.Add(rssiVal);
                                destValue.Add(CreateFrame(toClientDataList.ToArray()));
                            }
                        }
                    }
                    else
                    {
                        var destination = _modules.Where(x => x.Value.IsRFReceiveMode && x.Value.NodeId != srcNodeId && x.Value.NodeId == nodeId && x.Value.HomeId.SequenceEqual(_modules[sessionId].HomeId));
                        if (destination.Any())
                        {
                            if (_pool.TryGetValue(destination.First().Key, out BlockingCollection<byte[]> destValue))
                            {
                                // Create application command handler.
                                // ZW->PC: REQ | 0x04 | rxStatus | sourceNode | cmdLength | pCmd[] | rssiVal 
                                byte rxStatus = 0x00;
                                var toClientDataList = new List<byte>();
                                toClientDataList.AddRange(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdApplicationCommandHandler, rxStatus, srcNodeId, dataLength });
                                toClientDataList.AddRange(cmdData);
                                toClientDataList.Add(rssiVal);
                                destValue.Add(CreateFrame(toClientDataList.ToArray()));
                            }
                        }
                    }
                }
            }
        }

        private static void CmdZWaveSendDataAbort(ushort sessionId)
        {
            // HOST->ZW: REQ | 0x16
            // Ignore.
        }

        private void CmdSerialApiGetInitData(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                var frame = new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdSerialApiGetInitData, 0, (byte)_modules[sessionId].ControllerCapability }; switch (_modules[sessionId].NodesList.Count)
                {
                    case 1:
                        frame = frame.Concat(new byte[] { 29, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0 }).ToArray();
                        break;
                    case 2:
                        frame = frame.Concat(new byte[] { 29, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0 }).ToArray();
                        break;
                    case 3:
                        frame = frame.Concat(new byte[] { 29, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0 }).ToArray();
                        break;
                    default: throw new NotImplementedException();
                }
                if (_modules[sessionId].NodesList.Count > 0)
                {
                    frame = frame.Concat(_modules[sessionId].NodesList).ToArray();
                }
                frame = frame.Concat(new byte[] { 0, 0 }).ToArray();
                value.Add(CreateFrame(frame));
            }
        }

        private void CmdSerialApiApplNodeInformation(ushort sessionId, byte[] data)
        {
            int ccLen = data[7];
            var supportedCmdClasses = new byte[ccLen];
            Array.Copy(data, 8, supportedCmdClasses, 0, ccLen);
            _modules[sessionId].CmdClasses = supportedCmdClasses;
        }

        private void CmdMemoryGetId(ushort sessionId)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                // HOST->ZW: REQ | 0x20
                // ZW->HOST: RES | 0x20 | HomeId(4 bytes) | NodeId
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Response, (byte)CommandTypes.CmdMemoryGetId, _modules[sessionId].HomeId[0], _modules[sessionId].HomeId[1], _modules[sessionId].HomeId[2], _modules[sessionId].HomeId[3], _modules[sessionId].NodeId }));
            }
        }

        private void CmdZWaveSetDefault(ushort sessionId, byte[] data)
        {
            if (_pool.TryGetValue(sessionId, out BlockingCollection<byte[]> value))
            {
                ResetModule(sessionId);
                var funcId = data[4];
                // HOST->ZW: REQ | 0x42 | funcID
                // ZW->HOST: REQ | 0x42 | funcID
                value.Add(CreateFrame(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdZWaveSetDefault, funcId }));
            }
        }

        public void ResetModule(ushort sessionId)
        {
            _modules[sessionId].Reset();
        }

        public void SetControllerNetworkRole(ushort sessionId, ControllerCapabilities controllerCapabilities)
        {
            _modules[sessionId].ControllerCapability = controllerCapabilities;
        }
        #endregion
    }

    internal class SendDataDelayedInfo
    {
        public int FramesCount { get; set; }
        public int DelayMs { get; set; }
    }
}
