using NLog;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.SocketComponents.AsyncSocketClient;
using SystemModule.SocketComponents.Event;

namespace PlanesSystem
{
    /// <summary>
    /// 位面服务器
    /// </summary>
    public class PlanesClient
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private string sRecvMsg = string.Empty;
        private readonly ScoketClient _msgClient;
        private readonly PlanesMessage _groupMessageHandle;

        private static PlanesClient instance;

        public static PlanesClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlanesClient();
                }
                return instance;
            }
        }

        private PlanesClient()
        {
            _msgClient = new ScoketClient(new IPEndPoint(IPAddress.Parse(SystemShare.Config.MasterSrvAddr), SystemShare.Config.MasterSrvPort));
            _msgClient.OnConnected += MsgClientConnect;
            _msgClient.OnReceivedData += MsgClientRead;
            _msgClient.OnError += MsgClientError;
            _msgClient.OnDisconnected += MsgClientDisconnected;
            _groupMessageHandle = new PlanesMessage();
        }

        public void ConnectPlanesServer()
        {
            _msgClient.Connect();
        }

        public void Run()
        {
            if (_msgClient.IsConnected)
            {
                DecodeSocStr();
            }
        }

        public void CheckConnected()
        {
            if (SystemShare.Config.nServerNumber > 0)
            {
                if (_msgClient.IsConnected)
                {
                    return;
                }
                _msgClient.Connect();
            }
        }

        private bool IsConnect()
        {
            return _msgClient.IsConnected;
        }

        private void DecodeSocStr()
        {
            string Str = string.Empty;
            string sNumStr = string.Empty;
            string Head = string.Empty;
            int Ident;
            int sNum;
            const string sExceptionMsg = "[Exception] TFrmSrvMsg::DecodeSocStr";
            if (sRecvMsg.IndexOf(')') <= 0)
            {
                return;
            }
            try
            {
                string BufStr = sRecvMsg;
                sRecvMsg = string.Empty;
                while (BufStr.IndexOf(')') > -1)
                {
                    BufStr = HUtil32.ArrestStringEx(BufStr, "(", ")", ref Str);
                    if (!string.IsNullOrEmpty(Str))
                    {
                        string Body = HUtil32.GetValidStr3(Str, ref Head, HUtil32.Backslash);
                        Body = HUtil32.GetValidStr3(Body, ref sNumStr, HUtil32.Backslash);
                        Ident = HUtil32.StrToInt(Head, 0);
                        sNum = HUtil32.StrToInt(sNumStr, -1);
                        _groupMessageHandle.ProcessData(Ident, sNum, Body);
                    }
                    else
                    {
                        break;
                    }
                }
                sRecvMsg = BufStr + sRecvMsg;
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
        }

        public void SendSocket(string sMsg)
        {
            if (IsConnect())
            {
                byte[] buff = HUtil32.GetBytes("(" + sMsg + ")");
                _msgClient.Send(buff);
            }
        }

        private void MsgClientConnect(object sender, DSCClientConnectedEventArgs e)
        {
            _logger.Info("连接主服务器(" + e.RemoteEndPoint + ")成功...");
            //todo 链接主服务器成功后需要发消息链接主服务器告知主服务器当前服务器IP和端口，保持登录数据同步
        }

        private void MsgClientError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case SocketError.ConnectionRefused:
                    _logger.Error("主游戏引擎[" + _msgClient.RemoteEndPoint + "]拒绝链接...");
                    break;
                case SocketError.ConnectionReset:
                    _logger.Error("主游戏引擎[" + _msgClient.RemoteEndPoint + "]关闭连接...");
                    break;
                case SocketError.TimedOut:
                    _logger.Error("主游戏引擎[" + _msgClient.RemoteEndPoint + "]链接超时...");
                    break;
            }
        }

        private void MsgClientDisconnected(object sender, DSCClientConnectedEventArgs e)
        {
            _logger.Error("节点服务器(" + e.RemoteEndPoint + ")断开连接...");
        }

        /// <summary>
        /// 接收主机发送过来的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgClientRead(object sender, DSCClientDataInEventArgs e)
        {
            sRecvMsg += HUtil32.GetString(e.Buff.ToArray(), 0, e.BuffLen);
        }
    }
}

