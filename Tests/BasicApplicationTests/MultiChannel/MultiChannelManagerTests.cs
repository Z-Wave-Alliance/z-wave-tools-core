// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using NUnit.Framework;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace BasicApplicationTests.MultiChannel
{
    [TestFixture]
    public class MultiChannelManagerTests
    {
        private readonly NetworkViewPoint _network = new NetworkViewPoint();

        [Test]
        public void SubstituteActionInternal_EndPointCommand_AddsMultiChannelEncap()
        {
            var action = new SendDataOperation(_network, new NodeTag(2, 1), new byte[] { 0x20, 0x01, 0xFF },
                TransmitOptions.TransmitOptionAcknowledge);

            var result = (SendDataOperation)new MultiChannelManager(_network).SubstituteActionInternal(action);

            Assert.AreEqual(COMMAND_CLASS_MULTI_CHANNEL_V2.ID, result.Data[0]);
            Assert.AreEqual(COMMAND_CLASS_MULTI_CHANNEL_V2.MULTI_CHANNEL_CMD_ENCAP.ID, result.Data[1]);
        }

        /// <summary>
        /// The SecurityManager answers an S2 MOS extension with an action that carries only the MPAN
        /// extension and no command data.
        /// </summary>
        [Test]
        public void SubstituteActionInternal_NoCommandData_LeavesActionUnchanged()
        {
            var action = new SendDataOperation(_network, new NodeTag(2, 1), null, TransmitOptions.TransmitOptionAcknowledge);

            Assert.IsNull(new MultiChannelManager(_network).SubstituteActionInternal(action));
            Assert.IsNull(action.Data);
        }
    }
}
