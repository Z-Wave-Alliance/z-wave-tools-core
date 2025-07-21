/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.CommandClasses;
using ZWave.Enums;

namespace ZWave.Devices
{
    public class NetworkViewPoint
    {
        private NodeViewPointCollection Nodes { get; set; }
        private readonly Action<string> _propertyChanged;
        public NetworkViewPoint(Action<string> propertyChanged, bool isZipGateway = false, bool preInitNodes = true)
            : this(preInitNodes)
        {
            _propertyChanged = propertyChanged;
            IsZipGateway = isZipGateway;
        }

        public NetworkViewPoint(bool preInitNodes = true)
        {
            Nodes = new NodeViewPointCollection(MAX_NODES, MAX_ENDPOINTS, preInitNodes);
            IsEnabledS0 = true;
            IsEnabledS2_ACCESS = true;
            IsEnabledS2_AUTHENTICATED = true;
            IsEnabledS2_UNAUTHENTICATED = true;
        }

        // Do not remove default .ctor - it's needed for integration tests!
        public NetworkViewPoint() : this(true)
        {
        }

        public const int MAX_NODES = 0x0FFF;//1024 + 256; // 12 bit LR nodeIds + 8 bit nodeIds
        public const int MAX_ENDPOINTS = 128;
        private byte[] _secureCommandClasses = new byte[]
        {
            COMMAND_CLASS_VERSION.ID,
            COMMAND_CLASS_MANUFACTURER_SPECIFIC.ID,
            COMMAND_CLASS_POWERLEVEL.ID,
            COMMAND_CLASS_ASSOCIATION.ID,
            COMMAND_CLASS_ASSOCIATION_GRP_INFO.ID,
            COMMAND_CLASS_DEVICE_RESET_LOCALLY.ID,
            COMMAND_CLASS_CONFIGURATION_V3.ID,
            COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.ID,
            COMMAND_CLASS_MULTI_CHANNEL_V4.ID,
            COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.ID,
            COMMAND_CLASS_WAKE_UP_V2.ID,
            COMMAND_CLASS_TIME_V2.ID,

            // Multi Channel Support only. Not presented in supported CCs.
            COMMAND_CLASS_SWITCH_BINARY_V2.ID,
            COMMAND_CLASS_SWITCH_ALL.ID,
            COMMAND_CLASS_NOTIFICATION_V8.ID,
        };

        public static byte[] HighSecureCommandClasses = new byte[] // According to SDS13548
        {
            COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID,
            COMMAND_CLASS_SECURITY.ID,
            COMMAND_CLASS_SECURITY_2.ID,
            COMMAND_CLASS_BASIC_V2.ID,
            COMMAND_CLASS_SUPERVISION.ID,
            COMMAND_CLASS_INCLUSION_CONTROLLER.ID,
            COMMAND_CLASS_CRC_16_ENCAP.ID,
            COMMAND_CLASS_APPLICATION_STATUS.ID
        };

        public bool CheckIfSupportSecurityCC { get; set; }
        public NodeIdBaseTypes NodeIdBaseType { get; set; }
        public bool IsNodeIdBaseTypeLR { get { return NodeIdBaseType == NodeIdBaseTypes.Base2; } }

        private NodeTag _nodeTag;
        public NodeTag NodeTag
        {
            get { return _nodeTag; }
            set
            {
                if (_nodeTag != value)
                {
                    ResetAndSelfRestore(_nodeTag, value);
                    _nodeTag = value;
                    Nodes[_nodeTag].IsZipGateway = IsZipGateway;
                    SetEnabledSecuritySchemes();
                    Notify("Id");
                }
            }
        }

        private byte[] _homeId = new byte[4];
        public byte[] HomeId
        {
            get { return _homeId; }
            set
            {
                _homeId[0] = 0;
                _homeId[1] = 0;
                _homeId[2] = 0;
                _homeId[3] = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (value != null && value.Length > i)
                    {
                        _homeId[i] = value[i];
                    }
                }
                Notify("HomeId");
            }
        }

        public void SetFailed(NodeTag node, bool value)
        {
            Nodes[node].IsFailed = value;
        }

