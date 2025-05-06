using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class DisableEooSModeOperation : ActionBase
    {
        public DisableEooSModeOperation()
            : base(true)
        {
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.DisableEooSMode, 0xFF, 0xFF, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.DisableEooSMode), new ByteIndex(0xFF), new ByteIndex(0xFF), new ByteIndex(0xFF));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.DisableEooSMode), new ByteIndex(0xD0), new ByteIndex(0xFF), new ByteIndex(0xFF));
        }
    }
}
