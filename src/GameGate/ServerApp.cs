using System;
using System.IO;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class ServerApp
    {
        private readonly LogQueue _logQueue;
        private readonly ServerService _serverService;
        private readonly ClientManager _clientManager;
        private readonly SessionManager _sessionManager;

        public ServerApp(LogQueue logQueue, ServerService serverService, SessionManager sessionManager, ClientManager clientManager)
        {
            _logQueue = logQueue;
            _serverService = serverService;
            _clientManager = clientManager;
            _sessionManager = sessionManager;
        }

        public async Task Start()
        {
            var gTasks = new Task[2];
            var consumerTask1 = Task.Factory.StartNew(_serverService.ProcessReviceMessage);
            gTasks[0] = consumerTask1;

            var consumerTask2 = Task.Factory.StartNew(_sessionManager.ProcessSendMessage);
            gTasks[1] = consumerTask2;

            await Task.WhenAll(gTasks);
        }

        public void StartService()
        {
            GateShare.Initialization();
            LoadAbuseFile();
            LoadBlockIPFile();
            _serverService.Start();
            _clientManager.Initialization();
            _clientManager.Start();
        }

        public void StopService()
        {
            _logQueue.Enqueue("正在停止服务...", 2);
            _serverService.Stop();
            _clientManager.Stop();
            _logQueue.Enqueue("服务停止成功...", 2);
        }

        public void LoadAbuseFile()
        {
            _logQueue.Enqueue("正在加载文字过滤配置信息...", 4);
            var sFileName = ".\\WordFilter.txt";
            if (File.Exists(sFileName))
            {
                //GateShare.AbuseList.LoadFromFile(sFileName);
            }
            _logQueue.Enqueue("文字过滤信息加载完成...", 4);
        }

        private void LoadBlockIPFile()
        {
            _logQueue.Enqueue("正在加载IP过滤配置信息...", 4);
            var sFileName = ".\\BlockIPList.txt";
            if (File.Exists(sFileName))
            {
                GateShare.BlockIPList.LoadFromFile(sFileName);
            }
            _logQueue.Enqueue("IP过滤配置信息加载完成...", 4);
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