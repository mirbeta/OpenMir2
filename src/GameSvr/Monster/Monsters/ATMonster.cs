using GameSvr.Actor;

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
            if (CanMove())
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }

        /// <summary>
        /// 属下攻击
        /// </summary>
        protected void SlaveAttackTarget()
        {
            if (TargetCret != null)
            {
                if (TargetCret.Death || TargetCret.Ghost)
                {
                    TargetCret = null;
                }
                base.Run();
                return;
            }
            if (Master != null && (Master.TargetCret != null || Master.LastHiter != null))
            {
                BaseObject attackTarget = null;
                int n10 = 999;
                for (int i = 0; i < Master.VisibleActors.Count; i++) //共享主人的视野
                {
                    BaseObject baseObject = Master.VisibleActors[i].BaseObject;
                    if (baseObject.Death || baseObject.Ghost || (baseObject.Envir != Envir) || (Math.Abs(baseObject.CurrX - CurrX) > 15) || (Math.Abs(baseObject.CurrY - CurrY) > 15))
                    {
                        ClearTargetCreat(baseObject);
                        continue;
                    }
                    if (this.IsProperTarget(baseObject) && (!baseObject.HideMode || this.CoolEye))
                    {
                        int nC = Math.Abs(this.CurrX - baseObject.CurrX) + Math.Abs(this.CurrY - baseObject.CurrY);
                        if (nC < n10)
                        {
                            n10 = nC;
                            attackTarget = baseObject;
                        }
                    }
                }
                if (attackTarget != null)
                {
                    this.SetTargetCreat(attackTarget);
                }
            }
            base.Run();
        }
    }
}