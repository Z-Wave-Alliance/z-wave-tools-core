/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZWave.Xml.Application;

namespace ZWaveTests
{
    /// <summary>
    /// Define sets are not stored in the Z-Wave XML file - they are derived while the file
    /// is read, and their names become enum type names in the generated C headers. Deriving
    /// them from the order of the commands therefore renames public types whenever the file
    /// is laid out differently, which is what sorting the commands by Key once did.
    ///
    /// These tests pin the names to the content of a Command Class.
    /// </summary>
    [TestFixture]
    public class DefineSetNamingTests
    {
        private readonly IList<string> _tempFiles = new List<string>();

        [TearDown]
        public void TearDown()
        {
            foreach (string path in _tempFiles)
            {
                try
                {
                    File.Delete(path);
                }
                catch (IOException)
                {
                    // A leftover temporary file must not fail the test run.
                }
            }
            _tempFiles.Clear();
        }

        /// <summary>
        /// One Command Class holding two commands, each with a 'State' parameter that
        /// enumerates different values, so the two define sets compete for the same name.
        /// </summary>
        /// <param name="reversed">writes the commands in the opposite order</param>
        /// <returns>path of the written file</returns>
        private string WriteChimneyFanLike(bool reversed)
        {
            const string stateSet = @"    <cmd key=""0x01"" name=""FAN_STATE_SET"" help=""Fan State Set"">
      <param key=""0x00"" name=""State"" type=""CONST"">
        <const key=""0x00"" flagname=""Next State"" flagmask=""0x01"" />
      </param>
    </cmd>
";
            const string stateReport = @"    <cmd key=""0x02"" name=""FAN_STATE_REPORT"" help=""Fan State Report"">
      <param key=""0x00"" name=""State"" type=""CONST"">
        <const key=""0x00"" flagname=""Off"" flagmask=""0x00"" />
        <const key=""0x01"" flagname=""Boost"" flagmask=""0x01"" />
        <const key=""0x02"" flagname=""Exhaust"" flagmask=""0x02"" />
      </param>
    </cmd>
";
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"us-ascii\"?>\n");
            xml.Append("<zw_classes version=\"2.19.1\">\n");
            xml.Append("  <cmd_class key=\"0x2A\" version=\"1\" name=\"COMMAND_CLASS_FAN\" help=\"Command Class Fan\">\n");
            xml.Append(reversed ? stateReport + stateSet : stateSet + stateReport);
            xml.Append("  </cmd_class>\n");
            xml.Append("</zw_classes>\n");

            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
            File.WriteAllText(path, xml.ToString());
            _tempFiles.Add(path);
            return path;
        }

        /// <summary>
        /// Renders the define sets of a definition as 'name: define,define' lines, ordered,
        /// so two definitions can be compared as text.
        /// </summary>
        private static IList<string> DescribeDefineSets(ZWaveDefinition definition)
        {
            return definition.CommandClasses
                .Where(cmdClass => cmdClass.DefineSet != null)
                .SelectMany(cmdClass => cmdClass.DefineSet
                    .Select(defineSet => string.Format("{0}.{1}: {2}", cmdClass.Name, defineSet.Name,
                        string.Join(",", defineSet.Define.Select(define => define.Name + "=" + define.KeyId)))))
                .OrderBy(item => item, StringComparer.Ordinal)
                .ToList();
        }

        [Test]
        public void DefineSetNames_AreTheSame_WhenTheCommandsAreInTheOppositeOrder()
        {
            IList<string> straight = DescribeDefineSets(ZWaveDefinition.Load(WriteChimneyFanLike(false)));
            IList<string> reversed = DescribeDefineSets(ZWaveDefinition.Load(WriteChimneyFanLike(true)));

            Assert.That(reversed, Is.EqualTo(straight));
        }

        [Test]
        public void DefineSetNames_FollowTheContent_NotTheCommandOrder()
        {
            IList<string> ret = DescribeDefineSets(ZWaveDefinition.Load(WriteChimneyFanLike(false)));

            // Both sets are named after the 'State' parameter, so the one whose defines come
            // first keeps the plain name and the other is numbered.
            Assert.That(ret, Is.EqualTo(new[]
            {
                "COMMAND_CLASS_FAN.state1: NEXT_STATE=1",
                "COMMAND_CLASS_FAN.state: OFF=0,BOOST=1,EXHAUST=2"
            }));
        }

