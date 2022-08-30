using SystemModule;
using SystemModule.Data;

namespace GameSvr.Npc
{
    /// <summary>
    /// 练功师
    /// </summary>
    public class Trainer : NormNpc
    {
        private int AttackTick = 0;
        private int AttackPower = 0;
        private int AttackCount = 0;

        public Trainer() : base()
        {
            AttackTick = HUtil32.GetTickCount();
            AttackPower = 0;
            AttackCount = 0;
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            var result = false;
            if (ProcessMsg.wIdent == Grobal2.RM_STRUCK || ProcessMsg.wIdent == Grobal2.RM_MAGSTRUCK)
            {
                if (ProcessMsg.BaseObject == this.ObjectId)
                {
                    AttackPower += ProcessMsg.wParam;
                    AttackTick = HUtil32.GetTickCount();
                    AttackCount++;
                    this.ProcessSayMsg("破坏力为 " + ProcessMsg.wParam + ",平均值为 " + AttackPower / AttackCount);
                }
            }
            if (ProcessMsg.wIdent == Grobal2.RM_MAGSTRUCK)
            {
                result = base.Operate(ProcessMsg);
            }
            return result;
        }

        public override void Run()
        {
            if (AttackCount > 0)
            {
                if ((HUtil32.GetTickCount() - AttackTick) > 3 * 1000)
                {
                    this.ProcessSayMsg("总破坏力为  " + AttackPower + ",平均值为 " + AttackPower / AttackCount);
                    AttackCount = 0;
                    AttackPower = 0;
                }
            }
            base.Run();
        }
    }
}

