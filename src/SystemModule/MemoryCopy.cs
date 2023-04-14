using System;

namespace SystemModule
{
    public static class MemoryCopy
    {
        public static void ArrayCopy(byte[] source, int srcOff, byte[] destination, int dstOff, int count)
        {
            Array.Copy(source, srcOff, destination, dstOff, count);
        }

        public static unsafe void BlockCopy(nint source, int srcOff, Span<byte> destination, int dstOff, int count)
        {
            Span<byte> destinationSpan = new Span<byte>(source.ToPointer(), count);
            fixed (byte* src = &destinationSpan[srcOff])
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

        public static unsafe void BlockCopy(ReadOnlySpan<byte> source, int srcOff, void* destination, int dstOff, int count)
        {
            unsafe
            {
                fixed (byte* src = &source[srcOff])
                {
                    Buffer.MemoryCopy(
                        source: src, //要复制的字节的地址
                        destination: destination, //目标地址
                        destinationSizeInBytes: count, //目标内存块中可用的字节数
                        sourceBytesToCopy: count //要复制的字节数
                    );
                }
            }
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
    }
}