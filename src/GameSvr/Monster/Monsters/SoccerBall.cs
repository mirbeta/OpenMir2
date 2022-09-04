using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class SoccerBall : AnimalObject
    {
        public int n548 = 0;
        public int n54C = 0;
        public int n550 = 0;

        public SoccerBall() : base()
        {
            Animal = false;
            SuperMan = true;
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
                if (m_PEnvir.GetNextPosition(CurrX, CurrY, Direction, 1, ref n08, ref n0C))
                {
                    if (m_PEnvir.CanWalk(n08, n0C, bo0D))
                    {
                        switch (Direction)
                        {
                            case 0:
                                Direction = 4;
                                break;
                            case 1:
                                Direction = 7;
                                break;
                            case 2:
                                Direction = 6;
                                break;
                            case 3:
                                Direction = 5;
                                break;
                            case 4:
                                Direction = 0;
                                break;
                            case 5:
                                Direction = 3;
                                break;
                            case 6:
                                Direction = 2;
                                break;
                            case 7:
                                Direction = 1;
                                break;
                        }
                        m_PEnvir.GetNextPosition(CurrX, CurrY, Direction, n550, ref m_nTargetX, ref m_nTargetY);
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
                if (m_nTargetX == CurrX && m_nTargetY == CurrY)
                {
                    n550 = 0;
                }
            }
            base.Run();
        }

        public override void Struck(TBaseObject hiter)
        {
            if (hiter == null)
            {
                return;
            }
            Direction = hiter.Direction;
            n550 = M2Share.RandomNumber.Random(4) + n550 + 4;
            n550 = HUtil32._MIN(20, n550);
            m_PEnvir.GetNextPosition(CurrX, CurrY, Direction, n550, ref m_nTargetX, ref m_nTargetY);
        }
    }
}

