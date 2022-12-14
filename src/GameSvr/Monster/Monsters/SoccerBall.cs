using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class SoccerBall : AnimalObject
    {
        public int N550;

        public SoccerBall() : base()
        {
            Animal = false;
            SuperMan = true;
            N550 = 0;
            TargetX = -1;
        }

        public override void Run()
        {
            short n08 = 0;
            short n0C = 0;
            var bo0D = false;
            if (N550 > 0)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, Direction, 1, ref n08, ref n0C))
                {
                    if (Envir.CanWalk(n08, n0C, bo0D))
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
                        Envir.GetNextPosition(CurrX, CurrY, Direction, N550, ref TargetX, ref TargetY);
                    }
                }
            }
            else
            {
                TargetX = -1;
            }
            if (TargetX != -1)
            {
                GotoTargetXy();
                if (TargetX == CurrX && TargetY == CurrY)
                {
                    N550 = 0;
                }
            }
            base.Run();
        }

        public override void Struck(BaseObject hiter)
        {
            if (hiter == null)
            {
                return;
            }
            Direction = hiter.Direction;
            N550 = M2Share.RandomNumber.Random(4) + N550 + 4;
            N550 = HUtil32._MIN(20, N550);
            Envir.GetNextPosition(CurrX, CurrY, Direction, N550, ref TargetX, ref TargetY);
        }
    }
}

