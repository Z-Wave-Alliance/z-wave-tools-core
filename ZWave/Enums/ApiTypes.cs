/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    public enum ApiTypes
    {
        /// <summary>
        /// Legacy Zniffer ZW030x, ZW040x, ZW050x series
        /// </summary>
        Zniffer = 0x00,
        /// <summary>
        /// Serial API 
        /// </summary>
        Basic = 0x01,
        /// <summary>
        /// Legacy Programmer ZW010x, ZW020x, ZW030x, ZW040x, ZW050x series
        /// </summary>
        Programmer = 0x02,
        /// <summary>
        /// Z/IP Gateway
        /// </summary>
        Zip = 0x03,
        /// <summary>
        /// Reserved
        /// </summary>
        Reserved4 = 0x04,
        /// <summary>
        /// Reserved
        /// </summary>
        Reserved5 = 0x05,
        /// <summary>
        /// Information attached into trace, could be keys, sourcename, etc
        /// </summary>
        Attachment = 0x06,
        /// <summary>
        /// Any text including log messages
        /// </summary>
        Text = 0x07,
        /// <summary>
        /// XModem OTW
        /// </summary>
        XModem = 0x08,
        /// <summary>
        /// Sniffer PTI
        /// </summary>
        Pti = 0x09,
        /// <summary>
        /// Sniffer PTI with not completed fragments
        /// </summary>
        PtiDiagnostic = 0x0A,
        /// <summary>
        /// UIC 
        /// </summary>
        Uic = 0x0B,
        /// <summary>
        /// WSTK board instrument
        /// </summary>
        WstkInstrument = 0x80,
        /// <summary>
        /// Programmable attenuator instrument
        /// </summary>
        AttenuatorRudatInstrument = 0x81,
        /// <summary>
        /// Power supply unit instrument
        /// </summary>
        PsuE364xInstrument = 0x82,
        /// <summary>
        /// Digital multimeter instrument
        /// </summary>
        Dmm7510Instrument = 0x83,
        /// <summary>
        /// Sample's IO command line interface
        /// </summary>
        SampleCLI = 0x84
    }
}
