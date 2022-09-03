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
                if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanHit)// 防麻
                {
                    return result;
                }
                if (!M2Share.g_Config.boSpeedHackCheck)
                {
                    if (!boLateDelivery)
                    {
                        if (!CheckActionStatus(wIdent, ref dwDelayTime))
                        {
                            m_boFilterAction = false;
                            return result;
                        }
                        m_boFilterAction = true;
                        int dwAttackTime = HUtil32._MAX(0, M2Share.g_Config.dwHitIntervalTime - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed);
                        int dwCheckTime = HUtil32.GetTickCount() - m_dwAttackTick;
                        if (dwCheckTime < dwAttackTime)
                        {
                            m_dwAttackCount++;
                            dwDelayTime = dwAttackTime - dwCheckTime;
                            if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed)
                            {
                                if (m_dwAttackCount >= 4)
                                {
                                    m_dwAttackTick = HUtil32.GetTickCount();
                                    m_dwAttackCount = 0;
                                    dwDelayTime = M2Share.g_Config.dwDropOverSpeed;
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
                if (nX == m_nCurrX && nY == m_nCurrY)
                {
                    result = true;
                    m_dwAttackTick = HUtil32.GetTickCount();
                    if (wIdent == Grobal2.CM_HEAVYHIT && m_UseItems[Grobal2.U_WEAPON] != null && m_UseItems[Grobal2.U_WEAPON].Dura > 0)// 挖矿
                    {
                        if (GetFrontPosition(ref n14, ref n18) && !m_PEnvir.CanWalk(n14, n18, false))
                        {
                            StdItem StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_WEAPON].wIndex);
                            if (StdItem != null && StdItem.Shape == 19)
                            {
                                if (PileStones(n14, n18))
                                {
                                    SendSocket("=DIG");
                                }
                                m_nHealthTick -= 30;
                                m_nSpellTick -= 50;
                                m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                                m_nPerHealth -= 2;
                                m_nPerSpell -= 2;
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
                    if (m_MagicArr[SpellsDef.SKILL_YEDO] != null && m_UseItems[Grobal2.U_WEAPON].Dura > 0)
                    {
                        m_btAttackSkillCount -= 1;
                        if (m_btAttackSkillPointCount == m_btAttackSkillCount)
                        {
                            m_boPowerHit = true;
                            SendSocket("+PWR");
                        }
                        if (m_btAttackSkillCount <= 0)
                        {
                            m_btAttackSkillCount = (byte)(7 - m_MagicArr[SpellsDef.SKILL_YEDO].btLevel);
                            m_btAttackSkillPointCount = M2Share.RandomNumber.RandomByte(m_btAttackSkillCount);
                        }
                    }
                    m_nHealthTick -= 30;
                    m_nSpellTick -= 100;
                    m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                    m_nPerHealth -= 2;
                    m_nPerSpell -= 2;
                }
            }
            catch (Exception e)
            {
                M2Share.LogSystem.Error(sExceptionMsg);
                M2Share.LogSystem.Error(e.Message);
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
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanRun)// 防麻
            {
                return result;
            }
            if (!M2Share.g_Config.boSpeedHackCheck)
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
                    if (dwCheckTime < M2Share.g_Config.dwRunIntervalTime)
                    {
                        m_dwMoveCount++;
                        dwDelayTime = M2Share.g_Config.dwRunIntervalTime - dwCheckTime;
                        if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed)
                        {
                            if (m_dwMoveCount >= 4)
                            {
                                m_dwMoveTick = HUtil32.GetTickCount();
                                m_dwMoveCount = 0;
                                dwDelayTime = M2Share.g_Config.dwDropOverSpeed;
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
            m_bo316 = false;
#if Debug
            Debug.WriteLine(format("当前X:{0} 当前Y:{1} 目标X:{2} 目标Y:{3}", new object[] {this.m_nCurrX, this.m_nCurrY, nX, nY}), TMsgColor.c_Green, TMsgType.t_Hint);
#endif
            n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY);
            if (HorseRunTo(n14, false))
            {
                if (m_boTransparent && m_boHideMode)
                {
                    m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;
                }
                if (m_bo316 || m_nCurrX == nX && m_nCurrY == nY)
                {
                    result = true;
                }
                m_nHealthTick -= 60;
                m_nSpellTick -= 10;
                m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                m_nPerHealth -= 1;
                m_nPerSpell -= 1;
            }
            else
            {
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }

        private bool ClientSpellXY(short wIdent, int nKey, int nTargetX, int nTargetY, TBaseObject TargeTBaseObject, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            dwDelayTime = 0;
            if (!m_boCanSpell)
            {
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanSpell)// 防麻
            {
                return result;
            }
            var UserMagic = GetMagicInfo(nKey);
            if (UserMagic == null)
            {
                return result;
            }
            var boIsWarrSkill = M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx);
            if (!boLateDelivery && !boIsWarrSkill && (!M2Share.g_Config.boSpeedHackCheck))
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
                    if (dwDelayTime > M2Share.g_Config.dwMagicHitIntervalTime / 3)
                    {
                        if (m_dwMagicAttackCount >= 4)
                        {
                            m_dwMagicAttackTick = HUtil32.GetTickCount();
                            m_dwMagicAttackCount = 0;
                            dwDelayTime = M2Share.g_Config.dwMagicHitIntervalTime / 3;
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
            m_nSpellTick -= 450;
            m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
            if (!boIsWarrSkill)
            {
                m_dwMagicAttackInterval = UserMagic.MagicInfo.dwDelayTime + M2Share.g_Config.dwMagicHitIntervalTime;
            }
            m_dwMagicAttackTick = HUtil32.GetTickCount();
            ushort nSpellPoint;
            switch (UserMagic.wMagIdx)
            {
                case SpellsDef.SKILL_ERGUM:
                    if (m_MagicArr[SpellsDef.SKILL_ERGUM] != null)
                    {
                        if (!m_boUseThrusting)
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
                case SpellsDef.SKILL_BANWOL:
                    if (m_MagicArr[SpellsDef.SKILL_BANWOL] != null)
                    {
                        if (!m_boUseHalfMoon)
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
                case SpellsDef.SKILL_REDBANWOL:
                    if (m_MagicArr[SpellsDef.SKILL_REDBANWOL] != null)
                    {
                        if (!m_boRedUseHalfMoon)
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
                case SpellsDef.SKILL_FIRESWORD:
                    if (m_MagicArr[SpellsDef.SKILL_FIRESWORD] != null)
                    {
                        if (AllowFireHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (m_WAbil.MP >= nSpellPoint)
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
                case SpellsDef.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - m_dwDoMotaeboTick) > 3 * 1000)
                    {
                        m_dwDoMotaeboTick = HUtil32.GetTickCount();
                        Direction = (byte)nTargetX;
                        nSpellPoint = GetSpellPoint(UserMagic);
                        if (m_WAbil.MP >= nSpellPoint)
                        {
                            if (nSpellPoint > 0)
                            {
                                DamageSpell(nSpellPoint);
                                HealthSpellChanged();
                            }
                            if (DoMotaebo(Direction, UserMagic.btLevel))
                            {
                                if (UserMagic.btLevel < 3)
                                {
                                    if (UserMagic.MagicInfo.TrainLevel[UserMagic.btLevel] < m_Abil.Level)
                                    {
                                        TrainSkill(UserMagic, M2Share.RandomNumber.Random(3) + 1);
                                        if (!CheckMagicLevelup(UserMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "", 1000);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case SpellsDef.SKILL_CROSSMOON:
                    if (m_MagicArr[SpellsDef.SKILL_CROSSMOON] != null)
                    {
                        if (!m_boCrsHitkill)
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
                case SpellsDef.SKILL_TWINBLADE:// 狂风斩
                    if (m_MagicArr[SpellsDef.SKILL_TWINBLADE] != null)
                    {
                        if (AllowTwinHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (m_WAbil.MP >= nSpellPoint)
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
                case SpellsDef.SKILL_43:// 破空剑
                    if (m_MagicArr[SpellsDef.SKILL_43] != null)
                    {
                        if (!m_bo43kill)
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
                    Direction = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nTargetX, nTargetY); ;
                    TBaseObject BaseObject = null;
                    if (CretInNearXY(TargeTBaseObject, nTargetX, nTargetY)) // 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                    {
                        BaseObject = TargeTBaseObject;
                        nTargetX = BaseObject.m_nCurrX;
                        nTargetY = BaseObject.m_nCurrY;
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
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanRun)
            {
                return result;
            }
            if (nFlag != wIdent && (!M2Share.g_Config.boSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.g_Config.dwRunIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.g_Config.dwRunIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.g_Config.dwRunIntervalTime / 3)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.g_Config.dwRunIntervalTime / 3;
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
            m_bo316 = false;
            nDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY);
            if (RunTo(nDir, false, nX, nY))
            {
                if (m_boTransparent && m_boHideMode)
                {
                    m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;
                }
                if (m_bo316 || m_nCurrX == nX && m_nCurrY == nY)
                {
                    result = true;
                }
                m_nHealthTick -= 60;
                m_nSpellTick -= 10;
                m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                m_nPerHealth -= 1;
                m_nPerSpell -= 1;
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
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanWalk)
            {
                return result; // 防麻
            }
            if (!boLateDelivery && (!M2Share.g_Config.boSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.g_Config.dwWalkIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.g_Config.dwWalkIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.g_Config.dwWalkIntervalTime / 3)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.g_Config.dwWalkIntervalTime / 3;
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
            m_bo316 = false;
            n18 = m_nCurrX;
            n1C = m_nCurrY;
            n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY);
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
                if (m_bo316 || m_nCurrX == nX && m_nCurrY == nY)
                {
                    result = true;
                }
                m_nHealthTick -= 10;
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