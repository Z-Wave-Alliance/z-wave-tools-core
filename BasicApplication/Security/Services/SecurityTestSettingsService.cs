/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Threading;
using ZWave.BasicApplication.Operations;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;

namespace ZWave.BasicApplication.Security
{
    public class SecurityTestSettingsService : ISecurityTestSettingsService
    {
        private readonly SecurityManagerInfo _securityManagerInfo;
        private readonly bool _isInclusionProcesses;

        public SecurityTestSettingsService(SecurityManagerInfo securityManagerInfo, bool isInclusionProcesses)
        {
            _securityManagerInfo = securityManagerInfo;
            _isInclusionProcesses = isInclusionProcesses;
        }

        public bool ActivateTestPropertiesForFrame(SecurityS2TestFrames testFrameType, ISendDataAction apiOperation)
        {
            bool ret = false;
            if (_securityManagerInfo.TestFramesS2.ContainsKey(testFrameType))
            {
                ret = true;
                var testFrame = _securityManagerInfo.TestFramesS2[testFrameType];
                apiOperation.Data = testFrame.Command ?? apiOperation.Data;
                var peerNodeId = new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, apiOperation.DstNode);

                if (testFrame.IsEncryptedSpecified)
                {
                    if (testFrame.IsEncrypted)
                    {
                        apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                        if (testFrame.IsTemp)
                        {
                            if (testFrame.NetworkKey != null)
                            {
                                _securityManagerInfo.ActivateNetworkKeyS2CustomForNode(peerNodeId, testFrame.IsTemp, testFrame.NetworkKey);
                            }
                            else
                            {
                                _securityManagerInfo.ActivateNetworkKeyS2CustomForNode(peerNodeId, testFrame.IsTemp, _securityManagerInfo.GetActualNetworkKeyS2Temp());
                            }
                        }
                        else
                        {
                            if (testFrame.NetworkKey != null)
                            {
                                _securityManagerInfo.ActivateNetworkKeyS2CustomForNode(peerNodeId, testFrame.IsTemp, testFrame.NetworkKey);
                            }
                        }
                    }
                    else
                    {
                        apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
                    }
                }
                if (testFrame.IsMulticastSpecified)
                {
                    if (testFrame.IsMulticast)
                    {
                        apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.DenyMulticast);
                        apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.UseMulticast);
                        if (_isInclusionProcesses)
                        {
                            apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.UseFollowup);
                            apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.DenyFollowup);
                        }
                    }
                    else
                    {
                        apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.UseMulticast);
                        apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.DenyMulticast);
                    }
                }
                if (testFrame.IsBroadcastSpecified)
                {
                    if (testFrame.IsBroadcast)
                    {
                        apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.DenyBroadcast);
                        apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.UseBroadcast);
                        if (_isInclusionProcesses)
                        {
                            apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.UseFollowup);
                            apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.DenyFollowup);
                        }
                    }
                    else
                    {
                        apiOperation.SubstituteSettings.ClearFlag(SubstituteFlags.UseBroadcast);
                        apiOperation.SubstituteSettings.SetFlag(SubstituteFlags.DenyBroadcast);
                    }
                }
                if (testFrame.DelaySpecified)
                {
                    apiOperation.DataDelay = testFrame.Delay;
                };
            }
            return ret;
        }
    }
}
