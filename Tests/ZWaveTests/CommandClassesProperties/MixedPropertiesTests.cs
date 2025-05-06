using System.Linq;
using NUnit.Framework;
using ZWave.CommandClasses;

namespace ZWaveTests.CommandClassesProperties
{
    /* - Double Convert - */
    /*
     * *special cases, where one value refers to another*
     * COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION.MULTI_CHANNEL_ASSOCIATION_REMOVE:nodeId //prm.SizeReference == MsgMarker -> {marker} - private
     * COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.COMMAND_CONFIGURATION_REPORT:commandByte //prm.SizeReference == MsgLength ->to the frame end
     * COMMAND_CLASS_ASSOCIATION.ASSOCIATION_REMOVE:nodeId //prm.SizeReference == MsgLength
     * COMMAND_CLASS_METER.METER_REPORT:previousMeterValue DownGrade and reverse
     * COMMAND_CLASS_ZIP_GATEWAY.COMMAND_APPLICATION_NODE_INFO_SET:securityScheme0Mark
     * 
     * {COMMAND_CLASS_TRANSPORT_SERVICE_V2}.{COMMAND_FIRST_SEGMENT}:"properties1" -- GenerateStructReverseImplicitOperatorCmdMask
     * {COMMAND_CLASS_USER_CODE}."EXTENDED_USER_CODE_REPORT"
     * IR_REPEATER_CAPABILITIES_GET.IR_REPEATER_CONFIGURATION_SET !!!!! 
     */

    //* IR_REPEATER_CAPABILITIES_GET.IR_REPEATER_CONFIGURATION_SET !!!!! 


    [TestFixture]
    public class MixedPropertiesTests
    {
        [Test]
        public void SimpleProperty_AddMissingAfterCastInClass_AddedToArray()
        {
            var expected = new byte[] { 0x20, 0x03, 0x01, 0x01, 0x01 };
            var expectedM = expected.Take(3).ToArray();
            var arrange = (COMMAND_CLASS_BASIC_V2.BASIC_REPORT)expectedM;

            var actM = (byte[])arrange;
            Assert.AreEqual(expectedM, expectedM);

            arrange.duration = 0x01;
            arrange.targetValue = 0x01;
            var act = (byte[])arrange;

            //arr
            Assert.AreEqual(expected, act);
        }

        [Test]
        public void ReferencedTypeMsgMarker_MarkerExists_ArrayMatched()
        {
            byte correctMarker = 0x00;
            var expected = new byte[] { 0x8E, 0x04, 0x01, 0x02, 0x03, correctMarker, 0x01, 0x02 };
            var arrange = (COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V2.MULTI_CHANNEL_ASSOCIATION_REMOVE)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReferencedTypeMsgMarker_MarkerMissed_NotFullArrayMatched()
        {
            byte wrongMarker = 0xFF;
            var expected = new byte[] { 0x8E, 0x04, 0x01, 0x02, wrongMarker, wrongMarker };
            var arrange = (COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V2.MULTI_CHANNEL_ASSOCIATION_REMOVE)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReferencedTypeMsgLength_CorrectVal_FullArrayMatched()
        {
            var expected = new byte[] { 0x9B, 0x05, 0x01, 0x02, 0x80, 0x03, 0x20, 0x01, 0x01 };
            var arange = (COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.COMMAND_CONFIGURATION_REPORT)expected;

            var actual = (byte[])arange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReferencedTypeMsgLength_WrongVal_NotFullArrayMatched()
        {
            var expected = new byte[] { 0x9B, 0x05, 0x01, 0x02, 0x80, /*0x03,*/ 0x20, 0x01, 0x01 }; //missed {commandLength}
            var arange = (COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.COMMAND_CONFIGURATION_REPORT)expected;

            var actual = (byte[])arange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReferencedTypeProperties1_WrongVal_MatchedArray()
        {
            byte wrongVal = 0x02;
            var expected = new byte[] { 0x71, 0x08, wrongVal, 0x04 /*missed*/ }; //count < wrongVal
            var arrange = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReferencedTypeProperties2_HigherVersionCC_CorrectVal()
        {
            var expected = new byte[] { 0x32, 0x02, 0x01, 0x01, 0xAA/*, 0x00, 0x02, 0xBB*/ };

            var arrange_v2 = (COMMAND_CLASS_METER_V2.METER_REPORT)expected;
            var arrange_v1 = (COMMAND_CLASS_METER.METER_REPORT)expected;

            var actual_v2 = (byte[])arrange_v2;
            var actual_v1 = (byte[])arrange_v1;

            Assert.AreEqual(expected, actual_v2);
            Assert.AreEqual(expected, actual_v1);
        }

        [Test]
        public void ReferencedTypeProperties2_LowerVersionCC_CorrectVal()
        {
            var expected = new byte[] { 0x32, 0x02, 0x21, 0x29, 0xAA, 0x00, 0x02, 0xBB };
            // previousMeterValue

            var arrange_v2 = (COMMAND_CLASS_METER_V2.METER_REPORT)expected;
            var arrange_v1 = (COMMAND_CLASS_METER.METER_REPORT)expected;

            var actual_v2 = (byte[])arrange_v2;
            var actual_v1 = (byte[])arrange_v1;

            Assert.AreEqual(expected, actual_v2);
            Assert.AreEqual(expected.Take(5).ToArray(), actual_v1);
        }

        [Test]
        public void BytesMarkIList_InArray_CorrectVal()
        {
            var expected = new byte[] { 0x5F, 0x0B, 0x98, 0xF1, 0x00, 0x20 };
            var arrange = (COMMAND_CLASS_ZIP_GATEWAY.COMMAND_APPLICATION_NODE_INFO_SET)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Ignore("review COMMAND_FIRST_SEGMENT.ID changes with CmdMask")]
        public void ClassIdRelatesToProperty_InArray_CorrectVal() //GenerateStructReverseImplicitOperatorCmdMask
        {
            var expected = new byte[] { 0x55, 0xC1, 0x02, 0x30, 0x01, 0x01, 0xAA, 0x00, 0x00 };
            var arrange = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT)expected;

            var actual = (byte[])arrange;
            Assert.AreEqual(expected, actual);
        }
    }
}
