using GameSvr.Actor;
using SystemModule;
using SystemModule.Consts;

namespace GameSvr.Monster.Monsters
{
    public class GasAttackMonster : AtMonster
    {
        public GasAttackMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            Animal = true;
        }

        protected virtual BaseObject GasAttack(byte bt05)
        {
            BaseObject result = null;
            Direction = bt05;
            var nPower = HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
            if (nPower > 0)
            {
                SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                var baseObject = GetPoseCreate();
                if (baseObject != null && IsProperTarget(baseObject) && M2Share.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint)
                {
                    nPower = baseObject.GetMagStruckDamage(this, nPower);
                    if (nPower > 0)
                    {
                        baseObject.StruckDamage((ushort)nPower);
                        baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_REFMESSAGE, nPower, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 300);
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
            var result = false;
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

