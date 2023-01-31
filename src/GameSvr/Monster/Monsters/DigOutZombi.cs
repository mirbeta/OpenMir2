﻿using GameSvr.Event;

namespace GameSvr.Monster.Monsters
{
    public class DigOutZombi : MonsterObject
    {
        public DigOutZombi() : base()
        {
            ViewRange = 7;
            SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            SearchTick = HUtil32.GetTickCount();
            FixedHideMode = true;
        }

        private void sub_4AA8DC()
        {
            EventInfo digEvent = new EventInfo(Envir, CurrX, CurrY, 1, 5 * 60 * 1000, true);
            M2Share.EventMgr.AddEvent(digEvent);
            FixedHideMode = false;
            SendRefMsg(Messages.RM_DIGUP, Direction, CurrX, CurrY, digEvent.Id, "");
        }

        public override void Run()
        {
            if (CanMove() && (HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
            {
                if (FixedHideMode)
                {
                    for (int i = 0; i < VisibleActors.Count; i++)
                    {
                        Actor.BaseObject baseObject = VisibleActors[i].BaseObject;
                        if (baseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(baseObject))
                        {
                            if (!baseObject.HideMode || CoolEye)
                            {
                                if (Math.Abs(CurrX - baseObject.CurrX) <= 3 && Math.Abs(CurrY - baseObject.CurrY) <= 3)
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

