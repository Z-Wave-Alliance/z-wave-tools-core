/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Utils;

namespace ZWave.Xml.Application
{
    public partial class ZWaveDefinition
    {
        public static ZWaveDefinition Load(string path)
        {
            DefinitionConverter conv = new DefinitionConverter(path, null);
            conv.UpgradeConvert(false);
            var ret = conv.ZWaveDefinition;
            return ret;
        }

        public static void Save(ZWaveDefinition zWaveDefinition, string path)
        {
            DefinitionConverter conv = new DefinitionConverter(null, null) { ZWaveDefinition = zWaveDefinition };
            conv.DowngradeConvert();
            conv.SaveZXmlDefinition(path);
        }

        public static cmd_class DowngradeCommandClass(CommandClass cmdClass)
        {
            DefinitionConverter conv = new DefinitionConverter(null, null);
            return (cmd_class)conv.DowngradeCommandClass(cmdClass, null);
        }

        public string ParseApplicationStringName(byte[] payload)
        {
            string ret = string.Empty;
            if (payload != null && payload.Length > 0)
            {
                ParseApplicationObject(payload, out CommandClassValue[] ccv);
                if (ccv.Length > 0)
                {
                    var comDef = ccv[0].CommandValue.CommandDefinition;
                    ret = comDef.Name;
                }
            }
            return ret;
        }

        public void ParseApplicationObject(byte[] data, out CommandClassValue[] commandClasses)
        {
            SortedList<byte, CommandClassValue> ret = new SortedList<byte, CommandClassValue>();
            if (data == null || data.Length == 0)
            {
                CommandClassValue ccv =
                    new CommandClassValue { CommandClassDefinition = new CommandClass { Text = "No payload" } };
                ret.Add(0, ccv);
            }
            else
            {
                IEnumerable<CommandClass> iter = CommandClasses.Where(x => Tools.GetByte(x.Key) == data[0]);
                foreach (var itemCommandClass in iter)
                {
                    if (itemCommandClass.Command != null && data.Length > 1)
                    {
                        Command itemCommand = itemCommandClass.Command.FirstOrDefault(
                            x => x.Bits > 0 && x.Bits < 8
                                ? Tools.GetByte(x.Key) ==
                                  (data[1] & Tools.GetMaskFromBits(x.Bits, (byte)(8 - x.Bits)))
                                : Tools.GetByte(x.Key) == data[1]);
                        if (itemCommand == null)
                        {
                            CommandClassValue ccv =
                                new CommandClassValue
                                {
                                    CommandClassDefinition = itemCommandClass,
                                    CommandValue = new CommandValue(this)
                                    {
                                        CommandDefinition = new Command
                                        {
                                            Text = Tools.FormatStr("Command {0} not found",
                                                Tools.GetHex(data[1], true)),
                                            Param = new Collection<Param>()
                                        }
                                    }
                                };
                            Param p = new Param
                            {
                                Text = "Hex Data",
                                Bits = 8,
                                SizeReference = "MSG_LENGTH",
                                Type = zwParamType.HEX
                            };
                            ccv.CommandValue.CommandDefinition.Param.Add(p);
                            ccv.CommandValue.ProcessParameters(data, 0);
                            ProcessParametersTextValue(ccv.CommandValue.ParamValues, ccv.CommandClassDefinition);
                            ret.Add((byte)(0 - itemCommandClass.Version), ccv);
                        }
                        else
                        {
                            CommandValue cv = new CommandValue(this) { CommandDefinition = itemCommand };
                            byte[] payload = null;
                            if (itemCommand.Bits == 8 || itemCommand.Bits == 0)
                            {
                                if (data.Length > 2)
                                {
                                    payload = new byte[data.Length - 2];
                                    Array.Copy(data, 2, payload, 0, payload.Length);
                                    cv.ProcessParameters(payload, 0);
                                }
                            }
                            else
                            {
                                // transform |C|C|C|C|C|P|P|P| to |P|P|P|0|0|0|0|0|
                                // where c - command bit, p - parameter bit
                                byte offset = itemCommand.Bits;
                                payload = new byte[data.Length - 1];
                                payload[0] = (byte)(data[1] << offset);
                                if (data.Length > 2)
                                {
                                    Array.Copy(data, 2, payload, 1, payload.Length - 1);
                                }
                                cv.ProcessParameters(payload, offset);
                            }
                            ProcessParametersTextValue(cv.ParamValues, itemCommandClass);

                            CommandClassValue ccv =
                                new CommandClassValue
                                {
                                    CommandClassDefinition = itemCommandClass,
                                    CommandValue = cv,
                                    Payload = payload,
                                    Data = data
                                };
                            ret.Add((byte)(0 - itemCommandClass.Version), ccv);
                        }
                    }
                }

                if (ret.Count == 0)
                {
                    CommandClassValue ccv = new CommandClassValue
                    {
                        CommandClassDefinition = new CommandClass
                        {
                            Text = Tools.FormatStr("Command Class {0} not found", Tools.GetHex(data[0], true))
                        },
                        CommandValue = new CommandValue(this)
                        {
                            CommandDefinition = new Command
                            {
                                Text = "N/A",
                                Param = new Collection<Param>()
                            }
                        }
                    };
                    Param p = new Param
                    {
                        Text = "Hex Data",
                        Bits = 8,
                        SizeReference = "MSG_LENGTH",
                        Type = zwParamType.HEX
                    };
                    ccv.CommandValue.CommandDefinition.Param.Add(p);
                    ccv.CommandValue.ProcessParameters(data, 0);
                    ProcessParametersTextValue(ccv.CommandValue.ParamValues, ccv.CommandClassDefinition);
                    ret.Add(0, ccv);
                }
            }
            commandClasses = ret.Values.ToArray();
        }

