using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class EnableZnifferApmOperation : ActionBase
    {
        public EnableZnifferApmOperation()
            : base(true)
        {
        }

        HandlerStates handlerState = HandlerStates.WaitingH1;
        CommandHandler handler1;
        CommandHandler handler2;
        CommandHandler handler3;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, message));
            ActionUnits.Add(new DataReceivedUnit(handler1, OnHandler1));
            ActionUnits.Add(new DataReceivedUnit(handler2, OnHandler2));
            ActionUnits.Add(new DataReceivedUnit(handler3, OnHandler3));
            StopActionUnit = new StopActionUnit(OnExpired, 0);
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData(0x23, 0x12, 0x00);

            handler1 = new CommandHandler();
            handler1.AddConditions(new ByteIndex(0x23));

            handler2 = new CommandHandler();
            handler2.AddConditions(new ByteIndex(0x12));

            handler3 = new CommandHandler();
            handler3.AddConditions(new ByteIndex(0x01));

        }

        private void OnHandler1(DataReceivedUnit ou)
        {
            if (handlerState == HandlerStates.WaitingH1)
            {
                handlerState = HandlerStates.WaitingH2;
            }
        }

        private void OnHandler2(DataReceivedUnit ou)
        {
            if (handlerState == HandlerStates.WaitingH2)
            {
                handlerState = HandlerStates.WaitingH3;
            }
        }

        private void OnHandler3(DataReceivedUnit ou)
        {
            if (handlerState == HandlerStates.WaitingH3)
            {
                handlerState = HandlerStates.Done;
                SetStateCompleted(ou);
            }
        }

        private void OnExpired(IActionUnit ou)
        {
            if (handlerState != HandlerStates.WaitingH1)
                SetStateCompleted(ou);
        }

        private enum HandlerStates
        {
            WaitingH1,
            WaitingH2,
            WaitingH3,
            Done
        }
    }
}
