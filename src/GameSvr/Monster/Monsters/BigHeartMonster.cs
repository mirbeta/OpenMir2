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
                var WAbil = MWAbil;
                var nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    var BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject.Death)
                    {
                        continue;
                    }
                    if (IsProperTarget(BaseObject))
                    {
                        if (Math.Abs(CurrX - BaseObject.CurrX) <= ViewRange && Math.Abs(CurrY - BaseObject.CurrY) <= ViewRange)
                        {
                            SendDelayMsg(this, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(BaseObject.CurrX, BaseObject.CurrY), 1, BaseObject.ObjectId, "", 200);
                            SendRefMsg(Grobal2.RM_10205, 0, BaseObject.CurrX, BaseObject.CurrY, 1, "");
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public override void Run()
        {
            if (!Ghost && !Death && MWStatusTimeArr[Grobal2.POISON_STONE] == 0)
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

