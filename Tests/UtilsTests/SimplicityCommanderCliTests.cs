// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using NUnit.Framework;
using Utils;

namespace UtilsTests
{
    /// <summary>
    /// Covers the pure helpers of the Simplicity Commander wrapper. Running Commander itself needs the
    /// tool and a J-Link adapter, so only argument building and output parsing are exercised here.
    /// </summary>
    [TestFixture]
    public class SimplicityCommanderCliTests
    {
        private const string PUK_HEX = "A1B2C3D4E5F60718293A4B5C6D7E8F900102030405060708090A0B0C0D0E0F10";

        #region Adapter argument

        [Test]
        public void GetAdapterArgument_IpAddress_SelectsByIp()
        {
            Assert.AreEqual("--ip 192.168.1.20", SimplicityCommanderCli.GetAdapterArgument("192.168.1.20", "440244321"));
        }

        [Test]
        public void GetAdapterArgument_SerialPort_SelectsBySerialNumber()
        {
            Assert.AreEqual("--serialno 440244321", SimplicityCommanderCli.GetAdapterArgument("COM7", "440244321"));
        }

        [Test]
        public void GetAdapterArgument_NoIpAndNoSerialNumber_ReturnsNull()
        {
            Assert.IsNull(SimplicityCommanderCli.GetAdapterArgument("COM7", null));
            Assert.IsNull(SimplicityCommanderCli.GetAdapterArgument(null, ""));
        }

        #endregion

        #region Token arguments

        [Test]
        public void TokenNames_MatchCommander()
        {
            Assert.AreEqual("znet", SimplicityCommanderCli.ZWAVE_TOKEN_GROUP);
            Assert.AreEqual("MFG_ZW_PUK", SimplicityCommanderCli.MFG_ZW_PUK_TOKEN);
            Assert.AreEqual("MFG_ZW_PRK", SimplicityCommanderCli.MFG_ZW_PRK_TOKEN);
            Assert.AreEqual("MFG_ZWAVE_COUNTRY_FREQ", SimplicityCommanderCli.MFG_ZWAVE_COUNTRY_FREQ_TOKEN);
        }

        [Test]
        public void GetTokenDumpArguments_WithAndWithoutDevice()
        {
            Assert.AreEqual("tokendump --tokengroup znet --token MFG_ZW_PUK --serialno 440244321",
                SimplicityCommanderCli.GetTokenDumpArguments(SimplicityCommanderCli.MFG_ZW_PUK_TOKEN, "--serialno 440244321"));
            Assert.AreEqual("tokendump --tokengroup znet --token MFG_ZW_PUK --ip 10.0.0.5 --device Cortex-M4",
                SimplicityCommanderCli.GetTokenDumpArguments(SimplicityCommanderCli.MFG_ZW_PUK_TOKEN, "--ip 10.0.0.5", "--device Cortex-M4"));
        }

        [Test]
        public void GetTokenDumpArguments_SeveralTokens_DumpsAllInOneRun()
        {
            string[] tokens = { SimplicityCommanderCli.MFG_ZW_PRK_TOKEN, SimplicityCommanderCli.MFG_ZW_PUK_TOKEN };

            Assert.AreEqual("tokendump --tokengroup znet --token MFG_ZW_PRK --token MFG_ZW_PUK --serialno 440244321",
                SimplicityCommanderCli.GetTokenDumpArguments(tokens, "--serialno 440244321"));
        }

        [Test]
        public void GetTokenFlashArguments_SingleByteToken_WritesHexValue()
        {
            Assert.AreEqual("flash --tokengroup znet --token MFG_ZWAVE_COUNTRY_FREQ:0x01 --serialno 440244321",
                SimplicityCommanderCli.GetTokenFlashArguments(SimplicityCommanderCli.MFG_ZWAVE_COUNTRY_FREQ_TOKEN, 0x01, "--serialno 440244321"));
            Assert.AreEqual("flash --tokengroup znet --token MFG_ZWAVE_COUNTRY_FREQ:0xFF --ip 10.0.0.5 --device Cortex-M4",
                SimplicityCommanderCli.GetTokenFlashArguments(
                    SimplicityCommanderCli.MFG_ZWAVE_COUNTRY_FREQ_TOKEN, 0xFF, "--ip 10.0.0.5", "--device Cortex-M4"));
        }

