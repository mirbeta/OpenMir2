using GameSvr.Items;
using GameSvr.Npc;
using GameSvr.Player;
using GameSvr.RobotPlay;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;

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
                    WAbil.HP = WAbil.MaxHP;
                    WAbil.MP = WAbil.MaxMP;
                }
                int dwC = (HUtil32.GetTickCount() - MDwHpmpTick) / 20;
                MDwHpmpTick = HUtil32.GetTickCount();
                HealthTick += dwC;
                SpellTick += dwC;
                if (!Death)
                {
                    ushort n18;
                    if ((WAbil.HP < WAbil.MaxHP) && (HealthTick >= M2Share.Config.HealthFillTime))
                    {
                        n18 = (ushort)((WAbil.MaxHP / 75) + 1);
                        if ((WAbil.HP + n18) < WAbil.MaxHP)
                        {
                            WAbil.HP += n18;
                        }
                        else
                        {
                            WAbil.HP = WAbil.MaxHP;
                        }
                        HealthSpellChanged();
                    }
                    if ((WAbil.MP < WAbil.MaxMP) && (SpellTick >= M2Share.Config.SpellFillTime))
                    {
                        n18 = (ushort)((WAbil.MaxMP / 18) + 1);
                        if ((WAbil.MP + n18) < WAbil.MaxMP)
                        {
                            WAbil.MP += n18;
                        }
                        else
                        {
                            WAbil.MP = WAbil.MaxMP;
                        }
                        HealthSpellChanged();
                    }
                    if (WAbil.HP == 0)
                    {
                        if (((LastHiter == null) || !LastHiter.UnRevival) && Revival && ((HUtil32.GetTickCount() - RevivalTick) > M2Share.Config.RevivalTime))// 60 * 1000
                        {
                            RevivalTick = HUtil32.GetTickCount();
                            ItemDamageRevivalRing();
                            WAbil.HP = WAbil.MaxHP;
                            HealthSpellChanged();
                            SysMsg(M2Share.g_sRevivalRecoverMsg, MsgColor.Green, MsgType.Hint);
                        }
                        if (WAbil.HP == 0)
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
                    int dwInChsTime = 600 - HUtil32._MIN(400, WAbil.Level * 10);
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
                            PerHealth = WAbil.Level / 10 + 5;
                            PerSpell = WAbil.Level / 10 + 5;
                            PerHealing = 5;
                            IncHealthSpell(nHP, nMP);
                            if (WAbil.HP == WAbil.MaxHP)
                            {
                                IncHealth = 0;
                                IncHealing = 0;
                            }
                            if (WAbil.MP == WAbil.MaxMP)
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
                if ((HealthTick < -M2Share.Config.HealthFillTime) && (WAbil.HP > 1))
                {
                    WAbil.HP -= 1;
                    HealthTick += M2Share.Config.HealthFillTime;
                    HealthSpellChanged();
                }
                // 检查HP/MP值是否大于最大值，大于则降低到正常大小
                bool boNeedRecalc = false;
                if (WAbil.HP > WAbil.MaxHP)
                {
                    boNeedRecalc = true;
                    WAbil.HP = (ushort)(WAbil.MaxHP - 1);
                }
                if (WAbil.MP > WAbil.MaxMP)
                {
                    boNeedRecalc = true;
                    WAbil.MP = (ushort)(WAbil.MaxMP - 1);
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
                if (UseItems.Length >= Grobal2.U_CHARM && UseItems[Grobal2.U_CHARM] != null && UseItems[Grobal2.U_CHARM].Index > 0)
                {
                    if (!Death && Race == ActorRace.Play || Race == ActorRace.PlayClone)
                    {
                        StdItem StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].Index);
                        if ((StdItem.StdMode == 7) && (StdItem.Shape == 2 || StdItem.Shape == 3))
                        {
                            int nCount;
                            int dCount;
                            int bCount;
                            // 加HP
                            if ((IncHealth == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > M2Share.Config.HPStoneIntervalTime) && ((WAbil.HP / WAbil.MaxHP * 100) < M2Share.Config.HPStoneStartRate))
                            {
                                IncHpStoneTime = HUtil32.GetTickCount();
                                nCount = UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = nCount / M2Share.Config.HPStoneAddRate;
                                dCount = WAbil.MaxHP - WAbil.HP;
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
                                    if (Race == ActorRace.Play)
                                    {
                                        SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_CHARM, UseItems[Grobal2.U_CHARM].Dura, UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    UseItems[Grobal2.U_CHARM].Dura = 0;
                                    if (Race == ActorRace.Play)
                                    {
                                        (this as PlayObject).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                    }
                                    UseItems[Grobal2.U_CHARM].Index = 0;
                                }
                            }
                            // 加MP
                            if ((IncSpell == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > M2Share.Config.MpStoneIntervalTime) && ((WAbil.MP / WAbil.MaxMP * 100) < M2Share.Config.MPStoneStartRate))
                            {
                                IncMpStoneTime = HUtil32.GetTickCount();
                                nCount = UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = nCount / M2Share.Config.MPStoneAddRate;
                                dCount = WAbil.MaxMP - WAbil.MP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
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
                                    if (Race == ActorRace.Play)
                                    {
                                        SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_CHARM, UseItems[Grobal2.U_CHARM].Dura, UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    UseItems[Grobal2.U_CHARM].Dura = 0;
                                    if (Race == ActorRace.Play)
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
                                nInteger = PoisonState.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = PoisonState.STONE;
                                break;
                            case 2:
                                nInteger = PoisonState.DONTMOVE;
                                break;
                            case 3:
                                nInteger = PoisonState.POISON_68;
                                break;
                            case 4:
                                nInteger = PoisonState.DECHEALTH;
                                break;
                            case 5:
                                nInteger = PoisonState.LOCKSPELL;
                                break;
                            case 6:
                                nInteger = PoisonState.DAMAGEARMOR;
                                break;
                            default:
                                AutoChangeIdx = 0;
                                nInteger = PoisonState.STATE_TRANSPARENT;
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
                                nInteger = PoisonState.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = PoisonState.STONE;
                                break;
                            case 2:
                                nInteger = PoisonState.DONTMOVE;
                                break;
                            case 3:
                                nInteger = PoisonState.POISON_68;
                                break;
                            case 4:
                                nInteger = PoisonState.DECHEALTH;
                                break;
                            case 5:
                                nInteger = PoisonState.LOCKSPELL;
                                break;
                            case 6:
                                nInteger = PoisonState.DAMAGEARMOR;
                                break;
                            default:
                                FixColorIdx = 0;
                                nInteger = PoisonState.STATE_TRANSPARENT;
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
                            WAbil.DC = (ushort)HUtil32.MakeLong((short)(HUtil32.LoByte(WAbil.DC) * M2Share.Config.MasterDieMutinyPower), (short)(HUtil32.HiByte(WAbil.DC) * M2Share.Config.MasterDieMutinyPower));
                            WalkSpeed = WalkSpeed / M2Share.Config.MasterDieMutinySpeed;
                            RefNameColor();
                            RefShowName();
                        }
                        else
                        {
                            WAbil.HP = 0;
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
                    if (Race == ActorRace.Play)
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
                            WAbil.HP = (ushort)(WAbil.HP / 10);
                            RefShowName();
                        }
                        if (MasterTick != 0)
                        {
                            if ((HUtil32.GetTickCount() - MasterTick) > 12 * 60 * 60 * 1000)
                            {
                                WAbil.HP = 0;
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
                                    case PoisonState.DEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case PoisonState.MAGDEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("魔法防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case PoisonState.BUBBLEDEFENCEUP:
                                        AbilMagBubbleDefence = false;
                                        break;
                                    case PoisonState.STATE_TRANSPARENT:
                                        HideMode = false;
                                        break;
                                }
                            }
                            else if (StatusArr[i] == 10)
                            {
                                if (i == PoisonState.DEFENCEUP)
                                {
                                    SysMsg("防御力" + StatusArr[i] + "秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                }
                                if (i == PoisonState.MAGDEFENCEUP)
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
            catch (Exception ex)
            {
                M2Share.Log.Error(sExceptionMsg5);
            }
            try
            {
                if ((HUtil32.GetTickCount() - PoisoningTick) > M2Share.Config.PosionDecHealthTime)
                {
                    PoisoningTick = HUtil32.GetTickCount();
                    if (StatusArr[PoisonState.DECHEALTH] > 0)
                    {
                        if (Animal)
                        {
                            MeatQuality -= 1000;
                        }
                        DamageHealth((ushort)(GreenPoisoningPoint + 1));
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
                if (Race != ActorRace.Play && LastHiter != null)
                {
                    if (M2Share.Config.MonSayMsg)
                    {
                        MonsterSayMsg(LastHiter, MonStatus.Die);
                    }
                    if (ExpHitter != null)
                    {
                        if (ExpHitter.Race == ActorRace.Play)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(ExpHitter as PlayObject, "@PlayKillMob", false);
                            }
                            tExp = ExpHitter.CalcGetExp(WAbil.Level, FightExp);
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
                                        QuestNPC = Envir.GetQuestNpc(GroupHuman, ChrName, "", tCheck);
                                        if (QuestNPC != null)
                                        {
                                            QuestNPC.Click(GroupHuman);
                                        }
                                    }
                                }
                                QuestNPC = Envir.GetQuestNpc(ExpHitter, ChrName, "", false);
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
                                ExpHitter.GainSlaveExp(WAbil.Level);
                                tExp = ExpHitter.Master.CalcGetExp(WAbil.Level, FightExp);
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
                        if (LastHiter.Race == ActorRace.Play)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(LastHiter as PlayObject, "@PlayKillMob", false);
                            }
                            tExp = LastHiter.CalcGetExp(WAbil.Level, FightExp);
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
                if (M2Share.Config.MonSayMsg && Race == ActorRace.Play && LastHiter != null)
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
                    if (Race == ActorRace.Play && LastHiter != null && PvpLevel() < 2)
                    {
                        if ((LastHiter.Race == ActorRace.Play) || (LastHiter.Race == ActorRace.NPC))//允许NPC杀死人物
                        {
                            boPK = true;
                        }
                        if (LastHiter.Master != null)
                        {
                            if (LastHiter.Master.Race == ActorRace.Play)
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
                        if ((M2Share.Config.IsKillHumanWinLevel || M2Share.Config.IsKillHumanWinExp || Envir.Flag.boPKWINLEVEL || Envir.Flag.boPKWINEXP) && LastHiter.Race == ActorRace.Play)
                        {
                            (this as PlayObject).PKDie(LastHiter as PlayObject);
                        }
                        else
                        {
                            if (!LastHiter.IsGoodKilling(this))
                            {
                                LastHiter.IncPkPoint(M2Share.Config.KillHumanAddPKPoint);
                                LastHiter.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                                SysMsg(Format(M2Share.g_sYouKilledByMsg, LastHiter.ChrName), MsgColor.Red, MsgType.Hint);
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
                        if (LastHiter.Race == ActorRace.Play)
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
                    if (Race != ActorRace.Play)
                    {
                        DropUseItems(AttackBaseObject);
                        if (Master == null && (!NoItem || !Envir.Flag.boNODROPITEM))
                        {
                            ScatterBagItems(AttackBaseObject);
                        }
                        if (Race >= ActorRace.Animal && Master == null && (!NoItem || !Envir.Flag.boNODROPITEM))
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
                                if (M2Share.Config.KillByHumanDropUseItem && AttackBaseObject.Race == ActorRace.Play || M2Share.Config.KillByMonstDropUseItem && AttackBaseObject.Race != ActorRace.Play)
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
                        AddBodyLuck(-(50 - (50 - WAbil.Level * 5)));
                    }
                }
                string tStr;
                if (Envir.Flag.boFight3Zone)
                {
                    FightZoneDieCount++;
                    if (MyGuild != null)
                    {
                        MyGuild.TeamFightWhoDead(ChrName);
                    }
                    if (LastHiter != null)
                    {
                        if (LastHiter.MyGuild != null && MyGuild != null)
                        {
                            LastHiter.MyGuild.TeamFightWhoWinPoint(LastHiter.ChrName, 100);
                            tStr = LastHiter.MyGuild.sGuildName + ':' + LastHiter.MyGuild.nContestPoint + "  " + MyGuild.sGuildName + ':' + MyGuild.nContestPoint;
                            M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, Envir, CurrX, CurrY, 1000, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, "- " + tStr);
                        }
                    }
                }
                if (Race == ActorRace.Play)
                {
                    if (GroupOwner != null)
                    {
                        GroupOwner.DelMember(this);// 人物死亡立即退组，以防止组队刷经验
                    }
                    if (LastHiter != null)
                    {
                        if (LastHiter.Race == ActorRace.Play)
                        {
                            tStr = LastHiter.ChrName;
                        }
                        else
                        {
                            tStr = '#' + LastHiter.ChrName;
                        }
                    }
                    else
                    {
                        tStr = "####";
                    }
                    M2Share.EventSource.AddEventLog(GameEventLogType.PlayDie,MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + "FZ-" + HUtil32.BoolToIntStr(Envir.Flag.boFightZone) + "_F3-" + HUtil32.BoolToIntStr(Envir.Flag.boFight3Zone) + "\t" + '0' + "\t" + '1' + "\t" + tStr);
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
                return true;
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
                        if (!BaseObject.PvpFlag && BaseObject.WAbil.Level <= M2Share.Config.nPKProtectLevel && BaseObject.PvpLevel() < 2)// 被攻击的人物小指定等级没有红名，则不可以攻击。
                        {
                            return false;
                        }
                    }
                    if (Abil.Level <= M2Share.Config.nPKProtectLevel)// 如果小于指定等级
                    {
                        if (!BaseObject.PvpFlag && BaseObject.WAbil.Level > M2Share.Config.nPKProtectLevel && BaseObject.PvpLevel() < 2)
                        {
                            return false;
                        }
                    }
                }
                // 大于指定级别的红名人物不可以杀指定级别未红名的人物。
                if (PvpLevel() >= 2 && Abil.Level > M2Share.Config.nRedPKProtectLevel)
                {
                    if (BaseObject.Abil.Level <= M2Share.Config.nRedPKProtectLevel && BaseObject.PvpLevel() < 2)
                    {
                        return false;
                    }
                }
                // 小于指定级别的非红名人物不可以杀指定级别红名人物。
                if (Abil.Level <= M2Share.Config.nRedPKProtectLevel && PvpLevel() < 2)
                {
                    if (BaseObject.PvpLevel() >= 2 && BaseObject.Abil.Level > M2Share.Config.nRedPKProtectLevel)
                    {
                        return false;
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
            string sChrName;
            if (Race == ActorRace.Play)
            {
                sChrName = ChrName;
            }
            else
            {
                sChrName = M2Share.FilterShowName(ChrName);
            }
            SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, sChrName + ':' + sMsg);
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
                if ((Race == ActorRace.PlayClone) && (Master != null))
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
            IList<DeleteItem> DropItemList;
            const string sExceptionMsg = "[Exception] BaseObject::DropUseItems";
            const byte MaxUseItem = 8;
            try
            {
                if (NoDropUseItem)
                {
                    return;
                }
                DropItemList = new List<DeleteItem>();
                if (Race == ActorRace.Play)
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
                                DropItemList.Add(new DeleteItem()
                                {
                                    MakeIndex = UseItems[nC].MakeIndex
                                });
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(16, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + StdItem.Name + "\t" + UseItems[nC].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == ActorRace.Play) + "\t" + '0');
                                }
                                UseItems[nC].Index = 0;
                            }
                        }
                        nC++;
                        if (nC >= MaxUseItem)
                        {
                            break;
                        }
                    }
                }
                nRate = PvpLevel() > 2 ? 15 : 30; //掉落几率
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
                                    if (Race == ActorRace.Play)
                                    {
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
                    if (nC >= MaxUseItem)
                    {
                        break;
                    }
                }
                if (DropItemList.Count > 0)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, DropItemList);
                    SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(sExceptionMsg + $" 地图名称:{Envir.MapName} 怪物名称:{ChrName} X:{CurrX} Y:{CurrY}");
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
            switch (Race)
            {
                case >= ActorRace.Animal:
                    {
                        if (BaseObject.Race >= ActorRace.Animal)
                        {
                            result = true;
                        }
                        if (BaseObject.Master != null)
                        {
                            result = false;
                        }
                        return result;
                    }
                case ActorRace.Play:
                    {
                        result = IsProperIsFriend(BaseObject);
                        if (BaseObject.Race < ActorRace.Animal)
                        {
                            return result;
                        }
                        if (BaseObject.Master == this)
                        {
                            return true;
                        }
                        if (BaseObject.Master != null)
                        {
                            return IsProperIsFriend(BaseObject.Master);
                        }
                        break;
                    }
                default:
                    result = true;
                    break;
            }
            return result;
        }

        protected virtual bool Operate(ProcessMessage processMsg)
        {
            int nTargetX;
            int nTargetY;
            int nPower;
            int nRage;
            BaseObject TargetBaseObject;
            const string sExceptionMsg = "[Exception] TBaseObject::Operate ";
            try
            {
                switch (processMsg.wIdent)
                {
                    case Grobal2.RM_MAGSTRUCK:
                    case Grobal2.RM_MAGSTRUCK_MINE:
                        if ((processMsg.wIdent == Grobal2.RM_MAGSTRUCK) && (Race >= ActorRace.Animal) && !RushMode && (WAbil.Level < 50))
                        {
                            WalkTick = WalkTick + 800 + M2Share.RandomNumber.Random(1000);
                        }
                        var nDamage = GetMagStruckDamage(null, processMsg.nParam1);
                        if (nDamage > 0)
                        {
                            StruckDamage(nDamage);
                            HealthSpellChanged();
                            SendRefMsg(Grobal2.RM_STRUCK_MAG, nDamage, WAbil.HP, WAbil.MaxHP, processMsg.BaseObject, "");
                            TargetBaseObject = M2Share.ActorMgr.Get(processMsg.BaseObject);
                            if (M2Share.Config.MonDelHptoExp)
                            {
                                switch (TargetBaseObject.Race)
                                {
                                    case ActorRace.Play:
                                        if (TargetBaseObject.WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(ChrName))
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
                                        break;
                                    case ActorRace.PlayClone:
                                        if (TargetBaseObject.Master != null)
                                        {
                                            if (TargetBaseObject.Master.WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                            {
                                                if (!M2Share.GetNoHptoexpMonList(ChrName))
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
                                        break;
                                }
                            }
                            if (Race != ActorRace.Play)
                            {
                                if (Animal)
                                {
                                    MeatQuality -= (ushort)(nDamage * 1000);
                                }
                                SendMsg(this, Grobal2.RM_STRUCK, nDamage, WAbil.HP, WAbil.MaxHP, processMsg.BaseObject, "");
                            }
                        }
                        if (FastParalysis)
                        {
                            StatusArr[PoisonState.STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_MAGHEALING:
                        if ((IncHealing + processMsg.nParam1) < 300)
                        {
                            if (Race == ActorRace.Play)
                            {
                                IncHealing += processMsg.nParam1;
                                PerHealing = 5;
                            }
                            else
                            {
                                IncHealing += processMsg.nParam1;
                                PerHealing = 5;
                            }
                        }
                        else
                        {
                            IncHealing = 300;
                        }
                        break;
                    case Grobal2.RM_REFMESSAGE:
                        SendRefMsg(processMsg.BaseObject, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg);
                        if ((processMsg.BaseObject == Grobal2.RM_STRUCK) && (Race != ActorRace.Play))
                        {
                            SendMsg(this, processMsg.BaseObject, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg);
                        }
                        if (FastParalysis)
                        {
                            StatusArr[PoisonState.STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_DELAYMAGIC:
                        nPower = processMsg.wParam;
                        nTargetX = HUtil32.LoWord(processMsg.nParam1);
                        nTargetY = HUtil32.HiWord(processMsg.nParam1);
                        nRage = processMsg.nParam2;
                        TargetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if ((TargetBaseObject != null) && (TargetBaseObject.GetMagStruckDamage(this, nPower) > 0))
                        {
                            SetTargetCreat(TargetBaseObject);
                            if (TargetBaseObject.Race >= ActorRace.Animal)
                            {
                                nPower = HUtil32.Round(nPower / 1.2);
                            }
                            if ((Math.Abs(nTargetX - TargetBaseObject.CurrX) <= nRage) && (Math.Abs(nTargetY - TargetBaseObject.CurrY) <= nRage))
                            {
                                TargetBaseObject.SendMsg(this, Grobal2.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                            }
                        }
                        break;
                    case Grobal2.RM_RANDOMSPACEMOVE:
                        MapRandomMove(processMsg.Msg, processMsg.wParam);
                        break;
                    case Grobal2.RM_DELAYPUSHED:
                        nPower = processMsg.wParam;
                        nTargetX = HUtil32.LoWord(processMsg.nParam1);
                        nTargetY = HUtil32.HiWord(processMsg.nParam1);
                        nRage = processMsg.nParam2;
                        TargetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if (TargetBaseObject != null)
                        {
                            TargetBaseObject.CharPushed((byte)nPower, nRage);
                        }
                        break;
                    case Grobal2.RM_POISON:
                        TargetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam2);
                        if (TargetBaseObject != null)
                        {
                            if (IsProperTarget(TargetBaseObject))
                            {
                                SetTargetCreat(TargetBaseObject);
                                if ((Race == ActorRace.Play) && (TargetBaseObject.Race == ActorRace.Play))
                                {
                                    SetPkFlag(TargetBaseObject);
                                }
                                SetLastHiter(TargetBaseObject);
                            }
                            MakePosion(processMsg.wParam, (ushort)processMsg.nParam1, processMsg.nParam3);// 中毒类型
                        }
                        else
                        {
                            MakePosion(processMsg.wParam, (ushort)processMsg.nParam1, processMsg.nParam3);// 中毒类型
                        }
                        break;
                    case Grobal2.RM_TRANSPARENT:
                        M2Share.MagicMgr.MagMakePrivateTransparent(this, (ushort)processMsg.nParam1);
                        break;
                    case Grobal2.RM_DOOPENHEALTH:
                        MakeOpenHealth();
                        break;
                    default:
                        M2Share.Log.Warn(string.Format("人物: {0} 消息: Ident {1} Param {2} P1 {3} P2 {3} P3 {4} Msg {5}", ChrName, processMsg.wIdent, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg));
                        break;
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
            return false;
        }

        public virtual string GetShowName()
        {
            var sShowName = ChrName;
            var result = M2Share.FilterShowName(sShowName);
            if ((Master != null) && !Master.ObMode)
            {
                result = result + '(' + Master.ChrName + ')';
            }
            return result;
        }
    }
}