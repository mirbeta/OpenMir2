using NetFramework.AsyncSocketServer;

namespace M2Server
{
    public class GateServer
    {
        /// <summary>
        /// 游戏网关
        /// </summary>
        private readonly IServerSocket _gateSocket = null;

        public GateServer()
        {
            _gateSocket = new IServerSocket(20, 2048);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.Init();
        }

        public void Start()
        {
            _gateSocket.Start(M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort);
        }

        private void GateSocketClientError(object sender, NetFramework.AsyncUserToken e)
        {
            M2Share.RunSocket.CloseErrGate(e.Socket);
        }

        private void GateSocketClientDisconnect(object sender, NetFramework.AsyncUserToken e)
        {
            M2Share.RunSocket.CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, NetFramework.AsyncUserToken e)
        {
            M2Share.RunSocket.AddGate(e);
        }

        private void GateSocketClientRead(object sender, NetFramework.AsyncUserToken e)
        {
            M2Share.RunSocket.SocketRead(e);
        }
    }
}