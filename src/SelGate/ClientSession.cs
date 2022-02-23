using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net.Sockets;
using SelGate.Conf;
using SelGate.Services;
using SystemModule;
using SystemModule.Packages;

namespace SelGate
{
    /// <summary>
    /// 用户会话封包处理
    /// </summary>
    public class ClientSession
    {
        private int LastDirection = -1;
        private byte _handleLogin = 0;
        private TSessionInfo _session;
        private bool m_fOverClientCount;
        private bool m_KickFlag = false;
        public int m_nSvrListIdx = 0;
        private int m_nSvrObject = 0;
        private int m_SendCheckTick = 0;
        private TCheckStep m_Stat;
        private long m_FinishTick = 0;
        private int m_dwClientTimeOutTick = 0;
        private readonly ClientThread lastGameSvr;
        private readonly ConfigManager _configManager;

        public ClientSession(ConfigManager configManager, TSessionInfo session, ClientThread clientThread)
        {
            _session = session;
            lastGameSvr = clientThread;
            _configManager = configManager;
            m_fOverClientCount = false;
            m_Stat = TCheckStep.CheckLogin;
        }

        public TSessionInfo Session => _session;

        private GateConfig Config => _configManager.GateConfig;

        /// <summary>
        /// 处理客户端发送过来的封包
        /// 发送创建角色，删除角色，恢复角色，创建名字等功能
        /// </summary>
        /// <param name="UserData"></param>
        public void HandleUserPacket(TSendUserData UserData)
        {
            int dwCurrentTick = 0;
            if (m_KickFlag)
            {
                m_KickFlag = false;
                return;
            }
            if ((UserData.BufferLen >= 5) && Config.m_fDefenceCCPacket)
            {
                var sMsg = HUtil32.GetString(UserData.Buffer, 2, UserData.BufferLen - 3);
                if (sMsg.IndexOf("HTTP/", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    //if (LogManager.Units.LogManager.g_pLogMgr.CheckLevel(6))
                    //{
                    //    Console.WriteLine("CC Attack, Kick: " + m_pUserOBJ.pszIPAddr);
                    //}
                    //Misc.KickUser(m_pUserOBJ.nIPAddr);
                    //Succeed = false;
                    //return;
                }
            }
            if ((m_Stat == TCheckStep.CheckLogin) || (m_Stat == TCheckStep.SendCheck))
            {
                dwCurrentTick = HUtil32.GetTickCount();
                if (0 == m_SendCheckTick)
                {
                    m_SendCheckTick = dwCurrentTick;
                }
                if ((dwCurrentTick - m_SendCheckTick) > 1000 * 5)// 如果5 秒 没有回数据 就下发数据
                {
                    m_Stat = TCheckStep.SendSmu;
                }
            }
            // 如果下发成功  得多少秒有数据如果没有的话，那就是有问题
            if ((m_Stat == TCheckStep.SendFinsh))
            {
                dwCurrentTick = HUtil32.GetTickCount();
                if ((dwCurrentTick - m_FinishTick) > 1000 * 10)
                {
                    //todo 
                }
            }
            var success = false;//todo 优化此段代码
            var packBuff = new byte[UserData.BufferLen];
            var tempBuff = new byte[UserData.BufferLen - 3];//跳过#1....!
            Buffer.BlockCopy(UserData.Buffer, 2, tempBuff, 0, tempBuff.Length);
            var nDeCodeLen = Misc.DecodeBuf(tempBuff, UserData.BufferLen - 3, ref packBuff);
            var CltCmd = new TCmdPack(packBuff, nDeCodeLen);
            if (_handleLogin == 0)
            {
                HandleLogin(CltCmd, UserData.Buffer, nDeCodeLen, ref success);
                if (!success)
                {
                    //KickUser("ip");
                }
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        public void HandleDelayMsg()
        {
            if (!m_KickFlag)
            {
                if(_handleLogin<3)
                {
                    if (HUtil32.GetTickCount() - m_dwClientTimeOutTick > Config.m_nClientTimeOutTime)
                    {
                        m_dwClientTimeOutTick = HUtil32.GetTickCount();
                        SendDefMessage(Grobal2.SM_OUTOFCONNECTION, m_nSvrObject, 0, 0, 0, "");
                        m_KickFlag = true;
                        //BlockUser(this);
                    }
                }
            }
            else
            {
                if (HUtil32.GetTickCount() - m_dwClientTimeOutTick > Config.m_nClientTimeOutTime)
                {
                    m_dwClientTimeOutTick =HUtil32.GetTickCount();
                    _session.Socket.Close();
                }
            }
        }

        /// <summary>
        /// 处理服务端发送过来的消息并发送到游戏客户端
        /// </summary>
        public void ProcessSvrData(TSendUserData sendData)
        {
            var sSendMsg = string.Empty;
            switch (sendData.BufferLen)
            {
                case < 0:
                    sSendMsg = "#" + HUtil32.GetString(sendData.Buffer, 0, sendData.Buffer.Length - 1) + "!";
                    break;
                case >= 12:
                {
                    var pDefMsg = new TDefaultMessage(sendData.Buffer);
                    if (sendData.BufferLen > 12)
                    {
                        var sb = new System.Text.StringBuilder();
                        sb.Append('#');
                        sb.Append(EDcode.EncodeMessage(pDefMsg));
                        var strBuff = new byte[sendData.BufferLen - 12];
                        Buffer.BlockCopy(sendData.Buffer, 12, strBuff, 0, strBuff.Length);
                        sb.Append(HUtil32.StrPasTest(strBuff));
                        sb.Append('!');
                        sSendMsg = sb.ToString();
                    }
                    else
                    {
                        sSendMsg = "#" + EDcode.EncodeMessage(pDefMsg) + "!";
                    }
                    break;
                }
            }
            if (_session.Socket != null && _session.Socket.Connected)
            {
                _session.Socket.Send(HUtil32.GetBytes(sSendMsg));
            }
        }

        private void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg)
        {
            int iLen = 0;
            TCmdPack Cmd;
            byte[] TempBuf = new byte[1048 - 1 + 1];
            byte[] SendBuf = new byte[1048 - 1 + 1];
            if ((lastGameSvr == null) || !lastGameSvr.IsConnected)
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
            //Move(Cmd, TempBuf[1], TCmdPack.PackSize);
            Array.Copy(Cmd.GetPacket(6), 0, TempBuf, 0, TCmdPack.PackSize);
            if (!string.IsNullOrEmpty(sMsg))
            {
                //Move(sMsg[1], TempBuf[TCmdPack.PackSize + 1], sMsg.Length);
                var sBuff = HUtil32.GetBytes(sMsg);
                Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
                iLen = Misc.EncodeBuf(TempBuf, TCmdPack.PackSize + sMsg.Length, SendBuf);
            }
            else
            {
                iLen = Misc.EncodeBuf(TempBuf, TCmdPack.PackSize, SendBuf);
            }
            SendBuf[iLen + 1] = (byte)'!';
            _session.Socket.Send(SendBuf, iLen + 2, SocketFlags.None);
        }

        /// <summary>
        /// 处理登录数据
        /// </summary>
        private void HandleLogin(TCmdPack ctlCmd,byte[] buffer, int nDeCodeLen, ref bool success)
        {
            switch (ctlCmd.Cmd)
            {
                //case Grobal2.CM_QUERYSELCHARCODE:
                case Grobal2.CM_QUERYCHR:
                case Grobal2.CM_NEWCHR:
                case Grobal2.CM_DELCHR:
                case Grobal2.CM_SELCHR:
                //case Grobal2.CM_QUERYDELCHR:
                //case Grobal2.CM_GETBACKDELCHR:
                    m_dwClientTimeOutTick = HUtil32.GetTickCount();
                    var tempBuff = new byte[nDeCodeLen];
                    var nEnCodeLen = Misc.EncodeBuf(buffer, nDeCodeLen, tempBuff, 1);
                    //StrFmt函数 必须保证格式化的字符串是AnsiString 否则消息错误.
                    tempBuff[0] = (byte)'%';
                    //StrFmt(@pszBuf[1], AnsiString('>%d/#1%s!$'), [m_pUserOBJ._SendObj.Socket, PAnsiChar(nBBuf)]);
                    Array.Copy(buffer, nDeCodeLen, tempBuff, 1, tempBuff.Length - 1);
                    lastGameSvr.SendBuffer(tempBuff, tempBuff.Length);
                    break;
                default:
                    Console.WriteLine($"错误的数据包索引:[{ctlCmd.Cmd}]");
                    break;
            }
        }

        private void SendSysMsg(string szMsg)
        {
            TCmdPack Cmd;
            byte[] TempBuf = new byte[1023 + 1];
            byte[] SendBuf = new byte[1023 + 1];
            if ((lastGameSvr == null) || !lastGameSvr.IsConnected)
            {
                return;
            }
            Cmd = new TCmdPack();
            Cmd.UID = m_nSvrObject;
            Cmd.Cmd = Grobal2.SM_SYSMESSAGE;
            Cmd.X = HUtil32.MakeWord(0xFF, 0xF9);
            Cmd.Y = 0;
            Cmd.Direct = 0;
            SendBuf[0] = (byte)'#';
            //Move(Cmd, TempBuf[1], TCmdPack.PackSize);
            Array.Copy(Cmd.GetPacket(0), 0, TempBuf, 0, TCmdPack.PackSize);
            var sBuff = HUtil32.GetBytes(szMsg);
            Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
            //Move(szMsg[1], TempBuf[13], szMsg.Length);
            var iLen = TCmdPack.PackSize + szMsg.Length;
            iLen = Misc.EncodeBuf(TempBuf, iLen, SendBuf);
            SendBuf[iLen + 1] = (byte)'!';
            //m_tIOCPSender.SendData(m_pOverlapSend, SendBuf[0], iLen + 2);
            _session.Socket.Send(SendBuf, iLen + 2, SocketFlags.None);
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

    public class GameSpeed
    {
        /// <summary>
        /// 是否速度限制
        /// </summary>
        public bool SpeedLimit = false;
        /// <summary>
        /// 最高的人物身上所有装备+速度，默认6。
        /// </summary>
        public int ItemSpeed = 0;
        /// <summary>
        /// 玩家加速度装备因数，数值越小，封加速越严厉，默认60。
        /// </summary>
        public int DefItemSpeed;
        /// <summary>
        /// 加速的累计值
        /// </summary>
        public int nErrorCount;
        /// <summary>
        /// 交易时间
        /// </summary>
        public int dwDealTick;
        /// <summary>
        /// 装备加速
        /// </summary>
        public int m_nHitSpeed;
        /// <summary>
        /// 发言时间
        /// </summary>
        public int dwSayMsgTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public int dwMoveTick;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int dwAttackTick;
        /// <summary>
        /// 魔法时间
        /// </summary>
        public int dwSpellTick;
        /// <summary>
        /// 走路时间
        /// </summary>
        public long dwWalkTick;
        /// <summary>
        /// 跑步时间
        /// </summary>
        public long dwRunTick;
        /// <summary>
        /// 转身时间
        /// </summary>
        public long dwTurnTick;
        /// <summary>
        /// 挖肉时间
        /// </summary>
        public int dwButchTick;
        /// <summary>
        /// 蹲下时间
        /// </summary>
        public int dwSitDownTick;
        /// <summary>
        /// 吃药时间
        /// </summary>
        public long dwEatTick;
        /// <summary>
        /// 捡起时间
        /// </summary>
        public long dwPickupTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public long dwRunWalkTick;
        /// <summary>
        /// 传送时间
        /// </summary>
        public long dwFeiDnItemsTick;
        /// <summary>
        /// 变速齿轮时间
        /// </summary>
        public long dwSupSpeederTick;
        /// <summary>
        /// 变速齿轮累计
        /// </summary>
        public int dwSupSpeederCount;
        /// <summary>
        /// 超级加速时间
        /// </summary>
        public long dwSuperNeverTick;
        /// <summary>
        /// 超级加速累计
        /// </summary>
        public int dwSuperNeverCount;
        /// <summary>
        /// 记录上一次操作
        /// </summary>
        public int dwUserDoTick;
        /// <summary>
        /// 保存停顿操作时间
        /// </summary>
        public long dwContinueTick;
        /// <summary>
        /// 带有攻击并发累计
        /// </summary>
        public int dwConHitMaxCount;
        /// <summary>
        /// 带有魔法并发累计
        /// </summary>
        public int dwConSpellMaxCount;
        /// <summary>
        /// 记录上一次移动方向
        /// </summary>
        public int dwCombinationTick;
        /// <summary>
        /// 智能攻击累计
        /// </summary>
        public int dwCombinationCount;
        public long dwGameTick;
        public int dwWaringTick;

        public GameSpeed()
        {
            var dwCurrentTick = HUtil32.GetTickCount();
            nErrorCount = dwCurrentTick;
            dwDealTick = dwCurrentTick;
            m_nHitSpeed = dwCurrentTick;
            dwSayMsgTick = dwCurrentTick;
            dwMoveTick = dwCurrentTick;
            dwAttackTick = dwCurrentTick;
            dwSpellTick = dwCurrentTick;
            dwWalkTick = dwCurrentTick;
            dwRunTick = dwCurrentTick;
            dwTurnTick = dwCurrentTick;
            dwButchTick = dwCurrentTick;
            dwSitDownTick = dwCurrentTick;
            dwEatTick = dwCurrentTick;
            dwPickupTick = dwCurrentTick;
            dwRunWalkTick = dwCurrentTick;
            dwFeiDnItemsTick = dwCurrentTick;
            dwSupSpeederTick = dwCurrentTick;
            dwSupSpeederCount = dwCurrentTick;
            dwSuperNeverTick = dwCurrentTick;
            dwSuperNeverCount = dwCurrentTick;
            dwUserDoTick = dwCurrentTick;
            dwContinueTick = dwCurrentTick;
            dwConHitMaxCount = dwCurrentTick;
            dwConSpellMaxCount = dwCurrentTick;
            dwCombinationTick = dwCurrentTick;
            dwCombinationCount = dwCurrentTick;
            dwGameTick = dwCurrentTick;
            dwWaringTick = dwCurrentTick;
        }
    }
}