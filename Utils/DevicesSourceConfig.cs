/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Utils
{
    public static class DevicesSourceConfig
    {
        public static string IntegrationTestsConfigFileName { get { return "IntegrationTestsSources.config"; } }
        public static string IntegrationTestsProfileFileName { get { return "IntegrationTestsProfile.config"; } }

        public static SourcesProfile LoadIntegrationTestsConfig()
        {
            string activeProfileName = "BUILD";
            if (File.Exists(IntegrationTestsProfileFileName))
            {
                using (var configReader = new StreamReader(IntegrationTestsProfileFileName))
                {
                    var configserializer = new XmlSerializer(typeof(ActiveSourcesProfile));
                    var activeProfile = (ActiveSourcesProfile)configserializer.Deserialize(configReader);
                    activeProfileName = activeProfile.Name;
                }
            }
            using (var reader = new StreamReader(IntegrationTestsConfigFileName))
            {
                var serializer = new XmlSerializer(typeof(DevicesConfiguration));
                var allProfiles = (DevicesConfiguration)serializer.Deserialize(reader);
                var ret = allProfiles.
                    SourcesProfiles.
                    FirstOrDefault(prof => string.Compare(prof.Name, activeProfileName, StringComparison.InvariantCulture) == 0);
                return ret;
            }
        }
    }

    public class ActiveSourcesProfile
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public class DevicesConfiguration
    {
        public SourcesProfile[] SourcesProfiles { get; set; } 
    }

    public class SourcesProfile
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlArrayItem(Type = typeof(SerialSource)),
        XmlArrayItem(Type = typeof(StaticControllerSource)),
        XmlArrayItem(Type = typeof(EndDeviceSource)),
        XmlArrayItem(Type = typeof(BridgeSource))]
        public SerialSource[] SerialSources { get; set; }
        public ZnifferSource ZnifferSource { get; set; }
        public TcpSource[] TcpSources { get; set; }
        public ZipSource ZipSource { get; set; }
        public Security Security { get; set; }
        public string OtaFileName { get; set; }
    }

    public class SerialSource
    {
        public string PortAlias { get; set; }
    }

    public class StaticControllerSource : SerialSource
    {
    }

    public class EndDeviceSource : SerialSource
    {
    }

    public class BridgeSource : SerialSource
    {
    }

    public class ZnifferSource : SerialSource
    {
    }

    public class TcpSource
    {
        public string Address { get; set; }
        public int Port { get; set; }
    }

    public class ZipSource
    {
        public string AddressIPv4 { get; set; }
        public string AddressIPv6 { get; set; }
        public int UdpPort { get; set; }
        public int DtlsPort { get; set; }
        public string Psk { get; set; }
    }

    public class Security
    {
        [XmlElement(DataType = "hexBinary", IsNullable = true)]
        public byte[][] NetworkKeys { get; set; }
        [XmlElement(DataType = "hexBinary", IsNullable = false)]
        public byte[] Dsk { get; set; }
    }
}
