/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Net;

namespace ZWave.Layers
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SoketSourceTypes
    {
        [Description("Z/IP")]
        ZIP = 0,
        [Description("Serial API")]
        TCP
    }

    public class SocketDataSource : DataSourceBase
    {
        public SoketSourceTypes Type { get; set; }
        public int Port { get; set; }
        public bool CanReconnect { get; set; }

        public SocketDataSource()
        {
        }

        public SocketDataSource(string sourceName, int port)
            : this(sourceName, port, null)
        {
        }

        public SocketDataSource(string sourceName, int port, string args)
        {
            Type = string.IsNullOrEmpty(args) ? SoketSourceTypes.TCP : SoketSourceTypes.ZIP;
            SourceName = sourceName;
            Port = port;
            Args = args;
        }

        public SocketDataSource(string sourceName, int port, string args, string alias)
            : this(sourceName, port, args)
        {
            Alias = alias;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(SourceName) && Port != 0;
        }

        public override string ToString()
        {
            return $"{SourceName}:{Port}";
        }

        public static bool TryParse(string addressString, out string ipAddress, out int port)
        {
            ipAddress = null;
            port = -1;
            var addressParts = Regex.Split(addressString, @"[\s:]+");
            if (addressParts.Length != 2)
            {
                return false;
            }
            if (!IPAddress.TryParse(addressParts[0], out IPAddress temp))
            {
                return false;
            }
            ipAddress = addressParts[0];
            if (!int.TryParse(addressParts[1], out port))
            {
                return false;
            }
            return true;
        }
    }
}
