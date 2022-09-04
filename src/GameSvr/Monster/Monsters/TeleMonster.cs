using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class TeleMonster : MonsterObject
    {
        public TeleMonster() : base()
        {
            this.SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!this.Death && !this.bo554 && !this.Ghost && this.m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if (this.TargetCret != null)
                {
                    if (Math.Abs(this.CurrX - this.m_nTargetX) > 5 || Math.Abs(this.CurrY - this.m_nTargetY) > 5)
                    {
                        this.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        this.SpaceMove(this.TargetCret.MapName, this.TargetCret.CurrX, this.TargetCret.CurrY, 0);
                    }
                }
                if ((HUtil32.GetTickCount() - this.SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - this.SearchEnemyTick) > 1000 && this.TargetCret == null)
                {
                    this.SearchEnemyTick = HUtil32.GetTickCount();
                    this.SearchTarget();
                }
            }
            base.Run();
        }
    }
}

