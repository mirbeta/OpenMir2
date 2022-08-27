using LoginGate.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate
{
    public class ServerApp
    {
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;
        private readonly MirLog _logger;

        public ServerApp(MirLog logger, ServerManager serverManager, ClientManager clientManager)
        {
            _logger = logger;
            _serverManager = serverManager;
            _clientManager = clientManager;
        }

        public void Start(CancellationToken stoppingToken)
        {
            _serverManager.Start();
            _clientManager.Start();
            _serverManager.ProcessLoginMessage(stoppingToken);
            _clientManager.ProcessSendMessage(stoppingToken);
        }

        public void Initialization()
        {
            _serverManager.Initialization();
            _clientManager.Initialization();
        }

        public void StopService()
        {
            _logger.LogInformation("正在停止服务...", 2);
            _serverManager.Stop();
            _clientManager.Stop();
            _logger.LogInformation("服务停止成功...", 2);
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            string sBlockIPaddr;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i++)
            {
                sBlockIPaddr = GateShare.TempBlockIPList[i];
                if (string.Compare(sIPaddr, sBlockIPaddr, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < GateShare.BlockIPList.Count; i++)
            {
                sBlockIPaddr = GateShare.BlockIPList[i];
                if (HUtil32.CompareLStr(sIPaddr, sBlockIPaddr, sBlockIPaddr.Length))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsConnLimited(string sIPaddr)
        {
            bool result = false;
            int nCount = 0;
            //for (var i = 0; i < ServerSocket.Socket.ActiveConnections; i++)
            //{
            //    if ((sIPaddr).CompareTo((ServerSocket.Connections[i].RemoteAddress)) == 0)
            //    {
            //        nCount++;
            //    }
            //}
            if (nCount > GateShare.nMaxConnOfIPaddr)
            {
                result = true;
            }
            return result;
        }
    }
}