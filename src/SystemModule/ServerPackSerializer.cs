using MemoryPack;
using MemoryPack.Compression;
using System;
using System.Runtime.CompilerServices;

namespace SystemModule
{
    public static class ServerPackSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Serialize<T>(T origin)
        {
            return MemoryPackSerializer.Serialize(origin);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(byte[] bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes)!;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(Span<byte> bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes)!;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] MemoryPackBrotli<T>(T origin)
        {
            using var compressor = new BrotliCompressor();
            MemoryPackSerializer.Serialize(compressor, origin);
            return compressor.ToArray();
        }
    }
}