using System;
using System.IO;
namespace M2Server
{
    public struct MD5Context
    {
        public double[] State;
        public double[] Count;
        public byte[] Buffer;
    } // end MD5Context

    public struct __Long
    {
        public short LoWord;
        public short HiWord;
    } // end __Long

}

namespace M2Server
{
    public class MD5Unit
    {
        public static byte[] PADDING = {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
        public static long[] CRCTable = new long[512 + 1];
        public const uint CRCPOLY = 0xEDB88320;
        public static double F(double x, double y, double z)
        {
            double result;
            result = (x & y) | (~x & z);
            return result;
        }

        public static double G(double x, double y, double z)
        {
            double result;
            result = (x & z) | (y & ~z);
            return result;
        }

        public static double H(double x, double y, double z)
        {
            double result;
            result = x ^ y ^ z;
            return result;
        }

        public static double I(double x, double y, double z)
        {
            double result;
            result = y ^ (x | ~z);
            return result;
        }

        public static void rot(ref double x, byte n)
        {
            x = (x << n) | (x >> (32 - n));
        }

        public static void FF(ref double a, double b, double c, double d, double x, byte s, double ac)
        {
            a += F(b, c, d) + x + ac;
            rot(ref a, s);
            a += b;
        }

        public static void GG(ref double a, double b, double c, double d, double x, byte s, double ac)
        {
            a += G(b, c, d) + x + ac;
            rot(ref a, s);
            a += b;
        }

        public static void HH(ref double a, double b, double c, double d, double x, byte s, double ac)
        {
            a += H(b, c, d) + x + ac;
            rot(ref a, s);
            a += b;
        }

        public static void II(ref double a, double b, double c, double d, double x, byte s, double ac)
        {
            a += I(b, c, d) + x + ac;
            rot(ref a, s);
            a += b;
        }

        public static void Encode(object Source, object Target, long Count)
        {
            byte S;
            double T;
            long I;
            S = Source;
            T = Target;
            for (I = 1; I <= Count / 4; I ++ )
            {
                T = S;
                S ++;
                T = T | (S << 8);
                S ++;
                T = T | (S << 16);
                S ++;
                T = T | (S << 24);
                S ++;
                T ++;
            }
        }

        public static void Decode(object Source, object Target, long Count)
        {
            double S;
            byte T;
            long I;
            S = Source;
            T = Target;
            for (I = 1; I <= Count; I ++ )
            {
                T = S & 0xff;
                T ++;
                T = (S >> 8) & 0xff;
                T ++;
                T = (S >> 16) & 0xff;
                T ++;
                T = (S >> 24) & 0xff;
                T ++;
                S ++;
            }
        }

        public static void Transform(object Buffer, ref double[] State)
        {
            double a;
            double b;
            double c;
            double d;
            double[] Block;
            Encode(Buffer, Block, 64);
            a = State[0];
            b = State[1];
            c = State[2];
            d = State[3];
            FF(ref a, b, c, d, Block[0], 7, 0xd76aa478);
            FF(ref d, a, b, c, Block[1], 12, 0xe8c7b756);
            FF(ref c, d, a, b, Block[2], 17, 0x242070db);
            FF(ref b, c, d, a, Block[3], 22, 0xc1bdceee);
            FF(ref a, b, c, d, Block[4], 7, 0xf57c0faf);
            FF(ref d, a, b, c, Block[5], 12, 0x4787c62a);
            FF(ref c, d, a, b, Block[6], 17, 0xa8304613);
            FF(ref b, c, d, a, Block[7], 22, 0xfd469501);
            FF(ref a, b, c, d, Block[8], 7, 0x698098d8);
            FF(ref d, a, b, c, Block[9], 12, 0x8b44f7af);
            FF(ref c, d, a, b, Block[10], 17, 0xffff5bb1);
            FF(ref b, c, d, a, Block[11], 22, 0x895cd7be);
            FF(ref a, b, c, d, Block[12], 7, 0x6b901122);
            FF(ref d, a, b, c, Block[13], 12, 0xfd987193);
            FF(ref c, d, a, b, Block[14], 17, 0xa679438e);
            FF(ref b, c, d, a, Block[15], 22, 0x49b40821);
            GG(ref a, b, c, d, Block[1], 5, 0xf61e2562);
            GG(ref d, a, b, c, Block[6], 9, 0xc040b340);
            GG(ref c, d, a, b, Block[11], 14, 0x265e5a51);
            GG(ref b, c, d, a, Block[0], 20, 0xe9b6c7aa);
            GG(ref a, b, c, d, Block[5], 5, 0xd62f105d);
            GG(ref d, a, b, c, Block[10], 9, 0x2441453);
            GG(ref c, d, a, b, Block[15], 14, 0xd8a1e681);
            GG(ref b, c, d, a, Block[4], 20, 0xe7d3fbc8);
            GG(ref a, b, c, d, Block[9], 5, 0x21e1cde6);
            GG(ref d, a, b, c, Block[14], 9, 0xc33707d6);
            GG(ref c, d, a, b, Block[3], 14, 0xf4d50d87);
            GG(ref b, c, d, a, Block[8], 20, 0x455a14ed);
            GG(ref a, b, c, d, Block[13], 5, 0xa9e3e905);
            GG(ref d, a, b, c, Block[2], 9, 0xfcefa3f8);
            GG(ref c, d, a, b, Block[7], 14, 0x676f02d9);
            GG(ref b, c, d, a, Block[12], 20, 0x8d2a4c8a);
            HH(ref a, b, c, d, Block[5], 4, 0xfffa3942);
            HH(ref d, a, b, c, Block[8], 11, 0x8771f681);
            HH(ref c, d, a, b, Block[11], 16, 0x6d9d6122);
            HH(ref b, c, d, a, Block[14], 23, 0xfde5380c);
            HH(ref a, b, c, d, Block[1], 4, 0xa4beea44);
            HH(ref d, a, b, c, Block[4], 11, 0x4bdecfa9);
            HH(ref c, d, a, b, Block[7], 16, 0xf6bb4b60);
            HH(ref b, c, d, a, Block[10], 23, 0xbebfbc70);
            HH(ref a, b, c, d, Block[13], 4, 0x289b7ec6);
            HH(ref d, a, b, c, Block[0], 11, 0xeaa127fa);
            HH(ref c, d, a, b, Block[3], 16, 0xd4ef3085);
            HH(ref b, c, d, a, Block[6], 23, 0x4881d05);
            HH(ref a, b, c, d, Block[9], 4, 0xd9d4d039);
            HH(ref d, a, b, c, Block[12], 11, 0xe6db99e5);
            HH(ref c, d, a, b, Block[15], 16, 0x1fa27cf8);
            HH(ref b, c, d, a, Block[2], 23, 0xc4ac5665);
            II(ref a, b, c, d, Block[0], 6, 0xf4292244);
            II(ref d, a, b, c, Block[7], 10, 0x432aff97);
            II(ref c, d, a, b, Block[14], 15, 0xab9423a7);
            II(ref b, c, d, a, Block[5], 21, 0xfc93a039);
            II(ref a, b, c, d, Block[12], 6, 0x655b59c3);
            II(ref d, a, b, c, Block[3], 10, 0x8f0ccc92);
            II(ref c, d, a, b, Block[10], 15, 0xffeff47d);
            II(ref b, c, d, a, Block[1], 21, 0x85845dd1);
            II(ref a, b, c, d, Block[8], 6, 0x6fa87e4f);
            II(ref d, a, b, c, Block[15], 10, 0xfe2ce6e0);
            II(ref c, d, a, b, Block[6], 15, 0xa3014314);
            II(ref b, c, d, a, Block[13], 21, 0x4e0811a1);
            II(ref a, b, c, d, Block[4], 6, 0xf7537e82);
            II(ref d, a, b, c, Block[11], 10, 0xbd3af235);
            II(ref c, d, a, b, Block[2], 15, 0x2ad7d2bb);
            II(ref b, c, d, a, Block[9], 21, 0xeb86d391);
            State[0] += a;
            State[1] += b;
            State[2] += c;
            State[3] += d;
        }

        public static void MD5Init(ref MD5Context Context)
        {
            Context.State[0] = 0x67452301;
            Context.State[1] = 0xefcdab89;
            Context.State[2] = 0x98badcfe;
            Context.State[3] = 0x10325476;
            Context.Count[0] = 0;
            Context.Count[1] = 0;
           
            Context.ZeroMemory(Context.Buffer, sizeof(byte));
        }

        public static void MD5Update(ref MD5Context Context, string Input, long Length)
        {
            long Index;
            long PartLen;
            long I;
            Index = (Context.Count[0] >> 3) & 0x3f;
            Context.Count[0] += Length << 3;
            if (Context.Count[0] < (Length << 3))
            {
                Context.Count[1] ++;
            }
            Context.Count[1] += Length >> 29;
            PartLen = 64 - Index;
            if (Length >= PartLen)
            {
                
                CopyMemory(Context.Buffer[Index], Input, PartLen);
                Transform(Context.Buffer, ref Context.State);
                I = PartLen;
                while (I + 63 < Length)
                {
                    Transform(Input[I], ref Context.State);
                    I += 64;
                }
                Index = 0;
            }
            else
            {
                I = 0;
            }
            
            CopyMemory(Context.Buffer[Index], Input[I], Length - I);
        }

        public static void MD5Result(ref MD5Context Context, ref byte[] Digest)
        {
            byte[] Bits;
            long Index;
            long PadLen;
            Decode(Context.Count, Bits, 2);
            Index = (Context.Count[0] >> 3) & 0x3f;
            if (Index < 56)
            {
                PadLen = 56 - Index;
            }
            else
            {
                PadLen = 120 - Index;
            }
            MD5Update(ref Context, PADDING, PadLen);
            MD5Update(ref Context, Bits, 8);
            Decode(Context.State, Digest, 4);
            
            ZeroMemory(Context, sizeof(MD5Context));
        }

        public static string GetMD5Text(string Input)
        {
            string result;
            MD5Context Context;
            byte I;
            byte[] DigestResult;
            char[] Digits = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
            result = "";
            MD5Init(ref Context);
            MD5Update(ref Context, (Input as string), Input.Length);
            MD5Result(ref Context, ref DigestResult);
            for (I = 0; I <= 15; I ++ )
            {
                result = result + Digits[(DigestResult[I] >> 4) & 0x0f] + Digits[DigestResult[I] & 0x0f];
            }
            return result;
        }

        // 
        // CRC - Damian
        public static void BuildCRCTable()
        {
            short i;
            short j;
            long r;
            //@ Unsupported function or procedure: 'FillChar'
            FillChar(CRCTable, sizeof(CRCTable), 0);
            for (i = 0; i <= 255; i ++ )
            {
                r = i << 1;
                for (j = 8; j >= 0; j-- )
                {
                    if ((r & 1) != 0)
                    {
                        r = (r >> 1) ^ CRCPOLY;
                    }
                    else
                    {
                        r = r >> 1;
                    }
                }
                CRCTable[i] = r;
            }
        }

        public static long RecountCRC(byte b, long CrcOld)
        {
            long result;
            result = CRCTable[((byte)CrcOld ^ (long)b)] ^ ((CrcOld >> 8) & 0x00FFFFFF);
            return result;
        }
    }
}

