using SystemModule.Enums;

namespace GameSrv.Actor {
    public partial class BaseObject {
        protected bool AttackDir(BaseObject attackTarget, ushort nPower, byte nDir)
        {
            Direction = nDir;
            if (_Attack(nPower, attackTarget))
            {
                SetTargetCreat(attackTarget);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 基础攻击力
        /// </summary>
        /// <returns></returns>
        internal ushort GetBaseAttackPoewr()
        {
            return GetAttackPower(HUtil32.LoByte(WAbil.DC), (sbyte)(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
        }

        internal bool _Attack(ushort nPower, BaseObject attackTarget) {
            if (attackTarget == null) {
                return false;
            }
            bool result = false;
            if (IsProperTarget(attackTarget)) {
                if (attackTarget.HitPoint > 0) {
                    if (HitPoint < M2Share.RandomNumber.RandomByte(attackTarget.SpeedPoint)) {
                        nPower = 0;
                    }
                }
            }
            else {
                nPower = 0;
            }
            if (nPower > 0) {
                nPower = attackTarget.GetHitStruckDamage(this, nPower);
                if (nPower > 0) {
                    attackTarget.StruckDamage(nPower);
                    attackTarget.SendDelayMsg(Messages.RM_STRUCK, Messages.RM_REFMESSAGE, nPower, attackTarget.WAbil.HP, attackTarget.WAbil.MaxHP, ActorId, "", 200);
                    result = true;
                }
            }
            if (attackTarget.Race > ActorRace.Play) {
                attackTarget.SendMsg(attackTarget, Messages.RM_STRUCK, nPower, attackTarget.WAbil.HP, attackTarget.WAbil.MaxHP, ActorId, "");
            }
            return result;
        }

        private bool AttackDirect(BaseObject BaseObject, int nSecPwr) {
            bool result = false;
            if ((Race == ActorRace.Play) || (BaseObject.Race == ActorRace.Play) || !(InSafeZone() && BaseObject.InSafeZone())) {
                if (IsProperTarget(BaseObject)) {
                    if (M2Share.RandomNumber.RandomByte(BaseObject.SpeedPoint) < HitPoint) {
                        BaseObject.StruckDamage((ushort)nSecPwr);
                        BaseObject.SendDelayMsg(Messages.RM_STRUCK, Messages.RM_REFMESSAGE, nSecPwr, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, ActorId, "", 500);
                        if (BaseObject.Race != ActorRace.Play) {
                            BaseObject.SendMsg(BaseObject, Messages.RM_STRUCK, nSecPwr, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, ActorId, "");
                        }
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 刺杀前面一个位置的攻击
        /// </summary>
        internal bool SwordLongAttack(ref int nSecPwr) {
            bool result = false;
            short nX = 0;
            short nY = 0;
            nSecPwr = HUtil32.Round((nSecPwr * M2Share.Config.SwordLongPowerRate) / 100.0);
            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref nX, ref nY)) {
                BaseObject baseObject = Envir.GetMovingObject(nX, nY, true);
                if (baseObject != null) {
                    if ((nSecPwr > 0) && IsProperTarget(baseObject)) {
                        AttackDirect(baseObject, nSecPwr);
                        SetTargetCreat(baseObject);
                    }
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 半月攻击
        /// </summary>
        /// <returns></returns>
        internal bool SwordWideAttack(ref int nSecPwr) {
            bool result = false;
            byte nC = 0;
            short nX = 0;
            short nY = 0;
            while (true) {
                byte nDir = (byte)((Direction + M2Share.Config.WideAttack[nC]) % 8);
                if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nX, ref nY)) {
                    BaseObject BaseObject = Envir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (BaseObject != null) && IsProperTarget(BaseObject)) {
                        result = AttackDirect(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                }
                nC++;
                if (nC >= 3) {
                    break;
                }
            }
            return result;
        }

        internal bool CrsWideAttack(int nSecPwr) {
            bool result = false;
            int nC = 0;
            short nX = 0;
            short nY = 0;
            while (true) {
                byte nDir = (byte)((Direction + M2Share.Config.CrsAttack[nC]) % 8);
                if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nX, ref nY)) {
                    BaseObject BaseObject = Envir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (BaseObject != null) && IsProperTarget(BaseObject)) {
                        result = AttackDirect(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                }
                nC++;
                if (nC >= 7) {
                    break;
                }
            }
            return result;
        }

        protected void SendAttackMsg(int wIdent, byte btDir, short nX, short nY) {
            SendRefMsg(wIdent, btDir, nX, nY, 0, "");
        }
    }
}
