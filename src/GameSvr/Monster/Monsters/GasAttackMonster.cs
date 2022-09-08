using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class GasAttackMonster : AtMonster
    {
        public GasAttackMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            Animal = true;
        }

        protected virtual BaseObject sub_4A9C78(byte bt05)
        {
            BaseObject result = null;
            Direction = bt05;
            var wAbil = Abil;
            var n10 = M2Share.RandomNumber.Random(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1) + HUtil32.LoWord(wAbil.DC);
            if (n10 > 0)
            {
                SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                var baseObject = GetPoseCreate();
                if (baseObject != null && IsProperTarget(baseObject) && M2Share.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint)
                {
                    n10 = baseObject.GetMagStruckDamage(this, n10);
                    if (n10 > 0)
                    {
                        baseObject.StruckDamage(n10);
                        baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)n10, baseObject.Abil.HP, baseObject.Abil.MaxHP, ObjectId, "", 300);
                        if (M2Share.RandomNumber.Random(baseObject.AntiPoison + 20) == 0)
                        {
                            baseObject.MakePosion(Grobal2.POISON_STONE, 5, 0);
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
                    sub_4A9C78(btDir);
                    BreakHolySeizeMode();
                }
                result = true;
            }
            else
            {
                if (TargetCret.Envir == Envir)
                {
                    SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
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