        private byte _configPropFormat = 0xFF;
        private void ProcessParametersTextValue(IEnumerable<ParamValue> paramValues, CommandClass cmdClass)
        {
            ParamValue prevParamValue = null;
            foreach (var item in paramValues)
            {
                if (item.HasTextValue)
                    break;
                if (item.InnerValues != null)
                {
                    ProcessParametersTextValue(item.InnerValues, cmdClass);
                }
                if (item.ParamDefinition != null)
                {
                    if (item.ParamDefinition.Defines != null && cmdClass.DefineSet != null)
                    {
                        Collection<Define> defines = null;
                        DefineSet dSet = cmdClass.DefineSet.FirstOrDefault(x => x.Name == item.ParamDefinition.Defines);
                        if (dSet != null)
                        {
                            defines = dSet.Define;
                            bool isMultiarray =
                                dSet.Define.Aggregate(true, (current, d) => current & d.Define1 != null);
                            if (isMultiarray && prevParamValue != null)
                            {
                                if (prevParamValue.ByteValueList != null && prevParamValue.ByteValueList.Count > 0)
                                {
                                    var d = dSet.Define.FirstOrDefault(x => x.KeyId == prevParamValue.ByteValueList[0]);
                                    if (d != null)
                                    {
                                        defines = d.Define1;
                                    }
                                }
                            }
                            if (item.ParamDefinition.Name.Contains("format") &&
                                item.ParamDefinition.ParentCmd != null &&
                                item.ParamDefinition.ParentCmd.Parent.KeyId == 0x70 &&      //COMMAND_CLASS_CONFIGURATION
                                item.ParamDefinition.ParentCmd.KeyId == 0x0f)               //CONFIGURATION_PROPERTIES_REPORT
                            {
                                _configPropFormat = item.ByteValueList[0];
                            }
                        }
                        IEnumerable<string> range =
                            ParameterToString(item.ByteValueList, item.ParamDefinition.Type, defines);
                        foreach (var str in range)
                        {
                            item.TextValueList.Add(str);
                            item.HasTextValue = true;
                        }
                    }
                    else
                    {
                        var paramType = item.ParamDefinition != null ? item.ParamDefinition.Type : zwParamType.HEX;
                        IEnumerable<string> range = ParameterToString(item.ByteValueList, paramType, null);
                        foreach (var str in range)
                        {
                            item.TextValueList.Add(str);
                            item.HasTextValue = true;
                        }
                        if (item.ParamDefinition.ParentCmd != null &&
                            item.ParamDefinition.ParentCmd.Parent.KeyId == 0x70 &&      //COMMAND_CLASS_CONFIGURATION
                            item.ParamDefinition.ParentCmd.KeyId == 0x0f &&             //CONFIGURATION_PROPERTIES_REPORT
                            (item.ParamDefinition.Name.Contains("minValue") || item.ParamDefinition.Name.Contains("maxValue")))
                        {
                            string ret = string.Empty;
                            switch (_configPropFormat)
                            {
                                case 0:
                                    ret = Tools.GetSignedInt32(item.ByteValueList.ToArray()).ToString();
                                    break;
                                case 1:
                                    ret = Tools.ByteArrayToUInt32(item.ByteValueList.ToArray()).ToString();
                                    break;
                                case 2:
                                case 3:
                                default:
                                    break;
                            }
                            if (!ret.IsNullOrEmpty())
                            {
                                item.TextValueList.Clear();
                                item.TextValueList.Add(ret);
                                item.HasTextValue = true;
                            }
                        }
                    }
                }
                else
                {
                    var paramType = item.ParamDefinition != null ? item.ParamDefinition.Type : zwParamType.HEX;
                    IEnumerable<string> range = ParameterToString(item.ByteValueList, paramType, null);
                    foreach (var str in range)
                    {
                        item.TextValueList.Add(str);
                        item.HasTextValue = true;
                    }
                }
                prevParamValue = item;
            }
        }

