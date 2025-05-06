/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Utils;
using System.Xml.Serialization;
using System.Text;
using System.Globalization;

namespace ZWave.ZnifferApplication
{
    public class FilterObject : INotifyPropertyChanged
    {
        public FilterObject()
        {
            ExpectedFramesCount = 1;
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        private int mFilteredCount;
        public int FilteredCount
        {
            get { return mFilteredCount; }
            set
            {
                mFilteredCount = value;
                RaisePropertyChanged("FilteredCount");
            }
        }

        public int FramesCount { get; set; }
        public int ExpectedFramesCount { get; set; }

        private Func<DataItem, bool> mFilterCallback;
        [XmlIgnore]
        public Func<DataItem, bool> FilterCallback
        {
            get { return mFilterCallback; }
            set
            {
                mFilterCallback = value;
                RaisePropertyChanged("FilterCallback");
            }
        }

        private bool mIsDisplayFilterCallback = true;
        public bool IsDisplayFilterCallback
        {
            get { return mIsDisplayFilterCallback; }
            set
            {
                mIsDisplayFilterCallback = value;
                RaisePropertyChanged("IsDisplayFilterCallback");
            }
        }

        private bool? mFilterIsLow;
        public bool? FilterIsLow
        {
            get { return mFilterIsLow; }
            set
            {
                mFilterIsLow = value;
                RaisePropertyChanged("FilterIsLow");
            }
        }

        private bool mIsDisplayIsLow = true;
        public bool IsDisplayIsLow
        {
            get { return mIsDisplayIsLow; }
            set
            {
                mIsDisplayIsLow = value;
                RaisePropertyChanged("IsDisplayIsLow");
            }
        }

        private byte[] mFilterSpeed;
        public byte[] FilterSpeed
        {
            get { return mFilterSpeed; }
            set
            {
                mFilterSpeed = value;
                RaisePropertyChanged("FilterSpeed");
            }
        }

        private bool mIsDisplaySpeed = true;
        public bool IsDisplaySpeed
        {
            get { return mIsDisplaySpeed; }
            set
            {
                mIsDisplaySpeed = value;
                RaisePropertyChanged("IsDisplaySpeed");
            }
        }

        private sbyte? mFilterRssi;
        public sbyte? FilterRssi
        {
            get { return mFilterRssi; }
            set
            {
                mFilterRssi = value;
                RaisePropertyChanged("FilterRssi");
            }
        }

        private bool mIsDisplayRssi = true;
        public bool IsDisplayRssi
        {
            get { return mIsDisplayRssi; }
            set
            {
                mIsDisplayRssi = value;
                RaisePropertyChanged("IsDisplayRssi");
            }
        }

        private List<ByteIndex[]> mFilterRepeaters;
        public List<ByteIndex[]> FilterRepeaters
        {
            get { return mFilterRepeaters; }
            set
            {
                mFilterRepeaters = value;
                RaisePropertyChanged("FilterRepeaters");
            }
        }

        private bool mIsDisplayRepeaters = true;
        public bool IsDisplayRepeaters
        {
            get { return mIsDisplayRepeaters; }
            set
            {
                mIsDisplayRepeaters = value;
                RaisePropertyChanged("IsDisplayRepeaters");
            }
        }

        private ushort[] _filterNodeId;
        public ushort[] FilterNodeId
        {
            get { return _filterNodeId; }
            set
            {
                _filterNodeId = value;
                RaisePropertyChanged("FilterNodeId");
            }
        }

        private bool mIsDisplayNodeId = true;
        public bool IsDisplayNodeId
        {
            get { return mIsDisplayNodeId; }
            set
            {
                mIsDisplayNodeId = value;
                RaisePropertyChanged("IsDisplayNodeId");
            }
        }

        private ushort[] _filterSource;
        public ushort[] FilterSource
        {
            get { return _filterSource; }
            set
            {
                _filterSource = value;
                RaisePropertyChanged("FilterSource");
            }
        }


        private bool mIsDisplaySource = true;
        public bool IsDisplaySource
        {
            get { return mIsDisplaySource; }
            set
            {
                mIsDisplaySource = value;
                RaisePropertyChanged("IsDisplaySource");
            }
        }

        private byte[] _filterSourceHop;
        public byte[] FilterSourceHop
        {
            get { return _filterSourceHop; }
            set
            {
                _filterSourceHop = value;
                RaisePropertyChanged("FilterSourceHop");
            }
        }

        private bool mIsDisplaySourceHop = true;
        public bool IsDisplaySourceHop
        {
            get { return mIsDisplaySourceHop; }
            set
            {
                mIsDisplaySourceHop = value;
                RaisePropertyChanged("IsDisplaySourceHop");
            }
        }

        private byte[] _filterDestinationHop;
        public byte[] FilterDestinationHop
        {
            get { return _filterDestinationHop; }
            set
            {
                _filterDestinationHop = value;
                RaisePropertyChanged("FilterDestinationHop");
            }
        }

        private bool mIsDisplayDestinationHop = true;
        public bool IsDisplayDestinationHop
        {
            get { return mIsDisplayDestinationHop; }
            set
            {
                mIsDisplayDestinationHop = value;
                RaisePropertyChanged("IsDisplayDestinationHop");
            }
        }

        private ushort[] _filterDestination;
        public ushort[] FilterDestination
        {
            get { return _filterDestination; }
            set
            {
                _filterDestination = value;
                RaisePropertyChanged("FilterDestination");
            }
        }

        private bool mIsDisplayDestination = true;
        public bool IsDisplayDestination
        {
            get { return mIsDisplayDestination; }
            set
            {
                mIsDisplayDestination = value;
                RaisePropertyChanged("IsDisplayDestination");
            }
        }

        private List<ByteIndex[]> _filterHomeId;
        public List<ByteIndex[]> FilterHomeId
        {
            get { return _filterHomeId; }
            set
            {
                _filterHomeId = value;
                RaisePropertyChanged("FilterHomeId");
            }
        }

        private bool mIsDisplayHomeId = true;
        public bool IsDisplayHomeId
        {
            get { return mIsDisplayHomeId; }
            set
            {
                mIsDisplayHomeId = value;
                RaisePropertyChanged("IsDisplayHomeId");
            }
        }

        private byte[] mFilterData;
        public byte[] FilterData
        {
            get { return mFilterData; }
            set
            {
                mFilterData = value;
                RaisePropertyChanged("FilterData");
            }
        }

        private string _filterPayloadString;
        public string FilterPayloadString
        {
            get { return _filterPayloadString; }
            set
            {
                _filterPayloadString = value;
                RaisePropertyChanged("FilterPayloadString");
            }
        }

        private FilterPayloadObject mFilterPayload;
        public FilterPayloadObject FilterPayload
        {
            get { return mFilterPayload; }
            set
            {
                mFilterPayload = value;
                RaisePropertyChanged("FilterPayload");
            }
        }

        private bool mIsDisplayPayload = true;
        public bool IsDisplayPayload
        {
            get { return mIsDisplayPayload; }
            set
            {
                mIsDisplayPayload = value;
                RaisePropertyChanged("IsDisplayPayload");
            }
        }

        private List<ByteIndex[]> _filterHex;
        public List<ByteIndex[]> FilterHex
        {
            get { return _filterHex; }
            set
            {
                _filterHex = value;
                RaisePropertyChanged("FilterHex");
            }
        }

        private bool mIsDisplayHex = true;
        public bool IsDisplayHex
        {
            get { return mIsDisplayHex; }
            set
            {
                mIsDisplayHex = value;
                RaisePropertyChanged("IsDisplayHex");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        public bool HasFilter
        {
            get
            {
                return
                    FilterCallback != null ||
                    FilterPayload != null ||
                    FilterPayloadString != null ||
                    FilterHex != null ||
                    FilterData != null ||
                    FilterDestination != null ||
                    FilterDestinationHop != null ||
                    FilterRepeaters != null ||
                    FilterHomeId != null ||
                    FilterSpeed != null ||
                    FilterRssi != null ||
                    FilterNodeId != null ||
                    FilterSource != null ||
                    FilterSourceHop != null ||
                    FilterNodeId != null;
            }
        }

        public void Reset()
        {
            FilterCallback = null;
            FilteredCount = 0;
            FilterPayload = null;
            FilterPayloadString = null;
            FilterHex = null;
            FilterData = null;
            FilterDestination = null;
            FilterHomeId = null;
            FilterSpeed = null;
            FilterRssi = null;
            FilterNodeId = null;
            FilterSource = null;
            FilterSourceHop = null;
            FilterDestinationHop = null;
            FilterRepeaters = null;
        }

        public void FillFrom(FilterObject src)
        {
            Reset();
            if (src != null)
            {
                FilterCallback = src.FilterCallback;
                FilterPayload = src.FilterPayload;
                FilterPayloadString = src.FilterPayloadString;
                FilterHex = src.FilterHex;
                FilterData = src.FilterData;
                FilterDestination = src.FilterDestination;
                FilterHomeId = src.FilterHomeId;
                FilterNodeId = src.FilterNodeId;
                FilterSource = src.FilterSource;
                FilterSourceHop = src.FilterSourceHop;
                FilterDestinationHop = src.FilterDestinationHop;
                FilterRepeaters = src.FilterRepeaters;
                FilterSpeed = src.FilterSpeed;
                FilterRssi = src.FilterRssi;
                FilterIsLow = src.FilterIsLow;

                IsDisplayFilterCallback = src.IsDisplayFilterCallback;
                IsDisplayPayload = src.IsDisplayPayload;
                IsDisplayHex = src.IsDisplayHex;
                IsDisplayDestination = src.IsDisplayDestination;
                IsDisplayDestinationHop = src.IsDisplayDestinationHop;
                IsDisplayHomeId = src.IsDisplayHomeId;
                IsDisplayNodeId = src.IsDisplayNodeId;
                IsDisplaySource = src.IsDisplaySource;
                IsDisplaySourceHop = src.IsDisplaySourceHop;
                IsDisplaySpeed = src.IsDisplaySpeed;
            }
        }

        public bool FilterIt(DataItem ditem, SecurityManager securityManager)
        {
            if (!HasFilter)
                return false;
            if (ditem == null)
            {
                return true;
            }

            bool ret = true;
            {
                try
                {
                    ret = ApplyFilterState(ditem, securityManager);
                }
                catch (Exception ex)
                {
                    ex.Message._EXLOG();
                }
            }
            if (ret)
            {
                FilteredCount++;
            }
            return ret;
        }

        protected virtual bool ApplyFilterState(DataItem ditem, SecurityManager securityManager)
        {
            bool ret = true;
            DataItem zf = ditem;

            if (ret && FilterIsLow != null)
            {
                ret = BoolFilterIt(zf.IsLow, IsDisplayIsLow, FilterIsLow);
            }

            if (ret && FilterSpeed != null)
            {
                ret = ValueFilterIt(zf.Speed, IsDisplaySpeed, FilterSpeed);
            }

            if (ret && FilterRssi != null)
            {
                ret = SByteValueGreaterOrEqualsFilterIt(zf.Rssi, IsDisplayRssi, FilterRssi.Value);
            }

            if (ret && FilterHex != null)
            {
                ret = ArrayFilterIt(zf.DataBuffer, IsDisplayHex, FilterHex, false);
            }

            if (ret && FilterNodeId != null)
            {
                ret = ValueFilterIt(zf.Source, IsDisplayNodeId, FilterNodeId);
                ret |= ValueFilterIt(zf.SrcHop, IsDisplayNodeId, FilterNodeId);
                ret |= FilterDestinationInner(zf.Destinations, IsDisplayNodeId, FilterNodeId);
                ret |= ValueFilterIt(zf.DestHop, IsDisplayNodeId, FilterNodeId);
            }

            if (ret && FilterSource != null)
            {
                ret = ValueFilterIt(zf.Source, IsDisplaySource, FilterSource);
            }

            if (ret && FilterSourceHop != null)
            {
                ret = ValueFilterIt(zf.SrcHop, IsDisplaySourceHop, FilterSourceHop);
            }

            if (ret && FilterDestination != null)
            {
                ret = FilterDestinationInner(zf.Destinations, IsDisplayDestination, FilterDestination);
            }

            if (ret && FilterDestinationHop != null)
            {
                ret = ValueFilterIt(zf.DestHop, IsDisplayDestinationHop, FilterDestinationHop);
            }

            if (ret && FilterRepeaters != null)
            {
                ret = ArrayFilterIt(zf.Repeaters, IsDisplayRepeaters, FilterRepeaters, true);
            }

            if (ret && FilterHomeId != null)
            {
                ret = ArrayFilterIt(zf.HomeId, IsDisplayHomeId, FilterHomeId, false);
            }

            if (ret && FilterData != null)
            {
                ret = ValueFilterIt(zf.HeaderType, true, FilterData);
            }

            if (ret && FilterPayload != null)
            {
                bool passRes = false;
                if (zf.CarryPayload != null)
                {
                    CipherDataItem cipherBlock = new CipherDataItem();
                    if (FilterPayload.Indexes == null || FilterPayload.Indexes.Count == 0)
                    {
                        passRes = true;
                    }
                    else
                    {
                        foreach (var payload in zf.CarryPayload)
                        {
                            var res = ArrayFilterIt(payload, IsDisplayPayload, FilterPayload.Indexes, false);
                            if (!(IsDisplayPayload ^ res))
                            {
                                passRes = res;
                                break;
                            }
                            else
                            {
                                passRes |= res;
                            }
                        }

                        if ((IsDisplayPayload ^ passRes))
                        {
                            cipherBlock = zf.Decrypt(securityManager);
                            if (cipherBlock.IsDecrypted)
                            {
                                passRes = ArrayFilterIt(cipherBlock.Payload, IsDisplayPayload, FilterPayload.Indexes, false);
                            }
                        }
                    }
                    if (passRes && FilterPayload.CipherExtensionType != null && IsDisplayPayload)
                    {
                        if (zf.GetCipherVersion() >= 0)
                        {
                            if (!cipherBlock.HasData)
                            {
                                cipherBlock = zf.Decrypt(securityManager);
                            }
                            if (cipherBlock.S2Extensions != null)
                            {
                                if (FilterPayload.CipherExtensionEncrypted != null)
                                {
                                    if ((bool)(FilterPayload.CipherExtensionEncrypted))
                                    {
                                        passRes = cipherBlock.S2Extensions.EncryptedExtensionsList.
                                      Where(x => (x.properties1.type == (byte)FilterPayload.CipherExtensionType) || (byte)FilterPayload.CipherExtensionType == 0x00).Any();
                                    }
                                    else
                                    {
                                        passRes = cipherBlock.S2Extensions.ExtensionsList.
                                          Where(x => (x.properties1.type == (byte)FilterPayload.CipherExtensionType) || (byte)FilterPayload.CipherExtensionType == 0x00).Any();
                                    }
                                }
                                else
                                {
                                    passRes = cipherBlock.S2Extensions.ExtensionsList.Union(cipherBlock.S2Extensions.EncryptedExtensionsList).
                                        Where(x => (x.properties1.type == (byte)FilterPayload.CipherExtensionType) || (byte)FilterPayload.CipherExtensionType == 0x00).Any();
                                }
                            }
                            else
                            {
                                passRes = false;
                            }
                        }
                        else
                        {
                            passRes = false;
                        }
                    }
                }
                ret = passRes;
            }

            if (ret && FilterCallback != null)
            {
                ret = !(FilterCallback(zf) ^ IsDisplayFilterCallback);
            }

            return ret;
        }

        private bool FilterDestinationInner(ushort[] destinations, bool isDisplayDestination, ushort[] filterDestination)
        {
            bool ret = !isDisplayDestination;
            if (destinations != null)
            {
                bool result = false;
                foreach (var item in filterDestination)
                {
                    result = destinations.Contains(item);
                    if (result)
                        break;
                }
                ret = !(!result ^ !isDisplayDestination);
            }
            return ret;
        }

        private bool BoolFilterIt(bool data, bool isDisplay, bool? filter)
        {
            bool result = data == filter;
            return !(result ^ isDisplay);
        }

        private bool ValueFilterIt(byte data, bool isDisplay, byte[] filter)
        {
            bool result = filter.Contains(data);
            return !(result ^ isDisplay);
        }

        private bool ValueFilterIt(ushort data, bool isDisplay, ushort[] filter)
        {
            bool result = filter.Contains(data);
            return !(result ^ isDisplay);
        }

        private bool SByteValueGreaterOrEqualsFilterIt(byte data, bool isDisplay, sbyte filter)
        {
            bool result = ((sbyte)data) >= filter;
            return !(result ^ isDisplay);
        }

        private bool ArrayFilterIt(byte[] data, bool isDisplay, List<ByteIndex[]> indexes, bool compareLength)
        {
            bool ret = !isDisplay;
            if (data != null)
            {
                bool result = false;
                foreach (var item in indexes)
                {
                    if (!compareLength || (data.Length == item.Length))
                    {
                        for (int i = 0; i < item.Length; i++)
                        {
                            if (i < data.Length)
                            {
                                if (item[i].Presence == Presence.Value)
                                {
                                    result = (data[i] & item[i].MaskInData) == item[i].Value;
                                    if (!result)
                                    {
                                        break;
                                    }
                                }
                                else if (item[i].Presence == Presence.AnyValue)
                                {
                                    result = true;
                                }
                            }
                            else
                            {
                                result = false;
                                break;
                            }
                        }
                        if (result)
                        {
                            ret = isDisplay;
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        public static string GetHexDataString(List<ByteIndex[]> list)
        {
            string ret = null;
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in list)
                {
                    foreach (var bIndex in item)
                    {
                        switch (bIndex.Presence)
                        {
                            case Presence.Value:
                                sb.Append(Tools.GetHex(bIndex.Value, false));
                                sb.Append(" ");
                                break;
                            case Presence.AnyValue:
                                sb.Append("? ");
                                break;
                        }
                    }
                    sb.Append("; ");
                }
                ret = sb.ToString();
            }
            return ret;
        }

        public static List<ByteIndex[]> GetHexDataList(string str)
        {
            List<ByteIndex[]> ret = null;
            if (str != null)
            {
                ret = new List<ByteIndex[]>();
                string textValue = str;
                if (textValue.Length > 1)
                {
                    string[] tokens = textValue.Split(';');
                    foreach (var item in tokens)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            string itemTrunk = item.Replace(" ", "");
                            List<ByteIndex> tt = new List<ByteIndex>();
                            string[] subtokens = itemTrunk.Split('?');
                            bool isFirst = true;
                            foreach (var it in subtokens)
                            {
                                List<byte> innerValue = new List<byte>();
                                if (isFirst)
                                {
                                    isFirst = false;
                                }
                                else
                                {
                                    tt.Add(ByteIndex.AnyValue);
                                }

                                string s = it.Trim();
                                if (!string.IsNullOrEmpty(s))
                                {
                                    int iter = 0;
                                    while (s.Length >= iter + 2)
                                    {
                                        string sItem = s.Substring(iter, 2);
                                        iter += 2;
                                        byte bt;
                                        if (byte.TryParse(sItem, NumberStyles.HexNumber, null, out bt))
                                        {
                                            innerValue.Add(bt);
                                        }
                                    }
                                }
                                tt.AddRange(innerValue.Select(vit => new ByteIndex(vit)));
                            }
                            if (tt.Count > 0)
                                ret.Add(tt.ToArray());
                        }
                    }
                }
            }
            return ret;
        }

        public static string GetHomeIdString(List<ByteIndex[]> list)
        {
            string ret = null;
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in list)
                {
                    foreach (var bIndex in item)
                    {
                        switch (bIndex.Presence)
                        {
                            case Presence.Value:
                                sb.Append(Tools.GetHex(bIndex.Value, false));
                                sb.Append(" ");
                                break;
                            case Presence.AnyValue:
                                sb.Append("? ");
                                break;
                        }
                    }
                    sb.Append("; ");
                }
                ret = sb.ToString();
            }
            return ret;
        }

        public static List<ByteIndex[]> GetHomeIdList(string str)
        {
            List<ByteIndex[]> ret = null;
            if (!string.IsNullOrWhiteSpace(str))
            {
                ret = new List<ByteIndex[]>();
                string textValue = str;
                if (textValue.Length > 1)
                {
                    string[] tokens = textValue.Split(';');
                    foreach (var item in tokens)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string itemTrunk = item.Replace(" ", "");
                            List<ByteIndex> tt = new List<ByteIndex>();
                            string[] subtokens = itemTrunk.Split('?');
                            bool isFirst = true;
                            foreach (var it in subtokens)
                            {
                                List<byte> innerValue = new List<byte>();
                                if (isFirst)
                                {
                                    isFirst = false;
                                }
                                else
                                {
                                    if (tt.Count < 4)
                                        tt.Add(ByteIndex.AnyValue);
                                }

                                string s = it.Trim();
                                if (!string.IsNullOrEmpty(s))
                                {
                                    int iter = 0;
                                    while (s.Length >= iter + 2)
                                    {
                                        string sItem = s.Substring(iter, 2);
                                        iter += 2;
                                        byte bt;
                                        if (byte.TryParse(sItem, NumberStyles.HexNumber, null, out bt))
                                        {
                                            innerValue.Add(bt);
                                        }
                                    }
                                }
                                foreach (var vit in innerValue)
                                {
                                    if (tt.Count < 4)
                                        tt.Add(new ByteIndex(vit));
                                }
                            }
                            if (tt.Count > 0)
                                ret.Add(tt.ToArray());
                        }
                    }
                }
            }
            return ret;
        }

