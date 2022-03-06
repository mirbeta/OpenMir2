using System;
using System.Collections.Generic;

namespace SystemModule
{
    public class MD5
    {
        public static byte[] g_MD5EmptyDigest = { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };

        private static byte[] _padding = {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        private static uint F(uint x, uint y, uint z)
        {
            return  (x & y) | (~x & z);
        }

        private static uint g(uint x, uint y, uint z)
        {
            return  (x & z) | (y & ~z);
        }

        private static uint H(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }

        private static uint i(uint x, uint y, uint z)
        {
            return y ^ (x | ~z);
        }

        private static void Rot(ref uint x, byte n)
        {
            x = (x << n) | (x >> (32 - n));
        }

        private static void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint AC)
        {
            var calc = (x + AC);
            a += F(b, c, d) + calc;
            Rot(ref a, s);
            a += b;
        }

        private static void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint AC)
        {
            a += g(b, c, d) + x + AC;
            Rot(ref a, s);
            a += b;
        }

        private static void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint AC)
        {
            a += H(b, c, d) + x + AC;
            Rot(ref a, s);
            a += b;
        }

        private static void ii(ref uint a, uint b, uint c, uint d, uint x, byte s, uint AC)
        {
            a += i(b, c, d) + x + AC;
            Rot(ref a, s);
            a += b;
        }

        /// <summary>
        /// Encode Count bytes at Source into (Count / 4) DWORDs at Target
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        /// <param name="Count"></param>
        private static void Encode(byte[] Source, ref uint[] Target, int Count)
        {
            var s = Source;
            var len = Count / 4;
            var c = 0;
            for (var i = 0; i < len; i++)
            {
                var t = Source[c];
                Target[i] = ((uint)(t | (s[c + 1] << 8)));
                Target[i] = (uint)(Target[i] | (s[c + 2] << 16));
                Target[i] = (uint)(Target[i] | (s[c + 3] << 24));
                c += 4;
            }
        }

        /// <summary>
        /// Decode Count DWORDs at Source into (Count * 4) Bytes at Target
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        /// <param name="Count"></param>
        private static void Decode(uint[] Source, ref byte[] Target, int Count)
        {
            var list = new List<byte>(Count);
            for (var i = 0; i < Count; i++)
            {
                list.Add((byte)(Source[i] & 0xFF));
                list.Add((byte)((Source[i] >> 8) & 0xFF));
                list.Add((byte)((Source[i] >> 16) & 0xFF));
                list.Add((byte)((Source[i] >> 24) & 0xFF));
            }
            Target = list.ToArray();
        }

        private static void Transform(byte[] Buffer, ref uint[] State)
        {
            var Block = new uint[16];
            Encode(Buffer, ref Block, 64);
            var a = State[0];
            var b = State[1];
            var c = State[2];
            var d = State[3];
            FF(ref a, b, c, d, Block[0], 7, 0xD76AA478);
            FF(ref d, a, b, c, Block[1], 12, 0xE8C7B756);
            FF(ref c, d, a, b, Block[2], 17, 0x242070DB);
            FF(ref b, c, d, a, Block[3], 22, 0xC1BDCEEE);
            FF(ref a, b, c, d, Block[4], 7, 0xF57C0FAF);
            FF(ref d, a, b, c, Block[5], 12, 0x4787C62A);
            FF(ref c, d, a, b, Block[6], 17, 0xA8304613);
            FF(ref b, c, d, a, Block[7], 22, 0xFD469501);
            FF(ref a, b, c, d, Block[8], 7, 0x698098D8);
            FF(ref d, a, b, c, Block[9], 12, 0x8B44F7AF);
            FF(ref c, d, a, b, Block[10], 17, 0xFFFF5BB1);
            FF(ref b, c, d, a, Block[11], 22, 0x895CD7BE);
            FF(ref a, b, c, d, Block[12], 7, 0x6B901122);
            FF(ref d, a, b, c, Block[13], 12, 0xFD987193);
            FF(ref c, d, a, b, Block[14], 17, 0xA679438E);
            FF(ref b, c, d, a, Block[15], 22, 0x49B40821);
            GG(ref a, b, c, d, Block[1], 5, 0xF61E2562);
            GG(ref d, a, b, c, Block[6], 9, 0xC040B340);
            GG(ref c, d, a, b, Block[11], 14, 0x265E5A51);
            GG(ref b, c, d, a, Block[0], 20, 0xE9B6C7AA);
            GG(ref a, b, c, d, Block[5], 5, 0xD62F105D);
            GG(ref d, a, b, c, Block[10], 9, 0x2441453);
            GG(ref c, d, a, b, Block[15], 14, 0xD8A1E681);
            GG(ref b, c, d, a, Block[4], 20, 0xE7D3FBC8);
            GG(ref a, b, c, d, Block[9], 5, 0x21E1CDE6);
            GG(ref d, a, b, c, Block[14], 9, 0xC33707D6);
            GG(ref c, d, a, b, Block[3], 14, 0xF4D50D87);
            GG(ref b, c, d, a, Block[8], 20, 0x455A14ED);
            GG(ref a, b, c, d, Block[13], 5, 0xA9E3E905);
            GG(ref d, a, b, c, Block[2], 9, 0xFCEFA3F8);
            GG(ref c, d, a, b, Block[7], 14, 0x676F02D9);
            GG(ref b, c, d, a, Block[12], 20, 0x8D2A4C8A);
            HH(ref a, b, c, d, Block[5], 4, 0xFFFA3942);
            HH(ref d, a, b, c, Block[8], 11, 0x8771F681);
            HH(ref c, d, a, b, Block[11], 16, 0x6D9D6122);
            HH(ref b, c, d, a, Block[14], 23, 0xFDE5380C);
            HH(ref a, b, c, d, Block[1], 4, 0xA4BEEA44);
            HH(ref d, a, b, c, Block[4], 11, 0x4BDECFA9);
            HH(ref c, d, a, b, Block[7], 16, 0xF6BB4B60);
            HH(ref b, c, d, a, Block[10], 23, 0xBEBFBC70);
            HH(ref a, b, c, d, Block[13], 4, 0x289B7EC6);
            HH(ref d, a, b, c, Block[0], 11, 0xEAA127FA);
            HH(ref c, d, a, b, Block[3], 16, 0xD4EF3085);
            HH(ref b, c, d, a, Block[6], 23, 0x4881D05);
            HH(ref a, b, c, d, Block[9], 4, 0xD9D4D039);
            HH(ref d, a, b, c, Block[12], 11, 0xE6DB99E5);
            HH(ref c, d, a, b, Block[15], 16, 0x1FA27CF8);
            HH(ref b, c, d, a, Block[2], 23, 0xC4AC5665);
            ii(ref a, b, c, d, Block[0], 6, 0xF4292244);
            ii(ref d, a, b, c, Block[7], 10, 0x432AFF97);
            ii(ref c, d, a, b, Block[14], 15, 0xAB9423A7);
            ii(ref b, c, d, a, Block[5], 21, 0xFC93A039);
            ii(ref a, b, c, d, Block[12], 6, 0x655B59C3);
            ii(ref d, a, b, c, Block[3], 10, 0x8F0CCC92);
            ii(ref c, d, a, b, Block[10], 15, 0xFFEFF47D);
            ii(ref b, c, d, a, Block[1], 21, 0x85845DD1);
            ii(ref a, b, c, d, Block[8], 6, 0x6FA87E4F);
            ii(ref d, a, b, c, Block[15], 10, 0xFE2CE6E0);
            ii(ref c, d, a, b, Block[6], 15, 0xA3014314);
            ii(ref b, c, d, a, Block[13], 21, 0x4E0811A1);
            ii(ref a, b, c, d, Block[4], 6, 0xF7537E82);
            ii(ref d, a, b, c, Block[11], 10, 0xBD3AF235);
            ii(ref c, d, a, b, Block[2], 15, 0x2AD7D2BB);
            ii(ref b, c, d, a, Block[9], 21, 0xEB86D391);
            State[0] += (uint)a;
            State[1] += (uint)b;
            State[2] += (uint)c;
            State[3] += (uint)d;
        }

        private static void MD5Init(ref MD5Context Context)
        {
            Context.State[0] = 0x67452301;
            Context.State[1] = 0xEFCDAB89;
            Context.State[2] = 0x98BADCFE;
            Context.State[3] = 0x10325476;
            Context.Count[0] = 0;
            Context.Count[1] = 0;
            //Context.ZeroMemory(Context.Buffer, sizeof(byte));
        }

        private static void MD5Update(ref MD5Context Context, ref byte[] Input, int Length)
        {
            var i = 0;
            var Index = (int)((Context.Count[0] >> 3) & 0x3F);
            Context.Count[0] += (uint)(Length << 3);
            if (Context.Count[0] < (Length << 3))
            {
                Context.Count[1]++;
            }
            Context.Count[1] += (uint)(Length >> 29);
            var PartLen = 64 - Index;
            if (Length >= PartLen)
            {
                Array.Copy(Input, 0, Context.Buffer, Index, PartLen);
                Transform(Context.Buffer, ref Context.State);
                i = PartLen;
                while (i + 63 < Length)
                {
                    //Transform(Input[i], ref Context.State);
                    i += 64;
                }
                Index = 0;
            }
            else
            {
                i = 0;
            }
            Array.Copy(Input, 0, Context.Buffer, 0, (int)PartLen);
        }

        private static void MD5Final(ref MD5Context Context, ref byte[] Digest)
        {
            byte[] Bits = new byte[8];
            int PadLen;
            Decode(Context.Count, ref Bits, 2);
            var Index = (int)((Context.Count[0] >> 3) & 0x3F);
            if (Index < 56)
            {
                PadLen = 56 - Index;
            }
            else
            {
                PadLen = 120 - Index;
            }
            MD5Update(ref Context, ref _padding, PadLen);
            MD5Update(ref Context, ref Bits, 8);
            Decode(Context.State, ref Digest, 4);
            //ZeroMemory(Context, sizeof(MD5Context));
        }

        private static byte[] MD5String(string m)
        {
            var buff = HUtil32.GetBytes(m);
            byte[] result = new byte[16];
            var Context = new MD5Context();
            MD5Init(ref Context);
            MD5Update(ref Context, ref buff, m.Length);
            MD5Final(ref Context, ref result);
            return result;
        }

        public static string MD5Print(byte[] d)
        {
            char[] Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            var result = "";
            for (var i = 0; i <= 15; i++)
            {
                result = result + Digits[(d[i] >> 4) & 0x0F] + Digits[d[i] & 0x0F];
            }
            return result;
        }

        public static byte[] MD5UnPrInt(string s)
        {
            byte[] result = null;
            byte[] Digest = new byte[16];
            if (s.Length == 32)
            {
                try
                {
                    for (var i = 0; i < 15; i++)
                    {
                        Digest[i] = Convert.ToByte("$" + s.Substring(1 + i * 2 - 1, 2));
                    }
                }
                catch
                {
                    result = Digest;
                }
            }
            return result;
        }

        /// <summary>
        /// Compare two Digests
        /// </summary>
        /// <param name="D1"></param>
        /// <param name="D2"></param>
        /// <returns></returns>
        public static bool MD5Match(byte[] D1, byte[] D2)
        {
            byte i = 0;
            var result = true;
            while (result && (i < 16))
            {
                result = D1[i] == D2[i];
                i++;
            }
            return result;
        }

        public static string RivestStr(string Str)
        {
            return MD5Print(MD5String(Str));
        }
    }

    public class MD5Context
    {
        public uint[] State;
        public uint[] Count;
        public byte[] Buffer;

        public MD5Context()
        {
            State = new uint[4];
            Count = new uint[2];
            Buffer = new byte[64];
        }
    }
}