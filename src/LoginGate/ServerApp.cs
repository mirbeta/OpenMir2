using System;
using System.Threading.Tasks;
using SystemModule;

namespace LoginGate
{
    public class ServerApp
    {
        private ServerManager _ServerManager => ServerManager.Instance;
        private ClientManager _clientManager => ClientManager.Instance;
        private SessionManager _sessionManager => SessionManager.Instance;
        private LogQueue _logQueue => LogQueue.Instance;

        public ServerApp()
        {

        }

        public async Task Start()
        {
            var gTasks = new Task[2];
            var consumerTask1 = Task.Factory.StartNew(_ServerManager.ProcessReviceMessage);
            gTasks[0] = consumerTask1;

            var consumerTask2 = Task.Factory.StartNew(_sessionManager.ProcessSendMessage);
            gTasks[1] = consumerTask2;

            await Task.WhenAll(gTasks);
        }

        public void StartService()
        {
            _clientManager.Initialization();
            _ServerManager.Start();
        }

        public void StopService()
        {
            _logQueue.Enqueue("正在停止服务...", 2);
            _ServerManager.Stop();
            _logQueue.Enqueue("服务停止成功...", 2);
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