using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class ResetChipOperation : ActionBase
    {
        public ResetChipOperation()
            : base(true)
        {
        }

        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, message));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.ResetChip, 0xFF, 0xFF, 0xFF);
        }
    }
}