        public static string GetSByteString(sbyte? value)
        {
            string ret = null;
            if (value != null)
            {
                ret = value.ToString();
            }
            return ret;
        }

        public static sbyte? GetSByte(string value)
        {
            sbyte? ret = null;
            if (!string.IsNullOrWhiteSpace(value) && sbyte.TryParse(value, out sbyte tmp))
            {
                ret = tmp;
            }
            return ret;
        }

        public static string GetNodesString(byte[] value)
        {
            string ret = null;
            if (value != null && value.Length > 0)
            {
                try
                {
                    ret = Tools.GetNodeIds(value, "; ");
                }
                catch (Exception ex)
                {
                    ex.Message._EXLOG();
                }
            }
            return ret;
        }

        public static string GetNodesString(ushort[] value)
        {
            string ret = null;
            if (value != null && value.Length > 0)
            {
                try
                {
                    ret = Tools.GetNodeIds(value, "; ");
                }
                catch (Exception ex)
                {
                    ex.Message._EXLOG();
                }
            }
            return ret;
        }

        public static byte[] GetNodesBytes(string value)
        {
            byte[] ret = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string[] destTokens = value.Split(' ', ',', ';', '\t');
                    {
                        List<byte> tmp = new List<byte>();
                        foreach (string item in destTokens)
                        {
                            string sItem = item.Trim();
                            if (!string.IsNullOrEmpty(sItem))
                            {
                                byte bt;
                                if (byte.TryParse(sItem, out bt))
                                {
                                    tmp.Add(bt);
                                }
                                else
                                    tmp.Add(0x00);
                            }
                        }
                        ret = tmp.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message._EXLOG();
            }
            return ret;
        }

        public static ushort[] GetNodesWords(string value)
        {
            ushort[] ret = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string[] destTokens = value.Split(' ', ',', ';', '\t');
                    {
                        List<ushort> tmp = new List<ushort>();
                        foreach (string item in destTokens)
                        {
                            string sItem = item.Trim();
                            if (!string.IsNullOrEmpty(sItem))
                            {
                                ushort bt;
                                if (ushort.TryParse(sItem, out bt))
                                {
                                    tmp.Add(bt);
                                }
                                else
                                    tmp.Add(0x0000);
                            }
                        }
                        ret = tmp.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message._EXLOG();
            }
            return ret;
        }
    }

    public class FilterPayloadObject
    {
        public byte? CipherExtensionType { get; set; }
        public bool? CipherExtensionEncrypted { get; set; }
        public List<ByteIndex[]> Indexes { get; set; }

        public FilterPayloadObject()
        {
            Indexes = new List<ByteIndex[]>();
        }
    }
}
