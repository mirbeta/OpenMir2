using System.Runtime.InteropServices;
using SystemModule.Packets.ServerPackets;
using TouchSocket.Sockets;

namespace SystemModule.DataHandlingAdapters
{
    public class PacketFixedHeaderDataHandlingAdapter : CustomFixedHeaderDataHandlingAdapter<DataMessageFixedHeaderRequestInfo>
    {
        /// <summary>
        /// 接口实现，指示固定包头长度
        /// </summary>
        public override int HeaderLength => 20;

        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <returns></returns>
        protected override DataMessageFixedHeaderRequestInfo GetInstance()
        {
            return new DataMessageFixedHeaderRequestInfo();
        }
    }

    public class DataMessageFixedHeaderRequestInfo : IFixedHeaderRequestInfo
    {
        private int bodyLength;
        public int BodyLength => bodyLength;
        private ServerMessage _header;
        public ServerMessage Header => _header;
        private byte[] _message;
        public byte[] Message => _message;

        public bool OnParsingHeader(byte[] header)
        {
            if (!MemoryMarshal.TryRead(header, out _header))
                return false;
            this.bodyLength = _header.PackLength;
            return true;
        }

        public bool OnParsingBody(byte[] body)
        {
            this._message = body;
            return true;
        }
    }
}