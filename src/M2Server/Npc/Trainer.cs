using SystemModule;
using SystemModule.Data;

namespace M2Server.Npc
{
    /// <summary>
    /// 练功师
    /// </summary>
    public class Trainer : NormNpc
    {
        int attackPower;
        int attackCount;

        public Trainer() : base()
        {
            AttackTick = HUtil32.GetTickCount();
            attackPower = 0;
            attackCount = 0;
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            bool result = false;
            if (ProcessMsg.wIdent == Messages.RM_STRUCK || ProcessMsg.wIdent == Messages.RM_MAGSTRUCK)
            {
                if (ProcessMsg.ActorId == ActorId)
                {
                    attackPower += ProcessMsg.wParam;
                    AttackTick = HUtil32.GetTickCount();
                    attackCount++;
                    ProcessSayMsg("破坏力为 " + ProcessMsg.wParam + ",平均值为 " + attackPower / attackCount);
                }
            }
            if (ProcessMsg.wIdent == Messages.RM_MAGSTRUCK)
            {
                result = base.Operate(ProcessMsg);
            }
            return result;
        }

        public override void Run()
        {
            if (attackCount > 0)
            {
                if ((HUtil32.GetTickCount() - AttackTick) > 3 * 1000)
                {
                    ProcessSayMsg("总破坏力为  " + attackPower + ",平均值为 " + attackPower / attackCount);
                    attackCount = 0;
                    attackPower = 0;
                }
            }
            base.Run();
        }
    }
}