        /// <summary>
        /// One Command Class where two commands hold a 'Level' parameter that enumerates
        /// different values and a third holds a 'Level 1' parameter, whose name reaches the
        /// converter as 'level1' - the very name numbering the two 'level' sets produces.
        /// </summary>
        /// <param name="reversed">writes the commands in the opposite order</param>
        /// <returns>path of the written file</returns>
        private string WriteCollidingNames(bool reversed)
        {
            const string levelAlpha = @"    <cmd key=""0x01"" name=""COLLIDE_A"" help=""Collide A"">
      <param key=""0x00"" name=""Level"" type=""CONST"">
        <const key=""0x00"" flagname=""Alpha"" flagmask=""0x00"" />
      </param>
    </cmd>
";
            const string levelBravo = @"    <cmd key=""0x02"" name=""COLLIDE_B"" help=""Collide B"">
      <param key=""0x00"" name=""Level"" type=""CONST"">
        <const key=""0x00"" flagname=""Bravo"" flagmask=""0x00"" />
      </param>
    </cmd>
";
            const string levelOne = @"    <cmd key=""0x03"" name=""COLLIDE_C"" help=""Collide C"">
      <param key=""0x00"" name=""Level 1"" type=""CONST"">
        <const key=""0x00"" flagname=""Charlie"" flagmask=""0x00"" />
      </param>
    </cmd>
";
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"us-ascii\"?>\n");
            xml.Append("<zw_classes version=\"2.19.1\">\n");
            xml.Append("  <cmd_class key=\"0x2C\" version=\"1\" name=\"COMMAND_CLASS_COLLIDE\" help=\"Command Class Collide\">\n");
            xml.Append(reversed ? levelOne + levelBravo + levelAlpha : levelAlpha + levelBravo + levelOne);
            xml.Append("  </cmd_class>\n");
            xml.Append("</zw_classes>\n");

            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
            File.WriteAllText(path, xml.ToString());
            _tempFiles.Add(path);
            return path;
        }

        [Test]
        public void DefineSetNames_StayApart_WhenANumberedNameIsAlsoAParameterName()
        {
            IList<string> names = ZWaveDefinition.Load(WriteCollidingNames(false)).CommandClasses
                .Single().DefineSet.Select(defineSet => defineSet.Name)
                .OrderBy(item => item, StringComparer.Ordinal).ToList();

            // Two sets are named after 'Level' and one after 'Level 1', which reaches the
            // converter as 'level1'. Numbering the 'level' sets on their own would hand the
            // second one that same name, leaving the header with the type declared twice.
            Assert.That(names, Is.EqualTo(new[] { "level", "level1", "level2" }));
        }

        [Test]
        public void DefineSetNames_KeepAParameterName_WhenNumberingCompetesForIt()
        {
            IDictionary<string, string> defines = ZWaveDefinition.Load(WriteCollidingNames(false))
                .CommandClasses.Single().Command
                .ToDictionary(cmd => cmd.Name, cmd => cmd.Param.Single().Defines, StringComparer.Ordinal);

            // The set of the 'Level 1' parameter keeps 'level1'; the numbered set moves on,
            // so no two parameters are left pointing at one and the same set.
            Assert.That(defines["COLLIDE_C"], Is.EqualTo("level1"));
            Assert.That(defines.Values.Distinct(StringComparer.Ordinal).Count(), Is.EqualTo(defines.Count));
        }

        [Test]
        public void CollidingDefineSetNames_AreTheSame_WhenTheCommandsAreInTheOppositeOrder()
        {
            IList<string> straight = DescribeDefineSets(ZWaveDefinition.Load(WriteCollidingNames(false)));
            IList<string> reversed = DescribeDefineSets(ZWaveDefinition.Load(WriteCollidingNames(true)));

            Assert.That(reversed, Is.EqualTo(straight));
        }

        [Test]
        public void DefineSetNumbering_IsPerName_NotPerCommandClass()
        {
            // 'Mode' collides once and 'State' collides once. Numbering them from a counter
            // shared by the whole Command Class yields a 'mode1' with no 'mode', or a
            // 'state2' with no 'state1', depending on which collision is seen first.
            const string xml = @"<?xml version=""1.0"" encoding=""us-ascii""?>
<zw_classes version=""2.19.1"">
  <cmd_class key=""0x2B"" version=""1"" name=""COMMAND_CLASS_MIXED"" help=""Command Class Mixed"">
    <cmd key=""0x01"" name=""MIXED_A"" help=""Mixed A"">
      <param key=""0x00"" name=""Mode"" type=""CONST"">
        <const key=""0x00"" flagname=""Alpha"" flagmask=""0x00"" />
      </param>
      <param key=""0x01"" name=""State"" type=""CONST"">
        <const key=""0x00"" flagname=""Bravo"" flagmask=""0x00"" />
      </param>
    </cmd>
    <cmd key=""0x02"" name=""MIXED_B"" help=""Mixed B"">
      <param key=""0x00"" name=""Mode"" type=""CONST"">
        <const key=""0x00"" flagname=""Charlie"" flagmask=""0x00"" />
      </param>
      <param key=""0x01"" name=""State"" type=""CONST"">
        <const key=""0x00"" flagname=""Delta"" flagmask=""0x00"" />
      </param>
    </cmd>
  </cmd_class>
</zw_classes>
";
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
            File.WriteAllText(path, xml);
            _tempFiles.Add(path);

            IList<string> names = ZWaveDefinition.Load(path).CommandClasses
                .Single().DefineSet.Select(defineSet => defineSet.Name)
                .OrderBy(item => item, StringComparer.Ordinal).ToList();

            Assert.That(names, Is.EqualTo(new[] { "mode", "mode1", "state", "state1" }));
        }
    }
}
