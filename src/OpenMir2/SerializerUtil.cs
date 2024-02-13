using MemoryPack;
using System;
using System.Runtime.CompilerServices;

namespace OpenMir2
{
    public static class SerializerUtil
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
        public static T Deserialize<T>(byte[] bytes, bool decrypt)
        {
            if (decrypt)
            {
                return MemoryPackSerializer.Deserialize<T>(EDCode.DecodeBuff(bytes), MemoryPackSerializerOptions.Utf16)!;
            }
            return MemoryPackSerializer.Deserialize<T>(bytes, MemoryPackSerializerOptions.Utf16)!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(Span<byte> bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes, MemoryPackSerializerOptions.Utf16)!;
        }
    }
}