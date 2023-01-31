using System.Buffers;

namespace ConsoleApp1
{
    public class BufferManagers
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