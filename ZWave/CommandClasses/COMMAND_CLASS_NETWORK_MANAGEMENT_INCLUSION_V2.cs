using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2
    {
        public const byte ID = 0x34;
        public const byte VERSION = 2;
        public partial class FAILED_NODE_REMOVE
        {
            public const byte ID = 0x07;
            public ByteValue seqNo = 0;
            public ByteValue nodeId = 0;
            public static implicit operator FAILED_NODE_REMOVE(byte[] data)
            {
                FAILED_NODE_REMOVE ret = new FAILED_NODE_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FAILED_NODE_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class FAILED_NODE_REMOVE_STATUS
        {
            public const byte ID = 0x08;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public ByteValue nodeId = 0;
            public static implicit operator FAILED_NODE_REMOVE_STATUS(byte[] data)
            {
                FAILED_NODE_REMOVE_STATUS ret = new FAILED_NODE_REMOVE_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FAILED_NODE_REMOVE_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class NODE_ADD
        {
            public const byte ID = 0x01;
            public ByteValue seqNo = 0;
            public ByteValue reserved = 0;
            public ByteValue mode = 0;
            public ByteValue txOptions = 0;
            public static implicit operator NODE_ADD(byte[] data)
            {
                NODE_ADD ret = new NODE_ADD();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.txOptions = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_ADD command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.mode.HasValue) ret.Add(command.mode);
                if (command.txOptions.HasValue) ret.Add(command.txOptions);
                return ret.ToArray();
            }
        }
        public partial class NODE_ADD_STATUS
        {
            public const byte ID = 0x02;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public ByteValue reserved = 0;
            public ByteValue newNodeId = 0;
            public ByteValue nodeInfoLength = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte zWaveProtocolSpecificPart1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte zWaveProtocolSpecificPart2
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte opt
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClass = new List<byte>();
            public ByteValue grantedKeys = 0;
            public ByteValue kexFailType = 0;
            public static implicit operator NODE_ADD_STATUS(byte[] data)
            {
                NODE_ADD_STATUS ret = new NODE_ADD_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.newNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeInfoLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClass = new List<byte>();
                    for (int i = 0; i < ret.nodeInfoLength - 6; i++)
                    {
                        if (data.Length > index) ret.commandClass.Add(data[index++]);
                    }
                    ret.grantedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.kexFailType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_ADD_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.newNodeId.HasValue) ret.Add(command.newNodeId);
                if (command.nodeInfoLength.HasValue) ret.Add(command.nodeInfoLength);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                if (command.commandClass != null)
                {
                    foreach (var tmp in command.commandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.grantedKeys.HasValue) ret.Add(command.grantedKeys);
                if (command.kexFailType.HasValue) ret.Add(command.kexFailType);
                return ret.ToArray();
            }
        }
        public partial class NODE_REMOVE
        {
            public const byte ID = 0x03;
            public ByteValue seqNo = 0;
            public ByteValue reserved = 0;
            public ByteValue mode = 0;
            public static implicit operator NODE_REMOVE(byte[] data)
            {
                NODE_REMOVE ret = new NODE_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class NODE_REMOVE_STATUS
        {
            public const byte ID = 0x04;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public ByteValue nodeid = 0;
            public static implicit operator NODE_REMOVE_STATUS(byte[] data)
            {
                NODE_REMOVE_STATUS ret = new NODE_REMOVE_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_REMOVE_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                return ret.ToArray();
            }
        }
        public partial class FAILED_NODE_REPLACE
        {
            public const byte ID = 0x09;
            public ByteValue seqNo = 0;
            public ByteValue nodeId = 0;
            public ByteValue txOptions = 0;
            public ByteValue mode = 0;
            public static implicit operator FAILED_NODE_REPLACE(byte[] data)
            {
                FAILED_NODE_REPLACE ret = new FAILED_NODE_REPLACE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.txOptions = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FAILED_NODE_REPLACE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.txOptions.HasValue) ret.Add(command.txOptions);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class FAILED_NODE_REPLACE_STATUS
        {
            public const byte ID = 0x0A;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public ByteValue nodeId = 0;
            public ByteValue grantedKeys = 0;
            public ByteValue kexFailType = 0;
            public static implicit operator FAILED_NODE_REPLACE_STATUS(byte[] data)
            {
                FAILED_NODE_REPLACE_STATUS ret = new FAILED_NODE_REPLACE_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.grantedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.kexFailType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FAILED_NODE_REPLACE_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.grantedKeys.HasValue) ret.Add(command.grantedKeys);
                if (command.kexFailType.HasValue) ret.Add(command.kexFailType);
                return ret.ToArray();
            }
        }
        public partial class NODE_NEIGHBOR_UPDATE_REQUEST
        {
            public const byte ID = 0x0B;
            public ByteValue seqNo = 0;
            public ByteValue nodeId = 0;
            public static implicit operator NODE_NEIGHBOR_UPDATE_REQUEST(byte[] data)
            {
                NODE_NEIGHBOR_UPDATE_REQUEST ret = new NODE_NEIGHBOR_UPDATE_REQUEST();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_NEIGHBOR_UPDATE_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class NODE_NEIGHBOR_UPDATE_STATUS
        {
            public const byte ID = 0x0C;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public static implicit operator NODE_NEIGHBOR_UPDATE_STATUS(byte[] data)
            {
                NODE_NEIGHBOR_UPDATE_STATUS ret = new NODE_NEIGHBOR_UPDATE_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_NEIGHBOR_UPDATE_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class RETURN_ROUTE_ASSIGN
        {
            public const byte ID = 0x0D;
            public ByteValue seqNo = 0;
            public ByteValue sourceNodeId = 0;
            public ByteValue destinationNodeId = 0;
            public static implicit operator RETURN_ROUTE_ASSIGN(byte[] data)
            {
                RETURN_ROUTE_ASSIGN ret = new RETURN_ROUTE_ASSIGN();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sourceNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.destinationNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RETURN_ROUTE_ASSIGN command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.sourceNodeId.HasValue) ret.Add(command.sourceNodeId);
                if (command.destinationNodeId.HasValue) ret.Add(command.destinationNodeId);
                return ret.ToArray();
            }
        }
        public partial class RETURN_ROUTE_ASSIGN_COMPLETE
        {
            public const byte ID = 0x0E;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public static implicit operator RETURN_ROUTE_ASSIGN_COMPLETE(byte[] data)
            {
                RETURN_ROUTE_ASSIGN_COMPLETE ret = new RETURN_ROUTE_ASSIGN_COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RETURN_ROUTE_ASSIGN_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class RETURN_ROUTE_DELETE
        {
            public const byte ID = 0x0F;
            public ByteValue seqNo = 0;
            public ByteValue nodeId = 0;
            public static implicit operator RETURN_ROUTE_DELETE(byte[] data)
            {
                RETURN_ROUTE_DELETE ret = new RETURN_ROUTE_DELETE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RETURN_ROUTE_DELETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class RETURN_ROUTE_DELETE_COMPLETE
        {
            public const byte ID = 0x10;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public static implicit operator RETURN_ROUTE_DELETE_COMPLETE(byte[] data)
            {
                RETURN_ROUTE_DELETE_COMPLETE ret = new RETURN_ROUTE_DELETE_COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RETURN_ROUTE_DELETE_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class NODE_ADD_KEYS_REPORT
        {
            public const byte ID = 0x11;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte requestCsa
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue requestedKeys = 0;
            public static implicit operator NODE_ADD_KEYS_REPORT(byte[] data)
            {
                NODE_ADD_KEYS_REPORT ret = new NODE_ADD_KEYS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.requestedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_ADD_KEYS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.requestedKeys.HasValue) ret.Add(command.requestedKeys);
                return ret.ToArray();
            }
        }
        public partial class NODE_ADD_KEYS_SET
        {
            public const byte ID = 0x12;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte accept
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte grantCsa
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public ByteValue grantedKeys = 0;
            public static implicit operator NODE_ADD_KEYS_SET(byte[] data)
            {
                NODE_ADD_KEYS_SET ret = new NODE_ADD_KEYS_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.grantedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_ADD_KEYS_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.grantedKeys.HasValue) ret.Add(command.grantedKeys);
                return ret.ToArray();
            }
        }
        public partial class NODE_ADD_DSK_REPORT
        {
            public const byte ID = 0x13;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte inputDskLength
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public const byte dskBytesCount = 16;
            public byte[] dsk = new byte[dskBytesCount];
            public static implicit operator NODE_ADD_DSK_REPORT(byte[] data)
            {
                NODE_ADD_DSK_REPORT ret = new NODE_ADD_DSK_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = (data.Length - index) >= dskBytesCount ? new byte[dskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dsk[0] = data[index++];
                    if (data.Length > index) ret.dsk[1] = data[index++];
                    if (data.Length > index) ret.dsk[2] = data[index++];
                    if (data.Length > index) ret.dsk[3] = data[index++];
                    if (data.Length > index) ret.dsk[4] = data[index++];
                    if (data.Length > index) ret.dsk[5] = data[index++];
                    if (data.Length > index) ret.dsk[6] = data[index++];
                    if (data.Length > index) ret.dsk[7] = data[index++];
                    if (data.Length > index) ret.dsk[8] = data[index++];
                    if (data.Length > index) ret.dsk[9] = data[index++];
                    if (data.Length > index) ret.dsk[10] = data[index++];
                    if (data.Length > index) ret.dsk[11] = data[index++];
                    if (data.Length > index) ret.dsk[12] = data[index++];
                    if (data.Length > index) ret.dsk[13] = data[index++];
                    if (data.Length > index) ret.dsk[14] = data[index++];
                    if (data.Length > index) ret.dsk[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NODE_ADD_DSK_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_ADD_DSK_SET
        {
            public const byte ID = 0x14;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte inputDskLength
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte accept
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public IList<byte> inputDsk = new List<byte>();
            public static implicit operator NODE_ADD_DSK_SET(byte[] data)
            {
                NODE_ADD_DSK_SET ret = new NODE_ADD_DSK_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.inputDsk = new List<byte>();
                    for (int i = 0; i < ret.properties1.inputDskLength; i++)
                    {
                        if (data.Length > index) ret.inputDsk.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_ADD_DSK_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.inputDsk != null)
                {
                    foreach (var tmp in command.inputDsk)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

