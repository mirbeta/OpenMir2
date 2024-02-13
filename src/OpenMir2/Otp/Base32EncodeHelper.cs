using System;

namespace OpenMir2.Otp
{
    /// <summary>
    /// Base32Encode
    /// https://stackoverflow.com/questions/641361/base32-decoding/7135008#7135008
    /// </summary>
    public static class Base32EncodeHelper
    {
        /// <summary>
        /// Standard Base32 characters
        /// https://www.rfc-editor.org/rfc/rfc4648#section-6
        /// </summary>
        public static readonly char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

        public static byte[] GetBytes(string base32String, char paddingChar = '=')
        {
            if (string.IsNullOrEmpty(base32String))
            {
                throw new ArgumentNullException(nameof(base32String));
            }

            base32String = base32String.TrimEnd(paddingChar); //remove padding characters
            var byteCount = base32String.Length * 5 / 8; //this must be TRUNCATED
            var returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            var arrayIndex = 0;
            foreach (var c in base32String)
            {
                var cValue = CharToValue(c);

                int mask;
                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        public static string FromBytes(byte[] base32Bytes, char paddingChar = '=')
        {
            if (base32Bytes == null)
            {
                return string.Empty;
            }

            var charCount = (int)Math.Ceiling(base32Bytes.Length / 5d) * 8;
            var returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            var arrayIndex = 0;

            foreach (var b in base32Bytes)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = paddingChar; //padding
            }

            return new string(returnArray);
        }

        private static int CharToValue(char c)
        {
            var value = (int)c;
            return value switch
            {
                //65-90 == uppercase letters
                < 91 and > 64 => value - 65,
                //50-55 == numbers 2-7
                < 56 and > 49 => value - 24,
                //97-122 == lowercase letters
                < 123 and > 96 => value - 97,
                _ => throw new ArgumentException(@"Character is not a Base32 character.", nameof(c))
            };
        }

        private static char ValueToChar(byte b)
        {
            return b switch
            {
                < 26 => (char)(b + 65),
                < 32 => (char)(b + 24),
                _ => throw new ArgumentException(@"The byte is not a Base32 value.", nameof(b))
            };
        }
    }
}