/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers;
using ZWave.ProgrammerApplication.Operations;
using ZWave.Layers.Transport;
using Utils;
using System.IO.Ports;
using System.Threading;
using ZWave.Enums;
using ZWave.ProgrammerApplication.Enums;
using ZWave.Layers.Application;
using System.IO;

namespace ZWave.ProgrammerApplication.Devices
{
    public class Programmer : ApplicationClient
    {
        const byte x11111001 = 0xF9;
        const byte x11111011 = 0xFB;
        const byte x11111101 = 0xFD;
        const byte x00000010 = 0x02;
        const byte x00000100 = 0x04;
        const int _2_KB = 0x800;

        public byte[] WriteFlashSectorCommand_StopOnBadResponseInSectorsBigEndian { get; set; }

        #region properties

        public bool IsResetNSignalAsserted { get; private set; }

        public int CommandsCount { get; set; }
        public int AttemptsCount { get; set; }
        public byte CheckStateResult { get; private set; }

        private Action mAssertResetNSignalCallback = null;
        public Action AssertResetNSignalCallback
        {
            get { return mAssertResetNSignalCallback; }
            set { mAssertResetNSignalCallback = value; }
        }

        private Action mDeassertResetNSignalCallback = null;
        public Action DeassertResetNSignalCallback
        {
            get { return mDeassertResetNSignalCallback; }
            set { mDeassertResetNSignalCallback = value; }
        }

        private Action mPowerResetCallback = null;
        public Action PowerResetCallback
        {
            get { return mPowerResetCallback; }
            set { mPowerResetCallback = value; }
        }

        private Action mCheckDriverCallback = null;
        public Action CheckDriverCallback
        {
            get { return mCheckDriverCallback; }
            set { mCheckDriverCallback = value; }
        }

        private byte[] mUsbLoader1 = null;
        public byte[] UsbLoader1
        {
            get { return mUsbLoader1; }
            set { mUsbLoader1 = value; }
        }

        private byte[] mUsbLoader2 = null;
        public byte[] UsbLoader2
        {
            get { return mUsbLoader2; }
            set { mUsbLoader2 = value; }
        }

        private byte[] mCalibrationData = null;
        public byte[] CalibrationData
        {
            get { return mCalibrationData; }
            set { mCalibrationData = value; }
        }
        #endregion

        internal Programmer(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(ApiTypes.Programmer, sessionId, sc, fc, tc)
        {
            CommandsCount = 6;
            AttemptsCount = 10;
        }

        #region static
        public static bool IsNvrCrc16Valid(byte[] nvrData, NVRData _nvr)
        {
            return (nvrData[NVRData.CRC16_ADDRESS] == _nvr.CRC16[0] &&
                nvrData[NVRData.CRC16_ADDRESS + 1] == _nvr.CRC16[1]);
        }

        public static bool IsAPM_LB(byte value)
        {
            bool ret = (value & x00000010) == 0 && (value & x00000100) > 0;
            return ret;
        }

        public static byte ClearAutoProg1(byte value)
        {
            byte ret = value;
            ret &= x11111011;
            return ret;
        }

        public static byte ClearAutoProg0(byte value)
        {
            byte ret = value;
            ret &= x11111101;
            return ret;
        }

        public static byte ClearAutoProg0_SetAutoProg1(byte value)
        {
            byte ret = value;
            ret &= x11111001;
            ret += x00000010;
            return ret;
        }

        public static bool AddCrc32AtEnd(byte[] data, int length)
        {
            bool ret = false;
            byte[] crc32 = Tools.CalculateCrc32(data);
            for (int i = 0; i < crc32.Length; i++)
            {
                if (data[length - crc32.Length + i] != 0xFF && data[length - crc32.Length + i] != crc32[i])
                {
                }
                else
                    ret = true;
            }
            Array.Copy(crc32, 0, data, length - crc32.Length, crc32.Length);
            return ret;
        }

        private static int IterationParameters(byte[] data, int index, int address, int length, int sectorNo, int sectorSize, out int iterIndex, out int iterAddress, out int iterLength, out int iterTripletLength)
        {
            iterAddress = 0;
            int currentAddress = sectorSize * sectorNo;
            if (address - currentAddress >= 0 && iterAddress - currentAddress < sectorSize)
            {
                iterAddress = address - currentAddress;

            }

            int ret = _2_KB - iterAddress;
            if (length < ret)
                ret = length;

            iterIndex = index;
            for (int i = index; i < index + ret; i++)
            {
                if (data[i] != 0xFF)
                {
                    iterIndex = i;
                    break;
                }
            }
            iterAddress += iterIndex - index;

            iterLength = ret - (iterIndex - index);
            for (int i = index + ret; i > iterIndex; i--)
            {
                if (data[i - 1] != 0xFF)
                    break;
                else
                    iterLength--;
            }

            iterTripletLength = 1 + (((iterLength - 1) / 3)) * 3;
            return ret;
        }

        #endregion

        #region private

        private string[] namesBefore;
        private string currentNameBefore;
        private void MarkCurrentDataSource()
        {
            namesBefore = SerialPort.GetPortNames();
            currentNameBefore = TransportClient.DataSource.SourceName;
        }


        private bool ResolveCurrentDataSource(bool isFromUZB)
        {
            //Disconnect();
            bool ret = false;
            string newNameAfter = ResolveInner();

            if (newNameAfter == null && CheckDriverCallback != null && isFromUZB)
            {
                CheckDriverCallback();
                newNameAfter = ResolveInner();
            }

            if (newNameAfter != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    ret = Connect(new SerialPortProgrammerDataSource(newNameAfter)) == CommunicationStatuses.Done;
                    if (ret)
                        break;
                    Thread.Sleep(500);
                }
            }
            return ret;
        }

