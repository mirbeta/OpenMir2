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
            short nSx = 0;
            short nSy = 0;
            short nTx = 0;
            short nTy = 0;
            Direction = nDir;
            SendRefMsg(Grobal2.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ActorId, "");
            if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nSx, ref nSy))
            {
                Envir.GetNextPosition(CurrX, CurrY, nDir, 9, ref nTx, ref nTy);
                var nPower = M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1)) + HUtil32.LoWord(WAbil.DC);
                MagPassThroughMagic(nSx, nSy, nTx, nTy, nDir, nPower, true);
                BreakHolySeizeMode();
            }
        }

        public override void Run()
        {
            if (CanMove() && (HUtil32.GetTickCount() - SearchEnemyTick) > 8000)
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

