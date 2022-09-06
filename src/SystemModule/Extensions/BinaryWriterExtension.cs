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
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (defaultSize == 0)
                throw new ArgumentNullException(nameof(defaultSize));

            var buffer = HUtil32.StringToByte(value);
            var reSize = buffer.Length + 1;
            var tempSize = defaultSize;
            if (reSize < tempSize)
            {
                reSize = tempSize;
            }
            binaryWriter.Write((byte)buffer.Length);
            if (buffer.Length != reSize)
            {
                Array.Resize(ref buffer, reSize);
            }
            binaryWriter.Write(buffer, 0, buffer.Length);
        }
    }
}