using M2Server.Actor;
using OpenMir2;
using SystemModule.Actors;

namespace M2Server.Monster.Monsters
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
            bool bo0D = false;
            if (N550 > 0)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, Dir, 1, ref n08, ref n0C))
                {
                    if (Envir.CanWalk(n08, n0C, bo0D))
                    {
                        switch (Dir)
                        {
                            case 0:
                                Dir = 4;
                                break;
                            case 1:
                                Dir = 7;
                                break;
                            case 2:
                                Dir = 6;
                                break;
                            case 3:
                                Dir = 5;
                                break;
                            case 4:
                                Dir = 0;
                                break;
                            case 5:
                                Dir = 3;
                                break;
                            case 6:
                                Dir = 2;
                                break;
                            case 7:
                                Dir = 1;
                                break;
                        }
                        Envir.GetNextPosition(CurrX, CurrY, Dir, N550, ref TargetX, ref TargetY);
                    }
                }
            }
            else
            {
                TargetX = -1;
            }
            if (TargetX != -1)
            {
                GotoTargetXY();
                if (TargetX == CurrX && TargetY == CurrY)
                {
                    N550 = 0;
                }
            }
            base.Run();
        }

        public override void Struck(IActor hiter)
        {
            if (hiter == null)
            {
                return;
            }
            Dir = hiter.Dir;
            N550 = M2Share.RandomNumber.Random(4) + N550 + 4;
            N550 = HUtil32._MIN(20, N550);
            Envir.GetNextPosition(CurrX, CurrY, Dir, N550, ref TargetX, ref TargetY);
        }
    }
}
