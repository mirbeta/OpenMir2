using System;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public static class EDCode
    {
        private const int BufferSize = 10000;

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static CommandMessage DecodePacket(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            byte[] tempBuf = HUtil32.GetBytes(str);
            int buffLen = 0;
            byte[] encBuf = EncryptUtil.Decode(tempBuf, str.Length, ref buffLen);
            return SerializerUtil.Deserialize<CommandMessage>(encBuf);
        }

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static CommandMessage DecodePacket(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            int buffLen = 0;
            byte[] encBuf = EncryptUtil.Decode(data, data.Length, ref buffLen);
            return SerializerUtil.Deserialize<CommandMessage>(encBuf);
        }

        /// <summary>
        /// 解密Buffer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] DecodeBuff(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            int buffLen = 0;
            return EncryptUtil.Decode(data, data.Length, ref buffLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string DeCodeString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            int nLen = 0;
            byte[] bSrc = HUtil32.GetBytes(str);
            byte[] encBuf = EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
            return HUtil32.GetString(encBuf, 0, nLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DeCodeString(ReadOnlySpan<char> str)
        {
            if (str.IsEmpty)
            {
                throw new ArgumentNullException(nameof(str));
            }

            int nLen = 0;
            byte[] encBuf = EncryptUtil.Decode(str, str.Length, ref nLen);
            return HUtil32.GetString(encBuf);
        }
        
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DeCodeString(ReadOnlySpan<byte> str)
        {
            if (str.IsEmpty)
            {
                throw new ArgumentNullException(nameof(str));
            }

            int nLen = 0;
            byte[] encBuf = EncryptUtil.Decode(str, str.Length, ref nLen);
            return HUtil32.GetString(encBuf);
        }

        public static byte[] DecodeBuffer(string strSrc)
        {
            if (strSrc == null)
            {
                throw new ArgumentNullException(nameof(strSrc));
            }

            byte[] bSrc = HUtil32.GetBytes(strSrc);
            int nLen = 0;
            return EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
        }

        public static Span<byte> DecodeBuffer(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            int nLen = 0;
            return EncryptUtil.Decode(buffer, buffer.Length, ref nLen);
        }

        public static Span<byte> DecodeBuffer(string src, int size)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            byte[] bSrc = HUtil32.GetBytes(src);
            int nLen = 0;
            return EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
        }

        public static T DecodeBuffer<T>(string src) where T : new()
        {
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentNullException(nameof(src));
            }

            byte[] bSrc = HUtil32.GetBytes(src);
            int nLen = 0;
            byte[] data = EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
            return SerializerUtil.Deserialize<T>(data);
        }

        public static T DecodeClientBuffer<T>(string src) where T : ClientPacket, new()
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            byte[] bSrc = HUtil32.GetBytes(src);
            int nLen = 0;
            byte[] data = EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
            return ClientPacket.ToPacket<T>(data);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <returns></returns>
        public static string EncodeString(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            byte[] bSrc = HUtil32.GetBytes(str);
            Span<byte> encBuf = stackalloc byte[bSrc.Length * 2];
            int destLen = EncryptUtil.Encode(bSrc, bSrc.Length, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }

        public static string EncodeBuffer<T>(T obj) where T : ClientPacket, new()
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            string result = string.Empty;
            byte[] data = obj.GetBuffer();
            int buffSize = data.Length;
            if (buffSize <= 0)
            {
                return result;
            }

            if (buffSize < BufferSize)
            {
                Span<byte> encBuf = stackalloc byte[buffSize * 2];
                int destLen = EncryptUtil.Encode(data, buffSize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return result;
        }

        public static string EncodeMessage<T>(T packet) where T : class
        {
            byte[] data = SerializerUtil.Serialize(packet);
            var newData = data[1..];
            int buffSize = data.Length;
            if (buffSize <= 0)
            {
                return string.Empty;
            }

            if (buffSize < BufferSize)
            {
                Span<byte> encBuf = stackalloc byte[buffSize * 2];
                int destLen = EncryptUtil.Encode(data, buffSize, encBuf, pos: 1);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return string.Empty;
        }

        public static string EncodePacket<T>(T packet) where T : struct
        {
            byte[] data = SerializerUtil.Serialize(packet);
            int buffSize = data.Length;
            if (buffSize <= 0)
            {
                return string.Empty;
            }

            if (buffSize < BufferSize)
            {
                Span<byte> encBuf = stackalloc byte[buffSize * 2];
                int destLen = EncryptUtil.Encode(data, buffSize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return string.Empty;
        }

        public static string EncodePacket(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            int buffSize = data.Length;
            if (buffSize <= 0)
            {
                return string.Empty;
            }

            if (buffSize < BufferSize)
            {
                Span<byte> encBuf = stackalloc byte[buffSize * 2];
                int destLen = EncryptUtil.Encode(data, buffSize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return string.Empty;
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        /// <returns></returns>
        public static byte[] EncodeBuffer(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            int buffSize = data.Length;
            if (buffSize >= BufferSize)
            {
                return Array.Empty<byte>();
            }
            Span<byte> encBuf = stackalloc byte[buffSize * 2];
            int destLen = EncryptUtil.Encode(data, buffSize, encBuf);
            return encBuf[..destLen].ToArray();
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        public static string EncodeBuffer(byte[] data, int buffSize)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (buffSize < BufferSize)
            {
                Span<byte> encBuf = stackalloc byte[buffSize * 2];
                int destLen = EncryptUtil.Encode(data, buffSize, encBuf);
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
            if (msgBuf == null)
            {
                throw new ArgumentNullException(nameof(msgBuf));
            }

            return EncryptUtil.Encode(msgBuf, 12, encBuff);
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static string EncodeMessage(CommandMessage packet)
        {
            byte[] packetData = SerializerUtil.Serialize(packet);
            if (packetData.Length <= 0)
            {
                return string.Empty;
            }
            Span<byte> encBuf = stackalloc byte[packetData.Length * 2];
            int destLen = EncryptUtil.Encode(packetData, 12, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }
    }
}