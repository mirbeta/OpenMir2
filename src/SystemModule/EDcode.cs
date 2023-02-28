using System;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public class EDCode
    {
        private const int BufferSize = 10000;
        
        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static CommandMessage DecodePacket(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = 0;
            var encBuf = EncryptUtil.Decode(tempBuf, str.Length, ref buffLen);
            return SerializerUtil.Deserialize<CommandMessage>(encBuf);
        }

        /// <summary>
        /// 解码客户端封包
        /// </summary>
        public static CommandMessage DecodePacket(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffLen = 0;
            var encBuf = EncryptUtil.Decode(data, data.Length, ref buffLen);
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
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffLen = 0;
            return EncryptUtil.Decode(data, data.Length, ref buffLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string DeCodeString(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var nLen = 0;
            var bSrc = HUtil32.GetBytes(str);
            var encBuf = EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
            return HUtil32.GetString(encBuf, 0, nLen);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DeCodeString(Span<byte> str)
        {
            if (str.IsEmpty) throw new ArgumentNullException(nameof(str));
            var nLen = 0;
            var encBuf = EncryptUtil.Decode(str, str.Length, ref nLen);
            return HUtil32.GetString(encBuf);
        }

        public static byte[] DecodeBuffer(string strSrc)
        {
            if (strSrc == null) throw new ArgumentNullException(nameof(strSrc));
            var bSrc = HUtil32.GetBytes(strSrc);
            var nLen = 0;
            return EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
        }

        public static Span<byte> DecodeBuffer(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            var nLen = 0;
            return EncryptUtil.Decode(buffer, buffer.Length, ref nLen);
        }

        public static Span<byte> DecodeBuffer(string src, int size)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            var bSrc = HUtil32.GetBytes(src);
            var nLen = 0;
            return EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
        }

        public static T DecodeBuffer<T>(string src) where T : new()
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            var bSrc = HUtil32.GetBytes(src);
            var nLen = 0;
            var data = EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
            return SerializerUtil.Deserialize<T>(data);
        }
        
        public static T DecodeClientBuffer<T>(string src) where T : ClientPacket, new()
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            var bSrc = HUtil32.GetBytes(src);
            var nLen = 0;
            var data = EncryptUtil.Decode(bSrc, bSrc.Length, ref nLen);
            return ClientPacket.ToPacket<T>(data);
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
            var destLen = EncryptUtil.Encode(bSrc, bSrc.Length, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }

        public static string EncodeBuffer<T>(T obj) where T : ClientPacket, new()
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var result = string.Empty;
            var data = obj.GetBuffer();
            var buffSize = data.Length;
            if (buffSize <= 0) return result;
            if (buffSize < BufferSize)
            {
                var encBuf = new byte[buffSize * 2];
                var destLen = EncryptUtil.Encode(data, buffSize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return result;
        }

        public static string EncodePacket<T>(T packet) where T : struct
        {
            var data = SerializerUtil.Serialize(packet);
            var buffSize = data.Length;
            if (buffSize <= 0) return string.Empty;
            if (buffSize < BufferSize)
            {
                var encBuf = new byte[buffSize * 2];
                var destLen = EncryptUtil.Encode(data, buffSize, encBuf);
                return HUtil32.GetString(encBuf, 0, destLen);
            }
            return string.Empty;
        }

         public static string EncodePacket(byte[] data)
         {
             if (data == null) throw new ArgumentNullException(nameof(data));
             var buffSize = data.Length;
             if (buffSize <= 0) return string.Empty;
             if (buffSize < BufferSize)
             {
                 var encBuf = new byte[buffSize * 2];
                 var destLen = EncryptUtil.Encode(data, buffSize, encBuf);
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
            if (data == null) throw new ArgumentNullException(nameof(data));
            var buffSize = data.Length;
            if (buffSize >= BufferSize) return Array.Empty<byte>();
            var encBuf = new byte[buffSize * 2];
            var destLen = EncryptUtil.Encode(data, buffSize, encBuf);
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
                var encBuf = new byte[bufsize * 2];
                var destLen = EncryptUtil.Encode(data, bufsize, encBuf);
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
            return EncryptUtil.Encode(msgBuf, 12, encBuff);
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static string EncodeMessage(CommandMessage packet)
        {
            var packetData = SerializerUtil.Serialize(packet);
            if (packetData.Length <= 0)
            {
                return string.Empty;
            }
            var encBuf = new byte[packetData.Length * 2];
            var destLen = EncryptUtil.Encode(packetData, 12, encBuf);
            return HUtil32.GetString(encBuf, 0, destLen);
        }
    }
}