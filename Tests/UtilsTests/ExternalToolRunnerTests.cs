// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.IO;
using NUnit.Framework;
using Utils;

namespace UtilsTests
{
    /// <summary>
    /// Runs the Windows command interpreter and PowerShell as stand-ins for a third party tool, so the tests
    /// need no tool installation. They are skipped on other operating systems.
    /// </summary>
    [TestFixture]
    public class ExternalToolRunnerTests
    {
        private static readonly string CMD_PATH = Path.Combine(Environment.SystemDirectory, "cmd.exe");
        private static readonly string POWERSHELL_PATH =
            Path.Combine(Environment.SystemDirectory, "WindowsPowerShell", "v1.0", "powershell.exe");

        private readonly IExternalToolRunner _runner = new ExternalToolRunner();

        [SetUp]
        public void RequireWindows()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                Assert.Ignore("The tests run cmd.exe and powershell.exe");
            }
        }

        [Test]
        public void Run_ReturnsExitCodeAndOutput()
        {
            int exitCode = _runner.Run(CMD_PATH, "/c echo hello", 10000, out string output);

            Assert.AreEqual(0, exitCode);
            Assert.AreEqual("hello", output.Trim());
        }

        [Test]
        public void Run_ToolFails_ReturnsItsExitCode()
        {
            int exitCode = _runner.Run(CMD_PATH, "/c exit 3", 10000, out string output);

            Assert.AreEqual(3, exitCode);
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void Run_Timeout_KillsTheToolAndReturnsMinusOne()
        {
            int exitCode = _runner.Run(POWERSHELL_PATH, "-NoProfile -Command Start-Sleep -Seconds 30", 500, out string output);

            Assert.AreEqual(-1, exitCode);
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void Run_MissingTool_ReturnsMinusOne()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".exe");

            Assert.AreEqual(-1, _runner.Run(missingPath, "", 1000, out string output), "missing file");
            Assert.AreEqual(string.Empty, output);
            Assert.AreEqual(-1, _runner.Run(null, "", 1000, out output), "null path");
            Assert.AreEqual(-1, _runner.Run("", "", 1000, out output), "empty path");
        }
    }
}
