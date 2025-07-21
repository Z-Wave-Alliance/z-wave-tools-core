/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Linq;
using Utils;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;

namespace ZWave.BasicApplication.Operations
{
    public class ExpectDataOperation : ApiAchOperation
    {
        public bool IsHandler { get; set; }
        public ExpectDataOperation(NetworkViewPoint network, NodeTag destNode, NodeTag srcNode, byte[] data, int bytesTocompare, int timeoutMs)
            : base(network, destNode, srcNode, data, bytesTocompare)
        {
            TimeoutMs = timeoutMs;
        }

        public ExpectDataOperation(NetworkViewPoint network, NodeTag destNode, NodeTag srcNode, byte[] data, int bytesTocompare, ExtensionTypes[] extensionTypes, int timeoutMs)
            : base(network, destNode, srcNode, data, bytesTocompare, extensionTypes)
        {
            TimeoutMs = timeoutMs;
        }

        public ExpectDataOperation(NetworkViewPoint network, NodeTag destNode, NodeTag srcNode, ByteIndex[] data, int timeoutMs)
            : base(network, destNode, srcNode, data)
        {
            TimeoutMs = timeoutMs;
        }

        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            if (IsHandler)
            {
                ou.DataFrame.IsHandled = true;
            }
            SpecificResult.Options = ReceivedAchData.Options;
            SpecificResult.SrcNode = ReceivedAchData.SrcNode;
            SpecificResult.DestNode = ReceivedAchData.DstNode;
            SpecificResult.Command = ReceivedAchData.Command;
            SpecificResult.Rssi = ReceivedAchData.Rssi;
            SpecificResult.SecurityScheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
            SpecificResult.Extensions = ReceivedAchData.Extensions;
            SpecificResult.SubstituteStatus = (ou.DataFrame.SubstituteIncomingFlags & SubstituteIncomingFlags.Security) > 0 ?
                SubstituteStatuses.Done : SubstituteStatuses.Failed;
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Data={0}", DataToCompare != null && DataToCompare.Length > 0 ? DataToCompare.Select(x => x.ToString()).Aggregate((x, y) => x + " " + y) : "");
        }

        public ExpectDataResult SpecificResult
        {
            get { return (ExpectDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectDataResult();
        }
    }

    public class ExpectDataResult : ActionResult
    {
        public ReceiveStatuses Options { get; set; }
        public NodeTag SrcNode { get; set; }
        public NodeTag DestNode { get; set; }
        public byte[] Command { get; set; }
        public sbyte Rssi { get; set; }
        public SecuritySchemes SecurityScheme { get; set; }
        public SubstituteStatuses SubstituteStatus { get; set; }
        public Extensions Extensions { get; set; }
    }
}
