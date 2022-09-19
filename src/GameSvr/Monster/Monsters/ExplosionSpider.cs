using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ExplosionSpider : MonsterObject
    {
        public int Dw558;

        public ExplosionSpider() : base()
        {
            ViewRange = 5;
            RunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = 0;
            Dw558 = HUtil32.GetTickCount();
        }

        private void sub_4A65C4()
        {
            Abil.HP = 0;
            var nPower = M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiWord(Abil.DC) - HUtil32.LoWord(Abil.DC) + 1)) + HUtil32.LoWord(Abil.DC);
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                var baseObject = VisibleActors[i].BaseObject;
                if (baseObject.Death)
                {
                    continue;
                }
                if (IsProperTarget(baseObject))
                {
                    if (Math.Abs(CurrX - baseObject.CurrX) <= 1 && Math.Abs(CurrY - baseObject.CurrY) <= 1)
                    {
                        var damage = 0;
                        damage += baseObject.GetHitStruckDamage(this, nPower / 2);
                        damage += baseObject.GetMagStruckDamage(this, nPower / 2);
                        if (damage > 0)
                        {
                            baseObject.StruckDamage(damage);
                            baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)damage, baseObject.Abil.HP, baseObject.Abil.MaxHP, ActorId, "", 700);
                        }
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
                return result;
            }
            if (GetAttackDir(TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    sub_4A65C4();
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

        public override void Run()
        {
            if (!Death && !Ghost)
            {
                if ((HUtil32.GetTickCount() - Dw558) > (60 * 1000))
                {
                    Dw558 = HUtil32.GetTickCount();
                    sub_4A65C4();
                }
            }
            base.Run();
        }
    }
}

