/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class StringExt
    {
        public static string TrimEx(this string value)
        {
            var ret = value.Trim();
            while (ret.Contains("  "))
            {
                ret = ret.Replace("  ", " ");
            }
            return ret;
        }

        public static string FormatStr(this string format, params object[] args)
        {
            return Tools.FormatStr(format, args);
        }

        public static void _EXLOG(this string format, params object[] args)
        {
            Tools._writeDebugDiagnosticExceptionMessage(Tools.FormatStr(format, args));
        }

        public static void _DLOG(this string format, params object[] args)
        {
            string text = null;
            try
            {
                if (args != null && args.Length > 0)
                {
                    text = Tools.FormatStr(format, args);
                }
                else
                {
                    text = format;
                }
            }
            catch (ArgumentNullException)
            {
                text = "-- -- --";
            }
            catch (FormatException)
            {
                text = "-- -- --";
            }
            try
            {
                Tools._writeDebugDiagnosticMessage(text, true, true, 3, 4);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unhandled:" + ex.Message);
            }
        }

        public static void _DLOG_List(this string format, IEnumerable list, params object[] args)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (args != null && args.Length > 0)
                    {
                        object[] newargs = new object[args.Length + 1];
                        Array.Copy(args, 0, newargs, 1, args.Length);
                        newargs[0] = item.ToString();
                        Tools._writeDebugDiagnosticMessage(Tools.FormatStr(format, newargs), true, true, 2, 1);
                    }
                    else
                    {
                        Tools._writeDebugDiagnosticMessage(Tools.FormatStr(format, item.ToString()), true, true, 2, 1);
                    }
                }
            }
            else
            {
                if (args != null && args.Length > 0)
                {
                    object[] newargs = new object[args.Length + 1];
                    Array.Copy(args, 0, newargs, 1, args.Length);
                    newargs[0] = "NULL";
                    Tools._writeDebugDiagnosticMessage(Tools.FormatStr(format, newargs), true, true, 2, 1);
                }
                else
                {
                    Tools._writeDebugDiagnosticMessage(Tools.FormatStr(format, "NULL"), true, true, 2, 1);
                }
            }
        }
    }

    public static class ExtentionsExt
    {
        public static bool Contains(this byte[] source, byte value)
        {
            try
            {
                return Array.IndexOf(source, value) >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Belongs(this byte value, byte[] array)
        {
            return Array.IndexOf(array, value) >= 0;
        }

        public static bool IsInRange(this byte value, byte start, byte end)
        {
            return value > start && value < end;
        }

        public static TResult Try<T, TResult>(this T input, Func<T, TResult> func)
            where T : class
            where TResult : class
        {
            if (input == null) return null;
            return func(input);
        }

        public static void Try<T>(this T input, Action<T> func)
           where T : class
        {
            if (input == null) ;
            func(input);
        }

        public static byte At(this byte[] array, int index)
        {
            if (array == null) return 0;
            if (array.Length > index)
            {
                return array[index];
            }
            return 0;
        }

        public static byte At(this IList<byte> array, int index)
        {
            if (array == null) return 0;
            if (array.Count > index)
            {
                return array[index];
            }
            return 0;
        }

        public static byte At(this IEnumerable<byte> array, int index)
        {
            if (array == null) return 0;
            int counter = 0;
            foreach (var item in array)
            {
                if (counter == index)
                    return item;
                counter++;
            }
            return 0;
        }

        public static string GetHex(this ByteIndex[] array)
        {
            if (array == null || array.Length == 0) return string.Empty;
            return Tools.GetHex(array, " ");
        }

        public static string GetHex(this IEnumerable<byte> array)
        {
            if (array == null) return string.Empty;
            return Tools.GetHexShort(array);
        }

        public static string GetHex(this IEnumerable<byte> array, bool putSpaces)
        {
            if (array == null) return string.Empty;
            if (putSpaces)
            {
                return Tools.GetHex(array.ToArray());
            }
            else
                return array.GetHex();
        }

        public static string GetHex(this byte[] array)
        {
            if (array == null || array.Length == 0) return string.Empty;
            return Tools.GetHex(array);
        }

        public static string GetHex(this IList<byte> array)
        {
            if (array == null || array.Count == 0) return string.Empty;
            return Tools.GetHex(array);
        }

        public static string GetHex(this byte value)
        {
            return Tools.GetHex(value, true);
        }

        public static byte[] GetBytes(this string hexString)
        {
            return Tools.GetBytes(hexString);
        }


        #region Arrays

        public static bool StartsWithArray(this byte[] array, byte[] subarray)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (subarray == null) throw new ArgumentNullException("subarray");

            return !subarray.Where((t, i) => array[i] != t).Any();
        }

        public static bool EqualsWithArray(this byte[] array, byte[] subarray)
        {
            bool ret = false;
            if (array != null && subarray != null && array.Length == subarray.Length)
            {
                ret = !subarray.Where((t, i) => array[i] != t).Any();
            }
            return ret;
        }

        public static bool EndsWithArray(this byte[] array, byte[] subarray)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (subarray == null) throw new ArgumentNullException("subarray");
            int offset = array.Length - subarray.Length;
            var ret = offset >= 0;

            if (ret)
            {
                if (array.Where((t, i) => i >= offset && t != subarray[i - offset]).Any())
                {
                    ret = false;
                }
            }
            return ret;
        }

        public static bool ContainsArray(this byte[] array, byte[] subarray)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (subarray == null) throw new ArgumentNullException("subarray");
            bool ret = false;
            for (int i = 0; i < array.Length - subarray.Length + 1; i++)
            {
                if (array[i] == subarray[0])
                {
                    ret = true;
                    for (int j = i + 1; j < i + subarray.Length; j++)
                    {
                        if (array[j] != subarray[j - i])
                        {
                            ret = false;
                            break;
                        }
                    }
                    if (ret)
                        break;
                }
            }

            return ret;
        }

        #endregion
    }
}