        private string ResolveInner()
        {
            bool prevDisabled = false;
            string newNameAfter = null;
            for (int i = 0; i < 120 && !prevDisabled; i++)
            {
                Thread.Sleep(500);
                string[] namesAfter = SerialPort.GetPortNames();
                if (!namesAfter.Contains(currentNameBefore))
                {
                    prevDisabled = true;
                }
            }

            if (prevDisabled) // previos port dissapear
            {
                for (int i = 0; i < 120 && newNameAfter == null; i++)
                {
                    string[] namesAfter = SerialPort.GetPortNames();
                    foreach (var item in namesAfter)
                    {
                        if (!namesBefore.Contains(item))
                        {
                            newNameAfter = item;
                            break;
                        }
                    }
                    Thread.Sleep(500);
                }
            }
            return newNameAfter;
        }

        private T AttemptWithSyncSignature<T>(Func<T> action) where T : ActionResult
        {
            return Tools.Attempt(AttemptsCount, (x) => x.IsStateCompleted, () =>
            {
                T res = action();
                if (!res.IsStateCompleted)
                    AttemptWithSync(() => Execute(new ReadFirstSignatureByteOperation()));
                return res;
            });
        }

        private T AttemptWithSync<T>(Func<T> action, Predicate<T> predicate) where T : ActionResult
        {
            return Tools.Attempt(7, (x) => x.IsStateCompleted && predicate(x), () =>
            {
                T res = action();
                if (!res.IsStateCompleted)
                    Execute(new SyncByteOperation());
                return res;
            });
        }

        private T AttemptWithSync<T>(Func<T> action) where T : ActionResult
        {
            return Tools.Attempt(7, (x) => x.IsStateCompleted, () =>
            {
                T res = action();
                if (!res.IsStateCompleted)
                    Execute(new SyncByteOperation());
                return res;
            });
        }

        private T CheckStateAfter<T>(Func<T> action, bool isIgnoreActionResult) where T : ActionResult
        {
            T ret = action();
            if (isIgnoreActionResult || ret.IsStateCompleted)
            {

                CheckStateResult res = Tools.Attempt(50,
                    (x) => (x.Value & 0x01) == 0 && (x.Value & 0x08) == 0,
                    () =>
                    {
                        Thread.Sleep(100);
                        return (CheckStateResult)Execute(new CheckStateOperation());
                    });
                CheckStateResult = res.Value;
                if (isIgnoreActionResult)
                    ret = res as T;
            }
            return ret;
        }

        #endregion

        public ActionResult RunCrcCheck()
        {
            return CheckStateAfter(() => Execute(new RunCrcCheckOperation()), false);
        }

        public CheckStateResult CheckState()
        {
            return (CheckStateResult)Execute(new CheckStateOperation());
        }

