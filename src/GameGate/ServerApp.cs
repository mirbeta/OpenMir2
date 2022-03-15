using System.Threading.Tasks;

namespace GameGate
{
    public class ServerApp
    {
        private LogQueue logQueue => LogQueue.Instance;
        private ClientManager clientManager => ClientManager.Instance;
        private SessionManager sessionManager => SessionManager.Instance;
        private ServerManager serverManager => ServerManager.Instance;

        public ServerApp()
        {

        }

        public async Task Start()
        {
            var gTasks = new Task[2];
            var consumerTask1 = Task.Factory.StartNew(serverManager.ProcessReviceMessage);
            gTasks[0] = consumerTask1;

            var consumerTask2 = Task.Factory.StartNew(sessionManager.ProcessSendMessage);
            gTasks[1] = consumerTask2;

            await Task.WhenAll(gTasks);
        }

        public void StartService()
        {
            GateShare.Initialization();
            clientManager.Initialization();
            serverManager.Start();
        }

        public void StopService()
        {
            logQueue.Enqueue("正在停止服务...", 2);
            serverManager.Stop();
            logQueue.Enqueue("服务停止成功...", 2);
        }
    }
}