/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils;

namespace ZWave.Security
{
    public static class SecurityS0Utils
    {
        public static byte[] GenerateNetworkKey(ZWavePRNG prng)
        {
            byte[] ret = new byte[16];
            byte[] tempKeyLSB = new byte[8];
            byte[] tempKeyMSB = new byte[8];
            prng.PRNGOutput(tempKeyLSB);
            prng.PRNGOutput(tempKeyMSB);
            for (int i = 0; i < 16; i++)
            {
                if (i > 7)
                {
                    ret[i] = tempKeyMSB[i - 8];
                }
                else
                {
                    ret[i] = tempKeyLSB[i];
                }
            }
            return ret;
        }

        public static void LoadKeys(ZWaveAES aesEngine, byte[] networkKey, out byte[] authKey, out byte[] encKey)
        {
            authKey = new byte[16];
            encKey = new byte[16];
            byte[] pattern = ZWaveAES.RepeatByte16(0x55);
            aesEngine.AES_ECB(networkKey, pattern, authKey); // K_A = AES(K_N,pattern)
            pattern = ZWaveAES.RepeatByte16(0xAA);
            aesEngine.AES_ECB(networkKey, pattern, encKey); // K_E = AES(K_N,pattern)
        }

        public static byte[] MakeAuthTag(ZWaveAES aesEngine, byte[] authKey, byte[] header, byte[] payload)
        {
            byte[] tempMAC = new byte[16];
            aesEngine.AES_CBCMAC(authKey, header, payload, tempMAC);
            byte[] temp8bitMAC = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                temp8bitMAC[i] = tempMAC[i];
            }
            //Tools._writeDebugDiagnosticMessage("****MAKE - AK:" + Tools.GetHex(authKey, "", false) + " header:" + Tools.GetHex(header, "", false) + " payload:" + Tools.GetHex(payload, "", false) + " code:" + Tools.GetHex(tempMAC, "", false));
            return temp8bitMAC;
        }

        public static bool VerifyAuthTag(ZWaveAES aesEngine, byte[] authKey, byte[] header, byte[] payload, byte[] remoteMAC)
        {
            // Calculate actual MAC
            byte[] localMAC = new byte[16];
            aesEngine.AES_CBCMAC(authKey, header, payload, localMAC);
            byte[] local8bitMAC = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                local8bitMAC[i] = localMAC[i];
            }

            bool ret = Tools.ByteArrayEquals(local8bitMAC, remoteMAC);
            //Tools._writeDebugDiagnosticMessage("****VERF - AK:" + Tools.GetHex(authKey, "", false) + " header:" + Tools.GetHex(header, "", false) + " payload:" + Tools.GetHex(payload, "", false) + " code:" + Tools.GetHex(localMAC, "", false) + " remote:" + Tools.GetHex(remoteMAC, "", false));
            return ret;
        }

        public static void Encrypt(ZWaveAES aesEngine, byte[] encKey, byte[] iv, ref byte[] payload)
        {
            string pre = payload.GetHex();
            aesEngine.AES_OFB(encKey, iv, payload, false);
            string after = payload.GetHex();
            "S0 {0} = {1}"._DLOG(pre, after);
        }

        public static void Decrypt(ZWaveAES aesEngine, byte[] encKey, byte[] iv, ref byte[] payload)
        {
            string pre = payload.GetHex();
            aesEngine.AES_OFB(encKey, iv, payload, false);
            string after = payload.GetHex();
            "S0 {0} = {1}"._DLOG(after, pre);
        }
    }
}
