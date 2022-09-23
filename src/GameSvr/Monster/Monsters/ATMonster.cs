using GameSvr.Actor;
using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class AtMonster : MonsterObject
    {
        public AtMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (CanWalk())
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }

        public void SavleAttackTarget()
        {
            if (TargetCret != null)
            {
                if (TargetCret.Death || TargetCret.Ghost)
                {
                    TargetCret = null;
                }
                base.Run();
            }
            if (Master != null && (Master.TargetCret != null || Master.LastHiter != null))
            {
                BaseObject BaseObject18 = null;
                var n10 = 999;
                for (var i = 0; i < Master.VisibleActors.Count; i++)
                {
                    var baseObject = Master.VisibleActors[i].BaseObject;
                    if (baseObject.Death || baseObject.Ghost || (baseObject.Envir != Envir) || (Math.Abs(baseObject.CurrX - CurrX) > 15) || (Math.Abs(baseObject.CurrY - CurrY) > 15))
                    {
                        ClearTargetCreat(baseObject);
                        continue;
                    }
                    if (!baseObject.Death)
                    {
                        if (this.IsProperTarget(baseObject) && (!baseObject.HideMode || this.CoolEye))
                        {
                            var nC = Math.Abs(this.CurrX - baseObject.CurrX) + Math.Abs(this.CurrY - baseObject.CurrY);
                            if (nC < n10)
                            {
                                n10 = nC;
                                BaseObject18 = baseObject;
                            }
                        }
                    }
                }
                if (BaseObject18 != null)
                {
                    this.SetTargetCreat(BaseObject18);
                }
            }
            base.Run();
        }
    }
}