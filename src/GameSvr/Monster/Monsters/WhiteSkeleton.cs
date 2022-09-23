using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    /// <summary>
    /// 变异骷髅
    /// </summary>
    public class WhiteSkeleton : AtMonster
    {
        public bool MBoIsFirst;

        public WhiteSkeleton() : base()
        {
            MBoIsFirst = true;
            this.FixedHideMode = true;
            this.ViewRange = 6;
            Race = Grobal2.RC_WHITESKELETON;
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            this.NextHitTime = 3000 - this.SlaveMakeLevel * 600;
            this.WalkSpeed = 1200 - this.SlaveMakeLevel * 250;
            this.WalkTick = HUtil32.GetTickCount() + 2000;
        }

        public override void Run()
        {
            if (MBoIsFirst)
            {
                MBoIsFirst = false;
                this.Direction = 5;
                this.FixedHideMode = false;
                this.SendRefMsg(Grobal2.RM_DIGUP, this.Direction, this.CurrX, this.CurrY, 0, "");
            }
            SavleAttackTarget();
        }
    }
}