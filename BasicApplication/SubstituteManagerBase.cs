/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Devices;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.TransportService;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Frame;

namespace ZWave.BasicApplication
{
    public abstract class SubstituteManagerBase : ISubstituteManager
    {
        protected NetworkViewPoint _network;

        #region ISubstituteManager Members

        public SubstituteIncomingFlags Id
        {
            get { return GetId(); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get
            {
                if (!_isActive)
                {
                    return false;
                }

                if (this is TransportServiceManager || this is SecurityManager)
                {
                    // Skip Transport Service Manager and Security Manager for the End Device SAPI
                    // because these encapsulations are handled in the SAPI firmware itself.
                    return !_network.Library.IsSerialApiEndDevice() && !_network.Library.IsSampleApplication();
                }
                else
                {
                    return true;
                }
            }
            protected set
            { 
                _isActive = value;
            } 
        }

        protected abstract SubstituteIncomingFlags GetId();

        public SubstituteManagerBase(NetworkViewPoint network)
        {
            _network = network;
            IsActive = true;
        }

        public bool TryParseCommand(CustomDataFrame packet, out NodeTag destNode, out NodeTag srcNode, out int lenIndex, out byte[] cmdData)
        {
            bool ret = false;
            cmdData = null;
            srcNode = NodeTag.Empty;
            destNode = NodeTag.Empty;
            lenIndex = 0;
            if (packet != null && packet.Data != null && packet.Data.Length > 1 &&
                (packet.Data[1] == (byte)CommandTypes.CmdApplicationCommandHandler || packet.Data[1] == (byte)CommandTypes.CmdApplicationCommandHandler_Bridge))
            {
                int srcIndex = 3;
                lenIndex = 4;
                int destIndex = -1;
                if (_network.IsNodeIdBaseTypeLR)
                {
                    srcIndex = 4;
                    lenIndex = 5;
                }
                byte[] frameData = packet.Data;
                if (frameData[1] == (byte)CommandTypes.CmdApplicationCommandHandler_Bridge)
                {
                    destIndex = 3;
                    srcIndex = 4;
                    lenIndex = 5;
                    if (_network.IsNodeIdBaseTypeLR)
                    {
                        destIndex = 4;
                        srcIndex = 6;
                        lenIndex = 7;
                    }
                }
                if (frameData.Length > lenIndex && frameData[lenIndex] > 0 && frameData.Length > lenIndex + frameData[lenIndex])
                {
                    srcNode = new NodeTag(frameData[srcIndex]);
                    if (destIndex > 0)
                    {
                        destNode = new NodeTag(frameData[destIndex]);
                    }
                    if (_network.IsNodeIdBaseTypeLR)
                    {
                        srcNode = new NodeTag((ushort)((frameData[srcIndex - 1] << 8) + frameData[srcIndex]));
                        if (destIndex > 0)
                        {
                            destNode = new NodeTag((ushort)((frameData[destIndex - 1] << 8) + frameData[destIndex]));
                        }
                    }
                    cmdData = new byte[frameData[lenIndex]];
                    Array.Copy(frameData, lenIndex + 1, cmdData, 0, cmdData.Length);
                    if (cmdData.Length > 2)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        public CustomDataFrame SubstituteIncoming(CustomDataFrame packet, out ActionBase additionalAction, out ActionBase completeAction)
        {
            CustomDataFrame ret = packet;
            additionalAction = null;
            completeAction = null;
            if (TryParseCommand(packet, out NodeTag destNode, out NodeTag srcNode, out int lenIndex, out byte[] cmdData))
            {
                ret = SubstituteIncomingInternal(packet, destNode, srcNode, cmdData, lenIndex, out additionalAction, out completeAction);
            }
            return ret;
        }

        protected virtual CustomDataFrame SubstituteIncomingInternal(CustomDataFrame packet, NodeTag destNode, NodeTag srcNode, byte[] cmdData, int lenIndex, out ActionBase additionalAction, out ActionBase completeAction)
        {
            additionalAction = null;
            completeAction = null;
            return null;
        }

        protected CustomDataFrame CreateNewFrame(CustomDataFrame packet, byte[] newFrameData)
        {
            var ret = new DataFrame(packet.SessionId, packet.DataFrameType, false, false, packet.SystemTimeStamp);
            ret.SrcEndPoint = packet.SrcEndPoint;
            ret.DstEndPoint = packet.DstEndPoint;
            ret.IsBitAdress = packet.IsBitAdress;
            byte[] buffer = DataFrame.CreateFrameBuffer(newFrameData);
            ret.SetBuffer(buffer, 0, buffer.Length);
            ret.SubstituteIncomingFlags = packet.SubstituteIncomingFlags | GetId();
            ret.Extensions = packet.Extensions;
            return ret;
        }

        protected CustomDataFrame CreateNewFrame(CustomDataFrame packet, byte[] newFrameData, int cmdLength)
        {
            var ret = new DataFrame(packet.SessionId, packet.DataFrameType, false, false, packet.SystemTimeStamp);
            byte[] buffer = DataFrame.CreateFrameBuffer(newFrameData);
            ret.SetBuffer(buffer, cmdLength);
            ret.SubstituteIncomingFlags = packet.SubstituteIncomingFlags | GetId();
            return ret;
        }

        public virtual bool OnIncomingSubstituted(CustomDataFrame dataFrameOri, CustomDataFrame dataFrameSub, List<ActionHandlerResult> ahResults, out ActionBase additionalAction)
        {
            additionalAction = null;
            return false;
        }

        public ActionBase SubstituteAction(ActionBase action)
        {
            ActionBase ret = null;

            ApiOperation apiAction = action as ApiOperation;
            if (apiAction != null)
            {
                Action<IActionItem> completedCallback = action.CompletedCallback;
                ActionToken token = action.Token;
                ActionResult result = action.Result;
                ActionBase parent = action.ParentAction;
                int actionId = action.Id;

                ret = SubstituteActionInternal(apiAction);

                if (ret != null)
                {
                    ret.Id = actionId;
                    ret.ParentAction = parent;
                    ret.Token = token;
                    if (ret.CompletedCallback == null)
                    {
                        ret.CompletedCallback = completedCallback;
                    }
                    else
                    {
                        var newCompletedCallback = ret.CompletedCallback;
                        ret.CompletedCallback = (x) =>
                        {
                            newCompletedCallback(x);
                            completedCallback(x);
                        };
                    }
                }
            }
            return ret;
        }

        public virtual ActionBase SubstituteActionInternal(ApiOperation action)
        {
            ActionBase ret = null;
            return ret;
        }

        public virtual List<ActionToken> GetRunningActionTokens()
        {
            return null;
        }

        public virtual void AddRunningActionToken(ActionToken token)
        {
        }

        public virtual void RemoveRunningActionToken(ActionToken token)
        {
        }

        public virtual void SetDefault()
        {
        }

        public virtual void Suspend()
        {
            IsActive = false;
        }

        public virtual void Resume()
        {
            IsActive = true;
        }

        #endregion
    }
}
