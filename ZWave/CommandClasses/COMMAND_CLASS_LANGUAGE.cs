using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_LANGUAGE
    {
        public const byte ID = 0x89;
        public const byte VERSION = 1;
        public partial class LANGUAGE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator LANGUAGE_GET(byte[] data)
            {
                LANGUAGE_GET ret = new LANGUAGE_GET();
                return ret;
            }
            public static implicit operator byte[](LANGUAGE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_LANGUAGE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class LANGUAGE_REPORT
        {
            public const byte ID = 0x03;
            public const byte languageBytesCount = 3;
            public byte[] language = new byte[languageBytesCount];
            public const byte countryBytesCount = 2;
            public byte[] country = new byte[countryBytesCount];
            public static implicit operator LANGUAGE_REPORT(byte[] data)
            {
                LANGUAGE_REPORT ret = new LANGUAGE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.language = (data.Length - index) >= languageBytesCount ? new byte[languageBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.language[0] = data[index++];
                    if (data.Length > index) ret.language[1] = data[index++];
                    if (data.Length > index) ret.language[2] = data[index++];
                    ret.country = (data.Length - index) >= countryBytesCount ? new byte[countryBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.country[0] = data[index++];
                    if (data.Length > index) ret.country[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](LANGUAGE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_LANGUAGE.ID);
                ret.Add(ID);
                if (command.language != null)
                {
                    foreach (var tmp in command.language)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.country != null)
                {
                    foreach (var tmp in command.country)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class LANGUAGE_SET
        {
            public const byte ID = 0x01;
            public const byte languageBytesCount = 3;
            public byte[] language = new byte[languageBytesCount];
            public const byte countryBytesCount = 2;
            public byte[] country = new byte[countryBytesCount];
            public static implicit operator LANGUAGE_SET(byte[] data)
            {
                LANGUAGE_SET ret = new LANGUAGE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.language = (data.Length - index) >= languageBytesCount ? new byte[languageBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.language[0] = data[index++];
                    if (data.Length > index) ret.language[1] = data[index++];
                    if (data.Length > index) ret.language[2] = data[index++];
                    ret.country = (data.Length - index) >= countryBytesCount ? new byte[countryBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.country[0] = data[index++];
                    if (data.Length > index) ret.country[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](LANGUAGE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_LANGUAGE.ID);
                ret.Add(ID);
                if (command.language != null)
                {
                    foreach (var tmp in command.language)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.country != null)
                {
                    foreach (var tmp in command.country)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

