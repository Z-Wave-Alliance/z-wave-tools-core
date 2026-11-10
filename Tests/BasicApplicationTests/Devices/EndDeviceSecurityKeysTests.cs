// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System.Threading;
using NUnit.Framework;
using ZWave;
using ZWave.BasicApplication.Devices;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.Layers;

namespace BasicApplicationTests.Devices
{
    /// <summary>
    /// The own security schemes of a Serial API End Device follow the keys of the chip: The Learn Mode and the MemoryGetId of
    /// an End Device read them with Security Setup Get Security Keys, since the chip does the bootstrapping while the NodeTag
    /// setter grants every enabled scheme.
    /// </summary>
    [TestFixture]
    public class EndDeviceSecurityKeysTests : TestBase
    {
        private const byte KEYS_S2_AUTHENTICATED_AND_ACCESS = (byte)(NetworkKeyS2Flags.S2Class1 | NetworkKeyS2Flags.S2Class2);
        private static readonly byte[] COMMAND_CLASSES = { COMMAND_CLASS_BASIC.ID, COMMAND_CLASS_SECURITY_2.ID };

        private EndDevice _endDevice;

        [SetUp]
        public void SetUp()
        {
            _endDevice = _app.CreateEndDevice();
            _endDevice.Connect(new SerialPortDataSource("COM4"));
            _endDevice.Library = Libraries.EndDeviceLib;
            _transport.SetEndDeviceApi(_endDevice.SessionId, true);
            _endDevice.ApplicationNodeInformation(DeviceOptions.Listening, 2, 1, COMMAND_CLASSES);
            _ctrlFirst.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, COMMAND_CLASSES);
        }

        [TearDown]
        public void TearDown()
        {
            _endDevice.Dispose();
        }

        [Test]
        public void SetLearnMode_EndDeviceLibrary_OwnSchemesFollowTheChipKeys()
        {
            _transport.SetSecurityKeys(_endDevice.SessionId, KEYS_S2_AUTHENTICATED_AND_ACCESS);

            SetLearnModeResult learnRes = IncludeEndDevice();

            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.AreNotEqual(0, _endDevice.Network.NodeTag.Id);
            Assert.AreEqual(_endDevice.Network.NodeTag, learnRes.Node);
            SecuritySchemes[] expected = { SecuritySchemes.S2_ACCESS, SecuritySchemes.S2_AUTHENTICATED };
            CollectionAssert.AreEquivalent(expected, _endDevice.Network.GetSecuritySchemes());
            CollectionAssert.AreEquivalent(expected, learnRes.SecuritySchemes);
            _endDevice.Network.SetNifInclusionStateByKeys(isAccessEndDevice: true);
            Assert.AreEqual(InclusionStates.IncludedSecurely, _endDevice.Network.NifInclusionState);
        }

        [Test]
        public void SetLearnMode_NoKeysOnTheChip_OwnSchemesAreEmpty()
        {
            SetLearnModeResult learnRes = IncludeEndDevice();

            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.IsFalse(_endDevice.Network.HasSecurityScheme(SecuritySchemeSet.ALL));
            _endDevice.Network.SetNifInclusionStateByKeys(isAccessEndDevice: false);
            Assert.AreEqual(InclusionStates.IncludedNonSecurely, _endDevice.Network.NifInclusionState);
        }

        [Test]
        public void SetLearnMode_NoAnswerToTheKeyRead_CompletesWithTheLearnModeResult()
        {
            int timeoutMs = SetLearnModeWithSecurityKeysOperation.SECURITY_KEYS_TIMEOUT;
            SetLearnModeWithSecurityKeysOperation.SECURITY_KEYS_TIMEOUT = 500;
            try
            {
                _transport.SetSecurityKeysAnswered(_endDevice.SessionId, false);

                SetLearnModeResult learnRes = IncludeEndDevice();

                Assert.AreEqual(ActionStates.Completed, learnRes.State);
                Assert.AreNotEqual(0, learnRes.Node.Id);
                Assert.IsNull(learnRes.SecuritySchemes);
            }
            finally
            {
                SetLearnModeWithSecurityKeysOperation.SECURITY_KEYS_TIMEOUT = timeoutMs;
            }
        }

        [Test]
        public void SetLearnMode_SysTestLibrary_KeepsTheEnabledSchemes()
        {
            _endDevice.Library = Libraries.EndDeviceSysTestLib;

            SetLearnModeResult learnRes = IncludeEndDevice();

            Assert.AreEqual(ActionStates.Completed, learnRes.State);
            Assert.IsNull(learnRes.SecuritySchemes);
            Assert.IsTrue(_endDevice.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2));
        }

        [Test]
        public void MemoryGetId_EndDeviceLibrary_ReadsTheChipKeys()
        {
            _transport.SetSecurityKeys(_endDevice.SessionId, (byte)NetworkKeyS2Flags.S0);

            MemoryGetIdResult res = _endDevice.MemoryGetId();

            Assert.AreEqual(ActionStates.Completed, res.State);
            CollectionAssert.AreEquivalent(new[] { SecuritySchemes.S0 }, _endDevice.Network.GetSecuritySchemes());
        }

        [Test]
        public void MemoryGetIdAsync_EndDeviceLibrary_ReadsTheChipKeysBeforeTheCallback()
        {
            _transport.SetSecurityKeys(_endDevice.SessionId, (byte)NetworkKeyS2Flags.S0);
            SecuritySchemes[] schemesAtCallback = null;
            using (ManualResetEvent callbackSignal = new ManualResetEvent(false))
            {
                _endDevice.MemoryGetId(x =>
                {
                    schemesAtCallback = _endDevice.Network.GetSecuritySchemes();
                    callbackSignal.Set();
                });

                Assert.IsTrue(callbackSignal.WaitOne(EXPECT_TIMEOUT));
            }
            CollectionAssert.AreEquivalent(new[] { SecuritySchemes.S0 }, schemesAtCallback);
        }

        [Test]
        public void MemoryGetId_SysTestLibrary_KeepsTheEnabledSchemes()
        {
            _endDevice.Library = Libraries.EndDeviceSysTestLib;
            _transport.SetSecurityKeys(_endDevice.SessionId, (byte)NetworkKeyS2Flags.S0);

            _endDevice.MemoryGetId();

            Assert.IsTrue(_endDevice.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2));
        }

        private SetLearnModeResult IncludeEndDevice()
        {
            var addToken = _ctrlFirst.AddNodeToNetwork(Modes.NodeAny, INCLUSION_TIMEOUT, null);
            _ctrlFirst.WaitNodeStatusSignal(NodeStatuses.LearnReady, EXPECT_TIMEOUT);
            var learnToken = _endDevice.SetLearnMode(LearnModes.LearnModeClassic, INCLUSION_TIMEOUT, null);
            addToken.WaitCompletedSignal();
            return (SetLearnModeResult)learnToken.WaitCompletedSignal();
        }
    }
}