        [Test]
        public void GetTokenFileFlashArguments_QuotesEveryFile()
        {
            string[] files = { @"C:\keys\encrypt-key.txt", @"C:\keys\sign-key-tokens.txt" };

            string expected = "flash --tokengroup znet --tokenfile \"C:\\keys\\encrypt-key.txt\" --tokenfile \"C:\\keys\\sign-key-tokens.txt\""
                + " --serialno 440244321";

            Assert.AreEqual(expected, SimplicityCommanderCli.GetTokenFileFlashArguments(files, "--serialno 440244321"));
        }

        #endregion

        #region Token parsing

        [Test]
        public void ParseTokenBytes_PlainHexByteArray_ReturnsBytes()
        {
            string output = "# tokendump of znet\r\nMFG_ZW_PRK : 00112233\r\nMFG_ZW_PUK : " + PUK_HEX + "\r\nDONE\r\n";

            byte[] puk = SimplicityCommanderCli.ParseTokenBytes(output, "MFG_ZW_PUK");

            Assert.AreEqual(32, puk.Length);
            Assert.AreEqual(0xA1, puk[0]);
            Assert.AreEqual(0x10, puk[31]);
        }

        [Test]
        public void ParseTokenBytes_PrefixedInteger_ReturnsSingleByte()
        {
            byte[] region = SimplicityCommanderCli.ParseTokenBytes("MFG_ZWAVE_COUNTRY_FREQ: 0x01\nDONE", "MFG_ZWAVE_COUNTRY_FREQ");

            Assert.AreEqual(new byte[] { 0x01 }, region);
        }

        [Test]
        public void ParseTokenBytes_PrefixedBytesSeparatedBySpaces_ReturnsBytes()
        {
            byte[] bytes = SimplicityCommanderCli.ParseTokenBytes("MFG_ZW_PUK : 0xA1 0xB2 0xc3", "MFG_ZW_PUK");

            Assert.AreEqual(new byte[] { 0xA1, 0xB2, 0xC3 }, bytes);
        }

        [Test]
        public void ParseTokenBytes_TokenNameIsCaseInsensitive()
        {
            Assert.AreEqual(new byte[] { 0xFF }, SimplicityCommanderCli.ParseTokenBytes("mfg_zwave_country_freq : 0xFF", "MFG_ZWAVE_COUNTRY_FREQ"));
        }

        [Test]
        public void ParseTokenBytes_CommentLineIsIgnored()
        {
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes("# MFG_ZW_PUK : " + PUK_HEX, "MFG_ZW_PUK"));
        }

        [Test]
        public void ParseTokenBytes_MissingToken_ReturnsNull()
        {
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes("MFG_ZW_PRK : 00112233\nDONE", "MFG_ZW_PUK"));
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes("", "MFG_ZW_PUK"));
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes(null, "MFG_ZW_PUK"));
        }

        [Test]
        public void ParseTokenBytes_InvalidValue_ReturnsNull()
        {
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes("MFG_ZW_PUK : not hex", "MFG_ZW_PUK"), "non hex");
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes("MFG_ZW_PUK : A1B", "MFG_ZW_PUK"), "odd length");
            Assert.IsNull(SimplicityCommanderCli.ParseTokenBytes("MFG_ZW_PUK :", "MFG_ZW_PUK"), "empty value");
        }

        [Test]
        public void IsErased_AllFF_IsTrue()
        {
            Assert.IsTrue(SimplicityCommanderCli.IsErased(new byte[] { 0xFF, 0xFF, 0xFF }));
            Assert.IsFalse(SimplicityCommanderCli.IsErased(new byte[] { 0xFF, 0x00, 0xFF }));
            Assert.IsFalse(SimplicityCommanderCli.IsErased(new byte[0]));
            Assert.IsFalse(SimplicityCommanderCli.IsErased(null));
        }

        #endregion
    }
}
