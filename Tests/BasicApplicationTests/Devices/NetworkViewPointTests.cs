/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
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
    }
}
