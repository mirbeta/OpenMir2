using GameGate.Auth;
using GameGate.Conf;
using GameGate.Packet;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SessionSpeedRule gameSpeed;
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
            gameSpeed = new SessionSpeedRule();
            SessionKey = Guid.NewGuid().ToString("N");
            _authenticator = new DynamicAuthenticator();
        }

        public SessionSpeedRule GetGameSpeed()
        {
            return gameSpeed;
        }

        public ClientThread ServerThread => ClientThread;

        public SessionInfo Session => _session;
        
        private static GateConfig Config => ConfigManager.Instance.GateConfig;

        private void Kick(byte code)
        {
            Session.Socket.Close();
        }

        /// <summary>
        /// 处理客户端封包
        /// </summary>
        public void ProcessPacket(SessionMessage messagePacket)
        {
            _session.ReceiveTick = HUtil32.GetTickCount();
            var sMsg = string.Empty;
            int currentTick;
            if (KickFlag)
            {
                KickFlag = false;
                return;
            }

            if (Config.IsDefenceCCPacket && (messagePacket.PacketLen >= 5))
            {
                sMsg = HUtil32.GetString(messagePacket.Buffer.AsSpan().Slice(2, messagePacket.PacketLen - 3));
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

                if ((currentTick - SendCheckTick) > 1000 * 5) // 如果5 秒 没有回数据 就下发数据
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
                    return;
                }
            }

            var success = false;
            if (HandleLogin)
            {
                if (messagePacket.PacketLen < GateShare.CommandFixedLength)
                {
                    _session.Socket.Close(); //关闭异常会话
                    return;
                }

                var tempBuff = messagePacket.Buffer[2..^1]; //跳过#1....! 只保留消息内容
                var deCodeLen = 0;
                var decodeBuff = EncryptUtil.DecodeSpan(tempBuff, tempBuff.Length, ref deCodeLen);

                var recog = BitConverter.ToInt32(decodeBuff[..4]);
                var ident = BitConverter.ToUInt16(decodeBuff.Slice(4, 2));
                var param = BitConverter.ToUInt16(decodeBuff.Slice(6, 2));
                var tag = BitConverter.ToUInt16(decodeBuff.Slice(8, 2));
                var series = BitConverter.ToUInt16(decodeBuff.Slice(10, 2));

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
                    case Messages.CM_GUILDUPDATENOTICE:
                    case Messages.CM_GUILDUPDATERANKINFO:
                        if (messagePacket.PacketLen > Config.MaxClientPacketSize)
                        {
                            Logger.Info("Kick off user,over max client packet size: " + messagePacket.PacketLen);
                            // Misc.KickUser(m_pUserOBJ.nIPAddr);
                            return;
                        }

                        break;
                    default:
                        if (messagePacket.PacketLen > Config.NomClientPacketSize)
                        {
                            Logger.Info("Kick off user,over nom client packet size: " + messagePacket.PacketLen);
                            // Misc.KickUser(m_pUserOBJ.nIPAddr);
                            return;
                        }

                        break;
                }

                int delayMsgCount;
                int dwDelay;
                int nInterval;
                switch (ident)
                {
                    case Messages.CM_WALK:
                    case Messages.CM_RUN:
                        if (Config.IsMoveInterval) // 700
                        {
                            currentTick = HUtil32.GetTickCount();
                            int nMoveInterval;
                            if (gameSpeed.SpeedLimit)
                            {
                                nMoveInterval = Config.MoveInterval + Config.PunishMoveInterval;
                            }
                            else
                            {
                                nMoveInterval = Config.MoveInterval;
                            }
                            nInterval = currentTick - gameSpeed.MoveTick;
                            if ((nInterval >= nMoveInterval))
                            {
                                gameSpeed.MoveTick = currentTick;
                                gameSpeed.SpellTick = currentTick - Config.MoveNextSpellCompensate;
                                if (gameSpeed.AttackTick < currentTick - Config.MoveNextAttackCompensate)
                                {
                                    gameSpeed.AttackTick = currentTick - Config.MoveNextAttackCompensate;
                                }
                                LastDirection = tag;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == PunishMethod.DelaySend)
                                {
                                    delayMsgCount = GetDelayMsgCount();
                                    if (delayMsgCount == 0)
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((nMoveInterval - nInterval) * Config.PunishIntervalRate);
                                        gameSpeed.MoveTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        gameSpeed.MoveTick = currentTick + (nMoveInterval - nInterval);
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
                    case Messages.CM_HIT:
                    case Messages.CM_HEAVYHIT:
                    case Messages.CM_BIGHIT:
                    case Messages.CM_POWERHIT:
                    case Messages.CM_LONGHIT:
                    case Messages.CM_WIDEHIT:
                    case Messages.CM_CRSHIT:
                    case Messages.CM_FIREHIT:
                        if (Config.IsAttackInterval)
                        {
                            currentTick = HUtil32.GetTickCount();
                            int nAttackInterval;
                            if (gameSpeed.SpeedLimit)
                            {
                                nAttackInterval = Config.AttackInterval + Config.PunishAttackInterval;
                            }
                            else
                            {
                                nAttackInterval = Config.AttackInterval;
                            }

                            var nAttackFixInterval = HUtil32._MAX(0, (nAttackInterval - Config.MaxItemSpeedRate * gameSpeed.ItemSpeed));
                            nInterval = currentTick - gameSpeed.AttackTick;
                            if ((nInterval >= nAttackFixInterval))
                            {
                                gameSpeed.AttackTick = currentTick;
                                if (Config.IsItemSpeedCompensate)
                                {
                                    gameSpeed.MoveTick = currentTick - (Config.AttackNextMoveCompensate + Config.MaxItemSpeedRate * gameSpeed.ItemSpeed); // 550
                                    gameSpeed.SpellTick = currentTick - (Config.AttackNextSpellCompensate + Config.MaxItemSpeedRate * gameSpeed.ItemSpeed); // 1150
                                }
                                else
                                {
                                    gameSpeed.MoveTick = currentTick - Config.AttackNextMoveCompensate; // 550
                                    gameSpeed.SpellTick = currentTick - Config.AttackNextSpellCompensate; // 1150
                                }

                                LastDirection = tag;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == PunishMethod.DelaySend)
                                {
                                    delayMsgCount = GetDelayMsgCount();
                                    if (delayMsgCount == 0)
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((nAttackFixInterval - nInterval) * Config.PunishIntervalRate);
                                        gameSpeed.AttackTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        gameSpeed.AttackTick = currentTick + (nAttackFixInterval - nInterval);
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
                    case Messages.CM_SPELL:
                        if (Config.IsSpellInterval) // 1380
                        {
                            currentTick = HUtil32.GetTickCount();
                            if (tag >= 0128)
                            {
                                return;
                            }
                            if (TableDef.MaigicDelayArray[tag]) // 过滤武士魔法
                            {
                                int nSpellInterval;
                                if (gameSpeed.SpeedLimit)
                                {
                                    nSpellInterval = TableDef.MaigicDelayTimeList[tag] + Config.PunishSpellInterval;
                                }
                                else
                                {
                                    nSpellInterval = TableDef.MaigicDelayTimeList[tag];
                                }
                                nInterval = (currentTick - gameSpeed.SpellTick);
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

                                    gameSpeed.SpellTick = currentTick;
                                    gameSpeed.MoveTick = currentTick - dwNextMove;
                                    gameSpeed.AttackTick = currentTick - dwNextAtt;
                                    LastDirection = tag;
                                }
                                else
                                {
                                    if (Config.OverSpeedPunishMethod == PunishMethod.DelaySend)
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
                    case Messages.CM_SITDOWN:
                        if (Config.IsSitDownInterval)
                        {
                            currentTick = HUtil32.GetTickCount();
                            nInterval = (currentTick - gameSpeed.SitDownTick);
                            if (nInterval >= Config.SitDownInterval)
                            {
                                gameSpeed.SitDownTick = currentTick;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == PunishMethod.DelaySend)
                                {
                                    delayMsgCount = GetDelayMsgCount();
                                    if (delayMsgCount == 0)
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.SitDownInterval - nInterval) * Config.PunishIntervalRate);
                                        gameSpeed.SitDownTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        gameSpeed.SitDownTick = currentTick + (Config.SitDownInterval - nInterval);
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
                    case Messages.CM_BUTCH:
                        if (Config.IsButchInterval)
                        {
                            currentTick = HUtil32.GetTickCount();
                            nInterval = currentTick - gameSpeed.ButchTick;
                            if (nInterval >= Config.ButchInterval)
                            {
                                gameSpeed.ButchTick = currentTick;
                            }
                            else
                            {
                                if (Config.OverSpeedPunishMethod == PunishMethod.DelaySend)
                                {
                                    if (!PeekDelayMsg(ident))
                                    {
                                        dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.ButchInterval - nInterval) * Config.PunishIntervalRate);
                                        gameSpeed.ButchTick = currentTick + dwDelay;
                                    }
                                    else
                                    {
                                        gameSpeed.SitDownTick = currentTick + (Config.ButchInterval - nInterval);
                                        return;
                                    }
                                }
                            }
                        }
                        break;
                    case Messages.CM_TURN:
                        if (Config.IsTurnInterval && (Config.OverSpeedPunishMethod != PunishMethod.TurnPack))
                        {
                            if (LastDirection != tag)
                            {
                                currentTick = HUtil32.GetTickCount();
                                if (currentTick - gameSpeed.TurnTick >= Config.TurnInterval)
                                {
                                    LastDirection = tag;
                                    gameSpeed.TurnTick = currentTick;
                                }
                                else
                                {
                                    if (Config.OverSpeedPunishMethod == PunishMethod.DelaySend)
                                    {
                                        if (!PeekDelayMsg(ident))
                                        {
                                            dwDelay = Config.PunishBaseInterval + (int)Math.Round((Config.TurnInterval - (currentTick - gameSpeed.TurnTick)) * Config.PunishIntervalRate);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case Messages.CM_DEALTRY:
                        currentTick = HUtil32.GetTickCount();
                        if ((currentTick - gameSpeed.DealTick < 10000))
                        {
                            if ((currentTick - -gameSpeed.WaringTick > 2000))
                            {
                                gameSpeed.WaringTick = currentTick;
                                SendSysMsg($"攻击状态不能交易！请稍等{(10000 - currentTick + gameSpeed.DealTick) / 1000 + 1}秒……");
                            }
                            return;
                        }
                        break;
                    case Messages.CM_SAY:
                        sMsg = HUtil32.GetString(decodeBuff[12..]);
                        if (Config.IsChatInterval)
                        {
                            if (!sMsg.StartsWith("@"))
                            {
                                currentTick = HUtil32.GetTickCount();
                                if (currentTick - gameSpeed.SayMsgTick < Config.ChatInterval)
                                {
                                    return;
                                }
                                gameSpeed.SayMsgTick = currentTick;
                            }
                        }
                        if (deCodeLen > GateShare.CommandFixedLength)
                        {
                            if (sMsg.StartsWith("@"))
                            {
                                var pszChatBuffer = new byte[255];
                                var pszChatCmd = string.Empty;
                                MemoryCopy.BlockCopy(decodeBuff, GateShare.CommandFixedLength, pszChatBuffer, 0, deCodeLen - GateShare.CommandFixedLength);
                                pszChatBuffer[deCodeLen - GateShare.CommandFixedLength] = (byte)'\0';
                                //var tempStr = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                //var nChatStrPos = tempStr.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
                                //if (nChatStrPos > 0)
                                //{
                                //    Move(pszChatBuffer[0], pszChatCmd[0], nChatStrPos - 1);
                                //    pszChatCmd[nChatStrPos - 1] = '\0';
                                //}
                                //else
                                //{
                                //    Move(pszChatBuffer[0], pszChatCmd[0], pszChatBuffer.Length);
                                //}

                                if (GateShare.ChatCommandFilterMap.ContainsKey(pszChatCmd))
                                {
                                    var cmd = new CommandPacket
                                    {
                                        Recog = SvrObjectId,
                                        Ident = Messages.SM_WHISPER,
                                        Param = HUtil32.MakeWord(0xFF, 56)
                                    };
                                    pszChatBuffer = HUtil32.GetBytes(string.Format(Protocol.CmdFilter, pszChatCmd));
                                    var pszSendBuf = new byte[255];
                                    pszSendBuf[0] = (byte)'#';
                                    Buffer.BlockCopy(SerializerUtil.Serialize(cmd), 0, pszSendBuf, 1, pszSendBuf.Length);//Move(Cmd, m_pOverlapRecv.BBuffer[1], TCmdPack.PackSize);
                                    Buffer.BlockCopy(pszChatBuffer, 0, pszSendBuf, 13, pszChatBuffer.Length);//Move(pszChatBuffer[0], m_pOverlapRecv.BBuffer[13], pszChatBuffer.Length);
                                    //var nEnCodeLen = EncryptUtil.Encode(messagePacket.Buffer, 0, CommandPacket.Size + pszChatBuffer.Length, 1);
                                    var nEnCodeLen = EncryptUtil.Encode(decodeBuff, GateShare.CommandFixedLength + pszChatBuffer.Length, pszSendBuf);
                                    pszSendBuf[nEnCodeLen + 1] = (byte)'!';
                                    Session.Socket.Send(pszSendBuf, SocketFlags.None);
                                    //m_tIOCPSender.SendData(m_pOverlapSend, pszSendBuf[0], nEnCodeLen + 2);
                                    return;
                                }

                                if (Config.IsSpaceMoveNextPickupInterval)
                                {
                                    var buffString = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                    if (string.Compare(buffString, Config.SpaceMoveCommand, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        gameSpeed.PickupTick = HUtil32.GetTickCount() + Config.SpaceMoveNextPickupInterval;
                                    }
                                }

                            }
                            else if (Config.IsChatFilter)
                            {
                                if (sMsg.StartsWith("/"))
                                {
                                    var pszChatBuffer = new byte[255];
                                    var pszChatCmd = string.Empty;
                                    //Move((nABuf + TCmdPack.PackSize as string), pszChatBuffer[0], nDeCodeLen - TCmdPack.PackSize);
                                    MemoryCopy.BlockCopy(messagePacket.Buffer, GateShare.CommandFixedLength, pszChatBuffer, 0, deCodeLen - GateShare.CommandFixedLength);
                                    pszChatBuffer[deCodeLen - GateShare.CommandFixedLength] = (byte)'\0';
                                    var tempStr = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                    var nChatStrPos = tempStr.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
                                    if (nChatStrPos > 0)
                                    {
                                        //Move(pszChatBuffer[0], pszChatCmd[0], nChatStrPos - 1);
                                        //pszChatCmd[nChatStrPos - 1] = '\0';
                                        //szChatBuffer = pszChatBuffer[nChatStrPos];
                                        //var fChatFilter = AbusiveFilter.CheckChatFilter(ref szChatBuffer, ref Succeed);
                                        //if ((fChatFilter > 0) && !Succeed)
                                        //{
                                        //    _logger.Info("Kick off user,saying in filter");
                                        //    return;
                                        //}
                                        //if (fChatFilter == 2)
                                        //{
                                        //    var buffString = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                        //    pszChatBuffer = $"{pszChatCmd} {szChatBuffer}";
                                        //    nDeCodeLen = pszChatBuffer.Length + TCmdPack.PackSize;
                                        //    Move(pszChatBuffer[0], (nABuf + TCmdPack.PackSize as string), pszChatBuffer.Length);
                                        //}
                                    }
                                }
                                else if (!sMsg.StartsWith("@"))
                                {
                                    var pszChatBuffer = new byte[255];
                                    MemoryCopy.BlockCopy(messagePacket.Buffer, GateShare.CommandFixedLength, pszChatBuffer, 0, deCodeLen - GateShare.CommandFixedLength);
                                    pszChatBuffer[deCodeLen - GateShare.CommandFixedLength] = (byte)'\0';
                                    var tempStr = HUtil32.GetString(pszChatBuffer, 0, pszChatBuffer.Length);
                                    //szChatBuffer = nABuf + CommandPacket.Size;
                                    var isSucceed = false;
                                    var fChatFilter = GateShare.AbusiveFilter.CheckChatFilter(ref tempStr, ref isSucceed);
                                    if ((fChatFilter > 0) && !isSucceed)
                                    {
                                        Logger.Info("Kick off user,saying in filter");
                                        return;
                                    }
                                    if (fChatFilter == 2)
                                    {
                                        deCodeLen = pszChatBuffer.Length + CommandPacket.Size;
                                        //  Move(szChatBuffer[1], (nABuf + TCmdPack.PackSize as string), szChatBuffer.Length);
                                    }
                                }
                            }
                        }
                        break;
                    case Messages.CM_PICKUP:
                        if (Config.IsPickupInterval)
                        {
                            currentTick = HUtil32.GetTickCount();
                            if (currentTick - gameSpeed.PickupTick > Config.PickupInterval)
                            {
                                gameSpeed.PickupTick = currentTick;
                            }
                            else
                            {
                                return;
                            }
                        }

                        break;
                    case Messages.CM_EAT:
                        if (Config.IsEatInterval)
                        {
                            if (series == 0 || series == 1 || series == 3)
                            {
                                currentTick = HUtil32.GetTickCount();
                                if (currentTick - gameSpeed.EatTick > Config.EatInterval)
                                {
                                    gameSpeed.EatTick = currentTick;
                                }
                                else
                                {
                                    var eatPacket = new CommandPacket();
                                    eatPacket.Recog = recog;
                                    eatPacket.Ident = Messages.SM_EAT_FAIL;
                                    var pszSendBuf = new byte[GateShare.CommandFixedLength];
                                    pszSendBuf[0] = (byte)'#';
                                    var nEnCodeLen = EncryptUtil.Encode(SerializerUtil.Serialize(eatPacket), GateShare.CommandFixedLength, pszSendBuf);
                                    pszSendBuf[nEnCodeLen + 1] = (byte)'!';
                                    ClientThread.Send(pszSendBuf);
                                    return;
                                }
                            }
                        }
                        break;
                }

                byte[] bodyBuffer;
                var commandPack = new ServerMessage
                {
                    PacketCode = Grobal2.RunGateCode,
                    Socket = _session.SckHandle,
                    Ident = Grobal2.GM_DATA,
                    ServerIndex = SvrListIdx
                };
                int sendLen;
                if (deCodeLen > CommandPacket.Size)
                {
                    var sendBuffer = new byte[messagePacket.Buffer.Length - CommandPacket.Size + 1];
                    var tLen = EncryptUtil.Encode(decodeBuff, deCodeLen - CommandPacket.Size, sendBuffer);
                    commandPack.PackLength = CommandPacket.Size + tLen + 1;
                    sendLen = ServerMessage.PacketSize + commandPack.PackLength;
                    bodyBuffer = GateShare.BytePool.Rent(sendLen);
                    MemoryCopy.BlockCopy(decodeBuff, 0, bodyBuffer, ServerMessage.PacketSize, CommandPacket.Size);
                    MemoryCopy.BlockCopy(tempBuff, GateShare.CommandFixedLength, bodyBuffer, ServerMessage.PacketSize + CommandPacket.Size, tLen);//消息体
                }
                else
                {
                    sendLen = ServerMessage.PacketSize + decodeBuff.Length;
                    bodyBuffer = GateShare.BytePool.Rent(sendLen);
                    commandPack.PackLength = CommandPacket.Size;
                    MemoryCopy.BlockCopy(decodeBuff, 0, bodyBuffer, ServerMessage.PacketSize, decodeBuff.Length);
                }
                Buffer.BlockCopy(SerializerUtil.Serialize(commandPack), 0, bodyBuffer, 0, ServerMessage.PacketSize); //复制消息头
                ClientThread.Send(bodyBuffer[..sendLen]);
                GateShare.BytePool.Return(bodyBuffer);
            }
            else
            {
                var tempStr = EDCode.DeCodeString(messagePacket.Buffer[2..(messagePacket.PacketLen - 3)]);
                ClientLogin(tempStr, messagePacket.PacketLen, "", ref success);
                if (!success)
                {
                    Logger.Info("客户端登陆消息处理失败，剔除链接");
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
                    ClientThread.Send(delayMsg.Buffer); //发送消息到GameSvr
                    var dwCurrentTick = HUtil32.GetTickCount();
                    switch (delayMsg.Cmd)
                    {
                        case Messages.CM_BUTCH:
                            gameSpeed.ButchTick = dwCurrentTick;
                            break;
                        case Messages.CM_SITDOWN:
                            gameSpeed.SitDownTick = dwCurrentTick;
                            break;
                        case Messages.CM_TURN:
                            gameSpeed.TurnTick = dwCurrentTick;
                            break;
                        case Messages.CM_WALK:
                        case Messages.CM_RUN:
                            gameSpeed.MoveTick = dwCurrentTick;
                            gameSpeed.SpellTick = dwCurrentTick - Config.MoveNextSpellCompensate; //1200
                            if (gameSpeed.AttackTick < dwCurrentTick - Config.MoveNextAttackCompensate)
                            {
                                gameSpeed.AttackTick = dwCurrentTick - Config.MoveNextAttackCompensate; //900
                            }

                            LastDirection = delayMsg.Dir;
                            break;
                        case Messages.CM_HIT:
                        case Messages.CM_HEAVYHIT:
                        case Messages.CM_BIGHIT:
                        case Messages.CM_POWERHIT:
                        case Messages.CM_LONGHIT:
                        case Messages.CM_WIDEHIT:
                        case Messages.CM_CRSHIT:
                        case Messages.CM_FIREHIT:
                            if (gameSpeed.AttackTick < dwCurrentTick)
                            {
                                gameSpeed.AttackTick = dwCurrentTick;
                            }

                            if (Config.IsItemSpeedCompensate)
                            {
                                gameSpeed.MoveTick = dwCurrentTick - (Config.AttackNextMoveCompensate +
                                                                       Config.MaxItemSpeedRate *
                                                                       gameSpeed.ItemSpeed); // 550
                                gameSpeed.SpellTick = dwCurrentTick - (Config.AttackNextSpellCompensate +
                                                                        Config.MaxItemSpeedRate *
                                                                        gameSpeed.ItemSpeed); // 1150
                            }
                            else
                            {
                                gameSpeed.MoveTick = dwCurrentTick - Config.AttackNextMoveCompensate; // 550
                                gameSpeed.SpellTick = dwCurrentTick - Config.AttackNextSpellCompensate; // 1150
                            }

                            LastDirection = delayMsg.Dir;
                            break;
                        case Messages.CM_SPELL:
                            gameSpeed.SpellTick = dwCurrentTick;
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

                            gameSpeed.MoveTick = dwCurrentTick - nNextMov; // 550
                            if (gameSpeed.AttackTick < dwCurrentTick - nNextAtt) // 900
                            {
                                gameSpeed.AttackTick = dwCurrentTick - nNextAtt;
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
                if (nCmd == Messages.CM_HIT)
                {
                    if ((iCmd == Messages.CM_HIT) || (iCmd == Messages.CM_HEAVYHIT) ||
                        (iCmd == Messages.CM_BIGHIT) || (iCmd == Messages.CM_POWERHIT) ||
                        (iCmd == Messages.CM_LONGHIT) || (iCmd == Messages.CM_WIDEHIT) ||
                        (iCmd == Messages.CM_CRSHIT) || (iCmd == Messages.CM_FIREHIT))
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
                else if (nCmd == Messages.CM_RUN)
                {
                    if ((iCmd == Messages.CM_WALK) || (iCmd == Messages.CM_RUN))
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
                Logger.Debug($"发送延时处理消息:User:[{_session.ChrName}] MagicID:[{magicId}] DelayTime:[{delayTime}]");
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
        public void ProcessServerPacket(SessionMessage message)
        {
            var bufferLen = message.PacketLen;
            var sourcePacket = message.Buffer;
            byte[] sendBuffer;
            if (bufferLen <= 0)//小包 走路 攻击等
            {
                var buffLen = -bufferLen;
                sendBuffer = GateShare.BytePool.Rent(buffLen + 2);
                sendBuffer[0] = (byte)'#';
                MemoryCopy.BlockCopy(sourcePacket, 0, sendBuffer, 1, buffLen);
                sendBuffer[buffLen + 1] = (byte)'!';
                Session.Socket.Send(sendBuffer[..(buffLen + 2)], SocketFlags.None);
                GateShare.BytePool.Return(sendBuffer);
            }
            else
            {
                sendBuffer = GateShare.BytePool.Rent(bufferLen + CommandPacket.Size);
                sendBuffer[0] = (byte)'#';
                var nLen = EncryptUtil.Encode(sourcePacket, CommandPacket.Size, sendBuffer, 1);//消息头
                if (bufferLen > CommandPacket.Size)
                {
                    MemoryCopy.BlockCopy(sourcePacket, CommandPacket.Size, sendBuffer, nLen + 1, bufferLen - CommandPacket.Size);
                    nLen = bufferLen - CommandPacket.Size + nLen;
                }
                sendBuffer[nLen + 1] = (byte)'!';
                Session.Socket.Send(sendBuffer[..(nLen + 2)], SocketFlags.None);
            }

            if (bufferLen > 10)
            {
                var messagePacket = sourcePacket.AsSpan();
                var recog = BitConverter.ToInt32(messagePacket[..4]);
                var ident = BitConverter.ToUInt16(messagePacket.Slice(4, 2));
                //var param = BitConverter.ToUInt16(messagePacket.Slice(6, 2));
                //var tag = BitConverter.ToUInt16(messagePacket.Slice(8, 2));
                int series;
                switch (ident)
                {
                    case Messages.SM_RUSH:
                        if (SvrObjectId == recog)
                        {
                            var dwCurrentTick = HUtil32.GetTickCount();
                            gameSpeed.MoveTick = dwCurrentTick;
                            gameSpeed.AttackTick = dwCurrentTick;
                            gameSpeed.SpellTick = dwCurrentTick;
                            gameSpeed.SitDownTick = dwCurrentTick;
                            gameSpeed.ButchTick = dwCurrentTick;
                            gameSpeed.DealTick = dwCurrentTick;
                        }
                        break;
                    case Messages.SM_NEWMAP:
                    case Messages.SM_CHANGEMAP:
                    case Messages.SM_LOGON:
                        if (SvrObjectId == 0)
                        {
                            SvrObjectId = recog;
                        }
                        break;
                    case Messages.SM_PLAYERCONFIG:

                        break;
                    case Messages.SM_CHARSTATUSCHANGED:
                        series = BitConverter.ToUInt16(messagePacket.Slice(10, 2));
                        if (SvrObjectId == recog)
                        {
                            gameSpeed.DefItemSpeed = series;
                            gameSpeed.ItemSpeed = HUtil32._MIN(Config.MaxItemSpeed, series);
                            //_mNChrStutas = HUtil32.MakeLong(param, tag);
                            //message.Buffer[10] = (byte)_gameSpeed.ItemSpeed; //同时限制客户端
                        }
                        break;
                    case Messages.SM_HWID:
                        series = BitConverter.ToUInt16(messagePacket.Slice(10, 2));
                        if (Config.IsProcClientHardwareID)
                        {
                            switch (series)
                            {
                                case 1:
                                    Logger.Debug("封机器码");
                                    break;
                                case 2:
                                    Logger.Debug("清理机器码");
                                    GateShare.HardwareFilter.ClearDeny();
                                    GateShare.HardwareFilter.SaveDenyList();
                                    break;
                            }
                        }
                        break;
                    case Messages.SM_RUNGATELOGOUT:
                        SendKickMsg(2);
                        break;
                }
            }

            GateShare.BytePool.Return(sendBuffer);
        }

        private void SendKickMsg(int killType)
        {
            var sendMsg = string.Empty;
            var defMsg = new CommandPacket();
            switch (killType)
            {
                case 0:
                    if (Config.IsKickOverSpeed)
                    {
                    }
                    sendMsg = Config.OverSpeedSendBack;
                    break;
                case 1:
                    sendMsg = Config.PacketDecryptFailed;
                    break;
                case 2:
                    sendMsg = "当前登录帐号正在其它位置登录，本机已被强行离线";
                    break;
                case 4: //todo 版本号验证
                    //defMsg.Cmd = Messages.SM_VERSION_FAIL;
                    break;
                case 5:
                    sendMsg = Config.ClientOverCntMsg;
                    break;
                case 6:
                    sendMsg = Config.HWIDBlockedMsg;
                    break;
                case 12:
                    sendMsg = "反外挂模块更新失败,请重启客户端!!!!";
                    break;
            }

            Logger.Debug(sendMsg);

            //defMsg.UID = m_nSvrObject;
            //defMsg.Cmd = Messages.SM_SYSMESSAGE;
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
                Logger.Debug("ClientLogin: " + loginData);
                if (loginData[0] != '*' || loginData[1] != '*')
                {
                    Logger.Info($"[ClientLogin] Kicked 1: {loginData}");
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
                        Logger.Info($"[ClientLogin] Kicked 2: {sHumName} clientVer validation failed.");
                        success = false;
                        return;
                    }
                    if (szCode.Length != 10)
                    {
                        success = false;
                        return;
                    }
                    if (GateShare.PunishList.TryGetValue(sHumName, out var userType))
                    {
                        gameSpeed.SpeedLimit = true;
                        GateShare.PunishList[sHumName] = this;
                    }
                    var hardWareDigest = MD5.EmptyDigest;
                    if (Config.IsProcClientHardwareID)
                    {
                        if (string.IsNullOrEmpty(szHarewareId) || (szHarewareId.Length > 256) || ((szHarewareId.Length % 2) != 0))
                        {
                            Logger.Info($"[ClientLogin] Kicked 3: {sHumName}");
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
                            Logger.Info($"[ClientLogin] Kicked 5: {sHumName}", 1);
                            SendKickMsg(4);
                            return;
                        }
                        HardwareHeader pHardwareHeader = ClientPacket.ToPacket<HardwareHeader>(dest);
                        //todo session会话里面需要存用户ip
                        Logger.Info($"HWID: {MD5.MD5Print(pHardwareHeader.xMd5Digest)}  {sHumName.Trim()}  {addr}");
                        if (pHardwareHeader.dwMagicCode == 0x13F13F13)
                        {
                            if (MD5.MD5Match(MD5.EmptyDigest, pHardwareHeader.xMd5Digest))
                            {
                                Logger.Info($"[ClientLogin] Kicked 6: {sHumName}");
                                SendKickMsg(4);
                                return;
                            }
                            hardWareDigest = pHardwareHeader.xMd5Digest;
                            bool overClientCount = false;
                            if (GateShare.HardwareFilter.IsFilter(hardWareDigest, ref overClientCount))
                            {
                                Logger.Info($"[ClientLogin] Kicked 7: {sHumName}");
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
                            Logger.Info($"[ClientLogin] Kicked 8: {sHumName}");
                            SendKickMsg(4);
                            return;
                        }
                    }
                    var loginPacket = $"**{sAccount}/{sHumName}/{szCert}/{szClientVerNo}/{szCode}/{MD5.MD5Print(hardWareDigest)}";
                    var tempBuf = HUtil32.GetBytes(loginPacket);
                    Span<byte> pszLoginPacket = stackalloc byte[tempBuf.Length + 100];
                    var encodeLen = EncryptUtil.Encode(tempBuf, tempBuf.Length, pszLoginPacket, 2);
                    pszLoginPacket[0] = (byte)'#';
                    pszLoginPacket[1] = (byte)'0';
                    pszLoginPacket[encodeLen + 2] = (byte)'!';
                    _session.Account = sAccount;
                    _session.ChrName = sHumName;
                    SendLoginPacket(pszLoginPacket, encodeLen + 3);
                    success = true;
                    HandleLogin = true;
                    /*var secretKey = _authenticator.GenerateSetupCode("openmir2", sAccount, SessionKey, 5);
                    _logger.Info($"动态密钥:{secretKey.AccountSecretKey}", 1);
                    _logger.Info($"动态验证码：{secretKey.ManualEntryKey}", 1);
                    _logger.Info($"{_authenticator.DefaultClockDriftTolerance.TotalMilliseconds}秒后验证新的密钥,容错5秒.", 1);*/
                }
                else
                {
                    Logger.Info($"[ClientLogin] Kicked 2: {loginData}");
                    success = false;
                }
            }
            else
            {
                Logger.Info($"[ClientLogin] Kicked 0: {loginData}");
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
                tempBuff = new byte[ServerMessage.PacketSize + packet.Length];
            }
            else
            {
                tempBuff = new byte[ServerMessage.PacketSize + len];
            }
            var packetHeader = new ServerMessage();
            packetHeader.PacketCode = Grobal2.RunGateCode;
            packetHeader.Socket = (int)_session.Socket.Handle;
            packetHeader.SessionId = _session.SessionId;
            packetHeader.Ident = Grobal2.GM_DATA;
            packetHeader.ServerIndex = _session.UserListIndex;
            packetHeader.PackLength = tempBuff.Length - ServerMessage.PacketSize;
            var sendBuffer = SerializerUtil.Serialize(packetHeader);
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(packetHeader), 0, tempBuff, 0, sendBuffer.Length);
            if (len == 0)
            {
                MemoryCopy.BlockCopy(packet, 0, tempBuff, sendBuffer.Length, packet.Length);
            }
            else
            {
                MemoryCopy.BlockCopy(packet, 0, tempBuff, sendBuffer.Length, len);
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
            var clientPacket = new CommandPacket();
            //clientPacket.UID = SvrObjectId;
            //clientPacket.Cmd = Messages.SM_SYSMESSAGE;
            //clientPacket.X = HUtil32.MakeWord(0xFF, 0xF9);
            //clientPacket.Y = 0;
            //clientPacket.Direct = 0;
            Buffer.BlockCopy(SerializerUtil.Serialize(clientPacket), 0, tempBuf, 0, GateShare.CommandFixedLength);
            var sBuff = HUtil32.GetBytes(szMsg);
            Buffer.BlockCopy(sBuff, 0, tempBuf, 13, sBuff.Length);
            var iLen = GateShare.CommandFixedLength + szMsg.Length;
            var sendBuf = GateShare.BytePool.Rent(iLen + 1);
            sendBuf[0] = (byte)'#';
            iLen = EncryptUtil.Encode(tempBuf, iLen, sendBuf);
            sendBuf[iLen + 1] = (byte)'!';
            //SendQueue.AddClientQueue(_session.ConnectionId, _session.ThreadId, sendBuf);
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

    public class SessionSpeedRule
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

        public SessionSpeedRule()
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