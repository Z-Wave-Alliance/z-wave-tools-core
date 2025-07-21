/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Utils;

namespace ZWave.Xml.FrameHeader
{
    public partial class BaseHeader
    {
        public BaseHeader()
        {
        }

        public BaseHeader(int key, string name, string text, Param[] param, ItemReference homeId, 
            ItemReference source, ItemReference sequenceNumber, ItemReference headerType, ItemReference isLTX)
        {
            this.Key = (byte)key;
            this.Name = name;
            this.Text = text;
            this.Param = new Collection<Param>(param);
            this.HomeId = homeId;
            this.Source = source;
            this.SequenceNumber = sequenceNumber;
            this.HeaderType = headerType;
            this.IsLTX = isLTX;
        }

        public void Initialize()
        {
            FillDataIndexes();
        }

        private void FillDataIndexes()
        {
            Calculatelength();// base header have no optional or list parameters
            HomeId.Index = new DataIndex(0);
            FrameDefinition.ProcessHeader(HomeId.Index, Param, HomeId.Ref);
            Source.Index = new DataIndex(0);
            FrameDefinition.ProcessHeader(Source.Index, Param, Source.Ref);
            if (Destination != null)
            {
                Destination.Index = new DataIndex(0);
                FrameDefinition.ProcessHeader(Destination.Index, Param, Destination.Ref);
            }
            SequenceNumber.Index = new DataIndex(0);
            FrameDefinition.ProcessHeader(SequenceNumber.Index, Param, SequenceNumber.Ref);
            HeaderType.Index = new DataIndex(0);
            FrameDefinition.ProcessHeader(HeaderType.Index, Param, HeaderType.Ref);
            IsLTX.Index = new DataIndex(0);
            FrameDefinition.ProcessHeader(IsLTX.Index, Param, IsLTX.Ref);
            ProcessDataIndexes();
        }

        private Param _prevVariableParam;
        private void ProcessDataIndexes()
        {
            _prevVariableParam = null;
            byte indexInData = 0;
            foreach (var item in Param)
            {
                if (item.Bits % 8 == 0)
                {
                    if (string.IsNullOrEmpty(item.SizeRef))
                    {
                        byte bytesCount = (byte)(item.Bits / 8);
                        if (item.Size > 0)
                        {
                            bytesCount = (byte)(bytesCount * item.Size);
                        }
                        byte offsetInData = 0x00;
                        byte maskInData = 0xFF;

                        item.PreviousVariableParam = _prevVariableParam;
                        item.ItemRef = new ItemReference
                        {
                            Index = new DataIndex(0)
                            {
                                IndexInData = indexInData,
                                MaskInData = maskInData,
                                OffsetInData = offsetInData
                            },
                        };
                        if (item.Param1 != null && item.Param1.Count > 0)
                        {
                            foreach (var prm1 in item.Param1)
                            {
                                maskInData = Tools.GetMaskFromBits(prm1.Bits, offsetInData);
                                prm1.PreviousVariableParam = _prevVariableParam;
                                prm1.ItemRef = new ItemReference
                                {
                                    Index = new DataIndex(0)
                                    {
                                        IndexInData = indexInData,
                                        MaskInData = maskInData,
                                        OffsetInData = offsetInData
                                    },
                                    IndexOfOpt = item.ItemRef.IndexOfOpt
                                };
                                offsetInData += prm1.Bits;
                            }
                        }
                        indexInData = (byte)(indexInData + bytesCount);
                        if (item.ItemRef.IndexOfOpt != null)
                        {
                            indexInData = 0;
                            _prevVariableParam = item;
                        }
                    }
                    else
                    {
                        item.PreviousVariableParam = _prevVariableParam;
                        item.ListRef = new ListReference
                        {
                            Index = new DataIndex(0) { IndexInData = indexInData },
                        };
                        indexInData = 0;
                        _prevVariableParam = item;
                    }

                }
            }
        }

        [XmlIgnore]
        public byte Length { get; set; }

        public byte[] GetHomeId(byte[] store, int dataLength)
        {
            byte[] ret = null;
            if (dataLength > 4 + HomeId.Index.IndexInData)
            {
                ret = new byte[4];
                ret[0] = store[HomeId.Index.IndexInData];
                ret[1] = store[HomeId.Index.IndexInData + 1];
                ret[2] = store[HomeId.Index.IndexInData + 2];
                ret[3] = store[HomeId.Index.IndexInData + 3];
            }
            return ret;
        }

        public ushort GetSource(byte[] store, int dataLength)
        {
            ushort ret = 0;
            if (dataLength >= Source.Index.IndexInData + Source.Index.SizeInData)
            {
                int val = Tools.GetInt32(store, Source.Index.IndexInData, Source.Index.SizeInData);
                ret = (ushort)((val & Source.Index.MaskInData) >> Source.Index.OffsetInData);
            }
            return ret;
        }

        public ushort GetDestination(byte[] store, int dataLength)
        {
            ushort ret = 0;
            if (dataLength >= Destination.Index.IndexInData + Destination.Index.SizeInData)
            {
                int val = Tools.GetInt32(store, Destination.Index.IndexInData, Destination.Index.SizeInData);
                ret = (ushort)(val & Destination.Index.MaskInData);
            }
            return ret;
        }

        public int GetSequenceNumber(byte[] store, int dataLength)
        {
            if (dataLength > SequenceNumber.Index.IndexInData)
                return (store[SequenceNumber.Index.IndexInData] & SequenceNumber.Index.MaskInData) >> SequenceNumber.Index.OffsetInData;
            return -1;
        }

        public bool GetIsLtx(byte[] store, int dataLength)
        {
            if (dataLength > IsLTX.Index.IndexInData)
                return (store[IsLTX.Index.IndexInData] & IsLTX.Index.MaskInData) >> IsLTX.Index.OffsetInData > 0;
            return false;
        }

        public byte GetHeaderType(byte[] store, int dataLength)
        {
            if (dataLength > HeaderType.Index.IndexInData)
                return (byte)((store[HeaderType.Index.IndexInData] & HeaderType.Index.MaskInData) >> HeaderType.Index.OffsetInData);
            return 0;
        }

        private void Calculatelength()
        {
            foreach (var item in Param)
            {
                if (item.Bits % 8 == 0)
                {
                    var bytesCount = (byte)(item.Bits / 8);
                    if (item.Size > 0)
                    {
                        bytesCount = (byte)(bytesCount * item.Size);
                    }
                    Length = (byte)(Length + bytesCount);
                }
            }
        }
    }
}
