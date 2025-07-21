/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using NUnit.Framework;
using ZWave.CommandClasses;

namespace ZWaveTests.CommandClassesProperties
{
    /* - Result of GenerateVgProperties in XML Editor -
     * Tests check results of GenerateVgImplicitOperator, GenerateVgReverseImplicitOperator as methods which are using generated properties
     * Terms:
     * tvg1 - Class parameter, contains set of other simple params
     * 
     * - Rules:
     * After typecasting must exist a possibility to detect was it presented value in original frame
     * Init - default behaviour on create instance of Command Class, property values MUST Be SPECIFIED and FILLED
     * IsSpecified = received - containts in array
     * Exist in array = IsSpecified or Assigned (refTypes)
     * Convert byte array to struct and visa-versa MUST match
     * 
     * - Commands:fields under test:
     * COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT
     * 
     */

    [TestFixture]
    public class VgPropertiesTests
    {
        [Test]
        public void TVGInClass_Default_Init_IsSpecifiedTrue()
        {
            var act = new COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT();
            Assert.IsNotNull(act.vg1);
            Assert.IsEmpty(act.vg1);
        }

        [Test]
        public void TVGInClass_Empty_CastToArray_IsSpecifiedFalse()
        {
            var expected = new byte[] { 0x59, 0x04, 0x00 };
            var arr = new COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT();

            var actual = (byte[])arr;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TVGInClass_Exists_CastToArray_IsSpecifiedTrue()
        {
            var expected = new byte[] { 0x59, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02 };
            var arr = new COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT
            {
                vg1 = new List<COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT.TVG1>()
            };
            arr.vg1.Add(new COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT.TVG1());
            arr.vg1[0].eventCode = new byte[] { 0, 2 };

            var actual = (byte[])arr;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TVGInArray_Empty_CastToClass_IsSpecifiedFalse()
        {
            var arr = new byte[] { 0x59, 0x04, 0x00 };

            var actual = (COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT)arr;

            Assert.IsEmpty(actual.vg1);
        }

        [Test]
        public void TVGInArray_Exists_CastToClass_IsSpecifiedTrue()
        {
            var expected = new byte[] { 0x59, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02 };
            var arr = new COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT
            {
                vg1 = new List<COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT.TVG1>()
            };
            arr.vg1.Add(new COMMAND_CLASS_ASSOCIATION_GRP_INFO.ASSOCIATION_GROUP_INFO_REPORT.TVG1());
            arr.vg1[0].eventCode = new byte[] { 0, 2 };

            var actual = (byte[])arr;

            Assert.AreEqual(expected, actual);
        }

    }
}
