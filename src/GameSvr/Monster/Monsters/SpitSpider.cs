using GameSvr.Actor;
using SystemModule;
using SystemModule.Consts;

namespace GameSvr.Monster.Monsters
{
    public class SpitSpider : AtMonster
    {
        public bool UsePoison;

        public SpitSpider() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            Animal = true;
            UsePoison = true;
        }

        private void SpitAttack(byte btDir)
        {
            Direction = btDir;
            var nDamage = M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1)) + HUtil32.LoWord(WAbil.DC);
            if (nDamage <= 0)
            {
                return;
            }
            SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
            for (var i = 0; i < 4; i++)
            {
                for (var k = 0; k < 4; k++)
                {
                    if (M2Share.Config.SpitMap[btDir, i, k] == 1)
                    {
                        var nX = (short)(CurrX - 2 + k);
                        var nY = (short)(CurrY - 2 + i);
                        var baseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                        if (baseObject != null && baseObject != this && IsProperTarget(baseObject) && M2Share.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint)
                        {
                            nDamage = baseObject.GetMagStruckDamage(this, nDamage);
                            if (nDamage > 0)
                            {
                                baseObject.StruckDamage((ushort)nDamage);
                                baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_REFMESSAGE, nDamage, WAbil.HP, WAbil.MaxHP, ActorId, "", 300);
                                if (UsePoison)
                                {
                                    if (M2Share.RandomNumber.Random(AntiPoison + 20) == 0)
                                    {
                                        baseObject.MakePosion(StatuStateConst.POISON_DECHEALTH, 30, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
                    SpitAttack(btDir);
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
    }
}

