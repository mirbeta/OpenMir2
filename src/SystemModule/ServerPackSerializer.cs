using MemoryPack;
using System.Runtime.CompilerServices;

namespace SystemModule
{
    public class ServerPackSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Serialize<T>(T origin)
        {
            return MemoryPackSerializer.Serialize(origin);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes)!;
        }
    }
}