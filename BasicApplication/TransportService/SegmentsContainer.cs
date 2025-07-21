/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZWave.CommandClasses;

namespace ZWave.BasicApplication.TransportService
{
    public class SegmentsContainer
    {
        private Dictionary<ushort, byte[]> _segments;
        /// <summary>
        /// For test purposes only
        /// </summary>
        internal ReadOnlyCollection<byte[]> Segments { get { return Array.AsReadOnly(_segments.Values.ToArray()); } }

        /// <summary>
        /// Returns session id for current container
        /// </summary>
        public byte SessionId { get; private set; }

        /// <summary>
        /// Returns number of segments needed to complete datagram
        /// </summary>
        public byte PendingSegmentsCount
        {
            get
            {
                if (_segments.Count > 0)
                {
                    var segmentSize = _segments.Values.First().Length;
                    return (byte)(_datagramSize / segmentSize + 1);
                }
                return 1;
            }
        }

        /// <summary>
        /// Returns datagram size that is expected to get
        /// </summary>
        private ushort _datagramSize;
        public ushort DatagramSize { get { return _datagramSize; } }

        /// <summary>
        /// Indicates that last segment by offset has been added to container
        /// </summary>
        private bool _isLastSegmentReceived;
        public bool IsLastSegmentReceived { get { return _isLastSegmentReceived; } }

        /// <summary>
        /// Indicates that all expected segments have been received
        /// </summary>
        private bool _isCompleted;
        public bool IsCompleted { get { return _isCompleted; } }

        /// <summary>
        /// Returns true if given datagram is last segment
        /// </summary>
        private byte[] _lastSegment;
        public bool CheckForLastSegment(byte[] segmentData)
        {
            if (_lastSegment != null)
            {
                return _lastSegment.SequenceEqual(segmentData);
            }
            return false;
        }

        public SegmentsContainer(byte[] firstSegment)
        {
            _segments = new Dictionary<ushort, byte[]>();
            AddFirstSegment(firstSegment);
        }

        private void AddFirstSegment(byte[] firstSegment)
        {
            var firstSegmentCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT)firstSegment;
            int payloadOffset = 4;
            SessionId = firstSegmentCmd.properties2.sessionId;
            if (firstSegmentCmd.properties2.ext == 1)
            {
                payloadOffset += sizeof(byte);
                payloadOffset += firstSegmentCmd.headerExtensionLength;
            }

            ushort datagramSizeHighPart = firstSegmentCmd.properties1.datagramSize1;
            _datagramSize = firstSegmentCmd.datagramSize2;
            _datagramSize = (ushort)(_datagramSize | (datagramSizeHighPart << 8));
            int payloadLength = firstSegment.Length - payloadOffset - firstSegmentCmd.frameCheckSequence.Length;
            var payload = new byte[payloadLength];
            Array.Copy(firstSegmentCmd.payload.ToArray(), payload, payloadLength);
            _segments.Add(0, payload);

            if (payload.Length == _datagramSize)
            {
                _lastSegment = firstSegment;
                _isLastSegmentReceived = true;
                _isCompleted = true;
            }
        }

        /// <summary>
        /// Adds one of datagram's segment to container
        /// </summary>
        /// <param name="segment">Segment data</param>
        /// <returns>Indicates if segment has been added</returns>
        public bool AddSegment(byte[] segment)
        {
            if (_isCompleted)
            {
                return false;
            }

            var subsequentSegmentCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT)segment;
            byte sessionId = subsequentSegmentCmd.properties2.sessionId;
            if (sessionId != SessionId)
            {
                return false;
            }

            ushort datagramOffset = GetSegmentOffset(segment);
            int payloadLength = GetSegmentPayloadLength(segment);
            var payload = new byte[payloadLength];
            Array.Copy(subsequentSegmentCmd.payload.ToArray(), payload, payloadLength);

            if (!_segments.ContainsKey(datagramOffset))
            {
                _segments.Add(datagramOffset, payload);
            }

            if (!_isLastSegmentReceived && _datagramSize == datagramOffset + (ushort)payload.Length)
            {
                _isLastSegmentReceived = true;
                _lastSegment = segment;
            }

            if (_isLastSegmentReceived)
            {
                _isCompleted = (0 == GetFirstMissingFragmentOffset());
            }

            return true;
        }

        /// <summary>
        /// Gets offset value from subsequent segment frame
        /// </summary>
        /// <param name="segment">Subsequent segment frame data</param>
        /// <returns>Offset value</returns>
        public static ushort GetSegmentOffset(byte[] segment)
        {
            ushort ret = 0;
            var subsequentSegmentCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT)segment;
            ushort datagramOffsetHighPart = subsequentSegmentCmd.properties2.datagramOffset1;
            ret = subsequentSegmentCmd.datagramOffset2;
            ret = (ushort)(ret | (datagramOffsetHighPart << 8));
            return ret;
        }

        /// <summary>
        /// Gets subsequent segment's payload data length
        /// </summary>
        /// <param name="segment">Subsequent segment frame data</param>
        /// <returns>Payload data length</returns>
        public static int GetSegmentPayloadLength(byte[] segment)
        {
            int ret = 0;
            var subsequentSegmentCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SUBSEQUENT_SEGMENT)segment;
            int payloadOffset = 5;
            if (subsequentSegmentCmd.properties2.ext == 1)
            {
                payloadOffset += sizeof(byte);
                payloadOffset += subsequentSegmentCmd.headerExtensionLength;
            }
            ret = segment.Length - payloadOffset - subsequentSegmentCmd.frameCheckSequence.Length;
            return ret;
        }

        /// <summary>
        /// Returns full datagram
        /// </summary>
        /// <returns>Datagram data or null in case it's not complete</returns>
        public byte[] GetDatagram()
        {
            if (!_isCompleted)
            {
                return null;
            }
            var dataList = new List<byte>();
            var keysArray = _segments.Keys.ToArray();
            Array.Sort(keysArray);
            foreach (var offset in keysArray)
            {
                dataList.AddRange(_segments[offset]);
            }
            return dataList.ToArray();
        }

        /// <summary>
        /// Returns first missing datagram's fragment offset
        /// </summary>
        /// <returns>Datagram offset value or 0 if datagram is complete</returns>
        public ushort GetFirstMissingFragmentOffset()
        {
            if (_isCompleted)
            {
                return 0;
            }

            ushort missingOffset = 0;
            while (missingOffset < _datagramSize)
            {
                if (!_segments.ContainsKey(missingOffset))
                {
                    return missingOffset;
                }
                missingOffset += (ushort)_segments[missingOffset].Length;
            }
            return 0;
        }
    }
}
