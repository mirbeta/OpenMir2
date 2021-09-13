using System.Threading;
using SystemModule;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameSvr
{
    public class DataServer
    {
        private readonly IClientScoket _clientScoket;
        private Timer _connectTimer = null;

        public DataServer()
        {
            _clientScoket = new IClientScoket();
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.ReceivedDatagram += DBSocketRead;
        }

        public void Start()
        { 
            _connectTimer = new Timer(ConnectTimerTime, null, 5000, 3000);
            _clientScoket.Connect(M2Share.g_Config.sDBAddr, M2Share.g_Config.nDBPort);
        }

        public void Stop()
        {
            _connectTimer.Dispose();
            _clientScoket.Disconnect();
        }

        private void ConnectTimerTime(object obj)
        {
            if (!_clientScoket.IsConnected)
            {
                _clientScoket.Connect(M2Share.g_Config.sDBAddr, M2Share.g_Config.nDBPort);
            }
        }

        public void SendMessage(int nQueryID, string sMsg)
        {
            if (!_clientScoket.IsConnected)
            {
                return;
            }
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                M2Share.g_Config.sDBSocketRecvText = string.Empty;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
            }
            var nCheckCode = HUtil32.MakeLong(nQueryID ^ 170, sMsg.Length + 6);
            var by = new byte[sizeof(int)];
            unsafe
            {
                fixed (byte* pb = by)
                {
                    *(int*)pb = nCheckCode;
                }
            }
            var sCheckStr = EDcode.EncodeBuffer(@by, @by.Length);
            var sSendMsg = "#" + nQueryID + "/" + sMsg + sCheckStr + "!";
            M2Share.g_Config.boDBSocketWorking = true;
            var data = HUtil32.GetBytes(sSendMsg);
            _clientScoket.Send(data);
        }
        
        private void DbScoketDisconnected(object sender, DSCClientConnectedEventArgs e)
        {
            _clientScoket.IsConnected = false;
            M2Share.ErrorMessage("数据库服务器[" + e.RemoteAddress + ':' + e.RemotePort + "]断开连接...");
        }

        private void DbScoketConnected(object sender, DSCClientConnectedEventArgs e)
        {
            _clientScoket.IsConnected = true;
            M2Share.MainOutMessage("数据库服务器[" + e.RemoteAddress + ':' + e.RemotePort + "]连接成功...", messageColor: System.ConsoleColor.Green);
        }

        private void DBSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                M2Share.g_Config.sDBSocketRecvText = M2Share.g_Config.sDBSocketRecvText + e.Data;
                if (!M2Share.g_Config.boDBSocketWorking)
                {
                    M2Share.g_Config.sDBSocketRecvText = "";
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
            }
        }
    }
}