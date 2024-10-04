/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZWave.Layers.Frame;
using Utils;
using ZWave.CommandClasses;
using ZWave.BasicApplication.Operations;
using ZWave.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public class SettingsSubstituteManager : SubstituteManagerBase
    {
        public SettingsSubstituteManager(NetworkViewPoint network)
            : base(network)
        { }

        private SendDataDeviceSettings _sendDataDeviceSettings = new SendDataDeviceSettings();
        public SendDataDeviceSettings SendDataDeviceSettings
        {
            get { return _sendDataDeviceSettings; }
            set { _sendDataDeviceSettings = value; }
        }

        protected override SubstituteIncomingFlags GetId()
        {
            return SubstituteIncomingFlags.Settings;
        }

        protected override CustomDataFrame SubstituteIncomingInternal(CustomDataFrame packet, NodeTag destNode, NodeTag srcNode, byte[] cmdData, int lenIndex,
            out ActionBase additionalAction, out ActionBase completeAction)
        {
            additionalAction = null;
            completeAction = null;
            return packet;
        }
    }

    public class SendDataDeviceSettings
    {
        public bool IsOverride { get; set; }
        public bool IsCRC16Enabled { get; set; }
        public bool IsSupressMulticastFollowUpEnabled { get; set; }
        public bool IsForceMultiCastEnabled { get; set; }
        public bool IsSkipWaitingSendCallbackEnabled { get; set; }
        public bool IsSecureEnabledSpecified { get; set; } // if not default security
        public bool IsSecureEnabled { get; set; }
        public int MaxBytesPerFrameSize { get; set; }
        public int TransportServiceMaxSize { get; set; }
        public bool IsSupervisionGetEnabled { get; set; }
        public bool IsStatusUpdateEnabled { get; set; }
        public bool IsAutoIncrementEnabled { get; set; }
        public byte SessionID { get; set; }
        public bool IsMultiChannelEnabled { get; set; }
        public bool IsBitAdress { get; set; }
        public byte SrcEndPoint { get; set; }
        public byte DstEndPoint { get; set; }
        public int ResponseDelayMs { get; set; }
        public int CallbackWaitTimeoutMs { get; set; }

        public void Clear()
        {
            IsAutoIncrementEnabled = false;
            IsBitAdress = false;
            IsCRC16Enabled = false;
            DstEndPoint = 0x00;
            IsForceMultiCastEnabled = false;
            IsSkipWaitingSendCallbackEnabled = false;
            IsMultiChannelEnabled = false;

            SessionID = 0x00;
            SrcEndPoint = 0x00;
            IsStatusUpdateEnabled = false;
            IsSupervisionGetEnabled = false;
            IsSupressMulticastFollowUpEnabled = false;
            IsSecureEnabled = false;
            IsSecureEnabledSpecified = false;
            ResponseDelayMs = 0;
            CallbackWaitTimeoutMs = 0;
        }
    }
}

