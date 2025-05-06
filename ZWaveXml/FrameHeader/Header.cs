/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using Utils;

namespace ZWave.Xml.FrameHeader
{
    public partial class Header
    {
        [XmlIgnore]
        public BaseHeader BaseHeaderDefinition { get; set; }

        public Header()
        { }

        public Header(int key, string name, string text, int baseHeaderKey,
       bool isAck, bool isError, bool isMulticast, bool isRouted,
       Param[] param, Validation[] validation, ComplexReference destination,
       ItemReference hops, ListReference repeaters)
        {
            this.Key = (byte)key;
            this.Name = name;
            this.Text = text;
            this.BaseHeaderKey = (byte)baseHeaderKey;
            this.IsAck = isAck;
            this.IsError = isError;
            this.IsMulticast = isMulticast;
            this.IsRouted = isRouted;
            this.Param = new Collection<Param>(param);
            this.Validation = new Collection<Validation>(validation);
            this.Destination = destination;
            this.Hops = hops;
            this.Repeaters = repeaters;
        }

        public void Initialize(BaseHeader baseHeader)
        {
            BaseHeaderDefinition = baseHeader;
            ProcessDataIndexes();
            FillDataIndexes();
        }

        private void FillDataIndexes()
        {
            if (IsMulticast)
            {
                ListReference lRef = (ListReference)Destination.Item;
                lRef.IndexOfSize = new DataIndex(BaseHeaderDefinition.Length);
                FrameDefinition.ProcessHeader(lRef.IndexOfSize, Param, lRef.SizeRef);
                lRef.Index = new DataIndex(BaseHeaderDefinition.Length);
                FrameDefinition.ProcessHeader(lRef.Index, Param, lRef.Ref);
            }
            else
            {
                if (Destination != null)
                {
                    ItemReference iRef = (ItemReference)Destination.Item;
                    iRef.Index = new DataIndex(BaseHeaderDefinition.Length);
                    FrameDefinition.ProcessHeader(iRef.Index, Param, iRef.Ref);
                }
            }
            if (IsRouted)
            {
                Hops.Index = new DataIndex(BaseHeaderDefinition.Length);
                FrameDefinition.ProcessHeader(Hops.Index, Param, Hops.Ref);
                Repeaters.IndexOfSize = new DataIndex(BaseHeaderDefinition.Length);
                FrameDefinition.ProcessHeader(Repeaters.IndexOfSize, Param, Repeaters.SizeRef);
                Repeaters.Index = new DataIndex(BaseHeaderDefinition.Length);
                FrameDefinition.ProcessHeader(Repeaters.Index, Param, Repeaters.Ref);
            }
        }

