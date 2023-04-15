using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.Monster
{
    public class MonsterObject : AnimalObject
    {
        /// <summary>
        /// 杀怪计数
        /// </summary>
        public int KillMonCount;
        /// <summary>
        /// 宝宝等级(1-7)
        /// </summary>
        public byte SlaveExpLevel;
        /// <summary>
        /// 召唤等级
        /// </summary>
        public byte SlaveMakeLevel;
        /// <summary>
        /// 不进入火墙
        /// </summary>
        public bool BoFearFire;
        private int m_dwThinkTick;
        private bool m_boDupMode;
        /// <summary>
        /// 怪物叛变时间
        /// </summary>
        public int MasterRoyaltyTick;
        /// <summary>
        /// 诱惑怪物时间
        /// </summary>
        public int MasterTick;
        /// <summary>
        /// 怪物叛变时间间隔
        /// </summary>
        private int CheckRoyaltyTick;
        /// <summary>
        /// 狂暴模式
        /// </summary>
        public bool CrazyMode;
        /// <summary>
        /// 狂暴开始时间
        /// </summary>
        private int CrazyModeTick;
        /// <summary>
        /// 狂暴时常
        /// </summary>
        private int CrazyModeInterval;
        /// <summary>
        /// 不能走动模式(困魔咒)
        /// </summary>
        public bool HolySeize;
        /// <summary>
        /// 不能走动时间(困魔咒)
        /// </summary>
        public int HolySeizeTick;
        /// <summary>
        /// 不能走动时长(困魔咒)
        /// </summary>
        public int HolySeizeInterval;
        
        public MonsterObject() : base()
        {
            m_boDupMode = false;
            m_dwThinkTick = HUtil32.GetTickCount();
            ViewRange = 5;
            RunTime = 250;
            SearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            SearchTick = HUtil32.GetTickCount();
            CheckRoyaltyTick = HUtil32.GetTickCount();
            CrazyMode = false;
        }
        
        private void GainSlaveExp(byte nLevel)
        {
            KillMonCount += nLevel;
            if (GainSlaveUpKillCount() < KillMonCount)
            {
                KillMonCount -= GainSlaveUpKillCount();
                if (SlaveExpLevel < (SlaveMakeLevel * 2 + 1))
                {
                    SlaveExpLevel++;
                    RecalcAbilitys();
                    RefNameColor();
                }
            }
        }
        
        private int GainSlaveUpKillCount()
        {
            int tCount;
            if (SlaveExpLevel < Grobal2.SlaveMaxLevel - 2)
            {
                tCount = M2Share.Config.MonUpLvNeedKillCount[SlaveExpLevel];
            }
            else
            {
                tCount = 0;
            }
            return (Abil.Level * M2Share.Config.MonUpLvRate) - Abil.Level + M2Share.Config.MonUpLvNeedKillBase + tCount;
        }

        protected BaseObject MakeClone(string sMonName, MonsterObject OldMon)
        {
            MonsterObject ElfMon = (MonsterObject)M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, sMonName);
            if (ElfMon != null)
            {
                if (OldMon.TargetCret == null)
                {
                    OldMon.TargetCret = OldMon.Master.TargetCret == null ? OldMon.Master.LastHiter : OldMon.Master.TargetCret;
                }
                ElfMon.Master = OldMon.Master;
                ElfMon.MasterRoyaltyTick = OldMon.MasterRoyaltyTick;
                ElfMon.SlaveMakeLevel = OldMon.SlaveMakeLevel;
                ElfMon.SlaveExpLevel = OldMon.SlaveExpLevel;
                ElfMon.RecalcAbilitys();
                ElfMon.RefNameColor();
                ElfMon.Abil = OldMon.Abil;
                ElfMon.StatusTimeArr = OldMon.StatusTimeArr;
                ElfMon.TargetCret = OldMon.TargetCret;
                ElfMon.TargetFocusTick = OldMon.TargetFocusTick;
                ElfMon.LastHiter = OldMon.LastHiter;
                ElfMon.LastHiterTick = OldMon.LastHiterTick;
                ElfMon.Dir = OldMon.Dir;
                ElfMon.IsSlave = true;
                if (OldMon.Master != null)
                {
                    OldMon.Master.SlaveList.Add(ElfMon);
                }
                return ElfMon;
            }
            return null;
        }

        /// <summary>
        /// 更新自身视野对象（可见对象）
        /// </summary>
        /// <param name="acrotId"></param>
        private void UpdateMonsterVisible(int acrotId)
        {
            if ((HUtil32.GetTickCount() - SearchTick) <= SearchTime)
                return;
            SearchTick = HUtil32.GetTickCount();
            bool boIsVisible = false;
            VisibleBaseObject visibleBaseObject;
            var baseObject = M2Share.ActorMgr.Get(acrotId);
            if ((baseObject.Race == ActorRace.Play) || (baseObject.Master != null))// 如果是人物或宝宝则置TRUE
            {
                IsVisibleActive = true;
            }
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                visibleBaseObject = VisibleActors[i];
                if (visibleBaseObject == null)
                {
                    continue;
                }
                if (visibleBaseObject.BaseObject == baseObject)
                {
                    visibleBaseObject.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            visibleBaseObject = new VisibleBaseObject
            {
                VisibleFlag = VisibleFlag.Show,
                BaseObject = baseObject
            };
            VisibleActors.Add(visibleBaseObject);
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            switch (processMsg.wIdent)
            {
                case Messages.RM_UPDATEVIEWRANGE:
                    UpdateMonsterVisible(processMsg.wParam);
                    return true;
                case Messages.RM_STRUCK:
                {
                    var struckObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                    if (processMsg.ActorId == ActorId && struckObject != null)
                    {
                        SetLastHiter(struckObject);
                        Struck(struckObject);
                        BreakHolySeizeMode(); 
                        if (Master != null && struckObject != Master && struckObject.Race == ActorRace.Play)
                        {
                            ((PlayObject)Master).SetPkFlag(struckObject);
                        }
                        MonsterSayMessage(struckObject, MonStatus.UnderFire);
                    }
                    return true;
                }
                default:
                    return base.Operate(processMsg);
            }
        }

        private bool Think()
        {
            bool result = false;
            if ((HUtil32.GetTickCount() - m_dwThinkTick) > (3 * 1000))
            {
                m_dwThinkTick = HUtil32.GetTickCount();
                if (Envir.GetXyObjCount(CurrX, CurrY) >= 2)
                {
                    m_boDupMode = true;
                }
                if (!IsProperTarget(TargetCret))
                {
                    TargetCret = null;
                }
            }
            if (m_boDupMode)
            {
                if (HolySeize)
                {
                    return false;
                }
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(M2Share.RandomNumber.RandomByte(8), false);
                if (nOldX != CurrX || nOldY != CurrY)
                {
                    m_boDupMode = false;
                    result = true;
                }
            }
            return result;
        }

        protected virtual bool AttackTarget()
        {
            byte btDir = 0;
            if (TargetCret != null)
            {
                if (GetAttackDir(TargetCret, ref btDir))
                {
                    if (HUtil32.GetTickCount() - AttackTick > NextHitTime)
                    {
                        AttackTick = HUtil32.GetTickCount();
                        TargetFocusTick = HUtil32.GetTickCount();
                        Attack(TargetCret, btDir);
                        BreakHolySeizeMode();
                    }
                    return true;
                }
                else
                {
                    if (TargetCret.Envir == Envir)
                    {
                        SetTargetXy(TargetCret.CurrX, TargetCret.CurrY);
                    }
                    else
                    {
                        DelTargetCreat();
                    }
                }
            }
            return false;
        }

        public override void Run()
        {
            if (CanMove() && !FixedHideMode && !StoneMode)
            {
                if (Think())
                {
                    base.Run();
                    return;
                }
                if (WalkWaitLocked)
                {
                    if ((HUtil32.GetTickCount() - WalkWaitTick) > WalkWait)
                    {
                        WalkWaitLocked = false;
                    }
                }
                if (!WalkWaitLocked && (HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    WalkCount++;
                    if (WalkCount > WalkStep)
                    {
                        WalkCount = 0;
                        WalkWaitLocked = true;
                        WalkWaitTick = HUtil32.GetTickCount();
                    }
                    if (!RunAwayMode)
                    {
                        if (!NoAttackMode)
                        {
                            if (TargetCret != null)
                            {
                                if (AttackTarget())
                                {
                                    base.Run();
                                    return;
                                }
                            }
                            else
                            {
                                TargetX = -1;
                                if (Mission)
                                {
                                    TargetX = MissionX;
                                    TargetY = MissionY;
                                }
                            }
                        }
                        if (Master != null)
                        {
                            short nX = 0;
                            short nY = 0;
                            if (TargetCret == null)
                            {
                                Master.GetBackPosition(ref nX, ref nY);
                                if (Math.Abs(TargetX - nX) > 1 || Math.Abs(TargetY - nY) > 1)
                                {
                                    TargetX = nX;
                                    TargetY = nY;
                                    if (Math.Abs(CurrX - nX) <= 2 && Math.Abs(CurrY - nY) <= 2)
                                    {
                                        if (Envir.GetMovingObject(nX, nY, true) != null)
                                        {
                                            TargetX = CurrX;
                                            TargetY = CurrY;
                                        }
                                    }
                                }
                            }
                            if (!Master.SlaveRelax && (Envir != Master.Envir || Math.Abs(CurrX - Master.CurrX) > 20 || Math.Abs(CurrY - Master.CurrY) > 20)) //离主人视野范围超过20
                            {
                                SpaceMove(Master.Envir.MapName, TargetX, TargetY, 1);//飞到主人身边
                            }
                        }
                    }
                    else
                    {
                        if (RunAwayTime > 0 && (HUtil32.GetTickCount() - RunAwayStart) > RunAwayTime)
                        {
                            RunAwayMode = false;
                            RunAwayTime = 0;
                        }
                    }
                    if (Master != null && Master.SlaveRelax)
                    {
                        base.Run();
                        return;
                    }
                    if (TargetX != -1)
                    {
                        GotoTargetXy();
                    }
                    else
                    {
                        if (TargetCret == null)
                        {
                            Wondering();
                        }
                    }
                }
            }
            CheckRoyalty();
            if (CrazyMode && ((HUtil32.GetTickCount() - CrazyModeTick) > CrazyModeInterval))
            {
                BreakCrazyMode();
            }
            if (HolySeize && ((HUtil32.GetTickCount() - HolySeizeTick) > HolySeizeInterval))
            {
                BreakHolySeizeMode();
            }
            base.Run();
        }

        /// <summary>
        /// 宠物叛变时间检测
        /// </summary>
        private void CheckRoyalty()
        {
            if ((HUtil32.GetTickCount() - CheckRoyaltyTick) > 10000)
            {
                CheckRoyaltyTick = HUtil32.GetTickCount();
                if (Master != null)
                {
                    if ((M2Share.SpiritMutinyTick > HUtil32.GetTickCount()) && (this.SlaveExpLevel < 5))
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
                    if (MasterTick > 0)
                    {
                        if ((HUtil32.GetTickCount() - MasterTick) > 12 * 60 * 60 * 1000) //超过叛变时间则死亡
                        {
                            WAbil.HP = 0;
                        }
                    }
                }
            }
        }

        protected bool GetLongAttackDirDis(BaseObject baseObject, int dis, ref byte dir)
        {
            var result = false;
            var nC = 0;
            while (true)
            {
                if (CurrX - nC <= baseObject.CurrX && (CurrX + nC >= baseObject.CurrX) && (CurrY - nC <= baseObject.CurrY) && (CurrY + nC >= baseObject.CurrY) && ((CurrX != baseObject.CurrX) || (CurrY != baseObject.CurrY)))
                {
                    if ((CurrX - nC == baseObject.CurrX) && (CurrY == baseObject.CurrY))
                    {
                        dir = Direction.Left;
                        result = true;
                    }
                    if ((CurrX + nC == baseObject.CurrX) && (CurrY == baseObject.CurrY))
                    {
                        dir = Direction.Right;
                        result = true;
                        break;
                    }
                    if (CurrX == baseObject.CurrX && ((CurrY - nC) == baseObject.CurrY))
                    {
                        dir = Direction.Up;
                        result = true;
                        break;
                    }
                    if (CurrX == baseObject.CurrX && ((CurrY + nC) == baseObject.CurrY))
                    {
                        dir = Direction.Down;
                        result = true;
                        break;
                    }
                    if ((CurrX - nC) == baseObject.CurrX && ((CurrY - nC) == baseObject.CurrY))
                    {
                        dir = Direction.UpLeft;
                        result = true;
                        break;
                    }
                    if ((CurrX + nC) == baseObject.CurrX && ((CurrY - nC) == baseObject.CurrY))
                    {
                        dir = Direction.UpRight;
                        result = true;
                        break;
                    }
                    if ((CurrX - nC) == baseObject.CurrX && ((CurrY + nC) == baseObject.CurrY))
                    {
                        dir = Direction.DownLeft;
                        result = true;
                        break;
                    }
                    if ((CurrX + nC) == baseObject.CurrX && ((CurrY + nC) == baseObject.CurrY))
                    {
                        dir = Direction.DownRight;
                        result = true;
                        break;
                    }
                    dir = 0;
                }
                nC++;
                if (nC > dis)
                {
                    break;
                }
            }
            return result;
        }

        public void OpenCrazyMode(int nTime)
        {
            CrazyMode = true;
            CrazyModeTick = HUtil32.GetTickCount();
            CrazyModeInterval = nTime * 1000;
            RefNameColor();
        }

        public void BreakCrazyMode()
        {
            if (CrazyMode)
            {
                CrazyMode = false;
                RefNameColor();
            }
        }
        
        public void OpenHolySeizeMode(int dwInterval)
        {
            HolySeize = true;
            HolySeizeTick = HUtil32.GetTickCount();
            HolySeizeInterval = dwInterval;
            RefNameColor();
        }

        public void BreakHolySeizeMode()
        {
            HolySeize = false;
            RefNameColor();
        }

        protected override byte GetChrColor(BaseObject baseObject)
        {
            if (baseObject.ActorId == this.ActorId && this.CrazyMode)
            {
                return 0xF9;
            }
            if (baseObject.ActorId == this.ActorId && this.HolySeize)
            {
                return 0x7D;
            }
            return base.GetChrColor(baseObject);
        }
    }
}