using M2Server.Npc;
using SystemModule.Data;

namespace GameSrv.NPC
{
    /// <summary>
    /// 练功师
    /// </summary>
    public class Trainer : NormNpc
    {
        private int AttackPower { get; set; }
        private int AttackCount { get; set; }

        public Trainer() : base()
        {
            AttackTick = HUtil32.GetTickCount();
            AttackPower = 0;
            AttackCount = 0;
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            bool result = false;
            if (ProcessMsg.wIdent == Messages.RM_STRUCK || ProcessMsg.wIdent == Messages.RM_MAGSTRUCK)
            {
                if (ProcessMsg.ActorId == ActorId)
                {
                    AttackPower += ProcessMsg.wParam;
                    AttackTick = HUtil32.GetTickCount();
                    AttackCount++;
                    ProcessSayMsg("破坏力为 " + ProcessMsg.wParam + ",平均值为 " + AttackPower / AttackCount);
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
            if (AttackCount > 0)
            {
                if ((HUtil32.GetTickCount() - AttackTick) > 3 * 1000)
                {
                    ProcessSayMsg("总破坏力为  " + AttackPower + ",平均值为 " + AttackPower / AttackCount);
                    AttackCount = 0;
                    AttackPower = 0;
                }
            }
            base.Run();
        }
    }
}