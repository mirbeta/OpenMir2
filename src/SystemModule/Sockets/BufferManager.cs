using System.Collections.Generic;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    /// <summary>
    /// 这个类创建一个可以被划分和分配给SocketAsyncEventArgs对象每次操作可以使用的单独的打的缓冲区
    /// 这可以使缓冲区很容易地被重复使用并且防止在内从中堆积碎片。
    /// BufferManager 类暴露的操作不是线程安全的(需要做线程安全处理)
    /// </summary>
    internal class BufferManager
    {
        private readonly int m_numBytes;                 // 被缓冲区池管理的字节总数
        private byte[] m_buffer;                // 被BufferManager维持的基础字节数组
        private readonly Stack<int> m_freeIndexPool;     // 释放的索引池
        private int m_currentIndex;             //当前索引
        private readonly int m_bufferSize;               //缓冲区大小

        /// <summary>
        /// 初始化缓冲区管理对象
        /// </summary>
        /// <param name="totalBytes">缓冲区管理对象管理的字节总数</param>
        /// <param name="bufferSize">每个缓冲区大小</param>
        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// 分配被缓冲区池使用的缓冲区空间
        /// </summary>
        public void InitBuffer()
        {
            // 创建一个大的大缓冲区并且划分给每一个SocketAsyncEventArgs对象
            m_buffer = new byte[m_numBytes];
        }

        /// <summary>
        /// 从缓冲区池中分配一个缓冲区给指定的SocketAsyncEventArgs对象
        /// </summary>
        /// <returns>如果缓冲区被成功设置返回真否则返回假</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        /// <summary>
        /// 从一个SocketAsyncEventArgs对象上删除缓冲区，这将把缓冲区释放回缓冲区池
        /// </summary>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}