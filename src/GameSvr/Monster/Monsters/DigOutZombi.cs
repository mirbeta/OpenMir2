using GameSvr.Actor;
using GameSvr.Event;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class DigOutZombi : MonsterObject
    {
        public DigOutZombi() : base()
        {
            bo554 = false;
            ViewRange = 7;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = HUtil32.GetTickCount();
            FixedHideMode = true;
        }

        private void sub_4AA8DC()
        {
            var digEvent = new MirEvent(Envir, CurrX, CurrY, 1, 5 * 60 * 1000, true);
            M2Share.EventManager.AddEvent(digEvent);
            FixedHideMode = false;
            SendRefMsg(Grobal2.RM_DIGUP, Direction, CurrX, CurrY, digEvent.Id, "");
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
            {
                if (FixedHideMode)
                {
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        BaseObject = VisibleActors[i].BaseObject;
                        if (BaseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            if (!BaseObject.HideMode || CoolEye)
                            {
                                if (Math.Abs(CurrX - BaseObject.CurrX) <= 3 && Math.Abs(CurrY - BaseObject.CurrY) <= 3)
                                {
                                    sub_4AA8DC();
                                    WalkTick = HUtil32.GetTickCount() + 1000;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                    {
                        SearchEnemyTick = HUtil32.GetTickCount();
                        SearchTarget();
                    }
                }
            }
            base.Run();
        }
    }
}

