using System;
using System.Linq;
using ZWave.Layers;
using ZWave.ProgrammerApplication.Enums;
using ZWave.Layers.Frame;
using ZWave.Enums;
using System.Threading;

namespace ZWave.ProgrammerApplication
{
    public class ProgrammerFrameClient : IFrameClient
    {
        private const byte MAX_FRAME_SIZE = 255;
        private const byte MIN_FRAME_SIZE = 3;
        private const byte MAX_FRAME_PARSE_TIME_MS = 200;

        public ushort SessionId { get; set; }
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }

        public DataFrame ReceivingDataFrame { get; set; }
        public int ReceivingDataFrameLength { get; set; }
        private int mReceivingDataFrameLengthCounter;
        private readonly byte[] mReceivingDataFrameBuffer = new byte[MAX_FRAME_SIZE];
        private FrameReceiveStates mParserState;
        private ManualResetEvent mDataReadyEvent = new ManualResetEvent(true);
        private bool mIsParseEnabled;
        public bool IsParseEnabled
        {
            get { return mIsParseEnabled; }
            set
            {
                mIsParseEnabled = value;
                //Tools._writeDebugDiagnosticMessage(mIsParseEnabled.ToString(), true, true, 1);
            }
        }

        private readonly Action<DataFrame> mTransmitCallback;
        public ProgrammerFrameClient(Action<DataFrame> transmitCallback)
        {
            mTransmitCallback = transmitCallback;
            IsParseEnabled = true;
        }

        private byte[] CreateBuffer(CommandMessage frame)
        {
            byte[] data = frame.Data;
            if (frame.IsSequenceNumberRequired)
            {
                data = new byte[frame.Data.Length + 1];
                Array.Copy(frame.Data, 0, data, 0, frame.Data.Length);
                data[frame.Data.Length] = frame.SequenceNumber;
            }
            byte[] tmp = DataFrame.CreateFrameBuffer(data);
            return tmp;
        }

        private void UnblockSending()
        {
            //Tools._writeDebugDiagnosticMessage("", true, true, 1);
            mDataReadyEvent.Set();
        }

        private void BlockSending(byte value)
        {
            //Tools._writeDebugDiagnosticMessage(value.ToString("X2"), true, true, 1);
            mDataReadyEvent.Reset(); // block sending frames during parse incoming frame
        }

        private void WriteDataSafe(byte[] data)
        {
            //Tools._writeDebugDiagnosticMessage("request", true, true, 1);
            if (mTransmitCallback != null)
            {
                DataFrame dataFrame = new DataFrame(SessionId, DataFrameTypes.Data, false, true, DateTime.Now);
                dataFrame.SetBuffer(data, 0, data.Length);
                mTransmitCallback(dataFrame);
            }
            mDataReadyEvent.WaitOne(MAX_FRAME_PARSE_TIME_MS);
            //Tools._writeDebugDiagnosticMessage("allowed", true, true, 1);
            ResetParser();
            WriteData(data);
        }

        private int WriteData(byte[] data)
        {
            if (SendDataCallback != null)
                return SendDataCallback(data);
            else
                return -1;
        }


        public void ResetParser()
        {
            mParserState = FrameReceiveStates.FRS_SOF_HUNT;
            ResetReceivingDataFrameBuffer();
            UnblockSending();
        }

        private int AddToReceivingDataFrameBuffer(byte buffer)
        {
            if (mReceivingDataFrameLengthCounter < MAX_FRAME_SIZE)
            {
                mReceivingDataFrameBuffer[mReceivingDataFrameLengthCounter] = buffer;
                mReceivingDataFrameLengthCounter++;
                return 1;
            }
            else return 0;
        }

        private void ResetReceivingDataFrameBuffer()
        {
            mReceivingDataFrameLengthCounter = 0;
        }

        public bool SendFrames(ActionHandlerResult frameData)
        {
            if (frameData != null && frameData.NextActions != null)
            {
                var sendFrames = frameData.NextActions.Where(x => x is CommandMessage);
                if (sendFrames.Any())
                {
                    foreach (CommandMessage item in sendFrames)
                    {
                        if (item.Data != null)
                        {
                            byte[] data = CreateBuffer(item);
                            WriteDataSafe(data);
                            frameData.Parent.AddTraceLogItem(DateTime.Now, data, true);
                        }
                    }
                }
                frameData.NextFramesCompletedCallback?.Invoke(true);
            }
            return true;
        }

