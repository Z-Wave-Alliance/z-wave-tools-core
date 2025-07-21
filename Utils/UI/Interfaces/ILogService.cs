/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils.UI.Enums;
using Utils.UI.Logging;

namespace Utils.UI.Interfaces
{
    public interface ILogService
    {
        ILogModel LogModel { get; set; }
        void LogTitle(string text, LogRawData logRawData = null);
        void LogFail(string text, LogRawData logRawData = null);
        void LogWarning(string text, LogRawData logRawData = null);
        void LogOk(string text, LogRawData logRawData = null);
        void Log(string text);
        void Log(string text, LogSettings logSettings, LogRawData logRawData = null);
        LogPacket CreatePacket(string text, Dyes dye, bool isBold, LogLevels level, LogRawData logRawData);
    }
}
