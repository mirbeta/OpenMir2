using OpenMir2;
using OpenMir2.Packets.ClientPackets;
using OpenMir2.Packets.ServerPackets;
using SelGate.Conf;
using SelGate.Datas;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SelGate.Services
{
    /// <summary>
    /// 会话封包处理
    /// </summary>
    public class ClientSession
    {
        private readonly SessionInfo _session;
        private bool _kickFlag = false;
        private int _clientTimeOutTick = 0;

        private readonly ClientThread _lastDbSvr;
        private readonly ConfigManager _configManager;
        private readonly ServerService _serverService;

        public ClientSession(ConfigManager configManager, SessionInfo session, ClientThread clientThread)
        {
            _session = session;
            _lastDbSvr = clientThread;
            _serverService = GateShare.ServiceProvider.GetService<ServerService>();
            _configManager = configManager;
            _clientTimeOutTick = HUtil32.GetTickCount();
        }

        private SessionInfo Session => _session;

        private GateConfig Config => _configManager.GateConfig;

        public ClientThread ClientThread => _lastDbSvr;

        /// <summary>
        /// 处理客户端发送过来的封包
        /// 发送创建角色，删除角色，恢复角色，创建名字等功能
        /// </summary>
        /// <param name="userData"></param>
        public void HandleUserPacket(MessageData userData)
        {
            if ((userData.MsgLen >= 5) && Config.DefenceCCPacket)
            {
                string sMsg = HUtil32.GetString(userData.Body, 2, userData.MsgLen - 3);
                if (sMsg.IndexOf("HTTP/", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    //if (LogManager.g_pLogMgr.CheckLevel(6))
                    //{
                    //    Console.WriteLine("CC Attack, Kick: " + m_pUserOBJ.pszIPAddr);
                    //}
                    //Misc.KickUser(m_pUserOBJ.nIPAddr);
                    //Succeed = false;
                    //return;
                }
            }
            bool success = false;
            byte[] tempBuff = userData.Body[2..^1];//跳过#....! 只保留消息内容
            int nDeCodeLen = 0;
            Span<byte> packBuff = EncryptUtil.DecodeSpan(tempBuff, userData.MsgLen - 3, ref nDeCodeLen);
            CommandMessage cltCmd = SerializerUtil.Deserialize<CommandMessage>(packBuff);
            switch (cltCmd.Ident)
            {
                case Messages.CM_QUERYCHR:
                case Messages.CM_NEWCHR:
                case Messages.CM_DELCHR:
                case Messages.CM_SELCHR:
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    ServerDataMessage accountPacket = new ServerDataMessage();
                    accountPacket.Data = userData.Body;
                    accountPacket.DataLen = (byte)userData.Body.Length;
                    accountPacket.Type = ServerDataType.Data;
                    accountPacket.SocketId = Session.SocketId;
                    _lastDbSvr.SendSocket(SerializerUtil.Serialize(accountPacket));
                    break;
                default:
                    LogService.Debug($"错误的数据包索引:[{cltCmd.Ident}]");
                    break;
            }
            if (!success)
            {
                //KickUser("ip");
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        public void HandleDelayMsg(ref bool success)
        {
            if (!_kickFlag)
            {
                if (HUtil32.GetTickCount() - _clientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    SendDefMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                    _kickFlag = true;
                    //BlockUser(this);
                    success = true;
                    LogService.Info($"Client Connect Time Out: {Session.ClientIP}");
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - _clientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    _serverService.CloseClient(Session.SocketId);
                    success = true;
                }
            }
        }

        /// <summary>
        /// 处理服务端发送过来的消息并发送到游戏客户端
        /// </summary>
        public void ProcessSvrData(byte[] sendData)
        {
            if (_kickFlag)
            {
                _kickFlag = false;
                _serverService.CloseClient(Session.SocketId);
                return;
            }
            _serverService.SendMessage(Session.SocketId, sendData);
        }

        private void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg)
        {
            int iLen = 0;
            CommandMessage Cmd;
            byte[] TempBuf = new byte[1048 - 1 + 1];
            byte[] SendBuf = new byte[1048 - 1 + 1];
            if ((_lastDbSvr == null) || !_lastDbSvr.IsConnected)
            {
                return;
            }
            Cmd = new CommandMessage();
            Cmd.Recog = nRecog;
            Cmd.Ident = wIdent;
            Cmd.Param = nParam;
            Cmd.Tag = nTag;
            Cmd.Series = nSeries;
            SendBuf[0] = (byte)'#';
            Array.Copy(SerializerUtil.Serialize(Cmd), 0, TempBuf, 0, CommandMessage.Size);
            if (!string.IsNullOrEmpty(sMsg))
            {
                byte[] sBuff = HUtil32.GetBytes(sMsg);
                Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
                iLen = EncryptUtil.Encode(TempBuf, CommandMessage.Size + sMsg.Length, SendBuf);
            }
            else
            {
                iLen = EncryptUtil.Encode(TempBuf, CommandMessage.Size, SendBuf);
            }
            SendBuf[iLen + 1] = (byte)'!';
            _serverService.SendMessage(Session.SocketId, SendBuf, iLen + 2);
        }

        /// <summary>
        /// 通知DBSvr有客户端链接
        /// </summary>
        public void UserEnter()
        {
            string sendStr = $"%K{_session.SocketId}/{_session.ClientIP}/{_session.ClientIP}$";
            byte[] body = HUtil32.GetBytes(sendStr);
            ServerDataMessage accountPacket = new ServerDataMessage();
            accountPacket.Data = body;
            accountPacket.DataLen = (short)body.Length;
            accountPacket.Type = ServerDataType.Enter;
            accountPacket.SocketId = Session.SocketId;
            _lastDbSvr.SendSocket(SerializerUtil.Serialize(accountPacket));
            LogService.Debug("[UserEnter] " + sendStr);
        }

        /// <summary>
        /// 通知DBSvr客户端断开链接
        /// </summary>
        public void UserLeave()
        {
            if (_session == null)
            {
                return;
            }
            string sendStr = $"%L{_session.SocketId}$";
            byte[] body = HUtil32.GetBytes(sendStr);
            ServerDataMessage accountPacket = new ServerDataMessage();
            accountPacket.Data = body;
            accountPacket.DataLen = (short)body.Length;
            accountPacket.Type = ServerDataType.Leave;
            accountPacket.SocketId = Session.SocketId;
            _lastDbSvr.SendSocket(SerializerUtil.Serialize(accountPacket));
            _kickFlag = false;
            LogService.Debug("[UserLeave] " + sendStr);
        }

        public void CloseSession()
        {
            _serverService.CloseClient(Session.SocketId);
        }
    }

    public enum TCheckStep
    {
        CheckLogin,
        SendCheck,
        SendSmu,
        SendFinsh,
        CheckTick
    }
}