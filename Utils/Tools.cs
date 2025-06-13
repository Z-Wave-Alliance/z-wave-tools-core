/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Utils
{
    public class Tools
    {
        private static Thread _debugOutputWorker;
        private static bool _isDebugOutputCancelling;
        public static void StartDebugOutputWorker()
        {
            if (_debugOutputWorker == null)
            {
                _debugOutputWorker = new Thread(DoDebugOutputWork) { IsBackground = true };
                _debugOutputWorker.Start();
            }
        }

        public static void StopDebugOutputWorker()
        {
            _isDebugOutputCancelling = true;
            if (_debugOutputWorker != null)
            {
                _debugOutputWorker.Join();
            }
        }

        private static readonly object LockObject = new object();
        private static readonly AutoResetEvent DebugOutputSignal = new AutoResetEvent(false);
        private static readonly Queue<string> DebugOutputQueue = new Queue<string>();
        private static void DoDebugOutputWork(object state)
        {
            while (!_isDebugOutputCancelling)
            {
                DebugOutputSignal.WaitOne();
                string message;
                do
                {
                    message = null;
                    lock (LockObject)
                    {
                        if (DebugOutputQueue.Count > 0)
                            message = DebugOutputQueue.Dequeue();
                    }
                    if (message != null)
                    {
                        _writeInner(message);
                    }
                }
                while (message != null);
            }
        }

        private static void AddDebugOutput(string message)
        {
            lock (LockObject)
            {
                DebugOutputQueue.Enqueue(message);
            }
            DebugOutputSignal.Set();
        }

        /// <summary>
        /// Gets the byte from string.
        /// </summary>
        /// <param name="str">The string value.</param>
        /// <returns></returns>
        public static byte GetByte(string str)
        {
            byte ret = 0;
            if (!string.IsNullOrEmpty(str))
            {
                if (str.StartsWith("0x") || str.StartsWith("0X"))
                {
                    byte.TryParse(str.Substring(2), NumberStyles.HexNumber, null, out ret);
                }
                else
                {
                    byte.TryParse(str, out ret);
                }
            }
            return ret;
        }

        public static ushort GetUShort(string str)
        {
            ushort ret = 0;
            if (!string.IsNullOrEmpty(str))
            {
                if (str.StartsWith("0x") || str.StartsWith("0X"))
                {
                    ushort.TryParse(str.Substring(2), NumberStyles.HexNumber, null, out ret);
                }
                else
                {
                    ushort.TryParse(str, out ret);
                }
            }
            return ret;
        }

        public static string FormatStr(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string JoinStr(string separator, params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = new StringBuilder();
                bool isFirst = true;
                foreach (string arg in args)
                {
                    if (arg != null)
                    {
                        if (!isFirst)
                        {
                            sb.Append(separator);
                        }
                        sb.Append(arg);
                        isFirst = false;
                    }
                }
                return sb.ToString();
            }
            return null;
        }

        public static string GetHex(byte key, bool add_0X)
        {
            char[] chs;
            byte b;
            if (add_0X)
            {
                chs = new[] { '0', 'x', '0', '0' };
                b = (byte)(key >> 4);
                chs[2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = (byte)(key & 0x0F);
                chs[3] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            else
            {
                chs = new char[2];
                b = (byte)(key >> 4);
                chs[0] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = (byte)(key & 0x0F);
                chs[1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(chs);
        }

        public static string GetHex(ushort key)
        {
            char[] chs = new char[4];
            var b = (byte)(key >> 12);
            chs[0] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            b = (byte)((key >> 8) & 0x0F);
            chs[1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            b = (byte)((key >> 4) & 0x0F);
            chs[2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            b = (byte)(key & 0x0F);
            chs[3] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            return new string(chs);
        }

        public static string GetHex(byte key)
        {
            char[] chs = new char[2];
            var b = (byte)(key >> 4);
            chs[0] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            b = (byte)(key & 0x0F);
            chs[1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            return new string(chs);
        }


        /// <summary>
        /// Example: 12 34 F4 55
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetHex(byte[] data)
        {
            if (data != null)
                return GetHex(data, 0, data.Length);
            return string.Empty;
        }

        public static string GetHex(ushort[] data)
        {
            if (data != null)
                return GetHex(data, 0, data.Length);
            return string.Empty;
        }

        /// <summary>
        /// Example: 12 34 F4 55
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetHex(byte[] data, int offset, int length)
        {
            if (data == null || data.Length == 0 || length == 0)
                return "";

            char[] chs = new char[length * 3 - 1];
            bool isFirst = true;
            int index = 0;
            for (int i = offset; i < length; ++i)
            {
                if (isFirst)
                    isFirst = false;
                else
                    chs[index++] = ' ';

                var b = (byte)(data[i] >> 4);
                chs[index++] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = (byte)(data[i] & 0x0F);
                chs[index++] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(chs);
        }

        public static string GetHex(ushort[] data, int offset, int length)
        {
            if (data == null || data.Length == 0 || length == 0)
                return "";

            char[] chs = new char[length * 3 - 1];
            bool isFirst = true;
            int index = 0;
            for (int i = offset; i < length; ++i)
            {
                if (isFirst)
                    isFirst = false;
                else
                    chs[index++] = ' ';

                var b = (byte)(data[i] >> 4);
                chs[index++] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = (byte)(data[i] & 0x0F);
                chs[index++] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(chs);
        }

        public static string GetHex(IList<byte> data)
        {
            if (data == null || data.Count == 0)
                return "";
            char[] chs;
            int index = 0;
            if (data.Count == 1)
            {
                chs = new char[2 + data.Count * 3 - 1];
                chs[index++] = '0';
                chs[index++] = 'x';
            }
            else
            {
                chs = new char[data.Count * 3 - 1];
            }
            bool isFirst = true;
            foreach (byte val in data)
            {
                if (isFirst)
                    isFirst = false;
                else
                    chs[index++] = ' ';

                var b = (byte)(val >> 4);
                chs[index++] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = (byte)(val & 0x0F);
                chs[index++] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(chs);
        }

        /// <summary>
        /// Example: 1234FD55
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetHexShort(byte[] data)
        {
            char[] chs = new char[data.Length * 2];
            for (int bx = 0, cx = 0; bx < data.Length; ++bx, ++cx)
            {
                var b = (byte)(data[bx] >> 4);
                chs[cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = (byte)(data[bx] & 0x0F);
                chs[++cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(chs);
        }

        public static string GetHexShort(IEnumerable<byte> data)
        {
            char[] chs = new char[data.Count() * 2];
            int bx = 0;
            int cx = 0;
            foreach (var item in data)
            {
                var b = (byte)(item >> 4);
                chs[cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = (byte)(item & 0x0F);
                chs[++cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                bx++;
                cx++;
            }
            return new string(chs);
        }

        public static string GetHex(ByteIndex[] val, string separator)
        {
            if (val != null && val.Length > 0)
            {
                string[] strvals = new string[val.Length];
                for (int i = 0; i < val.Length; i++)
                {
                    strvals[i] = val[i].ToString();
                }
                return string.Join(separator, strvals);
            }
            return null;
        }

        public static string GetNodeIds(byte[] val, string separator)
        {
            StringBuilder strData = new StringBuilder();
            if (val != null && val.Length > 0)
            {
                for (int i = 0; i < val.Length; i++)
                {
                    strData.Append(val[i].ToString(""));
                    if (i != val.Length - 1)
                    {
                        strData.Append(separator);
                    }
                }
            }
            return strData.ToString();
        }

        public static string GetNodeIds(ushort[] val, string separator)
        {
            StringBuilder strData = new StringBuilder();
            if (val != null && val.Length > 0)
            {
                for (int i = 0; i < val.Length; i++)
                {
                    strData.Append(val[i].ToString(""));
                    if (i != val.Length - 1)
                    {
                        strData.Append(separator);
                    }
                }
            }
            return strData.ToString();
        }

        public static byte[] ToNodeIds(string strValue, char[] separators)
        {
            if (strValue != null && separators != null)
            {
                List<byte> ret = new List<byte>();
                var separatedValues = strValue.Split(separators);
                foreach (var separateValue in separatedValues)
                {
                    if (int.TryParse(separateValue, out int val))
                    {
                        ret.Add((byte)val);
                    }
                }
                return ret.ToArray();
            }
            else
            {
                return null;
            }
        }

        public static int GetMaskFromBitsInt(byte bits, byte offsetInData)
        {
            int tmp1 = (int)(0xFFFFFFFF >> (32 - offsetInData - bits));
            int tmp2 = (int)(0xFFFFFFFF << offsetInData);
            int mask = (int)(tmp1 & tmp2);
            return mask;
        }

        public static ushort GetMaskFromBitsUShort(byte bits, byte offsetInData)
        {
            ushort tmp1 = (ushort)(0xFFFF >> (16 - offsetInData - bits));
            ushort tmp2 = (ushort)(0xFFFF << offsetInData);
            ushort mask = (ushort)(tmp1 & tmp2);
            return mask;
        }

        public static byte GetMaskFromBits(byte bits, byte offsetInData)
        {
            byte tmp1 = (byte)(0xFF >> (8 - offsetInData - bits));
            byte tmp2 = (byte)(0xFF << offsetInData);
            byte mask = (byte)(tmp1 & tmp2);
            return mask;
        }

        public static byte GetBitsFromMask(byte mask)
        {
            byte ret = 0;
            if ((mask & 0x01) != 0)
                ret++;
            if ((mask & 0x02) != 0)
                ret++;
            if ((mask & 0x04) != 0)
                ret++;
            if ((mask & 0x08) != 0)
                ret++;
            if ((mask & 0x10) != 0)
                ret++;
            if ((mask & 0x20) != 0)
                ret++;
            if ((mask & 0x40) != 0)
                ret++;
            if ((mask & 0x80) != 0)
                ret++;
            return ret;
        }
        /// <summary>
        /// Converts Str="XXX_YYY_ZZZ" to "Xxx{separator}Yyy{separator}Zzz"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string UpperUnderscoreToMixedUpperLower(string str, string separator)
        {
            string result = "";
            string[] split = str.Split('_');
            for (int j = 0; j < split.Length; j++)
            {
                string s = split[j];
                if (s.Length > 0)
                {
                    result += s[0];
                }
                if (s.Length > 1)
                {
                    result += s.Substring(1).ToLower();
                }
                if (j < split.Length - 1)
                {
                    result += separator;
                }
            }
            for (int j = result.Length - 1; j > 0; j--)
            {
                if (char.IsNumber(result[j]))
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Converts Str="XXX_YYY_ZZZ" to "YyyZzz" when preStr="XXX_"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="preStr"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string CutUpperUnderscoreToMixedUpperLower(string str, string preStr, string separator)
        {
            return UpperUnderscoreToMixedUpperLower(CutString(str, preStr), separator);
        }

        /// <summary>
        /// Cuts of a specified string from the beginning of another string 
        /// </summary>
        /// <param name="sourceStr">Source string</param>
        /// <param name="preStr">String to be removed from the beginning of the source string</param>
        /// <returns>Destination string</returns>
        public static string CutString(string sourceStr, string preStr)
        {
            string destStr = sourceStr;
            int preStrIdx = sourceStr.IndexOf(preStr, StringComparison.Ordinal);
            if (preStrIdx == 0 && preStr.Length < sourceStr.Length)
            {
                destStr = sourceStr.Substring(preStr.Length);
            }
            return destStr;
        }

        public static string MakeLegalUpperCaseIdentifier(string inStr)
        {
            string result = "";
            string tmp = inStr.Replace(@"'", "").Trim();
            tmp = tmp.Replace(@"’", "");
            string[] split = tmp.Split(" –-+ґ!\"#¤%&/()=?`@Ј$Ђ{[]}|,.;:*'^Ё~<>".ToCharArray());
            for (int j = 0; j < split.Length; j++)
            {
                result += split[j].ToUpper();
                if (j < split.Length - 1 && split[j + 1].Length > 0)
                {
                    result += "_";
                }
            }
            if (result.Length == 0)
            {
                result = "_";
            }
            else
            {
                while (result.Contains("__"))
                {
                    result = result.Replace("__", "_");
                }
                if (result.StartsWith("_"))
                {
                    result = result.Substring(1);
                }
                if (result.EndsWith("_"))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            return result;
        }

        public static string MakeLegalMixCaseIdentifier(string inStr)
        {
            string result = "";
            string tmp = inStr.Replace(@"'", "").Trim();
            tmp = tmp.Replace(@"’", "");
            string[] split = tmp.Split("_ –-+ґ!\"#¤%&/()=?`@Ј$Ђ{[]}|,.;:*'^Ё~<>".ToCharArray());
            foreach (string s in split)
            {
                if (s.Length > 0)
                {
                    if (result.Length == 0)
                    {
                        result += s.Substring(0, 1).ToLower();
                    }
                    else
                    {
                        result += s.Substring(0, 1).ToUpper();
                    }
                }
                if (s.Length > 1)
                {
                    result += s.Substring(1).ToLower();
                }
            }
            if (result.Length == 0)
            {
                result = "NONAME";
            }
            else
            {
                while (result.Contains("__"))
                {
                    result = result.Replace("__", "_");
                }
                if (result.StartsWith("_"))
                {
                    result = result.Substring(1);
                }
                if (result.EndsWith("_"))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }

            if (char.IsDigit(result[0]) || Exceptions.Contains(result))
            {
                result = "m" + result;
            }
            return result;
        }

        private static readonly List<string> Exceptions = new List<string>
        {
             "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
             "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum",
             "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto",
             "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace",
             "new", "null", "object", "operator", "out", "out", "override", "params", "private", "protected",
             "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static",
             "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong",
             "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
        };

        public static StreamWriter LogFileSw;
        private static bool _isLogFileSwOpen;
        public static StreamWriter ErrorFileSw;
        public static void CreateDebugExceptionFile(string errorFile)
        {
            string str = errorFile;
            if (!File.Exists(str))
            {
                ErrorFileSw = File.CreateText(str);
            }
            else
            {
                ErrorFileSw = File.AppendText(str);
            }
        }
        public static void CreateDebugLogFile(string logname, bool appendDate)
        {
            string str = CurrentDateTime.ToString("hhmmss") + "_" + logname;
            if (!appendDate)
                str = logname;
            if (!File.Exists(str))
            {
                LogFileSw = File.CreateText(str);
                LogFileSw.AutoFlush = true;
                _isLogFileSwOpen = true;
            }
            else
            {
                LogFileSw = File.AppendText(str);
                _isLogFileSwOpen = true;
            }
        }
        public static void CloseDebugLogFile()
        {
            try
            {

                if (LogFileSw != null && _isLogFileSwOpen)
                {
                    _isLogFileSwOpen = false;
                    LogFileSw.Flush();
                    LogFileSw.Close();
                }
            }
            catch (Exception)
            {

                //  throw;
            }
        }
        public static void CloseDebugExceptionFile()
        {
            if (ErrorFileSw != null)
            {
                ErrorFileSw.Flush();
                ErrorFileSw.Close();
            }
        }

        private static void _writeDebugDiagnosticMessage(string msg)
        {
            //#if DEBUG
            if (_debugOutputWorker != null)
            {
                AddDebugOutput(msg);
            }
            else
            {
                _writeInner(msg);
            }
            //#endif
        }

        private static readonly object WriterLock = new object();

        public static bool IsOutputToConsole = false;
        private static void _writeInner(string msg)
        {
            if (IsOutputToConsole)
            {
                Console.WriteLine(msg);
            }
            else
            {
                Debug.WriteLine(msg);
            }
            lock (WriterLock)
            {
                if (LogFileSw != null && _isLogFileSwOpen)
                {
                    LogFileSw.WriteLine(msg);
                }
            }
        }

        internal static void _writeDebugDiagnosticMessage(string msg, bool showTime, bool showCallingPoint, int skipInCallStack, int takeFromCallStack)
        {
            //#if DEBUG
            string methodName = string.Empty;
            string timestr = string.Empty;
            if (showCallingPoint)
            {
                methodName = GetMethodName(skipInCallStack, takeFromCallStack);
            }
            if (showTime)
            {
                timestr = CurrentDateTime.ToString("HH:mm:ss.fff");
            }
            _writeDebugDiagnosticMessage(
                FormatStr("{0} [{1}]:{2:000} {3}",
                timestr,
                string.IsNullOrEmpty(methodName) ? string.Empty : FormatStr("{0}", methodName),
                Thread.CurrentThread.ManagedThreadId,
                msg));
            //#endif
        }

        public static string GetMethodName(int skipInCallStack)
        {
            return GetMethodName(skipInCallStack + 1, 1);
        }

        public static string GetMethodName(int skipInCallStack, int takeFromCallStack)
        {
            string methodName = "N/A";
            int takeCount = takeFromCallStack < 1 ? 1 : takeFromCallStack;
            int skipFrames = 1 + skipInCallStack;
            var st = new StackTrace();

            if (st.FrameCount > skipFrames)
            {
                if (takeCount > 1)
                {
                    methodName = null;
                    methodName = st.GetFrames().
                        Skip(skipFrames).
                        Take(takeCount).
                        Select(x =>
                        {
                            const int len = 9;
                            var mb = x.GetMethod();
                            if (mb.DeclaringType != null)
                            {
                                var tp = mb.DeclaringType.Name.Length > len
                                ? mb.DeclaringType.Name.Substring(0, len)
                                : mb.DeclaringType.Name;

                                return string.Format("{0}.{1}",
                                    tp,
                                    mb.Name.PadRight(len + len - tp.Length).Substring(0, len + len - tp.Length));
                            }
                            else
                            {
                                return string.Format("{0}.{1}", "Unknown",
                                    mb.Name.PadRight(len).Substring(0, len));
                            }
                        }).
                        Aggregate(methodName, (c, n) =>
                        {
                            if (c == null)
                            {
                                return n;
                            }
                            else
                            {
                                return c + " " + n;
                            }
                        });
                }
                else
                {
                    var sf = st.GetFrame(skipFrames);
                    var mb = sf.GetMethod();
                    if (mb.DeclaringType != null)
                    {
                        methodName = string.Format("{0}.{1}", mb.DeclaringType.Name, mb.Name);
                    }
                }
            }
            return methodName;
        }


        internal static void _writeDebugDiagnosticExceptionMessage(string msg)
        {
            //#if DEBUG
            if (ErrorFileSw != null)
            {
                Debug.WriteLine(msg);
                ErrorFileSw.WriteLine(msg);
                ErrorFileSw.Flush();
            }
            else
            {
                Debug.WriteLine(msg);
            }
            //#endif
        }
        internal static void _writeDebugDiagnosticExceptionMessage(Exception ex, int showCallingPointShifter)
        {
            //#if DEBUG
            string methodName;
            string timestr;
            if (true)
            {
                methodName = GetMethodName(showCallingPointShifter);
            }
            if (true)
            {
                timestr = CurrentDateTime.ToString("HH:mm:ss.fff");
            }
            _writeDebugDiagnosticExceptionMessage(
                FormatStr("{0} [{1}]: {2}",
                timestr,
                string.IsNullOrEmpty(methodName) ? string.Empty : FormatStr("{0}", methodName),
                ex.Message + Environment.NewLine + ex.StackTrace));
            //#endif
        }

        public static DateTime CurrentDateTime
        {
            get
            {
                return GetCurrentTime();
            }
        }

        private static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Returns Int32 from byte array using big endian convention
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetInt32(byte[] data)
        {
            int ret = 0;
            if (data != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (data.Length > i)
                    {
                        ret += data[data.Length - 1 - i] << (i * 8);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns Int32 from byte array using big endian convention
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int GetInt32(IEnumerable<byte> data, int offset, int length)
        {
            int ret = 0;
            if (data != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (length > i)
                    {
                        ret += data.ElementAt(offset + length - 1 - i) << (i * 8);
                    }
                }
            }
            return ret;
        }

        public static int GetSignedInt32(byte[] data)
        {
            int ret = 0;
            if (data != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (data.Length > i + 1)
                    {
                        ret += data[data.Length - 1 - i] << (i * 8);
                    }
                    else if (data.Length == i + 1)
                    {
                        if (data[data.Length - 1 - i] > 0x7F)
                        {
                            ret += (data[data.Length - 1 - i] - 0x80) << (i * 8);
                            ret = -ret;
                        }
                        else ret += data[data.Length - 1 - i] << (i * 8);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns bytes in big endian convention
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetBytes(int value)
        {
            return new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }

        /// <summary>
        /// Returns bytes in big endian convention
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetBytes(uint value)
        {
            return new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }

        /// <summary>
        /// returns bytes in big endian convention
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] GetBytes(ushort hex)
        {
            return new[] { (byte)(hex >> 8), (byte)hex };
        }


        public static byte[] GetBytes(string hex) //Convert.FromHexString ?
        {
            if (string.IsNullOrEmpty(hex))
                return new byte[0];

            byte[] buffer = new byte[hex.Length / 2 + 1];
            int index = 0;
            bool isZeroFound = false;
            byte tmp = 0;
            bool isSecondPart = false;
            foreach (char item in hex)
            {
                if (isZeroFound)
                {
                    if (item == 'x' || item == 'X')
                    {
                        if (isSecondPart)
                        {
                            buffer[index++] = tmp;
                            isSecondPart = false;
                        }
                        isZeroFound = false;
                        continue;
                    }
                    //add 0
                    if (isSecondPart)
                    {
                        tmp = (byte)(tmp << 4);
                        buffer[index++] = tmp;
                        isSecondPart = false;
                    }
                    else
                    {
                        tmp = 0;
                        isSecondPart = true;
                    }
                    isZeroFound = false;
                }
                if (item == '0')
                {
                    isZeroFound = true;
                }
                else
                {
                    if (char.IsDigit(item) || item >= 'A' && item <= 'F' || item >= 'a' && item <= 'f')
                    {
                        if (isSecondPart)
                        {
                            tmp = (byte)(tmp << 4);
                            tmp += GetHexDigit(item);
                            buffer[index++] = tmp;
                            isSecondPart = false;
                        }
                        else
                        {
                            tmp = GetHexDigit(item);
                            isSecondPart = true;
                        }
                    }
                    else if (isSecondPart)
                    {
                        buffer[index++] = tmp;
                        isSecondPart = false;
                    }
                }
            }
            if (isZeroFound)
            {
                //add 0
                if (isSecondPart)
                {
                    tmp = (byte)(tmp << 4);
                    buffer[index++] = tmp;
                }
                else
                {
                    buffer[index++] = 0;
                }
            }
            else if (isSecondPart)
            {
                buffer[index++] = tmp;
            }
            byte[] ret = new byte[index];
            Array.Copy(buffer, 0, ret, 0, index);
            return ret;
        }

        private static byte GetHexDigit(char item)
        {
            if (item > '9')
            {
                if (item > 'F')
                {
                    return (byte)(item - 'a' + 10);
                }
                return (byte)(item - 'A' + 10);
            }
            return (byte)(item - '0');
        }

        public static bool ByteArrayStartsWith(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            if (array1 == null || array2 == null)
            {
                return false;
            }
            if (array1.Length < array2.Length)
            {
                return false;
            }
            return !array2.Where((t, i) => array1[i] != t).Any();
        }

        public static bool ByteArrayEquals(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            if (array1 == null || array2 == null)
            {
                return false;
            }
            if (array1.Length != array2.Length)
            {
                return false;
            }
            return !array1.Where((t, i) => t != array2[i]).Any();
        }

        public static bool IsEmptyArray(byte[] data)
        {
            return data.All(t => t == 0);
        }

        public static T Attempt<T>(int attemptsLeft, Func<T, bool> predicate, Func<T> action)
        {
            T ret = action();
            attemptsLeft--;
            if (attemptsLeft > 0 && !predicate(ret))
                ret = Attempt(attemptsLeft, predicate, action);
            return ret;
        }

        private const short Poly = 0x1021;

        public static byte[] CalculateCrc16Array(IEnumerable<byte> data)
        {
            short res = CalculateCrc16(data, 0, 0);
            return new[] { (byte)(res >> 8), (byte)res };
        }

        public static short CalculateCrc16(IEnumerable<byte> data)
        {
            return CalculateCrc16(data, 0, 0);
        }

        public static byte[] CalculateCrc16Array(IEnumerable<byte> data, int index, int length)
        {
            short res = CalculateCrc16(data, index, length);
            return new[] { (byte)(res >> 8), (byte)res };
        }

        public static short CalculateCrc16(IEnumerable<byte> data, int index, int length)
        {
            short crc = 0x1D0F;
            int i = index;
            foreach (byte b in data)
            {
                for (byte bitMask = 0x80; bitMask != 0; bitMask >>= 1)
                {
                    byte newBit = (byte)(Convert.ToByte((b & bitMask) != 0) ^ Convert.ToByte((crc & 0x8000) != 0));
                    crc <<= 1;
                    if (newBit != 0)
                    {
                        crc ^= Poly;
                    }
                }
                if (length > 0 && ++i >= length)
                    break;
            }
            return crc;
        }

        public static ushort ZW_CheckCrc16(ushort crc, byte[] pDataAddr, ushort bDataLen)
        {
            const ushort poly = 0x1021;
            byte i = 0;
            while (bDataLen-- > 0)
            {
                var workData = pDataAddr[i++];
                byte bitMask;
                for (bitMask = 0x80; bitMask != 0; bitMask >>= 1)
                {
                    /* Align test bit with next bit of the message byte, starting with msb.
                    */
                    var newBit = ((workData & bitMask) != 0) ^ ((crc & 0x8000) != 0);
                    crc <<= 1;
                    if (newBit)
                    {
                        crc ^= poly;
                    }
                } /* for (bitMask = 0x80; bitMask != 0; bitMask >>= 1) */
            }
            return crc;
        }

        public static ushort ZW_CreateCrc16(byte[] pHeaderAddr, byte bHeaderLen, byte[] pPayloadAddr, byte bPayloadLen)
        {
            ushort crc = 0x1D0F;
            if (pHeaderAddr != null && bHeaderLen > 0)
            {
                crc = ZW_CheckCrc16(crc, pHeaderAddr, bHeaderLen);
            }
            crc = ZW_CheckCrc16(crc, pPayloadAddr, bPayloadLen);
            return crc;
        }

        public static byte[] CalculateCrc32(byte[] data)
        {
            //Copy data to temp array
            byte[] crcBuf = new byte[data.Length];
            for (int j = 0; j < crcBuf.Length; j++)
                crcBuf[j] = data[j];

            //Clear last 4 bytes: 0xFF - flash blank value 
            //notice that OTP has different blank value
            crcBuf[crcBuf.Length - 4] = 0xFF;
            crcBuf[crcBuf.Length - 3] = 0xFF;
            crcBuf[crcBuf.Length - 2] = 0xFF;
            crcBuf[crcBuf.Length - 1] = 0xFF;

            //Reverse bits in buffer
            ReverseBitsInBuf(crcBuf, 0, crcBuf.Length);

            //Calculate CRC32
            uint[] crcTab = chksum_crc32gentab();
            uint crc = 0xFFFFFFFF;
            for (int i = 0; i < crcBuf.Length - 4; i++)
            {
                crc = ((crc >> 8) & 0x00FFFFFF) ^ crcTab[(crc ^ crcBuf[i]) & 0xFF];
            }
            byte[] crc32 = new byte[4];
            crc32[0] = (byte)(0x000000FF & crc);
            crc32[1] = (byte)(0x000000FF & (crc >> 8));
            crc32[2] = (byte)(0x000000FF & (crc >> 16));
            crc32[3] = (byte)(0x000000FF & (crc >> 24));

            ReverseBitsInBuf(crc32, 0, 4);

            return crc32;
        }

        public static void ReverseBitsInBuf(byte[] buffer, int offset, int length)
        {
            int i;

            for (i = offset; i < length + offset; i++)
            {
                buffer[i] = (byte)(((buffer[i] << 4) & 0xF0) | ((buffer[i] >> 4) & 0x0F));
                buffer[i] = (byte)(((buffer[i] << 2) & 0xCC) | ((buffer[i] >> 2) & 0x33));
                buffer[i] = (byte)(((buffer[i] << 1) & 0xAA) | ((buffer[i] >> 1) & 0x55));
            }
        }


        public static uint[] chksum_crc32gentab()
        {
            uint[] crcTab = new uint[256];
            int i;

            const uint poly = 0xEDB88320;
            for (i = 0; i < 256; i++)
            {
                var crc = (uint)i;
                int j;
                for (j = 8; j > 0; j--)
                {
                    if ((crc & 1) > 0)
                    {
                        crc = (crc >> 1) ^ poly;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
                crcTab[i] = crc;
            }
            return crcTab;
        }

        public static string DataToText(byte[] data)
        {
            int bpl = 16;
            StringBuilder sb = new StringBuilder();
            if (data != null && data.Length > 0)
            {
                for (int j = 0; j < data.Length / bpl; j++)
                {
                    byte[] tmp = new byte[bpl];
                    Array.Copy(data, j * bpl, tmp, 0, bpl);
                    sb.Append(":        ");
                    sb.Append(GetHexShort(tmp));
                    sb.AppendLine("  ");
                }
                if (data.Length % bpl != 0)
                {
                    bpl = data.Length % bpl;
                    byte[] tmp = new byte[bpl];
                    Array.Copy(data, data.Length - bpl, tmp, 0, bpl);
                    sb.Append(":        ");
                    sb.Append(GetHexShort(tmp));
                    sb.AppendLine("  ");
                }
            }
            return sb.ToString();
        }

        public static StringBuilder GetDifferenceText(byte[] source, byte[] value, string sourceTitle, string valueTitle)
        {
            StringBuilder ret = new StringBuilder();
            sourceTitle = "_" + sourceTitle.Replace(' ', '_') + "________________________________________";
            valueTitle = "_" + valueTitle.Replace(' ', '_') + "________________________________________";
            sourceTitle = Regex.Split(sourceTitle, @"(?<=\G.{32})", RegexOptions.Singleline)[0];
            valueTitle = Regex.Split(valueTitle, @"(?<=\G.{32})", RegexOptions.Singleline)[0];

            ret.AppendLine();
            const int perLine = 0x10;
            int index = 0;
            bool isTitleAdded = false;
            while (true)
            {
                int min = index * perLine;
                int max = min + perLine;
                if (max > source.Length)
                    max = source.Length;
                if (max > value.Length)
                    max = value.Length;
                bool hasDiffInLine = false;
                char[] srcLine = new string('.', perLine * 2).ToCharArray();
                char[] valLine = new string('.', perLine * 2).ToCharArray();
                for (int i = min; i < max; i++)
                {
                    if (source[i] != value[i])
                    {
                        Array.Copy(GetHex(source[i]).ToCharArray(), 0, srcLine, 2 * (i - min), 2);
                        Array.Copy(GetHex(value[i]).ToCharArray(), 0, valLine, 2 * (i - min), 2);
                        hasDiffInLine = true;
                    }
                }
                if (hasDiffInLine)
                {
                    if (!isTitleAdded)
                    {
                        ret.AppendLine(FormatStr("_addr_|_{0}_|_{1} ", sourceTitle, valueTitle));
                        isTitleAdded = true;
                    }
                    ret.Append(FormatStr(" {0} | ", GetHex((ushort)min)));
                    ret.Append(srcLine);
                    ret.Append(" | ");
                    ret.Append(valLine);
                    ret.AppendLine();
                }
                if (max == source.Length || max == value.Length)
                    break;
                index++;
            }
            return ret;
        }

        public static List<byte[]> ArraySplit(byte[] data, int fragmentSize)
        {
            if (fragmentSize != 0)
            {
                List<byte[]> result = new List<byte[]>();
                if (data.Length <= fragmentSize)
                {
                    result.Add(data);
                }
                else
                {
                    for (int i = 0; i < data.Length; i += fragmentSize)
                    {
                        byte[] arr = new byte[fragmentSize];
                        if (i < data.Length - fragmentSize)
                        {
                            Buffer.BlockCopy(data, i, arr, 0, fragmentSize);
                        }
                        else
                        {
                            arr = new byte[data.Length - i];
                            Buffer.BlockCopy(data, i, arr, 0, arr.Length);
                        }
                        result.Add(arr);
                    }
                }
                return result;
            }
            return null;
        }

        public static byte[] UInt32ToByteArray(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        public static uint ByteArrayToUInt32(byte[] bytes)
        {
            byte[] tmp;
            if (bytes.Length < 4)
            {
                tmp = new byte[4];
                Array.Copy(bytes, 0, tmp, 4 - bytes.Length, bytes.Length);
            }
            else
            {
                tmp = bytes;
            }
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmp);

            uint result = BitConverter.ToUInt32(tmp, 0);
            return result;
        }

        public static bool FolderHasAccess(string folderPath)
        {
            bool ret = false;
            string tempFile = "tempFile.tmp";
            string fullPath = Path.Combine(folderPath, tempFile);

            try
            {
                if (File.Exists(fullPath))
                {
                    tempFile = Path.GetFileNameWithoutExtension(fullPath) + "_PCController.tmp";
                    fullPath = Path.Combine(folderPath, tempFile);
                }
                using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew,
                                                                FileAccess.Write))
                {
                    fs.WriteByte(0xff);
                }

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    ret = true;
                }
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// Determines whether a string is valid IP address to IPv4.
        /// </summary>
        /// <param name="ipString">String IP value to validate.</param>
        /// <param name="address">The IPAddress version of string</param>
        /// <returns></returns>
        public static bool TryParseIPv4(string ipString, out IPAddress address)
        {
            bool ret = false;
            address = null;
            if (!string.IsNullOrEmpty(ipString))
            {
                var octets = ipString.Split('.');
                if (octets.Length == 4 && IPAddress.TryParse(ipString, out address))
                {
                    ret = address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && // Is IPv4.
                        address.GetAddressBytes().Length == 4;
                }
            }
            return ret;
        }

        /// <summary>
        /// Creates Ip v6 address from Ip v4 format. If addr is not Ip v4 family
        /// the source address will be returned without modifications
        /// </summary>
        /// <param name="addr">Source address</param>
        /// <returns>IpAddress in Ip v6 format</returns>
        public static IPAddress MapToIPv6(IPAddress addr)
        {
            IPAddress ret;
            if (addr.AddressFamily != AddressFamily.InterNetwork)
            {
                ret = addr;
            }
            else
            {
                string ipv4Str = addr.ToString();
                ret = IPAddress.Parse("::ffff:" + ipv4Str);
            }
            return ret;
        }

        /// <summary>
        /// Creates Ip v4 address from Ip v6 format. If addr is not in Ip v4 style
        /// the source address will be returned without modifications
        /// </summary>
        /// <param name="addr">Source address</param>
        /// <returns>IpAddress in Ip v4 format</returns>
        public static IPAddress MapToIPv4(IPAddress addr)
        {
            if (addr.AddressFamily == AddressFamily.InterNetworkV6 &&
                (addr.ToString().StartsWith("0000:0000:0000:0000:0000:ffff:") ||
                        addr.ToString().StartsWith("0:0:0:0:0:ffff:") ||
                        addr.ToString().StartsWith("::ffff:"))
                && IPAddress.TryParse(addr.ToString().Substring(addr.ToString().LastIndexOf(":", StringComparison.Ordinal) + 1), out IPAddress ret))
            {
                //Already parsed
            }
            else
            {
                ret = addr;
            }
            return ret;
        }

        /// <summary>
        /// Transforms bytes to Human friendly view with a standard format of xxxxx-xxxxx
        /// </summary>
        /// <param name="dsk">16 bytes dsk value</param>
        /// <returns>String in readable dsk format.</returns>
        public static string GetDskStringFromBytes(byte[] dsk)
        {
            string ret;
            var sb = new StringBuilder();
            for (int i = 0; i < 16; i += 2)
            {
                if (i >= 14)
                {
                    if (i < dsk.Length)
                    {
                        sb.Append(Tools.GetInt32(dsk.Skip(i).Take(2).ToArray()).ToString("00000"));
                    }
                    else
                    {
                        sb.Append("xxxxx");
                    }
                }
                else
                {
                    if (i < dsk.Length)
                    {
                        sb.Append(Tools.GetInt32(dsk.Skip(i).Take(2).ToArray()).ToString("00000") + "-");
                    }
                    else
                    {
                        sb.Append("xxxxx-");
                    }
                }
            }

            ret = sb.ToString();
            return ret;
        }

        //not correct
        //public static byte[] GetBytesFromByteArray(byte[] data, int bitsOffset, int bitsLength)
        //{
        //    int bitsLeft = bitsLength;
        //    int byteOffset = bitsOffset / 8;
        //    int bitOffset = bitsOffset % 8;
        //    byte[] result = new byte[(bitsLength + 7) / 8];
        //    int resultIndex = 0;

        //    while (bitsLeft > 0)
        //    {
        //        byte b = data[byteOffset];
        //        int bitsToRead = Math.Min(8 - bitOffset, bitsLeft);
        //        byte mask = (byte)((1 << bitsToRead) - 1);

        //        byte bits = (byte)((b >> (8 - bitOffset - bitsToRead)) & mask);

        //        if (bitsLeft < 5)
        //        {
        //            bits <<= 4 - bitsLeft;
        //        }

        //        result[resultIndex] |= bits;

        //        bitsLeft -= bitsToRead;
        //        bitOffset += bitsToRead;

        //        if (bitOffset >= 8)
        //        {
        //            bitOffset -= 8;
        //            byteOffset++;
        //        }

        //        resultIndex += (bitsLeft >= 0) ? 1 : 0;
        //    }

        //    return result;
        //}

        public static byte[] GetBytesFromByteArray(byte[] data, int bitsOffset, int bitsLength)
        {
            var hexes = string.Concat(data.Select(x => x.ToString("x2")));
            var hexesRes = hexes.Substring(bitsOffset / 4, bitsLength / 4).ToUpper();
            if (hexesRes.Length % 2 != 0)
            {
                hexesRes = "0" + hexesRes;
            }
            return GetBytes(hexesRes);
        }


    }
}
