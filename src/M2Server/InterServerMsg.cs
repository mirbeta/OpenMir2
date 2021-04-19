using NetFramework.AsyncSocketServer;
using System;
using System.Net.Sockets;

namespace M2Server
{
    public class TFrmSrvMsg
    {
        private readonly TServerMsgInfo[] m_SrvArray;
        private readonly IServerSocket _msgServer;

        public TFrmSrvMsg()
        {
            m_SrvArray = new TServerMsgInfo[10];
            _msgServer = new IServerSocket(10, 512);
            _msgServer.OnClientConnect += MsgServerClientConnect;
            _msgServer.OnClientDisconnect += MsgServerClientDisconnect;
            _msgServer.OnClientRead += MsgServerClientRead;
            _msgServer.Init();
        }

        public void StartMsgServer()
        {
            _msgServer.Start(M2Share.g_Config.sMsgSrvAddr, M2Share.g_Config.nMsgSrvPort);
        }

        public void DecodeSocStr_SendOtherServer(TServerMsgInfo ps,string msgstr)
        {
            for (var i = m_SrvArray.GetLowerBound(0); i <= m_SrvArray.GetUpperBound(0); i++)
            {
                var serverMsgInfo = m_SrvArray[i];
                if (serverMsgInfo.Socket == null) continue;
                if (serverMsgInfo.SocketId != ps.SocketId)
                {
                    SendSocket(serverMsgInfo.Socket, msgstr);
                }
            }
        }

        private void DecodeSocStr(TServerMsgInfo ps)
        {
            var BufStr = string.Empty;
            var Str = string.Empty;
            var sNumStr = string.Empty;
            var Head = string.Empty;
            var Body = string.Empty;
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
                BufStr = ps.SocData;
                ps.SocData = "";
                while (BufStr.IndexOf(')') > 0)
                {
                    BufStr = HUtil32.ArrestStringEx(BufStr, "(", ")", ref Str);
                    if (Str != "")
                    {
                        DecodeSocStr_SendOtherServer(ps, Str);
                        Body = HUtil32.GetValidStr3(Str, ref Head, "/");
                        Body = HUtil32.GetValidStr3(Body, ref sNumStr, "/");
                        Ident = HUtil32.Str_ToInt(Head, 0);
                        sNum = HUtil32.Str_ToInt(sNumStr, -1);
                        M2Share.GroupServer.ProcessData(Ident, sNum, Body);
                    }
                    else
                    {
                        break;
                    }
                }
                ps.SocData = BufStr + ps.SocData;
            }
            catch(Exception ex)
            {
                M2Share.MainOutMessage(ex.StackTrace, MessageType.Error);
            }
        }

        private void SendSocket(Socket Socket, string sMsg)
        {
            if (Socket.Connected)
            {
                var buffer = System.Text.Encoding.Default.GetBytes("(" + sMsg + ")");
                Socket.Send(buffer);
            }
        }

        public void SendServerSocket(string msgstr)
        {
            TServerMsgInfo ServerMsgInfo;
            for (var i = m_SrvArray.GetLowerBound(0); i <= m_SrvArray.GetUpperBound(0); i++)
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

        public void MsgServerClientConnect(object sender, NetFramework.AsyncUserToken e)
        {
            TServerMsgInfo ServerMsgInfo;
            for (var i = m_SrvArray.GetLowerBound(0); i <= m_SrvArray.GetUpperBound(0); i++)
            {
                ServerMsgInfo = m_SrvArray[i];
                if (ServerMsgInfo == null)
                {
                    ServerMsgInfo = new TServerMsgInfo();
                    ServerMsgInfo.Socket = e.Socket;
                    ServerMsgInfo.SocData = "";
                    ServerMsgInfo.SocketId = e.ConnectionId;
                    M2Share.MainOutMessage("连接从服务器(" + e.RemoteIPaddr + ':' + e.EndPoint.Port + ")成功...");
                    m_SrvArray[i] = ServerMsgInfo;
                    break;
                }
            }
        }

        private void MsgServerClientDisconnect(object sender, NetFramework.AsyncUserToken e)
        {
            TServerMsgInfo ServerMsgInfo;
            for (var i = m_SrvArray.GetLowerBound(0); i <= m_SrvArray.GetUpperBound(0); i++)
            {
                ServerMsgInfo = m_SrvArray[i];
                if (ServerMsgInfo == null) {
                    continue;
                }
                if (ServerMsgInfo.SocketId == e.ConnectionId)
                {
                    ServerMsgInfo.Socket = null;
                    ServerMsgInfo.SocData = "";
                    M2Share.ErrorMessage("节点断开服务器(" + e.RemoteIPaddr + ':' + e.EndPoint.Port + ")断开连接...", MessageType.Error);
                    m_SrvArray[i] = null;
                    break;
                }
            }
        }

        private void MsgServerClientRead(object sender, NetFramework.AsyncUserToken e)
        {
            for (var i = 0; i < m_SrvArray.Length; i++)
            {
                if (m_SrvArray[i] == null)
                {
                    continue;
                }
                if (m_SrvArray[i].SocketId == e.ConnectionId)
                {
                    m_SrvArray[i].SocData = m_SrvArray[i].SocData + System.Text.Encoding.Default.GetString(e.ReceiveBuffer, 0, e.BytesReceived);
                }
            }
        }

        public void Run()
        {
            TServerMsgInfo ps;
            const string sExceptionMsg = "[Exception] TFrmSrvMsg::Run";
            try
            {
                for (var i = m_SrvArray.GetLowerBound(0); i <= m_SrvArray.GetUpperBound(0); i++)
                {
                    ps = m_SrvArray[i];
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
                M2Share.MainOutMessage(sExceptionMsg, MessageType.Error);
            }
        }
    } 

    public class TServerMsgInfo
    {
        public Socket Socket;
        public string SocData;
        public string SocketId;
    } 
}

namespace M2Server
{
    public class InterServerMsg
    {
        public static TFrmSrvMsg instance = null;

        public static TFrmSrvMsg Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TFrmSrvMsg();
                }
                return instance;
            }
        }
    } 
}

