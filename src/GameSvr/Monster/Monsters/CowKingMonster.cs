using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class CowKingMonster : AtMonster
    {
        private int _dw558;
        private bool _bo55C;
        private bool _bo55D;
        private int _n560;
        private int _dw564;
        private int _dw568;
        private int _dw56C;
        private int _dw570;

        public CowKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 500;
            _dw558 = HUtil32.GetTickCount();
            Bo2Bf = true;
            _n560 = 0;
            _bo55C = false;
            _bo55D = false;
        }

        public override void Attack(BaseObject targeTBaseObject, byte nDir)
        {
            var nPower = GetAttackPower(HUtil32.LoWord(Abil.DC), HUtil32.HiWord(Abil.DC) - HUtil32.LoWord(Abil.DC));
            HitMagAttackTarget(targeTBaseObject, nPower / 2, nPower / 2, true);
        }

        public override void Initialize()
        {
            _dw56C = NextHitTime;
            _dw570 = WalkSpeed;
            base.Initialize();
        }

        public override void Run()
        {
            if (!Death && !Ghost && (HUtil32.GetTickCount() - _dw558) > (30 * 1000))
            {
                short n8 = 0;
                short nC = 0;
                int n10;
                _dw558 = HUtil32.GetTickCount();
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
                n10 = _n560;
                _n560 = 7 - Abil.HP / (Abil.MaxHP / 7);
                if (_n560 >= 2 && _n560 != n10)
                {
                    _bo55C = true;
                    _dw564 = HUtil32.GetTickCount();
                }
                if (_bo55C)
                {
                    if ((HUtil32.GetTickCount() - _dw564) < 8000)
                    {
                        NextHitTime = 10000;
                    }
                    else
                    {
                        _bo55C = false;
                        _bo55D = true;
                        _dw568 = HUtil32.GetTickCount();
                    }
                }
                if (_bo55D)
                {
                    if ((HUtil32.GetTickCount() - _dw568) < 8000)
                    {
                        NextHitTime = 500;
                        WalkSpeed = 400;
                    }
                    else
                    {
                        _bo55D = false;
                        NextHitTime = _dw56C;
                        WalkSpeed = _dw570;
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

