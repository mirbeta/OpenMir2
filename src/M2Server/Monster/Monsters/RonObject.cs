using OpenMir2;
using SystemModule.Actors;

namespace M2Server.Monster.Monsters
{
    public class RonObject : MonsterObject
    {
        public RonObject()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        private void AroundAttack()
        {
            byte attDir = 0;
            GetAttackDir(TargetCret, ref attDir);
            Dir = attDir;
            IList<IActor> objectList = new List<IActor>();
            GetMapBaseObjects(Envir, CurrX, CurrY, 1, ref objectList);
            if (objectList.Count > 0)
            {
                int nPower = GetBaseAttackPoewr();
                for (int i = objectList.Count - 1; i >= 0; i--)
                {
                    if (objectList[i] != null)
                    {
                        _Attack(nPower, objectList[i]);
                        objectList.RemoveAt(i);
                    }
                }
            }
            SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
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
                if (TargetCret != null && Math.Abs(CurrX - TargetCret.CurrX) < 6 && Math.Abs(CurrY - TargetCret.CurrY) < 6 && (HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    AroundAttack();
                }
            }
            base.Run();
        }
    }
}