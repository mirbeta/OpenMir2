using System;

namespace SystemModule
{
    /// <summary>
    /// 游戏数据包加解密工具类
    /// </summary>
    public static class EncryptUtil
    {
        private const byte BySeed = 0xAC;
        private const byte ByBase = 0x3C;

        public static string EncodeString(string str)
        {
            var encBuf = new byte[4096];
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = Encode(tempBuf, str.Length, encBuf);
            return HUtil32.GetString(encBuf, 0, buffLen);
        }

        public static string DecodeString(string str)
        {
            var tempBuf = HUtil32.GetBytes(str);
            var buffLen = 0;
            var encBuf = Decode(tempBuf, str.Length, ref buffLen);
            return HUtil32.GetString(encBuf, 0, buffLen);
        }

        /// <summary>
        /// 加密
        /// </summary>
        public static int Encode(byte[] srcBuf, int len, byte[] dstBuf, int dstOffset = 0)
        {
            var no = 2;
            byte remainder = 0;
            var pos = 0;
            var dstPos = dstOffset;
            for (var i = 0; i < len; i++)
            {
                var c = (byte)(srcBuf[pos] ^ BySeed);
                pos++;
                if (no == 6)
                {
                    dstBuf[dstPos] = (byte)((c & 0x3F) + ByBase);
                    dstPos++;
                    remainder = (byte)(remainder | ((c >> 2) & 0x30));
                    dstBuf[dstPos] = (byte)(remainder + ByBase);
                    dstPos++;
                    remainder = 0;
                }
                else
                {
                    var temp = (byte)(c >> 2);
                    dstBuf[dstPos] = (byte)(((temp & 0x3C) | (c & 0x3)) + ByBase);
                    dstPos++;
                    remainder = (byte)((remainder << 2) | (temp & 0x3));
                }
                no = no % 6 + 2;
            }
            if (no != 2)
            {
                dstBuf[dstPos] = (byte)(remainder + ByBase);
                dstPos++;
            }
            return dstPos - dstOffset;
        }
        
        /// <summary>
        /// 加密
        /// </summary>
        public static int Encode(Span<byte> srcBuf, int len, Span<byte> dstBuf, int dstOffset = 0)
        {
            var no = 2;
            byte remainder = 0;
            var pos = 0;
            var dstPos = dstOffset;
            for (var i = 0; i < len; i++)
            {
                var c = (byte)(srcBuf[pos] ^ BySeed);
                pos++;
                if (no == 6)
                {
                    dstBuf[dstPos] = (byte)((c & 0x3F) + ByBase);
                    dstPos++;
                    remainder = (byte)(remainder | ((c >> 2) & 0x30));
                    dstBuf[dstPos] = (byte)(remainder + ByBase);
                    dstPos++;
                    remainder = 0;
                }
                else
                {
                    var temp = (byte)(c >> 2);
                    dstBuf[dstPos] = (byte)(((temp & 0x3C) | (c & 0x3)) + ByBase);
                    dstPos++;
                    remainder = (byte)((remainder << 2) | (temp & 0x3));
                }
                no = no % 6 + 2;
            }
            if (no != 2)
            {
                dstBuf[dstPos] = (byte)(remainder + ByBase);
                dstPos++;
            }
            return dstPos - dstOffset;
        }

        public static Span<byte> DecodeSpan(Span<byte> srcBuf, int len, ref int decodeLen)
        {
            return Decode(srcBuf, len, ref decodeLen);
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static byte[] Decode(byte[] srcBuf, int len, ref int decodeLen)
        {
            byte temp;
            byte remainder;
            byte c;
            var cycles = len / 4;
            var bytesLeft = len % 4;
            var dstPos = 0;
            decodeLen = GetDecodeLen(cycles, bytesLeft);
            var dstBuffer = new byte[decodeLen];
            for (var i = 0; i < cycles; i++)
            {
                var curCycleBegin = i * 4;
                remainder = (byte)((srcBuf[curCycleBegin + 3]) - ByBase);
                temp = (byte)(srcBuf[curCycleBegin] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | (remainder & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
                temp = (byte)((srcBuf[curCycleBegin + 1]) - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
                temp = (byte)(srcBuf[curCycleBegin + 2] - ByBase);
                c = (byte)(temp | ((remainder << 2) & 0xC0));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
            }
            if (bytesLeft == 2)
            {
                remainder = (byte)(srcBuf[len - 1] - ByBase);
                temp = (byte)(srcBuf[len - 2] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
            }
            else if (bytesLeft == 3)
            {
                remainder = (byte)(srcBuf[len - 1] - ByBase);
                temp = (byte)(srcBuf[len - 3] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | (remainder & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
                temp = (byte)(srcBuf[len - 2] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
            }
            return dstBuffer;
        }
        
        /// <summary>
        /// 解密
        /// </summary>
        public static byte[] Decode(Span<byte> srcBuf, int len, ref int decodeLen)
        {
            byte temp;
            byte remainder;
            byte c;
            var nCycles = len / 4;
            var nBytesLeft = len % 4;
            var dstPos = 0;
            decodeLen = GetDecodeLen(nCycles, nBytesLeft);
            var dstBuffer = new byte[decodeLen];
            for (var i = 0; i < nCycles; i++)
            {
                var curCycleBegin = i * 4;
                remainder = (byte)((srcBuf[curCycleBegin + 3]) - ByBase);
                temp = (byte)(srcBuf[curCycleBegin] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | (remainder & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
                temp = (byte)((srcBuf[curCycleBegin + 1]) - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
                temp = (byte)(srcBuf[curCycleBegin + 2] - ByBase);
                c = (byte)(temp | ((remainder << 2) & 0xC0));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
            }
            if (nBytesLeft == 2)
            {
                remainder = (byte)(srcBuf[len - 1] - ByBase);
                temp = (byte)(srcBuf[len - 2] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
            }
            else if (nBytesLeft == 3)
            {
                remainder = (byte)(srcBuf[len - 1] - ByBase);
                temp = (byte)(srcBuf[len - 3] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | (remainder & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
                dstPos++;
                temp = (byte)(srcBuf[len - 2] - ByBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                dstBuffer[dstPos] = (byte)(c ^ BySeed);
            }
            return dstBuffer;
        }

        private static int GetDecodeLen(int cycles, int bytesLeft)
        {
            var dstPos = cycles * 3;
            switch (bytesLeft)
            {
                case 2:
                    dstPos++;
                    break;
                case 3:
                    dstPos += 2;
                    break;
            }
            return dstPos;
        }

    }
}