using System.Collections.Concurrent;
using System.Diagnostics;

namespace TouchSocket.Core
{
    /// <summary>
    /// 字节块集合
    /// </summary>
    [DebuggerDisplay("Count = {bytesQueue.Count}")]
    internal class BytesQueue : ConcurrentStack<byte[]>
    {
        internal int m_size;

        internal BytesQueue(int size)
        {
            m_size = size;
        }

        /// <summary>
        /// 占用空间
        /// </summary>
        public long FullSize => m_size * this.Count;

        internal long m_referenced;

        /// <summary>
        /// 获取当前实例中的空闲的Block
        /// </summary>
        /// <returns></returns>
        public bool TryGet(out byte[] bytes)
        {
            m_referenced++;
            return base.TryPop(out bytes);
        }

        /// <summary>
        /// 向当前集合添加Block
        /// </summary>
        /// <param name="bytes"></param>
        public void Add(byte[] bytes)
        {
            base.Push(bytes);
        }
    }
}