/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security.S2;
using ZWave.Xml.Application;
using ZWave.ZipApplication.Operations;

namespace ZWave.ZipApplication.CommandClasses
{
    public class NetworkManagementInclusionSupport : ResponseDataOperation
    {
        byte[] _commandsToSupport;
        private bool _isDSKNeededCallback;
        private readonly Func<byte[], byte[]> _dskNeededCallback;
        readonly Func<IEnumerable<SecuritySchemes>, bool, KEXSetConfirmResult> _kexSetConfirmCallback;
        readonly Action _csaPinCallback;
        readonly Action<AddNodeResult> _updateControllerCallback;
        private bool IsClientSideAuthRequested = false;


        public NetworkManagementInclusionSupport(byte[] commandsToSupport, Func<byte[], byte[]> dskNeededCallback, Func<IEnumerable<SecuritySchemes>, bool, KEXSetConfirmResult> kexSetConfirmCallback,
            Action csaPinCallback, Action<AddNodeResult> updateControllerCallback)
            : base(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.ID)
        {
            ReceiveCallback = OnReceiveCallback;
            _commandsToSupport = commandsToSupport;
            _dskNeededCallback = dskNeededCallback;
            _kexSetConfirmCallback = kexSetConfirmCallback;
            _csaPinCallback = csaPinCallback;
            _updateControllerCallback = updateControllerCallback;
            _isHeaderExtensionSpecified = true;
        }

