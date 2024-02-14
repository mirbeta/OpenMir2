namespace GameGate.Services
{
    /// <summary>
    /// GameGate->GameSrv
    /// </summary>
    public class ClientManager
    {
        private static readonly ClientManager instance = new ClientManager();
        public static ClientManager Instance => instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        private readonly ConcurrentDictionary<string, ClientThread> _clientThreadMap;

        private ClientManager()
        {
            _clientThreadMap = new ConcurrentDictionary<string, ClientThread>();
        }

        /// <summary>
        /// 添加用户对应网关
        /// </summary>
        public void AddClientThread(string connectionId, ClientThread clientThread)
        {
            _clientThreadMap.TryAdd(connectionId, clientThread); //链接成功后建立对应关系
        }

        /// <summary>
        /// 获取用户链接对应网关
        /// </summary>
        /// <returns></returns>
        public ClientThread GetClientThread(string connectionId)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                return _clientThreadMap.TryGetValue(connectionId, out ClientThread userClinet) ? userClinet : null;
            }
            return null;
        }

        /// <summary>
        /// 从字典删除网关对应关系
        /// </summary>
        public void DeleteClientThread(string connectionId)
        {
            _clientThreadMap.TryRemove(connectionId, out ClientThread userClinet);
        }

        public ClientThread[] GetClients()
        {
            return !_clientThreadMap.IsEmpty ? _clientThreadMap.Values.ToArray() : null;
        }

    }
}