namespace GameSrv.Monster.Monsters {
    public class ElectronicScolpionMon : MonsterObject {
        public bool UseMagic;

        public ElectronicScolpionMon() : base() {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            UseMagic = false;
        }

        private void LightingAttack(byte nDir) {
            Direction = nDir;
            ushort nPower = GetAttackPower(HUtil32.LoByte(WAbil.DC), Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
            ushort nDamage = TargetCret.GetMagStruckDamage(this, nPower);
            if (nDamage > 0) {
                int btGetBackHp = HUtil32.LoByte(WAbil.MP);
                if (btGetBackHp != 0) {
                    WAbil.HP += (ushort)(nDamage / btGetBackHp);
                }
                TargetCret.StruckDamage(nDamage);
                TargetCret.SendDelayMsg(Messages.RM_STRUCK, Messages.RM_REFMESSAGE, nDamage, TargetCret.WAbil.HP, TargetCret.WAbil.MaxHP, ActorId, "", 200);
            }
            SendRefMsg(Messages.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ActorId, "");
        }

        public override void Run() {
            if (CanMove()) {
                if (WAbil.HP < WAbil.MaxHP / 2)// 血量低于一半时开始用魔法攻击
                {
                    UseMagic = true;
                }
                else {
                    UseMagic = false;
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null) {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if (TargetCret == null) {
                    return;
                }
                int nX = Math.Abs(CurrX - TargetCret.CurrX);
                int nY = Math.Abs(CurrY - TargetCret.CurrY);
                if (nX <= 2 && nY <= 2) {
                    if (UseMagic || nX == 2 || nY == 2) {
                        if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime) {
                            AttackTick = HUtil32.GetTickCount();
                            byte nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                            LightingAttack(nAttackDir);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

