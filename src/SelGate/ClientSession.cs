using SelGate.Conf;
using SelGate.Services;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using SystemModule;

namespace SelGate
{
    /// <summary>
    /// 会话封包处理
    /// </summary>
    public class ClientSession
    {
        private TSessionInfo _session;
        private bool _KickFlag = false;
        private int _nSvrObject = 0;
        private int _clientTimeOutTick = 0;
        private readonly ClientThread _lastDBSvr;
        private readonly ConfigManager _configManager;

        public ClientSession(ConfigManager configManager, TSessionInfo session, ClientThread clientThread)
        {
            _session = session;
            _lastDBSvr = clientThread;
            _configManager = configManager;
            _clientTimeOutTick = HUtil32.GetTickCount();
        }

        public TSessionInfo Session => _session;

        private GateConfig Config => _configManager.GateConfig;

        public ClientThread ClientThread => _lastDBSvr;

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
            var packBuff = Misc.DecodeBuf(tempBuff, userData.MsgLen - 3, ref nDeCodeLen);
            var CltCmd = Packets.ToPacket<ClientPacket>(packBuff);
            switch (CltCmd.Cmd)
            {
                case Grobal2.CM_QUERYCHR:
                case Grobal2.CM_NEWCHR:
                case Grobal2.CM_DELCHR:
                case Grobal2.CM_SELCHR:
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    var sendStr = $"%A{(int)_session.Socket.Handle}/{HUtil32.GetString(userData.Body, 0, userData.MsgLen)}$";//todo 待优化
                    _lastDBSvr.SendData(sendStr);
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
            if (!_KickFlag)
            {
                if (HUtil32.GetTickCount() - _clientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    _clientTimeOutTick = HUtil32.GetTickCount();
                    SendDefMessage(Grobal2.SM_OUTOFCONNECTION, _nSvrObject, 0, 0, 0, "");
                    _KickFlag = true;
                    //BlockUser(this);
                    success = true;
                    Debug.WriteLine($"Client Connect Time Out: {Session.ClientIP}");
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
            if (_KickFlag)
            {
                _KickFlag = false;
                _session.Socket.Close();
                return;
            }
            if (_session.Socket != null && _session.Socket.Connected)
            {
                _session.Socket.Send(sendData.Body);
            }
            else
            {
                Console.WriteLine("Scoket会话失效，无法处理登陆封包");
            }
        }

        private void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg)
        {
            int iLen = 0;
            ClientPacket Cmd;
            byte[] TempBuf = new byte[1048 - 1 + 1];
            byte[] SendBuf = new byte[1048 - 1 + 1];
            if ((_lastDBSvr == null) || !_lastDBSvr.IsConnected)
            {
                return;
            }
            Cmd = new ClientPacket();
            Cmd.Recog = nRecog;
            Cmd.Ident = wIdent;
            Cmd.Param = nParam;
            Cmd.Tag = nTag;
            Cmd.Series = nSeries;
            SendBuf[0] = (byte)'#';
            Array.Copy(Cmd.GetBuffer(), 0, TempBuf, 0, ClientPacket.PackSize);
            if (!string.IsNullOrEmpty(sMsg))
            {
                var sBuff = HUtil32.GetBytes(sMsg);
                Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
                iLen = Misc.EncodeBuf(TempBuf, ClientPacket.PackSize + sMsg.Length, SendBuf);
            }
            else
            {
                iLen = Misc.EncodeBuf(TempBuf, ClientPacket.PackSize, SendBuf);
            }
            SendBuf[iLen + 1] = (byte)'!';
            _session.Socket.Send(SendBuf, iLen + 2, SocketFlags.None);
        }

        /// <summary>
        /// 通知DBSvr有客户端链接
        /// </summary>
        public void UserEnter()
        {
            var sendStr = $"%K{(int)_session.Socket.Handle}/{_session.ClientIP}/{_session.ClientIP}$";
            _lastDBSvr.SendData(sendStr);
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
            var szSenfBuf = $"%L{(int)_session.Socket.Handle}$";
            _lastDBSvr.SendData(szSenfBuf);
            _KickFlag = false;
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