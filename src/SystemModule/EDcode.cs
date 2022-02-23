using System;
using System.Text;
using SystemModule.Packages;

namespace SystemModule
{
    public class EDcode
    {
        private const int BUFFERSIZE = 10000;
        
        private static int Encode6BitBuf(byte[] pSrc, byte[] pDest, int nSrcLen, int nDestLen)
        {
            var nRestCount = 0;
            byte btRest = 0;
            var nDestPos = 0;
            for (var i = 0; i < nSrcLen; i++)
            {
                if (nDestPos >= nDestLen) break;
                var btCh = pSrc[i];
                var btMade = Convert.ToByte((byte) (btRest | (btCh >> Convert.ToByte(2 + nRestCount))) & 0x3F);
                btRest = Convert.ToByte((byte) ((btCh << (8 - (2 + nRestCount))) >> 2) & 0x3F);
                nRestCount += 2;
                if (nRestCount < 6)
                {
                    pDest[nDestPos] = Convert.ToByte(btMade + 0x3C);
                    nDestPos++;
                }
                else
                {
                    if (nDestPos < nDestLen - 1)
                    {
                        pDest[nDestPos] = Convert.ToByte(btMade + 0x3C);
                        pDest[nDestPos + 1] = Convert.ToByte(btRest + 0x3C);
                        nDestPos += 2;
                    }
                    else
                    {
                        pDest[nDestPos] = Convert.ToByte(btMade + 0x3C);
                        nDestPos++;
                    }

                    nRestCount = 0;
                    btRest = 0;
                }
            }

            if (nRestCount > 0)
            {
                pDest[nDestPos] = Convert.ToByte(btRest + 0x3C);
                nDestPos++;
            }

            pDest[nDestPos] = 0;
            return nDestPos;
        }

        internal static int Decode6BitBuf(byte[] source, byte[] pbuf, int nSrcLen, int nBufLen)
        {
            byte[] masks = {0xFC, 0xF8, 0xF0, 0xE0, 0xC0};
            var nBitPos = 2;
            var nMadeBit = 0;
            var nBufPos = 0;
            byte btCh = 0;
            byte btTmp = 0;
            byte btByte = 0;
            for (var i = 0; i < nSrcLen; i++)
            {
                if (source[i] - 0x3C >= 0)
                {
                    btCh = Convert.ToByte(source[i] - 0x3C);
                }
                else
                {
                    nBufPos = 0;
                    break;
                }

                if (nBufPos >= nBufLen) break;
                if (nMadeBit + 6 >= 8)
                {
                    btByte = Convert.ToByte(btTmp | ((btCh & 0x3F) >> (6 - nBitPos)));
                    pbuf[nBufPos] = btByte;
                    nBufPos++;
                    nMadeBit = 0;
                    if (nBitPos < 6)
                    {
                        nBitPos += 2;
                    }
                    else
                    {
                        nBitPos = 2;
                        continue;
                    }
                }
                btTmp = Convert.ToByte((byte) (btCh << nBitPos) & masks[nBitPos - 2]);
                nMadeBit += 8 - nBitPos;
            }
            pbuf[nBufPos] = 0;
            return nBufPos;
        }

