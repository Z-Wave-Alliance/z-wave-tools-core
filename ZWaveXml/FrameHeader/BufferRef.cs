/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ZWave.Xml.FrameHeader
{
    public class BufferRef
    {
        private byte[] _data;
        private List<byte[]> _extensions;
        private int _offset { get; set; }
        private int _dataLength { get; set; }

        public BufferRef(byte[] data, int offset, int length)
        {
            _data = data;
            _offset = offset;
            _dataLength = length;
            if (data == null)
            {
                throw new ArgumentNullException("data argument");
            }
            if (_dataLength < 0 || _dataLength > _data.Length - _offset)
            {
                throw new ArgumentOutOfRangeException("length argument");
            }
            if (_offset < 0 || _offset >= _data.Length)
            {
                throw new ArgumentOutOfRangeException("offset argument");
            }
        }

        private int _length = -1;
        public int Length
        {
            get
            {
                if (_length < 0)
                {
                    int _totalLength = _dataLength;
                    if (_extensions != null)
                    {
                        foreach (var ext in _extensions)
                        {
                            _totalLength += ext.Length;
                        }
                    }
                }
                return _length;
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index < _dataLength)
                {
                    return _data[index + _offset];
                }
                else
                {
                    int ret = -1;
                    if (_extensions != null)
                    {
                        int newIndex = index - _dataLength;
                        foreach (var ext in _extensions)
                        {
                            if (newIndex < ext.Length)
                            {
                                ret = ext[newIndex];
                                break;
                            }
                            else
                            {
                                newIndex -= ext.Length;
                            }
                        }
                    }
                    if (ret >= 0)
                    {
                        return (byte)ret;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index argument");
                    }
                }
            }
            set
            {
                if (index < _dataLength)
                {
                    _data[index + _offset] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index argument");
                }
            }
        }

        public int GetInt32(int startIndex, int length)
        {
            int ret = Tools.GetInt32(_data, startIndex + _offset, length);
            return ret;
        }

        public byte[] ToArray()
        {
            byte[] ret = new byte[Length];
            CopyTo(ret, ret.Length);
            return ret;
        }

        public void CopyTo(byte[] destination, int length)
        {
            CopyTo(0, destination, 0, length);
        }

        public void CopyTo(int sourceIndex, byte[] destination, int destinationIndex, int length)
        {
            if (_extensions == null)
            {
                if (sourceIndex + length > _dataLength)
                {
                    throw new ArgumentOutOfRangeException("length argument");
                }
                Array.Copy(_data, sourceIndex + _offset, destination, destinationIndex, length);
            }
            else
            {
                if (sourceIndex + length > Length)
                {
                    throw new ArgumentOutOfRangeException("length argument");
                }
                int copyIndex = sourceIndex;
                int copyLength = length > (_dataLength - sourceIndex) ? (_dataLength - sourceIndex) : length;
                if (copyIndex < _dataLength)
                {
                    Array.Copy(_data, copyIndex + _offset, destination, destinationIndex, copyLength);
                    copyIndex = 0;
                    copyLength = length - copyLength;
                }
                else
                {
                    copyIndex -= _dataLength;
                    copyLength -= _dataLength;
                }

                if (copyLength > 0)
                {
                    foreach (var ext in _extensions)
                    {
                        if (copyIndex < ext.Length)
                        {
                            if (copyLength > ext.Length)
                            {
                                Array.Copy(ext, sourceIndex + _offset, destination, destinationIndex, copyLength);
                                copyLength -= ext.Length;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            copyIndex -= ext.Length;
                        }
                    }
                }
            }
        }

        public void AddExtension(byte type, byte[] data)
        {
            _length = -1; // invalidate total length
            if (_extensions == null)
            {
                _extensions = new List<byte[]>();
            }
            byte[] tmp = new byte[data.Length + 3];
            tmp[0] = type;
            tmp[1] = (byte)(data.Length >> 8);
            tmp[2] = (byte)(data.Length);
            Array.Copy(data, 0, tmp, 3, data.Length);
            _extensions.Add(tmp);
        }
    }
}
