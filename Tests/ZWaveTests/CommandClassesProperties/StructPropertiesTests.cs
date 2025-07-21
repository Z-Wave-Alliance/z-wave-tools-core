/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using NUnit.Framework;
using ZWave.CommandClasses;

namespace ZWaveTests.CommandClassesProperties
{
    /* - Result of GenerateStructProperty in XML Editor -
     * Tests check results of GenerateStructImplicitOperator, GenerateStructReverseImplicitOperator as methods which are using generated properties
     * Terms:
     * properties1 - Structured parameter, byte contains bits values
     * bitMask (<IList>) - struct related property
     * 
     * - Rules:
     * After typecasting must exist a possibility to detect was it presented value in original frame
     * Init - default behaviour on create instance of Command Class, property values MUST Be SPECIFIED and FILLED
     * IsSpecified = received - containts in array
     * Exist in array = IsSpecified or Assigned (refTypes)
     * Convert byte array to struct and visa-versa MUST match
     * 
     * - Commands:fields under test:
     * COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT:properties1
     * COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT:bitMask
     * 
     */

    [TestFixture]
    public class StructPropertiesTests
    {

        /* properties1 */

        [Test]
        public void Properties1InClass_Default_Init_IsSpecifiedTrue()
        {
            var act = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT();
            Assert.NotNull(act.properties1);
        }

        [Test]
        public void Properties1InArray_Exists_CastToClass_IsSpecifiedTrue()
        {
            byte exp = 0x83; //3,0, true
            var arr = new byte[] { 0x71, 0x08, exp, 0x28 };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)arr;

            Assert.NotNull(act.properties1);
            Assert.AreEqual(3, act.properties1.numberOfBitMasks);
            Assert.AreEqual(1, act.properties1.v1Alarm);
        }

        [Test]
        public void Properties1InArray_Absent_CastToClass_IsSpecifiedFalse()
        {
            var arr = new byte[] { 0x71, 0x08 };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)arr;

            Assert.IsFalse(act.properties1.HasValue);
            Assert.IsTrue(0 == act.properties1);
        }

        [Test]
        public void Properties1InClass_Exists_CastToArray_IsSpecifiedTrue()
        {
            var exp = new byte[] { 0x71, 0x08, 0x01, 0x08 };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT();
            arr.properties1 = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT.Tproperties1() { numberOfBitMasks = 1 };
            arr.bitMask = new List<byte>();
            arr.bitMask.Add(0x08);

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Properties1InClass_Null_CastToArray_IsSpecifiedFalse()
        {
            var exp = new byte[] { 0x71, 0x08 };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT();
            arr.properties1 = 0xaa;
            arr.properties1 = COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT.Tproperties1.Empty;

            var act = (byte[])arr;

            Assert.IsTrue(0 == arr.properties1);
            Assert.AreEqual(exp, act);
        }

        /* bitMask */

        [Test]
        public void BitMaskInArray_Exists_CastToClass_EqualnumberOfBitMasks()
        {
            byte exp = 0x01;
            var arr = new byte[] { 0x71, 0x08, exp, 0x02, 0x01 };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)arr;

            Assert.IsNotNull(act.bitMask);
            Assert.AreEqual(exp, act.properties1.numberOfBitMasks);
            Assert.AreEqual(act.properties1.numberOfBitMasks, act.bitMask.Count);
        }

        [Test]
        public void BitMaskInArray_Absent_CastToClass_IsSpecifiedFalse()
        {
            byte exp = 0x01;
            var arr = new byte[] { 0x71, 0x08, exp };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)arr;

            Assert.IsEmpty(act.bitMask);
            Assert.AreEqual(exp, act.properties1.numberOfBitMasks);
            Assert.IsTrue(0 != act.properties1);
        }

        [Test]
        public void BitMaskInClass_Exists_CastToArray_IsSpecifiedTrue()
        {
            var exp = new byte[] { 0x71, 0x08, 0x01, 0x08 };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT();
            arr.properties1 = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT.Tproperties1() { numberOfBitMasks = 1 };
            arr.bitMask = new List<byte>();
            arr.bitMask.Add(0x08);

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BitMaskInClass_Null_CastToArray_IsSpecifiedFalse()
        {
            var exp = new byte[] { 0x71, 0x08, 0x01 };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT();
            arr.properties1.numberOfBitMasks = 1;
            arr.bitMask = null;

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        ////////////////////////
        /* - Double Convert - */
        ////////////////////////

        [Test]
        public void X2Convert_Properties1Specified_MatchedType()
        {
            var expected = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT()
            {
                properties1 = 0x01,
            };
            var arrange = (byte[])expected;

            var actual = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)arrange;
            Assert.NotNull(expected.properties1);
            Assert.AreEqual(expected.properties1, actual.properties1);
        }

        [Test]
        public void X2Convert_Properies1NotSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT()
            {
                properties1 = COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT.Tproperties1.Empty,
            };
            var arrange = (byte[])expected;

            var actual = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)arrange;

            Assert.IsFalse(actual.properties1.HasValue);
            Assert.AreEqual(expected.properties1, actual.properties1);
        }

        [Test]
        public void X2Convert_Properies1Specified_MatchedArray()
        {
            var expected = new byte[] { 0x71, 0x08, 0x02, 0x0A, 0x0B };
            var arrange = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void X2Convert_Properies1NotSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x71, 0x08 };
            var arrange = (COMMAND_CLASS_ALARM_V2.ALARM_TYPE_SUPPORTED_REPORT)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

    }
}
