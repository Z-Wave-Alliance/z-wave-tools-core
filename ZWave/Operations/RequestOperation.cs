/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Linq;
using Utils;

namespace ZWave.Operations
{
    public class RequestOperation : ActionBase
    {
        public byte[] Data { get; set; }
        private ByteIndex[] _mask;
        private readonly int _timeoutMs;
        public RequestOperation(byte[] data, ByteIndex[] mask, int timeoutMs)
            : base(false)
        {
            _timeoutMs = timeoutMs;
            Data = data;
            _mask = mask;
            IsSequenceNumberRequired = true;
        }

        private CommandMessage _sendData;
        private CommandHandler _dataReceived;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeoutMs, _sendData));
            ActionUnits.Add(new DataReceivedUnit(_dataReceived, OnHandled));
        }

        protected override void CreateInstance()
        {
            _sendData = new CommandMessage();
            _sendData.Data = Data;
            _sendData.IsNoAck = true;
            _sendData.SetSequenceNumber(SequenceNumber);

            _dataReceived = new CommandHandler
            {
                Mask = _mask
            };
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SpecificResult.Data = ou.DataFrame.Data;
            SpecificResult.Payload = ou.DataFrame.Payload;
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format($"Expect={(_mask ?? new ByteIndex[0]).DefaultIfEmpty().Select(x => x.ToString()).Aggregate((a, b) => a + ' ' + b)}, Data={SpecificResult.Data?.GetHex()}");
        }

        public RequestResult SpecificResult
        {
            get { return (RequestResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestResult();
        }
    }

    public class RequestResult : ActionResult
    {
        public byte[] Data { get; set; }
        public byte[] Payload { get; set; }
    }
}
