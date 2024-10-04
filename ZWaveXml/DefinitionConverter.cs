/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Utils;
using ZWave.Xml.Application;

namespace ZWave.Xml
{
    internal class DefinitionConverter
    {
        public const string MsgLength = "MSG_LENGTH";
        public const string MsgMarker = "MSG_MARKER";
        public const string SpecificTypeNotUsed = "SPECIFIC_TYPE_NOT_USED";
        public const string Vg = "vg";
        public ZWaveDefinition ZWaveDefinition { get; set; }
        public ZWaveDefinition ZWaveTempDefinition { get; set; }
        public zw_classes ZXmlDefinition { get; set; }

        public string ZWaveDefinitionFile { get; set; }
        public string ZXmlDefinitionFile { get; set; }


        public DefinitionConverter(string zxmlDefinitionFile, string zwaveDefinitionFile)
        {
            ZXmlDefinitionFile = zxmlDefinitionFile;
            ZWaveDefinitionFile = zwaveDefinitionFile;
            if (File.Exists(zxmlDefinitionFile))
            {
                LoadZXmlDefinition(zxmlDefinitionFile);
            }
            else
            {
                ZXmlDefinition = new zw_classes();
            }
            if (File.Exists(zwaveDefinitionFile))
            {
                LoadZWaveDefinition(zwaveDefinitionFile);
            }
            else
            {
                ZWaveDefinition = new ZWaveDefinition
                {
                    BasicDevices = new Collection<BasicDevice>(),
                    CommandClasses = new Collection<CommandClass>(),
                    GenericDevices = new Collection<GenericDevice>()
                };
            }
            ZWaveTempDefinition = new ZWaveDefinition
            {
                BasicDevices = new Collection<BasicDevice>(),
                CommandClasses = new Collection<CommandClass>(),
                GenericDevices = new Collection<GenericDevice>()
            };
        }

        private readonly XmlSerializer _zwXmlNewSerializer = new XmlSerializer(typeof(ZWaveDefinition));

        private void LoadZWaveDefinition(string zWaveDefinitionFileName)
        {
            XmlReader reader = XmlReader.Create(zWaveDefinitionFileName);
            try
            {
                ZWaveDefinition = (ZWaveDefinition)_zwXmlNewSerializer.Deserialize(reader);
                AssignParentProperties();
            }
            catch (InvalidOperationException)
            {
            }
            finally
            {
                reader.Close();
            }
        }

        public bool SaveZWaveDefinition(string zWaveDefinitionFileName)
        {
            bool ret = false;
            XmlWriterSettings sett = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = Environment.NewLine
            };
            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add(string.Empty, string.Empty);
            XmlWriter writer = XmlWriter.Create(zWaveDefinitionFileName, sett);
            try
            {
                _zwXmlNewSerializer.Serialize(writer, ZWaveDefinition, names);
                ret = true;
            }
            catch (InvalidOperationException)
            {
            }
            finally
            {
                writer.Close();
            }
            return ret;
        }

        private void LoadZXmlDefinition(string zXmlDefinitionFileName)
        {
            XmlReader reader = XmlReader.Create(zXmlDefinitionFileName);
            try
            {
                ZXmlDefinition = (zw_classes)_zwXmlOldSerializer.Deserialize(reader);
            }
            catch (InvalidOperationException iex)
            {
                iex.Message._EXLOG();
            }
            finally
            {
                reader.Close();
            }
        }

        private void AssignParentProperties()
        {
            foreach (var generic in ZWaveDefinition.GenericDevices)
            {
                if (generic.SpecificDevice != null)
                    foreach (var specific in generic.SpecificDevice)
                    {
                        specific.Parent = generic;
                    }
            }
            foreach (var item in ZWaveDefinition.CommandClasses)
            {
                if (item.DefineSet != null)
                    foreach (var ds in item.DefineSet)
                    {
                        ds.Parent = item;
                        if (ds.Define != null)
                            foreach (var d in ds.Define)
                            {
                                d.Parent = ds;
                                if (d.Define1 != null)
                                    foreach (var d1 in d.Define1)
                                    {
                                        d1.Parent = ds;
                                        d1.ParentDefine = d;
                                    }
                            }
                    }
                if (item.Command != null)
                    foreach (var cmd in item.Command)
                    {
                        cmd.Parent = item;
                        if (cmd.Param != null)
                            foreach (var par in cmd.Param)
                            {
                                par.ParentCmd = cmd;
                                AssignParentPropertiesInner(cmd, par);
                            }
                    }
            }
        }

        private static void AssignParentPropertiesInner(Command cmd, Param prm)
        {
            if (prm.Param1 != null)
                foreach (var p in prm.Param1)
                {
                    p.ParentParam = prm;
                    p.ParentCmd = cmd;
                    AssignParentPropertiesInner(cmd, p);
                }
        }

        private static string GetReference(string valName, IList<Param> searchScope, IList<Param> variantScope,
            byte parameterNumber, byte mask)
        {
            string ret = null;
            if (parameterNumber == 0xFF)
                return MsgLength;
            Param paramStructByte = null;
            string pre = "";
            if (variantScope != null)
            {
                if (parameterNumber > 0x7F) // parameter number placed outside the variant group
                {
                    if (searchScope.Count > parameterNumber - 0x80)
                    {
                        paramStructByte = searchScope[parameterNumber - 0x80];
                    }
                    pre = "Parent.";
                }
                else
                {
                    if (variantScope.Count > parameterNumber)
                    {
                        paramStructByte = variantScope[parameterNumber];
                    }
                }
            }
            else
            {
                if (searchScope.Count > parameterNumber)
                {
                    paramStructByte = searchScope[parameterNumber];
                }
            }
            if (paramStructByte != null)
            {
                if (mask != 255 && paramStructByte.Param1 != null && paramStructByte.Param1.Count > 0)
                {
                    byte totalbits = 0;
                    foreach (var item in paramStructByte.Param1)
                    {
                        if (mask == Tools.GetMaskFromBits(item.Bits, totalbits))
                        {
                            ret = Tools.FormatStr("{0}.{1}", paramStructByte.Name, item.Name);
                        }
                        totalbits += item.Bits;
                    }
                }
                else
                {
                    ret = paramStructByte.Name;
                }
            }
            else
            {
                "!!!Reference not found: {0}"._DLOG(valName);
            }
            return pre + ret;
        }

