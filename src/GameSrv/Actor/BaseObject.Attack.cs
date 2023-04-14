using SystemModule.Enums;

namespace GameSrv.Actor
{
    public partial class BaseObject
    {
        protected bool AttackDir(BaseObject attackTarget, int nPower, byte nDir)
        {
            Dir = nDir;
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
        internal int GetBaseAttackPoewr()
        {
            return GetAttackPower(HUtil32.LoByte(WAbil.DC), (sbyte)(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
        }

        internal bool _Attack(int nPower, BaseObject targetObject)
        {
            if (targetObject == null)
            {
                return false;
            }
            var result = false;
            if (IsProperTarget(targetObject))
            {
                if (targetObject.HitPoint > 0)
                {
                    if (HitPoint < M2Share.RandomNumber.RandomByte(targetObject.SpeedPoint))
                    {
                        nPower = 0;
                    }
                }
            }
            else
            {
                nPower = 0;
            }
            if (nPower > 0)
            {
                nPower = targetObject.GetHitStruckDamage(this, nPower);
                if (nPower > 0)
                {
                    targetObject.StruckDamage(nPower);
                    targetObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nPower, targetObject.WAbil.HP, targetObject.WAbil.MaxHP, ActorId, "", 200);
                    result = true;
                }
            }
            if (targetObject.Race > ActorRace.Play)
            {
                targetObject.SendMsg(targetObject, Messages.RM_STRUCK, nPower, targetObject.WAbil.HP, targetObject.WAbil.MaxHP, ActorId);
            }
            return result;
        }

        private bool AttackDirect(BaseObject targetObject, int nSecPwr)
        {
            var result = false;
            if ((Race == ActorRace.Play) || (targetObject.Race == ActorRace.Play) || !(InSafeZone() && targetObject.InSafeZone()))
            {
                if (IsProperTarget(targetObject))
                {
                    if (M2Share.RandomNumber.RandomByte(targetObject.SpeedPoint) < HitPoint)
                    {
                        targetObject.StruckDamage(nSecPwr);
                        targetObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nSecPwr, targetObject.WAbil.HP, targetObject.WAbil.MaxHP, ActorId, "", 500);
                        if (targetObject.Race != ActorRace.Play)
                        {
                            targetObject.SendMsg(targetObject, Messages.RM_STRUCK, nSecPwr, targetObject.WAbil.HP, targetObject.WAbil.MaxHP, ActorId);
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
        internal bool SwordLongAttack(ref int nSecPwr)
        {
            var result = false;
            short nX = 0;
            short nY = 0;
            nSecPwr = HUtil32.Round((nSecPwr * M2Share.Config.SwordLongPowerRate) / 100.0);
            if (Envir.GetNextPosition(CurrX, CurrY, Dir, 2, ref nX, ref nY))
            {
                var baseObject = Envir.GetMovingObject(nX, nY, true);
                if (baseObject != null)
                {
                    if ((nSecPwr > 0) && IsProperTarget(baseObject))
                    {
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
        internal bool SwordWideAttack(ref int nSecPwr)
        {
            var result = false;
            byte nC = 0;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                var nDir = (byte)((Dir + M2Share.Config.WideAttack[nC]) % 8);
                if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nX, ref nY))
                {
                    var targetObject = Envir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (targetObject != null) && IsProperTarget(targetObject))
                    {
                        result = AttackDirect(targetObject, nSecPwr);
                        SetTargetCreat(targetObject);
                    }
                }
                nC++;
                if (nC >= 3)
                {
                    break;
                }
            }
            return result;
        }

        internal bool CrsWideAttack(int nSecPwr)
        {
            var result = false;
            var nC = 0;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                var nDir = (byte)((Dir + M2Share.Config.CrsAttack[nC]) % 8);
                if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nX, ref nY))
                {
                    var targetObject = Envir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (targetObject != null) && IsProperTarget(targetObject))
                    {
                        result = AttackDirect(targetObject, nSecPwr);
                        SetTargetCreat(targetObject);
                    }
                }
                nC++;
                if (nC >= 7)
                {
                    break;
                }
            }
            return result;
        }

        protected void SendAttackMsg(int wIdent, byte btDir, short nX, short nY)
        {
            SendRefMsg(wIdent, btDir, nX, nY, 0, "");
        }
    }
}