        public void HandleData(DataChunk dataChunk, bool isFromFile)
        {
            if (dataChunk.ApiType == ApiTypes.Programmer)
            {
                byte[] data = dataChunk.GetDataBuffer();
                if (data != null && data.Length > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        ParseRawData(data[i], dataChunk.IsOutcome, dataChunk.TimeStamp, isFromFile);
                    }
                }
            }
        }

        private byte[] ParseRawData(byte buffer, bool isOutcome, DateTime timeStamp, bool isFromFile)
        {
            byte[] ret = null;
            AddToReceivingDataFrameBuffer(buffer);
            switch (mParserState)
            {
                case FrameReceiveStates.FRS_SOF_HUNT:
                    switch ((CommandTypes)buffer)
                    {
                        case CommandTypes.EnableInterface:
                        case CommandTypes.ReadFlash:
                        case CommandTypes.ReadSRAM:
                        case CommandTypes.ContinueRead:
                        case CommandTypes.WriteSRAM:
                        case CommandTypes.ContinueWriteSRAM:
                        case CommandTypes.EraseChip:
                        case CommandTypes.EraseSector:
                        case CommandTypes.WriteFlashSector:
                        case CommandTypes.CheckState:
                        case CommandTypes.ReadSignatureByte:
                        case CommandTypes.DisableEooSMode:
                        case CommandTypes.EnableEooSMode:
                        case CommandTypes.SetLockBits:
                        case CommandTypes.ReadLockBits:
                        case CommandTypes.SetNvrByte:
                        case CommandTypes.ReadNvrByte:
                        case CommandTypes.RunCrcCheck:
                        case CommandTypes.ResetChip:
                        case CommandTypes.SpiFwSetPin:
                        case CommandTypes.SpiFwToggleSck:
                        case CommandTypes.SpiFwInit:
                        case CommandTypes.SpiFwGetVersion:
                            if (IsParseEnabled)
                            {
                                ResetReceivingDataFrameBuffer();
                                AddToReceivingDataFrameBuffer(buffer);
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                mParserState = FrameReceiveStates.FRS_DATA;
                                ReceivingDataFrameLength = 4;
                                BlockSending(buffer);
                            }
                            else
                            {
                                ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                                ReceivingDataFrame.SetBuffer(new byte[] { buffer }, 0, 1);
                                OnFrameReceived();
                            }
                            break;
                        default:
                            ReceivingDataFrame = new DataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                            ReceivingDataFrame.SetBuffer(new byte[] { buffer }, 0, 1);
                            OnFrameReceived();
                            break;
                    }
                    break;
                case FrameReceiveStates.FRS_DATA:
                    if (mReceivingDataFrameLengthCounter >= ReceivingDataFrameLength)
                    {
                        mParserState = FrameReceiveStates.FRS_SOF_HUNT;
                        UnblockSending();
                    }
                    else if (mReceivingDataFrameLengthCounter == ReceivingDataFrameLength - 1)
                        mParserState = FrameReceiveStates.FRS_EOF;
                    break;
                case FrameReceiveStates.FRS_EOF:
                    // Frame received successfully -> Send acknowledge (ACK)
                    ReceivingDataFrame.SetBuffer(mReceivingDataFrameBuffer, 0, mReceivingDataFrameLengthCounter);
                    mParserState = FrameReceiveStates.FRS_SOF_HUNT;
                    UnblockSending();
                    OnFrameReceived();
                    break;
                default:
                    mParserState = FrameReceiveStates.FRS_SOF_HUNT;
                    UnblockSending();
                    break;
            }
            return ret;
        }

        private void OnFrameReceived()
        {
            if (ReceivingDataFrame != null)
            {
                if (mTransmitCallback != null && ReceivingDataFrame.DataFrameType == DataFrameTypes.Data)
                    mTransmitCallback(ReceivingDataFrame);
                ReceiveFrameCallback?.Invoke(ReceivingDataFrame);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (mDataReadyEvent != null)
            {
                mDataReadyEvent.Close();
            }
        }

        #endregion
    }
}
