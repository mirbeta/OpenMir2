namespace M2Server
{
    public class TSoccerBall : TAnimalObject
    {
        public int n548 = 0;
        public int n54C = 0;
        public int n550 = 0;

        public TSoccerBall() : base()
        {
            m_boAnimal = false;
            m_boSuperMan = true;
            n550 = 0;
            m_nTargetX = -1;
        }

        public override void Run()
        {
            short n08 = 0;
            short n0C = 0;
            var bo0D = false;
            if (n550 > 0)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 1, ref n08, ref n0C))
                {
                    if (m_PEnvir.CanWalk(n08, n0C, bo0D))
                    {
                        switch (m_btDirection)
                        {
                            case 0:
                                m_btDirection = 4;
                                break;
                            case 1:
                                m_btDirection = 7;
                                break;
                            case 2:
                                m_btDirection = 6;
                                break;
                            case 3:
                                m_btDirection = 5;
                                break;
                            case 4:
                                m_btDirection = 0;
                                break;
                            case 5:
                                m_btDirection = 3;
                                break;
                            case 6:
                                m_btDirection = 2;
                                break;
                            case 7:
                                m_btDirection = 1;
                                break;
                        }
                        // case
                        m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, n550, ref m_nTargetX, ref m_nTargetY);
                    }
                }
            }
            else
            {
                m_nTargetX = -1;
            }
            if (m_nTargetX != -1)
            {
                GotoTargetXY();
                if ((m_nTargetX == m_nCurrX) && (m_nTargetY == m_nCurrY))
                {
                    n550 = 0;
                }
            }
            base.Run();
        }

        public virtual void Struck(TBaseObject hiter)
        {
            if (hiter == null)
            {
                return;
            }
            m_btDirection = hiter.m_btDirection;
            n550 = M2Share.RandomNumber.Random(4) + n550 + 4;
            n550 = HUtil32._MIN(20, n550);
            m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, n550, ref m_nTargetX, ref m_nTargetY);
        }
    }
}