        public static TDefaultMessage DecodeMessage(string str)
        {
            var EncBuf = new byte[str.Length];
#if ISWOL
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = (byte)Misc.DecodeBuf(tempBuf, str.Length, ref EncBuf);
            return new TDefaultMessage(EncBuf, buffLen);
#else
            var bSrc = HUtil32.StringToByteAry(str);
            Decode6BitBuf(bSrc, EncBuf, bSrc.Length, BUFFERSIZE);
            return new TDefaultMessage(EncBuf);
#endif
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="chinese">是否返回中文</param>
        /// <returns></returns>
        public static unsafe string DeCodeString(string str, bool chinese = true)
        {
            var encBuf = new byte[BUFFERSIZE];
            var bSrc = HUtil32.StringToByteAry(str);
            var nLen = Misc.DecodeBuf(bSrc, bSrc.Length, ref encBuf);
            if (chinese)
            {
                return HUtil32.GetString(encBuf, 0, nLen);
            }
            else
            {
                fixed (byte* pb = encBuf)
                {
                    return HUtil32.SBytePtrToString((sbyte*)pb, 0, nLen);
                }
            }
        }

        public static byte[] DecodeBuffer(string Src)
        {
            var EncBuf = new byte[BUFFERSIZE];
            var bSrc = HUtil32.GetBytes(Src);
            Misc.DecodeBuf(bSrc, bSrc.Length, ref EncBuf);
            return EncBuf;
        }
        
        public static byte[] DecodeBuffer(string Src,int size)
        {
            var EncBuf = new byte[size];
            var bSrc = HUtil32.GetBytes(Src);
            Misc.DecodeBuf(bSrc, bSrc.Length, ref EncBuf);
            return EncBuf;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static unsafe string EncodeString(string str)
        {
            string result;
            var EncBuf = new byte[BUFFERSIZE];
            var bSrc = HUtil32.GetBytes(str);
            var DestLen = Misc.EncodeBuf(bSrc, bSrc.Length, EncBuf);
            fixed (byte* pb = EncBuf)
            {
                result = HUtil32.SBytePtrToString((sbyte*) pb, 0, DestLen);
            }
            return result;
        }

        public static unsafe string EncodeBuffer<T>(T obj)
        {
            var result = string.Empty;
            var type = typeof(T);
            var targetsMethord = type.GetMethod("GetPacket");
            if (targetsMethord == null) throw new Exception($"序列化{type.Name}失败");
            byte[] methordResult;
            if (targetsMethord.GetParameters().Length > 0)
            {
                methordResult = (byte[]) targetsMethord.Invoke(obj, new object[] { (byte)6 });
            }
            else
            {
                methordResult = (byte[]) targetsMethord.Invoke(obj, new object[] { });
            }
            var buffSize = methordResult.Length;
            if (buffSize > 0)
            {
                if (buffSize < BUFFERSIZE)
                {
                    var encBuf = new byte[BUFFERSIZE];
                    var tempBuf = new byte[BUFFERSIZE];
                    Array.Copy(methordResult, 0, tempBuf, 0, buffSize);
                    var destLen = Misc.EncodeBuf(tempBuf, buffSize, encBuf);
                    fixed (byte* pb = encBuf)
                    {
                        result = HUtil32.SBytePtrToString((sbyte*) pb, 0, destLen);
                    }
                }
                else
                {
                    result = string.Empty;
                }
                return result;
            }
            return result;
        }

        /// <summary>
        /// 加密Byte数组
        /// </summary>
        /// <param name="Buf"></param>
        /// <param name="bufsize"></param>
        /// <returns></returns>
        public static unsafe string EncodeBuffer(byte[] Buf, int bufsize)
        {
            string result;
            var EncBuf = new byte[BUFFERSIZE];
            var TempBuf = new byte[BUFFERSIZE];
            if (bufsize < BUFFERSIZE)
            {
                Array.Copy(Buf, 0, TempBuf, 0, bufsize);
                var destLen = Misc.EncodeBuf(TempBuf, bufsize, EncBuf);
                fixed (byte* pb = EncBuf)
                {
                    result = HUtil32.SBytePtrToString((sbyte*) pb, 0, destLen);
                }
            }
            else
            {
                result = "";
            }
            return result;
        }
        
        /// <summary>
        /// 加密消息
        /// </summary>
        /// <returns></returns>
        public static int EncodeMessage(byte[] msgBuf,ref byte[] encBuff)
        {
            return Misc.EncodeBuf(msgBuf, 12, encBuff);
        }
        
        /// <summary>
        /// 加密消息
        /// </summary>
        /// <param name="smsg"></param>
        /// <returns></returns>
        public static unsafe string EncodeMessage(TDefaultMessage smsg)
        {
            string result = string.Empty;
            byte[] EncBuf = new byte[1024];
            byte[] msgBuf = smsg.GetPacket(6);
            int DestLen = Misc.EncodeBuf(msgBuf, 12, EncBuf);
            fixed (byte* pb = EncBuf)
            {
                result = HUtil32.SBytePtrToString((sbyte*)pb, 0, DestLen);
            }
            return result;
        }
    }
}