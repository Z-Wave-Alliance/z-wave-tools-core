using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.Enums;

namespace ZWaveTests
{
    public class FakeApplicationLayer : ApplicationLayer
    {
        public FakeApplicationLayer(ApiTypes apiType, ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer) :
            base(apiType, sessionLayer, frameLayer, transportLayer)
        {
        }
    }
}
