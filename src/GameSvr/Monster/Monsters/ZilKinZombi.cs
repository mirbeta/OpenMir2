using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ZilKinZombi : AtMonster
    {
        private int dw558;
        private int ZilKillCount;
        private int dw560;

        public ZilKinZombi() : base()
        {
            this.ViewRange = 6;
            this.SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.SearchTick = HUtil32.GetTickCount();
            this.ZilKillCount = 0;
            if (M2Share.RandomNumber.Random(3) == 0)
            {
                ZilKillCount = M2Share.RandomNumber.Random(3) + 1;
            }
        }

        public override void Die()
        {
            base.Die();
            if (ZilKillCount > 0)
            {
                dw558 = HUtil32.GetTickCount();
                dw560 = (M2Share.RandomNumber.Random(20) + 4) * 1000;
            }
            ZilKillCount -= 1;
        }

        public override void Run()
        {
            if (this.Death && !this.Ghost && ZilKillCount >= 0 && this.MWStatusTimeArr[Grobal2.POISON_STONE] == 0 && this.VisibleActors.Count > 0 && (HUtil32.GetTickCount() - dw558) >= dw560)
            {
                this.Abil.MaxHP = (ushort)(this.Abil.MaxHP >> 1);
                this.MDwFightExp = this.MDwFightExp / 2;
                this.Abil.HP = this.Abil.MaxHP;
                this.MWAbil.HP = this.Abil.MaxHP;
                this.ReAlive();
                this.WalkTick = HUtil32.GetTickCount() + 1000;
            }
            base.Run();
        }
    }
}

