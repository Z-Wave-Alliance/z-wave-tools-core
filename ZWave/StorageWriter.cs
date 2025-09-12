/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using ZWave.Layers;
using Utils;
using ZWave.Enums;
using Utils.Threading;

namespace ZWave
{
    public sealed class StorageWriter : IDisposable
    {
        private Thread worker;
        private SignalSlim signal = new SignalSlim(false);
        private bool isClosing = false;
        private readonly object locker = new object();
        private List<DataChunk> innerListAdd = new List<DataChunk>();
        private List<DataChunk> innerListTmp = new List<DataChunk>();
        private List<DataChunk> innerListProcessing = new List<DataChunk>();
        public bool IsOpen { get; private set; }

        private bool isHeaderCommitRequired = false;
        public DateTime CreatedAt = DateTime.Now;
        public long TotalBytes = StorageHeader.STORAGE_HEADER_SIZE;
        private StorageHeader _storageHeader;

        /// <summary>
        /// Creates new file. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="header"></param>
        public void Open(string fileName, StorageHeader header)
        {
            Open(fileName, header, FileMode.Create);
        }

        /// <summary>
        /// Continue writing file or create new. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="header"></param>
        public void OpenAppend(string fileName, StorageHeader header)
        {
            Open(fileName, header, FileMode.Append);
        }

        private void Open(string fileName, StorageHeader header, FileMode fileMode)
        {
            if (!IsOpen && !string.IsNullOrEmpty(fileName))
            {
                _storageHeader = header;
                try
                {
                    CreatedAt = DateTime.Now;
                    TotalBytes = StorageHeader.STORAGE_HEADER_SIZE;
                    if (fileMode == FileMode.Create)
                    {
                        worker = new Thread(DoWorkCreate);
                    }
                    else if (fileMode == FileMode.Append)
                    {
                        worker = new Thread(DoWorkAppend);
                    }
                    worker.IsBackground = true;
                    worker.Start(fileName);
                    IsOpen = true;
                }
                catch (OutOfMemoryException)
                {
                    IsOpen = false;
                }
                catch (IOException)
                {
                    IsOpen = false;
                }
            }
        }

        public void Close()
        {
            if (IsOpen)
            {
                isClosing = true;
                signal.Set();
                if (worker.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
                {
                    worker.Join();
                }
                isClosing = false;
            }
        }

        public void Reset()
        {
            if (IsOpen)
            {

            }
        }

        public void Write(StorageAttachment attachment)
        {
            Write(new DataChunk(attachment.ToByteArray(), 0, false, ApiTypes.Attachment));
        }

        public void Write(DataChunk dc)
        {
            if (IsOpen && !isClosing)
            {
                lock (locker)
                {
                    TotalBytes += dc.TotalBytes;
                    innerListAdd.Add(dc);
                }
                signal.Set();
            }
        }

        public static void SetAttachments(string fileName, byte sessionId, IList<StorageAttachment> attachments)
        {
            if (fileName != null && attachments != null)
            {
                if (File.Exists(fileName))
                {
                    using (BinaryWriter sWriter = new BinaryWriter(new FileStream(fileName, FileMode.Open)))
                    {
                        sWriter.Seek(0, SeekOrigin.End);
                        foreach (var attachment in attachments)
                        {
                            DataChunk dcWrite = new DataChunk(attachment.ToByteArray(), sessionId, false, ApiTypes.Attachment);
                            byte[] dataBuffer = dcWrite.ToByteArray();
                            sWriter.Write(dataBuffer, 0, dataBuffer.Length);
                        }
                    }
                }
            }
        }

        public static void SaveHeader(string fileName, StorageHeader storageHeader)
        {
            if (fileName != null)
            {
                byte[] buffer = storageHeader.GetBuffer();
                if (File.Exists(fileName))
                {
                    using (BinaryWriter sWriter = new BinaryWriter(new FileStream(fileName, FileMode.Open)))
                    {
                        sWriter.Seek(0, SeekOrigin.Begin);
                        sWriter.Write(buffer, 0, StorageHeader.STORAGE_HEADER_SIZE);
                        sWriter.Seek(0, SeekOrigin.End);
                    }
                }
                else
                {
                    using (BinaryWriter sWriter = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                    {
                        sWriter.Seek(0, SeekOrigin.Begin);
                        sWriter.Write(buffer, 0, StorageHeader.STORAGE_HEADER_SIZE);
                        sWriter.Seek(0, SeekOrigin.End);
                    }
                }
            }
        }

        private void DoWorkCreate(object fileName)
        {
            DoWork((string)fileName, FileMode.Create);
        }

        private void DoWorkAppend(object fileName)
        {
            DoWork((string)fileName, FileMode.Append);
        }

        private void DoWork(string fileName, FileMode fileMode)
        {
            string fileDirectory = Path.GetDirectoryName(fileName); // empty if the given 'fileName' contains no directory information
            if (fileDirectory != string.Empty && !Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            using (BinaryWriter sWriter = new BinaryWriter(new FileStream(fileName, fileMode)))
            {
                if (sWriter.BaseStream.Position == 0)
                {
                    if (_storageHeader == null)
                    {
                        _storageHeader = new StorageHeader();
                    }
                    sWriter.Write(_storageHeader.GetBuffer(), 0, StorageHeader.STORAGE_HEADER_SIZE);
                    sWriter.Flush();
                }
                while (!isClosing)
                {
                    signal.WaitOne();
                    signal.Reset();
                    lock (locker)
                    {
                        innerListTmp = innerListProcessing;
                        innerListProcessing = innerListAdd;
                        innerListAdd = innerListTmp;
                    }
                    if (fileMode == FileMode.Create && isHeaderCommitRequired && _storageHeader != null)
                    {
                        isHeaderCommitRequired = false;
                        sWriter.Seek(0, SeekOrigin.Begin);
                        sWriter.Write(_storageHeader.GetBuffer(), 0, StorageHeader.STORAGE_HEADER_SIZE);
                        sWriter.Seek(0, SeekOrigin.End);
                    }
                    foreach (DataChunk dataChunk in innerListProcessing)
                    {
                        byte[] d = dataChunk.ToByteArray();
                        sWriter.Write(d, 0, d.Length);
                        sWriter.Flush();
                    }
                    innerListProcessing.Clear();
                }
            }
            IsOpen = false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Close();
                    signal.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StorageWriter() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
