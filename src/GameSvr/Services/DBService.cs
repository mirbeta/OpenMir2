using System;
using System.IO;
using ProtoBuf;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;

namespace GameSvr
{
    public class DBService
    {
        private readonly IClientScoket _clientScoket;
        private AsynQueue<SaveHumData> _saveQueue;

        public DBService()
        {
            _clientScoket = new IClientScoket();
            _clientScoket.OnConnected += DbScoketConnected;
            _clientScoket.OnDisconnected += DbScoketDisconnected;
            _clientScoket.ReceivedDatagram += DBSocketRead;
            _clientScoket.OnError += DBSocketError;
            //_saveQueue = new AsynQueue<SaveHumData>();
            //_saveQueue.ProcessItemFunction += ProcessSaveHumData;
        }

        public void Start()
        {
            _clientScoket.Connect(M2Share.g_Config.sDBAddr, M2Share.g_Config.nDBPort);
            //_saveQueue.Start();
        }

        public void Stop()
        {
            _clientScoket.Disconnect();
            //_saveQueue.Stop();
        }

        public void CheckConnected()
        {
            if (_clientScoket.IsConnected)
            {
                return;
            }
            if (_clientScoket.IsBusy)
            {
                return;
            }
            _clientScoket.Connect(M2Share.g_Config.sDBAddr, M2Share.g_Config.nDBPort);
        }

      
        
        public void SendRequest<T>(int nQueryID, ServerMessagePacket packet,T requet) where T : CmdPacket
        {
            if (!_clientScoket.IsConnected)
            {
                return;
            }
            M2Share.g_Config.sDBSocketRecvText = string.Empty;

            var packBuff = EDcode.EncodeBuffer(ServerPacketDecoder.Serialize(packet));
            var bodyBuff = EDcode.EncodeBuffer(ServerPacketDecoder.Serialize(requet));

            var s = HUtil32.MakeLong(nQueryID ^ 170,  packBuff.Length + bodyBuff.Length);
            var nCheckCode = BitConverter.GetBytes(s);
            var codeBuff = EDcode.EncodeBuffer(nCheckCode);

            var requestPacket = new RequestServerPacket();
            requestPacket.QueryId = codeBuff;
            requestPacket.PacketHead = packBuff;
            requestPacket.PacketBody = bodyBuff;
            
            _clientScoket.Send(requestPacket.GetPacket());
            M2Share.g_Config.boDBSocketWorking = true;
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
            var sSendMsg = $"#{nQueryID}{sMsg}{sCheckStr}!";
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

        private void DBSocketError(object sender, DSCClientErrorEventArgs e)
        {
            _clientScoket.IsConnected = false;
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    M2Share.ErrorMessage("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    M2Share.ErrorMessage("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    M2Share.ErrorMessage("数据库服务器[" + M2Share.g_Config.sDBAddr + ":" + M2Share.g_Config.nDBPort + "]链接超时...");
                    break;
            }
        }

        private void DBSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                M2Share.g_Config.sDBSocketRecvText += HUtil32.GetString(e.Buff, 0, e.BuffLen);
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

        public void AddToSaveQueue(SaveHumData saveHumData)
        {
            _saveQueue.Enqueue(saveHumData);
        }

        private void ProcessSaveHumData(SaveHumData saveHumData)
        {
            if (saveHumData.MsgType == 1)
            {
                SendMessage(saveHumData.QueryId, EDcode.EncodeBuffer(Grobal2.MakeDefaultMsg(Grobal2.DB_SAVEHUMANRCD, saveHumData.SessionID, 0, 0, 0)) +
                                                 EDcode.EncodeString(saveHumData.sAccount) + "/" + EDcode.EncodeString(saveHumData.sCharName) + "/" + saveHumData.sHunData);
            }
            else if (saveHumData.MsgType == 0)
            {
                SendMessage(saveHumData.QueryId, EDcode.EncodeBuffer(Grobal2.MakeDefaultMsg(Grobal2.DB_LOADHUMANRCD, saveHumData.SessionID, 0, 0, 0)) +
                                                 EDcode.EncodeString(saveHumData.sAccount) + "/" + EDcode.EncodeString(saveHumData.sCharName) + "/" + saveHumData.sHunData);
            }
        }
        
 
    }

    public struct SaveHumData
    {
        public string sAccount;
        public string sCharName;
        public int QueryId;
        public int SessionID;
        public string sHunData;
        public int MsgType;
    }
}