using GameSrv.Actor;
using SystemModule.Consts;

namespace GameSrv.Monster.Monsters
{
    public class GasAttackMonster : AtMonster
    {
        public GasAttackMonster() : base()
        {
            SearchTime = GameShare.RandomNumber.Random(1500) + 1500;
            Animal = true;
        }

        protected virtual BaseObject GasAttack(byte bt05)
        {
            BaseObject result = null;
            Dir = bt05;
            int nPower = HUtil32.LoByte(WAbil.DC) + GameShare.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
            if (nPower > 0)
            {
                SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                BaseObject baseObject = GetPoseCreate();
                if (baseObject != null && IsProperTarget(baseObject) && GameShare.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint)
                {
                    nPower = baseObject.GetMagStruckDamage(this, nPower);
                    if (nPower > 0)
                    {
                        baseObject.StruckDamage(nPower);
                        baseObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nPower, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 300);
                        if (GameShare.RandomNumber.Random(baseObject.AntiPoison + 20) == 0)
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