using System.Runtime.InteropServices;
using SystemModule.CoreSocket;
using SystemModule.Packets.ServerPackets;

namespace SystemModule.DataHandlingAdapters
{
    /// <summary>
    /// 服务器消息适配器
    /// 用于服务器之间的消息传递
    /// </summary>
    public class ServerPacketFixedHeaderDataHandlingAdapter : CustomFixedHeaderDataHandlingAdapter<ServerDataMessageFixedHeaderRequestInfo>
    {
        /// <summary>
        /// 接口实现，指示固定包头长度
        /// </summary>
        public override int HeaderLength => 6;

        private static readonly ServerDataMessageFixedHeaderRequestInfo instance = new ServerDataMessageFixedHeaderRequestInfo();

        private static ServerDataMessageFixedHeaderRequestInfo Instance => instance;

        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <returns></returns>
        protected override ServerDataMessageFixedHeaderRequestInfo GetInstance()
        {
            return Instance;
        }
    }

    public class ServerDataMessageFixedHeaderRequestInfo : IFixedHeaderRequestInfo
    {
        private int bodyLength;
        public int BodyLength => bodyLength;
        private ServerDataPacket _header;
        public ServerDataPacket Header => _header;
        private byte[] _message;
        public byte[] Message => _message;

        public bool OnParsingHeader(byte[] header)
        {
            if (!MemoryMarshal.TryRead(header, out _header))
            {
                return false;
            }

            this.bodyLength = _header.PacketLen;
            return true;
        }

        public bool OnParsingBody(byte[] body)
        {
            this._message = body;
            return true;
        }
    }
}