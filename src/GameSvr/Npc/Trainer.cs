using SystemModule.Data;

namespace GameSvr.Npc
{
    /// <summary>
    /// 练功师
    /// </summary>
    public class Trainer : NormNpc
    {
        //private int AttackTick;
        private int _attackPower;
        private int _attackCount;

        public Trainer() : base()
        {
            AttackTick = HUtil32.GetTickCount();
            _attackPower = 0;
            _attackCount = 0;
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            bool result = false;
            if (ProcessMsg.wIdent == Messages.RM_STRUCK || ProcessMsg.wIdent == Messages.RM_MAGSTRUCK)
            {
                if (ProcessMsg.BaseObject == ActorId)
                {
                    _attackPower += ProcessMsg.wParam;
                    AttackTick = HUtil32.GetTickCount();
                    _attackCount++;
                    ProcessSayMsg("破坏力为 " + ProcessMsg.wParam + ",平均值为 " + _attackPower / _attackCount);
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
            if (_attackCount > 0)
            {
                if ((HUtil32.GetTickCount() - AttackTick) > 3 * 1000)
                {
                    ProcessSayMsg("总破坏力为  " + _attackPower + ",平均值为 " + _attackPower / _attackCount);
                    _attackCount = 0;
                    _attackPower = 0;
                }
            }
            base.Run();
        }
    }
}

