/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.Security
{
    public class Aes128Algorythm
    {
        private ZWaveAES Engine;
        /// <summary>
        /// Initializes a new instance of the <see cref="Aes128Algorythm"/> class.
        /// </summary>
        public Aes128Algorythm()
        {
            Engine = new ZWaveAES();
        }

        /// <summary>
        /// Authenticates the specified network key.
        /// </summary>
        /// <param name="networkKey">The network key.</param>
        /// <param name="noncesPayload">The nonces payload.</param>
        /// <param name="framePayload">The frame payload.</param>
        /// <param name="commandClassKey">The command class key.</param>
        /// <param name="sourceId">The source id.</param>
        /// <param name="destId">The dest id.</param>
        /// <returns></returns>
        public AuthData Authenticate(byte[] networkKey, byte[] noncesPayload, byte[] framePayload, byte commandClassKey, byte sourceId, byte destId)
        {
            try
            {

                AuthData authData = new AuthData(networkKey);
                if (noncesPayload.Length > 9 &&
                                 framePayload.Length > framePayload.Length - 9 &&
                                 framePayload.Length > 19)
                {

                    authData.Nonces = new byte[] { noncesPayload[2], 
                                    noncesPayload[3], 
                                    noncesPayload[4], 
                                    noncesPayload[5], 
                                    noncesPayload[6], 
                                    noncesPayload[7], 
                                    noncesPayload[8], 
                                    noncesPayload[9]};

                    authData.RI = framePayload[framePayload.Length - 9];

                    authData.MAC = new byte[] { framePayload[framePayload.Length-8], 
                                    framePayload[framePayload.Length-7], 
                                    framePayload[framePayload.Length-6], 
                                    framePayload[framePayload.Length-5], 
                                    framePayload[framePayload.Length-4], 
                                    framePayload[framePayload.Length-3], 
                                    framePayload[framePayload.Length-2], 
                                    framePayload[framePayload.Length-1]};

                    authData.IV = new byte[] { framePayload[2], 
                                    framePayload[3], 
                                    framePayload[4], 
                                    framePayload[5], 
                                    framePayload[6], 
                                    framePayload[7], 
                                    framePayload[8], 
                                    framePayload[9]};

                    authData.EncryptedPayload = new byte[framePayload.Length - 19];
                    int index = 0;
                    for (int i = 10; i < framePayload.Length - 9; i++)
                    {
                        authData.EncryptedPayload[index] = framePayload[i];
                        index++;
                    }
                    ZWaveAES AES = new ZWaveAES();
                    byte[] tempMAC = new byte[16];
                    byte[] authKey = new byte[16];
                    byte[] encKey = new byte[16];

                    byte[] pattern = ZWaveAES.RepeatByte16(0x55);
                    AES.AES_ECB(authData.Key, pattern, authKey); // K_A = AES(K_N,pattern)
                    pattern = ZWaveAES.RepeatByte16(0xAA);
                    AES.AES_ECB(authData.Key, pattern, encKey); // K_E = AES(K_N,pattern)


                    authData.SecurityAuthData = new ZWaveSecurityAuthData(authData.IV, authData.Nonces, commandClassKey, sourceId, destId, (byte)authData.EncryptedPayload.Length);

                    AES.AES_CBCMAC(authKey,
                        authData.SecurityAuthData.ToByteArray(),
                        authData.EncryptedPayload, tempMAC);

                    authData.IsValid = true;
                    for (int i = 0; i < 8; i++)
                    {
                        if (tempMAC[i] != authData.MAC[i])
                        {
                            authData.IsValid = false;
                            break;
                        }
                    }

                }
                return authData;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("SecurityException", ex);
            }
        }

        internal byte[] ExpandEmptyArray(int length)
        {
            byte[] result = new byte[length];
            return result;
        }

        /// <summary>
        /// Decrypts the payload.
        /// </summary>
        /// <param name="networkKey">The network key.</param>
        /// <param name="encryptedPayload">The encrypted payload.</param>
        /// <param name="IV">The IV.</param>
        /// <returns></returns>
        public byte[] DecryptPayload(byte[] networkKey, byte[] encryptedPayload, byte[] IV)
        {
            try
            {
                /////////////////////////////
                //byte[] authKey = new byte[16];
                byte[] encKey = new byte[16];

                //byte[] pattern = ZWaveAES.RepeatByte16(0x55);
                //AES.AES_ECB(authData.Key, pattern, authKey); // K_A = AES(K_N,pattern)
                byte[] pattern = ZWaveAES.RepeatByte16(0xAA);
                Engine.AES_ECB(networkKey, pattern, encKey); // K_E = AES(K_N,pattern)
                /////////////////////////////
                byte[] output = new byte[0];
                byte[] encPayload = new byte[0];

                if (encryptedPayload.Length < 16)
                {
                    output = ExpandEmptyArray(16);
                    encPayload = ExpandEmptyArray(16);
                }
                else
                {
                    output = ExpandEmptyArray(encryptedPayload.Length);
                    encPayload = ExpandEmptyArray(encryptedPayload.Length);
                }

                for (int i = 0; i < encryptedPayload.Length; i++)
                {

                    encPayload[i] = encryptedPayload[i];
                }

                Engine.AES_OFB(encKey, IV, encPayload, true);

                byte[] decryptedPayload = new byte[encryptedPayload.Length];
                for (int i = 0; i < encryptedPayload.Length; i++)
                {
                    if (i < encPayload.Length)
                    {
                        decryptedPayload[i] = encPayload[i];
                    }
                }
                return decryptedPayload;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("SecurityException", ex);
            }
        }
    }
}
