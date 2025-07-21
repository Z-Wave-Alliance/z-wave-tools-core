/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

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
        MultiNotificationSensor = 50 //A sensor PIR that supports a large number of notification types.
    }
}
