using GameSrv.Magic;
using System.Collections;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.RobotPlay
{
    public partial class RobotPlayObject
    {
        public MapWalkXY FindGoodPathA(MapWalkXY[] WalkStep, int nRange, int nType)
        {
            MapWalkXY result = null;
            int n10 = int.MaxValue;
            int nMastrRange;
            int nMonCount;
            MapWalkXY MapWalkXY = null;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (WalkStep[i].WalkStep > 0 && Math.Abs(WalkStep[i].X - TargetCret.CurrX) >= nRange && Math.Abs(WalkStep[i].Y - TargetCret.CurrY) >= nRange)
                {
                    if (WalkStep[i].MonCount < n10)
                    {
                        n10 = WalkStep[i].MonCount;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null && Master != null)
            {
                nMonCount = MapWalkXY.MonCount;
                nMastrRange = MapWalkXY.MastrRange;
                n10 = int.MaxValue;
                MapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
                {
                    if (WalkStep[i].WalkStep > 0 && WalkStep[i].MonCount <= nMonCount && Math.Abs(WalkStep[i].X - TargetCret.CurrX) >= nRange && Math.Abs(WalkStep[i].Y - TargetCret.CurrY) >= nRange)
                    {
                        if (WalkStep[i].MastrRange < n10 && WalkStep[i].MastrRange < nMastrRange)
                        {
                            n10 = WalkStep[i].MastrRange;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null)
            {
                result = MapWalkXY;
            }
            return result;
        }

        public MapWalkXY FindGoodPathB(MapWalkXY[] WalkStep, int nType)
        {
            MapWalkXY result = null;
            int nMastrRange;
            int nMonCount;
            MapWalkXY MapWalkXY = null;
            int n10 = int.MaxValue;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (WalkStep[i].WalkStep > 0)
                {
                    if (WalkStep[i].MonCount < n10)
                    {
                        n10 = WalkStep[i].MonCount;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null && Master != null)
            {
                nMonCount = MapWalkXY.MonCount;
                nMastrRange = MapWalkXY.MastrRange;
                n10 = int.MaxValue;
                MapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
                {
                    if (WalkStep[i].WalkStep > 0 && WalkStep[i].MonCount <= nMonCount)
                    {
                        if (WalkStep[i].MastrRange < n10 && WalkStep[i].MastrRange < nMastrRange)
                        {
                            n10 = WalkStep[i].MastrRange;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null)
            {
                result = MapWalkXY;
            }
            return result;
        }

        public MapWalkXY FindMinRange(MapWalkXY[] WalkStep)
        {
            MapWalkXY result = null;
            int n10 = int.MaxValue;
            int n1C;
            int nMonCount;
            MapWalkXY MapWalkXY = null;
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (WalkStep[i].WalkStep > 0)
                {
                    n1C = Math.Abs(WalkStep[i].X - TargetCret.CurrX) + Math.Abs(WalkStep[i].Y - TargetCret.CurrY);
                    if (n1C < n10)
                    {
                        n10 = n1C;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null)
            {
                nMonCount = MapWalkXY.MonCount;
                MapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
                {
                    if (WalkStep[i].WalkStep > 0 && WalkStep[i].MonCount <= nMonCount)
                    {
                        n1C = Math.Abs(WalkStep[i].X - TargetCret.CurrX) + Math.Abs(WalkStep[i].Y - TargetCret.CurrY);
                        if (n1C <= n10)
                        {
                            n10 = n1C;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null)
            {
                result = MapWalkXY;
            }
            return result;
        }

        /// <summary>
        /// 检测下一步在不在攻击位
        /// </summary>
        /// <returns></returns>
        public bool CanWalkNextPosition(short nX, short nY, int nRange, byte btDir, bool boFlag)
        {
            short nCurrX = 0;
            short nCurrY = 0;
            if (Envir.GetNextPosition(nX, nY, btDir, 1, ref nCurrX, ref nCurrY) && CanMove(nX, nY, nCurrX, nCurrY, false) && !boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir))
            {
                return true;
            }
            if (Envir.GetNextPosition(nX, nY, btDir, 2, ref nCurrX, ref nCurrY) && CanMove(nX, nY, nCurrX, nCurrY, false) && !boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir))
            {
                return true;
            }
            return false;
        }

        public bool FindPosOfSelf(MapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPosOfSelf(MapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            bool result = false;
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir) || CanWalkNextPosition(nCurrX, nCurrY, nRange, i, boFlag))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool FindPosOfTarget(MapWalkXY[] WalkStep, short nTargetX, short nTargetY, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(nTargetX, nTargetY, i, nRange, ref nCurrX, ref nCurrY) && Envir.CanWalkEx(nCurrX, nCurrY, false))
                {
                    if ((!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) && IsGotoXy(CurrX, CurrY, nCurrX, nCurrY))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        WalkStep[i].MonCount = GetRangeTargetCount(nCurrX, nCurrY, 2);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool FindPos(MapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 2, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            if (result)
            {
                return result;
            }
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 1, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPos(MapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 1, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir) || CanWalkNextPosition(nCurrX, nCurrY, nRange, i, boFlag))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            if (result)
            {
                return result;
            }
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 2, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir) || CanWalkNextPosition(nCurrX, nCurrY, nRange, i, boFlag))
                    {
                        WalkStep[i].WalkStep = nRange;
                        WalkStep[i].X = nCurrX;
                        WalkStep[i].Y = nCurrY;
                        WalkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool WalkToRightPos(int wMagicID)
        {
            bool boFlag;
            int nRange;
            MapWalkXY[] WalkStep = null;
            MapWalkXY MapWalkXY;
            bool result = false;
            try
            {
                boFlag = Race == 108 || new ArrayList(new int[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID) || Job == 0;
                if (Job == 0 || wMagicID <= 0)
                {
                    nRange = 1;
                    if (wMagicID == 43)
                    {
                        nRange = 4;
                    }
                    if (wMagicID == 12)
                    {
                        nRange = 2;
                    }
                    if (new ArrayList(new[] { 60, 61, 62 }).Contains(wMagicID))
                    {
                        nRange = 6;
                    }
                    for (int i = nRange; i >= 1; i--)
                    {
                        if (FindPosOfTarget(WalkStep, TargetCret.CurrX, TargetCret.CurrY, i, boFlag))
                        {
                            MapWalkXY = FindGoodPathB(WalkStep, 0);
                            if (MapWalkXY.WalkStep > 0)
                            {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (int i = 2; i >= 1; i--)
                    {
                        if (FindPosOfSelf(WalkStep, i, boFlag))
                        {
                            if (Master != null)
                            {
                                MapWalkXY = FindGoodPathB(WalkStep, 1);
                            }
                            else
                            {
                                MapWalkXY = FindGoodPathB(WalkStep, 0);
                            }
                            if (MapWalkXY.WalkStep > 0)
                            {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (wMagicID > 0)
                    {
                        nRange = HUtil32._MAX(M2Share.RandomNumber.Random(3), 2);
                    }
                    else
                    {
                        nRange = 1;
                    }
                    boFlag = Race == 108 || new ArrayList(new int[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID) || nRange == 1;
                    for (int i = 2; i >= 1; i--)
                    {
                        if (FindPosOfSelf(WalkStep, i, boFlag))
                        {
                            MapWalkXY = FindGoodPathA(WalkStep, nRange, 0);
                            if (MapWalkXY.WalkStep > 0)
                            {
                                if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (int i = 2; i >= 1; i--)
                    {
                        if (ActThink__FindPosOfSelf(WalkStep, i, boFlag))
                        {
                            MapWalkXY = FindMinRange(WalkStep);
                            if (MapWalkXY.WalkStep > 0)
                            {
                                if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (int i = nRange; i >= 1; i--)
                    {
                        if (FindPosOfTarget(WalkStep, TargetCret.CurrX, TargetCret.CurrY, i, boFlag))
                        {
                            MapWalkXY = FindGoodPathB(WalkStep, 0);
                            if (MapWalkXY.WalkStep > 0)
                            {
                                if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("WalkToRightPos:" + ChrName);
            }
            return result;
        }

        public bool AvoidTarget(short wMagicID)
        {
            short nX = 0;
            short nY = 0;
            bool boFlag;
            MapWalkXY[] WalkStep = null;
            bool result = false;
            int nRange = HUtil32._MAX(M2Share.RandomNumber.Random(3), 2);
            boFlag = Race == 108 || new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID);
            byte btDir;
            MapWalkXY MapWalkXY;
            for (int i = nRange; i >= 1; i--)
            {
                if (FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = FindGoodPathB(WalkStep, 0);
                    if (MapWalkXY.WalkStep > 0)
                    {
                        btDir = M2Share.GetNextDirection(CurrX, CurrY, MapWalkXY.X, MapWalkXY.Y);
                        if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                        {
                            if (Race != 108)
                            {
                                for (int j = nRange; j >= 1; j--)
                                {
                                    // 再跑1次
                                    if (Envir.GetNextPosition(MapWalkXY.X, MapWalkXY.Y, btDir, j, ref nX, ref nY) && Envir.CanWalkEx(nX, nY, true) && GetNearTargetCount(nX, nY) <= MapWalkXY.MonCount)
                                    {
                                        GotoNextOne(nX, nY, Race != 108);
                                        break;
                                    }
                                }
                            }
                            MRunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            for (int i = nRange; i >= 1; i--)
            {
                if (ActThink__FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = FindGoodPathB(WalkStep, 0);
                    if (MapWalkXY.WalkStep > 0)
                    {
                        btDir = M2Share.GetNextDirection(CurrX, CurrY, MapWalkXY.X, MapWalkXY.Y);
                        if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                        {
                            for (int j = nRange; j >= 1; j--)
                            {
                                // 再跑1次
                                if (Envir.GetNextPosition(MapWalkXY.X, MapWalkXY.Y, btDir, j, ref nX, ref nY) && Envir.CanWalkEx(nX, nY, true) && GetNearTargetCount(nX, nY) <= MapWalkXY.MonCount)
                                {
                                    MapWalkXY.X = nX;
                                    MapWalkXY.Y = nY;
                                    GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108);
                                    break;
                                }
                            }
                            MRunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public bool FollowTarget(short wMagicID)
        {
            int nRange = 2;
            MapWalkXY[] WalkStep = null;
            MapWalkXY MapWalkXY;
            bool result = false;
            bool boFlag = Race == 108 || new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID);
            for (int i = nRange; i >= 1; i--)
            {
                if (FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = FindMinRange(WalkStep);
                    if (MapWalkXY.WalkStep > 0)
                    {
                        if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                        {
                            MRunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            for (int i = nRange; i >= 1; i--)
            {
                if (ActThink__FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = FindMinRange(WalkStep);
                    if (MapWalkXY.WalkStep > 0)
                    {
                        if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                        {
                            MRunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public bool MotaeboPos(short wMagicID)
        {
            short nTargetX = 0;
            short nTargetY = 0;
            byte btNewDir;
            if (TargetCret == null || Master == null)
            {
                return false;
            }
            if (GetPoseCreate() == TargetCret || TargetCret.GetPoseCreate() == this)
            {
                btNewDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, btNewDir, 1, ref nTargetX, ref nTargetY))
                {
                    if (Envir.CanWalk(nTargetX, nTargetY, true))
                    {
                        return true;
                    }
                }
            }
            return WalkToRightPos(wMagicID);
        }

        public MapWalkXY FindPosOfDir(byte nDir, int nRange, bool boFlag)
        {
            MapWalkXY result = null;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, nDir, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false) && (boFlag && CanLineAttack(nCurrX, nCurrY) || !boFlag) && IsGotoXy(CurrX, CurrY, nCurrX, nCurrY))
            {
                result = new MapWalkXY();
                result.WalkStep = nRange;
                result.X = nCurrX;
                result.Y = nCurrY;
                result.MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                result.MonCount = GetNearTargetCount(nCurrX, nCurrY);
                result.MastrRange = GetMasterRange(nCurrX, nCurrY);
            }
            return result;
        }

        public static byte RunPosAttackGetNextRunPos(byte btDir, bool boTurn)
        {
            byte result = 0;
            if (boTurn)
            {
                switch (btDir)
                {
                    case Direction.Up:
                        result = Direction.Right;
                        break;
                    case Direction.UpRight:
                        result = Direction.DownRight;
                        break;
                    case Direction.Right:
                        result = Direction.Down;
                        break;
                    case Direction.DownRight:
                        result = Direction.DownLeft;
                        break;
                    case Direction.Down:
                        result = Direction.Left;
                        break;
                    case Direction.DownLeft:
                        result = Direction.UpLeft;
                        break;
                    case Direction.Left:
                        result = Direction.Up;
                        break;
                    case Direction.UpLeft:
                        result = Direction.UpRight;
                        break;
                }
            }
            else
            {
                switch (btDir)
                {
                    case Direction.Up:
                        result = Direction.Left;
                        break;
                    case Direction.UpRight:
                        result = Direction.UpLeft;
                        break;
                    case Direction.Right:
                        result = Direction.Up;
                        break;
                    case Direction.DownRight:
                        result = Direction.UpRight;
                        break;
                    case Direction.Down:
                        result = Direction.Right;
                        break;
                    case Direction.DownLeft:
                        result = Direction.DownRight;
                        break;
                    case Direction.Left:
                        result = Direction.Down;
                        break;
                    case Direction.UpLeft:
                        result = Direction.DownLeft;
                        break;
                }
            }
            return result;
        }

        public bool RunPosAttack(int wMagicID)
        {
            MapWalkXY[] WalkStep = new MapWalkXY[2];
            MapWalkXY MapWalkXY;
            int nRange;
            bool boFlag;
            int nNearTargetCount;
            bool result = false;
            byte btDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
            byte btNewDir1 = RunPosAttackGetNextRunPos(btDir, true);
            byte btNewDir2 = RunPosAttackGetNextRunPos(btDir, false);
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 2, 0);
            if (Job == 0)
            {
                nRange = 1;
                if (wMagicID == 43)
                {
                    nRange = 2;
                }
                if (wMagicID == 12)
                {
                    nRange = 2;
                }
                if (new ArrayList(new[] { 60, 61, 62 }).Contains(wMagicID))
                {
                    nRange = 6;
                }
                WalkStep[0] = FindPosOfDir(btNewDir1, nRange, true);
                WalkStep[1] = FindPosOfDir(btNewDir2, nRange, true);
            }
            else
            {
                nRange = 2;
                boFlag = false;
                WalkStep[0] = FindPosOfDir(btNewDir1, nRange, boFlag);
                WalkStep[1] = FindPosOfDir(btNewDir2, nRange, boFlag);
            }
            nNearTargetCount = GetNearTargetCount(CurrX, CurrY);
            MapWalkXY = null;
            if (WalkStep[0].WalkStep > 0 && WalkStep[1].WalkStep > 0)
            {
                if (MRunPos.btDirection > 0)
                {
                    MapWalkXY = WalkStep[1];
                }
                else
                {
                    MapWalkXY = WalkStep[0];
                }
                if (nNearTargetCount < WalkStep[0].MonCount && nNearTargetCount < WalkStep[1].MonCount)
                {
                    MapWalkXY = null;
                }
                else if (MRunPos.btDirection > 0 && nNearTargetCount < WalkStep[1].MonCount)
                {
                    MapWalkXY = null;
                }
                else if (MRunPos.btDirection <= 0 && nNearTargetCount < WalkStep[0].MonCount)
                {
                    MapWalkXY = null;
                }
                if (nNearTargetCount > 0 && MapWalkXY != null && MapWalkXY.MonCount > nNearTargetCount)
                {
                    MapWalkXY = null;
                }
            }
            else if (WalkStep[0].WalkStep > 0)
            {
                MapWalkXY = WalkStep[0];
                if (nNearTargetCount < WalkStep[0].MonCount)
                {
                    MapWalkXY = null;
                }
                MRunPos.btDirection = 0;
            }
            else if (WalkStep[1].WalkStep > 0)
            {
                MapWalkXY = WalkStep[1];
                if (nNearTargetCount < WalkStep[1].MonCount)
                {
                    MapWalkXY = null;
                }
                MRunPos.btDirection = 1;
            }
            if (MapWalkXY != null)
            {
                if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                {
                    result = true;
                }
            }
            if (!result)
            {
                MRunPos.AttackCount = 0;
            }
            return result;
        }

        private bool ActThink(short wMagicID)
        {
            bool result = false;
            int nCode = 0;
            int nThinkCount = 0;
            try
            {
                while (true)
                {
                    if (TargetCret == null || wMagicID > 255)
                    {
                        break;
                    }
                    nThinkCount = nThinkCount + 1;
                    nCode = DoThink(wMagicID);
                    switch (Job)
                    {
                        case 0:
                            switch (nCode)
                            {
                                case 2:
                                    if (WalkToRightPos(wMagicID))
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        DelTargetCreat();
                                        if (nThinkCount < 2)
                                        {
                                            SearchTarget();
                                            continue;
                                        }
                                    }
                                    break;
                                case 5:
                                    if (RunPosAttack(wMagicID))
                                    {
                                        result = true;
                                    }

                                    break;
                            }
                            break;
                        case PlayJob.Wizard:
                        case PlayJob.Taoist:
                            switch (nCode)
                            {
                                case 1:
                                    result = AvoidTarget(wMagicID);
                                    break;
                                case 2:
                                    if (FollowTarget(wMagicID))
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        DelTargetCreat();
                                        if (nThinkCount < 2)
                                        {
                                            SearchTarget();
                                            continue;
                                        }
                                    }
                                    break;
                                case 3:
                                case 4:
                                    if (WalkToRightPos(wMagicID))
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        DelTargetCreat();
                                        if (nThinkCount < 2)
                                        {
                                            SearchTarget();
                                            continue;
                                        }
                                    }

                                    break;
                                case 5:
                                    result = RunPosAttack(wMagicID);
                                    break;
                            }
                            break;
                    }
                    break;
                }
            }
            catch
            {
                M2Share.Logger.Error(Format("RobotPlayObject::ActThink Name:{0} Code:{1} ", new object[] { ChrName, nCode }));
            }
            return result;
        }
    }
}