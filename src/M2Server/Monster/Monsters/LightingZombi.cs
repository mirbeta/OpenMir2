using OpenMir2;

namespace M2Server.Monster.Monsters
{
    /// <summary>
    /// 电僵尸
    /// </summary>
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
            Dir = nDir;
            SendRefMsg(Messages.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ActorId, "");
            if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nSx, ref nSy))
            {
                Envir.GetNextPosition(CurrX, CurrY, nDir, 9, ref nTx, ref nTy);
                int nPower = HUtil32._MAX(0, HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1));
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
                    byte nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                    LightingAttack(nAttackDir);
                }
            }
            base.Run();
        }
    }
}

