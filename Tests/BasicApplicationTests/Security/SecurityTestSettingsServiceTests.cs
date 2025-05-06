using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Security;
using ZWave.Devices;
using ZWave.Enums;

namespace BasicApplicationTests.Security
{
    //[TestFixture]
    class SecurityTestSettingsServiceTests
    {
        SecurityManagerInfo _securityManagerInfo;
        SecurityTestSettingsService _securityTestSettingsService;

        public object SecurityScheme { get; private set; }

        [SetUp]
        public void Setup()
        {
            _securityManagerInfo = new SecurityManagerInfo(new ZWave.Devices.NetworkViewPoint(null), new ZWave.Configuration.NetworkKey[0], new byte[0]);

            //networkKeys, new byte[]
            //    {
            //        0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
            //        0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
            //    }); ;

            _securityTestSettingsService = new SecurityTestSettingsService(_securityManagerInfo, false);
        }

        [Test]
        public void Activate_RequestDataOperation_DataChanged()
        {
            //arr
            var startValue = new byte[1].Select(x => x = 0x01).ToArray();
            var endValue = new byte[2].Select(x => x = 0x02).ToArray();
            _securityManagerInfo.SetTestFrameCommandS2(SecurityS2TestFrames.MessageEncapsulation, endValue);
            var operation = new RequestDataOperation(new NetworkViewPoint(), new NodeTag(0x01), new NodeTag(0x02), startValue, TransmitOptions.TransmitOptionAcknowledge, 1);
            operation.Data = startValue;
            //act
            _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.MessageEncapsulation, operation);
            //ass
            Assert.AreEqual(operation.Data, endValue);
        }

        [Test]
        public void Activate_SendDataOperation_DataChanged()
        {
            //arr
            var startValue = new byte[1].Select(x => x = 0x01).ToArray();
            var endValue = new byte[2].Select(x => x = 0x02).ToArray();
            _securityManagerInfo.SetTestFrameCommandS2(SecurityS2TestFrames.MessageEncapsulation, endValue);
            var operation = new SendDataOperation(new NetworkViewPoint(), new NodeTag(0x01), startValue, TransmitOptions.TransmitOptionAcknowledge);
            operation.Data = startValue;
            //act
            _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.MessageEncapsulation, operation);
            //ass
            Assert.AreEqual(operation.Data, endValue);
        }

        [Test]
        public void Activate_SendDataExOperation_DataChanged()
        {
            //arr
            var startValue = new byte[1].Select(x => x = 0x01).ToArray();
            var endValue = new byte[2].Select(x => x = 0x02).ToArray();
            _securityManagerInfo.SetTestFrameCommandS2(SecurityS2TestFrames.MessageEncapsulation, endValue);
            var operation = new SendDataExOperation(new NetworkViewPoint(), new NodeTag(0x01), startValue, TransmitOptions.TransmitOptionAcknowledge, SecuritySchemes.NONE);
            operation.Data = startValue;
            //act
            _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.MessageEncapsulation, operation);
            //ass
            Assert.AreEqual(operation.Data, endValue);
        }

        [Test]
        public void Activate_SendDataBridgeOperation_DataChanged()
        {
            //arr
            var startValue = new byte[1].Select(x => x = 0x01).ToArray();
            var endValue = new byte[2].Select(x => x = 0x02).ToArray();
            _securityManagerInfo.SetTestFrameCommandS2(SecurityS2TestFrames.MessageEncapsulation, endValue);
            var operation = new SendDataBridgeOperation(new NetworkViewPoint(), new NodeTag(0x01), new NodeTag(0x02), startValue, TransmitOptions.TransmitOptionAcknowledge);
            operation.Data = startValue;
            //act
            _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.MessageEncapsulation, operation);
            //ass
            Assert.AreEqual(operation.Data, endValue);
        }

        //TODO: implement all tests..
    }
}
