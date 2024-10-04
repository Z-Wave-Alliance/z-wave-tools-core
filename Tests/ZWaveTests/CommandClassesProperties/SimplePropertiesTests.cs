/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using NUnit.Framework;
using ZWave.CommandClasses;

namespace ZWaveTests.CommandClassesProperties
{
    /* - Result of GenerateSimpleProperty in XML Editor -
     * Tests check results of GenerateSimpleImplicitOperator, GenerateSimpleReverseImplicitOperator as methods which are using generated properties
     * Terms:
     * Byte - SimpleProperty Param, HEX value size/bytesCount = 1 Byte / public byte {0}; /
     * Bytes - SimpleProperty Param, HEX value size/bytesCount > 1 Byte / public byte[] {0} = new byte[{1}]; / elements like integer
     * IListByte - SizeReference Param, HEX TEXT / public IList<byte> {0} = new List<byte>(); / ALARM_REPORT.Event_paramet
     * 
     * - Rules:
     * After typecasting must exist a possibility to detect was it presented value in original frame
     * Init - default behaviour on create instance of Command Class, property values MUST Be SPECIFIED and FILLED
     * IsSpecified = received - containts in array
     * Exist in array = IsSpecified or Assigned (refTypes)
     * Convert byte array to struct and visa-versa MUST match
     * 
     * ? param count in array less than param num - IsParamSpecified = true
     * 
     * - Commands:fields under test:
     * COMMAND_CLASS_ALARM.ALARM_GET:alarmType
     * COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET:parameterNumber
     * COMMAND_CLASS_ALARM_V2.ALARM_REPORT:eventParameter
     * - Not Tested:
     * public List<byte[]> {0} = new List<byte[]>() - not found cc with this property
     *
     */

    [TestFixture]
    public class SimplePropertiesTests
    {
        [Test]
        public void ByteInClass_Default_Init_IsSpecifiedTrue()
        {
            var act = new COMMAND_CLASS_ALARM.ALARM_GET();
            Assert.IsTrue(act.alarmType.HasValue);
            Assert.IsTrue(0 == act.alarmType);
        }

        [Test]
        public void ByteInArray_Exists_CastToClass_IsSpecifiedTrue()
        {
            byte exp = 0xF0;
            var arr = new byte[] { 0x86, 0x12, exp };

            var act = (COMMAND_CLASS_ALARM.ALARM_GET)arr;

            Assert.IsTrue(act.alarmType.HasValue);
            Assert.IsTrue(exp == act.alarmType);
        }

        [Test]
        public void ByteInArray_Absent_CastToClass_IsSpecifiedFalse()
        {
            var arr = new byte[] { 0x86, 0x12, /* unexp */ };

            var act = (COMMAND_CLASS_ALARM.ALARM_GET)arr;

            Assert.IsFalse(act.alarmType.HasValue);
            Assert.IsTrue(0 == act.alarmType);
        }
        [Test]
        public void ByteInArray_AbsentNextField_CastToClass_IsSpecifiedTrue()
        {
            byte exp = 0x61;
            var arr = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, exp, /*unexp*/ };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.IsTrue(act.zwaveAlarmEvent.HasValue);
            Assert.IsTrue(exp == act.zwaveAlarmEvent);
            Assert.IsFalse(act.numberOfEventParameters.HasValue); //next value in array
            Assert.IsTrue(0 == act.numberOfEventParameters); //next value in array
        }

        [Test]
        public void ByteInClass_Exists_CastToArray_IsSpecifiedTrue()
        {
            byte expVal = 0x25;
            var exp = new byte[] { 0x71, 0x04, expVal };
            var arr = new COMMAND_CLASS_ALARM.ALARM_GET() { alarmType = expVal };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ByteInClass_Null_CastToArray_IsSpecifiedFalse()
        {
            var exp = new byte[] { 0x71, 0x04 };
            var arr = new COMMAND_CLASS_ALARM.ALARM_GET
            {
                alarmType = ByteValue.Empty
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        /* - Bytes - */

        [Test]
        public void BytesInClass_Default_Init_IsSpecifiedTrue()
        {
            var act = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET();
            Assert.IsNotNull(act.parameterNumber);
            Assert.AreEqual(new byte[2], act.parameterNumber);
        }

        [Test]
        public void BytesInArray_Exists_CastToClass_IsSpecifiedTrue()
        {
            byte exp1 = 0xaa;
            byte exp2 = 0xbb;
            var arr = new byte[] { 0x70, 0x0A, exp1, exp2 };

            var act = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)arr;

            Assert.IsNotNull(act.parameterNumber);
            Assert.IsNotEmpty(act.parameterNumber);
            Assert.AreEqual(exp1, act.parameterNumber[0]);
            Assert.AreEqual(exp2, act.parameterNumber[1]);
        }

        [Test]
        public void BytesInArray_Absent_CastToClass_IsSpecifiedFalse()
        {
            var arr = new byte[] { 0x70, 0x0A };

            var act = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)arr;

            Assert.IsEmpty(act.parameterNumber);
        }

        [Test]
        public void BytesInArray_Exists1b_CastToClass_IsSpecifiedTrue()
        {
            byte exp1 = 0xAA;
            var arr = new byte[] { 0x70, 0x0A, exp1/*, missed*/ };

            var act = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)arr;

            Assert.NotNull(act.parameterNumber);
            Assert.AreEqual(1, act.parameterNumber.Length);
            Assert.AreEqual(exp1, act.parameterNumber[0]);
        }

