using System.Threading;
using SystemModule;
using SystemModule.Sockets;

namespace LoginSvr
{
    /// <summary>
    /// 远程监控服务(无用)
    /// </summary>
    public class MonSocService : IService
    {
        private readonly ISocketServer socketServer;
        private Timer monThreandTime;
        private MasSocService _masSock;
        private ConfigManager _configManager;

        public MonSocService(MasSocService masSock, ConfigManager configManager)
        {
            _masSock = masSock;
            socketServer = new ISocketServer(ushort.MaxValue, 1024);
            socketServer.Init();
            _configManager = configManager;
        }

        public void Start()
        {
            socketServer.Start(_configManager.Config.sMonAddr, _configManager.Config.nMonPort);
            monThreandTime = new Timer(MonTimer, null, 5000, 20000);
        }

        private void MonTimer(object obj)
        {
            string sMsg = string.Empty;
            int nC = _masSock.m_ServerList.Count;
            for (var i = 0; i < _masSock.m_ServerList.Count; i++)
            {
                var msgServer = _masSock.m_ServerList[i];
                var sServerName = msgServer.sServerName;
                if (sServerName != "")
                {
                    sMsg = sMsg + sServerName + "/" + msgServer.nServerIndex + "/" + msgServer.nOnlineCount + "/";
                    if ((HUtil32.GetTickCount() - msgServer.dwKeepAliveTick) < 30000)
                    {
                        sMsg = sMsg + "正常 ;";
                    }
                    else
                    {
                        sMsg = sMsg + "超时 ;";
                    }
                }
                else
                {
                    sMsg = "-/-/-/-;";
                }
            }
            var socketList = socketServer.GetSockets();
            for (var i = 0; i < socketList.Count; i++)
            {
                socketList[i].Socket.SendText(nC + ";" + sMsg);
            }
        }
    }
}