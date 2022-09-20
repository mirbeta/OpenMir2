using GameSvr.Items;
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
                        if ((IncHealth == 0) && (UseItems[Grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > M2Share.Config.HPStoneIntervalTime) && ((Abil.HP / Abil.MaxHP * 100) < M2Share.Config.HPStoneStartRate))
                        {
                            IncHpStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].wIndex);
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
                                    UseItems[Grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[Grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > M2Share.Config.MpStoneIntervalTime) && ((Abil.MP / Abil.MaxMP * 100) < M2Share.Config.MPStoneStartRate))
                        {
                            IncMpStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].wIndex);
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
                    if ((StatusTimeArr[i] > 0) && (StatusTimeArr[i] < 60000))
                    {
                        if ((HUtil32.GetTickCount() - StatusArrTick[i]) > 1000)
                        {
                            StatusTimeArr[i] -= 1;
                            StatusArrTick[i] += 1000;
                            if (StatusTimeArr[i] == 0)
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
                for (var i = 0; i < StatusArrValue.Length; i++)
                {
                    if (StatusArrValue[i] > 0)
                    {
                        if (HUtil32.GetTickCount() > StatusArrTimeOutTick[i])
                        {
                            StatusArrValue[i] = 0;
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
                if ((HUtil32.GetTickCount() - PoisoningTick) > M2Share.Config.PosionDecHealthTime)
                {
                    PoisoningTick = HUtil32.GetTickCount();
                    if (StatusTimeArr[Grobal2.POISON_DECHEALTH] > 0)
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
                    var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
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
                        StdItem = M2Share.WorldEngine.GetStdItem(UseItems[nC].wIndex);
                        if (StdItem != null)
                        {
                            if ((StdItem.Reserved & 8) != 0)
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
                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[nC].wIndex);
                            if (StdItem != null)
                            {
                                if ((StdItem.Reserved & 10) == 0)
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (DropItemList == null)
                                        {
                                            DropItemList = new List<DeleteItem>();
                                        }
                                        DropItemList.Add(new DeleteItem()
                                        {
                                            ItemName = M2Share.WorldEngine.GetStdItemName(UseItems[nC].wIndex),
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
                            StatusTimeArr[Grobal2.POISON_STONE] = 1;
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
                            StatusTimeArr[Grobal2.POISON_STONE] = 1;
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
            AddAbil = new TAddAbility();
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
            MDb3B0 = 0;
            HongMoSuite = 0;
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
                var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].wIndex);
                if (StdItem == null)
                {
                    continue;
                }
                StdItem.ApplyItemParameters(ref AddAbil);
                if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND) || (i == Grobal2.U_DRESS))
                {
                    if (i == Grobal2.U_DRESS)
                    {
                        Abil.WearWeight += StdItem.Weight;
                    }
                    else
                    {
                        Abil.HandWeight += StdItem.Weight;
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
                        StatusTimeArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
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
                        MoXieSuite += StdItem.Weight / 10;
                    }
                    if (StdItem.AniCount == 138)
                    {
                        HongMoSuite += StdItem.Weight;
                    }
                    if (StdItem.AniCount == 139)
                    {
                        UnParalysis = true;
                    }
                    if (StdItem.AniCount == 140)
                    {
                        SuperManItem = true;
                    }
                    if (StdItem.AniCount == 141)
                    {
                        BoExpItem = true;
                        ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.ItemExpRate);
                    }
                    if (StdItem.AniCount == 142)
                    {
                        BoPowerItem = true;
                        PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.ItemPowerRate);
                    }
                    if (StdItem.AniCount == 182)
                    {
                        BoExpItem = true;
                        ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.ItemExpRate);
                    }
                    if (StdItem.AniCount == 183)
                    {
                        BoPowerItem = true;
                        PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.ItemPowerRate);
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
                        PkDieLostExp = StdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                    }
                    if (StdItem.AniCount == 181)
                    {
                        // PK 死亡掉等级
                        PkDieLostLevel = StdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                    }
                    // 新增结束
                }
                else
                {
                    Abil.WearWeight += StdItem.Weight;
                }
                Abil.Weight += StdItem.Weight;
                if (i == Grobal2.U_WEAPON)
                {
                    if ((StdItem.Source - 1 - 10) < 0)
                    {
                        AddAbil.btWeaponStrong = (byte)StdItem.Source;// 强度+
                    }
                    if ((StdItem.Source <= -1) && (StdItem.Source >= -50))  // -1 to -50
                    {
                        AddAbil.btUndead = (byte)(AddAbil.btUndead + -StdItem.Source);// Holy+
                    }
                    if ((StdItem.Source <= -51) && (StdItem.Source >= -100))// -51 to -100
                    {
                        AddAbil.btUndead = (byte)(AddAbil.btUndead + (StdItem.Source + 50));// Holy-
                    }
                    continue;
                }
                if (i == Grobal2.U_RIGHTHAND)
                {
                    if (StdItem.Shape >= 1 && StdItem.Shape <= 50)
                    {
                        DressEffType = StdItem.Shape;
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
                        DressEffType = UseItems[i].btValue[5];
                    }
                    if (StdItem.AniCount > 0)
                    {
                        DressEffType = StdItem.AniCount;
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
                    SuperManItem = true;
                }
                if (StdItem.Shape == 141)
                {
                    BoExpItem = true;
                    ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.ItemExpRate);
                }
                if (StdItem.Shape == 142)
                {
                    BoPowerItem = true;
                    PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.ItemPowerRate);
                }
                if (StdItem.Shape == 182)
                {
                    BoExpItem = true;
                    ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.ItemExpRate);
                }
                if (StdItem.Shape == 183)
                {
                    BoPowerItem = true;
                    PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.ItemPowerRate);
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
                    PkDieLostExp = StdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                }
                if (StdItem.Shape == 181)
                {
                    // PK 死亡掉等级
                    PkDieLostLevel = StdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
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
                    MoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 138)
                {
                    boHongMoSuite1 = true;
                    HongMoSuite += StdItem.AniCount;
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
                    StatusTimeArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
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
                    MoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 136)
                {
                    boHongMoSuite2 = true;
                    HongMoSuite += StdItem.AniCount;
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
                    AddAbil.btUndead = (byte)(AddAbil.btUndead + -StdItem.Source);
                    // Holy+
                }
                if ((StdItem.Source <= -51) && (StdItem.Source >= -100))
                {
                    // -51 to -100
                    AddAbil.btUndead = (byte)(AddAbil.btUndead + (StdItem.Source + 50));
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
                    MoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 137)
                {
                    boHongMoSuite3 = true;
                    HongMoSuite += StdItem.AniCount;
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
                RecallSuite = true;
            }
            if (boMoXieSuite[0] && boMoXieSuite[1] && boMoXieSuite[2])
            {
                MoXieSuite += 50;
            }
            if (boHongMoSuite1 && boHongMoSuite2 && boHongMoSuite3)
            {
                AddAbil.wHitPoint += 2;
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
            if (Transparent && (StatusTimeArr[Grobal2.STATE_TRANSPARENT] > 0))
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
                    StatusTimeArr[Grobal2.STATE_TRANSPARENT] = 0;
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
            SpeedPoint += (byte)AddAbil.wSpeedPoint;
            HitPoint += (byte)AddAbil.wHitPoint;
            AntiPoison += (byte)AddAbil.wAntiPoison;
            PoisonRecover += AddAbil.wPoisonRecover;
            HealthRecover += AddAbil.wHealthRecover;
            SpellRecover += AddAbil.wSpellRecover;
            AntiMagic += AddAbil.wAntiMagic;
            Luck += AddAbil.btLuck;
            Luck -= AddAbil.btUnLuck;
            HitSpeed = AddAbil.nHitSpeed;
            Abil.MaxWeight += AddAbil.Weight;
            Abil.MaxWearWeight += (byte)AddAbil.WearWeight;
            Abil.MaxHandWeight += (byte)AddAbil.HandWeight;
            Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + AddAbil.wHP);
            Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxMP + AddAbil.wMP);
            Abil.AC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.wAC) + HUtil32.LoWord(Abil.AC), HUtil32.HiWord(AddAbil.wAC) + HUtil32.HiWord(Abil.AC));
            Abil.MAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.wMAC) + HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(AddAbil.wMAC) + HUtil32.HiWord(Abil.MAC));
            Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.wDC) + HUtil32.LoWord(Abil.DC), HUtil32.HiWord(AddAbil.wDC) + HUtil32.HiWord(Abil.DC));
            Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.wMC) + HUtil32.LoWord(Abil.MC), HUtil32.HiWord(AddAbil.wMC) + HUtil32.HiWord(Abil.MC));
            Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.wSC) + HUtil32.LoWord(Abil.SC), HUtil32.HiWord(AddAbil.wSC) + HUtil32.HiWord(Abil.SC));
            if (StatusTimeArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                Abil.AC = HUtil32.MakeLong(HUtil32.LoWord(Abil.AC), HUtil32.HiWord(Abil.AC) + 2 + (Abil.Level / 7));
            }
            if (StatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                Abil.MAC = HUtil32.MakeLong(HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(Abil.MAC) + 2 + (Abil.Level / 7));
            }
            if (StatusArrValue[0] > 0)
            {
                Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC), HUtil32.HiWord(Abil.DC) + 2 + StatusArrValue[0]);
            }
            if (StatusArrValue[1] > 0)
            {
                Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(Abil.MC), HUtil32.HiWord(Abil.MC) + 2 + StatusArrValue[1]);
            }
            if (StatusArrValue[2] > 0)
            {
                Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(Abil.SC), HUtil32.HiWord(Abil.SC) + 2 + StatusArrValue[2]);
            }
            if (StatusArrValue[3] > 0)
            {
                HitSpeed += StatusArrValue[3];
            }
            if (StatusArrValue[4] > 0)
            {
                Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + StatusArrValue[4]);
            }
            if (StatusArrValue[5] > 0)
            {
                Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxMP + StatusArrValue[5]);
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
                Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + MoXieSuite);
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
                AddAbil.btUndead = (byte)(AddAbil.btUndead + -3);
                Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(Abil.SC) + 1, HUtil32.HiWord(Abil.SC) + 2 + 2);
            }
            if (MundaneSet)
            {
                // Bonus of Hp+50
                Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + 50);
            }
            if (NokChiSet)
            {
                // Bonus of Mp+50
                Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxMP + 50);
            }
            if (TaoBuSet)
            {
                // Bonus of Hp+30, Mp+30
                Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + 30);
                Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxMP + 30);
            }
            if (FiveStringSet)
            {
                // Bonus of Hp +30%, Ac+2
                Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP / 100 * 30);
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
    }
}