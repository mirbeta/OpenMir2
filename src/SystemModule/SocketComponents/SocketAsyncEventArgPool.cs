using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace SystemModule.SocketComponents
{
    /// <summary>
    /// 呈现一个可重用的SocketAsyncEventArgs对象集合.
    /// </summary>
    internal class SocketAsyncEventArgsPool
    {
        private readonly ConcurrentStack<SocketAsyncEventArgs> m_pool;

        /// <summary>
        /// 用指定的大小初始化对象池
        /// </summary>
        /// <param name="capacity">对象池可以管理的最大SocketAsyncEventArgs对象数量</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new ConcurrentStack<SocketAsyncEventArgs>();
        }

        /// <summary>
        /// 添加一个SocketAsyncEventArgs对象实例到池里
        /// </summary>
        /// <param name="item">要添加到池里的SocketAsyncEventArgs对象实例</param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException("要被添加到SocketAsyncEventArgs池的项目不能为空(null)"); }
            m_pool.Push(item);
        }

        /// <summary>
        /// 从池里删除一个SocketAsyncEventArgs对象实例
        /// </summary>
        /// <returns>要被从池里删除的对象</returns>
        public SocketAsyncEventArgs Pop()
        {
            if (m_pool.TryPop(out SocketAsyncEventArgs pop))
            {
                return pop;
            }
            return null;
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs对象实例的数量
        /// </summary>
        public int Count => m_pool.Count;
    }
}