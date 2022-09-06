using GameGate.Conf;
using GameGate.Packet;
using GameGate.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;

namespace GameGate
{
    /// <summary>
    /// 用户会话封包处理
    /// </summary>
    public class ClientSession
    {
        private readonly GameSpeed _gameSpeed;
        private readonly TSessionInfo _session;
        private readonly object _syncObj;
        private readonly ClientThread _clientThread;
        private readonly HardwareFilter _hwidFilter;
        private readonly SendQueue _sendQueue;
        private readonly IList<TDelayMsg> _msgList;
        private int _lastDirection = -1;
        private byte _handleLogin = 0;
        private bool _fOverClientCount;
        private bool _kickFlag = false;
        public int m_nSvrListIdx = 0;
        private int m_nSvrObject = 0;
        private int m_nChrStutas = 0;
        private int m_SendCheckTick = 0;
        private TCheckStep m_Stat;
        private readonly long m_FinishTick = 0;

        public ClientSession(TSessionInfo session, ClientThread clientThread, SendQueue sendQueue)
        {
            _session = session;
            _clientThread = clientThread;
            _sendQueue = sendQueue;
            _msgList = new List<TDelayMsg>();
            _hwidFilter = GateShare.HWFilter;
            _fOverClientCount = false;
            _kickFlag = false;
            m_Stat = TCheckStep.CheckLogin;
            _syncObj = new object();
            _gameSpeed = new GameSpeed();
        }

        public GameSpeed GetGameSpeed()
        {
            return _gameSpeed;
        }

        public TSessionInfo Session => _session;

        private static MirLog LogQueue => MirLog.Instance;

        private GateConfig Config => ConfigManager.Instance.GateConfig;

