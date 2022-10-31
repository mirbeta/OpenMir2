using LoginSvr.Services;
using System.Threading;
using SystemModule;

namespace LoginSvr
{
    public class AppServer
    {
        private readonly MirLogger _logger;
        private readonly SessionServer _massocService;

        public AppServer(MirLogger logger, SessionServer massocService)
        {
            _massocService = massocService;
            _logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("正在启动服务器...");
            _logger.LogInformation("正在等待服务器连接...");
        }
    }
}