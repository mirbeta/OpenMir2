using System;

namespace SystemModule
{
    public static class MemoryCopy
    {
        private const int Threshold = 128;
        private static readonly int PlatformWordSize = IntPtr.Size;
        private static readonly int PlatformWordSizeBits = PlatformWordSize * 8;

        public static void CopyMemory(byte[] src, int srcOff, byte[] dst, int dstOff, int count)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dst == null)
                throw new ArgumentNullException("dst");

            if (srcOff < 0)
                throw new ArgumentOutOfRangeException("srcOffset");

            if (dstOff < 0)
                throw new ArgumentOutOfRangeException("dstOffset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return;

            unsafe
            {
                fixed (byte* srcPtr = &src[srcOff])
                fixed (byte* dstPtr = &dst[dstOff])
                {
                    CopyMemory(srcPtr, dstPtr, count);
                }
            }
        }

        public static void CopyMemory(Span<byte> src, int srcOff, Span<byte> dst, int dstOff, int count)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dst == null)
                throw new ArgumentNullException("dst");

            if (srcOff < 0)
                throw new ArgumentOutOfRangeException("srcOffset");

            if (dstOff < 0)
                throw new ArgumentOutOfRangeException("dstOffset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return;

            unsafe
            {
                fixed (byte* srcPtr = &src[srcOff])
                fixed (byte* dstPtr = &dst[dstOff])
                {
                    CopyMemory(srcPtr, dstPtr, count);
                }
            }
        }

        private static unsafe void CopyMemory(byte* srcPtr, byte* dstPtr, int count)
        {
            const int u32Size = sizeof(uint);
            const int u64Size = sizeof(ulong);
            const int u128Size = sizeof(ulong) * 2;

            byte* srcEndPtr = srcPtr + count;

            if (PlatformWordSize == u32Size)
            {
                // 32-bit
                while (srcPtr + u64Size <= srcEndPtr)
                {
                    *(uint*)dstPtr = *(uint*)srcPtr;
                    dstPtr += u32Size;
                    srcPtr += u32Size;
                    *(uint*)dstPtr = *(uint*)srcPtr;
                    dstPtr += u32Size;
                    srcPtr += u32Size;
                }
            }
            else if (PlatformWordSize == u64Size)
            {
                // 64-bit            
                while (srcPtr + u128Size <= srcEndPtr)
                {
                    *(ulong*)dstPtr = *(ulong*)srcPtr;
                    dstPtr += u64Size;
                    srcPtr += u64Size;
                    *(ulong*)dstPtr = *(ulong*)srcPtr;
                    dstPtr += u64Size;
                    srcPtr += u64Size;
                }

                if (srcPtr + u64Size <= srcEndPtr)
                {
                    *(ulong*)dstPtr ^= *(ulong*)srcPtr;
                    dstPtr += u64Size;
                    srcPtr += u64Size;
                }
            }

            if (srcPtr + u32Size <= srcEndPtr)
            {
                *(uint*)dstPtr = *(uint*)srcPtr;
                dstPtr += u32Size;
                srcPtr += u32Size;
            }

            if (srcPtr + sizeof(ushort) <= srcEndPtr)
            {
                *(ushort*)dstPtr = *(ushort*)srcPtr;
                dstPtr += sizeof(ushort);
                srcPtr += sizeof(ushort);
            }

            if (srcPtr + 1 <= srcEndPtr)
            {
                *dstPtr = *srcPtr;
            }
        }

        public static void FastCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        {
            if (srcOffset < 0)
                throw new ArgumentOutOfRangeException("srcOffset");

            if (dst == null)
                throw new ArgumentNullException("dst");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (src.Length - srcOffset < count || dst.Length - dstOffset < count)
                throw new ArgumentOutOfRangeException();


            if (count <= Threshold)
                CopyMemory(src, srcOffset, dst, dstOffset, count);
            else
                Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
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