        public ActionResult EraseChip()
        {
            return CheckStateAfter(() => Execute(new EraseChipOperation()), false);
        }

        public ActionResult SetAPM_ClearAutoProg0()
        {
            return SetLockBitsByte(0x08, ClearAutoProg0(0xFF));
        }

        public ActionResult EraseSector(byte sectorNumber)
        {
            return CheckStateAfter(() => Execute(new EraseSectorOperation(sectorNumber)), false);
        }

        public ActionResult EnableEooSMode()
        {
            return CheckStateAfter(() => Execute(new EnableEooSModeOperation()), false);
        }

        public ActionResult DisableEooSMode()
        {
            return CheckStateAfter(() => Execute(new DisableEooSModeOperation()), false);
        }

        public ReadFlashResult ReadFlash(int address, int length, Action<int, int> progressCallback)
        {
            ReadFlashResult ret = (ReadFlashResult)Execute(new ReadFlashOperation(address, length, CommandsCount, progressCallback));
            byte[] data = ret.Value;
            int attempts = 30;
            int prevAddress = address;
            int prevCount = length;
            int commandCount = CommandsCount;
            while (!ret.IsStateCompleted && attempts > 0)
            {
                if (ret.ReadCount == 0)
                    attempts--;
                int newAddress = prevAddress + ret.ReadCount;
                int newCount = prevCount - ret.ReadCount;
                ReadFlashOperation op = new ReadFlashOperation(newAddress, newCount, commandCount > 1 ? --commandCount : commandCount, progressCallback);
                op.InitialProgressValue = newAddress - address;
                ret = (ReadFlashResult)Execute(op);
                Array.Copy(ret.Value, 0, data, newAddress - address, ret.ReadCount);
                prevAddress = newAddress;
                prevCount = newCount;
            }
            ret.Value = data;
            return ret;
        }

        public ActionResult WriteFlash(byte[] data, int index, int address, int length, Action<int, int> progressCallback)
        {
            ActionResult ret = new ActionResult();
            int sectorSize = _2_KB;
            int attempts = 30;
            int idx = index;
            int len = length;
            int commandCount = CommandsCount;
            for (int sectorNo = 0; sectorNo < 64; sectorNo++)
            {
                int sectorAddress = sectorNo * sectorSize;
                if (sectorAddress <= address - sectorSize)
                    continue;

                if (progressCallback != null && sectorAddress - address >= 0)
                    progressCallback(sectorAddress - address, length);
                int dataWritten = IterationParameters(data, idx, address, len, sectorNo, sectorSize, out int iterIndex, out int iterAddress, out int iterLength, out int iterTripletLength);
                idx += dataWritten;
                len -= dataWritten;

                if (iterLength > 0)
                {
                    if (iterLength - iterTripletLength > 0 && data[iterIndex] != 0xFF)
                    {
                        ret = Execute(new WriteSramOperation(data, iterIndex, iterAddress, 1, 1));
                        if (!ret.IsStateCompleted)
                            break;

                        CheckState();
                        ret = CheckStateAfter(() => Execute(new WriteFlashSectorOperation((byte)sectorNo)), !StopOnMissingLeadingBytes(sectorNo));
                        if (!ret.IsStateCompleted)
                            break;
                    }

                    if (iterLength - iterTripletLength > 1 && data[iterIndex + 1] != 0xFF)
                    {
                        ret = Execute(new WriteSramOperation(data, iterIndex + 1, iterAddress + 1, 1, 1));
                        if (!ret.IsStateCompleted)
                            break;

                        CheckState();
                        ret = CheckStateAfter(() => Execute(new WriteFlashSectorOperation((byte)sectorNo)), !StopOnMissingLeadingBytes(sectorNo));
                        if (!ret.IsStateCompleted)
                            break;
                    }


                    ret = Execute(new WriteSramOperation(data, iterIndex + iterLength - iterTripletLength, iterAddress + iterLength - iterTripletLength, iterTripletLength, commandCount));
                    while (!ret.IsStateCompleted && attempts > 0)
                    {
                        attempts--;
                        //attempts = 0;
                        ret = Execute(new WriteSramOperation(data, iterIndex + iterLength - iterTripletLength, iterAddress + iterLength - iterTripletLength, iterTripletLength, commandCount > 1 ? --commandCount : commandCount));
                    }
                    if (!ret.IsStateCompleted)
                        break;

                    CheckState();
                    ret = CheckStateAfter(() => Execute(new WriteFlashSectorOperation((byte)sectorNo)), !StopOnMissingLeadingBytes(sectorNo));
                    if (!ret.IsStateCompleted)
                        break;

                }

                if (sectorAddress + sectorSize >= index + length)
                {
                    progressCallback?.Invoke(length, length);
                    break;
                }
            }
            return ret;
        }

