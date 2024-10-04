/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZWave
{
    /// <summary>
    /// Storage Header 
    /// | 4 bytes header Version | 4 bytes EncodingCode | 512 bytes CommentText | 
    /// </summary>
    public class StorageHeader
    {
        public const int STORAGE_HEADER_SIZE = 2048;
        /// <summary>
        /// 100 - initial version
        /// 101 - added frequency dictionary, changed APIType as bitmask (00000ipb)
        /// 102 - added APIType - TEXT 
        /// 103 - added end of trace address
        /// 104 - remove frequencies, sessions, apiType, traceTotalLength
        /// </summary>
        public const int STORAGE_LATEST_VERSION = 104;
        private const int maxChars = COMMENT_BYTES / 2 - 2;
        private const int COMMENT_BYTES = 512;

        public StorageHeader()
        {
        }

        public static StorageHeader GetHeader(params byte[] buffer)
        {
            StorageHeader header = null;
            if (IsValid(buffer))
            {
                header = new StorageHeader();
             
                // Version
                header.Version = BitConverter.ToInt32(buffer, 0);

                // TextEncoding 
                header.TextEncoding = BitConverter.ToInt32(buffer, 4);

                header.Comment = Encoding.Unicode.GetString(buffer, 8, COMMENT_BYTES);
                if (header.Comment != null)
                {
                    header.Comment = header.Comment.TrimEnd(new char[] { (char)0x00 });
                }
            }
            return header;
        }

        public byte[] GetBuffer()
        {
            byte[] buffer = new byte[STORAGE_HEADER_SIZE];

            // Version
            Array.Copy(BitConverter.GetBytes(STORAGE_LATEST_VERSION), buffer, 4);

            // TextEncoding
            Array.Copy(BitConverter.GetBytes(TextEncoding), 0, buffer, 4, 4);

            // Comment
            Array.Copy(new byte[COMMENT_BYTES], 0, buffer, 8, COMMENT_BYTES);
            if (!string.IsNullOrEmpty(Comment))
            {
                Encoding.Unicode.GetBytes(Comment, 0, Comment.Length < maxChars ? Comment.Length : maxChars, buffer, 8);
            }

            // CRC
            Array.Copy(BitConverter.GetBytes(CalculateCRC(buffer)), 0, buffer, STORAGE_HEADER_SIZE - 2, 2);

            return buffer;
        }

        /// <summary>
        /// Gets or sets the version. First 4 bytes in Buffer.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TextEncoding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Type of the API: 
        /// 0 - Zniffer API; 
        /// 1 - Basic API;
        /// 2 - Programmer API.
        /// 3 - Zip API.
        /// 7 - Text
        /// </summary>
        //private byte ApiType { get; set; }

        public static bool IsValid(byte[] buffer)
        {
            if (buffer == null || buffer.Length < STORAGE_HEADER_SIZE)
            {
                return false;
            }
            else
            {
                return CalculateCRC(buffer) == BitConverter.ToUInt16(buffer, STORAGE_HEADER_SIZE - 2);
            }
        }

        private const uint POLY = 0x1021;          /* crc-ccitt mask */
        private static void update_crc(ushort ch, ref ushort crc)
        {
            ushort i, v, xor_flag;
            v = 0x80;
            for (i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) != 0)
                {
                    xor_flag = 1;
                }
                else
                {
                    xor_flag = 0;
                }
                crc = (ushort)(crc << 1);

                if ((ch & v) != 0)
                {
                    crc = (ushort)(crc + 1);
                }

                if (xor_flag != 0)
                {
                    crc = (ushort)(crc ^ POLY);
                }
                v = (ushort)(v >> 1);
            }
        }
        private static void augment_message_for_crc(ref ushort crc)
        {
            ushort i, xor_flag;

            for (i = 0; i < 16; i++)
            {
                if ((crc & 0x8000) != 0)
                {
                    xor_flag = 1;
                }
                else
                {
                    xor_flag = 0;
                }
                crc = (ushort)(crc << 1);

                if (xor_flag != 0)
                {
                    crc = (ushort)(crc ^ POLY);
                }
            }
        }
        private static ushort CalculateCRC(byte[] buffer)
        {
            ushort sum = 0xFFFF;
            for (int i = 0; i < STORAGE_HEADER_SIZE - 2; i++)
            {
                update_crc(buffer[i], ref sum);
            }
            augment_message_for_crc(ref sum);
            return sum;
        }
    }
}
