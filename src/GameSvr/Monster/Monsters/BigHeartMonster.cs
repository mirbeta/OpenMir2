using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class BigHeartMonster : AnimalObject
    {
        public BigHeartMonster() : base()
        {
            ViewRange = 16;
            Animal = false;
        }

        protected virtual bool AttackTarget()
        {
            var result = false;
            if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
            {
                AttackTick = HUtil32.GetTickCount();
                SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                var nPower = M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1)) + HUtil32.LoWord(WAbil.DC);
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    var baseObject = VisibleActors[i].BaseObject;
                    if (baseObject.Death)
                    {
                        continue;
                    }
                    if (IsProperTarget(baseObject))
                    {
                        if (Math.Abs(CurrX - baseObject.CurrX) <= ViewRange && Math.Abs(CurrY - baseObject.CurrY) <= ViewRange)
                        {
                            SendDelayMsg(this, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(baseObject.CurrX, baseObject.CurrY), 1, baseObject.ActorId, "", 200);
                            SendRefMsg(Grobal2.RM_10205, 0, baseObject.CurrX, baseObject.CurrY, 1, "");
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public override void Run()
        {
            if (CanWalk())
            {
                if (VisibleActors.Count > 0)
                {
                    AttackTarget();
                }
            }
            base.Run();
        }
    }
}

