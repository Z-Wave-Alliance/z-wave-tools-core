/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using System.Linq;
using Utils;
using ZWave.Security;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class CryptoApi
    {
        const int KEY_SIZE = 32;
        static readonly byte[] alice_secret_key = new byte[] {
            0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d,
            0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
            0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a,
            0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
        };

        static readonly byte[] expected_alice_public_key = new byte[] {
            0x85, 0x20, 0xf0, 0x09, 0x89, 0x30, 0xa7, 0x54,
            0x74, 0x8b, 0x7d, 0xdc, 0xb4, 0x3e, 0xf7, 0x5a,
            0x0d, 0xbf, 0x3a, 0x0d, 0x26, 0x38, 0x1a, 0xf4,
            0xeb, 0xa4, 0xa9, 0x8e, 0xaa, 0x9b, 0x4e, 0x6a
        };

        static readonly byte[] bob_secret_key = new byte[] {
            0x5d, 0xab, 0x08, 0x7e, 0x62, 0x4a, 0x8a, 0x4b,
            0x79, 0xe1, 0x7f, 0x8b, 0x83, 0x80, 0x0e, 0xe6,
            0x6f, 0x3b, 0xb1, 0x29, 0x26, 0x18, 0xb6, 0xfd,
            0x1c, 0x2f, 0x8b, 0x27, 0xff, 0x88, 0xe0, 0xeb
        };

        static readonly byte[] expected_bob_public_key = new byte[] {
            0xde, 0x9e, 0xdb, 0x7d, 0x7b, 0x7d, 0xc1, 0xb4,
            0xd3, 0x5b, 0x61, 0xc2, 0xec, 0xe4, 0x35, 0x37,
            0x3f, 0x83, 0x43, 0xc8, 0x5b, 0x78, 0x67, 0x4d,
            0xad, 0xfc, 0x7e, 0x14, 0x6f, 0x88, 0x2b, 0x4f
        };

        [Test]
        public void TestAliceCalculationOfPublicKey()
        {
            byte[] alice_public_key = new byte[KEY_SIZE];
            byte[] bob_public_key = new byte[KEY_SIZE];

            SecurityS2Utils.CryptoScalarmultCurve25519Base(alice_public_key, alice_secret_key);
            SecurityS2Utils.CryptoScalarmultCurve25519Base(bob_public_key, bob_secret_key);

            Assert.AreEqual(expected_alice_public_key, alice_public_key);
            Assert.AreEqual(expected_bob_public_key, bob_public_key);
        }


        [Test]
        public void TestAesCtrDrbgInstantiate()
        {
            CTR_DRBG_CTX ctx = new CTR_DRBG_CTX();
            byte[] tEntropy = Tools.GetBytes("cee23de86a69c7ef57f6e1e12bd16e35e51624226fa19597bf93ec476a44b0f2");
            byte[] tPersonal = Tools.GetBytes("a2ef16f226ea324f23abd59d5e3c660561c25e73638fe21c87566e86a9e04c3e");
            byte[] v = Tools.GetBytes("87 5C A0 9F 6C 98 D4 19 CB ED 40 78 B2 16 02 B4");
            byte[] k = Tools.GetBytes("34 EF D7 D4 B6 FD C5 C1 42 22 29 2B D1 0A 4D 6A");

            SecurityS2Utils.AesCtrDrbgInstantiate(ref ctx, tEntropy, tPersonal);
            Assert.AreEqual(v, ctx.GetBytes().Skip(1).Take(16).ToArray());
            Assert.AreEqual(k, ctx.GetBytes().Skip(17).Take(16).ToArray());
        }

        [Test]
        public void TestAesCtrDrbgInstantiate0()
        {
            CTR_DRBG_CTX ctx = new CTR_DRBG_CTX();
           
            byte[] v = Tools.GetBytes("03 88 DA CE 60 B6 A3 92 F3 28 C2 B9 71 B2 FE 78");
            byte[] k = Tools.GetBytes("58 E2 FC CE FA 7E 30 61 36 7F 1D 57 A4 E7 45 5A");

            SecurityS2Utils.AesCtrDrbgInstantiate(ref ctx, new byte[32], new byte[32]);
            Assert.AreEqual(v, ctx.GetBytes().Skip(1).Take(16).ToArray());
            Assert.AreEqual(k, ctx.GetBytes().Skip(17).Take(16).ToArray());
        }

        [Test]
        public void TestNextNonceInstantiate()
        {
            CTR_DRBG_CTX ctx = new CTR_DRBG_CTX();
            byte[] tSenderEntropy = Tools.GetBytes("cee23de86a69c7ef57f6e1e12bd16e35");
            byte[] tReceiverEntropy = Tools.GetBytes("e51624226fa19597bf93ec476a44b0f2");
            byte[] tPersonal = Tools.GetBytes("a2ef16f226ea324f23abd59d5e3c660561c25e73638fe21c87566e86a9e04c3e");
            byte[] v = Tools.GetBytes("82 82 92 10 88 F9 72 AB C5 22 34 DF 7B 66 46 56");
            byte[] k = Tools.GetBytes("A2 DD D4 34 0B F5 9F 0C 79 10 D6 DC 5F B5 39 86");

            SecurityS2Utils.NextNonceInstantiate(ref ctx, tSenderEntropy, tReceiverEntropy, tPersonal);
            Assert.AreEqual(v, ctx.GetBytes().Skip(1).Take(16).ToArray());
            Assert.AreEqual(k, ctx.GetBytes().Skip(17).Take(16).ToArray());
        }

        [Test]
        public void TestNextNonceGenerate()
        {
            byte[] initV = Tools.GetBytes("82 82 92 10 88 F9 72 AB C5 22 34 DF 7B 66 46 56");
            byte[] initK = Tools.GetBytes("A2 DD D4 34 0B F5 9F 0C 79 10 D6 DC 5F B5 39 86");
            CTR_DRBG_CTX ctx = new CTR_DRBG_CTX(new byte[1].Concat(initV).Concat(initK).ToArray());
            byte[] tRand = Tools.GetBytes("a2ef16f226ea324f23abd59d5e3c660561c25e73638fe21c87566e86a9e04c3e");
            byte[] v = Tools.GetBytes("65 1D D0 05 68 77 EF B7 4C D8 74 6D 71 B3 09 8A");
            byte[] k = Tools.GetBytes("E5 35 9A A6 F1 A6 91 50 54 4D 1C C0 D7 53 0E CD");

            SecurityS2Utils.NextNonceGenerate(ref ctx, tRand);
            Assert.AreEqual(v, ctx.GetBytes().Skip(1).Take(16).ToArray());
            Assert.AreEqual(k, ctx.GetBytes().Skip(17).Take(16).ToArray());
        }

        [Test]
        public void TestNextNonceGenerate0()
        {
            CTR_DRBG_CTX ctx = new CTR_DRBG_CTX();
            byte[] rand = new byte[32];
            byte[] v = Tools.GetBytes("F7 95 AA AB 49 4B 59 23 F7 FD 89 FF 94 8B C1 E0");
            byte[] k = Tools.GetBytes("03 88 DA CE 60 B6 A3 92 F3 28 C2 B9 71 B2 FE 78");

            SecurityS2Utils.AesCtrDrbgGenerate(ref ctx, rand);
            Assert.AreEqual(v, ctx.GetBytes().Skip(1).Take(16).ToArray());
            Assert.AreEqual(k, ctx.GetBytes().Skip(17).Take(16).ToArray());
        }
    }
}