        private bool StopOnMissingLeadingBytes(int sectorNo)
        {
            bool ret = false;
            if (WriteFlashSectorCommand_StopOnBadResponseInSectorsBigEndian != null)
            {
                int index = sectorNo / 8;
                int mask = 0x01 << (sectorNo % 8);
                if (WriteFlashSectorCommand_StopOnBadResponseInSectorsBigEndian.Length > index)
                {
                    ret = (WriteFlashSectorCommand_StopOnBadResponseInSectorsBigEndian[WriteFlashSectorCommand_StopOnBadResponseInSectorsBigEndian.Length - 1 - index] & mask) > 0;
                }
            }
            return ret;
        }

        public ReadSramResult ReadSram(int address, int length)
        {
            ReadSramResult ret = (ReadSramResult)Execute(new ReadSramOperation(address, length, CommandsCount));
            byte[] data = ret.Value;
            int attempts = 20;
            int prevAddress = address;
            int prevCount = length;
            int commandCount = CommandsCount;
            while (!ret.IsStateCompleted && attempts > 0)
            {
                if (ret.ReadCount == 0)
                    attempts--;
                int newAddress = prevAddress + ret.ReadCount;
                int newCount = prevCount - ret.ReadCount;
                ret = (ReadSramResult)Execute(new ReadSramOperation(newAddress, newCount, commandCount > 1 ? --commandCount : commandCount));
                Array.Copy(ret.Value, 0, data, newAddress - address, ret.ReadCount);
                prevAddress = newAddress;
                prevCount = newCount;
            }
            ret.Value = data;
            return ret;
        }

        public ActionResult WriteSram(byte[] data, int sourceIndex, int address, int length)
        {
            WriteSramResult ret = (WriteSramResult)Execute(new WriteSramOperation(data, sourceIndex, address, length, CommandsCount));
            int attempts = 20;
            int prevAddress = address;
            int prevCount = length;
            int prevIndex = sourceIndex;
            int commandCount = CommandsCount;
            while (!ret.IsStateCompleted && attempts > 0)
            {
                if (ret.WriteCount == 0)
                    attempts--;
                int newAddress = prevAddress + ret.WriteCount;
                int newCount = prevCount - ret.WriteCount;
                int newIndex = prevIndex + ret.WriteCount;
                ret = (WriteSramResult)Execute(new WriteSramOperation(data, newIndex, newAddress, newCount, commandCount > 1 ? --commandCount : commandCount));
                prevAddress = newAddress;
                prevCount = newCount;
                prevIndex = newIndex;
            }
            return ret;
        }

        public ActionResult SendDataTest(byte[] values)
        {
            ActionResult ret = Execute(new SendDataTestOperation(values));
            return ret;
        }

        public ActionResult SetNvrByte(byte address, byte data)
        {
            ActionResult ret = Execute(new SetNvrByteOperation(address, data));
            return ret;
        }

        public ReadNvrByteResult ReadNvrByte(byte pageIndex, byte address)
        {
            ReadNvrByteResult ret = (ReadNvrByteResult)Execute(new ReadNvrByteOperation(pageIndex, address));
            return ret;
        }

        public ActionResult SetNvr(byte[] data)
        {
            return CheckStateAfter(() => AttemptWithSyncSignature(() => Execute(new SetNvrOperation(data, 1))), false);
        }

        public ReadNvrResult ReadNvr()
        {
            ReadNvrResult ret = AttemptWithSyncSignature(() => (ReadNvrResult)Execute(new ReadNvrOperation(1)));
            return ret;
        }

        public ActionResult SetLockBitsByte(byte address, byte data)
        {
            ActionResult ret = Execute(new SetLockBitsByteOperation(address, data));
            return ret;
        }

