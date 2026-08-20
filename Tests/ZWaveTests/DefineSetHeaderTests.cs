/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ZWave.Xml.Application;
using ZWave.Xml.HeaderGenerator;

namespace ZWaveTests
{
    /// <summary>
    /// Nothing else covers CHeaderGeneratorEx, so these tests pin what a header publishes.
    ///
    /// A define set reaches a header from any parameter that names it. Only a plain byte
    /// field can carry the enum as its type; a bit field member, a multi byte field and a
    /// bit mask array cannot, yet they read the same defines. Publishing only the sets that
    /// happen to type a field would therefore drop constants the header exists to provide.
    /// </summary>
    [TestFixture]
    public class DefineSetHeaderTests
    {
        private string _xmlFile;
        private string _headerFolder;

        /// <summary>
        /// A Command Class whose three define sets are reached in three different ways: from
        /// a bit field member, from a bit mask array and from a plain byte field.
        /// </summary>
        private const string SampleXml = @"<?xml version=""1.0"" encoding=""us-ascii""?>
<zw_classes version=""2.19.1"">
  <cmd_class key=""0x2D"" version=""1"" name=""COMMAND_CLASS_SAMPLE"" help=""Command Class Sample"">
    <cmd key=""0x01"" name=""SAMPLE_REPORT"" help=""Sample Report"">
      <param key=""0x00"" name=""Level"" type=""STRUCT_BYTE"">
        <bitfield key=""0x00"" fieldname=""Hour"" fieldmask=""0x1F"" />
        <fieldenum key=""0x01"" fieldname=""Weekday"" fieldmask=""0xE0"" shifter=""5"">
          <fieldenum key=""0x01"" value=""Monday"" />
          <fieldenum key=""0x02"" value=""Tuesday"" />
        </fieldenum>
      </param>
      <param key=""0x01"" name=""Day Bitmask"" type=""BITMASK"">
        <bitmask key=""0x00"" paramoffs=""255"" lenmask=""0x00"" len=""2"" />
        <bitflag key=""0x00"" flagname=""Sunday"" flagmask=""0x00"" />
        <bitflag key=""0x01"" flagname=""Saturday"" flagmask=""0x01"" />
      </param>
      <param key=""0x02"" name=""Mode"" type=""CONST"">
        <const key=""0x00"" flagname=""Idle"" flagmask=""0x00"" />
        <const key=""0x01"" flagname=""Busy"" flagmask=""0x01"" />
      </param>
    </cmd>
  </cmd_class>
</zw_classes>
";

        [SetUp]
        public void SetUp()
        {
            _xmlFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
            File.WriteAllText(_xmlFile, SampleXml);
            _headerFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_headerFolder);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                File.Delete(_xmlFile);
                Directory.Delete(_headerFolder, true);
            }
            catch (IOException)
            {
                // Leftover temporary files must not fail the test run.
            }
        }

        /// <summary>
        /// Writes the headers of the sample Command Class.
        /// </summary>
        /// <returns>the definition the headers were written from</returns>
        private ZWaveDefinition Generate()
        {
            ZWaveDefinition definition = ZWaveDefinition.Load(_xmlFile);
            new CHeaderGeneratorEx(_headerFolder).Generate(null, null, definition.CommandClasses);
            return definition;
        }

        /// <summary>
        /// Reads back the header of the sample Command Class, leaving out the headers the
        /// generator writes beside it.
        /// </summary>
        /// <returns>lines of the Command Class header</returns>
        private IList<string> CommandClassLines()
        {
            return File.ReadAllLines(Path.Combine(_headerFolder, "COMMAND_CLASS_SAMPLE.h"));
        }

        /// <summary>
        /// Reads back every header the generator wrote, so a type declared in one of them
        /// counts as declared for the fields of another.
        /// </summary>
        /// <returns>lines of all headers</returns>
        private IList<string> AllLines()
        {
            return Directory.GetFiles(_headerFolder, "*.h").SelectMany(File.ReadAllLines).ToList();
        }

        [Test]
        public void EveryDefineSet_ThatAParameterNames_IsDeclared()
        {
            CommandClass cmdClass = Generate().CommandClasses.Single();
            IList<string> declared = CommandClassLines()
                .Where(line => line.StartsWith("typedef enum ", StringComparison.Ordinal))
                .ToList();

            // The three parameters of the sample reach their set in three different ways,
            // and each set is declared once - beside the enum holding the commands.
            Assert.That(cmdClass.DefineSet.Count, Is.EqualTo(3));
            Assert.That(declared.Count(line => !line.EndsWith("_COMMANDS_", StringComparison.Ordinal)),
                Is.EqualTo(cmdClass.DefineSet.Count));
        }

        [Test]
        public void DefineSet_OfABitFieldMember_IsDeclared()
        {
            Generate();
            IList<string> lines = CommandClassLines();

            // 'Weekday' is 3 bits wide, so its field is a bit field and C cannot type it
            // as the enum. Its constants are published all the same.
            Assert.That(lines, Has.Some.StartsWith("typedef enum _SAMPLE_WEEKDAY_"));
            Assert.That(lines, Has.Some.Contains("SAMPLE_WEEKDAY_MONDAY"));
            Assert.That(lines, Has.Some.Matches<string>(
                line => Regex.IsMatch(line, @"^\s*BYTE\s+weekday\s*:\s*3;")));
        }

        [Test]
        public void DefineSet_OfABitMaskArray_IsDeclared()
        {
            Generate();
            IList<string> lines = CommandClassLines();

            // A bit mask is an array of bytes, again a field the enum cannot type, and
            // again the constants are what callers need.
            Assert.That(lines, Has.Some.StartsWith("typedef enum _SAMPLE_DAYBITMASK_"));
            Assert.That(lines, Has.Some.Contains("SAMPLE_DAYBITMASK_SATURDAY"));
            Assert.That(lines, Has.Some.Matches<string>(
                line => Regex.IsMatch(line, @"^\s*BYTE\s+dayBitmask\[[0-9]+\];")));
        }

        [Test]
        public void NoStructField_UsesAnUndeclaredType()
        {
            Generate();
            IList<string> lines = AllLines();
            var declared = new HashSet<string>(StringComparer.Ordinal) { "BYTE" };
            foreach (Match match in lines.Select(line => Regex.Match(line, @"^\} +([A-Za-z0-9_]+);"))
                .Where(match => match.Success))
            {
                declared.Add(match.Groups[1].Value);
            }

            IList<string> used = lines
                .Select(line => Regex.Match(line, @"^ +([A-Za-z0-9_]+) +[A-Za-z0-9_]+ *(\[[0-9]+\])?(: *[0-9]+)?;"))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .Distinct(StringComparer.Ordinal)
                .ToList();

            Assert.That(used, Is.Not.Empty);
            Assert.That(used.Where(type => !declared.Contains(type)), Is.Empty);
        }
    }
}
