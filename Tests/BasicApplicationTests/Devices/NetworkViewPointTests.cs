// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Silicon Laboratories Inc. <https://www.silabs.com>
using NUnit.Framework;
using ZWave.Enums;
using ZWave.Devices;

namespace BasicApplicationTests.Devices
{
    [TestFixture]
    public class NetworkViewPointTests
    {
        [Test]
        public void NetworkSetNodeIncluded_IsNodeIncluded()
        {
            //Arrange
            var network = new NetworkViewPoint(null);

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
                if (node == network.NodeTag)
                {
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
                else
                {
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        [Test]
        public void NetworkSetAllIncluded_IsAllIncluded()
        {
            //Arrange
            var network = new NetworkViewPoint(null);

            //Act
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                byte nodeId = (byte)i;
            }

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
                if (node == network.NodeTag)
                {
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
                else
                {
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        [Test]
        public void NetworkSetAllIncludedAndClear_IsAllNotIncluded()
        {
            //Arrange
            var network = new NetworkViewPoint(null);

            //Act
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                byte nodeId = (byte)i;
            }
            network.ResetAndEnableAndSelfRestore();

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
                if (node == network.NodeTag)
                {
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
                else
                {
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        [Test]
        public void NetworkSetAllSecureS0_IsAllSecureS0()
        {
            //Arrange
            var network = new NetworkViewPoint(null);
            network.IsEnabledS0 = true;

            //Act
            //for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            //{
            var node = NodeTag.Empty;
            network.SetSecuritySchemes(node, SecuritySchemeSet.S0);
            //}

            //Assert
            //for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            //{
            //byte nodeId = (byte)i;
            Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S0));
            Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALLS2));
            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S0));
            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
            //}
        }

        [Test]
        public void NetworkSetAllSecureS0AndClear_IsAllNotSecureS0()
        {
            //Arrange
            var network = new NetworkViewPoint(null);

            //Act
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                network.SetSecuritySchemes(node, SecuritySchemeSet.S0);
            }
            network.ResetAndEnableAndSelfRestore();

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
                if (node == network.NodeTag)
                {
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S0));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALLS2));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
                else
                {
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S0));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALLS2));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        [Test]
        public void NetworkSetAllSecureS2_IsAllSecureS2()
        {
            //Arrange
            var network = new NetworkViewPoint(null);
            SecuritySchemes[] schemes = new SecuritySchemes[] { SecuritySchemes.S0, SecuritySchemes.S2_ACCESS };
            network.IsEnabledS2_ACCESS = true;
            network.IsEnabledS0 = true;

            //Act
            //for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            //{
            var node = NodeTag.Empty;
            network.SetSecuritySchemes(node, schemes);
            //}

            //Assert
            //for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            //{
            //    byte nodeId = (byte)i;
            Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S0));
            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALLS2));
            Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S2_UNAUTHENTICATED));
            Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S2_AUTHENTICATED));
            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S2_ACCESS));



            Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
            // }
        }


        [Test]
        public void NetworkSetAllSecureS2AndClear_IsAllNotSecureS2()
        {
            //Arrange
            var network = new NetworkViewPoint(null);
            SecuritySchemes[] schemes = new SecuritySchemes[] { SecuritySchemes.S0, SecuritySchemes.S2_ACCESS };

            //Act
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                network.SetSecuritySchemes(node, schemes);
            }
            network.ResetAndEnableAndSelfRestore();

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
                if (node == network.NodeTag)
                {
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S0));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALLS2));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S2_UNAUTHENTICATED));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S2_AUTHENTICATED));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S2_ACCESS));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemes.S0));
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
                else
                {
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S0));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALLS2));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S2_UNAUTHENTICATED));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S2_AUTHENTICATED));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S2_ACCESS));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemes.S0));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        [Test]
        public void NetworkSetAllSecuritySpecified_IsAllSecuritySpecified()
        {
            //Arrange
            var network = new NetworkViewPoint(null);

            //Act
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                network.SetSecuritySchemesSpecified(node);
            }

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                Assert.IsTrue(network.IsSecuritySchemesSpecified(node));
                if (node == network.NodeTag)
                {
                    Assert.IsTrue(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
                else
                {
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        [Test]
        public void NetworkSetAllSecuritySpecifiedAndClear_IsAllNotSecuritySpecified()
        {
            //Arrange
            var network = new NetworkViewPoint(null);
            network.NodeTag = new NodeTag(33);
            //Act
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                network.SetSecuritySchemesSpecified(node);
            }
            network.ResetAndEnableAndSelfRestore();

            //Assert
            for (int i = 0; i < NetworkViewPoint.MAX_NODES; i++)
            {
                var node = new NodeTag((byte)i);
                if (node != network.NodeTag)
                {
                    Assert.IsFalse(network.IsSecuritySchemesSpecified(node));
                    Assert.IsFalse(network.HasSecurityScheme(node, SecuritySchemeSet.ALL));
                }
            }
        }

        // Two Command Classes the NetworkViewPoint treats as supported non-securely (Z-Wave Plus Info, Security 2) and two it
        // treats as supported securely (Version, Association), so the NIF derivation has something to keep and something to drop.
        private static readonly byte[] CONFIGURED_COMMAND_CLASSES =
        {
            ZWave.CommandClasses.COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID,
            ZWave.CommandClasses.COMMAND_CLASS_SECURITY_2.ID,
            ZWave.CommandClasses.COMMAND_CLASS_VERSION.ID,
            ZWave.CommandClasses.COMMAND_CLASS_ASSOCIATION.ID
        };

        private static readonly byte[] NON_SECURE_COMMAND_CLASSES =
        {
            ZWave.CommandClasses.COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID,
            ZWave.CommandClasses.COMMAND_CLASS_SECURITY_2.ID
        };

        private static readonly byte[] SECURE_COMMAND_CLASSES =
        {
            ZWave.CommandClasses.COMMAND_CLASS_VERSION.ID,
            ZWave.CommandClasses.COMMAND_CLASS_ASSOCIATION.ID
        };

        private static NetworkViewPoint CreateIncludedNetwork()
        {
            var network = new NetworkViewPoint(null);
            network.NodeTag = new NodeTag(1);
            network.SetCommandClasses(CONFIGURED_COMMAND_CLASSES);
            return network;
        }

        [Test]
        public void NifCommandClasses_NotIncluded_AdvertisesAllUnlessAccessEndDevice()
        {
            var network = CreateIncludedNetwork();

            network.SetNifInclusionState(InclusionStates.NotIncluded, isAccessEndDevice: false);
            Assert.AreEqual(CONFIGURED_COMMAND_CLASSES, network.GetNifCommandClasses());

            // An Access Control end device leaves its securely supported Command Classes out before its inclusion as well,
            // like the End Device Serial API, which uses one list when excluded and when included without keys.
            network.SetNifInclusionState(InclusionStates.NotIncluded, isAccessEndDevice: true);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());
        }

        [Test]
        public void NifCommandClasses_IncludedSecurely_AdvertisesNonSecureOnly()
        {
            var network = CreateIncludedNetwork();

            network.SetNifInclusionState(InclusionStates.IncludedSecurely, isAccessEndDevice: false);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());

            network.SetNifInclusionState(InclusionStates.IncludedSecurely, isAccessEndDevice: true);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());
        }

        [Test]
        public void NifCommandClasses_IncludedNonSecurely_AdvertisesAllUnlessAccessEndDevice()
        {
            var network = CreateIncludedNetwork();

            network.SetNifInclusionState(InclusionStates.IncludedNonSecurely, isAccessEndDevice: false);
            Assert.AreEqual(CONFIGURED_COMMAND_CLASSES, network.GetNifCommandClasses());

            network.SetNifInclusionState(InclusionStates.IncludedNonSecurely, isAccessEndDevice: true);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());
        }

        [Test]
        public void NifCommandClasses_ForAGivenState_IgnoreTheCurrentState()
        {
            // The End Device Serial API keeps the list for the time without keys next to the lists for the time with keys, so
            // the list of a state is available while the own state is another one.
            var network = CreateIncludedNetwork();

            network.SetNifInclusionState(InclusionStates.IncludedSecurely, isAccessEndDevice: false);
            Assert.AreEqual(CONFIGURED_COMMAND_CLASSES, network.GetNifCommandClasses(InclusionStates.IncludedNonSecurely));
            Assert.AreEqual(CONFIGURED_COMMAND_CLASSES, network.GetNifCommandClasses(InclusionStates.NotIncluded));
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());

            network.SetNifInclusionState(InclusionStates.IncludedNonSecurely, isAccessEndDevice: true);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses(InclusionStates.NotIncluded));
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses(InclusionStates.IncludedSecurely));
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());
        }

        [Test]
        public void NifCommandClasses_DefaultState_IsNotIncluded()
        {
            var network = CreateIncludedNetwork();

            Assert.AreEqual(InclusionStates.NotIncluded, network.NifInclusionState);
            Assert.AreEqual(CONFIGURED_COMMAND_CLASSES, network.GetNifCommandClasses());
        }

        [Test]
        public void SetNifInclusionStateByKeys_FollowsNodeIdAndKeys()
        {
            var network = new NetworkViewPoint(null);

            // No NodeId: not included.
            network.SetNifInclusionStateByKeys(isAccessEndDevice: false);
            Assert.AreEqual(InclusionStates.NotIncluded, network.NifInclusionState);

            // The NodeId setter grants the own node the enabled schemes, like a Primary Controller holding its network keys.
            network.NodeTag = new NodeTag(1);
            network.SetCommandClasses(CONFIGURED_COMMAND_CLASSES);
            network.SetNifInclusionStateByKeys(isAccessEndDevice: false);
            Assert.AreEqual(InclusionStates.IncludedSecurely, network.NifInclusionState);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());

            // A failed bootstrapping drops the keys.
            network.ResetSecuritySchemes();
            network.SetNifInclusionStateByKeys(isAccessEndDevice: false);
            Assert.AreEqual(InclusionStates.IncludedNonSecurely, network.NifInclusionState);
            Assert.AreEqual(CONFIGURED_COMMAND_CLASSES, network.GetNifCommandClasses());

            network.SetNifInclusionStateByKeys(isAccessEndDevice: true);
            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNifCommandClasses());
        }

        [Test]
        public void NonSecureCommandClasses_AreConfiguredWithoutSecure()
        {
            var network = CreateIncludedNetwork();

            Assert.AreEqual(NON_SECURE_COMMAND_CLASSES, network.GetNonSecureCommandClasses());
            Assert.AreEqual(SECURE_COMMAND_CLASSES, network.GetSecureCommandClasses());
        }
    }
}
