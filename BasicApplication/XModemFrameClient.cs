/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Utils;
using System.Text;
using ZWave.Layers;
using ZWave.Layers.Frame;
using ZWave.BasicApplication.Enums;
using System.Text.RegularExpressions;

namespace ZWave.BasicApplication
{
    [Flags]
    internal enum XModemClientStates : byte
    {
        NonStarted = 0x00,
        OptionsMsgReceived = 0x01,
        BeginUploadMsgReceived = 0x02,
        C_startReceived = 0x04,
        C_confirmedReceived = 0x08,
        EotSent = 0x10,
        EotConfirmed = 0x20,
        Canceled = 0x40
    }

    internal enum XModemErrorCodes
    {
        KeyValidation = 0x50,
        BrokenFirmwareData = 0x21
    }

    public class XModemFrameClient : IFrameClient
    {
        private byte _packetNo;
        private XModemClientStates _currentState;
        private bool _hasTransmitFails;
        private int _runAppRetryCount;
        private AutoResetEvent _uploadStateSignal = new AutoResetEvent(false);
        private List<byte> _cancelationOutput = new List<byte>();

        private const XModemClientStates ReadyForUpload = XModemClientStates.OptionsMsgReceived | XModemClientStates.BeginUploadMsgReceived |
                    XModemClientStates.C_startReceived | XModemClientStates.C_confirmedReceived;
        private const XModemClientStates Completed = ReadyForUpload | XModemClientStates.EotSent | XModemClientStates.EotConfirmed;

        private string _cancelationMessage;
        public string CancelationMessage
        {
            get
            {
                return string.IsNullOrEmpty(_cancelationMessage) ? "XModem Firmware Update failed." : _cancelationMessage;
            }

            private set
            {
                _cancelationMessage = value;
                var regexPattern = new Regex("0x(?<byte>[0-9A-Fa-f]{2})");
                var hexMatches = regexPattern.Matches(_cancelationMessage);
                if (hexMatches.Count > 0)
                {
                    try
                    {
                        ErrorCode = Convert.ToInt32(hexMatches[0].ToString(), 16);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    ErrorCode = 0;
                }
            }
        }

        public int ErrorCode { get; private set; }

        public bool IsCompleted { get { return _currentState == Completed && !_hasTransmitFails; } }

        internal bool IsReady { get { return _currentState == ReadyForUpload; } }

        public ushort SessionId { get; set; }
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }
        public XModemDataFrame ReceivingDataFrame { get; set; }
        public FrameBufferBlock FrameBufferBlock { get; set; }

        public XModemFrameClient()
        {
            FrameBufferBlock = new FrameBufferBlock(WriteData, cmdMsg => new XModemDataFrame(_packetNo++, cmdMsg.Data));
        }

        public bool WaitReady(int timeout)
        {
            if (_currentState < ReadyForUpload)
            {
                _uploadStateSignal.WaitOne(timeout);
            }
            return IsReady;
        }

        internal void TestSetReady()
        {
            _currentState = ReadyForUpload;
            _uploadStateSignal.Set();
        }

        public bool CloseSession(int timeout)
        {
            switch (_currentState)
            {
                case ReadyForUpload:
                    _currentState |= XModemClientStates.EotSent;
                    WriteData(new byte[] { (byte)XModemRecieverTransmisionStatuses.EOT });
                    _uploadStateSignal.WaitOne(timeout);
                    break;
                case XModemClientStates.Canceled:
                    _uploadStateSignal.WaitOne(timeout);
                    CancelationMessage = ParseCancelationOutput(_cancelationOutput);
                    break;
            }
            return IsCompleted;
        }

        public bool SendFrames(ActionHandlerResult frameData)
        {
            bool ret = false;
            if (IsReady)
            {
                ret = FrameBufferBlock.Send(frameData);
                if (!_hasTransmitFails && !ret)
                {
                    _hasTransmitFails = true;
                }
            }
            return ret;
        }

