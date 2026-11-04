// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Utils
{
    /// <summary>
    /// Argument builders and output parser for the Simplicity Commander command line tool (commander.exe) of
    /// Silicon Labs.
    ///
    /// Holds the pieces every caller needs and used to duplicate: Addressing a J-Link adapter by IP address or
    /// serial number, the names of the Z-Wave manufacturing tokens, building the "commander tokendump" /
    /// "commander flash" token arguments, and parsing the tokendump output. Commander itself is run through an
    /// <see cref="IExternalToolRunner"/>.
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
