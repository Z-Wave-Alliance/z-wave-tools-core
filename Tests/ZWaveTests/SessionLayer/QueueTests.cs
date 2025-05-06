using Moq;
using NUnit.Framework;
using ZWave;
using ZWave.Layers.Session;


namespace ZWaveTests.Session
{
    [TestFixture]
    public class QueueTests
    {
        public void ActionExpired_Handle()
        {
            using (var scl = new SessionClient(null))
            {
                var moqAction = new Mock<IActionItem>();
                

                scl.ExecuteAsync(moqAction.Object);


            }


            //var sc = new Mock<ISessionClient>();
            //sc.SetupProperty(y => y.SessionId);
            //sc.SetupProperty(y => y.IsHandleFrameEnabled);
            //sc.Setup(y => y
            //    .ExecuteAsync(It.Is<IActionItem>(f => f is ActionBase)))
            //        .Returns((IActionItem a) => (a as ActionBase)?.Token);



            //sc.Object.ExecuteAsync(action);
            //sc.Object.TokenExpired(token);

        }
    }
}
