using GameSvr.Actor;
using GameSvr.Items;
using GameSvr.Magic;
using SystemModule;
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
                if (!m_boCanHit)
                {
                    return result;
                }
                if (Death || StatusArr[Grobal2.POISON_STONE] != 0 && !M2Share.Config.ClientConf.boParalyCanHit)// 防麻
                {
                    return result;
                }
                if (!M2Share.Config.CloseSpeedHackCheck)
                {
                    if (!boLateDelivery)
                    {
                        if (!CheckActionStatus(wIdent, ref dwDelayTime))
                        {
                            m_boFilterAction = false;
                            return result;
                        }
                        m_boFilterAction = true;
                        int dwAttackTime = HUtil32._MAX(0, M2Share.Config.HitIntervalTime - HitSpeed * M2Share.Config.ClientConf.btItemSpeed);
                        int dwCheckTime = HUtil32.GetTickCount() - m_dwAttackTick;
                        if (dwCheckTime < dwAttackTime)
                        {
                            m_dwAttackCount++;
                            dwDelayTime = dwAttackTime - dwCheckTime;
                            if (dwDelayTime > M2Share.Config.DropOverSpeed)
                            {
                                if (m_dwAttackCount >= 4)
                                {
                                    m_dwAttackTick = HUtil32.GetTickCount();
                                    m_dwAttackCount = 0;
                                    dwDelayTime = M2Share.Config.DropOverSpeed;
                                    if (m_boTestSpeedMode)
                                    {
                                        SysMsg("攻击攻击忙复位忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    m_dwAttackCount = 0;
                                }
                                return result;
                            }
                            else
                            {
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg("攻击步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                }
                                return result;
                            }
                        }

                    }
                }
                if (nX == CurrX && nY == CurrY)
                {
                    result = true;
                    m_dwAttackTick = HUtil32.GetTickCount();
                    if (wIdent == Grobal2.CM_HEAVYHIT && UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].Dura > 0)// 挖矿
                    {
                        if (GetFrontPosition(ref n14, ref n18) && !Envir.CanWalk(n14, n18, false))
                        {
                            Items.StdItem StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
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
                M2Share.Log.Error(e.Message);
            }
            return result;
        }

        private bool ClientHorseRunXY(short wIdent, int nX, int nY, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            byte n14;
            int dwCheckTime;
            dwDelayTime = 0;
            if (!m_boCanRun)
            {
                return result;
            }
            if (Death || StatusArr[Grobal2.POISON_STONE] != 0 && !M2Share.Config.ClientConf.boParalyCanRun)// 防麻
            {
                return result;
            }
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                if (!boLateDelivery)
                {
                    if (!CheckActionStatus(wIdent, ref dwDelayTime))
                    {
                        m_boFilterAction = false;
                        return result;
                    }
                    m_boFilterAction = true;
                    dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                    if (dwCheckTime < M2Share.Config.RunIntervalTime)
                    {
                        m_dwMoveCount++;
                        dwDelayTime = M2Share.Config.RunIntervalTime - dwCheckTime;
                        if (dwDelayTime > M2Share.Config.DropOverSpeed)
                        {
                            if (m_dwMoveCount >= 4)
                            {
                                m_dwMoveTick = HUtil32.GetTickCount();
                                m_dwMoveCount = 0;
                                dwDelayTime = M2Share.Config.DropOverSpeed;
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg("马跑步忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                m_dwMoveCount = 0;
                            }
                            return result;
                        }
                        else
                        {
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("马跑步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                            return result;
                        }
                    }
                }
            }
            m_dwMoveTick = HUtil32.GetTickCount();
            MBo316 = false;
#if Debug
            Debug.WriteLine(format("当前X:{0} 当前Y:{1} 目标X:{2} 目标Y:{3}", new object[] {this.m_nCurrX, this.m_nCurrY, nX, nY}), TMsgColor.c_Green, TMsgType.t_Hint);
#endif
            n14 = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (HorseRunTo(n14, false))
            {
                if (Transparent && HideMode)
                {
                    StatusArr[Grobal2.STATE_TRANSPARENT] = 1;
                }
                if (MBo316 || CurrX == nX && CurrY == nY)
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
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }

        private bool ClientSpellXY(short wIdent, int nKey, int nTargetX, int nTargetY, BaseObject TargeTBaseObject, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            dwDelayTime = 0;
            if (!m_boCanSpell)
            {
                return result;
            }
            if (Death || StatusArr[Grobal2.POISON_STONE] != 0 && !M2Share.Config.ClientConf.boParalyCanSpell)// 防麻
            {
                return result;
            }
            var UserMagic = GetMagicInfo(nKey);
            if (UserMagic == null)
            {
                return result;
            }
            var boIsWarrSkill = M2Share.MagicMgr.IsWarrSkill(UserMagic.MagIdx);
            if (!boLateDelivery && !boIsWarrSkill && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                var dwCheckTime = HUtil32.GetTickCount() - m_dwMagicAttackTick;
                if (dwCheckTime < m_dwMagicAttackInterval)
                {
                    m_dwMagicAttackCount++;
                    dwDelayTime = m_dwMagicAttackInterval - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.MagicHitIntervalTime / 3)
                    {
                        if (m_dwMagicAttackCount >= 4)
                        {
                            m_dwMagicAttackTick = HUtil32.GetTickCount();
                            m_dwMagicAttackCount = 0;
                            dwDelayTime = M2Share.Config.MagicHitIntervalTime / 3;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("魔法忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            m_dwMagicAttackCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("魔法忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                        }
                        return result;
                    }
                }
            }
            SpellTick -= 450;
            SpellTick = HUtil32._MAX(0, SpellTick);
            if (!boIsWarrSkill)
            {
                m_dwMagicAttackInterval = UserMagic.Magic.DelayTime + M2Share.Config.MagicHitIntervalTime;
            }
            m_dwMagicAttackTick = HUtil32.GetTickCount();
            ushort nSpellPoint;
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
                            if (Abil.MP >= nSpellPoint)
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
                    if ((HUtil32.GetTickCount() - MDwDoMotaeboTick) > 3 * 1000)
                    {
                        MDwDoMotaeboTick = HUtil32.GetTickCount();
                        Direction = (byte)nTargetX;
                        nSpellPoint = GetSpellPoint(UserMagic);
                        if (Abil.MP >= nSpellPoint)
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
                            if (Abil.MP >= nSpellPoint)
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
                    if (CretInNearXY(TargeTBaseObject, nTargetX, nTargetY)) // 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                    {
                        BaseObject = TargeTBaseObject;
                        nTargetX = BaseObject.CurrX;
                        nTargetY = BaseObject.CurrY;
                    }
                    if (!DoSpell(UserMagic, (short)nTargetX, (short)nTargetY, BaseObject))
                    {
                        SendRefMsg(Grobal2.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        private bool ClientRunXY(int wIdent, int nX, int nY, int nFlag, ref int dwDelayTime)
        {
            bool result = false;
            byte nDir;
            dwDelayTime = 0;
            if (!m_boCanRun)
            {
                return false;
            }
            if (Death || StatusArr[Grobal2.POISON_STONE] != 0 && !M2Share.Config.ClientConf.boParalyCanRun)
            {
                return false;
            }
            if (nFlag != wIdent && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return false;
                }
                m_boFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.Config.RunIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.Config.RunIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.RunIntervalTime / 3)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.Config.RunIntervalTime / 3;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("跑步忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            m_dwMoveCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("跑步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                        }
                        return result;
                    }
                }
            }
            m_dwMoveTick = HUtil32.GetTickCount();
            MBo316 = false;
            nDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (RunTo(nDir, false, nX, nY))
            {
                if (Transparent && HideMode)
                {
                    StatusArr[Grobal2.STATE_TRANSPARENT] = 1;
                }
                if (MBo316 || CurrX == nX && CurrY == nY)
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
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }

        private bool ClientWalkXY(int wIdent, int nX, int nY, bool boLateDelivery, ref int dwDelayTime)
        {
            bool result = false;
            int n14;
            int n18;
            int n1C;
            dwDelayTime = 0;
            if (!m_boCanWalk)
            {
                return result;
            }
            if (Death || StatusArr[Grobal2.POISON_STONE] != 0 && !M2Share.Config.ClientConf.boParalyCanWalk)
            {
                return result; // 防麻
            }
            if (!boLateDelivery && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.Config.WalkIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.Config.WalkIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.WalkIntervalTime / 3)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.Config.WalkIntervalTime / 3;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("走路忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            m_dwMoveCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("走路忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                        }
                        return result;
                    }
                }
            }
            m_dwMoveTick = HUtil32.GetTickCount();
            MBo316 = false;
            n18 = CurrX;
            n1C = CurrY;
            n14 = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (!m_boClientFlag)
            {
                if (n14 == 0 && m_nStep == 0)
                {
                    m_nStep++;
                }
                else if (n14 == 4 && m_nStep == 1)
                {
                    m_nStep++;
                }
                else if (n14 == 6 && m_nStep == 2)
                {
                    m_nStep++;
                }
                else if (n14 == 2 && m_nStep == 3)
                {
                    m_nStep++;
                }
                else if (n14 == 1 && m_nStep == 4)
                {
                    m_nStep++;
                }
                else if (n14 == 5 && m_nStep == 5)
                {
                    m_nStep++;
                }
                else if (n14 == 7 && m_nStep == 6)
                {
                    m_nStep++;
                }
                else if (n14 == 3 && m_nStep == 7)
                {
                    m_nStep++;
                }
                else
                {
                    m_nGameGold -= m_nStep;
                    GameGoldChanged();
                    m_nStep = 0;
                }
                if (m_nStep != 0)
                {
                    m_nGameGold++;
                    GameGoldChanged();
                }
            }
            if (WalkTo((byte)n14, false))
            {
                if (MBo316 || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 10;
            }
            else
            {
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }
    }
}