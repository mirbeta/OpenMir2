using System.Net.Sockets;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace GameSvr.Snaps
{
    /// <summary>
    /// 位面服务器
    /// </summary>
    public class PlanesServer
    {
        private readonly TServerMsgInfo[] m_SrvArray;
        private readonly SocketServer _msgServer;
        private readonly PlanesMessage _groupMessageHandle;

        private static PlanesServer instance;

        public static PlanesServer Instance => instance ??= new PlanesServer();

        private PlanesServer()
        {
            m_SrvArray = new TServerMsgInfo[10];
            _msgServer = new SocketServer(10, 512);
            _msgServer.OnClientConnect += MsgServerClientConnect;
            _msgServer.OnClientDisconnect += MsgServerClientDisconnect;
            _msgServer.OnClientRead += MsgServerClientRead;
            _msgServer.Init();
            _groupMessageHandle = new PlanesMessage();
        }

        public void StartSnapsServer()
        {
            _msgServer.Start(M2Share.Config.MsgSrvAddr, M2Share.Config.MsgSrvPort);
            M2Share.Log.Info($"节点数据服务[{M2Share.Config.MsgSrvAddr}:{M2Share.Config.MsgSrvPort}]已启动.");
        }

        private void DecodeSocStr_SendOtherServer(TServerMsgInfo ps, string msgstr)
        {
            for (var i = 0; i < m_SrvArray.Length; i++)
            {
                var serverMsgInfo = m_SrvArray[i];
                if (serverMsgInfo == null)
                {
                    continue;
                }
                if (serverMsgInfo.Socket == null) continue;
                if (serverMsgInfo.SocketId != ps.SocketId)
                {
                    SendSocket(serverMsgInfo.Socket, msgstr);
                }
            }
        }

        private void DecodeSocStr(TServerMsgInfo ps)
        {
            var Str = string.Empty;
            var sNumStr = string.Empty;
            var Head = string.Empty;
            int Ident;
            int sNum;
            if (string.IsNullOrEmpty(ps.SocData))
            {
                return;
            }
            if (ps.SocData.IndexOf(')') <= 0)
            {
                return;
            }
            try
            {
                string BufStr = ps.SocData;
                ps.SocData = "";
                while (BufStr.IndexOf(')') > 0)
                {
                    BufStr = HUtil32.ArrestStringEx(BufStr, "(", ")", ref Str);
                    if (!string.IsNullOrEmpty(Str))
                    {
                        DecodeSocStr_SendOtherServer(ps, Str);
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
                ps.SocData = BufStr + ps.SocData;
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(ex.StackTrace);
            }
        }

        private void SendSocket(Socket Socket, string sMsg)
        {
            if (Socket.Connected)
            {
                var buffer = HUtil32.GetBytes("(" + sMsg + ")");
                Socket.Send(buffer);
            }
        }

        /// <summary>
        /// 发送消息给所有节点服务器
        /// </summary>
        /// <param name="msgstr"></param>
        public void SendServerSocket(string msgstr)
        {
            TServerMsgInfo ServerMsgInfo;
            for (var i = 0; i < m_SrvArray.Length; i++)
            {
                ServerMsgInfo = m_SrvArray[i];
                if (ServerMsgInfo == null)
                {
                    continue;
                }
                if (ServerMsgInfo.Socket != null && ServerMsgInfo.Socket.Connected)
                {
                    SendSocket(ServerMsgInfo.Socket, msgstr);
                }
            }
        }

        private void MsgServerClientConnect(object sender, AsyncUserToken e)
        {
            TServerMsgInfo ServerMsgInfo;
            for (var i = 0; i < m_SrvArray.Length; i++)
            {
                ServerMsgInfo = m_SrvArray[i];
                if (ServerMsgInfo == null)
                {
                    ServerMsgInfo = new TServerMsgInfo();
                    ServerMsgInfo.Socket = e.Socket;
                    ServerMsgInfo.SocData = "";
                    ServerMsgInfo.SocketId = e.ConnectionId;
                    M2Share.Log.Info("节点服务器(" + e.RemoteIPaddr + ':' + e.EndPoint.Port + ")链接成功...");
                    m_SrvArray[i] = ServerMsgInfo;
                    break;
                }
            }
        }

        private void MsgServerClientDisconnect(object sender, AsyncUserToken e)
        {
            TServerMsgInfo ServerMsgInfo;
            for (var i = 0; i < m_SrvArray.Length; i++)
            {
                ServerMsgInfo = m_SrvArray[i];
                if (ServerMsgInfo == null)
                {
                    continue;
                }
                if (ServerMsgInfo.SocketId == e.ConnectionId)
                {
                    ServerMsgInfo.Socket = null;
                    ServerMsgInfo.SocData = "";
                    M2Share.Log.Error("节点服务器(" + e.RemoteIPaddr + ':' + e.EndPoint.Port + ")断开连接...");
                    m_SrvArray[i] = null;
                    break;
                }
            }
        }

        /// <summary>
        /// 接收节点服务器发送过来的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgServerClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < m_SrvArray.Length; i++)
            {
                if (m_SrvArray[i] == null)
                {
                    continue;
                }
                if (m_SrvArray[i].SocketId == e.ConnectionId)
                {
                    m_SrvArray[i].SocData = m_SrvArray[i].SocData + HUtil32.GetString(e.ReceiveBuffer, e.Offset, e.BytesReceived);
                }
            }
        }

        public void Run()
        {
            const string sExceptionMsg = "[Exception] TFrmSrvMsg::Run";
            try
            {
                for (var i = 0; i < m_SrvArray.Length; i++)
                {
                    var ps = m_SrvArray[i];
                    if (ps == null)
                    {
                        continue;
                    }
                    if (ps.Socket != null)
                    {
                        DecodeSocStr(ps);
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
        }
    }

    public class TServerMsgInfo
    {
        public Socket Socket;
        public string SocData;
        public string SocketId;
    }

    public class ServerGruopInfo
    {
        public int nServerIdx;
        public string sChrName;
    }
}


