using ZWave.Layers;
using ZWave.Enums;
using Utils;
using System;

namespace ZWaveTests
{
    public class FakeTransportClient : TransportClientBase
    {
        public override event Action<ITransportClient> Connected
        {
            add { throw new NotSupportedException(); }
            remove { }
        }
        public override event Action<ITransportClient> Disconnected
        {
            add { throw new NotSupportedException(); }
            remove { }
        }

        public bool InnerConnectCalled { get; private set; }
        public bool InnerDisconnectCalled { get; private set; }
        public bool InnerWriteDataCalled { get; private set; }
        public bool InnerDisposeCalled { get; private set; }

        private bool _isOpen;
        public override bool IsOpen
        {
            get { return _isOpen; }
        }

        protected override CommunicationStatuses InnerConnect(IDataSource dataSource)
        {
            _isOpen = true;
            InnerConnectCalled = true;
            return CommunicationStatuses.Done;
        }

        protected override void InnerDisconnect()
        {
            _isOpen = false;
            InnerDisconnectCalled = true;
        }

        protected override int InnerWriteData(byte[] data)
        {
            InnerWriteDataCalled = true;
            return data.Length;
        }

        protected override void InnerDispose()
        {
            InnerDisposeCalled = true;
        }
    }
}
