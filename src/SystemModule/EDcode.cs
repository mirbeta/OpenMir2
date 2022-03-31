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
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = 0;
            var encBuf = Misc.DecodeBuf(tempBuf, str.Length, ref buffLen);
            return new ClientPacket(encBuf, (byte)buffLen);
        }

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static ClientPacket DecodePacket(byte[] data)
        {
            var buffLen = 0;
            var encBuf = Misc.DecodeBuf(data, data.Length, ref buffLen);
            return new ClientPacket(encBuf, (byte)buffLen);
        }

        public static byte[] DecodeBuff(byte[] data)
        {
            var buffLen = 0;
            return Misc.DecodeBuf(data, data.Length, ref buffLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string DeCodeString(string str)
        {
            var nLen = 0;
            var bSrc = HUtil32.GetBytes(str);
            var encBuf = Misc.DecodeBuf(bSrc, bSrc.Length, ref nLen);
            return HUtil32.GetString(encBuf, 0, nLen);
        }

        public static byte[] DecodeBuffer(string strSrc)
        {
            var bSrc = HUtil32.GetBytes(strSrc);
            var nLen = 0;
            return Misc.DecodeBuf(bSrc, bSrc.Length, ref nLen);
        }

        public static byte[] DecodeBuffer(string Src, int size)
        {
            var bSrc = HUtil32.GetBytes(Src);
            var nLen = 0;
            return Misc.DecodeBuf(bSrc, bSrc.Length, ref nLen);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <returns></returns>
        public static string EncodeString(string str)
        {
            var bSrc = HUtil32.GetBytes(str);
            var encBuf = new byte[bSrc.Length * 2];
            var destLen = Misc.EncodeBuf(bSrc, bSrc.Length, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }

        public static string EncodeBuffer<T>(T obj) where T : Packets, new()
        {
            var result = string.Empty;
            var data = obj.GetPacket();
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
            var buffSize = data.Length;
            if (buffSize >= BUFFERSIZE) return Array.Empty<byte>();
            var encBuf = new byte[BUFFERSIZE];
            var destLen = Misc.EncodeBuf(data, buffSize, encBuf);
            return encBuf[..destLen];
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        public static string EncodeBuffer(byte[] data, int bufsize)
        {
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
            return Misc.EncodeBuf(msgBuf, 12, encBuff);
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static string EncodeMessage(ClientPacket packet)
        {
            var packetData = packet.GetPacket();
            var encBuf = new byte[packetData.Length * 2];
            var destLen = Misc.EncodeBuf(packetData, 12, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }
    }
}