        public ReadLockBitsByteResult ReadLockBitsByte(byte pageIndex, byte address)
        {
            ReadLockBitsByteResult ret = (ReadLockBitsByteResult)Execute(new ReadLockBitsByteOperation(pageIndex, address));
            return ret;
        }

        public ActionResult SetLockBits(byte[] data)
        {
            return CheckStateAfter(() => AttemptWithSyncSignature(() => Execute(new SetLockBitsOperation(data, 1))), false);
        }

        public ReadLockBitsResult ReadLockBits()
        {
            return AttemptWithSyncSignature(() => (ReadLockBitsResult)Execute(new ReadLockBitsOperation(CommandsCount)));
        }

        public ReadSignatureByteResult ReadSignatureByte(int byteNo)
        {
            ReadSignatureByteResult ret = (ReadSignatureByteResult)Execute(new ReadSignatureByteOperation(byteNo));
            return ret;
        }

        public ActionResult SpiFwToggleSck()
        {
            return Execute(new SpiFwToggleSckOperation());
        }

        public ActionResult SpiFwAssertResetN()
        {
            return Execute(new SpiFwAssertResetNOperation());
        }

        public ActionResult SpiFwDeassertResetN()
        {
            return Execute(new SpiFwDeassertResetNOperation());
        }

        public ActionResult SpiFwInit()
        {
            return Execute(new SpiFwInitOperation());
        }

        public ActionResult SpiFwGetVersion()
        {
            return Execute(new SpiFwGetVersionOperation());
        }

        public bool EnableInterface(StartModes startMode)
        {
            bool ret = false;
            AutoProgDeviceTypes type = SerialPortDataSource.GetAutoProgType(TransportClient.DataSource.SourceName);

            if (type == AutoProgDeviceTypes.SD_USB_0000)
            {
                AttemptWithSync(() => Execute(new ReadFirstSignatureByteOperation()));
                ret = EnableInterfaceInner(true);
            }
            else if (type == AutoProgDeviceTypes.SD_USB_0001)
            {
                AttemptWithSync(() => Execute(new ReadFirstSignatureByteOperation()));
                EnableBasicSoftwareAPM();
                //Disconnect();
                Thread.Sleep(2000);
                Connect(new SerialPortProgrammerDataSource(DataSource.SourceName));
                ret = EnableInterfaceInner(true);
            }
            else if (type == AutoProgDeviceTypes.UZB_0000 || type == AutoProgDeviceTypes.UZB_0001)
            {
                MarkCurrentDataSource();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(500);

                    if (startMode == StartModes.ZnifferAPI)
                    {
                        EnableZnifferSoftwareAPM();
                    }
                    else
                    {
                        EnableBasicSoftwareAPM();
                    }
                    ret = true;
                    if (ret)
                    {
                        ret = ResolveCurrentDataSource(true);
                    }

                    type = SerialPortDataSource.GetAutoProgType(TransportClient.DataSource.SourceName);
                    if (type == AutoProgDeviceTypes.SD_USB_0000)
                        break;
                }
                if (ret)
                {
                    ret = EnableInterfaceInner(true);
                }
            }
            else if (type == AutoProgDeviceTypes.UART)
            {
                IsResetNSignalAsserted = false;

                if (startMode == StartModes.ZnifferAPI)
                {
                    EnableZnifferSoftwareAPM();
                    Connect(new SerialPortProgrammerDataSource(DataSource.SourceName));
                }
                else
                {
                    EnableBasicSoftwareAPM();
                    Connect(new SerialPortProgrammerDataSource(DataSource.SourceName));
                }
                // may be in auto prog so ask enable anyway
                ret = EnableInterfaceInner(true);
                if (!ret)
                {
                    if (AssertResetNSignalCallback != null)
                    {
                        #region USB resetN
                        IsResetNSignalAsserted = true;
                        AssertResetNSignalCallback();
                        ret = EnableInterfaceInner(true);
                        #endregion
                    }
                }
            }
            return ret;
        }

        private bool EnableInterfaceInner(bool useSyncOnEnable)
        {
            bool ret = false;
            ActionResult res = null;
            if (useSyncOnEnable)
            {
                res = AttemptWithSync(() => Execute(new EnableInterfaceOperation()));
            }
            else
            {
                res = Execute(new EnableInterfaceOperation());
            }
            if (res.State != ActionStates.Expired)
                ret = AttemptWithSync(() => Execute(new ReadFirstSignatureByteOperation()));
            return ret;
        }


