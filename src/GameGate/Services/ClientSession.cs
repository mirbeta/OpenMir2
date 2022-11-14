using GameGate.Auth;
using GameGate.Conf;
using GameGate.Packet;
using System;
using System.Collections.Generic;
using SystemModule;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;

namespace GameGate.Services
{
    /// <summary>
    /// 用户会话封包处理
    /// </summary>
    public class ClientSession : IDisposable
    {
        private readonly SessionSpeed _gameSpeed;
        private readonly SessionInfo _session;
        private readonly object _syncObj;
        private ClientThread ClientThread { get; set; }
        private SendQueue SendQueue { get; set; }
        private IList<DelayMessage> MsgList { get; set; }
        private int LastDirection { get; set; }
        private bool HandleLogin { get; set; }
        private bool KickFlag { get; set; }
        public int SvrListIdx { get; set; }
        private int SvrObjectId { get; set; }
        private int SendCheckTick { get; set; }
        private CheckStep Stat { get; set; }
        /// <summary>
        /// 会话密钥
        /// 用于OTP动态口令验证
        /// </summary>
        private string SessionKey { get; set; }
        private long FinishTick { get; set; }
        private readonly DynamicAuthenticator _authenticator = null;

        public ClientSession(SessionInfo session, ClientThread clientThread, SendQueue sendQueue)
        {
            _session = session;
            ClientThread = clientThread;
            SendQueue = sendQueue;
            MsgList = new List<DelayMessage>();
            KickFlag = false;
            Stat = CheckStep.CheckLogin;
            LastDirection = -1;
            _syncObj = new object();
            _gameSpeed = new SessionSpeed();
            SessionKey = Guid.NewGuid().ToString("N");
            _authenticator = new DynamicAuthenticator();
        }

        public SessionSpeed GetGameSpeed()
        {
            return _gameSpeed;
        }

        public ClientThread ServerThread => ClientThread;

        public SessionInfo Session => _session;

        private static MirLog Logger => MirLog.Instance;

        private static GateConfig Config => ConfigManager.Instance.GateConfig;

        private void Kick(byte code)
        {
            Session.Socket.Close();
        }

