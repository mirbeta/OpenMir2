using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace GameGate
{
    /// <summary>
    /// 用户会话封包处理
    /// </summary>
    public class ClientSession
    {
        private GameSpeed _gameSpeed;
        private int nSpeedCount = 0;
        private int mSpeedCount = 0;
        /// <summary>
        /// 最高的人物身上所有装备+速度，默认6。
        /// </summary>
        private int nItemSpeed = 0;
        /// <summary>
        /// 玩家加速度装备因数，数值越小，封加速越严厉，默认60。
        /// </summary>
        private int DefItemSpeed;
        private int LastDirection = -1;
        private IList<TDelayMsg> _msgList;
        private byte _handleLogin = 0;
        private bool _SpeedLimit;
        private int _sessionIdx;
        private TSessionInfo _session;
        public string sSendData = string.Empty;
        private bool m_fOverClientCount;
        private byte[] m_xHWID;
        public bool m_fKickFlag = false;
        public bool m_fSpeedLimit = false;
        public int m_dwSessionID = 0;
        public int m_nSvrListIdx = 0;
        public int m_nSvrObject = 0;
        public int m_nChrStutas = 0;
        public int m_nItemSpeed = 0;
        public int m_nDefItemSpeed = 0;
        public int m_dwChatTick = 0;
        public int m_dwLastDirection = 0;
        public int m_dwEatTick = 0;
        public int m_dwHeroEatTick = 0;
        public int m_dwPickupTick = 0;
        public int m_dwMoveTick = 0;
        public int m_dwAttackTick = 0;
        public int m_dwSpellTick = 0;
        public int m_dwSitDownTick = 0;
        public int m_dwTurnTick = 0;
        public int m_dwButchTick = 0;
        public int m_dwDealTick = 0;
        public int m_dwOpenStoreTick = 0;
        public int m_dwWaringTick = 0;
        public int m_dwClientTimeOutTick = 0;
        public int m_SendCheckTick = 0;
        public TCheckStep m_Stat;
        public long m_FinishTick = 0;
        private readonly ClientThread lastGameSvr;
        private readonly ConfigManager _configManager;

        public ClientSession(TSessionInfo session, ClientThread clientThread, ConfigManager configManager)
        {
            _msgList = new List<TDelayMsg>();
            _session = session;
            _sessionIdx = session.SocketId;
            m_fOverClientCount = false;
            lastGameSvr = clientThread;
            _configManager = configManager;
            m_xHWID = MD5.g_MD5EmptyDigest;
        }

        public int SessionId => _sessionIdx;

        public GameSpeed GetGameSpeed()
        {
            return _gameSpeed;
        }

        public TSessionInfo GetSession()
        {
            return _session;
        }

        private GateConfig Config => _configManager.GateConfig;

        /// <summary>
        /// 处理客户端发送过来的封包
        /// </summary>
        /// <param name="UserData"></param>
        public void HandleUserPacket(TSendUserData UserData)
        {
            string sMsg = string.Empty;
            int dwCurrentTick = 0;
            if (m_fKickFlag)
            {
                m_fKickFlag = false;
                return;
            }
            if ((UserData.BufferLen >= 5) && Config.m_fDefenceCCPacket)
            {
                sMsg = HUtil32.GetString(UserData.Buffer, 2, UserData.BufferLen - 3);
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
            if ((m_Stat == TCheckStep.csCheckLogin) || (m_Stat == TCheckStep.csSendCheck))
            {
                dwCurrentTick = HUtil32.GetTickCount();
                if (0 == m_SendCheckTick)
                {
                    m_SendCheckTick = dwCurrentTick;
                }
                if ((dwCurrentTick - m_SendCheckTick) > 1000 * 5)// 如果5 秒 没有回数据 就下发数据
                {
                    m_Stat = TCheckStep.csSendSmu;
                }
            }
            // 如果下发成功  得多少秒有数据如果没有的话，那就是有问题
            if ((m_Stat == TCheckStep.csSendFinsh))
            {
                dwCurrentTick = HUtil32.GetTickCount();
                if ((dwCurrentTick - m_FinishTick) > 1000 * 10)
                {
                    SendKickMsg(12);
                }
            }
            var success = false;
            switch (_handleLogin)
            {
                case 0:
                {
                    sMsg = HUtil32.GetString(UserData.Buffer, 2, UserData.BufferLen - 3);
                    var tempStr = sMsg;
                    tempStr = EDcode.DeCodeString(sMsg);
                    HandleLogin(tempStr, UserData.BufferLen, "", ref success);
                    if (!success)
                    {
                        //KickUser("ip");œ
                    }
                    break;
                }
                case >= 2:
                {
                    if (UserData.BufferLen <= 1) //bug 收到为1的数据封包
                    {
                        Debug.WriteLine("无效的数据分包.");
                        return;
                    }

                    //todo 需要优化此处
                    var packBuff = new byte[UserData.BufferLen];
                    var tempBuff = new byte[UserData.BufferLen - 3];//跳过#1....!
                    Buffer.BlockCopy(UserData.Buffer, 2, tempBuff, 0, tempBuff.Length);
                    var nDeCodeLen = Misc.DecodeBuf(tempBuff, UserData.BufferLen - 3, ref packBuff);
                    var CltCmd = new TCmdPack(packBuff, nDeCodeLen);

                    switch(CltCmd.Cmd)
                    {
                        case Grobal2.CM_GUILDUPDATENOTICE:
                        case Grobal2.CM_GUILDUPDATERANKINFO:
                            if (UserData.BufferLen > Config.m_nMaxClientPacketSize)
                            {
                                // if (LogManager.Units.LogManager.g_pLogMgr.CheckLevel(3))
                                // {
                                //     LogManager.Units.LogManager.g_pLogMgr.Add("Kick off user,over max client packet size: " + (Len).ToString());
                                // }
                                // Misc.Units.Misc.KickUser(m_pUserOBJ.nIPAddr);
                                //Succeed = false;
                                return;
                            }
                            break;
                        default:
                            if (UserData.BufferLen > Config.m_nNomClientPacketSize)
                            {
                                // if (LogManager.Units.LogManager.g_pLogMgr.CheckLevel(3))
                                // {
                                //     LogManager.Units.LogManager.g_pLogMgr.Add("Kick off user,over nom client packet size: " + (Len).ToString());
                                // }
                                // Misc.KickUser(m_pUserOBJ.nIPAddr);
                                //Succeed = false;
                                //return;
                            }
                            break;
                    }

                    int nInterval;
                    int nMsgCount;
                    var dwDelay = 0;
                    switch (CltCmd.Cmd)
                    {
                        
                        case Grobal2.CM_LOGINNOTICEOK:
                            if (m_Stat == TCheckStep.csCheckLogin)
                            {
                                // m_checkstr = GenRandString(10);
                                // SendDefMessage(SM_CHECKCLIENT, 0, 0, 0, 0, m_checkstr);
                                // m_SendCheckTick = GetTickCount;
                                m_Stat = TCheckStep.csSendCheck;
                            }
                            break;
                        // case Grobal2.CM_CHECKCLIENT_RES:
                        //     if (m_Stat == TCheckStep.csSendCheck)
                        //     {
                        //         res2 = HUtil32.MakeLong(CltCmd.Y, CltCmd.X);
                        //         m_Stat = TCheckStep.csSendSmu;                            
                        //     }
                        //     break;
                        case Grobal2.CM_WALK:
                        case Grobal2.CM_RUN:
                            if (Config.m_fMoveInterval)// 700
                            {
                                dwCurrentTick = HUtil32.GetTickCount();
                                int nMoveInterval;
                                if (m_fSpeedLimit)
                                {
                                    nMoveInterval = Config.m_nMoveInterval + Config.m_nPunishMoveInterval;
                                }
                                else
                                {
                                    nMoveInterval = Config.m_nMoveInterval;
                                }
                                nInterval = (dwCurrentTick - m_dwMoveTick);
                                if ((nInterval >= nMoveInterval))
                                {
                                    m_dwMoveTick = dwCurrentTick;
                                    m_dwSpellTick = dwCurrentTick - Config.m_nMoveNextSpellCompensate;
                                    if (m_dwAttackTick < dwCurrentTick - Config.m_nMoveNextAttackCompensate)
                                    {
                                        m_dwAttackTick = dwCurrentTick - Config.m_nMoveNextAttackCompensate;
                                    }
                                    m_dwLastDirection = CltCmd.Dir;
                                }
                                else
                                {
                                    if (Config.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
                                    {
                                        nMsgCount = GetDelayMsgCount();
                                        if (nMsgCount == 0)
                                        {
                                            dwDelay = Config.m_nPunishBaseInterval + (int)Math.Round((nMoveInterval - nInterval) * Config.m_nPunishIntervalRate);
                                            m_dwMoveTick = dwCurrentTick + dwDelay;
                                        }
                                        else
                                        {
                                            m_dwMoveTick = dwCurrentTick + (nMoveInterval - nInterval);
                                            if (nMsgCount >= 2)
                                            {
                                                SendKickMsg(0);
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_HIT:
                        case Grobal2.CM_HEAVYHIT:
                        case Grobal2.CM_BIGHIT:
                        case Grobal2.CM_POWERHIT:
                        case Grobal2.CM_LONGHIT:
                        case Grobal2.CM_WIDEHIT:
                        case Grobal2.CM_CRSHIT:
                        case Grobal2.CM_FIREHIT:
                            if (Config.m_fAttackInterval)
                            {
                                dwCurrentTick = HUtil32.GetTickCount();
                                int nAttackInterval;
                                if (m_fSpeedLimit)
                                {
                                    nAttackInterval = Config.m_nAttackInterval + Config.m_nPunishAttackInterval;
                                }
                                else
                                {
                                    nAttackInterval = Config.m_nAttackInterval;
                                }
                                var nAttackFixInterval = HUtil32._MAX(0, (nAttackInterval - Config.m_nMaxItemSpeedRate * m_nItemSpeed));
                                nInterval = dwCurrentTick - m_dwAttackTick;
                                if ((nInterval >= nAttackFixInterval))
                                {
                                    m_dwAttackTick = dwCurrentTick;
                                    if (Config.m_fItemSpeedCompensate)
                                    {
                                        m_dwMoveTick = dwCurrentTick - (Config.m_nAttackNextMoveCompensate + Config.m_nMaxItemSpeedRate * m_nItemSpeed);// 550
                                        m_dwSpellTick = dwCurrentTick - (Config.m_nAttackNextSpellCompensate + Config.m_nMaxItemSpeedRate * m_nItemSpeed);// 1150
                                    }
                                    else
                                    {
                                        m_dwMoveTick = dwCurrentTick - Config.m_nAttackNextMoveCompensate;// 550
                                        m_dwSpellTick = dwCurrentTick - Config.m_nAttackNextSpellCompensate;// 1150
                                    }
                                    m_dwLastDirection = CltCmd.Dir;
                                }
                                else
                                {
                                    if (Config.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
                                    {
                                        nMsgCount = GetDelayMsgCount();
                                        if (nMsgCount == 0)
                                        {
                                            dwDelay = Config.m_nPunishBaseInterval + (int)Math.Round((nAttackFixInterval - nInterval) * Config.m_nPunishIntervalRate);
                                            m_dwAttackTick = dwCurrentTick + dwDelay;
                                        }
                                        else
                                        {
                                            m_dwAttackTick = dwCurrentTick + (nAttackFixInterval - nInterval);
                                            if (nMsgCount >= 2)
                                            {
                                                SendKickMsg(0);
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_SPELL:
                            if (Config.m_fSpellInterval)// 1380
                            {
                                dwCurrentTick = HUtil32.GetTickCount();
                                if (CltCmd.Magic >= 0128)
                                {
                                    return;
                                }
                                else if (TableDef.MAIGIC_DELAY_ARRAY[CltCmd.Magic]) // 过滤武士魔法
                                {
                                    int nSpellInterval;
                                    if (m_fSpeedLimit)
                                    {
                                        nSpellInterval = TableDef.MAIGIC_DELAY_TIME_LIST[CltCmd.Magic] + Config.m_nPunishSpellInterval;
                                    }
                                    else
                                    {
                                        nSpellInterval = TableDef.MAIGIC_DELAY_TIME_LIST[CltCmd.Magic];
                                    }
                                    nInterval = (dwCurrentTick - m_dwSpellTick);
                                    if ((nInterval >= nSpellInterval))
                                    {
                                        int dwNextMove;
                                        int dwNextAtt;
                                        if (TableDef.MAIGIC_ATTACK_ARRAY[CltCmd.Magic])
                                        {
                                            dwNextMove = Config.m_nSpellNextMoveCompensate;
                                            dwNextAtt = Config.m_nSpellNextAttackCompensate;
                                        }
                                        else
                                        {
                                            dwNextMove = Config.m_nSpellNextMoveCompensate + 80;
                                            dwNextAtt = Config.m_nSpellNextAttackCompensate + 80;
                                        }
                                        m_dwSpellTick = dwCurrentTick;
                                        m_dwMoveTick = dwCurrentTick - dwNextMove;
                                        m_dwAttackTick = dwCurrentTick - dwNextAtt;
                                        m_dwLastDirection = CltCmd.Dir;
                                    }
                                    else
                                    {
                                        if (Config.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
                                        {
                                            nMsgCount = GetDelayMsgCount();
                                            if (nMsgCount == 0)
                                            {
                                                dwDelay = Config.m_nPunishBaseInterval + (int)Math.Round((nSpellInterval - nInterval) * Config.m_nPunishIntervalRate);
                                            }
                                            else
                                            {
                                                if (nMsgCount >= 2)
                                                {
                                                    SendKickMsg(0);
                                                }
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_SITDOWN:
                            if (Config.m_fSitDownInterval)
                            {
                                dwCurrentTick = HUtil32.GetTickCount();
                                nInterval = (dwCurrentTick - m_dwSitDownTick);
                                if (nInterval >= Config.m_nSitDownInterval)
                                {
                                    m_dwSitDownTick = dwCurrentTick;
                                }
                                else
                                {
                                    if (Config.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
                                    {
                                        nMsgCount = GetDelayMsgCount();
                                        if (nMsgCount == 0)
                                        {
                                            dwDelay = Config.m_nPunishBaseInterval + (int)Math.Round((Config.m_nSitDownInterval - nInterval) * Config.m_nPunishIntervalRate);
                                            m_dwSitDownTick = dwCurrentTick + dwDelay;
                                        }
                                        else
                                        {
                                            m_dwSitDownTick = dwCurrentTick + (Config.m_nSitDownInterval - nInterval);
                                            if (nMsgCount >= 2)
                                            {
                                                SendKickMsg(0);
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_BUTCH:
                            if (Config.m_fButchInterval)
                            {
                                dwCurrentTick = HUtil32.GetTickCount();
                                nInterval = dwCurrentTick - m_dwButchTick;
                                if (nInterval >= Config.m_nButchInterval)
                                {
                                    m_dwButchTick = dwCurrentTick;
                                }
                                else
                                {
                                    if (Config.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
                                    {
                                        if (!PeekDelayMsg(CltCmd.Cmd))
                                        {
                                            dwDelay = Config.m_nPunishBaseInterval + (int)Math.Round((Config.m_nButchInterval - nInterval) * Config.m_nPunishIntervalRate);
                                            m_dwButchTick = dwCurrentTick + dwDelay;
                                        }
                                        else
                                        {
                                            m_dwSitDownTick = dwCurrentTick + (Config.m_nButchInterval - nInterval);
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_TURN:
                            if (Config.m_fTurnInterval && (Config.m_tOverSpeedPunishMethod != TPunishMethod.ptTurnPack))
                            {
                                if (m_dwLastDirection != CltCmd.Dir)
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    if (dwCurrentTick - m_dwTurnTick >= Config.m_nTurnInterval)
                                    {
                                        m_dwLastDirection = CltCmd.Dir;
                                        m_dwTurnTick = dwCurrentTick;
                                    }
                                    else
                                    {
                                        if (Config.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
                                        {
                                            if (!PeekDelayMsg(CltCmd.Cmd))
                                            {
                                                dwDelay = Config.m_nPunishBaseInterval + (int)Math.Round((Config.m_nTurnInterval - (dwCurrentTick - m_dwTurnTick)) * Config.m_nPunishIntervalRate);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_DEALTRY:
                            dwCurrentTick = HUtil32.GetTickCount();
                            if ((dwCurrentTick - m_dwDealTick < 10000))
                            {
                                if ((dwCurrentTick - m_dwWaringTick > 2000))
                                {
                                    m_dwWaringTick = dwCurrentTick;
                                    SendSysMsg(string.Format("攻击状态不能交易！请稍等{0}秒……", new double[] { (10000 - dwCurrentTick + m_dwDealTick) / 1000 + 1 }));
                                }
                                return;
                            }
                            break;
                        case Grobal2.CM_SAY:
                            if (Config.m_fChatInterval)
                            {
                                if(!sMsg.StartsWith("@"))
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    if (dwCurrentTick - m_dwChatTick < Config.m_nChatInterval)
                                    {
                                        return;
                                    }
                                    m_dwChatTick = dwCurrentTick;
                                }
                            }
                            if (nDeCodeLen > TCmdPack.PackSize)
                            {
                                if(sMsg.StartsWith("@"))
                                {
                                    //Move((nABuf + TCmdPack.PackSize as string), pszChatBuffer[0], nDeCodeLen - TCmdPack.PackSize);
                                    //pszChatBuffer[nDeCodeLen - TCmdPack.PackSize] = '\0';
                                    //nChatStrPos = pszChatBuffer.IndexOf(" ");
                                    //if (nChatStrPos > 0)
                                    //{
                                    //    Move(pszChatBuffer[0], pszChatCmd[0], nChatStrPos - 1);
                                    //    pszChatCmd[nChatStrPos - 1] = '\0';
                                    //}
                                    //else
                                    //{
                                    //    Move(pszChatBuffer[0], pszChatCmd[0], pszChatBuffer.Length);
                                    //}
                                    //if (g_ChatCmdFilterList.IndexOf(pszChatCmd) >= 0)
                                    //{
                                    //    Cmd.UID = m_nSvrObject;
                                    //    Cmd.Cmd = Grobal2.SM_WHISPER;
                                    //    Cmd.X = MakeWord(0xFF, 56);
                                    //    StrFmt(pszChatBuffer, Protocol.Units.Protocol._STR_CMD_FILTER, new char[] { pszChatCmd });
                                    //    pszSendBuf[0] = "#";
                                    //    Move(Cmd, m_pOverlapRecv.BBuffer[1], TCmdPack.PackSize);
                                    //    Move(pszChatBuffer[0], m_pOverlapRecv.BBuffer[13], pszChatBuffer.Length);
                                    //    nEnCodeLen = Misc.Units.Misc.EncodeBuf(((int)m_pOverlapRecv.BBuffer[1]), TCmdPack.PackSize + pszChatBuffer.Length, ((int)pszSendBuf[1]));
                                    //    pszSendBuf[nEnCodeLen + 1] = "!";
                                    //    m_tIOCPSender.SendData(m_pOverlapSend, pszSendBuf[0], nEnCodeLen + 2);
                                    //    return;
                                    //}
                                    //if (Config.m_fSpaceMoveNextPickupInterval)
                                    //{
                                    //    if ((pszChatBuffer).ToLower().CompareTo((Config.m_szCMDSpaceMove).ToLower()) == 0)
                                    //    {
                                    //        m_dwPickupTick = GetTickCount + Config.m_nSpaceMoveNextPickupInterval;
                                    //    }
                                    //}
                                }
                                else if (Config.m_fChatFilter)
                                {
                                    if(sMsg.StartsWith("/"))
                                    {
                                        // Move((nABuf + TCmdPack.PackSize as string), pszChatBuffer[0], nDeCodeLen - TCmdPack.PackSize);
                                        // pszChatBuffer[nDeCodeLen - TCmdPack.PackSize] = '\0';
                                        // nChatStrPos = pszChatBuffer.IndexOf(" ");
                                        // if (nChatStrPos > 0)
                                        // {
                                        //     Move(pszChatBuffer[0], pszChatCmd[0], nChatStrPos - 1);
                                        //     pszChatCmd[nChatStrPos - 1] = '\0';
                                        //     szChatBuffer = (pszChatBuffer[nChatStrPos] as string);
                                        //     fChatFilter = AbusiveFilter.Units.AbusiveFilter.CheckChatFilter(ref szChatBuffer, ref Succeed);
                                        //     if ((fChatFilter > 0) && !Succeed)
                                        //     {
                                        //         //g_pLogMgr.Add("Kick off user,saying in filter");
                                        //         return;
                                        //     }
                                        //     if (fChatFilter == 2)
                                        //     {
                                        //         StrFmt(pszChatBuffer, "%s %s", new string[] { pszChatCmd, szChatBuffer });
                                        //         nDeCodeLen = pszChatBuffer.Length + TCmdPack.PackSize;
                                        //         Move(pszChatBuffer[0], (nABuf + TCmdPack.PackSize as string), pszChatBuffer.Length);
                                        //     }
                                        // }
                                    }
                                    else if(!sMsg.StartsWith("@"))
                                    {
                                        // szChatBuffer = (nABuf + TCmdPack.PackSize as string);
                                        // fChatFilter = AbusiveFilter.Units.AbusiveFilter.CheckChatFilter(ref szChatBuffer, ref Succeed);
                                        // if ((fChatFilter > 0) && !Succeed)
                                        // {
                                        //     //g_pLogMgr.Add("Kick off user,saying in filter");
                                        //     return;
                                        // }
                                        // if (fChatFilter == 2)
                                        // {
                                        //     nDeCodeLen = szChatBuffer.Length + TCmdPack.PackSize;
                                        //     Move(szChatBuffer[1], (nABuf + TCmdPack.PackSize as string), szChatBuffer.Length);
                                        // }
                                    }
                                }
                            }
                            break;
                        case Grobal2.CM_PICKUP:
                            if (Config.m_fPickupInterval)
                            {
                                dwCurrentTick = HUtil32.GetTickCount();
                                if (dwCurrentTick - m_dwPickupTick > Config.m_nPickupInterval)
                                {
                                    m_dwPickupTick = dwCurrentTick;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;
                        case Grobal2.CM_EAT:
                            if (Config.m_fEatInterval)
                            {
                                if (CltCmd.Direct == 0 || CltCmd.Direct == 1 || CltCmd.Direct == 3)
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    if (dwCurrentTick - m_dwEatTick > Config.m_nEatInterval)
                                    {
                                        m_dwEatTick = dwCurrentTick;
                                    }
                                    else
                                    {
                                        //Cmd = new TCmdPack();
                                        //Cmd.UID = CltCmd.UID;
                                        //Cmd.Cmd = Grobal2.SM_EAT_FAIL;
                                        //pszSendBuf[0] = '#';
                                        //nEnCodeLen = Misc.EncodeBuf((int)Cmd, TCmdPack.PackSize, ((int)pszSendBuf[1]));
                                        //pszSendBuf[nEnCodeLen + 1] = '!';
                                        //m_tIOCPSender.SendData(m_pOverlapSend, pszSendBuf[0], nEnCodeLen + 2);
                                        return;
                                    }
                                }
                            }
                            break;
                    }

                    // if (fPacketOverSpeed)
                    // {
                    //     if (fConvertPacket)
                    //     {
                    //         CltCmd.Cmd = Grobal2.CM_TURN;
                    //     }
                    //     else
                    //     {
                    //         if (Config.m_fOverSpeedSendBack)
                    //         {
                    //             if (Config.m_tSpeedHackWarnMethod == TOverSpeedMsgMethod.ptSysmsg)
                    //             {
                    //                 CltCmd.Cmd = Grobal2.RM_WHISPER;
                    //                 CltCmd.UID = 0xFF; // FrontColor
                    //                 CltCmd.X = 0xF9; // BackColor
                    //             }
                    //             else
                    //             {
                    //                 CltCmd.Cmd = Grobal2.CM_SPEEDHACKMSG;
                    //             }
                    //             nBuffer = UserData.Buffer[TSvrCmdPack.PackSize];
                    //             TSvrCmdPack _wvar1 = ((TSvrCmdPack)(m_pOverlapRecv.BBuffer));
                    //             _wvar1.Flag = Protocol.Units.Protocol.RUNGATECODE;
                    //             _wvar1.SockID = m_dwSessionID;
                    //             _wvar1.Cmd = Grobal2.GM_DATA;
                    //             _wvar1.GGSock = m_nSvrListIdx;
                    //             _wvar1.DataLen = TCmdPack.PackSize;
                    //             Move((nABuf as object), nBuffer, TCmdPack.PackSize);
                    //             Move(Config.m_szOverSpeedSendBack, (nBuffer + TCmdPack.PackSize as string), Config.m_szOverSpeedSendBack.Length);
                    //             _wvar1.DataLen += Config.m_szOverSpeedSendBack.Length + 1;
                    //             ((byte)nBuffer + _wvar1.DataLen - 1) = 0;
                    //             lastGameSvr.SendBuffer(m_pOverlapRecv.BBuffer, _wvar1.DataLen + TSvrCmdPack.PackSize);
                    //             return;
                    //         }
                    //         switch(Config.m_tOverSpeedPunishMethod)
                    //         {
                    //             case TPunishMethod.ptTurnPack:
                    //                 CltCmd.Cmd = Grobal2.CM_TURN;
                    //                 break;
                    //             case TPunishMethod.ptDropPack:
                    //                 return;
                    //                 break;
                    //             case TPunishMethod.ptNullPack:
                    //                 CltCmd.Cmd = 0xFFFF;
                    //                 break;
                    //             case TPunishMethod.ptDelaySend:
                    //                 if (dwDelay > 0)
                    //                 {
                    //                     nBuffer = UserData.Buffer[TSvrCmdPack.PackSize];
                    //                     TSvrCmdPack _wvar2 = ((TSvrCmdPack)(m_pOverlapRecv.BBuffer));
                    //                     _wvar2.Flag = Protocol.Units.Protocol.RUNGATECODE;
                    //                     _wvar2.SockID = m_dwSessionID;
                    //                     _wvar2.Cmd = Grobal2.GM_DATA;
                    //                     _wvar2.GGSock = m_nSvrListIdx;
                    //                     _wvar2.DataLen = TCmdPack.PackSize;
                    //                     Move((nABuf as object), (nBuffer as object), TCmdPack.PackSize);
                    //                     if (nDeCodeLen > TCmdPack.PackSize)
                    //                     {
                    //                         _wvar2.DataLen += Misc.EncodeBuf(nABuf + TCmdPack.PackSize, nDeCodeLen - TCmdPack.PackSize, nBuffer + TCmdPack.PackSize) + 1;
                    //                         ((byte)nBuffer + _wvar2.DataLen - 1) = 0;
                    //                     }
                    //                     SendDelayMsg(CltCmd.Magic, CltCmd.Dir, CltCmd.Cmd, _wvar2.DataLen + TSvrCmdPack.PackSize, m_pOverlapRecv.BBuffer, dwDelay);
                    //                     return;
                    //                 }
                    //                 break;
                    //         }
                    //     }
                    // }

                    byte[] nBuffer;
                    TSvrCmdPack cmdPack = new TSvrCmdPack();
                    cmdPack.Flag = Protocol.RUNGATECODE;
                    cmdPack.SockID = _session.nSckHandle;
                    cmdPack.Cmd = Grobal2.GM_DATA;
                    cmdPack.GGSock = m_nSvrListIdx;
                    if (nDeCodeLen > TCmdPack.PackSize)
                    {
                        var len = UserData.BufferLen - 19;
                        cmdPack.DataLen = TCmdPack.PackSize + len + 1;
                        var bufferLen = TSvrCmdPack.PackSize + cmdPack.DataLen;
                        nBuffer = new byte[bufferLen];
                        Array.Copy(packBuff, 0, nBuffer, 20, TCmdPack.PackSize); 
                        Array.Copy(tempBuff, 16, nBuffer, 32, len); //消息体
                    }
                    else
                    {
                        nBuffer = new byte[TSvrCmdPack.PackSize + packBuff.Length];
                        cmdPack.DataLen = TCmdPack.PackSize;
                        Array.Copy(packBuff, 0, nBuffer, 20, packBuff.Length);
                    }
                    var cmdBuff = cmdPack.GetPacket();
                    Array.Copy(cmdBuff, 0, nBuffer, 0, 20);
                    lastGameSvr.SendBuffer(nBuffer, cmdPack.DataLen + TSvrCmdPack.PackSize);
                    break;
                }
            }
        }

        /// <summary>
        /// 处理延时消息
        /// </summary>
        public void HandleDelayMsg()
        {
            if (GetDelayMsgCount() <= 0)
            {
                return;
            }
            TDelayMsg delayMsg = null;
            var dwCurrentTick = 0;
            while (GetDelayMsg(ref delayMsg))
            {
                if (delayMsg.nBufLen > 0)
                {
                    lastGameSvr.SendBuffer(delayMsg.pBuffer, delayMsg.nBufLen);//发送消息到M2
                    dwCurrentTick = HUtil32.GetTickCount();
                    switch (delayMsg.nCmd)
                    {
                        case Grobal2.CM_BUTCH:
                            _gameSpeed.dwButchTick = dwCurrentTick;
                            break;
                        case Grobal2.CM_SITDOWN:
                            _gameSpeed.dwSitDownTick = dwCurrentTick;
                            break;
                        case Grobal2.CM_TURN:
                            _gameSpeed.dwTurnTick = dwCurrentTick;
                            break;
                        case Grobal2.CM_WALK:
                        case Grobal2.CM_RUN:
                            _gameSpeed.dwMoveTick = dwCurrentTick;
                            _gameSpeed.dwSpellTick = dwCurrentTick - Config.m_nMoveNextSpellCompensate; //1200
                            if (_gameSpeed.dwAttackTick < dwCurrentTick - Config.m_nMoveNextAttackCompensate)
                            {
                                _gameSpeed.dwAttackTick = dwCurrentTick - Config.m_nMoveNextAttackCompensate; //900
                            }
                            LastDirection = delayMsg.nDir;
                            break;
                        case Grobal2.CM_HIT:
                        case Grobal2.CM_HEAVYHIT:
                        case Grobal2.CM_BIGHIT:
                        case Grobal2.CM_POWERHIT:
                        case Grobal2.CM_LONGHIT:
                        case Grobal2.CM_WIDEHIT:
                        case Grobal2.CM_CRSHIT:
                        case Grobal2.CM_FIREHIT:
                            if (_gameSpeed.dwAttackTick < dwCurrentTick)
                            {
                                _gameSpeed.dwAttackTick = dwCurrentTick;
                            }
                            if (Config.m_fItemSpeedCompensate)
                            {
                                _gameSpeed.dwMoveTick = dwCurrentTick - (Config.m_nAttackNextMoveCompensate + Config.m_nMaxItemSpeedRate * nItemSpeed);// 550
                                _gameSpeed.dwSpellTick = dwCurrentTick - (Config.m_nAttackNextSpellCompensate + Config.m_nMaxItemSpeedRate * nItemSpeed);// 1150
                            }
                            else
                            {
                                _gameSpeed.dwMoveTick = dwCurrentTick - Config.m_nAttackNextMoveCompensate; // 550
                                _gameSpeed.dwSpellTick = dwCurrentTick - Config.m_nAttackNextSpellCompensate;// 1150
                            }
                            LastDirection = delayMsg.nDir;
                            break;
                        case Grobal2.CM_SPELL:
                            _gameSpeed.dwSpellTick = dwCurrentTick;
                            int nNextMov = 0;
                            int nNextAtt = 0;
                            if (GateShare.Magic_Attack_Array[delayMsg.nMag])
                            {
                                nNextMov = Config.m_nSpellNextMoveCompensate;
                                nNextAtt = Config.m_nSpellNextAttackCompensate;
                            }
                            else
                            {
                                nNextMov = Config.m_nSpellNextMoveCompensate + 80;
                                nNextAtt = Config.m_nSpellNextAttackCompensate + 80;
                            }
                            _gameSpeed.dwMoveTick = dwCurrentTick - nNextMov;// 550
                            if (_gameSpeed.dwAttackTick < dwCurrentTick - nNextAtt)// 900
                            {
                                _gameSpeed.dwAttackTick = dwCurrentTick - nNextAtt;
                            }
                            LastDirection = delayMsg.nDir;
                            break;
                    }
                }
            }
        }

        private bool PeekDelayMsg(int nCmd)
        {
            int i;
            int iCmd;
            var result = false;
            try
            {
                i = 0;
                while (_msgList.Count > i)
                {
                    iCmd = _msgList[i].nCmd;
                    if (nCmd == Grobal2.CM_HIT)
                    {
                        if ((iCmd == Grobal2.CM_HIT) || (iCmd == Grobal2.CM_HEAVYHIT) ||
                            (iCmd == Grobal2.CM_BIGHIT) || (iCmd == Grobal2.CM_POWERHIT) ||
                            (iCmd == Grobal2.CM_LONGHIT) || (iCmd == Grobal2.CM_WIDEHIT) ||
                            (iCmd == Grobal2.CM_CRSHIT) || (iCmd == Grobal2.CM_FIREHIT))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else if (nCmd == Grobal2.CM_RUN)
                    {
                        if ((iCmd == Grobal2.CM_WALK) || (iCmd == Grobal2.CM_RUN))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else if (iCmd == nCmd)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            finally
            {
            }
            return result;
        }

        private int GetDelayMsgCount()
        {
            return _msgList.Count;
        }

        /// <summary>
        /// 获取延时消息
        /// </summary>
        private bool GetDelayMsg(ref TDelayMsg delayMsg)
        {
            lock (this)
            {
                var result = false;
                TDelayMsg _delayMsg = null;
                var count = 0;
                while (_msgList.Count > 0)
                {
                    _delayMsg = _msgList[count];
                    if (_delayMsg.dwDelayTime != 0 && HUtil32.GetTickCount() < _delayMsg.dwDelayTime)
                    {
                        count++;
                        continue;
                    }
                    _msgList.RemoveAt(count);
                    delayMsg = new TDelayMsg();
                    delayMsg.nMag = _delayMsg.nMag;
                    delayMsg.nDir = _delayMsg.nDir;
                    delayMsg.nCmd = _delayMsg.nCmd;
                    delayMsg.nBufLen = _delayMsg.nBufLen;
                    delayMsg.pBuffer = _delayMsg.pBuffer;
                    _delayMsg = null;
                    result = true;
                }
                return result;
            }
        }

        /// <summary>
        /// 发送延时处理消息
        /// </summary>
        private void SendDelayMsg(int nMid, int nDir, int nIdx, int nLen, string sMsg, int dwDelay)
        {
            const int DELAY_BUFFER_LEN = 1024;
            if (nLen > 0 && nLen <= DELAY_BUFFER_LEN)
            {
                var pDelayMsg = new TDelayMsg();
                pDelayMsg.nMag = nMid;
                pDelayMsg.nDir = nDir;
                pDelayMsg.nCmd = nIdx;
                pDelayMsg.dwDelayTime = HUtil32.GetTickCount() + dwDelay;
                pDelayMsg.nBufLen = nLen;
                if (!string.IsNullOrEmpty(sMsg))
                {
                    var bMsg = HUtil32.StringToByteAry(sMsg);
                    pDelayMsg.pBuffer = bMsg;
                }
                _msgList.Add(pDelayMsg);
            }
        }

        /// <summary>
        /// 发送延时处理消息
        /// </summary>
        private void SendDelayMsg(int nMid, int nDir, int nIdx, int nLen, byte[] pMsg, int dwDelay)
        {
            const int DELAY_BUFFER_LEN = 1024;
            if (nLen > 0 && nLen <= DELAY_BUFFER_LEN)
            {
                var pDelayMsg = new TDelayMsg();
                pDelayMsg.nMag = nMid;
                pDelayMsg.nDir = nDir;
                pDelayMsg.nCmd = nIdx;
                pDelayMsg.dwDelayTime = HUtil32.GetTickCount() + dwDelay;
                pDelayMsg.nBufLen = nLen;
                pDelayMsg.pBuffer = pMsg;
                _msgList.Add(pDelayMsg);
            }
        }

        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        public void ProcessSvrData(byte[] sendBuffer)
        {
            if (_session.Socket != null && _session.Socket.Connected)
            {
                _session.Socket.Send(sendBuffer);
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
            Array.Copy(Cmd.ToByte(), 0, TempBuf, 0, TCmdPack.PackSize);
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
            //m_tIOCPSender.SendData(m_pOverlapSend, SendBuf[0], iLen + 2);
            _session.Socket.Send(SendBuf, iLen + 2, SocketFlags.None);
        }

        private void GetRealMsgId(ref int msgid)
        {
            var result = msgid;
            switch (msgid)
            {
                case 3014:
                    result = 3018; //CM_POWERHIT;
                    break;
                case 3003:
                    result = 3019; //CM_LONGHIT;
                    break;
                case 1007:
                    result = 1008; //CM_MAGICKEYCHANGE;
                    break;
                case 3017:
                    result = 3012; //CM_SITDOWN;
                    break;
                case 3016:
                    result = 3013; //CM_RUN;
                    break;
                case 3009:
                    result = 3010; //CM_TURN;
                    break;
                case 3018:
                    result = 3011; //CM_WALK;
                    break;
                case 3011:
                    result = 3016; //CM_BIGHIT;
                    break;
                case 3002:
                    result = 3017; //CM_SPELL;
                    break;
                case 3013:
                    result = 3014; //CM_HIT;
                    break;
                case 3012:
                    result = 3015; //CM_HEAVYHIT;
                    break;
                case 3010:
                    result = 3005; //CM_THROW;
                    break;
                case 1008:
                    result = 3003; //CM_SQUHIT;
                    break;
                case 3019:
                    result = 3002; //CM_PURSUEHIT;
                    break;
                case 1006:
                    result = 1007; //CM_BUTCH;
                    break;
                case 3015:
                    result = 1006; //CM_EAT;
                    break;
                case 3005:
                    result = 3009; //CM_HORSERUN;
                    break;

            }
        }

        private void SendKickMsg(int killType)
        {
            var kick = false;
            var SendMsg = string.Empty;
            var defMsg = new TDefaultMessage();
            switch (killType)
            {
                case 0:
                    if (Config.m_fKickOverSpeed)
                    {
                        kick = true;
                    }
                    SendMsg = Config.m_szOverSpeedSendBack;
                    break;
                case 1:
                    kick = true;
                    SendMsg = Config.m_szPacketDecryptFailed;
                    break;
                case 2:
                    kick = true;
                    SendMsg = "当前登录帐号正在其它位置登录，本机已被强行离线";
                    break;
                case 4: //todo 版本号验证
                    kick = true;
                    defMsg.Cmd = Grobal2.SM_VERSION_FAIL;
                    break;
                case 5:
                    kick = true;
                    SendMsg = Config.m_szOverClientCntMsg;
                    break;
                case 6:
                    kick = true;
                    SendMsg = Config.m_szHWIDBlockedMsg;
                    break;
                case 12:
                    kick = true;
                    SendMsg = "反外挂模块更新失败,请重启客户端!!!!";
                    break;
            }

            defMsg.UID = m_nSvrObject;
            defMsg.Cmd = Grobal2.SM_SYSMESSAGE;
            defMsg.X = HUtil32.MakeWord(0xFF, 0xF9);
            defMsg.Y = 0;
            defMsg.Direct = 0;
            
            byte[] TempBuf = new byte[1024];
            byte[] SendBuf = new byte[1024];
            SendBuf[0] = (byte)'#';
            //Move(Cmd, TempBuf[1], TCmdPack.PackSize);
            var iLen = 0;
            if (!string.IsNullOrEmpty(SendMsg))
            {
                //Move(SendMsg[1], TempBuf[13], SendMsg.Length);
                TempBuf = HUtil32.GetBytes(SendMsg);
                iLen = TCmdPack.PackSize + SendMsg.Length;
            }
            else
            {
                iLen = TCmdPack.PackSize;
            }
            iLen = Misc.EncodeBuf(TempBuf, iLen, SendBuf);
            SendBuf[iLen + 1] = (byte)'!';
            //m_tIOCPSender.SendData(m_pOverlapSend, SendBuf[0], iLen + 2);
            _session.Socket.Send(SendBuf);
            m_fKickFlag = kick;
        }

        /// <summary>
        /// 处理登录数据
        /// </summary>
        /// <param name="loginData"></param>
        /// <param name="nLen"></param>
        /// <param name="Addr"></param>
        /// <param name="success"></param>
        private void HandleLogin(string loginData, int nLen, string Addr, ref bool success)
        {
            const int FIRST_PAKCET_MAX_LEN = 254;
            if (nLen < FIRST_PAKCET_MAX_LEN && nLen > 15)
            {
                if (loginData[0] != '*' || loginData[1] != '*')
                {
                    GateShare.AddMainLogMsg($"[HandleLogin] Kicked 1: {loginData}", 1);
                    success = false;
                    return;
                }
                var sDataText = loginData.Remove(0, 2);
                var sHumName = string.Empty;//人物名称
                var sAccount = string.Empty;//账号
                var szCert = string.Empty;
                var szClientVerNO = string.Empty;//客户端版本号
                var szCode = string.Empty;
                var szHarewareID = string.Empty;//硬件ID
                var sData = string.Empty;
                
                sDataText = HUtil32.GetValidStr3(sDataText, ref sAccount, HUtil32.Backslash);
                sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, HUtil32.Backslash);
                if ((sAccount.Length > 4) && (sAccount.Length <= 12) && (sHumName.Length > 2) && (sHumName.Length < 15))
                {
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szCert, HUtil32.Backslash);
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szClientVerNO, HUtil32.Backslash);
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szCode, HUtil32.Backslash);
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szHarewareID, HUtil32.Backslash);
                    if (szCert.Length <= 0 || szCert.Length > 8)
                    {
                        success = false;
                        return;
                    }
                    if (szClientVerNO.Length != 9)
                    {
                        success = false;
                        return;
                    }
                    if (szCode.Length != 1)
                    {
                        success = false;
                        return;
                    }
                    var userType = GateShare.PunishList.ContainsKey(sHumName);
                    if (userType)
                    {
                        _SpeedLimit = true;
                        GateShare.PunishList[sHumName] = this;
                    }
                    if (Config.m_fProcClientHWID)
                    {
                        if (string.IsNullOrEmpty(szHarewareID) || (szHarewareID.Length > 256) || ((szHarewareID.Length % 2) != 0))
                        {
                            GateShare.AddMainLogMsg($"[HandleLogin] Kicked 3: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        var Src = szHarewareID;
                        var Key = "openmir2";
                        var KeyLen = Key.Length;
                        var KeyPos = 0;
                        var OffSet = Convert.ToInt32("$" + Src.Substring(0, 2));
                        var SrcPos = 3;
                        var i = 0;
                        var SrcAsc = 0;
                        var TmpSrcAsc = 0;
                        var dest = new byte[1024];
                        var fMatch = false;
                        try
                        {
                            do
                            {
                                SrcAsc = Convert.ToInt32("$" + Src.Substring(SrcPos - 1, 2));
                                if (KeyPos < KeyLen)
                                {
                                    KeyPos = KeyPos + 1;
                                }
                                else
                                {
                                    KeyPos = 1;
                                }
                                TmpSrcAsc = SrcAsc ^ Key[KeyPos];
                                if (TmpSrcAsc <= OffSet)
                                {
                                    TmpSrcAsc = 255 + TmpSrcAsc - OffSet;
                                }
                                else
                                {
                                    TmpSrcAsc = TmpSrcAsc - OffSet;
                                }
                                dest[i] = (byte)(TmpSrcAsc);
                                i++;
                                OffSet = SrcAsc;
                                SrcPos = SrcPos + 2;
                            } while (!(SrcPos >= Src.Length));
                        }
                        catch (Exception e)
                        {
                            fMatch = true;
                        }
                        if (fMatch)
                        {
                            GateShare.AddMainLogMsg($"[HandleLogin] Kicked 5: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        THardwareHeader pHardwareHeader = new THardwareHeader(dest);
                        //todo session会话里面需要存用户ip
                        GateShare.AddMainLogMsg($"HWID: {MD5.MD5Print(pHardwareHeader.xMd5Digest)}  {sHumName.Trim()}  {Addr}", 1);
                        if (pHardwareHeader.dwMagicCode == 0x13F13F13)
                        {
                            if (MD5.MD5Match(MD5.g_MD5EmptyDigest, pHardwareHeader.xMd5Digest))
                            {
                                GateShare.AddMainLogMsg($"[HandleLogin] Kicked 6: {sHumName}", 1);
                                SendKickMsg(4);
                                return;
                            }
                            m_xHWID = pHardwareHeader.xMd5Digest;
                            if (Filter.g_HWIDFilter.IsFilter(m_xHWID, ref m_fOverClientCount))
                            {
                                GateShare.AddMainLogMsg($"[HandleLogin] Kicked 7: {sHumName}", 1);
                                if (m_fOverClientCount)
                                {
                                    SendKickMsg(5);
                                }
                                else
                                {
                                    SendKickMsg(6);
                                }
                                return;
                            }
                        }
                        else
                        {
                            GateShare.AddMainLogMsg($"[HandleLogin] Kicked 8: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                    }
                    var szTemp = $"**{sAccount}/{sHumName}/{szCert}/{szClientVerNO}/{szCode}/{MD5.MD5Print(m_xHWID)}";
                    // #0.........!
                    var tempBuf = HUtil32.GetBytes(szTemp);
                    var pszLoginPacket = new byte[tempBuf.Length + 100];
                    var encodelen = Misc.EncodeBuf(tempBuf, tempBuf.Length, pszLoginPacket, 2);
                    pszLoginPacket[0] = (byte)'#';
                    pszLoginPacket[1] = (byte)'0';
                    pszLoginPacket[encodelen + 2] = (byte)'!';
                    _handleLogin = 2;
                    Console.WriteLine("发送延时消息:" + HUtil32.GetString(pszLoginPacket, 0, encodelen + 3));
                    SendFirstPack(pszLoginPacket, encodelen + 3);
                }
                else
                {
                    GateShare.AddMainLogMsg($"[HandleLogin] Kicked 2: {loginData}", 1);
                    success = false;
                }
            }
            else
            {
                GateShare.AddMainLogMsg($"[HandleLogin] Kicked 0: {loginData}", 1);
                success = false;
            }
        }

        /// <summary>
        /// 发送消息到GameSvr
        /// </summary>
        /// <param name="packet"></param>
        private void SendFirstPack(byte[] packet, int len = 0)
        {
            byte[] tempBuff;
            if (len == 0)
            {
                tempBuff = new byte[20 + packet.Length];
            }
            else
            {
                tempBuff = new byte[20 + len];
            }
            var GateMsg = new TMsgHeader();
            GateMsg.dwCode = Grobal2.RUNGATECODE;
            GateMsg.nSocket = (int)_session.Socket.Handle;
            GateMsg.wGSocketIdx = (ushort)_session.SocketId;
            GateMsg.wIdent = Grobal2.GM_DATA;
            GateMsg.wUserListIndex = _session.nUserListIndex;
            GateMsg.nLength = tempBuff.Length;
            var sendBuffer = GateMsg.GetPacket();
            Buffer.BlockCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
            if (len == 0)
            {
                Buffer.BlockCopy(packet, 0, tempBuff, sendBuffer.Length, packet.Length);
            }
            else
            {
                Buffer.BlockCopy(packet, 0, tempBuff, sendBuffer.Length, len);
            }
            SendDelayMsg(0, 0, 0, tempBuff.Length, tempBuff, 1);
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
            Array.Copy(Cmd.ToByte(), 0, TempBuf, 0, TCmdPack.PackSize);
            var sBuff = HUtil32.GetBytes(szMsg);
            Array.Copy(sBuff, 0, TempBuf, 13, sBuff.Length);
            //Move(szMsg[1], TempBuf[13], szMsg.Length);
            var iLen = TCmdPack.PackSize + szMsg.Length;
            iLen = Misc.EncodeBuf(TempBuf, iLen, SendBuf);
            SendBuf[iLen + 1] = (byte)'!';
            //m_tIOCPSender.SendData(m_pOverlapSend, SendBuf[0], iLen + 2);
            _session.Socket.Send(SendBuf, iLen + 2, SocketFlags.None);
        }

        /// <summary>
        /// 文字消息处理(过滤，已经发言间隔)
        /// </summary>
        /// <param name="sMsg"></param>
        private void FilterSayMsg(ref string sMsg)
        {
            int nLen;
            string sReplaceText;
            string sFilterText;
            try
            {
                HUtil32.EnterCriticalSection(GateShare.CS_FilterMsg);
                for (var i = 0; i < GateShare.AbuseList.Count; i++)
                {
                    sFilterText = GateShare.AbuseList[i];
                    sReplaceText = "";
                    if (sMsg.IndexOf(sFilterText, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        for (nLen = 0; nLen <= sFilterText.Length; nLen++)
                        {
                            sReplaceText = sReplaceText + GateShare.sReplaceWord;
                        }
                        sMsg = sMsg.Replace(sFilterText, sReplaceText);
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(GateShare.CS_FilterMsg);
            }
        }
    }

    public enum TCheckStep
    {
        csCheckLogin,
        csSendCheck,
        csSendSmu,
        csSendFinsh,
        csCheckTick
    }

    public class GameSpeed
    {
        /// <summary>
        /// 加速的累计值
        /// </summary>
        public int nErrorCount;
        /// <summary>
        /// 装备加速
        /// </summary>
        public int m_nHitSpeed;
        /// <summary>
        /// 发言时间
        /// </summary>
        public long dwSayMsgTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public long dwMoveTick;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public long dwAttackTick;
        /// <summary>
        /// 魔法时间
        /// </summary>
        public long dwSpellTick;
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
        public long dwButchTick;
        /// <summary>
        /// 蹲下时间
        /// </summary>
        public long dwSitDownTick;
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

        public GameSpeed()
        {

        }
    }
}