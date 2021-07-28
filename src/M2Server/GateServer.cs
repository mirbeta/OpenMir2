using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace M2Server
{
    public class GateServer
    {
        /// <summary>
        /// 游戏网关
        /// </summary>
        private readonly ISocketServer _gateSocket = null;

        public GateServer()
        {
            _gateSocket = new ISocketServer(20, 2048);
            _gateSocket.OnClientConnect += GateSocketClientConnect;
            _gateSocket.OnClientDisconnect += GateSocketClientDisconnect;
            _gateSocket.OnClientRead += GateSocketClientRead;
            _gateSocket.OnClientError += GateSocketClientError;
            _gateSocket.Init();
        }

        public void Start()
        {
            _gateSocket.Start(M2Share.g_Config.sGateAddr, M2Share.g_Config.nGatePort);
        }

        private void GateSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            //M2Share.RunSocket.CloseErrGate();
        }

        private void GateSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            M2Share.RunSocket.CloseGate(e);
        }

        private void GateSocketClientConnect(object sender, AsyncUserToken e)
        {
            M2Share.RunSocket.AddGate(e);
        }

        private void GateSocketClientRead(object sender, AsyncUserToken e)
        {
            M2Share.RunSocket.SocketRead(e);
        }
    }
}