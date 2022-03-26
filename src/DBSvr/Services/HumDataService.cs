using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Sockets;

namespace DBSvr
{
    /// <summary>
    /// 玩家数据服务
    /// </summary>
    public class HumDataService
    {
        private IList<TServerInfo> ServerList = null;
        private IList<THumSession> HumSessionList = null;
        private ClientPacket m_DefMsg;
        private readonly MySqlHumDB HumDB;
        private readonly ISocketServer serverSocket;
        private readonly LoginSocService _LoginSoc;

        public HumDataService(LoginSocService frmIdSoc, MySqlHumDB humDb)
        {
            _LoginSoc = frmIdSoc;
            HumDB = humDb;
            ServerList = new List<TServerInfo>();
            HumSessionList = new List<THumSession>();
            serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            serverSocket.OnClientConnect += ServerSocketClientConnect;
            serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            serverSocket.OnClientRead += ServerSocketClientRead;
            serverSocket.OnClientError += ServerSocketClientError;
            serverSocket.Init();
        }

        public void Start()
        {
            serverSocket.Start(DBShare.sServerAddr, DBShare.nServerPort);
            DBShare.MainOutMessage($"数据库角色服务[{DBShare.sServerAddr}:{DBShare.nServerPort}]已启动.等待链接...");
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            TServerInfo ServerInfo;
            string sIPaddr = e.RemoteIPaddr;
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                DBShare.MainOutMessage("非法服务器连接: " + sIPaddr);
                e.Socket.Close();
                return;
            }
            ServerInfo = new TServerInfo();
            ServerInfo.nSckHandle = (int)e.Socket.Handle;
            ServerInfo.Data = null;
            ServerInfo.Socket = e.Socket;
            ServerList.Add(ServerInfo);
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < ServerList.Count; i++)
            {
                if (ServerList[i].nSckHandle == (int)e.Socket.Handle)
                {
                    ServerList[i] = null;
                    ServerList.RemoveAt(i);
                    ClearSocket(e.Socket);
                    break;
                }
            }
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < ServerList.Count; i++)
            {
                var serverInfo = ServerList[i];
                if (serverInfo.nSckHandle == (int)e.Socket.Handle)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    if (serverInfo.DataLen == 0 && data[0] == (byte)'#')
                    {
                        serverInfo.DataLen = BitConverter.ToInt32(data[1..5]);
                    }
                    if (serverInfo.Data != null && serverInfo.Data.Length > 0)
                    {
                        var tempBuff = new byte[serverInfo.Data.Length + nReviceLen];
                        Buffer.BlockCopy(serverInfo.Data, 0, tempBuff, 0, serverInfo.Data.Length);
                        Buffer.BlockCopy(data, 0, tempBuff, serverInfo.Data.Length, data.Length);
                        serverInfo.Data = tempBuff;
                    }
                    else
                    {
                        serverInfo.Data = data;
                    }
                    var len = serverInfo.Data.Length - serverInfo.DataLen;
                    if (len > 0)
                    {
                        var tempBuff = new byte[len];
                        Buffer.BlockCopy(serverInfo.Data, serverInfo.DataLen, tempBuff, 0, len);
                        data = serverInfo.Data[..serverInfo.DataLen];
                        serverInfo.Data = tempBuff;
                        ProcessServerPacket(serverInfo, data);
                        serverInfo.DataLen = tempBuff.Length;
                    }
                    else if (len == 0)
                    {
                        ProcessServerPacket(serverInfo, serverInfo.Data);
                        serverInfo.Data = null;
                        serverInfo.DataLen = 0;
                    }
                    break;
                }
            }
        }
        
        private void ProcessServerPacket(TServerInfo serverInfo,byte[] data)
        {
            var bo25 = false;
            var nQueryId = 0;
            if (data.Length > 0)
            {
                var requestPacket = Packets.ToPacket<RequestServerPacket>(data);
                if (requestPacket == null)
                {
                    return;
                }
                nQueryId = requestPacket.QueryId;
                var packet = ProtoBufDecoder.DeSerialize<ServerMessagePacket>(EDcode.DecodeBuff(requestPacket.Message));
                var packetLen = requestPacket.Message.Length + requestPacket.Packet.Length + 6;
                if ((packetLen >= Grobal2.DEFBLOCKSIZE) && nQueryId > 0)
                {
                    var wE = nQueryId ^ 170;
                    var n18 = HUtil32.MakeLong(wE, packetLen);
                    var checkCode = BitConverter.ToInt32(requestPacket.CheckKey);
                    if (checkCode == n18)
                    {
                        ProcessServerMsg(nQueryId, packet, requestPacket.Packet, serverInfo.Socket);
                        bo25 = true;
                    }
                    else
                    {
                        serverInfo.Socket.Close();
                        Console.WriteLine("关闭错误的查询请求.");
                    }
                }
            }
            serverInfo.Data = null;
            if (!bo25)
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                SendSocket(nQueryId, serverInfo.Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SendRequest(Socket socket, RequestServerPacket requestPacket)
        {
            var s = HUtil32.MakeLong(requestPacket.QueryId ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            var nCheckCode = BitConverter.GetBytes(s);
            var codeBuff = EDcode.EncodeBuffer(nCheckCode);
            requestPacket.CheckKey = codeBuff;
            var pk = requestPacket.GetPacket();
            socket.Send(pk, pk.Length, SocketFlags.None);
        }
        
        private void SendRequest<T>(Socket socket, RequestServerPacket requestPacket, T packet) where T : class, new()
        {
            if (packet != null)
            {
                requestPacket.Packet = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));
            }
            var s = HUtil32.MakeLong(requestPacket.QueryId ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            var nCheckCode = BitConverter.GetBytes(s);
            var codeBuff = EDcode.EncodeBuffer(nCheckCode);
            requestPacket.CheckKey = codeBuff;
            var pk = requestPacket.GetPacket();
            socket.Send(pk, pk.Length, SocketFlags.None);
        }

        private void SendSocket(int queryId, Socket Socket, string sMsg)
        {
            int nQueryId = HUtil32.MakeLong(queryId ^ 170, sMsg.Length + 6);
            var by = BitConverter.GetBytes(nQueryId);
            var s18 = EDcode.EncodeBuffer(by, by.Length);
            Socket.SendText("#" + nQueryId + "/" + sMsg + s18 + "!");
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void ClearTimeoutSession()
        {
            THumSession HumSession;
            int i = 0;
            while (true)
            {
                if (HumSessionList.Count <= i)
                {
                    break;
                }
                HumSession = HumSessionList[i];
                if (!HumSession.bo24)
                {
                    if (HumSession.bo2C)
                    {
                        if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 20 * 1000)
                        {
                            HumSession = null;
                            HumSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 2 * 60 * 1000)
                        {
                            HumSession = null;
                            HumSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 40 * 60 * 1000)
                {
                    HumSession = null;
                    HumSessionList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        public bool CopyHumData(string sSrcChrName, string sDestChrName, string sUserID)
        {
            THumDataInfo HumanRCD = null;
            bool result = false;
            bool bo15 = false;
            try
            {
                if (HumDB.Open())
                {
                    int n14 = HumDB.Index(sSrcChrName);
                    if ((n14 >= 0) && (HumDB.Get(n14, ref HumanRCD) >= 0))
                    {
                        bo15 = true;
                    }
                    if (bo15)
                    {
                        n14 = HumDB.Index(sDestChrName);
                        if ((n14 >= 0))
                        {
                            HumanRCD.Header.sName = sDestChrName;
                            HumanRCD.Data.sCharName = sDestChrName;
                            HumanRCD.Data.sAccount = sUserID;
                            HumDB.Update(n14, ref HumanRCD);
                            result = true;
                        }
                    }
                }
            }
            finally
            {
                HumDB.Close();
            }
            return result;
        }

        private void ProcessServerMsg(int nQueryId,ServerMessagePacket packet, byte[] sData, Socket Socket)
        {
            sData = EDcode.DecodeBuff(sData);
            switch (packet.Ident)
            {
                case Grobal2.DB_LOADHUMANRCD:
                    LoadHumanRcd(nQueryId, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCD:
                    SaveHumanRcd(nQueryId, packet.Recog, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(nQueryId, sData, packet.Recog, Socket);
                    break;
                default:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    SendSocket(nQueryId, Socket, EDcode.EncodeMessage(m_DefMsg));
                    break;
            }
        }

        private void LoadHumanRcd(int queryId,byte[] data, Socket Socket)
        {
            THumDataInfo HumanRCD = null;
            bool boFoundSession = false;
            LoadHumDataPacket LoadHuman = ProtoBufDecoder.DeSerialize<LoadHumDataPacket>(data);
            int nCheckCode = -1;
            if ((!string.IsNullOrEmpty(LoadHuman.sAccount)) && (!string.IsNullOrEmpty(LoadHuman.sChrName)))
            {
                nCheckCode = _LoginSoc.CheckSessionLoadRcd(LoadHuman.sAccount, LoadHuman.sUserAddr, LoadHuman.nSessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    DBShare.MainOutMessage("[非法请求] " + "帐号: " + LoadHuman.sAccount + " IP: " + LoadHuman.sUserAddr + " 标识: " + LoadHuman.nSessionID);
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                int nIndex = HumDB.Index(LoadHuman.sChrName);
                if (nIndex >= 0)
                {
                    if (HumDB.Get(nIndex, ref HumanRCD) < 0)
                    {
                        nCheckCode = -2;
                    }
                }
                else
                {
                    nCheckCode = -3;
                }
            }
            var responsePack = new RequestServerPacket();
            responsePack.QueryId = queryId;
            if ((nCheckCode == 1) || boFoundSession)
            {
                var loadHumData = new LoadHumanRcdResponsePacket();
                loadHumData.sChrName = EDcode.EncodeString(LoadHuman.sChrName);
                loadHumData.HumDataInfo = HumanRCD;
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                responsePack.Message = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(Socket, responsePack, loadHumData);
            }
            else
            {
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                responsePack.Message = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(Socket, responsePack);
            }
        }

        private void SaveHumanRcd(int nQueryId,int nRecog, byte[] sMsg, Socket Socket)
        {
            var saveHumDataPacket = ProtoBufDecoder.DeSerialize<SaveHumDataPacket>(sMsg);
            if (saveHumDataPacket == null)
            {
                Console.WriteLine("保存玩家数据出错.");
                return;
            }
            var sUserID = saveHumDataPacket.sAccount;
            var sChrName = saveHumDataPacket.sCharName;
            var humanRcd = saveHumDataPacket.HumDataInfo;
            bool bo21 = humanRcd == null;
            if (!bo21)
            {
                bo21 = true;
                try
                {
                    int nIndex = HumDB.Index(sChrName);
                    if (nIndex < 0)
                    {
                        humanRcd.Header.sName = sChrName;
                        HumDB.Add(ref humanRcd);
                        nIndex = HumDB.Index(sChrName);
                    }
                    if (nIndex >= 0)
                    {
                        humanRcd.Header.sName = sChrName;
                        HumDB.Update(nIndex, ref humanRcd);
                        bo21 = false;
                    }
                }
                finally
                {
                   
                }
                _LoginSoc.SetSessionSaveRcd(sUserID);
            }
            if (!bo21)
            {
                for (var i = 0; i < HumSessionList.Count; i++)
                {
                    THumSession HumSession = HumSessionList[i];
                    if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                    {
                        HumSession.lastSessionTick = HUtil32.GetTickCount();
                        break;
                    }
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_SAVEHUMANRCD, 1, 0, 0, 0);
                SendSocket(nQueryId, Socket, EDcode.EncodeMessage(m_DefMsg));
            }
            else
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                SendSocket(nQueryId, Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SaveHumanRcdEx(int nQueryId, byte[] sMsg, int nRecog, Socket Socket)
        {
            var saveHumDataPacket = ProtoBufDecoder.DeSerialize<SaveHumDataPacket>(sMsg);
            if (saveHumDataPacket == null)
            {
                Console.WriteLine("保存玩家数据出错.");
                return;
            }
            var sChrName = saveHumDataPacket.sCharName;
            for (var i = 0; i < HumSessionList.Count; i++)
            {
                THumSession HumSession = HumSessionList[i];
                if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                {
                    HumSession.bo24 = false;
                    HumSession.Socket = Socket;
                    HumSession.bo2C = true;
                    HumSession.lastSessionTick = HUtil32.GetTickCount();
                    break;
                }
            }
            SaveHumanRcd(nQueryId, nRecog, sMsg, Socket);
        }

        private void ClearSocket(Socket Socket)
        {
            THumSession HumSession;
            int nIndex = 0;
            while (true)
            {
                if (HumSessionList.Count <= nIndex)
                {
                    break;
                }
                HumSession = HumSessionList[nIndex];
                if (HumSession.Socket == Socket)
                {
                    HumSession = null;
                    HumSessionList.RemoveAt(nIndex);
                    continue;
                }
                nIndex++;
            }
        }
    }
}