        public virtual bool IsFailed(NodeTag node)
        {
            return Nodes[node].IsFailed;
        }

        public void SetVirtual(NodeTag node, bool value)
        {
            Nodes[node].IsVirtual = value;
        }

        public virtual bool IsVirtual(NodeTag node)
        {
            return Nodes[node].IsVirtual;
        }

        /// <summary>
        /// Is Node can support Long Range
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>is supported LR</returns>
        public virtual bool IsLongRange(NodeTag node)
        {
            return (Nodes[node].NodeInfo.IsEmpty && node.Id > 255) || Nodes[node].IsLongRange;
        }

        public virtual bool IsLongRangeEnabled(NodeTag node)
        {
            return (!Nodes[node].NodeInfo.IsEmpty && Nodes[node].NodeInfo.LongRange > 0) || node.Id > 255;
        }

        public bool IsEndDeviceApi(NodeTag node)
        {
            return Nodes[node].IsEndDeviceApi;
        }

        public bool IsDeviceListening(NodeTag node)
        {
            return Nodes[node].IsForcedListening || Nodes[node].IsListening || Nodes[node].IsFlirs250ms || Nodes[node].IsFlirs1000ms;
        }

        public virtual bool IsListening(NodeTag node)
        {
            return Nodes[node].IsListening;
        }

        public bool IsForcedListening(NodeTag node)
        {
            return Nodes[node].IsForcedListening;
        }

        public void SetForcedListening(NodeTag node, bool value)
        {
            Nodes[node].IsForcedListening = value;
        }

        public bool IsFlirs(NodeTag node)
        {
            return Nodes[node].IsFlirs;
        }

        public bool IsFlirs(byte nodeId)
        {
            return Nodes[nodeId].IsFlirs;
        }

        private ushort _sucNodeId;
        public ushort SucNodeId
        {
            get { return _sucNodeId; }
            set
            {
                _sucNodeId = value;
                Notify("SucNodeId");
            }
        }

        private byte _serialApiCapability;
        public byte SerialApiCapability
        {
            get { return _serialApiCapability; }
            set
            {
                _serialApiCapability = value;
                Notify("SerialApiCapability");
            }
        }

        public void Notify(string name)
        {
            _propertyChanged?.Invoke(name);
        }

        public void SetWakeupInterval(NodeTag node, bool value)
        {
            Nodes[node.Id].WakeupInterval = value;
        }

        public void SetWakeupIntervalValue(NodeTag node, int value)
        {
            Nodes[node.Id].WakeupIntervalValue = value;
        }

        public void SetWakeupInterval(byte nodeId, bool value)
        {
            Nodes[nodeId].WakeupInterval = value;
        }

        public bool GetWakeupInterval(NodeTag node)
        {
            return Nodes[node.Id].WakeupInterval;
        }

        public int GetWakeupIntervalValue(NodeTag node)
        {
            return GetWakeupInterval(node) ? Nodes[node.Id].WakeupIntervalValue : -1;
        }

        public bool GetWakeupInterval(byte nodeId)
        {
            return Nodes[nodeId].WakeupInterval;
        }

        public void SetRoleType(RoleTypes roleType)
        {
            SetRoleType(NodeTag, roleType);
        }
        public void SetRoleType(NodeTag node, RoleTypes roleType)
        {
            Nodes[node].RoleType = roleType;
        }

        public virtual RoleTypes GetRoleType()
        {
            return GetRoleType(NodeTag);
        }
        public virtual RoleTypes GetRoleType(NodeTag node)
        {
            return Nodes[node].RoleType;
        }

        public virtual NodeTypes GetNodeType()
        {
            return GetNodeType(NodeTag);
        }
        public virtual NodeTypes GetNodeType(NodeTag node)
        {
            return Nodes[NodeTag].NodeType;
        }

        public void SetNodeType(NodeTypes nodeType)
        {
            SetNodeType(NodeTag, nodeType);
        }

        public void SetNodeType(NodeTag node, NodeTypes nodeType)
        {
            Nodes[node].NodeType = nodeType;
        }

        private void Reset()
        {
            Nodes.Reset();
        }

