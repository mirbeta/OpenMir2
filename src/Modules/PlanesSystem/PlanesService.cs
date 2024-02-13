using OpenMir2;
using SystemModule;

namespace PlanesSystem
{
    public class PlanesService : IPlanesService
    {


        public void Start()
        {
            if (SystemShare.ServerIndex == 0)
            {
                PlanesServer.Instance.StartPlanesServer();
                LogService.Debug("主机运行模式...");
            }
            else
            {
                PlanesClient.Instance.ConnectPlanesServer();
                LogService.Info($"节点运行模式...主机端口:[{SystemShare.Config.MasterSrvAddr}:{SystemShare.Config.MasterSrvPort}]");
            }
        }
    }
}