/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// ZW->HOST: REQ | 0x49 | bStatus | bNodeID | bLen | basic | generic | specific | commandclasses[ ]
    /// 
    /// ApplicationControllerUpdate via the Serial API also have the possibility for receiving 
    /// the status UPDATE_STATE_NODE_INFO_REQ_FAILED, which means that a node did not acknowledge 
    /// a ZW_RequestNodeInfo call.
    /// </summary>
    public class ApplicationControllerUpdateOperation : ApiOperation
    {
        public Action<ApplicationControllerUpdateResult> UpdateCallback { get; set; }
        public ApplicationControllerUpdateOperation(NetworkViewPoint network, Action<ApplicationControllerUpdateResult> updateCallback)
            : base(false, CommandTypes.CmdApplicationControllerUpdate, false)
        {
            UpdateCallback = updateCallback;
            _network = network;
        }

        private ApiHandler applicationControllerUpdateHandler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(applicationControllerUpdateHandler, OnReceived));
        }

        protected override void CreateInstance()
        {
            applicationControllerUpdateHandler = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            FillReceived(ou.DataFrame.Payload);
            if (UpdateCallback != null)
            {
                UpdateCallback(SpecificResult);
            }
        }

        private void FillReceived(byte[] data)
        {
            if (data.Length > 0)
            {
                SpecificResult.Status = (ControllerUpdateStatuses)data[0];
            }
            if (data.Length > 1)
            {
                SpecificResult.NodeId = data[1];
                SpecificResult.Payload = data.Skip(2).ToArray();
                if (_network.IsNodeIdBaseTypeLR)
                {
                    if (data.Length > 2)
                    {
                        SpecificResult.NodeId = (ushort)((data[1] << 8) + data[2]);
                        SpecificResult.Payload = data.Skip(3).ToArray();
                    }
                }
            }
            SpecificResult.Data = data;
        }

        public ApplicationControllerUpdateResult SpecificResult
        {
            get { return (ApplicationControllerUpdateResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ApplicationControllerUpdateResult();
        }
    }


    public class ExpectControllerUpdateOperation : ApplicationControllerUpdateOperation
    {
        private int TimeoutMs { get; set; }
        private ControllerUpdateStatuses UpdateStatus { get; set; }
        public ExpectControllerUpdateOperation(NetworkViewPoint network, ControllerUpdateStatuses updateStatus, int timeoutMs)
            : base(network, null)
        {
            UpdateStatus = updateStatus;
            TimeoutMs = timeoutMs;
            UpdateCallback = OnCallback1;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            base.CreateWorkflow();
        }

        private void OnCallback1(ApplicationControllerUpdateResult result)
        {
            if (result.Status == UpdateStatus)
                SetStateCompleted(null);
        }
    }

    public class ApplicationControllerUpdateResult : ActionResult
    {
        public ControllerUpdateStatuses Status { get; set; }
        public ushort NodeId { get; set; }
        public byte[] Data { get; set; }
        public byte[] Payload { get; set; }

    }
}
