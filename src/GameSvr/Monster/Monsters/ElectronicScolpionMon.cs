using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ElectronicScolpionMon : MonsterObject
    {
        public bool m_boUseMagic = false;

        public ElectronicScolpionMon() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_boUseMagic = false;
        }

        private void LightingAttack(byte nDir)
        {
            Direction = nDir;
            var WAbil = m_WAbil;
            int nPower = GetAttackPower(HUtil32.LoWord(WAbil.MC), HUtil32.HiWord(WAbil.MC) - HUtil32.LoWord(WAbil.MC));
            var nDamage = TargetCret.GetMagStruckDamage(this, nPower);
            if (nDamage > 0)
            {
                int btGetBackHP = HUtil32.LoByte(m_WAbil.MP);
                if (btGetBackHP != 0)
                {
                    m_WAbil.HP += (ushort)(nDamage / btGetBackHP);
                }
                TargetCret.StruckDamage(nDamage);
                TargetCret.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, TargetCret.m_WAbil.HP, TargetCret.m_WAbil.MaxHP, ObjectId, "", 200);
            }
            SendRefMsg(Grobal2.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ObjectId, "");
        }

        public override void Run()
        {
            if (!Death && !bo554 && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (m_WAbil.HP < m_WAbil.MaxHP / 2)// 血量低于一半时开始用魔法攻击
                {
                    m_boUseMagic = true;
                }
                else
                {
                    m_boUseMagic = false;
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
                    if (m_boUseMagic || nX == 2 || nY == 2)
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

