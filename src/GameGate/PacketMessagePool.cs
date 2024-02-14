namespace GameGate
{
    public class PacketMessagePool
    {
        private readonly ConcurrentStack<SessionMessage> m_pool;

        /// <summary>
        /// 用指定的大小初始化对象池
        /// </summary>
        public PacketMessagePool()
        {
            m_pool = new ConcurrentStack<SessionMessage>();
        }

        /// <summary>
        /// 添加一个SendSessionMessage对象实例到池里
        /// </summary>
        /// <param name="item">要添加到池里的SendSessionMessage对象实例</param>
        public void Push(SessionMessage item)
        {
            //if (item.BuffLen == 0) { throw new ArgumentNullException("要被添加到SendSessionMessage池的项目不能为空(null)"); }
            m_pool.Push(item);
        }

        /// <summary>
        /// 从池里删除一个SendSessionMessage对象实例
        /// </summary>
        /// <returns>要被从池里删除的对象</returns>
        public SessionMessage Pop()
        {
            if (m_pool.TryPop(out SessionMessage pop))
            {
                return pop;
            }
            return default;
        }

        /// <summary>
        /// 归还一个SendSessionMessage对象实例到池里
        /// </summary>
        /// <param name="item"></param>
        public void Return(SessionMessage item)
        {
            item.BuffLen = 0;
            item.ServiceId = 0;
            //item.Buffer = IntPtr.Zero;
            item.ConnectionId = string.Empty;
            item.ServiceId = 0;
            m_pool.Push(item);
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs对象实例的数量
        /// </summary>
        public int Count => m_pool.Count;
    }
}