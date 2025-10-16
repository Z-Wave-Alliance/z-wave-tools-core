/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;
using ZWave.Xml.Application;

namespace ZWave.Xml.HeaderGenerator
{
    /// <summary>
    /// C Header Generation class
    /// </summary>
    public class CHeaderGenerator : Generator
    {
        private const string LineComment = "// ";
        private int _zwFrameCollectionMacroCounter;

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="opt">Reference to the application options</param>
        /// <param name="bDl">Reference to the basic device list</param>
        /// <param name="gDl">Reference to the generic device list</param>
        /// <param name="cCl">Reference to the command class list</param>
        public CHeaderGenerator(string version, Options opt, IEnumerable<BasicDevice> bDl, IList<GenericDevice> gDl,
            IList<CommandClass> cCl)
        {
            Version = version;
            Options1 = opt;
            //bDL.Sort((x, y) => x.Name.CompareTo(y.Name));
            //gDL.Sort((x, y) => x.Name.CompareTo(y.Name));
            //csDL.Sort((x, y) => x.Name.CompareTo(y.Name));
            ////cCL.Sort((x, y) => x.Name.CompareTo(y.Name));
            BasicDeviceList = bDl;
            GenericDeviceList = gDl;
            var deprecatedList = new List<string>(new[]
                {
                    "COMMAND_CLASS_LOCK"
                });
            CommandClassList = cCl.Where(x => !deprecatedList.Contains(x.Name)).ToList();

            ControlFieldList = new ArrayList
            {
                "GENERATING_INFO",
                "GENERATING_VERSION",
                "FRAME_MACRO",
                "FRAME_MACRO_EX",
                "BASIC_DEVICE_DEF",
                "GEN_SPEC_DEVICE_DEF",
                "COMMAND_CLASS_DEF",
                "COMMAND_DEF",
                "COMMAND_STRUCTS"
            };
        }

        /// <summary>
        /// Call this method to perform the actual generation 
        /// </summary>
        public override void Generate(string optionsChTemplateFile, string optionsDefaultFileName,
            bool keepLegacyExceptions, bool isZats)
        {
            Generate(new StreamReader(optionsChTemplateFile), new StreamWriter(optionsDefaultFileName),
                keepLegacyExceptions);
        }

