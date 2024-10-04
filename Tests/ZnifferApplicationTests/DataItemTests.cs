/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZWave.ZnifferApplication;
using ZWave.Enums;
using System.Threading;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class DataItemTests
    {
        [Test]
        public void ByteArray40_GetBytes_AreEqual()
        {
            ByteArray40 val = new ByteArray40();
            for (int i = 0; i < ByteArray40.SIZE; i++)
            {
                val[i] = (byte)(i + 1);
            }

            for (int i = 0; i < ByteArray40.SIZE; i++)
            {
                Assert.AreEqual((byte)(i + 1), val[i]);
            }
        }

        [Test]
        public void ByteArray40_InverseGetBytes_AreEqual()
        {
            ByteArray40 val = new ByteArray40();
            for (int i = ByteArray40.SIZE; i > 0; i--)
            {
                val[i - 1] = (byte)(i);
            }

            for (int i = ByteArray40.SIZE; i > 0; i--)
            {
                Assert.AreEqual((byte)(i), val[i - 1]);
            }
        }


        [Test]
        public void ByteArray40_CopyFrom_AreEqual()
        {
            ByteArray40 val = new ByteArray40();
            byte[] arr = new byte[ByteArray40.SIZE];
            for (int i = 0; i < ByteArray40.SIZE; i++)
            {
                arr[i] = (byte)(i + 1);
            }

            val.CopyFrom(arr);
            byte[] arrAfter = new byte[ByteArray40.SIZE];
            val.CopyTo(0, arrAfter, 0, ByteArray40.SIZE);
            Assert.AreEqual(arr, arrAfter);
        }

        [Test]
        public void ByteArray40_CopyTo_AreEqual()
        {
            ByteArray40 val = new ByteArray40();
            byte[] arr = new byte[ByteArray40.SIZE];
            for (int i = 0; i < ByteArray40.SIZE; i++)
            {
                val[i] = (byte)(i + 1);
                arr[i] = (byte)(i + 1);
            }

            byte[] arrAfter = new byte[ByteArray40.SIZE];
            val.CopyTo(0, arrAfter, 0, ByteArray40.SIZE);
            Assert.AreEqual(arr, arrAfter);
        }

        [Test]
        public void ByteArray40_ToArray_AreEqual()
        {
            ByteArray40 val = new ByteArray40();
            byte[] arr = new byte[ByteArray40.SIZE];
            for (int i = 0; i < ByteArray40.SIZE; i++)
            {
                val[i] = (byte)(i + 1);
                arr[i] = (byte)(i + 1);
            }

            byte[] arrAfter = val.ToArray();
            Assert.AreEqual(arr, arrAfter);
        }

        [Test]
        public void Implicit_GetBytes_AreEqual()
        {
            DateTime now = DateTime.Now;
            byte[] store = new byte[] 
            {
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 
                30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
                40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
                50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
                60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
                70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
                80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
                90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
            };

            DataItem diBefore = new DataItem();
            diBefore.LineNo = 1111;
            diBefore.CreatedAt = now;
            diBefore.Frequency = 2;
            diBefore.Speed = 3;
            diBefore.Rssi = 4;
            diBefore.Channel = 5;
            diBefore.Systime = 6666;
            diBefore.WakeupCounter = 7777;
            diBefore.HeaderType = 8;
            diBefore.SetData(store);
            diBefore.ApiType = ApiTypes.Zniffer;

            IDataItemBox[] boxesBefore = new IDataItemBox[1000];
            diBefore.ToDataItemBoxes(boxesBefore);
            DataItem diAfter = DataItem.CreateFrom(boxesBefore);
            IDataItemBox[] boxesAfter = new IDataItemBox[1000];
            diAfter.ToDataItemBoxes(boxesAfter);

            Assert.AreEqual(boxesBefore[0], boxesAfter[0]);
            Assert.AreEqual(diBefore.LineNo, diAfter.LineNo);
            Assert.AreEqual(diBefore.CreatedAt, diAfter.CreatedAt);
            Assert.AreEqual(diBefore.Frequency, diAfter.Frequency);
            Assert.AreEqual(diBefore.Speed, diAfter.Speed);
            Assert.AreEqual(diBefore.Rssi, diAfter.Rssi);
            Assert.AreEqual(diBefore.Channel, diAfter.Channel);
            Assert.AreEqual(diBefore.Systime, diAfter.Systime);
            Assert.AreEqual(diBefore.WakeupCounter, diAfter.WakeupCounter);
            Assert.AreEqual(diBefore.HeaderType, diAfter.HeaderType);
            Assert.AreEqual(diBefore.ApiType, diAfter.ApiType);
            Assert.AreEqual(diBefore.Store, diAfter.Store);
            Assert.AreEqual(boxesBefore, boxesAfter);
        }

        [TestCase(byte.MaxValue - 1)]
        [TestCase(byte.MaxValue)]
        [TestCase(ushort.MaxValue - 1)]
        [TestCase(ushort.MaxValue)]
        [TestCase(1000000)]
        public void Implicit_GetRandomBytes_AreEqual(int storeLength)
        {
            Random rnd = new Random();
            DateTime now = DateTime.Now;
            byte[] store = new byte[storeLength];
            byte[] extension = new byte[storeLength];
            rnd.NextBytes(store);
            rnd.NextBytes(extension);
            DataItem diBefore = new DataItem();
            diBefore.LineNo = 1111;
            diBefore.CreatedAt = now;
            diBefore.Frequency = 2;
            diBefore.Speed = 3;
            diBefore.Rssi = 4;
            diBefore.Channel = 5;
            diBefore.Systime = 6666;
            diBefore.WakeupCounter = 7777;
            diBefore.HeaderType = 8;
            diBefore.SetData(store);
            diBefore.AddExtension(DataItemExtensionTypes.CarryPayload, extension);
            diBefore.ApiType = ApiTypes.Zniffer;

            IDataItemBox[] boxesBefore = new IDataItemBox[28000];
            diBefore.ToDataItemBoxes(boxesBefore);
            DataItem diAfter = DataItem.CreateFrom(boxesBefore);
            IDataItemBox[] boxesAfter = new IDataItemBox[28000];
            diAfter.ToDataItemBoxes(boxesAfter);

            Assert.AreEqual(boxesBefore[0], boxesAfter[0]);
            Assert.AreEqual(diBefore.LineNo, diAfter.LineNo);
            Assert.AreEqual(diBefore.CreatedAt, diAfter.CreatedAt);
            Assert.AreEqual(diBefore.Frequency, diAfter.Frequency);
            Assert.AreEqual(diBefore.Speed, diAfter.Speed);
            Assert.AreEqual(diBefore.Rssi, diAfter.Rssi);
            Assert.AreEqual(diBefore.Channel, diAfter.Channel);
            Assert.AreEqual(diBefore.Systime, diAfter.Systime);
            Assert.AreEqual(diBefore.WakeupCounter, diAfter.WakeupCounter);
            Assert.AreEqual(diBefore.HeaderType, diAfter.HeaderType);
            Assert.AreEqual(diBefore.ApiType, diAfter.ApiType);
            Assert.AreEqual(diBefore.Store, diAfter.Store);
            Assert.AreEqual(diBefore.DataLength, diAfter.DataLength);
            Assert.AreEqual(boxesBefore, boxesAfter);
        }
    }
}