        private static byte GetLength(byte[] store, Param param)
        {
            int ret = 0;
            if (param.ItemRef != null)
            {
                ItemReference iref = param.ItemRef;
                if (iref.IndexOfOpt == null ||
                    iref.IndexOfOpt != null && (store[iref.IndexOfOpt.IndexInData] & iref.IndexOfOpt.MaskInData) >> iref.IndexOfOpt.OffsetInData > 0)
                {
                    int bCount = param.Bits / 8 * (param.Size > 0 ? param.Size : 1);
                    if (bCount >= 0)
                    {
                        ret += iref.Index.IndexInData + bCount;
                    }
                }
                else
                {
                    ret += iref.Index.IndexInData;
                }
            }
            else
            {
                ListReference lref = param.ListRef;
                if (lref.IndexOfOpt == null ||
                    lref.IndexOfOpt != null && (store[lref.IndexOfOpt.IndexInData] & lref.IndexOfOpt.MaskInData) >> lref.IndexOfOpt.OffsetInData > 0)
                {
                    if (param.PreviousVariableParam != null)
                    {
                        byte prevLen = (byte)(GetLength(store, param.PreviousVariableParam) + lref.Index.IndexInData);
                        if (store.Length > prevLen - 1 + lref.IndexOfSize.IndexInData)
                        {
                            int len = (store[prevLen - 1 + lref.IndexOfSize.IndexInData] & lref.IndexOfSize.MaskInData) >> lref.IndexOfSize.OffsetInData;
                            if (len >= 0)
                            {
                                int bCount = param.Bits / 8 * len + (param.SizeCorrectionSpecified ? param.SizeCorrection : 0);
                                if (bCount >= 0)
                                {
                                    ret += lref.Index.IndexInData + bCount;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (store.Length > lref.IndexOfSize.IndexInData)
                        {
                            int len = (store[lref.IndexOfSize.IndexInData] & lref.IndexOfSize.MaskInData) >> lref.IndexOfSize.OffsetInData;
                            if (len >= 0)
                            {
                                int bCount = param.Bits / 8 * len + (param.SizeCorrectionSpecified ? param.SizeCorrection : 0);
                                if (bCount >= 0)
                                {
                                    ret += lref.Index.IndexInData + bCount;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ret += lref.Index.IndexInData;
                }
            }
            if (param.PreviousVariableParam != null)
            {
                ret += GetLength(store, param.PreviousVariableParam);
            }
            if (ret < 0 || ret > 0xFF)
                ret = 0;
            return (byte)ret;
        }

        public byte GetLength(byte[] store, int dataLength)
        {
            byte ret = 0;
            if (Param != null && Param.Any())
            {
                ret = GetLength(store, Param.Last());
            }
            else
            {
                ret = BaseHeaderDefinition.Length;
            }
            if (ret > dataLength)
            {
                ret = 0;
            }
            return ret;
        }

        public byte GetHops(byte[] store, int dataLength)
        {
            byte ret = 0;
            if (IsRouted)
            {
                if (Hops != null && dataLength > Hops.Index.IndexInData)
                {
                    int tmp = (store[Hops.Index.IndexInData] & Hops.Index.MaskInData) >> Hops.Index.OffsetInData;
                    if (tmp > 0 && tmp < 0xFF)
                        ret = (byte)tmp;
                }
            }
            return ret;
        }

        public byte GetRoutesCount(byte[] store, int dataLength)
        {
            byte ret = 0;
            if (IsRouted)
            {
                if (Repeaters != null && dataLength > Repeaters.IndexOfSize.IndexInData)
                {
                    int tmp = (store[Repeaters.IndexOfSize.IndexInData] & Repeaters.IndexOfSize.MaskInData) >> Repeaters.IndexOfSize.OffsetInData;
                    if (tmp > 0 && tmp < 0xFF)
                        ret = (byte)tmp;
                }
            }
            return ret;
        }

        public byte[] GetRepeaters(byte[] store, int dataLength)
        {
            byte[] ret = null;
            if (IsRouted)
            {
                if (Repeaters != null && dataLength > Repeaters.IndexOfSize.IndexInData)
                {
                    int len = (store[Repeaters.IndexOfSize.IndexInData] & Repeaters.IndexOfSize.MaskInData) >> Repeaters.IndexOfSize.OffsetInData;
                    if (len > 0 && dataLength > len + Repeaters.Index.IndexInData)
                    {
                        ret = new byte[len];
                        for (int i = 0; i < len; i++)
                        {
                            ret[i] = store[Repeaters.Index.IndexInData + i];
                        }
                    }
                }
            }
            return ret;
        }

        ConcurrentDictionary<string, ItemReference> _itemReferencesCache = new ConcurrentDictionary<string, ItemReference>();

        public ushort GetParameter(byte[] store, string paramName, int dataLength)
        {
            ushort ret = 0;
            ItemReference paramRef = null;
            if (!_itemReferencesCache.TryGetValue(paramName, out paramRef))
            {
                paramRef = new ItemReference();
                paramRef.Index = new DataIndex(0);
                if (FrameDefinition.ProcessHeader(paramRef.Index, Param, paramName))
                {
                    paramRef.Ref = paramName;
                }
                _itemReferencesCache.TryAdd(paramName, paramRef);
            }
            if (paramRef != null && paramRef.Ref == paramName)
            {
                if (store.Length > paramRef.Index.IndexInData)
                {
                    ret = store[paramRef.Index.IndexInData];
                }
            }
            return ret;
        }

        public ushort GetDestination(byte[] store, int dataLength)
        {
            ushort ret = 0;
            if (!IsMulticast)
            {
                if (Destination != null)
                {
                    ItemReference iref = (ItemReference)Destination.Item;
                    ret = store[iref.Index.IndexInData];
                }
                else if (BaseHeaderDefinition.Destination != null)
                {
                    ret = BaseHeaderDefinition.GetDestination(store, dataLength);
                }
            }
            return ret;
        }

        public ushort[] GetDestinations(byte[] store, int dataLength)
        {
            ushort[] ret = null;
            if (IsMulticast)
            {
                ListReference lref = (ListReference)Destination.Item;
                int len = (store[lref.IndexOfSize.IndexInData] & lref.IndexOfSize.MaskInData) >> lref.IndexOfSize.OffsetInData;
                if (len > 0 && dataLength > len + lref.Index.IndexInData)
                {
                    IList<ushort> tmp = new List<ushort>();
                    byte nodeIdx = 0;
                    for (int i = 0; i < len; i++)
                    {
                        byte availabilityMask = store[lref.Index.IndexInData + i];
                        for (byte bit = 0; bit < 8; bit++)
                        {
                            nodeIdx++;
                            if ((availabilityMask & (1 << bit)) > 0)
                            {
                                tmp.Add(nodeIdx);
                            }
                        }
                    }
                    if (tmp.Count > 0)
                        ret = tmp.ToArray();
                }
            }
            else
            {
                if (Destination != null)
                {
                    ItemReference iref = (ItemReference)Destination.Item;
                    ret = new ushort[] { store[iref.Index.IndexInData] };
                }
                else if (BaseHeaderDefinition.Destination != null)
                {
                    ushort tmp = BaseHeaderDefinition.GetDestination(store, dataLength);
                    ret = new ushort[] { tmp };
                }
            }
            return ret;
        }

        private Param _prevVariableParam;
        private void ProcessDataIndexes()
        {
            _prevVariableParam = null;
            byte indexInData = BaseHeaderDefinition.Length;
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
                            IndexOfOpt = GetDataIndex(item.OptRef)
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
                            IndexOfOpt = GetDataIndex(item.OptRef),
                            IndexOfSize = GetDataIndex(item.SizeRef)
                        };
                        indexInData = 0;
                        _prevVariableParam = item;
                    }

                }
            }
        }

        private DataIndex GetDataIndex(string paramName)
        {
            if (!string.IsNullOrEmpty(paramName))
            {
                foreach (var item in Param)
                {
                    DataIndex ret;
                    if (item.Param1 != null && item.Param1.Count > 0)
                    {
                        foreach (var prm1 in item.Param1)
                        {
                            string[] tokens = paramName.Split('.');
                            if (tokens.Length == 2)
                            {
                                if (tokens[0] == item.Name && tokens[1] == prm1.Name)
                                {
                                    ret = prm1.ItemRef.Index;
                                    return ret;
                                }
                            }
                        }
                    }
                    if (item.Name == paramName)
                    {
                        ret = item.ItemRef.Index;
                        return ret;
                    }
                }
                foreach (var item in BaseHeaderDefinition.Param)
                {
                    DataIndex ret;
                    if (item.Param1 != null && item.Param1.Count > 0)
                    {
                        foreach (var prm1 in item.Param1)
                        {
                            string[] tokens = paramName.Split('.');
                            if (tokens.Length == 2)
                            {
                                if (tokens[0] == item.Name && tokens[1] == prm1.Name)
                                {
                                    ret = prm1.ItemRef.Index;
                                    return ret;
                                }
                            }
                        }
                    }
                    if (item.Name == paramName)
                    {
                        ret = item.ItemRef.Index;
                        return ret;
                    }
                }
            }
            return null;
        }
    }
}
