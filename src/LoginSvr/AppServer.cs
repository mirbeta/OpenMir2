using LoginSvr.Services;
using System.Threading;

namespace LoginSvr
{
    public class AppServer
    {
        private readonly MirLog _logger;
        private readonly MasSocService _massocService;

        public AppServer(MirLog logger, MasSocService massocService)
        {
            _massocService = massocService;
            _logger = logger;
        }

        public void Start()
        {
            _logger.Information("正在启动服务器...");
            _logger.Information("正在等待服务器连接...");
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