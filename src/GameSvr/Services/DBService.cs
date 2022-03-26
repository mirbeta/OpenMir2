using System;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;

namespace GameSvr
{
    public class DBService
    {
        private readonly IClientScoket _clientScoket;
        private AsynQueue<SaveHumData> _saveQueue;
        private static int packetLen = 0;

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

        public void SendRequest<T>(int nQueryID, ServerMessagePacket packet, T requet) where T : CmdPacket
        {
            if (!_clientScoket.IsConnected)
            {
                return;
            }
            M2Share.g_Config.sDBSocketRecvBuff = null;

            var requestPacket = new RequestServerPacket();
            requestPacket.QueryId = nQueryID;
            requestPacket.Message = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));
            requestPacket.Packet = EDcode.EncodeBuffer(ProtoBufDecoder.Serialize(requet));

            var s = HUtil32.MakeLong(nQueryID ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            var nCheckCode = BitConverter.GetBytes(s);
            var codeBuff = EDcode.EncodeBuffer(nCheckCode);
            requestPacket.CheckBody = codeBuff;
            
            _clientScoket.Send(requestPacket.GetPacket());
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
                M2Share.g_Config.sDBSocketRecvBuff = null;
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
                var data = e.Buff;
                if (packetLen == 0 && data[0] == (byte)'#')
                {
                    packetLen = BitConverter.ToInt32(data[1..5]);
                }
                if (M2Share.g_Config.sDBSocketRecvBuff != null && M2Share.g_Config.sDBSocketRecvBuff.Length > 0)
                {
                    var tempBuff = new byte[M2Share.g_Config.sDBSocketRecvBuff.Length + e.BuffLen];
                    Buffer.BlockCopy(M2Share.g_Config.sDBSocketRecvBuff, 0, tempBuff, 0, M2Share.g_Config.sDBSocketRecvBuff.Length);
                    Buffer.BlockCopy(e.Buff, 0, tempBuff, M2Share.g_Config.sDBSocketRecvBuff.Length, e.BuffLen);
                    M2Share.g_Config.sDBSocketRecvBuff = tempBuff;
                }
                else
                {
                    M2Share.g_Config.sDBSocketRecvBuff = e.Buff;
                }
                var len = M2Share.g_Config.sDBSocketRecvBuff.Length - packetLen;
                if (len > 0)
                {
                    var tempBuff = new byte[len];
                    Buffer.BlockCopy(M2Share.g_Config.sDBSocketRecvBuff, packetLen, tempBuff, 0, len);
                    data = M2Share.g_Config.sDBSocketRecvBuff[..packetLen];
                    M2Share.g_Config.sDBSocketRecvBuff = tempBuff;
                    M2Share.g_Config.boDBSocketWorking = true;
                    ProcessData(data);
                }
                else if (len == 0)
                {
                    M2Share.g_Config.sDBSocketRecvBuff = data;
                    M2Share.g_Config.boDBSocketWorking = true;
                    ProcessData(data);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
            }
        }

        private void ProcessData(byte[] data)
        {
            if (M2Share.g_Config.boDBSocketWorking)
            {
                data = data[1..^1];
                var responsePacket = Packets.ToPacket<RequestServerPacket>(data);
                if (responsePacket != null && responsePacket.PacketLen > 0)
                {
                    var respCheckCode = responsePacket.QueryId;
                    var nLen = responsePacket.Message.Length + responsePacket.Packet.Length + responsePacket.CheckBody.Length;
                    if (nLen >= 12)
                    {
                        var nCheckCode = HUtil32.MakeLong(respCheckCode ^ 170, nLen);
                        var checkdata = BitConverter.GetBytes(nCheckCode);
                        var sCheckFlag = EDcode.EncodeBuffer(checkdata, checkdata.Length);
                        if (nLen == sCheckFlag.Length)
                        {
                            HumDataService.AddToProcess(respCheckCode, responsePacket);
                        }
                    }
                }
                M2Share.g_Config.boDBSocketWorking = false;
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