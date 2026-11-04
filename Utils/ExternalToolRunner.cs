// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Diagnostics;

namespace Utils
{
    /// <summary>
    /// Runs a command line tool on the local machine without a window. The callers keep their own retry and
    /// logging policy, this class never logs and never throws for a failed run. For applications that stay
    /// running, see <see cref="ProcessProxy"/>.
    /// </summary>
    public sealed class ExternalToolRunner : IExternalToolRunner
    {
        /// <inheritdoc/>
        public int Run(string toolPath, string arguments, int timeoutMs, out string output)
        {
            output = string.Empty;
            if (string.IsNullOrEmpty(toolPath))
            {
                return -1;
            }

            var process = new Process();
            process.StartInfo.FileName = toolPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            try
            {
                process.Start();
            }
            catch (Exception)
            {
                // Path does not exist, access denied, ...
                process.Close();
                return -1;
            }

            var readOutput = process.StandardOutput.ReadToEndAsync();
            if (!process.WaitForExit(timeoutMs))
            {
                try
                {
                    process.Kill();
                    // Give the killed process a moment to release its handles, but never block indefinitely.
                    process.WaitForExit(5000);
                }
                catch (InvalidOperationException)
                {
                    // The process exited between the timeout and the kill.
                }
            }

            try
            {
                output = readOutput.Result ?? string.Empty;
            }
            catch (AggregateException)
            {
                output = string.Empty;
            }

            int exitCode = process.HasExited ? process.ExitCode : -1;
            process.Close();
            return exitCode;
        }
    }
}
