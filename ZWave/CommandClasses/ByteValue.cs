/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// Genareted file from the Z-Wave XML Editor.
namespace ZWave.CommandClasses
{
    public struct ByteValue
    {
        public byte Value { get; private set; }
        public bool HasValue { get; private set; }
        public static ByteValue Empty => new ByteValue() { Value = 0, HasValue = false };

        public static implicit operator byte(ByteValue ByteValue) => ByteValue.Value;
        public static implicit operator ByteValue(byte value) => new ByteValue() { Value = value, HasValue = true };

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(string val)
        {
            return Value.ToString(val);
        }

        public bool BelongsTo(params byte[] array)
        {
            if (array != null)
                return System.Array.IndexOf<byte>(array, Value) >= 0;
            else
                return false;
        }

        public bool IsInRange(byte start, byte end)
        {
            return Value >= start && Value <= end;
        }
    }

}

