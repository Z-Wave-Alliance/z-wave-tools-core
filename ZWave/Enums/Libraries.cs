/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System;
using System.Linq;

namespace ZWave.Enums
{
    [Serializable]
    public enum Libraries
    {
        NoLib = 0,
        ControllerStaticLib = 1,
        ControllerPortableLib = 2,
        EndDeviceLib = 3,
        EndDeviceLegacyLib = 4,
        InstallerLib = 5,
        EndDeviceLegacyRoutingLib = 6,
        ControllerBridgeLib = 7,
        EndDeviceSysTestLib = 12,
        TestController = 30,
        Manipulator = 31,
        DoorLockKeyPad = 32,
        PowerStrip = 33,
        SensorPIR = 34,
        SwitchOnOff = 35,
        WallController = 36,
        RailTest = 37,
        LEDBulb = 38,
        ZnifferPTI = 39,
        Thermostat = 40,
        MultilevelSensor = 41,
        MultilevelSwitch = 42,
        Meter = 44,
        LEDBulb_v1 = 45, //ZWave Led Bulbs may contain different classes
        LEDBulb_v2 = 46, //ZWave Led Bulbs may contain different classes
        LEDBulb_v3 = 47, //ZWave Led Bulbs may contain different classes
        LEDBulb_v4 = 48, //ZWave Led Bulbs may contain different classes
        BarrierOperator = 49,
        MultiNotificationSensor = 50, //A sensor PIR that supports a large number of notification types.
        ZnifferNCP = 200
    }

    public static class LibrariesExtensions
    {
        public static bool IsTerminalApplication(this Libraries library)
        {
            return new[]
            {
                Libraries.RailTest
            }.Contains(library);
        }

        public static bool IsSampleApplication(this Libraries library)
        {
            return new[]
            {
                Libraries.DoorLockKeyPad,
                Libraries.PowerStrip,
                Libraries.SensorPIR,
                Libraries.SwitchOnOff,
                Libraries.WallController,
                Libraries.LEDBulb,
                Libraries.Thermostat,
                Libraries.MultilevelSensor,
                Libraries.MultilevelSwitch,
                Libraries.Meter,
                Libraries.LEDBulb_v1,
                Libraries.LEDBulb_v2,
                Libraries.LEDBulb_v3,
                Libraries.LEDBulb_v4,
                Libraries.BarrierOperator,
                Libraries.MultiNotificationSensor
            }.Contains(library);
        }

        public static bool IsSerialApiController(this Libraries library)
        {
            return new[]
            {
                Libraries.ControllerBridgeLib,
                Libraries.ControllerPortableLib,
                Libraries.ControllerStaticLib,
                Libraries.InstallerLib
            }.Contains(library);
        }

        public static bool IsSerialApiEndDevice(this Libraries library)
        {
            return new[]
            {
                Libraries.EndDeviceLib,
                Libraries.EndDeviceLegacyLib,
                Libraries.EndDeviceLegacyRoutingLib
            }.Contains(library);
        }

        public static bool IsSerialApiTestLibrary(this Libraries library)
        {
            return new[]
            {
                Libraries.TestController,
                Libraries.EndDeviceSysTestLib
            }.Contains(library);
        }

        public static bool IsSerialApi(this Libraries library)
        {
            return library.IsSerialApiController() || library.IsSerialApiEndDevice() || library.IsSerialApiTestLibrary();
        }

        public static bool IsZniffer(this Libraries library)
        {
            return new[]
            {
                Libraries.ZnifferPTI,
                Libraries.ZnifferNCP
            }.Contains(library);
        }
    }
}
