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
            gTasks[0] = ServerManager.ProcessReviceMessage();
            gTasks[1] = SessionManager.ProcessSendMessage();
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