using System;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;

namespace SystemModule
{
    public class EDCode
    {
        private const int BufferSize = 10000;

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static ClientMesaagePacket DecodePacket(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = 0;
            var encBuf = PacketEncoder.DecodeBuf(tempBuf, str.Length, ref buffLen);
            return Packets.ToPacket<ClientMesaagePacket>(encBuf);
        }

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static ClientMesaagePacket DecodePacket(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffLen = 0;
            var encBuf = PacketEncoder.DecodeBuf(data, data.Length, ref buffLen);
            return Packets.ToPacket<ClientMesaagePacket>(encBuf);
        }

        /// <summary>
        /// 解密Buffer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] DecodeBuff(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffLen = 0;
            return PacketEncoder.DecodeBuf(data, data.Length, ref buffLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string DeCodeString(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var nLen = 0;
            var bSrc = HUtil32.GetBytes(str);
            var encBuf = PacketEncoder.DecodeBuf(bSrc, bSrc.Length, ref nLen);
            return HUtil32.GetString(encBuf, 0, nLen);
        }
        
        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string DeCodeString(Span<char> str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var nLen = 0;
            var encBuf = PacketEncoder.DecodeBuf(str, str.Length, ref nLen);
            return HUtil32.GetString(encBuf);
        }

        public static byte[] DecodeBuffer(string strSrc)
        {
            if (strSrc == null) throw new ArgumentNullException(nameof(strSrc));
            var bSrc = HUtil32.GetBytes(strSrc);
            var nLen = 0;
            return PacketEncoder.DecodeBuf(bSrc, bSrc.Length, ref nLen);
        }

        public static byte[] DecodeBuffer(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            var nLen = 0;
            return PacketEncoder.DecodeBuf(buffer, buffer.Length, ref nLen);
        }

        public static byte[] DecodeBuffer(string src, int size)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            var bSrc = HUtil32.GetBytes(src);
            var nLen = 0;
            return PacketEncoder.DecodeBuf(bSrc, bSrc.Length, ref nLen);
        }
        
        public static T DecodeBuffer<T>(string src) where T : Packets, new()
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            var bSrc = HUtil32.GetBytes(src);
            var nLen = 0;
            var data = PacketEncoder.DecodeBuf(bSrc, bSrc.Length, ref nLen);
            return Packets.ToPacket<T>(data);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <returns></returns>
        public static string EncodeString(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var bSrc = HUtil32.GetBytes(str);
            var encBuf = new byte[bSrc.Length * 2];
            var destLen = PacketEncoder.EncodeBuf(bSrc, bSrc.Length, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }

        public static string EncodeBuffer<T>(T obj) where T : Packets, new()
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var result = string.Empty;
            var data = obj.GetBuffer();
            var buffSize = data.Length;
            if (buffSize <= 0) return result;
            if (buffSize < BufferSize)
            {
                var encBuf = new byte[buffSize * 2];
                var tempBuf = new byte[buffSize];
                Buffer.BlockCopy(data, 0, tempBuf, 0, buffSize);
                var destLen = PacketEncoder.EncodeBuf(tempBuf, buffSize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return result;
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        /// <returns></returns>
        public static byte[] EncodeBuffer(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffSize = data.Length;
            if (buffSize >= BufferSize) return Array.Empty<byte>();
            var encBuf = new byte[buffSize * 2];
            var destLen = PacketEncoder.EncodeBuf(data, buffSize, encBuf);
            return encBuf[..destLen];
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        public static string EncodeBuffer(byte[] data, int bufsize)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (bufsize < BufferSize)
            {
                var tempBuf = new byte[data.Length];
                var encBuf = new byte[tempBuf.Length * 2];
                Buffer.BlockCopy(data, 0, tempBuf, 0, bufsize);
                var destLen = PacketEncoder.EncodeBuf(tempBuf, bufsize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return string.Empty;
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static int EncodeMessage(byte[] msgBuf, ref byte[] encBuff)
        {
            if (msgBuf == null) throw new ArgumentNullException(nameof(msgBuf));
            return PacketEncoder.EncodeBuf(msgBuf, 12, encBuff);
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static string EncodeMessage(ClientMesaagePacket packet)
        {
            if (packet == null) throw new ArgumentNullException(nameof(packet));
            var packetData = packet.GetBuffer();
            if (packetData.Length <= 0)
            {
                return string.Empty;
            }
            var encBuf = new byte[packetData.Length * 2];
            var destLen = PacketEncoder.EncodeBuf(packetData, 12, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }
    }
}