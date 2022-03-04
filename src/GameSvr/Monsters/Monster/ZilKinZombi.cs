using SystemModule;

namespace GameSvr
{
    public class ZilKinZombi: AtMonster
    {
        private int dw558 = 0;
        private int nZilKillCount = 0;
        private int dw560 = 0;

        public ZilKinZombi() : base()
        {
            this.m_nViewRange = 6;
            this.m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.m_dwSearchTick =HUtil32.GetTickCount();
            nZilKillCount = 0;
            if (M2Share.RandomNumber.Random(3) == 0)
            {
                nZilKillCount = M2Share.RandomNumber.Random(3) + 1;
            }
        }

        public override void Die()
        {
            base.Die();
            if (nZilKillCount > 0)
            {
                dw558 =HUtil32.GetTickCount();
                dw560 = (M2Share.RandomNumber.Random(20) + 4) * 1000;
            }
            nZilKillCount -= 1;
        }

        public override void Run()
        {
            if (this.m_boDeath && !this.m_boGhost && nZilKillCount >= 0 && this.m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && this.m_VisibleActors.Count > 0 && (HUtil32.GetTickCount() - dw558) >= dw560)
            {
                this.m_Abil.MaxHP = (ushort)(this.m_Abil.MaxHP >> 1);
                this.m_dwFightExp = this.m_dwFightExp / 2;
                this.m_Abil.HP = this.m_Abil.MaxHP;
                this.m_WAbil.HP = this.m_Abil.MaxHP;
                this.ReAlive();
                this.m_dwWalkTick = HUtil32.GetTickCount() + 1000;
            }
            base.Run();
        }
    }
}

