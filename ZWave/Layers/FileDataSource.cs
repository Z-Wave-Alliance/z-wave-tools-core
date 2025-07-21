/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.IO;
using Utils;

namespace ZWave.Layers
{
    public class FileDataSource : DataSourceBase
    {
        public FileDataSource(string fileName)
        {
            SourceName = fileName;
        }

        public bool IsKeepOpened { get; set; }
        public Pair<long, long> Range { get; set; }

        int displayPathChars = 50;
        public override string ToString()
        {
            string ret = "<empty>";
            if (!string.IsNullOrWhiteSpace(SourceName))
            {
                if (SourceName.Length > displayPathChars)
                {
                    ret = "..." + SourceName.Substring(SourceName.Length - displayPathChars);
                }
                else
                {
                    ret = SourceName;
                }
            }
            return ret;
        }
    }
}
