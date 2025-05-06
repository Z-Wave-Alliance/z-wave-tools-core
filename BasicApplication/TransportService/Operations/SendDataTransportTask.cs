/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Threading;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.TransportService.Operations
{
    public class SendDataTransportTask : ApiOperation
    {
        public const int WAIT_TIMEOUT = 100;

        public const int FIRST_SEGMENT_CMD_BYTES = 8;
        public const int SUBSEQUENT_SEGMENT_CMD_BYTES = 9;

        private readonly int _transportServiceMaxSegmentSize;
        private readonly int _firstSegmentPayloadSize;
        private readonly int _subsequentSegmentPayloadSize;

        private byte[] _data;
        private TransportServiceManagerInfo _transportServiceManagerInfo;
        private int _dataOffset = 0;
        private readonly byte _sessionId;
        private SendDataOperation _firstSegmentSendOperation;
        private SendDataOperation _nextSegmentSendOperation;
        private ExpectDataOperation _segmentCompleteExpect;
        private ResponseDataOperation _segmentRequestResponse;
        private ResponseDataOperation _waitSegmentResponse;
        private bool _isLastSegmentRetransmited = false;
        private ActionCompletedUnit _sendNextSegmentCompletedUnit;
        private ITimeoutItem _timeoutItem;

        public TransmitOptions TxOptions { get; private set; }
        public NodeTag Node { get; private set; }

        public SendDataResult SpecificResult
        {
            get { return (SendDataResult)Result; }
        }

        internal SendDataTransportTask(NetworkViewPoint network, TransportServiceManagerInfo transportServiceManagerInfo, NodeTag node,
            byte[] data, TransmitOptions txOptions/*, Action<ActionUnit> onHandledCallback*/)
            : base(false, null, false)
        {
            _network = network;
            _transportServiceManagerInfo = transportServiceManagerInfo;
            _transportServiceMaxSegmentSize = network.TransportServiceMaxSegmentSize;
            if (network.IsLongRangeEnabled(node))
            {
                _transportServiceMaxSegmentSize = network.TransportServiceMaxLRSegmentSize;
            }
            _firstSegmentPayloadSize = _transportServiceMaxSegmentSize - FIRST_SEGMENT_CMD_BYTES;
            _subsequentSegmentPayloadSize = _transportServiceMaxSegmentSize - SUBSEQUENT_SEGMENT_CMD_BYTES;
            Node = node;
            _data = data;
            TxOptions = txOptions;
            _sessionId = _transportServiceManagerInfo.GenerateSessionId();
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, new ActionBase[] { _segmentRequestResponse, _waitSegmentResponse, _firstSegmentSendOperation }));
            ActionUnits.Add(new ActionCompletedUnit(_firstSegmentSendOperation, OnFirstSegment, _nextSegmentSendOperation) { Name = "_firstSegmentSendOperation" });
            _sendNextSegmentCompletedUnit = new ActionCompletedUnit(_nextSegmentSendOperation, OnSendNextSegment, _nextSegmentSendOperation) { Name = "_nextSegmentSendOperation" };
            ActionUnits.Add(_sendNextSegmentCompletedUnit);
            ActionUnits.Add(new ActionCompletedUnit(_segmentCompleteExpect, OnSegmentComplete) { Name = "_segmentCompleteExpect" });
            ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, OnTimeElapsed, 0) { Name = "_timeElapsed" });
        }

        protected override void CreateInstance()
        {
            var firstSegmentCmd = CreateFirstSegmentCmd(_data);
            _firstSegmentSendOperation = new SendDataOperation(_network, Node, firstSegmentCmd, TxOptions)
            {
                SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0)
            };

            _nextSegmentSendOperation = new SendDataOperation(_network, Node, null, TxOptions)
            {
                SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0)
            };

            var completeCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_COMPLETE();
            completeCmd.properties2.sessionId = _sessionId;
            byte[] completeCmdData = completeCmd;
            _segmentCompleteExpect = new ExpectDataOperation(_network, NodeTag.Empty,
                Node,
                new ByteIndex[] { completeCmdData[0], completeCmdData[1], completeCmdData[2] },
                DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT
                )
            { SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0), Name = "ExpectData-SegmentComplete" };

            var requestCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_REQUEST();
            _segmentRequestResponse = new ResponseDataOperation(_network, new ResponseDataDelegate(OnRequestSegment),
                _transportServiceManagerInfo.TxOptions,
                Node,
                ((byte[])requestCmd)[0],
                ((byte[])requestCmd)[1]
                )
            { SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0) };

            var waitCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_WAIT();
            _waitSegmentResponse = new ResponseDataOperation(_network, new ResponseDataDelegate(OnWaitSegment),
                _transportServiceManagerInfo.TxOptions,
                Node,
                ((byte[])waitCmd)[0],
                ((byte[])waitCmd)[1]
                )
            { SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0) };
            _timeoutItem = new TimeInterval(GetNextCounter(), Id, 100);
        }

        private void OnFirstSegment(ActionCompletedUnit actionUnit)
        {
            "<!> OnFirstSegment"._DLOG();
            _dataOffset = _firstSegmentPayloadSize;
            OnSendNextSegment(actionUnit);
        }

        private void OnSendNextSegment(ActionCompletedUnit actionUnit)
        {
            "<!> OnSendNextSegment"._DLOG();
            if ((TxOptions & TransmitOptions.TransmitOptionAcknowledge) == TransmitOptions.TransmitOptionAcknowledge)
            {
                var sendData = actionUnit.Action as SendDataOperation;
                if (sendData.SpecificResult.TransmitStatus == TransmitStatuses.CompleteNoAcknowledge)
                {
                    SpecificResult.TxSubstituteStatus = SubstituteStatuses.Failed;
                    "<!> OnSendNextSegment Failed"._DLOG();
                    SetStateFailed(actionUnit);
                }
            }

            if (_dataOffset < _data.Length)
            {
                _nextSegmentSendOperation.NewToken();
                _nextSegmentSendOperation.Data = CreateSubsequentSegmentCmd(_data, _dataOffset);

                if (_dataOffset + _subsequentSegmentPayloadSize >= _data.Length)
                {
                    if (_segmentCompleteExpect.Token.State == ActionStates.Running)
                    {
                        actionUnit.AddNextActionItems(new TimeInterval(0, _segmentCompleteExpect.Id, DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT));
                    }
                    else
                    {
                        _segmentCompleteExpect.NewToken();
                        actionUnit.AddFirstNextActionItems(_segmentCompleteExpect);
                    }
                }
                _dataOffset += _subsequentSegmentPayloadSize;
            }
        }

        private byte[] OnRequestSegment(ReceiveStatuses options, NodeTag destNode, NodeTag srcNode, byte[] data)
        {
            if (_dataOffset >= _data.Length)
            {
                //actionUnit.AddNextActionItems(new TimeInterval(0, _segmentCompleteExpect.Id, DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT));
            }
            var requestCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_REQUEST)data;
            ushort offset = requestCmd.datagramOffset2;
            ushort offsetHighPart = requestCmd.properties2.datagramOffset1;
            offset = (ushort)(offset | (offsetHighPart << 8));
            return CreateSubsequentSegmentCmd(_data, offset);
        }

        private byte[] OnWaitSegment(ReceiveStatuses options, NodeTag destNode, NodeTag srcNode, byte[] data)
        {
            _segmentCompleteExpect.SetCancelled();
            var waitCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_WAIT)data;
            var waitMs = waitCmd.pendingFragments * WAIT_TIMEOUT;
            _timeoutItem.TimeoutMs = waitMs;
            _sendNextSegmentCompletedUnit.SetNextActionItems(_timeoutItem);
            "<!> OnWaitSegment"._DLOG();
            return null;
        }

        private void OnTimeElapsed(TimeElapsedUnit timeElapsedUnit)
        {
            "<!> OnTimeElapsed <!>"._DLOG();
            _firstSegmentSendOperation.NewToken();
            _firstSegmentSendOperation.Data = CreateFirstSegmentCmd(_data);
            timeElapsedUnit.SetNextActionItems(_firstSegmentSendOperation);
            _nextSegmentSendOperation.NewToken();
            _sendNextSegmentCompletedUnit.SetNextActionItems(_nextSegmentSendOperation);
        }

        private void OnSegmentComplete(ActionCompletedUnit actionUnit)
        {
            "<!> OnSegmentComplete 1"._DLOG();
            if (_segmentCompleteExpect.Result.State == ActionStates.Cancelled || _segmentCompleteExpect.Result.State == ActionStates.Expired)
            {
                "<!> OnSegmentComplete 2 expired"._DLOG();
                if (!_isLastSegmentRetransmited)
                {
                    "<!> OnSegmentComplete 3 not last"._DLOG();
                    _segmentCompleteExpect.NewToken();
                    _nextSegmentSendOperation.NewToken();
                    _dataOffset -= _subsequentSegmentPayloadSize;
                    _nextSegmentSendOperation.Data = CreateSubsequentSegmentCmd(_data, _dataOffset);
                    actionUnit.SetNextActionItems(_segmentCompleteExpect, _nextSegmentSendOperation);
                    _dataOffset += _subsequentSegmentPayloadSize;
                    _isLastSegmentRetransmited = true;
                }
                else
                {
                    "<!> OnSegmentComplete 4 last"._DLOG();
                    SpecificResult.TxSubstituteStatus = SubstituteStatuses.Failed;
                    SetStateFailed(actionUnit);
                }
            }
            else if (_segmentCompleteExpect.Result.State == ActionStates.Completed)
            {

                SpecificResult.TxSubstituteStatus = SubstituteStatuses.Done;
                SetStateCompleted(actionUnit);
            }
        }

        private byte[] CreateSubsequentSegmentCmd(byte[] data, int offsetInData)
        {
            var offset = _data.Length > offsetInData ? offsetInData : 0;
            var dataToSendSize = _data.Length - offset > _subsequentSegmentPayloadSize ? _subsequentSegmentPayloadSize : _data.Length - offset;
            var dataToSend = new byte[dataToSendSize];
            Array.Copy(_data, offset, dataToSend, 0, dataToSendSize);

            var ret = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT();
            ret.properties1.datagramSize1 = (byte)(((ushort)_data.Length) >> 8);
            ret.datagramSize2 = (byte)_data.Length;
            ret.properties2.datagramOffset1 = (byte)(((ushort)offset) >> 8);
            ret.datagramOffset2 = (byte)offset;
            ret.properties2.ext = 0;
            ret.properties2.sessionId = _sessionId;
            ret.payload = new List<byte>(dataToSend);
            if (_transportServiceManagerInfo.TestOffset.PullValue(
                val =>
                {
                    return val >= offset && val <= (offset + _subsequentSegmentPayloadSize);
                }) != null &&
                _transportServiceManagerInfo.TestSubsequentFragmentCRC16.CanBeUsed
                )
            {
                ret.frameCheckSequence = _transportServiceManagerInfo.TestSubsequentFragmentCRC16.PullValue();
            }
            else
            {
                ret.frameCheckSequence = CalculateCRC16Checksum(ret);
            }
            return ret;
        }

        private byte[] CreateFirstSegmentCmd(byte[] data)
        {
            _dataOffset = 0;
            var dataToSend = new byte[_firstSegmentPayloadSize];
            Array.Copy(_data, dataToSend, _firstSegmentPayloadSize);
            _dataOffset += _firstSegmentPayloadSize;
            var firstSegmentCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT();
            firstSegmentCmd.properties1.datagramSize1 = (byte)(((ushort)_data.Length) >> 8);
            firstSegmentCmd.datagramSize2 = (byte)_data.Length;
            firstSegmentCmd.properties2.sessionId = _sessionId;
            firstSegmentCmd.properties2.ext = 0;
            firstSegmentCmd.payload = new List<byte>(dataToSend);
            if (_transportServiceManagerInfo.TestFirstFragmentCRC16.CanBeUsed)
            {
                firstSegmentCmd.frameCheckSequence = _transportServiceManagerInfo.TestFirstFragmentCRC16.PullValue();
            }
            else
            {
                firstSegmentCmd.frameCheckSequence = CalculateCRC16Checksum(firstSegmentCmd);
            }
            return firstSegmentCmd;
        }

        private byte[] CalculateCRC16Checksum(byte[] cmd)
        {
            ushort crc = Tools.ZW_CreateCrc16(null, 0, cmd, (byte)(cmd.Length - 2));
            return new byte[] { (byte)(crc >> 8), (byte)crc };
        }
    }
}
