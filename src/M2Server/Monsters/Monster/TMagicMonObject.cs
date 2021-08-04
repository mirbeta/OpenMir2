using System;
using SystemModule;

namespace M2Server
{
    public class TMagicMonObject : TMonster
    {
        private bool m_boUseMagic = false;

        public TMagicMonObject() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_boUseMagic = false;
        }

        private void LightingAttack(int nDir)
        {

        }

        public override void Run()
        {
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                // 血量低于一半时开始用魔法攻击
                if (m_WAbil.HP < m_WAbil.MaxHP / 2)
                {
                    m_boUseMagic = true;
                }
                else
                {
                    m_boUseMagic = false;
                }
                if (HUtil32.GetTickCount() - m_dwSearchEnemyTick > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if (m_Master == null)
                {
                    return;
                }
                var nX = Math.Abs(m_nCurrX - m_Master.m_nCurrX);
                var nY = Math.Abs(m_nCurrY - m_Master.m_nCurrY);
                if (nX <= 5 && nY <= 5)
                {
                    if (m_boUseMagic || nX == 5 || nY == 5)
                    {
                        if (HUtil32.GetTickCount() - m_dwHitTick > m_nNextHitTime)
                        {
                            m_dwHitTick = HUtil32.GetTickCount();
                            int nAttackDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_Master.m_nCurrX, m_Master.m_nCurrY);
                            LightingAttack(nAttackDir);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

