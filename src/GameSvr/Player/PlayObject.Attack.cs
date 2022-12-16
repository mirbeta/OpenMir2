using GameSvr.Actor;
using GameSvr.Items;
using GameSvr.Magic;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        private bool ClientHitXY(int wIdent, int nX, int nY, byte nDir, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            short n14 = 0;
            short n18 = 0;
            const string sExceptionMsg = "[Exception] TPlayObject::ClientHitXY";
            dwDelayTime = 0;
            try
            {
                if (!MBoCanHit)
                {
                    return false;
                }
                if (Death || StatusArr[PoisonState.STONE] != 0)// 防麻
                {
                    return false;
                }
                if (!M2Share.Config.CloseSpeedHackCheck)
                {
                    if (!boLateDelivery)
                    {
                        if (!CheckActionStatus(wIdent, ref dwDelayTime))
                        {
                            MBoFilterAction = false;
                            return false;
                        }
                        MBoFilterAction = true;
                        int dwAttackTime = HUtil32._MAX(0, M2Share.Config.HitIntervalTime - HitSpeed * M2Share.Config.ItemSpeed);
                        int dwCheckTime = HUtil32.GetTickCount() - MDwAttackTick;
                        if (dwCheckTime < dwAttackTime)
                        {
                            MDwAttackCount++;
                            dwDelayTime = dwAttackTime - dwCheckTime;
                            if (dwDelayTime > M2Share.Config.DropOverSpeed)
                            {
                                if (MDwAttackCount >= 4)
                                {
                                    MDwAttackTick = HUtil32.GetTickCount();
                                    MDwAttackCount = 0;
                                    dwDelayTime = M2Share.Config.DropOverSpeed;
                                    if (MBoTestSpeedMode)
                                    {
                                        SysMsg($"攻击忙!!!{dwDelayTime}", MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    MDwAttackCount = 0;
                                }
                                return false;
                            }
                            if (MBoTestSpeedMode)
                            {
                                SysMsg($"攻击步忙!!!{dwDelayTime}", MsgColor.Red, MsgType.Hint);
                            }
                            return false;
                        }

                    }
                }
                if (nX == CurrX && nY == CurrY)
                {
                    result = true;
                    MDwAttackTick = HUtil32.GetTickCount();
                    if (wIdent == Grobal2.CM_HEAVYHIT && UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].Dura > 0)// 挖矿
                    {
                        if (GetFrontPosition(ref n14, ref n18) && !Envir.CanWalk(n14, n18, false))
                        {
                            var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
                            if (StdItem != null && StdItem.Shape == 19)
                            {
                                if (PileStones(n14, n18))
                                {
                                    SendSocket("=DIG");
                                }
                                HealthTick -= 30;
                                SpellTick -= 50;
                                SpellTick = HUtil32._MAX(0, SpellTick);
                                PerHealth -= 2;
                                PerSpell -= 2;
                                return result;
                            }
                        }
                    }
                    switch (wIdent)
                    {
                        case Grobal2.CM_HIT:
                            AttackDir(null, 0, nDir);
                            break;
                        case Grobal2.CM_HEAVYHIT:
                            AttackDir(null, 1, nDir);
                            break;
                        case Grobal2.CM_BIGHIT:
                            AttackDir(null, 2, nDir);
                            break;
                        case Grobal2.CM_POWERHIT:
                            AttackDir(null, 3, nDir);
                            break;
                        case Grobal2.CM_LONGHIT:
                            AttackDir(null, 4, nDir);
                            break;
                        case Grobal2.CM_WIDEHIT:
                            AttackDir(null, 5, nDir);
                            break;
                        case Grobal2.CM_FIREHIT:
                            AttackDir(null, 7, nDir);
                            break;
                        case Grobal2.CM_CRSHIT:
                            AttackDir(null, 8, nDir);
                            break;
                        case Grobal2.CM_TWINHIT:
                            AttackDir(null, 9, nDir);
                            break;
                        case Grobal2.CM_42HIT:
                            AttackDir(null, 10, nDir);
                            AttackDir(null, 11, nDir);
                            break;
                    }
                    if (MagicArr[MagicConst.SKILL_YEDO] != null && UseItems[Grobal2.U_WEAPON].Dura > 0)
                    {
                        AttackSkillCount -= 1;
                        if (AttackSkillPointCount == AttackSkillCount)
                        {
                            PowerHit = true;
                            SendSocket("+PWR");
                        }
                        if (AttackSkillCount <= 0)
                        {
                            AttackSkillCount = (byte)(7 - MagicArr[MagicConst.SKILL_YEDO].Level);
                            AttackSkillPointCount = M2Share.RandomNumber.RandomByte(AttackSkillCount);
                        }
                    }
                    HealthTick -= 30;
                    SpellTick -= 100;
                    SpellTick = HUtil32._MAX(0, SpellTick);
                    PerHealth -= 2;
                    PerSpell -= 2;
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.StackTrace);
            }
            return result;
        }

        private bool ClientHorseRunXY(int wIdent, short nX, short nY, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            byte n14;
            int dwCheckTime;
            dwDelayTime = 0;
            if (!MBoCanRun)
            {
                return result;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)// 防麻
            {
                return result;
            }
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                if (!boLateDelivery)
                {
                    if (!CheckActionStatus(wIdent, ref dwDelayTime))
                    {
                        MBoFilterAction = false;
                        return result;
                    }
                    MBoFilterAction = true;
                    dwCheckTime = HUtil32.GetTickCount() - MDwMoveTick;
                    if (dwCheckTime < M2Share.Config.RunIntervalTime)
                    {
                        MDwMoveCount++;
                        dwDelayTime = M2Share.Config.RunIntervalTime - dwCheckTime;
                        if (dwDelayTime > M2Share.Config.DropOverSpeed)
                        {
                            if (MDwMoveCount >= 4)
                            {
                                MDwMoveTick = HUtil32.GetTickCount();
                                MDwMoveCount = 0;
                                dwDelayTime = M2Share.Config.DropOverSpeed;
                                if (MBoTestSpeedMode)
                                {
                                    SysMsg("马跑步忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                MDwMoveCount = 0;
                            }
                            return result;
                        }
                        else
                        {
                            if (MBoTestSpeedMode)
                            {
                                SysMsg("马跑步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                            return result;
                        }
                    }
                }
            }
            MDwMoveTick = HUtil32.GetTickCount();
            SpaceMoved = false;
            n14 = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (HorseRunTo(n14, false))
            {
                if (Transparent && HideMode)
                {
                    StatusArr[PoisonState.STATE_TRANSPARENT] = 1;
                }
                if (SpaceMoved || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 60;
                SpellTick -= 10;
                SpellTick = HUtil32._MAX(0, SpellTick);
                PerHealth -= 1;
                PerSpell -= 1;
            }
            else
            {
                MDwMoveCount = 0;
            }
            return result;
        }

        private bool ClientSpellXY(int wIdent, int nKey, short nTargetX, short nTargetY, BaseObject TargeTBaseObject, bool boLateDelivery, ref int dwDelayTime)
        {
            dwDelayTime = 0;
            if (!MBoCanSpell)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)// 防麻
            {
                return false;
            }
            var UserMagic = GetMagicInfo(nKey);
            if (UserMagic == null)
            {
                return false;
            }
            var boIsWarrSkill = M2Share.MagicMgr.IsWarrSkill(UserMagic.MagIdx);
            if (!boLateDelivery && !boIsWarrSkill && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    MBoFilterAction = false;
                    return false;
                }
                MBoFilterAction = true;
                var dwCheckTime = HUtil32.GetTickCount() - MDwMagicAttackTick;
                if (dwCheckTime < MDwMagicAttackInterval)
                {
                    MDwMagicAttackCount++;
                    dwDelayTime = MDwMagicAttackInterval - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.MagicHitIntervalTime / 3)
                    {
                        if (MDwMagicAttackCount >= 4)
                        {
                            MDwMagicAttackTick = HUtil32.GetTickCount();
                            MDwMagicAttackCount = 0;
                            dwDelayTime = M2Share.Config.MagicHitIntervalTime / 3;
                            if (MBoTestSpeedMode)
                            {
                                SysMsg("魔法忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            MDwMagicAttackCount = 0;
                        }
                        return false;
                    }
                    if (MBoTestSpeedMode)
                    {
                        SysMsg("魔法忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                    }
                    return false;
                }
            }
            SpellTick -= 450;
            SpellTick = HUtil32._MAX(0, SpellTick);
            if (!boIsWarrSkill)
            {
                MDwMagicAttackInterval = UserMagic.Magic.DelayTime + M2Share.Config.MagicHitIntervalTime;
            }
            MDwMagicAttackTick = HUtil32.GetTickCount();
            ushort nSpellPoint;
            bool result;
            switch (UserMagic.MagIdx)
            {
                case MagicConst.SKILL_ERGUM:
                    if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                    {
                        if (!UseThrusting)
                        {
                            ThrustingOnOff(true);
                            SendSocket("+LNG");
                        }
                        else
                        {
                            ThrustingOnOff(false);
                            SendSocket("+ULNG");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_BANWOL:
                    if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                    {
                        if (!UseHalfMoon)
                        {
                            HalfMoonOnOff(true);
                            SendSocket("+WID");
                        }
                        else
                        {
                            HalfMoonOnOff(false);
                            SendSocket("+UWID");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_REDBANWOL:
                    if (MagicArr[MagicConst.SKILL_REDBANWOL] != null)
                    {
                        if (!RedUseHalfMoon)
                        {
                            RedHalfMoonOnOff(true);
                            SendSocket("+WID");
                        }
                        else
                        {
                            RedHalfMoonOnOff(false);
                            SendSocket("+UWID");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_FIRESWORD:
                    if (MagicArr[MagicConst.SKILL_FIRESWORD] != null)
                    {
                        if (AllowFireHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (WAbil.MP >= nSpellPoint)
                            {
                                if (nSpellPoint > 0)
                                {
                                    DamageSpell(nSpellPoint);
                                    HealthSpellChanged();
                                }
                                SendSocket("+FIR");
                            }
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - DoMotaeboTick) > 3 * 1000)
                    {
                        DoMotaeboTick = HUtil32.GetTickCount();
                        Direction = (byte)nTargetX;
                        nSpellPoint = GetSpellPoint(UserMagic);
                        if (WAbil.MP >= nSpellPoint)
                        {
                            if (nSpellPoint > 0)
                            {
                                DamageSpell(nSpellPoint);
                                HealthSpellChanged();
                            }
                            if (DoMotaebo(Direction, UserMagic.Level))
                            {
                                if (UserMagic.Level < 3)
                                {
                                    if (UserMagic.Magic.TrainLevel[UserMagic.Level] < Abil.Level)
                                    {
                                        TrainSkill(UserMagic, M2Share.RandomNumber.Random(3) + 1);
                                        if (!CheckMagicLevelup(UserMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.Magic.MagicId, UserMagic.Level, UserMagic.TranPoint, "", 1000);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MagicConst.SKILL_CROSSMOON:
                    if (MagicArr[MagicConst.SKILL_CROSSMOON] != null)
                    {
                        if (!CrsHitkill)
                        {
                            SkillCrsOnOff(true);
                            SendSocket("+CRS");
                        }
                        else
                        {
                            SkillCrsOnOff(false);
                            SendSocket("+UCRS");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_TWINBLADE:// 狂风斩
                    if (MagicArr[MagicConst.SKILL_TWINBLADE] != null)
                    {
                        if (AllowTwinHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (WAbil.MP >= nSpellPoint)
                            {
                                if (nSpellPoint > 0)
                                {
                                    DamageSpell(nSpellPoint);
                                    HealthSpellChanged();
                                }
                                SendSocket("+TWN");
                            }
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_43:// 破空剑
                    if (MagicArr[MagicConst.SKILL_43] != null)
                    {
                        if (!MBo43Kill)
                        {
                            Skill43OnOff(true);
                            SendSocket("+CID");
                        }
                        else
                        {
                            Skill43OnOff(false);
                            SendSocket("+UCID");
                        }
                    }
                    result = true;
                    break;
                default:
                    Direction = M2Share.GetNextDirection(CurrX, CurrY, nTargetX, nTargetY); ;
                    BaseObject BaseObject = null;
                    if (CretInNearXy(TargeTBaseObject, nTargetX, nTargetY)) // 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                    {
                        BaseObject = TargeTBaseObject;
                        nTargetX = BaseObject.CurrX;
                        nTargetY = BaseObject.CurrY;
                    }
                    if (!DoSpell(UserMagic, nTargetX, nTargetY, BaseObject))
                    {
                        SendRefMsg(Grobal2.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        private bool ClientRunXY(int wIdent, short nX, short nY, int nFlag, ref int dwDelayTime)
        {
            bool result = false;
            byte nDir;
            dwDelayTime = 0;
            if (!MBoCanRun)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)
            {
                return false;
            }
            if (nFlag != wIdent && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    MBoFilterAction = false;
                    return false;
                }
                MBoFilterAction = true;
                var dwCheckTime = HUtil32.GetTickCount() - MDwMoveTick;
                if (dwCheckTime < M2Share.Config.RunIntervalTime)
                {
                    MDwMoveCount++;
                    dwDelayTime = M2Share.Config.RunIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.RunIntervalTime / 3)
                    {
                        if (MDwMoveCount >= 4)
                        {
                            MDwMoveTick = HUtil32.GetTickCount();
                            MDwMoveCount = 0;
                            dwDelayTime = M2Share.Config.RunIntervalTime / 3;
                            if (MBoTestSpeedMode)
                            {
                                SysMsg("跑步忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            MDwMoveCount = 0;
                        }
                        return result;
                    }
                    if (MBoTestSpeedMode)
                    {
                        SysMsg("跑步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            MDwMoveTick = HUtil32.GetTickCount();
            SpaceMoved = false;
            nDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (RunTo(nDir, false, nX, nY))
            {
                if (Transparent && HideMode)
                {
                    StatusArr[PoisonState.STATE_TRANSPARENT] = 1;
                }
                if (SpaceMoved || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 60;
                SpellTick -= 10;
                SpellTick = HUtil32._MAX(0, SpellTick);
                PerHealth -= 1;
                PerSpell -= 1;
            }
            else
            {
                MDwMoveCount = 0;
            }
            return result;
        }

        private bool ClientWalkXY(int wIdent, short nX, short nY, bool boLateDelivery, ref int dwDelayTime)
        {
            bool result = false;
            int n14;
            dwDelayTime = 0;
            if (!MBoCanWalk)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)
            {
                return false; // 防麻
            }
            if (!boLateDelivery && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    MBoFilterAction = false;
                    return false;
                }
                MBoFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - MDwMoveTick;
                if (dwCheckTime < M2Share.Config.WalkIntervalTime)
                {
                    MDwMoveCount++;
                    dwDelayTime = M2Share.Config.WalkIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.WalkIntervalTime / 3)
                    {
                        if (MDwMoveCount >= 4)
                        {
                            MDwMoveTick = HUtil32.GetTickCount();
                            MDwMoveCount = 0;
                            dwDelayTime = M2Share.Config.WalkIntervalTime / 3;
                            if (MBoTestSpeedMode)
                            {
                                SysMsg("走路忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            MDwMoveCount = 0;
                        }
                        return false;
                    }
                    if (MBoTestSpeedMode)
                    {
                        SysMsg("走路忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                    }
                    return false;
                }
            }
            MDwMoveTick = HUtil32.GetTickCount();
            SpaceMoved = false;
            n14 = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (WalkTo((byte)n14, false))
            {
                if (SpaceMoved || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 10;
            }
            else
            {
                MDwMoveCount = 0;
            }
            return result;
        }
    }
}