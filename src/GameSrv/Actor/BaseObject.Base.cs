using GameSrv.Magic;
using GameSrv.Player;
using GameSrv.RobotPlay;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.Actor
{
    public partial class BaseObject
    {
        public virtual void Initialize()
        {
            AbilCopyToWAbil();
            AddtoMapSuccess = true;
            if (Envir.CanWalk(CurrX, CurrY, true) && AddToMap())
            {
                AddtoMapSuccess = false;
            }
            CharStatus = GetCharStatus();
        }

        public virtual void Run()
        {
            ProcessMessage processMessage = default;
            while (GetMessage(ref processMessage))
            {
                Operate(processMessage);
            }
            if (SuperMan)
            {
                WAbil.HP = WAbil.MaxHP;
                WAbil.MP = WAbil.MaxMP;
            }
            if (!Death)
            {
                int recoveryTick = (HUtil32.GetTickCount() - AutoRecoveryTick) / 20;
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
                    Die();
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
                    int makeGhostTime = HUtil32._MAX(10 * 1000, M2Share.WorldEngine.GetMonstersZenTime(MonGen.ZenTime) - 20 * 1000);
                    if (makeGhostTime > M2Share.Config.MakeGhostTime)
                    {
                        makeGhostTime = M2Share.Config.MakeGhostTime;
                    }
                    if (HUtil32.GetTickCount() - DeathTick > makeGhostTime)
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
            if (!Death && ((IncSpell > 0) || (IncHealth > 0) || (IncHealing > 0)))
            {
                int dwInChsTime = 600 - HUtil32._MIN(400, WAbil.Level * 10);
                if (((HUtil32.GetTickCount() - IncHealthSpellTick) >= dwInChsTime) && !Death)
                {
                    int incHealthTick = HUtil32._MIN(200, HUtil32.GetTickCount() - IncHealthSpellTick - dwInChsTime);
                    IncHealthSpellTick = HUtil32.GetTickCount() + incHealthTick;
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
            // 清理目标对象
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
                            nInteger = PoisonState.STATETRANSPARENT;
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
                            nInteger = PoisonState.STATETRANSPARENT;
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
                            nInteger = PoisonState.STATETRANSPARENT;
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
                            nInteger = PoisonState.STATETRANSPARENT;
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
            for (int i = SlaveList.Count - 1; i >= 0; i--)
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
            if ((HUtil32.GetTickCount() - CheckRoyaltyTick) > 10000)
            {
                CheckRoyaltyTick = HUtil32.GetTickCount();
                if (Master != null)
                {
                    if ((M2Share.SpiritMutinyTick > HUtil32.GetTickCount()) && (SlaveExpLevel < 5))
                    {
                        MasterRoyaltyTick = 0;
                    }
                    if (HUtil32.GetTickCount() > MasterRoyaltyTick)
                    {
                        for (int i = 0; i < Master.SlaveList.Count; i++)
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
                        if ((HUtil32.GetTickCount() - MasterTick) > 12 * 60 * 60 * 1000) //超过叛变时间则死亡
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
            bool boChg = false;
            boNeedRecalc = false;
            for (int i = 0; i < StatusArrTick.Length; i++)
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
                                case PoisonState.DefenceUP:
                                    boNeedRecalc = true;
                                    SysMsg("防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case PoisonState.MagDefenceUP:
                                    boNeedRecalc = true;
                                    SysMsg("魔法防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case PoisonState.STATETRANSPARENT:
                                    HideMode = false;
                                    break;
                            }
                        }
                        else if (StatusTimeArr[i] == 10)
                        {
                            if (i == PoisonState.DefenceUP)
                            {
                                SysMsg($"防御力{StatusTimeArr[i]}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                break;
                            }
                            if (i == PoisonState.MagDefenceUP)
                            {
                                SysMsg($"魔法防御力{StatusTimeArr[i]}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
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
                SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0, "");
            }
            if ((HUtil32.GetTickCount() - PoisoningTick) > M2Share.Config.PosionDecHealthTime)
            {
                PoisoningTick = HUtil32.GetTickCount();
                if (StatusTimeArr[PoisonState.DECHEALTH] > 0)
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
            SendRefMsg(Messages.RM_DEATH, Direction, CurrX, CurrY, 1, "");
        }

        internal virtual void ReAlive()
        {
            Death = false;
            SendRefMsg(Messages.RM_ALIVE, Direction, CurrX, CurrY, 0, "");
        }

        protected bool IsProtectTarget(BaseObject targetObject)
        {
            if (targetObject == null)
            {
                return true;
            }
            if (InSafeZone() || targetObject.InSafeZone())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否可以攻击的目标
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsAttackTarget(BaseObject baseObject)
        {
            bool result = false;
            if ((baseObject == null) || (baseObject == this))
            {
                return false;
            }
            if (baseObject.AdminMode || baseObject.StoneMode)
            {
                return false;
            }
            if (Race >= ActorRace.Animal)
            {
                if (Master != null)
                {
                    if ((Master.LastHiter == baseObject) || (Master.ExpHitter == baseObject) || (Master.TargetCret == baseObject))
                    {
                        result = true;
                    }
                    if (baseObject.TargetCret != null)
                    {
                        if ((baseObject.TargetCret == Master) || (baseObject.TargetCret.Master == Master) && (baseObject.Race != ActorRace.Play))
                        {
                            result = true;
                        }
                    }
                    if ((baseObject.TargetCret == this) && (baseObject.Race >= ActorRace.Animal))
                    {
                        result = true;
                    }
                    if (baseObject.Master != null)
                    {
                        if ((baseObject.Master == Master.LastHiter) || (baseObject.Master == Master.TargetCret))
                        {
                            result = true;
                        }
                    }
                    if (baseObject.Master == Master)
                    {
                        result = false;
                    }
                    if (baseObject.HolySeize)
                    {
                        result = false;
                    }
                    if (Master.SlaveRelax)
                    {
                        result = false;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (baseObject.InSafeZone())
                        {
                            result = false;
                        }
                    }
                    BreakCrazyMode();
                }
                else
                {
                    if (baseObject.Race == ActorRace.Play)
                    {
                        result = true;
                    }
                    if ((Race > ActorRace.PeaceNpc) && (Race < ActorRace.Animal))
                    {
                        result = true;
                    }
                    if (baseObject.Master != null)
                    {
                        result = true;
                    }
                }
                if (CrazyMode && ((baseObject.Race == ActorRace.Play) || (baseObject.Race > ActorRace.PeaceNpc)))
                {
                    result = true;
                }
                if (NastyMode && ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc)))
                {
                    result = true;
                }
                return result;
            }
            return true;
        }

        /// <summary>
        /// 检查对象是否可以被攻击
        /// </summary>
        /// <returns></returns>
        public virtual bool IsProperTarget(BaseObject baseObject)
        {
            return IsAttackTarget(baseObject);
        }

        protected virtual void ProcessSayMsg(string sMsg)
        {
            string sChrName = Race == ActorRace.Play ? ChrName : M2Share.FilterShowName(ChrName);
            SendRefMsg(Messages.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, sChrName + ':' + sMsg);
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
                int dropWide = HUtil32._MIN(M2Share.Config.DropItemRage, 7);
                for (int i = ItemList.Count - 1; i >= 0; i--)
                {
                    var stdItem = M2Share.WorldEngine.GetStdItem(ItemList[i].Index);
                    var boCanNotDrop = false;
                    if (stdItem != null)
                    {
                        if (M2Share.MonDropLimitLIst.TryGetValue(stdItem.Name, value: out MonsterLimitDrop monDrop))
                        {
                            if (monDrop.DropCount < monDrop.CountLimit)
                            {
                                monDrop.DropCount++;
                                M2Share.MonDropLimitLIst[stdItem.Name] = monDrop;
                            }
                            else
                            {
                                monDrop.NoDropCount++;
                                boCanNotDrop = true;
                            }
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
                M2Share.Logger.Error(sExceptionMsg);
            }
        }

        internal virtual void DropUseItems(int baseObject)
        {

        }

        public virtual void SetTargetCreat(BaseObject baseObject)
        {
            TargetCret = baseObject;
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
                    if (VisibleActors[i] == null)
                    {
                        continue;
                    }
                    if (VisibleActors[i].BaseObject == baseObject)
                    {
                        VisibleActors.RemoveAt(i);
                        break;
                    }
                }
            }
            DelTargetCreat();
        }

        public virtual bool IsProperFriend(BaseObject attackTarget)
        {
            bool result = false;
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
            return false;
        }

        protected virtual bool Operate(ProcessMessage processMsg)
        {
            const string sExceptionMsg = "[Exception] BaseObject::Operate ";
            try
            {
                BaseObject targetBaseObject;
                switch (processMsg.wIdent)
                {
                    case Messages.RM_MAGSTRUCK:
                    case Messages.RM_MAGSTRUCK_MINE:
                        if ((processMsg.wIdent == Messages.RM_MAGSTRUCK) && (Race >= ActorRace.Animal) && !RushMode && (WAbil.Level < 50))
                        {
                            WalkTick = WalkTick + 800 + M2Share.RandomNumber.Random(1000);
                        }
                        var nDamage = GetMagStruckDamage(null, processMsg.nParam1);
                        if (nDamage > 0)
                        {
                            StruckDamage(nDamage);
                            HealthSpellChanged();
                            SendRefMsg(Messages.RM_STRUCK_MAG, nDamage, WAbil.HP, WAbil.MaxHP, processMsg.ActorId, "");
                            if (M2Share.Config.MonDelHptoExp)
                            {
                                targetBaseObject = M2Share.ActorMgr.Get(processMsg.ActorId);
                                switch (targetBaseObject.Race)
                                {
                                    case ActorRace.Play:
                                        if (targetBaseObject.WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(ChrName))
                                            {
                                                if (targetBaseObject.IsRobot)
                                                {
                                                    ((RobotPlayer)targetBaseObject).GainExp(GetMagStruckDamage(targetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
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
                                                        ((RobotPlayer)targetBaseObject.Master).GainExp(GetMagStruckDamage(targetBaseObject, nDamage) * M2Share.Config.MonHptoExpmax);
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
                                SendMsg(Messages.RM_STRUCK, nDamage, WAbil.HP, WAbil.MaxHP, processMsg.ActorId, "");
                            }
                        }
                        if (FastParalysis)
                        {
                            StatusTimeArr[PoisonState.STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Messages.RM_MAGHEALING:
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
                    case Messages.RM_STRUCKEFFECT:
                        SendRefMsg(processMsg.ActorId, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg);
                        if (processMsg.ActorId == Messages.RM_STRUCK)
                        {
                            SendMsg(this, processMsg.ActorId, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg);
                        }
                        if (FastParalysis)
                        {
                            StatusTimeArr[PoisonState.STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Messages.RM_REFMESSAGE:
                        SendRefMsg(processMsg.ActorId, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg);
                        if ((processMsg.ActorId == Messages.RM_STRUCK) && (Race != ActorRace.Play))
                        {
                            SendMsg(processMsg.ActorId, processMsg.wParam, processMsg.nParam1, processMsg.nParam2, processMsg.nParam3, processMsg.Msg);
                        }
                        if (FastParalysis)
                        {
                            StatusTimeArr[PoisonState.STONE] = 1;
                            FastParalysis = false;
                        }
                        break;
                    case Messages.RM_DELAYMAGIC:
                        ushort nPower = (ushort)processMsg.wParam;
                        ushort nTargetX = HUtil32.LoWord(processMsg.nParam1);
                        ushort nTargetY = HUtil32.HiWord(processMsg.nParam1);
                        int nRage = processMsg.nParam2;
                        targetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if ((targetBaseObject != null) && (targetBaseObject.GetMagStruckDamage(this, nPower) > 0))
                        {
                            SetTargetCreat(targetBaseObject);
                            if (targetBaseObject.Race >= ActorRace.Animal)
                            {
                                nPower = (ushort)HUtil32.Round(nPower / 1.2);
                            }
                            if ((Math.Abs(nTargetX - targetBaseObject.CurrX) <= nRage) && (Math.Abs(nTargetY - targetBaseObject.CurrY) <= nRage))
                            {
                                targetBaseObject.SendMsg(this, Messages.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                            }
                        }
                        break;
                    case Messages.RM_RANDOMSPACEMOVE:
                        MapRandomMove(processMsg.Msg, processMsg.wParam);
                        break;
                    case Messages.RM_DELAYPUSHED:
                        targetBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if (targetBaseObject != null)
                        {
                            targetBaseObject.CharPushed((byte)processMsg.wParam, (byte)processMsg.nParam2);
                        }
                        break;
                    case Messages.RM_POISON:
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
                    case Messages.RM_TRANSPARENT:
                        MagicManager.MagMakePrivateTransparent(this, (ushort)processMsg.nParam1);
                        break;
                    case Messages.RM_DOOPENHEALTH:
                        MakeOpenHealth();
                        break;
                    case Messages.RM_DIEDROPITEM:
                        DieDropItems(processMsg.wParam);
                        break;
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
            return false;
        }

        public virtual string GetShowName()
        {
            string result = M2Share.FilterShowName(ChrName);
            if ((Master != null) && !Master.ObMode)
            {
                result = result + '(' + Master.ChrName + ')';
            }
            return result;
        }

        public virtual ushort GetHitStruckDamage(BaseObject target, int nDamage)
        {
            int nArmor;
            int nRnd = HUtil32.LoByte(WAbil.AC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.AC) - HUtil32.LoByte(WAbil.AC)) + 1);
            if (nRnd > 0)
            {
                nArmor = HUtil32.LoByte(WAbil.AC) + M2Share.RandomNumber.Random(nRnd);
            }
            else
            {
                nArmor = HUtil32.LoByte(WAbil.AC);
            }
            nDamage = HUtil32._MAX(0, nDamage - nArmor);
            if (nDamage > 0)
            {
                if ((LifeAttrib == Grobal2.LA_UNDEAD) && (target != null))
                {
                    nDamage += target.AddAbil.UndeadPower;
                }
            }
            return (ushort)nDamage;
        }

        public virtual int GetMagStruckDamage(BaseObject baseObject, int nDamage)
        {
            var n14 = HUtil32.LoByte(WAbil.MAC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.MAC) - HUtil32.LoByte(WAbil.MAC)) + 1);
            nDamage = (ushort)HUtil32._MAX(0, nDamage - n14);
            if ((LifeAttrib == Grobal2.LA_UNDEAD) && (baseObject != null))
            {
                nDamage += AddAbil.UndeadPower;
            }
            return nDamage;
        }

        /// <summary>
        /// 受攻击,减身上装备的持久
        /// </summary>
        /// <param name="nDamage"></param>
        public virtual void StruckDamage(int nDamage)
        {
            if (nDamage <= 0)
            {
                return;
            }
            if ((Race >= ActorRace.Animal) && (LastHiter != null) && (LastHiter.Race == ActorRace.Play)) // 人攻击怪物
            {
                switch (((PlayObject)LastHiter).Job)
                {
                    case PlayJob.Warrior:
                        nDamage = (ushort)(nDamage * M2Share.Config.WarrMon / 10);
                        break;
                    case PlayJob.Wizard:
                        nDamage = (ushort)(nDamage * M2Share.Config.WizardMon / 10);
                        break;
                    case PlayJob.Taoist:
                        nDamage = (ushort)(nDamage * M2Share.Config.TaosMon / 10);
                        break;
                }
            }
            if ((Race == ActorRace.Play) && (LastHiter != null) && (LastHiter.Master != null)) // 人物下属怪物攻击人
            {
                nDamage = (ushort)(nDamage * M2Share.Config.MonHum / 10);
            }
            if (StatusTimeArr[PoisonState.DAMAGEARMOR] > 0)
            {
                nDamage = (ushort)HUtil32.Round(nDamage * (M2Share.Config.PosionDamagarmor / 10.0)); // 1.2
            }
            DamageHealth(nDamage);
        }

        public virtual string GetBaseObjectInfo()
        {
            return ChrName + ' ' + "地图:" + MapName + '(' + Envir.MapDesc + ") " + "座标:" + CurrX +
                         '/' + CurrY + ' ' + "等级:" + Abil.Level + ' ' + "经验:" + Abil.Exp + ' ' + "生命值: " + WAbil.HP + '-' + WAbil.MaxHP + ' ' + "魔法值: " + WAbil.MP + '-' +
                         WAbil.MaxMP + ' ' + "攻击力: " + HUtil32.LoByte(WAbil.DC) + '-' +
                         HUtil32.HiByte(WAbil.DC) + ' ' + "魔法力: " + HUtil32.LoByte(WAbil.MC) + '-' + HUtil32.HiByte(WAbil.MC) + ' ' + "道术: " +
                         HUtil32.LoByte(WAbil.SC) + '-' + HUtil32.HiByte(WAbil.SC) + ' ' + "防御力: " + HUtil32.LoByte(WAbil.AC) + '-' + HUtil32.HiByte(WAbil.AC) + ' ' + "魔防力: " +
                         HUtil32.LoByte(WAbil.MAC) + '-' + HUtil32.HiByte(WAbil.MAC) + ' ' + "准确:" + HitPoint + ' ' + "敏捷:" + SpeedPoint;
        }

        protected virtual byte GetChrColor(BaseObject baseObject)
        {
            if (baseObject.Race == ActorRace.NPC) //增加NPC名字颜色单独控制
            {
                return M2Share.Config.NpcNameColor;
            }
            if (baseObject.CrazyMode)
            {
                return 0xF9;
            }
            if (baseObject.HolySeize)
            {
                return 0x7D;
            }
            if (baseObject.IsSlave && baseObject.SlaveExpLevel <= Grobal2.SlaveMaxLevel)
            {
                return M2Share.Config.SlaveColor[baseObject.SlaveExpLevel];
            }
            return baseObject.GetNameColor();
        }

        public virtual int GetAttackPower(int basePower, int power)
        {
            if (power < 0)
            {
                power = 0;
            }
            var result = basePower + M2Share.RandomNumber.Random(power + 1);
            if (AutoChangeColor)
            {
                result = result * AutoChangeIdx + 1;
            }
            if (FixColor)
            {
                result = result * FixColorIdx + 1;
            }
            return result;
        }

        protected virtual byte GetNameColor()
        {
            return NameColor;
        }

        public virtual int GetFeature(BaseObject baseObject)
        {
            return M2Share.MakeMonsterFeature(RaceImg, MonsterWeapon, Appr);
        }

        public virtual void Disappear()
        {

        }

        protected virtual void KickException()
        {
            Death = true;
            DeathTick = HUtil32.GetTickCount();
            MakeGhost();
        }

        public virtual ushort GetFeatureEx()
        {
            return 0;
        }
    }
}