using GameSvr.Actor;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Monster.Monsters;
using GameSvr.Npc;
using GameSvr.Services;
using GameSvr.World;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        public override void Run()
        {
            int tObjCount;
            int nInteger;
            var processMsg = default(ProcessMessage);
            const string sPayMentExpire = "您的帐户充值时间已到期!!!";
            const string sDisConnectMsg = "游戏被强行中断!!!";
            const string sExceptionMsg1 = "[Exception] TPlayObject::Run -> Operate 1";
            const string sExceptionMsg2 = "[Exception] TPlayObject::Run -> Operate 2 # {0} Ident:{1} Sender:{2} wP:{3} nP1:{4} nP2:{5} np3:{6} Msg:{7}";
            const string sExceptionMsg3 = "[Exception] TPlayObject::Run -> GetHighHuman";
            const string sExceptionMsg4 = "[Exception] TPlayObject::Run -> ClearObj";
            try
            {
                if (Dealing)
                {
                    if (GetPoseCreate() != DealCreat || DealCreat == this || DealCreat == null)
                    {
                        DealCancel();
                    }
                }
                if (M2Share.Config.PayMentMode == 3)
                {
                    if (HUtil32.GetTickCount() - AccountExpiredTick > QueryExpireTick)//一分钟查询一次账号游戏到期时间
                    {
                        ExpireTime = ExpireTime - 60;//游戏时间减去一分钟
                        IdSrvClient.Instance.SendUserPlayTime(UserAccount, ExpireTime);
                        AccountExpiredTick = HUtil32.GetTickCount();
                        CheckExpiredTime();
                    }
                    if (AccountExpired)
                    {
                        SysMsg(sPayMentExpire, MsgColor.Red, MsgType.Hint);
                        SysMsg(sDisConnectMsg, MsgColor.Red, MsgType.Hint);
                        BoEmergencyClose = true;
                        AccountExpired = false;
                    }
                }
                if (FireHitSkill && (HUtil32.GetTickCount() - LatestFireHitTick) > 20 * 1000)
                {
                    FireHitSkill = false;
                    SysMsg(Settings.SpiritsGone, MsgColor.Red, MsgType.Hint);
                    SendSocket("+UFIR");
                }
                if (TwinHitSkill && (HUtil32.GetTickCount() - LatestTwinHitTick) > 60 * 1000)
                {
                    TwinHitSkill = false;
                    SendSocket("+UTWN");
                }
                if (IsTimeRecall && HUtil32.GetTickCount() > TimeRecallTick) //执行 TimeRecall回到原地
                {
                    IsTimeRecall = false;
                    SpaceMove(TimeRecallMoveMap, TimeRecallMoveX, TimeRecallMoveY, 0);
                }
                for (var i = 0; i < 20; i++) //个人定时器
                {
                    if (AutoTimerStatus[i] > 500)
                    {
                        if ((HUtil32.GetTickCount() - AutoTimerTick[i]) > AutoTimerStatus[i])
                        {
                            if (M2Share.ManageNPC != null)
                            {
                                AutoTimerTick[i] = HUtil32.GetTickCount();
                                ScriptGotoCount = 0;
                                M2Share.ManageNPC.GotoLable(this, "@OnTimer" + i, false);
                            }
                        }
                    }
                }
                var boNeedRecalc = false;
                for (var i = 0; i < ExtraAbil.Length; i++)
                {
                    if (ExtraAbil[i] > 0)
                    {
                        if (HUtil32.GetTickCount() > ExtraAbilTimes[i])
                        {
                            ExtraAbil[i] = 0;
                            ExtraAbilFlag[i] = 0;
                            boNeedRecalc = true;
                            switch (i)
                            {
                                case 0:
                                    SysMsg("攻击力恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 1:
                                    SysMsg("魔法力恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 2:
                                    SysMsg("精神力恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 3:
                                    SysMsg("攻击速度恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 4:
                                    SysMsg("体力值恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 5:
                                    SysMsg("魔法值恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 6:
                                    SysMsg("攻击能力恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                            }
                        }
                        else if (ExtraAbilFlag[i] == 0 && HUtil32.GetTickCount() > ExtraAbilTimes[i] - 10000)
                        {
                            ExtraAbilFlag[i] = 1;
                            switch (i)
                            {
                                case AbilConst.EABIL_DCUP:
                                    SysMsg("攻击力10秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case AbilConst.EABIL_MCUP:
                                    SysMsg("魔法力10秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case AbilConst.EABIL_SCUP:
                                    SysMsg("精神力10秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case AbilConst.EABIL_HITSPEEDUP:
                                    SysMsg("攻击速度10秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case AbilConst.EABIL_HPUP:
                                    SysMsg("体力值10秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                case AbilConst.EABIL_MPUP:
                                    SysMsg("魔法值10秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                            }
                        }
                    }
                }
                if (boNeedRecalc)
                {
                    HealthSpellChanged();
                }
                LifeStone();
                if (IsTimeGoto && (HUtil32.GetTickCount() > TimeGotoTick)) //Delaygoto延时跳转
                {
                    IsTimeGoto = false;
                    ((Merchant)TimeGotoNpc)?.GotoLable(this, TimeGotoLable, false);
                }
                // 增加挂机
                if (OffLineFlag && HUtil32.GetTickCount() > KickOffLineTick)
                {
                    OffLineFlag = false;
                    BoSoftClose = true;
                }
                if (IsDelayCall && (HUtil32.GetTickCount() - DelayCallTick) > DelayCall)
                {
                    IsDelayCall = false;
                    NormNpc normNpc = WorldServer.FindMerchant<Merchant>(MDelayCallNpc);
                    if (normNpc == null)
                    {
                        normNpc = WorldServer.FindNpc<NormNpc>(MDelayCallNpc);
                    }
                    if (normNpc != null)
                    {
                        normNpc.GotoLable(this, DelayCallLabel, false);
                    }
                }
                if ((HUtil32.GetTickCount() - DecPkPointTick) > M2Share.Config.DecPkPointTime)// 减少PK值
                {
                    DecPkPointTick = HUtil32.GetTickCount();
                    if (PkPoint > 0)
                    {
                        DecPkPoint(M2Share.Config.DecPkPointCount);
                    }
                }
                if ((HUtil32.GetTickCount() - DecLightItemDrugTick) > M2Share.Config.DecLightItemDrugTime)
                {
                    DecLightItemDrugTick += M2Share.Config.DecLightItemDrugTime;
                    UseLamp();
                    CheckPkStatus();
                }
                if ((HUtil32.GetTickCount() - CheckDupObjTick) > 3000)
                {
                    CheckDupObjTick = HUtil32.GetTickCount();
                    GetStartPoint();
                    tObjCount = Envir.GetXyObjCount(CurrX, CurrY);
                    if (tObjCount >= 2)
                    {
                        if (!BoDuplication)
                        {
                            BoDuplication = true;
                            DupStartTick = HUtil32.GetTickCount();
                        }
                    }
                    else
                    {
                        BoDuplication = false;
                    }
                    if ((tObjCount >= 3 && ((HUtil32.GetTickCount() - DupStartTick) > 3000) || tObjCount == 2
                        && ((HUtil32.GetTickCount() - DupStartTick) > 10000)) && ((HUtil32.GetTickCount() - DupStartTick) < 20000))
                    {
                        CharPushed(M2Share.RandomNumber.RandomByte(8), 1);
                    }
                }
                var castle = M2Share.CastleMgr.InCastleWarArea(this);
                if (castle != null && castle.UnderWar)
                {
                    ChangePkStatus(true);
                }
                if ((HUtil32.GetTickCount() - DiscountForNightTick) > 1000)
                {
                    DiscountForNightTick = HUtil32.GetTickCount();
                    var wHour = DateTime.Now.Hour;
                    var wMin = DateTime.Now.Minute;
                    var wSec = DateTime.Now.Second;
                    var wMSec = DateTime.Now.Millisecond;
                    if (M2Share.Config.DiscountForNightTime && (wHour == M2Share.Config.HalfFeeStart || wHour == M2Share.Config.HalfFeeEnd))
                    {
                        if (wMin == 0 && wSec <= 30 && (HUtil32.GetTickCount() - LogonTick) > 60000)
                        {
                            LogonTimcCost();
                            LogonTick = HUtil32.GetTickCount();
                            LogonTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        }
                    }
                    if (MyGuild != null)
                    {
                        if (MyGuild.GuildWarList.Count > 0)
                        {
                            var boInSafeArea = InSafeArea();
                            if (boInSafeArea != IsSafeArea)
                            {
                                IsSafeArea = boInSafeArea;
                                RefNameColor();
                            }
                        }
                    }
                    if (castle != null && castle.UnderWar)
                    {
                        if (Envir == castle.PalaceEnvir && MyGuild != null)
                        {
                            if (!castle.IsMember(this))
                            {
                                if (castle.IsAttackGuild(MyGuild))
                                {
                                    if (castle.CanGetCastle(MyGuild))
                                    {
                                        castle.GetCastle(MyGuild);
                                        WorldServer.SendServerGroupMsg(Messages.SS_211, M2Share.ServerIndex, MyGuild.sGuildName);
                                        if (castle.InPalaceGuildCount() <= 1)
                                        {
                                            castle.StopWallconquestWar();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ChangePkStatus(false);
                    }
                    if (NameColorChanged)
                    {
                        NameColorChanged = false;
                        RefUserState();
                        RefShowName();
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg1);
            }
            try
            {
                MDwGetMsgTick = HUtil32.GetTickCount();
                while (((HUtil32.GetTickCount() - MDwGetMsgTick) < M2Share.Config.HumanGetMsgTime) && GetMessage(out processMsg))
                {
                    if (!Operate(processMsg))
                    {
                        break;
                    }
                }
                if (BoEmergencyClose || BoKickFlag || BoSoftClose)
                {
                    if (SwitchData)
                    {
                        MapName = SwitchMapName;
                        CurrX = SwitchMapX;
                        CurrY = SwitchMapY;
                    }
                    MakeGhost();
                    if (BoKickFlag)
                    {
                        SendDefMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                    }
                    if (!BoReconnection && BoSoftClose)
                    {
                        MyGuild = M2Share.GuildMgr.MemberOfGuild(ChrName);
                        if (MyGuild != null)
                        {
                            MyGuild.SendGuildMsg(ChrName + " 已经退出游戏.");
                            WorldServer.SendServerGroupMsg(Messages.SS_208, M2Share.ServerIndex, MyGuild.sGuildName + '/' + "" + '/' + ChrName + " has exited the game.");
                        }
                        IdSrvClient.Instance.SendHumanLogOutMsg(UserAccount, SessionId);
                    }
                }
            }
            catch (Exception e)
            {
                if (processMsg.wIdent >= 0)
                {
                    if (processMsg.wIdent == 0)
                    {
                        MakeGhost();//用于处理 人物异常退出，但人物还在游戏中问题
                    }
                    M2Share.Logger.Error(Format(sExceptionMsg2, ChrName, processMsg.wIdent, processMsg.BaseObject, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg));
                }
                M2Share.Logger.Error(e.Message);
            }
            var boTakeItem = false;
            // 检查身上的装备有没不符合
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].Index > 0)
                {
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (stdItem != null)
                    {
                        if (!CheckItemsNeed(stdItem))
                        {
                            // m_ItemList.Add((UserItem));
                            var userItem = UseItems[i];
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                WeightChanged();
                                boTakeItem = true;
                            }
                            else
                            {
                                if (DropItemDown(UseItems[i], 1, false, 0, ActorId))
                                {
                                    boTakeItem = true;
                                }
                            }
                            if (boTakeItem)
                            {
                                SendDelItems(UseItems[i]);
                                UseItems[i].Index = 0;
                                RecalcAbilitys();
                            }
                        }
                    }
                    else
                    {
                        UseItems[i].Index = 0;
                    }
                }
            }
            tObjCount = GameGold;
            if (BoDecGameGold && (HUtil32.GetTickCount() - DecGameGoldTick) > DecGameGoldTime)
            {
                DecGameGoldTick = HUtil32.GetTickCount();
                if (GameGold >= DecGameGold)
                {
                    GameGold -= DecGameGold;
                    nInteger = DecGameGold;
                }
                else
                {
                    nInteger = GameGold;
                    GameGold = 0;
                    BoDecGameGold = false;
                    MoveToHome();
                }
                if (M2Share.GameLogGameGold)
                {
                    M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (BoIncGameGold && (HUtil32.GetTickCount() - IncGameGoldTick) > IncGameGoldTime)
            {
                IncGameGoldTick = HUtil32.GetTickCount();
                if (GameGold + IncGameGold < 2000000)
                {
                    GameGold += IncGameGold;
                    nInteger = IncGameGold;
                }
                else
                {
                    GameGold = 2000000;
                    nInteger = 2000000 - GameGold;
                    BoIncGameGold = false;
                }
                if (M2Share.GameLogGameGold)
                {
                    M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '-', "Auto"));
                }
            }
            if (!BoDecGameGold && Envir.Flag.boDECGAMEGOLD)
            {
                if ((HUtil32.GetTickCount() - DecGameGoldTick) > Envir.Flag.nDECGAMEGOLDTIME * 1000)
                {
                    DecGameGoldTick = HUtil32.GetTickCount();
                    if (GameGold >= Envir.Flag.nDECGAMEGOLD)
                    {
                        GameGold -= Envir.Flag.nDECGAMEGOLD;
                        nInteger = Envir.Flag.nDECGAMEGOLD;
                    }
                    else
                    {
                        nInteger = GameGold;
                        GameGold = 0;
                        BoDecGameGold = false;
                        MoveToHome();
                    }
                    if (M2Share.GameLogGameGold)
                    {
                        M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '-', "Map"));
                    }
                }
            }
            if (!BoIncGameGold && Envir.Flag.boINCGAMEGOLD)
            {
                if ((HUtil32.GetTickCount() - IncGameGoldTick) > (Envir.Flag.nINCGAMEGOLDTIME * 1000))
                {
                    IncGameGoldTick = HUtil32.GetTickCount();
                    if (GameGold + Envir.Flag.nINCGAMEGOLD <= 2000000)
                    {
                        GameGold += Envir.Flag.nINCGAMEGOLD;
                        nInteger = Envir.Flag.nINCGAMEGOLD;
                    }
                    else
                    {
                        nInteger = 2000000 - GameGold;
                        GameGold = 2000000;
                    }
                    if (M2Share.GameLogGameGold)
                    {
                        M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GameGoldName, nInteger, '+', "Map"));
                    }
                }
            }
            if (tObjCount != GameGold)
            {
                SendUpdateMsg(this, Messages.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
            if (Envir.Flag.Fight3Zone)
            {
                FightZoneDieCount++;
                if (MyGuild != null)
                {
                    MyGuild.TeamFightWhoDead(ChrName);
                }
                if (LastHiter != null && LastHiter.Race == ActorRace.Play)
                {
                    var lastHiterPlay = LastHiter as PlayObject;
                    if (lastHiterPlay.MyGuild != null && MyGuild != null)
                    {
                        lastHiterPlay.MyGuild.TeamFightWhoWinPoint(LastHiter.ChrName, 100);
                        var tStr = lastHiterPlay.MyGuild.sGuildName + ':' + lastHiterPlay.MyGuild.nContestPoint + "  " + MyGuild.sGuildName + ':' + MyGuild.nContestPoint;
                        M2Share.WorldEngine.CryCry(Messages.RM_CRY, Envir, CurrX, CurrY, 1000, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, "- " + tStr);
                    }
                }
            }
            if (Envir.Flag.boINCGAMEPOINT)
            {
                if ((HUtil32.GetTickCount() - IncGamePointTick) > (Envir.Flag.nINCGAMEPOINTTIME * 1000))
                {
                    IncGamePointTick = HUtil32.GetTickCount();
                    if (GamePoint + Envir.Flag.nINCGAMEPOINT <= 2000000)
                    {
                        GamePoint += Envir.Flag.nINCGAMEPOINT;
                        nInteger = Envir.Flag.nINCGAMEPOINT;
                    }
                    else
                    {
                        GamePoint = 2000000;
                        nInteger = 2000000 - GamePoint;
                    }
                    if (M2Share.GameLogGamePoint)
                    {
                        M2Share.EventSource.AddEventLog(Grobal2.LogGamePoint, Format(CommandHelp.GameLogMsg1, MapName, CurrX, CurrY, ChrName, M2Share.Config.GamePointName, nInteger, '+', "Map"));
                    }
                }
            }
            if (Envir.Flag.boDECHP && (HUtil32.GetTickCount() - DecHpTick) > (Envir.Flag.nDECHPTIME * 1000))
            {
                DecHpTick = HUtil32.GetTickCount();
                if (WAbil.HP > Envir.Flag.nDECHPPOINT)
                {
                    WAbil.HP -= (ushort)Envir.Flag.nDECHPPOINT;
                }
                else
                {
                    WAbil.HP = 0;
                }
                HealthSpellChanged();
            }
            if (Envir.Flag.boINCHP && (HUtil32.GetTickCount() - IncHpTick) > (Envir.Flag.nINCHPTIME * 1000))
            {
                IncHpTick = HUtil32.GetTickCount();
                if (WAbil.HP + Envir.Flag.nDECHPPOINT < WAbil.MaxHP)
                {
                    WAbil.HP += (ushort)Envir.Flag.nDECHPPOINT;
                }
                else
                {
                    WAbil.HP = WAbil.MaxHP;
                }
                HealthSpellChanged();
            }
            // 降饥饿点
            if (M2Share.Config.HungerSystem)
            {
                if ((HUtil32.GetTickCount() - DecHungerPointTick) > 1000)
                {
                    DecHungerPointTick = HUtil32.GetTickCount();
                    if (HungerStatus > 0)
                    {
                        tObjCount = GetMyStatus();
                        HungerStatus -= 1;
                        if (tObjCount != GetMyStatus())
                        {
                            RefMyStatus();
                        }
                    }
                    else
                    {
                        if (M2Share.Config.HungerDecHP)
                        {
                            // 减少涨HP，MP
                            HealthTick -= 60;
                            SpellTick -= 10;
                            SpellTick = HUtil32._MAX(0, SpellTick);
                            PerHealth -= 1;
                            PerSpell -= 1;
                            if (WAbil.HP > WAbil.HP / 100)
                            {
                                WAbil.HP -= (ushort)HUtil32._MAX(1, WAbil.HP / 100);
                            }
                            else
                            {
                                if (WAbil.HP <= 2)
                                {
                                    WAbil.HP = 0;
                                }
                            }
                            HealthSpellChanged();
                        }
                    }
                }
            }
            if ((HUtil32.GetTickCount() - ExpRateTick) > 1000)
            {
                ExpRateTick = HUtil32.GetTickCount();
                if (KillMonExpRateTime > 0)
                {
                    KillMonExpRateTime -= 1;
                    if (KillMonExpRateTime == 0)
                    {
                        KillMonExpRate = 100;
                        SysMsg("经验倍数恢复正常...", MsgColor.Red, MsgType.Hint);
                    }
                }
                if (PowerRateTime > 0)
                {
                    PowerRateTime -= 1;
                    if (PowerRateTime == 0)
                    {
                        PowerRate = 100;
                        SysMsg("攻击力倍数恢复正常...", MsgColor.Red, MsgType.Hint);
                    }
                }
            }
            try
            {
                // 取得在线最高等级、PK、攻击力、魔法、道术 的人物
                if (M2Share.HighLevelHuman == ActorId && (Death || Ghost))
                {
                    M2Share.HighLevelHuman = 0;
                }
                if (M2Share.HighPKPointHuman == ActorId && (Death || Ghost))
                {
                    M2Share.HighPKPointHuman = 0;
                }
                if (M2Share.HighDCHuman == ActorId && (Death || Ghost))
                {
                    M2Share.HighDCHuman = 0;
                }
                if (M2Share.HighMCHuman == ActorId && (Death || Ghost))
                {
                    M2Share.HighMCHuman = 0;
                }
                if (M2Share.HighSCHuman == ActorId && (Death || Ghost))
                {
                    M2Share.HighSCHuman = 0;
                }
                if (M2Share.HighOnlineHuman == ActorId && (Death || Ghost))
                {
                    M2Share.HighOnlineHuman = 0;
                }
                if (Permission < 6)
                {
                    // 最高等级
                    var targetObject = M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
                    if (M2Share.HighLevelHuman == 0 || targetObject.Ghost)
                    {
                        M2Share.HighLevelHuman = ActorId;
                    }
                    else
                    {
                        if (Abil.Level > targetObject.Abil.Level)
                        {
                            M2Share.HighLevelHuman = ActorId;
                        }
                    }

                    // 最高PK
                    targetObject = M2Share.ActorMgr.Get(M2Share.HighPKPointHuman);
                    if (M2Share.HighPKPointHuman == 0 || targetObject.Ghost)
                    {
                        if (PkPoint > 0)
                        {
                            M2Share.HighPKPointHuman = ActorId;
                        }
                    }
                    else
                    {
                        if (PkPoint > ((PlayObject)targetObject).PkPoint)
                        {
                            M2Share.HighPKPointHuman = ActorId;
                        }
                    }

                    // 最高攻击力
                    targetObject = M2Share.ActorMgr.Get(M2Share.HighDCHuman);
                    if (M2Share.HighDCHuman == 0 || targetObject.Ghost)
                    {
                        M2Share.HighDCHuman = ActorId;
                    }
                    else
                    {
                        if (HUtil32.HiWord(WAbil.DC) > HUtil32.HiWord(targetObject.WAbil.DC))
                        {
                            M2Share.HighDCHuman = ActorId;
                        }
                    }

                    // 最高魔法
                    targetObject = M2Share.ActorMgr.Get(M2Share.HighMCHuman);
                    if (M2Share.HighMCHuman == 0 || targetObject.Ghost)
                    {
                        M2Share.HighMCHuman = ActorId;
                    }
                    else
                    {
                        if (HUtil32.HiWord(WAbil.MC) > HUtil32.HiWord(targetObject.WAbil.MC))
                        {
                            M2Share.HighMCHuman = ActorId;
                        }
                    }

                    // 最高道术
                    targetObject = M2Share.ActorMgr.Get(M2Share.HighSCHuman);
                    if (M2Share.HighSCHuman == 0 || targetObject.Ghost)
                    {
                        M2Share.HighSCHuman = ActorId;
                    }
                    else
                    {
                        if (HUtil32.HiWord(WAbil.SC) > HUtil32.HiWord(targetObject.WAbil.SC))
                        {
                            M2Share.HighSCHuman = ActorId;
                        }
                    }

                    // 最长在线时间
                    targetObject = M2Share.ActorMgr.Get(M2Share.HighOnlineHuman);
                    if (M2Share.HighOnlineHuman == 0 || targetObject.Ghost)
                    {
                        M2Share.HighOnlineHuman = ActorId;
                    }
                    else
                    {
                        if (LogonTick < ((PlayObject)targetObject).LogonTick)
                        {
                            M2Share.HighOnlineHuman = ActorId;
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Logger.Error(sExceptionMsg3);
            }
            if (M2Share.Config.ReNewChangeColor && ReLevel > 0 && (HUtil32.GetTickCount() - ReColorTick) > M2Share.Config.ReNewNameColorTime)
            {
                ReColorTick = HUtil32.GetTickCount();
                ReColorIdx++;
                if (ReColorIdx >= M2Share.Config.ReNewNameColor.Length)
                {
                    ReColorIdx = 0;
                }
                NameColor = M2Share.Config.ReNewNameColor[ReColorIdx];
                RefNameColor();
            }
            // 检测侦听私聊对像
            if (WhisperHuman != null)
            {
                if (WhisperHuman.Death || WhisperHuman.Ghost)
                {
                    WhisperHuman = null;
                }
            }
            ProcessSpiritSuite();
            try
            {
                if ((HUtil32.GetTickCount() - ClearInvalidObjTick) > 30 * 1000)
                {
                    ClearInvalidObjTick = HUtil32.GetTickCount();
                    if (DearHuman != null && (DearHuman.Death || DearHuman.Ghost))
                    {
                        DearHuman = null;
                    }
                    if (IsMaster)
                    {
                        for (var i = MasterList.Count - 1; i >= 0; i--)
                        {
                            if (MasterList[i].Death || MasterList[i].Ghost)
                            {
                                MasterList.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        if (MasterHuman != null && (MasterHuman.Death || MasterHuman.Ghost))
                        {
                            MasterHuman = null;
                        }
                    }

                    // 清组队已死亡成员
                    if (GroupOwner != 0)
                    {
                        var groupOwnerPlay = (PlayObject)M2Share.ActorMgr.Get(GroupOwner);
                        if (groupOwnerPlay.Death || groupOwnerPlay.Ghost)
                        {
                            GroupOwner = 0;
                        }
                    }

                    if (GroupOwner == ActorId)
                    {
                        for (var i = GroupMembers.Count - 1; i >= 0; i--)
                        {
                            BaseObject baseObject = GroupMembers[i];
                            if (baseObject.Death || baseObject.Ghost)
                            {
                                GroupMembers.RemoveAt(i);
                            }
                        }
                    }

                    // 检查交易双方 状态
                    if ((DealCreat != null) && DealCreat.Ghost)
                    {
                        DealCreat = null;
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg4);
                M2Share.Logger.Error(e.Message);
            }
            if (AutoGetExpPoint > 0 && (AutoGetExpEnvir == null || AutoGetExpEnvir == Envir) && (HUtil32.GetTickCount() - AutoGetExpTick) > AutoGetExpTime)
            {
                AutoGetExpTick = HUtil32.GetTickCount();
                if (!AutoGetExpInSafeZone || AutoGetExpInSafeZone && InSafeZone())
                {
                    GetExp(AutoGetExpPoint);
                }
            }
            base.Run();
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            CharDesc charDesc;
            int nObjCount;
            string sendMsg;
            var dwDelayTime = 0;
            int nMsgCount;
            var result = true;
            BaseObject baseObject = null;
            if (processMsg.BaseObject > 0)
            {
                baseObject = M2Share.ActorMgr.Get(processMsg.BaseObject);
            }
            MessageBodyWL messageBodyWl;
            switch (processMsg.wIdent)
            {
                case Messages.CM_QUERYUSERNAME:
                    ClientQueryUserName(processMsg.nParam1, processMsg.nParam2, processMsg.nParam3);
                    break;
                case Messages.CM_QUERYBAGITEMS: //僵尸攻击：不断刷新包裹发送大量数据，导致网络阻塞
                    if ((HUtil32.GetTickCount() - QueryBagItemsTick) > 30 * 1000)
                    {
                        QueryBagItemsTick = HUtil32.GetTickCount();
                        ClientQueryBagItems();
                    }
                    else
                    {
                        SysMsg(Settings.QUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case Messages.CM_QUERYUSERSTATE:
                    ClientQueryUserInformation(processMsg.nParam1, processMsg.nParam2, processMsg.nParam3);
                    break;
                case Messages.CM_QUERYUSERSET:
                    ClientQueryUserSet(processMsg);
                    break;
                case Messages.CM_DROPITEM:
                    if (ClientDropItem(processMsg.Msg, processMsg.nParam1))
                    {
                        SendDefMessage(Messages.SM_DROPITEM_SUCCESS, processMsg.nParam1, 0, 0, 0, processMsg.Msg);
                    }
                    else
                    {
                        SendDefMessage(Messages.SM_DROPITEM_FAIL, processMsg.nParam1, 0, 0, 0, processMsg.Msg);
                    }
                    break;
                case Messages.CM_PICKUP:
                    if (CurrX == processMsg.nParam2 && CurrY == processMsg.nParam3)
                    {
                        ClientPickUpItem();
                    }
                    break;
                case Messages.CM_OPENDOOR:
                    ClientOpenDoor(processMsg.nParam2, processMsg.nParam3);
                    break;
                case Messages.CM_TAKEONITEM:
                    ClientTakeOnItems((byte)processMsg.nParam2, processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_TAKEOFFITEM:
                    ClientTakeOffItems((byte)processMsg.nParam2, processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_EAT:
                    ClientUseItems(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_BUTCH:
                    if (!ClientGetButchItem(processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, (byte)processMsg.wParam, ref dwDelayTime))
                    {
                        if (dwDelayTime != 0)
                        {
                            nMsgCount = GetDigUpMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxDigUpMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.BunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.Config.DropOverSpeed)
                                {
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendSocket(M2Share.GetGoodTick);
                                }
                                else
                                {
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Messages.CM_MAGICKEYCHANGE:
                    ClientChangeMagicKey(processMsg.nParam1, (char)processMsg.nParam2);
                    break;
                case Messages.CM_SOFTCLOSE:
                    if (!OffLineFlag)
                    {
                        BoReconnection = true;
                        BoSoftClose = true;
                        if (processMsg.wParam == 1)
                        {
                            BoEmergencyClose = true;
                        }
                    }
                    break;
                case Messages.CM_CLICKNPC:
                    ClientClickNpc(processMsg.nParam1);
                    break;
                case Messages.CM_MERCHANTDLGSELECT:
                    ClientMerchantDlgSelect(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_MERCHANTQUERYSELLPRICE:
                    ClientMerchantQuerySellPrice(processMsg.nParam1, HUtil32.MakeLong((short)processMsg.nParam2, (short)processMsg.nParam3), processMsg.Msg);
                    break;
                case Messages.CM_USERSELLITEM:
                    ClientUserSellItem(processMsg.nParam1, HUtil32.MakeLong((ushort)processMsg.nParam2, (ushort)processMsg.nParam3), processMsg.Msg);
                    break;
                case Messages.CM_USERBUYITEM:
                    ClientUserBuyItem(processMsg.wIdent, processMsg.nParam1, HUtil32.MakeLong((ushort)processMsg.nParam2, (ushort)processMsg.nParam3), 0, processMsg.Msg);
                    break;
                case Messages.CM_USERGETDETAILITEM:
                    ClientUserBuyItem(processMsg.wIdent, processMsg.nParam1, 0, processMsg.nParam2, processMsg.Msg);
                    break;
                case Messages.CM_DROPGOLD:
                    if (processMsg.nParam1 > 0)
                    {
                        ClientDropGold(processMsg.nParam1);
                    }
                    break;
                case Messages.CM_TEST:
                    SendDefMessage(1, 0, 0, 0, 0, "");
                    break;
                case Messages.CM_GROUPMODE:
                    if (processMsg.nParam2 == 0)
                    {
                        ClientGroupClose();
                    }
                    else
                    {
                        AllowGroup = true;
                    }
                    if (AllowGroup)
                    {
                        SendDefMessage(Messages.SM_GROUPMODECHANGED, 0, 1, 0, 0, "");
                    }
                    else
                    {
                        SendDefMessage(Messages.SM_GROUPMODECHANGED, 0, 0, 0, 0, "");
                    }
                    break;
                case Messages.CM_CREATEGROUP:
                    ClientCreateGroup(processMsg.Msg.Trim());
                    break;
                case Messages.CM_ADDGROUPMEMBER:
                    ClientAddGroupMember(processMsg.Msg.Trim());
                    break;
                case Messages.CM_DELGROUPMEMBER:
                    ClientDelGroupMember(processMsg.Msg.Trim());
                    break;
                case Messages.CM_USERREPAIRITEM:
                    ClientRepairItem(processMsg.nParam1, HUtil32.MakeLong((ushort)processMsg.nParam2, (ushort)processMsg.nParam3), processMsg.Msg);
                    break;
                case Messages.CM_MERCHANTQUERYREPAIRCOST:
                    ClientQueryRepairCost(processMsg.nParam1, HUtil32.MakeLong((ushort)processMsg.nParam2, (ushort)processMsg.nParam3), processMsg.Msg);
                    break;
                case Messages.CM_DEALTRY:
                    ClientDealTry(processMsg.Msg.Trim());
                    break;
                case Messages.CM_DEALADDITEM:
                    ClientAddDealItem(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_DEALDELITEM:
                    ClientDelDealItem(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_DEALCANCEL:
                    ClientCancelDeal();
                    break;
                case Messages.CM_DEALCHGGOLD:
                    ClientChangeDealGold(processMsg.nParam1);
                    break;
                case Messages.CM_DEALEND:
                    ClientDealEnd();
                    break;
                case Messages.CM_USERSTORAGEITEM:
                    ClientStorageItem(processMsg.nParam1, HUtil32.MakeLong((ushort)processMsg.nParam2, (ushort)processMsg.nParam3), processMsg.Msg);
                    break;
                case Messages.CM_USERTAKEBACKSTORAGEITEM:
                    ClientTakeBackStorageItem(processMsg.nParam1, HUtil32.MakeLong((ushort)processMsg.nParam2, (ushort)processMsg.nParam3), processMsg.Msg);
                    break;
                case Messages.CM_WANTMINIMAP:
                    ClientGetMinMap();
                    break;
                case Messages.CM_USERMAKEDRUGITEM:
                    ClientMakeDrugItem(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_OPENGUILDDLG:
                    ClientOpenGuildDlg();
                    break;
                case Messages.CM_GUILDHOME:
                    ClientGuildHome();
                    break;
                case Messages.CM_GUILDMEMBERLIST:
                    ClientGuildMemberList();
                    break;
                case Messages.CM_GUILDADDMEMBER:
                    ClientGuildAddMember(processMsg.Msg);
                    break;
                case Messages.CM_GUILDDELMEMBER:
                    ClientGuildDelMember(processMsg.Msg);
                    break;
                case Messages.CM_GUILDUPDATENOTICE:
                    ClientGuildUpdateNotice(processMsg.Msg);
                    break;
                case Messages.CM_GUILDUPDATERANKINFO:
                    ClientGuildUpdateRankInfo(processMsg.Msg);
                    break;
                case Messages.CM_1042:
                    M2Share.Logger.Warn("[非法数据] " + ChrName);
                    break;
                case Messages.CM_ADJUST_BONUS:
                    ClientAdjustBonus(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_GUILDALLY:
                    ClientGuildAlly();
                    break;
                case Messages.CM_GUILDBREAKALLY:
                    ClientGuildBreakAlly(processMsg.Msg);
                    break;
                case Messages.CM_TURN:
                    if (ClientChangeDir((short)processMsg.wIdent, processMsg.nParam1, processMsg.nParam2, processMsg.wParam, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetTurnMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxTurnMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.BunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.Config.DropOverSpeed)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Messages.CM_WALK:
                    if (ClientWalkXY(processMsg.wIdent, (short)processMsg.nParam1, (short)processMsg.nParam2, processMsg.LateDelivery, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetWalkMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxWalkMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.WalkOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                                if (TestSpeedMode)
                                {
                                    SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && IsFilterAction)
                                {
                                    SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Messages.CM_HORSERUN:
                    if (ClientHorseRunXY(processMsg.wIdent, (short)processMsg.nParam1, (short)processMsg.nParam2, processMsg.LateDelivery, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetRunMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxRunMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.RunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, ""); // 如果超速则发送攻击失败信息
                                if (TestSpeedMode)
                                {
                                    SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                if (TestSpeedMode)
                                {
                                    SysMsg(Format("操作延迟 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                }
                                SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                result = false;
                            }
                        }
                    }
                    break;
                case Messages.CM_RUN:
                    if (ClientRunXY(processMsg.wIdent, (short)processMsg.nParam1, (short)processMsg.nParam2, processMsg.nParam3, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetRunMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxRunMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.RunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, ""); // 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && IsFilterAction)
                                {
                                    SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, Messages.CM_RUN, "", dwDelayTime);
                                    result = false;
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
                case Messages.CM_TWINHIT:
                case Messages.CM_FIREHIT:
                    if (ClientHitXY(processMsg.wIdent, processMsg.nParam1, processMsg.nParam2, (byte)processMsg.wParam, processMsg.LateDelivery, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetHitMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxHitMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.HitOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && IsFilterAction)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (TestSpeedMode)
                                    {
                                        SysMsg("操作延迟 Ident: " + processMsg.wIdent + " Time: " + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Messages.CM_SITDOWN:
                    if (ClientSitDownHit(processMsg.nParam1, processMsg.nParam2, processMsg.wParam, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetSiteDownMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxSitDonwMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.BunOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime < M2Share.Config.DropOverSpeed)
                                {
                                    SendSocket(M2Share.GetGoodTick);
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Messages.CM_SPELL:
                    if (ClientSpellXY(processMsg.wIdent, processMsg.wParam, (short)processMsg.nParam1, (short)processMsg.nParam2, M2Share.ActorMgr.Get(processMsg.nParam3), processMsg.LateDelivery, ref dwDelayTime))
                    {
                        ActionTick = HUtil32.GetTickCount();
                        SendSocket(M2Share.GetGoodTick);
                    }
                    else
                    {
                        if (dwDelayTime == 0)
                        {
                            SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                        }
                        else
                        {
                            nMsgCount = GetSpellMsgCount();
                            if (nMsgCount >= M2Share.Config.MaxSpellMsgCount)
                            {
                                OverSpeedCount++;
                                if (OverSpeedCount > M2Share.Config.OverSpeedKickCount)
                                {
                                    if (M2Share.Config.KickOverSpeed)
                                    {
                                        SysMsg(Settings.KickClientUserMsg, MsgColor.Red, MsgType.Hint);
                                        BoEmergencyClose = true;
                                    }
                                    if (M2Share.Config.ViewHackMessage)
                                    {
                                        M2Share.Logger.Warn(Format(CommandHelp.SpellOverSpeed, ChrName, dwDelayTime, nMsgCount));
                                    }
                                }
                                SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");// 如果超速则发送攻击失败信息
                            }
                            else
                            {
                                if (dwDelayTime > M2Share.Config.DropOverSpeed && M2Share.Config.SpeedControlMode == 1 && IsFilterAction)
                                {
                                    SendRefMsg(Messages.RM_MOVEFAIL, 0, 0, 0, 0, "");
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("速度异常 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    if (TestSpeedMode)
                                    {
                                        SysMsg(Format("操作延迟 Ident: {0} Time: {1}", processMsg.wIdent, dwDelayTime), MsgColor.Red, MsgType.Hint);
                                    }
                                    SendDelayMsg(this, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "", dwDelayTime);
                                    result = false;
                                }
                            }
                        }
                    }
                    break;
                case Messages.CM_SAY:
                    ProcessUserLineMsg(processMsg.Msg);
                    break;
                case Messages.CM_PASSWORD:
                    ProcessClientPassword(processMsg);
                    break;
                case Messages.CM_QUERYVAL:
                    ProcessQueryValue(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.RM_WALK:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_WALK, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                        charDesc = new CharDesc();
                        charDesc.Feature = baseObject.GetFeature(baseObject);
                        charDesc.Status = baseObject.CharStatus;
                        SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    }
                    break;
                case Messages.RM_HORSERUN:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_HORSERUN, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                        charDesc = new CharDesc();
                        charDesc.Feature = baseObject.GetFeature(baseObject);
                        charDesc.Status = baseObject.CharStatus;
                        SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    }
                    break;
                case Messages.RM_RUN:
                    if (processMsg.BaseObject != ActorId && baseObject != null)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_RUN, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                        charDesc = new CharDesc();
                        charDesc.Feature = baseObject.GetFeature(baseObject);
                        charDesc.Status = baseObject.CharStatus;
                        SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    }
                    break;
                case Messages.RM_HIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_HIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_HEAVYHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_HEAVYHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg, processMsg.Msg);
                    }
                    break;
                case Messages.RM_BIGHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_BIGHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_SPELL:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SPELL, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg, processMsg.nParam3.ToString());
                    }
                    break;
                case Messages.RM_SPELL2:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_POWERHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_MOVEFAIL:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_MOVEFAIL, ActorId, CurrX, CurrY, Direction);
                    charDesc = new CharDesc();
                    charDesc.Feature = baseObject.GetFeatureToLong();
                    charDesc.Status = baseObject.CharStatus;
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    break;
                case Messages.RM_LONGHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_LONGHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_WIDEHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_WIDEHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_FIREHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_FIREHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_CRSHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_CRSHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_41:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_41, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_TWINHIT:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_TWINHIT, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_43:
                    if (processMsg.BaseObject != ActorId)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_43, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_TURN:
                case Messages.RM_PUSH:
                case Messages.RM_RUSH:
                case Messages.RM_RUSHKUNG:
                    if (processMsg.BaseObject != ActorId || processMsg.wIdent == Messages.RM_PUSH || processMsg.wIdent == Messages.RM_RUSH || processMsg.wIdent == Messages.RM_RUSHKUNG)
                    {
                        switch (processMsg.wIdent)
                        {
                            case Messages.RM_PUSH:
                                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_BACKSTEP, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                                break;
                            case Messages.RM_RUSH:
                                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_RUSH, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                                break;
                            case Messages.RM_RUSHKUNG:
                                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_RUSHKUNG, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                                break;
                            default:
                                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_TURN, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                                break;
                        }
                        charDesc = new CharDesc();
                        charDesc.Feature = baseObject.GetFeature(baseObject);
                        charDesc.Status = baseObject.CharStatus;
                        sendMsg = EDCode.EncodeBuffer(charDesc);
                        nObjCount = GetChrColor(baseObject);
                        if (!string.IsNullOrEmpty(processMsg.Msg))
                        {
                            sendMsg = sendMsg + EDCode.EncodeString($"{processMsg.Msg}/{nObjCount}");
                        }
                        SendSocket(ClientMsg, sendMsg);
                        if (processMsg.wIdent == Messages.RM_TURN)
                        {
                            nObjCount = baseObject.GetFeatureToLong();
                            SendDefMessage(Messages.SM_FEATURECHANGED, processMsg.BaseObject, HUtil32.LoWord(nObjCount), HUtil32.HiWord(nObjCount), baseObject.GetFeatureEx(), "");
                        }
                    }
                    break;
                case Messages.RM_STRUCK:
                case Messages.RM_STRUCK_MAG:
                    if (processMsg.wParam > 0)
                    {
                        if (processMsg.BaseObject == ActorId)
                        {
                            if (M2Share.ActorMgr.Get(processMsg.nParam3) != null)
                            {
                                if (M2Share.ActorMgr.Get(processMsg.nParam3).Race == ActorRace.Play)
                                {
                                    SetPkFlag(M2Share.ActorMgr.Get(processMsg.nParam3));
                                }
                                SetLastHiter(M2Share.ActorMgr.Get(processMsg.nParam3));
                            }
                            if (M2Share.CastleMgr.IsCastleMember(this) != null && M2Share.ActorMgr.Get(processMsg.nParam3) != null)
                            {
                                if (M2Share.ActorMgr.Get(processMsg.nParam3).Race == ActorRace.Guard)
                                {
                                    ((GuardUnit)M2Share.ActorMgr.Get(processMsg.nParam3)).BoCrimeforCastle = true;
                                    ((GuardUnit)M2Share.ActorMgr.Get(processMsg.nParam3)).CrimeforCastleTime = HUtil32.GetTickCount();
                                }
                            }
                            HealthTick = 0;
                            SpellTick = 0;
                            PerHealth -= 1;
                            PerSpell -= 1;
                            StruckTick = HUtil32.GetTickCount();
                        }
                        if (processMsg.BaseObject != 0)
                        {
                            if (processMsg.BaseObject == ActorId && M2Share.Config.DisableSelfStruck || baseObject.Race == ActorRace.Play && M2Share.Config.DisableStruck)
                            {
                                baseObject.SendRefMsg(Messages.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
                            }
                            else
                            {
                                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_STRUCK, processMsg.BaseObject, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, processMsg.wParam);
                                messageBodyWl = new MessageBodyWL();
                                messageBodyWl.Param1 = baseObject.GetFeature(this);
                                messageBodyWl.Param2 = baseObject.CharStatus;
                                messageBodyWl.Tag1 = processMsg.nParam3;
                                if (processMsg.wIdent == Messages.RM_STRUCK_MAG)
                                {
                                    messageBodyWl.Tag2 = 1;
                                }
                                else
                                {
                                    messageBodyWl.Tag2 = 0;
                                }
                                SendSocket(ClientMsg, EDCode.EncodeBuffer(messageBodyWl));
                            }
                        }
                    }
                    break;
                case Messages.RM_DEATH:
                    if (processMsg.nParam3 == 1)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_NOWDEATH, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        if (processMsg.BaseObject == ActorId)
                        {
                            if (M2Share.FunctionNPC != null)
                            {
                                M2Share.FunctionNPC.GotoLable(this, "@OnDeath", false);
                            }
                        }
                    }
                    else
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_DEATH, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                    }
                    charDesc = new CharDesc();
                    charDesc.Feature = baseObject.GetFeature(this);
                    charDesc.Status = baseObject.CharStatus;
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    break;
                case Messages.RM_DISAPPEAR:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_DISAPPEAR, processMsg.BaseObject, 0, 0, 0);
                    SendSocket(ClientMsg);
                    break;
                case Messages.RM_SKELETON:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SKELETON, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                    charDesc = new CharDesc();
                    charDesc.Feature = baseObject.GetFeature(this);
                    charDesc.Status = baseObject.CharStatus;
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    break;
                case Messages.RM_USERNAME:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_USERNAME, processMsg.BaseObject, GetChrColor(baseObject), 0, 0);
                    SendSocket(ClientMsg, EDCode.EncodeString(processMsg.Msg));
                    break;
                case Messages.RM_WINEXP:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_WINEXP, Abil.Exp, HUtil32.LoWord(processMsg.nParam1), HUtil32.HiWord(processMsg.nParam1), 0);
                    SendSocket(ClientMsg);
                    break;
                case Messages.RM_LEVELUP:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_LEVELUP, Abil.Exp, Abil.Level, 0, 0);
                    SendSocket(ClientMsg);
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_ABILITY, Gold, HUtil32.MakeWord((byte)Job, 99), HUtil32.LoWord(GameGold), HUtil32.HiWord(GameGold));
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(WAbil));
                    SendDefMessage(Messages.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(AntiMagic, 0), 0), HUtil32.MakeWord(HitPoint, SpeedPoint), HUtil32.MakeWord(AntiPoison, PoisonRecover), HUtil32.MakeWord(HealthRecover, SpellRecover), "");
                    break;
                case Messages.RM_CHANGENAMECOLOR:
                    SendDefMessage(Messages.SM_CHANGENAMECOLOR, processMsg.BaseObject, GetChrColor(baseObject), 0, 0, "");
                    break;
                case Messages.RM_LOGON:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_NEWMAP, ActorId, CurrX, CurrY, DayBright());
                    SendSocket(ClientMsg, EDCode.EncodeString(MapFileName));
                    SendMsg(this, Messages.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                    SendLogon();
                    SendServerConfig();
                    ClientQueryUserName(ActorId, CurrX, CurrY);
                    RefUserState();
                    SendMapDescription();
                    SendGoldInfo(true);
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_VERSION_FAIL, M2Share.Config.nClientFile1_CRC, HUtil32.LoWord(M2Share.Config.nClientFile2_CRC), HUtil32.HiWord(M2Share.Config.nClientFile2_CRC), 0);
                    SendSocket(ClientMsg, "<<<<<<");
                    break;
                case Messages.RM_HEAR:
                case Messages.RM_WHISPER:
                case Messages.RM_CRY:
                case Messages.RM_SYSMESSAGE:
                case Messages.RM_GROUPMESSAGE:
                case Messages.RM_SYSMESSAGE2:
                case Messages.RM_GUILDMESSAGE:
                case Messages.RM_SYSMESSAGE3:
                case Messages.RM_MOVEMESSAGE:
                case Messages.RM_MERCHANTSAY:
                    switch (processMsg.wIdent)
                    {
                        case Messages.RM_HEAR:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_HEAR, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_WHISPER:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_WHISPER, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_CRY:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_HEAR, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_SYSMESSAGE:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SYSMESSAGE, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_GROUPMESSAGE:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SYSMESSAGE, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_GUILDMESSAGE:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_GUILDMESSAGE, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_MERCHANTSAY:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_MERCHANTSAY, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), 0, 1);
                            break;
                        case Messages.RM_MOVEMESSAGE:
                            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_MOVEMESSAGE, processMsg.BaseObject, HUtil32.MakeWord((ushort)processMsg.nParam1, (ushort)processMsg.nParam2), processMsg.nParam3, processMsg.wParam);
                            break;
                    }
                    SendSocket(ClientMsg, EDCode.EncodeString(processMsg.Msg));
                    break;
                case Messages.RM_ABILITY:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_ABILITY, Gold, HUtil32.MakeWord((byte)Job, 99), HUtil32.LoWord(GameGold), HUtil32.HiWord(GameGold));
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(WAbil));
                    break;
                case Messages.RM_HEALTHSPELLCHANGED:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_HEALTHSPELLCHANGED, processMsg.BaseObject, baseObject.WAbil.HP, baseObject.WAbil.MP, baseObject.WAbil.MaxHP);
                    SendSocket(ClientMsg);
                    break;
                case Messages.RM_DAYCHANGING:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_DAYCHANGING, 0, Bright, DayBright(), 0);
                    SendSocket(ClientMsg);
                    break;
                case Messages.RM_ITEMSHOW:
                    SendDefMessage(Messages.SM_ITEMSHOW, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam, processMsg.Msg);
                    break;
                case Messages.RM_ITEMHIDE:
                    SendDefMessage(Messages.SM_ITEMHIDE, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, 0, "");
                    break;
                case Messages.RM_DOOROPEN:
                    SendDefMessage(Messages.SM_OPENDOOR_OK, 0, processMsg.nParam1, processMsg.nParam2, 0, "");
                    break;
                case Messages.RM_DOORCLOSE:
                    SendDefMessage(Messages.SM_CLOSEDOOR, 0, processMsg.nParam1, processMsg.nParam2, 0, "");
                    break;
                case Messages.RM_SENDUSEITEMS:
                    SendUseItems();
                    break;
                case Messages.RM_WEIGHTCHANGED:
                    SendDefMessage(Messages.SM_WEIGHTCHANGED, WAbil.Weight, WAbil.WearWeight, WAbil.HandWeight, (((WAbil.Weight + WAbil.WearWeight + WAbil.HandWeight) ^ 0x3A5F) ^ 0x1F35) ^ 0xaa21, "");
                    break;
                case Messages.RM_FEATURECHANGED:
                    SendDefMessage(Messages.SM_FEATURECHANGED, processMsg.BaseObject, HUtil32.LoWord(processMsg.nParam1), HUtil32.HiWord(processMsg.nParam1), processMsg.wParam, "");
                    break;
                case Messages.RM_CLEAROBJECTS:
                    SendDefMessage(Messages.SM_CLEAROBJECTS, 0, 0, 0, 0, "");
                    break;
                case Messages.RM_CHANGEMAP:
                    SendDefMessage(Messages.SM_CHANGEMAP, ActorId, CurrX, CurrY, DayBright(), processMsg.Msg);
                    RefUserState();
                    SendMapDescription();
                    SendServerConfig();
                    break;
                case Messages.RM_BUTCH:
                    if (processMsg.BaseObject != 0)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_BUTCH, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg);
                    }
                    break;
                case Messages.RM_MAGICFIRE:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_MAGICFIRE, processMsg.BaseObject, HUtil32.LoWord(processMsg.nParam2), HUtil32.HiWord(processMsg.nParam2), processMsg.nParam1);
                    var by = BitConverter.GetBytes(processMsg.nParam3);
                    var sSendStr = EDCode.EncodeBuffer(by, by.Length);
                    SendSocket(ClientMsg, sSendStr);
                    break;
                case Messages.RM_MAGICFIREFAIL:
                    SendDefMessage(Messages.SM_MAGICFIRE_FAIL, processMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Messages.RM_SENDMYMAGIC:
                    SendUseMagic();
                    break;
                case Messages.RM_MAGIC_LVEXP:
                    SendDefMessage(Messages.SM_MAGIC_LVEXP, processMsg.nParam1, processMsg.nParam2, HUtil32.LoWord(processMsg.nParam3), HUtil32.HiWord(processMsg.nParam3), "");
                    break;
                case Messages.RM_DURACHANGE:
                    SendDefMessage(Messages.SM_DURACHANGE, processMsg.nParam1, processMsg.wParam, HUtil32.LoWord(processMsg.nParam2), HUtil32.HiWord(processMsg.nParam2), "");
                    break;
                case Messages.RM_MERCHANTDLGCLOSE:
                    SendDefMessage(Messages.SM_MERCHANTDLGCLOSE, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_SENDGOODSLIST:
                    SendDefMessage(Messages.SM_SENDGOODSLIST, processMsg.nParam1, processMsg.nParam2, 0, 0, processMsg.Msg);
                    break;
                case Messages.RM_SENDUSERSELL:
                    SendDefMessage(Messages.SM_SENDUSERSELL, processMsg.nParam1, processMsg.nParam2, 0, 0, processMsg.Msg);
                    break;
                case Messages.RM_SENDBUYPRICE:
                    SendDefMessage(Messages.SM_SENDBUYPRICE, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_USERSELLITEM_OK:
                    SendDefMessage(Messages.SM_USERSELLITEM_OK, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_USERSELLITEM_FAIL:
                    SendDefMessage(Messages.SM_USERSELLITEM_FAIL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_BUYITEM_SUCCESS:
                    SendDefMessage(Messages.SM_BUYITEM_SUCCESS, processMsg.nParam1, HUtil32.LoWord(processMsg.nParam2), HUtil32.HiWord(processMsg.nParam2), 0, "");
                    break;
                case Messages.RM_BUYITEM_FAIL:
                    SendDefMessage(Messages.SM_BUYITEM_FAIL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_SENDDETAILGOODSLIST:
                    SendDefMessage(Messages.SM_SENDDETAILGOODSLIST, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, 0, processMsg.Msg);
                    break;
                case Messages.RM_GOLDCHANGED:
                    SendDefMessage(Messages.SM_GOLDCHANGED, Gold, HUtil32.LoWord(GameGold), HUtil32.HiWord(GameGold), 0, "");
                    break;
                case Messages.RM_GAMEGOLDCHANGED:
                    SendGoldInfo(false);
                    break;
                case Messages.RM_CHANGELIGHT:
                    SendDefMessage(Messages.SM_CHANGELIGHT, processMsg.BaseObject, baseObject.Light, (short)M2Share.Config.nClientKey, 0, "");
                    break;
                case Messages.RM_LAMPCHANGEDURA:
                    SendDefMessage(Messages.SM_LAMPCHANGEDURA, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_CHARSTATUSCHANGED:
                    SendDefMessage(Messages.SM_CHARSTATUSCHANGED, processMsg.BaseObject, HUtil32.LoWord(processMsg.nParam1), HUtil32.HiWord(processMsg.nParam1), processMsg.wParam, "");
                    break;
                case Messages.RM_GROUPCANCEL:
                    SendDefMessage(Messages.SM_GROUPCANCEL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_SENDUSERREPAIR:
                case Messages.RM_SENDUSERSREPAIR:
                    SendDefMessage(Messages.SM_SENDUSERREPAIR, processMsg.nParam1, processMsg.nParam2, 0, 0, "");
                    break;
                case Messages.RM_USERREPAIRITEM_OK:
                    SendDefMessage(Messages.SM_USERREPAIRITEM_OK, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, 0, "");
                    break;
                case Messages.RM_SENDREPAIRCOST:
                    SendDefMessage(Messages.SM_SENDREPAIRCOST, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_USERREPAIRITEM_FAIL:
                    SendDefMessage(Messages.SM_USERREPAIRITEM_FAIL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_USERSTORAGEITEM:
                    SendDefMessage(Messages.SM_SENDUSERSTORAGEITEM, processMsg.nParam1, processMsg.nParam2, 0, 0, "");
                    break;
                case Messages.RM_USERGETBACKITEM:
                    SendSaveItemList(processMsg.nParam1);
                    break;
                case Messages.RM_SENDDELITEMLIST:
                    var delItemList = (IList<DeleteItem>)M2Share.ActorMgr.GetOhter(processMsg.nParam1);
                    SendDelItemList(delItemList);
                    M2Share.ActorMgr.RevomeOhter(processMsg.nParam1);
                    break;
                case Messages.RM_USERMAKEDRUGITEMLIST:
                    SendDefMessage(Messages.SM_SENDUSERMAKEDRUGITEMLIST, processMsg.nParam1, processMsg.nParam2, 0, 0, processMsg.Msg);
                    break;
                case Messages.RM_MAKEDRUG_SUCCESS:
                    SendDefMessage(Messages.SM_MAKEDRUG_SUCCESS, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_MAKEDRUG_FAIL:
                    SendDefMessage(Messages.SM_MAKEDRUG_FAIL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_ALIVE:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_ALIVE, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                    charDesc = new CharDesc();
                    charDesc.Feature = baseObject.GetFeature(this);
                    charDesc.Status = baseObject.CharStatus;
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    break;
                case Messages.RM_DIGUP:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_DIGUP, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                    messageBodyWl = new MessageBodyWL();
                    messageBodyWl.Param1 = baseObject.GetFeature(this);
                    messageBodyWl.Param2 = baseObject.CharStatus;
                    messageBodyWl.Tag1 = processMsg.nParam3;
                    messageBodyWl.Tag1 = 0;
                    sendMsg = EDCode.EncodeBuffer(messageBodyWl);
                    SendSocket(ClientMsg, sendMsg);
                    break;
                case Messages.RM_DIGDOWN:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_DIGDOWN, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, 0);
                    SendSocket(ClientMsg);
                    break;
                case Messages.RM_FLYAXE:
                    if (M2Share.ActorMgr.Get(processMsg.nParam3) != null)
                    {
                        var messageBodyW = new MessageBodyW();
                        messageBodyW.Param1 = (ushort)M2Share.ActorMgr.Get(processMsg.nParam3).CurrX;
                        messageBodyW.Param2 = (ushort)M2Share.ActorMgr.Get(processMsg.nParam3).CurrY;
                        messageBodyW.Tag1 = HUtil32.LoWord(processMsg.nParam3);
                        messageBodyW.Tag2 = HUtil32.HiWord(processMsg.nParam3);
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_FLYAXE, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.wParam);
                        SendSocket(ClientMsg, EDCode.EncodeBuffer(messageBodyW));
                    }
                    break;
                case Messages.RM_LIGHTING:
                    if (M2Share.ActorMgr.Get(processMsg.nParam3) != null)
                    {
                        messageBodyWl = new MessageBodyWL();
                        messageBodyWl.Param1 = M2Share.ActorMgr.Get(processMsg.nParam3).CurrX;
                        messageBodyWl.Param2 = M2Share.ActorMgr.Get(processMsg.nParam3).CurrY;
                        messageBodyWl.Tag1 = processMsg.nParam3;
                        messageBodyWl.Tag2 = processMsg.wParam;
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_LIGHTING, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, baseObject.Direction);
                        SendSocket(ClientMsg, EDCode.EncodeBuffer(messageBodyWl));
                    }
                    break;
                case Messages.RM_10205:
                    SendDefMessage(Messages.SM_716, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, "");
                    break;
                case Messages.RM_CHANGEGUILDNAME:
                    SendChangeGuildName();
                    break;
                case Messages.RM_SUBABILITY:
                    SendDefMessage(Messages.SM_SUBABILITY, HUtil32.MakeLong(HUtil32.MakeWord(AntiMagic, 0), 0), HUtil32.MakeWord(HitPoint, SpeedPoint), HUtil32.MakeWord(AntiPoison, PoisonRecover), HUtil32.MakeWord(HealthRecover, SpellRecover), "");
                    break;
                case Messages.RM_BUILDGUILD_OK:
                    SendDefMessage(Messages.SM_BUILDGUILD_OK, 0, 0, 0, 0, "");
                    break;
                case Messages.RM_BUILDGUILD_FAIL:
                    SendDefMessage(Messages.SM_BUILDGUILD_FAIL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_DONATE_OK:
                    SendDefMessage(Messages.SM_DONATE_OK, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_DONATE_FAIL:
                    SendDefMessage(Messages.SM_DONATE_FAIL, processMsg.nParam1, 0, 0, 0, "");
                    break;
                case Messages.RM_MYSTATUS:
                    SendDefMessage(Messages.SM_MYSTATUS, 0, (short)GetMyStatus(), 0, 0, "");
                    break;
                case Messages.RM_MENU_OK:
                    SendDefMessage(Messages.SM_MENU_OK, processMsg.nParam1, 0, 0, 0, processMsg.Msg);
                    break;
                case Messages.RM_SPACEMOVE_FIRE:
                case Messages.RM_SPACEMOVE_FIRE2:
                    if (processMsg.wIdent == Messages.RM_SPACEMOVE_FIRE)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SPACEMOVE_HIDE, processMsg.BaseObject, 0, 0, 0);
                    }
                    else
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SPACEMOVE_HIDE2, processMsg.BaseObject, 0, 0, 0);
                    }
                    SendSocket(ClientMsg);
                    break;
                case Messages.RM_SPACEMOVE_SHOW:
                case Messages.RM_SPACEMOVE_SHOW2:
                    if (processMsg.wIdent == Messages.RM_SPACEMOVE_SHOW)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SPACEMOVE_SHOW, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                    }
                    else
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SPACEMOVE_SHOW2, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, HUtil32.MakeWord((ushort)processMsg.wParam, baseObject.Light));
                    }
                    charDesc = new CharDesc();
                    charDesc.Feature = baseObject.GetFeature(this);
                    charDesc.Status = baseObject.CharStatus;
                    sendMsg = EDCode.EncodeBuffer(charDesc);
                    nObjCount = GetChrColor(baseObject);
                    if (processMsg.Msg != "")
                    {
                        sendMsg = sendMsg + EDCode.EncodeString(processMsg.Msg + '/' + nObjCount);
                    }
                    SendSocket(ClientMsg, sendMsg);
                    break;
                case Messages.RM_RECONNECTION:
                    BoReconnection = true;
                    SendDefMessage(Messages.SM_RECONNECT, 0, 0, 0, 0, processMsg.Msg);
                    break;
                case Messages.RM_HIDEEVENT:
                    SendDefMessage(Messages.SM_HIDEEVENT, processMsg.nParam1, processMsg.wParam, processMsg.nParam2, processMsg.nParam3, "");
                    break;
                case Messages.RM_SHOWEVENT:
                    var shortMessage = new ShortMessage();
                    shortMessage.Ident = HUtil32.HiWord(processMsg.nParam2);
                    shortMessage.wMsg = 0;
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SHOWEVENT, processMsg.nParam1, processMsg.wParam, processMsg.nParam2, processMsg.nParam3);
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(shortMessage));
                    break;
                case Messages.RM_ADJUST_BONUS:
                    SendAdjustBonus();
                    break;
                case Messages.RM_10401:
                    ChangeServerMakeSlave((SlaveInfo)M2Share.ActorMgr.GetOhter(processMsg.nParam1));
                    M2Share.ActorMgr.RevomeOhter(processMsg.nParam1);
                    break;
                case Messages.RM_OPENHEALTH:
                    SendDefMessage(Messages.SM_OPENHEALTH, processMsg.BaseObject, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, 0, "");
                    break;
                case Messages.RM_CLOSEHEALTH:
                    SendDefMessage(Messages.SM_CLOSEHEALTH, processMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Messages.RM_BREAKWEAPON:
                    SendDefMessage(Messages.SM_BREAKWEAPON, processMsg.BaseObject, 0, 0, 0, "");
                    break;
                case Messages.RM_10414:
                    SendDefMessage(Messages.SM_INSTANCEHEALGUAGE, processMsg.BaseObject, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, 0, "");
                    break;
                case Messages.RM_CHANGEFACE:
                    if (processMsg.nParam1 != 0 && processMsg.nParam2 != 0)
                    {
                        ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_CHANGEFACE, processMsg.nParam1, HUtil32.LoWord(processMsg.nParam2), HUtil32.HiWord(processMsg.nParam2), 0);
                        charDesc = new CharDesc();
                        charDesc.Feature = M2Share.ActorMgr.Get(processMsg.nParam2).GetFeature(this);
                        charDesc.Status = M2Share.ActorMgr.Get(processMsg.nParam2).CharStatus;
                        SendSocket(ClientMsg, EDCode.EncodeBuffer(charDesc));
                    }
                    break;
                case Messages.RM_PASSWORD:
                    SendDefMessage(Messages.SM_PASSWORD, 0, 0, 0, 0, "");
                    break;
                case Messages.RM_PLAYDICE:
                    messageBodyWl = new MessageBodyWL();
                    messageBodyWl.Param1 = processMsg.nParam1;
                    messageBodyWl.Param2 = processMsg.nParam2;
                    messageBodyWl.Tag1 = processMsg.nParam3;
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_PLAYDICE, processMsg.BaseObject, processMsg.wParam, 0, 0);
                    SendSocket(ClientMsg, EDCode.EncodeBuffer(messageBodyWl) + EDCode.EncodeString(processMsg.Msg));
                    break;
                case Messages.RM_PASSWORDSTATUS:
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_PASSWORDSTATUS, processMsg.BaseObject, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                // ---------------------------元宝寄售系统---------------------------------------
                case Messages.RM_SENDDEALOFFFORM:// 打开出售物品窗口
                    SendDefMessage(Messages.SM_SENDDEALOFFFORM, processMsg.nParam1, processMsg.nParam2, 0, 0, processMsg.Msg);
                    break;
                case Messages.RM_QUERYYBSELL:// 查询正在出售的物品
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_QUERYYBSELL, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_QUERYYBDEAL:// 查询可以的购买物品
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_QUERYYBDEAL, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.CM_SELLOFFADDITEM:// 客户端往出售物品窗口里加物品 
                    ClientAddSellOffItem(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_SELLOFFDELITEM:// 客户端删除出售物品窗里的物品 
                    ClientDelSellOffItem(processMsg.nParam1, processMsg.Msg);
                    break;
                case Messages.CM_SELLOFFCANCEL:// 客户端取消元宝寄售 
                    ClientCancelSellOff();
                    break;
                case Messages.CM_SELLOFFEND:// 客户端元宝寄售结束 
                    ClientSellOffEnd(processMsg.Msg, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3);
                    break;
                case Messages.CM_CANCELSELLOFFITEMING:// 取消正在寄售的物品(出售人)
                    ClientCancelSellOffIng();
                    break;
                case Messages.CM_SELLOFFBUYCANCEL:// 取消寄售 物品购买(购买人)
                    ClientBuyCancelSellOff(processMsg.Msg);// 出售人
                    break;
                case Messages.CM_SELLOFFBUY:// 确定购买寄售物品
                    ClientBuySellOffItme(processMsg.Msg);// 出售人
                    break;
                case Messages.RM_SELLOFFCANCEL:// 元宝寄售取消出售
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SellOffCANCEL, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SELLOFFADDITEM_OK:// 客户端往出售物品窗口里加物品 成功
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SELLOFFADDITEM_OK, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SellOffADDITEM_FAIL:// 客户端往出售物品窗口里加物品 失败
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SellOffADDITEM_FAIL, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SELLOFFDELITEM_OK:// 客户端删除出售物品窗里的物品 成功
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SELLOFFDELITEM_OK, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SELLOFFDELITEM_FAIL:// 客户端删除出售物品窗里的物品 失败
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SELLOFFDELITEM_FAIL, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SELLOFFEND_OK:// 客户端元宝寄售结束 成功
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SELLOFFEND_OK, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SELLOFFEND_FAIL:// 客户端元宝寄售结束 失败
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SELLOFFEND_FAIL, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                case Messages.RM_SELLOFFBUY_OK:// 购买成功
                    ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SELLOFFBUY_OK, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.wParam);
                    SendSocket(ClientMsg, processMsg.Msg);
                    break;
                default:
                    result = base.Operate(processMsg);
                    break;
            }
            return result;
        }

        public override void Disappear()
        {
            if (BoReadyRun)
            {
                DisappearA();
            }
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 0;
            }
            if (GroupOwner != 0)
            {
                var groupOwnerPlay = (PlayObject)M2Share.ActorMgr.Get(GroupOwner);
                groupOwnerPlay.DelMember(this);
            }
            if (MyGuild != null)
            {
                MyGuild.DelHumanObj(this);
            }
            LogonTimcCost();
            base.Disappear();
        }

        internal override void DropUseItems(int baseObject)
        {
            const string sExceptionMsg = "[Exception] TPlayObject::DropUseItems";
            try
            {
                if (AngryRing || NoDropUseItem)
                {
                    return;
                }
                IList<DeleteItem> dropItemList = new List<DeleteItem>();
                StdItem stdItem;
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] == null)
                    {
                        continue;
                    }
                    stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (stdItem != null)
                    {
                        if ((stdItem.ItemDesc & 8) != 0)
                        {
                            dropItemList.Add(new DeleteItem() { MakeIndex = UseItems[i].MakeIndex });
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(16, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + UseItems[i].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == ActorRace.Play) + "\t" + '0');
                            }
                            UseItems[i].Index = 0;
                        }
                    }
                }
                var nRate = PvpLevel() > 2 ? M2Share.Config.DieRedDropUseItemRate : M2Share.Config.DieDropUseItemRate;
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (M2Share.RandomNumber.Random(nRate) != 0)
                    {
                        continue;
                    }
                    if (UseItems[i] != null && M2Share.InDisableTakeOffList(UseItems[i].Index))
                    {
                        continue;
                    }
                    // 检查是否在禁止取下列表,如果在列表中则不掉此物品
                    if (DropItemDown(UseItems[i], 2, true, baseObject, ActorId))
                    {
                        stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        if (stdItem != null)
                        {
                            if ((stdItem.ItemDesc & 10) == 0)
                            {
                                if (Race == ActorRace.Play)
                                {
                                    dropItemList.Add(new DeleteItem()
                                    {
                                        ItemName = M2Share.WorldEngine.GetStdItemName(UseItems[i].Index),
                                        MakeIndex = UseItems[i].MakeIndex
                                    });
                                }
                                UseItems[i].Index = 0;
                            }
                        }
                    }
                }
                if (dropItemList != null)
                {
                    var objectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(objectId, dropItemList);
                    SendMsg(this, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
                }
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 蜡烛勋章减少持久
        /// </summary>
        private void UseLamp()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::UseLamp";
            try
            {
                if (UseItems[Grobal2.U_RIGHTHAND] != null && UseItems[Grobal2.U_RIGHTHAND].Index > 0)
                {
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_RIGHTHAND].Index);
                    if ((stdItem == null) || (stdItem.SpecialPwr != 0))
                    {
                        return;
                    }
                    var nOldDura = HUtil32.Round((ushort)(UseItems[Grobal2.U_RIGHTHAND].Dura / 1000));
                    ushort nDura;
                    if (M2Share.Config.DecLampDura)
                    {
                        nDura = (ushort)(UseItems[Grobal2.U_RIGHTHAND].Dura - 1);
                    }
                    else
                    {
                        nDura = UseItems[Grobal2.U_RIGHTHAND].Dura;
                    }
                    if (nDura <= 0)
                    {
                        UseItems[Grobal2.U_RIGHTHAND].Dura = 0;
                        if (Race == ActorRace.Play)
                        {
                            SendDelItems(UseItems[Grobal2.U_RIGHTHAND]);
                        }
                        UseItems[Grobal2.U_RIGHTHAND].Index = 0;
                        Light = 0;
                        SendRefMsg(Messages.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                        SendMsg(this, Messages.RM_LAMPCHANGEDURA, 0, 0, 0, 0, "");
                        RecalcAbilitys();
                    }
                    else
                    {
                        UseItems[Grobal2.U_RIGHTHAND].Dura = nDura;
                    }
                    if (nOldDura != HUtil32.Round(nDura / 1000))
                    {
                        SendMsg(this, Messages.RM_LAMPCHANGEDURA, 0, UseItems[Grobal2.U_RIGHTHAND].Dura, 0, 0, "");
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
        }
    }
}
