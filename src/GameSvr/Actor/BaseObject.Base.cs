using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Npc;
using GameSvr.Player;
using GameSvr.RobotPlay;
using System;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        public virtual void Run()
        {
            ProcessMessage ProcessMsg = null;
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
                    Abil.HP = Abil.MaxHP;
                    Abil.MP = Abil.MaxMP;
                }
                int dwC = (HUtil32.GetTickCount() - MDwHpmpTick) / 20;
                MDwHpmpTick = HUtil32.GetTickCount();
                HealthTick += dwC;
                SpellTick += dwC;
                if (!Death)
                {
                    ushort n18;
                    if ((Abil.HP < Abil.MaxHP) && (HealthTick >= M2Share.Config.HealthFillTime))
                    {
                        n18 = (ushort)((Abil.MaxHP / 75) + 1);
                        if ((Abil.HP + n18) < Abil.MaxHP)
                        {
                            Abil.HP += n18;
                        }
                        else
                        {
                            Abil.HP = Abil.MaxHP;
                        }
                        HealthSpellChanged();
                    }
                    if ((Abil.MP < Abil.MaxMP) && (SpellTick >= M2Share.Config.SpellFillTime))
                    {
                        n18 = (ushort)((Abil.MaxMP / 18) + 1);
                        if ((Abil.MP + n18) < Abil.MaxMP)
                        {
                            Abil.MP += n18;
                        }
                        else
                        {
                            Abil.MP = Abil.MaxMP;
                        }
                        HealthSpellChanged();
                    }
                    if (Abil.HP == 0)
                    {
                        if (((LastHiter == null) || !LastHiter.UnRevival) && Revival && ((HUtil32.GetTickCount() - RevivalTick) > M2Share.Config.RevivalTime))// 60 * 1000
                        {
                            RevivalTick = HUtil32.GetTickCount();
                            ItemDamageRevivalRing();
                            Abil.HP = Abil.MaxHP;
                            HealthSpellChanged();
                            SysMsg(M2Share.g_sRevivalRecoverMsg, MsgColor.Green, MsgType.Hint);
                        }
                        if (Abil.HP == 0)
                        {
                            Die();
                        }
                    }
                    if (HealthTick >= M2Share.Config.HealthFillTime)
                    {
                        HealthTick = 0;
                    }
                    if (SpellTick >= M2Share.Config.SpellFillTime)
                    {
                        SpellTick = 0;
                    }
                }
                else
                {
                    if (CanReAlive && MonGen != null)
                    {
                        var dwMakeGhostTime = HUtil32._MAX(10 * 1000, M2Share.WorldEngine.GetMonstersZenTime(MonGen.ZenTime) - 20 * 1000);
                        if (dwMakeGhostTime > M2Share.Config.MakeGhostTime)
                        {
                            dwMakeGhostTime = M2Share.Config.MakeGhostTime;
                        }
                        if (HUtil32.GetTickCount() - DeathTick > dwMakeGhostTime)
                        {
                            MakeGhost();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - DeathTick) > M2Share.Config.MakeGhostTime)// 3 * 60 * 1000
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
                if (!Death && ((IncSpell > 0) || (IncHealth > 0) || (IncHealing > 0)))
                {
                    int dwInChsTime = 600 - HUtil32._MIN(400, Abil.Level * 10);
                    if (((HUtil32.GetTickCount() - IncHealthSpellTick) >= dwInChsTime) && !Death)
                    {
                        int dwC = HUtil32._MIN(200, HUtil32.GetTickCount() - IncHealthSpellTick - dwInChsTime);
                        IncHealthSpellTick = HUtil32.GetTickCount() + dwC;
                        if ((IncSpell > 0) || (IncHealth > 0) || (PerHealing > 0))
                        {
                            if (PerHealth <= 0)
                            {
                                PerHealth = 1;
                            }
                            if (PerSpell <= 0)
                            {
                                PerSpell = 1;
                            }
                            if (PerHealing <= 0)
                            {
                                PerHealing = 1;
                            }
                            int nHP;
                            if (IncHealth < PerHealth)
                            {
                                nHP = IncHealth;
                                IncHealth = 0;
                            }
                            else
                            {
                                nHP = PerHealth;
                                IncHealth -= PerHealth;
                            }
                            int nMP;
                            if (IncSpell < PerSpell)
                            {
                                nMP = IncSpell;
                                IncSpell = 0;
                            }
                            else
                            {
                                nMP = PerSpell;
                                IncSpell -= PerSpell;
                            }
                            if (IncHealing < PerHealing)
                            {
                                nHP += IncHealing;
                                IncHealing = 0;
                            }
                            else
                            {
                                nHP += PerHealing;
                                IncHealing -= PerHealing;
                            }
                            PerHealth = Abil.Level / 10 + 5;
                            PerSpell = Abil.Level / 10 + 5;
                            PerHealing = 5;
                            IncHealthSpell(nHP, nMP);
                            if (Abil.HP == Abil.MaxHP)
                            {
                                IncHealth = 0;
                                IncHealing = 0;
                            }
                            if (Abil.MP == Abil.MaxMP)
                            {
                                IncSpell = 0;
                            }
                        }
                    }
                }
                else
                {
                    IncHealthSpellTick = HUtil32.GetTickCount();
                }
                if ((HealthTick < -M2Share.Config.HealthFillTime) && (Abil.HP > 1))
                {
                    Abil.HP -= 1;
                    HealthTick += M2Share.Config.HealthFillTime;
                    HealthSpellChanged();
                }
                // 检查HP/MP值是否大于最大值，大于则降低到正常大小
                bool boNeedRecalc = false;
                if (Abil.HP > Abil.MaxHP)
                {
                    boNeedRecalc = true;
                    Abil.HP = (ushort)(Abil.MaxHP - 1);
                }
                if (Abil.MP > Abil.MaxMP)
                {
                    boNeedRecalc = true;
                    Abil.MP = (ushort)(Abil.MaxMP - 1);
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
                    if (!Death && Race == Grobal2.RC_PLAYOBJECT || Race == Grobal2.RC_PLAYCLONE)
                    {
                        int nCount;
                        int dCount;
                        int bCount;
                        Items.StdItem StdItem;
                        // 加HP
                        if ((IncHealth == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > M2Share.Config.HPStoneIntervalTime) && ((Abil.HP / Abil.MaxHP * 100) < M2Share.Config.HPStoneStartRate))
                        {
                            IncHpStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].Index);
                            if ((StdItem.StdMode == 7) && (StdItem.Shape == 1 || StdItem.Shape == 3))
                            {
                                nCount = UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.Config.HPStoneAddRate);
                                dCount = Abil.MaxHP - Abil.HP;
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
                                    UseItems[Grobal2.U_CHARM].Index = 0;
                                }
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > M2Share.Config.MpStoneIntervalTime) && ((Abil.MP / Abil.MaxMP * 100) < M2Share.Config.MPStoneStartRate))
                        {
                            IncMpStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].Index);
                            if ((StdItem.StdMode == 7) && (StdItem.Shape == 2 || StdItem.Shape == 3))
                            {
                                nCount = UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.Config.MPStoneAddRate);
                                dCount = Abil.MaxMP - Abil.MP;
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
                                    UseItems[Grobal2.U_CHARM].Index = 0;
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
                if (TargetCret != null)//修复弓箭卫士在人物进入房间后再出来，还会攻击人物(人物的攻击目标没清除)
                {
                    if (((HUtil32.GetTickCount() - TargetFocusTick) > 30000) || TargetCret.Death || TargetCret.Ghost || (TargetCret.Envir != Envir) || (Math.Abs(TargetCret.CurrX - CurrX) > 15) || (Math.Abs(TargetCret.CurrY - CurrY) > 15))
                    {
                        ClearTargetCreat(TargetCret);
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
                    NoItem = true;
                    // 宝宝变色
                    int nInteger;
                    if (AutoChangeColor && (HUtil32.GetTickCount() - AutoChangeColorTick > M2Share.Config.BBMonAutoChangeColorTime))
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
                        CharStatus = (int)(CharStatusEx | ((0x80000000 >> nInteger) | 0));
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
                        CharStatus = (int)(CharStatusEx | ((0x80000000 >> nInteger) | 0));
                        FixStatus = CharStatus;
                        StatusChanged();
                    }
                    // 宝宝在主人死亡后死亡处理
                    if (Master.Death && ((HUtil32.GetTickCount() - Master.DeathTick) > 1000))
                    {
                        if (M2Share.Config.MasterDieMutiny && (Master.LastHiter != null) && (M2Share.RandomNumber.Random(M2Share.Config.MasterDieMutinyRate) == 0))
                        {
                            Master = null;
                            SlaveExpLevel = (byte)M2Share.Config.SlaveColor.Length;
                            RecalcAbilitys();
                            Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC) * M2Share.Config.MasterDieMutinyPower, HUtil32.HiWord(Abil.DC) * M2Share.Config.MasterDieMutinyPower);
                            WalkSpeed = WalkSpeed / M2Share.Config.MasterDieMutinySpeed;
                            RefNameColor();
                            RefShowName();
                        }
                        else
                        {
                            Abil.HP = 0;
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
                if (HolySeize && ((HUtil32.GetTickCount() - HolySeizeTick) > HolySeizeInterval))
                {
                    BreakHolySeizeMode();
                }
                if (CrazyMode && ((HUtil32.GetTickCount() - CrazyModeTick) > CrazyModeInterval))
                {
                    BreakCrazyMode();
                }
                if (ShowHp && ((HUtil32.GetTickCount() - ShowHpTick) > ShowHpInterval))
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
                if ((HUtil32.GetTickCount() - DecPkPointTick) > M2Share.Config.DecPkPointTime)// 120000
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
                            Abil.HP = (ushort)(Abil.HP / 10);
                            RefShowName();
                        }
                        if (MasterTick != 0)
                        {
                            if ((HUtil32.GetTickCount() - MasterTick) > 12 * 60 * 60 * 1000)
                            {
                                Abil.HP = 0;
                            }
                        }
                    }
                }

                if ((HUtil32.GetTickCount() - VerifyTick) > 30 * 1000)
                {
                    VerifyTick = HUtil32.GetTickCount();
                    // 清组队已死亡成员
                    if (GroupOwner != null)
                    {
                        if (GroupOwner.Death || GroupOwner.Ghost)
                        {
                            GroupOwner = null;
                        }
                    }

                    if (GroupOwner == this)
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
                    if (!DenyRefStatus)
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
                for (var i = 0; i < StatusArrTick.Length; i++)
                {
                    if ((StatusArr[i] > 0) && (StatusArr[i] < 60000))
                    {
                        if ((HUtil32.GetTickCount() - StatusArrTick[i]) > 1000)
                        {
                            StatusArr[i] -= 1;
                            StatusArrTick[i] += 1000;
                            if (StatusArr[i] == 0)
                            {
                                boChg = true;
                                switch (i)
                                {
                                    case Grobal2.STATE_DEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case Grobal2.STATE_MAGDEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("魔法防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case Grobal2.STATE_BUBBLEDEFENCEUP:
                                        AbilMagBubbleDefence = false;
                                        break;
                                    case Grobal2.STATE_TRANSPARENT:
                                        HideMode = false;
                                        break;
                                }
                            }
                            else if (StatusArr[i] == 10)
                            {
                                if (i == Grobal2.STATE_DEFENCEUP)
                                {
                                    SysMsg("防御力" + StatusArr[i] + "秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                }
                                if (i == Grobal2.STATE_MAGDEFENCEUP)
                                {
                                    SysMsg("魔法防御力" + StatusArr[i] + "秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                }
                            }
                        }
                    }
                }
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
                if ((HUtil32.GetTickCount() - PoisoningTick) > M2Share.Config.PosionDecHealthTime)
                {
                    PoisoningTick = HUtil32.GetTickCount();
                    if (StatusArr[Grobal2.POISON_DECHEALTH] > 0)
                    {
                        if (Animal)
                        {
                            MeatQuality -= 1000;
                        }
                        DamageHealth(GreenPoisoningPoint + 1);
                        HealthTick = 0;
                        SpellTick = 0;
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
            if (SuperManItem)
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
                    if (MonGen.ActiveCount > 0)
                    {
                        MonGen.ActiveCount--;
                    }
                    MonGen = null;
                }
            }

            IncSpell = 0;
            IncHealth = 0;
            IncHealing = 0;
            KillFunc();
            try
            {
                if (Race != Grobal2.RC_PLAYOBJECT && LastHiter != null)
                {
                    if (M2Share.Config.MonSayMsg)
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
                            tExp = ExpHitter.CalcGetExp(Abil.Level, FightExp);
                            if (!M2Share.Config.VentureServer)
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
                                if (ExpHitter.GroupOwner != null)
                                {
                                    for (var i = 0; i < ExpHitter.GroupOwner.GroupMembers.Count; i++)
                                    {
                                        PlayObject GroupHuman = ExpHitter.GroupOwner.GroupMembers[i];
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
                                tExp = ExpHitter.Master.CalcGetExp(Abil.Level, FightExp);
                                if (!M2Share.Config.VentureServer)
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
                            tExp = LastHiter.CalcGetExp(Abil.Level, FightExp);
                            if (!M2Share.Config.VentureServer)
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
                if (M2Share.Config.MonSayMsg && Race == Grobal2.RC_PLAYOBJECT && LastHiter != null)
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
                if (!M2Share.Config.VentureServer && !Envir.Flag.boFightZone && !Envir.Flag.boFight3Zone)
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
                    if (Castle != null && Castle.UnderWar || InFreePkArea)
                    {
                        guildwarkill = true;
                    }
                    if (!guildwarkill)
                    {
                        if ((M2Share.Config.IsKillHumanWinLevel || M2Share.Config.IsKillHumanWinExp || Envir.Flag.boPKWINLEVEL || Envir.Flag.boPKWINEXP) && LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            (this as PlayObject).PKDie(LastHiter as PlayObject);
                        }
                        else
                        {
                            if (!LastHiter.IsGoodKilling(this))
                            {
                                LastHiter.IncPkPoint(M2Share.Config.KillHumanAddPKPoint);
                                LastHiter.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                                SysMsg(Format(M2Share.g_sYouKilledByMsg, LastHiter.CharName), MsgColor.Red, MsgType.Hint);
                                LastHiter.AddBodyLuck(-M2Share.Config.KillHumanDecLuckPoint);
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
                                LastHiter.SysMsg(M2Share.g_sYouprotectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                            }
                        }
                        // 检查攻击人是否用了着经验或等级装备
                        if (LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            if (LastHiter.PkDieLostExp > 0)
                            {
                                if (Abil.Exp >= LastHiter.PkDieLostExp)
                                {
                                    Abil.Exp -= (short)LastHiter.PkDieLostExp;
                                }
                                else
                                {
                                    Abil.Exp = 0;
                                }
                            }
                            if (LastHiter.PkDieLostLevel > 0)
                            {
                                if (Abil.Level >= LastHiter.PkDieLostLevel)
                                {
                                    Abil.Level -= (byte)LastHiter.PkDieLostLevel;
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
                        if (Master == null && (!NoItem || !Envir.Flag.boNODROPITEM))
                        {
                            ScatterBagItems(AttackBaseObject);
                        }
                        if (Race >= Grobal2.RC_ANIMAL && Master == null && (!NoItem || !Envir.Flag.boNODROPITEM))
                        {
                            ScatterGolds(AttackBaseObject);
                        }
                    }
                    else
                    {
                        if (!NoItem || !Envir.Flag.boNODROPITEM)//允许设置 m_boNoItem 后人物死亡不掉物品
                        {
                            if (AttackBaseObject != null)
                            {
                                if (M2Share.Config.KillByHumanDropUseItem && AttackBaseObject.Race == Grobal2.RC_PLAYOBJECT || M2Share.Config.KillByMonstDropUseItem && AttackBaseObject.Race != Grobal2.RC_PLAYOBJECT)
                                {
                                    DropUseItems(null);
                                }
                            }
                            else
                            {
                                DropUseItems(null);
                            }
                            if (M2Share.Config.DieScatterBag)
                            {
                                ScatterBagItems(null);
                            }
                            if (M2Share.Config.DieDropGold)
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
                            M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, Envir, CurrX, CurrY, 1000, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, "- " + tStr);
                        }
                    }
                }
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    if (GroupOwner != null)
                    {
                        GroupOwner.DelMember(this);// 人物死亡立即退组，以防止组队刷经验
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

        /// <summary>
        /// 精灵死亡，彻底释放对象
        /// </summary>
        public virtual void MakeGhost()
        {
            if (CanReAlive)
            {
                Invisible = true;
            }
            else
            {
                Ghost = true;
            }
            GhostTick = HUtil32.GetTickCount();
            DisappearA();
        }

        /// <summary>
        /// 散落包裹物品
        /// </summary>
        protected virtual void ScatterBagItems(BaseObject ItemOfCreat)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::ScatterBagItems";
            try
            {
                var dropWide = HUtil32._MIN(M2Share.Config.DropItemRage, 7);
                if ((Race == Grobal2.RC_PLAYCLONE) && (Master != null))
                {
                    return;
                }
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    var UserItem = ItemList[i];
                    var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
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
                    if (DropItemDown(UserItem, dropWide, true, ItemOfCreat, this))
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
            IList<DeleteItem> DropItemList = null;
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
                        StdItem = M2Share.WorldEngine.GetStdItem(UseItems[nC].Index);
                        if (StdItem != null)
                        {
                            if ((StdItem.ItemDesc & 8) != 0)
                            {
                                if (DropItemList == null)
                                {
                                    DropItemList = new List<DeleteItem>();
                                }
                                DropItemList.Add(new DeleteItem()
                                {
                                    MakeIndex = UseItems[nC].MakeIndex
                                });
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog("16" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + StdItem.Name + "\t" + UseItems[nC].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                                }
                                UseItems[nC].Index = 0;
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
                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[nC].Index);
                            if (StdItem != null)
                            {
                                if ((StdItem.ItemDesc & 10) == 0)
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (DropItemList == null)
                                        {
                                            DropItemList = new List<DeleteItem>();
                                        }
                                        DropItemList.Add(new DeleteItem()
                                        {
                                            ItemName = M2Share.WorldEngine.GetStdItemName(UseItems[nC].Index),
                                            MakeIndex = UseItems[nC].MakeIndex
                                        });
                                    }
                                    UseItems[nC].Index = 0;
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

        protected void ClearTargetCreat(BaseObject baseObject)
        {
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    if (VisibleActors[i].BaseObject == baseObject)
                    {
                        VisibleActors.RemoveAt(i);
                        DelTargetCreat();
                        break;
                    }
                }
            }
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

        protected virtual bool Operate(ProcessMessage ProcessMsg)
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
                            SendRefMsg(Grobal2.RM_STRUCK_MAG, (short)nDamage, Abil.HP, Abil.MaxHP, ProcessMsg.BaseObject, "");
                            TargetBaseObject = M2Share.ActorMgr.Get(ProcessMsg.BaseObject);
                            if (M2Share.Config.MonDelHptoExp)
                            {
                                if (TargetBaseObject.Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    if ((TargetBaseObject as PlayObject).Abil.Level <= M2Share.Config.MonHptoExpLevel)
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
                                        if ((TargetBaseObject.Master as PlayObject).Abil.Level <= M2Share.Config.MonHptoExpLevel)
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
                                    MeatQuality -= (ushort)(nDamage * 1000);
                                }
                                SendMsg(this, Grobal2.RM_STRUCK, nDamage, Abil.HP, Abil.MaxHP, ProcessMsg.BaseObject, "");
                            }
                        }
                        if (FastParalysis)
                        {
                            StatusArr[Grobal2.POISON_STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_MAGHEALING:
                        if ((IncHealing + ProcessMsg.nParam1) < 300)
                        {
                            if (Race == Grobal2.RC_PLAYOBJECT)
                            {
                                IncHealing += ProcessMsg.nParam1;
                                PerHealing = 5;
                            }
                            else
                            {
                                IncHealing += ProcessMsg.nParam1;
                                PerHealing = 5;
                            }
                        }
                        else
                        {
                            IncHealing = 300;
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
                            StatusArr[Grobal2.POISON_STONE] = 1;
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
        public virtual void RecalcAbilitysOld()
        {
            bool[] boRecallSuite = new bool[4] { false, false, false, false };
            bool[] boMoXieSuite = new bool[3] { false, false, false };
            bool[] boSpirit = new bool[4] { false, false, false, false };
            AddAbil = new AddAbility();
            ushort wOldHP = Abil.HP;
            ushort wOldMP = Abil.MP;
            WAbil = Abil;
            WAbil.HP = wOldHP;
            WAbil.MP = wOldMP;
            WAbil.Weight = 0;
            WAbil.WearWeight = 0;
            WAbil.HandWeight = 0;
            AntiPoison = 0;
            PoisonRecover = 0;
            HealthRecover = 0;
            SpellRecover = 0;
            AntiMagic = 1;
            Luck = 0;
            HitSpeed = 0;
            BoExpItem = false;
            ExpItem = 0;
            BoPowerItem = false;
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
            SuperManItem = false;
            GuildMove = false;
            UnParalysis = false;
            BoExpItem = false;
            BoPowerItem = false;
            NoDropItem = false;
            NoDropUseItem = false;
            MBopirit = false;
            HorseType = 0;
            DressEffType = 0;
            AutoAddHpmpMode = 0;
            // 气血石
            MoXieSuite = 0;
            SuckupEnemyHealth = 0;
            SuckupEnemyHealthRate = 0;
            bool boHongMoSuite1 = false;
            bool boHongMoSuite2 = false;
            bool boHongMoSuite3 = false;
            RecallSuite = false;
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
            PkDieLostExp = 0;
            PkDieLostLevel = 0;
            if (Race == Grobal2.RC_PLAYOBJECT || Race == Grobal2.RC_PLAYCLONE)
            {
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] == null || (UseItems[i].Index <= 0) || (UseItems[i].Dura <= 0))
                    {
                        continue;
                    }
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (stdItem == null)
                    {
                        continue;
                    }
                    ApplyItemParameters(UseItems[i], stdItem, ref AddAbil);
                    ApplyItemParametersEx(UseItems[i], ref WAbil);
                    if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND) || (i == Grobal2.U_DRESS))
                    {
                        if (i == Grobal2.U_DRESS)
                        {
                            Abil.WearWeight += stdItem.Weight;
                        }
                        else
                        {
                            Abil.HandWeight += stdItem.Weight;
                        }
                        switch (stdItem.AniCount)
                        {
                            case 120:
                                FastTrain = true;
                                break;
                            case 121:
                                ProbeNecklace = true;
                                break;
                            case 145:
                                GuildMove = true;
                                break;
                            case 111:
                                StatusArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                                HideMode = true;
                                break;
                            case 112:
                                Teleport = true;
                                break;
                            case 113:
                                Paralysis = true;
                                break;
                            case 114:
                                Revival = true;
                                break;
                            case 115:
                                FlameRing = true;
                                break;
                            case 116:
                                RecoveryRing = true;
                                break;
                            case 117:
                                AngryRing = true;
                                break;
                            case 118:
                                MagicShield = true;
                                break;
                            case 119:
                                MuscleRing = true;
                                break;
                            case 135:
                                boMoXieSuite[0] = true;
                                MoXieSuite += stdItem.Weight / 10;
                                break;
                            case 138:
                                SuckupEnemyHealthRate += stdItem.Weight;
                                break;
                            case 139:
                                UnParalysis = true;
                                break;
                            case 140:
                                SuperManItem = true;
                                break;
                            case 141:
                                BoExpItem = true;
                                ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.ItemExpRate);
                                break;
                            case 142:
                                BoPowerItem = true;
                                PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.ItemPowerRate);
                                break;
                            case 182:
                                BoExpItem = true;
                                ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.ItemExpRate);
                                break;
                            case 183:
                                BoPowerItem = true;
                                PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.ItemPowerRate);
                                break;
                            case 143:
                                UnMagicShield = true;
                                break;
                            case 144:
                                UnRevival = true;
                                break;
                            case 170:
                                AngryRing = true;
                                break;
                            case 171:
                                NoDropItem = true;
                                break;
                            case 172:
                                NoDropUseItem = true;
                                break;
                            case 150:// 麻痹护身
                                Paralysis = true;
                                MagicShield = true;
                                break;
                            case 151:// 麻痹火球
                                Paralysis = true;
                                FlameRing = true;
                                break;
                            case 152:// 麻痹防御
                                Paralysis = true;
                                RecoveryRing = true;
                                break;
                            case 153:// 麻痹负载
                                Paralysis = true;
                                MuscleRing = true;
                                break;
                        }
                        if (stdItem.Shape == 154)
                        {
                            // 护身火球
                            MagicShield = true;
                            FlameRing = true;
                        }
                        switch (stdItem.AniCount)
                        {
                            case 155:// 护身防御
                                MagicShield = true;
                                RecoveryRing = true;
                                break;
                            case 156:// 护身负载
                                MagicShield = true;
                                MuscleRing = true;
                                break;
                            case 157:// 传送麻痹
                                Teleport = true;
                                Paralysis = true;
                                break;
                            case 158:// 传送护身
                                Teleport = true;
                                MagicShield = true;
                                break;
                            case 159:// 传送探测
                                Teleport = true;
                                ProbeNecklace = true;
                                break;
                            case 160:// 传送复活
                                Teleport = true;
                                Revival = true;
                                break;
                            case 161:// 麻痹复活
                                Paralysis = true;
                                Revival = true;
                                break;
                            case 162:// 护身复活
                                MagicShield = true;
                                Revival = true;
                                break;
                            case 180:// PK 死亡掉经验
                                PkDieLostExp = stdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                                break;
                            case 181:// PK 死亡掉等级
                                PkDieLostLevel = stdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                                break;
                        }
                    }
                    else
                    {
                        Abil.WearWeight += stdItem.Weight;
                    }
                    Abil.Weight += stdItem.Weight;
                    switch (i)
                    {
                        case Grobal2.U_WEAPON:
                            {
                                if ((stdItem.SpecialPwr - 1 - 10) < 0)
                                {
                                    AddAbil.WeaponStrong = (byte)stdItem.SpecialPwr;// 强度+
                                }
                                if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))  // -1 to -50
                                {
                                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + -stdItem.SpecialPwr);// Holy+
                                }
                                if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))// -51 to -100
                                {
                                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (stdItem.SpecialPwr + 50));// Holy-
                                }
                                continue;
                            }
                        case Grobal2.U_RIGHTHAND:
                            {
                                if (stdItem.Shape >= 1 && stdItem.Shape <= 50)
                                {
                                    DressEffType = stdItem.Shape;
                                }
                                if (stdItem.Shape >= 51 && stdItem.Shape <= 100)
                                {
                                    HorseType = (byte)(stdItem.Shape - 50);
                                }
                                continue;
                            }
                        case Grobal2.U_DRESS:
                            {
                                if (UseItems[i].Desc[5] > 0)
                                {
                                    DressEffType = UseItems[i].Desc[5];
                                }
                                if (stdItem.AniCount > 0)
                                {
                                    DressEffType = stdItem.AniCount;
                                }
                                if (stdItem.Light > 0)
                                {
                                    Light = 3;
                                }
                                continue;
                            }
                    }
                    switch (stdItem.Shape)
                    {
                        case 139:
                            UnParalysis = true;
                            break;
                        case 140:
                            SuperManItem = true;
                            break;
                        case 141:
                            BoExpItem = true;
                            ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.ItemExpRate);
                            break;
                        case 142:
                            BoPowerItem = true;
                            PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.ItemPowerRate);
                            break;
                        case 182:
                            BoExpItem = true;
                            ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.ItemExpRate);
                            break;
                        case 183:
                            BoPowerItem = true;
                            PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.ItemPowerRate);
                            break;
                        case 143:
                            UnMagicShield = true;
                            break;
                        case 144:
                            UnRevival = true;
                            break;
                        case 170:
                            AngryRing = true;
                            break;
                        case 171:
                            NoDropItem = true;
                            break;
                        case 172:
                            NoDropUseItem = true;
                            break;
                        case 150:// 麻痹护身
                            Paralysis = true;
                            MagicShield = true;
                            break;
                        case 151:// 麻痹火球
                            Paralysis = true;
                            FlameRing = true;
                            break;
                        case 152:// 麻痹防御
                            Paralysis = true;
                            RecoveryRing = true;
                            break;
                        case 153:// 麻痹负载
                            Paralysis = true;
                            MuscleRing = true;
                            break;
                        case 154:// 护身火球
                            MagicShield = true;
                            FlameRing = true;
                            break;
                        case 155:// 护身防御
                            MagicShield = true;
                            RecoveryRing = true;
                            break;
                        case 156:// 护身负载
                            MagicShield = true;
                            MuscleRing = true;
                            break;
                        case 157:// 传送麻痹
                            Teleport = true;
                            Paralysis = true;
                            break;
                        case 158:// 传送护身
                            Teleport = true;
                            MagicShield = true;
                            break;
                        case 159:// 传送探测
                            Teleport = true;
                            ProbeNecklace = true;
                            break;
                        case 160:// 传送复活
                            Teleport = true;
                            Revival = true;
                            break;
                        case 161:// 麻痹复活
                            Paralysis = true;
                            Revival = true;
                            break;
                        case 162:// 护身复活
                            MagicShield = true;
                            Revival = true;
                            break;
                        case 180:// PK 死亡掉经验
                            PkDieLostExp = stdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                            break;
                        case 181:// PK 死亡掉等级
                            PkDieLostLevel = stdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                            break;
                        case 120:
                            FastTrain = true;
                            break;
                        case 121:
                            ProbeNecklace = true;
                            break;
                        case 123:
                            boRecallSuite[0] = true;
                            break;
                        case 145:
                            GuildMove = true;
                            break;
                        case 127:
                            boSpirit[0] = true;
                            break;
                        case 135:
                            boMoXieSuite[0] = true;
                            MoXieSuite += stdItem.AniCount;
                            break;
                        case 138:
                            boHongMoSuite1 = true;
                            SuckupEnemyHealthRate += stdItem.AniCount;
                            break;
                        case 200:
                            boSmash1 = true;
                            break;
                        case 203:
                            boHwanDevil1 = true;
                            break;
                        case 206:
                            boPurity1 = true;
                            break;
                        case 216:
                            boFiveString1 = true;
                            break;
                        case 111:
                            StatusArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                            HideMode = true;
                            break;
                        case 112:
                            Teleport = true;
                            break;
                        case 113:
                            Paralysis = true;
                            break;
                        case 114:
                            Revival = true;
                            break;
                        case 115:
                            FlameRing = true;
                            break;
                        case 116:
                            RecoveryRing = true;
                            break;
                        case 117:
                            AngryRing = true;
                            break;
                        case 118:
                            MagicShield = true;
                            break;
                        case 119:
                            MuscleRing = true;
                            break;
                        case 122:
                            boRecallSuite[1] = true;
                            break;
                        case 128:
                            boSpirit[1] = true;
                            break;
                        case 133:
                            boMoXieSuite[1] = true;
                            MoXieSuite += stdItem.AniCount;
                            break;
                        case 136:
                            boHongMoSuite2 = true;
                            SuckupEnemyHealthRate += stdItem.AniCount;
                            break;
                        case 201:
                            boSmash2 = true;
                            break;
                        case 204:
                            boHwanDevil2 = true;
                            break;
                        case 207:
                            boPurity2 = true;
                            break;
                        case 210:
                            boMundane1 = true;
                            break;
                        case 212:
                            boNokChi1 = true;
                            break;
                        case 214:
                            boTaoBu1 = true;
                            break;
                        case 217:
                            boFiveString2 = true;
                            break;
                    }
                    if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))
                    {
                        // -1 to -50
                        AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + -stdItem.SpecialPwr);
                        // Holy+
                    }
                    if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))
                    {
                        // -51 to -100
                        AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (stdItem.SpecialPwr + 50));
                        // Holy-
                    }
                    switch (stdItem.Shape)
                    {
                        case 124:
                            boRecallSuite[2] = true;
                            break;
                        case 126:
                            boSpirit[2] = true;
                            break;
                        case 145:
                            GuildMove = true;
                            break;
                        case 134:
                            boMoXieSuite[2] = true;
                            MoXieSuite += stdItem.AniCount;
                            break;
                        case 137:
                            boHongMoSuite3 = true;
                            SuckupEnemyHealthRate += stdItem.AniCount;
                            break;
                        case 202:
                            boSmash3 = true;
                            break;
                        case 205:
                            boHwanDevil3 = true;
                            break;
                        case 208:
                            boPurity3 = true;
                            break;
                        case 211:
                            boMundane2 = true;
                            break;
                        case 213:
                            boNokChi2 = true;
                            break;
                        case 215:
                            boTaoBu2 = true;
                            break;
                        case 218:
                            boFiveString3 = true;
                            break;
                        case 125:
                            boRecallSuite[3] = true;
                            break;
                        case 129:
                            boSpirit[3] = true;
                            break;
                    }
                }
                if (boRecallSuite[0] && boRecallSuite[1] && boRecallSuite[2] && boRecallSuite[3])
                {
                    RecallSuite = true;
                }
                if (boMoXieSuite[0] && boMoXieSuite[1] && boMoXieSuite[2])
                {
                    MoXieSuite += 50;
                }
                if (boHongMoSuite1 && boHongMoSuite2 && boHongMoSuite3)
                {
                    AddAbil.HIT += 2;
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
                Abil.Weight = RecalcBagWeight();
            }
            if (Transparent && (StatusArr[Grobal2.STATE_TRANSPARENT] > 0))
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
                    StatusArr[Grobal2.STATE_TRANSPARENT] = 0;
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                RecalcHitSpeed();//增加此行，只有类型为人物的角色才重新计算攻击敏捷
            }
            int nOldLight = Light;
            if ((UseItems[Grobal2.U_RIGHTHAND] != null) && (UseItems[Grobal2.U_RIGHTHAND].Index > 0) && (UseItems[Grobal2.U_RIGHTHAND].Dura > 0))
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
            SpeedPoint += (byte)AddAbil.SPEED;
            HitPoint += (byte)AddAbil.HIT;
            AntiPoison += (byte)AddAbil.AntiPoison;
            PoisonRecover += AddAbil.PoisonRecover;
            HealthRecover += AddAbil.HealthRecover;
            SpellRecover += AddAbil.SpellRecover;
            AntiMagic += AddAbil.AntiMagic;
            Luck += AddAbil.Luck;
            Luck -= AddAbil.UnLuck;
            HitSpeed = AddAbil.HitSpeed;
            //Abil.MaxWeight += AddAbil.Weight;
            //Abil.MaxWearWeight += (byte)AddAbil.WearWeight;
            //Abil.MaxHandWeight += (byte)AddAbil.HandWeight;
            Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + AddAbil.HP);
            Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + AddAbil.MP);
            Abil.AC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.AC) + HUtil32.LoWord(Abil.AC), HUtil32.HiWord(AddAbil.AC) + HUtil32.HiWord(Abil.AC));
            Abil.MAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.MAC) + HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(AddAbil.MAC) + HUtil32.HiWord(Abil.MAC));
            Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.DC) + HUtil32.LoWord(Abil.DC), HUtil32.HiWord(AddAbil.DC) + HUtil32.HiWord(Abil.DC));
            Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.MC) + HUtil32.LoWord(Abil.MC), HUtil32.HiWord(AddAbil.MC) + HUtil32.HiWord(Abil.MC));
            Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.SC) + HUtil32.LoWord(Abil.SC), HUtil32.HiWord(AddAbil.SC) + HUtil32.HiWord(Abil.SC));
            if (StatusArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                Abil.AC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.AC), HUtil32.HiWord(Abil.AC) + 2 + (Abil.Level / 7));
            }
            if (StatusArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                Abil.MAC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(Abil.MAC) + 2 + (Abil.Level / 7));
            }
            if (ExtraAbil[0] > 0)
            {
                Abil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.DC), HUtil32.HiWord(Abil.DC) + 2 + ExtraAbil[0]);
            }
            if (ExtraAbil[1] > 0)
            {
                Abil.MC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.MC), HUtil32.HiWord(Abil.MC) + 2 + ExtraAbil[1]);
            }
            if (ExtraAbil[2] > 0)
            {
                Abil.SC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.SC), HUtil32.HiWord(Abil.SC) + 2 + ExtraAbil[2]);
            }
            if (ExtraAbil[3] > 0)
            {
                HitSpeed += ExtraAbil[3];
            }
            if (ExtraAbil[4] > 0)
            {
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + ExtraAbil[4]);
            }
            if (ExtraAbil[5] > 0)
            {
                Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + ExtraAbil[5]);
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
                Abil.MaxWeight += Abil.MaxWeight;
                Abil.MaxWearWeight += Abil.MaxWearWeight;
                Abil.MaxHandWeight += Abil.MaxHandWeight;
            }
            if (MoXieSuite > 0)
            {
                // 魔血
                if (Abil.MaxMP <= MoXieSuite)
                {
                    MoXieSuite = Abil.MaxMP - 1;
                }
                Abil.MaxMP -= (ushort)MoXieSuite;
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + MoXieSuite);
            }
            if (MBopirit)
            {
                // Bonus DC Min +2,DC Max +5,A.Speed + 2
                Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC) + 2, HUtil32.HiWord(Abil.DC) + 2 + 5);
                HitSpeed += 2;
            }
            if (SmashSet)
            {
                // Attack Speed +1, DC1-3
                Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC) + 1, HUtil32.HiWord(Abil.DC) + 2 + 3);
                HitSpeed++;
            }
            if (HwanDevilSet)
            {
                // Hand Carrying Weight Increase +5, Bag Weight Limit Increase +20, +MC 1-2
                Abil.MaxHandWeight += 5;
                Abil.MaxWeight += 20;
                Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(Abil.MC) + 1, HUtil32.HiWord(Abil.MC) + 2 + 2);
            }
            if (PuritySet)
            {
                // Holy +3, Sc 1-2
                AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + -3);
                Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(Abil.SC) + 1, HUtil32.HiWord(Abil.SC) + 2 + 2);
            }
            if (MundaneSet)
            {
                // Bonus of Hp+50
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + 50);
            }
            if (NokChiSet)
            {
                // Bonus of Mp+50
                Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + 50);
            }
            if (TaoBuSet)
            {
                // Bonus of Hp+30, Mp+30
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + 30);
                Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + 30);
            }
            if (FiveStringSet)
            {
                // Bonus of Hp +30%, Ac+2
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP / 100 * 30);
                HitPoint += 2;
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
            Abil.AC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.AC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.AC)));
            Abil.MAC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.MAC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.MAC)));
            Abil.DC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.DC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.DC)));
            Abil.MC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.MC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.MC)));
            Abil.SC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.SC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.SC)));
            if (M2Share.Config.HungerSystem && M2Share.Config.HungerDecPower)
            {
                if (HUtil32.RangeInDefined(HungerStatus, 0, 999))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.2), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.2));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.2), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.2));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.2), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.2));
                }
                else if (HUtil32.RangeInDefined(HungerStatus, 1000, 1999))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.4), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.4));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.4), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.4));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.4), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.4));
                }
                else if (HUtil32.RangeInDefined(HungerStatus, 2000, 2999))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.6), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.6));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.6), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.6));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.6), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.6));
                }
                else if (HUtil32.RangeInDefined(HungerStatus, 3000, 3000))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.9), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.9));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.9), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.9));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.9), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.9));
                }
            }
        }

        public virtual void RecalcAbilitys()
        {
            int i;
            int oldlight;
            int n;
            int m;
            bool[] cghi = new bool[3 + 1];
            StdItem pstd;
            Ability temp;
            bool fastmoveflag;
            bool oldhmode;
            bool mh_ring;
            bool mh_bracelet;
            bool mh_necklace;
            bool sh_ring;
            bool sh_bracelet;
            bool sh_necklace;
            // 2003/01/15 技飘 酒捞袍 眠啊...技符悸, 踌秒悸, 档何悸
            bool hp_ring;
            bool hp_bracelet;
            bool mp_ring;
            bool mp_bracelet;
            bool hpmp_ring;
            bool hpmp_bracelet;
            // 2003/02/11 技飘 酒捞袍 眠啊...坷泅悸, 檬去悸
            bool hpp_necklace;
            bool hpp_bracelet;
            bool hpp_ring;
            bool cho_weapon;
            bool cho_necklace;
            bool cho_ring;
            bool cho_helmet;
            bool cho_bracelet;
            // 2003/03/04 技飘 酒捞袍 眠啊...颇尖悸, 券付籍悸, 康飞苛悸
            bool pset_necklace;
            bool pset_bracelet;
            bool pset_ring;
            bool hset_necklace;
            bool hset_bracelet;
            bool hset_ring;
            bool yset_necklace;
            bool yset_bracelet;
            bool yset_ring;
            bool dset_wingdress;
            // 2003/11/17 力炼 傈侩 技飘 酒捞袍 眠啊
            // 焕促蓖悸,国饭悸,归陛悸,楷苛悸,全苛悸,碍拳归陛悸,碍拳楷苛悸,碍拳全苛悸.
            bool boneset_weapon;
            bool boneset_helmet;
            bool boneset_dress;
            bool bugset_necklace;
            bool bugset_ring;
            bool bugset_bracelet;
            bool ptset_belt;
            bool ptset_boots;
            bool ptset_necklace;
            bool ptset_bracelet;
            bool ptset_ring;
            bool ksset_belt;
            bool ksset_boots;
            bool ksset_necklace;
            bool ksset_bracelet;
            bool ksset_ring;
            bool rubyset_belt;
            bool rubyset_boots;
            bool rubyset_necklace;
            bool rubyset_bracelet;
            bool rubyset_ring;
            bool strong_ptset_belt;
            bool strong_ptset_boots;
            bool strong_ptset_necklace;
            bool strong_ptset_bracelet;
            bool strong_ptset_ring;
            bool strong_ksset_belt;
            bool strong_ksset_boots;
            bool strong_ksset_necklace;
            bool strong_ksset_bracelet;
            bool strong_ksset_ring;
            bool strong_rubyset_belt;
            bool strong_rubyset_boots;
            bool strong_rubyset_necklace;
            bool strong_rubyset_bracelet;
            bool strong_rubyset_ring;
            bool dragonset_ring_left;
            bool dragonset_ring_right;
            bool dragonset_bracelet_left;
            bool dragonset_bracelet_right;
            bool dragonset_necklace;
            bool dragonset_dress;
            bool dragonset_helmet;
            bool dragonset_weapon;
            bool dragonset_boots;
            bool dragonset_belt;
            AddAbil = new AddAbility();//FillChar(AddAbil, sizeof(TAddAbility), 0);
            temp = WAbil;
            WAbil = Abil;
            WAbil.HP = temp.HP;
            WAbil.MP = temp.MP;
            WAbil.Weight = 0;
            WAbil.WearWeight = 0;
            WAbil.HandWeight = 0;
            AntiPoison = 0;
            PoisonRecover = 0;
            HealthRecover = 0;
            SpellRecover = 0;
            AntiMagic = 1;
            Luck = 0;
            HitSpeed = 0;
            oldhmode = HideMode;
            HideMode = false;
            Teleport = false;
            Paralysis = false;
            Revival = false;
            FlameRing = false;
            RecoveryRing = false;
            AngryRing = false;
            MagicShield = false;
            MuscleRing = false;
            FastTrain = false;
            ProbeNecklace = false;
            MoXieSuite = 0;
            mh_ring = false;
            mh_bracelet = false;
            mh_necklace = false;
            SuckupEnemyHealthRate = 0;
            SuckupEnemyHealth = 0;
            sh_ring = false;
            sh_bracelet = false;
            sh_necklace = false;
            hp_ring = false;
            hp_bracelet = false;
            mp_ring = false;
            mp_bracelet = false;
            hpmp_ring = false;
            hpmp_bracelet = false;
            hpp_necklace = false;
            hpp_bracelet = false;
            hpp_ring = false;
            cho_weapon = false;
            cho_necklace = false;
            cho_ring = false;
            cho_helmet = false;
            cho_bracelet = false;
            pset_necklace = false;
            pset_bracelet = false;
            pset_ring = false;
            hset_necklace = false;
            hset_bracelet = false;
            hset_ring = false;
            yset_necklace = false;
            yset_bracelet = false;
            yset_ring = false;
            boneset_weapon = false;
            boneset_helmet = false;
            boneset_dress = false;
            bugset_necklace = false;
            bugset_ring = false;
            bugset_bracelet = false;
            ptset_belt = false;
            ptset_boots = false;
            ptset_necklace = false;
            ptset_bracelet = false;
            ptset_ring = false;
            ksset_belt = false;
            ksset_boots = false;
            ksset_necklace = false;
            ksset_bracelet = false;
            ksset_ring = false;
            rubyset_belt = false;
            rubyset_boots = false;
            rubyset_necklace = false;
            rubyset_bracelet = false;
            rubyset_ring = false;
            strong_ptset_belt = false;
            strong_ptset_boots = false;
            strong_ptset_necklace = false;
            strong_ptset_bracelet = false;
            strong_ptset_ring = false;
            strong_ksset_belt = false;
            strong_ksset_boots = false;
            strong_ksset_necklace = false;
            strong_ksset_bracelet = false;
            strong_ksset_ring = false;
            strong_rubyset_belt = false;
            strong_rubyset_boots = false;
            strong_rubyset_necklace = false;
            strong_rubyset_bracelet = false;
            strong_rubyset_ring = false;
            dragonset_ring_left = false;
            dragonset_ring_right = false;
            dragonset_bracelet_left = false;
            dragonset_bracelet_right = false;
            dragonset_necklace = false;
            dragonset_dress = false;
            dragonset_helmet = false;
            dragonset_weapon = false;
            dragonset_boots = false;
            dragonset_belt = false;
            RecallSuite = false;
            cghi[0] = false;
            cghi[1] = false;
            cghi[2] = false;
            cghi[3] = false;
            dset_wingdress = false;
            if ((Race == Grobal2.RC_PLAYOBJECT))
            {
                for (i = 0; i <= 12; i++)
                {
                    if ((UseItems[i].Index > 0))
                    {
                        if (UseItems[i].Dura == 0)
                        {
                            pstd = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                            if (pstd != null)
                            {
                                if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND))
                                {
                                    WAbil.HandWeight = (byte)(WAbil.HandWeight + pstd.Weight);
                                }
                                else
                                {
                                    WAbil.WearWeight = (byte)(WAbil.WearWeight + pstd.Weight);
                                }
                            }
                            continue;
                        }
                        pstd = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        ApplyItemParameters(UseItems[i], pstd, ref AddAbil);
                        ApplyItemParametersEx(UseItems[i], ref WAbil);
                        if (pstd != null)
                        {
                            if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND))
                            {
                                WAbil.HandWeight = (byte)(WAbil.HandWeight + pstd.Weight);
                            }
                            else
                            {
                                WAbil.WearWeight = (byte)(WAbil.WearWeight + pstd.Weight);
                            }
                            if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_ARMRINGL) || (i == Grobal2.U_ARMRINGR))
                            {
                                if ((pstd.SpecialPwr <= -1) && (pstd.SpecialPwr >= -50))
                                {
                                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (-pstd.SpecialPwr));
                                }
                                if ((pstd.SpecialPwr <= -51) && (pstd.SpecialPwr >= -100))
                                {
                                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (pstd.SpecialPwr + 50));
                                }
                                if (pstd.Shape == ItemShapeConst.CCHO_WEAPON)
                                {
                                    cho_weapon = true;
                                }
                                if ((pstd.Shape == ItemShapeConst.BONESET_WEAPON_SHAPE) && (pstd.StdMode == 6))
                                {
                                    boneset_weapon = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_WEAPON_SHAPE)
                                {
                                    dragonset_weapon = true;
                                }
                            }
                            if (i == Grobal2.U_NECKLACE)
                            {
                                if (pstd.Shape == ItemShapeConst.NECTLACE_FASTTRAINING_ITEM)
                                {
                                    FastTrain = true;
                                }
                                if (pstd.Shape == ItemShapeConst.NECTLACE_SEARCH_ITEM)
                                {
                                    ProbeNecklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.NECKLACE_GI_ITEM)
                                {
                                    cghi[1] = true;
                                }
                                if (pstd.Shape == ItemShapeConst.NECKLACE_OF_MANATOHEALTH)
                                {
                                    mh_necklace = true;
                                    MoXieSuite = MoXieSuite + pstd.AniCount;
                                }
                                if (pstd.Shape == ItemShapeConst.NECKLACE_OF_SUCKHEALTH)
                                {
                                    sh_necklace = true;
                                    SuckupEnemyHealthRate = SuckupEnemyHealthRate + pstd.AniCount;
                                }
                                if (pstd.Shape == ItemShapeConst.NECKLACE_OF_HPPUP)
                                {
                                    hpp_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.CCHO_NECKLACE)
                                {
                                    cho_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.PSET_NECKLACE_SHAPE)
                                {
                                    pset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.HSET_NECKLACE_SHAPE)
                                {
                                    hset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.YSET_NECKLACE_SHAPE)
                                {
                                    yset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BUGSET_NECKLACE_SHAPE)
                                {
                                    bugset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.PTSET_NECKLACE_SHAPE)
                                {
                                    ptset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.KSSET_NECKLACE_SHAPE)
                                {
                                    ksset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RUBYSET_NECKLACE_SHAPE)
                                {
                                    rubyset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_PTSET_NECKLACE_SHAPE)
                                {
                                    strong_ptset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_KSSET_NECKLACE_SHAPE)
                                {
                                    strong_ksset_necklace = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_RUBYSET_NECKLACE_SHAPE)
                                {
                                    strong_rubyset_necklace = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_NECKLACE_SHAPE)
                                {
                                    dragonset_necklace = true;
                                }
                            }
                            if ((i == Grobal2.U_RINGR) || (i == Grobal2.U_RINGL))
                            {
                                if (pstd.Shape == ItemShapeConst.RING_TRANSPARENT_ITEM)
                                {
                                    StatusArr[Grobal2.STATE_TRANSPARENT] = 60000;
                                    HideMode = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_SPACEMOVE_ITEM)
                                {
                                    Teleport = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_MAKESTONE_ITEM)
                                {
                                    Paralysis = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_REVIVAL_ITEM)
                                {
                                    Revival = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_FIREBALL_ITEM)
                                {
                                    FlameRing = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_HEALING_ITEM)
                                {
                                    RecoveryRing = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_ANGERENERGY_ITEM)
                                {
                                    AngryRing = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_MAGICSHIELD_ITEM)
                                {
                                    MagicShield = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_SUPERSTRENGTH_ITEM)
                                {
                                    MuscleRing = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_CHUN_ITEM)
                                {
                                    cghi[0] = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_OF_MANATOHEALTH)
                                {
                                    mh_ring = true;
                                    MoXieSuite = MoXieSuite + pstd.AniCount;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_OF_SUCKHEALTH)
                                {
                                    sh_ring = true;
                                    SuckupEnemyHealthRate = SuckupEnemyHealthRate + pstd.AniCount;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_OF_HPUP)
                                {
                                    hp_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_OF_MPUP)
                                {
                                    mp_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_OF_HPMPUP)
                                {
                                    hpmp_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RING_OH_HPPUP)
                                {
                                    hpp_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.CCHO_RING)
                                {
                                    cho_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.PSET_RING_SHAPE)
                                {
                                    pset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.HSET_RING_SHAPE)
                                {
                                    hset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.YSET_RING_SHAPE)
                                {
                                    yset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BUGSET_RING_SHAPE)
                                {
                                    bugset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.PTSET_RING_SHAPE)
                                {
                                    ptset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.KSSET_RING_SHAPE)
                                {
                                    ksset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RUBYSET_RING_SHAPE)
                                {
                                    rubyset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_PTSET_RING_SHAPE)
                                {
                                    strong_ptset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_KSSET_RING_SHAPE)
                                {
                                    strong_ksset_ring = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_RUBYSET_RING_SHAPE)
                                {
                                    strong_rubyset_ring = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_RING_SHAPE)
                                {
                                    if ((i == Grobal2.U_RINGL))
                                    {
                                        dragonset_ring_left = true;
                                    }
                                    if ((i == Grobal2.U_RINGR))
                                    {
                                        dragonset_ring_right = true;
                                    }
                                }
                            }
                            if ((i == Grobal2.U_ARMRINGL) || (i == Grobal2.U_ARMRINGR))
                            {
                                if (pstd.Shape == ItemShapeConst.ARMRING_HAP_ITEM)
                                {
                                    cghi[2] = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BRACELET_OF_MANATOHEALTH)
                                {
                                    mh_bracelet = true;
                                    MoXieSuite = MoXieSuite + pstd.AniCount;
                                }
                                if (pstd.Shape == ItemShapeConst.BRACELET_OF_SUCKHEALTH)
                                {
                                    sh_bracelet = true;
                                    SuckupEnemyHealthRate = SuckupEnemyHealthRate + pstd.AniCount;
                                }
                                if (pstd.Shape == ItemShapeConst.BRACELET_OF_HPUP)
                                {
                                    hp_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BRACELET_OF_MPUP)
                                {
                                    mp_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BRACELET_OF_HPMPUP)
                                {
                                    hpmp_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BRACELET_OF_HPPUP)
                                {
                                    hpp_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.CCHO_BRACELET)
                                {
                                    cho_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.PSET_BRACELET_SHAPE)
                                {
                                    pset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.HSET_BRACELET_SHAPE)
                                {
                                    hset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.YSET_BRACELET_SHAPE)
                                {
                                    yset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BUGSET_BRACELET_SHAPE)
                                {
                                    bugset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.PTSET_BRACELET_SHAPE)
                                {
                                    ptset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.KSSET_BRACELET_SHAPE)
                                {
                                    ksset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RUBYSET_BRACELET_SHAPE)
                                {
                                    rubyset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_PTSET_BRACELET_SHAPE)
                                {
                                    strong_ptset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_KSSET_BRACELET_SHAPE)
                                {
                                    strong_ksset_bracelet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_RUBYSET_BRACELET_SHAPE)
                                {
                                    strong_rubyset_bracelet = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_BRACELET_SHAPE)
                                {
                                    if ((i == Grobal2.U_ARMRINGL))
                                    {
                                        dragonset_bracelet_left = true;
                                    }
                                    if ((i == Grobal2.U_ARMRINGR))
                                    {
                                        dragonset_bracelet_right = true;
                                    }
                                }
                            }
                            if ((i == Grobal2.U_HELMET))
                            {
                                if (pstd.Shape == ItemShapeConst.HELMET_IL_ITEM)
                                {
                                    cghi[3] = true;
                                }
                                if (pstd.Shape == ItemShapeConst.CCHO_HELMET)
                                {
                                    cho_helmet = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BONESET_HELMET_SHAPE)
                                {
                                    boneset_helmet = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_HELMET_SHAPE)
                                {
                                    dragonset_helmet = true;
                                }
                            }
                            if ((i == Grobal2.U_DRESS))
                            {
                                if (pstd.Shape == ItemShapeConst.DRESS_SHAPE_WING)
                                {
                                    dset_wingdress = true;
                                }
                                if (pstd.Shape == ItemShapeConst.BONESET_DRESS_SHAPE)
                                {
                                    boneset_dress = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_DRESS_SHAPE)
                                {
                                    dragonset_dress = true;
                                }
                            }
                            if ((i == Grobal2.U_BELT))
                            {
                                if (pstd.Shape == ItemShapeConst.PTSET_BELT_SHAPE)
                                {
                                    ptset_belt = true;
                                }
                                if (pstd.Shape == ItemShapeConst.KSSET_BELT_SHAPE)
                                {
                                    ksset_belt = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RUBYSET_BELT_SHAPE)
                                {
                                    rubyset_belt = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_PTSET_BELT_SHAPE)
                                {
                                    strong_ptset_belt = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_KSSET_BELT_SHAPE)
                                {
                                    strong_ksset_belt = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_RUBYSET_BELT_SHAPE)
                                {
                                    strong_rubyset_belt = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_BELT_SHAPE)
                                {
                                    dragonset_belt = true;
                                }
                            }
                            if ((i == Grobal2.U_BOOTS))
                            {
                                if (pstd.Shape == ItemShapeConst.PTSET_BOOTS_SHAPE)
                                {
                                    ptset_boots = true;
                                }
                                if (pstd.Shape == ItemShapeConst.KSSET_BOOTS_SHAPE)
                                {
                                    ksset_boots = true;
                                }
                                if (pstd.Shape == ItemShapeConst.RUBYSET_BOOTS_SHAPE)
                                {
                                    rubyset_boots = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_PTSET_BOOTS_SHAPE)
                                {
                                    strong_ptset_boots = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_KSSET_BOOTS_SHAPE)
                                {
                                    strong_ksset_boots = true;
                                }
                                if (pstd.Shape == ItemShapeConst.STRONG_RUBYSET_BOOTS_SHAPE)
                                {
                                    strong_rubyset_boots = true;
                                }
                                if (pstd.Shape == DragonConst.DRAGON_BOOTS_SHAPE)
                                {
                                    dragonset_boots = true;
                                }
                            }
                            if ((i == Grobal2.U_CHARM))
                            {
                                if ((pstd.StdMode == 53) && (pstd.Shape == ItemShapeConst.SHAPE_OF_LUCKYLADLE))
                                {
                                    AddAbil.Luck = (byte)(HUtil32._MIN(255, AddAbil.Luck + 1));
                                }
                            }
                        }
                    }
                }
                if (cghi[0] && cghi[1] && cghi[2] && cghi[3])
                {
                    RecallSuite = true;
                }
                if (mh_necklace && mh_bracelet && mh_ring)
                {
                    MoXieSuite = MoXieSuite + 50;
                }
                if (sh_necklace && sh_bracelet && sh_ring)
                {
                    AddAbil.HIT = (ushort)(AddAbil.HIT + 2);
                }
                if (hp_bracelet && hp_ring)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + 50);
                }
                if (mp_bracelet && mp_ring)
                {
                    AddAbil.MP = (ushort)(AddAbil.MP + 50);
                }
                if (hpmp_bracelet && hpmp_ring)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + 30);
                    AddAbil.MP = (ushort)(AddAbil.MP + 30);
                }
                if (hpp_necklace && hpp_bracelet && hpp_ring)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + ((WAbil.MaxHP * 30) / 100));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(2, 2));
                }
                if (cho_weapon && cho_necklace && cho_ring && cho_helmet && cho_bracelet)
                {
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 4);
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(2, 5));
                }
                if (pset_bracelet && pset_ring)
                {
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    if (pset_necklace)
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(1, 3));
                    }
                }
                if (hset_bracelet && hset_ring)
                {
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 20);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 5));
                    if (hset_necklace)
                    {
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (yset_bracelet && yset_ring)
                {
                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + 3);
                    if (yset_necklace)
                    {
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (boneset_weapon && boneset_helmet && boneset_dress)
                {
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 2));
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 1));
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 1));
                }
                if (bugset_necklace && bugset_ring && bugset_bracelet)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 1));
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 1));
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 1));
                    AddAbil.AntiMagic = (ushort)(AddAbil.AntiMagic + 1);
                    AddAbil.AntiPoison = (ushort)(AddAbil.AntiPoison + 1);
                }
                if (ptset_belt && ptset_boots && ptset_necklace && ptset_bracelet && ptset_ring)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 2));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 2));
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (ksset_belt && ksset_boots && ksset_necklace && ksset_bracelet && ksset_ring)
                {
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 1));
                    AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 1));
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (rubyset_belt && rubyset_boots && rubyset_necklace && rubyset_bracelet && rubyset_ring)
                {
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                    AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 2));
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strong_ptset_belt && strong_ptset_boots && strong_ptset_necklace && strong_ptset_bracelet && strong_ptset_ring)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 3));
                    AddAbil.HP = (ushort)(AddAbil.HP + 30);
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strong_ksset_belt && strong_ksset_boots && strong_ksset_necklace && strong_ksset_bracelet && strong_ksset_ring)
                {
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                    AddAbil.HP = (ushort)(AddAbil.HP + 15);
                    AddAbil.MP = (ushort)(AddAbil.MP + 20);
                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + 1);
                    AddAbil.HIT = (ushort)(AddAbil.HIT + 1);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strong_rubyset_belt && strong_rubyset_boots && strong_rubyset_necklace && strong_rubyset_bracelet && strong_rubyset_ring)
                {
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                    AddAbil.MP = (ushort)(AddAbil.MP + 40);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 2);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (dragonset_ring_left && dragonset_ring_right && dragonset_bracelet_left && dragonset_bracelet_right && dragonset_necklace && dragonset_dress && dragonset_helmet && dragonset_weapon && dragonset_boots && dragonset_belt)
                {
                    AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 4));
                    AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 4));
                    AddAbil.Luck = (byte)(HUtil32._MIN(255, AddAbil.Luck + 2));
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    AddAbil.AntiMagic = (ushort)(AddAbil.AntiMagic + 6);
                    AddAbil.AntiPoison = (ushort)(AddAbil.AntiPoison + 6);
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 34));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 27));
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 120);
                    WAbil.MaxHP = (ushort)(WAbil.MaxHP + 70);
                    WAbil.MaxMP = (ushort)(WAbil.MaxMP + 80);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 4));
                    AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 3));
                    AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 3));
                }
                else
                {
                    if (dragonset_dress && dragonset_helmet && dragonset_weapon && dragonset_boots && dragonset_belt)
                    {
                        WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 34));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 50);
                        AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 4));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 3));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 3));
                    }
                    else if (dragonset_dress && dragonset_boots && dragonset_belt)
                    {
                        WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 1));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 1));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 1));
                    }
                    else if (dragonset_dress && dragonset_helmet && dragonset_weapon)
                    {
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 2));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 1));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 1));
                        AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    }
                    if (dragonset_ring_left && dragonset_ring_right && dragonset_bracelet_left && dragonset_bracelet_right && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 27));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 50);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 3));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 3));
                    }
                    else if ((dragonset_ring_left || dragonset_ring_right) && dragonset_bracelet_left && dragonset_bracelet_right && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                    }
                    else if (dragonset_ring_left && dragonset_ring_right && (dragonset_bracelet_left || dragonset_bracelet_right) && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 2));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 2));
                    }
                    else if ((dragonset_ring_left || dragonset_ring_right) && (dragonset_bracelet_left || dragonset_bracelet_right) && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                    }
                    else
                    {
                        if (dragonset_bracelet_left && dragonset_bracelet_right)
                        {
                            AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC)));
                            AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC)));
                        }
                        if (dragonset_ring_left && dragonset_ring_right)
                        {
                            AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                            AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                        }
                    }
                }
                if (dset_wingdress && (Abil.Level >= 20))
                {
                    if ((Abil.Level < 30))
                    {

                    }
                    else if ((Abil.Level < 40))
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 1));
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                        AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(2, 3));
                        AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 2));
                    }
                    else if ((Abil.Level < 50))
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 3));
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 4));
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 4));
                        AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(5, 5));
                        AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(1, 2));
                    }
                    else
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 5));
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 6));
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 6));
                        AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(9, 7));
                        AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(2, 4));
                    }
                }
                WAbil.Weight = RecalcBagWeight();
            }
            if (Transparent && (StatusArr[Grobal2.STATE_TRANSPARENT] > 0))
            {
                HideMode = true;
            }
            if (HideMode)
            {
                if (!oldhmode)
                {
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else
            {
                if (oldhmode)
                {
                    StatusArr[Grobal2.STATE_TRANSPARENT] = 0;
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            RecalcHitSpeed();
            if (AddAbil.HitSpeed >= 0)
            {
                AddAbil.HitSpeed = ((ushort)(AddAbil.HitSpeed / 2));
            }
            else
            {
                AddAbil.HitSpeed = (ushort)((AddAbil.HitSpeed - 1) / 2);
            }
            AddAbil.HitSpeed = (ushort)HUtil32._MIN(15, AddAbil.HitSpeed);
            oldlight = Light;
            Light = GetMyLight();
            if (oldlight != Light)
            {
                SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
            }
            SpeedPoint = (byte)(SpeedPoint + AddAbil.SPEED);
            HitPoint = (byte)(HitPoint + AddAbil.HIT);
            AntiPoison = (byte)(AntiPoison + AddAbil.AntiPoison);
            PoisonRecover = (ushort)(PoisonRecover + AddAbil.PoisonRecover);
            HealthRecover = (ushort)(HealthRecover + AddAbil.HealthRecover);
            SpellRecover = (ushort)(SpellRecover + AddAbil.SpellRecover);
            AntiMagic = (ushort)(AntiMagic + AddAbil.AntiMagic);
            Luck = Luck + AddAbil.Luck;
            Luck = Luck - AddAbil.UnLuck;
            HitSpeed = AddAbil.HitSpeed;
            WAbil.MaxHP = (ushort)(Abil.MaxHP + AddAbil.HP);
            WAbil.MaxMP = (ushort)(Abil.MaxMP + AddAbil.MP);
            WAbil.AC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.AC) + HUtil32.LoByte(Abil.AC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + HUtil32.HiByte(Abil.AC)));
            WAbil.MAC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MAC) + HUtil32.LoByte(Abil.MAC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + HUtil32.HiByte(Abil.MAC)));
            WAbil.DC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.DC) + HUtil32.LoByte(Abil.DC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + HUtil32.HiByte(Abil.DC)));
            WAbil.MC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MC) + HUtil32.LoByte(Abil.MC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + HUtil32.HiByte(Abil.MC)));
            WAbil.SC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.SC) + HUtil32.LoByte(Abil.SC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + HUtil32.HiByte(Abil.SC)));
            if (StatusArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                WAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(WAbil.AC) + (Abil.Level / 7) + StatusArrTick[Grobal2.STATE_DEFENCEUP]));
            }
            if (StatusArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                WAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(WAbil.MAC) + (Abil.Level / 7) + StatusArrTick[Grobal2.STATE_MAGDEFENCEUP]));
            }
            if (ExtraAbil[AbilConst.EABIL_DCUP] > 0)
            {
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_MCUP] > 0)
            {
                WAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MC), HUtil32.HiByte(WAbil.MC) + ExtraAbil[AbilConst.EABIL_MCUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_SCUP] > 0)
            {
                WAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.SC), HUtil32.HiByte(WAbil.SC) + ExtraAbil[AbilConst.EABIL_SCUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_HITSPEEDUP] > 0)
            {
                HitSpeed = (ushort)(HitSpeed + ExtraAbil[AbilConst.EABIL_HITSPEEDUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_HPUP] > 0)
            {
                WAbil.MaxHP = (ushort)(WAbil.MaxHP + ExtraAbil[AbilConst.EABIL_HPUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_MPUP] > 0)
            {
                WAbil.MaxMP = (ushort)(WAbil.MaxMP + ExtraAbil[AbilConst.EABIL_MPUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_PWRRATE] > 0)
            {
                WAbil.DC = HUtil32.MakeWord((HUtil32.LoByte(WAbil.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100, (HUtil32.HiByte(WAbil.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100);
                WAbil.MC = HUtil32.MakeWord((HUtil32.LoByte(WAbil.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100, (HUtil32.HiByte(WAbil.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100);
                WAbil.SC = HUtil32.MakeWord((HUtil32.LoByte(WAbil.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100, (HUtil32.HiByte(WAbil.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100);
            }
            if (FlameRing)
            {
                AddItemSkill(M2Share.AM_FIREBALL);
            }
            else
            {
                DelItemSkill(M2Share.AM_FIREBALL);
            }
            if (RecoveryRing)
            {
                AddItemSkill(M2Share.AM_HEALING);
            }
            else
            {
                DelItemSkill(M2Share.AM_HEALING);
            }
            if (MuscleRing)
            {
                WAbil.MaxWeight = (ushort)(WAbil.MaxWeight * 2);
                WAbil.MaxWearWeight = (byte)HUtil32._MIN(255, WAbil.MaxWearWeight * 2);
                if ((WAbil.MaxHandWeight * 2 > 255))
                {
                    WAbil.MaxHandWeight = 255;
                }
                else
                {
                    WAbil.MaxHandWeight = (byte)(WAbil.MaxHandWeight * 2);
                }
            }
            if (MoXieSuite > 0)
            {
                if (MoXieSuite >= WAbil.MaxMP)
                {
                    MoXieSuite = WAbil.MaxMP - 1;
                }
                WAbil.MaxMP = (ushort)(WAbil.MaxMP - MoXieSuite);
                WAbil.MaxHP = (ushort)(WAbil.MaxHP + MoXieSuite);
                if ((Race == Grobal2.RC_PLAYOBJECT) && (WAbil.HP > WAbil.MaxHP))
                {
                    WAbil.HP = WAbil.MaxHP;
                }
            }
            if ((Race == Grobal2.RC_PLAYOBJECT) && (WAbil.HP > WAbil.MaxHP) && (!mh_necklace && !mh_bracelet && !mh_ring))
            {
                WAbil.HP = WAbil.MaxHP;
            }
            if ((Race == Grobal2.RC_PLAYOBJECT) && (WAbil.MP > WAbil.MaxMP))
            {
                WAbil.MP = WAbil.MaxMP;
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                fastmoveflag = false;
                if ((UseItems[Grobal2.U_BOOTS].Dura > 0) && (UseItems[Grobal2.U_BOOTS].Index == M2Share.INDEX_MIRBOOTS))
                {
                    fastmoveflag = true;
                }
                if (fastmoveflag)
                {
                    StatusArr[Grobal2.STATE_FASTMOVE] = 60000;
                }
                else
                {
                    StatusArr[Grobal2.STATE_FASTMOVE] = 0;
                }
                //if ((Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL))
                //{
                //    if (BoHighLevelEffect)
                //    {
                //        StatusArr[Grobal2.STATE_50LEVELEFFECT] = 60000;
                //    }
                //    else
                //    {
                //        StatusArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                //    }
                //}
                //else
                //{
                //    StatusArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                //}
                CharStatus = GetCharStatus();
                StatusChanged();
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
            }
            if (Race >= Grobal2.RC_ANIMAL)
            {
                ApplySlaveLevelAbilitys();
            }
        }

        private void ApplyItemParameters(UserItem uitem, StdItem stdItem, ref AddAbility aabil)
        {
            var item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                var clientItem = new ClientItem();
                item.GetUpgradeStdItem(uitem, ref clientItem);
                ApplyItemParametersByJob(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 5:
                    case 6:
                        aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + stdItem.RealAttackSpeed(HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        if (clientItem.Item.SpecialPwr >= 1 && clientItem.Item.SpecialPwr <= 10)
                        {
                            aabil.WeaponStrong = (byte)clientItem.Item.SpecialPwr;
                        }
                        break;
                    case 10:
                    case 11:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (byte)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        aabil.HP = (byte)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (byte)(aabil.MP + clientItem.Item.MpAdd);
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_HP_MP_ADD:
                                    if ((aabil.HealthRecover + clientItem.Item.EffRate1 > 65000))
                                    {
                                        aabil.HealthRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + clientItem.Item.EffRate1);
                                    }
                                    if ((aabil.SpellRecover + clientItem.Item.EffValue1 > 65000))
                                    {
                                        aabil.SpellRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_HP_MP_ADD:
                                    if ((aabil.HealthRecover + clientItem.Item.EffRate2 > 65000))
                                    {
                                        aabil.HealthRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + clientItem.Item.EffRate2);
                                    }
                                    if ((aabil.SpellRecover + clientItem.Item.EffValue2 > 65000))
                                    {
                                        aabil.SpellRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType1 == EfftypeConst.EFFTYPE_LUCK_ADD)
                        {
                            if (aabil.Luck + clientItem.Item.EffValue1 > 255)
                            {
                                aabil.Luck = 255;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue1);
                            }
                        }
                        else if (clientItem.Item.EffType2 == EfftypeConst.EFFTYPE_LUCK_ADD)
                        {
                            if (aabil.Luck + clientItem.Item.EffValue2 > 255)
                            {
                                aabil.Luck = 255;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue2);
                            }
                        }
                        break;
                    case 15:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (byte)((byte)(aabil.AntiPoison + clientItem.Item.ToxAvoid));
                        break;
                    case 19:
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 20:
                        aabil.HIT = (byte)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SPEED = (byte)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 21:
                        aabil.HealthRecover = (byte)(aabil.HealthRecover + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SpellRecover = (byte)(aabil.SpellRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 22:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.HP = (byte)(aabil.HP + clientItem.Item.HpAdd);
                        break;
                    case 23:
                        aabil.AntiPoison = (byte)(aabil.AntiPoison + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.PoisonRecover = (byte)(aabil.PoisonRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 24:
                    case 26:
                        if (clientItem.Item.SpecialPwr >= 1 && clientItem.Item.SpecialPwr <= 10)
                        {
                            aabil.WeaponStrong = (byte)clientItem.Item.SpecialPwr;
                        }
                        switch (item.StdMode)
                        {
                            case 24:
                                aabil.HIT = (byte)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                                aabil.SPEED = (byte)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                                break;
                            case 26:
                                aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                                aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                                aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                                aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                                aabil.MP = (byte)(aabil.MP + clientItem.Item.MpAdd);
                                break;
                        }
                        break;
                    case 52:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                        break;
                    case 54:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiPoison = (byte)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        break;
                    case 53:
                        aabil.HP = (byte)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (byte)(aabil.MP + clientItem.Item.MpAdd);
                        break;
                    default:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        break;
                }
                aabil.DC = HUtil32.MakeWord(HUtil32.LoByte(aabil.DC) + HUtil32.LoByte(clientItem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(aabil.DC) + HUtil32.HiByte(clientItem.Item.DC)));
                aabil.MC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MC) + HUtil32.LoByte(clientItem.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(aabil.MC) + HUtil32.HiByte(clientItem.Item.MC)));
                aabil.SC = HUtil32.MakeWord(HUtil32.LoByte(aabil.SC) + HUtil32.LoByte(clientItem.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(aabil.SC) + HUtil32.HiByte(clientItem.Item.SC)));
            }
        }

        private void ApplyItemParametersEx(UserItem uitem, ref Ability AWabil)
        {
            StdItem item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                var clientItem = new ClientItem();
                item.GetUpgradeStdItem(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 52:
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((AWabil.MaxHandWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        AWabil.MaxHandWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxHandWeight = (byte)(AWabil.MaxHandWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((AWabil.MaxWearWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        AWabil.MaxWearWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxWearWeight = (byte)(AWabil.MaxWearWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((AWabil.MaxHandWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        AWabil.MaxHandWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxHandWeight = (byte)(AWabil.MaxHandWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((AWabil.MaxWearWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        AWabil.MaxWearWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxWearWeight = (byte)(AWabil.MaxWearWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                    case 54:
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((AWabil.MaxWeight + clientItem.Item.EffValue1 > 65000))
                                    {
                                        AWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        AWabil.MaxWeight = (byte)(AWabil.MaxWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((AWabil.MaxWeight + clientItem.Item.EffValue2 > 65000))
                                    {
                                        AWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        AWabil.MaxWeight = (byte)(AWabil.MaxWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        protected void ChangeItemByJob(ref ClientItem citem, int lv)
        {
            if ((citem.Item.StdMode == 22) && (citem.Item.Shape == DragonConst.DRAGON_RING_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 4));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 26) && (citem.Item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Wizard:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 19) && (citem.Item.Shape == DragonConst.DRAGON_NECKLACE_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if (((citem.Item.StdMode == 10) || (citem.Item.StdMode == 11)) && (citem.Item.Shape == DragonConst.DRAGON_DRESS_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 15) && (citem.Item.Shape == DragonConst.DRAGON_HELMET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if (((citem.Item.StdMode == 5) || (citem.Item.StdMode == 6)) && (citem.Item.Shape == DragonConst.DRAGON_WEAPON_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 28));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC) - 2, HUtil32.HiByte(citem.Item.AC));
                        break;
                    case PlayJob.Taoist:
                        citem.Item.SC = 0;
                        if (HUtil32.HiByte(citem.Item.MAC) > 12)
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), HUtil32.HiByte(citem.Item.MAC) - 12);
                        }
                        else
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), 0);
                        }
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC) + 2, HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 10));
                        citem.Item.MC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC) - 2, HUtil32.HiByte(citem.Item.AC));
                        break;
                }
            }
            else if ((citem.Item.StdMode == 53))
            {
                if ((citem.Item.Shape == ShapeConst.LOLLIPOP_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC) + 2));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC) + 2));
                            break;
                    }
                }
                else if ((citem.Item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC)));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC)));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC)));
                            break;
                    }
                }
            }
        }

        private void ApplyItemParametersByJob(UserItem uitem, ref ClientItem std)
        {
            var item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                if ((item.StdMode == 22) && (item.Shape == DragonConst.DRAGON_RING_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 4));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 26) && (item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 2));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.AC) + 1));
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.AC) + 1));
                            break;
                        case PlayJob.Wizard:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 19) && (item.Shape == DragonConst.DRAGON_NECKLACE_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if (((item.StdMode == 10) || (item.StdMode == 11)) && (item.Shape == DragonConst.DRAGON_DRESS_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 15) && (item.Shape == DragonConst.DRAGON_HELMET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if (((item.StdMode == 5) || (item.StdMode == 6)) && (item.Shape == DragonConst.DRAGON_WEAPON_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 28));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC) - 2, HUtil32.HiByte(std.Item.AC));
                            break;
                        case PlayJob.Taoist:
                            std.Item.SC = 0;
                            if (HUtil32.HiByte(std.Item.MAC) > 12)
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MAC), HUtil32.HiByte(std.Item.MAC) - 12);
                            }
                            else
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MAC), 0);
                            }
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC) + 2, HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 10));
                            std.Item.MC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC) - 2, HUtil32.HiByte(std.Item.AC));
                            break;
                    }
                }
                else if ((item.StdMode == 53))
                {
                    if ((item.Shape == ShapeConst.LOLLIPOP_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 2));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.MC) + 2));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.SC) + 2));
                                break;
                        }
                    }
                    else if ((std.Item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (std.Item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (std.Item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC)));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.MC)));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.SC)));
                                break;
                        }
                    }
                }
            }
        }

        public void ApplySlaveLevelAbilitys()
        {
            //if ((Race == Grobal2.RC_ANGEL) || (Race == Grobal2.RC_CLONE))
            //{
            //    return;
            //}
            int chp = 0;
            if ((Race == Grobal2.RC_WHITESKELETON) || (Race == Grobal2.RC_ELFMON) || (Race == Grobal2.RC_ELFWARRIORMON))
            {
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (int)Math.Round(HUtil32.HiByte(WAbil.DC) + (3 * (0.3 + SlaveExpLevel * 0.1) * SlaveExpLevel)));
                chp = (ushort)(chp + Math.Round(Abil.MaxHP * (0.3 + SlaveExpLevel * 0.1)) * SlaveExpLevel);
                chp = Abil.MaxHP + chp;
                if (SlaveExpLevel > 0)
                {
                    WAbil.MaxHP = (ushort)chp;
                }
                else
                {
                    WAbil.MaxHP = Abil.MaxHP;
                }
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]);
            }
            else
            {
                if (Master != null)
                {
                    chp = Abil.MaxHP;
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (int)Math.Abs(HUtil32.HiByte(WAbil.DC) + (2 * SlaveExpLevel)));
                    chp = (ushort)(chp + Math.Round(Abil.MaxHP * 0.15) * SlaveExpLevel);
                    WAbil.MaxHP = (ushort)HUtil32._MIN(Math.Abs(Abil.MaxHP + 60 * SlaveExpLevel), chp);
                    WAbil.MAC = 0;
                }
            }
            //AccuracyPoint = 15;
        }

        private int GetMyLight_CheckLightValue()
        {
            int result;
            StdItem ps;
            int CurrentLight;
            PlayObject hum;
            CurrentLight = 0;
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                hum = ((this) as PlayObject);
                if (hum != null)
                {
                    if (true)//BoHighLevelEffect
                    {
                        if (Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL)
                        {
                            CurrentLight = 1;
                        }
                    }
                }
            }
            for (var i = Grobal2.U_DRESS; i <= Grobal2.U_CHARM; i++)
            {
                if ((UseItems[i].Index > 0) && (UseItems[i].Dura > 0))
                {
                    ps = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (ps != null)
                    {
                        if (CurrentLight < ps.Light)
                        {
                            CurrentLight = ps.Light;
                        }
                    }
                }
            }
            result = CurrentLight;
            return result;
        }

        public int GetMyLight()
        {
            return GetMyLight_CheckLightValue();
        }

    }
}