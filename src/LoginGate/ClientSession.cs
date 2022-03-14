using LoginGate.Conf;
using LoginGate.Package;
using LoginGate.Services;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using SystemModule;

namespace LoginGate
{
    /// <summary>
    /// 会话封包处理
    /// </summary>
    public class ClientSession
    {
        private TSessionInfo _session;
        private bool m_KickFlag = false;
        private int m_nSvrObject = 0;
        private int m_dwClientTimeOutTick = 0;
        private readonly ClientThread _lastGameSvr;
        private readonly ConfigManager _configManager;

        public ClientSession(ConfigManager configManager, TSessionInfo session, ClientThread clientThread)
        {
            _session = session;
            _lastGameSvr = clientThread;
            _configManager = configManager;
            m_dwClientTimeOutTick = HUtil32.GetTickCount();
        }

        public TSessionInfo Session => _session;

        private GateConfig Config => _configManager.GateConfig;

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
                    //if (g_pLogMgr.CheckLevel(6))
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
            var packBuff = Misc.DecodeBuf(tempBuff, userData.MsgLen - 3, ref nDeCodeLen);
            var CltCmd = new TCmdPack(packBuff);
            var sReviceMsg = HUtil32.GetString(userData.Body, 0, userData.Body.Length);
            if (!string.IsNullOrEmpty(sReviceMsg))
            {
                int nPos = sReviceMsg.IndexOf("*");
                if (nPos > 0)
                {
                    string s10 = sReviceMsg.Substring(0, nPos - 1);
                    string s1C = sReviceMsg.Substring(nPos, sReviceMsg.Length - nPos);
                    sReviceMsg = s10 + s1C;
                }
                if (!string.IsNullOrEmpty(sReviceMsg))
                {
                    AccountPacket accountPacket = new AccountPacket(AccountPacktType.Data, (int)Session.Socket.Handle, userData.Body);
                    _lastGameSvr.SendBuffer(accountPacket.GetPacket());
                }
            }

            /*if (CltCmd.Cmd == Grobal2.CM_QUERYDYNCODE)
            { 
                m_dwClientTimeOutTick = HUtil32.GetTickCount();
            }*/
            switch (CltCmd.Cmd)
            {
                case Grobal2.CM_IDPASSWORD: //登录消息
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    //pszSendBuff = new byte[nDeCodeLen];
                    //Misc.EncodeBuf(userData.Body, nDeCodeLen, pszSendBuff);
                    //accountPacket = new AccountPacket((int)Session.Socket.Handle, pszSendBuff);
                    //lastGameSvr.SendBuffer(accountPacket.GetPacket());
                    break;
                case Grobal2.CM_PROTOCOL:
                case Grobal2.CM_SELECTSERVER:
                case Grobal2.CM_CHANGEPASSWORD:
                case Grobal2.CM_UPDATEUSER:
                case Grobal2.CM_GETBACKPASSWORD:
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    //pszSendBuff = new byte[nDeCodeLen];
                    //Misc.EncodeBuf(userData.Body, nDeCodeLen, pszSendBuff);
                    //accountPacket = new AccountPacket((int)Session.Socket.Handle, pszSendBuff);
                    //lastGameSvr.SendBuffer(accountPacket.GetPacket());
                    break;
                case Grobal2.CM_ADDNEWUSER: //注册账号
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    //if (nDeCodeLen > TCmdPack.PackSize)
                    //{
                    //    pszSendBuff = new byte[nDeCodeLen];
                    //    Misc.EncodeBuf(userData.Body, nDeCodeLen, pszSendBuff);
                    //    accountPacket = new AccountPacket((int)Session.Socket.Handle, pszSendBuff);
                    //    lastGameSvr.SendBuffer(accountPacket.GetPacket());
                    //}
                    break;
                default:
                    Console.WriteLine($"错误的数据包索引:[{CltCmd.Cmd}]");
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
            if (!m_KickFlag)
            {
                if (HUtil32.GetTickCount() - m_dwClientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    if (_session.Socket == null || _session.Socket.Handle == IntPtr.Zero)
                    {
                        return;
                    }
                    SendDefMessage(Grobal2.SM_OUTOFCONNECTION, m_nSvrObject, 0, 0, 0);
                    m_KickFlag = true;
                    //BlockUser(this);
                    Debug.WriteLine($"Client Connect Time Out: {Session.ClientIP}");
                    success = true;
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - m_dwClientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
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
            if (_session.Socket != null && _session.Socket.Connected)
            {
                _session.Socket.Send(sendData.Body);
            }
        }

        private void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg = "")
        {
            int iLen = 0;
            TCmdPack Cmd;
            byte[] SendBuf = new byte[1024];
            if ((_lastGameSvr == null) || !_lastGameSvr.IsConnected)
            {
                return;
            }
            Cmd = new TCmdPack();
            Cmd.Recog = nRecog;
            Cmd.Ident = wIdent;
            Cmd.Param = nParam;
            Cmd.Tag = nTag;
            Cmd.Series = nSeries;
            SendBuf[0] = (byte)'#';
            byte[] TempBuf = null;
            if (!string.IsNullOrEmpty(sMsg))
            {
                var sBuff = HUtil32.GetBytes(sMsg);
                TempBuf = new byte[TCmdPack.PackSize + sBuff.Length];
                Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
                iLen = Misc.EncodeBuf(TempBuf, TCmdPack.PackSize + sMsg.Length, SendBuf);
            }
            else
            {
                TempBuf = Cmd.GetPacket();
                iLen = Misc.EncodeBuf(TempBuf, TCmdPack.PackSize, SendBuf, 1);
            }
            SendBuf[iLen + 1] = (byte)'!';
            _session.Socket.Send(SendBuf, iLen, SocketFlags.None);
        }

        /// <summary>
        /// 通知LoginSvr有客户端链接
        /// </summary>
        public void UserEnter()
        {
            var str = $"{_session.ClientIP}/{_session.ClientIP}";
            var accountPacket = new AccountPacket(AccountPacktType.Open, (int)Session.Socket.Handle, str);
            _lastGameSvr.SendBuffer(accountPacket.GetPacket());
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
            var accountPacket = new AccountPacket(AccountPacktType.Leave, (int)Session.Socket.Handle, "");
            _lastGameSvr.SendBuffer(accountPacket.GetPacket());
            m_KickFlag = false;
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

    public class TSessionInfo
    {
        public Socket Socket;
        public int SocketId;
        public int dwReceiveTick;
        public string sAccount;
        public string sChrName;
        public string ClientIP;
    }
}