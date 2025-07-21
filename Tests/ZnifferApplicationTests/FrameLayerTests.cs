/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.ZnifferApplication;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class FrameLayerTests
    {
        [Test]
        public void Parse_Success()
        {
            var frameLayer = new ZnifferFrameLayer(null);
            var frameClient = frameLayer.CreateClient(1);

            frameClient.ReceiveFrameCallback = x => Console.WriteLine(x);
            frameClient.HandleData(new ZWave.Layers.DataChunk("21 01 ec 4e 00 00 44 21 03 0d ca fe ba be 01 01 01 0d ff 01 08 05 30".GetBytes(), 0, false, ZWave.Enums.ApiTypes.Zniffer), true);
            
        }
    }
}
