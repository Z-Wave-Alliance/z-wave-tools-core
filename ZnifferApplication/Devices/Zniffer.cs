/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.Xml.FrameHeader;
using ZWave.ZnifferApplication.Enums;
using ZWave.ZnifferApplication.Operations;

namespace ZWave.ZnifferApplication.Devices
{
    public class Zniffer : ApplicationClient, ISnifferDevice
    {
        private ActionToken _listenToken;

        internal Zniffer(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(ApiTypes.Zniffer, sessionId, sc, fc, tc)
        {
        }

        public FrameDefinition FrameDefinition { get; set; }
        public ZnifferApiVersion ApiVersion { get; set; }
        public int BaudRate { get; set; }
        public byte CustomFrequency { get; set; }
        public byte DefaultFrequency { get; set; }
        public int DefaultSpeed { get; set; }
        public byte[] Frequencies { get; set; }
        public byte Frequency { get; set; }
        public byte[] LRConfigs { get; set; }
        public byte[] LRRegions { get; set; }
        public byte LrConfig { get; set; }
        public byte Revision { get; set; }
        public int Speed { get; set; }
        public string Uri { get; set; }
        /// <summary>
        /// RF Region Name returned by device, must be filled on a device init stage.
        /// </summary>
        public string RfRegionName { get; set; }

        public ActionToken RestartListening(Func<ActionToken, DataItem, bool> onListenDataItemReceived)
        {
            if (_listenToken != null)
            {
                Cancel(_listenToken);
            }
            _listenToken = ExpectData(true, null, null, onListenDataItemReceived, 0, null);
            return _listenToken;
        }

        public ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            actionItem.CompletedCallback = completedCallback;
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Zniffer";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            return SessionClient.ExecuteAsync(actionItem);
        }

        public ActionResult Execute(IActionItem actionItem)
        {
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Zniffer";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            var token = SessionClient.ExecuteAsync(actionItem);
            token.WaitCompletedSignal();
            return token.Result;
        }

        public GetVersionResult GetVersion()
        {
            GetVersionResult ret = null;
            GetVersionOperation op = new GetVersionOperation(ApiVersion);
            ret = (GetVersionResult)Execute(op);
            if (ret)
            {
                AssignProperties(ret);
            }
            return ret;
        }

        public ActionToken GetVersion(Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            GetVersionOperation op = new GetVersionOperation(ApiVersion);
            ret = op.Token;
            ExecuteAsync(op, (x) =>
            {
                var action = x as ActionBase;
                if (action != null)
                {
                    if (action.IsStateCompleted)
                    {
                        GetVersionResult res = (GetVersionResult)action.Result;
                        if (res)
                        {
                            AssignProperties(res);
                        }
                    }
                    completedCallback?.Invoke(action);
                }
            });
            return ret;
        }

        private void AssignProperties(GetVersionResult ret)
        {
            ChipType = (ChipTypes)ret.ChipType;
            ChipRevision = ret.ChipVersion;
            Version = ret.SnifferVersion.ToString();
            Revision = ret.SnifferRevision;
            ApiVersion = ret.ApiVersion;
            if (ApiVersion == ZnifferApiVersion.V3)
            {
                this.Frequency = ret.CurrentFrequency;
                // operation3x.SupportedFrequencies returns incorrect count use all frequencies (except 2.4Gz) instead

                //this.Frequencies = new byte[operation3x.SupportedFrequencies];
                //for (byte i = 0; i < this.Frequencies.Length; i++)
                //{
                //    this.Frequencies[i] = i;
                //}
                //for RU frequency 
                //for IL frequency 
                this.Frequencies = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 26, 27 };
            }
        }

        public ActionToken ExpectData(bool isTop, byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectDataOperation op = new ExpectDataOperation(homeId, nodeId, dataItemPredicate, timeoutMs) { IsFirstPriority = isTop };
            return ExecuteAsync(op, completedCallback);
        }

        public ActionToken ExpectData(byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectDataOperation op = new ExpectDataOperation(homeId, nodeId, dataItemPredicate, timeoutMs);
            return ExecuteAsync(op, completedCallback);
        }

        public ActionToken CollectData(byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback)
        {
            CollectDataOperation op = new CollectDataOperation(homeId, nodeId, dataItemPredicate, timeoutMs);
            return ExecuteAsync(op, completedCallback);
        }

        public ActionToken ListenData(ListenDataOperation.DataItemCallbackDelegate dataItemCallback, Action<IActionItem> completedCallback)
        {
            ListenDataOperation op = new ListenDataOperation(dataItemCallback);
            return ExecuteAsync(op, completedCallback);
        }

        public bool Start()
        {
            bool ret = false;
            if (ApiVersion == ZnifferApiVersion.V4)
            {
                Start4xOperation operation = new Start4xOperation();
                Execute(operation);
                ret = operation.Token.State == ActionStates.Completed;
            }
            else
                ret = true;
            return ret;
        }

        public bool Stop()
        {
            bool ret = false;
            if (ApiVersion == ZnifferApiVersion.V4)
            {
                Stop4xOperation operation = new Stop4xOperation();
                Execute(operation);
                ret = operation.Token.State == ActionStates.Completed;
            }
            else
                ret = true;
            return ret;
        }

