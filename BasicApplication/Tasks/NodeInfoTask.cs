/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Linq;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Tasks
{
    public class NodeInfoTask : ActionSerialGroup
    {
        private readonly TransmitOptions _txOptions =
           TransmitOptions.TransmitOptionAcknowledge |
           TransmitOptions.TransmitOptionAutoRoute |
           TransmitOptions.TransmitOptionExplore;
        private NetworkViewPoint _network;

        internal NodeTag Node
        {
            get { return _nodeInfo.Node; }
            set
            {
                _nodeInfo.Node = value;
                _nodeInfoSecondAttempt.Node = value;
                _requestEndPoints.DstNode = value;
            }
        }

        private RequestNodeInfoOperation _nodeInfo;
        private RequestNodeInfoOperation _nodeInfoSecondAttempt;
        private RequestDataOperation _requestEndPoints;
        private ActionSerialGroup _requestEndPointsCapabilities;

        public NodeInfoTask(NetworkViewPoint network, NodeTag node)
        {
            _network = network;

            _nodeInfo = new RequestNodeInfoOperation(_network, node);
            _nodeInfoSecondAttempt = new RequestNodeInfoOperation(_network, node);

            _requestEndPoints = new RequestDataOperation(_network, NodeTag.Empty, node, new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_GET(), _txOptions,
                new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT(), 2, 5000);

            _requestEndPointsCapabilities = new ActionSerialGroup();

            SpecificResult.RequestNodeInfo = _nodeInfo.SpecificResult;

            Actions = new ActionBase[]
            {
                _nodeInfo,
                _nodeInfoSecondAttempt,
                _requestEndPoints,
                _requestEndPointsCapabilities,
            };
        }
        bool _isIdentical;
        int _endPoints;
        bool isFirstFailed;
        protected override void OnCompletedInternal(ActionCompletedUnit ou)
        {
            var result = ou.Action.Result;
            if (result is RequestNodeInfoResult)
            {
                RequestNodeInfoResult res = (RequestNodeInfoResult)result;
                if (res)
                {
                    if (_nodeInfo.Result)
                    {
                        _nodeInfoSecondAttempt.SetCancelled();
                    }
                    else
                    {
                        if (_nodeInfoSecondAttempt.Result)
                            isFirstFailed = true;
                        SpecificResult.RequestNodeInfo = _nodeInfoSecondAttempt.SpecificResult;
                    }

                    if ((res.CommandClasses == null || !res.CommandClasses.Contains(COMMAND_CLASS_MULTI_CHANNEL_V4.ID)) &&
                        (res.SecureCommandClasses == null || !res.SecureCommandClasses.Contains(COMMAND_CLASS_MULTI_CHANNEL_V4.ID)))
                    {
                        _requestEndPoints.SetCancelled();
                        _requestEndPointsCapabilities.SetCancelled();
                    }
                }
                else if (isFirstFailed)
                {
                    _requestEndPoints.SetCancelled();
                    _requestEndPointsCapabilities.SetCancelled();
                }
                else
                {
                    isFirstFailed = true;
                }

            }
            else if (result is RequestDataResult)
            {
                RequestDataResult res = (RequestDataResult)result;
                if (res && res.Command != null && res.Command.Length > 1 && res.Command[0] == COMMAND_CLASS_MULTI_CHANNEL_V4.ID)
                {
                    if (res.Command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT.ID)
                    {
                        COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_END_POINT_REPORT epReport = res.Command;
                        _isIdentical = epReport.properties1.identical == 0x01;
                        _endPoints = epReport.properties2.individualEndPoints;
                        var count = _isIdentical ? 1 : _endPoints;
                        if ((count > 0 && count < 0xFF))
                        {
                            var tmpList = new List<ActionBase>();
                            for (int i = 0; i < count; i++)
                            {
                                COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_GET cGet = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_GET();
                                cGet.properties1.res = 0x00;
                                cGet.properties1.endPoint = (byte)(i + 1);
                                var requestCapability = new RequestDataOperation(_network, NodeTag.Empty, res.Node,
                                    cGet, _txOptions,
                                    new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_REPORT(), 2, 5000);
                                tmpList.Add(requestCapability);

                                if (_network != null && _network.HasSecurityScheme(res.Node.Id, SecuritySchemeSet.ALL))
                                {
                                    byte[] data = null;
                                    if (_network.HasSecurityScheme(res.Node.Id, SecuritySchemeSet.ALLS2))
                                    {
                                        data = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET();
                                    }
                                    else if (_network.HasSecurityScheme(res.Node.Id, SecuritySchemes.S0))
                                    {
                                        data = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET();
                                    }

                                    COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP multiChannelCmd = new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP();
                                    multiChannelCmd.commandClass = data[0];
                                    multiChannelCmd.command = data[1];
                                    multiChannelCmd.parameter = new List<byte>();
                                    for (int j = 2; j < data.Length; j++)
                                    {
                                        multiChannelCmd.parameter.Add(data[j]);
                                    }
                                    multiChannelCmd.properties1.res = 0;
                                    multiChannelCmd.properties1.sourceEndPoint = 0;
                                    multiChannelCmd.properties2.bitAddress = 0;
                                    multiChannelCmd.properties2.destinationEndPoint = (byte)(i + 1);
                                    data = multiChannelCmd;

                                    var requestSupported = new RequestDataOperation(_network, NodeTag.Empty, res.Node,
                                        multiChannelCmd, _txOptions,
                                        new COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP(), 2, 5000);
                                    tmpList.Add(requestSupported);

                                }
                            }

                            _requestEndPointsCapabilities.AddActions(tmpList.ToArray());
                        }
                        SpecificResult.RequestEndPoints = res;
                    }
                }
            }
            else if (result.InnerResults != null && result.InnerResults.Count > 0)
            {
                SpecificResult.RequestEndPointCapabilities = new List<RequestDataResult>();
                foreach (var item in result.InnerResults)
                {
                    if (item is RequestDataResult)
                    {
                        RequestDataResult res = (RequestDataResult)item;
                        if (res && res.Command != null && res.Command.Length > 1 && res.Command[0] == COMMAND_CLASS_MULTI_CHANNEL_V4.ID)
                        {
                            if (res.Command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_REPORT.ID)
                            {
                                COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CAPABILITY_REPORT cmd = res.Command;
                                if (_isIdentical)
                                {
                                    SpecificResult.RequestEndPointCapabilities.Clear();
                                    for (int i = 0; i < _endPoints; i++)
                                    {
                                        SpecificResult.RequestEndPointCapabilities.Add(res);
                                    }
                                }
                                else
                                {
                                    SpecificResult.RequestEndPointCapabilities.Add(res);
                                }
                            }
                            else if (res.Command[1] == COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP.ID)
                            {
                                try
                                {
                                    byte[] secureCC = null;
                                    COMMAND_CLASS_MULTI_CHANNEL_V4.MULTI_CHANNEL_CMD_ENCAP cmd = res.Command;
                                    if (cmd.commandClass == COMMAND_CLASS_SECURITY.ID && cmd.command == COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT.ID)
                                    {
                                        COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT supportedS0 = new byte[] { cmd.commandClass, cmd.command }.Concat(cmd.parameter).ToArray();
                                        secureCC = supportedS0.commandClassSupport.TakeWhile(x => x != 0xEF).ToArray();
                                    }
                                    else if (cmd.commandClass == COMMAND_CLASS_SECURITY_2.ID && cmd.command == COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID)
                                    {
                                        COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT supportedS2 = new byte[] { cmd.commandClass, cmd.command }.Concat(cmd.parameter).ToArray();
                                        secureCC = supportedS2.commandClass.TakeWhile(x => x != 0xEF).ToArray();
                                    }
                                    if (_isIdentical)
                                    {
                                        for (int i = 0; i < _endPoints; i++)
                                        {
                                            _network.SetSecureCommandClasses(new NodeTag(Node.Id, (byte)(i + 1)), secureCC);
                                        }
                                    }
                                    else
                                    {
                                        _network.SetSecureCommandClasses(new NodeTag(Node.Id, cmd.properties1.sourceEndPoint), secureCC);
                                    }
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
            }
        }

        public NodeInfoResult SpecificResult
        {
            get { return (NodeInfoResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NodeInfoResult();
        }
    }

    public class NodeInfoResult : ActionResult
    {
        public RequestNodeInfoResult RequestNodeInfo { get; set; }
        public RequestDataResult RequestEndPoints { get; set; }
        public List<RequestDataResult> RequestEndPointCapabilities { get; set; }
    }
}
