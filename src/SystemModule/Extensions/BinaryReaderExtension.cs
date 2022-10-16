using System.IO;

namespace SystemModule.Extensions
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
            return PacketEncoder.DecodeBuf(data, data.Length, ref buffLen);
        }
        
        public static string ReadPascalString(this BinaryReader binaryReader, int size)
        {
            var packegeLen = binaryReader.ReadByte();
            if (size < packegeLen)
            {
                size = packegeLen;
            }
            var strbuff = binaryReader.ReadBytes(size);
            return HUtil32.GetString(strbuff, 0, packegeLen);
        }
    }
}