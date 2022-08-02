using System.Threading;

namespace LoginSvr
{
    public class AppServer
    {
        private LogQueue _logQueue => LogQueue.Instance;
        private MasSocService _massocService => MasSocService.Instance;

        public AppServer()
        {

        }

        public void Start()
        {
            _logQueue.Enqueue("正在启动服务器...");
            _logQueue.Enqueue("正在等待服务器连接...");
            while (true)
            {
                if (_massocService.CheckReadyServers())
                {
                    break;
                }
                Thread.Sleep(1);
            }
        }

    }
}