/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZWave.Xml.FrameHeader
{
    [Serializable]
    public class ParamValue
    {
        public Param ParamDefinition { get; set; }
        private string _textValue;
        public string TextValue
        {
            get
            {
                if (_textValue == null)
                {
                    if (TextValueList != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < TextValueList.Count; i++)
                        {
                            if (i < TextValueList.Count - 1)
                            {
                                sb.AppendLine(TextValueList[i]);
                            }
                            else
                            {
                                sb.Append(TextValueList[i]);
                            }
                        }
                        _textValue = sb.ToString();
                    }
                }
                return _textValue;
            }

        }
        public IList<string> TextValueList { get; set; }
        public IList<byte> ByteValueList { get; set; }
        public object Formatter { get; set; }
        public IList<ParamValue> InnerValues { get; set; }
        public bool HasTextValue { get; set; }
        public string ParamDefinitionTextPrefix { get; set; }
        public string ParamDefinitionTextSuffix { get; set; }

        public string ParamDefinitionText
        {
            get
            {
                if (ParamDefinition != null)
                    return ParamDefinitionTextPrefix + ParamDefinition.Text + ParamDefinitionTextSuffix;
                return ParamDefinitionTextPrefix + ParamDefinitionTextSuffix;
            }
        }
        
        public ParamValue()
        {
            TextValueList = new List<string>();
            ByteValueList = new List<byte>();
        }
        public static ParamValue CreateParamValue(Param item)
        {
            ParamValue ret = new ParamValue
            {
                ByteValueList = new List<byte>(),
                TextValueList = new List<string>(),
                ParamDefinition = item
            };
            return ret;
        }
    }
}
