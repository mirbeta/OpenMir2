using GameSvr.Actor;
using GameSvr.Maps;
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
            BaseObject result = null;
            var ElfMon = M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, CurrX, CurrY, sMonName);
            if (ElfMon != null)
            {
                ElfMon.Master = OldMon.Master;
                ElfMon.MasterRoyaltyTick = OldMon.MasterRoyaltyTick;
                ElfMon.SlaveMakeLevel = OldMon.SlaveMakeLevel;
                ElfMon.SlaveExpLevel = OldMon.SlaveExpLevel;
                ElfMon.RecalcAbilitys();
                ElfMon.RefNameColor();
                if (OldMon.Master != null)
                {
                    OldMon.Master.SlaveList.Add(ElfMon);
                }
                ElfMon.Abil = OldMon.Abil;
                ElfMon.StatusTimeArr = OldMon.StatusTimeArr;
                ElfMon.TargetCret = OldMon.TargetCret;
                ElfMon.TargetFocusTick = OldMon.TargetFocusTick;
                ElfMon.LastHiter = OldMon.LastHiter;
                ElfMon.LastHiterTick = OldMon.LastHiterTick;
                ElfMon.Direction = OldMon.Direction;
                result = ElfMon;
            }
            return result;
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
            var result = false;
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
                    result = true;
                }
                else
                {
                    if (TargetCret.Envir == Envir)
                    {
                        SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
                    }
                    else
                    {
                        DelTargetCreat();
                    }
                }
            }
            return result;
        }

        public override void Run()
        {
            if (!Ghost && !Death && !FixedHideMode && !StoneMode && StatusTimeArr[Grobal2.POISON_STONE] == 0)
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
                    if (!m_boRunAwayMode)
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
                            if (!Master.SlaveRelax && (Envir != Master.Envir || Math.Abs(CurrX - Master.CurrX) > 20 || Math.Abs(CurrY - Master.CurrY) > 20))
                            {
                                SpaceMove(Master.Envir.MapName, TargetX, TargetY, 1);
                            }
                        }
                    }
                    else
                    {
                        if (m_dwRunAwayTime > 0 && (HUtil32.GetTickCount() - m_dwRunAwayStart) > m_dwRunAwayTime)
                        {
                            m_boRunAwayMode = false;
                            m_dwRunAwayTime = 0;
                        }
                    }
                    if (Master != null && Master.SlaveRelax)
                    {
                        base.Run();
                        return;
                    }
                    if (TargetX != -1)
                    {
                        GotoTargetXY();
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

