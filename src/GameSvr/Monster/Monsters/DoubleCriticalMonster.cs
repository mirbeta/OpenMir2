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
            var WAbil = m_WAbil;
            var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
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
                        var BaseObject = (TBaseObject)Envir.GetMovingObject(nX, nY, true);
                        if (BaseObject != null && BaseObject != this && IsProperTarget(BaseObject) && M2Share.RandomNumber.Random(BaseObject.SpeedPoint) < m_btHitPoint)
                        {
                            nDamage = BaseObject.GetHitStruckDamage(this, nDamage);
                            if (nDamage > 0)
                            {
                                BaseObject.StruckDamage(nDamage);
                                BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, m_WAbil.HP, m_WAbil.MaxHP, ObjectId, "", 300);
                            }
                        }
                    }
                }
            }
        }
    }
}

