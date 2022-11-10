using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace SystemModule.Sockets
{
    /// <summary>
    /// 呈现一个可重用的SocketAsyncEventArgs对象集合.
    /// </summary>
    internal class SocketAsyncEventArgsPool
    {
        private readonly Stack<SocketAsyncEventArgs> m_pool;
        private readonly ReaderWriterLock toolLock = new ReaderWriterLock();

        /// <summary>
        /// 用指定的大小初始化对象池
        /// </summary>
        /// <param name="capacity">对象池可以管理的最大SocketAsyncEventArgs对象数量</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// 添加一个SocketAsyncEventArgs对象实例到池里
        /// </summary>
        /// <param name="item">要添加到池里的SocketAsyncEventArgs对象实例</param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException("要被添加到SocketAsyncEventArgs池的项目不能为空(null)"); }
            toolLock.AcquireReaderLock(10);
            m_pool.Push(item);
            toolLock.ReleaseReaderLock();
        }

        /// <summary>
        /// 从池里删除一个SocketAsyncEventArgs对象实例
        /// </summary>
        /// <returns>要被从池里删除的对象</returns>
        public SocketAsyncEventArgs Pop()
        {
            try
            {
                if (Count <= 100)
                {
                    toolLock.AcquireReaderLock(50);
                }
                else
                {
                    toolLock.AcquireReaderLock(10);
                }
                return m_pool.Pop();
            }
            finally
            {
                toolLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs对象实例的数量
        /// </summary>
        public int Count => m_pool.Count;
    }
}