using System;
using ZWave.Layers.Frame;
using ZWave.Enums;

namespace ZWaveTests
{
    public class FakeDataFrame : CustomDataFrame
    {
        public FakeDataFrame(byte sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp) :
            base(sessionId, type, isHandled, isOutcome, timeStamp)
        {
        }

        protected override int GetMaxLength()
        {
            throw new NotImplementedException();
        }

        protected override byte[] RefreshData()
        {
            throw new NotImplementedException();
        }

        protected override byte[] RefreshPayload()
        {
            throw new NotImplementedException();
        }
    }
}
