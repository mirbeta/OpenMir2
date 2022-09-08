using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class DoubleCriticalMonster : AtMonster
    {
        public DoubleCriticalMonster() : base()
        {
            Animal = false;
        }

        protected override bool AttackTarget()
        {
            byte btDir = 0;
            if (TargetCret == null)
            {
                return false;
            }
            if (TargetInSpitRange(TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    DoubleAttack(btDir);
                    BreakHolySeizeMode();
                }
                return true;
            }
            if (TargetCret.Envir == Envir)
            {
                SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
            }
            else
            {
                DelTargetCreat();
            }
            return false;
        }

        private void DoubleAttack(byte btDir)
        {
            Direction = btDir;
            var wAbil = Abil;
            var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1) + HUtil32.LoWord(wAbil.DC);
            if (nDamage <= 0)
            {
                return;
            }
            SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
            for (var i = 0; i < 4; i++)
            {
                for (var k = 0; k <= 4; k++)
                {
                    if (M2Share.Config.SpitMap[btDir, i, k] == 1)
                    {
                        var nX = (short)(CurrX - 2 + k);
                        var nY = (short)(CurrY - 2 + i);
                        var baseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                        if (baseObject != null && baseObject != this && IsProperTarget(baseObject) && M2Share.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint)
                        {
                            nDamage = baseObject.GetHitStruckDamage(this, nDamage);
                            if (nDamage > 0)
                            {
                                baseObject.StruckDamage(nDamage);
                                baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, Abil.HP, Abil.MaxHP, ObjectId, "", 300);
                            }
                        }
                    }
                }
            }
        }
    }
}

