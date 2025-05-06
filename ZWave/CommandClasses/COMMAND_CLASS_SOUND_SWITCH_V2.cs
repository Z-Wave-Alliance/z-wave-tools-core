using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SOUND_SWITCH_V2
    {
        public const byte ID = 0x79;
        public const byte VERSION = 2;
        public partial class SOUND_SWITCH_TONES_NUMBER_GET
        {
            public const byte ID = 0x01;
            public static implicit operator SOUND_SWITCH_TONES_NUMBER_GET(byte[] data)
            {
                SOUND_SWITCH_TONES_NUMBER_GET ret = new SOUND_SWITCH_TONES_NUMBER_GET();
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONES_NUMBER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_TONES_NUMBER_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue supportedTones = 0;
            public static implicit operator SOUND_SWITCH_TONES_NUMBER_REPORT(byte[] data)
            {
                SOUND_SWITCH_TONES_NUMBER_REPORT ret = new SOUND_SWITCH_TONES_NUMBER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedTones = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONES_NUMBER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.supportedTones.HasValue) ret.Add(command.supportedTones);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_TONE_INFO_GET
        {
            public const byte ID = 0x03;
            public ByteValue toneIdentifier = 0;
            public static implicit operator SOUND_SWITCH_TONE_INFO_GET(byte[] data)
            {
                SOUND_SWITCH_TONE_INFO_GET ret = new SOUND_SWITCH_TONE_INFO_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.toneIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONE_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.toneIdentifier.HasValue) ret.Add(command.toneIdentifier);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_TONE_INFO_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue toneIdentifier = 0;
            public const byte toneDurationBytesCount = 2;
            public byte[] toneDuration = new byte[toneDurationBytesCount];
            public ByteValue nameLength = 0;
            public IList<byte> name = new List<byte>();
            public static implicit operator SOUND_SWITCH_TONE_INFO_REPORT(byte[] data)
            {
                SOUND_SWITCH_TONE_INFO_REPORT ret = new SOUND_SWITCH_TONE_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.toneIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.toneDuration = (data.Length - index) >= toneDurationBytesCount ? new byte[toneDurationBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.toneDuration[0] = data[index++];
                    if (data.Length > index) ret.toneDuration[1] = data[index++];
                    ret.nameLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.name = new List<byte>();
                    for (int i = 0; i < ret.nameLength; i++)
                    {
                        if (data.Length > index) ret.name.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONE_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.toneIdentifier.HasValue) ret.Add(command.toneIdentifier);
                if (command.toneDuration != null)
                {
                    foreach (var tmp in command.toneDuration)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nameLength.HasValue) ret.Add(command.nameLength);
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
        public partial class SOUND_SWITCH_CONFIGURATION_SET
        {
            public const byte ID = 0x05;
            public ByteValue volume = 0;
            public ByteValue defaultToneIdentifier = 0;
            public static implicit operator SOUND_SWITCH_CONFIGURATION_SET(byte[] data)
            {
                SOUND_SWITCH_CONFIGURATION_SET ret = new SOUND_SWITCH_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.volume = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.defaultToneIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.volume.HasValue) ret.Add(command.volume);
                if (command.defaultToneIdentifier.HasValue) ret.Add(command.defaultToneIdentifier);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_CONFIGURATION_GET
        {
            public const byte ID = 0x06;
            public static implicit operator SOUND_SWITCH_CONFIGURATION_GET(byte[] data)
            {
                SOUND_SWITCH_CONFIGURATION_GET ret = new SOUND_SWITCH_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_CONFIGURATION_REPORT
        {
            public const byte ID = 0x07;
            public ByteValue volume = 0;
            public ByteValue defaultToneIdentifer = 0;
            public static implicit operator SOUND_SWITCH_CONFIGURATION_REPORT(byte[] data)
            {
                SOUND_SWITCH_CONFIGURATION_REPORT ret = new SOUND_SWITCH_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.volume = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.defaultToneIdentifer = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.volume.HasValue) ret.Add(command.volume);
                if (command.defaultToneIdentifer.HasValue) ret.Add(command.defaultToneIdentifer);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_TONE_PLAY_SET
        {
            public const byte ID = 0x08;
            public ByteValue toneIdentifier = 0;
            public ByteValue playCommandToneVolume = 0;
            public static implicit operator SOUND_SWITCH_TONE_PLAY_SET(byte[] data)
            {
                SOUND_SWITCH_TONE_PLAY_SET ret = new SOUND_SWITCH_TONE_PLAY_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.toneIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.playCommandToneVolume = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONE_PLAY_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.toneIdentifier.HasValue) ret.Add(command.toneIdentifier);
                if (command.playCommandToneVolume.HasValue) ret.Add(command.playCommandToneVolume);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_TONE_PLAY_GET
        {
            public const byte ID = 0x09;
            public static implicit operator SOUND_SWITCH_TONE_PLAY_GET(byte[] data)
            {
                SOUND_SWITCH_TONE_PLAY_GET ret = new SOUND_SWITCH_TONE_PLAY_GET();
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONE_PLAY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SOUND_SWITCH_TONE_PLAY_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue toneIdentifier = 0;
            public ByteValue playCommandToneVolume = 0;
            public static implicit operator SOUND_SWITCH_TONE_PLAY_REPORT(byte[] data)
            {
                SOUND_SWITCH_TONE_PLAY_REPORT ret = new SOUND_SWITCH_TONE_PLAY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.toneIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.playCommandToneVolume = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SOUND_SWITCH_TONE_PLAY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SOUND_SWITCH_V2.ID);
                ret.Add(ID);
                if (command.toneIdentifier.HasValue) ret.Add(command.toneIdentifier);
                if (command.playCommandToneVolume.HasValue) ret.Add(command.playCommandToneVolume);
                return ret.ToArray();
            }
        }
    }
}

