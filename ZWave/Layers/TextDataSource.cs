/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.IO;
using Utils;

namespace ZWave.Layers
{
    public class TextDataSource : DataSourceBase
    {
        public TextWriter Writer { get; private set; }
        public TextReader Reader { get; private set; }
        public TextDataSource(TextWriter writer, TextReader reader)
        {
            Writer = writer;
            Reader = reader;
        }

        public override string ToString()
        {
            return SourceName;
        }
    }
}
