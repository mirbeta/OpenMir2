using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
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
        private int s34C;
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
            ServerInfo.bo08 = true;
            ServerInfo.nSckHandle = (int)e.Socket.Handle;
            ServerInfo.sData = null;
            ServerInfo.Socket = e.Socket;
            ServerList.Add(ServerInfo);
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TServerInfo ServerInfo;
            for (var i = 0; i < ServerList.Count; i++)
            {
                ServerInfo = ServerList[i];
                if (ServerInfo.nSckHandle == (int)e.Socket.Handle)
                {
                    ServerInfo = null;
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
                    if (serverInfo.sData != null && serverInfo.sData.Length > 0)
                    {
                        Buffer.BlockCopy(data, 0, serverInfo.sData, serverInfo.sData.Length, data.Length);
                    }
                    else
                    {
                        serverInfo.sData = data;
                    }
                    ProcessServerPacket(serverInfo);
                    break;
                }
            }
        }
        
        private void ProcessServerPacket(TServerInfo serverInfo)
        {
            var bo25 = false;
            var sData = serverInfo.sData;
            if (sData[0] == (byte)'#' && sData[^1] == (byte)'!')
            {
                sData = sData[1..^1];
            }
            else
            {
                return;
            }
            if (sData.Length > 0)
            {
                RequestServerPacket requestPacket = new RequestServerPacket(sData);
                requestPacket.ToPacket();
                
                var nQueryId=  BitConverter.ToInt32(EDcode.DecodeBuff(requestPacket.QueryId));
                var packet = ServerPacketDecoder.DeSerialize<ServerMessagePacket>(requestPacket.PacketHead);
                var packetLen = requestPacket.PacketBody.Length + requestPacket.PacketHead.Length;
                if ((packetLen >= Grobal2.DEFBLOCKSIZE) && nQueryId > 0)
                {
                    var wE = nQueryId ^ 170;
                    int w10 = packetLen;
                    int n18 = HUtil32.MakeLong(wE, w10);
                    s34C = nQueryId;
                    ProcessServerMsg(packet, requestPacket.PacketBody, serverInfo.Socket); 
                    bo25 = true;
                }
            }
            serverInfo.sData = null;
            if (!bo25)
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                SendSocket(serverInfo.Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SendSocket<T>(Socket socket, RequestServerPacket packet, T requet)
        {
            var packBuff = EDcode.EncodeBuffer(ServerPacketDecoder.Serialize(packet));
            var bodyBuff = EDcode.EncodeBuffer(ServerPacketDecoder.Serialize(requet));

            var s = HUtil32.MakeLong(s34C ^ 170, packBuff.Length + bodyBuff.Length);
            var nCheckCode = BitConverter.GetBytes(s);
            var codeBuff = EDcode.EncodeBuffer(nCheckCode);

            var requestPacket = new RequestServerPacket();
            requestPacket.QueryId = codeBuff;
            requestPacket.PacketHead = packBuff;
            requestPacket.PacketBody = bodyBuff;

            socket.Send(requestPacket.GetPacket());

            //var by = BitConverter.GetBytes(nQueryId);
            //var s18 = EDcode.EncodeBuffer(by, by.Length);
            //socket.SendText("#" + s34C + "/" + sMsg + s18 + "!");
        }

        private void SendSocket(Socket Socket, string sMsg)
        {
            int nQueryId = HUtil32.MakeLong(s34C ^ 170, sMsg.Length + 6);
            var by = BitConverter.GetBytes(nQueryId);
            var s18 = EDcode.EncodeBuffer(by, by.Length);
            Socket.SendText("#" + s34C + "/" + sMsg + s18 + "!");
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

        private void ProcessServerMsg(ServerMessagePacket packet, byte[] sData, Socket Socket)
        {
            //string sDefMsg;
            /*if (nLen == Grobal2.DEFBLOCKSIZE)
            {
                sDefMsg = sMsg;
                sData = "";
            }
            else
            {
                sDefMsg = sMsg.Substring(0, Grobal2.DEFBLOCKSIZE);
                sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE, sMsg.Length - Grobal2.DEFBLOCKSIZE - 6);
            }*/
            switch (packet.Ident)
            {
                case Grobal2.DB_LOADHUMANRCD:
                    LoadHumanRcd(sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCD:
                    SaveHumanRcd(packet.Recog, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(sData, packet.Recog, Socket);
                    break;
                default:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
                    break;
            }
        }

        private void LoadHumanRcd(byte[] data, Socket Socket)
        {
            THumDataInfo HumanRCD = null;
            bool boFoundSession = false;
            LoadHumDataPacket LoadHuman = ServerPacketDecoder.DeSerialize<LoadHumDataPacket>(data);
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
            if ((nCheckCode == 1) || boFoundSession)
            {
                var loadHumData = new LoadHumanRcdResponsePacket();
                loadHumData.sChrName = loadHumData.sChrName;
                loadHumData.HumDataInfo = HumanRCD;
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                responsePack.PacketHead = ServerPacketDecoder.Serialize(messagePacket);
                responsePack.PacketBody = ServerPacketDecoder.Serialize(loadHumData);
                SendSocket(Socket, responsePack, loadHumData);
            }
            else
            {
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                responsePack.PacketHead = ServerPacketDecoder.Serialize(messagePacket);
                SendSocket(Socket, responsePack, messagePacket);
            }
        }

        private void SaveHumanRcd(int nRecog, byte[] sMsg, Socket Socket)
        {
            var saveHumDataPacket = ServerPacketDecoder.DeSerialize<SaveHumDataPacket>(sMsg);
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
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
            else
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SaveHumanRcdEx(byte[] sMsg, int nRecog, Socket Socket)
        {
            var saveHumDataPacket = ServerPacketDecoder.DeSerialize<SaveHumDataPacket>(sMsg);
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
            SaveHumanRcd(nRecog, sMsg, Socket);
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