        public bool SaveZXmlDefinition(string zXmlDefinitionFileName)
        {
            FileIOPermission permission2 = new FileIOPermission(FileIOPermissionAccess.AllAccess,
                Path.GetFullPath(zXmlDefinitionFileName));
            try
            {
                permission2.Demand();
            }
            catch (SecurityException s)
            {
                //logger("AC: SecurityException when temp file requested (init)");
                s.Message._EXLOG();
            }
            bool ret = false;
            XmlWriterSettings sett = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = Environment.NewLine
            };
            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add(string.Empty, string.Empty);
            XmlWriter writer = XmlWriter.Create(zXmlDefinitionFileName, sett);
            try
            {
                _zwXmlOldSerializer.Serialize(writer, ZXmlDefinition, names);
                ret = true;
            }
            catch (InvalidOperationException)
            {
            }
            finally
            {
                writer.Close();
            }
            return ret;
        }

        public void UpgradeConvert(bool isSaveConverted)
        {
            if (ZXmlDefinition == null)
                return;
            if (ZXmlDefinition.Items == null)
                return;
            ZWaveDefinition = new ZWaveDefinition
            {
                BasicDevices = new Collection<BasicDevice>(),
                CommandClasses = new Collection<CommandClass>(),
                GenericDevices = new Collection<GenericDevice>(),
            };
            ZWaveDefinition.Version = ZXmlDefinition.version;
            foreach (var item in ZXmlDefinition.Items)
            {
                if (item is bas_dev)
                {
                    ZWaveDefinition.BasicDevices.Add(UpgradeBasicDevice(item));
                }
                if (item is gen_dev)
                {
                    ZWaveDefinition.GenericDevices.Add(UpgradeGenericDevice(item));
                }
                if (item is cmd_class)
                {
                    ZWaveDefinition.CommandClasses.Add(UpgradeCommandClass(item));
                }
            }
            AssignParentProperties();
            /* 
             * update Commands supporting mode according to trunk\Various\CommandsSupportMode.xlsx
             * (selected from SDS13548) */
            //AssignCommandsModes(); 
            if (isSaveConverted)
            {
                FileIOPermission permission2 = new FileIOPermission(FileIOPermissionAccess.AllAccess,
                    Path.GetFullPath(ZWaveDefinitionFile));
                try
                {
                    permission2.Demand();
                }
                catch (SecurityException s)
                {
                    s.Message._EXLOG();
                }
                SaveZWaveDefinition(ZWaveDefinitionFile);
            }
        }

        private void AssignCommandsModes()
        {
            var fp = @"D:\anicca\trunk\Various\commandsSupportModes.txt";
            if (File.Exists(fp))
            {
                var lines = File.ReadAllLines(fp);
                foreach (var cmdCls in ZWaveDefinition.CommandClasses.Where(x => x.Command != null))
                {
                    foreach (var cmd in cmdCls.Command)
                    {
                        var l = lines.FirstOrDefault(x => x.Contains(cmd.Name));
                        if (!string.IsNullOrEmpty(l))
                        {
                            var values = l.Trim().Replace("\t", ";").Split(';');
                            var mode = zwSupportModes.APP.ToString();
                            if (values.Length == 2 && !string.IsNullOrEmpty(values[1]))
                            {
                                mode = values[1];
                            }
                            if (mode == zwSupportModes.TX_RX.ToString())
                            {
                                cmd.SupportMode = zwSupportModes.TX_RX;
                            }
                            else if (mode == zwSupportModes.TX.ToString())
                            {
                                cmd.SupportMode = zwSupportModes.TX;
                            }
                            else if (mode == zwSupportModes.RX.ToString())
                            {
                                cmd.SupportMode = zwSupportModes.RX;
                            }
                            else if (mode == zwSupportModes.APP.ToString())
                            {
                                cmd.SupportMode = zwSupportModes.APP;
                            }
                        }
                    }
                }
            }
        }

        public void UpgradeTempConvert()
        {
            ZWaveTempDefinition = new ZWaveDefinition
            {
                BasicDevices = new Collection<BasicDevice>(),
                CommandClasses = new Collection<CommandClass>(),
                GenericDevices = new Collection<GenericDevice>()
            };
            foreach (var item in ZXmlDefinition.Items)
            {
                if (item is bas_dev)
                {
                    ZWaveTempDefinition.BasicDevices.Add(UpgradeBasicDevice(item));
                }
                if (item is gen_dev)
                {
                    ZWaveTempDefinition.GenericDevices.Add(UpgradeGenericDevice(item));
                }
                if (item is cmd_class)
                {
                    ZWaveTempDefinition.CommandClasses.Add(UpgradeCommandClass(item));
                }
            }
            AssignParentProperties();
        }

        private CommandClass UpgradeCommandClass(object commandClass)
        {
            if (!(commandClass is cmd_class))
                return null;
            cmd_class val = (cmd_class)commandClass;
            CommandClass ret = new CommandClass
            {
                Comment = val.comment,
                Key = val.key,
                Name = val.name,
                Text = val.help,
                Version = byte.Parse(val.version)
            };
            IList<DefineSet> defineSetList = new List<DefineSet>();
            int definesCounter = 0;
            if (val.cmd != null)
            {
                IList<Command> commands = val.cmd.Select(item => UpgradeCommand(ret, item, ref defineSetList, ref definesCounter)).ToList();
                if (commands.Count > 0)
                {
                    ret.Command = new Collection<Command>(commands);
                }
            }
            if (defineSetList.Count > 0)
            {
                ret.DefineSet = new Collection<DefineSet>(defineSetList);
            }
            return ret;
        }

        private static GenericDevice UpgradeGenericDevice(object genericDevice)
        {
            if (!(genericDevice is gen_dev))
                return null;
            gen_dev val = (gen_dev)genericDevice;
            GenericDevice ret = new GenericDevice
            {
                Comment = val.comment,
                Key = val.key,
                Name = val.name,
                Text = val.help
            };
            if (val.spec_dev != null)
            {
                IList<SpecificDevice> specificDevices = val.spec_dev.Select(item => UpgradeSpecificDevice(item, ret)).Where(sd => sd != null).ToList();
                if (specificDevices.Count > 0)
                    ret.SpecificDevice = new Collection<SpecificDevice>(specificDevices);
            }
            return ret;
        }

        private static BasicDevice UpgradeBasicDevice(object basicDevice)
        {
            if (!(basicDevice is bas_dev))
                return null;
            bas_dev val = (bas_dev)basicDevice;
            BasicDevice ret = new BasicDevice
            {
                Comment = val.comment,
                Key = val.key,
                Name = val.name,
                Text = val.help
            };
            return ret;
        }

        private static SpecificDevice UpgradeSpecificDevice(object specificDevice, GenericDevice parentGeneric)
        {
            if (!(specificDevice is spec_dev))
                return null;
            spec_dev val = (spec_dev)specificDevice;
            SpecificDevice ret = new SpecificDevice
            {
                Comment = val.comment,
                Key = val.key,
                Name = val.name,
                Text = val.help
            };
            if (val.name == SpecificTypeNotUsed)
            {
                ret.ScopeKeyId = parentGeneric.KeyId;
            }
            return ret;
        }

        private Command UpgradeCommand(CommandClass cmdClass, object command, ref IList<DefineSet> defineSetList,
            ref int definesCounter)
        {
            if (!(command is cmd))
                return null;
            cmd val = (cmd)command;
            Command ret = new Command { Comment = val.comment };
            if (val.cmd_mask != null)
            {
                ret.Bits = Tools.GetBitsFromMask(Tools.GetByte(val.cmd_mask));
                ret.BitsSpecified = true;
            }
            ret.Key = val.key;
            ret.Name = val.name;
            ret.SupportMode = (zwSupportModes)val.support_mode;
            ret.Text = val.help;
            if (val.Items != null)
            {
                IList<Param> pars = new List<Param>();
                byte structCount = 0;
                foreach (var item in val.Items)
                {
                    Param p = UpgradeParam(cmdClass, ret, item, pars, null, ref defineSetList, ref definesCounter,
                        ref structCount);
                    pars.Add(p);
                }
                if (pars.Count > 0)
                    ret.Param = new Collection<Param>(pars);
            }
            return ret;
        }

        private Param UpgradeParam(CommandClass cmdClass, Command cmd, object param, IList<Param> cmdParams,
            IList<Param> vgParams, ref IList<DefineSet> defineSetList, ref int definesCounter, ref byte structCount)
        {
            Param ret = null;
            var val = param as param;
            var vg = param as variant_group;
            if (val != null)
            {
                IList<Define> defines;
                Param parameter;
                switch (val.type)
                {
                    case zwXmlParamType.ARRAY:
                        ret = UgradeParameterTypeArray(val, cmdParams, vgParams);
                        break;
                    case zwXmlParamType.BIT_24:
                        ret = UgradeParameterTypeBit24(val);
                        break;
                    case zwXmlParamType.BITMASK:
                        defines = new List<Define>();
                        parameter = UgradeParameterTypeBitmask(val, cmdParams, vgParams, ref defines);
                        if (defines.Count > 0)
                        {
                            DefineSet defineSet = new DefineSet
                            {
                                Type = zwDefineSetType.Unknown,
                                Define = new Collection<Define>(defines)
                            };
                            MergeDefineSet(ref defineSetList, defineSet, ref parameter, ref definesCounter);
                        }
                        ret = parameter;
                        break;
                    case zwXmlParamType.BYTE:
                        defines = new List<Define>();
                        parameter = UgradeParameterTypeByte(val, ref defines);
                        if (defines.Count > 0)
                        {
                            DefineSet defineSet = new DefineSet
                            {
                                Type = zwDefineSetType.Unknown,
                                Define = new Collection<Define>(defines)
                            };
                            MergeDefineSet(ref defineSetList, defineSet, ref parameter, ref definesCounter);
                        }
                        ret = parameter;
                        break;
                    case zwXmlParamType.CONST:
                        defines = new List<Define>();
                        parameter = UgradeParameterTypeConst(val, ref defines);
                        if (defines.Count > 0)
                        {
                            DefineSet defineSet = new DefineSet
                            {
                                Type = zwDefineSetType.Full,
                                Define = new Collection<Define>(defines)
                            };
                            MergeDefineSet(ref defineSetList, defineSet, ref parameter, ref definesCounter);
                        }
                        ret = parameter;
                        break;
                    case zwXmlParamType.DWORD:
                        ret = UgradeParameterTypeDWord(val);
                        break;
                    case zwXmlParamType.MARKER:
                        if (vgParams != null)
                            vgParams.Last().SizeReference = MsgMarker;
                        else
                            cmdParams.Last().SizeReference = MsgMarker;
                        ret = UgradeParameterTypeMarker(val);
                        break;
                    case zwXmlParamType.MULTI_ARRAY:
                        defines = new List<Define>();
                        parameter = UgradeParameterTypeMultiArray(val, ref defines);
                        if (defines.Count > 0)
                        {
                            DefineSet defineSet = new DefineSet
                            {
                                Type = zwDefineSetType.Full,
                                Define = new Collection<Define>(defines)
                            };
                            MergeDefineSet(ref defineSetList, defineSet, ref parameter, ref definesCounter);
                        }
                        ret = parameter;
                        break;
                    case zwXmlParamType.STRUCT_BYTE:
                        ret = UgradeParameterTypeStructByte(val, ref defineSetList, ref definesCounter,
                            ref structCount);
                        break;
                    case zwXmlParamType.VARIANT:
                        ret = UgradeParameterTypeVariant(val, cmdParams, vgParams);
                        break;
                    case zwXmlParamType.WORD:
                        ret = UgradeParameterTypeWord(val);
                        break;
                    default:
                        ret = UpgradeParameterCommon(val);
                        break;
                }
                if (val.optionaloffs != null)
                    ret.OptionalReference = GetReference(val.name, cmdParams, vgParams, Tools.GetByte(val.optionaloffs),
                        Tools.GetByte(val.optionalmask));
            }
            else if (vg != null)
            {
                ret = UpgradeVariantGroup(cmdClass, cmd, vg, vgParams ?? cmdParams, ref defineSetList, ref definesCounter);
            }

            bool hasNameAlready = false;
            if (ret != null && ret.Mode == ParamModes.Property)
            {
                foreach (var item in ret.Param1)
                {
                    if (vgParams != null)
                    {
                        hasNameAlready = LookUp(vgParams, item);
                    }
                    else if (cmdParams != null)
                    {
                        hasNameAlready = LookUp(cmdParams, item);
                    }
                    if (hasNameAlready)
                    {
                        "DDD: {0} IN {1}.{2} v{3}"._DLOG(item.Text, cmdClass.Name, cmd.Name, cmdClass.Version);
                    }
                }
            }
            return ret;
        }

        private static bool LookUp(IList<Param> vgParams, Param prm)
        {
            bool ret = false;
            var res = vgParams.Where(x => x.Name == prm.Name).ToArray();
            if (res.Length > 0)
            {
                ret = true;
            }
            else
            {
                foreach (var item in vgParams)
                {
                    if (item.Param1 != null)
                        ret = LookUp(item.Param1, prm);
                    if (ret)
                        break;
                }
            }
            return ret;
        }

        private static void MergeDefineSet(ref IList<DefineSet> defineSetList, DefineSet defineSet, ref Param parameter,
            ref int counter)
        {
            string str = ContainDefineSet(defineSetList, defineSet);
            if (str == null)
            {
                str = ContainDefineSetName(defineSetList, parameter.Name) ? parameter.Name + ++counter : parameter.Name;
                defineSet.Name = str;
                defineSetList.Add(defineSet);
            }
            parameter.Defines = str;
        }

        private static bool ContainDefineSetName(IEnumerable<DefineSet> defineSetList, string name)
        {
            return defineSetList.Any(item => item.Name == name);
        }

        /// <summary>
        /// Looks for similar define set.
        /// </summary>
        /// <param name="defineSetList">The define set list.</param>
        /// <param name="defineSet">The define set.</param>
        /// <returns>name of the define set if found</returns>
        private static string ContainDefineSet(IEnumerable<DefineSet> defineSetList, DefineSet defineSet)
        {
            string ret = null;
            foreach (var item in defineSetList)
            {
                int mismatches = defineSet.Define.Count;
                if (item.Define.Count != mismatches)
                {
                    continue;
                }
                foreach (var defItem1 in item.Define)
                {
                    foreach (var defItem2 in defineSet.Define)
                    {
                        if (defItem1.Name == defItem2.Name && defItem1.Key == defItem2.Key)
                        {
                            mismatches--;
                        }
                    }
                }
                if (mismatches == 0)
                {
                    ret = item.Name;
                    break;
                }
            }
            return ret;
        }

        private static Param UpgradeParameterCommon(param val)
        {
            Param ret = new Param
            {
                Type = zwParamType.HEX,
            };
            ret.Type = FromEncapType(val.encaptype);
            ret.Comment = val.comment;
            ret.SkipField = val.skipfield;
            ret.SkipFieldSpecified = val.skipfield;
            ret.Name = Tools.MakeLegalMixCaseIdentifier(val.name);
            ret.Text = val.name;
            return ret;
        }

        private static void ToEncapType(Param param, param ret)
        {
            switch (param.Type)
            {
                case zwParamType.NODE_NUMBER:
                    ret.encaptype = zwXmlEncapType.NODE_NUMBER;
                    break;
                case zwParamType.BAS_DEV_REF:
                    ret.encaptype = zwXmlEncapType.BAS_DEV_REF;
                    break;
                case zwParamType.GEN_DEV_REF:
                    ret.encaptype = zwXmlEncapType.GEN_DEV_REF;
                    break;
                case zwParamType.SPEC_DEV_REF:
                    ret.encaptype = zwXmlEncapType.SPEC_DEV_REF;
                    break;
                case zwParamType.CMD_CLASS_REF:
                    ret.encaptype = zwXmlEncapType.CMD_CLASS_REF;
                    break;
                case zwParamType.CMD_REF:
                    ret.encaptype = zwXmlEncapType.CMD_REF;
                    break;
                case zwParamType.CMD_DATA:
                    ret.encaptype = zwXmlEncapType.CMD_DATA;
                    break;
                case zwParamType.CMD_ENCAP:
                    ret.encaptype = zwXmlEncapType.CMD_ENCAP;
                    break;
            }
        }

        private static zwParamType FromEncapType(zwXmlEncapType zwXmlEncapType)
        {
            zwParamType ret = zwParamType.HEX;
            switch (zwXmlEncapType)
            {
                case zwXmlEncapType.HEX:
                    ret = zwParamType.HEX;
                    break;
                case zwXmlEncapType.BOOLEAN:
                    ret = zwParamType.BOOLEAN;
                    break;
                case zwXmlEncapType.CHAR:
                    ret = zwParamType.CHAR;
                    break;
                case zwXmlEncapType.NUMBER:
                    ret = zwParamType.NUMBER;
                    break;
                case zwXmlEncapType.NUMBER_SIGNED:
                    ret = zwParamType.NUMBER_SIGNED;
                    break;
                case zwXmlEncapType.NODE_NUMBER:
                    ret = zwParamType.NODE_NUMBER;
                    break;
                case zwXmlEncapType.BITMASK:
                    ret = zwParamType.BITMASK;
                    break;
                case zwXmlEncapType.MARKER:
                    ret = zwParamType.MARKER;
                    break;
                case zwXmlEncapType.BAS_DEV_REF:
                    ret = zwParamType.BAS_DEV_REF;
                    break;
                case zwXmlEncapType.GEN_DEV_REF:
                    ret = zwParamType.GEN_DEV_REF;
                    break;
                case zwXmlEncapType.SPEC_DEV_REF:
                    ret = zwParamType.SPEC_DEV_REF;
                    break;
                case zwXmlEncapType.CMD_CLASS_REF:
                    ret = zwParamType.CMD_CLASS_REF;
                    break;
                case zwXmlEncapType.CMD_REF:
                    ret = zwParamType.CMD_REF;
                    break;
                case zwXmlEncapType.CMD_DATA:
                    ret = zwParamType.CMD_DATA;
                    break;
                case zwXmlEncapType.CMD_ENCAP:
                    ret = zwParamType.CMD_ENCAP;
                    break;
            }
            return ret;
        }

        private static Param UgradeParameterTypeWord(param val)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 16;
            return ret;
        }

        private static Param UgradeParameterTypeVariant(param val, IList<Param> cmdParams, IList<Param> vgParams)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 8;
            foreach (var item in val.Items)
            {
                var itemAttrib = item as variant;
                if (itemAttrib != null)
                {
                    if (itemAttrib.is_ascii)
                    {
                        ret.Type = zwParamType.CHAR;
                    }
                    ret.SizeReference = GetReference(val.name, cmdParams, vgParams, itemAttrib.paramoffs,
                        Tools.GetByte(itemAttrib.sizemask));
                    ret.SizeChange = itemAttrib.sizechange;
                    ret.SizeChangeSpecified = ret.SizeChange != 0;
                }
            }
            return ret;
        }

        private static Param UgradeParameterTypeStructByte(param val, ref IList<DefineSet> defineSetList,
            ref int definesCounter, ref byte structCount)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Mode = ParamModes.Property;
            ret.Name = Tools.FormatStr("properties{0}", ++structCount);
            ret.Bits = 8;
            ret.Param1 = new Collection<Param>();

            byte sumBits = 0;
            if (val.cmd_mask != null)
            {
                sumBits = (byte)(8 - Tools.GetBitsFromMask(Tools.GetByte(val.cmd_mask)));
            }
            foreach (var item in val.Items)
            {
                Param p = new Param { Type = zwParamType.HEX };

                var itemBitfield = item as bitfield;
                var itemBitflag = item as bitflag;
                var itemFieldenum = item as fieldenum;
                if (itemBitfield != null)
                {
                    p.Bits = Tools.GetBitsFromMask(Tools.GetByte(itemBitfield.fieldmask));
                    sumBits += p.Bits;
                    p.Name = Tools.MakeLegalMixCaseIdentifier(itemBitfield.fieldname);
                    p.Text = itemBitfield.fieldname;
                    ret.Param1.Add(p);
                }
                else if (itemBitflag != null)
                {
                    p.Bits = 1;
                    sumBits += p.Bits;
                    p.Type = zwParamType.BOOLEAN;
                    p.Name = Tools.MakeLegalMixCaseIdentifier(itemBitflag.flagname);
                    p.Text = itemBitflag.flagname;
                    ret.Param1.Add(p);
                }
                else if (itemFieldenum != null)
                {
                    p.Bits = Tools.GetBitsFromMask(Tools.GetByte(itemFieldenum.fieldmask));
                    sumBits += p.Bits;
                    p.Name = Tools.MakeLegalMixCaseIdentifier(itemFieldenum.fieldname);
                    p.Text = itemFieldenum.fieldname;
                    if (itemFieldenum.fieldenum1 != null)
                    {
                        byte counter = 0;
                        IList<Define> defines = new List<Define>();
                        byte innerCounter = 0;
                        bool isStartedfromZero = true;
                        for (int i = 0; i < itemFieldenum.fieldenum1.Count; i++)
                        {
                            var itemEnum = itemFieldenum.fieldenum1[i];
                            if (itemEnum != null)
                            {
                                fieldenum parAttrib = itemEnum;
                                if (i == 0 && parAttrib.key != null && Tools.GetByte(parAttrib.key) > 0)
                                    isStartedfromZero = false;
                                var d = new Define();
                                if (isStartedfromZero)
                                    d.Key = Tools.GetHex(counter, true);
                                else
                                    d.Key = parAttrib.key;
                                d.Name = GetUnique(Tools.MakeLegalUpperCaseIdentifier(parAttrib.value), defines,
                                    ref innerCounter);
                                d.Text = parAttrib.value;
                                defines.Add(d);
                                counter++;
                            }
                        }
                        if (defines.Count > 0)
                        {
                            DefineSet defineSet = new DefineSet
                            {
                                Type = zwDefineSetType.Full,
                                Define = new Collection<Define>(defines)
                            };
                            MergeDefineSet(ref defineSetList, defineSet, ref p, ref definesCounter);
                        }
                    }
                    ret.Param1.Add(p);
                }
                else
                    throw new ApplicationException("Invalid parameter");
            }
            if (sumBits < 8)
            {
                Param p = new Param
                {
                    Type = zwParamType.HEX,
                    Bits = (byte)(8 - sumBits),
                    Name = Tools.MakeLegalMixCaseIdentifier("Reserved"),
                    Text = "Reserved"
                };
                ret.Param1.Add(p);
            }
            else if (sumBits > 8)
            {
                throw new ApplicationException("Invalid STRUCT_BYTE parameter");
            }
            return ret;
        }

        private static string GetUnique(string name, IEnumerable<Define> defines, ref byte uniqueCounter)
        {
            if (defines.Any(item => item.Name == name))
            {
                uniqueCounter++;
                return Tools.FormatStr("{0}{1}", name, uniqueCounter);
            }
            return name;
        }

        private static Param UgradeParameterTypeMultiArray(param val, ref IList<Define> defines)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 8;
            if (val.encaptype == zwXmlEncapType.SPEC_DEV_REF)
            {
                ret.Type = zwParamType.SPEC_DEV_REF;
            }
            else
            {
                if (val.Items != null)
                {
                    byte innerCounter = 0;
                    foreach (var item in val.Items)
                    {
                        if (item is multi_array)
                        {
                            AddDefinesFromMultiArray(defines, item, ref innerCounter);
                        }
                    }
                }
            }
            return ret;
        }

        private static Param UgradeParameterTypeMarker(param val)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 8;
            ret.Type = zwParamType.MARKER;
            if (val.Items != null)
            {
                IList<byte> tmp = val.Items.OfType<@const>().Select(itemConst => Tools.GetByte(itemConst.flagmask)).ToList();
                ret.DefaultValue = tmp.ToArray();
                ret.Bits = (byte)(ret.DefaultValue.Length * 8);
            }

            return ret;
        }

        private static Param UgradeParameterTypeDWord(param val)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 32;
            return ret;
        }

        private static Param UgradeParameterTypeConst(param val, ref IList<Define> defines)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 8;
            if (val.Items != null)
            {
                byte innerCounter = 0;
                foreach (var item in val.Items)
                {
                    @const parAttrib = (@const)item;
                    defines.Add(new Define
                    {
                        Key = Tools.GetHex(Tools.GetByte(parAttrib.flagmask), true),
                        Name = GetUnique(Tools.MakeLegalUpperCaseIdentifier(parAttrib.flagname), defines,
                            ref innerCounter),
                        Text = parAttrib.flagname
                    });
                }
            }
            return ret;
        }

        private static Param UgradeParameterTypeByte(param val, ref IList<Define> defines)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 8;
            if (val.Items != null)
            {
                byte innerCounter = 0;
                foreach (var item in val.Items)
                {
                    var bf = item as bitflag;
                    var ma = item as multi_array;
                    if (bf != null)
                    {
                        AddDefinesFromBitflag(defines, bf, ref innerCounter);
                    }
                    else if (ma != null)
                    {
                        AddDefinesFromMultiArray(defines, ma, ref innerCounter);
                    }
                }
            }
            return ret;
        }

        private static void AddDefinesFromMultiArray(ICollection<Define> defines, object item, ref byte innerCounter)
        {
            multi_array parAttribMa = (multi_array)item;
            if (parAttribMa.Items != null)
            {
                Define d = new Define { Define1 = new Collection<Define>() };
                foreach (var it in parAttribMa.Items)
                {
                    var parAttrib = it as bitflag;
                    if (parAttrib != null)
                    {
                        d.Key = Tools.GetHex(Tools.GetByte(parAttrib.key), true);
                        d.Name = "define" + parAttrib.key;
                        d.Text = "define" + parAttrib.key;
                        d.Define1.Add(new Define
                        {
                            Key = Tools.GetHex(Tools.GetByte(parAttrib.flagmask), true),
                            Name = GetUnique(Tools.MakeLegalUpperCaseIdentifier(parAttrib.flagname), defines,
                                ref innerCounter),
                            Text = parAttrib.flagname
                        });
                    }
                }
                if (d.Define1.Count > 0)
                {
                    defines.Add(d);
                }
            }
        }

        private static Param UgradeParameterTypeBitmask(param val, IList<Param> cmdParams, IList<Param> vgParams,
            ref IList<Define> defines)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 8;
            ret.Type = zwParamType.BITMASK;
            if (val.Items != null)
            {
                byte innerCounter = 0;
                foreach (var item in val.Items)
                {
                    var bm = item as bitmask;
                    var bf = item as bitflag;
                    if (bm != null)
                    {
                        if (bm.len > 0)
                        {
                            ret.Size = bm.len;
                        }
                        else
                        {
                            ret.SizeReference = GetReference(val.name, cmdParams, vgParams, bm.paramoffs, Tools.GetByte(bm.lenmask));
                            // Specific case to use List of 16bits: BITMASK with lenmask = 0XFFFF
                            if (Tools.GetUShort(bm.lenmask) == 0xFFFF)
                            {
                                ret.Bits = 16;
                            }
                        }
                    }
                    else if (bf != null)
                    {
                        AddDefinesFromBitflag(defines, bf, ref innerCounter);
                    }
                }
            }
            return ret;
        }

        private static void AddDefinesFromBitflag(ICollection<Define> defines, object item, ref byte innerCounter)
        {
            bitflag parAttrib = (bitflag)item;
            defines.Add(new Define
            {
                Key = Tools.GetHex(Tools.GetByte(parAttrib.flagmask), true),
                Name = GetUnique(Tools.MakeLegalUpperCaseIdentifier(parAttrib.flagname), defines, ref innerCounter),
                Text = parAttrib.flagname
            });
        }

        private static Param UgradeParameterTypeBit24(param val)
        {
            Param ret = UpgradeParameterCommon(val);
            ret.Bits = 24;
            return ret;
        }

        private static Param UgradeParameterTypeArray(param val, IList<Param> cmdParams, IList<Param> vgParams)
        {
            Param ret = UpgradeParameterCommon(val);

            ret.Bits = 8;
            foreach (var item in val.Items)
            {
                var itemAttrib = item as arrayattrib;
                if (itemAttrib != null)
                {
                    if (itemAttrib.is_ascii)
                    {
                        ret.Type = zwParamType.CHAR;
                    }
                    ret.Size = Tools.GetByte(itemAttrib.len);
                }
            }
            return ret;
        }

        private Param UpgradeVariantGroup(CommandClass cmdClass, Command cmd, object variantGroup,
            IList<Param> cmdParams, ref IList<DefineSet> defineSetList, ref int definesCounter)
        {
            if (!(variantGroup is variant_group))
                return null;
            variant_group val = (variant_group)variantGroup;
            Param ret = new Param
            {
                Mode = ParamModes.VariantGroup,
                Type = zwParamType.HEX,
                Name = Tools.MakeLegalMixCaseIdentifier(val.name),
                Bits = 8,
                Comment = val.comment,
                SkipField = val.skipfield,
                SkipFieldSpecified = val.skipfield,
                SizeReference = GetReference(val.name, cmdParams, null, Tools.GetByte(val.paramOffs),
                    Tools.GetByte(val.sizemask)),
                SizeChange = val.sizechange
            };
            ret.SizeChangeSpecified = ret.SizeChange != 0;
            if (val.optionaloffs != null)
            {
                if (val.optionaloffs != null)
                    ret.OptionalReference = GetReference(val.name, cmdParams, null, Tools.GetByte(val.optionaloffs),
                        Tools.GetByte(val.optionalmask));
            }
            ret.Text = val.name;
            if (val.Items != null)
            {
                IList<Param> pars = new List<Param>();
                byte structCount = 0;
                foreach (var item in val.Items)
                {
                    Param p = UpgradeParam(cmdClass, cmd, item, cmdParams, pars, ref defineSetList, ref definesCounter,
                        ref structCount);
                    pars.Add(p);
                }
                if (pars.Count > 0)
                    ret.Param1 = new Collection<Param>(pars);
                if (val.moretofollowoffs != null)
                {
                    ret.MoreToFollowReference = val.name + ".";
                    ret.MoreToFollowReference += GetReference(val.name, cmdParams, pars,
                        Tools.GetByte(val.moretofollowoffs), Tools.GetByte(val.moretofollowmask));
                }
            }
            return ret;
        }

        public void DowngradeConvert(params object[] args)
        {
            ZXmlDefinition = new zw_classes { Items = new Collection<object>() };
            ZXmlDefinition.version = ZWaveDefinition.Version;
            if (args == null || args.Length == 0)
            {
                foreach (var item in ZWaveDefinition.BasicDevices)
                {
                    ZXmlDefinition.Items.Add(DowngradeBasicDevice(item));
                }
                foreach (var item in ZWaveDefinition.GenericDevices)
                {
                    ZXmlDefinition.Items.Add(DowngradeGenericDevice(item));
                }
                foreach (var item in ZWaveDefinition.CommandClasses)
                {
                    ZXmlDefinition.Items.Add(DowngradeCommandClass(item, null));
                }
            }
            else if (args.Length > 0 && args[0] is Command)
            {
                Command cmd = (Command)args[0];
                ZXmlDefinition.Items.Add(DowngradeCommandClass(cmd.Parent, cmd));
            }
        }

        private static bas_dev DowngradeBasicDevice(BasicDevice item)
        {
            bas_dev ret = new bas_dev
            {
                comment = string.IsNullOrEmpty(item.Comment) ? null : item.Comment,
                help = item.Text,
                key = item.Key,
                name = item.Name,
            };
            return ret;
        }

        private static gen_dev DowngradeGenericDevice(GenericDevice item)
        {
            gen_dev ret = new gen_dev
            {
                comment = string.IsNullOrEmpty(item.Comment) ? null : item.Comment,
                help = item.Text,
                key = item.Key,
                name = item.Name,
                spec_dev = new Collection<spec_dev>()
            };
            if (item.SpecificDevice != null)
                foreach (var spec in item.SpecificDevice)
                {
                    ret.spec_dev.Add(DowngradeSpecificDevice(spec));
                }
            return ret;
        }

        private static spec_dev DowngradeSpecificDevice(SpecificDevice item)
        {
            spec_dev ret = new spec_dev
            {
                comment = string.IsNullOrEmpty(item.Comment) ? null : item.Comment,
                help = item.Text,
                key = item.Key,
                name = item.Name
            };
            return ret;
        }

        public object DowngradeCommandClass(CommandClass item, Command cmdItem)
        {
            cmd_class ret = new cmd_class
            {
                cmd = new Collection<cmd>(),
                comment = string.IsNullOrEmpty(item.Comment) ? null : item.Comment,
                help = item.Text,
                key = item.Key,
                name = item.Name,
                version = item.Version.ToString(),
            };
            if (item.Command != null)
                foreach (var cmd in item.Command)
                {
                    if (cmdItem == null || cmdItem.Name == cmd.Name)
                        ret.cmd.Add(DowngradeCommand(cmd));
                }
            return ret;
        }

        private cmd DowngradeCommand(Command item)
        {
            cmd ret = new cmd
            {
                comment = string.IsNullOrEmpty(item.Comment) ? null : item.Comment,
                help = item.Text,
                key = item.Key,
                name = item.Name,
                support_mode = (zwXmlSupportModes)item.SupportMode
            };
            if (item.Param != null)
            {
                ret.Items = DowngradeParameters(item.Param);
            }
            if (item.Bits > 0 && item.Bits < 8)
            {
                ret.cmd_mask = Tools.GetHex(Tools.GetMaskFromBits(item.Bits, (byte)(8 - item.Bits)), true);
                if (ret.Items.Count > 0)
                {
                    ((param)ret.Items[0]).cmd_mask =
                        Tools.GetHex(Tools.GetMaskFromBits((byte)(8 - item.Bits), 0), true);
                }
            }
            return ret;
        }

        private Collection<object> DowngradeParameters(IEnumerable<Param> parameters)
        {
            Collection<object> items = new Collection<object>();
            byte order = 0;
            foreach (var param in parameters)
            {
                if (param.Param1 != null && param.Param1.Count > 0 && param.Mode == ParamModes.VariantGroup)
                {
                    variant_group vg = DowngradeVariantGroup(param);
                    vg.key = Tools.GetHex(order, true);
                    items.Add(vg);
                }
                else if (param.Param1 != null && param.Param1.Count > 0 && param.Mode == ParamModes.Property)
                {
                    param sbp = DowngradeParamStructByte(param);
                    sbp.key = Tools.GetHex(order, true);
                    items.Add(sbp);
                }
                else if (param.Type == zwParamType.BITMASK)
                {
                    param bm = DowngradeParamBitmask(param);
                    bm.key = Tools.GetHex(order, true);
                    items.Add(bm);
                }
                else
                {
                    param p = DowngradeParam(param);
                    p.key = Tools.GetHex(order, true);
                    items.Add(p);
                }
                order++;
            }
            return items;
        }


        private static param DowngradeParamBitmask(Param param)
        {
            param ret = DowngradeParamCommon(param);
            ret.type = zwXmlParamType.BITMASK;
            ret.Items = new Collection<object>();
            bitmask bm = new bitmask { key = Tools.GetHex(0, true) };
            if (param.SizeReference != null)
            {
                CalculateReference(param.SizeReference, param, out byte reference, out byte mask, out byte shifter);
                bm.paramoffs = reference;
                if (param.Bits < 16)
                {
                    bm.lenmask = Tools.GetHex(mask, true);
                }
                else
                {
                    // Specific case to use List of 16bits: BITMASK with lenmask = 0XFFFF
                    bm.lenmask = "0x" + Tools.GetHex(Tools.GetMaskFromBitsUShort(param.Bits, shifter));
                }
                bm.lenoffs = shifter;
            }
            else
            {
                bm.paramoffs = 255;
                bm.lenoffs = 0;
                bm.lenmask = Tools.GetHex(0, true);
                bm.len = param.Size;
            }
            ret.Items.Add(bm);

            if (param.Defines != null)
            {
                DefineSet ds = param.ParentCmd.Parent.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
                if (ds != null)
                {
                    byte order = 0;
                    foreach (var item in ds.Define)
                    {
                        bitflag bf = new bitflag
                        {
                            flagname = item.Text,
                            key = Tools.GetHex(order, true),
                            flagmask = item.Key
                        };
                        ret.Items.Add(bf);
                        order++;
                    }
                }
            }
            return ret;
        }

        private static void CalculateReference(string name, Param parameter, out byte reference, out byte mask,
            out byte shifter)
        {
            reference = 0xFF;
            mask = 0;
            shifter = 0;
            if (name != MsgLength && name != MsgMarker)
            {
            string[] tokens = name.Split('.');
            Param referenceParameter;
            if (parameter.ParentParam == null)
            {
                byte referenceLevel = 0;
                    referenceParameter =
                        parameter.ParentCmd.Param.FirstOrDefault(x => x.Name == tokens[referenceLevel]);
                if (referenceParameter != null)
                {
                    reference = (byte)parameter.ParentCmd.Param.IndexOf(referenceParameter);
                        mask = 255;
                        shifter = 0;
                    while (tokens.Length > referenceLevel + 1)
                    {
                        referenceLevel++;
                        if (referenceParameter != null)
                        {
                            if (referenceParameter.Mode == ParamModes.Property)
                            {
                                foreach (var item in referenceParameter.Param1)
                                {
                                    mask = Tools.GetMaskFromBits(item.Bits, shifter);
                                    if (item.Name == tokens[referenceLevel])
                                    {
                                        if (referenceParameter.Mode != ParamModes.Property)
                                        {
                                            reference = (byte)referenceParameter.Param1.IndexOf(item);
                                        }
                                        break;
                                    }
                                    shifter += item.Bits;
                                }
                            }
                            else
                            {
                                var tmp = referenceParameter.Param1.FirstOrDefault(x => x.Name == tokens[referenceLevel]);
                                if (tmp != null)
                                {
                                    reference = (byte)referenceParameter.Param1.IndexOf(tmp);
                                    referenceParameter = tmp;
                                }
                            }
                        }
                    }
                }
            }
            else if (parameter.ParentParam != null && tokens[0] == "Parent")
            {
                referenceParameter = parameter.ParentCmd.Param.FirstOrDefault(x => x.Name == tokens[1]);
                if (referenceParameter != null)
                {
                    reference = (byte)(parameter.ParentCmd.Param.IndexOf(referenceParameter) + 0x80);
                        mask = 255;
                        shifter = 0;
                    if (tokens.Length == 3)
                    {
                            foreach (var item in referenceParameter.Param1)
                            {
                                mask = Tools.GetMaskFromBits(item.Bits, shifter);
                                if (item.Name == tokens[2])
                                    break;
                                shifter += item.Bits;
                            }
                    }
                }
            }
            else
            {
                referenceParameter = parameter.ParentParam.Param1.FirstOrDefault(x => x.Name == tokens[0]);
                if (referenceParameter != null)
                {
                    reference = (byte)parameter.ParentParam.Param1.IndexOf(referenceParameter);
                        mask = 255;
                        shifter = 0;
                    if (tokens.Length == 2)
                    {
            foreach (var item in referenceParameter.Param1)
            {
                mask = Tools.GetMaskFromBits(item.Bits, shifter);
                                if (item.Name == tokens[1])
                    break;
                shifter += item.Bits;
            }
        }
                    }
                }
            }
        }

        private variant_group DowngradeVariantGroup(Param param)
        {
            variant_group ret = new variant_group();
            if (param.Param1 != null && param.Param1.Count > 0)
            {
                ret.Items = new Collection<object>();
                foreach (var item in DowngradeParameters(param.Param1))
                {
                    ret.Items.Add(item);
                }
            }
            ret.comment = string.IsNullOrEmpty(param.Comment) ? null : param.Comment;
            ret.skipfield = param.SkipField;
            ret.name = param.Text;
            ret.sizechange = param.SizeChange;
            if (param.SizeReference == null && param.Size == 0)
                param.SizeReference = MsgLength;
            CalculateReference(param.SizeReference, param, out byte reference, out byte mask, out byte shifter);
            ret.paramOffs = Tools.GetHex(reference, true);
            ret.sizemask = Tools.GetHex(mask, true);
            ret.sizeoffs = Tools.GetHex(shifter, true);
            if (param.OptionalReference != null)
            {
                CalculateReference(param.OptionalReference, param, out reference, out mask, out shifter);
                ret.optionaloffs = Tools.GetHex(reference, true);
                ret.optionalmask = Tools.GetHex(mask, true);
            }
            if (param.MoreToFollowReference != null)
            {
                CalculateReference(param.MoreToFollowReference, param, out reference, out mask, out shifter);
                ret.moretofollowoffs = Tools.GetHex(reference, true);
                ret.moretofollowmask = Tools.GetHex(mask, true);
            }
            return ret;
        }

        private static param DowngradeParam(Param param)
        {
            if (param.Bits == 0)
                throw new ApplicationException("param.Bits must be > 0");

            param currentParam = DowngradeParamCommon(param);
            if (param.Size > 1)
            {
                currentParam.type = zwXmlParamType.ARRAY;
                currentParam.Items = new Collection<object>();
                arrayattrib aa = new arrayattrib();
                switch (param.Type)
                {
                    case zwParamType.CHAR:
                        aa.is_ascii = true;
                        break;
                }
                aa.len = (param.Size * (param.Bits / 8)).ToString();
                aa.key = Tools.GetHex(0, true);
                currentParam.Items.Add(aa);
            }
            else if (param.SizeReference != null)
            {
                currentParam.type = zwXmlParamType.VARIANT;
                currentParam.Items = new Collection<object>();
                variant v = new variant();
                v.sizechange = param.SizeChange;
                switch (param.Type)
                {
                    case zwParamType.CHAR:
                        v.is_ascii = true;
                        break;
                }

                CalculateReference(param.SizeReference, param, out byte reference, out byte mask, out byte shifter);
                v.paramoffs = reference;
                v.sizemask = Tools.GetHex(mask, true);
                v.sizeoffs = shifter;
                currentParam.Items.Add(v);
            }
            else if (param.Type == zwParamType.MARKER)
            {
                currentParam.type = zwXmlParamType.MARKER;
                currentParam.Items = new Collection<object>();
                for (byte i = 0; i < param.DefaultValue.Length; i++)
                {
                    @const bf = new @const
                    {
                        flagname = param.Text,
                        key = Tools.GetHex(i, true),
                        flagmask = Tools.GetHex(param.DefaultValue[i], true)
                    };
                    currentParam.Items.Add(bf);
                }
            }
            else switch (param.Bits / 8)
                {
                    case 1:
                        if (param.Defines != null)
                        {
                            DefineSet ds = param.ParentCmd.Parent.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
                            if (ds != null)
                            {
                                bool isMultiarray = ds.Define.Aggregate(true, (current, defineItem) => current & defineItem.Define1 != null);

                                if (isMultiarray)
                                {
                                    //int refParam = param.Order > 0 ? param.Order - 1 : 0;
                                    int refParam = Math.Max((param.ParentParam?.Param1 ?? param.ParentCmd.Param).IndexOf(param) - 1, 0);
                                    currentParam.type = zwXmlParamType.MULTI_ARRAY;
                                    currentParam.Items = new Collection<object>();
                                    multi_array maref = new multi_array
                                    {
                                        Items = new Collection<object>
                                    {
                                        new paramdescloc
                                        {
                                            key = "0x00",
                                            param = refParam.ToString(),
                                            paramdesc = "255",
                                            paramstart = refParam.ToString()
                                        }
                                    }
                                    };
                                    currentParam.Items.Add(maref);
                                    foreach (var itemMa in ds.Define)
                                    {
                                        multi_array ma = new multi_array { Items = new Collection<object>() };
                                        foreach (var item in itemMa.Define1)
                                        {
                                            bitflag bf = new bitflag
                                            {
                                                flagname = item.Text,
                                                key = itemMa.Key,
                                                flagmask = item.Key
                                            };
                                            ma.Items.Add(bf);
                                        }
                                        currentParam.Items.Add(ma);
                                    }
                                }
                                else if (ds.Type == zwDefineSetType.Full)
                                {
                                    currentParam.type = zwXmlParamType.CONST;
                                    currentParam.Items = new Collection<object>();
                                    byte order = 0;
                                    foreach (var item in ds.Define)
                                    {
                                        @const bf = new @const
                                        {
                                            flagname = item.Text,
                                            key = Tools.GetHex(order, true),
                                            flagmask = item.Key
                                        };
                                        currentParam.Items.Add(bf);
                                        order++;
                                    }
                                }
                                else
                                {
                                    currentParam.type = zwXmlParamType.BYTE;
                                    currentParam.Items = new Collection<object>();
                                    byte order = 1;
                                    foreach (var item in ds.Define)
                                    {
                                        bitflag bf = new bitflag
                                        {
                                            flagname = item.Text,
                                            key = Tools.GetHex(order, true),
                                            flagmask = item.Key
                                        };
                                        currentParam.Items.Add(bf);
                                        order++;
                                    }
                                }
                            }
                        }

                        else
                        {
                            currentParam.type = zwXmlParamType.BYTE;
                            currentParam.Items = new Collection<object>();
                        }
                        break;
                    case 2:
                        {
                            currentParam.type = zwXmlParamType.WORD;
                            currentParam.Items = new Collection<object>();
                            if (param.Defines != null)
                            {
                                DefineSet ds = param.ParentCmd.Parent.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
                                if (ds != null)
                                {
                                    byte order = 0;
                                    foreach (var item in ds.Define)
                                    {
                                        bitflag bf = new bitflag
                                        {
                                            flagmask = item.Text,
                                            key = Tools.GetHex(order, true)
                                        };
                                        bf.flagmask = item.Key;
                                        currentParam.Items.Add(bf);
                                        order++;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        {
                            currentParam.type = zwXmlParamType.BIT_24;
                            currentParam.Items = new Collection<object>();
                            if (param.Defines != null)
                            {
                                DefineSet ds = param.ParentCmd.Parent.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
                                if (ds != null)
                                {
                                    byte order = 0;
                                    foreach (var item in ds.Define)
                                    {
                                        bitflag bf = new bitflag
                                        {
                                            flagmask = item.Text,
                                            key = Tools.GetHex(order, true)
                                        };
                                        bf.flagmask = item.Key;
                                        currentParam.Items.Add(bf);
                                        order++;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        {
                            currentParam.type = zwXmlParamType.DWORD;
                            currentParam.Items = new Collection<object>();
                            if (param.Defines != null)
                            {
                                DefineSet ds = param.ParentCmd.Parent.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
                                if (ds != null)
                                {
                                    byte order = 0;
                                    foreach (var item in ds.Define)
                                    {
                                        bitflag bf = new bitflag
                                        {
                                            flagmask = item.Text,
                                            key = Tools.GetHex(order, true)
                                        };
                                        bf.flagmask = item.Key;
                                        currentParam.Items.Add(bf);
                                        order++;
                                    }
                                }
                            }
                        }
                        break;
                }
            return currentParam;
        }

        private static param DowngradeParamStructByte(Param param)
        {
            param currentParam = DowngradeParamCommon(param);
            currentParam.name = param.Text;
            currentParam.Items = new Collection<object>();
            currentParam.type = zwXmlParamType.STRUCT_BYTE;

            byte sumBits = 0;
            byte order = 0;
            foreach (var item in param.Param1)
            {
                if (item.Bits == 1)
                {
                    var bf = DowngradeBitflag(item, sumBits);
                    bf.key = Tools.GetHex(order, true);
                    currentParam.Items.Add(bf);
                }
                else if (item.Defines == null)
                {
                    var bf = DowngradeBitfield(item, sumBits);
                    bf.key = Tools.GetHex(order, true);
                    currentParam.Items.Add(bf);
                }
                else
                {
                    var bf = DowngradeFieldenum(item, sumBits);
                    bf.key = Tools.GetHex(order, true);
                    currentParam.Items.Add(bf);
                }
                sumBits += item.Bits;
                order++;
            }

            return currentParam;
        }

        private static bitflag DowngradeBitflag(Param param, byte sumBits)
        {
            bitflag ret = new bitflag
            {
                flagname = param.Text,
                flagmask = Tools.GetHex(Tools.GetMaskFromBits(1, sumBits), true)
            };
            return ret;
        }

        private static bitfield DowngradeBitfield(Param param, byte sumBits)
        {
            bitfield ret = new bitfield
            {
                fieldname = param.Text,
                fieldmask = Tools.GetHex(Tools.GetMaskFromBits(param.Bits, sumBits), true),
                shifter = sumBits
            };
            return ret;
        }

        private static fieldenum DowngradeFieldenum(Param param, byte sumBits)
        {
            fieldenum ret = new fieldenum
            {
                fieldname = param.Text,
                fieldmask = Tools.GetHex(Tools.GetMaskFromBits(param.Bits, sumBits), true),
                shifter = sumBits,
                fieldenum1 = new Collection<fieldenum>()
            };
            DefineSet ds = param.ParentCmd.Parent.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
            if (ds != null)
            {
                bool isStartedfromZero = true;
                for (int i = 0; i < ds.Define.Count; i++)
                {
                    var item = ds.Define[i];
                    if (i == 0 && item.KeyId != 0)
                        isStartedfromZero = false;
                    fieldenum fe = new fieldenum { value = item.Text };
                    if (!isStartedfromZero)
                        fe.key = item.Key;
                    ret.fieldenum1.Add(fe);
                }
            }
            return ret;
        }


        private static param DowngradeParamCommon(Param param)
        {
            param ret = new param
            {
                comment = string.IsNullOrEmpty(param.Comment) ? null : param.Comment,
                skipfield = param.SkipField,
                name = param.Text
            };
            ToEncapType(param, ret);
            if (param.OptionalReference != null)
            {
                CalculateReference(param.OptionalReference, param, out byte reference, out byte mask, out byte shifter);
                ret.optionaloffs = Tools.GetHex(reference, true);
                ret.optionalmask = Tools.GetHex(mask, true);
            }
            return ret;
        }

        private readonly XmlSerializer _zwXmlOldSerializer = new XmlSerializer(typeof(zw_classes));

        private static string DefinitionToString(XmlSerializer serializer, object definition)
        {
            StringBuilder ret = new StringBuilder();
            XmlWriterSettings sett = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = Environment.NewLine
            };
            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add(string.Empty, string.Empty);
            XmlWriter writer = XmlWriter.Create(ret, sett);
            try
            {
                serializer.Serialize(writer, definition, names);
            }
            catch (InvalidOperationException)
            {
            }
            finally
            {
                writer.Close();
            }
            return ret.ToString();
        }

        public XmlDocument ZXmlDefinitionToDocument(string node)
        {
            XmlDocument ret = new XmlDocument();
            using (StringReader reader = new StringReader(DefinitionToString(_zwXmlOldSerializer, ZXmlDefinition)))
            {
                ret.Load(reader);
            }
            if (node != null)
            {
                XmlNode xn = ret.SelectSingleNode(node);
                ret.RemoveAll();
                if (xn != null)
                {
                    ret.AppendChild(xn);
                }
            }
            return ret;
        }

        public XmlDocument ZWaveDefinitionToDocument(string node)
        {
            XmlDocument ret = new XmlDocument();
            using (StringReader reader = new StringReader(DefinitionToString(_zwXmlNewSerializer, ZWaveTempDefinition)))
            {
                ret.Load(reader);
            }
            if (node != null)
            {
                XmlNode xn = ret.SelectSingleNode(node);
                ret.RemoveAll();
                if (xn != null)
                {
                    ret.AppendChild(xn);
                }
            }
            return ret;
        }
    }
}