/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace ZWave.Xml.FrameHeader
{
    public partial class Param
    {

        private Header _parentHeaderField;
        [XmlIgnore]
        public Header ParentHeader
        {
            get
            {
                return _parentHeaderField;
            }
            set
            {
                _parentHeaderField = value;
                RaisePropertyChanged("ParentHeader");
            }
        }

        private Param _parentParamField;
        [XmlIgnore]
        public Param ParentParam
        {
            get
            {
                return _parentParamField;
            }
            set
            {
                _parentParamField = value;
                RaisePropertyChanged("ParentParam");
            }
        }

        [XmlIgnore]
        public ItemReference ItemRef { get; set; }

        [XmlIgnore]
        public ListReference ListRef { get; set; }

        [XmlIgnore]
        public Param PreviousVariableParam { get; set; }

        public Param()
        { }

        public Param(string name, string text)
            : this(name, text, zwParamType.HEX, null, 8, null, null, null)
        {
        }


        public Param(string name, string text, zwParamType type)
            : this(name, text, type, null, 8, null, null, null)
        {
            if (type == zwParamType.BOOLEAN)
            {
                Bits = 1;
            }
        }

        public Param(string name, string text, zwParamType type, int bits)
            : this(name, text, type, null, bits, null, null, null)
        {
        }

        public Param(string name, string text, zwParamType type, string defines, int bits)
            : this(name, text, type, defines, bits, null, null, null)
        {
        }

        public Param(string name, string text, Param[] innerParams)
            : this(name, text, zwParamType.HEX, null, 8, null, null, innerParams)
        {
        }

        public Param(string name, string text, zwParamType type, string sizeRef)
            : this(name, text, type, null, 8, sizeRef, null, null)
        {
        }

        public Param(string name, string text, zwParamType type, string sizeRef, string optRef)
            : this(name, text, type, null, 8, sizeRef, optRef, null)
        {
        }

        private Param(string name, string text, zwParamType type, string defines,
            int bits, string sizeRef, string optRef, Param[] innerParams)
        {
            this.Name = name;
            this.Text = text;
            this.Type = type;
            this.Defines = defines;
            this.Bits = (byte)bits;
            this.SizeRef = sizeRef;
            this.OptRef = optRef;
            if (innerParams != null)
            {
                this.Param1 = new Collection<Param>(innerParams);
            }
        }
    }
}
