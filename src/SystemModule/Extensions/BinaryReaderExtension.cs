using System.IO;

namespace SystemModule
{
    public static class BinaryReaderExtension
    {
        public static string ReadString(this BinaryReader binaryReader, int size)
        {
            return new string(binaryReader.ReadChars(size));
        }

        public static byte[] ReadDeCodeBytes(this BinaryReader binaryReader, int size)
        {
            var buffLen = 0;
            var data = binaryReader.ReadBytes(size);
            return Misc.DecodeBuf(data, data.Length, ref buffLen);
        }
    }
}