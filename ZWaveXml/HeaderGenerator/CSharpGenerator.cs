/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
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
    public class CSharpGenerator : Generator
    {
        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="opt">Reference to the application options</param>
        /// <param name="bDl">Reference to the basic device list</param>
        /// <param name="gDl">Reference to the generic device list</param>
        /// <param name="cCl">Reference to the command class list</param>
        public CSharpGenerator(string version, Options opt, IEnumerable<BasicDevice> bDl, IEnumerable<GenericDevice> gDl,
            IEnumerable<CommandClass> cCl)
        {
            Version = version;
            Options1 = opt;
            //bDL.Sort((x, y) => x.Name.CompareTo(y.Name));
            //gDL.Sort((x, y) => x.Name.CompareTo(y.Name));
            //csDL.Sort((x, y) => x.Name.CompareTo(y.Name));
            ////cCL.Sort((x, y) => x.Name.CompareTo(y.Name));
            BasicDeviceList = bDl;
            GenericDeviceList = gDl;
            CommandClassList = cCl;

            // exceptions
            foreach (var cc98 in CommandClassList.Where(x => x.KeyId == 0x98))
            {
                var c80 = cc98.Command.FirstOrDefault(x => x.KeyId == 0x80);
                var p = c80.Param.FirstOrDefault(x => x.Size > 1);
                if (p != null)
                {
                    p.Size = 1;
                    p.SizeReference = MsgLength;
                }
            }

            foreach (var cc9F in CommandClassList.Where(x => x.KeyId == 0x9F))
            {
                var c02 = cc9F.Command.FirstOrDefault(x => x.KeyId == 0x02);
                var p = c02.Param.FirstOrDefault(x => x.Size > 1);
                if (p != null)
                {
                    p.Size = 1;
                    p.SizeReference = MsgLength;
                }
            }
        }

        public override void Generate(string optionsChTemplateFile, string optionsDefaultFileName,
            bool keepLegacyExceptions, bool isZats)
        {
            string folder = Path.GetDirectoryName(optionsDefaultFileName);
            CreateByteSpecifiedStruct(folder, isZats);
            foreach (var cmdClass in CommandClassList)
            {
                if (!isZats && cmdClass.KeyId < 0x20)
                {
                    continue;
                }
                string name = cmdClass.Version > 1
                    ? Tools.FormatStr("{0}_V{1}", cmdClass.Name, cmdClass.Version)
                    : cmdClass.Name;
                using (FileStream fs = new FileStream(Path.Combine(folder ?? "", name + ".cs"), FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs) { NewLine = NEW_LINE };
                    sw.AddLicenseInfo();
                    if (cmdClass.Command != null && cmdClass.Command.Count > 0)
                    {
                        GenerateUsingSection(sw);
                    }
                    GenerateNamespaceBegin(sw, isZats);
                    GenerateCmdClassBegin(sw, name, cmdClass);
                    if (cmdClass.Command != null)
                        foreach (var cmd in cmdClass.Command)
                        {
                            GenerateCmdBegin(sw, cmd);
                            if (cmd.Param != null)
                            {
                                GenerateProperties(sw, cmd);
                            }
                            GenerateImplicitOperatorBegin(sw, cmd.Name);
                            if (cmd.Param != null)
                            {
                                GenerateImplicitOperator(sw, cmd);
                            }
                            GenerateImplicitOperatorEnd(sw);
                            GenerateReverseImplicitOperatorBegin(sw, name, cmd);
                            if (cmd.Param != null)
                            {
                                GenerateReverseImplicitOperator(sw, cmd);
                            }
                            GenerateReverseImplicitOperatorEnd(sw);
                            GenerateCmdEnd(sw);
                        }
                    GenerateCmdClassEnd(sw);
                    GenerateNamespaceEnd(sw);
                    sw.WriteLine("");
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private static void GenerateProperties(TextWriter sw, Command cmd)
        {
            foreach (var prm in cmd.Param)
            {
                if (prm.Param1 != null && prm.Param1.Count > 0 && (prm.Size > 1 || prm.SizeReference != null))
                {
                    GenerateVgProperties(sw, prm);
                }
                else if (prm.Param1 != null && prm.Param1.Count > 0 && prm.Bits == 8)
                {
                    GenerateStructProperty(sw, prm, 0);
                }
                else
                    GenerateSimpleProperty(sw, prm, 0);
            }
        }

        private static void GenerateVgProperties(TextWriter sw, Param vg)
        {
            GenerateVgBegin(sw, vg);
            foreach (var prm in vg.Param1)
            {
                if (prm.Param1 != null && prm.Param1.Count > 0 & prm.Bits == 8)
                {
                    GenerateStructProperty(sw, prm, 4);
                }
                else
                    GenerateSimpleProperty(sw, prm, 4);
            }
            GenerateVgEnd(sw);
            GenerateVg(sw, vg);
        }

        private static void GenerateImplicitOperator(TextWriter sw, Command cmd)
        {
            if (cmd.Name == "NODE_ADD_STATUS" && cmd.Parent.Version == 3)
            {
            }
            sw.WriteLine("                if (data != null)");
            sw.WriteLine("                {");
            if (cmd.Bits > 0 && cmd.Bits < 8)
            {
                sw.WriteLine("                    int index = 1;");
            }
            else
            {
                sw.WriteLine("                    int index = 2;");
            }
            foreach (var prm in cmd.Param)
            {
                if (prm.Param1 != null && prm.Param1.Count > 0 && (prm.Size > 1 || prm.SizeReference != null))
                {
                    GenerateVgImplicitOperator(sw, prm, "ret", GetTailSize(cmd), 0);
                }
                else if (prm.Param1 != null && prm.Param1.Count > 0 && prm.Bits == 8)
                {
                    GenerateStructImplicitOperator(sw, prm, "ret", 0);
                }
                else if (prm.SizeReference != null && prm.SizeReference == MsgLength)
                {
                    GenerateSimpleImplicitOperator(sw, prm, "ret", GetTailSize(cmd, prm), 0);
                }
                else
                {
                    GenerateSimpleImplicitOperator(sw, prm, "ret", 0, 0);
                }
            }
            sw.WriteLine("                }");
        }

        private static void GenerateReverseImplicitOperator(TextWriter sw, Command cmd)
        {
            bool isFirstStructInCmdMask = cmd.Bits > 0 && cmd.Bits < 8;
            foreach (var prm in cmd.Param)
            {
                if (prm.Param1 != null && prm.Param1.Count > 0 && (prm.Size > 1 || prm.SizeReference != null))
                {
                    GenerateVgReverseImplicitOperator(sw, prm);
                }
                else if (prm.Param1 != null && prm.Param1.Count > 0 && prm.Bits == 8)
                {
                    if (isFirstStructInCmdMask)
                    {
                        GenerateStructReverseImplicitOperatorCmdMask(sw, prm, "command", 0);
                    }
                    else
                    {
                        GenerateStructReverseImplicitOperator(sw, prm, "command", 0);
                    }
                }
                else
                    GenerateSimpleReverseImplicitOperator(sw, prm, "command", 0);

                isFirstStructInCmdMask = false;
            }
        }

        private static Param GetParamDefinition(Param item, string optRef)
        {
            Param ret = null;
            string[] tokens = optRef.Split('.');
            ICollection<Param> list = item.ParentParam != null ? item.ParentParam.Param1 : item.ParentCmd.Param;
            Param p = list.FirstOrDefault(x => x.Name == tokens[0]);
            if (p != null)
            {
                if (tokens.Length == 2 && p.Param1 != null)
                {
                    Param s = p.Param1.FirstOrDefault(x => x.Name == tokens[1]);
                    if (s != null)
                    {
                        ret = s;
                    }
                }
                else
                {
                    ret = p;
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

        private static string GetMarkerName(Param item)
        {
            string ret = null;
            if (item.ParentParam != null)
            {
                foreach (var p in item.ParentParam.Param1)
                {
                    if (p.Type == zwParamType.MARKER)
                    {
                        ret = p.Name;
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
                        ret = p.Name;
                        break;
                    }
                }
            }
            return ret;
        }

        private static void GenerateVgImplicitOperatorBegin(TextWriter sw, Param vg, byte tail, int indent)
        {
            Indent(sw, indent);
            sw.WriteLine("                    ret.{0} = new List<T{1}>();", vg.Name, vg.Name.ToUpper());
            if (vg.SizeReference != null && vg.SizeReference == MsgLength)
            {
                Indent(sw, indent);
                sw.WriteLine("                    while (data.Length - {0} > index)", tail + vg.SizeChange);
            }
            else if (vg.SizeReference != null && vg.SizeReference == MsgMarker)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < GetMarkerValue(vg).Length; i++)
                {
                    sb.Append(Tools.FormatStr(" && data[index + {0}] != {1}[{0}]", i, GetMarkerName(vg)));
                }
                Indent(sw, indent);
                sw.WriteLine("                    while (data.Length - {0} > index{1})", tail + vg.SizeChange, sb);
            }
            else if (vg.SizeReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("                    for (int j = 0; j < ret.{0}; j++)", vg.SizeReference);
            }
            else if (vg.Size > 1)
            {
                Indent(sw, indent);
                sw.WriteLine("                    for (int j = 0; j < {0}; j++)", vg.Size);
            }

            Indent(sw, indent);
            sw.WriteLine("                    {");
            Indent(sw, indent);
            sw.WriteLine("                        T{0} tmp = new T{0}();", vg.Name.ToUpper());
        }

        private static void GenerateVgImplicitOperator(TextWriter sw, Param vg, string pre, byte tail, int indent)
        {
            int indentOpt = 0;
            if (vg.OptionalReference != null)
            {
                indentOpt = 4;
                Indent(sw, indent);
                sw.WriteLine("                    if ({0}.{1} > 0)", pre, vg.OptionalReference);
                Indent(sw, indent);
                sw.WriteLine("                    {");
            }

            GenerateVgImplicitOperatorBegin(sw, vg, tail, indentOpt);
            foreach (var prm in vg.Param1)
            {
                if (prm.Param1 != null && prm.Param1.Count > 0 && prm.Bits == 8)
                {
                    GenerateStructImplicitOperator(sw, prm, "tmp", indentOpt + 4);
                }
                else
                    GenerateSimpleImplicitOperator(sw, prm, "tmp", GetTailSize(vg), indentOpt + 4);
            }
            GenerateVgImplicitOperatorEnd(sw, vg, indentOpt);

            if (vg.OptionalReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("                    }");
            }
        }

        private static void GenerateVgImplicitOperatorEnd(TextWriter sw, Param vg, int indent)
        {
            Indent(sw, indent);
            sw.WriteLine("                        ret.{0}.Add(tmp);", vg.Name);
            if (!string.IsNullOrEmpty(vg.MoreToFollowReference))
            {
                string reference = vg.MoreToFollowReference;
                reference = reference.Replace(vg.Name + ".", "tmp.");
                Indent(sw, indent);
                sw.WriteLine("                        if ({0} == 0)", reference);
                Indent(sw, indent);
                sw.WriteLine("                            break;");
            }
            Indent(sw, indent);
            sw.WriteLine("                    }");
        }

        private static void GenerateVgReverseImplicitOperatorBegin(TextWriter sw, Param vg)
        {
            sw.WriteLine("                if (command.{0} != null)", vg.Name);
            sw.WriteLine("                {");
            sw.WriteLine("                    foreach (var item in command.{0})", vg.Name);
            sw.WriteLine("                    {");
        }

        private static void GenerateVgReverseImplicitOperator(TextWriter sw, Param vg)
        {
            GenerateVgReverseImplicitOperatorBegin(sw, vg);
            foreach (var prm in vg.Param1)
            {
                if (prm.Param1 != null && prm.Param1.Count > 0 && prm.Bits == 8)
                {
                    GenerateStructReverseImplicitOperator(sw, prm, "item", 8);
                }
                else
                    GenerateSimpleReverseImplicitOperator(sw, prm, "item", 8);
            }
            GenerateVgReverseImplicitOperatorEnd(sw);
        }

        private static void GenerateVgReverseImplicitOperatorEnd(TextWriter sw)
        {
            sw.WriteLine("                    }");
            sw.WriteLine("                }");
        }

        private static void GenerateStructProperty(TextWriter sw, Param prm, int indent)
        {
            Indent(sw, indent);
            sw.WriteLine("            public struct T{0}", prm.Name);
            Indent(sw, indent);
            sw.WriteLine("            {");
            Indent(sw, indent);
            sw.WriteLine("                private byte _value;");
            Indent(sw, indent);
            sw.WriteLine("                public bool HasValue { get; private set; }");
            Indent(sw, indent);
            sw.WriteLine("                public static T{0} Empty {{ get {{ return new T{0}() {{ _value = 0, HasValue = false }}; }} }}", prm.Name);
            byte totalBits = 0;
            foreach (var item in prm.Param1)
            {
                Indent(sw, indent);
                sw.WriteLine("                public byte {0}", item.Name);
                Indent(sw, indent);
                sw.WriteLine("                {");
                Indent(sw, indent);
                sw.WriteLine("                    get {{ return (byte)(_value >> {0} & 0x{1:X2}); }}", totalBits,
                    Tools.GetMaskFromBits(item.Bits, 0));
                Indent(sw, indent);
                sw.WriteLine(
                    "                    set {{ HasValue = true; _value &= 0xFF - 0x{1:X2}; _value += (byte)(value << {0} & 0x{1:X2}); }}",
                    totalBits, Tools.GetMaskFromBits(item.Bits, totalBits));
                Indent(sw, indent);
                sw.WriteLine("                }");
                totalBits += item.Bits;
            }
            Indent(sw, indent);
            sw.WriteLine("                public static implicit operator T{0}(byte data)", prm.Name);
            Indent(sw, indent);
            sw.WriteLine("                {");
            Indent(sw, indent);
            sw.WriteLine("                    T{0} ret = new T{0}();", prm.Name);
            Indent(sw, indent);
            sw.WriteLine("                    ret._value = data;");
            Indent(sw, indent);
            sw.WriteLine("                    ret.HasValue = true;");
            Indent(sw, indent);
            sw.WriteLine("                    return ret;");
            Indent(sw, indent);
            sw.WriteLine("                }");
            Indent(sw, indent);
            sw.WriteLine("                public static implicit operator byte(T{0} prm)", prm.Name);
            Indent(sw, indent);
            sw.WriteLine("                {");
            Indent(sw, indent);
            sw.WriteLine("                    return prm._value;");
            Indent(sw, indent);
            sw.WriteLine("                }");
            Indent(sw, indent);
            sw.WriteLine("            }");
            Indent(sw, indent);
            sw.WriteLine("            public T{0} {0} = 0;", prm.Name);
        }

        private static void GenerateStructImplicitOperator(TextWriter sw, Param prm, string pre, int indent)
        {
            int indentOpt = 0;
            if (prm.OptionalReference != null)
            {
                indentOpt = 4;
                Indent(sw, indent);
                sw.WriteLine("                    if ({0}.{1} > 0)", pre, prm.OptionalReference);
                Indent(sw, indent);
                sw.WriteLine("                    {");
            }
            Indent(sw, indent + indentOpt);
            sw.WriteLine("                    {0}.{1} = data.Length > index ? ({2}T{1})data[index++] : {2}T{1}.Empty;", pre, prm.Name,
                prm.ParentParam == null ? string.Empty : "T" + prm.ParentParam.Name.ToUpper() + ".");
            if (prm.OptionalReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("                    }");
            }
        }

        private static void GenerateStructReverseImplicitOperatorCmdMask(TextWriter sw, Param prm, string pre,
            int indent)
        {
            Indent(sw, indent);
            sw.WriteLine("                ret.Add((byte)((ID & ID_MASK) + {0}.{1}));", pre, prm.Name);
        }

        private static void GenerateStructReverseImplicitOperator(TextWriter sw, Param prm, string pre, int indent)
        {
            int indentOpt = 0;
            if (prm.OptionalReference != null)
            {
                indentOpt = 4;
                Indent(sw, indent);
                sw.WriteLine("                if (command.{0} > 0)", prm.OptionalReference);
                Indent(sw, indent);
                sw.WriteLine("                {");
            }
            Indent(sw, indent + indentOpt);
            sw.WriteLine("                if ({0}.{1}.HasValue) ret.Add({0}.{1});", pre, prm.Name);
            if (prm.OptionalReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("                }");
            }
        }

        private static void GenerateSimpleProperty(TextWriter sw, Param prm, int indent)
        {
            int bytesCount = prm.Bits / 8;
            if (prm.Size > 1)
                bytesCount = bytesCount * prm.Size;

            if (prm.Type == zwParamType.MARKER)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < prm.DefaultValue.Length; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(Tools.FormatStr("0x{0}", prm.DefaultValue[i].ToString("X2")));
                }
                Indent(sw, indent);
                sw.WriteLine("            private byte[] {0} = {{{1}}};", prm.Name, sb);
            }

            else if (prm.SizeReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("            public IList<byte> {0} = new List<byte>();", prm.Name);
            }
            else
            {
                if (bytesCount == 1)
                {
                    Indent(sw, indent);
                    sw.WriteLine("            public ByteValue {0} = 0;", prm.Name);
                }
                else
                {
                    Indent(sw, indent);
                    sw.WriteLine("            public const byte {0}BytesCount = {1};", prm.Name, bytesCount);
                    Indent(sw, indent);
                    sw.WriteLine("            public byte[] {0} = new byte[{0}BytesCount];", prm.Name);
                }
            }
        }

        private static void GenerateSimpleImplicitOperator(TextWriter sw, Param prm, string pre, byte tail, int indent)
        {
            string type = prm.Bits > 8 ? "byte[]" : "byte";
            int indentOpt = 0;

            int bytesCount = prm.Bits / 8;
            if (prm.Size > 1)
                bytesCount = bytesCount * prm.Size;

            if (prm.OptionalReference != null)
            {
                var optPrm = GetParamDefinition(prm, prm.OptionalReference);
                string format = "{0}.{1} > 0";
                if (optPrm != null && optPrm.Bits / 8 > 1)
                {
                    format = "{0}.{1} != null && {0}.{1}.Length == {1}BytesCount";
                }
                indentOpt = 4;
                Indent(sw, indent);
                sw.WriteLine("                    if (" + format + ")", pre, prm.OptionalReference);
                Indent(sw, indent);
                sw.WriteLine("                    {");
            }

            if (prm.SizeReference != null && prm.SizeReference == MsgLength)
            {
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {0}.{1} = new List<{2}>();", pre, prm.Name, type);
                Indent(sw, indent + indentOpt);
                if (tail + prm.SizeChange == 0)
                {
                    sw.WriteLine("                    while (data.Length - {0} > index)", tail);
                }
                else
                {
                    sw.WriteLine("                    while (data.Length - {0} > index)", tail + prm.SizeChange);
                }
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                        if (data.Length > index) {0}.{1}.Add(data[index++]);",
                    pre, prm.Name);
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    }");
            }
            else if (prm.SizeReference != null && prm.SizeReference == MsgMarker)
            {
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {0}.{1} = new List<{2}>();", pre, prm.Name, type);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < GetMarkerValue(prm).Length; i++)
                {
                    sb.Append(Tools.FormatStr(" || data[index + {0}] != {1}.{2}[{0}]", i, pre, GetMarkerName(prm)));
                }
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    while (data.Length - {0} > index && (data.Length - {1} < index{2}))",
                    tail + prm.SizeChange, GetMarkerValue(prm).Length, sb);

                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                        if (data.Length > index) {0}.{1}.Add(data[index++]);",
                    pre, prm.Name);
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    }");
            }
            else if (prm.SizeReference != null)
            {
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {0}.{1} = new List<{2}>();", pre, prm.Name, type.Replace("[]", "")); // Leave it as bytes list for referenced types
                string strFrmt = GetStringFormatForSizeReferencedField(prm);
                if (prm.SizeReference.StartsWith("Parent."))
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    for (int i = 0; i < " + strFrmt + "; i++)", "ret",
                        prm.SizeReference.Substring(7), GetSignedDeltaToString(prm.SizeChange));
                }
                else
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    for (int i = 0; i < " + strFrmt + "; i++)", pre, prm.SizeReference,
                        GetSignedDeltaToString(prm.SizeChange));
                }
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                        if (data.Length > index) {0}.{1}.Add(data[index++]);",
                    pre, prm.Name);
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    }");
            }
            else
            {
                if (prm.Type == zwParamType.MARKER)
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    {0}.{1} = (data.Length - index) >= {2} ? new byte[{2}] : new byte[data.Length - index]; //Marker",
                        pre, prm.Name, prm.DefaultValue.Length);

                    for (int i = 0; i < prm.DefaultValue.Length; i++)
                    {
                        Indent(sw, indent + indentOpt);
                        sw.WriteLine("                    if (data.Length > index) {0}.{1}[{2}] = data[index++]; //Marker", pre, prm.Name, i);
                        //sw.WriteLine("                    index++; //Marker");
                    }
                }
                else if (bytesCount == 1)
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    {0}.{1} = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;",
                        pre, prm.Name);
                }
                else
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    {0}.{1} = (data.Length - index) >= {2}{1}BytesCount ? new byte[{2}{1}BytesCount] : new byte[data.Length - index];", pre, prm.Name,
                        prm.ParentParam != null ? "T" + prm.ParentParam.Name.ToUpper() + "." : string.Empty);
                    for (int i = 0; i < bytesCount - 1; i++)
                    {
                        Indent(sw, indent + indentOpt);
                        sw.WriteLine("                    if (data.Length > index) {0}.{1}[{2}] = data[index++];", pre, prm.Name, i);
                    }
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    if (data.Length > index) {0}.{1}[{2}] = data[index++];", pre, prm.Name, bytesCount - 1);
                }
            }
            if (prm.OptionalReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("                    }");
            }
        }

        private static string GetStringFormatForSizeReferencedField(Param prm)
        {
            var strFormatRet = "{0}.{1}{2}";
            Param parentParam = prm.ParentCmd.Param.FirstOrDefault(x => string.Compare(x.Name, prm.SizeReference, true) == 0);
            if (parentParam == null)
            {
                parentParam = prm.ParentParam?.Param1.FirstOrDefault(x => string.Compare(x.Name, prm.SizeReference, true) == 0);
            }
            if (parentParam != null && parentParam.Bits > 8)
            {

                switch (parentParam.Bits)
                {
                    case (16):
                        strFormatRet = "({0}.{1}[0] << 8) + {0}.{1}[1]{2}";
                        break;
                    case (24):
                        strFormatRet = "({0}.{1}[0] << 16) + ({0}.{1}[1] << 8) + {0}.{1}[2]{2}";
                        break;
                    case (32):
                        strFormatRet = "({0}.{1}[0] << 24) + ({0}.{1}[1] << 16) + ({0}.{1}[2] << 8) + {0}.{1}[3]{2}";
                        break;
                    default:
                        strFormatRet = "{0}.{1}{2}";
                        break;
                }
            }
            return strFormatRet;
        }

        private static string GetSignedDeltaToString(sbyte value)
        {
            if (value > 0)
                return string.Format(" + {0}", value);
            if (value < 0)
                return string.Format(" - {0}", 0 - value);
            return "";
        }

        private static void GenerateSimpleReverseImplicitOperator(TextWriter sw, Param prm, string pre, int indent)
        {
            int indentOpt = 0;
            if (prm.OptionalReference != null)
            {
                var optPrm = GetParamDefinition(prm, prm.OptionalReference);
                indentOpt = 4;
                string format = "command.{0} > 0";
                if (optPrm != null && optPrm.Bits / 8 > 1)
                {
                    format = "command.{0} != null && command.{0}.Length == {0}BytesCount";
                }
                Indent(sw, indent);
                sw.WriteLine("                if (" + format + ")", prm.OptionalReference);
                Indent(sw, indent);
                sw.WriteLine("                {");
            }
            if (prm.SizeReference != null || prm.Size > 1)
            {
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                if ({0}.{1} != null)", pre, prm.Name);
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                {");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    foreach (var tmp in {0}.{1})", pre, prm.Name);
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    {");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                        ret.Add(tmp);");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                    }");
                Indent(sw, indent + indentOpt);
                sw.WriteLine("                }");
            }
            else
            {
                if (prm.Type == zwParamType.MARKER)
                {
                    for (int i = 0; i < prm.DefaultValue.Length; i++)
                    {
                        Indent(sw, indent + indentOpt);
                        sw.WriteLine("                if ({0}.{1} != null && {0}.{1}.Length > {2}) ret.Add({0}.{1}[{2}]);", pre, prm.Name, i);
                    }
                }
                else if (prm.Bits > 8)
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                if ({0}.{1} != null)", pre, prm.Name);
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                {");
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    foreach (var tmp in {0}.{1})", pre, prm.Name);
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    {");
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                        ret.Add(tmp);");
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                    }");
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                }");
                }
                else
                {
                    Indent(sw, indent + indentOpt);
                    sw.WriteLine("                if ({0}.{1}.HasValue) ret.Add({0}.{1});", pre, prm.Name);
                }
            }
            if (prm.OptionalReference != null)
            {
                Indent(sw, indent);
                sw.WriteLine("                }");
            }
        }

        private static void GenerateVgBegin(TextWriter sw, Param prm)
        {
            sw.WriteLine("            public class T{0}", prm.Name.ToUpper());
            sw.WriteLine("            {");
        }

        private static void GenerateVg(TextWriter sw, Param prm)
        {
            sw.WriteLine("            public List<T{0}> {1} = new List<T{0}>();", prm.Name.ToUpper(), prm.Name);
        }

        private static void GenerateVgEnd(TextWriter sw)
        {
            sw.WriteLine("            }");
        }

        #region framing

        private static void GenerateUsingSection(TextWriter sw)
        {
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("");
        }

        private static void GenerateNamespaceBegin(TextWriter sw, bool isZats)
        {
            if (isZats)
            {
                sw.WriteLine("namespace Zats.Services");
            }
            else
            {
                sw.WriteLine("namespace ZWave.CommandClasses");
            }
            sw.WriteLine("{");
        }

        private static void GenerateCmdClassBegin(TextWriter sw, string name, CommandClass cmdClass)
        {
            sw.WriteLine("    public partial class {0}", name);
            sw.WriteLine("    {");
            sw.WriteLine("        public const byte ID = {0};", cmdClass.Key);
            sw.WriteLine("        public const byte VERSION = {0};", cmdClass.Version);
        }

        private static void GenerateCmdBegin(TextWriter sw, Command cmd)
        {
            sw.WriteLine("        public partial class {0}", cmd.Name);
            sw.WriteLine("        {");
            sw.WriteLine("            public const byte ID = {0};", cmd.Key);
            if (cmd.Bits > 0 && cmd.Bits < 8)
            {
                sw.WriteLine("            public const byte ID_MASK = 0x{0:X2};",
                    Tools.GetMaskFromBits(cmd.Bits, (byte)(8 - cmd.Bits)));
            }
        }

        private static void GenerateImplicitOperatorBegin(TextWriter sw, string cmd)
        {
            sw.WriteLine("            public static implicit operator {0}(byte[] data)", cmd);
            sw.WriteLine("            {");
            sw.WriteLine("                {0} ret = new {0}();", cmd);
        }

        private static void GenerateImplicitOperatorEnd(TextWriter sw)
        {
            sw.WriteLine("                return ret;");
            sw.WriteLine("            }");
        }

        private static void GenerateReverseImplicitOperatorBegin(TextWriter sw, string cmdClass, Command cmd)
        {
            sw.WriteLine("            public static implicit operator byte[]({0} command)", cmd.Name);
            sw.WriteLine("            {");
            sw.WriteLine("                List<byte> ret = new List<byte>();");
            sw.WriteLine("                ret.Add({0}.ID);", cmdClass);
            if (cmd.Bits > 0 && cmd.Bits < 8)
            {
            }
            else
            {
                sw.WriteLine("                ret.Add(ID);");
            }
        }

        private static void GenerateReverseImplicitOperatorEnd(TextWriter sw)
        {
            sw.WriteLine("                return ret.ToArray();");
            sw.WriteLine("            }");
        }

        private static void GenerateCmdEnd(TextWriter sw)
        {
            sw.WriteLine("        }");
        }

        private static void GenerateCmdClassEnd(TextWriter sw)
        {
            sw.WriteLine("    }");
        }

        private static void GenerateNamespaceEnd(TextWriter sw)
        {
            sw.WriteLine("}");
        }

        #endregion

        #region private

        private const string MsgLength = "MSG_LENGTH";
        private const string MsgMarker = "MSG_MARKER";

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

        private static byte GetTailSize(Command cmd, Param prm)
        {
            byte ret = 0;
            if (cmd.Param != null && cmd.Param.Count > 0)
            {
                for (int i = cmd.Param.Count - 1; i >= 0; i--)
                {
                    Param item = cmd.Param[i];
                    if (item == prm)
                    {
                        break;
                    }
                    if (item.SizeReference != null)
                    {
                        if (item.SizeReference != MsgLength && item.SizeReference != MsgMarker)
                        {
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (item.Param1 != null && item.Param1.Count > 0)
                    {
                        break;
                    }
                    else
                    {
                        ret += GetParamBits(item);
                    }
                }
            }
            return (byte)(ret / 8);
        }

        private static byte GetTailSize(Param prm)
        {
            byte ret = 0;
            if (prm.Param1 != null && prm.Param1.Count > 0)
            {
                for (int i = prm.Param1.Count - 1; i >= 0; i--)
                {
                    Param item = prm.Param1[i];
                    if (item.SizeReference != null || item.Param1 != null && item.Param1.Count > 0)
                        break;
                    ret += GetParamBits(item);
                }
            }
            return (byte)(ret / 8);
        }

        private static void Indent(TextWriter sw, int indent)
        {
            sw.Write(new string(' ', indent));
        }

        private void CreateByteSpecifiedStruct(string folder, bool isZats)
        {
            var expStrName = "ByteValue.cs";
            var structName = "ByteSpecifiedStruct.txt";
            var structText = string.Empty;
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(structName));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                structText = reader.ReadToEnd();
            }
            using (FileStream fs = new FileStream(Path.Combine(folder ?? "", expStrName), FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs) { NewLine = NEW_LINE };
                sw.AddLicenseInfo();
                sw.WriteLine("/// Generated file from the Z-Wave XML Editor.");
                GenerateNamespaceBegin(sw, isZats);

                sw.WriteLine(structText);

                GenerateNamespaceEnd(sw);
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
        }

        #endregion
    }
}
