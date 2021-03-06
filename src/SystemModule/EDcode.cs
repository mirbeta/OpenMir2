using System;

namespace SystemModule
{
    public class EDcode
    {
        private const int BUFFERSIZE = 10000;

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static ClientPacket DecodePacket(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = 0;
            var encBuf = Misc.DecodeBuf(tempBuf, str.Length, ref buffLen);
            return Packets.ToPacket<ClientPacket>(encBuf);
        }

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static ClientPacket DecodePacket(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffLen = 0;
            var encBuf = Misc.DecodeBuf(data, data.Length, ref buffLen);
            return Packets.ToPacket<ClientPacket>(encBuf);
        }

        public static byte[] DecodeBuff(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffLen = 0;
            return Misc.DecodeBuf(data, data.Length, ref buffLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string DeCodeString(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var nLen = 0;
            var bSrc = HUtil32.GetBytes(str);
            var encBuf = Misc.DecodeBuf(bSrc, bSrc.Length, ref nLen);
            return HUtil32.GetString(encBuf, 0, nLen);
        }

        public static byte[] DecodeBuffer(string strSrc)
        {
            if (strSrc == null) throw new ArgumentNullException(nameof(strSrc));
            var bSrc = HUtil32.GetBytes(strSrc);
            var nLen = 0;
            return Misc.DecodeBuf(bSrc, bSrc.Length, ref nLen);
        }

        public static byte[] DecodeBuffer(string src, int size)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            var bSrc = HUtil32.GetBytes(src);
            var nLen = 0;
            return Misc.DecodeBuf(bSrc, bSrc.Length, ref nLen);
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
            var destLen = Misc.EncodeBuf(bSrc, bSrc.Length, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }

        public static string EncodeBuffer<T>(T obj) where T : Packets, new()
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var result = string.Empty;
            var data = obj.GetBuffer();
            var buffSize = data.Length;
            if (buffSize <= 0) return result;
            if (buffSize < BUFFERSIZE)
            {
                var encBuf = new byte[buffSize * 2];
                var tempBuf = new byte[buffSize];
                Buffer.BlockCopy(data, 0, tempBuf, 0, buffSize);
                var destLen = Misc.EncodeBuf(tempBuf, buffSize, encBuf);
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
            if (buffSize >= BUFFERSIZE) return Array.Empty<byte>();
            var encBuf = new byte[buffSize * 2];
            var destLen = Misc.EncodeBuf(data, buffSize, encBuf);
            return encBuf[..destLen];
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        public static string EncodeBuffer(byte[] data, int bufsize)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var tempBuf = new byte[data.Length];
            var encBuf = new byte[tempBuf.Length * 2];
            if (bufsize < BUFFERSIZE)
            {
                Buffer.BlockCopy(data, 0, tempBuf, 0, bufsize);
                var destLen = Misc.EncodeBuf(tempBuf, bufsize, encBuf);
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
            return Misc.EncodeBuf(msgBuf, 12, encBuff);
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static string EncodeMessage(ClientPacket packet)
        {
            if (packet == null) throw new ArgumentNullException(nameof(packet));
            var packetData = packet.GetBuffer();
            var encBuf = new byte[packetData.Length * 2];
            var destLen = Misc.EncodeBuf(packetData, 12, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }
    }
}