/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils
{
    public enum Presence
    {
        Value,
        ExceptValue,
        AnyValue
    }

    [Serializable]
    public struct ByteIndex
    {
        public static ByteIndex AnyValue
        {
            get
            {
                ByteIndex bi = new ByteIndex(0, 0, Presence.AnyValue);
                return bi;
            }
        }

        private readonly Presence _presence;
        public Presence Presence
        {
            get { return _presence; }
        }

        private readonly byte _maskInData;
        public byte MaskInData
        {
            get { return _maskInData; }
        }

        private readonly byte _value;
        public byte Value
        {
            get { return _value; }
        }

        public ByteIndex(byte value)
            : this(value, 0xFF, Presence.Value)
        {
        }

        public ByteIndex(byte value, byte mask)
            : this(value, mask, Presence.Value)
        {
        }

        public ByteIndex(byte value, byte mask, Presence presence)
        {
            _value = value;
            _maskInData = mask;
            _presence = presence;
        }

        public override int GetHashCode()
        {
            return _value ^ _maskInData ^ (int)_presence;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ByteIndex))
                return false;

            return Equals((ByteIndex)obj);
        }

        public bool Equals(ByteIndex other)
        {
            return _value == other._value && _maskInData == other._maskInData && _presence == other._presence;
        }

        public static bool operator ==(ByteIndex index1, ByteIndex index2)
        {
            return index1.Equals(index2);
        }

        public static bool operator !=(ByteIndex index1, ByteIndex index2)
        {
            return !index1.Equals(index2);
        }

        public static implicit operator ByteIndex(byte value)
        {
            return new ByteIndex(value);
        }

        public override string ToString()
        {
            string ret = "*_";
            switch (Presence)
            {
                case Presence.Value:
                    if (MaskInData == 0xFF)
                        ret = Tools.FormatStr("{0:X2}", Value);
                    else
                        ret = Tools.FormatStr("{0:X2}&{1:X2}", Value, MaskInData);
                    break;
                case Presence.ExceptValue:
                    if (MaskInData == 0xFF)
                        ret = Tools.FormatStr("!{0:X2}", Value);
                    else
                        ret = Tools.FormatStr("!{0:X2}&{1:X2}", Value, MaskInData);
                    break;
                case Presence.AnyValue:
                    ret = "__";
                    break;
            }
            return ret;
        }
    }
}
