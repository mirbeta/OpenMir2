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
            int n10 = 9999;
            TBaseObject BaseObject1C = null;
            TBaseObject BaseObject = null;
            if (!Death && !bo554 && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
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
                                var nC = Math.Abs(CurrX - BaseObject.CurrX) + Math.Abs(CurrY - BaseObject.CurrY);
                                if (nC < n10)
                                {
                                    n10 = nC;
                                    BaseObject1C = BaseObject;
                                }
                            }
                        }
                    }
                    if (BaseObject1C != null)
                    {
                        m_boRunAwayMode = true;
                        TargetCret = BaseObject1C;
                    }
                    else
                    {
                        m_boRunAwayMode = false;
                        TargetCret = null;
                    }
                }
                if (m_boRunAwayMode && TargetCret != null && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    if (Math.Abs(CurrX - BaseObject.CurrX) <= 6 && Math.Abs(CurrX - BaseObject.CurrX) <= 6)
                    {
                        int n14 = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                        m_PEnvir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, n14, 5, ref m_nTargetX, ref m_nTargetY);
                    }
                }
            }
            base.Run();
        }
    }
}