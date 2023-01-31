using System;

namespace SystemModule
{
    public static class MemoryCopy
    {
        public static void ArrayCopy(byte[] source, int srcOff, byte[] destination, int dstOff, int count)
        {
            Array.Copy(source, srcOff, destination, dstOff, count);
        }

        public static void BlockCopy(byte[] source, int srcOff, Span<byte> destination, int dstOff, int count)
        {
            unsafe
            {
                fixed (byte* src = &source[srcOff])
                {
                    fixed (byte* dest = &destination[dstOff])
                    {
                        Buffer.MemoryCopy(
                            source: src, //要复制的字节的地址
                            destination: dest, //目标地址
                            destinationSizeInBytes: count, //目标内存块中可用的字节数
                            sourceBytesToCopy: count //要复制的字节数
                        );
                    }
                }
            }
        }

        public static void BlockCopy(Span<byte> source, int srcOff, Span<byte> destination, int dstOff, int count)
        {
            unsafe
            {
                fixed (byte* src = &source[srcOff])
                {
                    fixed (byte* dest = &destination[dstOff])
                    {
                        Buffer.MemoryCopy(
                            source: src, //要复制的字节的地址
                            destination: dest, //目标地址
                            destinationSizeInBytes: count, //目标内存块中可用的字节数
                            sourceBytesToCopy: count //要复制的字节数
                        );
                    }
                }
            }
        }

        public static void BlockCopy(ReadOnlySpan<byte> source, int srcOff, Span<byte> destination, int dstOff, int count)
        {
            unsafe
            {
                fixed (byte* src = &source[srcOff])
                {
                    fixed (byte* dest = &destination[dstOff])
                    {
                        System.Buffer.MemoryCopy(
                            source: src, //要复制的字节的地址
                            destination: dest, //目标地址
                            destinationSizeInBytes: count, //目标内存块中可用的字节数
                            sourceBytesToCopy: count //要复制的字节数
                        );
                    }
                }
            }
        }
    }
}