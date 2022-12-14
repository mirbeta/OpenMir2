using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Monster
{
    public class MonsterObject : AnimalObject
    {
        private int m_dwThinkTick;
        private bool m_boDupMode;

        public MonsterObject() : base()
        {
            m_boDupMode = false;
            m_dwThinkTick = HUtil32.GetTickCount();
            ViewRange = 5;
            RunTime = 250;
            SearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            SearchTick = HUtil32.GetTickCount();
        }

        protected BaseObject MakeClone(string sMonName, BaseObject OldMon)
        {
            var ElfMon = M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, sMonName);
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
                ElfMon.StatusArr = OldMon.StatusArr;
                ElfMon.TargetCret = OldMon.TargetCret;
                ElfMon.TargetFocusTick = OldMon.TargetFocusTick;
                ElfMon.LastHiter = OldMon.LastHiter;
                ElfMon.LastHiterTick = OldMon.LastHiterTick;
                ElfMon.Direction = OldMon.Direction;
                ElfMon.IsSlave = true;
                if (OldMon.Master != null)
                {
                    OldMon.Master.SlaveList.Add(ElfMon);
                }
                return ElfMon;
            }
            return null;
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        private bool Think()
        {
            var result = false;
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
                    if (!MBoRunAwayMode)
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
                        if (MDwRunAwayTime > 0 && (HUtil32.GetTickCount() - MDwRunAwayStart) > MDwRunAwayTime)
                        {
                            MBoRunAwayMode = false;
                            MDwRunAwayTime = 0;
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
            base.Run();
        }
    }
}

