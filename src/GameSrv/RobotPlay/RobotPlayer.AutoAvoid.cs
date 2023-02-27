using GameSrv.Actor;
using SystemModule.Enums;

namespace GameSrv.RobotPlay
{
    public partial class RobotPlayer
    {
        /// <summary>
        /// 是否需要躲避
        /// </summary>
        /// <returns></returns>
        private bool Avoid()
        {
            bool result = false;
            try
            {
                if (HUtil32.GetTickCount() - AutoAvoidTick > 1100 && AutoUseMagic && !Death)
                {
                    if (Job > 0 && (AutoMagicId == 0 || WAbil.HP <= Math.Round(WAbil.MaxHP * 0.15)))// 血低于15%时,必定要躲 
                    {
                        AutoAvoidTick = HUtil32.GetTickCount();
                        if (M2Share.Config.boHeroAttackTarget && Abil.Level < 22) // 22级前道法不躲避
                        {
                            if ((byte)Job == 1)// 法放魔法后要躲
                            {
                                if (CheckTargetXyCount(CurrX, CurrY, 4) > 0)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                        }
                        else
                        {
                            switch (Job)
                            {
                                case PlayJob.Wizard:
                                    if (CheckTargetXyCount(CurrX, CurrY, 4) > 0)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case PlayJob.Taoist:
                                    if (TargetCret != null)
                                    {
                                        if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play) // 22级砍血量的怪
                                        {
                                            if (TargetCret.WAbil.MaxHP >= 700)
                                            {
                                                if (CheckTargetXyCount(CurrX, CurrY, 4) > 0)
                                                {
                                                    result = true;
                                                    return result;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (CheckTargetXyCount(CurrX, CurrY, 4) > 0)
                                            {
                                                result = true;
                                                return result;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (CheckTargetXyCount(CurrX, CurrY, 4) > 0)
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
                M2Share.Logger.Error("RobotPlayObject.IsNeedAvoid");
            }
            return result;
        }

        // 气功波，抗拒火环使用
        // 检测指定方向和范围内坐标的怪物数量
        private int CheckTargetXYCountOfDirection(int nX, int nY, int nDir, int nRange)
        {
            int result = 0;
            BaseObject BaseObject;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.Death)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.HideMode || CoolEye))
                            {
                                switch (nDir)
                                {
                                    case Direction.Up:
                                        if (Math.Abs(nX - BaseObject.CurrX) <= nRange && BaseObject.CurrY - nY >= 0 && BaseObject.CurrY - nY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.UpRight:
                                        if (BaseObject.CurrX - nX >= 0 && BaseObject.CurrX - nX <= nRange && BaseObject.CurrY - nY >= 0 && BaseObject.CurrY - nY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.Right:
                                        if (BaseObject.CurrX - nX >= 0 && BaseObject.CurrX - nX <= nRange && Math.Abs(nY - BaseObject.CurrY) <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.DownRight:
                                        if (BaseObject.CurrX - nX >= 0 && BaseObject.CurrX - nX <= nRange && nY - BaseObject.CurrY >= 0 && nY - BaseObject.CurrY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.Down:
                                        if (Math.Abs(nX - BaseObject.CurrX) <= nRange && nY - BaseObject.CurrY >= 0 && nY - BaseObject.CurrY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.DownLeft:
                                        if (nX - BaseObject.CurrX >= 0 && nX - BaseObject.CurrX <= nRange && nY - BaseObject.CurrY >= 0 && nY - BaseObject.CurrY <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.Left:
                                        if (nX - BaseObject.CurrX >= 0 && nX - BaseObject.CurrX <= nRange && Math.Abs(nY - BaseObject.CurrY) <= nRange)
                                        {
                                            result++;
                                        }
                                        break;
                                    case Direction.UpLeft:
                                        if (nX - BaseObject.CurrX >= 0 && nX - BaseObject.CurrX <= nRange && BaseObject.CurrY - nY >= 0 && BaseObject.CurrY - nY <= nRange)
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
            n10 = TargetCret.CurrX;
            n14 = TargetCret.CurrY;
            int result = Direction.Down;
            if (n10 > CurrX)
            {
                result = Direction.Left;
                if (n14 > CurrY)
                {
                    result = Direction.DownLeft;
                }
                if (n14 < CurrY)
                {
                    result = Direction.UpLeft;
                }
            }
            else
            {
                if (n10 < CurrX)
                {
                    result = Direction.Right;
                    if (n14 > CurrY)
                    {
                        result = Direction.DownRight;
                    }
                    if (n14 < CurrY)
                    {
                        result = Direction.UpRight;
                    }
                }
                else
                {
                    if (n14 > CurrY)
                    {
                        result = Direction.Up;
                    }
                    else if (n14 < CurrY)
                    {
                        result = Direction.Down;
                    }
                }
            }
            return result;
        }

        public byte AutoAvoid_GetDirXY(int nTargetX, int nTargetY)
        {
            int n10 = nTargetX;
            int n14 = nTargetY;
            byte result = Direction.Down;
            if (n10 > CurrX)
            {
                result = Direction.Right;
                if (n14 > CurrY)
                {
                    result = Direction.DownRight;
                }
                if (n14 < CurrY)
                {
                    result = Direction.UpRight;
                }
            }
            else
            {
                if (n10 < CurrX)
                {
                    result = Direction.Left;
                    if (n14 > CurrY)
                    {
                        result = Direction.DownLeft;
                    }
                    if (n14 < CurrY)
                    {
                        result = Direction.UpLeft;
                    }
                }
                else
                {
                    if (n14 > CurrY)
                    {
                        result = Direction.Down;
                    }
                    else if (n14 < CurrY)
                    {
                        result = Direction.Up;
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
                    case Direction.Up:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.UpRight:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.Right:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.DownRight:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.Down:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.DownLeft:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.Left:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
                    case Direction.UpLeft:
                        if (Envir.CanWalk(nTargetX, nTargetY, false) && CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0)
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
            bool result = AutoAvoid_GetGotoXY(MBtLastDirection, ref nTargetX, ref nTargetY);
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
            MBtLastDirection = nDir;
            return result;
        }

        /// <summary>
        /// 是否需要躲避
        /// </summary>
        /// <returns></returns>
        private bool AutoAvoid()
        {
            bool result = true;
            if (TargetCret != null && !TargetCret.Death)
            {
                byte nDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                nDir = GetBackDir(nDir);
                Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, nDir, 5, ref TargetX, ref TargetY);
                result = GotoTargetXy(TargetX, TargetY, 1);
            }
            return result;
        }
    }
}