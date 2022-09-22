﻿using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ExplosionSpider : MonsterObject
    {
        private int MakeTime;

        public ExplosionSpider() : base()
        {
            ViewRange = 5;
            RunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = 0;
            MakeTime = HUtil32.GetTickCount();
        }

        private void DoSelfExplosion()
        {
            WAbil.HP = 0;
            var nPower = M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1)) + HUtil32.LoWord(WAbil.DC);
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
                            baseObject.StruckDamage((ushort)damage);
                            baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, damage, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 700);
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
                return false;
            }
            if (GetAttackDir(TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    DoSelfExplosion();
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
                if ((HUtil32.GetTickCount() - MakeTime) > (60 * 1000))
                {
                    MakeTime = HUtil32.GetTickCount();
                    DoSelfExplosion();
                }
            }
            base.Run();
        }
    }
}

