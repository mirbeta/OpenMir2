using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 客户端集合
    /// </summary>
    [DebuggerDisplay("Count={Count}")]
    public sealed class SocketClientCollection
    {
        private readonly ConcurrentDictionary<string, ISocketClient> m_tokenDic = new ConcurrentDictionary<string, ISocketClient>();

        /// <summary>
        /// 数量
        /// </summary>
        public int Count => m_tokenDic.Count;

        /// <summary>
        /// 获取SocketClient
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ISocketClient this[string id]
        {
            get
            {
                TryGetSocketClient(id, out ISocketClient t);
                return t;
            }
        }

        /// <summary>
        /// 获取所有的客户端
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISocketClient> GetClients()
        {
            return m_tokenDic.Values;
        }

        /// <summary>
        /// 获取ID集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetIDs()
        {
            return m_tokenDic.Keys;
        }

        /// <summary>
        /// 根据ID判断SocketClient是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SocketClientExist(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            if (m_tokenDic.ContainsKey(id))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试获取实例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        public bool TryGetSocketClient(string id, out ISocketClient socketClient)
        {
            if (string.IsNullOrEmpty(id))
            {
                socketClient = null;
                return false;
            }

            return m_tokenDic.TryGetValue(id, out socketClient);
        }

        /// <summary>
        /// 尝试获取实例
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="id"></param>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        public bool TryGetSocketClient<TClient>(string id, out TClient socketClient) where TClient : ISocketClient
        {
            if (string.IsNullOrEmpty(id))
            {
                socketClient = default;
                return false;
            }

            if (m_tokenDic.TryGetValue(id, out ISocketClient client))
            {
                socketClient = (TClient)client;
                return true;
            }
            socketClient = default;
            return false;
        }

        internal bool TryAdd(ISocketClient socketClient)
        {
            return m_tokenDic.TryAdd(socketClient.ID, socketClient);
        }

        internal bool TryRemove(string id, out ISocketClient socketClient)
        {
            if (string.IsNullOrEmpty(id))
            {
                socketClient = null;
                return false;
            }
            return m_tokenDic.TryRemove(id, out socketClient);
        }

        internal bool TryRemove<TClient>(string id, out TClient socketClient) where TClient : ISocketClient
        {
            if (string.IsNullOrEmpty(id))
            {
                socketClient = default;
                return false;
            }

            if (m_tokenDic.TryRemove(id, out ISocketClient client))
            {
                socketClient = (TClient)client;
                return true;
            }
            socketClient = default;
            return false;
        }
    }
}