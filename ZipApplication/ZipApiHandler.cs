/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils;
using ZWave.CommandClasses;
using ZWave.ZipApplication.Data;
using System.Collections.Generic;

namespace ZWave.ZipApplication
{
    public class ZipApiHandler : CommandHandler
    {
        public byte CommandClassKey { get; set; }
        public byte CommandKey { get; set; }
        private ZipApiHandler()
        {
        }

        public ZipApiHandler(byte commandClassKey, byte commandKey, params ByteIndex[] payloadFilter)
        {
            CommandClassKey = commandClassKey;
            CommandKey = commandKey;
            if (payloadFilter != null && payloadFilter.Length > 0)
            {
                mMask = new ByteIndex[2 + payloadFilter.Length];
                mMask[0] = new ByteIndex(commandClassKey);
                mMask[1] = new ByteIndex(commandKey);
                for (int i = 0; i < payloadFilter.Length; i++)
                {
                    mMask[2 + i] = payloadFilter[i];
                }
            }
            else
            {
                mMask = new ByteIndex[] 
                {
                    new ByteIndex(commandClassKey), 
                    commandKey > 0 ? new ByteIndex(commandKey) : ByteIndex.AnyValue 
                };
            }
        }

        protected override bool IsExpectedData(IList<byte> data)
        {
            bool ret = false;
            if (IsZipPacket(data) && !IsZipEncapFilter(Mask))
            {
                COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET packet = (byte[])data;
                if (packet.zWaveCommand != null && packet.zWaveCommand.Count > 1)
                {
                    ret = base.IsExpectedData(packet.zWaveCommand);
                }
            }
            else
            {
                ret = base.IsExpectedData(data);
            }
            return ret;
        }

        private static bool IsZipEncapFilter(ByteIndex[] mask)
        {
            return
                mask != null && mask.Length > 0 &&
                (mask[0].Value == COMMAND_CLASS_ZIP_V4.ID || mask[0].Value == COMMAND_CLASS_ZIP_ND.ID);
        }

        private static bool IsZipPacket(IList<byte> data)
        {
            return
                data != null && data is byte[] && data.Count > 2 &&
                data[0] == COMMAND_CLASS_ZIP_V4.ID && data[1] == COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.ID;
        }

        public void SetSeqNo(byte seqNo)
        {
            if (mMask[0].Value == COMMAND_CLASS_ZIP_V4.ID && mMask[1].Value == COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.ID)
            {
                mMask[4] = new ByteIndex(seqNo);
            }
        }

        public static ZipApiHandler CreateAckHandler(byte sequenceNumber, bool isCheckSeqNo)
        {
            ZipApiHandler ret = new ZipApiHandler();
            ByteIndex seqNo = isCheckSeqNo ? new ByteIndex(sequenceNumber) : ByteIndex.AnyValue;
            ret.mMask = new ByteIndex[] 
                {  
                    new ByteIndex(COMMAND_CLASS_ZIP_V4.ID), 
                    new ByteIndex(COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.ID),
                    new ByteIndex(0x40),
                    ByteIndex.AnyValue,
                    seqNo,
                    ByteIndex.AnyValue,
                    ByteIndex.AnyValue
                };
            return ret;
        }

        public static ZipApiHandler CreateNAckHandler(byte sequenceNumber, bool isCheckSeqNo)
        {
            ZipApiHandler ret = new ZipApiHandler();
            ByteIndex seqNo = isCheckSeqNo ? new ByteIndex(sequenceNumber) : ByteIndex.AnyValue;
            ret.mMask = new ByteIndex[] 
                {  
                    new ByteIndex(COMMAND_CLASS_ZIP_V4.ID), 
                    new ByteIndex(COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.ID),
                    new ByteIndex(0x20),
                    ByteIndex.AnyValue,
                    seqNo,
                    ByteIndex.AnyValue,
                    ByteIndex.AnyValue
                };
            return ret;
        }

        public static ZipApiHandler CreateWaitingNAckHandler(byte sequenceNumber, bool isCheckSeqNo)
        {
            ZipApiHandler ret = new ZipApiHandler();
            ByteIndex seqNo = isCheckSeqNo ? new ByteIndex(sequenceNumber) : ByteIndex.AnyValue;
            ret.mMask = new ByteIndex[]
                {
                    new ByteIndex(COMMAND_CLASS_ZIP_V4.ID),
                    new ByteIndex(COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.ID),
                    new ByteIndex(0x30),
                    ByteIndex.AnyValue,
                    seqNo,
                    ByteIndex.AnyValue,
                    ByteIndex.AnyValue
                };
            return ret;
        }
    }
}
