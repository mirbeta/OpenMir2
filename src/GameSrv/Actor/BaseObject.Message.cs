using GameSrv.Maps;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.Actor
{
    public partial class BaseObject
    {
        /// <summary>
        /// 发送优先级消息
        /// </summary>
        public void SendPriorityMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "", MessagePriority Priority = MessagePriority.Normal)
        {
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            if (!Ghost)
            {
                SendMessage sendMessage = new SendMessage
                {
                    wIdent = wIdent,
                    wParam = wParam,
                    nParam1 = nParam1,
                    nParam2 = nParam2,
                    nParam3 = nParam3,
                    DeliveryTime = 0,
                    ActorId = this.ActorId,
                    LateDelivery = false,
                    Buff = sMsg
                };
                if (Priority == MessagePriority.High)
                {
                    MsgQueue.Enqueue(sendMessage);
                }
                else
                {
                    MsgQueue.Enqueue(sendMessage);
                }
            }
            HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
        }

        public void SendMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                var boSend = false;
                if (IsRobot)
                {
                    switch (wIdent)
                    {
                        case Messages.RM_MAGSTRUCK:
                        case Messages.RM_MAGSTRUCK_MINE:
                        case Messages.RM_DELAYPUSHED:
                        case Messages.RM_POISON:
                        case Messages.RM_TRANSPARENT:
                        case Messages.RM_DOOPENHEALTH:
                        case Messages.RM_MAGHEALING:
                        case Messages.RM_DELAYMAGIC:
                        case Messages.RM_SENDDELITEMLIST:
                        case Messages.RM_10401:
                        case Messages.RM_STRUCK:
                        case Messages.RM_STRUCK_MAG:
                            boSend = true;
                            break;
                    }

                    if (!boSend && IsRobot && Race == ActorRace.Play)
                    {
                        switch (wIdent)
                        {
                            case Messages.RM_HEAR:
                            case Messages.RM_WHISPER:
                            case Messages.RM_CRY:
                            case Messages.RM_SYSMESSAGE:
                            case Messages.RM_MOVEMESSAGE:
                            case Messages.RM_GROUPMESSAGE:
                            case Messages.RM_SYSMESSAGE2:
                            case Messages.RM_GUILDMESSAGE:
                            case Messages.RM_SYSMESSAGE3:
                            case Messages.RM_MERCHANTSAY:
                                boSend = true;
                                break;
                        }
                    }
                }
                else
                {
                    boSend = true;
                }
                if (boSend && !Ghost)
                {
                    SendMessage sendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = nParam1,
                        nParam2 = nParam2,
                        nParam3 = nParam3,
                        DeliveryTime = 0,
                        ActorId = this.ActorId,
                        LateDelivery = false,
                        Buff = sMsg
                    };
                    MsgQueue.Enqueue(sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public void SendMsg(BaseObject baseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                var boSend = false;
                if (IsRobot)
                {
                    switch (wIdent)
                    {
                        case Messages.RM_MAGSTRUCK:
                        case Messages.RM_MAGSTRUCK_MINE:
                        case Messages.RM_DELAYPUSHED:
                        case Messages.RM_POISON:
                        case Messages.RM_TRANSPARENT:
                        case Messages.RM_DOOPENHEALTH:
                        case Messages.RM_MAGHEALING:
                        case Messages.RM_DELAYMAGIC:
                        case Messages.RM_SENDDELITEMLIST:
                        case Messages.RM_10401:
                        case Messages.RM_STRUCK:
                        case Messages.RM_STRUCK_MAG:
                            boSend = true;
                            break;
                    }

                    if (!boSend && IsRobot && Race == ActorRace.Play)
                    {
                        switch (wIdent)
                        {
                            case Messages.RM_HEAR:
                            case Messages.RM_WHISPER:
                            case Messages.RM_CRY:
                            case Messages.RM_SYSMESSAGE:
                            case Messages.RM_MOVEMESSAGE:
                            case Messages.RM_GROUPMESSAGE:
                            case Messages.RM_SYSMESSAGE2:
                            case Messages.RM_GUILDMESSAGE:
                            case Messages.RM_SYSMESSAGE3:
                            case Messages.RM_MERCHANTSAY:
                                boSend = true;
                                break;
                        }
                    }
                }
                else
                {
                    boSend = true;
                }
                if (boSend && !Ghost)
                {
                    SendMessage sendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = nParam1,
                        nParam2 = nParam2,
                        nParam3 = nParam3,
                        DeliveryTime = 0,
                        ActorId = baseObject.ActorId,
                        LateDelivery = false,
                        Buff = sMsg
                    };
                    MsgQueue.Enqueue(sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        /// <summary>
        /// 发送自身延时消息
        /// </summary>
        public void SendSelfDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!Ghost)
                {
                    SendMessage sendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        DeliveryTime = HUtil32.GetTickCount() + dwDelay,
                        LateDelivery = true,
                        Buff = sMsg,
                        ActorId = this.ActorId
                    };
                    MsgQueue.Enqueue(sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        /// <summary>
        /// 发送目标延时消息
        /// </summary>
        public void SendTargetDelayMsg(int actorId, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!Ghost)
                {
                    SendMessage sendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        DeliveryTime = HUtil32.GetTickCount() + dwDelay,
                        LateDelivery = true,
                        Buff = sMsg
                    };
                    sendMessage.ActorId = actorId == Messages.RM_STRUCK ? Messages.RM_STRUCK : actorId;
                    MsgQueue.Enqueue(sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        /// <summary>
        /// 更新延时消息
        /// </summary>
        internal void UpdateDelayMsg(int wIdent, short wParam, int lParam1, int lParam2,
            int lParam3, string sMsg, int dwDelay)
        {
            int i;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                i = 0;
                while (true)
                {
                    if (MsgQueue.Count <= i)
                    {
                        break;
                    }
                    if (MsgQueue.TryPeek(out SendMessage sendMessage))
                    {
                        if ((sendMessage.wIdent == wIdent) && (sendMessage.nParam1 == lParam1))
                        {
                            MsgQueue.TryDequeue(out sendMessage);
                            Dispose(sendMessage);
                        }
                    }
                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            SendTargetDelayMsg(ActorId, wIdent, wParam, lParam1, lParam2, lParam3, sMsg, dwDelay);
        }

        /// <summary>
        /// 更新普通消息
        /// </summary>
        public void SendUpdateMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
            string sMsg)
        {
            int i;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                i = 0;
                while (true)
                {
                    if (MsgQueue.Count <= i)
                    {
                        break;
                    }
                    if (MsgQueue.TryPeek(out SendMessage sendMessage))
                    {
                        if (sendMessage.wIdent == wIdent)
                        {
                            MsgQueue.TryDequeue(out sendMessage);
                            Dispose(sendMessage);
                        }
                    }
                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            SendMsg(this, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        public void SendActionMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
            string sMsg)
        {
            int i;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                i = 0;
                while (true)
                {
                    if (MsgQueue.Count <= i)
                    {
                        break;
                    }
                    if (MsgQueue.TryPeek(out SendMessage sendMessage))
                    {
                        if ((sendMessage.wIdent == Messages.CM_TURN) || (sendMessage.wIdent == Messages.CM_WALK) ||
                            (sendMessage.wIdent == Messages.CM_SITDOWN) || (sendMessage.wIdent == Messages.CM_HORSERUN) ||
                            (sendMessage.wIdent == Messages.CM_RUN) || (sendMessage.wIdent == Messages.CM_HIT) ||
                            (sendMessage.wIdent == Messages.CM_HEAVYHIT) || (sendMessage.wIdent == Messages.CM_BIGHIT) ||
                            (sendMessage.wIdent == Messages.CM_POWERHIT) || (sendMessage.wIdent == Messages.CM_LONGHIT) ||
                            (sendMessage.wIdent == Messages.CM_WIDEHIT) || (sendMessage.wIdent == Messages.CM_FIREHIT))
                        {
                            MsgQueue.TryDequeue(out sendMessage);
                            Dispose(sendMessage);
                        }
                    }
                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            SendMsg(this, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        internal bool GetMessage(ref ProcessMessage msg)
        {
            bool result = false;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                if (MsgQueue.TryDequeue(out SendMessage sendMessage))
                {
                    if ((sendMessage.DeliveryTime > 0) && (HUtil32.GetTickCount() < sendMessage.DeliveryTime)) //延时消息
                    {
                        MsgQueue.Enqueue(sendMessage);
                        return false;
                    }
                    msg.wIdent = sendMessage.wIdent;
                    msg.wParam = sendMessage.wParam;
                    msg.nParam1 = sendMessage.nParam1;
                    msg.nParam2 = sendMessage.nParam2;
                    msg.nParam3 = sendMessage.nParam3;
                    msg.ActorId = sendMessage.ActorId;
                    msg.LateDelivery = sendMessage.LateDelivery;
                    msg.Msg = sendMessage.Buff;
                    result = true;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 发送自身消息
        /// </summary>
        private void SendSelfMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            if (!Ghost)
            {
                SendMessage sendMessage = new SendMessage
                {
                    wIdent = wIdent,
                    wParam = wParam,
                    nParam1 = nParam1,
                    nParam2 = nParam2,
                    nParam3 = nParam3,
                    DeliveryTime = 0,
                    LateDelivery = false,
                    Buff = sMsg
                };
                MsgQueue.Enqueue(sendMessage);//优先处理自身消息
            }
        }

        /// <summary>
        /// 发送广播消息
        /// </summary>
        public void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SendRefMsg Name = {0}";
            if (Envir == null)
            {
                M2Share.Logger.Error(ChrName + " SendRefMsg nil PEnvir ");
                return;
            }
            if (ObMode || FixedHideMode)
            {
                SendMsg(wIdent, wParam, nParam1, nParam2, nParam3, sMsg); // 如果隐身模式则只发送信息给自己
                return;
            }
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                if (((HUtil32.GetTickCount() - SendRefMsgTick) >= 500) || (VisibleHumanList.Count == 0))
                {
                    SendRefMsgTick = HUtil32.GetTickCount();
                    VisibleHumanList.Clear();
                    short nLx = (short)(CurrX - M2Share.Config.SendRefMsgRange); // 12
                    short nHx = (short)(CurrX + M2Share.Config.SendRefMsgRange); // 12
                    short nLy = (short)(CurrY - M2Share.Config.SendRefMsgRange); // 12
                    short nHy = (short)(CurrY + M2Share.Config.SendRefMsgRange); // 12
                    for (short nCx = nLx; nCx <= nHx; nCx++)
                    {
                        for (short nCy = nLy; nCy <= nHy; nCy++)
                        {
                            if (!Envir.ValidCell(nCx, nCy))
                            {
                                continue;
                            }
                            ref MapCellInfo cellInfo = ref Envir.GetCellInfo(nCx, nCy, out bool cellSuccess);
                            if (cellSuccess)
                            {
                                if (cellInfo.IsAvailable)
                                {
                                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                                    {
                                        CellObject cellObject = cellInfo.ObjList[i];
                                        if (cellObject.CellObjId > 0)
                                        {
                                            if (cellObject.ActorObject)
                                            {
                                                if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                                {
                                                    cellInfo.Remove(i);
                                                    if (cellInfo.Count <= 0)
                                                    {
                                                        cellInfo.Clear();
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        BaseObject baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                                        if ((baseObject != null) && !baseObject.Ghost)
                                                        {
                                                            if (baseObject.Race == ActorRace.Play)
                                                            {
                                                                baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                                VisibleHumanList.Add(baseObject.ActorId);
                                                            }
                                                            else if (baseObject.WantRefMsg)
                                                            {
                                                                if ((wIdent == Messages.RM_STRUCK) || (wIdent == Messages.RM_HEAR) || (wIdent == Messages.RM_DEATH))
                                                                {
                                                                    baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                                    VisibleHumanList.Add(baseObject.ActorId);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        cellInfo.Remove(cellObject);
                                                        if (cellInfo.Count <= 0)
                                                        {
                                                            cellInfo.Clear();
                                                        }
                                                        M2Share.Logger.Error(Format(sExceptionMsg, ChrName));
                                                        M2Share.Logger.Error(e.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return;
                }

                for (int nC = 0; nC < VisibleHumanList.Count; nC++)
                {
                    BaseObject baseObject = M2Share.ActorMgr.Get(VisibleHumanList[nC]);
                    if (baseObject.Ghost)
                    {
                        continue;
                    }
                    if ((baseObject.Envir == Envir) && (Math.Abs(baseObject.CurrX - CurrX) < 11) && (Math.Abs(baseObject.CurrY - CurrY) < 11))
                    {
                        if (baseObject.Race == ActorRace.Play)
                        {
                            baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                        }
                        else if (baseObject.WantRefMsg)
                        {
                            if ((wIdent == Messages.RM_STRUCK) || (wIdent == Messages.RM_HEAR) || (wIdent == Messages.RM_DEATH))
                            {
                                baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                            }
                        }
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }
    }
}