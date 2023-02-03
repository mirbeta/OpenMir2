using LoginGate.Conf;
using LoginGate.Packet;
using System;
using System.Net.Sockets;
using NLog;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;

namespace LoginGate.Services
{
    /// <summary>
    /// 会话封包处理
    /// </summary>
    public class ClientSession
    {
        private readonly TSessionInfo _session;
        private readonly ClientThread _lastLoginSvr;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConfigManager _configManager;
        private bool m_KickFlag = false;
        private readonly int m_nSvrObject = 0;
        private int m_dwClientTimeOutTick = 0;

        public ClientSession(TSessionInfo session, ClientThread clientThread, ConfigManager configManager)
        {
            _session = session;
            _lastLoginSvr = clientThread;
            _configManager = configManager;
            m_dwClientTimeOutTick = HUtil32.GetTickCount();
        }

        private TSessionInfo Session => _session;

        private GateConfig Config => _configManager.GetConfig;

        public ClientThread ClientThread => _lastLoginSvr;

        /// <summary>
        /// 处理客户端消息封包
        /// 发送创建账号，修改密码，更新资料等到LoginSvr
        /// </summary>
        /// <param name="userData"></param>
        public void HandleClientPacket(MessageData userData)
        {
            if ((userData.MsgLen >= 5) && Config.DefenceCCPacket)
            {
                string sMsg = HUtil32.GetString(userData.Body, 2, userData.MsgLen - 3);
                if (sMsg.IndexOf("HTTP/", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    //if (g_pLogMgr.CheckLevel(6))
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
            var packBuff = EncryptUtil.DecodeSpan(tempBuff, userData.MsgLen - 3, ref nDeCodeLen);
            string sReviceMsg = HUtil32.GetString(userData.Body, 0, userData.Body.Length);
            if (!string.IsNullOrEmpty(sReviceMsg))
            {
                int nPos = sReviceMsg.IndexOf("*", StringComparison.OrdinalIgnoreCase);
                if (nPos > 0)
                {
                    string s10 = sReviceMsg[..(nPos - 1)];
                    string s1C = sReviceMsg.Substring(nPos, sReviceMsg.Length - nPos);
                    sReviceMsg = s10 + s1C;
                }
                if (!string.IsNullOrEmpty(sReviceMsg))
                {
                    ServerDataMessage accountPacket = new ServerDataMessage();
                    accountPacket.Data = userData.Body;
                    accountPacket.DataLen = (short)userData.Body.Length;
                    accountPacket.Type = ServerDataType.Data;
                    accountPacket.SocketId = Session.ConnectionId;
                    _lastLoginSvr.SendPacket(accountPacket);
                }
            }

            /*if (CltCmd.Cmd == Messages.CM_QUERYDYNCODE)
            { 
                m_dwClientTimeOutTick = HUtil32.GetTickCount();
            }*/
            //ClientMesaagePacket cltCmd = ClientPackage.ToPacket<ClientMesaagePacket>(packBuff);
            //switch (cltCmd.Cmd)
            //{
            //    case Messages.CM_ADDNEWUSER://注册账号
            //        m_dwClientTimeOutTick = HUtil32.GetTickCount();
            //        /*if (nDeCodeLen > TCmdPack.PackSize)
            //        {
            //            pszSendBuff = new byte[nDeCodeLen];
            //            Misc.EncodeBuf(userData.Body, nDeCodeLen, pszSendBuff);
            //            int nDeCodeLen = 0;
            //            byte[] packBuff = PacketEncoder.EncodeBuf(tempBuff, userData.MsgLen - 3, ref nDeCodeLen);
            //            var createAccount = new AccountPacket((int)Session.Socket.Handle, pszSendBuff);
            //            _lastLoginSvr.SendPacket(accountPacket);
            //        }*/
            //        break;
            //    case Messages.CM_IDPASSWORD://登录消息
            //        m_dwClientTimeOutTick = HUtil32.GetTickCount();
            //        //pszSendBuff = new byte[nDeCodeLen];
            //        //Misc.EncodeBuf(userData.Body, nDeCodeLen, pszSendBuff);
            //        //accountPacket = new AccountPacket((int)Session.Socket.Handle, pszSendBuff);
            //        //lastGameSvr.SendBuffer(accountPacket.GetPacket());
            //        break;
            //    case Messages.CM_PROTOCOL:
            //    case Messages.CM_SELECTSERVER:
            //    case Messages.CM_CHANGEPASSWORD:
            //    case Messages.CM_UPDATEUSER:
            //    case Messages.CM_GETBACKPASSWORD:
            //        m_dwClientTimeOutTick = HUtil32.GetTickCount();
            //        //pszSendBuff = new byte[nDeCodeLen];
            //        //Misc.EncodeBuf(userData.Body, nDeCodeLen, pszSendBuff);
            //        //accountPacket = new AccountPacket((int)Session.Socket.Handle, pszSendBuff);
            //        //lastGameSvr.SendBuffer(accountPacket.GetPacket());
            //        break;
            //    default:
            //        Console.WriteLine($"错误的数据包索引:[{cltCmd.Cmd}]");
            //        break;
            //}

            //if (!success)
            //{
            //    //KickUser("ip");
            //}
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        public void HandleDelayMsg(ref bool success)
        {
            if (!m_KickFlag)
            {
                if (HUtil32.GetTickCount() - m_dwClientTimeOutTick > Config.ClientTimeOutTime)
                {
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    if (_session.Socket == null || _session.Socket.Handle == IntPtr.Zero)
                    {
                        return;
                    }
                    SendDefMessage(Messages.SM_OUTOFCONNECTION, m_nSvrObject, 0, 0, 0);
                    m_KickFlag = true;
                    //BlockUser(this);
                    _logger.Debug($"Client Connect TimeOut: {Session.ClientIP}");
                    success = true;
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - m_dwClientTimeOutTick > Config.ClientTimeOutTime)
                {
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    success = true;
                }
            }
        }

        /// <summary>
        /// 处理服务端发送过来的消息并发送到游戏客户端
        /// </summary>
        public void ProcessSvrData(byte[] sendData)
        {
            if (m_KickFlag)
            {
                m_KickFlag = false;
                _session.Socket.Close();
                return;
            }
            if (_session.Socket != null && _session.Socket.Connected)
            {
                _session.Socket.Send(sendData);
            }
            else
            {
                _logger.Info("Scoket会话失效，无法处理登陆封包", 5);
            }
        }

        private void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg = "")
        {
            int iLen = 0;
            byte[] sendBuf = new byte[1024];
            if ((_lastLoginSvr == null) || !_lastLoginSvr.IsConnected)
            {
                return;
            }
            CommandPacket cmd = new CommandPacket();
            cmd.Recog = nRecog;
            cmd.Ident = wIdent;
            cmd.Param = nParam;
            cmd.Tag = nTag;
            cmd.Series = nSeries;
            sendBuf[0] = (byte)'#';
            byte[] tempBuf = null;
            if (!string.IsNullOrEmpty(sMsg))
            {
                byte[] sBuff = HUtil32.GetBytes(sMsg);
                tempBuf = new byte[CommandPacket.Size + sBuff.Length];
                Array.Copy(sBuff, 0, tempBuf, 13, sBuff.Length);
                iLen = EncryptUtil.Encode(tempBuf, CommandPacket.Size + sMsg.Length, sendBuf);
            }
            else
            {
                tempBuf = SerializerUtil.Serialize(cmd);
                iLen = EncryptUtil.Encode(tempBuf, CommandPacket.Size, sendBuf, 1);
            }
            sendBuf[iLen + 1] = (byte)'!';
            _session.Socket.Send(sendBuf, iLen, SocketFlags.None);
        }

        /// <summary>
        /// 通知LoginSvr有客户端链接
        /// </summary>
        public void UserEnter()
        {
            byte[] body = HUtil32.GetBytes($"{_session.ClientIP}/{_session.ClientIP}");
            ServerDataMessage accountPacket = new ServerDataMessage();
            accountPacket.Data = body;
            accountPacket.DataLen = (byte)body.Length;
            accountPacket.Type = ServerDataType.Enter;
            accountPacket.SocketId = Session.ConnectionId;
            _lastLoginSvr.SendPacket(accountPacket);
        }

        /// <summary>
        /// 通知LoginSvr客户端断开链接
        /// </summary>
        public void UserLeave()
        {
            if (Session == null || Session.Socket == null)
            {
                return;
            }
            ServerDataMessage accountPacket = new ServerDataMessage();
            accountPacket.Data = Array.Empty<byte>();
            accountPacket.DataLen = 0;
            accountPacket.Type = ServerDataType.Leave;
            accountPacket.SocketId = Session.ConnectionId;
            _lastLoginSvr.SendPacket(accountPacket);
            m_KickFlag = false;
        }

        public void CloseSession()
        {
            if (_session.Socket == null)
            {
                _logger.Debug($"会话[{_session.ConnectionId}]已经关闭");
                return;
            }
            _session.Socket.Close();
        }
    }

    public class TSessionInfo
    {
        public Socket Socket;
        public int ConnectionId;
        public int ReceiveTick;
        public string sAccount;
        public string sChrName;
        public string ClientIP;
    }
}