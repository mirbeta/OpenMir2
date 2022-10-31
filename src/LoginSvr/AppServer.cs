using LoginSvr.Services;
using System.Threading;

namespace LoginSvr
{
    public class AppServer
    {
        private readonly MirLog _logger;
        private readonly SessionServer _massocService;

        public AppServer(MirLog logger, SessionServer massocService)
        {
            _massocService = massocService;
            _logger = logger;
        }

        public void Start()
        {
            _logger.Information("正在启动服务器...");
            _logger.Information("正在等待服务器连接...");
        }
    }
}