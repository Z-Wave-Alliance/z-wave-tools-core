using ZWave.Devices;
using ZWave.Enums;

namespace BasicApplicationTests
{
    public class FrameLogRecord
    {
        public NodeTag FromNode { get; private set; }
        public NodeTag ToNode { get; private set; }
        public byte CmdId { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is FrameLogRecord frameRecord))
                return false;

            if (this == frameRecord)
                return true;

            return this.CmdId == frameRecord.CmdId &&
                this.FromNode == frameRecord.FromNode &&
                this.ToNode == frameRecord.ToNode;
        }

        public override int GetHashCode()
        {
            return FromNode.GetHashCode() ^ ToNode.GetHashCode() ^ CmdId;
        }

        private FrameLogRecord()
        {
        }

        public static FrameLogRecord Create(NodeTag fromNode, NodeTag toNode, byte cmdId)
        {
            return new FrameLogRecord
            {
                FromNode = fromNode,
                ToNode = toNode,
                CmdId = cmdId
            };
        }
    }
}
