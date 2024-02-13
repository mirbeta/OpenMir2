using OpenMir2;
using OpenMir2.Consts;
using SystemModule;

namespace M2Server.Monster.Monsters
{
    public class Khazard : MonsterObject
    {
        public Khazard() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            short nX = 0;
            short nY = 0;
            if (CanMove())
            {
                if (TargetCret != null)
                {
                    TargetX = TargetCret.CurrX;
                    TargetY = TargetCret.CurrY;
                    if (Math.Abs(TargetX - CurrX) == 2 && Math.Abs(TargetY - CurrY) == 2)
                    {
                        if (M2Share.RandomNumber.Random(2) == 0)
                        {
                            GetFrontPosition(ref nX, ref nY);
                            TargetCret.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            TargetCret.SpaceMove(MapName, nX, nY, 0);
                            if (M2Share.RandomNumber.Random(1) == 0 && M2Share.RandomNumber.Random(TargetCret.AntiPoison + 7) <= 6)
                            {
                                TargetCret.MakePosion(PoisonState.DECHEALTH, 35, 2);
                                return;
                            }
                        }
                        else
                        {
                            if (TargetCret.WAbil.HP <= TargetCret.WAbil.MaxHP / 2)
                            {
                                GetFrontPosition(ref nX, ref nY);
                            }
                            TargetCret.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            TargetCret.SpaceMove(MapName, nX, nY, 0);
                            if (M2Share.RandomNumber.Random(1) == 0 && M2Share.RandomNumber.Random(TargetCret.AntiPoison + 7) <= 6)
                            {
                                TargetCret.MakePosion(PoisonState.DECHEALTH, 35, 2);
                                return;
                            }
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

