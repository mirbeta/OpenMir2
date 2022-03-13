using System.Threading;

namespace LoginSvr
{
    public class AppServer
    {
        private readonly LogQueue _logQueue;
        private readonly MasSocService _massocService;

        public AppServer(LogQueue logQueue, MasSocService masSocService)
        {
            LSShare.Initialization();
            _massocService = masSocService;
            _logQueue = logQueue;
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