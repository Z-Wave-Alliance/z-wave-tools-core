/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Utils;

namespace ZWave.Security
{
    public static class SecurityS2Utils
    {
        public const ushort AUTH_DATA_HEADER_LENGTH = 8;
        public const ushort KEY_SIZE = 16;
        public const ushort NONCE_SIZE = 16;
        public const ushort PERSONALIZATION_SIZE = 32;

        private static readonly object _syncObj = new object();
        private static bool Is64Bit { get { return Environment.Is64BitProcess; } }

        #region DllImports

        /**
        * Function: CCM_encrypt_and_auth
        * 
        * \param key 16 bytes
        * \param nonce 16 bytes
        * \param aad Additional Authenticated Data
        * \param auth_tag 8 bytes (most significant bytes of the full 16-byte tag)
        */
        [DllImport("s2crypto32", EntryPoint = "CCM_encrypt_and_auth", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        private extern static uint CcmEncryptAndAuth_32([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = NONCE_SIZE)] byte[] nonce,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] aad,
            [MarshalAs(UnmanagedType.U4)] uint aad_len,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] byte[] plain_ciphertext,
            [MarshalAs(UnmanagedType.U2)] ushort plaintext_len
            );

        [DllImport("s2crypto64", EntryPoint = "CCM_encrypt_and_auth", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        private extern static uint CcmEncryptAndAuth_64([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = NONCE_SIZE)] byte[] nonce,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] aad,
            [MarshalAs(UnmanagedType.U4)] uint aad_len,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] byte[] plain_ciphertext,
            [MarshalAs(UnmanagedType.U2)] ushort plaintext_len
            );

        private static uint CcmEncryptAndAuth(byte[] key, byte[] nonce, byte[] aad, uint aad_len, byte[] plain_ciphertext, ushort plaintext_len)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    return CcmEncryptAndAuth_64(key, nonce, aad, aad_len, plain_ciphertext, plaintext_len);
                }
                else
                {
                    return CcmEncryptAndAuth_32(key, nonce, aad, aad_len, plain_ciphertext, plaintext_len);
                }
            }
        }

        /**
        * Function: CCM_decrypt_and_auth
        * 
        * Decrypt and authenticate received ciphertext and AAD
        * \return FALSE if authentication failed, TRUE otherwise.
        * \param key 16 bytes
        * \param nonce 16 bytes
        *
        * \param auth_tag 8 bytes (most significant bytes of the full 16-byte tag)
        */
        [DllImport("s2crypto32", EntryPoint = "CCM_decrypt_and_auth", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U2)]
        private extern static ushort CcmDecryptAndAuth_32([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = NONCE_SIZE)] byte[] nonce,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] aad,
            [MarshalAs(UnmanagedType.U4)] uint aad_len,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] byte[] cipher_plaintext,
            [MarshalAs(UnmanagedType.U4)] uint ciphertext_len
            );

        [DllImport("s2crypto64", EntryPoint = "CCM_decrypt_and_auth", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U2)]
        private extern static ushort CcmDecryptAndAuth_64([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = NONCE_SIZE)] byte[] nonce,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] aad,
            [MarshalAs(UnmanagedType.U4)] uint aad_len,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] byte[] cipher_plaintext,
            [MarshalAs(UnmanagedType.U4)] uint ciphertext_len
            );

        private static ushort CcmDecryptAndAuth(byte[] key, byte[] nonce, byte[] aad, uint aad_len, byte[] cipher_plaintext, uint ciphertext_len)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    return CcmDecryptAndAuth_64(key, nonce, aad, aad_len, cipher_plaintext, ciphertext_len);
                }
                else
                {
                    return CcmDecryptAndAuth_32(key, nonce, aad, aad_len, cipher_plaintext, ciphertext_len);
                }
            }
        }

        /**
        * Function: networkkey_expand
        * 
        * Expand network key into ccm key and nonce personalization string (for CTR_DRBG).
        * \param[in] network_key The network key
        * \param[out] ccm_key 16 byte CCM key.
        * \param[out] nonce_pstring The 32-byte personalization string for instantiation of SPAN NextNonce functions.
        * \param[out] mpan_key 16 byte MPAN key.
        */
        [DllImport("s2crypto32", EntryPoint = "networkkey_expand", CallingConvention = CallingConvention.Cdecl)]
        private extern static void NetworkKeyExpand_32([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] network_key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] ccm_key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] nonce_pstring,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] mpan_key
            );

        [DllImport("s2crypto64", EntryPoint = "networkkey_expand", CallingConvention = CallingConvention.Cdecl)]
        private extern static void NetworkKeyExpand_64([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] network_key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] ccm_key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] nonce_pstring,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] mpan_key
            );

        public static void NetworkKeyExpand(byte[] network_key, byte[] ccm_key, byte[] nonce_pstring, byte[] mpan_key)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    NetworkKeyExpand_64(network_key, ccm_key, nonce_pstring, mpan_key);
                }
                else
                {
                    NetworkKeyExpand_32(network_key, ccm_key, nonce_pstring, mpan_key);
                }
            }
        }

        /*
         Function: tempkey_expand
          
         Input is defined by:
             PRK is defined in the previous section of tempkey_extract()
             Constant TE 0x88 repeated 15 times
             ECDH CPK = The ECDH Public Key of the Controller
             ECDH DPK = The ECDH Public Key of the Device being included
         Output is obtained by:
             T0 = empty zero length string
             T1 = CCM Key,                            K(C) = CMAC(PRK, T0 | Constant T0 | counter 0x01)
             T2 = Nonce Personalization string 1, K(NONCE) = CMAC(PRK, T1 | Constant TE | Counter 0x02)
             T3 = Nonce Personalization string 2, K(NONCE) = CMAC(PRK, T2 | Constant TE | Counter 0x03)
             T4 = MPAN Key,                       K(MPAN)  = CMAC(PRK, T3 | Constant TE | Counter 0x04)
        */
        [DllImport("s2crypto32", EntryPoint = "tempkey_expand", CallingConvention = CallingConvention.Cdecl)]
        private extern static void TempKeyExpand_32([In, MarshalAs(UnmanagedType.LPArray)] byte[] prk,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] temp_ccm_key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] temp_nonce_pstring,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] temp_mpan_key
            );

        [DllImport("s2crypto64", EntryPoint = "tempkey_expand", CallingConvention = CallingConvention.Cdecl)]
        private extern static void TempKeyExpand_64([In, MarshalAs(UnmanagedType.LPArray)] byte[] prk,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] temp_ccm_key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] temp_nonce_pstring,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] temp_mpan_key
            );

        public static void TempKeyExpand(byte[] prk, byte[] temp_ccm_key, byte[] temp_nonce_pstring, byte[] temp_mpan_key)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    TempKeyExpand_64(prk, temp_ccm_key, temp_nonce_pstring, temp_mpan_key);
                }
                else
                {
                    TempKeyExpand_32(prk, temp_ccm_key, temp_nonce_pstring, temp_mpan_key);
                }
            }
        }

        /*
        Function: tempkey_extract
        
        Input is defined by:
        ConstantPRK   = 0x33 repeated 16 times
        ECDH Shared Secret is the output of the ECDH key exchange
        AuthTag is defines as ECDH Controller Public Key | ECDH Device Public Key
        Output is obtained by:
        PRK =  CMAC(ConstantPRK, ECDH Shared Secret | AuthTag)
        */
        [DllImport("s2crypto32", EntryPoint = "tempkey_extract", CallingConvention = CallingConvention.Cdecl)]
        private extern static void TempKeyExtract_32([In, MarshalAs(UnmanagedType.LPArray)] byte[] ecdh_share_secret,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] auth_tag,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] prk
            );

        [DllImport("s2crypto64", EntryPoint = "tempkey_extract", CallingConvention = CallingConvention.Cdecl)]
        private extern static void TempKeyExtract_64([In, MarshalAs(UnmanagedType.LPArray)] byte[] ecdh_share_secret,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] auth_tag,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] prk
            );

        public static void TempKeyExtract(byte[] ecdh_share_secret, byte[] auth_tag, byte[] prk)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    TempKeyExtract_64(ecdh_share_secret, auth_tag, prk);
                }
                else
                {
                    TempKeyExtract_32(ecdh_share_secret, auth_tag, prk);
                }
            }
        }

        /**
        Function: crypto_scalarmult_curve25519_base
         
        * Calculate the public key corresponding to a random key. The random key is usually a random
        * number. Both keys are 32-byte integers.
        * \param[out] q The public key korresponding to n.
        * \param[in] n The private key.
        */
        [DllImport("s2crypto32", EntryPoint = "crypto_scalarmult_curve25519_base", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int CryptoScalarmultCurve25519Base_32([Out, MarshalAs(UnmanagedType.LPArray)] byte[] public_key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] private_key
            );

        [DllImport("s2crypto64", EntryPoint = "crypto_scalarmult_curve25519_base", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int CryptoScalarmultCurve25519Base_64([Out, MarshalAs(UnmanagedType.LPArray)] byte[] public_key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] private_key
            );

        public static int CryptoScalarmultCurve25519Base(byte[] public_key, byte[] private_key)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    return CryptoScalarmultCurve25519Base_64(public_key, private_key);
                }
                else
                {
                    return CryptoScalarmultCurve25519Base_32(public_key, private_key);
                }
            }
        }

        /**
        Function: crypto_scalarmult_curve25519
        
        * Calculate an ECDH shared secret from a public and a private key.
        * \param[out] r The shared secret.
        * \param[in] s The public key.
        * \param[in] p The private key
        */
        [DllImport("s2crypto32", EntryPoint = "crypto_scalarmult_curve25519", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int CryptoScalarmultCurve25519_32([Out, MarshalAs(UnmanagedType.LPArray)] byte[] shared_key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] private_key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] public_key
            );

        [DllImport("s2crypto64", EntryPoint = "crypto_scalarmult_curve25519", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int CryptoScalarmultCurve25519_64([Out, MarshalAs(UnmanagedType.LPArray)] byte[] shared_key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] private_key,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] public_key
            );

        public static int CryptoScalarmultCurve25519(byte[] shared_key, byte[] private_key, byte[] public_key)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    return CryptoScalarmultCurve25519_64(shared_key, private_key, public_key);
                }
                else
                {
                    return CryptoScalarmultCurve25519_32(shared_key, private_key, public_key);
                }
            }
        }

        /* Instantiate a DRBG
         * \param[out] The DBRG to instantiate and seed
         * \param[in] The 32-byte input entropy to seed the generator. MUST be generated by a truly random source, e.g. white radio noise.
         * \param[in] 32-byte personalization string of 0x00 repeated 32 times
         */
        [DllImport("s2crypto64", EntryPoint = "AES_CTR_DRBG_Instantiate", CallingConvention = CallingConvention.Cdecl)]
        private extern static void AesCtrDrbgInstantiate_64(ref CTR_DRBG_CTX ctx,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] entropy_input,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] personalization);

        [DllImport("s2crypto32", EntryPoint = "AES_CTR_DRBG_Instantiate", CallingConvention = CallingConvention.Cdecl)]
        private extern static void AesCtrDrbgInstantiate_32(ref CTR_DRBG_CTX ctx,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] entropy_input,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] personalization);

        public static void AesCtrDrbgInstantiate(ref CTR_DRBG_CTX ctx, byte[] entropy_input, byte[] personalization)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    AesCtrDrbgInstantiate_64(ref ctx, entropy_input, personalization);
                }
                else
                {
                    AesCtrDrbgInstantiate_32(ref ctx, entropy_input, personalization);
                }
            }
        }

        /* Generate 64 bytes of RNG output.
         * \param[inout] The DBRG to generate from
         * \param[out] The 16-byte buffer to write random data to.
         */
        [DllImport("s2crypto64", EntryPoint = "AES_CTR_DRBG_Generate", CallingConvention = CallingConvention.Cdecl)]
        private extern static int AesCtrDrbgGenerate_64(ref CTR_DRBG_CTX ctx,
            [MarshalAs(UnmanagedType.LPArray)] byte[] rand);

        [DllImport("s2crypto32", EntryPoint = "AES_CTR_DRBG_Generate", CallingConvention = CallingConvention.Cdecl)]
        private extern static int AesCtrDrbgGenerate_32(ref CTR_DRBG_CTX ctx,
            [MarshalAs(UnmanagedType.LPArray)] byte[] rand);

        public static int AesCtrDrbgGenerate(ref CTR_DRBG_CTX ctx, byte[] rand)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    return AesCtrDrbgGenerate_64(ref ctx, rand);
                }
                else
                {
                    return AesCtrDrbgGenerate_32(ref ctx, rand);
                }
            }
        }

        /* Instantiate nextnonce
         * \param[in] The 16-byte input mpan state
         * \param[in] The 16-byte input mpan key
         * \param[out] An 16-byte output next mpan state
         */
        [DllImport("s2crypto32", EntryPoint = "AES128_ECB_encrypt", CallingConvention = CallingConvention.Cdecl)]
        private extern static void Aes128EcbEncrypt_32([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] input,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] output);

        [DllImport("s2crypto64", EntryPoint = "AES128_ECB_encrypt", CallingConvention = CallingConvention.Cdecl)]
        private extern static void Aes128EcbEncrypt_64([In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] input,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = KEY_SIZE)] byte[] key,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] output);

        public static void Aes128EcbEncrypt(byte[] input, byte[] key, byte[] output)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    Aes128EcbEncrypt_64(input, key, output);
                }
                else
                {
                    Aes128EcbEncrypt_32(input, key, output);
                }
            }
        }

        /* Instantiate nextnonce
         * \param[out] The DBRG to instantiate and seed
         * \param[in] The 16-byte input entropy from sender to seed the generator
         * \param[in] The 16-byte input entropy from receiver to seed the generator
         * \param[in] An 32-byte fixed length personalization string
         * We dont use security strength parameters. No reseeding counter.
         */
        [DllImport("s2crypto32", EntryPoint = "next_nonce_instantiate", CallingConvention = CallingConvention.Cdecl)]
        private extern static void NextNonceInstantiate_32(ref CTR_DRBG_CTX ctx,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = SecurityS2Utils.NONCE_SIZE)] byte[] ei_sender,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = SecurityS2Utils.NONCE_SIZE)] byte[] ei_receiver,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = SecurityS2Utils.PERSONALIZATION_SIZE)] byte[] personalization);

        [DllImport("s2crypto64", EntryPoint = "next_nonce_instantiate", CallingConvention = CallingConvention.Cdecl)]
        private extern static void NextNonceInstantiate_64(ref CTR_DRBG_CTX ctx,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = SecurityS2Utils.NONCE_SIZE)] byte[] ei_sender,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = SecurityS2Utils.NONCE_SIZE)] byte[] ei_receiver,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = SecurityS2Utils.PERSONALIZATION_SIZE)] byte[] personalization);

        public static void NextNonceInstantiate(ref CTR_DRBG_CTX ctx, byte[] ei_sender, byte[] ei_receiver, byte[] personalization)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    NextNonceInstantiate_64(ref ctx, ei_sender, ei_receiver, personalization);
                }
                else
                {
                    NextNonceInstantiate_32(ref ctx, ei_sender, ei_receiver, personalization);
                }
            }
        }

        /* Generate 64 bytes of RNG output.
         * \param[inout] The DBRG to generate from
         * \param[out] The 64 byte buffer to write random data to.
         * \return OK if random data was generated, FAIL otherwise.
         */
        [DllImport("s2crypto32", EntryPoint = "next_nonce_generate", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int NextNonceGenerate_32(ref CTR_DRBG_CTX ctx,
            [MarshalAs(UnmanagedType.LPArray)] byte[] rand);

        [DllImport("s2crypto64", EntryPoint = "next_nonce_generate", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        private extern static int NextNonceGenerate_64(ref CTR_DRBG_CTX ctx,
            [MarshalAs(UnmanagedType.LPArray)] byte[] rand);

        public static int NextNonceGenerate(ref CTR_DRBG_CTX ctx, byte[] rand)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    return NextNonceGenerate_64(ref ctx, rand);
                }
                else
                {
                    return NextNonceGenerate_32(ref ctx, rand);
                }
            }
        }

        internal static void NextNonceGenerate(ref CTR_DRBG_CTX ctx, byte[] rand, int iterations)
        {
            lock (_syncObj)
            {
                if (Is64Bit)
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        NextNonceGenerate_64(ref ctx, rand);
                    }
                }
                else
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        NextNonceGenerate_32(ref ctx, rand);
                    }
                }
            }
        }

        #endregion

        public static byte[] CcmEncryptAndAuth(byte[] key, byte[] iv, byte[] aad, byte[] textToEncrypt)
        {
            var ciphertext = new byte[textToEncrypt.Length + AUTH_DATA_HEADER_LENGTH];
            Array.Copy(textToEncrypt, ciphertext, textToEncrypt.Length);
            var ciphertextLen = (int)CcmEncryptAndAuth(key, iv, aad, (ushort)aad.Length, ciphertext, (ushort)textToEncrypt.Length);
            if (ciphertextLen > 0)
            {
                return ciphertext;
            }
            return null;
        }

        public static byte[] CcmDecryptAndAuth(byte[] key, byte[] IV, byte[] aad, byte[] ciphertext)
        {
            var textToDecrypt = new byte[ciphertext.Length];
            Array.Copy(ciphertext, textToDecrypt, ciphertext.Length);
            var plaintextLen = (int)CcmDecryptAndAuth(key, IV, aad, (ushort)aad.Length, textToDecrypt, (ushort)textToDecrypt.Length);
            if (plaintextLen > 0)
            {
                var resultData = new byte[plaintextLen];
                Array.Copy(textToDecrypt, resultData, plaintextLen);
                return resultData;
            }
            else
            {
                return new byte[0];
            }
        }
    }

    /* 
     * Context for AES-128 CTR DRBG
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct CTR_DRBG_CTX
    {
        private readonly byte _df;    /**< Use DF or not */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] _v;   /**< Internal working state */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] _k;   /**< Key working state */

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("df - " + Tools.GetHex(_df));
            sb.AppendLine("v - " + Tools.GetHex(_v));
            sb.AppendLine("k - " + Tools.GetHex(_k));
            return sb.ToString();
        }

        public CTR_DRBG_CTX(byte df, byte[] v, byte[] k)
        {
            _df = df;
            _v = v;
            _k = k;
        }

        public CTR_DRBG_CTX(byte[] buffer)
        {
            int index = 0;
            if (buffer != null && buffer.Length == 33)
            {
                _df = buffer[index++];
                _v = buffer.Skip(1).Take(16).ToArray();
                _k = buffer.Skip(17).Take(16).ToArray();
            }
            else
            {
                _df = 0;
                _v = new byte[16];
                _k = new byte[16];
            }
        }

        public CTR_DRBG_CTX Clone()
        {
            return new CTR_DRBG_CTX(_df, _v.ToArray(), _k.ToArray());
        }

        public byte[] GetBytes()
        {
            byte[] ret = new byte[37];
            ret[0] = _df;
            Array.Copy(_v, 0, ret, 1, 16);
            Array.Copy(_k, 0, ret, 17, 16);
            return ret;
        }
    }
}