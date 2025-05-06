using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class EnableBasicApmOperation : ActionBase
    {
        public EnableBasicApmOperation() 
            : base(true)
        {
        }

        CommandHandler handler;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData(0x01, 0x03, 0x00, 0x27, 0xDB);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex(0x06));
        }
    }
}
