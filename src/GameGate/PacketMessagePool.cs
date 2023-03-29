using System;
using System.Collections.Concurrent;

namespace GameGate
{
    public class PacketMessagePool
    {
        private readonly ConcurrentStack<SendSessionMessage> m_pool;

        /// <summary>
        /// 用指定的大小初始化对象池
        /// </summary>
        public PacketMessagePool()
        {
            m_pool = new ConcurrentStack<SendSessionMessage>();
        }

        /// <summary>
        /// 添加一个SendSessionMessage对象实例到池里
        /// </summary>
        /// <param name="item">要添加到池里的SendSessionMessage对象实例</param>
        public void Push(SendSessionMessage item)
        {
            //if (item.BuffLen == 0) { throw new ArgumentNullException("要被添加到SendSessionMessage池的项目不能为空(null)"); }
            m_pool.Push(item);
        }

        /// <summary>
        /// 从池里删除一个SocketAsyncEventArgs对象实例
        /// </summary>
        /// <returns>要被从池里删除的对象</returns>
        public SendSessionMessage Pop()
        {
            if (m_pool.TryPop(out var pop))
            {
                return pop;
            }
            return default;
        }

        /// <summary>
        /// 归还一个SendSessionMessage对象实例到池里
        /// </summary>
        /// <param name="item"></param>
        public void Return(SendSessionMessage item)
        {
            item.BuffLen = 0;
            item.ServiceId = 0;
            item.Buffer = IntPtr.Zero;
            item.BuffLen = 0;
            item.ConnectionId = 0;
            m_pool.Push(item);
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs对象实例的数量
        /// </summary>
        public int Count => m_pool.Count;
    }
}