        private void ResetAndSelfRestore(NodeTag fromNode, NodeTag toNode)
        {
            var nInfo = GetNodeInfo(fromNode);
            var cmdClasses = GetCommandClasses(fromNode);
            var rt = GetRoleType(fromNode);
            var nt = GetNodeType(fromNode);
            Reset();
            SetNodeInfo(toNode, nInfo);
            SetCommandClasses(toNode, cmdClasses);
            SetRoleType(toNode, rt);
            SetNodeType(toNode, nt);
        }

        public void ResetAndEnableAndSelfRestore()
        {
            ResetAndSelfRestore(NodeTag, NodeTag);
            SetEnabledSecuritySchemes();
        }

        public void SetEnabledSecuritySchemes()
        {
            List<SecuritySchemes> tmp = new List<SecuritySchemes>();
            if (IsEnabledS0)
            {
                tmp.Add(SecuritySchemes.S0);
            }
            if (IsEnabledS2_UNAUTHENTICATED)
            {
                tmp.Add(SecuritySchemes.S2_UNAUTHENTICATED);
            }
            if (IsEnabledS2_AUTHENTICATED)
            {
                tmp.Add(SecuritySchemes.S2_AUTHENTICATED);
            }
            if (IsEnabledS2_ACCESS)
            {
                tmp.Add(SecuritySchemes.S2_ACCESS);
            }
            SetSecuritySchemes(tmp.ToArray());
        }

        public void SetCurrentSecurityScheme(NodeTag node, SecuritySchemes scheme)
        {
            Nodes[node.Id].SetCurrentSecurityScheme(scheme);
        }

        public void SetCurrentSecurityScheme(byte nodeId, SecuritySchemes scheme)
        {
            Nodes[nodeId].SetCurrentSecurityScheme(scheme);
        }

        public void ResetCurrentSecurityScheme(NodeTag node)
        {
            Nodes[node.Id].ResetCurrentSecurityScheme();
        }

        public void ResetCurrentSecurityScheme(byte nodeId)
        {
            Nodes[nodeId].ResetCurrentSecurityScheme();
        }

        public void ResetCurrentSecurityScheme()
        {
            Nodes.ResetCurrentSecurityScheme();
        }

        public bool IsSecuritySchemesSpecified(NodeTag node)
        {
            return Nodes[node].SecuritySchemesSpecified;
        }

        public bool HasSecurityScheme(NodeTag node, SecuritySchemes[] schemes)
        {
            return HasSecurityScheme(node.Id, schemes);
        }

