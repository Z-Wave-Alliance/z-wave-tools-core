/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Security;
using ZWave.Xml.FrameHeader;
using ZWave.ZnifferApplication;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class S2DecodingTest
    {
        [SetUp]
        public void SetUp()
        {
            if (HeaderStore.Headers == null)
            {
                HeaderStore.Headers = new Dictionary<byte, Header>();
            }

            if (!HeaderStore.Headers.ContainsKey(HeaderStore.H_SINGLECAST))
            {
                BaseHeader baseHeader = new BaseHeader(0, "BASIC", "Basic Frame Layout",
                     new Param[] { new Param("HomeID", "Home ID", zwParamType.HEX,
                                             32),
                                   new Param("SourceNodeID", "Source Node ID",
                                             zwParamType.NODE_NUMBER),
                                   new Param("Properties1", "Properties1",
                                             new Param[] { new Param("HeaderType",
                                                                     "Header Type",
                                                                     zwParamType.HEX,
                                                                     "HeaderType",
                                                                     4),
                                                           new Param("SpeedModified",
                                                                     "Speed Modified",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("LowPower",
                                                                     "Low Power",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("Ack",
                                                                     "Ack",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("Routed",
                                                                     "Routed",
                                                                     zwParamType.BOOLEAN) }),
                                   new Param("Properties2", "Properties2",
                                             new Param[] { new Param("SequenceNumber",
                                                                     "Sequence Number",
                                                                     zwParamType.NUMBER,
                                                                     4),
                                                           new Param("Reserved2",
                                                                     "Reserved2",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("SourceWakeupBeam250ms",
                                                                     "Source Wakeup Beam 250ms",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("WakeupSourceBeam1000ms",
                                                                     "Wakeup Source Beam 1000ms",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("SUCPresent",
                                                                     "SUC Present",
                                                                     zwParamType.BOOLEAN) }),
                                   new Param("Length", "Length",
                                             zwParamType.NUMBER) },
                     new ItemReference("HomeID"),
                     new ItemReference("SourceNodeID"),
                     new ItemReference("Properties2.SequenceNumber"),
                     new ItemReference("Properties1.HeaderType"),
                     new ItemReference("Properties1.LowPower"));
                baseHeader.Initialize();


                Header singleCast = new Header(HeaderStore.H_SINGLECAST, "SINGLECAST", "Singlecast", 0, false,
                 false, false, false,
                 new Param[] { new Param("DestinationNodeID",
                                         "Destination Node ID",
                                         zwParamType.NODE_NUMBER) },
                 new Validation[] { new Validation("Properties1.HeaderType",
                                                   0x01),
                                    new Validation("Properties1.Routed",
                                                   0x00) },
                 new ComplexReference { Item = new ItemReference("DestinationNodeID") },
                 null, null);
                singleCast.Initialize(baseHeader);
                foreach (var val in singleCast.Validation)
                {
                    val.Initialize(singleCast);
                }
                HeaderStore.Headers.Add(singleCast.Key, singleCast);
            }

            if (!HeaderStore.Headers.ContainsKey(HeaderStore.H_SINGLECAST24))
            {
                BaseHeader baseHeader = new BaseHeader(1, "BASIC24", "Basic Frame Layout (24)",
                          new Param[] { new Param("HomeID", "Home ID", zwParamType.HEX,
                                             32),
                                   new Param("SourceNodeID", "Source Node ID",
                                             zwParamType.NODE_NUMBER),
                                   new Param("Properties1", "Properties1",
                                             new Param[] { new Param("HeaderType",
                                                                     "Header Type",
                                                                     zwParamType.HEX,
                                                                     "HeaderType",
                                                                     4),
                                                           new Param("SpeedModified",
                                                                     "Speed Modified",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("SUCPresent",
                                                                     "SUC Present",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("LowPower",
                                                                     "Low Power",
                                                                     zwParamType.BOOLEAN),
                                                           new Param("Ack",
                                                                     "Ack",
                                                                     zwParamType.BOOLEAN) }),
                                   new Param("Properties2", "Properties2",
                                             new Param[] { new Param("Reserved2",
                                                                     "Reserved2",
                                                                     zwParamType.NUMBER,
                                                                     4),
                                                           new Param("SourceWakeup",
                                                                     "SourceWakeup",
                                                                     zwParamType.HEX,
                                                                     3),
                                                           new Param("Extended",
                                                                     "Extended",
                                                                     zwParamType.BOOLEAN) }),
                                   new Param("Length", "Length",
                                             zwParamType.NUMBER),
                                   new Param("SequenceNumber",
                                             "Sequence Number",
                                             zwParamType.NUMBER) },
                          new ItemReference("HomeID"),
                          new ItemReference("SourceNodeID"),
                          new ItemReference("SequenceNumber"),
                          new ItemReference("Properties1.HeaderType"),
                          new ItemReference("Properties1.LowPower"));
                baseHeader.Initialize();


                Header singleCast = new Header(HeaderStore.H_SINGLECAST24, "SINGLECAST24", "Singlecast", 1,
                 false, false, false, false,
                 new Param[] { new Param("DestinationNodeID",
                                         "Destination Node ID",
                                         zwParamType.NODE_NUMBER) },
                 new Validation[] { new Validation("Properties1.HeaderType",
                                                   0x01) },
                 new ComplexReference { Item = new ItemReference("DestinationNodeID") },
                 null, null);
                singleCast.Initialize(baseHeader);
                foreach (var val in singleCast.Validation)
                {
                    val.Initialize(singleCast);
                }
                HeaderStore.Headers.Add(singleCast.Key, singleCast);
            }
        }

        [Test]
        public void SyncTest()
        {
            byte[] payloadGet = Tools.GetBytes("86 11");
            byte[] payloadRpt = Tools.GetBytes("86 12 01 05 01 05 0E FF 00");
            byte[] networkKeySmpl1 = Tools.GetBytes("11 11 11 11 11 11 11 11 02 AC 7A 13 21 B2 4E 76");
            byte[] networkKeyValid = Tools.GetBytes("44 86 26 01 56 5E 15 A7 02 AC 7A 13 21 B2 4E 76");
            byte[] networkKeySmpl2 = Tools.GetBytes("44 86 26 01 56 5E 15 A7 11 11 11 11 11 11 11 11");
            DataItem nonceGet = new DataItem(HeaderStore.H_SINGLECAST, 2, Tools.GetBytes("E5 0E 8C 6C 01 41 09 0E 02 9F 01 EA D0 AE"));
            DataItem nonceRpt = new DataItem(HeaderStore.H_SINGLECAST, 2, Tools.GetBytes("E5 0E 8C 6C 02 41 0C 1F 01 9F 02 FE 01 AE 01 28 B0 33 8D AE 49 22 B6 F7 6D A8 6F 25 A3 D4 D0"));
            DataItem msgWSpan = new DataItem(HeaderStore.H_SINGLECAST, 2, Tools.GetBytes("E5 0E 8C 6C 01 41 0A 2B 02 9F 03 EB 01 12 41 AF 86 B0 22 77 70 F5 3A B7 4D B2 78 D0 BD DF C0 46 A0 BB 1F 87 6F EC DE 8A 0E A2 10"));
            DataItem msgNSpan = new DataItem(HeaderStore.H_SINGLECAST, 2, Tools.GetBytes("E5 0E 8C 6C 02 41 0D 20 01 9F 03 FF 00 AA F4 A0 1B 7B CE 10 2A 60 3A 9C 88 E6 EB ED F8 7B 09 B9"));

            var sm = new SecurityManager();
            sm.AddSecurityKeys(new List<byte[]>(new[] { networkKeySmpl1, networkKeyValid, networkKeySmpl2 }));
            sm.ParseSecurity(nonceGet);
            sm.ParseSecurity(nonceRpt);
            sm.ParseSecurity(msgWSpan);
            sm.ParseSecurity(msgNSpan);
            byte[] outNetworkKey;
            byte[] outPayload;
            Extensions outExtensions;
            sm.DecryptS2(msgWSpan, out outNetworkKey, out outPayload, out outExtensions);
            Assert.AreEqual(payloadGet, outPayload);
            Assert.AreEqual(networkKeyValid, outNetworkKey);

            sm.DecryptS2(msgNSpan, out outNetworkKey, out outPayload, out outExtensions);
            Assert.AreEqual(payloadRpt, outPayload);
            Assert.AreEqual(networkKeyValid, outNetworkKey);
        }

        [Test]
        public void SyncTest2()
        {
            NodeTag nodeA = new NodeTag(1);
            NodeTag nodeB = new NodeTag(2);
            byte[] cmd1 = Tools.GetBytes("20 01 FF");
            byte[] cmd2 = Tools.GetBytes("20 01 00");
            byte[] homeId = Tools.GetBytes("E8 EA AB 12");
            byte[] receiverNonce = Tools
                .GetBytes("52 5B 5A 65 18 6E 42 6A 0A 23 2B F2 9C 62 08 DD");
            byte[] senderNonce = Tools
                .GetBytes("74 BE D5 AB 72 05 0F C0 87 99 D2 60 47 ED 07 2F");

            byte[] msgWSpan = Tools
                .GetBytes("9F 03 64 01 12 41 74 BE D5 AB 72 05 0F C0 87 99 D2 60 47 ED 07 2F 6D 54 8D 46 99 7B EC E6 6F 5C 0D");
            byte[] msgNSpan = Tools
                .GetBytes("9F 03 66 00 51 14 F9 F9 08 D7 27 4F AE AD F7");
            byte[] networkKey = Tools
                .GetBytes("C2 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION msgWSpanCmd = msgWSpan;
            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION msgNSpanCmd = msgNSpan;

            SinglecastKey scKey = SecurityS2CryptoProviderBase.GetSinglecastKey(networkKey, false);
            SpanContainer spanContainer = new SpanContainer(receiverNonce, 0, 0);
            spanContainer.InstantiateWithSenderNonce(senderNonce,
                                                     scKey.Personalization);
            spanContainer.NextNonce();

            SecurityS2CryptoProviderBase.DecryptS2(scKey.CcmKey, spanContainer.Span, nodeA, nodeB, homeId,
                msgWSpanCmd,
                out byte[] command1,
                out List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> ext1);
            Assert.NotNull(command1);
            Assert.AreEqual(cmd1, command1);

            spanContainer.NextNonce();
            spanContainer.NextNonce();
            SecurityS2CryptoProviderBase.DecryptS2(scKey.CcmKey, spanContainer.Span, nodeA, nodeB, homeId,
              msgNSpanCmd,
              out byte[] command2,
              out List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> ext2);
            Assert.NotNull(command2);
            Assert.AreEqual(cmd2, command2);
        }


        [Test]
        public void SyncTest3()
        {
            NodeTag nodeA = new NodeTag(1);
            NodeTag nodeB = new NodeTag(4);
            byte[] cmd1 = Tools.GetBytes("86 11");
            byte[] cmd2 = Tools.GetBytes("86 12 03 07 0E 0A 0E 01 00");
            byte[] homeId = Tools.GetBytes("F9 36 C2 FE");
            byte[] receiverNonce = Tools
                .GetBytes("74 26 F3 F4 32 32 B7 FA AB 0B 0B A2 E4 6C 50 B5");
            byte[] senderNonce = Tools
                .GetBytes("75 0B C7 D6 C5 28 F7 D6 01 82 E3 87 FD C0 B9 57");

            byte[] msgWSpan = Tools
                .GetBytes("9F 03 69 01 12 41 75 0B C7 D6 C5 28 F7 D6 01 82 E3 87 FD C0 B9 57 5B F0 CF EF 06 0D FB 30 80 AB");
            byte[] msgNSpan = Tools
                .GetBytes("9F 03 EA 00 2C 6E 6C F6 64 ED 14 76 E4 CA E6 7E 59 24 51 19 02");
            byte[] networkKey = Tools
                .GetBytes("C0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION msgWSpanCmd = msgWSpan;
            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION msgNSpanCmd = msgNSpan;

            SinglecastKey scKey = SecurityS2CryptoProviderBase.GetSinglecastKey(networkKey, false);
            SpanContainer spanContainer = new SpanContainer(receiverNonce, 0, 0);
            spanContainer.InstantiateWithSenderNonce(senderNonce,
                                                     scKey.Personalization);
            spanContainer.NextNonce();

            SecurityS2CryptoProviderBase.DecryptS2(scKey.CcmKey, spanContainer.Span, nodeA, nodeB, homeId,
                msgWSpanCmd,
                out byte[] command1,
                out List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> ext1);
            Assert.NotNull(command1);
            Assert.AreEqual(cmd1, command1);

            spanContainer.NextNonce();
            SecurityS2CryptoProviderBase.DecryptS2(scKey.CcmKey, spanContainer.Span, nodeB, nodeA, homeId,
              msgNSpanCmd,
              out byte[] command2,
              out List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> ext2);
            Assert.NotNull(command2);
            Assert.AreEqual(cmd2, command2);
        }

        [Test]
        public void SyncTest4()
        {
            NodeTag nodeA = new NodeTag(257);
            NodeTag nodeB = new NodeTag(1);
            //byte[] cmd1 = Tools.GetBytes("86 11");
            byte[] homeId = Tools.GetBytes("F5 71 5F 1F");
            byte[] receiverNonce = Tools
                .GetBytes("B1 5A 07 3A 5E C1 6A 5D 81 B2 1C 23 CA F9 FA C7");
            byte[] senderNonce = Tools
                .GetBytes("E3 98 6C 80 5C D2 C0 CD 3A 6D 27 53 8F E4 CE 2A");

            byte[] msgWSpan = Tools
                .GetBytes("9F 03 57 01 12 41 E3 98 6C 80 5C D2 C0 CD 3A 6D 27 53 8F E4 CE 2A 77 9F 8B 08 5B EA 5A 5B C0 9E BA 8A 11 65 ");
            byte[] networkKey = Tools
                .GetBytes("36 80 2A 55 E0 D2 4F CE C6 AE D7 77 70 0A 3D C3");

            COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION msgWSpanCmd = msgWSpan;

            SinglecastKey scKey = SecurityS2CryptoProviderBase.GetSinglecastKey(networkKey, true);
            SpanContainer spanContainer = new SpanContainer(receiverNonce, 0, 0);
            spanContainer.InstantiateWithSenderNonce(senderNonce,
                                                     scKey.Personalization);
            spanContainer.NextNonce();

            SecurityS2CryptoProviderBase.DecryptS2(scKey.CcmKey, spanContainer.Span, nodeA, nodeB, homeId,
                msgWSpanCmd,
                out byte[] command1,
                out List<COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1> ext1);
            Assert.NotNull(command1);
            //Assert.IsTrue(command1.Length > 0);
        }
    }
}
