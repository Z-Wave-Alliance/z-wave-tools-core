/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace ZWave.ZnifferApplication
{
    public class DataItemsCollection
    {
        private volatile bool _isOpen = false;
        private object locker = new object();

        public event Action CollectionReseted;
        public int Count { get; private set; }
        public string FileName { get; set; }
        public FileStream FileStream { get; set; }
        public DataItemsCollection(string fileName)
        {
            FileName = fileName;
            Open();
        }

        public void Open()
        {
            FileStream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            _isOpen = true;
            Count = 0;
        }

        public void Close()
        {
            _isOpen = false;
            FileStream.Close();
        }

        public DataItem GetItemWithDelta(IntRef positionRef)
        {
            return GetItem(positionRef.Value, true);
        }

        public DataItem GetItem(IntRef positionRef)
        {
            return GetItem(positionRef.Value, false);
        }

        private readonly IDataItemBox[] diboxes = new IDataItemBox[28000];

        public DataItem GetItem(int position, bool includeDelta)
        {
            if (!_isOpen || position < 0 || position * DataItemBox.BOX_SIZE >= FileStream.Length)
                return null;
            lock (locker)
            {
                if (mReadingCachePosition == position && !includeDelta)
                    return mReadingCache;
                if (mWritingCachePosition == position)
                    return mWritingCache;
                mReadingCachePosition = position;
                FileStream.Position = position * DataItemBox.BOX_SIZE;
                byte[] buffer = new byte[DataItemBox.BOX_SIZE];
                FileStream.Read(buffer, 0, DataItemBox.BOX_SIZE);
                DataItemBox dib = buffer;
                position = position - dib.SeqNo;
                FileStream.Position = position * DataItemBox.BOX_SIZE;
                Read(buffer);
                var firstBox = (FirstDataItemBox)buffer;
                if (firstBox.BoxCount == 0)
                {
                    "NO BOXES!"._DLOG();
                }
                diboxes[0] = firstBox;
                for (int i = 1; i < firstBox.BoxCount; i++)
                {
                    Read(buffer);
                    diboxes[i] = (DataItemBox)buffer;
                }
                FileStream.Seek(0, SeekOrigin.End);
                DataItem ret = DataItem.CreateFrom(diboxes);
                ret.Position = position;
                ret.Boxes = firstBox.BoxCount;

                if (includeDelta)
                {
                    if (position > 0)
                    {
                        DataItem prev = GetItem((position - 1), false);
                        if (prev != null)
                        {
                            ret.Delta = CalculateDelta(ret, prev);
                        }
                    }
                }
                mReadingCache = ret;

                return ret.LineNo > 0 ? ret : null;
            }
        }

        private void Read(byte[] buffer)
        {
            int offset = 0;
            int remaining = DataItemBox.BOX_SIZE;
            int attempts = 3;
            while (remaining > 0 && attempts > 0)
            {
                int read = FileStream.Read(buffer, offset, remaining);
                if (read <= 0)
                {
                    //throw new EndOfStreamException
                    //    (String.Format("End of stream reached with {0} bytes left to read", remaining));
                    attempts--;
                }
                remaining -= read;
                offset += read;
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                mReadingCachePosition = int.MaxValue;
                mWritingCachePosition = int.MaxValue;
                mWritingCache = null;
                mFirst = null;
                mReadingCache = null;
                Close();
                Open();
                CollectionReseted?.Invoke();
            }
        }

        public int GetCurrrentPosition()
        {
            lock (locker)
            {
                long position = FileStream.Position;
                return (int)(position / DataItemBox.BOX_SIZE);
            }
        }

        public DataItem Add(DataItem dataItem)
        {
            lock (locker)
            {
                dataItem.Delta = CalculateDelta(dataItem, mWritingCache);
                mWritingCache = dataItem;
                long position = FileStream.Position;
                mWritingCachePosition = (int)(position / DataItemBox.BOX_SIZE);
                if (mWritingCachePosition == 0)
                    mFirst = dataItem;
                dataItem.ToDataItemBoxes(diboxes);
                var firstBox = (FirstDataItemBox)diboxes[0];
                FileStream.Write(firstBox, 0, DataItemBox.BOX_SIZE);
                Count++;
                for (int i = 1; i < firstBox.BoxCount; i++)
                {
                    FileStream.Write((DataItemBox)diboxes[i], 0, DataItemBox.BOX_SIZE);
                }
                dataItem.Position = mWritingCachePosition;
                dataItem.Boxes = firstBox.BoxCount;
            }
            return dataItem;
        }

        public DataItem GetLastAddedItem()
        {
            return mWritingCache;
        }

        public int GetLastAddedPosition()
        {
            return mWritingCachePosition;
        }

        public DataItem GetFirstAddedItem()
        {
            return mFirst;
        }

        private DataItem mFirst;
        private int mReadingCachePosition = int.MaxValue;
        private DataItem mReadingCache;
        private int mWritingCachePosition = int.MaxValue;
        private DataItem mWritingCache;

        private int CalculateDelta(DataItem currDataFrame, DataItem prevDataFrame)
        {
            int delta = 0;
            if (currDataFrame != null && prevDataFrame != null)
            {
                delta = (int)((TimeSpan)(currDataFrame.CreatedAt - prevDataFrame.CreatedAt)).TotalMilliseconds;
                if (currDataFrame.Systime != 0 && prevDataFrame.Systime != 0)
                {

                    int wrappingCounter = 0;// how many times device systime was wrapped
                    if (delta + prevDataFrame.Systime > UInt16.MaxValue)
                    {
                        wrappingCounter = (int)((delta + prevDataFrame.Systime) / UInt16.MaxValue);
                        delta = (int)(wrappingCounter * UInt16.MaxValue + currDataFrame.Systime - prevDataFrame.Systime);
                    }
                    else if (prevDataFrame.Systime > currDataFrame.Systime)
                    {
                        delta = (int)((UInt16.MaxValue - prevDataFrame.Systime) + currDataFrame.Systime);
                    }
                    else if (currDataFrame.Systime > prevDataFrame.Systime)
                    {
                        delta = (int)(currDataFrame.Systime - prevDataFrame.Systime);
                    }
                }
            }
            if (delta < 0)
                delta = 0;
            return delta;
        }
    }

    public class DataItemsCollectionEx
    {
        public event Action CollectionReseted;
        private readonly object syncObject = new object();

        private readonly int mDelta;
        private readonly int mMaxCapacity;
        private readonly DataItem[] LIST;
        private int mBegin = 0;
        private int mNextIndex = 0;
        private int mIndexWraps = 0;
        private int mBeginWraps = 0;

        public DataItemsCollectionEx(int capacity)
        {
            mDelta = 1; //capacity / 100;
            mMaxCapacity = capacity;
            LIST = new DataItem[mMaxCapacity];
        }

        public int BeginIndex { get { return mBegin + mBeginWraps * mMaxCapacity; } }
        public int NextIndex { get { return mNextIndex + mIndexWraps * mMaxCapacity; } }

        public int MaxCapacity
        {
            get { return mMaxCapacity; }
        }

        public DataItem GetItem(int lineNo)
        {
            return GetItemByIndex(lineNo - 1);
        }

        public DataItem GetItemByIndex(int index)
        {
            return LIST[index % mMaxCapacity];
        }

        public void SetBegin(int index)
        {
            lock (syncObject)
            {
                if (index > BeginIndex)
                {
                    mBegin = index % mMaxCapacity;
                    mBeginWraps = index / mMaxCapacity;
                }
            }
        }

        public int Add(DataItem dataItem)
        {
            int ret = 0;
            lock (syncObject)
            {
                while (mNextIndex + mIndexWraps * mMaxCapacity >= mBegin + (mBeginWraps + 1) * mMaxCapacity) //overflow
                {
                    mBegin = mBegin + mDelta;
                    if (mBegin >= mMaxCapacity)
                    {
                        mBeginWraps++;
                        mBegin = 0;
                    }
                    ret = mBegin + mBeginWraps * mMaxCapacity;
                }
            }
            LIST[mNextIndex] = dataItem;
            mNextIndex++;
            if (mNextIndex >= mMaxCapacity)
            {
                mIndexWraps++;
                mNextIndex = 0;
            }
            return ret;
        }

        public void Clear()
        {
            mBegin = 0;
            mNextIndex = 0;
            mIndexWraps = 0;
            mBeginWraps = 0;
            CollectionReseted?.Invoke();
        }
    }
}
