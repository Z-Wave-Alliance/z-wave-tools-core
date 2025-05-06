using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class ReadSignatureByteOperation : ActionBase
    {
        internal byte ByteNo { get; set; }
        public ReadSignatureByteOperation(int byteNo)
            : base(true)
        {
            ByteNo = (byte)byteNo;
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 200, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnHandled));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandled));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.ReadSignatureByte, ByteNo, 0xFF, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadSignatureByte), new ByteIndex(ByteNo), new ByteIndex(0xFF), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadSignatureByte), new ByteIndex(0x30), new ByteIndex(ByteNo), ByteIndex.AnyValue);
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SpecificResult.Value = ou.DataFrame.Payload[3];
            SetStateCompleted(ou);
        }

        public ReadSignatureByteResult SpecificResult
        {
            get { return (ReadSignatureByteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReadSignatureByteResult();
        }
    }

    public class ReadFirstSignatureByteOperation : ActionBase
    {
        public ReadFirstSignatureByteOperation()
            : base(true)
        {
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 200, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnHandled));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandled));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.ReadSignatureByte, 0x00, 0xFF, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadSignatureByte), new ByteIndex(0), new ByteIndex(0xFF), new ByteIndex(0x7F));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadSignatureByte), new ByteIndex(0x30), new ByteIndex(0x00), new ByteIndex(0x7F));
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SetStateCompleted(ou);
        }
    }

    public class ReadSignatureByteResult : ActionResult
    {
        public byte Value { get; set; }
    }
}
