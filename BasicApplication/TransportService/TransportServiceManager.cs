/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.TransportService.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWave.BasicApplication.TransportService
{
    public class TransportServiceManager : SubstituteManagerBase
    {
        public const int FRAGMENT_RX_TIMEOUT = 800;

        protected override SubstituteIncomingFlags GetId()
        {
            return SubstituteIncomingFlags.TransportService;
        }

        private readonly object _lockObject = new object();
        private ActionSerialGroup _rxTimerAction;

        private NodeTag _srcNode;
        internal NodeTag SrcNode { get { return _srcNode; } }

        private NodeTag _nodeIdWaitResponded;
        internal NodeTag NodeIdWaitResponded { get { return _nodeIdWaitResponded; } }

        private ReassemblingData _reassemblingData;
        internal ReassemblingData ReassemblingData { get { return _reassemblingData; } }

        public TransportServiceManagerInfo TransportServiceManagerInfo { get; private set; }
        public TransportServiceManager(NetworkViewPoint network, TransportServiceManagerInfo info)
            : base(network)
        {
            IsActive = true;
            TransportServiceManagerInfo = info;
        }

        protected override CustomDataFrame SubstituteIncomingInternal(CustomDataFrame packet, NodeTag destNode, NodeTag srcNode, byte[] cmdData, int lenIndex, out ActionBase additionalAction, out ActionBase completeAction)
        {
            CustomDataFrame ret = packet;
            additionalAction = null;
            completeAction = null;
            if (IsActive)
            {
                if (cmdData.Length > 1 && cmdData[0] == COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID && ValidateCRC16Checksum(cmdData))
                {
                    byte id = (byte)(cmdData[1] & COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT.ID_MASK);

                    if (id == (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT.ID) &&
                        TransportServiceManagerInfo.TestNeedToIgnoreFirstSegment.CanBeUsed &&
                        TransportServiceManagerInfo.TestNeedToIgnoreFirstSegment.PullValue().Value
                        )
                    {
                        return ret;
                    }

                    if (id == (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT.ID) &&
                        TransportServiceManagerInfo.TestNeedToIgnoreSubsequentSegment.CanBeUsed &&
                        TransportServiceManagerInfo.TestOffset.CanBeUsed
                        )
                    {
                        var datagramOffset = SegmentsContainer.GetSegmentOffset(cmdData);
                        var payloadLength = SegmentsContainer.GetSegmentPayloadLength(cmdData);
                        if (TransportServiceManagerInfo.TestOffset.PullValue(
                                val =>
                                {
                                    return val >= datagramOffset && val <= (datagramOffset + payloadLength);
                                }) != null &&
                            TransportServiceManagerInfo.TestNeedToIgnoreSubsequentSegment.PullValue().Value
                            )
                        {
                            return ret;
                        }
                    }

                    lock (_lockObject)
                    {
                        if (id == (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT.ID) &&
                            _reassemblingData != null &&
                            !_reassemblingData.SegmentsContainer.IsCompleted &&
                            _reassemblingData.SrcNode == srcNode)
                        {
                            RxReset();
                        }

                        if (_reassemblingData == null || _reassemblingData.SegmentsContainer.IsCompleted)
                        {
                            if (id == (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT.ID))
                            {
                                RxReset();
                                _srcNode = srcNode;

                                var cmdHeader = new byte[lenIndex];
                                Array.Copy(packet.Data, 0, cmdHeader, 0, cmdHeader.Length);

                                var cmdFooter = new byte[packet.Data.Length - cmdHeader.Length - cmdData.Length - 1];
                                Array.Copy(packet.Data, lenIndex + packet.Data[lenIndex] + 1, cmdFooter, 0, cmdFooter.Length);

                                _reassemblingData = new ReassemblingData
                                {
                                    SegmentsContainer = new SegmentsContainer(cmdData),
                                    CompletedCmdHeader = cmdHeader,
                                    CompletedCmdFooter = cmdFooter,
                                    SrcNode = srcNode
                                };

                                if (_reassemblingData.SegmentsContainer.IsCompleted)
                                {
                                    additionalAction = new SendDataOperation(_network, srcNode, CreateSegmentCompletedCmd(), TransportServiceManagerInfo.TxOptions)
                                    {
                                        SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0),
                                        IsTraceLogDisabled = true
                                    };
                                    ret = CreateDataFrameOnReassemblingCompleted(packet);
                                }
                                else
                                {
                                    // Start fragment rx timer.
                                    _rxTimerAction = CreateRxTimerAction();
                                    additionalAction = _rxTimerAction;
                                }
                            }
                            else if (id == (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT.ID))
                            {
                                if (_reassemblingData != null && _reassemblingData.SegmentsContainer.CheckForLastSegment(cmdData))
                                {
                                    additionalAction = new SendDataOperation(_network, srcNode, CreateSegmentCompletedCmd(), TransportServiceManagerInfo.TxOptions)
                                    {
                                        SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0),
                                        IsTraceLogDisabled = true
                                    };
                                }
                                else if (srcNode != _nodeIdWaitResponded)
                                {
                                    _nodeIdWaitResponded = srcNode;
                                    additionalAction = new SendDataOperation(_network, srcNode, CreateSegmentWaitCmd(0), TransportServiceManagerInfo.TxOptions) { SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0) };
                                }
                            }
                        }
                        else if (_srcNode == srcNode && id == (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT.ID))
                        {
                            if (_rxTimerAction != null)
                            {
                                //_rxTimerAction.Actions[0].Token.Reset(FRAGMENT_RX_TIMEOUT); // Reset fragment rx timer.
                            }
                            else
                            {
                                _rxTimerAction = CreateRxTimerAction();
                                additionalAction = _rxTimerAction;
                            }

                            _reassemblingData.SegmentsContainer.AddSegment(cmdData);
                            if (_reassemblingData.SegmentsContainer.IsLastSegmentReceived)
                            {
                                if (_reassemblingData.SegmentsContainer.IsCompleted)
                                {
                                    _rxTimerAction.Token.SetCompleted(); // Complete fragment rx timer.
                                    _rxTimerAction = null;

                                    additionalAction = new SendDataOperation(_network, srcNode, CreateSegmentCompletedCmd(), TransportServiceManagerInfo.TxOptions)
                                    {
                                        SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0),
                                        IsTraceLogDisabled = true
                                    };
                                    ret = CreateDataFrameOnReassemblingCompleted(packet);
                                }
                                else
                                {
                                    ushort missingOffset = _reassemblingData.SegmentsContainer.GetFirstMissingFragmentOffset();
                                    additionalAction = new SendDataOperation(_network, srcNode,
                                        CreateSegmentRequestCmd(missingOffset),
                                        TransportServiceManagerInfo.TxOptions)
                                    { SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0) };
                                }
                            }
                        }
                        else
                        {
                            if (srcNode != _nodeIdWaitResponded)
                            {
                                _nodeIdWaitResponded = srcNode;
                                additionalAction = new SendDataOperation(_network, srcNode,
                                        CreateSegmentWaitCmd(_reassemblingData.SegmentsContainer.PendingSegmentsCount),
                                        TransportServiceManagerInfo.TxOptions)
                                { SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0) };
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private CustomDataFrame CreateDataFrameOnReassemblingCompleted(CustomDataFrame incomePacket)
        {
            var dataHeader = _reassemblingData.CompletedCmdHeader;
            var dataFooter = _reassemblingData.CompletedCmdFooter;
            var reasembledCmd = _reassemblingData.SegmentsContainer.GetDatagram();
            var packetData = new byte[_reassemblingData.CompletedCmdHeader.Length +
                _reassemblingData.CompletedCmdFooter.Length +
                reasembledCmd.Length + 1];
            int offset = 0;
            Array.Copy(dataHeader, packetData, dataHeader.Length);
            offset += dataHeader.Length;

            packetData[offset] = (byte)reasembledCmd.Length;
            offset++;

            Array.Copy(reasembledCmd, 0, packetData, offset, reasembledCmd.Length);
            offset += reasembledCmd.Length;
            Array.Copy(dataFooter, 0, packetData, offset, dataFooter.Length);

            return CreateNewFrame(incomePacket, packetData, reasembledCmd.Length);
        }

        private ActionSerialGroup CreateRxTimerAction()
        {
            var expectFirstRx = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty, new byte[] { COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID, 0xFF }, 2, FRAGMENT_RX_TIMEOUT)
            {
                Name = "First Rx Timeout",
                IsTraceLogDisabled = true
            };

            var sendFragmentRequest = new SendDataOperation(_network, _srcNode, null, TransportServiceManagerInfo.TxOptions)
            {
                SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0),
                IsTraceLogDisabled = true
            };

            var expectSecondRx = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty, new byte[] { COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID, 0xFF }, 2, FRAGMENT_RX_TIMEOUT)
            {
                Name = "Second Rx Timeout",
                IsTraceLogDisabled = true
            };

            var ret = new ActionSerialGroup(
                OnFragmentRxTimeout,
                expectFirstRx,
                sendFragmentRequest,
                expectSecondRx)
            {
                CompletedCallback = OnRxTimerActionCompleted,
                IsTraceLogDisabled = true
            };
            return ret;
        }

        private void OnFragmentRxTimeout(ActionBase actionBase, ActionResult actionResult)
        {
            if (_rxTimerAction != null && actionBase is SendDataOperation)
            {
                var sendSegmentRequestAction = (SendDataOperation)actionBase;
                ushort missingOffset = _reassemblingData.SegmentsContainer.GetFirstMissingFragmentOffset();
                sendSegmentRequestAction.Data = CreateSegmentRequestCmd(missingOffset);
            }
        }

        private void OnRxTimerActionCompleted(IActionItem action)
        {
            var actionRxTimerAction = (ActionSerialGroup)action;
            if (actionRxTimerAction.Actions[2].Token.State == ActionStates.Expired)
            {
                lock (_lockObject)
                {
                    RxReset();
                }
            }
        }

        private void RxReset()
        {
            _nodeIdWaitResponded = NodeTag.Empty;
            _srcNode = NodeTag.Empty;
            _reassemblingData = null;
            if (_rxTimerAction != null)
            {
                _rxTimerAction.SetCancelled();
                _rxTimerAction = null;
            }
        }

        private byte[] CreateSegmentCompletedCmd()
        {
            var segmentCompleteActionCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_COMPLETE();
            if (TransportServiceManagerInfo.TestSegmentCompleteCmdSessionId.CanBeUsed)
            {
                segmentCompleteActionCmd.properties2.sessionId = TransportServiceManagerInfo.TestSegmentCompleteCmdSessionId.PullValue().Value;
            }
            else
            {
                segmentCompleteActionCmd.properties2.sessionId = _reassemblingData.SegmentsContainer.SessionId;
            }
            return segmentCompleteActionCmd;
        }

        private byte[] CreateSegmentWaitCmd(byte pendingSegments)
        {
            var segmentWaitCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_WAIT();
            segmentWaitCmd.pendingFragments = pendingSegments;
            return segmentWaitCmd;
        }

        private byte[] CreateSegmentRequestCmd(ushort missingOffset)
        {
            var segmentRequestAction = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_REQUEST();
            segmentRequestAction.properties2.sessionId = _reassemblingData.SegmentsContainer.SessionId;
            segmentRequestAction.properties2.datagramOffset1 = (byte)((missingOffset) >> 8);
            segmentRequestAction.datagramOffset2 = (byte)missingOffset;
            return segmentRequestAction;
        }

        public override ActionBase SubstituteActionInternal(ApiOperation action)
        {
            ActionBase ret = null;
            if (IsActive)
            {
                byte[] dataToSend = null;
                NodeTag node = NodeTag.Empty;
                TransmitOptions txOptions = TransmitOptions.TransmitOptionNone;
                switch (action)
                {
                    case SendDataOperation sdo:
                        dataToSend = sdo.Data;
                        node = sdo.DstNode;
                        txOptions = sdo.TxOptions;
                        break;
                    case SendDataBridgeOperation sdb:
                        dataToSend = sdb.Data;
                        node = sdb.DstNode;
                        txOptions = sdb.TxOptions;
                        break;
                    case SendDataExOperation sdex:
                        dataToSend = sdex.Data;
                        node = sdex.DstNode;
                        txOptions = sdex.TxOptions;
                        break;
                    default:
                        break;
                }

                int maxSegmentSize = _network.TransportServiceMaxSegmentSize;
                if (_network.IsLongRangeEnabled(node))
                {
                    maxSegmentSize = _network.TransportServiceMaxLRSegmentSize;
                }
                if (maxSegmentSize > 0 &&
                    dataToSend != null &&
                    dataToSend.Length > maxSegmentSize)
                {
                    if (dataToSend[0] != COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID && dataToSend[0] != COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID
                        && !((ApiOperation)action).SubstituteSettings.HasFlag(SubstituteFlags.DenyTransportService))
                    {
                        if (node.Id > 0x00 && node.Id != 0xFF && IsTransportServiceSupported(node))
                        {
                            ret = new SendDataTransportTask(_network, TransportServiceManagerInfo, node, dataToSend, txOptions);
                        }
                    }
                }
            }
            return ret;
        }

        private bool IsTransportServiceSupported(NodeTag node)
        {
            bool ret = false;
            if (TransportServiceManagerInfo.SendDataSubstitutionCallback != null)
            {
                ret = TransportServiceManagerInfo.SendDataSubstitutionCallback(node);
            }
            return ret;
        }

        private List<ActionToken> mRunningActionTokens = new List<ActionToken>();
        private readonly object mLockObject = new object();
        public override List<ActionToken> GetRunningActionTokens()
        {
            List<ActionToken> ret = null;
            lock (mLockObject)
            {
                ret = new List<ActionToken>(mRunningActionTokens);
            }
            return ret;
        }

        public override void AddRunningActionToken(ActionToken token)
        {
            lock (mLockObject)
            {
                mRunningActionTokens.Add(token);
            }
        }

        public override void RemoveRunningActionToken(ActionToken token)
        {
            lock (mLockObject)
            {
                mRunningActionTokens.Remove(token);
            }
        }

        private bool ValidateCRC16Checksum(byte[] cmd)
        {
            var crc16Checksum = cmd.Skip(cmd.Length - 2).ToArray();
            ushort crc = Tools.ZW_CreateCrc16(null, 0, cmd, (byte)(cmd.Length - 2));
            return ((byte)(crc >> 8)) == crc16Checksum[0] && ((byte)crc) == crc16Checksum[1];
        }
    }

    public class ReassemblingData
    {
        public SegmentsContainer SegmentsContainer { get; set; }
        public ActionBase TxTimerAction { get; set; }
        public byte[] CompletedCmdHeader { get; set; }
        public byte[] CompletedCmdFooter { get; set; }
        public NodeTag SrcNode { get; set; }
    }
}
