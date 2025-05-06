using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZIP_NAMING
    {
        public const byte ID = 0x68;
        public const byte VERSION = 1;
        public partial class ZIP_NAMING_NAME_SET
        {
            public const byte ID = 0x01;
            public IList<byte> name = new List<byte>();
            public static implicit operator ZIP_NAMING_NAME_SET(byte[] data)
            {
                ZIP_NAMING_NAME_SET ret = new ZIP_NAMING_NAME_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.name = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.name.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_NAMING_NAME_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_NAMING.ID);
                ret.Add(ID);
                if (command.name != null)
                {
                    foreach (var tmp in command.name)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ZIP_NAMING_NAME_GET
        {
            public const byte ID = 0x02;
            public static implicit operator ZIP_NAMING_NAME_GET(byte[] data)
            {
                ZIP_NAMING_NAME_GET ret = new ZIP_NAMING_NAME_GET();
                return ret;
            }
            public static implicit operator byte[](ZIP_NAMING_NAME_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_NAMING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZIP_NAMING_NAME_REPORT
        {
            public const byte ID = 0x03;
            public IList<byte> name = new List<byte>();
            public static implicit operator ZIP_NAMING_NAME_REPORT(byte[] data)
            {
                ZIP_NAMING_NAME_REPORT ret = new ZIP_NAMING_NAME_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.name = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.name.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_NAMING_NAME_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_NAMING.ID);
                ret.Add(ID);
                if (command.name != null)
                {
                    foreach (var tmp in command.name)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ZIP_NAMING_LOCATION_SET
        {
            public const byte ID = 0x04;
            public IList<byte> location = new List<byte>();
            public static implicit operator ZIP_NAMING_LOCATION_SET(byte[] data)
            {
                ZIP_NAMING_LOCATION_SET ret = new ZIP_NAMING_LOCATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.location = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.location.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_NAMING_LOCATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_NAMING.ID);
                ret.Add(ID);
                if (command.location != null)
                {
                    foreach (var tmp in command.location)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ZIP_NAMING_LOCATION_GET
        {
            public const byte ID = 0x05;
            public static implicit operator ZIP_NAMING_LOCATION_GET(byte[] data)
            {
                ZIP_NAMING_LOCATION_GET ret = new ZIP_NAMING_LOCATION_GET();
                return ret;
            }
            public static implicit operator byte[](ZIP_NAMING_LOCATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_NAMING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZIP_NAMING_LOCATION_REPORT
        {
            public const byte ID = 0x06;
            public IList<byte> location = new List<byte>();
            public static implicit operator ZIP_NAMING_LOCATION_REPORT(byte[] data)
            {
                ZIP_NAMING_LOCATION_REPORT ret = new ZIP_NAMING_LOCATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.location = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.location.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_NAMING_LOCATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_NAMING.ID);
                ret.Add(ID);
                if (command.location != null)
                {
                    foreach (var tmp in command.location)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

