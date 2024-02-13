using OpenMir2;
using SystemModule.Actors;

namespace M2Server.Monster.Monsters
{
    public class CowKingMonster : AtMonster
    {
        private int JumpTime;
        private bool CrazyReadyMode;
        private bool CrazyKingMode;
        private int CrazyCount;
        private int CrazyReady;
        private int CrazyTime;
        private int oldHitTime;
        private int oldWalkTime;

        public CowKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 500;
            JumpTime = HUtil32.GetTickCount();
            RushMode = true;
            CrazyCount = 0;
            CrazyReadyMode = false;
            CrazyKingMode = false;
        }

        protected override void Attack(IActor targetBaseObject, byte nDir)
        {
            int nPower = GetAttackPower(HUtil32.LoByte(WAbil.DC), Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
            HitMagAttackTarget(targetBaseObject, nPower / 2, nPower / 2, true);
        }

        public override void Initialize()
        {
            oldHitTime = NextHitTime;
            oldWalkTime = WalkSpeed;
            base.Initialize();
        }

        public override void Run()
        {
            if (!Death && !Ghost && (HUtil32.GetTickCount() - JumpTime) > (30 * 1000))
            {
                short nX = 0;
                short nY = 0;
                JumpTime = HUtil32.GetTickCount();
                if (TargetCret != null && SiegeLockCount() >= 5)
                {
                    TargetCret.GetBackPosition(ref nX, ref nY);
                    if (Envir.CanWalk(nX, nY, false))
                    {
                        SpaceMove(Envir.MapName, nX, nY, 0);
                        return;
                    }
                    MapRandomMove(Envir.MapName, 0);
                    return;
                }
                int oldCrazyCount = CrazyCount;
                CrazyCount = 7 - WAbil.HP / (WAbil.MaxHP / 7);
                if (CrazyCount >= 2 && CrazyCount != oldCrazyCount)
                {
                    CrazyReadyMode = true;
                    CrazyReady = HUtil32.GetTickCount();
                }
                if (CrazyReadyMode)
                {
                    if ((HUtil32.GetTickCount() - CrazyReady) < 8000)
                    {
                        NextHitTime = 10000;
                    }
                    else
                    {
                        CrazyReadyMode = false;
                        CrazyKingMode = true;
                        CrazyTime = HUtil32.GetTickCount();
                    }
                }
                if (CrazyKingMode)
                {
                    if ((HUtil32.GetTickCount() - CrazyTime) < 8000)
                    {
                        NextHitTime = 500;
                        WalkSpeed = 400;
                    }
                    else
                    {
                        CrazyKingMode = false;
                        NextHitTime = oldHitTime;
                        WalkSpeed = oldWalkTime;
                    }
                }
            }
            base.Run();
        }

        private int SiegeLockCount()
        {
            int result = 0;
            int nC = -1;
            int n10;
            while (nC != 2)
            {
                n10 = -1;
                while (n10 != 2)
                {
                    if (!Envir.CanWalk(CurrX + nC, CurrY + n10, false))
                    {
                        if ((nC != 0) || (n10 != 0))
                        {
                            result++;
                        }
                    }
                    n10++;
                }
                nC++;
            }
            return result;
        }
    }
}

