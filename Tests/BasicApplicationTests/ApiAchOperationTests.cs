// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using NUnit.Framework;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace BasicApplicationTests
{
    [TestFixture]
    public class ApiAchOperationTests
    {
        private class HandlingGate : ApiAchOperation
        {
            private readonly NetworkViewPoint _gateNetwork;

            public HandlingGate(NetworkViewPoint network)
                : base(network, NodeTag.Empty, NodeTag.Empty)
            {
                _gateNetwork = network;
            }

            public bool Allows(byte[] command, SecuritySchemes scheme)
            {
                return IsHandlingAllowed(_gateNetwork, command, scheme);
            }
        }

        private static readonly byte[] VERSION_GET = new COMMAND_CLASS_VERSION.VERSION_GET();
        private static readonly byte[] ZWAVEPLUS_INFO_GET = new COMMAND_CLASS_ZWAVEPLUS_INFO.ZWAVEPLUS_INFO_GET();
        private static readonly byte[] BASIC_GET = new COMMAND_CLASS_BASIC.BASIC_GET();

        private static NetworkViewPoint CreateNetwork()
        {
            // The NodeId setter grants the own node the enabled schemes, so its highest Security Class is S2 Access Control.
            var network = new NetworkViewPoint(null);
            network.NodeTag = new NodeTag(1);
            network.SetCommandClasses(new byte[] { COMMAND_CLASS_ZWAVEPLUS_INFO.ID, COMMAND_CLASS_VERSION.ID });
            network.SetNifInclusionStateByKeys(isAccessEndDevice: false);
            return network;
        }

        [Test]
        public void IsHandlingAllowed_SecurelyIncluded_HandlesNonNifCommandClassesAtTheHighestClassOnly()
        {
            var gate = new HandlingGate(CreateNetwork());

            // Version is securely supported and so not in the NIF.
            Assert.IsTrue(gate.Allows(VERSION_GET, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(gate.Allows(VERSION_GET, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsFalse(gate.Allows(VERSION_GET, SecuritySchemes.S2_UNAUTHENTICATED));
            Assert.IsFalse(gate.Allows(VERSION_GET, SecuritySchemes.S0));
            Assert.IsFalse(gate.Allows(VERSION_GET, SecuritySchemes.NONE));

            // Z-Wave Plus Info is always in the NIF and works at any class.
            Assert.IsTrue(gate.Allows(ZWAVEPLUS_INFO_GET, SecuritySchemes.S2_ACCESS));
            Assert.IsTrue(gate.Allows(ZWAVEPLUS_INFO_GET, SecuritySchemes.S2_UNAUTHENTICATED));
            Assert.IsTrue(gate.Allows(ZWAVEPLUS_INFO_GET, SecuritySchemes.NONE));

            // Basic is in no list and still works only at the highest class.
            Assert.IsTrue(gate.Allows(BASIC_GET, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(gate.Allows(BASIC_GET, SecuritySchemes.S2_UNAUTHENTICATED));
            Assert.IsFalse(gate.Allows(BASIC_GET, SecuritySchemes.NONE));
        }

        [Test]
        public void IsHandlingAllowed_WithoutKeys_HandlesEveryCommandClassNonSecurely()
        {
            var network = CreateNetwork();
            network.ResetSecuritySchemes();
            network.SetNifInclusionStateByKeys(isAccessEndDevice: false);
            var gate = new HandlingGate(network);

            Assert.IsTrue(gate.Allows(VERSION_GET, SecuritySchemes.NONE));
            Assert.IsTrue(gate.Allows(ZWAVEPLUS_INFO_GET, SecuritySchemes.NONE));
            Assert.IsTrue(gate.Allows(BASIC_GET, SecuritySchemes.NONE));
        }

        [Test]
        public void IsHandlingAllowed_EmptyCommand_IsRefused()
        {
            var gate = new HandlingGate(CreateNetwork());

            Assert.IsFalse(gate.Allows(null, SecuritySchemes.S2_ACCESS));
            Assert.IsFalse(gate.Allows(new byte[0], SecuritySchemes.S2_ACCESS));
        }
    }
}