        public void SetFrequency(byte frequencyCode)
        {
            if (ApiVersion == ZnifferApiVersion.V3)
            {
                SetFrequency3xOperation operation = new SetFrequency3xOperation(frequencyCode);
                Execute(operation);
            }
            else if (ApiVersion == ZnifferApiVersion.V4)
            {
                byte code = frequencyCode;
                if (ChipType == ChipTypes.ZW030x && code == 0x1A)
                    code = 0x0A;
                SetFrequency4xOperation operation = new SetFrequency4xOperation(code);
                Execute(operation);
            }
        }

        public void SetLRChannel(byte channelCode)
        {
            if (ApiVersion == ZnifferApiVersion.V4)
            {
                byte code = channelCode;
                SetLRChConfigOperation operation = new SetLRChConfigOperation(code);
                Execute(operation);
            }
            else throw new NotSupportedException();
        }

        public Dictionary<byte, RFrequency> GetFrequencies()
        {
            Dictionary<byte, RFrequency> ret = new Dictionary<byte, RFrequency>();
            if (ApiVersion == ZnifferApiVersion.V4)
            {
                GetFrequencies4xOperation operation = new GetFrequencies4xOperation();
                Execute(operation);
                this.Frequency = operation.CurrentFrequency;
                if (ChipType == ChipTypes.ZW030x && operation.CurrentFrequency == 0x0A)
                {
                    this.Frequency = 0x1A;
                }
                if (operation.Frequencies != null)
                {
                    this.Frequencies = new byte[operation.Frequencies.Length];
                    for (byte i = 0; i < this.Frequencies.Length; i++)
                    {
                        if (ChipType == ChipTypes.ZW030x && operation.Frequencies[i] == 0x0A)
                        {
                            this.Frequencies[i] = 0x1A;
                        }
                        else
                        {
                            this.Frequencies[i] = operation.Frequencies[i];
                        }
                    }

                    for (byte i = 0; i < this.Frequencies.Length; i++)
                    {
                        GetFrequencyStr4xResult opres = (GetFrequencyStr4xResult)Execute(new GetFrequencyStr4xOperation(Frequencies[i]));
                        if (opres.IsStateCompleted)
                        {
                            ret.Add(Frequencies[i], new RFrequency(opres.Channels, opres.Name));
                        }
                        else
                            break;
                    }
                }
            }
            return ret;
        }

        public Dictionary<byte, LRConfig> GetLRConfigs()
        {
            Dictionary<byte, LRConfig> ret = new Dictionary<byte, LRConfig>();
            if (ApiVersion == ZnifferApiVersion.V4)
            {
                GetLRChConfigsOperation operation = new GetLRChConfigsOperation();
                Execute(operation);
                this.LrConfig = operation.CurrentLRConfig;

                if (operation.LRConfigs != null)
                {
                    this.LRConfigs = new byte[operation.LRConfigs.Length];
                    for (byte i = 0; i < this.LRConfigs.Length; i++)
                        this.LRConfigs[i] = operation.LRConfigs[i];

                    for (byte i = 0; i < this.LRConfigs.Length; i++)
                    {
                        GetLrChConfigStrResult opres = (GetLrChConfigStrResult)Execute(new GetLRChConfigStrOperation(LRConfigs[i]));

                        if (opres.IsStateCompleted)
                            ret.Add(LRConfigs[i], new LRConfig(opres.lrChConfig, opres.lrChConfigName));
                        else
                            break;
                    }
                }
            }
            else
            {
                throw new NotSupportedException("UZB Zniffer isn't supported");
            }

            // If the NCP Zniffer reports 0 long range channel then it means this feature not supported 
            if (ret.Count == 0)
            {
                throw new NotSupportedException("This NCP Zniffer Version isn't supported");
            }
            return ret;
        }

        public Dictionary<byte, RFrequency> GetLRFRegions()
        {
            Dictionary<byte, RFrequency> ret = new Dictionary<byte, RFrequency>();
            if (ApiVersion == ZnifferApiVersion.V4)
            {
                GetLRRegionsOperation operation = new GetLRRegionsOperation();
                Execute(operation);
                this.LRRegions = operation.LRRegions;

                if (operation.LRRegions != null)
                {
                    this.LRConfigs = new byte[operation.LRRegions.Length];
                    for (byte i = 0; i < this.LRRegions.Length; i++)
                        this.LRRegions[i] = operation.LRRegions[i];

                    for (byte i = 0; i < this.LRRegions.Length; i++)
                    {
                        GetFrequencyStr4xResult opres = (GetFrequencyStr4xResult)Execute(new GetFrequencyStr4xOperation(LRRegions[i]));
                        if (opres.IsStateCompleted)
                            ret.Add(LRRegions[i], new RFrequency(opres.Channels, opres.Name));
                        else
                            break;
                    }

                }
                else
                {
                    // If the NCP Zniffer reports 0 long range channel then it means this feature not supported 
                    throw new NotSupportedException("This NCP Zniffer Version isn't supported");
                }
            }
            else
            {
                throw new NotSupportedException("UZB Zniffer isn't supported");
            }

            return ret;
        }

        public bool SetBaudRate(BaudRates baudRate)
        {
            SetBaudRate4xOperation operation = new SetBaudRate4xOperation(baudRate);
            Execute(operation);
            return operation.Token.State == ActionStates.Completed;
        }
    }
}