        [Test]
        public void BytesInClass_Exists_CastToArray_IsSpecifiedTrue()
        {
            byte expValue1 = 0x1F;
            byte expValue2 = 0x0B;
            var exp = new byte[] { 0x70, 0x0A, expValue1, expValue2 };
            var arr = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET
            {
                parameterNumber = new byte[] { expValue1, expValue2 }
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BytesInClass_1b_CastToArray_IsSpecifiedTrue()
        {
            byte expValue1b = 0xAA;
            var exp = new byte[] { 0x70, 0x0A, expValue1b, /*missed*/ };
            var arr = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET
            {
                parameterNumber = new byte[] { expValue1b }
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BytesInClass_Emtpy_CastToArray_IsSpecifiedFalse()
        {
            var exp = new byte[] { 0x70, 0x0A };
            var arr = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET
            {
                parameterNumber = new byte[0]
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BytesInClass_False_CastToArray_IsSpecifiedFalse()
        {
            var exp = new byte[] { 0x70, 0x0A };
            var arr = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET
            {
                parameterNumber = new byte[10]
            };
            arr.parameterNumber = null;

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BytesInClass_Null_CastToArray_IsSpecifiedFalse()
        {
            var exp = new byte[] { 0x70, 0x0A };
            var arr = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET
            {
                parameterNumber = null
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        /* - IListByte - */

        [Test]
        public void IListByteInClass_Default_Init_IsSpecifiedTrue()
        {
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT();

            Assert.IsNotNull(arr.eventParameter);
            Assert.IsEmpty(arr.eventParameter);
        }

        [Test]
        public void IListByteInArray_Exists_CastToClass_IsSpecifiedTrue()
        {
            byte exp = 0x61;
            byte expNumCount = 0x02;
            var arr = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, expNumCount, exp, exp };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.AreEqual(exp, act.eventParameter[0]);
            Assert.AreEqual(exp, act.eventParameter[1]);
        }

        [Test]
        public void IListByteInArray_ExistsLessNum_CastToClass_IsSpecifiedTrue()
        {
            byte exp = 0x61;
            byte expNumCount = 0x02;
            var arr = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, expNumCount, exp, /*exp*/ };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.IsNotNull(act.eventParameter);
            Assert.AreNotEqual(expNumCount, act.eventParameter.Count);
            Assert.AreEqual(exp, act.eventParameter[0]);
        }

        [Test]
        public void IListByteInArray_Absent_CastToClass_IsSpecifiedFalse()
        {
            byte expNumCount = 0x02;
            var arr = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, expNumCount, /*exp, exp*/ };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.IsTrue(expNumCount == act.numberOfEventParameters);
            Assert.IsEmpty(act.eventParameter);
        }

        [Test]
        public void IListByteInArray_AbsentNumber_CastToClass_IsSpecifiedFalse()
        {
            var arr = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, /*expNumCount, exp, exp*/ };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.IsFalse(act.numberOfEventParameters.HasValue);
            Assert.IsEmpty(act.eventParameter);
        }

        [Test]
        public void IListByteInClass_Exists_CastToArray_IsSpecifiedTrue()
        {
            byte expNum = 2;
            byte expValue1 = 0x0F;
            byte expValue2 = 0xA0;
            var exp = new byte[] { 0x71, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, expNum, expValue1, expValue2 };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT
            {
                numberOfEventParameters = expNum,
                eventParameter = new List<byte>()
            };
            arr.eventParameter.Add(expValue1);
            arr.eventParameter.Add(expValue2);

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IListByteInClass_Exists_CastToArray_IsSpecifiedFalse()
        {
            byte expNum = 2;
            byte expValue1 = 0x0F;
            byte expValue2 = 0xA0;
            var exp = new byte[] { 0x71, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, expNum, /*expValue1, expValue2*/ };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT
            {
                numberOfEventParameters = expNum,
                eventParameter = new List<byte>()
            };
            arr.eventParameter.Add(expValue1);
            arr.eventParameter.Add(expValue2);
            arr.eventParameter = null;

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IListByteInClass_Empty_CastToArray_IsSpecifiedFalse()
        {
            byte expNum = 2;
            var exp = new byte[] { 0x71, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, expNum };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT
            {
                numberOfEventParameters = expNum,
                eventParameter = new List<byte>()
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IListByteInClass_False_CastToArray_IsSpecifiedFalse()
        {
            byte expNum = 2;
            var exp = new byte[] { 0x71, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, expNum };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT
            {
                numberOfEventParameters = expNum,
                eventParameter = null
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IListByteInClass_Null_CastToArray_IsSpecifiedFalse()
        {
            byte expNum = 2;
            var exp = new byte[] { 0x71, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, expNum };
            var arr = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT
            {
                numberOfEventParameters = expNum,
                eventParameter = null
            };

            var act = (byte[])arr;

            Assert.AreEqual(exp, act);
        }

        //////////////////////
        /* - Double Convert - */
        //////////////////////

        [Test]
        public void X2Convert_ByteSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_ALARM.ALARM_GET()
            {
                alarmType = 88
            };
            var arrange = (byte[])expected;

            var actual = (COMMAND_CLASS_ALARM.ALARM_GET)arrange;

            Assert.IsNotNull(actual.alarmType);
            Assert.AreEqual(expected.alarmType, actual.alarmType);
        }

        [Test]
        public void X2Convert_ByteNotSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_ALARM.ALARM_GET()
            {
                alarmType = ByteValue.Empty
            };
            var arrange = (byte[])expected;

            var actual = (COMMAND_CLASS_ALARM.ALARM_GET)arrange;

            Assert.IsFalse(actual.alarmType.HasValue);
            Assert.IsTrue(0 == actual.alarmType);
            Assert.IsTrue(expected.alarmType == actual.alarmType);
        }

        [Test]
        public void X2Convert_ByteSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x71, 0x04, 0x02 };
            var arrange = (COMMAND_CLASS_ALARM.ALARM_GET)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void X2Convert_ByteNotSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x71, 0x04 };
            var arrange = (COMMAND_CLASS_ALARM.ALARM_GET)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void X2Convert_BytesSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET()
            {
                parameterNumber = new byte[] { 1, 2 }
            };
            var arrange = (byte[])expected;

            var actual = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)arrange;

            Assert.NotNull(actual.parameterNumber);
            Assert.AreEqual(expected.parameterNumber, actual.parameterNumber);
        }

        [Test]
        public void X2Convert_BytesNotSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET()
            {
                parameterNumber = null
            };
            var arrange = (byte[])expected;

            var actual = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)arrange;

            Assert.IsEmpty(actual.parameterNumber);
        }

        [Test]
        public void X2Convert_BytesSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x70, 0x0A, 0x00, 0x0C };
            var arrange = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void X2Convert_BytesNotSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x70, 0x0A };
            var arrange = (COMMAND_CLASS_CONFIGURATION_V4.CONFIGURATION_NAME_GET)expected;

            var actual = (byte[])arrange;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void X2Convert_IListSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT();
            expected.eventParameter.Add(0xAA);
            expected.numberOfEventParameters = (byte)expected.eventParameter.Count;

            var arr = (byte[])expected;
            var actual = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.IsNotNull(actual.eventParameter);
            Assert.IsNotEmpty(actual.eventParameter);
            Assert.AreEqual(expected.eventParameter, actual.eventParameter);
        }

        [Test]
        public void X2Convert_IListNotSpecified_MatchedType()
        {
            var expected = new COMMAND_CLASS_ALARM_V2.ALARM_REPORT();
            expected.eventParameter.Add(0xAA);
            expected.eventParameter = null;

            var arr = (byte[])expected;
            var actual = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)arr;

            Assert.IsEmpty(actual.eventParameter);
        }

        [Test]
        public void X2Convert_IListSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x02, 0xAA, 0xBB };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)expected;
            var actual = (byte[])act;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void X2Convert_IListNotSpecified_MatchedArray()
        {
            var expected = new byte[] { 0x71, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x10 };

            var act = (COMMAND_CLASS_ALARM_V2.ALARM_REPORT)expected;
            var actual = (byte[])act;

            Assert.AreEqual(expected, actual);
        }

    }
}