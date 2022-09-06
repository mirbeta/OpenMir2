using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class RunAway : MonsterObject
    {
        public RunAway() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            var time1 = 0;
            short nx = 0;
            short ny = 0;
            var borunaway = false;
            if (!Death && !Ghost)
            {
                if (TargetCret != null)
                {
                    TargetX = TargetCret.CurrX;
                    TargetY = TargetCret.CurrY;
                    if (Abil.HP <= HUtil32.Round(Abil.MaxHP / 2))
                    {
                        GetFrontPosition(ref nx, ref ny);
                        SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        SpaceMove(MapName, (short)(nx - 2), (short)(ny - 2), 0);
                        borunaway = true;
                    }
                    else
                    {
                        if (Abil.HP >= HUtil32.Round(Abil.MaxHP / 2))
                        {
                            borunaway = false;
                        }
                    }
                    if (borunaway)
                    {
                        if ((HUtil32.GetTickCount() - time1) > 5000)
                        {
                            if (Math.Abs(TargetX - CurrX) == 1 && Math.Abs(TargetY - CurrY) == 1)
                            {
                                WalkTo(M2Share.RandomNumber.RandomByte(4), true);
                            }
                            else
                            {
                                WalkTo(M2Share.RandomNumber.RandomByte(7), true);
                            }
                        }
                        else
                        {
                            time1 = HUtil32.GetTickCount();
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

