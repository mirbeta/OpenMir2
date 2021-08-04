using System;
using SystemModule;

namespace M2Server
{
    public class TKhazard : TMonster
    {
        public TKhazard() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            short nx = 0;
            short ny = 0;
            var time1 = -1;
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[grobal2.POISON_STONE] == 0)
            {
                time1 = M2Share.RandomNumber.Random(2);
                if (m_TargetCret != null)
                {
                    m_nTargetX = m_TargetCret.m_nCurrX;
                    m_nTargetY = m_TargetCret.m_nCurrY;
                    if (Math.Abs(m_nTargetX - m_nCurrX) == 2 && Math.Abs(m_nTargetY - m_nCurrY) == 2)
                    {
                        if (time1 == 0)
                        {
                            // do drag back on random
                            GetFrontPosition(ref nx, ref ny);
                            m_TargetCret.SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            m_TargetCret.SpaceMove(m_sMapName, nx, ny, 0);
                            if (M2Share.RandomNumber.Random(1) == 0 && M2Share.RandomNumber.Random(m_TargetCret.m_btAntiPoison + 7) <= 6)
                            {
                                m_TargetCret.MakePosion(grobal2.POISON_DECHEALTH, 35, 2);
                                return;
                            }
                        }
                        else
                        {
                            if (m_TargetCret.m_WAbil.HP <= m_TargetCret.m_WAbil.MaxHP / 2)
                            {
                                // if target below half hp
                                GetFrontPosition(ref nx, ref ny);
                            }
                            m_TargetCret.SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            m_TargetCret.SpaceMove(m_sMapName, nx, ny, 0);
                            if (M2Share.RandomNumber.Random(1) == 0 && M2Share.RandomNumber.Random(m_TargetCret.m_btAntiPoison + 7) <= 6)
                            {
                                m_TargetCret.MakePosion(grobal2.POISON_DECHEALTH, 35, 2);
                                return;
                            }
                        }
                    }
                }
                if (HUtil32.GetTickCount() - m_dwSearchEnemyTick > 8000 || HUtil32.GetTickCount() - m_dwSearchEnemyTick > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

