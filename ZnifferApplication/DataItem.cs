/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Utils.UI;
using ZWave.Enums;
using ZWave.Xml.Application;
using ZWave.Xml.FrameHeader;
using ZWave.ZnifferApplication.Enums;
using ZWave.Security;
using ZWave.CommandClasses;

namespace ZWave.ZnifferApplication
{
    [Serializable]
    public class DataItem : EntityBase
    {
        public DataItem()
        { }

        public DataItem(byte headerType, byte speed, byte[] data)
        {
            this.HeaderType = headerType;
            this.Speed = speed;
            SetData(data);
        }

        public SecurityManager SecurityManager { get; set; }
        public bool IsSubstituted
        {
            get { return _isSubstituted; }
            set
            {
                _isSubstituted = value;
                Notify("IsSubstituted");
            }
        }

        public byte[] Store
        {
            get { return _store; }
            private set
            {
                _store = value;
                Notify("Store");
            }
        }

        public sbyte CachedFilterStatus { get; set; }
        public byte Channel { get; set; }
        public DateTime CreatedAt { get; set; }

        public int Delta
        {
            get { return _delta; }
            set
            {
                _delta = value;
                Notify("Delta");
            }
        }

        public byte Frequency { get; set; }

        private byte _headerType;

        public byte HeaderType
        {
            get { return _headerType; }
            set
            {
                _headerType = value;
                Notify("HeaderType");
            }
        }

        public int LineNo { get; set; }
        public byte Rssi { get; set; }
        public byte Speed { get; set; }
        public ushort Systime { get; set; }
        public ushort WakeupCounter { get; set; }
        public WakeUpBeamTypes WakeUpBeamType { get; set; }
        public int Position { get; set; }
        public int Boxes { get; internal set; }
        public ApiTypes ApiType { get; set; }
        public ushort DataLength { get; set; }

        private bool _hasDestHop;
        private byte _destHop;

        public byte DestHop
        {
            get
            {
                if (!_hasDestHop)
                {
                    _destHop = HeaderStore.GetDestHop(HeaderType, Store, DataLength);
                    _hasDestHop = true;
                }
                return _destHop;
            }
        }

        private bool _isLow;
        private bool _hasIsLow;
        private bool _isAck;

        public bool IsLow
        {
            get
            {
                if (!_hasIsLow)
                {
                    _isLow = HeaderStore.GetIsLow(HeaderType, Store, DataLength);
                    _hasIsLow = true;
                }
                return _isLow;
            }
        }

        public bool IsAck
        {
            get
            {
                _isAck = HeaderStore.GetIsAck(HeaderType);
                return _isAck;
            }
        }

        private byte _routesCount;
        private bool _hasRoutesCount;

        public byte RoutesCount
        {
            get
            {
                if (!_hasRoutesCount)
                {
                    _routesCount = HeaderStore.GetRoutesCount(HeaderType, Store, DataLength);
                    _hasRoutesCount = true;
                }
                return _routesCount;
            }
        }

        private bool _isUnknownHeader;
        private bool _hasIsUnknownHeader;

        public bool IsUnknownHeader
        {
            get
            {
                if (!_hasIsUnknownHeader)
                {
                    _isUnknownHeader = HeaderStore.GetIsUnknown(HeaderType);
                    _hasIsUnknownHeader = true;
                }
                return _isUnknownHeader;
            }
        }

        private bool _isCrcOk;
        private bool _hasIsCrcOk;

        public bool IsCrcOk
        {
            get
            {
                if (!_hasIsCrcOk)
                {
                    _isCrcOk = HeaderStore.GetIsCrcOk(HeaderType);
                    _hasIsCrcOk = true;
                }
                return _isCrcOk;
            }
        }

        private bool _isWakeupBeam;
        private bool _hasIsWakeupBeam;

        public bool IsWakeupBeam
        {
            get
            {
                if (!_hasIsWakeupBeam)
                {
                    _isWakeupBeam = HeaderStore.GetIsWakeUpBeam(HeaderType);
                    _hasIsWakeupBeam = true;
                }
                return _isWakeupBeam;
            }
        }

        private byte _srcHop;
        private bool _hasSrcHop;

