using NLog;
using SystemModule;

namespace PlanesSystem
{
    public class PlanesService : IPlanesService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            if (ModuleShare.ServerIndex == 0)
            {
                PlanesServer.Instance.StartPlanesServer();
                _logger.Debug("主机运行模式...");
            }
            else
            {
                PlanesClient.Instance.ConnectPlanesServer();
                _logger.Info($"节点运行模式...主机端口:[{ModuleShare.Config.MasterSrvAddr}:{ModuleShare.Config.MasterSrvPort}]");
            }
        }
    }
}