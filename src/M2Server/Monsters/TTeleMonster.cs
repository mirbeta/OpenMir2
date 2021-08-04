using System;
using SystemModule;

namespace M2Server
{
    public class TTeleMonster: TMonster
    {
        public TTeleMonster() : base()
        {
            this.m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!this.m_boDeath && !this.bo554 && !this.m_boGhost && this.m_wStatusTimeArr[grobal2.POISON_STONE] == 0)
            {
                if (this.m_TargetCret != null)
                {
                    if (Math.Abs(this.m_nCurrX - this.m_nTargetX) > 5 || Math.Abs(this.m_nCurrY - this.m_nTargetY) > 5)
                    {
                        this.SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        this.SpaceMove(this.m_TargetCret.m_sMapName, this.m_TargetCret.m_nCurrX, this.m_TargetCret.m_nCurrY, 0);
                    }
                }
                if (HUtil32.GetTickCount() - this.m_dwSearchEnemyTick > 8000 || HUtil32.GetTickCount() - this.m_dwSearchEnemyTick > 1000 && this.m_TargetCret == null)
                {
                    this.m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    this.SearchTarget();
                }
            }
            base.Run();
        }
    }
}