        public byte SrcHop
        {
            get
            {
                if (!_hasSrcHop)
                {
                    _srcHop = HeaderStore.GetSrcHop(HeaderType, Store, DataLength);
                    _hasSrcHop = true;
                }
                return _srcHop;
            }
        }

        private ushort _source;
        private bool _hasSource;

        //public byte Source
        //{
        //    get { return (byte)SourceLR; }
        //}

        public ushort Source
        {
            get
            {
                if (!_hasSource)
                {
                    if (ApiType == ApiTypes.Zniffer || ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                    {
                        _source = HeaderStore.GetSource(HeaderType, Store, DataLength);
                    }
                    else if (ApiType == ApiTypes.Basic)
                    {
                        _source = Channel;
                    }
                    else if (ApiType == ApiTypes.Text)
                    {
                        _source = Channel;
                    }
                    else
                    {
                        _source = Channel;
                    }
                    _hasSource = true;
                }
                return _source;
            }
        }

        private int _sequenceNumber;
        private bool _hasSequenceNumber;

        public int SequenceNumber
        {
            get
            {
                if (!_hasSequenceNumber)
                {
                    _sequenceNumber = HeaderStore.GetSequenceNumber(HeaderType, Store, DataLength);
                    _hasSequenceNumber = true;
                }
                return _sequenceNumber;
            }
        }

        public byte[] LastPayload
        {
            get
            {
                return CarryPayload.Last();
            }
        }

        private List<byte[]> _carryPayload;
        private bool _hasCarryPayload;

        public List<byte[]> CarryPayload
        {
            get
            {
                if (!_hasCarryPayload)
                {
                    if (_carryPayload == null)
                    {
                        _carryPayload = new List<byte[]>();
                    }
                    else
                    {
                        _carryPayload.Clear();
                    }
                    _carryPayload.Add(Payload);
                    FillExtensions(DataItemExtensionTypes.CarryPayload, _carryPayload);
                    _hasCarryPayload = true;
                }
                return _carryPayload;
            }
        }

        private byte[] _nonceS0;
        private bool _hasNonceS0;

        public byte[] NonceS0
        {
            get
            {
                if (!_hasNonceS0)
                {
                    _nonceS0 = GetExtension(DataItemExtensionTypes.NonceS0);
                    _hasNonceS0 = true;
                }
                return _nonceS0;
            }
        }

        private byte[] _nonceS0Part0;
        private bool _hasNonceS0Part0;

        public byte[] NonceS0Part0
        {
            get
            {
                if (!_hasNonceS0Part0)
                {
                    _nonceS0Part0 = GetExtension(DataItemExtensionTypes.NonceS0Part0);
                    _hasNonceS0Part0 = true;
                }
                return _nonceS0Part0;
            }
        }

        private byte[] _encapS0Part0;
        private bool _hasEncapS0Part0;

        public byte[] EncapS0Part0
        {
            get
            {
                if (!_hasEncapS0Part0)
                {
                    _encapS0Part0 = GetExtension(DataItemExtensionTypes.EncapS0Part0);
                    _hasEncapS0Part0 = true;
                }
                return _encapS0Part0;
            }
        }

        private byte[] _span;
        private bool _hasSpan;

        public byte[] Span
        {
            get
            {
                if (!_hasSpan)
                {
                    _span = GetExtension(DataItemExtensionTypes.Span);
                    _hasSpan = true;
                }
                return _span;
            }
        }

        private byte[] _mpan;
        private bool _hasMpan;

        public byte[] Mpan
        {
            get
            {
                if (!_hasMpan)
                {
                    _mpan = GetExtension(DataItemExtensionTypes.Mpan);
                    _hasMpan = true;
                }
                return _mpan;
            }
        }

        private void FillExtensions(DataItemExtensionTypes extensionType, List<byte[]> list)
        {

            if (ExtensionBuffer != null && ExtensionBuffer.Length > 2)
            {
                int index = 0;
                while (index + 2 < ExtensionBuffer.Length)
                {
                    DataItemExtensionTypes type = (DataItemExtensionTypes)ExtensionBuffer[index];
                    var len = (ExtensionBuffer[index + 1] << 8) + ExtensionBuffer[index + 2];
                    if (index + 2 + len <= ExtensionBuffer.Length)
                    {
                        if (ExtensionBuffer[index] == (byte)extensionType)
                        {
                            var tmp = new byte[len];
                            Array.Copy(ExtensionBuffer, index + 3, tmp, 0, len);
                            list.Add(tmp);
                        }
                    }
                    index += 3 + len;
                }
            }
        }

        private byte[] GetExtension(DataItemExtensionTypes extensionType)
        {
            byte[] ret = null;
            if (ExtensionBuffer != null && ExtensionBuffer.Length > 2)
            {
                int index = 0;
                while (index + 2 < ExtensionBuffer.Length)
                {
                    DataItemExtensionTypes type = (DataItemExtensionTypes)ExtensionBuffer[index];
                    var len = (ExtensionBuffer[index + 1] << 8) + ExtensionBuffer[index + 2];
                    if (index + 2 + len <= ExtensionBuffer.Length)
                    {
                        if (ExtensionBuffer[index] == (byte)extensionType)
                        {
                            var tmp = new byte[len];
                            Array.Copy(ExtensionBuffer, index + 3, tmp, 0, len);
                            ret = tmp;
                        }
                    }
                    index += 3 + len;
                }
            }
            return ret;
        }

        private byte[] _payload;
        private bool _hasPayload;

        public byte[] Payload
        {
            get
            {
                if (!_hasPayload)
                {
                    if (ApiType == ApiTypes.Zniffer || ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                    {
                        byte crcBytes = (byte)((Speed > 1) ? 0x02 : 0x01);
                        _payload = HeaderStore.GetPayload(HeaderType, Store, DataLength, crcBytes);
                    }
                    else
                    {
                        _payload = Store;
                    }
                    _hasPayload = true;
                }
                return _payload;
            }
        }

        private byte[] _dataBuffer;
        private bool _hasDataBuffer;

        public byte[] DataBuffer
        {
            get
            {
                if (!_hasDataBuffer)
                {
                    _dataBuffer = new byte[DataLength];
                    if (Store != null && Store.Length >= DataLength)
                    {
                        Array.Copy(Store, _dataBuffer, DataLength);
                    }
                    _hasDataBuffer = true;
                }
                return _dataBuffer;
            }
        }

        private byte[] _extensionBuffer;
        private bool _hasExtensionBuffer;

        public byte[] ExtensionBuffer
        {
            get
            {
                if (!_hasExtensionBuffer)
                {
                    if (Store != null && Store.Length > DataLength)
                    {
                        _extensionBuffer = new byte[Store.Length - DataLength];
                        Array.Copy(Store, DataLength, _extensionBuffer, 0, Store.Length - DataLength);
                    }
                    else
                    {
                        _extensionBuffer = null;
                    }
                    _hasExtensionBuffer = true;
                }
                return _extensionBuffer;
            }
        }

        private byte[] _homeId;
        private bool _hasHomeId;

        public byte[] HomeId
        {
            get
            {
                if (!_hasHomeId)
                {
                    if (ApiType == ApiTypes.Zniffer || ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                    {
                        _homeId = HeaderStore.GetHomeId(HeaderType, Store, DataLength);
                    }
                    else if (ApiType == ApiTypes.Basic)
                    {
                        _homeId = new byte[4];
                        _homeId[1] = (byte)ApiType;
                        _homeId[3] = Rssi;
                    }
                    _hasHomeId = true;
                }
                return _homeId;
            }
        }

        private ushort _homeIdHash;
        private bool _hasHomeIdHash;
        public ushort HomeIdHash
        {
            get
            {
                if (!_hasHomeIdHash)
                {
                    if (ApiType == ApiTypes.Zniffer || ApiType == ApiTypes.Pti)
                    {
                        _homeIdHash = HeaderStore.GetHomeIdHash(HeaderType, Store, DataLength, Frequency, ApiType);
                    }
                    _hasHomeIdHash = true;
                }
                return _homeIdHash;
            }
        }

        private ushort _destination;
        private bool _hasDestination;

        public ushort Destination
        {
            get
            {
                if (!_hasDestination)
                {
                    if (ApiType == ApiTypes.Zniffer || ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                    {
                        _destination = HeaderStore.GetDestination(HeaderType, Store, DataLength, Frequency, ApiType);
                    }
                    else if (ApiType == ApiTypes.Basic)
                    {
                        _destination = (ushort)((~Channel) & 0x01);
                    }
                    else if (ApiType == ApiTypes.Text)
                    {
                        _destination = (ushort)((~Channel) & 0x01);
                    }
                    else
                    {
                        _destination = (ushort)((~Channel) & 0x01);
                    }
                    _hasDestination = true;
                }
                return _destination;
            }

        }

        private ushort[] _destinations;
        private bool _hasDestinations;

        public ushort[] Destinations
        {
            get
            {
                if (!_hasDestinations)
                {
                    if (ApiType == ApiTypes.Zniffer || ApiType == ApiTypes.Pti || ApiType == ApiTypes.PtiDiagnostic)
                    {
                        _destinations = HeaderStore.GetDestinations(HeaderType, Store, DataLength, Frequency, ApiType);
                    }
                    else if (ApiType == ApiTypes.Basic)
                    {
                        _destinations = new ushort[] { (ushort)((~Channel) & 0x01) };
                    }
                    else if (ApiType == ApiTypes.Text)
                    {
                        _destinations = new ushort[] { (ushort)((~Channel) & 0x01) };
                    }
                    else
                    {
                        _destinations = new ushort[] { (ushort)((~Channel) & 0x01) };
                    }
                    _hasDestinations = true;
                }
                return _destinations;
            }
        }

        private byte[] _repeaters;
        private bool _hasRepeaters;
        private bool _isSubstituted;
        private byte[] _store;
        private int _delta;

        public byte[] Repeaters
        {
            get
            {
                if (!_hasRepeaters)
                {
                    _repeaters = HeaderStore.GetRepeaters(HeaderType, Store, DataLength);
                    _hasRepeaters = true;
                }
                return _repeaters;
            }
        }

        private HeaderValue _headerObject;

        public HeaderValue HeaderObject
        {
            get { return _headerObject; }
            set
            {
                _headerObject = value;
                Notify("HeaderObject");
            }
        }

        public static DataItem CreateFrom(IDataItemBox[] dataItemBox)
        {
            if (dataItemBox != null && dataItemBox.Length >= 0 && dataItemBox[0] is FirstDataItemBox)
            {
                var firstBox = (FirstDataItemBox)dataItemBox[0];
                int count = firstBox.BoxCount;
                int length = firstBox.BoxStoreLength;
                for (int i = 1; i < count; i++)
                {
                    length += dataItemBox[i].BoxStoreLength;
                }
                byte[] dataBuffer = new byte[length];

                firstBox.buffer.CopyTo(FirstDataItemBox.DATA_INDEX, dataBuffer, 0, firstBox.BoxStoreLength);
                int boxLength = firstBox.BoxStoreLength;
                for (int i = 1; i < count; i++)
                {
                    var box = (DataItemBox)dataItemBox[i];
                    box.buffer.CopyTo(DataItemBox.DATA_INDEX, dataBuffer, boxLength, box.BoxStoreLength);
                    boxLength += box.BoxStoreLength;
                }
                DataItem ret = new DataItem
                {
                    LineNo = ((FirstDataItemBox)dataItemBox[0]).LineNo,
                    CreatedAt = DateTime.FromBinary(firstBox.CreatedAt),
                    Frequency = firstBox.Frequency,
                    Speed = firstBox.Speed,
                    Rssi = firstBox.Rssi,
                    Channel = firstBox.Channel,
                    Systime = firstBox.Systime,
                    WakeupCounter = firstBox.WakeupCounter,
                    DataLength = firstBox.DataLength,
                    HeaderType = firstBox.HeaderType,
                    Store = dataBuffer,
                    ApiType = (ApiTypes)firstBox.ApiType
                };
                return ret;
            }
            return null;
        }

        public int ToDataItemBoxes(IDataItemBox[] boxes)
        {
            var firstBox = new FirstDataItemBox
            {
                LineNo = LineNo,
                SeqNo = 0,
                CreatedAt = CreatedAt.ToBinary(),
                Frequency = Frequency,
                Speed = Speed,
                Channel = Channel,
                Systime = Systime,
                Rssi = Rssi,
                ApiType = (byte)ApiType,
                HeaderType = HeaderType,
                WakeupCounter = WakeupCounter,
                DataLength = DataLength
            };
            firstBox.FillStore(Store, 0);
            int dataBufferIndex = firstBox.BoxStoreLength;
            int boxIndex = 0;
            while (Store != null && dataBufferIndex < Store.Length)
            {
                boxIndex++;
                var box = new DataItemBox { SeqNo = (ushort)boxIndex };
                box.FillStore(Store, dataBufferIndex);
                dataBufferIndex += box.BoxStoreLength;
                boxes[boxIndex] = box;
            }

            firstBox.BoxCount = (ushort)(boxIndex + 1);
            boxes[0] = firstBox;
            return boxIndex;
        }

        public void AddExtension(DataItemExtensionTypes type, byte[] data)
        {
            if (data != null && data.Length > 0 && data.Length < ushort.MaxValue)
            {
                if (Store != null && Store.Length > 0)
                {
                    byte[] newStore = new byte[Store.Length + data.Length + 3];
                    Array.Copy(Store, newStore, Store.Length);
                    newStore[Store.Length] = (byte)type;
                    newStore[Store.Length + 1] = (byte)(data.Length >> 8);
                    newStore[Store.Length + 2] = (byte)(data.Length);
                    Array.Copy(data, 0, newStore, Store.Length + 3, data.Length);
                    Store = newStore;
                }
                else
                {
                    byte[] newStore = new byte[data.Length + 3];
                    newStore[0] = (byte)type;
                    newStore[1] = (byte)(data.Length >> 8);
                    newStore[2] = (byte)(data.Length);
                    Array.Copy(data, 0, newStore, 3, data.Length);
                    Store = newStore;
                }
            }
            _hasExtensionBuffer = false;
            switch (type)
            {
                case DataItemExtensionTypes.CarryPayload:
                    _hasCarryPayload = false;
                    break;
                case DataItemExtensionTypes.NonceS0:
                    _hasNonceS0 = false;
                    break;
                case DataItemExtensionTypes.Span:
                    _hasSpan = false;
                    break;
                default:
                    break;
            }
        }

        public void SetData(byte[] data)
        {
            SetData(data, 0, data.Length);
        }

        public void SetData(byte[] data, int offset, int length)
        {
            if (data != null && data.Length < ushort.MaxValue)
            {
                DataLength = (ushort)length;
                Store = new byte[DataLength];
                Array.Copy(data, offset, Store, 0, DataLength);
            }
            else
            {
                DataLength = 0;
                Store = new byte[DataLength];
            }
            _hasDataBuffer = false;
            _hasDestHop = false;
            _hasDestination = false;
            _hasHomeId = false;
            _hasIsCrcOk = false;
            _hasIsLow = false;
            _hasIsUnknownHeader = false;
            _hasIsWakeupBeam = false;
            _hasPayload = false;
            _hasRepeaters = false;
            _hasRoutesCount = false;
            _hasSource = false;
            _hasSrcHop = false;
        }

        public static DataItem DeepClone(DataItem obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (DataItem)formatter.Deserialize(ms);
            }
        }

        public CipherDataItem Decrypt(SecurityManager securityManager)
        {
            CipherDataItem ret = new CipherDataItem();
            byte[] key = null;
            byte[] payload = null;
            Extensions s2extensions = null;
            byte s0properties = 0;
            if (Destinations != null && Destinations.Length > 0)
            {
                var sender = Source;
                var receiver = Destination;
                bool isDecrypted = false;
                var version = GetCipherVersion();

                if (version == 0)
                {
                    isDecrypted = securityManager.DecryptS0(this, out key, out payload, out s0properties);
                }
                else if (version == 2)
                {
                    isDecrypted = securityManager.DecryptS2(this, out key, out payload, out s2extensions);
                }
                ret = new CipherDataItem()
                {
                    HasData = true,
                    IsDecrypted = isDecrypted,
                    Key = key,
                    Payload = payload,
                    S2Extensions = s2extensions,
                    S0Properties = s0properties,
                    Version = (byte)version
                };
            }
            return ret;
        }

        public int GetCipherVersion()
        {
            int ret = -1;
            if (LastPayload != null && LastPayload.Length > 2)
            {
                if (LastPayload[0] == COMMAND_CLASS_SECURITY.ID && LastPayload[1] == COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
                {
                    ret = 0;
                }
                else if (LastPayload[0] == COMMAND_CLASS_SECURITY_2.ID && LastPayload[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
                {
                    ret = 2;
                }
            }
            return ret;
        }
    }

    public struct CipherDataItem
    {
        public bool HasData { get; set; }
        public bool IsDecrypted { get; set; }
        public byte[] Key { get; set; }
        public byte[] Payload { get; set; }
        public Extensions S2Extensions { get; set; }
        public byte S0Properties { get; set; }
        public byte Version { get; set; }
    }

    public enum DataItemExtensionTypes : byte
    {
        CarryPayload = 0x01,
        NonceS0 = 0x02,
        Span = 0x03,
        Mpan = 0x04,
        NonceS0Part0 = 0x05,
        EncapS0Part0 = 0x06
    }

    public static class HeaderStore
    {
        public static byte H_BROADCAST = 10;
        public static byte H_BROADCAST24EX = 11;
        public static byte H_BROADCAST24 = 12;

        public static byte H_SINGLECAST = 13;
        public static byte H_SINGLECAST24EX = 14;
        public static byte H_SINGLECAST24 = 15;
        public static byte H_SINGLECASTLR = 70;

        public static byte H_MULTICAST = 16;
        public static byte H_MULTICAST24EX = 17;
        public static byte H_MULTICAST24 = 18;

        public static byte H_ACK = 19;
        public static byte H_ACK24EX = 20;
        public static byte H_ACK24 = 21;

        public static byte H_ROUTED_SINGLECAST = 22;
        public static byte H_ROUTED_SINGLECAST24EX = 23;
        public static byte H_ROUTED_SINGLECAST24 = 24;

        public static byte H_ROUTED_ACK = 25;
        public static byte H_ROUTED_ACK24EX = 26;
        public static byte H_ROUTED_ACK24 = 27;

        public static byte H_ROUTED_ERROR = 28;
        public static byte H_ROUTED_ERROR24EX = 29;
        public static byte H_ROUTED_ERROR24 = 30;

        public static byte H_EXPLORER_NORMAL = 33;
        public static byte H_EXPLORER_NORMAL24 = 34;
        public static byte H_EXPLORER_SEARCH_RESULT = 35;
        public static byte H_EXPLORER_SEARCH_RESULT24 = 36;
        public static byte H_EXPLORER_AUTOINCLUSION = 37;
        public static byte H_EXPLORER_AUTOINCLUSION24 = 38;

        public static byte H_WAKEUP = 60;
        public static byte H_WAKEUP_START = 61;
        public static byte H_WAKEUP_STOP = 62;
        public static byte H_WAKEUP_LR = 63;

        public static byte H_CRC_FALSE = 50;
        public static byte H_UNKNOWN = 99;
        public static byte H_OTHER = 100;
        public static byte H_OTHER_CRC_FALSE = 101;
        public static byte H_OTHER_UNKNOWN = 102;

        public static Dictionary<byte, Header> Headers { get; set; }

        public static ushort GetSource(byte header, byte[] store, int dataLength)
        {
            ushort ret = 0;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].BaseHeaderDefinition.GetSource(store, dataLength);
            }
            return ret;
        }

        public static byte GetRoutesCount(byte header, byte[] store, int dataLength)
        {
            byte ret = 0;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].GetRoutesCount(store, dataLength);
            }
            return ret;
        }

        public static byte[] GetPayload(byte header, byte[] store, int dataLength, byte crcBytes)
        {
            byte[] ret = null;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                byte startIndex = Headers[header].GetLength(store, dataLength);
                if (startIndex > 0)
                {
                    int len = dataLength - startIndex - crcBytes;
                    if (len > 0)
                    {
                        ret = new byte[len];
                        Array.Copy(store, startIndex, ret, 0, ret.Length);
                    }
                }
            }
            return ret;
        }

        public static byte[] GetHomeId(byte header, byte[] store, int dataLength)
        {
            byte[] ret = null;
            if (store != null && !GetIsWakeUpBeam(header))
            {
                if (GetIsCrcOk(header) && GetIsFrame(header))
                {
                    ret = Headers[header].BaseHeaderDefinition.GetHomeId(store, dataLength);
                }
                else if (store != null && store.Length > 4)
                {
                    ret = new byte[4];
                    Array.Copy(store, ret, 4);
                }
            }
            return ret;
        }

        public static ushort GetHomeIdHash(byte header, byte[] store, int dataLength, byte frequency, ApiTypes apiType)
        {
            ushort ret = 0;
            if (store != null && (header == H_WAKEUP || header == H_WAKEUP_LR))
            {
                ret = Headers[header].GetParameter(store, "HomeIDHash", dataLength);
            }
            return ret;
        }

        public static ushort GetDestination(byte header, byte[] store, int dataLength, byte frequency, ApiTypes apiType)
        {
            ushort ret = 0;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].GetDestination(store, dataLength);
            }
            else if (store != null && (header == H_WAKEUP || header == H_WAKEUP_LR))
            {
                if (dataLength > 2 && apiType == ApiTypes.Pti && frequency >= 0x0C /* Long Range */)
                    ret = (ushort)(((store[dataLength - 2] & 0x0F) << 8) + store[dataLength - 1]);
                else if (dataLength > 1)
                    ret = (ushort)store[dataLength - 1];
            }
            else if (store != null && header == H_WAKEUP_START)
            {
                if (dataLength > 2 && apiType == ApiTypes.Pti && frequency >= 0x0C /* Long Range */)
                    ret = (ushort)(((store[1] & 0x0F) << 8) + store[2]);
                else if (dataLength > 1)
                    ret = store[1];
            }
            return ret;
        }

