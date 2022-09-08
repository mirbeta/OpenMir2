using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ZilKinZombi : AtMonster
    {
        private int _dw558;
        private int _zilKillCount;
        private int _dw560;

        public ZilKinZombi() : base()
        {
            this.ViewRange = 6;
            this.SearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            this.SearchTick = HUtil32.GetTickCount();
            this._zilKillCount = 0;
            if (M2Share.RandomNumber.Random(3) == 0)
            {
                _zilKillCount = M2Share.RandomNumber.Random(3) + 1;
            }
        }

        public override void Die()
        {
            base.Die();
            if (_zilKillCount > 0)
            {
                _dw558 = HUtil32.GetTickCount();
                _dw560 = (M2Share.RandomNumber.Random(20) + 4) * 1000;
            }
            _zilKillCount -= 1;
        }

        public override void Run()
        {
            if (this.Death && !this.Ghost && _zilKillCount >= 0 && this.StatusTimeArr[Grobal2.POISON_STONE] == 0 && this.VisibleActors.Count > 0 && (HUtil32.GetTickCount() - _dw558) >= _dw560)
            {
                this.Abil.MaxHP = (ushort)(this.Abil.MaxHP >> 1);
                this.FightExp = this.FightExp / 2;
                this.Abil.HP = this.Abil.MaxHP;
                this.Abil.HP = this.Abil.MaxHP;
                this.ReAlive();
                this.WalkTick = HUtil32.GetTickCount() + 1000;
            }
            base.Run();
        }
    }
}

