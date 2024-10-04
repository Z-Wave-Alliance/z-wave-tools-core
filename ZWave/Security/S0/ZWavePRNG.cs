/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Security
{
    public class ZWavePRNG
    {
        private ZWaveAES AESEngine;
        private byte[] innerState;
        private readonly byte[] K1;
        private readonly byte[] K2;
        private byte[] H0;
        private byte[] H1;
        private byte[] H2;

        //private IApplicationLayer zwApplicationLayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZWavePRNG"/> class.
        /// 
        /// The ZWavePRNG is based on a seed which should be provided from a hardware source
        /// eg. a ZWave Module.
        /// The seed is then fed into the chosen Encryption method, it is up to the implementor to 
        /// ensure that the chosen encryption method is suitable for generating pseudo random numbers,
        /// which AES is considered to be.
        /// 
        /// Implementation specifics: 
        /// 
        /// State Initialisation (SDS10865 cp. 5.4.1):
        /// ------------------------------------------
        /// The inner state is initialized up including the node into the network and after SRAM loss
        /// 1. Set the inner state to zero
        /// 2. Run the state update function
        /// 
        /// State Update (SDS10865 cp. 5.4.2):
        /// ----------------------------------
        /// 1. Get 128 bits of fresh unput H2 as follow:
        ///     a. Collect 256 bit (32 byte) of data from the hardware generator
        ///     b. Split these bits into the 128-bit encoded keys doneted K1 and K2, and set H0 to be the value 0xA5 (repeated 16 times).
        ///     c. Compute H1 = AES(K1; H0) XOR H0
        ///     d. Compute H2 = AES(K2; H1) XOR H1
        /// 2. Compute S = Inner State XOR H2
        /// 3. Use S as AES key and encrypt the value 0x36 (repeated 16 time).
        /// 4. Store the result as the inner state in SRAM, and make sure to delete the old inner state all intermediate values.
        /// 
        /// Output Generation (SDS10865 cp. 5.4.3):
        /// ---------------------------------------
        /// Every time that k bits (k STE 128) of output are requested from the PRNG, the following steps are followed:
        /// 1. Use the current inner state as AES key.
        /// 2. Encrypt the value 0x5C (repeated 16 times) and use the least significant k bits of the result as PRNG output
        /// 3. Encrypt the value 0x36 (repeated 16 times) and store the result as the new inner state in SRAM. 
        ///    Make sure to delete the old inner state and all intermediate values.
        /// 
        /// 
        /// </summary>
        public ZWavePRNG(byte[] randomResult)
        {
            AESEngine = new ZWaveAES();
            K1 = new byte[16];
            K2 = new byte[16];
            H0 = new byte[16];
            H1 = new byte[16];
            H2 = new byte[16];

            // State Initialisation
            // 1. Zero innerState
            innerState = new byte[16];

            //this.zwApplicationLayer = zwApplicationLayer;

            // 2. Call StateUpdate

            PRNGUpdate(randomResult);
        }

        /// <summary>
        /// Called: When fresh input from hardware RNG is needed
        /// Task: Update the PRNG with data from hardware RNG
        /// Global vars: prngState[0..15]   Modify
        /// Temp data: byt k[16], h[16], temp[16], i, j
        /// </summary>
        public void PRNGUpdate(byte[] randomResult)
        {
            byte[] hwData;
            // 1.a. Collect 256 bit (32 byte) data from hw generator
            hwData = GetHWRNG(32, randomResult);

            // 1.b. 
            for (int i = 0; i < hwData.Length; i++)
            {
                if (i < 16)
                {
                    K1[i] = hwData[i];
                }
                else
                {
                    K2[i - 16] = hwData[i];
                }
            }
            H0 = new byte[] { 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5 };

            // 1.c.
            AESEngine.AES_ECB(K1, H0, H1);
            for (int i = 0; i < H1.Length; i++)
            {
                H1[i] ^= H0[i];
            }

            // 1.d.
            AESEngine.AES_ECB(K2, H1, H2);
            for (int i = 0; i < H2.Length; i++)
            {
                H2[i] ^= H1[i];
            }

            // 2.
            byte[] S = new byte[16];
            for (int i = 0; i < innerState.Length; i++)
            {
                S[i] = (byte)(innerState[i] ^ H2[i]);
            }

            // 3 + 4.
            byte[] ISGen = { 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36 };
            innerState = new byte[16];
            AESEngine.AES_ECB(S, ISGen, innerState);
        }


        /// <summary>
        /// Output from the PRNG
        /// Only 8 lowest bytes are written to output
        /// </summary>
        /// <returns></returns>
        public byte[] PRNGOutput(byte[] output)
        {
            byte[] tempOutput = new byte[16];
            //H0 = new byte[] { 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5, 0xA5 };
            H0 = new byte[] { 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C };

            AESEngine.AES_ECB(innerState, H0, tempOutput);

            byte[] ISGen = { 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36 };
            byte[] tempKey = new byte[innerState.Length];
            innerState.CopyTo(tempKey, 0);
            innerState = new byte[16];
            AESEngine.AES_ECB(tempKey, ISGen, innerState);

            for (int i = 0; i < 8; i++)
            {
                output[i] = tempOutput[i];
            }
            return output;
        }

        private byte[] GetHWRNG(int bytesNeeded, byte[] randomResult)
        {
            byte[] randomData = new byte[bytesNeeded];

            if (randomResult[0] == 1) // Success
            {
                byte len = randomResult[1];
                byte[] tempRandomData = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    tempRandomData[i] = randomResult[2 + i];
                }

                if (bytesNeeded > len)
                {
                    throw new InvalidOperationException("Insufficient random data");
                }

                for (int i = 0; i < bytesNeeded; i++)
                {
                    randomData[i] = tempRandomData[i];
                }
            }
            else
            {
                throw new InvalidOperationException("No random data returned, maybe Radio was blocked");
            }
            //byte[] temp =new byte[] { 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02 };
            //temp[8] = (byte) System.DateTime.Now.Millisecond;
            return randomData;
        }
    }
}