        public void HandleData(DataChunk dataChunk, bool isFromFile)
        {

            byte[] data = dataChunk.GetDataBuffer();
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    ParseRawData(data[i], dataChunk.IsOutcome, dataChunk.TimeStamp, isFromFile);
                }
            }
        }

        public void ResetParser()
        {
        }

        private int WriteData(byte[] data)
        {
            return SendDataCallback != null ? SendDataCallback(data) : -1;
        }

        private byte[] ParseRawData(byte val, bool isOutcome, DateTime timeStamp, bool isFromFile)
        {
            switch (_currentState)
            {
                case 0x00:
                    if (val == 0x3E) // '>'
                    {
                        "ParseRawData: Bootloader msg received"._DLOG();
                        _currentState |= XModemClientStates.OptionsMsgReceived;
                        _cancelationOutput.Clear();
                        CancelationMessage = string.Empty;
                        _packetNo = 1;
                        WriteData(new[] { (byte)XModemRunOptions.UploadGbl });
                    }
                    break;
                case XModemClientStates.OptionsMsgReceived:
                    if (val == 0x64) // 'd'
                    {
                        "ParseRawData: 'begin upload' received"._DLOG();
                        _currentState |= XModemClientStates.BeginUploadMsgReceived;
                    }
                    break;
                case XModemClientStates.OptionsMsgReceived | XModemClientStates.BeginUploadMsgReceived:
                    if (val == (byte)XModemHeaderTypes.C)
                    {
                        "ParseRawData: C-start received"._DLOG();
                        _currentState |= XModemClientStates.C_startReceived;
                    }
                    break;
                case XModemClientStates.OptionsMsgReceived | XModemClientStates.BeginUploadMsgReceived | XModemClientStates.C_startReceived:
                    if (val == (byte)XModemHeaderTypes.C)
                    {
                        $"ParseRawData: C-confirm received"._DLOG();
                        _currentState |= XModemClientStates.C_confirmedReceived;
                        _runAppRetryCount = 1;
                        _uploadStateSignal.Set();
                    }
                    break;
                case ReadyForUpload:
                    var parserState = (XModemHeaderTypes)val;
                    if (parserState == XModemHeaderTypes.ACK)
                    {
                        FrameBufferBlock.Acknowledge(true);
                    }
                    else if (parserState == XModemHeaderTypes.NACK)
                    {
                        "ParseRawData: Bootloader NACK received"._DLOG();
                        FrameBufferBlock.Acknowledge(false);
                    }
                    else if (parserState == XModemHeaderTypes.CAN)
                    {
                        "ParseRawData: Bootloader CAN received"._DLOG();
                        _currentState = XModemClientStates.Canceled;
                        FrameBufferBlock.Acknowledge(false);
                    }
                    break;
                case ReadyForUpload | XModemClientStates.EotSent:
                    if (val == (byte)XModemHeaderTypes.ACK)
                    {
                        _currentState |= XModemClientStates.EotConfirmed;
                    }
                    break;
                case Completed:
                    if (val == 0x3E && _runAppRetryCount++ < 3) // '>'
                    {
                        "ParseRawData: run uploaded app"._DLOG();
                        WriteData(new[] { (byte)XModemRunOptions.Run });
                        _uploadStateSignal.Set();
                    }
                    break;
                case XModemClientStates.Canceled:
                    _cancelationOutput.Add(val);
                    if (val == 0x3E && _runAppRetryCount++ < 3) // '>'
                    {
                        "ParseRawData: upload failed, run old app"._DLOG();
                        WriteData(new[] { (byte)XModemRunOptions.Run });
                        _uploadStateSignal.Set();
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        private string ParseCancelationOutput(List<byte> cancelationOutput)
        {
            while (cancelationOutput.Count > 0 && cancelationOutput.First() == (byte)XModemHeaderTypes.CAN)
            {
                cancelationOutput.RemoveAt(0);
            }
            if (cancelationOutput.Count > 0)
            {
                var stringOutput = Encoding.ASCII.GetString(cancelationOutput.ToArray()).Trim();
                var optionsMsgInd = stringOutput.IndexOf("Gecko Bootloader");
                if (optionsMsgInd > 0)
                {
                    stringOutput = stringOutput.Substring(0, optionsMsgInd);
                    stringOutput = stringOutput.Replace("\0", string.Empty);
                    stringOutput = stringOutput.Trim(new char[] { '\n', '\r' });
                    stringOutput = stringOutput.Replace("\r\n\r\n", " ");
                }
                return stringOutput;
            }
            return string.Empty;
        }

        #region IDisposable Members

        private bool _isDisposed = false;
        private readonly object _locker = new object();
        public void Dispose()
        {
            FrameBufferBlock.Dispose();
            lock (_locker)
            {
                if (_isDisposed)
                {
                    return;
                }
                _isDisposed = true;
                _uploadStateSignal.Close();
            }
        }

        #endregion
    }
}
