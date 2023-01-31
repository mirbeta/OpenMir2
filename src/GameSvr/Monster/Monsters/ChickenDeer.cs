using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ChickenDeer : MonsterObject
    {
        public ChickenDeer() : base()
        {
            ViewRange = 5;
        }

        public override void Run()
        {
            if (CanMove())
            {
                int n10 = 9999;
                BaseObject baseObject1C = null;
                BaseObject baseObject = null;
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        baseObject = VisibleActors[i].BaseObject;
                        if (baseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(baseObject))
                        {
                            if (!baseObject.HideMode || CoolEye)
                            {
                                var nC = Math.Abs(CurrX - baseObject.CurrX) + Math.Abs(CurrY - baseObject.CurrY);
                                if (nC < n10)
                                {
                                    n10 = nC;
                                    baseObject1C = baseObject;
                                }
                            }
                        }
                    }
                    if (baseObject1C != null)
                    {
                        MBoRunAwayMode = true;
                        TargetCret = baseObject1C;
                    }
                    else
                    {
                        MBoRunAwayMode = false;
                        TargetCret = null;
                    }
                }
                if (MBoRunAwayMode && TargetCret != null && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    if (Math.Abs(CurrX - baseObject.CurrX) <= 6 && Math.Abs(CurrX - baseObject.CurrX) <= 6)
                    {
                        var n14 = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                        Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, n14, 5, ref TargetX, ref TargetY);
                    }
                }
            }
            base.Run();
        }
    }
}