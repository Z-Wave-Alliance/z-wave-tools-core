using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using ZWave.Layers;

namespace BasicApplicationTests.XModem
{
    internal enum XModemDeviceStates
    {
        BootloaderNotInited = 0,
        OptionsAvailable,
        UploadingFirmware,
    }

    public class FakeXModemTransportClient : TransportClientBase
    {
        private const int MIN_DATA_FRAME_SIZE = 6;

        private readonly byte[] _bootloaderMenu = Encoding.ASCII.GetBytes("Gecko Bootloader v1.5.1\n1. upload gbl\n2. run\n3. ebl info\nBL >");
        private readonly byte[] _beginUpload = Encoding.ASCII.GetBytes("begin upload");
        private readonly byte[] _errorMessage = Encoding.ASCII.GetBytes("error code 0xFE\n");
        public byte[] InitFrame { get { return new byte[] { 0x01, 0x03, 0x00, 0x27, 0xdb }; } }
        
        public List<byte> ReceivedPayload { get; } = new List<byte>();
        public List<byte> ReceivedFrames { get; } = new List<byte>();
        private CancellationTokenSource _ctsSendingC = new CancellationTokenSource();
        private readonly DataChunk _errorMessageDataChunk;
        private readonly DataChunk _responceOptionsDataChunk;
        private readonly DataChunk _beginUploadDataChunk;
        private readonly DataChunk _ACK = new DataChunk(new byte[] { (byte)XModemHeaderTypes.ACK }, 0, true, ApiTypes.XModem);
        private readonly DataChunk _NACK = new DataChunk(new byte[] { (byte)XModemHeaderTypes.NACK }, 0, true, ApiTypes.XModem);
        private readonly DataChunk _CAN = new DataChunk(new byte[] { (byte)XModemHeaderTypes.CAN }, 0, true, ApiTypes.XModem);
        private readonly DataChunk _C = new DataChunk(new byte[] { (byte)XModemHeaderTypes.C }, 0, true, ApiTypes.XModem);

#pragma warning disable CS0067
        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;
#pragma warning restore CS0067

        public FakeXModemTransportClient()
        {
            _responceOptionsDataChunk = new DataChunk(_bootloaderMenu, 0, true, ApiTypes.XModem);
            _beginUploadDataChunk = new DataChunk(_beginUpload, 0, true, ApiTypes.XModem);
            _errorMessageDataChunk = new DataChunk(_errorMessage, 0, true, ApiTypes.XModem);
            CanStart = true;
        }
        public override bool IsOpen => true;

        public int ReceivedFramesCount { get; private set; }
        public bool IsEotReceived { get; private set; }
        public int RunAppReceivedCount { get; private set; }

        public bool CanStart { get; set; }

        private int _nackCount;

        private int? _canOnFrameNo;
        public void SendCanAfter(int receivedFrameNo)
        {
            _canOnFrameNo = receivedFrameNo;
        }

        private int? _nackOnFrameNo;
        public void SendNackAfter(int receivedFrameNo)
        {
            _nackOnFrameNo = receivedFrameNo;
        }

        private int? _skipedAcksFrameNo;
        private int? _skipedAcksCount;
        public void SkipAcksAfter(int receivedFrameNo, int acksCount)
        {
            _skipedAcksFrameNo = receivedFrameNo;
            _skipedAcksCount = acksCount;
        }

        public void SetUploadNewFirmwareState()
        {
            _xmodemDeviceStates = XModemDeviceStates.UploadingFirmware;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource dataSource)
        {
            return CommunicationStatuses.Done;
        }

        protected override void InnerDisconnect()
        {
        }

        protected override void InnerDispose()
        {
        }

        private bool IsDataRequest(byte[] data)
        {
            return data.Length >= MIN_DATA_FRAME_SIZE && (data[0] == (byte)XModemRecieverTransmisionStatuses.SOH);
        }

        private bool IsEndOfTransfer(byte[] data)
        {
            return data.Length == 1 && (data[0] == (byte)XModemRecieverTransmisionStatuses.EOT || data[0] == (byte)XModemRecieverTransmisionStatuses.ETB);
        }

