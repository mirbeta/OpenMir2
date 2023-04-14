namespace GameSrv.Monster.Monsters {
    public class SandMobObject : StickMonster {
        private int _mDwAppearStart;

        public SandMobObject() : base() {
            ComeOutValue = 8;
        }

        public override void Run() {
            if (!Death && !Ghost) {
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed) {
                    WalkTick = HUtil32.GetTickCount();
                    if (FixedHideMode) {
                        if (WAbil.HP > WAbil.MaxHP / 20 && CheckComeOut()) {
                            _mDwAppearStart = HUtil32.GetTickCount();
                        }
                    }
                    else {
                        if (WAbil.HP > 0 && WAbil.HP < WAbil.MaxHP / 20 && (HUtil32.GetTickCount() - _mDwAppearStart) > 3000) {
                            ComeDown();
                        }
                        else if (TargetCret != null) {
                            if (Math.Abs(CurrX - TargetCret.CurrX) > 15 && Math.Abs(CurrY - TargetCret.CurrY) > 15) {
                                ComeDown();
                                return;
                            }
                        }
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime) {
                            SearchTarget();
                        }
                        if (!FixedHideMode) {
                            if (AttackTarget()) {
                                base.Run();
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

