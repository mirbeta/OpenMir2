using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ScultureMonster : MonsterObject
    {
        public ScultureMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 7;
            StoneMode = true;
            m_nCharStatusEx = Grobal2.STATE_STONE_MODE;
        }

        private void MeltStone()
        {
            m_nCharStatusEx = 0;
            CharStatus = GetCharStatus();
            SendRefMsg(Grobal2.RM_DIGUP, Direction, CurrX, CurrY, 0, "");
            StoneMode = false;
        }

        private void MeltStoneAll()
        {
            TBaseObject BaseObject;
            MeltStone();
            IList<TBaseObject> List10 = new List<TBaseObject>();
            GetMapBaseObjects(Envir, CurrX, CurrY, 7, List10);
            for (var i = 0; i < List10.Count; i++)
            {
                BaseObject = List10[i];
                if (BaseObject.StoneMode)
                {
                    if (BaseObject is ScultureMonster)
                    {
                        (BaseObject as ScultureMonster).MeltStone();
                    }
                }
            }
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
            {
                if (StoneMode)
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
                                if (Math.Abs(CurrX - BaseObject.CurrX) <= 2 && Math.Abs(CurrY - BaseObject.CurrY) <= 2)
                                {
                                    MeltStoneAll();
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

