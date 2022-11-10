using SelGate.Conf;
using SelGate.Package;
using SelGate.Services;
using System;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace SelGate
{
    /// <summary>
    /// 会话封包处理
    /// </summary>
    public class ClientSession
    {
        private readonly TSessionInfo _session;
        private bool _kickFlag = false;
        private int _clientTimeOutTick = 0;
        private readonly MirLogger _logger;
        private readonly ClientThread _lastDbSvr;
        private readonly ConfigManager _configManager;

        public ClientSession(MirLogger logger, ConfigManager configManager, TSessionInfo session, ClientThread clientThread)
        {
            _logger = logger;
            _session = session;
            _lastDbSvr = clientThread;
            _configManager = configManager;
            _clientTimeOutTick = HUtil32.GetTickCount();
        }

        private TSessionInfo Session => _session;

        private GateConfig Config => _configManager.GateConfig;

        public ClientThread ClientThread => _lastDbSvr;

        /// <summary>
        /// 处理客户端发送过来的封包
        /// 发送创建角色，删除角色，恢复角色，创建名字等功能
        /// </summary>
        /// <param name="userData"></param>
        public void HandleUserPacket(TMessageData userData)
        {
            if ((userData.MsgLen >= 5) && Config.m_fDefenceCCPacket)
            {
                var sMsg = HUtil32.GetString(userData.Body, 2, userData.MsgLen - 3);
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
            var success = false;
            var tempBuff = userData.Body[2..^1];//跳过#....! 只保留消息内容
            var nDeCodeLen = 0;
            var packBuff = PacketEncoder.DecodeBuf(tempBuff, userData.MsgLen - 3, ref nDeCodeLen);
            var cltCmd = Packets.ToPacket<ClientMesaagePacket>(packBuff);
            if (cltCmd == null)
            {
                return;
            }
            switch (cltCmd.Cmd)
            {
                case Grobal2.CM_QUERYCHR:
                case Grobal2.CM_NEWCHR:
                case Grobal2.CM_DELCHR:
                case Grobal2.CM_SELCHR:
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    var accountPacket = new ServerDataMessage();
                    accountPacket.Body = userData.Body;
                    accountPacket.BuffLen = (byte)userData.Body.Length;
                    accountPacket.StartChar = '%';
                    accountPacket.Type = ServerDataType.Data;
                    accountPacket.SocketId = Session.SocketId;
                    accountPacket.EndChar = '$';
                    _lastDbSvr.SendSocket(accountPacket.GetBuffer());
                    break;
                default:
                    _logger.DebugLog($"错误的数据包索引:[{cltCmd.Cmd}]");
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
                    SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                    _kickFlag = true;
                    //BlockUser(this);
                    success = true;
                    _logger.DebugLog($"Client Connect Time Out: {Session.ClientIP}");
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - _clientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    _session.Socket.Close();
                    success = true;
                }
            }
        }

        /// <summary>
        /// 处理服务端发送过来的消息并发送到游戏客户端
        /// </summary>
        public void ProcessSvrData(TMessageData sendData)
        {
            if (_kickFlag)
            {
                _kickFlag = false;
                _session.Socket.Close();
                return;
            }
            if (_session.Socket != null && _session.Socket.Connected)
            {
                var sendLen = _session.Socket.Send(sendData.Body);
                if (sendLen <= 0)
                {
                    _logger.LogWarning("发送人物数据包失败.");
                }
            }
            else
            {
                _logger.DebugLog("Scoket会话失效，无法处理登陆封包");
            }
        }

        private void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg)
        {
            int iLen = 0;
            ClientMesaagePacket Cmd;
            byte[] TempBuf = new byte[1048 - 1 + 1];
            byte[] SendBuf = new byte[1048 - 1 + 1];
            if ((_lastDbSvr == null) || !_lastDbSvr.IsConnected)
            {
                return;
            }
            Cmd = new ClientMesaagePacket();
            Cmd.Recog = nRecog;
            Cmd.Ident = wIdent;
            Cmd.Param = nParam;
            Cmd.Tag = nTag;
            Cmd.Series = nSeries;
            SendBuf[0] = (byte)'#';
            Array.Copy(Cmd.GetBuffer(), 0, TempBuf, 0, ClientMesaagePacket.PackSize);
            if (!string.IsNullOrEmpty(sMsg))
            {
                var sBuff = HUtil32.GetBytes(sMsg);
                Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
                iLen = PacketEncoder.EncodeBuf(TempBuf, ClientMesaagePacket.PackSize + sMsg.Length, SendBuf);
            }
            else
            {
                iLen = PacketEncoder.EncodeBuf(TempBuf, ClientMesaagePacket.PackSize, SendBuf);
            }
            SendBuf[iLen + 1] = (byte)'!';
            _session.Socket.Send(SendBuf, iLen + 2, SocketFlags.None);
        }

        /// <summary>
        /// 通知DBSvr有客户端链接
        /// </summary>
        public void UserEnter()
        {
            var sendStr = $"%K{_session.SocketId}/{_session.ClientIP}/{_session.ClientIP}$";
            var body = HUtil32.GetBytes(sendStr);
            var accountPacket = new ServerDataMessage();
            accountPacket.Body = body;
            accountPacket.BuffLen = (short)body.Length;
            accountPacket.StartChar = '%';
            accountPacket.Type = ServerDataType.Enter;
            accountPacket.SocketId = Session.SocketId;
            accountPacket.EndChar = '$';
            _lastDbSvr.SendSocket(accountPacket.GetBuffer());
            _logger.DebugLog("[UserEnter] " + sendStr);
        }

        /// <summary>
        /// 通知DBSvr客户端断开链接
        /// </summary>
        public void UserLeave()
        {
            if (_session == null || _session.Socket == null)
            {
                return;
            }
            var sendStr = $"%L{_session.SocketId}$";
            var body = HUtil32.GetBytes(sendStr);
            var accountPacket = new ServerDataMessage();
            accountPacket.Body = body;
            accountPacket.BuffLen = (short)body.Length;
            accountPacket.StartChar = '%';
            accountPacket.Type = ServerDataType.Leave;
            accountPacket.SocketId = Session.SocketId;
            accountPacket.EndChar = '$';
            _lastDbSvr.SendSocket(accountPacket.GetBuffer());
            _kickFlag = false;
            _logger.DebugLog("[UserLeave] " + sendStr);
        }

        public void CloseSession()
        {
            _session.Socket.Close();
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