        /// <summary>
        /// 处理客户端封包
        /// </summary>
        public void ProcessPacket(MessagePacket messagePacket)
        {
            _session.ReceiveTick = HUtil32.GetTickCount();
            var sMsg = string.Empty;
            int currentTick;
            if (KickFlag)
            {
                KickFlag = false;
                return;
            }
            if (Config.IsDefenceCCPacket && (messagePacket.BufferLen >= 5))
            {
                sMsg = HUtil32.GetString(messagePacket.Buffer.AsSpan().Slice(2, messagePacket.BufferLen - 3));
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
            if ((Stat == CheckStep.CheckLogin) || (Stat == CheckStep.SendCheck))
            {
                currentTick = HUtil32.GetTickCount();
                if (0 == SendCheckTick)
                {
                    SendCheckTick = currentTick;
                }
                if ((currentTick - SendCheckTick) > 1000 * 5)// 如果5 秒 没有回数据 就下发数据
                {
                    Stat = CheckStep.SendSmu;
                }
            }
            // 如果下发成功  得多少秒有数据如果没有的话，那就是有问题
            if ((Stat == CheckStep.SendFinsh))
            {
                currentTick = HUtil32.GetTickCount();
                if ((currentTick - FinishTick) > 1000 * 10)
                {
                    SendKickMsg(12);
                }
            }
            var success = false;
            if (HandleLogin)
            {
                if (messagePacket.BufferLen < ClientMesaagePacket.PackSize)
                {
                    _session.Socket.Close();//关闭异常会话
                    return;
                }

                Span<byte> tempBuff = messagePacket.Buffer.AsSpan()[2..^1];//跳过#1....! 只保留消息内容
                var nDeCodeLen = 0;
                var decodeBuff = PacketEncoder.DecodeBuf(tempBuff, tempBuff.Length, ref nDeCodeLen);

                var packetHeader = decodeBuff.AsSpan();

                var recog = BitConverter.ToInt32(packetHeader[..4]);
                var ident = BitConverter.ToUInt16(packetHeader.Slice(4, 2));
                var param = BitConverter.ToUInt16(packetHeader.Slice(6, 2));
                var tag = BitConverter.ToUInt16(packetHeader.Slice(8, 2));
                var series = BitConverter.ToUInt16(packetHeader.Slice(10, 2));

                //if (Config.EnableOtp)
                //{
                //    if (CltCmd.OtpCode <= 0)
                //    {
                //        LogQueue.Enqueue("动态加密口令错误，剔除链接.", 1);
                //        Kick(100);
                //        return;
                //    }
                //    var authSuccess = _authenticator.ValidateTwoFactorPIN(SessionKey, CltCmd.OtpCode.ToString());
                //    if (!authSuccess)
                //    {
                //        LogQueue.Enqueue("动态加密口令验证失效,剔除链接.", 1);
                //        Kick(100);
                //        return;
                //    }
                //}
                switch (ident)
                {
                    case Grobal2.CM_GUILDUPDATENOTICE:
                    case Grobal2.CM_GUILDUPDATERANKINFO:
                        if (messagePacket.BufferLen > Config.MaxClientPacketSize)
                        {
                            Logger.Log("Kick off user,over max client packet size: " + messagePacket.BufferLen, 5);
                            // Misc.KickUser(m_pUserOBJ.nIPAddr);
                            return;
                        }
                        break;
                    default:
                        if (messagePacket.BufferLen > Config.NomClientPacketSize)
                        {
                            Logger.Log("Kick off user,over nom client packet size: " + messagePacket.BufferLen, 5);
                            // Misc.KickUser(m_pUserOBJ.nIPAddr);
                            return;
                        }
                        break;
                }

                int nInterval;
                int delayMsgCount;
                int dwDelay;
                switch (ident)
                {
                    case Grobal2.CM_WALK:
                    case Grobal2.CM_RUN:
                        if (Config.IsMoveInterval)// 700
                        {
                            currentTick = HUtil32.GetTickCount();
                            int nMoveInterval;
                            if (_gameSpeed.SpeedLimit)
                            {
                                nMoveInterval = Config.MoveInterval + Config.PunishMoveInterval;
                            }
                            else
                            {
                                nMoveInterval = Config.MoveInterval;
                            }
                            nInterval = currentTick - _gameSpeed.MoveTick;
                            if ((nInterval >= nMoveInterval))
                            {
                                _gameSpeed.MoveTick = currentTick;
                                _gameSpeed.SpellTick = currentTick - Config.MoveNextSpellCompensate;
                                if (_gameSpeed.AttackTick < currentTick - Config.MoveNextAttackCompensate)
                                {
                                    _gameSpeed.AttackTick = currentTick - Config.MoveNextAttackCompensate;
                                }
                                LastDirection = tag;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                {
                                    delayMsgCount = GetDelayMsgCount();
                                    if (delayMsgCount == 0)
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((nMoveInterval - nInterval) * Config.PunishIntervalRate);
                                        _gameSpeed.MoveTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        _gameSpeed.MoveTick = currentTick + (nMoveInterval - nInterval);
                                        if (delayMsgCount >= 2)
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
                            currentTick = HUtil32.GetTickCount();
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
                            nInterval = currentTick - _gameSpeed.AttackTick;
                            if ((nInterval >= nAttackFixInterval))
                            {
                                _gameSpeed.AttackTick = currentTick;
                                if (Config.IsItemSpeedCompensate)
                                {
                                    _gameSpeed.MoveTick = currentTick - (Config.AttackNextMoveCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 550
                                    _gameSpeed.SpellTick = currentTick - (Config.AttackNextSpellCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 1150
                                }
                                else
                                {
                                    _gameSpeed.MoveTick = currentTick - Config.AttackNextMoveCompensate;// 550
                                    _gameSpeed.SpellTick = currentTick - Config.AttackNextSpellCompensate;// 1150
                                }
                                LastDirection = tag;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                {
                                    delayMsgCount = GetDelayMsgCount();
                                    if (delayMsgCount == 0)
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((nAttackFixInterval - nInterval) * Config.PunishIntervalRate);
                                        _gameSpeed.AttackTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        _gameSpeed.AttackTick = currentTick + (nAttackFixInterval - nInterval);
                                        if (delayMsgCount >= 2)
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
                            currentTick = HUtil32.GetTickCount();
                            if (tag >= 0128)
                            {
                                return;
                            }
                            if (TableDef.MaigicDelayArray[tag]) // 过滤武士魔法
                            {
                                int nSpellInterval;
                                if (_gameSpeed.SpeedLimit)
                                {
                                    nSpellInterval = TableDef.MaigicDelayTimeList[tag] + Config.PunishSpellInterval;
                                }
                                else
                                {
                                    nSpellInterval = TableDef.MaigicDelayTimeList[tag];
                                }
                                nInterval = (currentTick - _gameSpeed.SpellTick);
                                if ((nInterval >= nSpellInterval))
                                {
                                    int dwNextMove;
                                    int dwNextAtt;
                                    if (TableDef.MaigicAttackArray[tag])
                                    {
                                        dwNextMove = Config.SpellNextMoveCompensate;
                                        dwNextAtt = Config.SpellNextAttackCompensate;
                                    }
                                    else
                                    {
                                        dwNextMove = Config.SpellNextMoveCompensate + 80;
                                        dwNextAtt = Config.SpellNextAttackCompensate + 80;
                                    }
                                    _gameSpeed.SpellTick = currentTick;
                                    _gameSpeed.MoveTick = currentTick - dwNextMove;
                                    _gameSpeed.AttackTick = currentTick - dwNextAtt;
                                    LastDirection = tag;
                                }
                                else
                                {
                                    if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                    {
                                        delayMsgCount = GetDelayMsgCount();
                                        if (delayMsgCount == 0)
                                        {
                                            dwDelay = Config.PunishBaseInterval + (int)Math.Round((nSpellInterval - nInterval) * Config.PunishIntervalRate);
                                        }
                                        else
                                        {
                                            if (delayMsgCount >= 2)
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
                            currentTick = HUtil32.GetTickCount();
                            nInterval = (currentTick - _gameSpeed.SitDownTick);
                            if (nInterval >= Config.SitDownInterval)
                            {
                                _gameSpeed.SitDownTick = currentTick;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                {
                                    delayMsgCount = GetDelayMsgCount();
                                    if (delayMsgCount == 0)
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.SitDownInterval - nInterval) * Config.PunishIntervalRate);
                                        _gameSpeed.SitDownTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        _gameSpeed.SitDownTick = currentTick + (Config.SitDownInterval - nInterval);
                                        if (delayMsgCount >= 2)
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
                            currentTick = HUtil32.GetTickCount();
                            nInterval = currentTick - _gameSpeed.ButchTick;
                            if (nInterval >= Config.ButchInterval)
                            {
                                _gameSpeed.ButchTick = currentTick;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                {
                                    if (!PeekDelayMsg(ident))
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.ButchInterval - nInterval) * Config.PunishIntervalRate);
                                        _gameSpeed.ButchTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        _gameSpeed.SitDownTick = currentTick + (Config.ButchInterval - nInterval);
                                        return;
                                    }
                                }
                            }
                        }
                        break;
                    case Grobal2.CM_TURN:
                        if (Config.IsTurnInterval && (Config.OverSpeedPunishMethod != TPunishMethod.TurnPack))
                        {
                            if (LastDirection != tag)
                            {
                                currentTick = HUtil32.GetTickCount();
                                if (currentTick - _gameSpeed.TurnTick >= Config.TurnInterval)
                                {
                                    LastDirection = tag;
                                    _gameSpeed.TurnTick = currentTick;
                                }
                                else
                                {
                                    if (Config.OverSpeedPunishMethod == TPunishMethod.DelaySend)
                                    {
                                        if (!PeekDelayMsg(ident))
                                        {
                                            dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.TurnInterval - (currentTick - _gameSpeed.TurnTick)) * Config.PunishIntervalRate);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case Grobal2.CM_DEALTRY:
                        currentTick = HUtil32.GetTickCount();
                        if ((currentTick - _gameSpeed.DealTick < 10000))
                        {
                            if ((currentTick - -_gameSpeed.WaringTick > 2000))
                            {
                                _gameSpeed.WaringTick = currentTick;
                                SendSysMsg($"攻击状态不能交易！请稍等{(10000 - currentTick + _gameSpeed.DealTick) / 1000 + 1}秒……");
                            }
                            return;
                        }
                        break;
                    case Grobal2.CM_SAY:
                        if (Config.IsChatInterval)
                        {
                            if (!sMsg.StartsWith("@"))
                            {
                                currentTick = HUtil32.GetTickCount();
                                if (currentTick - _gameSpeed.SayMsgTick < Config.ChatInterval)
                                {
                                    return;
                                }
                                _gameSpeed.SayMsgTick = currentTick;
                            }
                        }
                        if (nDeCodeLen > ClientMesaagePacket.PackSize)
                        {
                            if (sMsg.StartsWith("@"))
                            {
                                var pszChatBuffer = new byte[255];
                                var pszChatCmd = string.Empty;
                                // Move((nABuf + TCmdPack.PackSize), pszChatBuffer[0], nDeCodeLen - TCmdPack.PackSize);
                                Buffer.BlockCopy(messagePacket.Buffer, ClientMesaagePacket.PackSize, pszChatBuffer, 0, nDeCodeLen - ClientMesaagePacket.PackSize);
                                pszChatBuffer[nDeCodeLen - ClientMesaagePacket.PackSize] = (byte)'\0';
                                var tempStr = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                var nChatStrPos = tempStr.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
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
                                    var cmd = new ClientMesaagePacket();
                                    cmd.UID = SvrObjectId;
                                    cmd.Cmd = Grobal2.SM_WHISPER;
                                    cmd.X = HUtil32.MakeWord(0xFF, 56);
                                    //StrFmt(pszChatBuffer, Protocol._STR_CMD_FILTER, new char[] { pszChatCmd });
                                    pszChatBuffer = HUtil32.GetBytes(string.Format(Protocol._STR_CMD_FILTER, pszChatCmd));
                                    var pszSendBuf = new byte[255];
                                    pszSendBuf[0] = (byte)'#';
                                    Buffer.BlockCopy(cmd.GetBuffer(), 0, pszSendBuf, 0, pszSendBuf.Length);
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
                                    if (string.Compare(buffString, Config.m_szCMDSpaceMove, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        _gameSpeed.PickupTick = HUtil32.GetTickCount() + Config.SpaceMoveNextPickupInterval;
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
                            currentTick = HUtil32.GetTickCount();
                            if (currentTick - _gameSpeed.PickupTick > Config.PickupInterval)
                            {
                                _gameSpeed.PickupTick = currentTick;
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
                            if (series == 0 || series == 1 || series == 3)
                            {
                                currentTick = HUtil32.GetTickCount();
                                if (currentTick - _gameSpeed.EatTick > Config.EatInterval)
                                {
                                    _gameSpeed.EatTick = currentTick;
                                }
                                else
                                {
                                    var eatPacket = new ClientMesaagePacket();
                                    eatPacket.UID = recog;
                                    eatPacket.Cmd = Grobal2.SM_EAT_FAIL;
                                    var pszSendBuf = new byte[ClientMesaagePacket.PackSize];
                                    pszSendBuf[0] = (byte)'#';
                                    var nEnCodeLen = PacketEncoder.EncodeBuf(eatPacket.GetBuffer(), ClientMesaagePacket.PackSize, pszSendBuf);
                                    pszSendBuf[nEnCodeLen + 1] = (byte)'!';
                                    ClientThread.SendBuffer(pszSendBuf);
                                    return;
                                }
                            }
                        }
                        break;
                }

                //todo 需要优化
                byte[] bodyBuffer;
                var cmdPack = new GameServerPacket();
                cmdPack.PacketCode = Grobal2.RUNGATECODE;
                cmdPack.Socket = _session.SckHandle;
                cmdPack.Ident = Grobal2.GM_DATA;
                cmdPack.ServerIndex = SvrListIdx;
                if (nDeCodeLen > ClientMesaagePacket.PackSize)
                {
                    var sendBuffer = new byte[messagePacket.Buffer.Length - ClientMesaagePacket.PackSize + 1];
                    var tLen = PacketEncoder.EncodeBuf(decodeBuff, nDeCodeLen - ClientMesaagePacket.PackSize, sendBuffer);
                    cmdPack.PackLength = ClientMesaagePacket.PackSize + tLen + 1;
                    bodyBuffer = new byte[GameServerPacket.PacketSize + cmdPack.PackLength];
                    Buffer.BlockCopy(decodeBuff, 0, bodyBuffer, GameServerPacket.PacketSize, ClientMesaagePacket.PackSize);
                    Buffer.BlockCopy(tempBuff.ToArray(), 16, bodyBuffer, 32, tLen); //消息体
                }
                else
                {
                    bodyBuffer = new byte[GameServerPacket.PacketSize + decodeBuff.Length];
                    cmdPack.PackLength = ClientMesaagePacket.PackSize;
                    Buffer.BlockCopy(decodeBuff, 0, bodyBuffer, GameServerPacket.PacketSize, decodeBuff.Length);
                }
                Buffer.BlockCopy(cmdPack.GetBuffer(), 0, bodyBuffer, 0, GameServerPacket.PacketSize);//复制消息头
                ClientThread.SendBuffer(bodyBuffer);
            }
            else
            {
                var tempStr = EDCode.DeCodeString(messagePacket.Buffer.AsSpan().Slice(2, messagePacket.BufferLen - 3));
                ClientLogin(tempStr, messagePacket.BufferLen, "", ref success);
                if (!success)
                {
                    Logger.Log("客户端登陆消息处理失败，剔除链接", 1);
                    Kick(1);
                }
            }
        }

        /// <summary>
        /// 处理延时消息
        /// </summary>
        public void ProcessDelayMessage()
        {
            if (GetDelayMsgCount() <= 0)
            {
                return;
            }
            DelayMessage delayMsg = null;
            while (GetDelayMessage(ref delayMsg))
            {
                if (delayMsg.BufLen > 0)
                {
                    ClientThread.SendBuffer(delayMsg.Buffer);//发送消息到GameSvr
                    var dwCurrentTick = HUtil32.GetTickCount();
                    switch (delayMsg.Cmd)
                    {
                        case Grobal2.CM_BUTCH:
                            _gameSpeed.ButchTick = dwCurrentTick;
                            break;
                        case Grobal2.CM_SITDOWN:
                            _gameSpeed.SitDownTick = dwCurrentTick;
                            break;
                        case Grobal2.CM_TURN:
                            _gameSpeed.TurnTick = dwCurrentTick;
                            break;
                        case Grobal2.CM_WALK:
                        case Grobal2.CM_RUN:
                            _gameSpeed.MoveTick = dwCurrentTick;
                            _gameSpeed.SpellTick = dwCurrentTick - Config.MoveNextSpellCompensate; //1200
                            if (_gameSpeed.AttackTick < dwCurrentTick - Config.MoveNextAttackCompensate)
                            {
                                _gameSpeed.AttackTick = dwCurrentTick - Config.MoveNextAttackCompensate; //900
                            }
                            LastDirection = delayMsg.Dir;
                            break;
                        case Grobal2.CM_HIT:
                        case Grobal2.CM_HEAVYHIT:
                        case Grobal2.CM_BIGHIT:
                        case Grobal2.CM_POWERHIT:
                        case Grobal2.CM_LONGHIT:
                        case Grobal2.CM_WIDEHIT:
                        case Grobal2.CM_CRSHIT:
                        case Grobal2.CM_FIREHIT:
                            if (_gameSpeed.AttackTick < dwCurrentTick)
                            {
                                _gameSpeed.AttackTick = dwCurrentTick;
                            }
                            if (Config.IsItemSpeedCompensate)
                            {
                                _gameSpeed.MoveTick = dwCurrentTick - (Config.AttackNextMoveCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 550
                                _gameSpeed.SpellTick = dwCurrentTick - (Config.AttackNextSpellCompensate + Config.MaxItemSpeedRate * _gameSpeed.ItemSpeed);// 1150
                            }
                            else
                            {
                                _gameSpeed.MoveTick = dwCurrentTick - Config.AttackNextMoveCompensate; // 550
                                _gameSpeed.SpellTick = dwCurrentTick - Config.AttackNextSpellCompensate;// 1150
                            }
                            LastDirection = delayMsg.Dir;
                            break;
                        case Grobal2.CM_SPELL:
                            _gameSpeed.SpellTick = dwCurrentTick;
                            int nNextMov;
                            int nNextAtt;
                            if (TableDef.MaigicAttackArray[delayMsg.Mag])
                            {
                                nNextMov = Config.SpellNextMoveCompensate;
                                nNextAtt = Config.SpellNextAttackCompensate;
                            }
                            else
                            {
                                nNextMov = Config.SpellNextMoveCompensate + 80;
                                nNextAtt = Config.SpellNextAttackCompensate + 80;
                            }
                            _gameSpeed.MoveTick = dwCurrentTick - nNextMov;// 550
                            if (_gameSpeed.AttackTick < dwCurrentTick - nNextAtt)// 900
                            {
                                _gameSpeed.AttackTick = dwCurrentTick - nNextAtt;
                            }
                            LastDirection = delayMsg.Dir;
                            break;
                    }
                }
            }
        }

        private bool PeekDelayMsg(int nCmd)
        {
            var result = false;
            var i = 0;
            while (MsgList.Count > i)
            {
                var iCmd = MsgList[i].Cmd;
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
            return MsgList.Count;
        }

        /// <summary>
        /// 获取延时消息
        /// </summary>
        private bool GetDelayMessage(ref DelayMessage delayMsg)
        {
            HUtil32.EnterCriticalSection(_syncObj);
            var result = false;
            var count = 0;
            while (MsgList.Count > count)
            {
                DelayMessage msg = MsgList[count];
                if (msg.DelayTime != 0 && HUtil32.GetTickCount() < msg.DelayTime)
                {
                    count++;
                    continue;
                }
                MsgList.RemoveAt(count);
                delayMsg = new DelayMessage();
                delayMsg.Mag = msg.Mag;
                delayMsg.Dir = msg.Dir;
                delayMsg.Cmd = msg.Cmd;
                delayMsg.BufLen = msg.BufLen;
                delayMsg.Buffer = msg.Buffer;
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
            const int delayBufferLen = 1024;
            if (nLen > 0 && nLen <= delayBufferLen)
            {
                var pDelayMsg = new DelayMessage();
                pDelayMsg.Mag = nMid;
                pDelayMsg.Dir = nDir;
                pDelayMsg.Cmd = nIdx;
                pDelayMsg.DelayTime = HUtil32.GetTickCount() + dwDelay;
                pDelayMsg.BufLen = nLen;
                if (!string.IsNullOrEmpty(sMsg))
                {
                    var bMsg = HUtil32.GetBytes(sMsg);
                    pDelayMsg.Buffer = bMsg;
                }
                MsgList.Add(pDelayMsg);
            }
        }

        /// <summary>
        /// 发送延时处理消息
        /// </summary>
        private void SendDelayMsg(int magicId, byte nDir, int nIdx, int nLen, byte[] pMsg, int delayTime)
        {
            const int delayBufferLen = 1024;
            if (nLen > 0 && nLen <= delayBufferLen)
            {
                var pDelayMsg = new DelayMessage();
                pDelayMsg.Mag = magicId;
                pDelayMsg.Dir = nDir;
                pDelayMsg.Cmd = nIdx;
                pDelayMsg.DelayTime = HUtil32.GetTickCount() + delayTime;
                pDelayMsg.BufLen = nLen;
                pDelayMsg.Buffer = pMsg;
                MsgList.Add(pDelayMsg);
            }
            if (magicId > 0)
            {
                Logger.DebugLog($"发送延时处理消息:User:[{_session.ChrName}] MagicID:[{magicId}] DelayTime:[{delayTime}]");
            }
        }

        public static bool EqualsBytes(byte[] obj, byte[] target)
        {
            if (obj.Length != target.Length)
                return false;
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] != target[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void SendPacketData(byte[] packtData)
        {
            SendQueue.AddClientQueue(_session.ConnectionId, _session.ThreadId, packtData);
        }

        /// <summary>
        /// 处理GameSvr消息 
        /// 处理后发送到游戏客户端
        /// </summary>
        public void ProcessServerPacket(MessagePacket clientPacket)
        {
            switch (clientPacket.BufferLen)
            {
                case < 0://小包 走路 攻击等
                    {
                        var buffLen = -clientPacket.BufferLen;
                        var packetBuffer = new byte[buffLen + 2];
                        packetBuffer[0] = (byte)'#';
                        Buffer.BlockCopy(clientPacket.Buffer, 0, packetBuffer, 1, buffLen);
                        packetBuffer[buffLen + 1] = (byte)'!';
                        var sendData = packetBuffer[..(buffLen + 2)];
                        SendPacketData(sendData);
                        break;
                    }
                case < 1024://普通正常游戏数据包，正常的游戏操作
                    {
                        var packetBuffer = new byte[clientPacket.BufferLen + ClientMesaagePacket.PackSize];
                        packetBuffer[0] = (byte)'#';
                        var packetBuff = clientPacket.Buffer;
                        var nLen = PacketEncoder.EncodeBuf(packetBuff, ClientMesaagePacket.PackSize, packetBuffer, 1);//消息头
                        if (clientPacket.BufferLen > ClientMesaagePacket.PackSize)
                        {
                            var tempBuffer = packetBuff[ClientMesaagePacket.PackSize..];
                            MemoryCopy.BlockCopy(tempBuffer, 0, packetBuffer, nLen + 1, tempBuffer.Length);
                            nLen = tempBuffer.Length + nLen;
                        }
                        packetBuffer[nLen + 1] = (byte)'!';
                        var sendData = packetBuffer[..(nLen + 2)];
                        SendPacketData(sendData);
                        break;
                    }
                case > 1024://大型游戏数据包
                    {
                        var packBuff = new byte[clientPacket.BufferLen + ClientMesaagePacket.PackSize];
                        packBuff[0] = (byte)'#';
                        var packetBuff = clientPacket.Buffer;
                        var nLen = PacketEncoder.EncodeBuf(packetBuff, ClientMesaagePacket.PackSize, packBuff, 1);//消息头
                        if (clientPacket.BufferLen > ClientMesaagePacket.PackSize)
                        {
                            var tempBuffer = packetBuff[ClientMesaagePacket.PackSize..];
                            MemoryCopy.BlockCopy(tempBuffer, 0, packBuff, nLen + 1, tempBuffer.Length);
                            nLen = tempBuffer.Length + nLen;
                        }
                        packBuff[nLen + 1] = (byte)'!';
                        var sendData = packBuff[..(nLen + 2)];
                        SendPacketData(sendData);
                        break;
                    }
            }

            var messagePacket = clientPacket.Buffer.AsSpan();
            if (messagePacket.Length > 10)
            {
                var recog = BitConverter.ToInt32(messagePacket[..4]);
                var ident = BitConverter.ToUInt16(messagePacket.Slice(4, 2));
                var param = BitConverter.ToUInt16(messagePacket.Slice(6, 2));
                var tag = BitConverter.ToUInt16(messagePacket.Slice(8, 2));
                int series;
                switch (ident)
                {
                    case Grobal2.SM_RUSH:
                        if (SvrObjectId == recog)
                        {
                            var dwCurrentTick = HUtil32.GetTickCount();
                            _gameSpeed.MoveTick = dwCurrentTick;
                            _gameSpeed.AttackTick = dwCurrentTick;
                            _gameSpeed.SpellTick = dwCurrentTick;
                            _gameSpeed.SitDownTick = dwCurrentTick;
                            _gameSpeed.ButchTick = dwCurrentTick;
                            _gameSpeed.DealTick = dwCurrentTick;
                        }
                        break;
                    case Grobal2.SM_NEWMAP:
                    case Grobal2.SM_CHANGEMAP:
                    case Grobal2.SM_LOGON:
                        if (SvrObjectId == 0)
                        {
                            SvrObjectId = recog;
                        }
                        break;
                    case Grobal2.SM_PLAYERCONFIG:

                        break;
                    case Grobal2.SM_CHARSTATUSCHANGED:
                        series = BitConverter.ToUInt16(messagePacket.Slice(10, 2));
                        if (SvrObjectId == recog)
                        {
                            _gameSpeed.DefItemSpeed = series;
                            _gameSpeed.ItemSpeed = HUtil32._MIN(Config.MaxItemSpeed, series);
                            //_mNChrStutas = HUtil32.MakeLong(param, tag);
                            //message.Buffer[10] = (byte)_gameSpeed.ItemSpeed; //同时限制客户端
                        }
                        break;
                    case Grobal2.SM_HWID:
                        series = BitConverter.ToUInt16(messagePacket.Slice(10, 2));
                        if (Config.IsProcClientHardwareID)
                        {
                            switch (series)
                            {
                                case 1:
                                    Logger.DebugLog("封机器码");
                                    break;
                                case 2:
                                    Logger.DebugLog("清理机器码");
                                    GateShare.HardwareFilter.ClearDeny();
                                    GateShare.HardwareFilter.SaveDenyList();
                                    break;
                            }
                        }
                        break;
                    case Grobal2.SM_RUNGATELOGOUT:
                        SendKickMsg(2);
                        break;
                }
            }
        }

        private void SendKickMsg(int killType)
        {
            var sendMsg = string.Empty;
            var defMsg = new ClientMesaagePacket();
            switch (killType)
            {
                case 0:
                    if (Config.IsKickOverSpeed)
                    {
                    }
                    sendMsg = Config.m_szOverSpeedSendBack;
                    break;
                case 1:
                    sendMsg = Config.m_szPacketDecryptFailed;
                    break;
                case 2:
                    sendMsg = "当前登录帐号正在其它位置登录，本机已被强行离线";
                    break;
                case 4: //todo 版本号验证
                    defMsg.Cmd = Grobal2.SM_VERSION_FAIL;
                    break;
                case 5:
                    sendMsg = Config.m_szOverClientCntMsg;
                    break;
                case 6:
                    sendMsg = Config.m_szHWIDBlockedMsg;
                    break;
                case 12:
                    sendMsg = "反外挂模块更新失败,请重启客户端!!!!";
                    break;
            }

            Logger.DebugLog(sendMsg);

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
        private void ClientLogin(string loginData, int nLen, string addr, ref bool success)
        {
            const int firstPakcetMaxLen = 254;
            if (nLen < firstPakcetMaxLen && nLen > 15)
            {
                Logger.DebugLog("ClientLogin: " + loginData);
                if (loginData[0] != '*' || loginData[1] != '*')
                {
                    Logger.Log($"[ClientLogin] Kicked 1: {loginData}", 1);
                    success = false;
                    return;
                }
                var sDataText = loginData.AsSpan()[2..].ToString();
                var sHumName = string.Empty;//人物名称
                var sAccount = string.Empty;//账号
                var szCert = string.Empty;
                var szClientVerNo = string.Empty;//客户端版本号
                var szCode = string.Empty;
                var szHarewareId = string.Empty;//硬件ID

                sDataText = HUtil32.GetValidStr3(sDataText, ref sAccount, HUtil32.Backslash);
                sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, HUtil32.Backslash);

                if ((sAccount.Length >= 4) && (sAccount.Length <= 12) && (sHumName.Length > 2) && (sHumName.Length < 15))
                {
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szCert, HUtil32.Backslash);
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szClientVerNo, HUtil32.Backslash);
                    sDataText = HUtil32.GetValidStr3(sDataText, ref szCode, HUtil32.Backslash);
                    HUtil32.GetValidStr3(sDataText, ref szHarewareId, HUtil32.Backslash);
                    if (szCert.Length <= 0 || szCert.Length > 8)
                    {
                        success = false;
                        return;
                    }
                    if (szClientVerNo.Length < 8)
                    {
                        Logger.Log($"[ClientLogin] Kicked 2: {sHumName} clientVer validation failed.", 1);
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
                    var hardWareDigest = MD5.EmptyDigest;
                    if (Config.IsProcClientHardwareID)
                    {
                        if (string.IsNullOrEmpty(szHarewareId) || (szHarewareId.Length > 256) || ((szHarewareId.Length % 2) != 0))
                        {
                            Logger.Log($"[ClientLogin] Kicked 3: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        var src = szHarewareId;
                        var key = Config.ProClientHardwareKey;
                        var keyLen = key.Length;
                        var keyPos = 0;
                        var offSet = Convert.ToInt32("$" + src.Substring(0, 2));
                        var srcPos = 3;
                        var i = 0;
                        int srcAsc;
                        int tmpSrcAsc;
                        var dest = new byte[1024];
                        var fMatch = false;
                        try
                        {
                            do
                            {
                                srcAsc = Convert.ToInt32("$" + src.Substring(srcPos - 1, 2));
                                if (keyPos < keyLen)
                                {
                                    keyPos += 1;
                                }
                                else
                                {
                                    keyPos = 1;
                                }
                                tmpSrcAsc = srcAsc ^ key[keyPos];
                                if (tmpSrcAsc <= offSet)
                                {
                                    tmpSrcAsc = 255 + tmpSrcAsc - offSet;
                                }
                                else
                                {
                                    tmpSrcAsc -= offSet;
                                }
                                dest[i] = (byte)(tmpSrcAsc);
                                i++;
                                offSet = srcAsc;
                                srcPos += 2;
                            } while (!(srcPos >= src.Length));
                        }
                        catch (Exception)
                        {
                            fMatch = true;
                        }
                        if (fMatch)
                        {
                            Logger.Log($"[ClientLogin] Kicked 5: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        HardwareHeader pHardwareHeader = Packets.ToPacket<HardwareHeader>(dest);
                        //todo session会话里面需要存用户ip
                        Logger.Log($"HWID: {MD5.MD5Print(pHardwareHeader.xMd5Digest)}  {sHumName.Trim()}  {addr}", 1);
                        if (pHardwareHeader.dwMagicCode == 0x13F13F13)
                        {
                            if (MD5.MD5Match(MD5.EmptyDigest, pHardwareHeader.xMd5Digest))
                            {
                                Logger.Log($"[ClientLogin] Kicked 6: {sHumName}", 1);
                                SendKickMsg(4);
                                return;
                            }
                            hardWareDigest = pHardwareHeader.xMd5Digest;
                            bool overClientCount = false;
                            if (GateShare.HardwareFilter.IsFilter(hardWareDigest, ref overClientCount))
                            {
                                Logger.Log($"[ClientLogin] Kicked 7: {sHumName}", 1);
                                if (overClientCount)
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
                            Logger.Log($"[ClientLogin] Kicked 8: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                    }
                    var loginPacket = $"**{sAccount}/{sHumName}/{szCert}/{szClientVerNo}/{szCode}/{MD5.MD5Print(hardWareDigest)}";
                    var tempBuf = HUtil32.GetBytes(loginPacket);
                    Span<byte> pszLoginPacket = stackalloc byte[tempBuf.Length + 100];
                    var encodeLen = PacketEncoder.EncodeBuf(tempBuf, tempBuf.Length, pszLoginPacket, 2);
                    pszLoginPacket[0] = (byte)'#';
                    pszLoginPacket[1] = (byte)'0';
                    pszLoginPacket[encodeLen + 2] = (byte)'!';
                    _session.Account = sAccount;
                    _session.ChrName = sHumName;
                    SendLoginPacket(pszLoginPacket, encodeLen + 3);
                    success = true;
                    HandleLogin = true;
                    /*var secretKey = _authenticator.GenerateSetupCode("openmir2", sAccount, SessionKey, 5);
                    Logger.Log($"动态密钥:{secretKey.AccountSecretKey}", 1);
                    Logger.Log($"动态验证码：{secretKey.ManualEntryKey}", 1);
                    Logger.Log($"{_authenticator.DefaultClockDriftTolerance.TotalMilliseconds}秒后验证新的密钥,容错5秒.", 1);*/
                }
                else
                {
                    Logger.Log($"[ClientLogin] Kicked 2: {loginData}", 1);
                    success = false;
                }
            }
            else
            {
                Logger.Log($"[ClientLogin] Kicked 0: {loginData}", 1);
                success = false;
            }
        }

        /// <summary>
        /// 发送登录验证封包
        /// </summary>
        private void SendLoginPacket(Span<byte> packet, int len = 0)
        {
            byte[] tempBuff;
            if (len == 0)
            {
                tempBuff = new byte[GameServerPacket.PacketSize + packet.Length];
            }
            else
            {
                tempBuff = new byte[GameServerPacket.PacketSize + len];
            }
            var packetHeader = new GameServerPacket();
            packetHeader.PacketCode = Grobal2.RUNGATECODE;
            packetHeader.Socket = (int)_session.Socket.Handle;
            packetHeader.SessionId = _session.SessionId;
            packetHeader.Ident = Grobal2.GM_DATA;
            packetHeader.ServerIndex = _session.UserListIndex;
            packetHeader.PackLength = tempBuff.Length - GameServerPacket.PacketSize;
            var sendBuffer = packetHeader.GetBuffer();
            MemoryCopy.FastCopy(sendBuffer, 0, tempBuff, 0, sendBuffer.Length);
            if (len == 0)
            {
                MemoryCopy.CopyMemory(packet, 0, tempBuff, sendBuffer.Length, packet.Length);
            }
            else
            {
                MemoryCopy.CopyMemory(packet, 0, tempBuff, sendBuffer.Length, len);
            }
            SendDelayMsg(0, 0, 0, tempBuff.Length, tempBuff, 1);
        }

        private void SendSysMsg(string szMsg)
        {
            if ((ClientThread == null) || !ClientThread.IsConnected)
            {
                return;
            }
            var tempBuf = new byte[1024];
            var sendBuf = new byte[1024];
            var clientPacket = new ClientMesaagePacket();
            clientPacket.UID = SvrObjectId;
            clientPacket.Cmd = Grobal2.SM_SYSMESSAGE;
            clientPacket.X = HUtil32.MakeWord(0xFF, 0xF9);
            clientPacket.Y = 0;
            clientPacket.Direct = 0;
            sendBuf[0] = (byte)'#';
            Buffer.BlockCopy(clientPacket.GetBuffer(), 0, tempBuf, 0, ClientMesaagePacket.PackSize);
            var sBuff = HUtil32.GetBytes(szMsg);
            Buffer.BlockCopy(sBuff, 0, tempBuf, 13, sBuff.Length);
            var iLen = ClientMesaagePacket.PackSize + szMsg.Length;
            iLen = PacketEncoder.EncodeBuf(tempBuf, iLen, sendBuf);
            sendBuf[iLen + 1] = (byte)'!';
            SendQueue.AddClientQueue(_session.ConnectionId, _session.ThreadId, sendBuf[..(iLen + 1)]);
        }

        public void Dispose()
        {
            //todo 
        }
    }

    public enum CheckStep : byte
    {
        CheckLogin,
        SendCheck,
        SendSmu,
        SendFinsh,
        CheckTick
    }

    public class SessionSpeed
    {
        /// <summary>
        /// 是否速度限制
        /// </summary>
        public bool SpeedLimit;
        /// <summary>
        /// 最高的人物身上所有装备+速度，默认6。
        /// </summary>
        public int ItemSpeed;
        /// <summary>
        /// 玩家加速度装备因数，数值越小，封加速越严厉，默认60。
        /// </summary>
        public int DefItemSpeed;
        /// <summary>
        /// 加速的累计值
        /// </summary>
        public int ErrorCount;
        /// <summary>
        /// 交易时间
        /// </summary>
        public int DealTick;
        /// <summary>
        /// 装备加速
        /// </summary>
        public int HitSpeed;
        /// <summary>
        /// 发言时间
        /// </summary>
        public int SayMsgTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public int MoveTick;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int AttackTick;
        /// <summary>
        /// 魔法时间
        /// </summary>
        public int SpellTick;
        /// <summary>
        /// 走路时间
        /// </summary>
        public int DwWalkTick;
        /// <summary>
        /// 跑步时间
        /// </summary>
        public int DwRunTick;
        /// <summary>
        /// 转身时间
        /// </summary>
        public int TurnTick;
        /// <summary>
        /// 挖肉时间
        /// </summary>
        public int ButchTick;
        /// <summary>
        /// 蹲下时间
        /// </summary>
        public int SitDownTick;
        /// <summary>
        /// 吃药时间
        /// </summary>
        public int EatTick;
        /// <summary>
        /// 捡起物品时间
        /// </summary>
        public int PickupTick;
        /// <summary>
        /// 移动时间
        /// </summary>
        public int DwRunWalkTick;
        /// <summary>
        /// 传送时间
        /// </summary>
        public int DwFeiDnItemsTick;
        /// <summary>
        /// 变速齿轮时间
        /// </summary>
        public int DwSupSpeederTick;
        /// <summary>
        /// 变速齿轮累计
        /// </summary>
        public int DwSupSpeederCount;
        /// <summary>
        /// 超级加速时间
        /// </summary>
        public int DwSuperNeverTick;
        /// <summary>
        /// 超级加速累计
        /// </summary>
        public int DwSuperNeverCount;
        /// <summary>
        /// 记录上一次操作
        /// </summary>
        public int DwUserDoTick;
        /// <summary>
        /// 保存停顿操作时间
        /// </summary>
        public long ContinueTick;
        /// <summary>
        /// 带有攻击并发累计
        /// </summary>
        public int ConHitMaxCount;
        /// <summary>
        /// 带有魔法并发累计
        /// </summary>
        public int ConSpellMaxCount;
        /// <summary>
        /// 记录上一次移动方向
        /// </summary>
        public int CombinationTick;
        /// <summary>
        /// 智能攻击累计
        /// </summary>
        public int CombinationCount;
        public long GameTick;
        public int WaringTick;

        public SessionSpeed()
        {
            var dwCurrentTick = HUtil32.GetTickCount();
            ErrorCount = dwCurrentTick;
            DealTick = dwCurrentTick;
            HitSpeed = dwCurrentTick;
            SayMsgTick = dwCurrentTick;
            MoveTick = dwCurrentTick;
            AttackTick = dwCurrentTick;
            SpellTick = dwCurrentTick;
            DwWalkTick = dwCurrentTick;
            DwRunTick = dwCurrentTick;
            TurnTick = dwCurrentTick;
            ButchTick = dwCurrentTick;
            SitDownTick = dwCurrentTick;
            EatTick = dwCurrentTick;
            PickupTick = dwCurrentTick;
            DwRunWalkTick = dwCurrentTick;
            DwFeiDnItemsTick = dwCurrentTick;
            DwSupSpeederTick = dwCurrentTick;
            DwSupSpeederCount = dwCurrentTick;
            DwSuperNeverTick = dwCurrentTick;
            DwSuperNeverCount = dwCurrentTick;
            DwUserDoTick = dwCurrentTick;
            ContinueTick = dwCurrentTick;
            ConHitMaxCount = dwCurrentTick;
            ConSpellMaxCount = dwCurrentTick;
            CombinationTick = dwCurrentTick;
            CombinationCount = dwCurrentTick;
            GameTick = dwCurrentTick;
            WaringTick = dwCurrentTick;
        }
    }
}