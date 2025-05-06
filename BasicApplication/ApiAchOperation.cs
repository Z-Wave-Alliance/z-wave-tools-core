/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.CommandClasses;
using ZWave.Enums;
using System.Collections.Generic;
using ZWave.Security;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class ApiAchOperation : ApiOperation
    {
        /// <summary>
        /// Node to send reply
        /// </summary>
        internal NodeTag SrcNode { get; set; }
        /// <summary>
        /// Target node receiving data, also may contains target endpoint
        /// </summary>
        public NodeTag DstNode { get; set; }
        internal ByteIndex[] DataToCompare { get; set; }
        internal bool IsFillReceived { get; set; }
        protected AchData ReceivedAchData { get; set; }
        internal int TimeoutMs { get; set; }
        internal ReceiveStatuses RxStatuses { get; set; }
        internal ReceiveStatuses IgnoreRxStatuses { get; set; }
        private byte[] _extensionS2Types;
        public bool ExtensionS2TypeSpecified { get; set; }
        public ApiAchOperation(NetworkViewPoint network, NodeTag dstNode, NodeTag srcNode, params ByteIndex[] compareData)
            : base(false, null, false)
        {
            _network = network;
            IsFillReceived = true;
            ReceivedAchData = new AchData();
            DstNode = dstNode;
            SrcNode = srcNode;
            SetDataToCompare(compareData);
        }

        public ApiAchOperation(NetworkViewPoint network, NodeTag dstNode, NodeTag srcNode, byte[] data, int bytesToCompare)
            : base(false, null, false)
        {
            _network = network;
            IsFillReceived = true;
            ReceivedAchData = new AchData();
            DstNode = dstNode;
            SrcNode = srcNode;

            if (data != null)
            {
                bytesToCompare = data.Length < bytesToCompare ? data.Length : bytesToCompare;
                var compareData = new ByteIndex[bytesToCompare];
                for (int i = 0; i < bytesToCompare && i < data.Length; i++)
                {
                    compareData[i] = new ByteIndex(data[i]);
                }
                SetDataToCompare(compareData);
            }
        }

        public ApiAchOperation(NetworkViewPoint network, NodeTag destNode, NodeTag srcNode, byte[] data, int bytesToCompare, ExtensionTypes[] extensionTypes)
            : base(false, null, false)
        {
            _network = network;
            _extensionS2Types = extensionTypes.Select(val => (byte)((byte)val & 0x3F)).ToArray();
            ExtensionS2TypeSpecified = true;
            IsFillReceived = true;
            ReceivedAchData = new AchData();
            DstNode = destNode;
            SrcNode = srcNode;
            var compareData = new ByteIndex[bytesToCompare];
            if (data != null)
            {
                for (int i = 0; i < bytesToCompare && i < data.Length; i++)
                {
                    compareData[i] = new ByteIndex(data[i]);
                }
                SetDataToCompare(compareData);
            }
        }

        public void SetDataToCompare(ByteIndex[] data)
        {
            if (data != null)
            {
                DataToCompare = new ByteIndex[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == 1 && data[0].Value == COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID)
                    {
                        DataToCompare[i] = new ByteIndex(data[i].Value, 0xF8);
                    }
                    else
                    {
                        DataToCompare[i] = data[i];
                    }
                }
            }
        }

        private ApiHandler _expectReceived;
        private ApiHandler _expectPModeReceived;
        private ApiHandler _expectBridgeReceived;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            ActionUnits.Add(new DataReceivedUnit(_expectReceived, OnHandled));
            ActionUnits.Add(new DataReceivedUnit(_expectPModeReceived, OnPModeHandled));
            ActionUnits.Add(new DataReceivedUnit(_expectBridgeReceived, OnBridgeHandled));
        }

        protected override void CreateInstance()
        {
            var rxMask = ~(~RxStatuses & ~IgnoreRxStatuses);
            _expectReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationCommandHandler);
            _expectReceived.AddConditions(
               new ByteIndex((byte)(RxStatuses & ~IgnoreRxStatuses), (byte)rxMask),
               SrcNode.Id > 0 && SrcNode.Id < 255 ? new ByteIndex((byte)SrcNode.Id) : ByteIndex.AnyValue,
               ByteIndex.AnyValue);
            _expectPModeReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationCommandHandlerPMode);
            _expectPModeReceived.AddConditions(
               new ByteIndex((byte)(RxStatuses & ~IgnoreRxStatuses), (byte)rxMask),
               SrcNode.Id > 0 && SrcNode.Id != 0xFF ? new ByteIndex((byte)SrcNode.Id) : ByteIndex.AnyValue,
               ByteIndex.AnyValue);
            _expectBridgeReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationCommandHandler_Bridge);
            _expectBridgeReceived.AddConditions(
                new ByteIndex((byte)(RxStatuses & ~IgnoreRxStatuses), (byte)rxMask),
                DstNode.Id > 0 && DstNode.Id != 0xFF ? new ByteIndex((byte)DstNode.Id) : ByteIndex.AnyValue,
                SrcNode.Id > 0 && SrcNode.Id != 0xFF ? new ByteIndex((byte)SrcNode.Id) : ByteIndex.AnyValue,
                ByteIndex.AnyValue);

            if (_network.IsNodeIdBaseTypeLR)
            {
                _expectReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationCommandHandler);
                _expectReceived.AddConditions(
                   new ByteIndex((byte)(RxStatuses & ~IgnoreRxStatuses), (byte)rxMask),
                   SrcNode.Id > 0 && SrcNode.Id != 0xFF ? new ByteIndex((byte)(SrcNode.Id >> 8)) : ByteIndex.AnyValue,
                   SrcNode.Id > 0 && SrcNode.Id != 0xFF ? new ByteIndex((byte)SrcNode.Id) : ByteIndex.AnyValue,
                   ByteIndex.AnyValue);
                _expectPModeReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationCommandHandlerPMode);
                _expectPModeReceived.AddConditions(
                   new ByteIndex((byte)(RxStatuses & ~IgnoreRxStatuses), (byte)rxMask),
                   SrcNode.Id > 0 && SrcNode.Id != 0xFF ? new ByteIndex((byte)(SrcNode.Id >> 8)) : ByteIndex.AnyValue,
                   SrcNode.Id > 0 && SrcNode.Id != 0xFF ? new ByteIndex((byte)SrcNode.Id) : ByteIndex.AnyValue,
                   ByteIndex.AnyValue);
                _expectBridgeReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationCommandHandler_Bridge);
                _expectBridgeReceived.AddConditions(
                    new ByteIndex((byte)(RxStatuses & ~IgnoreRxStatuses), (byte)rxMask),
                    DstNode.Id > 0 && DstNode.Id != 0xFF ? new ByteIndex((byte)(DstNode.Id >> 8)) : ByteIndex.AnyValue,
                    DstNode.Id > 0 && DstNode.Id != 0xFF ? new ByteIndex((byte)DstNode.Id) : ByteIndex.AnyValue,
                    SrcNode.Id > 0 && DstNode.Id != 0xFF ? new ByteIndex((byte)(SrcNode.Id >> 8)) : ByteIndex.AnyValue,
                    SrcNode.Id > 0 && DstNode.Id != 0xFF ? new ByteIndex((byte)SrcNode.Id) : ByteIndex.AnyValue,
                    ByteIndex.AnyValue);
            }

            if (DataToCompare != null)
            {
                _expectReceived.AddConditions(DataToCompare);
                _expectBridgeReceived.AddConditions(DataToCompare);
            }
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            ReceivedAchData.TimeStamp = DateTime.Now;
            ReceivedAchData.CommandType = CommandTypes.CmdApplicationCommandHandler;
            ReceivedAchData.SubstituteIncomingFlags = ou.DataFrame.SubstituteIncomingFlags;
            int cmdLength = ou.DataFrame.CmdLength;
            if (IsFillReceived)
            {
                if (_network.IsNodeIdBaseTypeLR)
                {
                    FillReceived(ou.DataFrame.Payload, 0, 2, 3, cmdLength);
                }
                else
                {
                    FillReceived(ou.DataFrame.Payload, 0, 1, 2, cmdLength);
                }
                ReceivedAchData.Extensions = ou.DataFrame.Extensions;
                ReceivedAchData.SrcNode = new NodeTag(ReceivedAchData.SrcNode.Id, ou.DataFrame.SrcEndPoint);
                ReceivedAchData.DstNode = new NodeTag(ReceivedAchData.DstNode.Id, ou.DataFrame.DstEndPoint, ou.DataFrame.IsBitAdress);
            }
            OnHandledAA(ou);
        }

        private void OnHandledAA(DataReceivedUnit ou)
        {
            if (ExtensionS2TypeSpecified)
            {
                try
                {
                    if (ou.DataFrame.Extensions != null)
                    {
                        if (_extensionS2Types != null && _extensionS2Types.Length > 0)
                        {
                            if (ou.DataFrame.Extensions.ExtensionsList.Any() && ou.DataFrame.Extensions.EncryptedExtensionsList.Any())
                            {
                                var ext = ou.DataFrame.Extensions.ExtensionsList.Union(ou.DataFrame.Extensions.EncryptedExtensionsList);
                                if (ext.Where(x => _extensionS2Types.Contains(x.properties1.type)).Count() == _extensionS2Types.Length)
                                {
                                    OnHandledInternal(ou);
                                }
                            }
                            else if (ou.DataFrame.Extensions.ExtensionsList.Any())
                            {
                                var ext = ou.DataFrame.Extensions.ExtensionsList;
                                if (ext.Where(x => _extensionS2Types.Contains(x.properties1.type)).Count() == _extensionS2Types.Length)
                                {
                                    OnHandledInternal(ou);
                                }
                            }
                            else if (ou.DataFrame.Extensions.EncryptedExtensionsList.Any())
                            {
                                var ext = ou.DataFrame.Extensions.EncryptedExtensionsList;
                                if (ext.Where(x => _extensionS2Types.Contains(x.properties1.type)).Count() == _extensionS2Types.Length)
                                {
                                    OnHandledInternal(ou);
                                }
                            }
                        }
                        else
                        {
                            if (!ou.DataFrame.Extensions.ExtensionsList.Any() && !ou.DataFrame.Extensions.EncryptedExtensionsList.Any())
                            {
                                OnHandledInternal(ou);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    "{0}"._EXLOG(ex.Message);
#if DEBUG
                    throw ex;
#endif
                }
            }
            else
            {
                OnHandledInternal(ou);
            }
        }

        private void OnPModeHandled(DataReceivedUnit ou)
        {
            ReceivedAchData.TimeStamp = DateTime.Now;
            ReceivedAchData.CommandType = CommandTypes.CmdApplicationCommandHandlerPMode;
            ReceivedAchData.SubstituteIncomingFlags = ou.DataFrame.SubstituteIncomingFlags;
            int cmdLength = ou.DataFrame.CmdLength;
            if (IsFillReceived)
            {
                if (_network.IsNodeIdBaseTypeLR)
                {
                    FillReceived(ou.DataFrame.Payload, 0, 2, 3, cmdLength);
                }
                else
                {
                    FillReceived(ou.DataFrame.Payload, 0, 1, 2, cmdLength);
                }
                ReceivedAchData.Extensions = ou.DataFrame.Extensions;
                ReceivedAchData.SrcNode = new NodeTag(ReceivedAchData.SrcNode.Id, ou.DataFrame.SrcEndPoint);
                ReceivedAchData.DstNode = new NodeTag(ReceivedAchData.DstNode.Id, ou.DataFrame.DstEndPoint, ou.DataFrame.IsBitAdress);
            }
            if (DstNode.Id == 0 || DstNode == ReceivedAchData.DstNode)
            {
                OnHandledAA(ou);
            }
        }


        private void OnBridgeHandled(DataReceivedUnit ou)
        {
            ReceivedAchData.TimeStamp = DateTime.Now;
            ReceivedAchData.CommandType = CommandTypes.CmdApplicationCommandHandler_Bridge;
            ReceivedAchData.SubstituteIncomingFlags = ou.DataFrame.SubstituteIncomingFlags;
            int cmdLength = ou.DataFrame.CmdLength;
            if (IsFillReceived)
            {
                if (_network.IsNodeIdBaseTypeLR)
                {
                    FillReceived(ou.DataFrame.Payload, 2, 4, 5, cmdLength);
                }
                else
                {
                    FillReceived(ou.DataFrame.Payload, 1, 2, 3, cmdLength);
                }
                ReceivedAchData.Extensions = ou.DataFrame.Extensions;
                ReceivedAchData.SrcNode = new NodeTag(ReceivedAchData.SrcNode.Id, ou.DataFrame.SrcEndPoint);
                ReceivedAchData.DstNode = new NodeTag(ReceivedAchData.DstNode.Id, ou.DataFrame.DstEndPoint, ou.DataFrame.IsBitAdress);
            }
            OnHandledAA(ou);
        }

        private void FillReceived(byte[] payload, int dstIndex, int srcIndex, int lenIndex, int cmdLen)
        {
            ReceivedAchData.DstNode = NodeTag.Empty;
            ReceivedAchData.SrcNode = NodeTag.Empty;
            ReceivedAchData.Command = null;
            ReceivedAchData.Rssi = 0;
            ReceivedAchData.SecurityScheme = 0;
            if (payload.Length > 0)
            {
                ReceivedAchData.Options = (ReceiveStatuses)payload[0];
            }
            if (dstIndex > 0 && payload.Length > dstIndex)
            {
                ReceivedAchData.DstNode = new NodeTag(payload[dstIndex]);
                if (_network.IsNodeIdBaseTypeLR)
                {
                    ReceivedAchData.DstNode = new NodeTag((ushort)((payload[dstIndex - 1] << 8) + payload[dstIndex]));
                }
            }
            if (srcIndex > 0 && payload.Length > srcIndex)
            {
                ReceivedAchData.SrcNode = new NodeTag(payload[srcIndex]);
                if (_network.IsNodeIdBaseTypeLR)
                {
                    ReceivedAchData.SrcNode = new NodeTag((ushort)((payload[srcIndex - 1] << 8) + payload[srcIndex]));
                }
            }
            if (lenIndex + 1 > 0 && lenIndex < payload.Length)
            {
                var cmdLength = cmdLen > 0 ? cmdLen : payload[lenIndex];
                if (payload.Length > lenIndex + cmdLength)
                {
                    ReceivedAchData.Command = new byte[cmdLength];
                    Array.Copy(payload, lenIndex + 1, ReceivedAchData.Command, 0, cmdLength);
                }
                int rssiOffsetAfterCommand = 0;
                if (ReceivedAchData.CommandType == CommandTypes.CmdApplicationCommandHandler_Bridge &&
                    payload.Length > lenIndex + cmdLength + 1)
                {
                    rssiOffsetAfterCommand = 1 + payload[lenIndex + cmdLength + 1];
                }
                if (ReceivedAchData.CommandType == CommandTypes.CmdApplicationCommandHandlerPMode &&
                  payload.Length > lenIndex + cmdLength + 1)
                {
                    rssiOffsetAfterCommand = 1;
                    ReceivedAchData.DstNode = new NodeTag(payload[lenIndex + cmdLength + 1]);
                }
                if (payload.Length > lenIndex + cmdLength + 1 + rssiOffsetAfterCommand)
                {
                    ReceivedAchData.Rssi = (sbyte)payload[lenIndex + cmdLength + 1 + rssiOffsetAfterCommand];
                }
                if (payload.Length > lenIndex + cmdLength + 2 + rssiOffsetAfterCommand)
                {
                    ReceivedAchData.SecurityScheme = (SecuritySchemes)payload[lenIndex + cmdLength + 2 + rssiOffsetAfterCommand];
                    "get scheme = {0}"._DLOG(ReceivedAchData.SecurityScheme);
                }
            }
        }

        protected virtual void OnHandledInternal(DataReceivedUnit ou)
        {

        }

        protected bool IsSupportedScheme(NetworkViewPoint network, NodeTag node, byte[] command, SecuritySchemes scheme)
        {
            bool ret = false;
            if (command != null && command.Length > 0)
            {
                if (network.IsSecuritySchemesSpecified(node))
                {
                    if (scheme == SecuritySchemes.NONE)
                    {
                        ret = network.GetSecuritySchemes(node) == null
                            || network.GetSecuritySchemes(node).Intersect(SecuritySchemeSet.ALL).Any();
                    }
                    else
                    {
                        ret = true;
                    }
                }
                else
                {
                    if (scheme == SecuritySchemes.NONE && !network.HasNetworkAwareCommandClass(command[0]) && network.HasSecurityScheme(SecuritySchemeSet.ALL))
                    {
                        ret = !network.HasSecureCommandClass(command[0]);
                    }
                    else
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        protected virtual bool IsHandlingAllowed(NetworkViewPoint network, byte[] command, SecuritySchemes scheme, bool secureCommandClass)
        {
            bool ret = false;
            if (network.GetHighestSecurityScheme() == scheme)
            {
                ret = true;
            }
            else
            {
                // It is assumed that the Multi Channel CC is always a secure CC. 
                // This means if a command with MC encap is received it will not be answered on lower security classes even if
                // the Multi Channel CC is listed in the normal NIF. This is a known issue for the time being.
                if (!ReceivedAchData.SubstituteIncomingFlags.HasFlag(SubstituteIncomingFlags.MultiChannel))
                {
                    if (command != null && command.Length > 0)
                    {
                        if (!secureCommandClass)
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }
    }

    public class AchData
    {
        public CommandTypes CommandType { get; set; }
        public ReceiveStatuses Options { get; set; }
        public NodeTag SrcNode { get; set; }
        public NodeTag DstNode { get; set; }
        public byte[] Command { get; set; }
        public sbyte Rssi { get; set; }
        public SecuritySchemes SecurityScheme { get; set; }
        public SubstituteIncomingFlags SubstituteIncomingFlags { get; set; }
        public DateTime TimeStamp { get; set; }
        public Extensions Extensions { get; set; }
    }
}
