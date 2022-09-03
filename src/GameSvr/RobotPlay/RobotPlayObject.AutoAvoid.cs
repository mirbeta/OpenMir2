using GameSvr.Actor;
using SystemModule;

namespace GameSvr.RobotPlay
{
    public partial class RobotPlayObject
    {

        /// <summary>
        /// 是否需要躲避
        /// </summary>
        /// <returns></returns>
        private bool IsNeedAvoid()
        {
            bool result = false;
            try
            {
                if (HUtil32.GetTickCount() - m_dwAutoAvoidTick > 1100 && m_boIsUseMagic && !m_boDeath)
                {
                    // 血低于15%时,必定要躲 20080711
                    if (m_btJob > 0 && (m_nSelectMagic == 0 || m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.15)))
                    {
                        m_dwAutoAvoidTick = HUtil32.GetTickCount();
                        if (M2Share.g_Config.boHeroAttackTarget && m_Abil.Level < 22) // 22级前道法不躲避
                        {
                            if ((byte)m_btJob == 1)// 法放魔法后要躲
                            {
                                if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                        }
                        else
                        {
                            switch (m_btJob)
                            {
                                case PlayJob.Wizard:
                                    if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case PlayJob.Taoist:
                                    if (m_TargetCret != null)
                                    {
                                        if (M2Share.g_Config.boHeroAttackTao && m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT) // 22级砍血量的怪
                                        {
                                            if (m_TargetCret.m_WAbil.MaxHP >= 700)
                                            {
                                                if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                                {
                                                    result = true;
                                                    return result;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                            {
                                                result = true;
                                                return result;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.LogSystem.Error("TAIPlayObject.IsNeedAvoid");
            }
            return result;
        }

        // 气功波，抗拒火环使用
        // 检测指定方向和范围内坐标的怪物数量
        private int CheckTargetXYCountOfDirection(int nX, int nY, int nDir, int nRange)
        {
            int result = 0;
            TBaseObject BaseObject;
            if (m_VisibleActors.Count > 0)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.m_boDeath)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || m_boCoolEye))
                            {
                                switch (nDir)
                                {
                                    case Grobal2.DR_UP:
                                        if (Math.Abs(nX - BaseObject.m_nCurrX) <= nRange && BaseObject.m_nCurrY - nY >= 0 && BaseObject.m_nCurrY - nY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_UPRIGHT:
                                        if (BaseObject.m_nCurrX - nX >= 0 && BaseObject.m_nCurrX - nX <= nRange && BaseObject.m_nCurrY - nY >= 0 && BaseObject.m_nCurrY - nY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_RIGHT:
                                        if (BaseObject.m_nCurrX - nX >= 0 && BaseObject.m_nCurrX - nX <= nRange && Math.Abs(nY - BaseObject.m_nCurrY) <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_DOWNRIGHT:
                                        if (BaseObject.m_nCurrX - nX >= 0 && BaseObject.m_nCurrX - nX <= nRange && nY - BaseObject.m_nCurrY >= 0 && nY - BaseObject.m_nCurrY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_DOWN:
                                        if (Math.Abs(nX - BaseObject.m_nCurrX) <= nRange && nY - BaseObject.m_nCurrY >= 0 && nY - BaseObject.m_nCurrY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_DOWNLEFT:
                                        if (nX - BaseObject.m_nCurrX >= 0 && nX - BaseObject.m_nCurrX <= nRange && nY - BaseObject.m_nCurrY >= 0 && nY - BaseObject.m_nCurrY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_LEFT:
                                        if (nX - BaseObject.m_nCurrX >= 0 && nX - BaseObject.m_nCurrX <= nRange && Math.Abs(nY - BaseObject.m_nCurrY) <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Grobal2.DR_UPLEFT:
                                        if (nX - BaseObject.m_nCurrX >= 0 && nX - BaseObject.m_nCurrX <= nRange && BaseObject.m_nCurrY - nY >= 0 && BaseObject.m_nCurrY - nY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 自动躲避
        /// </summary>
        /// <returns></returns>
        public int AutoAvoid_GetAvoidDir()
        {
            int n10;
            int n14;
            n10 = m_TargetCret.m_nCurrX;
            n14 = m_TargetCret.m_nCurrY;
            int result = Grobal2.DR_DOWN;
            if (n10 > m_nCurrX)
            {
                result = Grobal2.DR_LEFT;
                if (n14 > m_nCurrY)
                {
                    result = Grobal2.DR_DOWNLEFT;
                }
                if (n14 < m_nCurrY)
                {
                    result = Grobal2.DR_UPLEFT;
                }
            }
            else
            {
                if (n10 < m_nCurrX)
                {
                    result = Grobal2.DR_RIGHT;
                    if (n14 > m_nCurrY)
                    {
                        result = Grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < m_nCurrY)
                    {
                        result = Grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n14 > m_nCurrY)
                    {
                        result = Grobal2.DR_UP;
                    }
                    else if (n14 < m_nCurrY)
                    {
                        result = Grobal2.DR_DOWN;
                    }
                }
            }
            return result;
        }

        public byte AutoAvoid_GetDirXY(int nTargetX, int nTargetY)
        {
            int n10 = nTargetX;
            int n14 = nTargetY;
            byte result = Grobal2.DR_DOWN;
            if (n10 > m_nCurrX)
            {
                result = Grobal2.DR_RIGHT;
                if (n14 > m_nCurrY)
                {
                    result = Grobal2.DR_DOWNRIGHT;
                }
                if (n14 < m_nCurrY)
                {
                    result = Grobal2.DR_UPRIGHT;
                }
            }
            else
            {
                if (n10 < m_nCurrX)
                {
                    result = Grobal2.DR_LEFT;
                    if (n14 > m_nCurrY)
                    {
                        result = Grobal2.DR_DOWNLEFT;
                    }
                    if (n14 < m_nCurrY)
                    {
                        result = Grobal2.DR_UPLEFT;
                    }
                }
                else
                {
                    if (n14 > m_nCurrY)
                    {
                        result = Grobal2.DR_DOWN;
                    }
                    else if (n14 < m_nCurrY)
                    {
                        result = Grobal2.DR_UP;
                    }
                }
            }
            return result;
        }

        public bool AutoAvoid_GetGotoXY(byte nDir, ref short nTargetX, ref short nTargetY)
        {
            int n01 = 0;
            while (true)
            {
                switch (nDir)
                {
                    case Grobal2.DR_UP:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetY -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetY -= 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_UPRIGHT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetX += 2;
                            nTargetY -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX += 2;
                            nTargetY -= 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_RIGHT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetX += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX += 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_DOWNRIGHT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetX += 2;
                            nTargetY += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX += 2;
                            nTargetY += 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_DOWN:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetY += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetY += 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_DOWNLEFT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetX -= 2;
                            nTargetY += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX -= 2;
                            nTargetY += 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_LEFT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetX -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX -= 2;
                            n01 += 2;
                            continue;
                        }
                    case Grobal2.DR_UPLEFT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
                        {
                            nTargetX -= 2;
                            nTargetY -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX -= 2;
                            nTargetY -= 2;
                            n01 += 2;
                            continue;
                        }
                    default:
                        break;
                }
            }
        }

        public bool AutoAvoid_GetAvoidXY(ref short nTargetX, ref short nTargetY)
        {
            int n10;
            byte nDir = 0;
            short nX = nTargetX;
            short nY = nTargetY;
            bool result = AutoAvoid_GetGotoXY(m_btLastDirection, ref nTargetX, ref nTargetY);
            n10 = 0;
            while (true)
            {
                if (n10 >= 7)
                {
                    break;
                }
                if (result)
                {
                    break;
                }
                nTargetX = nX;
                nTargetY = nY;
                nDir = M2Share.RandomNumber.RandomByte(7);
                result = AutoAvoid_GetGotoXY(nDir, ref nTargetX, ref nTargetY);
                n10++;
            }
            m_btLastDirection = nDir;
            return result;
        }

        /// <summary>
        /// 是否需要躲避
        /// </summary>
        /// <returns></returns>
        private bool AutoAvoid()
        {
            bool result = true;
            if (m_TargetCret != null && !m_TargetCret.m_boDeath)
            {
                byte nDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                nDir = GetBackDir(nDir);
                m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, nDir, 5, ref m_nTargetX, ref m_nTargetY);
                result = GotoTargetXY(m_nTargetX, m_nTargetY, 1);
            }
            return result;
        }
    }
}