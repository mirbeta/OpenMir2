using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class CowKingMonster : AtMonster
    {
        private int dw558 = 0;
        private bool bo55C = false;
        private bool bo55D = false;
        private int n560 = 0;
        private int dw564 = 0;
        private int dw568 = 0;
        private int dw56C = 0;
        private int dw570 = 0;

        public CowKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 500;
            dw558 = HUtil32.GetTickCount();
            bo2BF = true;
            n560 = 0;
            bo55C = false;
            bo55D = false;
        }

        public override void Attack(TBaseObject TargeTBaseObject, byte nDir)
        {
            var nPower = GetAttackPower(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC));
            HitMagAttackTarget(TargeTBaseObject, nPower / 2, nPower / 2, true);
        }

        public override void Initialize()
        {
            dw56C = NextHitTime;
            dw570 = WalkSpeed;
            base.Initialize();
        }

        public override void Run()
        {
            short n8 = 0;
            short nC = 0;
            int n10;
            if (!Death && !bo554 && !Ghost && (HUtil32.GetTickCount() - dw558) > (30 * 1000))
            {
                dw558 = HUtil32.GetTickCount();
                if (TargetCret != null && sub_4C3538() >= 5)
                {
                    TargetCret.GetBackPosition(ref n8, ref nC);
                    if (Envir.CanWalk(n8, nC, false))
                    {
                        SpaceMove(Envir.MapName, n8, nC, 0);
                        return;
                    }
                    MapRandomMove(Envir.MapName, 0);
                    return;
                }
                n10 = n560;
                n560 = 7 - m_WAbil.HP / (m_WAbil.MaxHP / 7);
                if (n560 >= 2 && n560 != n10)
                {
                    bo55C = true;
                    dw564 = HUtil32.GetTickCount();
                }
                if (bo55C)
                {
                    if ((HUtil32.GetTickCount() - dw564) < 8000)
                    {
                        NextHitTime = 10000;
                    }
                    else
                    {
                        bo55C = false;
                        bo55D = true;
                        dw568 = HUtil32.GetTickCount();
                    }
                }
                if (bo55D)
                {
                    if ((HUtil32.GetTickCount() - dw568) < 8000)
                    {
                        NextHitTime = 500;
                        WalkSpeed = 400;
                    }
                    else
                    {
                        bo55D = false;
                        NextHitTime = dw56C;
                        WalkSpeed = dw570;
                    }
                }
            }
            base.Run();
        }

        private int sub_4C3538()
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

