using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class LightingZombi : MonsterObject
    {
        public LightingZombi() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        private void LightingAttack(byte nDir)
        {
            short nSX = 0;
            short nSY = 0;
            short nTX = 0;
            short nTY = 0;
            Direction = nDir;
            SendRefMsg(Grobal2.RM_LIGHTING, 1, m_nCurrX, m_nCurrY, m_TargetCret.ObjectId, "");
            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, nDir, 1, ref nSX, ref nSY))
            {
                m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, nDir, 9, ref nTX, ref nTY);
                var WAbil = m_WAbil;
                var nPwr = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                MagPassThroughMagic(nSX, nSY, nTX, nTY, nDir, nPwr, true);
                BreakHolySeizeMode();
            }
        }

        public override void Run()
        {
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000)
            {
                if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if ((HUtil32.GetTickCount() - m_dwWalkTick) > m_nWalkSpeed && m_TargetCret != null && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4)
                {
                    if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 2 && M2Share.RandomNumber.Random(3) != 0)
                    {
                        base.Run();
                        return;
                    }
                    GetBackPosition(ref m_nTargetX, ref m_nTargetY);
                }
                if (m_TargetCret != null && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) < 6 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) < 6 && (HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                    var nAttackDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                    LightingAttack(nAttackDir);
                }
            }
            base.Run();
        }
    }
}

