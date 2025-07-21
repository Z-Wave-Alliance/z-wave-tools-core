/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading;
using ZWave.Security;
using Utils;
using ZWave.Devices;

namespace ZWaveTests.Security
{
    [TestFixture]
    public class SecurityS2UtilsTests
    {
        [Test]
        public void CCMEncryptAndAuth_StressWithStaticParams_NoCrashes()
        {
            // Arrange.
            byte[] key = new byte[16];
            key[0] = 0xaa;
            byte[] IV = new byte[] {
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
                0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F
            };
            byte[] textToEncrypt = new byte[] { 0x20, 0x02, 0x00 };
            var aad = new AAD
            {
                SenderNodeId = new NodeTag(1),
                ReceiverNodeId = new NodeTag(2),
                HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                PayloadLength = (ushort)(textToEncrypt.Length + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH),
                SequenceNumber = 0x01
            };

            // Act.
            byte[] firstEncrypted = new byte[11];
            byte[] lastEncrypted = new byte[11];
            for (int i = 0; i < 1000; i++)
            {
                lastEncrypted = SecurityS2Utils.CcmEncryptAndAuth(key, IV, aad, textToEncrypt);
                if (i == 0)
                {
                    firstEncrypted = lastEncrypted;
                }
            }
            // Assert.
            Assert.IsTrue(firstEncrypted.SequenceEqual(lastEncrypted));
        }

        [Test]
        public void CCMDecryptAndAuth_StressWithStaticParams_NoCrashes()
        {
            // Arrange.
            byte[] key = new byte[16];
            key[0] = 0xaa;
            byte[] IV = new byte[] {
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
                0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F
            };
            byte[] cipherText = new byte[] {
                 0xc9, 0xb3, 0xa9, 0xf0, 0x4f, 0xa4, 0x17, 0xf6,
                 0x5f, 0x63, 0x08
            };
            var aad = new AAD
            {
                SenderNodeId = new NodeTag(1),
                ReceiverNodeId = new NodeTag(2),
                HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                PayloadLength = (ushort)(cipherText.Length),
                SequenceNumber = 0x01
            };

            // Act.
            byte[] firstDecrypted = new byte[3];
            byte[] lastDecrypted = new byte[3];
            for (int i = 0; i < 1000; i++)
            {
                lastDecrypted = SecurityS2Utils.CcmDecryptAndAuth(key, IV, aad, cipherText);
                if (i == 0)
                {
                    firstDecrypted = lastDecrypted;
                }
            }
            // Assert.
            Assert.IsTrue(firstDecrypted.SequenceEqual(lastDecrypted));
        }

