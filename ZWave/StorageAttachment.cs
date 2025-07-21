/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave
{
    public abstract class StorageAttachment
    {
        public AttachmentTypes Type { get; set; }
        public byte Version { get; set; }
        public StorageAttachment(byte[] buffer)
        {
            if (buffer != null && buffer.Length > 1)
            {
                Type = (AttachmentTypes)buffer[0];
                Version = buffer[1];
                FromByteArrayInternal(buffer, 2);
            }
        }

        protected abstract void FromByteArrayInternal(byte[] buffer, int startIndex);

        protected abstract IList<byte> ToByteArrayInternal();

        public byte[] ToByteArray()
        {
            List<byte> ret = new List<byte>();
            ret.Add((byte)Type);
            ret.Add(Version);
            IList<byte> data = ToByteArrayInternal();
            if (data != null)
            {
                ret.AddRange(data);
            }
            return ret.ToArray();
        }

        public static StorageAttachment CreateAttachment(byte[] buffer)
        {
            StorageAttachment ret = null;
            if (buffer != null && buffer.Length > 1)
            {
                switch ((AttachmentTypes)buffer[0])
                {
                    case AttachmentTypes.NotUsed:
                        break;
                    case AttachmentTypes.Indexes:
                        break;
                    case AttachmentTypes.Filters:
                        break;
                    case AttachmentTypes.NetworkKeys:
                        ret = new NetworkKeysAttachment(buffer);
                        break;
                    case AttachmentTypes.FrameComment:
                        ret = new FrameCommentAttachment(buffer);
                        break;
                    case AttachmentTypes.Session:
                        ret = new SessionAttachment(buffer);
                        break;
                    default:
                        break;
                }
            }
            return ret;
        }
    }

    public class NetworkKeysAttachment : StorageAttachment
    {
        public static byte VER = 0x01;
        private const byte KEY_TEMP_FLAG = 0x01;
        public List<byte[]> NetworkKeys { get; set; }
        public List<byte[]> TempNetworkKeys { get; set; }
        public NetworkKeysAttachment(byte[] data) :
            base(data)
        {

        }

        public NetworkKeysAttachment() :
            base(new byte[] { (byte)AttachmentTypes.NetworkKeys, VER })
        {
            NetworkKeys = new List<byte[]>();
            TempNetworkKeys = new List<byte[]>();
        }

        protected override void FromByteArrayInternal(byte[] buffer, int startIndex)
        {
            if (buffer != null)
            {
                NetworkKeys = new List<byte[]>();
                TempNetworkKeys = new List<byte[]>();
                int index = startIndex;
                while (buffer.Length > index)
                {
                    index += 4; // skip homeId header
                    int count = 0;
                    if (buffer.Length > index)
                    {
                        count = buffer[index];
                    }
                    index++;
                    byte flags = buffer[index];
                    index++;//reserved;
                    if ((flags & KEY_TEMP_FLAG) == 0)
                    {
                        index = ParseKeys(NetworkKeys, buffer, index, count);
                    }
                    else
                    {
                        index = ParseKeys(TempNetworkKeys, buffer, index, count);
                    }
                }
            }
        }

        private static int ParseKeys(List<byte[]> keys, byte[] buffer, int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                byte[] key = new byte[16];
                if (buffer.Length >= index + 16)
                {
                    Array.Copy(buffer, index, key, 0, 16);
                }
                index += 16;
                if (key != null)
                {
                    keys.Add(key);
                }
            }
            return index;
        }

        protected override IList<byte> ToByteArrayInternal()
        {
            List<byte> ret = new List<byte>();
            ret.AddRange(new byte[4]); // empty homeId header
            ret.Add((byte)NetworkKeys.Count);
            ret.Add(0x00); // not temp keys
            foreach (var key in NetworkKeys)
            {
                ret.AddRange(key);
            }

            ret.AddRange(new byte[4]);
            ret.Add((byte)TempNetworkKeys.Count);
            ret.Add(0x01); // temp keys
            foreach (var key in TempNetworkKeys)
            {
                ret.AddRange(key);
            }
            return ret;
        }
    }

    public class SessionAttachment : StorageAttachment
    {
        public static byte VER = 0x01;
        public int Key
        {
            get { return (Reserved1 << 24) + (ApiType << 16) + (Reserved2 << 8) + SessionId; }
        }
        public byte Reserved1 { get; set; }
        public byte ApiType { get; set; }
        public byte Reserved2 { get; set; }
        public ushort SessionId { get; set; }
        public string Name { get; set; }
        public string Args { get; set; }
        public SessionAttachment(byte[] data) :
            base(data)
        {
        }

        public SessionAttachment() :
            base(new byte[] { (byte)AttachmentTypes.Session, VER })
        {
        }

        protected override void FromByteArrayInternal(byte[] buffer, int startIndex)
        {
            if (buffer != null)
            {
                int index = startIndex;
                Reserved1 = (buffer.Length > index) ? buffer[index++] : (byte)0x00;
                ApiType = (buffer.Length > index) ? buffer[index++] : (byte)0x00;
                Reserved2 = (buffer.Length > index) ? buffer[index++] : (byte)0x00;
                SessionId = (buffer.Length > index) ? buffer[index++] : (byte)0x00;
                var nameLen = (buffer.Length > index) ? buffer[index++] : (byte)0x00;
                if (buffer.Length > index + nameLen)
                {
                    Name = Encoding.ASCII.GetString(buffer.Skip(index).Take(nameLen).ToArray());
                    index += nameLen;
                }
                var argsLen = (buffer.Length > index) ? buffer[index++] : (byte)0x00;
                if (buffer.Length > index + argsLen)
                {
                    Args = Encoding.ASCII.GetString(buffer.Skip(index).Take(argsLen).ToArray());
                    index += argsLen;
                }
            }
        }

        protected override IList<byte> ToByteArrayInternal()
        {
            List<byte> ret = new List<byte>();
            ret.Add(Reserved1);
            ret.Add(ApiType);
            ret.Add(Reserved2);
            ret.Add((byte)SessionId);
            if (!string.IsNullOrEmpty(Name))
            {
                var data = Encoding.ASCII.GetBytes(Name);
                ret.Add((byte)data.Length);
                ret.AddRange(data);
            }
            else
            {
                ret.Add(0x00);
            }
            if (!string.IsNullOrEmpty(Args))
            {
                var data = Encoding.ASCII.GetBytes(Args);
                ret.Add((byte)data.Length);
                ret.AddRange(data);
            }
            else
            {
                ret.Add(0x00);
            }
            return ret;
        }
    }

    public class FrameCommentAttachment : StorageAttachment
    {
        public static byte VER = 0x01;
        private const byte RESERVED_FLAG = 0x00;
        private const byte OVERWRITE_FLAG = 0x00;
        public byte Reserved { get; set; }
        public int LineNo { get; set; }
        public byte BlockIndex { get; set; }
        public byte MergeMode { get; set; }
        public byte MediaType { get; set; }
        public string Comment { get; set; }
        public FrameCommentAttachment(byte[] data) :
            base(data)
        {

        }

        public FrameCommentAttachment() :
            base(new byte[] { (byte)AttachmentTypes.FrameComment, VER })
        {

        }

        protected override void FromByteArrayInternal(byte[] buffer, int startIndex)
        {
            if (buffer != null)
            {

                int index = startIndex;
                while (buffer.Length > index)
                {
                    index += 4; // skip homeId header
                    int count = 0;
                    if (buffer.Length > index)
                    {
                        count = buffer[index];
                    }
                    index++;
                    byte flags = buffer[index];
                    index++;//reserved;
                }
            }
        }

        private static int ParseKeys(List<byte[]> keys, byte[] buffer, int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                byte[] key = new byte[16];
                if (buffer.Length >= index + 16)
                {
                    Array.Copy(buffer, index, key, 0, 16);
                }
                index += 16;
                if (key != null)
                {
                    keys.Add(key);
                }
            }
            return index;
        }

        protected override IList<byte> ToByteArrayInternal()
        {
            List<byte> ret = new List<byte>();
            ret.AddRange(new byte[4]); // empty homeId header
            ret.Add(0x00); // not temp keys
            ret.AddRange(new byte[4]);
            ret.Add(0x01); // temp keys
            return ret;
        }
    }

    public enum AttachmentTypes
    {
        NotUsed = 0x00,
        Indexes = 0x01,
        Filters = 0x02,
        NetworkKeys = 0x03,
        FrameComment = 0x04,
        Session = 0x05

    }
}
