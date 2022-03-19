using System.Threading.Tasks;

namespace GameGate
{
    public class ServerApp
    {
        private ClientManager ClientManager => ClientManager.Instance;
        private SessionManager SessionManager => SessionManager.Instance;
        private ServerManager ServerManager => ServerManager.Instance;

        public ServerApp()
        {

        }

        public async Task Start()
        {
            var gTasks = new Task[2];
            var consumerTask1 = Task.Factory.StartNew(ServerManager.ProcessReviceMessage);
            gTasks[0] = consumerTask1;

            var consumerTask2 = Task.Factory.StartNew(SessionManager.ProcessSendMessage);
            gTasks[1] = consumerTask2;

            await Task.WhenAll(gTasks);
        }

        public void StartService()
        {
            GateShare.Initialization();
            ClientManager.Initialization();
            ServerManager.Start();
        }

        public void StopService()
        {
            ServerManager.Stop();
        }
    }
}