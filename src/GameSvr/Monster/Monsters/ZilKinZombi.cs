using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ZilKinZombi : AtMonster
    {
        private int dw558 = 0;
        private int nZilKillCount = 0;
        private int dw560 = 0;

        public ZilKinZombi() : base()
        {
            this.ViewRange = 6;
            this.SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.SearchTick = HUtil32.GetTickCount();
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
                dw558 = HUtil32.GetTickCount();
                dw560 = (M2Share.RandomNumber.Random(20) + 4) * 1000;
            }
            nZilKillCount -= 1;
        }

        public override void Run()
        {
            if (this.Death && !this.Ghost && nZilKillCount >= 0 && this.m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && this.VisibleActors.Count > 0 && (HUtil32.GetTickCount() - dw558) >= dw560)
            {
                this.Abil.MaxHP = (ushort)(this.Abil.MaxHP >> 1);
                this.m_dwFightExp = this.m_dwFightExp / 2;
                this.Abil.HP = this.Abil.MaxHP;
                this.m_WAbil.HP = this.Abil.MaxHP;
                this.ReAlive();
                this.WalkTick = HUtil32.GetTickCount() + 1000;
            }
            base.Run();
        }
    }
}

