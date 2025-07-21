/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using Utils;
using ZWave.Devices;
using System;
using System.Linq;

namespace ZWave.BasicApplication.CommandClasses
{
    /// <summary>
    /// Time Command Class Support Task, mandatory according to DT:31.11.0004.1
    /// </summary>
    public class TimeSupport : DelayedResponseOperation
    {
        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }

        public TimeSupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_TIME_V2.ID))
        {
            TxOptions = txOptions;
            TxOptions2 = TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE;
            TxSecOptions = TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY;
        }

        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();

            var node = ReceivedAchData.SrcNode;
            byte[] command = ReceivedAchData.Command;
            var receiveStatus = ReceivedAchData.Options;
            var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;

            // SDS13782: CC:0072.01.00.41.004:
            bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);

            //Z-Wave Management Command Class Specification: CC:008A.01.01.11.002, CC:008A.01.01.11.003, CC:008A.01.03.11.002, CC:008A.01.03.11.003, CC:008A.02.06.11.002, CC:008A.02.06.11.003:
            var isMustIgnore = receiveStatus.HasFlag(ReceiveStatuses.TypeBroad) || receiveStatus.HasFlag(ReceiveStatuses.TypeMulti) || ReceivedAchData.DstNode.EndPointId > 0;

            //DateTime.Today.Day
            if (command != null && command.Length > 1 && isSuportedScheme && !isMustIgnore)
            {
                if (command[1] == COMMAND_CLASS_TIME_V2.TIME_GET.ID)
                {
                    var data = new COMMAND_CLASS_TIME_V2.TIME_REPORT();
                    data.properties1 = new COMMAND_CLASS_TIME_V2.TIME_REPORT.Tproperties1()
                    {
                        rtcFailure = 0x01, //MUST BE set to 1
                        hourLocalTime = (byte)DateTime.Now.Hour // 0..23
                    };
                    data.minuteLocalTime = (byte)DateTime.Now.Minute; //0..59
                    data.secondLocalTime = (byte)DateTime.Now.Second; //0..59

                    var sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
                else if (command[1] == COMMAND_CLASS_TIME_V2.DATE_GET.ID)
                {
                    var data = new COMMAND_CLASS_TIME_V2.DATE_REPORT();
                    data.year = new byte[2]; //For example, Year1= 0x07 and Year2 =0xD7 MUST indicate year 2007
                    data.year[0] = (byte)(DateTime.Now.Year >> 8);
                    data.year[1] = (byte)DateTime.Now.Year;
                    data.month = (byte)DateTime.Now.Month; //1..12
                    data.day = (byte)DateTime.Now.Day; //1..31

                    var sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
                else if (command[1] == COMMAND_CLASS_TIME_V2.TIME_OFFSET_GET.ID)
                {
                    //CC: 008A.02.05.13.001 ?

                    //DateTime.UtcNow;
                    var tzi = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Utc);
                    var utcTZO = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow /*tzi*/); // get UTC offset from local time zone.
                    var data = new COMMAND_CLASS_TIME_V2.TIME_OFFSET_REPORT();
                    data.properties1 = new COMMAND_CLASS_TIME_V2.TIME_OFFSET_REPORT.Tproperties1()
                    {
                        signTzo = (byte)(utcTZO.TotalHours > 0 ? 0x00 : 0x01), // 0 - is Plus, 1 - is Minus sign
                        hourTzo = (byte)(Math.Abs(utcTZO.TotalHours)) // 0..14
                    };
                    data.minuteTzo = (byte)(Math.Abs(utcTZO.TotalMinutes)); // 0..59
                    var localAdjustmentRules = TimeZoneInfo.Local.GetAdjustmentRules();
                    if (localAdjustmentRules.Length > 0)
                    {
                        var rule = localAdjustmentRules.Last();
                        data.properties2 = new COMMAND_CLASS_TIME_V2.TIME_OFFSET_REPORT.Tproperties2()
                        {
                            signOffsetDst = (byte)(DateTime.Now.IsDaylightSavingTime() ? 0x00 : 0x01), //0 MUST indicate the Plus sign, otherwise 1
                            minuteOffsetDst = (byte)rule.DaylightDelta.TotalMinutes //Daylight Saving Time starts
                        };
                        data.monthStartDst = (byte)rule.DateStart.Month; //1..12
                        data.dayStartDst = (byte)rule.DateStart.Day; //1..31
                        data.hourStartDst = (byte)rule.DateStart.Hour; //0..23
                        data.monthEndDst = (byte)rule.DateEnd.Month; //1..12
                        data.dayEndDst = (byte)rule.DateEnd.Day; //1..31
                        data.hourEndDst = (byte)rule.DateEnd.Hour; //0..23
                    }
                    else
                    {
                        data.properties2 = new COMMAND_CLASS_TIME_V2.TIME_OFFSET_REPORT.Tproperties2()
                        {
                            signOffsetDst = (byte)(DateTime.Now.IsDaylightSavingTime() ? 0x00 : 0x01), //0 MUST indicate the Plus sign, otherwise 1
                        };
                    }

                    var sendData = new SendDataExOperation(_network, node, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
            }
        }
    }
}