        public byte[] OnReceiveCallback(byte[] headerExtension, byte[] payload)
        {
            byte[] ret = null;
            if (payload != null && payload.Length > 2)
            {
                if (payload[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_ADD_KEYS_REPORT.ID && _commandsToSupport.Contains(payload[1]))
                {
                    var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_KEYS_REPORT)payload;
                    KEXSetConfirmResult requestedKeysConfirmResult = new KEXSetConfirmResult() { IsConfirmed = false };
                    IsClientSideAuthRequested = packet.properties1.requestCsa > 0;
                    if (_kexSetConfirmCallback != null)
                    {
                        NetworkKeyS2Flags requestedKeysFlags = (NetworkKeyS2Flags)packet.requestedKeys.Value;
                        List<SecuritySchemes> requestedSecSchemes = new List<SecuritySchemes>();
                        if (requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S2Class2))
                        {
                            requestedSecSchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class2));
                        }
                        if (requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S2Class1))
                        {
                            requestedSecSchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class1));
                        }
                        if (requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S2Class0))
                        {
                            requestedSecSchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class0));
                        }
                        if (requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S0))
                        {
                            requestedSecSchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S0));
                        }

                        requestedKeysConfirmResult = _kexSetConfirmCallback(requestedSecSchemes, IsClientSideAuthRequested);

                        _isDSKNeededCallback = true;
                        if (IsClientSideAuthRequested ||
                            (!requestedSecSchemes.Contains(SecuritySchemes.S2_ACCESS) && !requestedSecSchemes.Contains(SecuritySchemes.S2_AUTHENTICATED))) // Request DSK check only for Access
                        {
                            _isDSKNeededCallback = false;
                        }
                    }
                    else
                    {
                        requestedKeysConfirmResult.IsConfirmed = true; //For using inside our apps and operations
                    }

                    byte confirmedKeys = KEXSetConfirmResult.ConvertToNetworkKeyFlags(requestedKeysConfirmResult.GrantedSchemes.ToArray());//packet.requestedKeys;
                    if (!requestedKeysConfirmResult.IsConfirmed)
                    {
                        confirmedKeys = 0;
                    }
                    else
                    {
                        //Show Client-Side authentication message with sender DSK
                        if (IsClientSideAuthRequested && _csaPinCallback != null)
                        {
                            _csaPinCallback();
                        }
                    }

                    ret = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_KEYS_SET()
                    {
                        properties1 = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_KEYS_SET.Tproperties1()
                        {
                            grantCsa = IsClientSideAuthRequested ? (byte)1 : (byte)0,
                            accept = (byte)1
                        },
                        seqNo = SequenceNumber,
                        grantedKeys = confirmedKeys
                    };
                }
                else if (payload[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_ADD_DSK_REPORT.ID && _commandsToSupport.Contains(payload[1]))
                {
                    byte[] dsk = null;
                    var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_DSK_REPORT)payload;
                    if (_isDSKNeededCallback && _dskNeededCallback != null)
                    {
                        if (packet.dsk.Length >= 16)
                        {
                            byte[] fullDSK = packet.dsk.Take(16).ToArray();
                            dsk = _dskNeededCallback(fullDSK);
                        }
                    }

                    if (dsk == null)
                    {
                        dsk = new byte[2];
                        Array.Copy(packet.dsk, dsk, dsk.Length);
                    }

                    var data = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_DSK_SET()
                    {
                        seqNo = SequenceNumber,
                        inputDsk = new List<byte>(dsk)
                    };
                    data.properties1.inputDskLength = (byte)(dsk != null ? dsk.Length : 0);
                    data.properties1.reserved = 0;
                    data.properties1.accept = 1;
                    ret = data;
                }
                else if (payload[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_ADD_STATUS.ID && _commandsToSupport.Contains(payload[1]))
                {
                    var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_ADD_STATUS)payload;
                    var result = new AddNodeResult();
                    result.Node = new NodeTag(packet.newNodeId);
                    result.NodeInfo = new NodeInfo
                    {
                        Capability = packet.properties1,
                        Security = packet.properties2,
                        Basic = packet.basicDeviceClass,
                        Generic = packet.genericDeviceClass,
                        Specific = packet.specificDeviceClass
                    };

                    if (packet.commandClass != null)
                    {
                        ZWaveDefinition.TryParseCommandClassRef(packet.commandClass, out byte[] commandClasses, out byte[] secureCommandClasses);
                        result.CommandClasses = commandClasses;
                        result.SecureCommandClasses = secureCommandClasses;
                    }
                    result.Status = packet.status;
                    _updateControllerCallback?.Invoke(result);
                }
                else if (payload[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.SMART_START_JOIN_STARTED_REPORT.ID && _commandsToSupport.Contains(payload[1]))
                {
                    // For now it's not clear if we should do something here.
                }
                else if (payload[1] == COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.EXTENDED_NODE_ADD_STATUS.ID && _commandsToSupport.Contains(payload[1]))
                {
                    var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.EXTENDED_NODE_ADD_STATUS)payload;
                    var result = new AddNodeResult();
                    result.Status = packet.status;
                    result.Node = new NodeTag((ushort)((packet.assignedNodeid[0] << 8) + packet.assignedNodeid[1]));
                    result.NodeInfo = new NodeInfo
                    {
                        Capability = packet.properties1,
                        Security = packet.properties2,
                        Basic = packet.basicDeviceClass,
                        Generic = packet.genericDeviceClass,
                        Specific = packet.specificDeviceClass
                    };
                    if (packet.commandClass != null)
                    {
                        ZWaveDefinition.TryParseCommandClassRef(packet.commandClass, out byte[] commandClasses, out byte[] secureCommandClasses);
                        result.CommandClasses = commandClasses;
                        result.SecureCommandClasses = secureCommandClasses;
                    }
                    NetworkKeyS2Flags grantedKeys = (NetworkKeyS2Flags)packet.grantedKeys.Value;
                    var securitySchemes = new List<SecuritySchemes>();
                    if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class2))
                    {
                        securitySchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class2));
                    }
                    if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class1))
                    {
                        securitySchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class1));
                    }
                    if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class0))
                    {
                        securitySchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class0));
                    }
                    if (grantedKeys.HasFlag(NetworkKeyS2Flags.S0))
                    {
                        securitySchemes.Add(ConvertToSecurityScheme(NetworkKeyS2Flags.S0));
                    }
                    result.SecuritySchemes = securitySchemes.ToArray();

                    _updateControllerCallback?.Invoke(result);
                }
            }
            return ret;
        }

        private SecuritySchemes ConvertToSecurityScheme(NetworkKeyS2Flags verifyKey)
        {
            SecuritySchemes ret = SecuritySchemes.NONE;
            switch (verifyKey)
            {
                case NetworkKeyS2Flags.S2Class0:
                    ret = SecuritySchemes.S2_UNAUTHENTICATED;
                    break;
                case NetworkKeyS2Flags.S2Class1:
                    ret = SecuritySchemes.S2_AUTHENTICATED;
                    break;
                case NetworkKeyS2Flags.S2Class2:
                    ret = SecuritySchemes.S2_ACCESS;
                    break;
                case NetworkKeyS2Flags.S0:
                    ret = SecuritySchemes.S0;
                    break;
            }
            return ret;
        }
    }
}