        public void EnableZnifferSoftwareAPM()
        {
            //bool ret = false;
            //((ProgrammerFrameClient)FrameClient).IsParseEnabled = false;
            //((SerialPortTransportClient)TransportClient).StopReadingThread();
            //var res = Execute(new EnableZnifferApmOperation());
            //ret = true;
            //((ProgrammerFrameClient)FrameClient).IsParseEnabled = true;
            //return ret;
            SerialPortDataSource dataSource = TransportClient.DataSource as SerialPortDataSource;
            Disconnect();
            SendDataInner(dataSource, new byte[] { 0x23, 0x12, 0x00 });
        }

        public void EnableBasicSoftwareAPM()
        {
            //bool ret = false;
            //((ProgrammerFrameClient)FrameClient).IsParseEnabled = false;
            //((SerialPortTransportClient)TransportClient).StopReadingThread();
            //ActionResult res = Execute(new EnableBasicApmOperation());
            //ret = true;
            //((ProgrammerFrameClient)FrameClient).IsParseEnabled = true;
            //return ret;
            SerialPortDataSource dataSource = TransportClient.DataSource as SerialPortDataSource;
            Disconnect();
            SendDataInner(dataSource, new byte[] { 0x01, 0x03, 0x00, 0x27, 0xDB });
        }

        public bool ResetChip()
        {
            return ResetChip(true);
        }

        public bool ResetChip(bool isPortChanging)
        {
            bool ret = true;
            MarkCurrentDataSource();
            if (IsResetNSignalAsserted)
            {
                DeassertResetNSignalCallback();
            }
            else
            {
                //((SerialPortTransportClient)TransportClient).StopReadingThread();
                //Execute(new ResetChipOperation());
                SerialPortDataSource dataSource = TransportClient.DataSource as SerialPortDataSource;
                Disconnect();
                SendDataInner(dataSource, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
            }
            IsResetNSignalAsserted = false;
            if (isPortChanging)
                ret = ResolveCurrentDataSource(false);
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(1000);
                    ret = Connect() == CommunicationStatuses.Done;
                    if (ret)
                    {
                        var rr = SerialPortDataSource.GetAutoProgType(DataSource.SourceName);
                        if (rr == AutoProgDeviceTypes.SD_USB_0001 || rr == AutoProgDeviceTypes.UART)
                            break;
                    }
                }
            }
            return ret;
        }

        private static void SendDataInner(SerialPortDataSource dataSource, byte[] data)
        {
            SerialPort port = null;
            Stream innerStream = null;
            try
            {
                port = new SerialPort(dataSource.SourceName, (int)dataSource.BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                port.Open();
                Thread.Sleep(500);
                Thread.MemoryBarrier();
                innerStream = port.BaseStream;
                port.DiscardInBuffer();
                port.DiscardOutBuffer();
                Thread.MemoryBarrier();
                //GC.SuppressFinalize(port.BaseStream);
                port.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                ex.Message._EXLOG();
            }
            Thread.Sleep(1000);
            try
            {
                if (port != null && port.IsOpen)
                {
                    //GC.ReRegisterForFinalize(port.BaseStream);
                    innerStream = null;
                    port.Close();
                    port.Dispose();
                    port = null;
                }
            }
            catch { }
            try
            {
                if (port != null)
                    port.Dispose();
            }
            catch { }
        }

        public ActionResult ClearAPM_ClearAutoProg1()
        {
            return SetLockBitsByte(0x08, ClearAutoProg1(0xFF));
        }

        public ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            actionItem.CompletedCallback = completedCallback;
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Programmer";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            return SessionClient.ExecuteAsync(actionItem);
        }

        public ActionResult Execute(IActionItem actionItem)
        {
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Programmer";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            var token = SessionClient.ExecuteAsync(actionItem);
            token.WaitCompletedSignal();
            return token.Result;
        }

        public void PowerReset()
        {
            MarkCurrentDataSource();
            Disconnect();
            PowerResetCallback?.Invoke();
            ResolveCurrentDataSource(false);
        }
    }

    public enum StartModes
    {
        Default,
        NoSerialAPI,
        BasicAPI,
        ZnifferAPI,
    }
}