        private GenericDevice _genericDevice = null;
        private CommandClass _commandClass = null;
        private Command _command = null;

        private IEnumerable<string> ParameterToString(IList<byte> list, zwParamType type, Collection<Define> defines)
        {
            IList<string> ret = new List<string>();
            bool first = true;
            StringBuilder sb;
            switch (type)
            {
                case zwParamType.CHAR:

                    try
                    {
                        ret.Add(Encoding.ASCII.GetString(list.ToArray(), 0, list.Count));
                    }
                    catch (Exception dex)
                    {
                        dex.Message._EXLOG();
#if DEBUG
                        throw dex;
#endif
                    }
                    break;
                case zwParamType.HEX:
                    if (defines == null)
                    {
                        ret.Add(Tools.GetHex(list));
                    }
                    else
                    {
                        sb = new StringBuilder();
                        bool isFirst = true;
                        foreach (var itemByte in list)
                        {
                            string str = null;
                            foreach (var dItem in defines)
                            {
                                if (dItem.KeyId == itemByte)
                                {
                                    str = dItem.Text;
                                }
                            }
                            if (!isFirst)
                            {
                                sb.Append(" ");
                            }
                            if (str != null)
                            {
                                sb.Append(str);
                                sb.Append("=");
                            }
                            sb.Append(Tools.GetHex(itemByte, true));
                            isFirst = false;
                        }
                        ret.Add(sb.ToString());
                    }
                    break;
                case zwParamType.NUMBER:
                    if (list.Count <= 4) //int
                    {
                        byte[] val = { 0, 0, 0, 0 };
                        Array.Copy(list.Reverse().ToArray(), val, list.Count);
                        int tmp = BitConverter.ToInt32(val, 0);
                        sb = new StringBuilder();
                        if (defines != null)
                        {
                            string str = null;
                            foreach (var dItem in defines)
                            {
                                if (dItem.KeyId == tmp)
                                {
                                    str = dItem.Text;
                                }
                            }
                            sb.Append(str ?? tmp.ToString());
                        }
                        else
                        {
                            if (tmp < 100)
                                sb.Append(tmp.ToString(""));
                            else
                                sb.Append(tmp.ToString());
                        }
                        ret.Add(sb.ToString());
                    }
                    else // as HEX
                    {
                        ret.Add(Tools.GetHex(list));
                    }
                    break;
                case zwParamType.NODE_NUMBER:
                    sb = new StringBuilder();
                    foreach (var item in list)
                    {
                        if (!first)
                        {
                            sb.Append(", ");
                        }
                        first = false;
                        sb.Append(item.ToString(""));
                    }
                    ret.Add(sb.ToString());
                    break;
                case zwParamType.BITMASK:
                    sb = new StringBuilder();
                    for (int i = 0; i < list.Count; i++)
                    {
                        byte maskByte = list[i];
                        if (maskByte == 0)
                        {
                            continue;
                        }
                        byte bitMask = 0x01;
                        for (int j = 0; j < 8; j++)
                        {
                            if ((bitMask & maskByte) != 0)
                            {
                                var bitIndex = i * 8 + j;
                                if (!first)
                                {
                                    sb.Append(", ");
                                }
                                first = false;

                                string str = defines?.ElementAtOrDefault(bitIndex)?.Text;
                                var keyId = defines?.ElementAtOrDefault(bitIndex)?.KeyId;
                                if (str != null)
                                    sb.Append(keyId == null ?
                                        ($"{str} (bit {bitIndex})") :
                                        ($"{str} (KeyId {keyId})"));
                                else
                                    sb.Append($"bit {bitIndex}");
                            }
                            bitMask <<= 1;
                        }
                    }
                    ret.Add(sb.ToString());
                    break;
                case zwParamType.BOOLEAN:
                    if (list.Count > 0)
                    {
                        if (defines == null)
                        {
                            ret.Add(list[0] > 0 ? "true" : "false");
                        }
                        else
                        {
                            string str = null;
                            foreach (var dItem in defines)
                            {
                                if (dItem.KeyId == list[0])
                                {
                                    str = dItem.Text;
                                }
                            }
                            if (str != null)
                            {
                                ret.Add(str);
                            }
                            else
                            {
                                ret.Add(list[0] > 0 ? "true" : "false");
                            }
                        }
                    }
                    else
                        ret.Add("false");
                    break;
                case zwParamType.MARKER:
                    ret.Add(Tools.GetHex(list));
                    break;
                case zwParamType.BAS_DEV_REF:
                    foreach (var item in list)
                    {
                        BasicDevice bd = BasicDevices.FirstOrDefault(x => x.Key == Tools.GetHex(item, true));
                        if (bd != null)
                        {
                            ret.Add(string.Format("{0} - {1}", Tools.GetHex(item, true), bd.Name));
                        }
                        else
                        {
                            ret.Add(Tools.GetHex(item, true));
                        }
                    }
                    break;
                case zwParamType.GEN_DEV_REF:
                    foreach (var item in list)
                    {
                        _genericDevice = GenericDevices.FirstOrDefault(x => x.Key == Tools.GetHex(item, true));
                        if (_genericDevice != null)
                        {
                            ret.Add(string.Format("{0} - {1}", Tools.GetHex(item, true), _genericDevice.Name));
                        }
                        else
                        {
                            ret.Add(Tools.GetHex(item, true));
                        }
                    }
                    break;
                case zwParamType.SPEC_DEV_REF:
                    if (_genericDevice != null)
                    {
                        foreach (var item in list)
                        {
                            SpecificDevice bd =
                                _genericDevice.SpecificDevice.FirstOrDefault(x => x.Key == Tools.GetHex(item, true));
                            if (bd != null)
                            {
                                ret.Add(string.Format("{0} - {1}", Tools.GetHex(item, true), bd.Name));
                            }
                            else
                            {
                                ret.Add(Tools.GetHex(item, true));
                            }
                        }
                    }
                    else
                    {
                        ret.Add(Tools.GetHex(list));
                    }
                    break;
                case zwParamType.CMD_CLASS_REF:
                    foreach (var item in list)
                    {
                        _commandClass = CommandClasses.OrderByDescending(x => x.Version).FirstOrDefault(x => x.Key == Tools.GetHex(item, true));
                        if (_commandClass != null)
                        {
                            ret.Add(string.Format("{0} - {1}", Tools.GetHex(item, true), _commandClass.Name));
                        }
                        else
                        {
                            ret.Add(Tools.GetHex(item, true));
                        }
                    }
                    break;
                case zwParamType.CMD_REF:
                    if (_commandClass != null)
                    {
                        foreach (var item in list)
                        {
                            if (_commandClass.Command != null)
                            {
                                _command = _commandClass.Command.FirstOrDefault(x => x.Key == Tools.GetHex(item, true));
                                if (_command != null)
                                {
                                    ret.Add(string.Format("{0} - {1}", Tools.GetHex(item, true), _command.Name));
                                }
                                else
                                {
                                    ret.Add(Tools.GetHex(item, true));
                                }
                            }
                            else
                            {
                                ret.Add(Tools.GetHex(item, true));
                            }
                        }
                    }
                    else
                    {
                        ret.Add(Tools.GetHex(list));
                    }
                    break;
                case zwParamType.CMD_DATA:
                    ret.Add(Tools.GetHex(list));
                    break;
                case zwParamType.CMD_ENCAP:
                    ret.Add(Tools.GetHex(list));
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Returns ParamValues as dictionary
        /// </summary>
        /// <param name="paramValues">paramValues list</param>
        /// <param name="isIncludeParentKey">dictionary key contains parent param key if 'true'. Example: "properties1.level"</param>
        /// <param name="useQualifiedParamNameAsKey">dictionary key contains parameter's 'Name' instead of parameter's 'Text' attribute if 'true'</param>
        /// <returns>dictionary</returns>
        public static Dictionary<string, List<ParamValue>> ToParamValuesDictionary(IList<ParamValue> paramValues,
            bool isIncludeParentKey, bool useQualifiedParamNameAsKey)
        {
            Dictionary<string, List<ParamValue>> ret = null;
            if (paramValues != null)
            {
                ret = new Dictionary<string, List<ParamValue>>();
                FillParamValuesDictionary(paramValues, null, ref ret, isIncludeParentKey, useQualifiedParamNameAsKey);
            }
            return ret;
        }

        private static void FillParamValuesDictionary(ICollection<ParamValue> paramValues, string parentName,
            ref Dictionary<string, List<ParamValue>> dictionary, bool isIncludeParentKey,
            bool useQualifiedParamNameAsKey)
        {
            if (paramValues != null && paramValues.Count > 0)
            {
                foreach (var paramValue in paramValues)
                {
                    List<ParamValue> list;
                    string key;
                    if (useQualifiedParamNameAsKey)
                    {
                        key = paramValue.ParamDefinition.Name;
                    }
                    else
                    {
                        key = paramValue.ParamDefinition.Text;
                    }

                    if (isIncludeParentKey && parentName != null)
                    {
                        key = parentName + "." + key;
                    }

                    if (dictionary.ContainsKey(key))
                    {
                        list = dictionary[key];
                    }
                    else
                    {
                        list = new List<ParamValue>();
                        dictionary.Add(key, list);
                    }
                    list.Add(paramValue);
                    FillParamValuesDictionary(paramValue.InnerValues, key, ref dictionary, isIncludeParentKey,
                        useQualifiedParamNameAsKey);
                }
            }
        }

        private enum CmdClassParseStates
        {
            CmdClass,
            SecureCmdClass,
            ControlMark,
            SecureControlMark,
            SecureMark0
        }

        public static void TryParseCommandClassRef(IList<byte> values, out byte[] commandClasses,
            out byte[] secureCommandClasses)
        {
            List<byte> cmdClasses = new List<byte>();
            List<byte> secureCmdClasses = new List<byte>();
            CmdClassParseStates state = CmdClassParseStates.CmdClass;
            if (values != null)
            {
                foreach (var value in values)
                {
                    switch (state)
                    {
                        case CmdClassParseStates.CmdClass:
                            switch (value)
                            {
                                case 0xEF:
                                    state = CmdClassParseStates.ControlMark;
                                    break;
                                case 0xF1:
                                    state = CmdClassParseStates.SecureMark0;
                                    break;
                                default:
                                    if (value < 0xF0)
                                    {
                                        cmdClasses.Add(value);
                                    }
                                    break;
                            }
                            break;
                        case CmdClassParseStates.SecureCmdClass:
                            if (value == 0xEF)
                            {
                                state = CmdClassParseStates.SecureControlMark;
                            }
                            else if (value < 0xF0)
                            {
                                secureCmdClasses.Add(value);
                            }
                            break;
                        case CmdClassParseStates.ControlMark:
                            if (value == 0xF1)
                            {
                                state = CmdClassParseStates.SecureMark0;
                            }
                            break;
                        case CmdClassParseStates.SecureControlMark:
                            break;
                        case CmdClassParseStates.SecureMark0:
                            switch (value)
                            {
                                case 0x00:
                                    state = CmdClassParseStates.SecureCmdClass;
                                    break;
                                case 0xEF:
                                    state = CmdClassParseStates.ControlMark;
                                    break;
                                default:
                                    if (value < 0xF0)
                                    {
                                        cmdClasses.Add(value);
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            commandClasses = cmdClasses.Count > 0 ? cmdClasses.ToArray() : null;
            secureCommandClasses = secureCmdClasses.Count > 0 ? secureCmdClasses.ToArray() : null;
        }
    }
}