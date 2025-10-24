/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_VERSION_V4
    {
        public const byte ID = 0x86;
        public const byte VERSION = 4;
        public partial class VERSION_COMMAND_CLASS_GET
        {
            public const byte ID = 0x13;
            public ByteValue requestedCommandClass = 0;
            public static implicit operator VERSION_COMMAND_CLASS_GET(byte[] data)
            {
                VERSION_COMMAND_CLASS_GET ret = new VERSION_COMMAND_CLASS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.requestedCommandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_COMMAND_CLASS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.requestedCommandClass.HasValue) ret.Add(command.requestedCommandClass);
                return ret.ToArray();
            }
        }
        public partial class VERSION_COMMAND_CLASS_REPORT
        {
            public const byte ID = 0x14;
            public ByteValue requestedCommandClass = 0;
            public ByteValue commandClassVersion = 0;
            public static implicit operator VERSION_COMMAND_CLASS_REPORT(byte[] data)
            {
                VERSION_COMMAND_CLASS_REPORT ret = new VERSION_COMMAND_CLASS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.requestedCommandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_COMMAND_CLASS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.requestedCommandClass.HasValue) ret.Add(command.requestedCommandClass);
                if (command.commandClassVersion.HasValue) ret.Add(command.commandClassVersion);
                return ret.ToArray();
            }
        }
        public partial class VERSION_GET
        {
            public const byte ID = 0x11;
            public static implicit operator VERSION_GET(byte[] data)
            {
                VERSION_GET ret = new VERSION_GET();
                return ret;
            }
            public static implicit operator byte[](VERSION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class VERSION_REPORT
        {
            public const byte ID = 0x12;
            public ByteValue zWaveLibraryType = 0;
            public ByteValue zWaveProtocolVersion = 0;
            public ByteValue zWaveProtocolSubVersion = 0;
            public ByteValue firmware0Version = 0;
            public ByteValue firmware0SubVersion = 0;
            public ByteValue hardwareVersion = 0;
            public ByteValue numberOfFirmwareTargets = 0;
            public class TVG
            {
                public ByteValue firmwareVersion = 0;
                public ByteValue firmwareSubVersion = 0;
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator VERSION_REPORT(byte[] data)
            {
                VERSION_REPORT ret = new VERSION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveLibraryType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zWaveProtocolVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zWaveProtocolSubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.firmware0Version = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.firmware0SubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hardwareVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfFirmwareTargets = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.numberOfFirmwareTargets; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.firmwareVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.firmwareSubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.zWaveLibraryType.HasValue) ret.Add(command.zWaveLibraryType);
                if (command.zWaveProtocolVersion.HasValue) ret.Add(command.zWaveProtocolVersion);
                if (command.zWaveProtocolSubVersion.HasValue) ret.Add(command.zWaveProtocolSubVersion);
                if (command.firmware0Version.HasValue) ret.Add(command.firmware0Version);
                if (command.firmware0SubVersion.HasValue) ret.Add(command.firmware0SubVersion);
                if (command.hardwareVersion.HasValue) ret.Add(command.hardwareVersion);
                if (command.numberOfFirmwareTargets.HasValue) ret.Add(command.numberOfFirmwareTargets);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.firmwareVersion.HasValue) ret.Add(item.firmwareVersion);
                        if (item.firmwareSubVersion.HasValue) ret.Add(item.firmwareSubVersion);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class VERSION_CAPABILITIES_GET
        {
            public const byte ID = 0x15;
            public static implicit operator VERSION_CAPABILITIES_GET(byte[] data)
            {
                VERSION_CAPABILITIES_GET ret = new VERSION_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](VERSION_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class VERSION_CAPABILITIES_REPORT
        {
            public const byte ID = 0x16;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte version
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte commandClass
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte zWaveSoftware
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte migrationSupport
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public static implicit operator VERSION_CAPABILITIES_REPORT(byte[] data)
            {
                VERSION_CAPABILITIES_REPORT ret = new VERSION_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class VERSION_ZWAVE_SOFTWARE_GET
        {
            public const byte ID = 0x17;
            public static implicit operator VERSION_ZWAVE_SOFTWARE_GET(byte[] data)
            {
                VERSION_ZWAVE_SOFTWARE_GET ret = new VERSION_ZWAVE_SOFTWARE_GET();
                return ret;
            }
            public static implicit operator byte[](VERSION_ZWAVE_SOFTWARE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class VERSION_ZWAVE_SOFTWARE_REPORT
        {
            public const byte ID = 0x18;
            public const byte sdkVersionBytesCount = 3;
            public byte[] sdkVersion = new byte[sdkVersionBytesCount];
            public const byte applicationFrameworkApiVersionBytesCount = 3;
            public byte[] applicationFrameworkApiVersion = new byte[applicationFrameworkApiVersionBytesCount];
            public const byte applicationFrameworkBuildNumberBytesCount = 2;
            public byte[] applicationFrameworkBuildNumber = new byte[applicationFrameworkBuildNumberBytesCount];
            public const byte hostInterfaceVersionBytesCount = 3;
            public byte[] hostInterfaceVersion = new byte[hostInterfaceVersionBytesCount];
            public const byte hostInterfaceBuildNumberBytesCount = 2;
            public byte[] hostInterfaceBuildNumber = new byte[hostInterfaceBuildNumberBytesCount];
            public const byte zWaveProtocolVersionBytesCount = 3;
            public byte[] zWaveProtocolVersion = new byte[zWaveProtocolVersionBytesCount];
            public const byte zWaveProtocolBuildNumberBytesCount = 2;
            public byte[] zWaveProtocolBuildNumber = new byte[zWaveProtocolBuildNumberBytesCount];
            public const byte applicationVersionBytesCount = 3;
            public byte[] applicationVersion = new byte[applicationVersionBytesCount];
            public const byte applicationBuildNumberBytesCount = 2;
            public byte[] applicationBuildNumber = new byte[applicationBuildNumberBytesCount];
            public static implicit operator VERSION_ZWAVE_SOFTWARE_REPORT(byte[] data)
            {
                VERSION_ZWAVE_SOFTWARE_REPORT ret = new VERSION_ZWAVE_SOFTWARE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sdkVersion = (data.Length - index) >= sdkVersionBytesCount ? new byte[sdkVersionBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.sdkVersion[0] = data[index++];
                    if (data.Length > index) ret.sdkVersion[1] = data[index++];
                    if (data.Length > index) ret.sdkVersion[2] = data[index++];
                    ret.applicationFrameworkApiVersion = (data.Length - index) >= applicationFrameworkApiVersionBytesCount ? new byte[applicationFrameworkApiVersionBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.applicationFrameworkApiVersion[0] = data[index++];
                    if (data.Length > index) ret.applicationFrameworkApiVersion[1] = data[index++];
                    if (data.Length > index) ret.applicationFrameworkApiVersion[2] = data[index++];
                    ret.applicationFrameworkBuildNumber = (data.Length - index) >= applicationFrameworkBuildNumberBytesCount ? new byte[applicationFrameworkBuildNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.applicationFrameworkBuildNumber[0] = data[index++];
                    if (data.Length > index) ret.applicationFrameworkBuildNumber[1] = data[index++];
                    ret.hostInterfaceVersion = (data.Length - index) >= hostInterfaceVersionBytesCount ? new byte[hostInterfaceVersionBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.hostInterfaceVersion[0] = data[index++];
                    if (data.Length > index) ret.hostInterfaceVersion[1] = data[index++];
                    if (data.Length > index) ret.hostInterfaceVersion[2] = data[index++];
                    ret.hostInterfaceBuildNumber = (data.Length - index) >= hostInterfaceBuildNumberBytesCount ? new byte[hostInterfaceBuildNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.hostInterfaceBuildNumber[0] = data[index++];
                    if (data.Length > index) ret.hostInterfaceBuildNumber[1] = data[index++];
                    ret.zWaveProtocolVersion = (data.Length - index) >= zWaveProtocolVersionBytesCount ? new byte[zWaveProtocolVersionBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zWaveProtocolVersion[0] = data[index++];
                    if (data.Length > index) ret.zWaveProtocolVersion[1] = data[index++];
                    if (data.Length > index) ret.zWaveProtocolVersion[2] = data[index++];
                    ret.zWaveProtocolBuildNumber = (data.Length - index) >= zWaveProtocolBuildNumberBytesCount ? new byte[zWaveProtocolBuildNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zWaveProtocolBuildNumber[0] = data[index++];
                    if (data.Length > index) ret.zWaveProtocolBuildNumber[1] = data[index++];
                    ret.applicationVersion = (data.Length - index) >= applicationVersionBytesCount ? new byte[applicationVersionBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.applicationVersion[0] = data[index++];
                    if (data.Length > index) ret.applicationVersion[1] = data[index++];
                    if (data.Length > index) ret.applicationVersion[2] = data[index++];
                    ret.applicationBuildNumber = (data.Length - index) >= applicationBuildNumberBytesCount ? new byte[applicationBuildNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.applicationBuildNumber[0] = data[index++];
                    if (data.Length > index) ret.applicationBuildNumber[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_ZWAVE_SOFTWARE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.sdkVersion != null)
                {
                    foreach (var tmp in command.sdkVersion)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.applicationFrameworkApiVersion != null)
                {
                    foreach (var tmp in command.applicationFrameworkApiVersion)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.applicationFrameworkBuildNumber != null)
                {
                    foreach (var tmp in command.applicationFrameworkBuildNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.hostInterfaceVersion != null)
                {
                    foreach (var tmp in command.hostInterfaceVersion)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.hostInterfaceBuildNumber != null)
                {
                    foreach (var tmp in command.hostInterfaceBuildNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.zWaveProtocolVersion != null)
                {
                    foreach (var tmp in command.zWaveProtocolVersion)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.zWaveProtocolBuildNumber != null)
                {
                    foreach (var tmp in command.zWaveProtocolBuildNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.applicationVersion != null)
                {
                    foreach (var tmp in command.applicationVersion)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.applicationBuildNumber != null)
                {
                    foreach (var tmp in command.applicationBuildNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class VERSION_MIGRATION_CAPABILITIES_GET
        {
            public const byte ID = 0x19;
            public static implicit operator VERSION_MIGRATION_CAPABILITIES_GET(byte[] data)
            {
                VERSION_MIGRATION_CAPABILITIES_GET ret = new VERSION_MIGRATION_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](VERSION_MIGRATION_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class VERSION_MIGRATION_CAPABILITIES_REPORT
        {
            public const byte ID = 0x1A;
            public ByteValue numberOfSupportedMigrationOperations = 0;
            public IList<byte> migrationOperationId = new List<byte>();
            public static implicit operator VERSION_MIGRATION_CAPABILITIES_REPORT(byte[] data)
            {
                VERSION_MIGRATION_CAPABILITIES_REPORT ret = new VERSION_MIGRATION_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSupportedMigrationOperations = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.migrationOperationId = new List<byte>();
                    for (int i = 0; i < ret.numberOfSupportedMigrationOperations; i++)
                    {
                        if (data.Length > index) ret.migrationOperationId.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_MIGRATION_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.numberOfSupportedMigrationOperations.HasValue) ret.Add(command.numberOfSupportedMigrationOperations);
                if (command.migrationOperationId != null)
                {
                    foreach (var tmp in command.migrationOperationId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class VERSION_MIGRATION_SET
        {
            public const byte ID = 0x1B;
            public ByteValue migrationOperationId = 0;
            public static implicit operator VERSION_MIGRATION_SET(byte[] data)
            {
                VERSION_MIGRATION_SET ret = new VERSION_MIGRATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.migrationOperationId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_MIGRATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.migrationOperationId.HasValue) ret.Add(command.migrationOperationId);
                return ret.ToArray();
            }
        }
        public partial class VERSION_MIGRATION_GET
        {
            public const byte ID = 0x1C;
            public ByteValue migrationOperationId = 0;
            public static implicit operator VERSION_MIGRATION_GET(byte[] data)
            {
                VERSION_MIGRATION_GET ret = new VERSION_MIGRATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.migrationOperationId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_MIGRATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.migrationOperationId.HasValue) ret.Add(command.migrationOperationId);
                return ret.ToArray();
            }
        }
        public partial class VERSION_MIGRATION_REPORT
        {
            public const byte ID = 0x1D;
            public ByteValue migrationOperationId = 0;
            public ByteValue migrationStatus = 0;
            public const byte estimatedTimeOfCompletionSecondsBytesCount = 2;
            public byte[] estimatedTimeOfCompletionSeconds = new byte[estimatedTimeOfCompletionSecondsBytesCount];
            public static implicit operator VERSION_MIGRATION_REPORT(byte[] data)
            {
                VERSION_MIGRATION_REPORT ret = new VERSION_MIGRATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.migrationOperationId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.migrationStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.estimatedTimeOfCompletionSeconds = (data.Length - index) >= estimatedTimeOfCompletionSecondsBytesCount ? new byte[estimatedTimeOfCompletionSecondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.estimatedTimeOfCompletionSeconds[0] = data[index++];
                    if (data.Length > index) ret.estimatedTimeOfCompletionSeconds[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_MIGRATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V4.ID);
                ret.Add(ID);
                if (command.migrationOperationId.HasValue) ret.Add(command.migrationOperationId);
                if (command.migrationStatus.HasValue) ret.Add(command.migrationStatus);
                if (command.estimatedTimeOfCompletionSeconds != null)
                {
                    foreach (var tmp in command.estimatedTimeOfCompletionSeconds)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

