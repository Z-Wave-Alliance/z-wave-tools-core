using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class EraseSectorOperation : ActionBase
    {
        internal byte SectorNumber { get; set; }
        public EraseSectorOperation(byte sectorNumber)
            :base(true)
        {
            SectorNumber = sectorNumber;
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
            message.AddData((byte)CommandTypes.EraseSector, SectorNumber, 0xFF, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.EraseSector), new ByteIndex(SectorNumber), new ByteIndex(0xFF), new ByteIndex(0xFF));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.EraseSector), new ByteIndex(0x0B), new ByteIndex(SectorNumber), new ByteIndex(0xFF));
        }
    }
}
