// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
namespace Utils
{
    /// <summary>
    /// Runs a command line tool of a third party once and captures what it prints. Nothing in here knows a
    /// specific tool: The callers build the arguments and interpret the output, see
    /// <see cref="SimplicityCommanderCli"/> for the Simplicity Commander argument builders and parser.
    /// <see cref="ExternalToolRunner"/> is the implementation for the local machine; tests and applications
    /// with other needs, e.g. logging every call, substitute their own.
    /// </summary>
    public interface IExternalToolRunner
    {
        /// <summary>
        /// Runs the tool with the given arguments and waits for it to exit.
        /// </summary>
        /// <param name="toolPath">Full path of the executable.</param>
        /// <param name="arguments">Command line arguments, e.g. "device reset --serialno 440244321".</param>
        /// <param name="timeoutMs">The tool is killed when it has not exited after this many milliseconds.</param>
        /// <param name="output">Standard output of the tool, empty when it could not be read.</param>
        /// <returns>The exit code of the tool, 0 on success, or -1 when the tool could not be started, was killed or
        /// has not exited.</returns>
        int Run(string toolPath, string arguments, int timeoutMs, out string output);
    }
}
