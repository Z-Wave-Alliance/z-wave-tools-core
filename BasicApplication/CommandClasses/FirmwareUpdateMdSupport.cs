/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    public abstract class FirmwareUpdateMdSupport : DelayedResponseOperation
    {
        const ushort START_REPORT_NUMBER = 0x01;
        byte[] FIRMWARE_ID = { 0x05, 0x01 }; //Hardcoded devkit 6.80
        byte[] MANUFACTURE_ID = { 0x00, 0x00 }; //(Former Zensys) 0x0000 by default (SDS13425) or device.ManufacturerId,
        readonly int MAX_FRAGMENT_SIZE = 40;
        public const byte RESULT_FAILED_STATUS = 0x00;
        public const byte RESULT_FAILED_DATA_STATUS = 0x01; // The device was unable to receive the requested firmware data. Number of retries and request sequence of missing frames are implementation specific.The image is not stored.
        public const byte RESULT_FAILED_KEYS_STATUS = 0x04;
        public const byte RESULT_SUCCUESSFULLY_COMPLETED_STATUS = 0xFF;

        private ushort _lastUsedReportNumber;
        private byte _pStatus = 0x00;
        private NodeTag _curNodeId = NodeTag.Empty;
        private SecuritySchemes _curSecuritySchemes;

        protected byte[] _fwuMdRequestGetChecksum;
        protected byte ReportStatusResult = 0x00;
        protected ActionBase _firmwareUpdateLoaded;
        protected readonly Action<bool> _setNewImageCompletedCallback;

        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }

        /// <summary>
        /// Over The Air support task.
        /// Firmaware Update Meta Data version 5
        /// </summary>
        public FirmwareUpdateMdSupport(NetworkViewPoint network, TransmitOptions txOptions, Action<bool> SetNewImageCompletedCallback)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.ID))
        {
            TxOptions = txOptions;
            TxOptions2 = TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE;
            TxSecOptions = TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY;

            _setNewImageCompletedCallback = SetNewImageCompletedCallback;
        }

        protected override void CreateWorkflow()
        {
            base.CreateWorkflow();
            ActionUnits.Add(new ActionCompletedUnit(_firmwareUpdateLoaded, OnSetNewImageCompleted));
        }

        protected override void CreateInstance()
        {
            base.CreateInstance();
        }

        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();
            var node = ReceivedAchData.SrcNode;
            byte[] command = ReceivedAchData.Command;
            var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
            bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);
            if (command != null && command.Length > 1 && isSuportedScheme)
            {
                switch (command[1])
                {
                    case (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_MD_GET.ID):
                        {
                            ou.SetNextActionItems(SendFwuMdReport(node, scheme));
                        }
                        break;
                    case (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_REQUEST_GET.ID):
                        {
                            ou.SetNextActionItems(HandleCmdClassFirmwareUpdateMdReqGet(node, command, scheme));
                            if (_pStatus == 0xFF)
                            {
                                ou.AddNextActionItems(SendFwuMdGet(node, START_REPORT_NUMBER, scheme));
                                FirmareUpdateStart(ou);
                                _curNodeId = node;
                                _curSecuritySchemes = scheme;
                            }
                        }
                        break;
                    case (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_REPORT.ID):
                        {
                            HandleCmdClassFirmwareUpdateMdReport(ou, node, command, scheme);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>handleCmdClassFirmwareUpdateMdReport</summary>
        private void HandleCmdClassFirmwareUpdateMdReport(DataReceivedUnit ou, NodeTag node, byte[] command, SecuritySchemes scheme)
        {
            var fwuMdReport = (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_REPORT)command;
            var lastReportNumber = Tools.GetBytes(_lastUsedReportNumber);
            if (fwuMdReport.properties1.reportNumber1 == lastReportNumber[0] &&
                fwuMdReport.reportNumber2 == lastReportNumber[1] /*&& nodeId == _curNodeId*/)
            {
                var crc16Res = Tools.CalculateCrc16Array(command.Take(command.Length - 2));
                if (crc16Res.SequenceEqual(fwuMdReport.checksum))
                {
                    var data = fwuMdReport.data;
                    FirmwareUpdateAddPacket(ou, data);

                    if (fwuMdReport.properties1.last == 0x00)
                    {
                        ushort nextReportNumber = (ushort)(_lastUsedReportNumber + 1);
                        ou.AddNextActionItems(SendFwuMdGet(node, nextReportNumber, scheme));
                    }
                    else
                    {
                        FirmwareUpdateLoad(ou);
                    }
                }
                else //FW_EV_UPDATE_STATUS_UNABLE_TO_RECEIVE
                {
                    "invalid MD report"._DLOG();
                    //retransmit request on the previous frame
                    ou.SetNextActionItems(SendFwuMdGet(node, _lastUsedReportNumber, scheme));
                }
            }

        }

        /// <summary>handleCmdClassFirmwareUpdateMdReqGet</summary>
        private ActionBase HandleCmdClassFirmwareUpdateMdReqGet(NodeTag node, byte[] command, SecuritySchemes scheme)
        {
            var fwuMdRequestGet = (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_REQUEST_GET)command;
            _fwuMdRequestGetChecksum = fwuMdRequestGet.checksum;
            var fragmentSize = Tools.GetInt32(fwuMdRequestGet.fragmentSize);
            if (fwuMdRequestGet.firmwareId.SequenceEqual(FIRMWARE_ID) &&
                fwuMdRequestGet.manufacturerId.SequenceEqual(MANUFACTURE_ID) &&
                fragmentSize <= MAX_FRAGMENT_SIZE
                )
            {
                _pStatus = 0xFF;
            }
            return SendFwuMdRequestReport(node, _pStatus, scheme);
        }

        /// <summary>ZCB_CmdClassFwUpdateMdReqReport</summary>
        private ActionBase SendFwuMdRequestReport(NodeTag node, byte reportStatus, SecuritySchemes scheme)
        {
            var fwuMdRequestReport = new COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_REQUEST_REPORT() { status = reportStatus };
            var ret = new SendDataExOperation(_network, node, fwuMdRequestReport, TxOptions, scheme);
            return ret;
        }

        /// <summary>ZCB_UpdateStatusSuccess</summary>
        protected SendDataExOperation SendFwuMdStatusReport(/*byte nodeId, byte reportStatus, SecuritySchemes scheme*/)
        {
            var fwuMdRequestReport = new COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_STATUS_REPORT() { status = ReportStatusResult };
            var ret = new SendDataExOperation(_network, _curNodeId, fwuMdRequestReport, TxOptions, _curSecuritySchemes);
            return ret;
        }

        /// <summary>CmdClassFirmwareUpdateMdGet</summary>
        private ActionBase SendFwuMdGet(NodeTag node, ushort fwuReportNumber, SecuritySchemes scheme)
        {
            _lastUsedReportNumber = fwuReportNumber;
            var reportNumber = Tools.GetBytes(fwuReportNumber);
            var fwuMdGet = new COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_GET()
            {
                numberOfReports = 0x01,
                reportNumber2 = reportNumber[1],
                properties1 = new COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_GET.Tproperties1()
                {
                    reportNumber1 = reportNumber[0]
                }
            };
            var ret = new SendDataExOperation(_network, node, fwuMdGet, TxOptions, scheme);
            return ret;
        }

        /// <summary>handleCommandClassFWUpdate</summary>
        private ActionBase SendFwuMdReport(NodeTag node, SecuritySchemes scheme)
        {
            var fwuMdReport = new COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_MD_REPORT()
            {
                manufacturerId = MANUFACTURE_ID,
                firmware0Id = FIRMWARE_ID,
                firmwareUpgradable = 0xFF,
                maxFragmentSize = Tools.GetBytes(MAX_FRAGMENT_SIZE).Skip(2).ToArray(),
            };
            var ret = new SendDataExOperation(_network, node, fwuMdReport, TxOptions, scheme);
            return ret;
        }

        protected abstract void FirmareUpdateStart(DataReceivedUnit ou);
        protected abstract void FirmwareUpdateAddPacket(DataReceivedUnit ou, IList<byte> data);
        protected abstract void FirmwareUpdateLoad(DataReceivedUnit ou);
        protected abstract void OnSetNewImageCompleted(ActionCompletedUnit ou);

    }
}