        [Test]
        public void CCMParallelDecryptEncrypt_WithStaticParams_NoCrashes()
        {
            // Arrange.
            byte[] key = new byte[16];
            key[0] = 0xaa;
            byte[] IV = new byte[] {
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
                0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F
            };
            byte[] cipherText = new byte[] {
                 0xc9, 0xb3, 0xa9, 0xf0, 0x4f, 0xa4, 0x17, 0xf6,
                 0x5f, 0x63, 0x08
            };
            byte[] textToEncrypt = new byte[] { 0x20, 0x02, 0x00 };
            var aad = new AAD
            {
                SenderNodeId = new NodeTag(1),
                ReceiverNodeId = new NodeTag(2),
                HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                PayloadLength = (ushort)(cipherText.Length),
                SequenceNumber = 0x01
            };

            // Act.
            byte[] firstEncrypted = new byte[11];
            byte[] lastEncrypted = new byte[11];
            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var decrypted = SecurityS2Utils.CcmDecryptAndAuth(key, IV, aad, cipherText);
                }
            });

            Thread thread2 = new Thread(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        lastEncrypted = SecurityS2Utils.CcmEncryptAndAuth(key, IV, aad, textToEncrypt);
                        if (i == 0)
                        {
                            firstEncrypted = lastEncrypted;
                        }
                    }
                });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            // Assert.
        }
        private void doWork()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                byte[] key = new byte[16];
                rnd.NextBytes(key);
                byte[] IV = new byte[16];
                rnd.NextBytes(IV);
                byte[] textToEncrypt = new byte[rnd.Next(1, 1000)];
                rnd.NextBytes(textToEncrypt);
                byte[] ciphertext = new byte[textToEncrypt.Length + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH];
                var aad = new byte[10];
                rnd.NextBytes(aad);

                var cipherText = SecurityS2Utils.CcmEncryptAndAuth(key, IV, aad, textToEncrypt);
                var decrypted = SecurityS2Utils.CcmDecryptAndAuth(key, IV, aad, cipherText);
            }
        }

        [Test]
        public void CCMParallelDecryptEncrypt_WithRandomParams_NoCrashes()
        {
            // Arrange.


            // Act.
            byte[] firstEncrypted = new byte[11];
            byte[] lastEncrypted = new byte[11];

            Thread thread1 = new Thread(doWork);

            Thread thread2 = new Thread(doWork);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            // Assert.
        }

        [Test]
        public void CCMEncryptAndAuth_StressWithNonceGeneration_NoCrashes()
        {
            // Arrange.
            /*
             * Network key
             * { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 }
             */
            byte[] ccmKey = new byte[] { 0x9d, 0x67, 0x48, 0xdf, 0x1d, 0x18, 0x2f, 0xd2, 0xe0, 0x15, 0xf4, 0xda, 0x72, 0xea, 0x90, 0x61 };
            byte[] personalization = new byte[] { 0x24, 0x0c, 0x4c, 0xab, 0xab, 0x58, 0x4a, 0xf8, 0xfd, 0xde, 0xdd, 0xff, 0x53, 0x78, 0xec, 0x05, 0xb6, 0xcc, 0x22, 0x4a, 0xd3, 0x9f, 0x1a, 0x3b, 0xa3, 0x4f, 0xb1, 0xaf, 0x38, 0x97, 0x26, 0x32 };
            byte[] senderNonce = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] receiverNonce = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var spanTable = new SpanTable();
            spanTable.Add(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(2)), receiverNonce, 1, 1);
            SpanContainer nonceContainer = spanTable.GetContainer(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(2)));

            if (nonceContainer.SpanState == SpanStates.ReceiversNonce)
            {
                nonceContainer.InstantiateWithSenderNonce(senderNonce, personalization);
            }

            nonceContainer.NextNonce();

            byte[] IV = nonceContainer.Span;
            byte[] textToEncrypt = new byte[] { 0x20, 0x02, 0x00 };
            var aad = new AAD
            {
                SenderNodeId = new NodeTag(1),
                ReceiverNodeId = new NodeTag(2),
                HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                PayloadLength = (ushort)(textToEncrypt.Length + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH),
                SequenceNumber = 0x01
            };

            // Act.
            byte[] firstEncrypted = new byte[11];
            byte[] lastEncrypted = new byte[11];
            for (int i = 0; i < 1000; i++)
            {

                //lastEncrypted = SecurityS2Utils.CCMEncryptAndAuth(key, IV, aad, textToEncrypt);
                nonceContainer.NextNonce();
                //IV = nonceContainer.Span;
            }
            // Assert.

        }

        [Test]
        public void CCMEncryptAndDecrypt_StressWithNonceGeneration_NoCrashes()
        {
            // Arrange.
            /*
             * Network key
             * { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 }
             */
            byte[] ccmKey = new byte[] { 0x9d, 0x67, 0x48, 0xdf, 0x1d, 0x18, 0x2f, 0xd2, 0xe0, 0x15, 0xf4, 0xda, 0x72, 0xea, 0x90, 0x61 };
            byte[] personalization = new byte[] { 0x24, 0x0c, 0x4c, 0xab, 0xab, 0x58, 0x4a, 0xf8, 0xfd, 0xde, 0xdd, 0xff, 0x53, 0x78, 0xec, 0x05, 0xb6, 0xcc, 0x22, 0x4a, 0xd3, 0x9f, 0x1a, 0x3b, 0xa3, 0x4f, 0xb1, 0xaf, 0x38, 0x97, 0x26, 0x32 };
            byte[] senderNonce = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] receiverNonce = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var spanTable = new SpanTable();
            spanTable.Add(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(2)), receiverNonce, 1, 1);
            SpanContainer nonceContainer = spanTable.GetContainer(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(2)));
            if (nonceContainer.SpanState == SpanStates.ReceiversNonce)
            {
                nonceContainer.InstantiateWithSenderNonce(senderNonce, personalization);
            }

            nonceContainer.NextNonce();

            byte[] IV = nonceContainer.Span;
            byte[] textToEncrypt = new byte[] { 0x20, 0x02, 0x00 };
            var aad = new AAD
            {
                SenderNodeId = new NodeTag(1),
                ReceiverNodeId = new NodeTag(2),
                HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                PayloadLength = (ushort)(textToEncrypt.Length + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH),
                SequenceNumber = nonceContainer.TxSequenceNumber
            };

            // Act.
            byte[] firstEncrypted = new byte[11];
            byte[] lastEncrypted = new byte[11];
            for (int i = 0; i < 1000; i++)
            {
                aad = new AAD
                {
                    SenderNodeId = new NodeTag(1),
                    ReceiverNodeId = new NodeTag(2),
                    HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                    PayloadLength = (ushort)(textToEncrypt.Length + SecurityS2Utils.AUTH_DATA_HEADER_LENGTH),
                    SequenceNumber = nonceContainer.TxSequenceNumber
                };

                lastEncrypted = SecurityS2Utils.CcmEncryptAndAuth(ccmKey, nonceContainer.Span, aad, textToEncrypt);


                aad = new AAD
                {
                    SenderNodeId = new NodeTag(1),
                    ReceiverNodeId = new NodeTag(2),
                    HomeId = new byte[] { 0x10, 0x11, 0x12, 0x13, },
                    PayloadLength = (ushort)(lastEncrypted.Length),
                    SequenceNumber = nonceContainer.TxSequenceNumber
                };

                var plainData = SecurityS2Utils.CcmDecryptAndAuth(ccmKey, nonceContainer.Span, aad, lastEncrypted);
                Assert.IsTrue(plainData.Length > 0);

                nonceContainer.NextNonce();
                IV = nonceContainer.Span;
            }
            // Assert.
        }

        [Test]
        public void LoadKeys_CorrectNetworkKey_ReturnsNotEmptyCcmKey()
        {
            // Arrange.
            var networkKey = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
            var expectedCcmKey = new byte[] { 0x9d, 0x67, 0x48, 0xdf, 0x1d, 0x18, 0x2f, 0xd2, 0xe0, 0x15, 0xf4, 0xda, 0x72, 0xea, 0x90, 0x61 };

            // Act.
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            // Assert.
            Assert.IsTrue(expectedCcmKey.SequenceEqual(ccmKey));
        }

        [Test]
        public void LoadKeys_CorrectNetworkKey_ReturnsNotEmptyPersonalizationData()
        {
            // Arrange.
            var networkKey = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
            var expectedPersonalization = new byte[] { 0x24, 0x0c, 0x4c, 0xab, 0xab, 0x58, 0x4a, 0xf8,
                0xfd, 0xde, 0xdd, 0xff, 0x53, 0x78, 0xec, 0x05,
                0xb6, 0xcc, 0x22, 0x4a, 0xd3, 0x9f, 0x1a, 0x3b,
                0xa3, 0x4f, 0xb1, 0xaf, 0x38, 0x97, 0x26, 0x32 };

            // Act.
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            // Assert.
            Assert.IsTrue(expectedPersonalization.SequenceEqual(personalization));
        }

        [Test]
        public void LoadKeys_CorrectNetworkKey_ReturnsNotEmptyMpanKey()
        {
            // Arrange.
            var networkKey = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
            var expectedMpanKey = new byte[] { 0x26, 0xf4, 0xb8, 0x9b, 0x11, 0xf4, 0xb3, 0xd8, 0x8c, 0xe5, 0x3b, 0x92, 0x77, 0x8b, 0x28, 0x86 };

            // Act.
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.NetworkKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            // Assert.
            Assert.IsTrue(expectedMpanKey.SequenceEqual(mpanKey));
        }

        [Test]
        public void LoadTempKeys_CorrectNetworkKey_ReturnsNotEmptyCcmKey()
        {
            // Arrange.
            var networkKey = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
            var expectedTempCcmKey = new byte[] { 0xee, 0x8b, 0xbc, 0x87, 0xe0, 0x1d, 0xa3, 0xb4, 0xca, 0x4c, 0x60, 0xe5, 0x9b, 0x04, 0x5d, 0xc7 };

            // Act.
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            // Assert.
            Assert.IsTrue(expectedTempCcmKey.SequenceEqual(ccmKey));
        }

        [Test]
        public void LoadTempKeys_CorrectNetworkKey_ReturnsNotEmptyPersonalizationData()
        {
            // Arrange.
            var networkKey = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
            var expectedTempPersonalization = new byte[] { 0x98, 0xcc, 0x37, 0x0f, 0x0a, 0xf2, 0xaa, 0x25,
                0x66, 0x88, 0xa9, 0x33, 0x16, 0x04, 0xb4, 0x23,
                0xe6, 0xbe, 0x12, 0x0a, 0x32, 0x23, 0xd3, 0x98,
                0x69, 0x2d, 0x5a, 0x02, 0x25, 0x27, 0x80, 0x5b };

            // Act.
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            // Assert.
            Assert.IsTrue(expectedTempPersonalization.SequenceEqual(personalization));
        }

        [Test]
        public void LoadTempKeys_CorrectNetworkKey_ReturnsNotEmptyMpanKey()
        {
            // Arrange.
            var networkKey = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
            var expectedMpanKey = new byte[] { 0x17, 0x8c, 0x57, 0x60, 0x39, 0xc8, 0x72, 0x80, 0xf3, 0x7b, 0x2b, 0xef, 0x3e, 0x8c, 0x60, 0x91 };

            // Act.
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.TempKeyExpand(networkKey, ccmKey, personalization, mpanKey);

            // Assert.
            Assert.IsTrue(expectedMpanKey.SequenceEqual(mpanKey));
        }

        [Test]
        public void GenerateNextNonceWithPredefinedState_GeneratedNonceAlwaysTheSame()
        {
            byte[] expectedSpan = Tools.GetBytes("28 16 F5 C0 BE 91 22 4F EA 11 C5 D3 73 67 1D F9");
            byte df = 0;
            byte[] v = Tools.GetBytes("74 48 66 3C 55 00 B6 37 5A 6B E5 C9 EA 43 08 28");
            byte[] k = Tools.GetBytes("A3 7E 83 98 5C 4F FE 0B 13 90 DC A4 DC 29 65 C6");

            for (int i = 0; i < 100000; i++)
            {
                CTR_DRBG_CTX initialState = new CTR_DRBG_CTX(df, v, k);
                var actualSpan = new byte[16];
                SecurityS2Utils.NextNonceGenerate(ref initialState, actualSpan);
                Assert.IsTrue(expectedSpan.SequenceEqual(actualSpan));
            }

            for (int i = 0; i < 100000; i++)
            {
                CTR_DRBG_CTX initialState = new CTR_DRBG_CTX(df, v, k);
                var actualSpan = new byte[16];
                SecurityS2Utils.NextNonceGenerate(ref initialState, actualSpan);
                Assert.IsTrue(expectedSpan.SequenceEqual(actualSpan));
            }
        }

    }
}
