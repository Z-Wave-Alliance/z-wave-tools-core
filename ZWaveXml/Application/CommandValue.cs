/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace ZWave.Xml.Application
{
    public class CommandValue
    {
        public const string MsgLength = "MSG_LENGTH";
        public const string MsgMarker = "MSG_MARKER";

        private CommandClass _cmdClassRef;
        private Command _cmdRef;

        public Command CommandDefinition { get; set; }
        public ZWaveDefinition ZWaveDefinition { get; set; }
        public IList<ParamValue> ParamValues { get; set; }

        public CommandValue(ZWaveDefinition zWaveDefinition)
        {
            ZWaveDefinition = zWaveDefinition;
            ParamValues = new List<ParamValue>();
        }

        internal void ProcessParameters(byte[] data, byte initialOffsetInData)
        {
            ParamValues.Clear();
            _cmdClassRef = null;
            _cmdRef = null;
            int index = 0;
            if (CommandDefinition.Param == null)
                return;
            foreach (var item in CommandDefinition.Param)
            {
                ParamValue pv = ProcessParameter(data, ref index, item, null);
                if (item.Mode == ParamModes.VariantGroup && pv.InnerValues != null && pv.InnerValues.Count > 0)
                {
                    for (int i = 0; i < pv.InnerValues.Count; i++)
                    {
                        pv.InnerValues[i].ParamDefinitionTextSuffix = " " + (i + 1);
                        ParamValues.Add(pv.InnerValues[i]);
                    }
                }
                else
                    ParamValues.Add(pv);
            }
        }

        private ParamValue ProcessParameter(byte[] data, ref int index, Param item, IList<ParamValue> vgParams)
        {
            ParamValue ret = ParamValue.CreateParamValue(item);
            if (string.IsNullOrEmpty(item.OptionalReference) || IsIncluded(item.OptionalReference))
            {
                switch (item.Mode)
                {
                    case ParamModes.VariantGroup:
                        ret.InnerValues = new List<ParamValue>();
                        if (item.Size > 1)
                        {
                            for (int i = 0; i < item.Size; i++)
                            {
                                ParamValue vg = ParamValue.CreateParamValue(item);
                                vg.InnerValues = new List<ParamValue>();
                                foreach (var prm1 in item.Param1)
                                {
                                    ParamValue innerParam = ProcessParameter(data, ref index, prm1, vg.InnerValues);
                                    vg.InnerValues.Add(innerParam);
                                    vg.ByteValueList.AddRange(innerParam.ByteValueList);
                                }
                                ret.InnerValues.Add(vg);
                            }
                        }
                        else if (item.SizeReference != null && item.SizeReference == MsgLength)
                        {
                            while (data.Length - GetTailSize(CommandDefinition) > index)
                            {
                                ParamValue vg = ParamValue.CreateParamValue(item);
                                vg.InnerValues = new List<ParamValue>();
                                foreach (var prm1 in item.Param1)
                                {
                                    ParamValue innerParam = ProcessParameter(data, ref index, prm1, vg.InnerValues);
                                    vg.InnerValues.Add(innerParam);
                                    vg.ByteValueList.AddRange(innerParam.ByteValueList);
                                }
                                ret.InnerValues.Add(vg);
                                if (!string.IsNullOrEmpty(item.MoreToFollowReference) && !vg.IsIncluded(item.MoreToFollowReference))
                                {
                                    break;
                                }
                            }
                        }
                        else if (item.SizeReference != null && item.SizeReference == MsgMarker)
                        {
                            byte[] marker = GetMarkerValue(item);
                            while (data.Length - GetTailSize(CommandDefinition) > index && !IsMarker(data, index, marker))
                            {
                                ParamValue vg = ParamValue.CreateParamValue(item);
                                vg.InnerValues = new List<ParamValue>();
                                foreach (var prm1 in item.Param1)
                                {
                                    ParamValue innerParam = ProcessParameter(data, ref index, prm1, vg.InnerValues);
                                    vg.InnerValues.Add(innerParam);
                                    vg.ByteValueList.AddRange(innerParam.ByteValueList);
                                }
                                ret.InnerValues.Add(vg);
                            }
                        }
                        else if (item.SizeReference != null)
                        {
                            for (int i = 0; i < GetSize(item.SizeReference, vgParams); i++)
                            {
                                ParamValue vg = ParamValue.CreateParamValue(item);
                                vg.InnerValues = new List<ParamValue>();
                                foreach (var prm1 in item.Param1)
                                {
                                    ParamValue innerParam = ProcessParameter(data, ref index, prm1, vg.InnerValues);
                                    vg.InnerValues.Add(innerParam);
                                    vg.ByteValueList.AddRange(innerParam.ByteValueList);
                                }
                                ret.InnerValues.Add(vg);
                            }
                        }
                        break;
                    case ParamModes.Property:
                        ret.InnerValues = new List<ParamValue>();
                        byte bvalue = data.Length > index ? data[index++] : (byte)0;
                        ret.ByteValueList.Add(bvalue);
                        byte totalbits = 0;
                        foreach (var prm1 in item.Param1)
                        {
                            ParamValue sp = ParamValue.CreateParamValue(prm1);
                            sp.ByteValueList.Add((byte)(bvalue >> totalbits & Tools.GetMaskFromBits(prm1.Bits, 0)));
                            ret.InnerValues.Add(sp);
                            totalbits += prm1.Bits;
                        }
                        break;
                    default:
                        byte bytesCount = GetParamValueBytes(item);
                        if (item.Size > 1)
                        {
                            for (int i = 0; i < item.Size; i++)
                            {
                                for (int j = 0; j < bytesCount; j++)
                                {
                                    if (data.Length > index)
                                    {
                                        ret.ByteValueList.Add(data[index++]);
                                    }
                                }
                            }
                        }
                        else if (item.SizeReference != null && item.SizeReference == MsgLength)
                        {
                            if (data.Length > 1000)
                            {

                            }
                            while (data.Length - GetTailSize(CommandDefinition) > index)
                            {
                                for (int j = 0; j < bytesCount; j++)
                                {
                                    if (data.Length > index)
                                    {
                                        ret.ByteValueList.Add(data[index++]);
                                    }
                                }
                            }
                        }
                        else if (item.SizeReference != null && item.SizeReference == MsgMarker)
                        {
                            byte[] marker = GetMarkerValue(item);
                            while (data.Length - GetTailSize(CommandDefinition) > index && !IsMarker(data, index, marker))
                            {
                                for (int j = 0; j < bytesCount; j++)
                                {
                                    if (data.Length > index)
                                    {
                                        ret.ByteValueList.Add(data[index++]);
                                    }
                                }
                            }
                        }
                        else if (item.SizeReference != null)
                        {
                            for (int i = 0; i < GetSize(item.SizeReference, vgParams) + item.SizeChange; i++)
                            {
                                for (int j = 0; j < bytesCount; j++)
                                {
                                    if (data.Length > index)
                                    {
                                        ret.ByteValueList.Add(data[index++]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < bytesCount; j++)
                            {
                                if (data.Length > index)
                                {
                                    ret.ByteValueList.Add(data[index++]);
                                }
                            }
                        }

                        if (item.Type == zwParamType.CMD_CLASS_REF && ret.ByteValueList != null && ret.ByteValueList.Count > 0)
                        {
                            _cmdClassRef = ZWaveDefinition.CommandClasses.OrderByDescending(x=>x.Version).FirstOrDefault(x => x.KeyId == ret.ByteValueList[0]);
                        }
                        else if (item.Type == zwParamType.CMD_REF && _cmdClassRef != null)
                        {
                            if (_cmdClassRef.Command != null && ret.ByteValueList != null && ret.ByteValueList.Count > 0)
                            {
                                _cmdRef = _cmdClassRef.Command.FirstOrDefault(x => x.KeyId == ret.ByteValueList[0]);
                            }
                        }
                        if (item.Type == zwParamType.CMD_DATA && _cmdClassRef != null && _cmdRef != null && ret.ByteValueList != null && ret.ByteValueList.Count > 0)
                        {
                            byte[] cmddata = new byte[ret.ByteValueList.Count + 2];
                            cmddata[0] = _cmdClassRef.KeyId;
                            cmddata[1] = _cmdRef.KeyId;
                            Array.Copy(ret.ByteValueList.ToArray(), 0, cmddata, 2, cmddata.Length - 2);

                            ZWaveDefinition.ParseApplicationObject(cmddata, out CommandClassValue[] encapCmds);
                            ret.InnerValues = new List<ParamValue>();
                            if (encapCmds != null)
                            {
                                foreach (var ec in encapCmds)
                                {
                                    if (encapCmds.Length > 1)
                                    {
                                        ParamValue pv = new ParamValue
                                        {
                                            ParamDefinition = item,
                                            ParamDefinitionTextSuffix = ", ver." + ec.CommandClassDefinition.Version,
                                            InnerValues = ec.CommandValue.ParamValues
                                        };
                                        ret.InnerValues.Add(pv);
                                    }
                                    else
                                    {
                                        ret.InnerValues = ec.CommandValue.ParamValues;
                                    }
                                }
                            }

                        }
                        if (item.Type == zwParamType.CMD_ENCAP && ret.ByteValueList != null && ret.ByteValueList.Count > 0)
                        {
                            byte[] cmddata = new byte[ret.ByteValueList.Count];
                            Array.Copy(ret.ByteValueList.ToArray(), 0, cmddata, 0, cmddata.Length);

                            ZWaveDefinition.ParseApplicationObject(cmddata, out CommandClassValue[] encapCmds);
                            ret.InnerValues = new List<ParamValue>();
                            if (encapCmds != null)
                            {
                                foreach (var ec in encapCmds)
                                {
                                    if (encapCmds.Length > 1)
                                    {
                                        ParamValue pv = new ParamValue
                                        {
                                            ParamDefinition = item,
                                            ParamDefinitionTextPrefix = ec.CommandValue.CommandDefinition.Name,
                                            ParamDefinitionTextSuffix = ", ver." + ec.CommandClassDefinition.Version,
                                            InnerValues = ec.CommandValue.ParamValues
                                        };
                                        ret.InnerValues.Add(pv);
                                    }
                                    else
                                    {
                                        ret.InnerValues = ec.CommandValue.ParamValues;
                                    }
                                }
                            }

                        }
                        break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns ParamValues as dictionary
        /// </summary>
        /// <param name="isIncludeParentKey">dictionary key contains parent param key if 'true'. Example: "properties1.level"</param>
        /// <param name="useQualifiedParamNameAsKey">dictionary key contains parameter's 'Name' instead of parameter's 'Text' attribute if 'true'</param>
        /// <returns>dictionary</returns>
        public Dictionary<string, List<ParamValue>> ToParamValuesDictionary(bool isIncludeParentKey, bool useQualifiedParamNameAsKey)
        {
            return ZWaveDefinition.ToParamValuesDictionary(ParamValues, isIncludeParentKey, useQualifiedParamNameAsKey);
        }

        private static bool IsMarker(byte[] data, int index, byte[] marker)
        {
            bool ret = true;
            if (data.Length >= marker.Length + index)
            {
                for (int i = 0; i < marker.Length; i++)
                {
                    ret &= data[i + index] == marker[i];
                }
            }
            return ret;
        }

        private static byte[] GetMarkerValue(Param item)
        {
            byte[] ret = null;
            if (item.ParentParam != null)
            {
                foreach (var p in item.ParentParam.Param1)
                {
                    if (p.Type == zwParamType.MARKER)
                    {
                        ret = p.DefaultValue;
                        break;
                    }
                }
            }
            else if (item.ParentCmd != null)
            {
                foreach (var p in item.ParentCmd.Param)
                {
                    if (p.Type == zwParamType.MARKER)
                    {
                        ret = p.DefaultValue;
                        break;
                    }
                }
            }
            return ret;
        }

        private static byte GetParamValueBytes(Param item)
        {
            return (byte)(item.Bits / 8);
        }

        private IEnumerable<byte> GetBytesValue(string optRef)
        {
            IEnumerable<byte> ret = null;
            string[] tokens = optRef.Split('.');
            ParamValue p = ParamValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[0]);
            if (p != null)
            {
                if (tokens.Length == 2)
                {
                    ParamValue s = p.InnerValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[1]);
                    if (s != null)
                    {
                        ret = s.ByteValueList;
                    }
                }
                else
                {
                    ret = p.ByteValueList;
                }
            }
            return ret;
        }

        private bool IsIncluded(string optRef)
        {
            bool ret = false;
            try
            {
                var result = GetBytesValue(optRef);
                if (result != null)
                {
                    ret = result.Sum(x => x) > 0;
                }
            }
            catch (Exception ex)
            {
                ex.Message._DLOG();
            }
            return ret;
        }

        private byte GetSize(string sizeRef, IList<ParamValue> vgParams)
        {
            byte ret = 0;
            string[] tokens = sizeRef.Split('.');
            bool inParent = false;
            if (sizeRef.StartsWith("Parent."))
            {
                tokens = sizeRef.Substring(7).Split('.');
                inParent = true;
            }
            ParamValue p;
            if (vgParams == null || inParent)
            {
                p = ParamValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[0]);
            }
            else
            {
                p = vgParams.FirstOrDefault(x => x.ParamDefinition.Name == tokens[0]);
            }

            if (p != null)
            {
                if (tokens.Length == 2)
                {
                    ParamValue s = p.InnerValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[1]);
                    if (s != null)
                    {
                        if (s.ByteValueList.Count > 0)
                        {
                            ret = s.ByteValueList[0];
                        }
                    }
                }
                else
                {
                    if (p.ByteValueList.Count > 0)
                    {
                        ret = p.ByteValueList[0];
                    }
                }
            }
            return ret;
        }

        private static byte GetParamBits(Param prm)
        {
            byte ret = prm.Bits;
            if (ret == 0)
                ret = 8;
            if (prm.Size > 1)
                ret *= prm.Size;
            return ret;
        }

        private static byte GetTailSize(Command cmd)
        {
            byte ret = 0;
            if (cmd.Param != null && cmd.Param.Count > 0)
            {
                for (int i = cmd.Param.Count - 1; i >= 0; i--)
                {
                    Param item = cmd.Param[i];
                    if (item.SizeReference != null || item.Param1 != null && item.Param1.Count > 0)
                        break;
                    ret += GetParamBits(item);
                }
            }
            return (byte)(ret / 8);
        }
    }
}
