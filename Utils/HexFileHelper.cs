/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// Class used to work with Intel Hex files
    /// </summary>
    public class HexFileHelper
    {
        private enum Records
        {
            Data = 0x0,
            Eof = 0x1,
            ExtendedSegmentAddr = 0x02,
            ExtendedLinearAddr = 0x04
        }

        public static byte[] GetBytes(SortedList<short, List<byte>> data, byte emptyValue)
        {
            byte[] result = null;
            List<byte> r = new List<byte>();
            foreach (short key in data.Keys)
            {
                if (r.Count > 0)
                {
                    int nextSegmentAddress = key << 16;
                    if (r.Count < nextSegmentAddress)
                    {
                        r.AddRange(BlankArray(new byte[nextSegmentAddress - r.Count], emptyValue));
                    }
                }
                if (key < 0x0003)
                {
                    r.AddRange(data[key]);
                }
            }
            if (r.Count > 0) result = r.ToArray();
            return result;
        }

        public static byte[] GetBytes(SortedList<short, List<byte>> data, int length, byte emptyValue,
            bool isFillWithBlanksNeeded)
        {
            byte[] result = null;
            List<byte> r = new List<byte>();
            foreach (short key in data.Keys)
            {
                if (r.Count > 0)
                {
                    int nextSegmentAddress = key << 16;
                    if (r.Count < nextSegmentAddress)
                    {
                        r.AddRange(BlankArray(new byte[nextSegmentAddress - r.Count], emptyValue));
                    }
                }
                if (key < 0x0003)
                {
                    r.AddRange(data[key]);
                }
            }
            if (r.Count < length && isFillWithBlanksNeeded)
            {
                r.AddRange(BlankArray(new byte[length - r.Count], emptyValue));
            }
            if (r.Count > 0) result = r.ToArray();
            return result;
        }

        public static SortedList<short, List<byte>> ReadIntelHexFile(string fileName, byte emptyValue)
        {
            return ReadIntelHexFile(fileName, emptyValue, out int startAddress);
        }

        public static SortedList<short, List<byte>> ReadIntelHexFile(string fileName, byte emptyValue,
            out int startAddress)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException();
            SortedList<short, List<byte>> result = new SortedList<short, List<byte>>();
            using (StreamReader sr = File.OpenText(fileName))
            {
                startAddress = ReadHex(emptyValue, result, sr);
            }
            return result;
        }

        public static SortedList<short, List<byte>> ReadIntelHexData(string hexData, byte emptyValue)
        {
            SortedList<short, List<byte>> result = new SortedList<short, List<byte>>();
            using (StringReader sr = new StringReader(hexData))
            {
                ReadHex(emptyValue, result, sr);
            }
            return result;
        }

        public static SortedList<short, List<byte>> ReadIntelHexStream(StreamReader hexDataStream, byte emptyValue)
        {
            SortedList<short, List<byte>> result = new SortedList<short, List<byte>>();
            ReadHex(emptyValue, result, hexDataStream);
            return result;
        }

        private static int ReadHex(byte emptyValue, SortedList<short, List<byte>> result, TextReader sr)
        {
            int ret = -1;
            short currentSegmentKey = 0;
            string input;
            while ((input = sr.ReadLine()) != null)
            {
                if (!input.StartsWith(":"))
                {
                    continue;
                }
                byte[] hexRecord = Tools.GetBytes(input.Replace(":", ""));
                if (hexRecord.Length > 3)
                {
                    if (hexRecord[3] == (byte)Records.Data)
                    {
                        if (hexRecord.Length != hexRecord[0] + 5)
                            throw new FileFormatException("Not Intel HEX file.");

                        if (result.Count == 0)
                        {
                            result.Add(0, new List<byte>());
                        }

                        ushort address = (ushort)((hexRecord[1] << 8) + hexRecord[2]);
                        if (ret == -1)
                        {
                            ret = address;
                        }

                        if (result[currentSegmentKey].Count - 1 < address)
                        {
                            result[currentSegmentKey]
                                .AddRange(BlankArray(new byte[address + 1 - result[currentSegmentKey].Count],
                                    emptyValue));
                        }
                        if (result[currentSegmentKey].Count < hexRecord[0] + address)
                        {
                            result[currentSegmentKey].AddRange(BlankArray(new byte[hexRecord[0] - 1], emptyValue));
                        }

                        for (int i = 0; i < hexRecord[0]; i++)
                        {
                            result[currentSegmentKey][address] = hexRecord[i + 4];
                            address++;
                        }
                    }
                    else if (hexRecord[3] == (byte)Records.Eof)
                    {
                        break;
                    }
                    else if (hexRecord[3] == (byte)Records.ExtendedLinearAddr)
                    {
                        if (hexRecord.Length < 7)
                        {
                            throw new FileFormatException("Not Intel HEX file.");
                        }
                        currentSegmentKey = (byte)((hexRecord[4] << 8) + hexRecord[5]);
                        if (!result.ContainsKey(currentSegmentKey))
                            result.Add(currentSegmentKey, new List<byte>());
                    }
                    else if (hexRecord[3] == (byte)Records.ExtendedSegmentAddr)
                    {
                        if (hexRecord.Length < 7)
                        {
                            throw new FileFormatException("Not Intel HEX file.");
                        }
                    }
                    else
                    {
                        throw new FileFormatException("Unknown field found. Not Intel HEX file.");
                    }
                }
            }
            return ret;
        }

        public static byte[] BlankArray(int length, byte emptyValue)
        {
            byte[] array = new byte[length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = emptyValue;
            }
            return array;
        }

        public static byte[] BlankArray(byte[] array, byte emptyValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = emptyValue;
            }
            return array;
        }

        public static string WriteIntelHexFile(byte[] buffer, int recordLength, byte emptyValue)
        {
            StringBuilder sb = new StringBuilder();

            //Split data to segments by 64K
            List<byte[]> segments = Tools.ArraySplit(buffer, 64 * 1024);
            if (segments != null && segments.Count > 0)
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    //Write segment line
                    sb.AppendLine(string.Format(":02000004{0}{1}", Tools.GetHex((ushort)i),
                        Tools.GetHex((byte)(0xFA - i))));
                    //Split segment data to rows by recordLength
                    List<byte[]> rows = Tools.ArraySplit(segments[i], recordLength);
                    if (rows != null && rows.Count > 0)
                    {
                        short prevRecordLength = 0;
                        for (int j = 0; j < rows.Count; j++)
                        {
                            sb.AppendLine(string.Format(":{0}{1}00{2}{3}",
                                Tools.GetHex((byte)rows[j].Length),
                                Tools.GetHexShort(new byte[] { (byte)(prevRecordLength >> 8), (byte)prevRecordLength }),
                                Tools.GetHex(rows[j]).Replace(" ", ""),
                                Tools.GetHex(CalculateChecksum(rows[j], prevRecordLength))));

                            prevRecordLength += (short)rows[j].Length;
                        }
                    }
                }
            }
            sb.AppendLine(":00000001FF");
            return sb.ToString();
        }

        private static byte CalculateChecksum(byte[] data, short recordLength)
        {
            byte csum = (byte)(data.Length + (byte)(recordLength & 0x00FF) + (byte)(recordLength >> 8 & 0x00FF));
            csum = data.Aggregate(csum, (current, b) => (byte)(current + b));
            if (csum != 0)
                return (byte)(0x100 - csum);
            return 0;
        }

        public static string WriteIntelHexFile(byte[] buffer, int startAddress, int endAddress, int recordLength,
            byte emptyValue, bool skipNoDataLines)
        {
            StringBuilder sb = new StringBuilder();
            List<byte[]> sBuffer = new List<byte[]>();
            ushort segNum = 0;
            if (recordLength == 0) recordLength = 0x10;
            for (int i = 0; i < buffer.Length; i += recordLength)
            {
                byte[] arr = new byte[recordLength];
                if (i < buffer.Length - recordLength)
                {
                    Buffer.BlockCopy(buffer, i, arr, 0, recordLength);
                }
                else
                {
                    arr = new byte[buffer.Length - i];
                    Buffer.BlockCopy(buffer, i, arr, 0, arr.Length);
                }
                sBuffer.Add(arr);
            }
            if (!skipNoDataLines)
                sb.AppendLine(string.Format(":02000004{0}{1}", Tools.GetHex(segNum),
                    Tools.GetHex((byte)(0xFA - segNum))));
            for (int i = 0; i < sBuffer.Count; i++)
            {
                byte chksum = (byte)sBuffer[i].Length;
                chksum += (byte)((byte)((ushort)(i * recordLength) & 0x00FF) +
                                  (byte)((ushort)(i * recordLength) >> 8 & 0x00FF));
                if (i >> 12 > segNum)
                {
                    segNum = (ushort)(i >> 12);
                    sb.AppendLine(string.Format(":02000004{0}{1}", Tools.GetHex(segNum),
                        Tools.GetHex((byte)(0xFA - segNum))));
                }
                if (startAddress > 0)
                {
                    sb.Append(string.Format(":{0}{1}00", Tools.GetHex((byte)sBuffer[i].Length),
                        Tools.GetHex((ushort)(((startAddress + i) * recordLength) & 0x0000FFFF))));
                }
                else
                {
                    sb.Append(string.Format(":{0}{1}00", Tools.GetHex((byte)sBuffer[i].Length),
                        Tools.GetHex((ushort)((i * recordLength) & 0x0000FFFF))));
                }
                sb.Append(Tools.GetHexShort(sBuffer[i]));
                for (int j = 0; j < sBuffer[i].Length; j++)
                {
                    chksum += sBuffer[i][j];
                }
                if (chksum != 0)
                {
                    sb.Append(Tools.GetHex((byte)(0x100 - chksum)));
                }
                else
                {
                    sb.Append(Tools.GetHex(0x00));
                }
                sb.Append(Environment.NewLine);
            }
            if (!skipNoDataLines)
                sb.AppendLine(":00000001FF");
            return sb.ToString();
        }

        #region Old Code

        private struct FlashMem
        {
            public byte Length;
            public int Adr;
            public byte Type;
            public byte Crc;
            public int Sba;
            public int Lba;

            public void SetAdr(byte msb, byte lsb)
            {
                Adr = (msb << 8) | lsb;
            }

            public void SetLba(byte msb, byte lsb)
            {
                Lba = (msb << 8) | lsb;
            }

            public void SetSba(byte msb, byte lsb)
            {
                Sba = (msb << 8) | lsb;
            }
        }

        internal const byte
            FileRead = 0,
            FileWritten = 0,
            FileOpenError = 1,
            NotIntelHex = 2,
            AddressOutOfRange = 3,
            UnknownField = 4;

        internal const int
            CompareOk = 0x0000,
            CompareFail = 0xFFFF;

        /// <summary>
        /// Converts data in Intel HEX format to binary data.
        /// </summary>
        /// <param name="intelHex">The data in Intel HEX format.</param>
        /// <param name="defaultValue">The default value of byte (eraced value of memory).</param>
        /// <param name="minAdr">The lowest address to accept from Intel HEX data (byte from this address will be written to position 0 of output array, and so on).</param>
        /// <param name="maxAdr">The highest address to accept from Intel HEX data.</param>
        /// <returns>Array of bytes with binary data.</returns>
        public static byte[] FromIntelHexData(string intelHex, byte defaultValue, uint minAdr, uint maxAdr)
        {
            //TODO: implementation of this function should be replaced by solution without using temporary file.
            byte[] binData = null;
            try
            {
                string hexFileName = Path.GetTempFileName();
                StreamWriter sw = File.AppendText(hexFileName);
                sw.Write(intelHex);
                sw.Close();
                binData = BlankArray((int)(maxAdr - minAdr), defaultValue);
                if (ReadIntelHexFile(hexFileName, binData, (int)maxAdr, (int)minAdr) != FileRead)
                {
                    binData = null;
                }
                File.Delete(hexFileName);
            }
            catch
            {
                // ignored
            }
            return binData;
        }

        /// <summary>
        /// Reads the contents of a Intel hext file and converts it to
        /// its binary data.
        /// </summary>
        /// <param name="inputfile">Filename to open</param>
        /// <param name="flashData">Array to place data in, index = address</param>
        /// <param name="maxAdr">The highest address to accept from file</param>
        /// <returns>FILE_READ if no error</returns>
        public static byte ReadIntelHexFile(string inputfile, byte[] flashData, int maxAdr)
        {
            return ReadIntelHexFile(inputfile, flashData, maxAdr, 0);
        }

        /// <summary>
        /// Reads the contents of a Intel hext file and converts it to
        /// its binary data.
        /// </summary>
        /// <param name="inputfile">Filename to open</param>
        /// <param name="flashData">Array to place data in, index = address</param>
        /// <param name="maxAdr">The highest address to accept from file</param>
        /// <param name="minAdr">The lowest address to accept from file (byte from this address will be written to position 0 of flashData array, and so on)</param>
        /// <returns>FILE_READ if no error</returns>
        public static byte ReadIntelHexFile(string inputfile, byte[] flashData, int maxAdr, int minAdr)
        {
            bool addData = true;
            FlashMem flashInfo = new FlashMem();
            if (!File.Exists(inputfile))
            {
                return FileOpenError;
            }
            StreamReader sr = File.OpenText(inputfile);
            string input;
            string currentHex;
            int i;
            /*Reset when we start on a new file*/
            flashInfo.SetLba(0, 0);
            flashInfo.SetSba(0, 0);
            while ((input = sr.ReadLine()) != null)
            {
                if (!addData) continue;
                if (!input.StartsWith(":"))
                    return NotIntelHex;
                i = 1;
                currentHex = input.Substring(i, 2);
                //length first
                flashInfo.Length = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                    Thread.CurrentThread.CurrentCulture);
                //adr msb
                i += 2;
                currentHex = input.Substring(i, 2);
                byte msb = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                    Thread.CurrentThread.CurrentCulture);
                //adr lsb
                i += 2;
                currentHex = input.Substring(i, 2);
                byte lsb = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                    Thread.CurrentThread.CurrentCulture);
                //LOAD OFFSET
                flashInfo.SetAdr(msb, lsb);
                i += 2;
                currentHex = input.Substring(i, 2);
                //RECTYP
                flashInfo.Type = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                    Thread.CurrentThread.CurrentCulture);
                i += 2;
                //Handle remaining bytes by record type

                if (flashInfo.Type == (byte)Records.Data) //DATA_RECORD = 0
                {
                    if (flashInfo.Adr + flashInfo.Sba + (flashInfo.Lba << 16) > maxAdr)
                        return AddressOutOfRange;
                    if (flashInfo.Adr + flashInfo.Sba + (flashInfo.Lba << 16) < minAdr)
                        return AddressOutOfRange;
                    byte byteNo;
                    for (byteNo = 0; byteNo < flashInfo.Length; byteNo++)
                    {
                        currentHex = input.Substring(i, 2);
                        flashData[flashInfo.Adr + byteNo - minAdr] = byte.Parse(currentHex,
                            NumberStyles.AllowHexSpecifier, Thread.CurrentThread.CurrentCulture);
                        i += 2;
                    }
                    //Rest on line is crc
                    currentHex = input.Substring(i, 2);
                    flashInfo.Crc = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                        Thread.CurrentThread.CurrentCulture);
                }
                else if (flashInfo.Type == (byte)Records.Eof) //END_OF_FILE
                {
                }
                else if (flashInfo.Type == (byte)Records.ExtendedSegmentAddr)
                {
                    currentHex = input.Substring(i, 2);
                    msb = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                        Thread.CurrentThread.CurrentCulture);
                    i += 2;
                    currentHex = input.Substring(i, 2);
                    lsb = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                        Thread.CurrentThread.CurrentCulture);
                    i += 2;
                    flashInfo.SetSba(msb, lsb);
                }
                else if (flashInfo.Type == (byte)Records.ExtendedLinearAddr)
                {
                    /* Extended linear Base Address are the 31 to 16 upper bit of the following
                                     * DATA record.*/
                    currentHex = input.Substring(i, 2);
                    msb = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                        Thread.CurrentThread.CurrentCulture);
                    i += 2;
                    currentHex = input.Substring(i, 2);
                    lsb = byte.Parse(currentHex, NumberStyles.AllowHexSpecifier,
                        Thread.CurrentThread.CurrentCulture);
                    i += 2;
                    flashInfo.SetLba(msb, lsb);
                    addData = flashInfo.Lba == 0;
                }
                else
                {
                    return UnknownField;
                }
            }
            sr.Close();
            return FileRead;
        }

        #endregion
    }
}