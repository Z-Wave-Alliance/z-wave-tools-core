/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using ZWave.Layers;
using ZWave.Layers.Frame;
using ZWave.ZipApplication.Operations;
using ZWave.Enums;
using ZWave.Devices;

namespace ZWave.ZipApplication
{
    public class SubstituteManager : ISubstituteManager
    {
        public SubstituteIncomingFlags Id
        {
            get { return SubstituteIncomingFlags.None; }
        }
        public Func<NodeTag, byte[], bool> SendDataSubstitutionCallback { get; internal set; }
        public Action<NodeTag, byte[], bool> ReceiveDataSubstitutionCallback { get; internal set; }

        public SubstituteManager(Func<NodeTag, byte[], bool> sendDataSubstitutionCallback, Action<NodeTag, byte[], bool> receiveDataSubstitutionCallback)
        {
            SendDataSubstitutionCallback = sendDataSubstitutionCallback;
            ReceiveDataSubstitutionCallback = receiveDataSubstitutionCallback;
        }

        public bool OnIncomingSubstituted(CustomDataFrame dataFrameOri, CustomDataFrame dataFrameSub, List<ActionHandlerResult> ahResults, out ActionBase additionalAction)
        {
            additionalAction = null;
            return true;
        }

        public CustomDataFrame SubstituteIncoming(CustomDataFrame packet, out ActionBase additionalAction /*Optional*/, out ActionBase completeAction /*Optional*/)
        {
            additionalAction = null;
            completeAction = null;
            CustomDataFrame ret = packet;
            if (packet != null && packet.Payload != null && packet.Payload.Length > 1)
            {
                ReceiveDataSubstitutionCallback?.Invoke(NodeTag.Empty, packet.Payload, false);
            }
            return ret;
        }

        public ActionBase SubstituteAction(ActionBase action)
        {
            ActionBase ret = null;
            byte[] data = null;
            if (action is SendDataOperation)
                data = ((SendDataOperation)action).Data;
            else if (action is RequestDataOperation)
                data = ((RequestDataOperation)action).Data;
            if (data != null && SendDataSubstitutionCallback != null)
                SendDataSubstitutionCallback(NodeTag.Empty, data);
            return ret;
        }

        public void SetDefault()
        {
        }

        public void SetNodeId(byte nodeId)
        {
        }

        public void SetHomeId(byte[] homeId)
        {
        }

        public List<ActionToken> GetRunningActionTokens()
        {
            throw new NotImplementedException();
        }

        public void AddRunningActionToken(ActionToken token)
        {
            throw new NotImplementedException();
        }

        public void RemoveRunningActionToken(ActionToken token)
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            IsActive = false;
        }

        public void Resume()
        {
            IsActive = true;
        }

        public bool IsActive { get; private set; }
    }
}
