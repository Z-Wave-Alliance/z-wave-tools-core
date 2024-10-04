/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections;

namespace Utils
{
    public class BitMatrix
    {
        public BitArray[] Items { get; set; }

        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public BitMatrix(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Init();
        }

        private void Init()
        {
            Items = new BitArray[SizeY];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new BitArray(SizeX);
            }
        }
    }
}

