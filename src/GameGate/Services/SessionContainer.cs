using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using GameGate.Conf;
using NLog;

namespace GameGate.Services
{
    /// <summary>
    /// 客户端会话容器
    /// </summary>
    public class SessionContainer
    {
        private static readonly SessionContainer instance = new SessionContainer();
        public static SessionContainer Instance => instance;
        /// <summary>
        /// 配置文件
        /// </summary>
        private static ConfigManager ConfigManager => ConfigManager.Instance;
        /// <summary>
        /// 客户端会话列表
        /// </summary>
        private readonly ClientSession[][] _sessionMap;

        private SessionContainer()
        {
            _sessionMap = new ClientSession[ConfigManager.GateConfig.ServerWorkThread][];
            for (var i = 0; i < _sessionMap.Length; i++)
            {
                _sessionMap[i] = new ClientSession[GateShare.MaxSession];
            }
        }
        
        public void AddSession(byte serviceId, int sessionId, ClientSession clientSession)
        {
            _sessionMap[serviceId][sessionId] = clientSession;
        }

        public ClientSession GetSession(byte serviceId, int sessionId)
        {
            return _sessionMap[serviceId] == null ? null : _sessionMap[serviceId][sessionId];
        }

        public void CloseSession(byte serviceId, int sessionId)
        {
            if (serviceId > _sessionMap.Length)
            {
                return;
            }
            _sessionMap[serviceId][sessionId] = null;
        }

        public ClientSession[][] GetSessions()
        {
            return _sessionMap;
        }
    }
}