        /// <summary>
        /// 处理客户端发送过来的封包
        /// 这里所有的包都大于12个字节
        /// </summary>
        /// <param name="message"></param>
        public void HandleSessionPacket(TMessageData message)
        {
            var sMsg = string.Empty;
            int dwCurrentTick = 0;
            if (_kickFlag)
            {
                _kickFlag = false;
                return;
            }
            if (Config.IsDefenceCCPacket && (message.BufferLen >= 5))
            {
                sMsg = HUtil32.GetString(message.Buffer, 2, message.BufferLen - 3);
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
                    SendKickMsg(12);
                }
            }
            var success = false;
            switch (_handleLogin)
            {
                case 0:
                    {
                        sMsg = HUtil32.GetString(message.Buffer, 2, message.BufferLen - 3);
                        LogQueue.EnqueueDebugging("Login Packet: " + sMsg);
                        var tempStr = EDcode.DeCodeString(sMsg);
                        HandleLogin(tempStr, message.BufferLen, "", ref success);
                        if (!success)
                        {
                            LogQueue.Enqueue("客户端登陆消息处理失败，剔除链接", 1);
                            //KickUser("ip");
                            Session.Socket.Close();
                        }
                        break;
                    }
                case >= 2:
                    {
                        if (message.BufferLen < ClientPacket.PackSize) // 小于数据包大小的为无效封包
                        {
                            _session.Socket.Close();//直接关闭异常会话
                            return;
                        }

                        var tempBuff = message.Buffer[2..^1];//跳过#1....! 只保留消息内容
                        var nDeCodeLen = 0;
                        var packBuff = Misc.DecodeBuf(tempBuff, tempBuff.Length, ref nDeCodeLen);

                        var CltCmd = Packets.ToPacket<ClientPacket>(packBuff);
                        switch (CltCmd.Cmd)
                        {
                            case Grobal2.CM_GUILDUPDATENOTICE:
                            case Grobal2.CM_GUILDUPDATERANKINFO:
                                if (message.BufferLen > Config.MaxClientPacketSize)
                                {
                                    LogQueue.Enqueue("Kick off user,over max client packet size: " + message.BufferLen, 5);
                                    // Misc.KickUser(m_pUserOBJ.nIPAddr);
                                    return;
                                }
                                break;
                            default:
                                if (message.BufferLen > Config.NomClientPacketSize)
                                {
                                    LogQueue.Enqueue("Kick off user,over nom client packet size: " + message.BufferLen, 5);
                                    // Misc.KickUser(m_pUserOBJ.nIPAddr);
                                    return;
                                }
                                break;
                        }

                        int nInterval;
                        int nMsgCount;
                        var dwDelay = 0;
                        switch (CltCmd.Cmd)
                        {
                            case Grobal2.CM_WALK:
                            case Grobal2.CM_RUN:
                                if (Config.IsMoveInterval)// 700
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    int nMoveInterval;
                                    if (_gameSpeed.SpeedLimit)
                                    {
                                        nMoveInterval = Config.MoveInterval + Config.PunishMoveInterval;
                                    }
                                    else
                                    {
                                        nMoveInterval = Config.MoveInterval;
                                    }
                                    nInterval = dwCurrentTick - _gameSpeed.dwMoveTick;
                                    if ((nInterval >= nMoveInterval))
                                    {
                                        _gameSpeed.dwMoveTick = dwCurrentTick;
                                        _gameSpeed.dwSpellTick = dwCurrentTick - Config.MoveNextSpellCompensate;
                                        if (_gameSpeed.dwAttackTick < dwCurrentTick - Config.MoveNextAttackCompensate)
                                        {
                                            _gameSpeed.dwAttackTick = dwCurrentTick - Config.MoveNextAttackCompensate;
                                        }
                                        _lastDirection = CltCmd.Dir;
                                    }
                                    else
                                    {
                                        if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                        {
                                            nMsgCount = GetDelayMsgCount();
                                            if (nMsgCount == 0)
                                            {
                                                dwDelay = Config.PunishBaseInterval + (int)Math.Round((nMoveInterval - nInterval) * Config.PunishIntervalRate);
                                                _gameSpeed.dwMoveTick = dwCurrentTick + dwDelay;
                                            }
                                            else
                                            {
                                                _gameSpeed.dwMoveTick = dwCurrentTick + (nMoveInterval - nInterval);
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
                                if (Config.IsAttackInterval)
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    int nAttackInterval;
                                    if (_gameSpeed.SpeedLimit)
                                    {
                                        nAttackInterval = Config.AttackInterval + Config.PunishAttackInterval;
                                    }
                                    else
                                    {
                                        nAttackInterval = Config.AttackInterval;
                                    }
                                    var nAttackFixInterval = HUtil32._MAX(0, (nAttackInterval - Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed));
                                    nInterval = dwCurrentTick - _gameSpeed.dwAttackTick;
                                    if ((nInterval >= nAttackFixInterval))
                                    {
                                        _gameSpeed.dwAttackTick = dwCurrentTick;
                                        if (Config.IsItemSpeedCompensate)
                                        {
                                            _gameSpeed.dwMoveTick = dwCurrentTick - (Config.AttackNextMoveCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 550
                                            _gameSpeed.dwSpellTick = dwCurrentTick - (Config.AttackNextSpellCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 1150
                                        }
                                        else
                                        {
                                            _gameSpeed.dwMoveTick = dwCurrentTick - Config.AttackNextMoveCompensate;// 550
                                            _gameSpeed.dwSpellTick = dwCurrentTick - Config.AttackNextSpellCompensate;// 1150
                                        }
                                        _lastDirection = CltCmd.Dir;
                                    }
                                    else
                                    {
                                        if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                        {
                                            nMsgCount = GetDelayMsgCount();
                                            if (nMsgCount == 0)
                                            {
                                                dwDelay = Config.PunishBaseInterval + (int)Math.Round((nAttackFixInterval - nInterval) * Config.PunishIntervalRate);
                                                _gameSpeed.dwAttackTick = dwCurrentTick + dwDelay;
                                            }
                                            else
                                            {
                                                _gameSpeed.dwAttackTick = dwCurrentTick + (nAttackFixInterval - nInterval);
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
                                if (Config.IsSpellInterval)// 1380
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    if (CltCmd.Magic >= 0128)
                                    {
                                        return;
                                    }
                                    if (TableDef.MaigicDelayArray[CltCmd.Magic]) // 过滤武士魔法
                                    {
                                        int nSpellInterval;
                                        if (_gameSpeed.SpeedLimit)
                                        {
                                            nSpellInterval = TableDef.MaigicDelayTimeList[CltCmd.Magic] + Config.PunishSpellInterval;
                                        }
                                        else
                                        {
                                            nSpellInterval = TableDef.MaigicDelayTimeList[CltCmd.Magic];
                                        }
                                        nInterval = (dwCurrentTick - _gameSpeed.dwSpellTick);
                                        if ((nInterval >= nSpellInterval))
                                        {
                                            int dwNextMove;
                                            int dwNextAtt;
                                            if (TableDef.MaigicAttackArray[CltCmd.Magic])
                                            {
                                                dwNextMove = Config.SpellNextMoveCompensate;
                                                dwNextAtt = Config.SpellNextAttackCompensate;
                                            }
                                            else
                                            {
                                                dwNextMove = Config.SpellNextMoveCompensate + 80;
                                                dwNextAtt = Config.SpellNextAttackCompensate + 80;
                                            }
                                            _gameSpeed.dwSpellTick = dwCurrentTick;
                                            _gameSpeed.dwMoveTick = dwCurrentTick - dwNextMove;
                                            _gameSpeed.dwAttackTick = dwCurrentTick - dwNextAtt;
                                            _lastDirection = CltCmd.Dir;
                                        }
                                        else
                                        {
                                            if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                            {
                                                nMsgCount = GetDelayMsgCount();
                                                if (nMsgCount == 0)
                                                {
                                                    dwDelay = Config.PunishBaseInterval + (int)Math.Round((nSpellInterval - nInterval) * Config.PunishIntervalRate);
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
                                if (Config.IsSitDownInterval)
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    nInterval = (dwCurrentTick - _gameSpeed.dwSitDownTick);
                                    if (nInterval >= Config.SitDownInterval)
                                    {
                                        _gameSpeed.dwSitDownTick = dwCurrentTick;
                                    }
                                    else
                                    {
                                        if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                        {
                                            nMsgCount = GetDelayMsgCount();
                                            if (nMsgCount == 0)
                                            {
                                                dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.SitDownInterval - nInterval) * Config.PunishIntervalRate);
                                                _gameSpeed.dwSitDownTick = dwCurrentTick + dwDelay;
                                            }
                                            else
                                            {
                                                _gameSpeed.dwSitDownTick = dwCurrentTick + (Config.SitDownInterval - nInterval);
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
                                if (Config.IsButchInterval)
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    nInterval = dwCurrentTick - _gameSpeed.dwButchTick;
                                    if (nInterval >= Config.ButchInterval)
                                    {
                                        _gameSpeed.dwButchTick = dwCurrentTick;
                                    }
                                    else
                                    {
                                        if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                        {
                                            if (!PeekDelayMsg(CltCmd.Cmd))
                                            {
                                                dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.ButchInterval - nInterval) * Config.PunishIntervalRate);
                                                _gameSpeed.dwButchTick = dwCurrentTick + dwDelay;
                                            }
                                            else
                                            {
                                                _gameSpeed.dwSitDownTick = dwCurrentTick + (Config.ButchInterval - nInterval);
                                                return;
                                            }
                                        }
                                    }
                                }
                                break;
                            case Grobal2.CM_TURN:
                                if (Config.IsTurnInterval && (Config.OverSpeedPunishMethod != TPunishMethod.TurnPack))
                                {
                                    if (_lastDirection != CltCmd.Dir)
                                    {
                                        dwCurrentTick = HUtil32.GetTickCount();
                                        if (dwCurrentTick - _gameSpeed.dwTurnTick >= Config.TurnInterval)
                                        {
                                            _lastDirection = CltCmd.Dir;
                                            _gameSpeed.dwTurnTick = dwCurrentTick;
                                        }
                                        else
                                        {
                                            if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                            {
                                                if (!PeekDelayMsg(CltCmd.Cmd))
                                                {
                                                    dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.TurnInterval - (dwCurrentTick - _gameSpeed.dwTurnTick)) * Config.PunishIntervalRate);
                                                }
                                                else
                                                {
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                }
                                break;
                            case Grobal2.CM_DEALTRY:
                                dwCurrentTick = HUtil32.GetTickCount();
                                if ((dwCurrentTick - _gameSpeed.dwDealTick < 10000))
                                {
                                    if ((dwCurrentTick - -_gameSpeed.dwWaringTick > 2000))
                                    {
                                        _gameSpeed.dwWaringTick = dwCurrentTick;
                                        SendSysMsg($"攻击状态不能交易！请稍等{(10000 - dwCurrentTick + _gameSpeed.dwDealTick) / 1000 + 1}秒……");
                                    }
                                    return;
                                }
                                break;
                            case Grobal2.CM_SAY:
                                if (Config.IsChatInterval)
                                {
                                    if (!sMsg.StartsWith("@"))
                                    {
                                        dwCurrentTick = HUtil32.GetTickCount();
                                        if (dwCurrentTick - _gameSpeed.dwSayMsgTick < Config.ChatInterval)
                                        {
                                            return;
                                        }
                                        _gameSpeed.dwSayMsgTick = dwCurrentTick;
                                    }
                                }
                                if (nDeCodeLen > ClientPacket.PackSize)
                                {
                                    if (sMsg.StartsWith("@"))
                                    {
                                        var pszChatBuffer = new byte[255];
                                        var pszChatCmd = string.Empty;
                                        // Move((nABuf + TCmdPack.PackSize), pszChatBuffer[0], nDeCodeLen - TCmdPack.PackSize);
                                        Buffer.BlockCopy(message.Buffer, ClientPacket.PackSize, pszChatBuffer, 0, nDeCodeLen - ClientPacket.PackSize);
                                        pszChatBuffer[nDeCodeLen - ClientPacket.PackSize] = (byte)'\0';
                                        var tempStr = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                        var nChatStrPos = tempStr.IndexOf(" ", StringComparison.Ordinal);
                                        // if (nChatStrPos > 0)
                                        // {
                                        //     Move(pszChatBuffer[0], pszChatCmd[0], nChatStrPos - 1);
                                        //     pszChatCmd[nChatStrPos - 1] = '\0';
                                        // }
                                        // else
                                        // {
                                        //     Move(pszChatBuffer[0], pszChatCmd[0], pszChatBuffer.Length);
                                        // }
                                        if (GateShare.ChatCommandFilter.ContainsKey(pszChatCmd))
                                        {
                                            var Cmd = new ClientPacket();
                                            Cmd.UID = m_nSvrObject;
                                            Cmd.Cmd = Grobal2.SM_WHISPER;
                                            Cmd.X = HUtil32.MakeWord(0xFF, 56);
                                            //StrFmt(pszChatBuffer, Protocol._STR_CMD_FILTER, new char[] { pszChatCmd });
                                            pszChatBuffer = HUtil32.GetBytes(string.Format(Protocol._STR_CMD_FILTER, pszChatCmd));
                                            var pszSendBuf = new byte[255];
                                            pszSendBuf[0] = (byte)'#';
                                            Buffer.BlockCopy(Cmd.GetBuffer(), 0, pszSendBuf, 0, pszSendBuf.Length);
                                            //Move(Cmd, m_pOverlapRecv.BBuffer[1], TCmdPack.PackSize);
                                            //Move(pszChatBuffer[0], m_pOverlapRecv.BBuffer[13], pszChatBuffer.Length);
                                            //var nEnCodeLen = Misc.EncodeBuf(((int)m_pOverlapRecv.BBuffer[1]), TCmdPack.PackSize + pszChatBuffer.Length, ((int)pszSendBuf[1]));
                                            //pszSendBuf[nEnCodeLen + 1] = (byte)'!';
                                            //m_tIOCPSender.SendData(m_pOverlapSend, pszSendBuf[0], nEnCodeLen + 2);
                                            return;
                                        }
                                        if (Config.IsSpaceMoveNextPickupInterval)
                                        {
                                            var buffString = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                            if (String.Compare(buffString, Config.m_szCMDSpaceMove, StringComparison.OrdinalIgnoreCase) == 0)
                                            {
                                                _gameSpeed.dwPickupTick = HUtil32.GetTickCount() + Config.SpaceMoveNextPickupInterval;
                                            }
                                        }
                                    }
                                    else if (Config.IsChatFilter)
                                    {
                                        if (sMsg.StartsWith("/"))
                                        {
                                            // Move((nABuf + TCmdPack.PackSize as string), pszChatBuffer[0], nDeCodeLen - TCmdPack.PackSize);
                                            // pszChatBuffer[nDeCodeLen - TCmdPack.PackSize] = '\0';
                                            // nChatStrPos = pszChatBuffer.IndexOf(" ");
                                            // if (nChatStrPos > 0)
                                            // {
                                            //     Move(pszChatBuffer[0], pszChatCmd[0], nChatStrPos - 1);
                                            //     pszChatCmd[nChatStrPos - 1] = '\0';
                                            //     szChatBuffer = (pszChatBuffer[nChatStrPos] as string);
                                            //     fChatFilter = AbusiveFilter.CheckChatFilter(ref szChatBuffer, ref Succeed);
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
                                        else if (!sMsg.StartsWith("@"))
                                        {
                                            // szChatBuffer = (nABuf + TCmdPack.PackSize as string);
                                            // fChatFilter = AbusiveFilter.CheckChatFilter(ref szChatBuffer, ref Succeed);
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
                                if (Config.IsPickupInterval)
                                {
                                    dwCurrentTick = HUtil32.GetTickCount();
                                    if (dwCurrentTick - _gameSpeed.dwPickupTick > Config.PickupInterval)
                                    {
                                        _gameSpeed.dwPickupTick = dwCurrentTick;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                break;
                            case Grobal2.CM_EAT:
                                if (Config.IsEatInterval)
                                {
                                    if (CltCmd.Direct == 0 || CltCmd.Direct == 1 || CltCmd.Direct == 3)
                                    {
                                        dwCurrentTick = HUtil32.GetTickCount();
                                        if (dwCurrentTick - _gameSpeed.dwEatTick > Config.EatInterval)
                                        {
                                            _gameSpeed.dwEatTick = dwCurrentTick;
                                        }
                                        else
                                        {
                                            var eatCmd = new ClientPacket();
                                            eatCmd.UID = CltCmd.UID;
                                            eatCmd.Cmd = Grobal2.SM_EAT_FAIL;
                                            var pszSendBuf = new byte[ClientPacket.PackSize];
                                            pszSendBuf[0] = (byte)'#';
                                            var nEnCodeLen = Misc.EncodeBuf(eatCmd.GetBuffer(), ClientPacket.PackSize, pszSendBuf);
                                            pszSendBuf[nEnCodeLen + 1] = (byte)'!';
                                            _clientThread.SendBuffer(pszSendBuf);
                                            return;
                                        }
                                    }
                                }
                                break;
                        }

                        byte[] BodyBuffer;
                        var cmdPack = new PacketHeader();
                        cmdPack.PacketCode = Grobal2.RUNGATECODE;
                        cmdPack.Socket = _session.SckHandle;
                        cmdPack.Ident = Grobal2.GM_DATA;
                        cmdPack.ServerIndex = m_nSvrListIdx;
                        if (nDeCodeLen > ClientPacket.PackSize)
                        {
                            var sendBuffer = new byte[message.Buffer.Length - ClientPacket.PackSize + 1];
                            var tLen = Misc.EncodeBuf(packBuff, nDeCodeLen - ClientPacket.PackSize, sendBuffer);
                            cmdPack.PackLength = ClientPacket.PackSize + tLen + 1;
                            BodyBuffer = new byte[PacketHeader.PacketSize + cmdPack.PackLength];
                            Buffer.BlockCopy(packBuff, 0, BodyBuffer, PacketHeader.PacketSize, ClientPacket.PackSize);
                            Buffer.BlockCopy(tempBuff, 16, BodyBuffer, 32, tLen); //消息体
                        }
                        else
                        {
                            BodyBuffer = new byte[PacketHeader.PacketSize + packBuff.Length];
                            cmdPack.PackLength = ClientPacket.PackSize;
                            Buffer.BlockCopy(packBuff, 0, BodyBuffer, PacketHeader.PacketSize, packBuff.Length);
                        }
                        Buffer.BlockCopy(cmdPack.GetBuffer(), 0, BodyBuffer, 0, PacketHeader.PacketSize);//复制消息头
                        _clientThread.SendBuffer(BodyBuffer);
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
            while (GetDelayMsg(ref delayMsg))
            {
                if (delayMsg.nBufLen > 0)
                {
                    _clientThread.SendBuffer(delayMsg.Buffer);//发送消息到M2
                    var dwCurrentTick = HUtil32.GetTickCount();
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
                            _gameSpeed.dwSpellTick = dwCurrentTick - Config.MoveNextSpellCompensate; //1200
                            if (_gameSpeed.dwAttackTick < dwCurrentTick - Config.MoveNextAttackCompensate)
                            {
                                _gameSpeed.dwAttackTick = dwCurrentTick - Config.MoveNextAttackCompensate; //900
                            }
                            _lastDirection = delayMsg.nDir;
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
                            if (Config.IsItemSpeedCompensate)
                            {
                                _gameSpeed.dwMoveTick = dwCurrentTick - (Config.AttackNextMoveCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 550
                                _gameSpeed.dwSpellTick = dwCurrentTick - (Config.AttackNextSpellCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 1150
                            }
                            else
                            {
                                _gameSpeed.dwMoveTick = dwCurrentTick - Config.AttackNextMoveCompensate; // 550
                                _gameSpeed.dwSpellTick = dwCurrentTick - Config.AttackNextSpellCompensate;// 1150
                            }
                            _lastDirection = delayMsg.nDir;
                            break;
                        case Grobal2.CM_SPELL:
                            _gameSpeed.dwSpellTick = dwCurrentTick;
                            int nNextMov = 0;
                            int nNextAtt = 0;
                            if (TableDef.MaigicAttackArray[delayMsg.nMag])
                            {
                                nNextMov = Config.SpellNextMoveCompensate;
                                nNextAtt = Config.SpellNextAttackCompensate;
                            }
                            else
                            {
                                nNextMov = Config.SpellNextMoveCompensate + 80;
                                nNextAtt = Config.SpellNextAttackCompensate + 80;
                            }
                            _gameSpeed.dwMoveTick = dwCurrentTick - nNextMov;// 550
                            if (_gameSpeed.dwAttackTick < dwCurrentTick - nNextAtt)// 900
                            {
                                _gameSpeed.dwAttackTick = dwCurrentTick - nNextAtt;
                            }
                            _lastDirection = delayMsg.nDir;
                            break;
                    }
                }
            }
        }

        private bool PeekDelayMsg(int nCmd)
        {
            var result = false;
            var i = 0;
            while (_msgList.Count > i)
            {
                var iCmd = _msgList[i].nCmd;
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
            HUtil32.EnterCriticalSection(_syncObj);
            var result = false;
            TDelayMsg _delayMsg = null;
            var count = 0;
            while (_msgList.Count > count)
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
                delayMsg.Buffer = _delayMsg.Buffer;
                _delayMsg = null;
                result = true;
            }
            HUtil32.LeaveCriticalSection(_syncObj);
            return result;
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
                    var bMsg = HUtil32.GetBytes(sMsg);
                    pDelayMsg.Buffer = bMsg;
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
                pDelayMsg.Buffer = pMsg;
                _msgList.Add(pDelayMsg);
            }
            if (nMid > 0)
            {
                LogQueue.EnqueueDebugging($"发送延时处理消息:User:[{_session.sChrName}] MagicID:[{nMid}] DelayTime:[{dwDelay}]");
            }
        }

        /// <summary>
        /// 处理服务端发送过来的消息并发送到游戏客户端
        /// </summary>
        public void ProcessSvrData(TMessageData message)
        {
            byte[] pzsSendBuf;
            if (message.BufferLen <= 0) //正常的游戏封包，走路 攻击等都走下面的代码
            {
                pzsSendBuf = new byte[(0 - message.BufferLen) + 2];
                pzsSendBuf[0] = (byte)'#';
                Buffer.BlockCopy(message.Buffer, 0, pzsSendBuf, 1, -message.BufferLen);
                pzsSendBuf[^1] = (byte)'!';
                _sendQueue.AddToQueue(_session, pzsSendBuf);
                Debug.WriteLine(HUtil32.GetString(pzsSendBuf, 0, pzsSendBuf.Length));
                return;
            }
            var packet = Packets.ToPacket<ClientPacket>(message.Buffer);
            switch (packet.Cmd)
            {
                case Grobal2.SM_RUSH:
                    if (m_nSvrObject == packet.UID)
                    {
                        var dwCurrentTick = HUtil32.GetTickCount();
                        _gameSpeed.dwMoveTick = dwCurrentTick;
                        _gameSpeed.dwAttackTick = dwCurrentTick;
                        _gameSpeed.dwSpellTick = dwCurrentTick;
                        _gameSpeed.dwSitDownTick = dwCurrentTick;
                        _gameSpeed.dwButchTick = dwCurrentTick;
                        _gameSpeed.dwDealTick = dwCurrentTick;
                    }
                    break;
                case Grobal2.SM_NEWMAP:
                case Grobal2.SM_CHANGEMAP:
                case Grobal2.SM_LOGON:
                    if (m_nSvrObject == 0)
                    {
                        m_nSvrObject = packet.UID;
                    }
                    break;
                case Grobal2.SM_PLAYERCONFIG:

                    break;
                case Grobal2.SM_CHARSTATUSCHANGED:
                    if (m_nSvrObject == packet.UID)
                    {
                        _gameSpeed.DefItemSpeed = packet.Direct;
                        _gameSpeed.ItemSpeed = HUtil32._MIN(Config.MaxItemSpeed, packet.Direct);
                        m_nChrStutas = HUtil32.MakeLong(packet.X, packet.Y);
                        //message.Buffer[10] = (byte)_gameSpeed.ItemSpeed; //同时限制客户端
                    }
                    break;
                case Grobal2.SM_HWID:
                    if (Config.IsProcClientHardwareID)
                    {
                        switch (packet.Series)
                        {
                            case 1:
                                LogQueue.EnqueueDebugging("封机器码");
                                break;
                            case 2:
                                LogQueue.EnqueueDebugging("清理机器码");
                                _hwidFilter.ClearDeny();
                                _hwidFilter.SaveDenyList();
                                break;
                        }
                    }
                    break;
                case Grobal2.SM_RUNGATELOGOUT:
                    SendKickMsg(2);
                    break;
            }

            pzsSendBuf = new byte[message.BufferLen + ClientPacket.PackSize];
            pzsSendBuf[0] = (byte)'#';
            var nLen = Misc.EncodeBuf(packet.GetBuffer(), ClientPacket.PackSize, pzsSendBuf, 1);
            if (message.BufferLen > ClientPacket.PackSize)
            {
                var tempBuffer = message.Buffer[ClientPacket.PackSize..];
                Buffer.BlockCopy(tempBuffer, 0, pzsSendBuf, nLen + 1, tempBuffer.Length);
                nLen = message.BufferLen - ClientPacket.PackSize + nLen;
            }
            pzsSendBuf[nLen + 1] = (byte)'!';
            pzsSendBuf = pzsSendBuf[..(nLen + 2)];
            _sendQueue.AddToQueue(_session, pzsSendBuf);
        }

        private void SendKickMsg(int killType)
        {
            var SendMsg = string.Empty;
            var defMsg = new ClientPacket();
            switch (killType)
            {
                case 0:
                    if (Config.IsKickOverSpeed)
                    {
                    }
                    SendMsg = Config.m_szOverSpeedSendBack;
                    break;
                case 1:
                    SendMsg = Config.m_szPacketDecryptFailed;
                    break;
                case 2:
                    SendMsg = "当前登录帐号正在其它位置登录，本机已被强行离线";
                    break;
                case 4: //todo 版本号验证
                    defMsg.Cmd = Grobal2.SM_VERSION_FAIL;
                    break;
                case 5:
                    SendMsg = Config.m_szOverClientCntMsg;
                    break;
                case 6:
                    SendMsg = Config.m_szHWIDBlockedMsg;
                    break;
                case 12:
                    SendMsg = "反外挂模块更新失败,请重启客户端!!!!";
                    break;
            }

            //defMsg.UID = m_nSvrObject;
            //defMsg.Cmd = Grobal2.SM_SYSMESSAGE;
            //defMsg.X = HUtil32.MakeWord(0xFF, 0xF9);
            //defMsg.Y = 0;
            //defMsg.Direct = 0;

            //byte[] TempBuf = new byte[1024];
            //byte[] SendBuf = new byte[1024];
            //SendBuf[0] = (byte)'#';
            ////Move(Cmd, TempBuf[1], TCmdPack.PackSize);
            //var iLen = 0;
            //if (!string.IsNullOrEmpty(SendMsg))
            //{
            //    //Move(SendMsg[1], TempBuf[13], SendMsg.Length);
            //    TempBuf = HUtil32.GetBytes(SendMsg);
            //    iLen = TCmdPack.PackSize + SendMsg.Length;
            //}
            //else
            //{
            //    iLen = TCmdPack.PackSize;
            //}
            //iLen = Misc.EncodeBuf(TempBuf, iLen, SendBuf);
            //SendBuf[iLen + 1] = (byte)'!';
            ////m_tIOCPSender.SendData(m_pOverlapSend, SendBuf[0], iLen + 2);
            //_session.Socket.Send(SendBuf);
            //m_KickFlag = kick;
        }

        /// <summary>
        /// 处理登录数据
        /// </summary>
        private void HandleLogin(string loginData, int nLen, string Addr, ref bool success)
        {
            const int FIRST_PAKCET_MAX_LEN = 254;
            if (nLen < FIRST_PAKCET_MAX_LEN && nLen > 15)
            {
                if (loginData[0] != '*' || loginData[1] != '*')
                {
                    LogQueue.Enqueue($"[HandleLogin] Kicked 1: {loginData}", 1);
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
                if ((sAccount.Length >= 4) && (sAccount.Length <= 12) && (sHumName.Length > 2) && (sHumName.Length < 15))
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
                    if (szClientVerNO.Length < 8)
                    {
                        LogQueue.Enqueue($"[HandleLogin] Kicked 2: {sHumName} clientVer validation failed.", 1);
                        success = false;
                        return;
                    }
                    if (szCode.Length != 10)
                    {
                        success = false;
                        return;
                    }
                    var userType = GateShare.PunishList.ContainsKey(sHumName);
                    if (userType)
                    {
                        _gameSpeed.SpeedLimit = true;
                        GateShare.PunishList[sHumName] = this;
                    }
                    var hardWareDigest = MD5.g_MD5EmptyDigest;
                    if (Config.IsProcClientHardwareID)
                    {
                        if (string.IsNullOrEmpty(szHarewareID) || (szHarewareID.Length > 256) || ((szHarewareID.Length % 2) != 0))
                        {
                            LogQueue.Enqueue($"[HandleLogin] Kicked 3: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        var Src = szHarewareID;
                        var Key = Config.ProClientHardwareKey;
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
                                    KeyPos += 1;
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
                                    TmpSrcAsc -= OffSet;
                                }
                                dest[i] = (byte)(TmpSrcAsc);
                                i++;
                                OffSet = SrcAsc;
                                SrcPos += 2;
                            } while (!(SrcPos >= Src.Length));
                        }
                        catch (Exception)
                        {
                            fMatch = true;
                        }
                        if (fMatch)
                        {
                            LogQueue.Enqueue($"[HandleLogin] Kicked 5: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        HardwareHeader pHardwareHeader = new HardwareHeader(dest);
                        //todo session会话里面需要存用户ip
                        LogQueue.Enqueue($"HWID: {MD5.MD5Print(pHardwareHeader.xMd5Digest)}  {sHumName.Trim()}  {Addr}", 1);
                        if (pHardwareHeader.dwMagicCode == 0x13F13F13)
                        {
                            if (MD5.MD5Match(MD5.g_MD5EmptyDigest, pHardwareHeader.xMd5Digest))
                            {
                                LogQueue.Enqueue($"[HandleLogin] Kicked 6: {sHumName}", 1);
                                SendKickMsg(4);
                                return;
                            }
                            hardWareDigest = pHardwareHeader.xMd5Digest;
                            if (_hwidFilter.IsFilter(hardWareDigest, ref _fOverClientCount))
                            {
                                LogQueue.Enqueue($"[HandleLogin] Kicked 7: {sHumName}", 1);
                                if (_fOverClientCount)
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
                            LogQueue.Enqueue($"[HandleLogin] Kicked 8: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                    }
                    var szTemp = $"**{sAccount}/{sHumName}/{szCert}/{szClientVerNO}/{szCode}/{MD5.MD5Print(hardWareDigest)}";
                    // #0.........!
                    var tempBuf = HUtil32.GetBytes(szTemp);
                    var pszLoginPacket = new byte[tempBuf.Length + 100];
                    var encodelen = Misc.EncodeBuf(tempBuf, tempBuf.Length, pszLoginPacket, 2);
                    pszLoginPacket[0] = (byte)'#';
                    pszLoginPacket[1] = (byte)'0';
                    pszLoginPacket[encodelen + 2] = (byte)'!';
                    _handleLogin = 2;
                    SendFirstPack(pszLoginPacket, encodelen + 3);
                    _session.sAccount = sAccount;
                    _session.sChrName = sHumName;
                    success = true;
                }
                else
                {
                    LogQueue.Enqueue($"[HandleLogin] Kicked 2: {loginData}", 1);
                    success = false;
                }
            }
            else
            {
                LogQueue.Enqueue($"[HandleLogin] Kicked 0: {loginData}", 1);
                success = false;
            }
        }

        /// <summary>
        /// 发送消息到GameSvr
        /// </summary>
        private void SendFirstPack(byte[] packet, int len = 0)
        {
            byte[] tempBuff;
            if (len == 0)
            {
                tempBuff = new byte[PacketHeader.PacketSize + packet.Length];
            }
            else
            {
                tempBuff = new byte[PacketHeader.PacketSize + len];
            }
            var GateMsg = new PacketHeader();
            GateMsg.PacketCode = Grobal2.RUNGATECODE;
            GateMsg.Socket = (int)_session.Socket.Handle;
            GateMsg.SessionId = _session.SessionId;
            GateMsg.Ident = Grobal2.GM_DATA;
            GateMsg.ServerIndex = _session.nUserListIndex;
            GateMsg.PackLength = tempBuff.Length - PacketHeader.PacketSize;//只需要发送数据封包大小即可
            var sendBuffer = GateMsg.GetBuffer();
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
            if ((_clientThread == null) || !_clientThread.IsConnected)
            {
                return;
            }
            var TempBuf = new byte[1024];
            var SendBuf = new byte[1024];
            var Cmd = new ClientPacket();
            Cmd.UID = m_nSvrObject;
            Cmd.Cmd = Grobal2.SM_SYSMESSAGE;
            Cmd.X = HUtil32.MakeWord(0xFF, 0xF9);
            Cmd.Y = 0;
            Cmd.Direct = 0;
            SendBuf[0] = (byte)'#';
            Buffer.BlockCopy(Cmd.GetBuffer(), 0, TempBuf, 0, ClientPacket.PackSize);
            var sBuff = HUtil32.GetBytes(szMsg);
            Buffer.BlockCopy(sBuff, 0, TempBuf, 13, sBuff.Length);
            var iLen = ClientPacket.PackSize + szMsg.Length;
            iLen = Misc.EncodeBuf(TempBuf, iLen, SendBuf);
            SendBuf[iLen + 1] = (byte)'!';
            _sendQueue.AddToQueue(_session, SendBuf);
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