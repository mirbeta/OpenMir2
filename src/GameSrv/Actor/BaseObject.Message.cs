using GameSrv.Maps;
using M2Server;
using M2Server.Actor;
using Spectre.Console;
using System.Diagnostics;
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
            if (!Ghost)
            {
                SendMessage sendMessage = new SendMessage
                {
                    wIdent = wIdent,
                    wParam = wParam,
                    nParam1 = nParam1,
                    nParam2 = nParam2,
                    nParam3 = nParam3,
                    ActorId = this.ActorId,
                    LateDelivery = false,
                    Buff = sMsg
                };
                MsgQueue.Enqueue(sendMessage, (byte)Priority);
            }
        }

        public void SendMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "")
        {
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
                MsgQueue.Enqueue(sendMessage, (byte)wIdent);
            }
        }

        public void SendMsg(BaseObject baseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "")
        {
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
                //switch (wIdent)
                //{
                //    case Messages.CM_WALK:
                //    case Messages.CM_RUN:
                //    case Messages.CM_HIT:
                //    case Messages.CM_HEAVYHIT:
                //    case Messages.CM_BIGHIT:
                //    case Messages.CM_POWERHIT:
                //    case Messages.CM_LONGHIT:
                //    case Messages.CM_WIDEHIT:
                //    case Messages.CM_CRSHIT:
                //    case Messages.CM_FIREHIT:
                //    case Messages.CM_SPELL:
                //    case Messages.CM_SITDOWN:
                //    case Messages.CM_BUTCH:
                //    case Messages.CM_TURN:
                //    case Messages.CM_DEALTRY:
                //    case Messages.CM_SAY:
                //    case Messages.CM_PICKUP:
                //    case Messages.CM_EAT:
                //        sendMessage.wIdent = 0;
                //        break;
                //}
                MsgQueue.Enqueue(sendMessage, (byte)wIdent);
            }
        }

        /// <summary>
        /// 发送自身延时消息
        /// </summary>
        public void SendSelfDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
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
                MsgQueue.Enqueue(sendMessage, (byte)MessagePriority.Lowest);
            }
        }

        /// <summary>
        /// 发送伤害目标延时消息
        /// </summary>
        public void SendStruckDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
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
                    ActorId = Messages.RM_STRUCK
                };
                MsgQueue.Enqueue(sendMessage, (byte)MessagePriority.Lowest);
            }
        }

        /// <summary>
        /// 发送伤害目标延时消息
        /// </summary>
        public void SendStruckDelayMsg(int actorId, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
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
                MsgQueue.Enqueue(sendMessage, (byte)MessagePriority.Lowest);
            }
        }

        /// <summary>
        /// 更新延时消息
        /// </summary>
        internal void UpdateDelayMsg(int wIdent, short wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            int i;
            i = 0;
            while (true)
            {
                if (MsgQueue.Count <= i)
                {
                    break;
                }
                if (MsgQueue.TryPeek(out SendMessage sendMessage, out _))
                {
                    if ((sendMessage.wIdent == wIdent) && (sendMessage.nParam1 == lParam1))
                    {
                        MsgQueue.TryDequeue(out _, out _);
                        Dispose(sendMessage);
                    }
                }
                i++;
            }
            SendStruckDelayMsg(ActorId, wIdent, wParam, lParam1, lParam2, lParam3, sMsg, dwDelay);
        }

        /// <summary>
        /// 更新普通消息
        /// </summary>
        public void SendUpdateMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            int i;
            i = 0;
            while (true)
            {
                if (MsgQueue.Count <= i)
                {
                    break;
                }
                if (MsgQueue.TryPeek(out SendMessage sendMessage, out _))
                {
                    if (sendMessage.wIdent == wIdent)
                    {
                        MsgQueue.TryDequeue(out _, out _);
                        Dispose(sendMessage);
                    }
                }
                i++;
            }
            SendMsg(this, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        public void SendActionMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            int i;
            i = 0;
            while (true)
            {
                if (MsgQueue.Count <= i)
                {
                    break;
                }
                if (MsgQueue.TryPeek(out SendMessage sendMessage, out _))
                {
                    if ((sendMessage.wIdent == Messages.CM_TURN) || (sendMessage.wIdent == Messages.CM_WALK) ||
                        (sendMessage.wIdent == Messages.CM_SITDOWN) || (sendMessage.wIdent == Messages.CM_HORSERUN) ||
                        (sendMessage.wIdent == Messages.CM_RUN) || (sendMessage.wIdent == Messages.CM_HIT) ||
                        (sendMessage.wIdent == Messages.CM_HEAVYHIT) || (sendMessage.wIdent == Messages.CM_BIGHIT) ||
                        (sendMessage.wIdent == Messages.CM_POWERHIT) || (sendMessage.wIdent == Messages.CM_LONGHIT) ||
                        (sendMessage.wIdent == Messages.CM_WIDEHIT) || (sendMessage.wIdent == Messages.CM_FIREHIT))
                    {
                        MsgQueue.TryDequeue(out _, out _);
                        Dispose(sendMessage);
                    }
                }
                i++;
            }
            SendMsg(this, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        internal bool GetMessage(ref ProcessMessage msg)
        {
            bool result = false;
            if (MsgQueue.TryDequeue(out SendMessage sendMessage, out _))
            {
                if ((sendMessage.DeliveryTime > 0) && (HUtil32.GetTickCount() < sendMessage.DeliveryTime))
                {
                    MsgQueue.Enqueue(sendMessage, (byte)MessagePriority.Lowest);//延时消息优先级最低
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
            return result;
        }

        internal bool GetMessage(int msgTick, ref ProcessMessage msg)
        {
            bool result = false;
            if (MsgQueue.TryDequeue(out SendMessage sendMessage, out _))
            {
                if ((sendMessage.DeliveryTime > 0) && (msgTick < sendMessage.DeliveryTime))
                {
                    MsgQueue.Enqueue(sendMessage, (byte)MessagePriority.Lowest);//延时消息优先级最低
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
            return result;
        }

        /// <summary>
        /// 发送广播消息
        /// </summary>
        public void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SendRefMsg Name = {0}";
            if (Envir == null)
            {
                GameShare.Logger.Error(ChrName + " SendRefMsg nil PEnvir ");
                return;
            }
            if (ObMode || FixedHideMode)
            {
                SendMsg(wIdent, wParam, nParam1, nParam2, nParam3, sMsg); // 如果隐身模式则只发送信息给自己
                return;
            }
            if (((HUtil32.GetTickCount() - SendRefMsgTick) >= 500) || (VisibleHumanList.Count == 0))
            {
                SendRefMsgTick = HUtil32.GetTickCount();
                VisibleHumanList.Clear();
                short nLx = (short)(CurrX - GameShare.Config.SendRefMsgRange); // 12
                short nHx = (short)(CurrX + GameShare.Config.SendRefMsgRange); // 12
                short nLy = (short)(CurrY - GameShare.Config.SendRefMsgRange); // 12
                short nHy = (short)(CurrY + GameShare.Config.SendRefMsgRange); // 12
                for (short nCx = nLx; nCx <= nHx; nCx++)
                {
                    for (short nCy = nLy; nCy <= nHy; nCy++)
                    {
                        if (!Envir.CellMatch(nCx, nCy))
                        {
                            continue;
                        }
                        ref MapCellInfo cellInfo = ref Envir.GetCellInfo(nCx, nCy, out bool cellSuccess);//占用CPU
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            for (int i = 0; i < cellInfo.ObjList.Count; i++)
                            {
                                CellObject cellObject = cellInfo.ObjList[i];
                                if (cellObject.ActorObject)
                                {
                                    //if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                    //{
                                    //    cellInfo.Remove(i);
                                    //    if (cellInfo.Count <= 0)
                                    //    {
                                    //        cellInfo.Clear();
                                    //        break;
                                    //    }
                                    //}
                                    BaseObject targetObject = GameShare.ActorMgr.Get(cellObject.CellObjId);
                                    if ((targetObject != null) && !targetObject.Ghost)
                                    {
                                        if (targetObject.Race == ActorRace.Play)
                                        {
                                            targetObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                            VisibleHumanList.Add(targetObject.ActorId);
                                        }
                                        else if (targetObject.WantRefMsg)
                                        {
                                            if ((wIdent == Messages.RM_STRUCK) || (wIdent == Messages.RM_HEAR) || (wIdent == Messages.RM_DEATH))
                                            {
                                                targetObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                VisibleHumanList.Add(targetObject.ActorId);
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

            for (int i = 0; i < VisibleHumanList.Count; i++)
            {
                BaseObject targetObject = GameShare.ActorMgr.Get(VisibleHumanList[i]);
                if (targetObject.Ghost)
                {
                    continue;
                }
                if ((targetObject.Envir == Envir) && (Math.Abs(targetObject.CurrX - CurrX) < 11) && (Math.Abs(targetObject.CurrY - CurrY) < 11))
                {
                    if (targetObject.Race == ActorRace.Play)
                    {
                        targetObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                    }
                    else if (targetObject.WantRefMsg)
                    {
                        if ((wIdent == Messages.RM_STRUCK) || (wIdent == Messages.RM_HEAR) || (wIdent == Messages.RM_DEATH))
                        {
                            targetObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                        }
                    }
                }
            }
        }
    }
}