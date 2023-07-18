using SystemModule;

namespace M2Server.Monster.Monsters
{
    public class MagicMonster : MonsterObject
    {
        public MagicMonster() : base()
        {
            DupMode = false;
            ThinkTick = HUtil32.GetTickCount();
            ViewRange = 8;
            RunTime = 250;
            SearchTime = 3000 + M2Share.RandomNumber.Random(2000);
            SearchTick = HUtil32.GetTickCount();
            Race = 215;
        }

        private bool Think()
        {
            bool result = false;
            if ((HUtil32.GetTickCount() - ThinkTick) > (3 * 1000))
            {
                ThinkTick = HUtil32.GetTickCount();
                if (Envir.GetXYObjCount(CurrX, CurrY) >= 2)
                {
                    DupMode = true;
                }
                if (!IsProperTarget(TargetCret))
                {
                    TargetCret = null;
                }
            }
            if (DupMode)
            {
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(M2Share.RandomNumber.RandomByte(8), false, BoFearFire);
                if (nOldX != CurrX || nOldY != CurrY)
                {
                    DupMode = false;
                    result = true;
                }
            }
            return result;
        }

        protected override bool AttackTarget()
        {
            bool result = false;
            byte dir = 0;
            if (TargetCret != null)
            {
                if (TargetCret == Master)
                {
                    TargetCret = null;
                }
                else
                {
                    if (GetAttackDir(TargetCret, ref dir))
                    {
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            TargetFocusTick = HUtil32.GetTickCount();
                        }
                        result = true;
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
            }
            return result;
        }

        public override void Run()
        {
            short nX = 0;
            short nY = 0;
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