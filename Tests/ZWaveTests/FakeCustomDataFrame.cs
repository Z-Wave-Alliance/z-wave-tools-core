using System;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWaveTests
{
    class FakeCustomDataFrame : CustomDataFrame
    {
        public FakeCustomDataFrame(byte sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp) :
            base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
        }

        public bool GetMaxLengthCalled { get; private set; }
        protected override int GetMaxLength()
        {
            GetMaxLengthCalled = true;
            return Buffer.Length;
        }

        public bool RefreshDataCalled { get; private set; }
        protected override byte[] RefreshData()
        {
            RefreshDataCalled = true;
            return Buffer;
        }

        public bool RefreshPayloadCalled { get; private set; }
        protected override byte[] RefreshPayload()
        {
            RefreshPayloadCalled = true;
            return Buffer;
        }
    }
}
