using System;
using System.IO;

namespace SystemModule.Extensions
{
    public static class BinaryWriterExtension
    {
        /// <summary>
        /// 写入Ascii字符串
        /// </summary>
        /// <returns></returns>
        public static void WriteAsciiString(this BinaryWriter binaryWriter, string value, int defaultSize)
        {
            if (binaryWriter == null)
                throw new ArgumentNullException(nameof(binaryWriter));
            if (defaultSize == 0)
                throw new ArgumentNullException(nameof(defaultSize));

            byte[] buffer;
            if (string.IsNullOrEmpty(value) && defaultSize > 0)
            {
                buffer = new byte[defaultSize];
            }
            else
            {
                buffer = HUtil32.StringToByte(value);
            }

            int reSize = buffer.Length + 1;
            int tempSize = defaultSize;
            if (reSize < tempSize)
            {
                reSize = tempSize;
            }
            if (buffer.Length > defaultSize)
            {
                Array.Resize(ref buffer, defaultSize);
                binaryWriter.Write((byte)defaultSize);
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    binaryWriter.Write((byte)0);
                }
                else
                {
                    binaryWriter.Write((byte)buffer.Length);
                }
                if (buffer.Length != defaultSize)
                {
                    Array.Resize(ref buffer, defaultSize);
                }
            }
            binaryWriter.Write(buffer, 0, buffer.Length);
        }
    }
}