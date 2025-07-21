/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils;
using System.ComponentModel;

namespace ZWave.Layers
{
    public class MultiSocketDataSource : DataSourceBase
    {
        public MultiSoketSourceTypes Type { get; set; }
        public int Port { get; set; }

        public MultiSocketDataSource()
        {
        }

        public MultiSocketDataSource(string sourceNames)
          : this(sourceNames, 0, null)
        {
        }

        public MultiSocketDataSource(string sourceNames, int port)
            : this(sourceNames, port, null)
        {
        }

        public MultiSocketDataSource(string sourceNames, int port, string args)
        {
            Type = MultiSoketSourceTypes.PTI;
            SourceName = sourceNames;
            Port = port;
            Args = args;
            IsUserDefined = false;
        }

        public MultiSocketDataSource(string sourceName, int port, string args, string alias)
            : this(sourceName, port, args)
        {
            Alias = alias;
        }

        public override bool Validate()
        {
            return !string.IsNullOrWhiteSpace(SourceName); //&& Tools.TryParseIPv4..?
        }

        public override string ToString()
        {
            if (Port == 0)
            {
                return $"{SourceName}";
            }
            else
            {
                return $"{SourceName}:{Port}";
            }
        }

        public bool IsUserDefined { get; set; }
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MultiSoketSourceTypes
    {
        [Description("PTI")]
        PTI = 0,
    }
}