        private XModemDeviceStates _xmodemDeviceStates = XModemDeviceStates.BootloaderNotInited;
        protected override int InnerWriteData(byte[] data)
        {
            switch (_xmodemDeviceStates)
            {
                case XModemDeviceStates.BootloaderNotInited:
                    if (data.SequenceEqual(InitFrame))
                    {
                        _xmodemDeviceStates = XModemDeviceStates.OptionsAvailable;
                        ReceiveDataCallback(_responceOptionsDataChunk, true);
                    }
                    break;
                case XModemDeviceStates.OptionsAvailable:
                    if (data[0] == (byte)XModemRunOptions.UploadGbl) // '1' - upload gbl.
                    {
                        _xmodemDeviceStates = XModemDeviceStates.UploadingFirmware;
                        RunAppReceivedCount = 0;
                        ReceiveDataCallback(_beginUploadDataChunk, true);
                        Task.Factory.StartNew(() =>
                        {
                            int c = 0;
                            while (!_ctsSendingC.IsCancellationRequested && c++ < 20)
                            {
                                ReceiveDataCallback(_C, true);
                                Task.Delay(1000).Wait();
                            }
                        });
                    }
                    else if (data[0] == (byte)XModemRunOptions.Run) // '2' - run block.
                    {
                        RunAppReceivedCount++;
                        if (CanStart)
                        {
                            _xmodemDeviceStates = XModemDeviceStates.BootloaderNotInited;
                        }
                        else
                        {
                            ReceiveDataCallback(_responceOptionsDataChunk, true);
                        }
                    }
                    break;
                case XModemDeviceStates.UploadingFirmware: // Data uploaded block.
                    if (!_ctsSendingC.IsCancellationRequested)
                    {
                        _ctsSendingC.Cancel();
                    }
                    if (IsDataRequest(data))
                    {
                        ReceivedFramesCount++;
                        ReceivedFrames.AddRange(data);

                        if (_nackOnFrameNo.HasValue && ReceivedFramesCount == _nackOnFrameNo) // NACK
                        {
                            ReceiveDataCallback(_NACK, true);
                        }
                        else if (_canOnFrameNo.HasValue && ReceivedFramesCount == _canOnFrameNo) // CAN
                        {
                            _xmodemDeviceStates = XModemDeviceStates.OptionsAvailable;
                            ReceiveDataCallback(_CAN, true);
                            ReceiveDataCallback(_CAN, true);
                            ReceiveDataCallback(_CAN, true);
                            ReceiveDataCallback(_errorMessageDataChunk, true);
                            ReceiveDataCallback(_responceOptionsDataChunk, true);
                        }
                        else if (!ValidateData(data, out byte[] payload)) // Invalid payload.
                        {
                            if (_nackCount++ <= 3)
                            {
                                ReceiveDataCallback(_NACK, true);
                            }
                            else
                            {
                                ReceiveDataCallback(_CAN, true);
                            }
                        }
                        else // ACK.
                        {
                            _nackCount = 0;
                            if (!(_skipedAcksFrameNo.HasValue && ReceivedFramesCount >= _skipedAcksFrameNo && _skipedAcksCount-- > 0))
                            {
                                ReceivedPayload.AddRange(payload);
                                ReceiveDataCallback(_ACK, true);
                            }
                        }
                    }
                    else if (IsEndOfTransfer(data)) // EOT block.
                    {
                        IsEotReceived = true;
                        _xmodemDeviceStates = XModemDeviceStates.OptionsAvailable;
                        ReceiveDataCallback(_ACK, true);
                        ReceiveDataCallback(_responceOptionsDataChunk, true);
                    }
                    break;
                default:
                    break;
            }
            return data.Length;
        }

        private bool ValidateData(byte[] frame, out byte[] payload)
        {
            payload = new byte[frame.Length - (XModemDataFrame.HeaderLength + XModemDataFrame.CrcChecksumLength)];
            Array.Copy(frame, XModemDataFrame.HeaderLength, payload, 0, payload.Length);
            return frame.Length == XModemDataFrame.FrameLength &&
                frame[0] == (byte)XModemRecieverTransmisionStatuses.SOH &&
                frame[1] == (byte)~frame[2] &&
                XModemDataFrame.IsChecksumValid(payload, frame.Skip(XModemDataFrame.HeaderLength + payload.Length).ToArray());
        }
    }
}
