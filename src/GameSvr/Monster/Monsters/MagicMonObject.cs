using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class MagicMonObject : MonsterObject
    {
        public bool m_boUseMagic = false;

        public MagicMonObject() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_boUseMagic = false;
        }

        private void LightingAttack(int nDir)
        {

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
                if (m_Master == null)
                {
                    return;
                }
                var nX = Math.Abs(CurrX - m_Master.CurrX);
                var nY = Math.Abs(CurrY - m_Master.CurrY);
                if (nX <= 5 && nY <= 5)
                {
                    if (m_boUseMagic || nX == 5 || nY == 5)
                    {
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            int nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, m_Master.CurrX, m_Master.CurrY);
                            LightingAttack(nAttackDir);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

