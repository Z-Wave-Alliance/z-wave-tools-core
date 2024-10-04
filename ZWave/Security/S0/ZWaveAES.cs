/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Security
{
    public class ZWaveAES
    {
        #region Interface Required methods
        public byte[] PRNG(byte[] seed) { return new byte[] { }; }

        public void Initialize(byte[] key)
        {
            SetNbNkNr(key);

            this.key = new byte[Nk * 4];
            key.CopyTo(this.key, 0);

            KeyExpansion();
        }

        #endregion

        #region AES Core

        public void AES_ECB(byte[] key, byte[] input, byte[] output)
        {
            Initialize(key);
            Encrypt(input, output);
        }

        public void AES_ECB_Decrypt(byte[] key, byte[] input, byte[] output)
        {
            Initialize(key);
            Decrypt(input, output);
        }

        /// <summary>
        /// OFB Block Mode AES Encryption / Decryption
        /// </summary>
        /// <param name="key">Network Key</param>
        /// <param name="IV">Initialisation Vector</param>
        /// <param name="data">Input / Output Data</param>
        public void AES_OFB(byte[] key, byte[] IV, byte[] data, bool modifyIV)
        {
            byte[] tempIV;
            if (!modifyIV)
            {
                tempIV = new byte[IV.Length]; // Prevent modifications to the IV
                IV.CopyTo(tempIV, 0);
            }
            else
            {
                tempIV = IV;
            }
            /*
             * IV = Internal concat External    (16 bytes)
             * Key = Encryption Key             (16 bytes)
             * Plaintext = input                (n bytes)
             * Ciphertext = output              (n bytes)
             * Length = Plaintext.length
             * 
             * blockIndex = 0;
             * cipherIndex = 0;
             * plaintext16ByteChunk[0..plaintext.Length] = 0;
             * 
             * for(int cipherIndex=0; cipherIndex < Length; cipherIndex++)
             * {
             *   plaintext16ByteChunk[blockIndex] = Plaintext[cipherIndex]
             *   blockIndex++
             *   if(blockIndex == 15){
             *      AES_Encrypt(IV,IV,Key)
             *      Ciphertext[cipherIndex-16..cipherIndex] = plaintext16ByteChunk XOR IV
             *      plaintext16ByteChunk[0..15] = 0; // Reset 
             *      blockIndex = 0
             *   }
             * }
             * 
             * if(blockIndex != 0){
             *      AES_Encrypt(IV,IV,Key)
             *      Ciphertext[cipherIndex-16..cipherIndex] = plaintext16ByteChunk[0..blockIndex] XOR IV[0..blockIndex]
             * }
             * 
             * return blockIndex
             * 
             * */


            int cipherIndex;
            int blockIndex = 0;
            byte[] plaintext16ByteChunk = new byte[16];

            for (cipherIndex = 0; cipherIndex < data.Length; cipherIndex++)
            {
                plaintext16ByteChunk[blockIndex] = data[cipherIndex];
                blockIndex++;
                if (blockIndex == 16)
                {
                    AES_ECB(key, tempIV, tempIV);
                    int ivIndex = 0;
                    for (int i = cipherIndex - 15; i <= cipherIndex; i++)
                    {
                        //data[i] = (byte)(plaintext16ByteChunk[i] ^ tempIV[ivIndex]);
                        data[i] = (byte)(plaintext16ByteChunk[ivIndex] ^ tempIV[ivIndex]);
                        ivIndex++;
                    }
                    plaintext16ByteChunk = new byte[16];
                    blockIndex = 0;
                }
            }

            if (blockIndex != 0)
            {
                AES_ECB(key, tempIV, tempIV);
                int ivIndex = 0;
                for (int i = 0; i < blockIndex; i++)
                {
                    data[cipherIndex - blockIndex + i] = (byte)(plaintext16ByteChunk[i] ^ tempIV[i]);
                    ivIndex++;
                }
            }
        }

        public void AES_CBCMAC(byte[] key, byte[] header, byte[] data, byte[] MAC)
        {
            /*
             * key = Authentication Key
             * header = Frame Header + IV (always 20bytes)
             * data = payload data
             * dataLength = Length of data to authenticate
             * MAC = Message Authentication Code MSB8Byte((AES_ECB(header,encHeader) XOR data))
             * 
             * input = header . data
             * 
             * blockIndex = 0;
             * cipherIndex = 0;
             * plaintext16ByteChunk[0..plaintext.Length] = 0;
             * 
             * AES_ECB(input[0..15],tempMAC);
             * 
             * for(int cipherIndex=16; cipherIndex < Length; cipherIndex++)
             * {
             *   plaintext16ByteChunk[blockIndex] = Plaintext[cipherIndex]
             *   blockIndex++
             *   if(blockIndex == 15){
             *      tempMAC = tempMAC ^ plaintext16ByteChunk
             *      AES_Encrypt(tempMAC, tempMAC)
             * 
             *      plaintext16ByteChunk[0..15] = 0; // Reset 
             *      blockIndex = 0
             *   }
             * }
             * 
             * 
             * */

            byte[] input16ByteChunk = new byte[16];


            // Generate input: [header] . [data]
            byte[] input = new byte[header.Length + data.Length];
            header.CopyTo(input, 0);
            data.CopyTo(input, header.Length);


            // Perform initial hashing

            // Build initial input data, pad with 0 if lenght shorter than 16
            for (int i = 0; i < 16; i++)
            {
                if (i >= input.Length)
                {
                    input16ByteChunk[i] = 0;
                }
                else
                {
                    input16ByteChunk[i] = input[i];
                }

            }
            AES_ECB(key, input16ByteChunk, MAC);
            input16ByteChunk = new byte[16];

            // XOR tempMAC with any left over data and encrypt

            int cipherIndex;
            int blockIndex = 0;

            for (cipherIndex = 16; cipherIndex < input.Length; cipherIndex++)
            {
                input16ByteChunk[blockIndex] = input[cipherIndex];
                blockIndex++;
                if (blockIndex == 16)
                {
                    for (int i = 0; i <= 15; i++)
                    {
                        MAC[i] = (byte)(input16ByteChunk[i] ^ MAC[i]);
                    }
                    input16ByteChunk = new byte[16];
                    blockIndex = 0;

                    AES_ECB(key, MAC, MAC);
                }
            }

            if (blockIndex != 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    MAC[i] = (byte)(input16ByteChunk[i] ^ MAC[i]);
                }
                AES_ECB(key, MAC, MAC);
            }

        }


        #endregion

        #region Fields

        // Constants
        /*
        private int IN_TABLE_SIZE = 8;
        private int INTERNAL_NONCE_LIFE = 30; // 3 SEC, unit is 100ms
        private int EXTERNAL_NONCE_LIFE = 10;
        private int NONCE_REQ_TIMEOUT = 50;
        private int ILLEGAL_NODE_ID = 0xFF; // Broadcast 

        private byte[] networkKey = new byte[16];  // Network Key
        */

        private int Nb;         // block size in 32-bit words.  Always 4 for AES.  (128 bits).
        private int Nk;         // key size in 32-bit words.  4, 6, 8.  (128, 192, 256 bits).
        private int Nr;         // number of rounds. 10, 12, 14.

        private byte[] key;     // the seed key. size will be 4 * keySize from ctor.
        private byte[,] keyScheduleArray;
        private byte[,] state;  // State matrix


        private readonly byte[,] rcon = new byte[11, 4] { 
                               {0x00, 0x00, 0x00, 0x00},  
                               {0x01, 0x00, 0x00, 0x00},
                               {0x02, 0x00, 0x00, 0x00},
                               {0x04, 0x00, 0x00, 0x00},
                               {0x08, 0x00, 0x00, 0x00},
                               {0x10, 0x00, 0x00, 0x00},
                               {0x20, 0x00, 0x00, 0x00},
                               {0x40, 0x00, 0x00, 0x00},
                               {0x80, 0x00, 0x00, 0x00},
                               {0x1b, 0x00, 0x00, 0x00},
                               {0x36, 0x00, 0x00, 0x00} };

        private readonly byte[,] substitutionBox = new byte[16, 16] { 
                            {0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76},
                            {0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0},
                            {0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15},
                            {0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75},
                            {0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84},
                            {0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf},
                            {0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8},
                            {0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2},
                            {0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73},
                            {0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb},
                            {0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79},
                            {0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08},
                            {0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a},
                            {0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e},
                            {0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf},
                            {0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16} };

        private readonly byte[,] inverseSubstitutionBox = new byte[16, 16] {
                             {0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb},
                             {0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb},
                             {0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e},
                             {0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25},
                             {0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92},
                             {0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84},
                             {0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06},
                             {0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b},
                             {0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73},
                             {0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e},
                             {0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b},
                             {0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4},
                             {0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f},
                             {0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef},
                             {0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61},
                             {0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d} };

        #endregion

        #region Public Methods



        public ZWaveAES()
        {
        }


        /// <summary>
        /// Encrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        private void Encrypt(byte[] input, byte[] output)
        {

            /*
             * Initial Round:
             *      Plaintext->AddRoundKey (key0)
             *          key0 is found from the keyScheduleArray for round 0
             * 
             * */
            // state = input
            state = new byte[4, Nb];  // always [4,4]
            for (int i = 0; i < 4 * Nb; ++i)
            {
                state[i % 4, i / 4] = input[i];
            }

            AddRoundKey(0);


            /*
             * Main Loop:
             *      Inv/SubBytes->InvShiftRows->Inv/MixColumns->AddRoundKey(key1-9)  : Repeated NR-1 (NR = 10 for AES-128)
             * 
             * */

            for (int round = 1; round < Nr; round++)  // main round loop
            {
                SubBytes();
                ShiftRows();
                MixColumns();
                AddRoundKey(round);
            }  // main round loop


            /*
             * Final Roound
             *      Inv/SubBytes->InvShiftRows->AddRoundKey(key10)
             * 
             * */
            SubBytes();
            ShiftRows();
            AddRoundKey(Nr);

            /*
             * CipherText
             * */
            // output = state
            for (int i = 0; i < 4 * Nb; i++)
            {
                output[i] = state[i % 4, i / 4];
            }
        }

        /// <summary>
        /// Decrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        private void Decrypt(byte[] input, byte[] output)
        {
            // state = input
            state = new byte[4, Nb];  // always [4,4]
            for (int i = 0; i < 4 * Nb; ++i)
            {
                state[i % 4, i / 4] = input[i];
            }

            AddRoundKey(Nr);

            for (int round = Nr - 1; round >= 1; --round)  // main round loop
            {
                InvShiftRows();
                InvSubBytes();
                AddRoundKey(round);
                InvMixColumns();
            }  // end main round loop for InvCipher

            InvShiftRows();
            InvSubBytes();
            AddRoundKey(0);

            // output = state
            for (int i = 0; i < 4 * Nb; ++i)
            {
                output[i] = state[i % 4, i / 4];
            }

        }

        #endregion

        #region Private Methods

        #region Transformation methods

        private byte[] SubWord(byte[] word)
        {
            byte[] result = new byte[4];
            result[0] = substitutionBox[word[0] >> 4, word[0] & 0x0f];
            result[1] = substitutionBox[word[1] >> 4, word[1] & 0x0f];
            result[2] = substitutionBox[word[2] >> 4, word[2] & 0x0f];
            result[3] = substitutionBox[word[3] >> 4, word[3] & 0x0f];
            return result;
        }

        private byte[] RotWord(byte[] word)
        {
            byte[] result = new byte[4];
            result[0] = word[1];
            result[1] = word[2];
            result[2] = word[3];
            result[3] = word[0];
            return result;
        }

        private void SubBytes()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    state[r, c] = substitutionBox[state[r, c] >> 4, state[r, c] & 0x0f];
                }
            }
        }

        private void InvSubBytes()
        {
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    state[r, c] = inverseSubstitutionBox[state[r, c] >> 4, state[r, c] & 0x0f];
                }
            }
        }

        private void ShiftRows()
        {
            byte[,] temp = new byte[4, 4];
            for (int r = 0; r < 4; ++r)  // copy State into temp[]
            {
                for (int c = 0; c < 4; ++c)
                {
                    temp[r, c] = state[r, c];
                }
            }

            for (int r = 1; r < 4; ++r)  // shift temp into State
            {
                for (int c = 0; c < 4; ++c)
                {
                    state[r, c] = temp[r, (c + r) % Nb];
                }
            }
        }

        private void InvShiftRows()
        {
            byte[,] temp = new byte[4, 4];
            for (int r = 0; r < 4; ++r)  // copy State into temp[]
            {
                for (int c = 0; c < 4; ++c)
                {
                    temp[r, c] = state[r, c];
                }
            }
            for (int r = 1; r < 4; ++r)  // shift temp into State
            {
                for (int c = 0; c < 4; ++c)
                {
                    state[r, (c + r) % Nb] = temp[r, c];
                }
            }
        }

        private void MixColumns()
        {
            byte[,] temp = new byte[4, 4];
            for (int r = 0; r < 4; ++r)  // copy State into temp[]
            {
                for (int c = 0; c < 4; ++c)
                {
                    temp[r, c] = state[r, c];
                }
            }

            for (int c = 0; c < 4; ++c)
            {
                state[0, c] = (byte)(gfmultby02(temp[0, c]) ^ gfmultby03(temp[1, c]) ^
                                           gfmultby01(temp[2, c]) ^ gfmultby01(temp[3, c]));
                state[1, c] = (byte)(gfmultby01(temp[0, c]) ^ gfmultby02(temp[1, c]) ^
                                           gfmultby03(temp[2, c]) ^ gfmultby01(temp[3, c]));
                state[2, c] = (byte)(gfmultby01(temp[0, c]) ^ gfmultby01(temp[1, c]) ^
                                           gfmultby02(temp[2, c]) ^ gfmultby03(temp[3, c]));
                state[3, c] = (byte)(gfmultby03(temp[0, c]) ^ gfmultby01(temp[1, c]) ^
                                           gfmultby01(temp[2, c]) ^ gfmultby02(temp[3, c]));
            }
        }

        private void InvMixColumns()
        {
            byte[,] temp = new byte[4, 4];
            for (int r = 0; r < 4; ++r)  // copy State into temp[]
            {
                for (int c = 0; c < 4; ++c)
                {
                    temp[r, c] = state[r, c];
                }
            }

            for (int c = 0; c < 4; ++c)
            {
                state[0, c] = (byte)(gfmultby0e(temp[0, c]) ^ gfmultby0b(temp[1, c]) ^
                                           gfmultby0d(temp[2, c]) ^ gfmultby09(temp[3, c]));
                state[1, c] = (byte)(gfmultby09(temp[0, c]) ^ gfmultby0e(temp[1, c]) ^
                                           gfmultby0b(temp[2, c]) ^ gfmultby0d(temp[3, c]));
                state[2, c] = (byte)(gfmultby0d(temp[0, c]) ^ gfmultby09(temp[1, c]) ^
                                           gfmultby0e(temp[2, c]) ^ gfmultby0b(temp[3, c]));
                state[3, c] = (byte)(gfmultby0b(temp[0, c]) ^ gfmultby0d(temp[1, c]) ^
                                           gfmultby09(temp[2, c]) ^ gfmultby0e(temp[3, c]));
            }
        }

        #region Helper functions
        private static byte gfmultby01(byte b)
        {
            return b;
        }

        private static byte gfmultby02(byte b)
        {
            if (b < 0x80)
                return (byte)(b << 1);
            return (byte)(b << 1 ^ 0x1b);
        }

        private static byte gfmultby03(byte b)
        {
            return (byte)(gfmultby02(b) ^ b);
        }

        private static byte gfmultby09(byte b)
        {
            return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                           b);
        }

        private static byte gfmultby0b(byte b)
        {
            return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                           gfmultby02(b) ^
                           b);
        }

        private static byte gfmultby0d(byte b)
        {
            return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                           gfmultby02(gfmultby02(b)) ^
                           b);
        }

        private static byte gfmultby0e(byte b)
        {
            return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                           gfmultby02(gfmultby02(b)) ^
                           gfmultby02(b));
        }

        #endregion

        #endregion

        // SubBytes - substitutes each byte of the state.
        // ShiftRows - rotates each row of the state by an offset.
        // MixColumns - transforms columns of the state.
        // AddRoundKey - combines the 128-bit state with a 128-bit round key by adding corresponding bits mod 2. == XOR of the state and round key
        // NextRoundKey128

        /// <summary>
        /// XOR the RoundKey with current state
        /// </summary>
        private void AddRoundKey(int round)
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    state[r, c] = (byte)(state[r, c] ^ keyScheduleArray[round * 4 + c, r]);
                }
            }
        }

        private void SetNbNkNr(byte[] key)
        {
            Nb = 4;     // block size always = 4 words = 16 bytes = 128 bits for AES

            if (key.Length == 16)
            {
                Nk = 4;   // key size = 4 words = 16 bytes = 128 bits
                Nr = 10;  // rounds for algorithm = 10
            }
            else if (key.Length == 24)
            {
                Nk = 6;   // 6 words = 24 bytes = 192 bits
                Nr = 12;
            }
            else if (key.Length == 32)
            {
                Nk = 8;   // 8 words = 32 bytes = 256 bits
                Nr = 14;
            }
        }

        private void KeyExpansion()
        {
            keyScheduleArray = new byte[Nb * (Nr + 1), 4];

            for (int row = 0; row < Nk; ++row)
            {
                keyScheduleArray[row, 0] = key[4 * row];
                keyScheduleArray[row, 1] = key[4 * row + 1];
                keyScheduleArray[row, 2] = key[4 * row + 2];
                keyScheduleArray[row, 3] = key[4 * row + 3];
            }

            byte[] temp = new byte[4];

            for (int row = Nk; row < Nb * (Nr + 1); ++row)
            {
                temp[0] = keyScheduleArray[row - 1, 0]; temp[1] = keyScheduleArray[row - 1, 1];
                temp[2] = keyScheduleArray[row - 1, 2]; temp[3] = keyScheduleArray[row - 1, 3];

                if (row % Nk == 0)
                {
                    temp = SubWord(RotWord(temp));

                    temp[0] = (byte)(temp[0] ^ rcon[row / Nk, 0]);
                    temp[1] = (byte)(temp[1] ^ rcon[row / Nk, 1]);
                    temp[2] = (byte)(temp[2] ^ rcon[row / Nk, 2]);
                    temp[3] = (byte)(temp[3] ^ rcon[row / Nk, 3]);
                }
                else if (Nk > 6 && row % Nk == 4)
                {
                    temp = SubWord(temp);
                }

                // w[row] = w[row-Nk] xor temp
                keyScheduleArray[row, 0] = (byte)(keyScheduleArray[row - Nk, 0] ^ temp[0]);
                keyScheduleArray[row, 1] = (byte)(keyScheduleArray[row - Nk, 1] ^ temp[1]);
                keyScheduleArray[row, 2] = (byte)(keyScheduleArray[row - Nk, 2] ^ temp[2]);
                keyScheduleArray[row, 3] = (byte)(keyScheduleArray[row - Nk, 3] ^ temp[3]);

            }  // for loop

        }

        #endregion

        #region Helper Functions

        public static byte[] RepeatByte16(byte byteVal)
        {
            byte[] tempByte = new byte[16] { byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal, byteVal };
            return tempByte;
        }


        #endregion

    }
}
