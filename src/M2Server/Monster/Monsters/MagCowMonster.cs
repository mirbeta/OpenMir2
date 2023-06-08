using M2Server.Actor;
using SystemModule;

namespace M2Server.Monster.Monsters
{
    public class MagCowMonster : AtMonster
    {
        public MagCowMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        private void MagicAttack(byte btDir)
        {
            Dir = btDir;
            int nPower = HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
            if (nPower > 0)
            {
                SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                BaseObject baseObject = GetPoseCreate();
                if (baseObject != null && IsProperTarget(baseObject) && AntiMagic >= 0)
                {
                    nPower = baseObject.GetMagStruckDamage(this, nPower);
                    if (nPower > 0)
                    {
                        baseObject.StruckDamage(nPower);
                        baseObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nPower, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 300);
                    }
                }
            }
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
                if (HUtil32.GetTickCount() - AttackTick > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    MagicAttack(btDir);
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