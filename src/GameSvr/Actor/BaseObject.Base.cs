using GameSvr.Items;
using GameSvr.Player;
using GameSvr.RobotPlay;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        public virtual void Run()
        {
            const string sExceptionMsg0 = "[Exception] TBaseObject::Run 0";
            const string sExceptionMsg1 = "[Exception] TBaseObject::Run 1";
            const string sExceptionMsg2 = "[Exception] TBaseObject::Run 2";
            const string sExceptionMsg3 = "[Exception] TBaseObject::Run 3";
            const string sExceptionMsg4 = "[Exception] TBaseObject::Run 4 Code:{0}";
            const string sExceptionMsg5 = "[Exception] TBaseObject::Run 5";
            const string sExceptionMsg6 = "[Exception] TBaseObject::Run 6";
            try
            {
                while (GetMessage(out var ProcessMsg))
                {
                    Operate(ProcessMsg);
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg0);
                M2Share.Log.Error(e.StackTrace);
            }
            if (SuperMan)
            {
                WAbil.HP = WAbil.MaxHP;
                WAbil.MP = WAbil.MaxMP;
            }
            try
            {
                if (!Death)
                {
                    var recoveryTick = (HUtil32.GetTickCount() - AutoRecoveryTick) / 20;
                    AutoRecoveryTick = HUtil32.GetTickCount();
                    HealthTick += recoveryTick;
                    SpellTick += recoveryTick;
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
                        if (((LastHiter == null) || LastHiter.Race == ActorRace.Play && !((PlayObject)LastHiter).UnRevival))
                        {
                            if (Race == ActorRace.Play && ((PlayObject)this).Revival && ((HUtil32.GetTickCount() - RevivalTick) > M2Share.Config.RevivalTime))
                            {
                                RevivalTick = HUtil32.GetTickCount();
                                ItemDamageRevivalRing();
                                WAbil.HP = WAbil.MaxHP;
                                HealthSpellChanged();
                                SysMsg(M2Share.g_sRevivalRecoverMsg, MsgColor.Green, MsgType.Hint);
                            }
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
                    var dwInChsTime = 600 - HUtil32._MIN(400, WAbil.Level * 10);
                    if (((HUtil32.GetTickCount() - IncHealthSpellTick) >= dwInChsTime) && !Death)
                    {
                        var dwC = HUtil32._MIN(200, HUtil32.GetTickCount() - IncHealthSpellTick - dwInChsTime);
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
                            PerHealth = (byte)(WAbil.Level / 10 + 5);
                            PerSpell = (byte)(WAbil.Level / 10 + 5);
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
                var boNeedRecalc = false;
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
            LifeStone();
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
                if ((HUtil32.GetTickCount() - DecLightItemDrugTick) > M2Share.Config.DecLightItemDrugTime)
                {
                    DecLightItemDrugTick += M2Share.Config.DecLightItemDrugTime;
                    if (Race == ActorRace.Play)
                    {
                        UseLamp();
                        ((PlayObject)this).CheckPkStatus();
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
                var boChg = false;
                var boNeedRecalc = false;
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
        }

        public virtual void Die()
        {
            if (SuperMan)
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
            if (LastHiter != null && Race != ActorRace.Play)
            {
                MonsterSayMsg(LastHiter, MonStatus.Die);
            }
            else if (LastHiter != null && Race == ActorRace.Play)
            {
                LastHiter.MonsterSayMsg(this, MonStatus.KillHuman);
            }
            Master = null;
            if (Master == null && !DelFormMaped) // 减少地图上的计数
            {
                Envir.DelObjectCount(this);
                DelFormMaped = true;
            }
            SendRefMsg(Grobal2.RM_DEATH, Direction, CurrX, CurrY, 1, "");
        }

        /// <summary>
        /// 气血石和魔血石处理
        /// </summary>
        private void LifeStone()
        {
            if (!Death && Race == ActorRace.Play || Race == ActorRace.PlayClone)
            {
                if (UseItems.Length >= Grobal2.U_CHARM && UseItems[Grobal2.U_CHARM] != null && UseItems[Grobal2.U_CHARM].Index > 0)
                {
                    var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].Index);
                    if ((StdItem.StdMode == 7) && (StdItem.Shape == 2 || StdItem.Shape == 3))
                    {
                        ushort stoneDura;
                        ushort dCount;
                        ushort bCount;
                        // 加HP
                        if ((IncHealth == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > M2Share.Config.HPStoneIntervalTime) && ((WAbil.HP / WAbil.MaxHP * 100) < M2Share.Config.HPStoneStartRate))
                        {
                            IncHpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[Grobal2.U_CHARM].Dura * 10);
                            bCount = (ushort)(stoneDura / M2Share.Config.HPStoneAddRate);
                            dCount = (ushort)(WAbil.MaxHP - WAbil.HP);
                            if (dCount > bCount)
                            {
                                dCount = bCount;
                            }
                            if (stoneDura > dCount)
                            {
                                IncHealth += dCount;
                                UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                            }
                            else
                            {
                                stoneDura = 0;
                                IncHealth += stoneDura;
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
                                    ((PlayObject)this).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                }
                                UseItems[Grobal2.U_CHARM].Index = 0;
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > M2Share.Config.MpStoneIntervalTime) && ((WAbil.MP / WAbil.MaxMP * 100) < M2Share.Config.MPStoneStartRate))
                        {
                            IncMpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[Grobal2.U_CHARM].Dura * 10);
                            bCount = (ushort)(stoneDura / M2Share.Config.MPStoneAddRate);
                            dCount = (ushort)(WAbil.MaxMP - WAbil.MP);
                            if (dCount > bCount)
                            {
                                dCount = bCount;
                            }
                            if (stoneDura > dCount)
                            {
                                IncSpell += dCount;
                                UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                            }
                            else
                            {
                                stoneDura = 0;
                                IncSpell += stoneDura;
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
                                    ((PlayObject)this).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                }
                                UseItems[Grobal2.U_CHARM].Index = 0;
                            }
                        }
                    }
                }
            }
        }

        private void KillTarget()
        {
            if (ExpHitter != null && ExpHitter.Master != null)//如果是角色下属杀死对象
            {
                ((PlayObject)ExpHitter.Master).KillTargetTrigger(this);
                return;
            }
            if (ExpHitter != null && ExpHitter.Race == ActorRace.Play)
            {
                ((PlayObject)ExpHitter).KillTargetTrigger(this);
            }
        }

        private void KillFunc()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::KillFunc";
            try
            {
                KillTarget();
                if ((M2Share.g_FunctionNPC != null) && (Envir != null) && Envir.Flag.boKILLFUNC)
                {
                    if (Race != ActorRace.Play)
                    {
                        if (ExpHitter != null)
                        {
                            if (ExpHitter.Race == ActorRace.Play)
                            {
                                M2Share.g_FunctionNPC.GotoLable(ExpHitter as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                            }

                            if (ExpHitter.Master != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(ExpHitter.Master as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                            }
                        }
                        else
                        {
                            if (LastHiter != null)
                            {
                                if (LastHiter.Race == ActorRace.Play)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(LastHiter as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                                }
                                if (LastHiter.Master != null)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(LastHiter.Master as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((LastHiter != null) && (LastHiter.Race == ActorRace.Play))
                        {
                            M2Share.g_FunctionNPC.GotoLable(LastHiter as PlayObject, "@KillPlay" + Envir.Flag.nKILLFUNCNO, false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
        }

        internal virtual void ReAlive()
        {
            Death = false;
            SendRefMsg(Grobal2.RM_ALIVE, Direction, CurrX, CurrY, 0, "");
        }

        protected virtual bool IsProtectTarget(BaseObject targetObject)
        {
            var result = true;
            if (targetObject == null)
            {
                return true;
            }
            if (InSafeZone() || targetObject.InSafeZone())
            {
                result = false;
            }
            return result;
        }

        protected virtual void ProcessSayMsg(string sMsg)
        {
            var sChrName = Race == ActorRace.Play ? ChrName : M2Share.FilterShowName(ChrName);
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
        internal virtual void ScatterBagItems(int itemOfCreat)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::ScatterBagItems";
            try
            {
                if ((Race == ActorRace.PlayClone) && (Master != null))
                {
                    return;
                }
                var dropWide = HUtil32._MIN(M2Share.Config.DropItemRage, 7);
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    var StdItem = M2Share.WorldEngine.GetStdItem(ItemList[i].Index);
                    var boCanNotDrop = false;
                    if (StdItem != null)
                    {
                        if (M2Share.g_MonDropLimitLIst.TryGetValue(StdItem.Name, out var MonDrop))
                        {
                            if (MonDrop.DropCount < MonDrop.CountLimit)
                            {
                                MonDrop.DropCount++;
                                M2Share.g_MonDropLimitLIst[StdItem.Name] = MonDrop;
                            }
                            else
                            {
                                MonDrop.NoDropCount++;
                                boCanNotDrop = true;
                            }
                            break;
                        }
                    }
                    if (boCanNotDrop)
                    {
                        continue;
                    }
                    if (DropItemDown(ItemList[i], dropWide, true, itemOfCreat, this.ActorId))
                    {
                        Dispose(ItemList[i]);
                        ItemList.RemoveAt(i);
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
        }

        internal virtual void DropUseItems(int BaseObject)
        {

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
                for (var i = 0; i < VisibleActors.Count; i++)
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

        public virtual bool IsProperFriend(BaseObject attackTarget)
        {
            var result = false;
            if (attackTarget == null)
            {
                return false;
            }
            if (Race >= ActorRace.Animal)
            {
                if (attackTarget.Race >= ActorRace.Animal)
                {
                    result = true;
                }
                if (attackTarget.Master != null)
                {
                    result = false;
                }
                return result;
            }
            return result;
        }

        protected virtual bool Operate(ProcessMessage processMsg)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::Operate ";
            try
            {
                BaseObject targetBaseObject;
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
                            targetBaseObject = M2Share.ActorMgr.Get(processMsg.BaseObject);
                            if (M2Share.Config.MonDelHptoExp)
                            {
                                switch (targetBaseObject.Race)
                                {
                                    case ActorRace.Play:
                                        if (targetBaseObject.WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(ChrName))
                                            {
                                                if (targetBaseObject.IsRobot)
                                                {
                                                    ((RobotPlayObject)targetBaseObject).GainExp(GetMagStruckDamage(targetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                                }
                                                else
                                                {
                                                    ((PlayObject)targetBaseObject).GainExp(GetMagStruckDamage(targetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                                }
                                            }
                                        }
                                        break;
                                    case ActorRace.PlayClone:
                                        if (targetBaseObject.Master != null)
                                        {
                                            if (targetBaseObject.Master.WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                            {
                                                if (!M2Share.GetNoHptoexpMonList(ChrName))
                                                {
                                                    if (targetBaseObject.Master.IsRobot)
                                                    {
                                                        ((RobotPlayObject)targetBaseObject.Master).GainExp(GetMagStruckDamage(targetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
                                                    }
                                                    else
                                                    {
                                                        ((PlayObject)targetBaseObject.Master).GainExp(GetMagStruckDamage(targetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
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
                                IncHealing += (ushort)processMsg.nParam1;
                                PerHealing = 5;
                            }
                            else
                            {
                                IncHealing += (ushort)processMsg.nParam1;
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
                        var nPower = processMsg.wParam;
                        var nTargetX = HUtil32.LoWord(processMsg.nParam1);
                        var nTargetY = HUtil32.HiWord(processMsg.nParam1);
                        var nRage = processMsg.nParam2;
                        targetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if ((targetBaseObject != null) && (targetBaseObject.GetMagStruckDamage(this, nPower) > 0))
                        {
                            SetTargetCreat(targetBaseObject);
                            if (targetBaseObject.Race >= ActorRace.Animal)
                            {
                                nPower = HUtil32.Round(nPower / 1.2);
                            }
                            if ((Math.Abs(nTargetX - targetBaseObject.CurrX) <= nRage) && (Math.Abs(nTargetY - targetBaseObject.CurrY) <= nRage))
                            {
                                targetBaseObject.SendMsg(this, Grobal2.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                            }
                        }
                        break;
                    case Grobal2.RM_RANDOMSPACEMOVE:
                        MapRandomMove(processMsg.Msg, processMsg.wParam);
                        break;
                    case Grobal2.RM_DELAYPUSHED:
                        targetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if (targetBaseObject != null)
                        {
                            targetBaseObject.CharPushed((byte)processMsg.wParam, processMsg.nParam2);
                        }
                        break;
                    case Grobal2.RM_POISON:
                        targetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam2);
                        if (targetBaseObject != null)
                        {
                            if (IsProperTarget(targetBaseObject))
                            {
                                SetTargetCreat(targetBaseObject);
                                if ((Race == ActorRace.Play) && (targetBaseObject.Race == ActorRace.Play))
                                {
                                    ((PlayObject)this).SetPkFlag(targetBaseObject);
                                }
                                SetLastHiter(targetBaseObject);
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
            var result = M2Share.FilterShowName(ChrName);
            if ((Master != null) && !Master.ObMode)
            {
                result = result + '(' + Master.ChrName + ')';
            }
            return result;
        }
    }
}