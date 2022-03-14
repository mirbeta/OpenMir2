using System;
using System.IO;

namespace SystemModule
{
    public static class BinaryWriterExtension
    {
        /// <summary>
        /// 字符串转Byte数组
        /// </summary>
        /// <returns></returns>
        public static void Write(this BinaryWriter binaryWriter, string value, int defaultSize)
        {
            var size = value.Length + 1;
            defaultSize = defaultSize + 1;
            var buffer = HUtil32.StringToByteAry(value, out int strLen);
            buffer[0] = (byte)strLen;
            if (size < defaultSize)
            {
                size = defaultSize;
            }
            Array.Resize(ref buffer, size);
            binaryWriter.Write(buffer, 0, buffer.Length);
        }
    }
}