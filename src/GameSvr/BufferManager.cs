using System.Buffers;

namespace GameSvr
{
    public class BufferManager
    {
        private static readonly ArrayPool<byte> _pool = ArrayPool<byte>.Create(1024 * 10, 1024 * 10);

        public static byte[] GetBuffer(int size)
        {
            return _pool.Rent(size);
        }

        public static void ReturnBuffer(byte[] buffer)
        {
            _pool.Return(buffer);
        }
    }
}