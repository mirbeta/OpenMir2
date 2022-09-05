﻿using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Npc;
using GameSvr.Player;
using GameSvr.RobotPlay;
using System.Collections;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        public virtual void Run()
        {
            TProcessMessage ProcessMsg = null;
            const string sExceptionMsg0 = "[Exception] TBaseObject::Run 0";
            const string sExceptionMsg1 = "[Exception] TBaseObject::Run 1";
            const string sExceptionMsg2 = "[Exception] TBaseObject::Run 2";
            const string sExceptionMsg3 = "[Exception] TBaseObject::Run 3";
            const string sExceptionMsg4 = "[Exception] TBaseObject::Run 4 Code:{0}";
            const string sExceptionMsg5 = "[Exception] TBaseObject::Run 5";
            const string sExceptionMsg6 = "[Exception] TBaseObject::Run 6";
            const string sExceptionMsg7 = "[Exception] TBaseObject::Run 7";
            var dwRunTick = HUtil32.GetTickCount();
            try
            {
                while (GetMessage(ref ProcessMsg))
                {
                    Operate(ProcessMsg);
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg0);
                M2Share.Log.Error(e.StackTrace);
            }
            try
            {
                if (SuperMan)
                {
                    MWAbil.HP = MWAbil.MaxHP;
                    MWAbil.MP = MWAbil.MaxMP;
                }
                int dwC = (HUtil32.GetTickCount() - MDwHpmpTick) / 20;
                MDwHpmpTick = HUtil32.GetTickCount();
                MNHealthTick += dwC;
                MNSpellTick += dwC;
                if (!Death)
                {
                    ushort n18;
                    if ((MWAbil.HP < MWAbil.MaxHP) && (MNHealthTick >= M2Share.Config.nHealthFillTime))
                    {
                        n18 = (ushort)((MWAbil.MaxHP / 75) + 1);
                        if ((MWAbil.HP + n18) < MWAbil.MaxHP)
                        {
                            MWAbil.HP += n18;
                        }
                        else
                        {
                            MWAbil.HP = MWAbil.MaxHP;
                        }
                        HealthSpellChanged();
                    }
                    if ((MWAbil.MP < MWAbil.MaxMP) && (MNSpellTick >= M2Share.Config.nSpellFillTime))
                    {
                        n18 = (ushort)((MWAbil.MaxMP / 18) + 1);
                        if ((MWAbil.MP + n18) < MWAbil.MaxMP)
                        {
                            MWAbil.MP += n18;
                        }
                        else
                        {
                            MWAbil.MP = MWAbil.MaxMP;
                        }
                        HealthSpellChanged();
                    }
                    if (MWAbil.HP == 0)
                    {
                        if (((LastHiter == null) || !LastHiter.UnRevival) && Revival && ((HUtil32.GetTickCount() - RevivalTick) > M2Share.Config.dwRevivalTime))// 60 * 1000
                        {
                            RevivalTick = HUtil32.GetTickCount();
                            ItemDamageRevivalRing();
                            MWAbil.HP = MWAbil.MaxHP;
                            HealthSpellChanged();
                            SysMsg(M2Share.g_sRevivalRecoverMsg, MsgColor.Green, MsgType.Hint);
                        }
                        if (MWAbil.HP == 0)
                        {
                            Die();
                        }
                    }
                    if (MNHealthTick >= M2Share.Config.nHealthFillTime)
                    {
                        MNHealthTick = 0;
                    }
                    if (MNSpellTick >= M2Share.Config.nSpellFillTime)
                    {
                        MNSpellTick = 0;
                    }
                }
                else
                {
                    if (CanReAlive && MonGen != null)
                    {
                        var dwMakeGhostTime = HUtil32._MAX(10 * 1000, M2Share.UserEngine.ProcessMonsters_GetZenTime(MonGen.dwZenTime) - 20 * 1000);
                        if (dwMakeGhostTime > M2Share.Config.dwMakeGhostTime)
                        {
                            dwMakeGhostTime = M2Share.Config.dwMakeGhostTime;
                        }
                        if ((HUtil32.GetTickCount() - DeathTick > dwMakeGhostTime))
                        {
                            MakeGhost();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - DeathTick) > M2Share.Config.dwMakeGhostTime)// 3 * 60 * 1000
                        {
                            MakeGhost();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg1);
                M2Share.Log.Error(e.Message);
            }
            try
            {
                if (!Death && ((IncSpell > 0) || (IncHealth > 0) || (MNIncHealing > 0)))
                {
                    int dwInChsTime = 600 - HUtil32._MIN(400, Abil.Level * 10);
                    if (((HUtil32.GetTickCount() - MDwIncHealthSpellTick) >= dwInChsTime) && !Death)
                    {
                        int dwC = HUtil32._MIN(200, HUtil32.GetTickCount() - MDwIncHealthSpellTick - dwInChsTime);
                        MDwIncHealthSpellTick = HUtil32.GetTickCount() + dwC;
                        if ((IncSpell > 0) || (IncHealth > 0) || (MNPerHealing > 0))
                        {
                            if (MNPerHealth <= 0)
                            {
                                MNPerHealth = 1;
                            }
                            if (MNPerSpell <= 0)
                            {
                                MNPerSpell = 1;
                            }
                            if (MNPerHealing <= 0)
                            {
                                MNPerHealing = 1;
                            }
                            int nHP;
                            if (IncHealth < MNPerHealth)
                            {
                                nHP = IncHealth;
                                IncHealth = 0;
                            }
                            else
                            {
                                nHP = MNPerHealth;
                                IncHealth -= MNPerHealth;
                            }
                            int nMP;
                            if (IncSpell < MNPerSpell)
                            {
                                nMP = IncSpell;
                                IncSpell = 0;
                            }
                            else
                            {
                                nMP = MNPerSpell;
                                IncSpell -= MNPerSpell;
                            }
                            if (MNIncHealing < MNPerHealing)
                            {
                                nHP += MNIncHealing;
                                MNIncHealing = 0;
                            }
                            else
                            {
                                nHP += MNPerHealing;
                                MNIncHealing -= MNPerHealing;
                            }
                            MNPerHealth = Abil.Level / 10 + 5;
                            MNPerSpell = Abil.Level / 10 + 5;
                            MNPerHealing = 5;
                            IncHealthSpell(nHP, nMP);
                            if (MWAbil.HP == MWAbil.MaxHP)
                            {
                                IncHealth = 0;
                                MNIncHealing = 0;
                            }
                            if (MWAbil.MP == MWAbil.MaxMP)
                            {
                                IncSpell = 0;
                            }
                        }
                    }
                }
                else
                {
                    MDwIncHealthSpellTick = HUtil32.GetTickCount();
                }
                if ((MNHealthTick < -M2Share.Config.nHealthFillTime) && (MWAbil.HP > 1))
                {
                    MWAbil.HP -= 1;
                    MNHealthTick += M2Share.Config.nHealthFillTime;
                    HealthSpellChanged();
                }
                // 检查HP/MP值是否大于最大值，大于则降低到正常大小
                bool boNeedRecalc = false;
                if (MWAbil.HP > MWAbil.MaxHP)
                {
                    boNeedRecalc = true;
                    MWAbil.HP = (ushort)(MWAbil.MaxHP - 1);
                }
                if (MWAbil.MP > MWAbil.MaxMP)
                {
                    boNeedRecalc = true;
                    MWAbil.MP = (ushort)(MWAbil.MaxMP - 1);
                }
                if (boNeedRecalc)
                {
                    HealthSpellChanged();
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg2);
            }
            // 血气石处理开始
            try
            {
                if (UseItems.Length >= Grobal2.U_CHARM && UseItems[Grobal2.U_CHARM] != null)
                {
                    if (!Death && new ArrayList(new byte[] { Grobal2.RC_PLAYOBJECT, Grobal2.RC_PLAYCLONE }).Contains(Race))
                    {
                        int nCount;
                        int dCount;
                        int bCount;
                        StdItem StdItem;
                        // 加HP
                        if ((IncHealth == 0) && (UseItems[Grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - MNIncHpStoneTime) > M2Share.Config.HPStoneIntervalTime) && ((MWAbil.HP / MWAbil.MaxHP * 100) < M2Share.Config.HPStoneStartRate))
                        {
                            MNIncHpStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_CHARM].wIndex);
                            if ((StdItem.StdMode == 7) && new ArrayList(new byte[] { 1, 3 }).Contains(StdItem.Shape))
                            {
                                nCount = UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.Config.HPStoneAddRate);
                                dCount = MWAbil.MaxHP - MWAbil.HP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
                                    IncHealth += dCount;
                                    UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                                }
                                else
                                {
                                    nCount = 0;
                                    IncHealth += nCount;
                                    UseItems[Grobal2.U_CHARM].Dura = 0;
                                }
                                if (UseItems[Grobal2.U_CHARM].Dura >= 1000)
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_CHARM, UseItems[Grobal2.U_CHARM].Dura, UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    UseItems[Grobal2.U_CHARM].Dura = 0;
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        (this as PlayObject).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                    }
                                    UseItems[Grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[Grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - MNIncMpStoneTime) > M2Share.Config.MPStoneIntervalTime) && ((MWAbil.MP / MWAbil.MaxMP * 100) < M2Share.Config.MPStoneStartRate))
                        {
                            MNIncMpStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_CHARM].wIndex);
                            if ((StdItem.StdMode == 7) && new ArrayList(new byte[] { 2, 3 }).Contains(StdItem.Shape))
                            {
                                nCount = UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.Config.MPStoneAddRate);
                                dCount = MWAbil.MaxMP - MWAbil.MP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
                                    // Dec(nCount,dCount);
                                    IncSpell += dCount;
                                    UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                                }
                                else
                                {
                                    nCount = 0;
                                    IncSpell += nCount;
                                    UseItems[Grobal2.U_CHARM].Dura = 0;
                                }
                                if (UseItems[Grobal2.U_CHARM].Dura >= 1000)
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_CHARM, UseItems[Grobal2.U_CHARM].Dura, UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    UseItems[Grobal2.U_CHARM].Dura = 0;
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        (this as PlayObject).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                    }
                                    UseItems[Grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(sExceptionMsg7);
            }
            // 血气石处理结束
            // TBaseObject.Run 3 清理目标对象
            try
            {
                if (TargetCret != null)
                {
                    //修复弓箭卫士在人物进入房间后再出来，还会攻击人物(人物的攻击目标没清除)
                    if (((HUtil32.GetTickCount() - TargetFocusTick) > 30000) || TargetCret.Death || TargetCret.Ghost || (TargetCret.Envir != Envir) || (Math.Abs(TargetCret.CurrX - CurrX) > 15) || (Math.Abs(TargetCret.CurrY - CurrY) > 15))
                    {
                        TargetCret = null;
                    }
                }
                if (LastHiter != null)
                {
                    if (((HUtil32.GetTickCount() - LastHiterTick) > 30000) || LastHiter.Death || LastHiter.Ghost)
                    {
                        LastHiter = null;
                    }
                }
                if (ExpHitter != null)
                {
                    if (((HUtil32.GetTickCount() - ExpHitterTick) > 6000) || ExpHitter.Death || ExpHitter.Ghost)
                    {
                        ExpHitter = null;
                    }
                }
                if (Master != null)
                {
                    MBoNoItem = true;
                    // 宝宝变色
                    int nInteger;
                    if (AutoChangeColor && (HUtil32.GetTickCount() - AutoChangeColorTick > M2Share.Config.dwBBMonAutoChangeColorTime))
                    {
                        AutoChangeColorTick = HUtil32.GetTickCount();
                        switch (AutoChangeIdx)
                        {
                            case 0:
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = Grobal2.POISON_STONE;
                                break;
                            case 2:
                                nInteger = Grobal2.POISON_DONTMOVE;
                                break;
                            case 3:
                                nInteger = Grobal2.POISON_68;
                                break;
                            case 4:
                                nInteger = Grobal2.POISON_DECHEALTH;
                                break;
                            case 5:
                                nInteger = Grobal2.POISON_LOCKSPELL;
                                break;
                            case 6:
                                nInteger = Grobal2.POISON_DAMAGEARMOR;
                                break;
                            default:
                                AutoChangeIdx = 0;
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                        }
                        AutoChangeIdx++;
                        CharStatus = (int)((CharStatusEx & 0xFFFFF) | ((0x80000000 >> nInteger) | 0));
                        StatusChanged();
                    }
                    if (FixColor && (FixStatus != CharStatus))
                    {
                        switch (FixColorIdx)
                        {
                            case 0:
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = Grobal2.POISON_STONE;
                                break;
                            case 2:
                                nInteger = Grobal2.POISON_DONTMOVE;
                                break;
                            case 3:
                                nInteger = Grobal2.POISON_68;
                                break;
                            case 4:
                                nInteger = Grobal2.POISON_DECHEALTH;
                                break;
                            case 5:
                                nInteger = Grobal2.POISON_LOCKSPELL;
                                break;
                            case 6:
                                nInteger = Grobal2.POISON_DAMAGEARMOR;
                                break;
                            default:
                                FixColorIdx = 0;
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                        }
                        CharStatus = (int)((CharStatusEx & 0xFFFFF) | ((0x80000000 >> nInteger) | 0));
                        FixStatus = CharStatus;
                        StatusChanged();
                    }
                    // 宝宝在主人死亡后死亡处理
                    if (Master.Death && ((HUtil32.GetTickCount() - Master.DeathTick) > 1000))
                    {
                        if (M2Share.Config.boMasterDieMutiny && (Master.LastHiter != null) && (M2Share.RandomNumber.Random(M2Share.Config.nMasterDieMutinyRate) == 0))
                        {
                            Master = null;
                            SlaveExpLevel = (byte)M2Share.Config.SlaveColor.Length;
                            RecalcAbilitys();
                            MWAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.DC) * M2Share.Config.nMasterDieMutinyPower, HUtil32.HiWord(MWAbil.DC) * M2Share.Config.nMasterDieMutinyPower);
                            WalkSpeed = WalkSpeed / M2Share.Config.nMasterDieMutinySpeed;
                            RefNameColor();
                            RefShowName();
                        }
                        else
                        {
                            MWAbil.HP = 0;
                        }
                    }
                    if (Master.Ghost && ((HUtil32.GetTickCount() - Master.GhostTick) > 1000))
                    {
                        MakeGhost();
                    }
                }
                // 清除宝宝列表中已经死亡及叛变的宝宝信息
                for (var i = SlaveList.Count - 1; i >= 0; i--)
                {
                    if (SlaveList[i].Death || SlaveList[i].Ghost || (SlaveList[i].Master != this))
                    {
                        SlaveList.RemoveAt(i);
                    }
                }
                if (MBoHolySeize && ((HUtil32.GetTickCount() - MDwHolySeizeTick) > MDwHolySeizeInterval))
                {
                    BreakHolySeizeMode();
                }
                if (CrazyMode && ((HUtil32.GetTickCount() - _crazyModeTick) > CrazyModeInterval))
                {
                    BreakCrazyMode();
                }
                if (MBoShowHp && ((HUtil32.GetTickCount() - MDwShowHpTick) > MDwShowHpInterval))
                {
                    BreakOpenHealth();
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg3);
            }
            try
            {
                // 减少PK值开始
                if ((HUtil32.GetTickCount() - _decPkPointTick) > M2Share.Config.dwDecPkPointTime)// 120000
                {
                    _decPkPointTick = HUtil32.GetTickCount();
                    if (PkPoint > 0)
                    {
                        DecPkPoint(M2Share.Config.nDecPkPointCount);
                    }
                }
                if ((HUtil32.GetTickCount() - DecLightItemDrugTick) > M2Share.Config.dwDecLightItemDrugTime)
                {
                    DecLightItemDrugTick += M2Share.Config.dwDecLightItemDrugTime;
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        UseLamp();
                        CheckPkStatus();
                    }
                }
                if ((HUtil32.GetTickCount() - CheckRoyaltyTick) > 10000)
                {
                    CheckRoyaltyTick = HUtil32.GetTickCount();
                    if (Master != null)
                    {
                        if ((M2Share.g_dwSpiritMutinyTick > HUtil32.GetTickCount()) && (SlaveExpLevel < 5))
                        {
                            MasterRoyaltyTick = 0;
                        }
                        if (HUtil32.GetTickCount() > MasterRoyaltyTick)
                        {
                            for (var i = 0; i < Master.SlaveList.Count; i++)
                            {
                                if (Master.SlaveList[i] == this)
                                {
                                    Master.SlaveList.RemoveAt(i);
                                    break;
                                }
                            }
                            Master = null;
                            MWAbil.HP = (ushort)(MWAbil.HP / 10);
                            RefShowName();
                        }
                        if (MasterTick != 0)
                        {
                            if ((HUtil32.GetTickCount() - MasterTick) > 12 * 60 * 60 * 1000)
                            {
                                MWAbil.HP = 0;
                            }
                        }
                    }
                }

                if ((HUtil32.GetTickCount() - VerifyTick) > 30 * 1000)
                {
                    VerifyTick = HUtil32.GetTickCount();
                    // 清组队已死亡成员
                    if (MGroupOwner != null)
                    {
                        if (MGroupOwner.Death || MGroupOwner.Ghost)
                        {
                            MGroupOwner = null;
                        }
                    }

                    if (MGroupOwner == this)
                    {
                        for (var i = GroupMembers.Count - 1; i >= 0; i--)
                        {
                            BaseObject BaseObject = GroupMembers[i];
                            if (BaseObject.Death || BaseObject.Ghost)
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
                    if (!_denyRefStatus)
                    {
                        Envir.VerifyMapTime(CurrX, CurrY, this);// 刷新在地图上位置的时间
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg4);
                M2Share.Log.Error(e.Message);
            }
            try
            {
                bool boChg = false;
                bool boNeedRecalc = false;
                for (var i = 0; i < MDwStatusArrTick.Length; i++)
                {
                    if ((MWStatusTimeArr[i] > 0) && (MWStatusTimeArr[i] < 60000))
                    {
                        if ((HUtil32.GetTickCount() - MDwStatusArrTick[i]) > 1000)
                        {
                            MWStatusTimeArr[i] -= 1;
                            MDwStatusArrTick[i] += 1000;
                            if (MWStatusTimeArr[i] == 0)
                            {
                                boChg = true;
                                switch (i)
                                {
                                    case Grobal2.STATE_TRANSPARENT:
                                        HideMode = false;
                                        break;
                                    case Grobal2.STATE_DEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("Defense strength is back to normal.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case Grobal2.STATE_MAGDEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("Magical defense strength is back to normal.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case Grobal2.STATE_BUBBLEDEFENCEUP:
                                        AbilMagBubbleDefence = false;
                                        break;
                                }
                            }
                        }
                    }
                }
                for (var i = 0; i < MWStatusArrValue.Length; i++)
                {
                    if (MWStatusArrValue[i] > 0)
                    {
                        if (HUtil32.GetTickCount() > MDwStatusArrTimeOutTick[i])
                        {
                            MWStatusArrValue[i] = 0;
                            boNeedRecalc = true;
                            switch (i)
                            {
                                case 0:
                                    SysMsg("Removed temporarily increased destructive power.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 1:
                                    SysMsg("Removed temporarily increased magic power.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 2:
                                    SysMsg("Removed temporarily increased zen power.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 3:
                                    SysMsg("Removed temporarily increased hitting speed.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 4:
                                    SysMsg("Removed temporarily increased HP.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 5:
                                    SysMsg("Removed temporarily increased MP.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 6:
                                    SysMsg("Removed temporarily decreased attack ability.", MsgColor.Green, MsgType.Hint);
                                    break;
                            }
                        }
                    }
                }
                if (boChg)
                {
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
                if (boNeedRecalc)
                {
                    RecalcAbilitys();
                    SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(sExceptionMsg5);
            }
            try
            {
                if ((HUtil32.GetTickCount() - PoisoningTick) > M2Share.Config.dwPosionDecHealthTime)
                {
                    PoisoningTick = HUtil32.GetTickCount();
                    if (MWStatusTimeArr[Grobal2.POISON_DECHEALTH] > 0)
                    {
                        if (Animal)
                        {
                            MNMeatQuality -= 1000;
                        }
                        DamageHealth(_greenPoisoningPoint + 1);
                        MNHealthTick = 0;
                        MNSpellTick = 0;
                        HealthSpellChanged();
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg6);
            }
            M2Share.g_nBaseObjTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (M2Share.g_nBaseObjTimeMax < M2Share.g_nBaseObjTimeMin)
            {
                M2Share.g_nBaseObjTimeMax = M2Share.g_nBaseObjTimeMin;
            }
        }

        public virtual void Die()
        {
            int tExp;
            bool tCheck;
            const string sExceptionMsg1 = "[Exception] TBaseObject::Die 1";
            const string sExceptionMsg2 = "[Exception] TBaseObject::Die 2";
            const string sExceptionMsg3 = "[Exception] TBaseObject::Die 3";
            if (SuperMan)
            {
                return;
            }
            if (_superManItem)
            {
                return;
            }
            Death = true;
            DeathTick = HUtil32.GetTickCount();
            if (Master != null)
            {
                ExpHitter = null;
                LastHiter = null;
            }

            if (CanReAlive)
            {
                if ((MonGen != null) && (MonGen.Envir != Envir))
                {
                    CanReAlive = false;
                    if (MonGen.nActiveCount > 0)
                    {
                        MonGen.nActiveCount--;
                    }
                    MonGen = null;
                }
            }

            IncSpell = 0;
            IncHealth = 0;
            MNIncHealing = 0;
            KillFunc();
            try
            {
                if (Race != Grobal2.RC_PLAYOBJECT && LastHiter != null)
                {
                    if (M2Share.Config.boMonSayMsg)
                    {
                        MonsterSayMsg(LastHiter, MonStatus.Die);
                    }
                    if (ExpHitter != null)
                    {
                        if (ExpHitter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(ExpHitter as PlayObject, "@PlayKillMob", false);
                            }
                            tExp = ExpHitter.CalcGetExp(Abil.Level, MDwFightExp);
                            if (!M2Share.Config.boVentureServer)
                            {
                                if (ExpHitter.IsRobot)
                                {
                                    (ExpHitter as RobotPlayObject).GainExp(tExp);
                                }
                                else
                                {
                                    (ExpHitter as PlayObject).GainExp(tExp);
                                }
                            }
                            // 是否执行任务脚本
                            if (Envir.IsCheapStuff())
                            {
                                Merchant QuestNPC;
                                if (ExpHitter.MGroupOwner != null)
                                {
                                    for (var i = 0; i < ExpHitter.MGroupOwner.GroupMembers.Count; i++)
                                    {
                                        PlayObject GroupHuman = ExpHitter.MGroupOwner.GroupMembers[i];
                                        if (!GroupHuman.Death && ExpHitter.Envir == GroupHuman.Envir && Math.Abs(ExpHitter.CurrX - GroupHuman.CurrX) <= 12 && Math.Abs(ExpHitter.CurrX - GroupHuman.CurrX) <= 12 && ExpHitter == GroupHuman)
                                        {
                                            tCheck = false;
                                        }
                                        else
                                        {
                                            tCheck = true;
                                        }
                                        QuestNPC = Envir.GetQuestNpc(GroupHuman, CharName, "", tCheck);
                                        if (QuestNPC != null)
                                        {
                                            QuestNPC.Click(GroupHuman);
                                        }
                                    }
                                }
                                QuestNPC = Envir.GetQuestNpc(ExpHitter, CharName, "", false);
                                if (QuestNPC != null)
                                {
                                    QuestNPC.Click(ExpHitter as PlayObject);
                                }
                            }
                        }
                        else
                        {
                            if (ExpHitter.Master != null)
                            {
                                ExpHitter.GainSlaveExp(Abil.Level);
                                tExp = ExpHitter.Master.CalcGetExp(Abil.Level, MDwFightExp);
                                if (!M2Share.Config.boVentureServer)
                                {
                                    if (ExpHitter.Master.IsRobot)
                                    {
                                        (ExpHitter.Master as RobotPlayObject).GainExp(tExp);
                                    }
                                    else
                                    {
                                        (ExpHitter.Master as PlayObject).GainExp(tExp);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(LastHiter as PlayObject, "@PlayKillMob", false);
                            }
                            tExp = LastHiter.CalcGetExp(Abil.Level, MDwFightExp);
                            if (!M2Share.Config.boVentureServer)
                            {
                                if (LastHiter.IsRobot)
                                {
                                    (LastHiter as RobotPlayObject).GainExp(tExp);
                                }
                                else
                                {
                                    (LastHiter as PlayObject).GainExp(tExp);
                                }
                            }
                        }
                    }
                }
                if (M2Share.Config.boMonSayMsg && Race == Grobal2.RC_PLAYOBJECT && LastHiter != null)
                {
                    LastHiter.MonsterSayMsg(this, MonStatus.KillHuman);
                }
                Master = null;
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg1);
                M2Share.Log.Error(e.Message);
            }
            try
            {
                var boPK = false;
                if (!M2Share.Config.boVentureServer && !Envir.Flag.boFightZone && !Envir.Flag.boFight3Zone)
                {
                    if (Race == Grobal2.RC_PLAYOBJECT && LastHiter != null && PvpLevel() < 2)
                    {
                        if ((LastHiter.Race == Grobal2.RC_PLAYOBJECT) || (LastHiter.Race == Grobal2.RC_NPC))//允许NPC杀死人物
                        {
                            boPK = true;
                        }
                        if (LastHiter.Master != null)
                        {
                            if (LastHiter.Master.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                LastHiter = LastHiter.Master;
                                boPK = true;
                            }
                        }
                    }
                }
                if (boPK && LastHiter != null)
                {
                    var guildwarkill = false;
                    if (MyGuild != null && LastHiter.MyGuild != null)
                    {
                        if (GetGuildRelation(this, LastHiter) == 2)
                        {
                            guildwarkill = true;
                        }
                    }
                    var Castle = M2Share.CastleMgr.InCastleWarArea(this);
                    if (Castle != null && Castle.m_boUnderWar || InFreePkArea)
                    {
                        guildwarkill = true;
                    }
                    if (!guildwarkill)
                    {
                        if ((M2Share.Config.boKillHumanWinLevel || M2Share.Config.boKillHumanWinExp || Envir.Flag.boPKWINLEVEL || Envir.Flag.boPKWINEXP) && LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            (this as PlayObject).PKDie(LastHiter as PlayObject);
                        }
                        else
                        {
                            if (!LastHiter.IsGoodKilling(this))
                            {
                                LastHiter.IncPkPoint(M2Share.Config.nKillHumanAddPKPoint);
                                LastHiter.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                                SysMsg(Format(M2Share.g_sYouKilledByMsg, LastHiter.CharName), MsgColor.Red, MsgType.Hint);
                                LastHiter.AddBodyLuck(-M2Share.Config.nKillHumanDecLuckPoint);
                                if (PvpLevel() < 1)
                                {
                                    if (M2Share.RandomNumber.Random(5) == 0)
                                    {
                                        LastHiter.MakeWeaponUnlock();
                                    }
                                }
                            }
                            else
                            {
                                LastHiter.SysMsg(M2Share.g_sYouProtectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                            }
                        }
                        // 检查攻击人是否用了着经验或等级装备
                        if (LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            if (LastHiter.MDwPkDieLostExp > 0)
                            {
                                if (Abil.Exp >= LastHiter.MDwPkDieLostExp)
                                {
                                    Abil.Exp -= (short)LastHiter.MDwPkDieLostExp;
                                }
                                else
                                {
                                    Abil.Exp = 0;
                                }
                            }
                            if (LastHiter.MNPkDieLostLevel > 0)
                            {
                                if (Abil.Level >= LastHiter.MNPkDieLostLevel)
                                {
                                    Abil.Level -= (byte)LastHiter.MNPkDieLostLevel;
                                }
                                else
                                {
                                    Abil.Level = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg2);
            }
            try
            {
                if (!Envir.Flag.boFightZone && !Envir.Flag.boFight3Zone && !Animal)
                {
                    var AttackBaseObject = ExpHitter;
                    if (ExpHitter != null && ExpHitter.Master != null)
                    {
                        AttackBaseObject = ExpHitter.Master;
                    }
                    if (Race != Grobal2.RC_PLAYOBJECT)
                    {
                        DropUseItems(AttackBaseObject);
                        if (Master == null && (!MBoNoItem || !Envir.Flag.boNODROPITEM))
                        {
                            ScatterBagItems(AttackBaseObject);
                        }
                        if (Race >= Grobal2.RC_ANIMAL && Master == null && (!MBoNoItem || !Envir.Flag.boNODROPITEM))
                        {
                            ScatterGolds(AttackBaseObject);
                        }
                    }
                    else
                    {
                        if (!MBoNoItem || !Envir.Flag.boNODROPITEM)//允许设置 m_boNoItem 后人物死亡不掉物品
                        {
                            if (AttackBaseObject != null)
                            {
                                if (M2Share.Config.boKillByHumanDropUseItem && AttackBaseObject.Race == Grobal2.RC_PLAYOBJECT || M2Share.Config.boKillByMonstDropUseItem && AttackBaseObject.Race != Grobal2.RC_PLAYOBJECT)
                                {
                                    DropUseItems(null);
                                }
                            }
                            else
                            {
                                DropUseItems(null);
                            }
                            if (M2Share.Config.boDieScatterBag)
                            {
                                ScatterBagItems(null);
                            }
                            if (M2Share.Config.boDieDropGold)
                            {
                                ScatterGolds(null);
                            }
                        }
                        AddBodyLuck(-(50 - (50 - Abil.Level * 5)));
                    }
                }
                string tStr;
                if (Envir.Flag.boFight3Zone)
                {
                    FightZoneDieCount++;
                    if (MyGuild != null)
                    {
                        MyGuild.TeamFightWhoDead(CharName);
                    }
                    if (LastHiter != null)
                    {
                        if (LastHiter.MyGuild != null && MyGuild != null)
                        {
                            LastHiter.MyGuild.TeamFightWhoWinPoint(LastHiter.CharName, 100);
                            tStr = LastHiter.MyGuild.sGuildName + ':' + LastHiter.MyGuild.nContestPoint + "  " + MyGuild.sGuildName + ':' + MyGuild.nContestPoint;
                            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, Envir, CurrX, CurrY, 1000, M2Share.Config.btCryMsgFColor, M2Share.Config.btCryMsgBColor, "- " + tStr);
                        }
                    }
                }
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    if (MGroupOwner != null)
                    {
                        MGroupOwner.DelMember(this);// 人物死亡立即退组，以防止组队刷经验
                    }
                    if (LastHiter != null)
                    {
                        if (LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            tStr = LastHiter.CharName;
                        }
                        else
                        {
                            tStr = '#' + LastHiter.CharName;
                        }
                    }
                    else
                    {
                        tStr = "####";
                    }
                    M2Share.AddGameDataLog("19" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + "FZ-" + HUtil32.BoolToIntStr(Envir.Flag.boFightZone) + "_F3-" + HUtil32.BoolToIntStr(Envir.Flag.boFight3Zone) + "\t" + '0' + "\t" + '1' + "\t" + tStr);
                }
                // 减少地图上怪物计数
                if (Master == null && !DelFormMaped)
                {
                    Envir.DelObjectCount(this);
                    DelFormMaped = true;
                }
                SendRefMsg(Grobal2.RM_DEATH, Direction, CurrX, CurrY, 1, "");
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg3);
            }
        }

        internal virtual void ReAlive()
        {
            Death = false;
            SendRefMsg(Grobal2.RM_ALIVE, Direction, CurrX, CurrY, 0, "");
        }

        protected virtual bool IsProtectTarget(BaseObject BaseObject)
        {
            var result = true;
            if (BaseObject == null)
            {
                return result;
            }
            if (InSafeZone() || BaseObject.InSafeZone())
            {
                result = false;
            }
            if (!BaseObject.InFreePkArea)
            {
                if (M2Share.Config.boPKLevelProtect)// 新人保护
                {
                    if (Abil.Level > M2Share.Config.nPKProtectLevel)// 如果大于指定等级
                    {
                        if (!BaseObject.PvpFlag && BaseObject.Abil.Level <= M2Share.Config.nPKProtectLevel &&
                            BaseObject.PvpLevel() < 2)// 被攻击的人物小指定等级没有红名，则不可以攻击。
                        {
                            result = false;
                            return result;
                        }
                    }
                    if (Abil.Level <= M2Share.Config.nPKProtectLevel)// 如果小于指定等级
                    {
                        if (!BaseObject.PvpFlag && BaseObject.Abil.Level > M2Share.Config.nPKProtectLevel && BaseObject.PvpLevel() < 2)
                        {
                            result = false;
                            return result;
                        }
                    }
                }
                // 大于指定级别的红名人物不可以杀指定级别未红名的人物。
                if (PvpLevel() >= 2 && Abil.Level > M2Share.Config.nRedPKProtectLevel)
                {
                    if (BaseObject.Abil.Level <= M2Share.Config.nRedPKProtectLevel && BaseObject.PvpLevel() < 2)
                    {
                        result = false;
                        return result;
                    }
                }
                // 小于指定级别的非红名人物不可以杀指定级别红名人物。
                if (Abil.Level <= M2Share.Config.nRedPKProtectLevel && PvpLevel() < 2)
                {
                    if (BaseObject.PvpLevel() >= 2 && BaseObject.Abil.Level > M2Share.Config.nRedPKProtectLevel)
                    {
                        result = false;
                        return result;
                    }
                }
                if (((HUtil32.GetTickCount() - MapMoveTick) < 3000) || ((HUtil32.GetTickCount() - BaseObject.MapMoveTick) < 3000))
                {
                    result = false;
                }
            }
            return result;
        }

        protected virtual void ProcessSayMsg(string sMsg)
        {
            string sCharName;
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                sCharName = CharName;
            }
            else
            {
                sCharName = M2Share.FilterShowName(CharName);
            }
            SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, sCharName + ':' + sMsg);
        }

        public virtual void MakeGhost()
        {
            if (CanReAlive)
            {
                Invisible = true;
                GhostTick = HUtil32.GetTickCount();
                Envir.DeleteFromMap(CurrX, CurrY, CellType.MovingObject, this);
                SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
            }
            else
            {
                Ghost = true;
                GhostTick = HUtil32.GetTickCount();
                DisappearA();
            }
        }

        /// <summary>
        /// 散落包裹物品
        /// </summary>
        /// <param name="ItemOfCreat"></param>
        protected virtual void ScatterBagItems(BaseObject ItemOfCreat)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::ScatterBagItems";
            try
            {
                var DropWide = HUtil32._MIN(M2Share.Config.nDropItemRage, 7);
                if ((Race == Grobal2.RC_PLAYCLONE) && (Master != null))
                {
                    return;
                }
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    var UserItem = ItemList[i];
                    var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    var boCanNotDrop = false;
                    if (StdItem != null)
                    {
                        TMonDrop MonDrop = null;
                        if (M2Share.g_MonDropLimitLIst.TryGetValue(StdItem.Name, out MonDrop))
                        {
                            if (MonDrop.nDropCount < MonDrop.nCountLimit)
                            {
                                MonDrop.nDropCount++;
                                M2Share.g_MonDropLimitLIst[StdItem.Name] = MonDrop;
                            }
                            else
                            {
                                MonDrop.nNoDropCount++;
                                boCanNotDrop = true;
                            }
                            break;
                        }
                    }
                    if (boCanNotDrop)
                    {
                        continue;
                    }
                    if (DropItemDown(UserItem, DropWide, true, ItemOfCreat, this))
                    {
                        Dispose(UserItem);
                        ItemList.RemoveAt(i);
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
        }

        protected virtual void DropUseItems(BaseObject BaseObject)
        {
            int nC;
            int nRate;
            StdItem StdItem;
            IList<TDeleteItem> DropItemList = null;
            const string sExceptionMsg = "[Exception] TBaseObject::DropUseItems";
            try
            {
                if (NoDropUseItem)
                {
                    return;
                }
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    nC = 0;
                    while (true)
                    {
                        if (UseItems[nC] == null)
                        {
                            nC++;
                            continue;
                        }
                        StdItem = M2Share.UserEngine.GetStdItem(UseItems[nC].wIndex);
                        if (StdItem != null)
                        {
                            if ((StdItem.Reserved & 8) != 0)
                            {
                                if (DropItemList == null)
                                {
                                    DropItemList = new List<TDeleteItem>();
                                }
                                DropItemList.Add(new TDeleteItem()
                                {
                                    MakeIndex = UseItems[nC].MakeIndex
                                });
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog("16" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + StdItem.Name + "\t" + UseItems[nC].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                                }
                                UseItems[nC].wIndex = 0;
                            }
                        }
                        nC++;
                        if (nC >= 9)
                        {
                            break;
                        }
                    }
                }
                if (PvpLevel() > 2)
                {
                    nRate = 15;
                }
                else
                {
                    nRate = 30;
                }
                nC = 0;
                while (true)
                {
                    if (M2Share.RandomNumber.Random(nRate) == 0)
                    {
                        if (UseItems[nC] == null)
                        {
                            nC++;
                            continue;
                        }
                        if (DropItemDown(UseItems[nC], 2, true, BaseObject, this))
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(UseItems[nC].wIndex);
                            if (StdItem != null)
                            {
                                if ((StdItem.Reserved & 10) == 0)
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (DropItemList == null)
                                        {
                                            DropItemList = new List<TDeleteItem>();
                                        }
                                        DropItemList.Add(new TDeleteItem()
                                        {
                                            sItemName = M2Share.UserEngine.GetStdItemName(UseItems[nC].wIndex),
                                            MakeIndex = UseItems[nC].MakeIndex
                                        });
                                    }
                                    UseItems[nC].wIndex = 0;
                                }
                            }
                        }
                    }
                    nC++;
                    if (nC >= 9)
                    {
                        break;
                    }
                }
                if (DropItemList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, DropItemList);
                    SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(ex.StackTrace);
            }
        }

        public virtual void SetTargetCreat(BaseObject BaseObject)
        {
            TargetCret = BaseObject;
            TargetFocusTick = HUtil32.GetTickCount();
        }

        protected virtual void DelTargetCreat()
        {
            TargetCret = null;
        }

        public virtual bool IsProperFriend(BaseObject BaseObject)
        {
            bool result = false;
            if (BaseObject == null)
            {
                return false;
            }
            if (Race >= Grobal2.RC_ANIMAL)
            {
                if (BaseObject.Race >= Grobal2.RC_ANIMAL)
                {
                    result = true;
                }
                if (BaseObject.Master != null)
                {
                    result = false;
                }
                return result;
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                result = IsProperFriend_IsFriend(BaseObject);
                if (BaseObject.Race < Grobal2.RC_ANIMAL)
                {
                    return result;
                }
                if (BaseObject.Master == this)
                {
                    return true;
                }
                if (BaseObject.Master != null)
                {
                    return IsProperFriend_IsFriend(BaseObject.Master);
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        protected virtual bool Operate(TProcessMessage ProcessMsg)
        {
            int nDamage;
            int nTargetX;
            int nTargetY;
            int nPower;
            int nRage;
            BaseObject TargetBaseObject;
            const string sExceptionMsg = "[Exception] TBaseObject::Operate ";
            bool result = false;
            try
            {
                switch (ProcessMsg.wIdent)
                {
                    case Grobal2.RM_MAGSTRUCK:
                    case Grobal2.RM_MAGSTRUCK_MINE:
                        if ((ProcessMsg.wIdent == Grobal2.RM_MAGSTRUCK) && (Race >= Grobal2.RC_ANIMAL) && !Bo2Bf && (Abil.Level < 50))
                        {
                            WalkTick = WalkTick + 800 + M2Share.RandomNumber.Random(1000);
                        }
                        nDamage = GetMagStruckDamage(null, ProcessMsg.nParam1);
                        if (nDamage > 0)
                        {
                            StruckDamage(nDamage);
                            HealthSpellChanged();
                            SendRefMsg(Grobal2.RM_STRUCK_MAG, (short)nDamage, MWAbil.HP, MWAbil.MaxHP, ProcessMsg.BaseObject, "");
                            TargetBaseObject = M2Share.ActorMgr.Get(ProcessMsg.BaseObject);
                            if (M2Share.Config.boMonDelHptoExp)
                            {
                                if (TargetBaseObject.Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    if ((TargetBaseObject as PlayObject).MWAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(CharName))
                                        {
                                            if (TargetBaseObject.IsRobot)
                                            {
                                                (TargetBaseObject as RobotPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                            }
                                            else
                                            {
                                                (TargetBaseObject as PlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                            }
                                        }
                                    }
                                }
                                if (TargetBaseObject.Race == Grobal2.RC_PLAYCLONE)
                                {
                                    if (TargetBaseObject.Master != null)
                                    {
                                        if ((TargetBaseObject.Master as PlayObject).MWAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(CharName))
                                            {
                                                if (TargetBaseObject.Master.IsRobot)
                                                {
                                                    (TargetBaseObject.Master as RobotPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                                }
                                                else
                                                {
                                                    (TargetBaseObject.Master as PlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (Race != Grobal2.RC_PLAYOBJECT)
                            {
                                if (Animal)
                                {
                                    MNMeatQuality -= (ushort)(nDamage * 1000);
                                }
                                SendMsg(this, Grobal2.RM_STRUCK, nDamage, MWAbil.HP, MWAbil.MaxHP, ProcessMsg.BaseObject, "");
                            }
                        }
                        if (FastParalysis)
                        {
                            MWStatusTimeArr[Grobal2.POISON_STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_MAGHEALING:
                        if ((MNIncHealing + ProcessMsg.nParam1) < 300)
                        {
                            if (Race == Grobal2.RC_PLAYOBJECT)
                            {
                                MNIncHealing += ProcessMsg.nParam1;
                                MNPerHealing = 5;
                            }
                            else
                            {
                                MNIncHealing += ProcessMsg.nParam1;
                                MNPerHealing = 5;
                            }
                        }
                        else
                        {
                            MNIncHealing = 300;
                        }
                        break;
                    case Grobal2.RM_10101:
                        SendRefMsg(ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.Msg);
                        if ((ProcessMsg.BaseObject == Grobal2.RM_STRUCK) && (Race != Grobal2.RC_PLAYOBJECT))
                        {
                            SendMsg(this, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.Msg);
                        }
                        if (FastParalysis)
                        {
                            MWStatusTimeArr[Grobal2.POISON_STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_DELAYMAGIC:
                        nPower = ProcessMsg.wParam;
                        nTargetX = HUtil32.LoWord(ProcessMsg.nParam1);
                        nTargetY = HUtil32.HiWord(ProcessMsg.nParam1);
                        nRage = ProcessMsg.nParam2;
                        TargetBaseObject = M2Share.ActorMgr.Get(ProcessMsg.nParam3);
                        if ((TargetBaseObject != null) && (TargetBaseObject.GetMagStruckDamage(this, nPower) > 0))
                        {
                            SetTargetCreat(TargetBaseObject);
                            if (TargetBaseObject.Race >= Grobal2.RC_ANIMAL)
                            {
                                nPower = HUtil32.Round(nPower / 1.2);
                            }
                            if ((Math.Abs(nTargetX - TargetBaseObject.CurrX) <= nRage) && (Math.Abs(nTargetY - TargetBaseObject.CurrY) <= nRage))
                            {
                                TargetBaseObject.SendMsg(this, Grobal2.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                            }
                        }
                        break;
                    case Grobal2.RM_10155:
                        MapRandomMove(ProcessMsg.Msg, ProcessMsg.wParam);
                        break;
                    case Grobal2.RM_DELAYPUSHED:
                        nPower = ProcessMsg.wParam;
                        nTargetX = HUtil32.LoWord(ProcessMsg.nParam1);
                        nTargetY = HUtil32.HiWord(ProcessMsg.nParam1);
                        nRage = ProcessMsg.nParam2;
                        TargetBaseObject = M2Share.ActorMgr.Get(ProcessMsg.nParam3);// M2Share.ObjectSystem.Get(ProcessMsg.nParam3);
                        if (TargetBaseObject != null)
                        {
                            TargetBaseObject.CharPushed((byte)nPower, nRage);
                        }
                        break;
                    case Grobal2.RM_POISON:
                        TargetBaseObject = M2Share.ActorMgr.Get(ProcessMsg.nParam2);// ((ProcessMsg.nParam2) as TBaseObject);
                        if (TargetBaseObject != null)
                        {
                            if (IsProperTarget(TargetBaseObject))
                            {
                                SetTargetCreat(TargetBaseObject);
                                if ((Race == Grobal2.RC_PLAYOBJECT) && (TargetBaseObject.Race == Grobal2.RC_PLAYOBJECT))
                                {
                                    SetPkFlag(TargetBaseObject);
                                }
                                SetLastHiter(TargetBaseObject);
                            }
                            MakePosion(ProcessMsg.wParam, (ushort)ProcessMsg.nParam1, ProcessMsg.nParam3);// 中毒类型
                        }
                        else
                        {
                            MakePosion(ProcessMsg.wParam, (ushort)ProcessMsg.nParam1, ProcessMsg.nParam3);// 中毒类型
                        }
                        break;
                    case Grobal2.RM_TRANSPARENT:
                        M2Share.MagicMgr.MagMakePrivateTransparent(this, (ushort)ProcessMsg.nParam1);
                        break;
                    case Grobal2.RM_DOOPENHEALTH:
                        MakeOpenHealth();
                        break;
                        /*default:
                            Debug.WriteLine(format("人物: {0} 消息: Ident {1} Param {2} P1 {3} P2 {3} P3 {4} Msg {5}", m_sCharName, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg));
                            break;*/
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
            return result;
        }

        public virtual string GetShowName()
        {
            var sShowName = CharName;
            var result = M2Share.FilterShowName(sShowName);
            if ((Master != null) && !Master.ObMode)
            {
                result = result + '(' + Master.CharName + ')';
            }
            return result;
        }

        /// <summary>
        /// 计算自身属性
        /// </summary>
        public virtual void RecalcAbilitys()
        {
            bool[] boRecallSuite = new bool[4] { false, false, false, false };
            bool[] boMoXieSuite = new bool[3] { false, false, false };
            bool[] boSpirit = new bool[4] { false, false, false, false };
            _mAddAbil = new TAddAbility();
            ushort wOldHP = MWAbil.HP;
            ushort wOldMP = MWAbil.MP;
            MWAbil = Abil;
            MWAbil.HP = wOldHP;
            MWAbil.MP = wOldMP;
            MWAbil.Weight = 0;
            MWAbil.WearWeight = 0;
            MWAbil.HandWeight = 0;
            AntiPoison = 0;
            PoisonRecover = 0;
            MNHealthRecover = 0;
            MNSpellRecover = 0;
            AntiMagic = 1;
            MNLuck = 0;
            HitSpeed = 0;
            MBoExpItem = false;
            ExpItem = 0;
            MBoPowerItem = false;
            PowerItem = 0;
            HideMode = false;
            Teleport = false;
            Paralysis = false;
            Revival = false;
            UnRevival = false;
            FlameRing = false;
            RecoveryRing = false;
            AngryRing = false;
            MagicShield = false;
            UnMagicShield = false;
            MuscleRing = false;
            FastTrain = false;
            ProbeNecklace = false;
            _superManItem = false;
            GuildMove = false;
            UnParalysis = false;
            MBoExpItem = false;
            MBoPowerItem = false;
            NoDropItem = false;
            NoDropUseItem = false;
            MBopirit = false;
            HorseType = 0;
            _dressEffType = 0;
            MNAutoAddHpmpMode = 0;
            // 气血石
            MNMoXieSuite = 0;
            MDb3B0 = 0;
            MNHongMoSuite = 0;
            bool boHongMoSuite1 = false;
            bool boHongMoSuite2 = false;
            bool boHongMoSuite3 = false;
            MBoRecallSuite = false;
            SmashSet = false;
            bool boSmash1 = false;
            bool boSmash2 = false;
            bool boSmash3 = false;
            HwanDevilSet = false;
            bool boHwanDevil1 = false;
            bool boHwanDevil2 = false;
            bool boHwanDevil3 = false;
            PuritySet = false;
            bool boPurity1 = false;
            bool boPurity2 = false;
            bool boPurity3 = false;
            MundaneSet = false;
            bool boMundane1 = false;
            bool boMundane2 = false;
            NokChiSet = false;
            bool boNokChi1 = false;
            bool boNokChi2 = false;
            TaoBuSet = false;
            bool boTaoBu1 = false;
            bool boTaoBu2 = false;
            FiveStringSet = false;
            bool boFiveString1 = false;
            bool boFiveString2 = false;
            bool boFiveString3 = false;
            bool boOldHideMode = HideMode;
            MDwPkDieLostExp = 0;
            MNPkDieLostLevel = 0;
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == null)
                {
                    continue;
                }
                if ((UseItems[i].wIndex <= 0) || (UseItems[i].Dura <= 0))
                {
                    continue;
                }
                var StdItem = M2Share.UserEngine.GetStdItem(UseItems[i].wIndex);
                if (StdItem == null)
                {
                    continue;
                }
                StdItem.ApplyItemParameters(ref _mAddAbil);
                if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND) || (i == Grobal2.U_DRESS))
                {
                    if (i == Grobal2.U_DRESS)
                    {
                        MWAbil.WearWeight += StdItem.Weight;
                    }
                    else
                    {
                        MWAbil.HandWeight += StdItem.Weight;
                    }
                    // 新增开始
                    if (StdItem.AniCount == 120)
                    {
                        FastTrain = true;
                    }
                    if (StdItem.AniCount == 121)
                    {
                        ProbeNecklace = true;
                    }
                    if (StdItem.AniCount == 145)
                    {
                        GuildMove = true;
                    }
                    if (StdItem.AniCount == 111)
                    {
                        MWStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                        HideMode = true;
                    }
                    if (StdItem.AniCount == 112)
                    {
                        Teleport = true;
                    }
                    if (StdItem.AniCount == 113)
                    {
                        Paralysis = true;
                    }
                    if (StdItem.AniCount == 114)
                    {
                        Revival = true;
                    }
                    if (StdItem.AniCount == 115)
                    {
                        FlameRing = true;
                    }
                    if (StdItem.AniCount == 116)
                    {
                        RecoveryRing = true;
                    }
                    if (StdItem.AniCount == 117)
                    {
                        AngryRing = true;
                    }
                    if (StdItem.AniCount == 118)
                    {
                        MagicShield = true;
                    }
                    if (StdItem.AniCount == 119)
                    {
                        MuscleRing = true;
                    }
                    if (StdItem.AniCount == 135)
                    {
                        boMoXieSuite[0] = true;
                        MNMoXieSuite += StdItem.Weight / 10;
                    }
                    if (StdItem.AniCount == 138)
                    {
                        MNHongMoSuite += StdItem.Weight;
                    }
                    if (StdItem.AniCount == 139)
                    {
                        UnParalysis = true;
                    }
                    if (StdItem.AniCount == 140)
                    {
                        _superManItem = true;
                    }
                    if (StdItem.AniCount == 141)
                    {
                        MBoExpItem = true;
                        ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.nItemExpRate);
                    }
                    if (StdItem.AniCount == 142)
                    {
                        MBoPowerItem = true;
                        PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.nItemPowerRate);
                    }
                    if (StdItem.AniCount == 182)
                    {
                        MBoExpItem = true;
                        ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.nItemExpRate);
                    }
                    if (StdItem.AniCount == 183)
                    {
                        MBoPowerItem = true;
                        PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.nItemPowerRate);
                    }
                    if (StdItem.AniCount == 143)
                    {
                        UnMagicShield = true;
                    }
                    if (StdItem.AniCount == 144)
                    {
                        UnRevival = true;
                    }
                    if (StdItem.AniCount == 170)
                    {
                        AngryRing = true;
                    }
                    if (StdItem.AniCount == 171)
                    {
                        NoDropItem = true;
                    }
                    if (StdItem.AniCount == 172)
                    {
                        NoDropUseItem = true;
                    }
                    if (StdItem.AniCount == 150)
                    {
                        // 麻痹护身
                        Paralysis = true;
                        MagicShield = true;
                    }
                    if (StdItem.AniCount == 151)
                    {
                        // 麻痹火球
                        Paralysis = true;
                        FlameRing = true;
                    }
                    if (StdItem.AniCount == 152)
                    {
                        // 麻痹防御
                        Paralysis = true;
                        RecoveryRing = true;
                    }
                    if (StdItem.AniCount == 153)
                    {
                        // 麻痹负载
                        Paralysis = true;
                        MuscleRing = true;
                    }
                    if (StdItem.Shape == 154)
                    {
                        // 护身火球
                        MagicShield = true;
                        FlameRing = true;
                    }
                    if (StdItem.AniCount == 155)
                    {
                        // 护身防御
                        MagicShield = true;
                        RecoveryRing = true;
                    }
                    if (StdItem.AniCount == 156)
                    {
                        // 护身负载
                        MagicShield = true;
                        MuscleRing = true;
                    }
                    if (StdItem.AniCount == 157)
                    {
                        // 传送麻痹
                        Teleport = true;
                        Paralysis = true;
                    }
                    if (StdItem.AniCount == 158)
                    {
                        // 传送护身
                        Teleport = true;
                        MagicShield = true;
                    }
                    if (StdItem.AniCount == 159)
                    {
                        // 传送探测
                        Teleport = true;
                        ProbeNecklace = true;
                    }
                    if (StdItem.AniCount == 160)
                    {
                        // 传送复活
                        Teleport = true;
                        Revival = true;
                    }
                    if (StdItem.AniCount == 161)
                    {
                        // 麻痹复活
                        Paralysis = true;
                        Revival = true;
                    }
                    if (StdItem.AniCount == 162)
                    {
                        // 护身复活
                        MagicShield = true;
                        Revival = true;
                    }
                    if (StdItem.AniCount == 180)
                    {
                        // PK 死亡掉经验
                        MDwPkDieLostExp = StdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                    }
                    if (StdItem.AniCount == 181)
                    {
                        // PK 死亡掉等级
                        MNPkDieLostLevel = StdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                    }
                    // 新增结束
                }
                else
                {
                    MWAbil.WearWeight += StdItem.Weight;
                }
                MWAbil.Weight += StdItem.Weight;
                if (i == Grobal2.U_WEAPON)
                {
                    if ((StdItem.Source - 1 - 10) < 0)
                    {
                        _mAddAbil.btWeaponStrong = (byte)StdItem.Source;// 强度+
                    }
                    if ((StdItem.Source <= -1) && (StdItem.Source >= -50))  // -1 to -50
                    {
                        _mAddAbil.btUndead = (byte)(_mAddAbil.btUndead + -StdItem.Source);// Holy+
                    }
                    if ((StdItem.Source <= -51) && (StdItem.Source >= -100))// -51 to -100
                    {
                        _mAddAbil.btUndead = (byte)(_mAddAbil.btUndead + (StdItem.Source + 50));// Holy-
                    }
                    continue;
                }
                if (i == Grobal2.U_RIGHTHAND)
                {
                    if (StdItem.Shape >= 1 && StdItem.Shape <= 50)
                    {
                        _dressEffType = StdItem.Shape;
                    }
                    if (StdItem.Shape >= 51 && StdItem.Shape <= 100)
                    {
                        HorseType = (byte)(StdItem.Shape - 50);
                    }
                    continue;
                }
                if (i == Grobal2.U_DRESS)
                {
                    if (UseItems[i].btValue[5] > 0)
                    {
                        _dressEffType = UseItems[i].btValue[5];
                    }
                    if (StdItem.AniCount > 0)
                    {
                        _dressEffType = StdItem.AniCount;
                    }
                    if (StdItem.Light)
                    {
                        Light = 3;
                    }
                    continue;
                }
                // 新增开始
                if (StdItem.Shape == 139)
                {
                    UnParalysis = true;
                }
                if (StdItem.Shape == 140)
                {
                    _superManItem = true;
                }
                if (StdItem.Shape == 141)
                {
                    MBoExpItem = true;
                    ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.nItemExpRate);
                }
                if (StdItem.Shape == 142)
                {
                    MBoPowerItem = true;
                    PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.nItemPowerRate);
                }
                if (StdItem.Shape == 182)
                {
                    MBoExpItem = true;
                    ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.nItemExpRate);
                }
                if (StdItem.Shape == 183)
                {
                    MBoPowerItem = true;
                    PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.nItemPowerRate);
                }
                if (StdItem.Shape == 143)
                {
                    UnMagicShield = true;
                }
                if (StdItem.Shape == 144)
                {
                    UnRevival = true;
                }
                if (StdItem.Shape == 170)
                {
                    AngryRing = true;
                }
                if (StdItem.Shape == 171)
                {
                    NoDropItem = true;
                }
                if (StdItem.Shape == 172)
                {
                    NoDropUseItem = true;
                }
                if (StdItem.Shape == 150)
                {
                    // 麻痹护身
                    Paralysis = true;
                    MagicShield = true;
                }
                if (StdItem.Shape == 151)
                {
                    // 麻痹火球
                    Paralysis = true;
                    FlameRing = true;
                }
                if (StdItem.Shape == 152)
                {
                    // 麻痹防御
                    Paralysis = true;
                    RecoveryRing = true;
                }
                if (StdItem.Shape == 153)
                {
                    // 麻痹负载
                    Paralysis = true;
                    MuscleRing = true;
                }
                if (StdItem.Shape == 154)
                {
                    // 护身火球
                    MagicShield = true;
                    FlameRing = true;
                }
                if (StdItem.Shape == 155)
                {
                    // 护身防御
                    MagicShield = true;
                    RecoveryRing = true;
                }
                if (StdItem.Shape == 156)
                {
                    // 护身负载
                    MagicShield = true;
                    MuscleRing = true;
                }
                if (StdItem.Shape == 157)
                {
                    // 传送麻痹
                    Teleport = true;
                    Paralysis = true;
                }
                if (StdItem.Shape == 158)
                {
                    // 传送护身
                    Teleport = true;
                    MagicShield = true;
                }
                if (StdItem.Shape == 159)
                {
                    // 传送探测
                    Teleport = true;
                    ProbeNecklace = true;
                }
                if (StdItem.Shape == 160)
                {
                    // 传送复活
                    Teleport = true;
                    Revival = true;
                }
                if (StdItem.Shape == 161)
                {
                    // 麻痹复活
                    Paralysis = true;
                    Revival = true;
                }
                if (StdItem.Shape == 162)
                {
                    // 护身复活
                    MagicShield = true;
                    Revival = true;
                }
                if (StdItem.Shape == 180)
                {
                    // PK 死亡掉经验
                    MDwPkDieLostExp = StdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                }
                if (StdItem.Shape == 181)
                {
                    // PK 死亡掉等级
                    MNPkDieLostLevel = StdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                }
                // 新增结束
                if (StdItem.Shape == 120)
                {
                    FastTrain = true;
                }
                if (StdItem.Shape == 121)
                {
                    ProbeNecklace = true;
                }
                if (StdItem.Shape == 123)
                {
                    boRecallSuite[0] = true;
                }
                if (StdItem.Shape == 145)
                {
                    GuildMove = true;
                }
                if (StdItem.Shape == 127)
                {
                    boSpirit[0] = true;
                }
                if (StdItem.Shape == 135)
                {
                    boMoXieSuite[0] = true;
                    MNMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 138)
                {
                    boHongMoSuite1 = true;
                    MNHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 200)
                {
                    boSmash1 = true;
                }
                if (StdItem.Shape == 203)
                {
                    boHwanDevil1 = true;
                }
                if (StdItem.Shape == 206)
                {
                    boPurity1 = true;
                }
                if (StdItem.Shape == 216)
                {
                    boFiveString1 = true;
                }
                if (StdItem.Shape == 111)
                {
                    MWStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                    HideMode = true;
                }
                if (StdItem.Shape == 112)
                {
                    Teleport = true;
                }
                if (StdItem.Shape == 113)
                {
                    Paralysis = true;
                }
                if (StdItem.Shape == 114)
                {
                    Revival = true;
                }
                if (StdItem.Shape == 115)
                {
                    FlameRing = true;
                }
                if (StdItem.Shape == 116)
                {
                    RecoveryRing = true;
                }
                if (StdItem.Shape == 117)
                {
                    AngryRing = true;
                }
                if (StdItem.Shape == 118)
                {
                    MagicShield = true;
                }
                if (StdItem.Shape == 119)
                {
                    MuscleRing = true;
                }
                if (StdItem.Shape == 122)
                {
                    boRecallSuite[1] = true;
                }
                if (StdItem.Shape == 128)
                {
                    boSpirit[1] = true;
                }
                if (StdItem.Shape == 133)
                {
                    boMoXieSuite[1] = true;
                    MNMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 136)
                {
                    boHongMoSuite2 = true;
                    MNHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 201)
                {
                    boSmash2 = true;
                }
                if (StdItem.Shape == 204)
                {
                    boHwanDevil2 = true;
                }
                if (StdItem.Shape == 207)
                {
                    boPurity2 = true;
                }
                if (StdItem.Shape == 210)
                {
                    boMundane1 = true;
                }
                if (StdItem.Shape == 212)
                {
                    boNokChi1 = true;
                }
                if (StdItem.Shape == 214)
                {
                    boTaoBu1 = true;
                }
                if (StdItem.Shape == 217)
                {
                    boFiveString2 = true;
                }
                if ((StdItem.Source <= -1) && (StdItem.Source >= -50))
                {
                    // -1 to -50
                    _mAddAbil.btUndead = (byte)(_mAddAbil.btUndead + -StdItem.Source);
                    // Holy+
                }
                if ((StdItem.Source <= -51) && (StdItem.Source >= -100))
                {
                    // -51 to -100
                    _mAddAbil.btUndead = (byte)(_mAddAbil.btUndead + (StdItem.Source + 50));
                    // Holy-
                }
                if (StdItem.Shape == 124)
                {
                    boRecallSuite[2] = true;
                }
                if (StdItem.Shape == 126)
                {
                    boSpirit[2] = true;
                }
                if (StdItem.Shape == 145)
                {
                    GuildMove = true;
                }
                if (StdItem.Shape == 134)
                {
                    boMoXieSuite[2] = true;
                    MNMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 137)
                {
                    boHongMoSuite3 = true;
                    MNHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 202)
                {
                    boSmash3 = true;
                }
                if (StdItem.Shape == 205)
                {
                    boHwanDevil3 = true;
                }
                if (StdItem.Shape == 208)
                {
                    boPurity3 = true;
                }
                if (StdItem.Shape == 211)
                {
                    boMundane2 = true;
                }
                if (StdItem.Shape == 213)
                {
                    boNokChi2 = true;
                }
                if (StdItem.Shape == 215)
                {
                    boTaoBu2 = true;
                }
                if (StdItem.Shape == 218)
                {
                    boFiveString3 = true;
                }
                if (StdItem.Shape == 125)
                {
                    boRecallSuite[3] = true;
                }
                if (StdItem.Shape == 129)
                {
                    boSpirit[3] = true;
                }
            }
            if (boRecallSuite[0] && boRecallSuite[1] && boRecallSuite[2] && boRecallSuite[3])
            {
                MBoRecallSuite = true;
            }
            if (boMoXieSuite[0] && boMoXieSuite[1] && boMoXieSuite[2])
            {
                MNMoXieSuite += 50;
            }
            if (boHongMoSuite1 && boHongMoSuite2 && boHongMoSuite3)
            {
                _mAddAbil.wHitPoint += 2;
            }
            if (boSpirit[0] && boSpirit[1] && boSpirit[2] && boSpirit[3])
            {
                MBopirit = true;
            }
            if (boSmash1 && boSmash2 && boSmash3)
            {
                SmashSet = true;
            }
            if (boHwanDevil1 && boHwanDevil2 && boHwanDevil3)
            {
                HwanDevilSet = true;
            }
            if (boPurity1 && boPurity2 && boPurity3)
            {
                PuritySet = true;
            }
            if (boMundane1 && boMundane2)
            {
                MundaneSet = true;
            }
            if (boNokChi1 && boNokChi2)
            {
                NokChiSet = true;
            }
            if (boTaoBu1 && boTaoBu2)
            {
                TaoBuSet = true;
            }
            if (boFiveString1 && boFiveString2 && boFiveString3)
            {
                FiveStringSet = true;
            }
            MWAbil.Weight = RecalcBagWeight();
            if (Transparent && (MWStatusTimeArr[Grobal2.STATE_TRANSPARENT] > 0))
            {
                HideMode = true;
            }
            if (HideMode)
            {
                if (!boOldHideMode)
                {
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else
            {
                if (boOldHideMode)
                {
                    MWStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 0;
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                RecalcHitSpeed();//增加此行，只有类型为人物的角色才重新计算攻击敏捷
            }
            int nOldLight = Light;
            if ((UseItems[Grobal2.U_RIGHTHAND] != null) && (UseItems[Grobal2.U_RIGHTHAND].wIndex > 0) && (UseItems[Grobal2.U_RIGHTHAND].Dura > 0))
            {
                Light = 3;
            }
            else
            {
                Light = 0;
            }
            if (nOldLight != Light)
            {
                SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
            }
            SpeedPoint += (byte)_mAddAbil.wSpeedPoint;
            MBtHitPoint += (byte)_mAddAbil.wHitPoint;
            AntiPoison += (byte)_mAddAbil.wAntiPoison;
            PoisonRecover += _mAddAbil.wPoisonRecover;
            MNHealthRecover += _mAddAbil.wHealthRecover;
            MNSpellRecover += _mAddAbil.wSpellRecover;
            AntiMagic += _mAddAbil.wAntiMagic;
            MNLuck += _mAddAbil.btLuck;
            MNLuck -= _mAddAbil.btUnLuck;
            HitSpeed = _mAddAbil.nHitSpeed;
            MWAbil.MaxWeight += _mAddAbil.Weight;
            MWAbil.MaxWearWeight += (byte)_mAddAbil.WearWeight;
            MWAbil.MaxHandWeight += (byte)_mAddAbil.HandWeight;
            MWAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + _mAddAbil.wHP);
            MWAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxMP + _mAddAbil.wMP);
            MWAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(_mAddAbil.wAC) + HUtil32.LoWord(Abil.AC), HUtil32.HiWord(_mAddAbil.wAC) + HUtil32.HiWord(Abil.AC));
            MWAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(_mAddAbil.wMAC) + HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(_mAddAbil.wMAC) + HUtil32.HiWord(Abil.MAC));
            MWAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(_mAddAbil.wDC) + HUtil32.LoWord(Abil.DC), HUtil32.HiWord(_mAddAbil.wDC) + HUtil32.HiWord(Abil.DC));
            MWAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(_mAddAbil.wMC) + HUtil32.LoWord(Abil.MC), HUtil32.HiWord(_mAddAbil.wMC) + HUtil32.HiWord(Abil.MC));
            MWAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(_mAddAbil.wSC) + HUtil32.LoWord(Abil.SC), HUtil32.HiWord(_mAddAbil.wSC) + HUtil32.HiWord(Abil.SC));
            if (MWStatusTimeArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                MWAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.AC), HUtil32.HiWord(MWAbil.AC) + 2 + (Abil.Level / 7));
            }
            if (MWStatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                MWAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.MAC), HUtil32.HiWord(MWAbil.MAC) + 2 + (Abil.Level / 7));
            }
            if (MWStatusArrValue[0] > 0)
            {
                MWAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.DC), HUtil32.HiWord(MWAbil.DC) + 2 + MWStatusArrValue[0]);
            }
            if (MWStatusArrValue[1] > 0)
            {
                MWAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.MC), HUtil32.HiWord(MWAbil.MC) + 2 + MWStatusArrValue[1]);
            }
            if (MWStatusArrValue[2] > 0)
            {
                MWAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.SC), HUtil32.HiWord(MWAbil.SC) + 2 + MWStatusArrValue[2]);
            }
            if (MWStatusArrValue[3] > 0)
            {
                HitSpeed += MWStatusArrValue[3];
            }
            if (MWStatusArrValue[4] > 0)
            {
                MWAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxHP + MWStatusArrValue[4]);
            }
            if (MWStatusArrValue[5] > 0)
            {
                MWAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxMP + MWStatusArrValue[5]);
            }
            if (FlameRing)
            {
                AddItemSkill(1);
            }
            else
            {
                DelItemSkill(1);
            }
            if (RecoveryRing)
            {
                AddItemSkill(2);
            }
            else
            {
                DelItemSkill(2);
            }
            if (MuscleRing)
            {
                MWAbil.MaxWeight += MWAbil.MaxWeight;
                MWAbil.MaxWearWeight += MWAbil.MaxWearWeight;
                MWAbil.MaxHandWeight += MWAbil.MaxHandWeight;
            }
            if (MNMoXieSuite > 0)
            {
                // 魔血
                if (MWAbil.MaxMP <= MNMoXieSuite)
                {
                    MNMoXieSuite = MWAbil.MaxMP - 1;
                }
                MWAbil.MaxMP -= (ushort)MNMoXieSuite;
                MWAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxHP + MNMoXieSuite);
            }
            if (MBopirit)
            {
                // Bonus DC Min +2,DC Max +5,A.Speed + 2
                MWAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.DC) + 2, HUtil32.HiWord(MWAbil.DC) + 2 + 5);
                HitSpeed += 2;
            }
            if (SmashSet)
            {
                // Attack Speed +1, DC1-3
                MWAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.DC) + 1, HUtil32.HiWord(MWAbil.DC) + 2 + 3);
                HitSpeed++;
            }
            if (HwanDevilSet)
            {
                // Hand Carrying Weight Increase +5, Bag Weight Limit Increase +20, +MC 1-2
                MWAbil.MaxHandWeight += 5;
                MWAbil.MaxWeight += 20;
                MWAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.MC) + 1, HUtil32.HiWord(MWAbil.MC) + 2 + 2);
            }
            if (PuritySet)
            {
                // Holy +3, Sc 1-2
                _mAddAbil.btUndead = (byte)(_mAddAbil.btUndead + -3);
                MWAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(MWAbil.SC) + 1, HUtil32.HiWord(MWAbil.SC) + 2 + 2);
            }
            if (MundaneSet)
            {
                // Bonus of Hp+50
                MWAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxHP + 50);
            }
            if (NokChiSet)
            {
                // Bonus of Mp+50
                MWAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxMP + 50);
            }
            if (TaoBuSet)
            {
                // Bonus of Hp+30, Mp+30
                MWAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxHP + 30);
                MWAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxMP + 30);
            }
            if (FiveStringSet)
            {
                // Bonus of Hp +30%, Ac+2
                MWAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, MWAbil.MaxHP / 100 * 30);
                MBtHitPoint += 2;
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
            }
            if (Race >= Grobal2.RC_ANIMAL)
            {
                MonsterRecalcAbilitys();
            }
            // 限制最高属性
            MWAbil.AC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(MWAbil.AC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(MWAbil.AC)));
            MWAbil.MAC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(MWAbil.MAC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(MWAbil.MAC)));
            MWAbil.DC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(MWAbil.DC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(MWAbil.DC)));
            MWAbil.MC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(MWAbil.MC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(MWAbil.MC)));
            MWAbil.SC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(MWAbil.SC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(MWAbil.SC)));
            if (M2Share.Config.boHungerSystem && M2Share.Config.boHungerDecPower)
            {
                if (HUtil32.RangeInDefined(MNHungerStatus, 0, 999))
                {
                    MWAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.DC) * 0.2), HUtil32.Round(HUtil32.HiWord(MWAbil.DC) * 0.2));
                    MWAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.MC) * 0.2), HUtil32.Round(HUtil32.HiWord(MWAbil.MC) * 0.2));
                    MWAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.SC) * 0.2), HUtil32.Round(HUtil32.HiWord(MWAbil.SC) * 0.2));
                }
                else if (HUtil32.RangeInDefined(MNHungerStatus, 1000, 1999))
                {
                    MWAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.DC) * 0.4), HUtil32.Round(HUtil32.HiWord(MWAbil.DC) * 0.4));
                    MWAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.MC) * 0.4), HUtil32.Round(HUtil32.HiWord(MWAbil.MC) * 0.4));
                    MWAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.SC) * 0.4), HUtil32.Round(HUtil32.HiWord(MWAbil.SC) * 0.4));
                }
                else if (HUtil32.RangeInDefined(MNHungerStatus, 2000, 2999))
                {
                    MWAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.DC) * 0.6), HUtil32.Round(HUtil32.HiWord(MWAbil.DC) * 0.6));
                    MWAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.MC) * 0.6), HUtil32.Round(HUtil32.HiWord(MWAbil.MC) * 0.6));
                    MWAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.SC) * 0.6), HUtil32.Round(HUtil32.HiWord(MWAbil.SC) * 0.6));
                }
                else if (HUtil32.RangeInDefined(MNHungerStatus, 3000, 3000))
                {
                    MWAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.DC) * 0.9), HUtil32.Round(HUtil32.HiWord(MWAbil.DC) * 0.9));
                    MWAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.MC) * 0.9), HUtil32.Round(HUtil32.HiWord(MWAbil.MC) * 0.9));
                    MWAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(MWAbil.SC) * 0.9), HUtil32.Round(HUtil32.HiWord(MWAbil.SC) * 0.9));
                }
            }
        }
    }
}