        public bool HasSecurityScheme(ushort nodeId, SecuritySchemes[] schemes)
        {
            bool ret = false;
            if (schemes != null)
            {
                foreach (var scheme in schemes)
                {
                    if (HasSecurityScheme(nodeId, scheme))
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }

        public bool HasSecurityScheme(SecuritySchemes[] schemes)
        {
            return HasSecurityScheme(NodeTag, schemes);
        }

        public bool HasSecurityScheme(SecuritySchemes scheme)
        {
            return HasSecurityScheme(NodeTag, scheme);
        }

        public bool HasSecurityScheme(NodeTag node, SecuritySchemes scheme)
        {
            return HasSecurityScheme(node.Id, scheme);
        }

        public bool HasSecurityScheme(ushort nodeId, SecuritySchemes scheme)
        {
            bool ret =
                IsSecuritySchemeEnabled(scheme) &&
                Nodes[NodeTag].HasSecurityScheme(scheme) &&
                Nodes[nodeId].HasSecurityScheme(scheme);
            return ret;
        }

        public void ResetSecuritySchemes()
        {
            Nodes.ResetSecuritySchemes();
        }

        public void ResetSecuritySchemes(NodeTag node)
        {
            Nodes[node].ResetSecuritySchemes();
        }

        public void SetCommandClasses(byte[] cmdClasses)
        {
            SetCommandClasses(NodeTag, cmdClasses);
        }

        public void SetCommandClasses(NodeTag node, byte[] cmdClasses)
        {
            if (Nodes[node] != null)
            {
                Nodes[node].SetCommandClasses(cmdClasses);
            }
        }

        public void SetSecuritySchemesSpecified(NodeTag node)
        {
            Nodes[node].SecuritySchemesSpecified = true;
        }

        public void SetNodeInfo(NodeInfo nodeInfo)
        {
            SetNodeInfo(NodeTag, nodeInfo);
        }

        public void SetNodeInfo(NodeTag node, NodeInfo nodeInfo)
        {
            if (Nodes[node] != null)
            {
                Nodes[node].NodeInfo = nodeInfo;
            }
        }

        public void SetNodeInfo(NodeTag node, byte generic, byte specific)
        {
            var parentNodeInfo = Nodes[node.Parent].NodeInfo;
            Nodes[node].NodeInfo = NodeInfo.
                UpdateTo(Nodes[node].NodeInfo, parentNodeInfo.Basic, generic, specific);
        }

        public void SetNodeInfo(NodeTag node, byte basic, byte generic, byte specific)
        {
            Nodes[node].NodeInfo = NodeInfo.UpdateTo(Nodes[node].NodeInfo, basic, generic, specific);
        }

        public void SetSecuritySchemes(SecuritySchemes[] schemes)
        {
            SetSecuritySchemes(NodeTag, schemes);
        }

        public void SetSecuritySchemes(NodeTag node, SecuritySchemes[] schemes)
        {
            SetSecuritySchemes(node.Id, schemes);
        }

        public void SetSecuritySchemes(ushort nodeId, SecuritySchemes[] schemes)
        {
            Nodes[nodeId]?.SetSecuritySchemes(schemes);
            if (!schemes.IsNullOrEmpty() && IsZipGateway)
            {
                _isEnabledS2_ACCESS = schemes.Contains(SecuritySchemes.S2_ACCESS);
                _isEnabledS2_AUTHENTICATED = schemes.Contains(SecuritySchemes.S2_AUTHENTICATED);
                _isEnabledS2_UNAUTHENTICATED = schemes.Contains(SecuritySchemes.S2_UNAUTHENTICATED);
                _isEnabledS0 = schemes.Contains(SecuritySchemes.S0);
            }
        }

        public virtual byte[] GetSecureCommandClasses()
        {
            return GetSecureCommandClasses(NodeTag);
        }

        public byte[] GetSecureFilteredCommandClasses(byte[] commandClasses, bool securePart)
        {
            if (commandClasses != null)
            {
                if (securePart)
                {
                    return _secureCommandClasses.Where(x => commandClasses.Contains(x)).ToArray();
                }
                else
                {
                    return commandClasses.Where(x => !_secureCommandClasses.Contains(x)).ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        public virtual byte[] GetSecureCommandClasses(NodeTag node)
        {
            if (!IsZipGateway && node == NodeTag)
            {
                var commandClasses = GetCommandClasses(node);
                if (commandClasses != null)
                {
                    //Shouldn't we return the Command Classes that are secured for a specific device? Not only the ones defined on _secureCommandClasses ....
                    return _secureCommandClasses.Where(x => commandClasses.Contains(x)).ToArray();
                    //return commandClasses;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return Nodes[node].GetSecureCommandClasses();
            }
        }

        public virtual NodeInfo GetNodeInfo()
        {
            return GetNodeInfo(NodeTag);
        }

        public virtual NodeInfo GetNodeInfo(NodeTag node)
        {
            return Nodes[node] != null ? Nodes[node].NodeInfo : new NodeInfo();
        }

        public SecuritySchemes[] GetSecuritySchemes()
        {
            return GetSecuritySchemes(NodeTag);
        }

        public SecuritySchemes[] GetSecuritySchemes(NodeTag node)
        {
            return GetSecuritySchemes(node.Id);
        }

        public SecuritySchemes[] GetSecuritySchemes(ushort nodeId)
        {
            if (_isEnabledS0 || _isEnabledS2_UNAUTHENTICATED || _isEnabledS2_AUTHENTICATED || _isEnabledS2_ACCESS)
            {
                var enabledSchemes = new List<SecuritySchemes>();
                var schemes = Nodes[nodeId].GetSecuritySchemes();
                if (schemes != null)
                {
                    foreach (var scheme in schemes)
                    {
                        if (IsSecuritySchemeEnabled(scheme))
                        {
                            enabledSchemes.Add(scheme);
                        }
                    }
                    return enabledSchemes.ToArray();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public SecuritySchemes GetHighestSecurityScheme()
        {
            return GetHighestSecurityScheme(NodeTag);
        }

        public SecuritySchemes GetHighestSecurityScheme(NodeTag node)
        {
            return GetHighestSecurityScheme(node.Id);
        }

        public SecuritySchemes GetHighestSecurityScheme(ushort nodeId)
        {
            SecuritySchemes[] securitySchemes = GetSecuritySchemes(nodeId);

            SecuritySchemes highScheme = SecuritySchemes.NONE;
            if (securitySchemes != null)
            {
                if (securitySchemes.Contains(SecuritySchemes.S2_ACCESS))
                {
                    highScheme = SecuritySchemes.S2_ACCESS;
                }
                else if (securitySchemes.Contains(SecuritySchemes.S2_AUTHENTICATED))
                {
                    highScheme = SecuritySchemes.S2_AUTHENTICATED;
                }
                else if (securitySchemes.Contains(SecuritySchemes.S2_UNAUTHENTICATED))
                {
                    highScheme = SecuritySchemes.S2_UNAUTHENTICATED;
                }
                else if (securitySchemes.Contains(SecuritySchemes.S0))
                {
                    highScheme = SecuritySchemes.S0;
                }
            }
            return highScheme;
        }

        public SecuritySchemes GetCurrentOrSwitchToHighestSecurityScheme(NodeTag node)
        {
            return Nodes[node.Id].GetCurrentOrSwitchToHighestSecurityScheme(GetSecuritySchemes());
        }

        public SecuritySchemes GetCurrentOrSwitchToHighestSecurityScheme(byte nodeId)
        {
            return Nodes[nodeId].GetCurrentOrSwitchToHighestSecurityScheme(GetSecuritySchemes());
        }

        public bool HasSecureCommandClass(byte cmdClass)
        {
            return HasSecureCommandClass(NodeTag, cmdClass);
        }

        public bool HasSecureCommandClass(NodeTag node, byte cmdClass)
        {
            if (Nodes[node.Id].GetSecuritySchemes() == null)
            {
                return false;
            }
            else
            {
                if (node == NodeTag)
                {
                    return _secureCommandClasses.Contains(cmdClass);
                }
                else
                {
                    return Nodes[node].HasSecureCommandClass(cmdClass);
                }
            }
        }

        public bool HasNetworkAwareCommandClass(byte cmdClass)
        {
            return GetNetworkAwareCommandClasses().Contains(cmdClass);
        }

        public virtual bool HasCommandClass(byte cmdClass)
        {
            return HasCommandClass(NodeTag, cmdClass);
        }

        public virtual bool HasCommandClass(NodeTag node, byte cmdClass)
        {
            // There is no need to check secure command classes for controller itself 
            if (node.Id == NodeTag.Id)
            {
                return Nodes[node].HasCommandClass(cmdClass);
            }
            else
            {
                return Nodes[node].HasCommandClass(cmdClass) || HasSecureCommandClass(node, cmdClass);
            }
        }

        public byte[] GetNetworkAwareCommandClasses()
        {
            var commandClasses = GetCommandClasses(NodeTag);
            if (NodeTag.Id > 1 && commandClasses != null && (commandClasses.Contains(COMMAND_CLASS_SECURITY.ID) || commandClasses.Contains(COMMAND_CLASS_SECURITY_2.ID)))
            {
                return commandClasses.Where(x => !_secureCommandClasses.Contains(x)).ToArray();
            }
            else
            {
                return commandClasses;
            }
        }

        public virtual byte[] GetCommandClasses()
        {
            return GetCommandClasses(NodeTag);
        }

        public virtual byte[] GetCommandClasses(NodeTag node)
        {
            return Nodes[node].GetCommandClasses();
        }

        private byte[] _commandClassesSecureVirtual;
        public void SetCommandClassesSecureVirtual(byte[] cmdClasses)
        {
            _commandClassesSecureVirtual = cmdClasses;
        }

        public byte[] GetVirtualSecureCommandClasses()
        {
            return _commandClassesSecureVirtual;
        }

        public bool IsSecuritySchemeEnabled(SecuritySchemes scheme)
        {
            bool ret = false;
            switch (scheme)
            {
                case SecuritySchemes.NONE:
                    ret = true;
                    break;
                case SecuritySchemes.S2_UNAUTHENTICATED:
                    ret = IsEnabledS2_UNAUTHENTICATED;
                    break;
                case SecuritySchemes.S2_AUTHENTICATED:
                    ret = IsEnabledS2_AUTHENTICATED;
                    break;
                case SecuritySchemes.S2_ACCESS:
                    ret = IsEnabledS2_ACCESS;
                    break;
                case SecuritySchemes.S0:
                    ret = IsEnabledS0;
                    break;
                case SecuritySchemes.S2_TEMP:
                    ret = true;
                    break;
                default:
                    break;
            }
            return ret;
        }

        private void AddSecurityScheme(SecuritySchemes addScheme)
        {
            var schemes = GetSecuritySchemes();
            if (schemes != null)
            {
                SetSecuritySchemes(schemes.Union(new[] { addScheme }).ToArray());
            }
            else
            {
                SetSecuritySchemes(new[] { addScheme });
            }
        }

        public bool IsCsaEnabled { get; set; }

        public byte[] HasCommandClass(object iD)
        {
            throw new NotImplementedException();
        }

        private bool _isEnabledS0;
        public bool IsEnabledS0
        {
            get { return _isEnabledS0; }
            set
            {
                if (value && !Nodes[NodeTag].HasSecurityScheme(SecuritySchemes.S0))
                {
                    AddSecurityScheme(SecuritySchemes.S0);
                }
                if (_isEnabledS0 != value && EnableSecuritySchemeSettingsChanged != null)
                {
                    EnableSecuritySchemeSettingsChanged(SecuritySchemes.S0, value);
                }
                _isEnabledS0 = value;
            }
        }

        public event Action<SecuritySchemes, bool> EnableSecuritySchemeSettingsChanged;

        private bool _isEnabledS2_UNAUTHENTICATED;
        public bool IsEnabledS2_UNAUTHENTICATED
        {
            get { return _isEnabledS2_UNAUTHENTICATED; }
            set
            {
                if (value && !Nodes[NodeTag].HasSecurityScheme(SecuritySchemes.S2_UNAUTHENTICATED))
                {
                    AddSecurityScheme(SecuritySchemes.S2_UNAUTHENTICATED);
                }
                if (_isEnabledS2_UNAUTHENTICATED != value && EnableSecuritySchemeSettingsChanged != null)
                {
                    EnableSecuritySchemeSettingsChanged(SecuritySchemes.S2_UNAUTHENTICATED, value);
                }
                _isEnabledS2_UNAUTHENTICATED = value;
            }
        }

        private bool _isEnabledS2_AUTHENTICATED;
        public bool IsEnabledS2_AUTHENTICATED
        {
            get { return _isEnabledS2_AUTHENTICATED; }
            set
            {
                if (value && !Nodes[NodeTag].HasSecurityScheme(SecuritySchemes.S2_AUTHENTICATED))
                {
                    AddSecurityScheme(SecuritySchemes.S2_AUTHENTICATED);
                }
                if (_isEnabledS2_AUTHENTICATED != value && EnableSecuritySchemeSettingsChanged != null)
                {
                    EnableSecuritySchemeSettingsChanged(SecuritySchemes.S2_AUTHENTICATED, value);
                }
                _isEnabledS2_AUTHENTICATED = value;
            }
        }

        private bool _isEnabledS2_ACCESS;
        public bool IsEnabledS2_ACCESS
        {
            get { return _isEnabledS2_ACCESS; }
            set
            {
                if (value && !Nodes[NodeTag].HasSecurityScheme(SecuritySchemes.S2_ACCESS))
                {
                    AddSecurityScheme(SecuritySchemes.S2_ACCESS);
                }
                if (_isEnabledS2_ACCESS != value && EnableSecuritySchemeSettingsChanged != null)
                {
                    EnableSecuritySchemeSettingsChanged(SecuritySchemes.S2_ACCESS, value);
                }
                _isEnabledS2_ACCESS = value;
            }
        }

        public void SetSecureCommandClasses(NodeTag node, byte[] cmdClasses)
        {
            Nodes[node].SetSecureCommandClasses(cmdClasses);
        }

        public int GetTimeoutValue(NodeTag[] nodeIds, bool IsInclusion)
        {
            //Timeout Constants ms
            const int BASIC = 65000;
            const int LISTENING = 217;
            const int FLIRS = 3517;
            const int NETWRORK = 732;

            int ret = BASIC;
            if (nodeIds != null && nodeIds.Length > 0)
            {
                int listenningNodesCount = 0;
                int flirsNodesCount = 0;
                int networkNodesCount = 0;
                foreach (var nodeId in nodeIds)
                {
                    if (!nodeId.Equals(NodeTag.Id))
                    {
                        if (Nodes[nodeId].IsListening)
                        {
                            listenningNodesCount++;
                        }
                        if (Nodes[nodeId].IsFlirs)
                        {
                            flirsNodesCount++;
                        }
                        networkNodesCount++;
                    }
                }
                ret = BASIC +
                    listenningNodesCount * LISTENING +
                    flirsNodesCount * FLIRS +
                    networkNodesCount * NETWRORK;
            }
            return ret;
        }

        public bool IsBridgeController
        {
            get { return Library == Libraries.ControllerBridgeLib; }
        }

        public Libraries Library { get; set; }
        public byte SerialApiVersion { get; set; }
        public byte SerialApplicationVersion { get; set; }
        public byte SerialApplicationRevision { get; set; }
        public byte HardwareVersion { get; set; }
        public bool IsZipGateway { get; private set; }
        public BitArray SupportedSerialApiCommands { get; set; }
        public BitArray ExtendedSetupSupportedSubCommands { get; set; }
        #region SendDataSettings

        public int DelayWakeUpNoMoreInformationMs { get; set; } = 100;
        public int S0MaxBytesPerFrameSize { get; set; } = 26;
        public int TransportServiceMaxSegmentSize { get; set; } = 46;
        public int TransportServiceMaxLRSegmentSize { get; set; } = 148;
        public int RequestTimeoutMs { get; set; } = 1150;
        public int DelayResponseMs { get; set; }
        public int? InclusionControllerInitiateRequestTimeoutMs { get; set; }
        public SupervisionReportStatuses SupervisionReportStatusResponse { get; set; } = SupervisionReportStatuses.SUCCESS;

        public int SupervisionDuration { get; set; } = 0;

        public int SupervisionMoreStatusUpdates { get; set; } = 0;

        #endregion

        public void SetApplicationNodeInformation(DeviceOptions deviceOptions, byte generic, byte specific, byte[] cmdClasses)
        {
            var nif = GetNodeInfo();
            nif = NodeInfo.UpdateTo(nif, deviceOptions, generic, specific);
            SetNodeInfo(nif);
            SetCommandClasses(cmdClasses);
        }

        public bool IsSupportedSerialApiCommand(byte cmd)
        {
            return SupportedSerialApiCommands == null || SupportedSerialApiCommands[cmd];
        }
    }

    public enum NodeIdBaseTypes
    {
        Base1,
        Base2
    }

    public static class DefaultTimeouts
    {
        public static int EXPIRED_EXTRA_TIMEOUT = 500;
        public static int REQUEST_NODE_INFO_TIMEOUT = 5000;
        public static int TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT = 1000;
        public static int SECURITY_S2_KEX_GET_TIMEOUT = 30000;
        public static int SECURITY_S2_KEX_SET_TIMEOUT = 240000;
        public static int SECURITY_S2_NONCE_REQUEST_INCLUSION_TIMEOUT = 10000;
        public static int SECURITY_S2_NONCE_REQUEST_TIMEOUT = 20000;
        public static int SECURITY_S0_NONCE_REQUEST_INCLUSION_TIMEOUT = 10000;
        public static int SECURITY_S0_NONCE_REQUEST_TIMEOUT = 20000;
    }
}
