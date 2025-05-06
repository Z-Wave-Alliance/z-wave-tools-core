using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZWave;
using Utils;
using ZWave.Layers;
using System.IO;
using ZWave.Enums;

namespace ZWaveTests
{
    [TestFixture]
    public class StorageAttachmentTests
    {
        [Test]
        public void Sessions_CreateFromBuffer_Valid()
        {
            List<byte> data = new List<byte>();
            data.Add((byte)AttachmentTypes.Session);
            data.Add(SessionAttachment.VER);
            List<byte> ret = new List<byte>();
            data.Add(0);
            data.Add((byte)ApiTypes.Basic);
            data.Add(0);
            data.Add(3);
            var str = Encoding.ASCII.GetBytes("COM1");
            data.Add((byte)str.Length);
            data.AddRange(str);
            data.Add(0x00);
            SessionAttachment sa = new SessionAttachment(data.ToArray());
            Assert.AreEqual(AttachmentTypes.Session, sa.Type);
            Assert.AreEqual(SessionAttachment.VER, sa.Version);
            Assert.AreEqual("COM1", sa.Name);
            Assert.AreEqual(null, sa.Args);
            Assert.AreEqual((byte)ApiTypes.Basic, sa.ApiType);
            Assert.AreEqual(3, sa.SessionId);
        }

        [Test]
        public void Sessions_ToBuffer_Valid()
        {
            SessionAttachment sa = new SessionAttachment();
            sa.SessionId = 3;
            sa.ApiType = (byte)ApiTypes.Basic;
            sa.Name = "COM1";
            sa.Args = "bla bla";

            byte[] data = sa.ToByteArray();
            int index = 0;
            Assert.AreEqual((byte)AttachmentTypes.Session, data[index++]);
            Assert.AreEqual(SessionAttachment.VER, data[index++]);
            Assert.AreEqual(0, data[index++]);
            Assert.AreEqual((byte)ApiTypes.Basic, data[index++]);
            Assert.AreEqual(0, data[index++]);
            Assert.AreEqual(3, data[index++]);
            Assert.AreEqual(4, data[index++]);
            Assert.AreEqual("COM1", Encoding.ASCII.GetString(data.Skip(index).Take(4).ToArray()));
            index += 4;
            Assert.AreEqual(7, data[index++]);
            Assert.AreEqual("bla bla", Encoding.ASCII.GetString(data.Skip(index).Take(7).ToArray()));
        }


        readonly byte[] sk1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };
        readonly byte[] sk2 = new byte[] { 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7 };
        readonly byte[] sk3 = new byte[] { 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        readonly byte[] sk4 = new byte[] { 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        readonly byte[] sk5 = new byte[] { 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        [Test]
        public void CreateFromBuffer_Valid()
        {

            List<byte> data = new List<byte>();
            data.Add((byte)AttachmentTypes.NetworkKeys);
            data.Add(NetworkKeysAttachment.VER);
            data.AddRange(new byte[4]);
            data.Add(0x02);
            data.Add(0x00);
            data.AddRange(sk1);
            data.AddRange(sk2);
            data.AddRange(new byte[4]);
            data.Add(0x03);
            data.Add(0x00);
            data.AddRange(sk3);
            data.AddRange(sk4);
            data.AddRange(sk5);
            NetworkKeysAttachment sa = new NetworkKeysAttachment(data.ToArray());
            Assert.AreEqual(AttachmentTypes.NetworkKeys, sa.Type);
            Assert.AreEqual(NetworkKeysAttachment.VER, sa.Version);
            Assert.AreEqual(5, sa.NetworkKeys.Count);
            Assert.IsTrue(sa.NetworkKeys[0].SequenceEqual(sk1));
            Assert.IsTrue(sa.NetworkKeys[1].SequenceEqual(sk2));
            Assert.IsTrue(sa.NetworkKeys[2].SequenceEqual(sk3));
            Assert.IsTrue(sa.NetworkKeys[3].SequenceEqual(sk4));
            Assert.IsTrue(sa.NetworkKeys[4].SequenceEqual(sk5));
        }

        [Test]
        public void ToBuffer_Valid()
        {
            NetworkKeysAttachment sa = new NetworkKeysAttachment();
            sa.NetworkKeys.Add(sk1);
            sa.NetworkKeys.Add(sk2);
            sa.NetworkKeys.Add(sk3);
            sa.NetworkKeys.Add(sk4);
            sa.NetworkKeys.Add(sk5);

            byte[] data = sa.ToByteArray();
            int index = 0;
            Assert.AreEqual((byte)AttachmentTypes.NetworkKeys, data[index]);
            index++;
            Assert.AreEqual(NetworkKeysAttachment.VER, data[index]);
            index++;
            index += 4;
            Assert.AreEqual(5, data[index]);
            index++;
            index++;
            Assert.IsTrue(data.Skip(index).Take(16).SequenceEqual(sk1));
            index += 16;
            Assert.IsTrue(data.Skip(index).Take(16).SequenceEqual(sk2));
            index += 16;
            Assert.IsTrue(data.Skip(index).Take(16).SequenceEqual(sk3));
            index += 16;
            Assert.IsTrue(data.Skip(index).Take(16).SequenceEqual(sk4));
            index += 16;
            Assert.IsTrue(data.Skip(index).Take(16).SequenceEqual(sk5));
            index += 16;
        }

        [Test]
        public void NetworkKeyAttachment_GetBytes_AreEqual()
        {
            NetworkKeysAttachment attachment = new NetworkKeysAttachment();
            attachment.NetworkKeys.AddRange(new byte[][]
            {
                "000102030405060708090A0B0C0D0E0F".GetBytes(),
                "101112131415161718191A1B1C1D1E1F".GetBytes(),
                "202122232425262728292A2B2C2D2E2F".GetBytes(),
            });
            attachment.NetworkKeys.AddRange(new byte[][]
            {
                "303132333435363738393A3B3C3D3E3F".GetBytes(),
                "404142434445464748494A4B4C4D4E4F".GetBytes(),
            });
            attachment.TempNetworkKeys.AddRange(new byte[][]
            {
                "505152535455565758595A5B5C5D5E5F".GetBytes()
            });


            DataChunk dcWrite = new DataChunk(attachment.ToByteArray(), 0, true, ApiTypes.Attachment);

            byte[] writeBuffer = dcWrite.ToByteArray();
            BinaryReader br = new BinaryReader(new MemoryStream(writeBuffer));

            DataChunk dcRead = DataChunk.ReadDataChunk(br);
            var dataBuffer = dcRead.GetDataBuffer();

            Assert.AreEqual(ApiTypes.Attachment, dcRead.ApiType);
            Assert.AreEqual(attachment.ToByteArray(), dataBuffer);
            Assert.AreEqual(AttachmentTypes.NetworkKeys, (AttachmentTypes)dataBuffer[0]);

            NetworkKeysAttachment nka = (NetworkKeysAttachment)StorageAttachment.CreateAttachment(dataBuffer);

            Assert.AreEqual(5, nka.NetworkKeys.Count);
            Assert.AreEqual(1, nka.TempNetworkKeys.Count);
            Assert.AreEqual("000102030405060708090A0B0C0D0E0F".GetBytes(), nka.NetworkKeys[0]);
            Assert.AreEqual("303132333435363738393A3B3C3D3E3F".GetBytes(), nka.NetworkKeys[3]);
            Assert.AreEqual("505152535455565758595A5B5C5D5E5F".GetBytes(), nka.TempNetworkKeys[0]);
        }
    }
}
