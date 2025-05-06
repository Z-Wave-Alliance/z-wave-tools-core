using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class EnableInterfaceOperation : ActionBase
    {
        public EnableInterfaceOperation()
            : base(true)
        {
        }

        CommandHandler handlerOK;
        CommandHandler handlerUart_AA;
        CommandHandler handlerUart_55;
        CommandHandler handlerSpi;
        CommandHandler handlerFail;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 200, message));
            ActionUnits.Add(new DataReceivedUnit(handlerOK, OnHandledOK));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandledOK));
            ActionUnits.Add(new DataReceivedUnit(handlerUart_AA, OnHandledUart_AA));
            ActionUnits.Add(new DataReceivedUnit(handlerUart_55, OnHandledUart_55));
            //ActionUnits.Add(new DataReceivedUnit(handlerFail, OnHandledFail));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.EnableInterface, 0x53, 0xAA, 0x55);

            handlerOK = new CommandHandler();
            handlerOK.AddConditions(new ByteIndex((byte)CommandTypes.EnableInterface), new ByteIndex(0x53), new ByteIndex(0xAA), new ByteIndex(0x55));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.EnableInterface), ByteIndex.AnyValue, new ByteIndex(0x53), new ByteIndex(0xAA));

            handlerUart_AA = new CommandHandler();
            handlerUart_AA.AddConditions(new ByteIndex(0xAA));

            handlerUart_55 = new CommandHandler();
            handlerUart_55.AddConditions(new ByteIndex(0x55));

            handlerFail = new CommandHandler();
            handlerFail.AddConditions(new ByteIndex(0xAE, 0xFF, Presence.ExceptValue));
        }

        private void OnHandledOK(DataReceivedUnit ou)
        {
            SetStateCompleted(ou);
        }

        bool isAAReceived = false;
        private void OnHandledUart_AA(DataReceivedUnit ou)
        {
            isAAReceived = true;
        }

        private void OnHandledUart_55(DataReceivedUnit ou)
        {
            if (isAAReceived)
                SetStateCompleted(ou);
        }

        private void OnHandledFail(DataReceivedUnit ou)
        {
            SetStateFailed(ou);
        }
    }
}
