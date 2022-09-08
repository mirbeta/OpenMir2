using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ElectronicScolpionMon : MonsterObject
    {
        public bool MBoUseMagic;

        public ElectronicScolpionMon() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            MBoUseMagic = false;
        }

        private void LightingAttack(byte nDir)
        {
            Direction = nDir;
            var wAbil = Abil;
            int nPower = GetAttackPower(HUtil32.LoWord(wAbil.MC), HUtil32.HiWord(wAbil.MC) - HUtil32.LoWord(wAbil.MC));
            var nDamage = TargetCret.GetMagStruckDamage(this, nPower);
            if (nDamage > 0)
            {
                int btGetBackHp = HUtil32.LoByte(Abil.MP);
                if (btGetBackHp != 0)
                {
                    Abil.HP += (ushort)(nDamage / btGetBackHp);
                }
                TargetCret.StruckDamage(nDamage);
                TargetCret.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, TargetCret.Abil.HP, TargetCret.Abil.MaxHP, ObjectId, "", 200);
            }
            SendRefMsg(Grobal2.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ObjectId, "");
        }

        public override void Run()
        {
            if (!Death && !Ghost && StatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (Abil.HP < Abil.MaxHP / 2)// 血量低于一半时开始用魔法攻击
                {
                    MBoUseMagic = true;
                }
                else
                {
                    MBoUseMagic = false;
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if (TargetCret == null)
                {
                    return;
                }
                var nX = Math.Abs(CurrX - TargetCret.CurrX);
                var nY = Math.Abs(CurrY - TargetCret.CurrY);
                if (nX <= 2 && nY <= 2)
                {
                    if (MBoUseMagic || nX == 2 || nY == 2)
                    {
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            int nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                            LightingAttack((byte)nAttackDir);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