        public static ushort[] GetDestinations(byte header, byte[] store, int dataLength, byte frequency, ApiTypes apiType)
        {
            ushort[] ret = null;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].GetDestinations(store, dataLength);
            }
            else if (store != null && (header == H_WAKEUP || header == H_WAKEUP_LR))
            {
                if (dataLength > 3 && apiType == ApiTypes.Pti && frequency >= 0x0C /* Long Range */)
                    ret = new[] { (ushort)(((store[dataLength - 3] & 0x0F) << 8) + store[dataLength - 2]) };
                else if (dataLength > 2)
                    ret = new[] { (ushort)store[dataLength - 2] };
            }
            else if (store != null && header == H_WAKEUP_START)
            {
                if (dataLength > 2 && apiType == ApiTypes.Pti && frequency >= 0x0C /* Long Range */)
                    ret = new[] { (ushort)(((store[1] & 0x0F) << 8) + store[2]) };
                else if (dataLength > 1)
                    ret = new[] { (ushort)store[1] };
            }
            return ret;
        }

        public static byte[] GetRepeaters(byte header, byte[] store, int dataLength)
        {
            byte[] ret = null;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].GetRepeaters(store, dataLength);
            }
            return ret;
        }

        public static byte GetSrcHop(byte header, byte[] store, int dataLength)
        {
            byte ret = 0;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                byte hops = Headers[header].GetHops(store, dataLength);
                byte[] repeaters = Headers[header].GetRepeaters(store, dataLength);
                if (repeaters != null && repeaters.Length > 0)
                {
                    #region routed

                    // Hops (4 bits):
                    // The value of this field plus one is the number of hops the frame has to pass. 
                    // The value is initially equal to 'repeaters minus one' when this frame is issued. 
                    // The value is 0xF when the frame has reached the destination.
                    if (Headers[header].IsAck || Headers[header].IsError)
                    {
                        if (hops != 0x0F)
                        {
                            if (hops + 1 < repeaters.Length)
                            {
                                ret = repeaters[hops + 1];
                            }
                        }
                        else
                        {
                            ret = repeaters[0];
                        }

                        if (hops != 0x0F && hops < repeaters.Length)
                        {
                            //destHop = _repeaters[_hops];
                        }
                    }
                    // Hops (4 bits):
                    // Number of hops the frame has passed. 
                    // The value is initially 0 (zero) when the frame is issued.
                    else
                    {
                        if (hops != 0 && hops - 1 < repeaters.Length)
                        {
                            ret = repeaters[(hops - 1)];
                        }

                        if (hops < repeaters.Length)
                        {
                            //destHop = _repeaters[_hops];
                        }
                    }

                    #endregion
                }
            }
            return ret;
        }

        public static byte GetDestHop(byte header, byte[] store, int dataLength)
        {
            byte ret = 0;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                byte hops = Headers[header].GetHops(store, dataLength);
                byte[] repeaters = Headers[header].GetRepeaters(store, dataLength);
                if (repeaters != null && repeaters.Length > 0)
                {
                    #region routed

                    // Hops (4 bits):
                    // The value of this field plus one is the number of hops the frame has to pass. 
                    // The value is initially equal to 'repeaters minus one' when this frame is issued. 
                    // The value is 0xF when the frame has reached the destination.
                    if (Headers[header].IsAck || Headers[header].IsError)
                    {
                        if (hops != 0x0F)
                        {
                            if (hops + 1 < repeaters.Length)
                            {
                                //srcHop = _repeaters[_hops + 1];
                            }
                        }

                        if (hops != 0x0F && hops < repeaters.Length)
                        {
                            ret = repeaters[hops];
                        }
                    }
                    // Hops (4 bits):
                    // Number of hops the frame has passed. 
                    // The value is initially 0 (zero) when the frame is issued.
                    else
                    {
                        if (hops != 0 && hops - 1 < repeaters.Length)
                        {
                            //srcHop = _repeaters[(_hops - 1)];
                        }

                        if (hops < repeaters.Length)
                        {
                            ret = repeaters[hops];
                        }
                    }

                    #endregion
                }
            }
            return ret;
        }

        public static bool GetIsLow(byte header, byte[] store, int dataLength)
        {
            bool ret = false;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].BaseHeaderDefinition.GetIsLtx(store, dataLength);
            }
            return ret;
        }

        public static bool GetIsCrcOk(byte header)
        {
            return header != H_CRC_FALSE;
        }

        public static bool GetIsWakeUpBeam(byte header)
        {
            return header == H_WAKEUP || header == H_WAKEUP_START || header == H_WAKEUP_STOP || header == H_WAKEUP_LR;
        }

        public static bool GetIsSinglecast(byte header)
        {
            return header == H_SINGLECAST || header == H_SINGLECAST24 || header == H_SINGLECAST24EX || header == H_SINGLECASTLR;
        }

        public static bool GetIsBroadcast(byte header)
        {
            return header == H_BROADCAST || header == H_BROADCAST24 || header == H_BROADCAST24EX;
        }

        public static bool GetIsMulticast(byte header)
        {
            return header == H_MULTICAST || header == H_MULTICAST24 || header == H_MULTICAST24EX;
        }

        public static bool GetIsAck(byte header)
        {
            return header == H_ACK || header == H_ACK24 || header == H_ACK24EX;
        }

        public static bool GetIsRoutedSinglecast(byte header)
        {
            return header == H_ROUTED_SINGLECAST || header == H_ROUTED_SINGLECAST24 ||
                   header == H_ROUTED_SINGLECAST24EX;
        }

        public static bool GetIsRoutedAck(byte header)
        {
            return header == H_ROUTED_ACK || header == H_ROUTED_ACK24 || header == H_ROUTED_ACK24EX;
        }

        public static bool GetIsRoutedError(byte header)
        {
            return header == H_ROUTED_ERROR || header == H_ROUTED_ERROR24 || header == H_ROUTED_ERROR24EX;
        }

        public static bool GetIsExplorer(byte header)
        {
            return header == H_EXPLORER_AUTOINCLUSION || header == H_EXPLORER_AUTOINCLUSION24 ||
                   header == H_EXPLORER_NORMAL || header == H_EXPLORER_NORMAL24 ||
                   header == H_EXPLORER_SEARCH_RESULT || header == H_EXPLORER_SEARCH_RESULT24;
        }

        public static bool GetIsFrame(byte header)
        {
            return header != H_UNKNOWN && Headers.ContainsKey(header);
        }

        public static bool GetIsUnknown(byte header)
        {
            return header == H_UNKNOWN;
        }

        public static int GetSequenceNumber(byte header, byte[] store, int dataLength)
        {
            int ret = -1;
            if (store != null && GetIsCrcOk(header) && GetIsFrame(header) && !GetIsWakeUpBeam(header))
            {
                ret = Headers[header].BaseHeaderDefinition.GetSequenceNumber(store, dataLength);
            }
            return ret;
        }
    }
}