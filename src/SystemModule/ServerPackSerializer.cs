using MemoryPack;
using System;
using System.Runtime.CompilerServices;

namespace SystemModule
{
    public static class ServerPackSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Serialize<T>(T origin)
        {
            return MemoryPackSerializer.Serialize(origin, MemoryPackSerializerOptions.Utf16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(byte[] bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes, MemoryPackSerializerOptions.Utf16)!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(Span<byte> bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes, MemoryPackSerializerOptions.Utf16)!;
        }
    }
}