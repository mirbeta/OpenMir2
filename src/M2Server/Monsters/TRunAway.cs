using System;
namespace M2Server
{
    public class TRunAway : TMonster
    {
        public TRunAway() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            var time1 = 0;
            short nx = 0;
            short ny = 0;
            var borunaway = false;
            if (!m_boDeath && !bo554 && !m_boGhost)
            {
                if (m_TargetCret != null)
                {
                    m_nTargetX = m_TargetCret.m_nCurrX;
                    m_nTargetY = m_TargetCret.m_nCurrY;
                    if (m_WAbil.HP <= HUtil32.Round(m_WAbil.MaxHP / 2) && borunaway == false)
                    {
                        GetFrontPosition(ref nx, ref ny);
                        SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        SpaceMove(m_sMapName, (short)(nx - 2), (short)(ny - 2), 0);
                        borunaway = true;
                    }
                    else
                    {
                        if (m_WAbil.HP >= HUtil32.Round(m_WAbil.MaxHP / 2))
                        {
                            borunaway = false;
                        }
                    }
                    if (borunaway)
                    {
                        if (HUtil32.GetTickCount() - time1 > 5000)
                        {
                            if (Math.Abs(m_nTargetX - m_nCurrX) == 1 && Math.Abs(m_nTargetY - m_nCurrY) == 1)
                            {
                                WalkTo((byte)M2Share.RandomNumber.Random(4), true);
                            }
                            else
                            {
                                WalkTo((byte)M2Share.RandomNumber.Random(7), true);
                            }
                        }
                        else
                        {
                            time1 = HUtil32.GetTickCount();
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

