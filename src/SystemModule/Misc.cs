using System;
using SystemModule;

namespace SystemModule
{
    public class Misc
    {
        public static string EncodeString(string Str)
        {
            byte[] EncBuf = new byte[4096];
            var tempBuf = HUtil32.GetBytes(Str);
            var buffLen = EncodeBuf(tempBuf, Str.Length, EncBuf);
            string result = HUtil32.GetString(EncBuf, 0, buffLen);
            return result;
        }

        public static string DecodeString(string Str)
        {
            byte[] EncBuf = new byte[4096];
            var tempBuf = HUtil32.GetBytes(Str);
            var buffLen = DecodeBuf(tempBuf, Str.Length, EncBuf);
            string result = HUtil32.GetString(EncBuf, 0, buffLen);
            return result;
        }

        public static int EncodeBuf(byte[] Buf, int Len, byte[] DstBuf)
        {
            int result;
            byte temp;
            byte c;
            var no = 2;
            byte remainder = 0;
            byte bySeed = 0xAC;
            byte byBase = 0x3C;
            var pos = 0;
            var dstPos = 0;
            for (var i = 0; i < Len; i++)
            {
                c = (byte)(Buf[pos] ^ bySeed);
                pos++;
                if (no == 6)
                {
                    DstBuf[dstPos] = (byte)((c & 0x3F) + byBase);
                    dstPos++;
                    remainder = (byte)(remainder | ((c >> 2) & 0x30));
                    DstBuf[dstPos] = (byte)(remainder + byBase);
                    dstPos++;
                    remainder = 0;
                }
                else
                {
                    temp = (byte)(c >> 2);
                    DstBuf[dstPos] = (byte)(((temp & 0x3C) | (c & 0x3)) + byBase);
                    dstPos++;
                    remainder = (byte)((remainder << 2) | (temp & 0x3));
                }
                no = no % 6 + 2;
            }
            if (no != 2)
            {
                DstBuf[dstPos]= (byte)(remainder + byBase);
                dstPos++;
            }
            result = dstPos;
            DstBuf[dstPos + 1] = (byte)('\0');
            return result;
        }

        public static int DecodeBuf(byte[] Buf, int Len, byte[] DstBuf)
        {
            int result = 0;
            int CurCycleBegin;
            byte temp;
            byte remainder;
            byte c;
            var nCycles = Len / 4;
            var nBytesLeft = Len % 4;
            byte bySeed = 0xAC;
            byte byBase = 0x3C;
            var dstPos = 0;
            for (var i = 0; i < nCycles; i++)
            {
                CurCycleBegin = i * 4;
                remainder = (byte)((Buf[CurCycleBegin + 3]) - byBase);
                temp = (byte)(Buf[CurCycleBegin] - byBase);
                c = (byte)(((temp << 2) & 0xF0) | (remainder & 0x0C) | (temp & 0x3));
                DstBuf[dstPos] = (byte)(c ^ bySeed);
                dstPos++;
                temp = (byte)((Buf[CurCycleBegin + 1]) - byBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                DstBuf[dstPos] = (byte)(c ^ bySeed);
                dstPos++;
                temp = (byte)(Buf[CurCycleBegin + 2] - byBase);
                c =(byte)( temp | ((remainder << 2) & 0xC0));
                DstBuf[dstPos] = (byte)(c ^ bySeed);
                dstPos++;
            }
            if (nBytesLeft == 2)
            {
                remainder = (byte)(Buf[Len - 1] - byBase);
                temp = (byte)(Buf[Len - 2] - byBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                DstBuf[dstPos] = (byte)(c ^ bySeed);
                dstPos++;
            }
            else if (nBytesLeft == 3)
            {
                remainder = (byte)(Buf[Len - 1] - byBase);
                temp =(byte) (Buf[Len - 3] - byBase);
                c = (byte)(((temp << 2) & 0xF0) | (remainder & 0x0C) | (temp & 0x3));
                DstBuf[dstPos] = (byte)(c ^ bySeed);
                dstPos++;
                temp = (byte)(Buf[Len - 2] - byBase);
                c = (byte)(((temp << 2) & 0xF0) | ((remainder << 2) & 0x0C) | (temp & 0x3));
                DstBuf[dstPos] = (byte)(c ^ bySeed);
                dstPos++;
            }
            result = dstPos;
            DstBuf[dstPos + 1] = (byte)'\0';
            return result;
        }
    } 
}

