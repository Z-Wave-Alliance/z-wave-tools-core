/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Tasks;
using ZWave.Enums;
using ZWave.Exceptions;
using ZWave.Layers;
using ZWave.Layers.Application;

namespace ZWave.BasicApplication.Devices
{
    public interface ITestInterfaceDevice : IApplicationClient
    {
        byte[][] GetPins(ChipTypes chipSeries);
        byte[][] GetPins(ChipTypes chipSeries, PortMaskTypes maskType);
        bool ComparePins(ChipTypes chipSeries, PortMaskTypes maskType, bool isHigh);
        void SetPins(ChipTypes chipSeries, PortMaskTypes maskType, bool isHigh);
        void InitPinsInput(ChipTypes chipSeries, PortMaskTypes maskType, bool isEnable);
        void InitPinsOutput(ChipTypes chipSeries, PortMaskTypes maskType, bool isHigh);
        void Btn1Click();

        TestInterfaceSendDataResult TestInterfaceSendData2(byte[] TestInterfaceCmd, int timeoutMs);
        ReturnValueResult TestInterfaceSendData(byte[] TestInterfaceCmd, int timeoutMs);
        ReturnValueResult TestInterfaceSendData(byte[] TestInterfaceCmd, bool isRetransmitWithNextFuncId, int timeoutMs);
        ActionToken TestInterfaceSendData(byte[] TestInterfaceCmd, int timeoutMs, Action<IActionItem> completedCallback);
        ActionToken TestInterfaceNoiseData(byte[] TestInterfaceCmd, Action<ReturnValueResult> sendCallback, int intervalMs, int timeoutMs);
        bool TestInterfaceNoiseData(byte[] data, Action<ReturnValueResult> sendCallback, object intervalMs, object timeoutMs);
    }

    public enum PortMaskTypes
    {
        ButtonsAndLeds,
        Buttons,
        Manipulator
    }

