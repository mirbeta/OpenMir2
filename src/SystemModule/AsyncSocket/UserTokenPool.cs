using System;
using System.Collections.Concurrent;

namespace SystemModule.AsyncSocket
{
    public sealed class UserTokenPool
    {
        private readonly ConcurrentStack<UserToken> m_pool;

        public UserTokenPool()
        {
            m_pool = new ConcurrentStack<UserToken>();
        }

        public void Push(UserToken item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a UserTokenPool cannot be null");
            }
            m_pool.Push(item);
        }

        public void Reture(UserToken token)
        {
            token.TimeOut = -1;
            token.Id = Guid.Empty.ToString();
            token.ConnectSocket = null;
            token.ReceiveArgs.RemoteEndPoint = null;
            Push(token);
        }

        private void InitItem(UserToken token)
        {
            token.Id = Guid.NewGuid().ToString();
        }

        public UserToken Pop()
        {
            if (m_pool.TryPop(out UserToken item))
            {
                InitItem(item);
                return item;
            }
            return item;
        }

        public int Count
        {
            get
            {
                return m_pool.Count;
            }
        }

        public void Clear()
        {
            m_pool.Clear();
        }
    }
}