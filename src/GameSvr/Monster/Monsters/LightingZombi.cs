using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class LightingZombi : MonsterObject
    {
        public LightingZombi() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        private void LightingAttack(byte nDir)
        {
            short nSX = 0;
            short nSY = 0;
            short nTX = 0;
            short nTY = 0;
            Direction = nDir;
            SendRefMsg(Grobal2.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ObjectId, "");
            if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nSX, ref nSY))
            {
                Envir.GetNextPosition(CurrX, CurrY, nDir, 9, ref nTX, ref nTY);
                var WAbil = Abil;
                var nPwr = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                MagPassThroughMagic(nSX, nSY, nTX, nTY, nDir, nPwr, true);
                BreakHolySeizeMode();
            }
        }

        public override void Run()
        {
            if (!Death && !Ghost && StatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - SearchEnemyTick) > 8000)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed && TargetCret != null && Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) <= 4)
                {
                    if (Math.Abs(CurrX - TargetCret.CurrX) <= 2 && Math.Abs(CurrY - TargetCret.CurrY) <= 2 && M2Share.RandomNumber.Random(3) != 0)
                    {
                        base.Run();
                        return;
                    }
                    GetBackPosition(ref TargetX, ref TargetY);
                }
                if (TargetCret != null && Math.Abs(CurrX - TargetCret.CurrX) < 6 && Math.Abs(CurrY - TargetCret.CurrY) < 6 && (HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    var nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                    LightingAttack(nAttackDir);
                }
            }
            base.Run();
        }
    }
}

