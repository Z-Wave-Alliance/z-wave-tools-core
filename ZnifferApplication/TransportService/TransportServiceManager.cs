/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers;
using ZWave.Layers.Frame;
using ZWave.Enums;
using ZWave.CommandClasses;

namespace ZWave.ZnifferApplication
{
    public class TransportServiceManager : ISubstituteManager
    {
        #region ISubstituteManager Members

        public SubstituteIncomingFlags Id
        {
            get { return SubstituteIncomingFlags.TransportService; }
        }

        public CustomDataFrame SubstituteIncoming(CustomDataFrame packet, out ActionBase additionalAction, out ActionBase completeAction)
        {
            additionalAction = null;
            completeAction = null;
            CustomDataFrame ret = packet;
            if (packet is DataFrame && packet.DataFrameType == DataFrameTypes.Data)
            {
                DataFrame df = (DataFrame)packet;
                if (df.Payload != null && df.Payload.Length > 2)
                {
                    if (df.Payload[0] == COMMAND_CLASS_TRANSPORT_SERVICE.ID)
                    {
                        //df.DataItem.SetStore()
                    }
                }
            }
            return ret;
        }

        public bool OnIncomingSubstituted(CustomDataFrame dataFrameOri, CustomDataFrame dataFrameSub, List<ActionHandlerResult> ahResults, out ActionBase additionalAction)
        {
            additionalAction = null;
            return true;
        }

        public ActionBase SubstituteAction(ActionBase runningOperation)
        {
            return null;
        }

        public List<ActionToken> GetRunningActionTokens()
        {
            return null;
        }

        public void AddRunningActionToken(ActionToken token)
        {
            
        }

        public void RemoveRunningActionToken(ActionToken token)
        {
            
        }

        public void SetDefault()
        {
            
        }

        public void Suspend()
        {
            
        }

        public void Resume()
        {
            
        }

        public bool IsActive
        {
            get { return true; }
        }

        #endregion
    }
}
