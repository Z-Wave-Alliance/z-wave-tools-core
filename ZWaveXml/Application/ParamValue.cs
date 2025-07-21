/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.Xml.Application
{
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

        private byte GetByteValue(string optRef)
        {
            byte ret = 0;
            string[] tokens = optRef.Split('.');
            int refLevel = 0;
            if (tokens[0] == ParamDefinition.Name)
                refLevel++;
            ParamValue p = InnerValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[refLevel]);
            if (p != null)
            {
                if (tokens.Length > refLevel)
                {
                    refLevel++;
                    if (p.ParamDefinition.Mode == ParamModes.Property)
                    {
                        ParamValue s = p.InnerValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[refLevel]);
                        if (s != null)
                        {
                            ret = s.ByteValueList[0];
                        }
                    }
                    else 
                    {
                        ret = p.ByteValueList[0];
                    }
                }
                else
                {
                    ret = p.ByteValueList[0];
                }
            }
            return ret;
        }

        public bool IsIncluded(string optRef)
        {
            return GetByteValue(optRef) > 0;
        }
    }
}
