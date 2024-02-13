using OpenMir2;
using OpenMir2.Consts;
using SystemModule;
using SystemModule.Actors;

namespace M2Server.Monster.Monsters
{
    public class GasAttackMonster : AtMonster
    {
        public GasAttackMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            Animal = true;
        }

        protected virtual IActor GasAttack(byte bt05)
        {
            IActor result = null;
            Dir = bt05;
            int nPower = HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
            if (nPower > 0)
            {
                SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                IActor baseObject = GetPoseCreate();
                if (baseObject != null && IsProperTarget(baseObject) && M2Share.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint)
                {
                    nPower = baseObject.GetMagStruckDamage(this, nPower);
                    if (nPower > 0)
                    {
                        baseObject.StruckDamage(nPower);
                        baseObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nPower, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 300);
                        if (M2Share.RandomNumber.Random(baseObject.AntiPoison + 20) == 0)
                        {
                            baseObject.MakePosion(PoisonState.STONE, 5, 0);
                        }
                        result = baseObject;
                    }
                }
            }
            return result;
        }

        protected override bool AttackTarget()
        {
            bool result = false;
            byte btDir = 0;
            if (TargetCret == null)
            {
                return false;
            }
            if (GetAttackDir(TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    GasAttack(btDir);
                    BreakHolySeizeMode();
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
            return result;
        }
    }
}