// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Utils
{
    /// <summary>
    /// Thin wrapper around the Simplicity Commander command line tool (commander.exe) of Silicon Labs.
    ///
    /// Holds the pieces every caller needs and used to duplicate: running Commander with a timeout, addressing
    /// a J-Link adapter by IP address or serial number, the names of the Z-Wave manufacturing tokens, building
    /// the "commander tokendump" / "commander flash" token arguments, and parsing the tokendump output.
    /// The callers keep their own retry and logging policy, this class never logs and never throws for a
    /// failed Commander run.
    /// </summary>
    public static class SimplicityCommanderCli
    {
        /// <summary>
        /// Token group of the Z-Wave manufacturing tokens, see the MFG_* constants.
        /// </summary>
        public const string ZWAVE_TOKEN_GROUP = "znet";

        /// <summary>
        /// Manufacturing token holding the S2 public key (32 bytes); its first 16 bytes form the DSK.
        /// </summary>
        public const string MFG_ZW_PUK_TOKEN = "MFG_ZW_PUK";

        /// <summary>
        /// Manufacturing token holding the S2 private key (32 bytes).
        /// </summary>
        public const string MFG_ZW_PRK_TOKEN = "MFG_ZW_PRK";

        /// <summary>
        /// Manufacturing token holding the RF region as one byte (the Z-Wave region enumeration value).
        /// </summary>
        public const string MFG_ZWAVE_COUNTRY_FREQ_TOKEN = "MFG_ZWAVE_COUNTRY_FREQ";

        /// <summary>
        /// Runs Commander with the given arguments and waits for it to exit.
        /// </summary>
        /// <param name="commanderPath">Full path of commander.exe.</param>
        /// <param name="arguments">Command line arguments, e.g. "device reset --serialno 440244321".</param>
        /// <param name="timeoutMs">Commander is killed when it has not exited after this many milliseconds.</param>
        /// <param name="output">Standard output of Commander, empty when it could not be read.</param>
        /// <returns>The Commander exit code, 0 on success, or -1 when Commander could not be started, was killed or
        /// has not exited.</returns>
        public static int Run(string commanderPath, string arguments, int timeoutMs, out string output)
        {
            output = string.Empty;
            if (string.IsNullOrEmpty(commanderPath))
            {
                return -1;
            }

            var process = new Process();
            process.StartInfo.FileName = commanderPath;
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

        /// <summary>
        /// Builds the Commander argument that selects the J-Link adapter: "--ip" followed by the address when the port
        /// name is an IP address (J-Link over Ethernet), otherwise "--serialno" followed by the J-Link serial number.
        /// </summary>
        /// <returns>The argument, or null when neither an IP address nor a J-Link serial number is known.</returns>
        public static string GetAdapterArgument(string portNameOrIpAddress, string jLinkSerialNumber)
        {
            if (!string.IsNullOrEmpty(portNameOrIpAddress) && IPAddress.TryParse(portNameOrIpAddress, out IPAddress ipAddress))
            {
                return $"--ip {ipAddress}";
            }

            if (!string.IsNullOrEmpty(jLinkSerialNumber))
            {
                return $"--serialno {jLinkSerialNumber}";
            }

            return null;
        }

        /// <summary>
        /// Builds the arguments of "commander tokendump" for one token of the Z-Wave token group.
        /// </summary>
        /// <param name="tokenName">Token name, e.g. <see cref="MFG_ZW_PUK_TOKEN"/>.</param>
        /// <param name="adapterArgument">Adapter selection from <see cref="GetAdapterArgument"/>.</param>
        /// <param name="deviceArgument">Optional target selection, e.g. "--device Cortex-M4"; null to let Commander
        /// detect the part.</param>
        public static string GetTokenDumpArguments(string tokenName, string adapterArgument, string deviceArgument = null)
        {
            return GetTokenDumpArguments(new[] { tokenName }, adapterArgument, deviceArgument);
        }

        /// <summary>
        /// Builds the arguments of "commander tokendump" for several tokens of the Z-Wave token group, dumped in one run.
        /// </summary>
        public static string GetTokenDumpArguments(IEnumerable<string> tokenNames, string adapterArgument, string deviceArgument = null)
        {
            string tokens = string.Join(" ", tokenNames.Select(tokenName => $"--token {tokenName}"));
            return AppendTarget($"tokendump --tokengroup {ZWAVE_TOKEN_GROUP} {tokens}", adapterArgument, deviceArgument);
        }

        /// <summary>
        /// Builds the arguments of "commander flash" that write one single-byte token of the Z-Wave token group,
        /// e.g. the RF region into <see cref="MFG_ZWAVE_COUNTRY_FREQ_TOKEN"/>.
        /// </summary>
        public static string GetTokenFlashArguments(string tokenName, byte value, string adapterArgument, string deviceArgument = null)
        {
            return AppendTarget($"flash --tokengroup {ZWAVE_TOKEN_GROUP} --token {tokenName}:0x{value:X2}", adapterArgument, deviceArgument);
        }

        /// <summary>
        /// Builds the arguments of "commander flash" that write the Z-Wave token group tokens listed in token files
        /// (the bootloader encryption and signing keys, for example).
        /// </summary>
        public static string GetTokenFileFlashArguments(IEnumerable<string> tokenFilePaths, string adapterArgument, string deviceArgument = null)
        {
            string tokenFiles = string.Join(" ", tokenFilePaths.Select(path => $"--tokenfile \"{path}\""));
            return AppendTarget($"flash --tokengroup {ZWAVE_TOKEN_GROUP} {tokenFiles}", adapterArgument, deviceArgument);
        }

        private static string AppendTarget(string arguments, string adapterArgument, string deviceArgument)
        {
            arguments += $" {adapterArgument}";
            if (!string.IsNullOrEmpty(deviceArgument))
            {
                arguments += $" {deviceArgument}";
            }

            return arguments;
        }

        /// <summary>
        /// Extracts the value of one token from the output of "commander tokendump".
        ///
        /// Commander prints every token on its own line as the token name, a colon and the value. Integers are printed with a
        /// "0x" prefix ("MFG_ZWAVE_COUNTRY_FREQ : 0x01"), byte arrays as plain hex digits
        /// ("MFG_ZW_PUK : A1B2..."). Both forms are accepted, as are bytes separated by spaces.
        /// Comment lines (starting with '#') are ignored.
        /// </summary>
        /// <returns>The token bytes, or null when the token line is missing or its value is not hex.</returns>
        public static byte[] ParseTokenBytes(string tokenDumpOutput, string tokenName)
        {
            if (string.IsNullOrEmpty(tokenDumpOutput) || string.IsNullOrEmpty(tokenName))
            {
                return null;
            }

            string[] lines = tokenDumpOutput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string tokenLine = lines
                .Select(line => line.Trim())
                .FirstOrDefault(line => !line.StartsWith("#") && line.StartsWith(tokenName, StringComparison.OrdinalIgnoreCase));
            if (tokenLine == null)
            {
                return null;
            }

            string value = tokenLine.Substring(tokenName.Length).TrimStart(' ', '\t', ':');
            string[] parts = value.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            string hex = string.Concat(parts.Select(StripHexPrefix));
            if (hex.Length == 0 || hex.Length % 2 != 0 || !hex.All(Uri.IsHexDigit))
            {
                return null;
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// True when the token reads as erased flash (every byte 0xFF). The Z-Wave key pair tokens are generated at
        /// boot, so they read as erased when the device has not finished booting yet.
        /// </summary>
        public static bool IsErased(byte[] tokenBytes)
        {
            return tokenBytes != null && tokenBytes.Length > 0 && tokenBytes.All(b => b == 0xFF);
        }

        private static string StripHexPrefix(string part)
        {
            return part.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? part.Substring(2) : part;
        }
    }
}