        /// <summary>
        /// Call this method to perform the actual generation 
        /// </summary>
        public void Generate(TextReader readerTemplateStream, TextWriter writerStream, bool keepLegacyExceptions)
        {
            _zwFrameCollectionMacroCounter = 0;
            Sr = readerTemplateStream;
            Sw = writerStream;
            try
            {
                string line;
                string controlField = null;
                while ((line = Sr.ReadLine()) != null)
                {
                    if (LineIsComment(line))
                    {
                    }
                    else if (LineHasControlField(ref controlField, line))
                    {
                        switch (controlField)
                        {
                            case "GENERATING_INFO":
                                Generate_Info();
                                break;
                            case "GENERATING_VERSION":
                                Generate_Version(Version);
                                break;
                            case "FRAME_MACRO_EX":
                                {
                                    for (int i = 0; i < _zwFrameCollectionMacroCounter; i++)
                                    {
                                        Sw.WriteLine("ZW_FRAME_COLLECTION_MACRO" + i);
                                    }
                                }
                                break;
                            case "FRAME_MACRO":
                                {
                                    IList<string> frameLines = Generate_Frame_Macro();

                                    for (int i = 0; i < frameLines.Count; i++)
                                    {
                                        if (i % 400 == 0)
                                        {
                                            Sw.WriteLine("");
                                            Sw.WriteLine(@"#define ZW_FRAME_COLLECTION_MACRO" +
                                                         _zwFrameCollectionMacroCounter + @"\");
                                            if (_zwFrameCollectionMacroCounter == 0)
                                                Sw.WriteLine(
                                                    @"   ZW_COMMON_FRAME                                       ZW_Common;\");

                                            _zwFrameCollectionMacroCounter++;
                                        }
                                        //if (((i + 1) % 400) == 0)
                                        //{
                                        //    sw.WriteLine(frameLines[i].Replace(@";\", ";"));
                                        //}
                                        //else
                                        //{
                                        Sw.WriteLine(frameLines[i]);
                                        //}
                                    }
                                }
                                break;
                            case "BASIC_DEVICE_DEF":
                                Generate_Defines(BasicDeviceList);
                                break;
                            case "GEN_SPEC_DEVICE_DEF":
                                Generate_SpecificDevice();
                                break;
                            case "COMMAND_CLASS_DEF":
                                Generate_Defines(CommandClassList);
                                break;
                            case "COMMAND_DEF":
                                Generate_Command_Def(keepLegacyExceptions);
                                break;
                            case "COMMAND_STRUCTS":
                                Generate_Command_Structs();
                                break;
                        }
                    }
                    else
                    {
                        Sw.WriteLine(line);
                    }
                }
                Sr.Close();
                Sw.Close();
            }
            catch (IOException)
            {
                try
                {
                    Sr.Close();
                }
                catch (IOException)
                {
                }
                try
                {
                    Sw.Close();
                }
                catch (IOException)
                {
                }
                throw;
            }
        }

        private void Generate_Version(string version)
        {
            Sw.WriteLine($" * @version {version}");
        }

        /// <summary>
        /// Generates current date time stamp comment
        /// </summary>
        private void Generate_Info()
        {
            Sw.WriteLine(LineComment + "This file is auto generated. Do not edit it manually!");
            Sw.WriteLine(LineComment + "Generated with: https://github.com/Z-Wave-Alliance/z-wave-xml-tools");
        }

        private void Generate_Defines(IEnumerable<BasicDevice> basicDeviceList)
        {
            if (basicDeviceList != null)
                foreach (BasicDevice entry in basicDeviceList)
                {
                    Generate_DefinesInternal(entry);
                }
        }

        private void Generate_Defines(IEnumerable<CommandClass> commandClassList)
        {
            foreach (CommandClass entry in commandClassList)
            {
                if (entry.KeyId == 0 || entry.KeyId >= 0x20)
                {
                    Generate_DefinesInternal(entry);
                }
            }
        }

        private void Generate_Defines(IList<Command> commandList)
        {
            if (commandList != null)
                foreach (Command entry in commandList)
                {
                    Generate_DefinesInternal(entry);
                }
        }

        private void Generate_DefinesInternal(object zwC)
        {
            int version = 1;
            var cmdClass = zwC as CommandClass;
            if (cmdClass != null)
            {
                version = cmdClass.Version;
            }
            var cmd = zwC as Command;
            if (cmd != null)
            {
                cmdClass = cmd.Parent;
                version = cmdClass.Version;
                //Sw.WriteLine(Generate_DefineLineStr(cmd.Name + "_SMODE", cmd.SupportMode.ToString() + "_SMODE", null, cmd.Parent.Version));
            }
            Generate_DefineLine(zwC, version);
        }

        /// <summary>
        /// Generates a #define line based on ZWClass data and a specified command class version
        /// </summary>
        /// <param name="zwC"></param>
        /// <param name="version">Command Class Version</param>
        private void Generate_DefineLine(object zwC, int version)
        {
            var specificDevice = zwC as SpecificDevice;
            var genericDevice = zwC as GenericDevice;
            var command = zwC as Command;
            var commandClass = zwC as CommandClass;
            var basicDevice = zwC as BasicDevice;

            if (specificDevice != null)
            {
                Generate_DefineLine(specificDevice.Name, specificDevice.KeyId, specificDevice.Comment, version);
            }
            else if (genericDevice != null)
            {
                Generate_DefineLine(genericDevice.Name, genericDevice.KeyId, genericDevice.Comment, version);
            }
            else if (command != null)
            {
                Generate_DefineLine(command.Name, command.KeyId, command.Comment, version);
            }
            else if (commandClass != null)
            {
                Generate_DefineLine(commandClass.Name, commandClass.KeyId, commandClass.Comment, version);
            }
            else if (basicDevice != null)
            {
                Generate_DefineLine(basicDevice.Name, basicDevice.KeyId, basicDevice.Comment, version);
            }
        }

        /// <summary>
        /// Generates a #define line based in the following format:
        /// </summary>
        /// <param name="name">Name of define</param>
        /// <param name="key">Value of define in hexadecimal format</param>
        /// <param name="comment">Optional comment</param>
        /// <param name="version">Version is used if the value is greater than 1</param>
        /// <returns>Resulting define line</returns>
        /// <code>
        /// #define name[_Vversion] key /* comment */
        /// </code>
        private static string Generate_DefineLineStr(string name, byte key, string comment, int version)
        {
            return Generate_DefineLineStr(name, new[] { key }, comment, version);
        }

        private static string Generate_DefineLineStr(string name, byte[] key, string comment, int version)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            if (key != null && key.Length > 0)
            {
                foreach (byte k in key)
                {
                    sb.Append(k.ToString("X2"));
                }
            }
            else
            {
                sb.Append(0x00.ToString("X2"));
            }
            return Generate_DefineLineStr(name, sb.ToString(), comment, version);
        }

        private static string Generate_DefineLineStr(string name, string strKey, string comment, int version)
        {
            string result;

            if (version > 1)
            {
                name += "_V" + version;
            }
            if (!string.IsNullOrEmpty(comment))
            {
                result = Tools.FormatStr("#define {0,-80} {1} /*{2}*/", name, strKey, comment);
            }
            else
            {
                result = Tools.FormatStr("#define {0,-80} {1}", name, strKey);
            }
            return result;
        }

        /// <summary>
        /// Writes a define line using the stream writer instance by calling Generate_DefineLineStr.
        /// For arguments documentation see Generate_DefineLineStr 
        /// </summary>
        private void Generate_DefineLine(string name, byte key, string comment, int version)
        {
            Sw.WriteLine(Generate_DefineLineStr(name, key, comment, version));
        }

        /// <summary>
        /// Generates defines liens for generic and specific types. One line for each define:
        /// </summary>
        private void Generate_SpecificDevice()
        {
            if (GenericDeviceList != null)
            {
                foreach (GenericDevice gDev in GenericDeviceList)
                {
                    //if (gDev.KeyId < 0xE0 || gDev.KeyId >= 0xFF)
                    {
                        Sw.WriteLine("/* Device class {0} */",
                            Tools.CutUpperUnderscoreToMixedUpperLower(gDev.Name, Options1.ChGenericDeviceNamePrefix,
                                " "));
                        Generate_DefineLine(gDev, 0);
                        if (CommonSpecificDeviceList != null)
                            foreach (SpecificDevice entrySpecific in CommonSpecificDeviceList)
                            {
                                Generate_DefineLine(entrySpecific, 0);
                            }
                        if (gDev.SpecificDevice != null)
                            foreach (SpecificDevice entrySpecific in gDev.SpecificDevice)
                            {
                                Generate_DefineLine(entrySpecific, 0);
                            }
                        Sw.WriteLine("");
                    }
                }
            }
        }

        /// <summary>
        /// For all commands insert a collection macro. First a comment giving the
        /// command class, then a line for each command.
        /// </summary>
        /// <example>
        /// <code>
        /// /* Command class Xxxx */\
        /// ZW_YYYY_FRAME      ZW_YyyyFrame;\
        /// </code>
        /// where YYYY is the command name.
        /// <br/>
        /// NOTE1: 
        /// If the command has a parameter of type VARIANT then the command name is 
        /// repeated 4 times as follows:
        /// <code>
        /// ZW_YYYY_1BYTE_FRAME      ZW_Yyyy1ByteFrame;\
        /// ZW_YYYY_2BYTE_FRAME      ZW_Yyyy2ByteFrame;\
        /// ZW_YYYY_3BYTE_FRAME      ZW_Yyyy3ByteFrame;\
        /// ZW_YYYY_4BYTE_FRAME      ZW_Yyyy4ByteFrame;\
        /// </code>
        /// NOTE2: 
        /// Code is generated if the command name does not end with the string 
        /// contained by EncapsulatedCommandIdentifier
        /// <br/>
        /// NOTE3:
        /// The command class version is postfixed to YYYY if the version
        /// value is greater than 1 as follows:
        /// <code>
        /// ZW_YYYY_VN_FRAME      ZW_YyyyVNFrame;\
        /// </code>
        /// where N in VN is the version number
        /// </example>
        private IList<string> Generate_Frame_Macro()
        {
            IList<string> codeLines = new List<string>();
            foreach (CommandClass cmdClass in CommandClassList)
            {
                bool generateFrameMacro = false;
                if (cmdClass.Command != null)
                    if (cmdClass.Command.Any(cmd => !cmd.Name.EndsWith(Options1.ChEncapsulatedCommandIdentifier)))
                    {
                        generateFrameMacro = true;
                    }

                if (generateFrameMacro && (cmdClass.KeyId == 0 || cmdClass.KeyId > 0x1F))
                {
                    string versionPostfix = "";
                    if (cmdClass.Version > 1) versionPostfix = "_V" + cmdClass.Version;
                    codeLines.Add(string.Format("/* Command class {0} */\\",
                        Tools.CutUpperUnderscoreToMixedUpperLower(cmdClass.Name + versionPostfix,
                            Options1.ChCommandClassPrefix, " ")));
                    //sw.WriteLine("/* Command class {0} */\\", Tools.CutUpperUnderscoreToMixedUpperLower(cmdClass.Name + versionPostfix, options.CHCommandClassPrefix, " "));
                    if (cmdClass.Command != null)
                        foreach (Command cmd in cmdClass.Command)
                        {
                            if (cmdClass.KeyId == 0x98 && cmd != null && cmd.KeyId == 0x80)
                            {
                                cmd.Param[0].Size = 8;
                                cmd.Param[0].SizeReference = null;
                            }
                            else if (cmdClass.KeyId == 0x9F && cmd != null && cmd.KeyId == 0x02)
                            {
                                cmd.Param[2].Size = 16;
                                cmd.Param[2].SizeReference = null;
                            }
                            // determine if the command has Param.ParamType.VARIANT or BITMASK
                            int variantCount = 1;
                            int variantGroupCount = 1;
                            byte vgCounter = 0;
                            bool hasVariantGroup = false;
                            if (cmd.Param != null)
                                foreach (Param param in cmd.Param)
                                {
                                    if (param.SkipField)
                                        continue;
                                    if (param.Param1 != null && param.Param1.Count > 0 &&
                                        (param.Size > 1 || param.SizeReference != null))
                                    {
                                        hasVariantGroup = true;
                                        if (param.Param1.Any(par => par.SizeReference != null))
                                        {
                                            variantGroupCount = 4;
                                        }
                                        variantCount = 4;
                                        break;
                                    }
                                    if (param.Type == zwParamType.BITMASK && param.Size != 1 ||
                                        param.SizeReference != null)
                                    {
                                        variantCount = 4;
                                        break;
                                    }
                                }

                            if (!cmd.Name.EndsWith(Options1.ChEncapsulatedCommandIdentifier))
                            {
                                if (hasVariantGroup)
                                {
                                    for (int byteCountVg = 1; byteCountVg <= variantGroupCount; byteCountVg++)
                                    {
                                        string byteStrVg = "";
                                        if (variantGroupCount > 1)
                                        {
                                            byteStrVg = "_" + byteCountVg + "BYTE";
                                        }
                                        string strVg = Tools.FormatStr("  {0,-60} {1};\\",
                                            string.Format("VG_{0}_VG" + (vgCounter == 0 ? "" : "_" + vgCounter),
                                                Tools.CutString(cmd.Name + byteStrVg + versionPostfix,
                                                    Options1.ChCommandClassPrefix)),
                                            string.Format("VG_{0}VGroup" + (vgCounter == 0 ? "" : "_" + vgCounter),
                                                Tools.UpperUnderscoreToMixedUpperLower(
                                                    cmd.Name + byteStrVg + versionPostfix, "")));
                                        codeLines.Add(strVg);
                                        //sw.WriteLine(strVG);
                                    }
                                    vgCounter++;
                                }

                                for (int byteCount = 1; byteCount <= variantCount; byteCount++)
                                {
                                    string byteStr = "";
                                    if (variantCount > 1)
                                    {
                                        byteStr = "_" + byteCount + "BYTE";
                                    }
                                    string str = Tools.FormatStr("  {0,-60} {1};\\",
                                        string.Format("ZW_{0}_FRAME",
                                            Tools.CutString(cmd.Name + byteStr + versionPostfix,
                                                Options1.ChCommandClassPrefix)),
                                        string.Format("ZW_{0}Frame",
                                            Tools.UpperUnderscoreToMixedUpperLower(cmd.Name + byteStr + versionPostfix,
                                                "")));
                                    codeLines.Add(str);
                                    //sw.WriteLine(str);
                                }
                            }
                        }
                }
            }
            return codeLines;
        }

        /// <summary>
        /// Generates Command Defines in the following format
        /// </summary>
        /// <code>
        /// /* xxxx command class commands */
        /// #define XXXX_VERSION   0xHH
        /// </code>
        /// <example>
        /// For param key of type STRUCT_uint8_t the following is generated:
        /// <code>
        /// /* Values used for xxxx command */
        /// #define YYYY_ZZZZ_MASK 0x3F
        /// #define YYYY_ZZZZ_BIT_MASK 0x80
        /// #define YYYY_ZZZZ_SHIFT 0x05
        /// </code>
        /// For param key of type ENUM the following is generated
        /// <code>
        /// #define YYYY_ZZZZ_WWWW VVVV
        /// </code>
        /// For param key of type CONST the following is generated
        /// <code>
        /// #define YYYY_FLAGNAME FLAGMASK
        /// </code>
        /// where,<br/>
        /// XXXX is the command class name.<br/>
        /// YYYY is the command name.<br/>
        /// ZZZZ is the is the field name  (ParamField.Name).<br/>
        /// _BIT is added if ParamField.fieldName equals "bitflag".<br/>
        /// WWWW is the name of the enum (name attribute).<br/>
        /// VVVV is the hexadecimal value of the key attribute.<br/>
        /// FLAGNAME is the value of the flagname attribute in the const tag.<br/>
        /// FLAGMASK is the value of the flagmask attribute in the const tag.<br/>
        /// </example>        
        private void Generate_Command_Def(bool keepLegacyExceptions)
        {
            //Sw.WriteLine(Generate_DefineLineStr("APP_SMODE", 0x00, "application specific", 1));
            //Sw.WriteLine(Generate_DefineLineStr("TX_SMODE", 0x01, "sending node: support; receiving node: control", 1));
            //Sw.WriteLine(Generate_DefineLineStr("RX_SMODE", 0x02, "sending node: control; receiving node: support", 1));
            //Sw.WriteLine(Generate_DefineLineStr("TX_RX_SMODE", 0x03, "sending node: support; receiving node: support", 1));
            //Sw.WriteLine();
            foreach (CommandClass cmdClass in CommandClassList)
            {
                if (cmdClass.KeyId == 0x59)
                {
                }
                if (cmdClass.KeyId == 0 || cmdClass.KeyId >= 0x20)
                {
                    Sw.WriteLine("/* {0} command class commands */",
                        Tools.CutUpperUnderscoreToMixedUpperLower(cmdClass.Name, Options1.ChCommandClassPrefix, " "));
                    Generate_DefineLine(
                        Tools.CutString(cmdClass.Name, Options1.ChCommandClassPrefix) +
                        Options1.ChCommandClassVersionPostfix, cmdClass.Version, "", cmdClass.Version);
                    Generate_Defines(cmdClass.Command);

                    if (!keepLegacyExceptions)
                    {
                        if (cmdClass.DefineSet != null && cmdClass.DefineSet.Count > 0)
                        {
                            Sw.WriteLine("/* Values used for {0} command class */",
                                Tools.UpperUnderscoreToMixedUpperLower(cmdClass.Name, " "));
                            foreach (DefineSet dSet in cmdClass.DefineSet)
                            {
                                bool isMultiarray = dSet.Define.Aggregate(true, (current, df) => current & df.Define1 != null);
                                if (isMultiarray)
                                {
                                    foreach (var groupItem in dSet.Define)
                                    {
                                        foreach (var item in groupItem.Define1)
                                        {
                                            Sw.WriteLine(Generate_DefineLineStr(
                                                cmdClass.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(dSet.Name) +
                                                "_" + Tools.MakeLegalUpperCaseIdentifier(item.Name), item.KeyId, "",
                                                cmdClass.Version));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var item in dSet.Define)
                                    {
                                        Sw.WriteLine(Generate_DefineLineStr(
                                            cmdClass.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(dSet.Name) + "_" +
                                            Tools.MakeLegalUpperCaseIdentifier(item.Name), item.KeyId, "",
                                            cmdClass.Version));
                                    }
                                }
                            }
                        }
                    }

                    if (cmdClass.Command != null)
                        foreach (Command cmd in cmdClass.Command)
                        {
                            //DCP_LIST_SET
                            ArrayList defineLines = new ArrayList();
                            if (cmd.Bits > 0 && cmd.Bits < 8)
                            {
                                defineLines.Add(Generate_DefineLineStr(cmd.Name + "_MASK",
                                    Tools.GetMaskFromBits(cmd.Bits, (byte)(8 - cmd.Bits)), "", cmdClass.Version));
                            }
                            if (cmd.Param != null)
                                foreach (Param param in cmd.Param)
                                {
                                    if (cmd.Bits > 0 && cmd.Bits < 8 && cmd.Param.IndexOf(param) == 0 && param.Param1 != null &&
                                        param.Param1.Count > 0)
                                    {
                                        defineLines.Add(Generate_DefineLineStr(
                                            cmd.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(param.Param1[0].Text) +
                                            Options1.ChMaskPostfix, Tools.GetMaskFromBits((byte)(8 - cmd.Bits), 0),
                                            param.Comment, cmdClass.Version));
                                    }
                                    else if (param.Type == zwParamType.MARKER)
                                    {
                                        defineLines.Add(Generate_DefineLineStr(
                                            cmd.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(param.Text),
                                            param.DefaultValue, param.Comment, cmdClass.Version));
                                    }
                                    else if (param.Param1 != null)
                                    {
                                        switch (param.Mode)
                                        {
                                            case ParamModes.Property:
                                                byte shifter = 0;
                                                foreach (var prm1 in param.Param1)
                                                {
                                                    string postfix = Options1.ChMaskPostfix;
                                                    if (prm1.Bits == 1)
                                                    {
                                                        postfix = Options1.ChBitPostfix + Options1.ChMaskPostfix;
                                                    }
                                                    defineLines.Add(Generate_DefineLineStr(
                                                        cmd.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(param.Text) +
                                                        "_" + Tools.MakeLegalUpperCaseIdentifier(prm1.Text) + postfix,
                                                        Tools.GetMaskFromBits(prm1.Bits, shifter), param.Comment,
                                                        cmdClass.Version));

                                                    // shift definitions
                                                    if (prm1.Bits > 1 && shifter != 0 && shifter != 8)
                                                    {
                                                        defineLines.Add(Generate_DefineLineStr(
                                                            cmd.Name + "_" +
                                                            Tools.MakeLegalUpperCaseIdentifier(param.Text) + "_" +
                                                            Tools.MakeLegalUpperCaseIdentifier(prm1.Text) +
                                                            Options1.ChShiftPostfix, shifter, "", cmdClass.Version));
                                                    }
                                                    shifter += prm1.Bits;
                                                    if (keepLegacyExceptions)
                                                    {
                                                        if (prm1.Defines != null)
                                                        {
                                                            FillDefines(cmdClass, cmd, defineLines, prm1);
                                                        }
                                                    }
                                                }
                                                break;
                                            case ParamModes.VariantGroup:
                                                foreach (var prm1 in param.Param1)
                                                {
                                                    if (keepLegacyExceptions)
                                                    {
                                                        if (prm1.Defines != null)
                                                        {
                                                            FillDefines(cmdClass, cmd, defineLines, prm1);
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    if (keepLegacyExceptions)
                                    {
                                        if (param.Defines != null)
                                        {
                                            FillDefines(cmdClass, cmd, defineLines, param);
                                        }
                                    }
                                }

                            if (defineLines.Count > 0)
                            {
                                Sw.WriteLine("/* Values used for {0} command */",
                                    Tools.UpperUnderscoreToMixedUpperLower(cmd.Name, " "));
                                foreach (string s in defineLines)
                                {
                                    Sw.WriteLine(s);
                                }
                            }
                        }
                    Sw.WriteLine("");
                }
            }
        }

        private static void FillDefines(CommandClass cmdClass, Command cmd, IList defineLines, Param param)
        {
            DefineSet ds = cmdClass.DefineSet.FirstOrDefault(x => x.Name == param.Defines);
            if (ds != null && param.Type != zwParamType.BITMASK)
            {
                bool isMultiarray = ds.Define.Aggregate(true, (current, df) => current & df.Define1 != null);
                if (isMultiarray)
                {
                    foreach (var groupItem in ds.Define)
                    {
                        foreach (var item in groupItem.Define1)
                        {
                            defineLines.Add(Generate_DefineLineStr(
                                cmd.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(item.Text), item.KeyId, "",
                                cmdClass.Version));
                        }
                    }
                }
                else
                {
                    if (param.ParentParam != null && param.ParentParam.Mode == ParamModes.Property)
                    {
                        foreach (var item in ds.Define)
                        {
                            defineLines.Add(Generate_DefineLineStr(
                                cmd.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(param.Text) + "_" +
                                Tools.MakeLegalUpperCaseIdentifier(item.Text), item.KeyId, "", cmdClass.Version));
                        }
                    }
                    else if (ds.Type == zwDefineSetType.Full)
                    {
                        foreach (var item in ds.Define)
                        {
                            defineLines.Add(Generate_DefineLineStr(
                                cmd.Name + "_" + Tools.MakeLegalUpperCaseIdentifier(item.Text), item.KeyId, "",
                                cmdClass.Version));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates command class structs in the following format:
        /// </summary>
        /// <example>
        /// <code>
        /// /************************************************************/
        /// /* xxxx command class structs                               */
        /// /************************************************************/
        /// typedef struct _ZW_XXXX_FRAME_
        /// {
        ///    uint8_t        cmdClass;  /* The command class */
        ///    uint8_t        cmd;       /* The command */
        ///    uint8_t        y1;        /* comment */
        ///    uint8_t        y2;        /* comment */
        ///    uint8_t        ...        /* comment */
        ///    uint8_t        yn;        /* comment */
        /// } ZW_XXXX_FRAME;
        /// </code>
        /// Where xxxx is the comand name (excl. prefix) in Mixed upper lower case.,<br/>
        /// and XXXX is the command name (excl. prefix).,<br/>
        /// and y1..yn are the parameter names.<br/>
        /// <br/>
        /// NOTE 1: Code is generated if the command name does not end with the string 
        /// contained by EncapsulatedCommandIdentifier
        /// <br/>
        /// NOTE 2: M=4 frame structures are generated for commands that contains parameters of type VARIANT.
        /// _Nuint8_t is appended to XXXX where N goes from 1..M. The uint8_t field declaration is repeated N times in
        /// each structure.
        /// <br/>
        /// NOTE 3: For parameters of type BYTE, WORD, BIT_24 and DWORD the uint8_t field declaration is repeated
        /// 1, 2,3 and 4 times respectively. An index value is appendd to the field name. 
        /// <br/>
        /// NOTE 4: For parameters of type ARRAY. Len attribute of arrayattrib tag is used to specify the number of
        /// uint8_t fields
        /// <br/>
        /// NOTE 5: For parameters of type STRUCT_BYTE. Field names are concatenated and one uint8_t field declaration is 
        /// generated.
        /// </example>
        private void Generate_Command_Structs()
        {
            foreach (CommandClass commandClassEntry in CommandClassList)
            {
                if (commandClassEntry.KeyId == 0 || commandClassEntry.KeyId >= 0x20)
                {
                    if (commandClassEntry.Command != null)
                        foreach (Command commandEntry in commandClassEntry.Command)
                        {
                            Structs(commandClassEntry.Version, commandEntry.Name, false, commandEntry.Bits, commandEntry.Param);
                        }
                }
            }
        }

        private string _lastVgStructPostfix;
        const string structPrefixVg = "VG";
        const string structPostfixVg = "VG";
        private void Structs(byte cmdClassVersion, string cmdName, bool isVG, byte parentBits, IList<Param> parameters, int currentCC_VG_ID = 0)
        {
            _lastVgStructPostfix = isVG ? _lastVgStructPostfix : "";
            string structPrefix = isVG ? structPrefixVg : "ZW";
            string structPostfix = isVG ?
                (_lastVgStructPostfix ?? "") + structPostfixVg + (currentCC_VG_ID == 0 ? "" : "_" + currentCC_VG_ID)
                : "FRAME";

            const string structFieldFormat = "{0}{1,-10}{2,-30}{3}";
            if (cmdName == "CRC_16_ENCAP"
                || !cmdName.EndsWith(Options1.ChEncapsulatedCommandIdentifier))
            {
                // determine if the command has Param.ParamType.VARIANT or Param.ParamType.BITMASK

                #region Determine count params and groups

                int structCount = 1;
                var list = new SortedList<string, Param>();
                list.Add("", null);
                byte vgParamCounter = 0;

                if (parameters != null)
                    foreach (Param param in parameters)
                    {
                        if (param.SkipField)
                            continue;
                        if (param.Param1 != null && param.Param1.Count > 0 &&
                            (param.Size > 1 || param.SizeReference != null))
                        {
                            structCount = 4;
                            if (isVG)
                            {
                                _lastVgStructPostfix = structPostfix + "_";
                            }
                            else
                            {
                                _lastVgStructPostfix = "";
                            }
                            Structs(cmdClassVersion, cmdName, true, param.Bits, param.Param1, vgParamCounter);
                            param.VG_Id = vgParamCounter;
                            vgParamCounter++;
                        }
                        else
                        {
                            if (param.Bits >= 8)
                            {
                                if (!list.ContainsKey(param.Name))
                                    list.Add(param.Name, param);
                                else
                                    throw new ApplicationException($"Command {cmdName} already has parameter {param.Name}");
                            }
                            if (param.Type == zwParamType.BITMASK && param.Size != 1 ||
                                param.SizeReference != null)
                            {
                                if (structCount == 4)
                                {
                                    SystemLogSingleton.Instance.DoAddLogLine(
                                        SystemLogSingleton.Category.Warning,
                                        SystemLogSingleton.Action.CodeGeneration,
                                        $"Multiple VARIANT/BITMASK found in command {cmdName}"
                                    );
                                    break;
                                }
                                structCount = 4;
                            }
                        }
                    }

                #endregion

                string versionPostfix = cmdClassVersion > 1 ? "_V" + cmdClassVersion : "";
                for (int structIdx = 1; structIdx <= structCount; structIdx++)
                {
                    string byteStr = "";
                    if (structCount > 1)
                    {
                        byteStr = "_" + structIdx + "BYTE";
                    }

                    Sw.WriteLine("/{0}/", DuplicateStr("*", 60));
                    if (!isVG)
                    {
                        Sw.WriteLine("{0,-60}", GetTypeStructCaptionCmd(cmdName, byteStr, versionPostfix));
                    }
                    else
                    {
                        Sw.WriteLine("{0,-60}", GetTypeStructCaptionVg(cmdName, byteStr, versionPostfix));
                    }
                    Sw.WriteLine("/{0}/", DuplicateStr("*", 60));
                    Sw.WriteLine("typedef struct _{0}_{1}_{2}_", structPrefix,
                        GetTypeStructNameCmd(cmdName, byteStr, versionPostfix), structPostfix);
                    Sw.WriteLine("{");
                    string tab1 = MakeTabs(1);
                    if (!isVG)
                    {
                        Sw.WriteLine(structFieldFormat, tab1, "uint8_t", "cmdClass;", "/* The command class */");
                        string cmdDefName = "cmd;";
                        string cmdComment = "/* The command */";
                        if (parentBits > 0 && parentBits < 8)
                        {
                            if (parameters != null && parameters.Count > 0)
                            {
                                cmdDefName = "cmd_" + Tools.MakeLegalMixCaseIdentifier(parameters[0].Param1[0].Text) +
                                          ";";
                                cmdComment = "/* The command + parameter " + parameters[0].Param1[0].Text + " */";
                            }
                        }
                        Sw.WriteLine(structFieldFormat, tab1, "uint8_t", cmdDefName, cmdComment);
                    }

                    #region foreach params

                    //parameters = cmd != null ? cmd.Param : vgParam.Param1;
                    if (parameters != null)
                        foreach (Param param in parameters)
                        {
                            if (cmdName.StartsWith("SECURITY_MESSAGE_ENCAPSULATION") &&
                                param.Name == "commandClassIdentifier")
                                continue;
                            if (cmdName.StartsWith("SECURITY_MESSAGE_ENCAPSULATION") &&
                                param.Name == "commandIdentifier")
                                continue;
                            if (param.SkipField)
                                continue;
                            if (parentBits > 0 && parentBits < 8 && parameters.IndexOf(param) == 0 &&
                                param.Param1 != null && param.Param1.Count > 0)
                            {
                            }
                            else if (param.Param1 != null && param.Param1.Count > 0 &&
                                     (param.Size > 1 || param.SizeReference != null))
                            {
                                for (int i = 1; i <= structIdx; i++)
                                {
                                    var vgByteStr = "";
                                    if (param.Param1.Any(x => x.Size > 1 || x.SizeReference != null))
                                    {
                                        vgByteStr = "_4BYTE";
                                    }

                                    string vg_id = param.VG_Id == 0 ? "" : "_" + param.VG_Id;
                                    string lineStr = structFieldFormat.FormatStr(
                                        tab1,
                                        "{0}_{1}_{2} ".FormatStr(
                                            structPrefixVg,
                                            GetTypeStructNameCmd(cmdName, vgByteStr, versionPostfix),
                                            vgByteStr == "" ? (_lastVgStructPostfix ?? "") + structPrefixVg + vg_id :
                                                                structPrefixVg + vg_id),
                                            Tools.MakeLegalMixCaseIdentifier("variantGroup" + i) + vg_id + ";",
                                        "/*" + param.Comment + "*/");
                                    Sw.WriteLine(lineStr);
                                }
                            }
                            else
                            {
                                string comment = "/*" + param.Comment + "*/";
                                if (param.Param1 != null && param.Param1.Count > 0 && param.Bits > 1)
                                {
                                    comment = "/* masked byte */";
                                }
                                int byteFieldCount = 0;
                                if (param.Type == zwParamType.BITMASK && param.Size != 1 ||
                                    param.SizeReference != null)
                                {
                                    byteFieldCount = structIdx;
                                }

                                else if (param.Bits % 8 == 0)
                                {
                                    if (param.Bits == 8 && param.Size <= 1)
                                    {
                                        byteFieldCount = 0;
                                        Sw.WriteLine(structFieldFormat, tab1, "uint8_t",
                                            Tools.MakeLegalMixCaseIdentifier(param.Text) + ";", comment);
                                    }
                                    else
                                    {
                                        byteFieldCount = param.Size <= 1
                                            ? param.Bits / 8
                                            : param.Size * (param.Bits / 8);
                                    }
                                }
                                //if (byteFieldCount > 0 && param.Size > 1 &&
                                //    !((param.Type == zwParamType.BITMASK && param.Size != 1) || param.SizeReference != null))
                                //{
                                //    sw.WriteLine(StructFieldFormat, tab1, "uint8_t", Tools.MakeLegalMixCaseIdentifier(param.Text) + "[" + byteFieldCount.ToString() + "];", comment);
                                //}
                                //else
                                //{
                                if (cmdName.StartsWith("SECURITY_MESSAGE_ENCAPSULATION") &&
                                    param.Name == "commandByte")
                                {
                                    Sw.WriteLine(structFieldFormat, tab1, "uint8_t", "commandClassIdentifier" + ";",
                                        "/**/");
                                    Sw.WriteLine(structFieldFormat, tab1, "uint8_t", "commandIdentifier" + ";",
                                        "/**/");
                                }
                                for (int i = 1; i <= byteFieldCount; i++)
                                {
                                    if (i == 1 && byteFieldCount > 1)
                                    {
                                        comment = "/* MSB */";
                                    }
                                    else if (i == byteFieldCount && byteFieldCount > 1)
                                    {
                                        comment = "/* LSB */";
                                    }
                                    else
                                    {
                                        comment = "";
                                    }
                                    if (param.Text.StartsWith("Receiver’s Entropy Input"))
                                    {
                                    }
                                    Sw.WriteLine(structFieldFormat, tab1, "uint8_t",
                                        Tools.MakeLegalMixCaseIdentifier(param.Text) + i + ";", comment);
                                }
                                //}
                            }
                        }

                    #endregion

                    Sw.Write("}");
                    Sw.WriteLine(" {0}_{1}_{2};", structPrefix, GetTypeStructNameCmd(cmdName, byteStr, versionPostfix), structPostfix);
                    Sw.WriteLine("");
                }
            }
        }
    } // CHGenerator
} //ZensysXMLEditor namespace