    public class TestInterfaceDevice : ApplicationClient, ITestInterfaceDevice
    {
        public TestInterfaceDevice(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(ApiTypes.Basic, sessionId, sc, fc, tc)
        {
        }

        public byte[][] GetPins(ChipTypes chipSeries)
        {
            var pn = new byte[6][];
            for (byte port = 0; port < 6; port++)
            {
                var res = TestInterfaceSendData(new byte[] { (byte)ProgrammerCommandTypes.GetGpio, (byte)port }, true, 1000);
                if (!res)
                {
                    throw new OperationException();
                }
                if (res && res.ByteArray != null && res.ByteArray.Length == 5)
                {
                    pn[port] = res.ByteArray.Skip(1).ToArray();
                }
            }
            return pn;
        }

        public byte[][] GetPins(ChipTypes chipSeries, PortMaskTypes maskType)
        {
            var pn = new byte[6][];
            for (byte port = 0; port < 6; port++)
            {
                var pinMask = BrdPins.GetPortMask(chipSeries, maskType, port, null);
                if (pinMask != null)
                {
                    var res = TestInterfaceSendData(new byte[] { (byte)ProgrammerCommandTypes.GetGpio, (byte)port }, true, 1000);
                    if (!res)
                    {
                        throw new OperationException();
                    }
                    if (res && res.ByteArray != null && res.ByteArray.Length == 5)
                    {
                        pn[port] = res.ByteArray.Skip(1).ToArray();
                        var maskedValue = new[]
                        {
                            pn[port][0] & pinMask[0],
                            pn[port][1] & pinMask[1],
                            pn[port][2] & pinMask[2],
                            pn[port][3] & pinMask[3]
                        }
                        .Select(x => (byte)x).ToArray();
                        $"{DataSource.SourceName} {maskType} GET PORT_{port}={maskedValue.GetHex()}"._DLOG();
                    }
                }
            }
            return pn;
        }

        public bool ComparePins(ChipTypes chipSeries, PortMaskTypes maskType, bool isHigh)
        {
            bool ret = false;
            var pn = GetPins(chipSeries, maskType);
            for (byte port = 0; port < 6; port++)
            {
                var pinValue = pn[port];
                var pinMask = BrdPins.GetPortMask(chipSeries, maskType, port, null);
                if (pinMask != null)
                {
                    if (isHigh)
                    {
                        if (maskType == PortMaskTypes.Manipulator)
                        {
                            ret = (pinValue[0] & pinMask[0]) > 0
                                 || (pinValue[1] & pinMask[1]) > 0
                                 || (pinValue[2] & pinMask[2]) > 0
                                 || (pinValue[3] & pinMask[3]) > 0;
                        }
                        else
                        {
                            ret = (pinValue[0] & pinMask[0]) == pinMask[0]
                                && (pinValue[1] & pinMask[1]) == pinMask[1]
                                && (pinValue[2] & pinMask[2]) == pinMask[2]
                                && (pinValue[3] & pinMask[3]) == pinMask[3];
                        }
                    }
                    else
                    {
                        if (maskType == PortMaskTypes.Manipulator)
                        {
                            ret = !((pinValue[0] & pinMask[0]) == pinMask[0]
                              && (pinValue[1] & pinMask[1]) == pinMask[1]
                              && (pinValue[2] & pinMask[2]) == pinMask[2]
                              && (pinValue[3] & pinMask[3]) == pinMask[3]);

                        }
                        else
                        {
                            ret = (pinValue[0] & pinMask[0]) == 0
                               && (pinValue[1] & pinMask[1]) == 0
                               && (pinValue[2] & pinMask[2]) == 0
                               && (pinValue[3] & pinMask[3]) == 0;
                        }
                    }
                    if (!ret)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        public void SetPins(ChipTypes chipSeries, PortMaskTypes maskType, bool isHigh)
        {
            for (byte port = 0; port < 6; port++)
            {
                var stateMask = BrdPins.GetPortMask(chipSeries, maskType, port, isHigh);
                var pinsMask = BrdPins.GetPortMask(chipSeries, maskType, port, null);
                if (pinsMask != null)
                {
                    var res = TestInterfaceSendData(new byte[] { (byte)ProgrammerCommandTypes.SetGpio, port }
                    .Concat(pinsMask).Concat(stateMask).ToArray(), true, 1000);
                    if (!res)
                    {
                        throw new OperationException();
                    }
                    $"{DataSource.SourceName} {maskType} SET PORT_{port}={stateMask.GetHex()}"._DLOG();
                }
            }
        }

        public void InitPinsOutput(ChipTypes chipSeries, PortMaskTypes maskType, bool isHigh)
        {
            for (byte port = 0; port < 6; port++)
            {
                var stateMask = BrdPins.GetPortMask(chipSeries, maskType, port, isHigh);
                var pinsMask = BrdPins.GetPortMask(chipSeries, maskType, port, null);
                if (pinsMask != null)
                {
                    var res = TestInterfaceSendData(new byte[] { (byte)ProgrammerCommandTypes.ConfigGpio, port, 1 }
                    .Concat(pinsMask).Concat(stateMask).ToArray(), true, 1000);
                    if (!res)
                    {
                        throw new OperationException();
                    }

                }
            }
        }

        public void InitPinsInput(ChipTypes chipSeries, PortMaskTypes maskType, bool isEnable)
        {
            for (byte port = 0; port < 6; port++)
            {
                var stateMask = BrdPins.GetPortMask(chipSeries, maskType, port, isEnable);
                var pinsMask = BrdPins.GetPortMask(chipSeries, maskType, port, null);
                if (pinsMask != null)
                {
                    var res = TestInterfaceSendData(new byte[] { (byte)ProgrammerCommandTypes.ConfigGpio, port, 0 }
                    .Concat(pinsMask).Concat(stateMask).ToArray(), true, 1000);
                    if (!res)
                    {
                        throw new OperationException();
                    }
                }
            }
        }

        public void Btn1Click()
        {
            var res = TestInterfaceSendData(new byte[] { (byte)ProgrammerCommandTypes.ButtonPress, 2, ButtonOperations.Press, 1 }, true, 500);
            if (!res)
            {
                throw new OperationException();
            }
        }

        public TestInterfaceSendDataResult TestInterfaceSendData2(byte[] TestInterfaceCmd, int timeoutMs)
        {
            TestInterfaceSendDataTask operation = new TestInterfaceSendDataTask(TestInterfaceCmd, timeoutMs);
            return (TestInterfaceSendDataResult)Execute(operation);
        }

        public ReturnValueResult TestInterfaceSendData(byte[] TestInterfaceCmd, int timeoutMs)
        {
            TestInterfaceSendDataOperation operation = new TestInterfaceSendDataOperation(TestInterfaceCmd, timeoutMs);
            return (ReturnValueResult)Execute(operation);
        }

        public ReturnValueResult TestInterfaceSendData(byte[] TestInterfaceCmd, bool isRetransmitWithNextFuncId, int timeoutMs)
        {
            TestInterfaceSendDataOperation operation = new TestInterfaceSendDataOperation(TestInterfaceCmd, isRetransmitWithNextFuncId, timeoutMs);
            return (ReturnValueResult)Execute(operation);
        }

        public ActionToken TestInterfaceSendData(byte[] TestInterfaceCmd, int timeoutMs, Action<IActionItem> completedCallback)
        {
            TestInterfaceSendDataOperation operation = new TestInterfaceSendDataOperation(TestInterfaceCmd, timeoutMs);
            return ExecuteAsync(operation, completedCallback);
        }

        public ActionToken TestInterfaceNoiseData(byte[] TestInterfaceCmd, Action<ReturnValueResult> sendCallback, int intervalMs, int timeoutMs)
        {
            TestInterfaceNoiseDataOperation testInterfaceNoiseDataTask = new TestInterfaceNoiseDataOperation(TestInterfaceCmd, sendCallback, intervalMs, timeoutMs);
            return ExecuteAsync(testInterfaceNoiseDataTask, null);
        }

        public bool TestInterfaceNoiseData(byte[] data, Action<ReturnValueResult> sendCallback, object intervalMs, object timeoutMs)
        {
            throw new NotImplementedException();
        }

        public virtual ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            var actionBase = actionItem as ActionBase;
            if (actionBase != null)
            {
                actionBase.CompletedCallback = completedCallback;
                actionBase.Token.LogEntryPointCategory = "Basic";
                actionBase.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            return SessionClient.ExecuteAsync(actionBase);
        }

        public virtual ActionResult Execute(IActionItem actionItem)
        {
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Basic";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            ActionToken token = SessionClient.ExecuteAsync(action);
            ActionResult ret = WaitCompletedSignal(token);
            return ret;
        }

        public static class BrdPins
        {
            private static int rgbCounter = 0;
            public static void NextRgb()
            {
                rgbCounter++;
                if (rgbCounter > 5)
                {
                    rgbCounter = 0;
                }
            }

            private static byte GetRGBMask()
            {
                byte ret = 0x1C;
                switch (rgbCounter)
                {
                    case 0:
                        ret = 0x0C;
                        break;
                    case 1:
                        ret = 0x14;
                        break;
                    case 2:
                        ret = 0x18;
                        break;
                    case 3:
                        ret = 0x10;
                        break;
                    case 4:
                        ret = 0x08;
                        break;
                    case 5:
                        ret = 0x04;
                        break;
                    default:
                        break;
                }

                return ret;
            }

            public static byte[] GetPortMask(ChipTypes chipSeries, PortMaskTypes maskType, byte port, bool? isHigh)
            {
                byte[] ret = null;
                switch (chipSeries)
                {
                    case ChipTypes.ZGM130S_BRD4200A:
                    case ChipTypes.ZGM130S_BRD4202A:
                    case ChipTypes.ZGM130S_BRD4207A:
                    case ChipTypes.RZ13_BRD4201B:
                    case ChipTypes.RZ13_BRD4203A:
                    case ChipTypes.RZ13_BRD4209A:
                        switch (maskType)
                        {
                            case PortMaskTypes.ButtonsAndLeds:
                                switch (port)
                                {
                                    case 0: // port A: pin 2 (LED2-skip), pin 3 (LED3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x08 } : new byte[4];
                                        break;
                                    case 2: // port C: pin 10 (BTN2), pin 11 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x0C, 0 } : new byte[4];
                                        break;
                                    //case 3: // port D: pin 10 (R), pin 11 (G), pin 12 (B)
                                    //    ret = isHigh ?? false ? new byte[] { 0, 0, GetRGBMask(), 0 } : new byte[] { 0, 0, 0x1C, 0 };
                                    //    break;
                                    case 5: // port F: pin 3 (LED1), pin 4 (LED0), pin 6 (BTN0), pin 7 (BTN1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xD8 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Buttons:
                                switch (port)
                                {
                                    case 2: // port C: pin 10 (BTN2), pin 11 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x0C, 0 } : new byte[4];
                                        break;
                                    case 5: // port F: pin 6 (BTN0), pin 7 (BTN1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xC0 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Manipulator:
                                switch (port)
                                {
                                    case 2: // port C: pin 8 (TST), pin 9 (SW1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x03, 0 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case ChipTypes.ZG13L_BRD4201C:
                    case ChipTypes.ZG13S_BRD4201D:
                        switch (maskType)
                        {
                            case PortMaskTypes.ButtonsAndLeds:
                                switch (port)
                                {
                                    case 2: // port C: pin 10 (BTN2), pin 11 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x0C, 0 } : new byte[4];
                                        break;
                                    case 5: // port F: pin 3 (LED1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x08 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Buttons:
                                switch (port)
                                {
                                    case 2: // port C: pin 10 (BTN2), pin 11 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x0C, 0 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Manipulator:
                                break;
                            default:
                                break;
                        }
                        break;
                    case ChipTypes.ZG23_BRD4204A:
                    case ChipTypes.ZG23_BRD4204C:
                    case ChipTypes.ZG23_BRD4204D:
                    case ChipTypes.ZG23_BRD4210A:
                    case ChipTypes.ZGM230S_BRD4205A:
                    case ChipTypes.ZGM230S_BRD4205B:
                        switch (maskType)
                        {
                            case PortMaskTypes.ButtonsAndLeds:
                                switch (port)
                                {
                                    case 0: // port A: pin 0 (LED3), pin 5 (BTN0), pin 6 (LED0), pin 7 (LED1), pin 10 (LED2-skip)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xE1 } : new byte[4];
                                        break;
                                    case 2: // port C: pin 5 (BTN2), pin 7 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xA0 } : new byte[4];
                                        break;
                                    case 3: // port D: pin 2 (BTN1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x04 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Buttons:
                                switch (port)
                                {
                                    case 0: // port A: pin 5 (BTN0)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x20 } : new byte[4];
                                        break;
                                    case 2: // port C: pin 5 (BTN2), pin 7 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xA0 } : new byte[4];
                                        break;
                                    case 3: // port D: pin 2 (BTN1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x04 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Manipulator:
                                switch (port)
                                {
                                    case 2: // port C: pin 3 (TST), pin 0 (SW1),
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x09 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case ChipTypes.xG28_BRD4400A:
                    case ChipTypes.xG28_BRD4401A:
                    case ChipTypes.xG28_BRD4400B:
                    case ChipTypes.xG28_BRD4401B:
                    case ChipTypes.xG28_BRD4400C:
                    case ChipTypes.xG28_BRD4401C:
                        switch (maskType)
                        {
                            case PortMaskTypes.ButtonsAndLeds:
                                switch (port)
                                {
                                    case 0: // port A: pin 11 (LED2-skip), pin 12 (LED3), pin 13 (BTN0), pin 14 (BTN1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x70, 0 } : new byte[4];
                                        break;
                                    case 1: // port B: pin 4 (LED0), pin 5 (LED1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0x30 } : new byte[4];
                                        break;
                                    case 2: // port C: pin 5 (BTN2), pin 7 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xA0 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Buttons:
                                switch (port)
                                {
                                    case 0: // port A: pin 13 (BTN0), pin 14 (BTN1)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0, 0xE1 } : new byte[4];
                                        break;
                                    case 2: // port C: pin 5 (BTN2), pin 7 (BTN3)
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x60, 0 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case PortMaskTypes.Manipulator:
                                switch (port)
                                {
                                    case 3: // port D: pin 9 (TST), pin 10 (SW1),
                                        ret = isHigh ?? true ? new byte[] { 0, 0, 0x06, 0 } : new byte[4];
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                return ret;
            }
        }

        public static class ButtonOperations
        {
            public const byte Press = 0x01;
            public const byte Hold = 0x02;
        }

        public enum ProgrammerCommandTypes
        {
            Ack = 0x06,
            StoreHomeId = 0x13,
            GetSoftwareVersion = 0x26,
            ProgEnable = 0x40,
            ReadSignatureByte = 0x47,
            ProgDisable = 0x48,
            ToggleEeprom = 0x51,
            ButtonPress = 0x73,
            GetLedState = 0x74,
            ConfigGpio = 0x75,
            SetGpio = 0x76,
            GetGpio = 0x77,
            ReadNvr = 0xFE,
        }
    }
}
