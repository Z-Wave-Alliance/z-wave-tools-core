/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Xml.Serialization;
using Utils;

namespace ZWave.Xml.FrameHeader
{
    public partial class Validation
    {
        [XmlIgnore]
        public DataIndex DataIndex { get; set; }

        public Validation()
        { }

        public Validation(string paramName, byte paramHexValue)
        {
            this.ParamName = paramName;
            this.ParamHexValue = paramHexValue.ToString("X2");
        }


        public void Initialize(Header header)
        {
            bool _isFound = false;
            if (header != null)
            {
                if (header.BaseHeaderDefinition != null)
                {
                    DataIndex = new DataIndex(0);
                    _isFound = FrameDefinition.ProcessHeader(DataIndex, header.BaseHeaderDefinition.Param, ParamName);
                }
                if (!_isFound)
                {
                    DataIndex.IndexInData = header.BaseHeaderDefinition.Length;
                    _isFound = FrameDefinition.ProcessHeader(DataIndex, header.Param, ParamName);
                }
            }
        }
    }

    public class DataIndex
    {
        public DataIndex(int startIndex)
        {
            IndexInData = startIndex;
        }
        public int IndexInData { get; set; }
        public byte SizeInData { get; set; }
        public int MaskInData { get; set; }
        public byte OffsetInData { get; set; }
    }

    public class ValidationLeaf
    {
        public Validation Rule { get; set; }
        public Header Header { get; set; }
        public Dictionary<ushort, IList<ValidationLeaf>> Results { get; set; }
    }
}
