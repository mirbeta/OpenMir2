using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class MagCowMonster : AtMonster
    {
        public MagCowMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        private void sub_4A9F6C(byte btDir)
        {
            Direction = btDir;
            var wAbil = Abil;
            var n10 = M2Share.RandomNumber.Random(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1) + HUtil32.LoWord(wAbil.DC);
            if (n10 > 0)
            {
                SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                var baseObject = GetPoseCreate();
                if (baseObject != null && IsProperTarget(baseObject) && AntiMagic >= 0)
                {
                    n10 = baseObject.GetMagStruckDamage(this, n10);
                    if (n10 > 0)
                    {
                        baseObject.StruckDamage(n10);
                        baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)n10, baseObject.Abil.HP, baseObject.Abil.MaxHP, ObjectId, "", 300);
                    }
                }
            }
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
                if (HUtil32.GetTickCount() - AttackTick > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    sub_4A9F6C(btDir);
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

