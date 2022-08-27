using System;
using System.IO;

namespace SystemModule.Extensions
{
    public static class BinaryWriterExtension
    {
        /// <summary>
        /// 字符串转Byte数组
        /// </summary>
        /// <returns></returns>
        public static void Write(this BinaryWriter binaryWriter, string value, int defaultSize)
        {
            var buffer = HUtil32.StringToByteAry(value, out int strLen);
            var reSize = value.Length + 1;
            var tempSize = defaultSize + 1;
            if (string.IsNullOrEmpty(value))
            {
                buffer[0] = (byte)defaultSize;
                reSize = tempSize;
            }
            else
            {
                buffer[0] = (byte)strLen;
                if (reSize < tempSize)
                {
                    reSize = tempSize;
                }
            }
            Array.Resize(ref buffer, reSize);
            binaryWriter.Write(buffer, 0, buffer.Length);
        }
    }
}