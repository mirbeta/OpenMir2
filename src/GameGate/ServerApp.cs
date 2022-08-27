using GameGate.Services;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    public class ServerApp
    {
        private static MirLog LogQueue => MirLog.Instance;
        private static ClientManager ClientManager => ClientManager.Instance;
        private static SessionManager SessionManager => SessionManager.Instance;
        private static ServerManager ServerManager => ServerManager.Instance;

        public ServerApp()
        {

        }

        public void StartService(CancellationToken stoppingToken)
        {
            LogQueue.Enqueue("服务已启动成功...", 2);
            LogQueue.Enqueue("欢迎使用翎风系列游戏软件...", 0);
            LogQueue.Enqueue("网站:http://www.gameofmir.com", 0);
            LogQueue.Enqueue("论坛:http://bbs.gameofmir.com", 0);
            LogQueue.Enqueue("智能反外挂程序已启动...", 0);
            LogQueue.Enqueue("智能反外挂程序云端已连接...", 0);
            GateShare.Initialization();
            ClientManager.Initialization();
            ServerManager.Start();
            ServerManager.StartProcessMessage(stoppingToken);
            SessionManager.ProcessSendMessage(stoppingToken);
        }

        public void StopService()
        {
            ServerManager.Stop();
        }
    }
}