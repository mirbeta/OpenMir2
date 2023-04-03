using System.Runtime.InteropServices;
using SystemModule.CoreSocket;
using SystemModule.Packets.ServerPackets;

namespace SystemModule.DataHandlingAdapters
{
    public class PlayerDataFixedHeaderDataHandlingAdapter : CustomFixedHeaderDataHandlingAdapter<PlayerDataMessageFixedHeaderRequestInfo>
    {
        /// <summary>
        /// 接口实现，指示固定包头长度
        /// </summary>
        public override int HeaderLength => 6;

        private static readonly PlayerDataMessageFixedHeaderRequestInfo instance = new PlayerDataMessageFixedHeaderRequestInfo();

        private static PlayerDataMessageFixedHeaderRequestInfo Instance => instance;

        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <returns></returns>
        protected override PlayerDataMessageFixedHeaderRequestInfo GetInstance()
        {
            return Instance;
        }
    }

    public class PlayerDataMessageFixedHeaderRequestInfo : IFixedHeaderRequestInfo
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