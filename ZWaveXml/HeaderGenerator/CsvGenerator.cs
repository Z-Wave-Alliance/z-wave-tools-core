/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
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
    public class CsvGenerator : Generator
    {
        readonly string ctrl = "controlling";
        readonly string sprt = "supporting";
        public CsvGenerator(string version, IList<CommandClass> cCl)
        {
            Version = version;
            CommandClassList = cCl.OrderByDescending(x => x.Version).OrderBy(x => x.Text).ToList();
        }

        public override void Generate(string optionsChTemplateFile, string optionsDefaultFileName,
            bool keepLegacyExceptions, bool isZats)
        {
            using (FileStream fs = new FileStream(optionsDefaultFileName, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                var commandClassPrev = "";
                foreach (var cmdClass in CommandClassList)
                {
                    if (cmdClass.KeyId >= 0x20 && cmdClass.Name != commandClassPrev)
                    {
                        if (cmdClass.Command != null && cmdClass.Command.Count > 0)
                        {
                            sw.Write(cmdClass.Text.Replace("Command Class", "").Trim() + " Command Class");
                            sw.WriteLine(cmdClass.Command.OrderBy(x => x.KeyId).Select(x => $",{x.Text},{x.Name},{x.Key},{(x.SupportMode == zwSupportModes.TX ? sprt : ctrl)},{(x.SupportMode == zwSupportModes.RX ? sprt : ctrl)}").Aggregate((a, b) => $"{a}{Environment.NewLine}{b}"));
                        }
                        commandClassPrev = cmdClass.Name;
                    }
                }
                sw.Flush();
                sw.Close();
            }
        }
    }
}