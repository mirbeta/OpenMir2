using OpenMir2;
using OpenMir2.Data;
using OpenMir2.Enums;
using System.Collections;
using SystemModule;
using SystemModule.Const;

namespace RobotSystem.Services
{
    public partial class RobotPlayer
    {
        private MapWalkXY FindGoodPathA(MapWalkXY[] WalkStep, int nRange, int nType)
        {
            int n10 = int.MaxValue;
            MapWalkXY result = default;
            MapWalkXY MapWalkXY = default;
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
            if (MapWalkXY.WalkStep > 0 && Master != null)
            {
                int nMonCount = MapWalkXY.MonCount;
                int nMastrRange = MapWalkXY.MastrRange;
                n10 = int.MaxValue;
                MapWalkXY MapWalkXYA = MapWalkXY;
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
                if (MapWalkXY.WalkStep == 0)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY.WalkStep > 0)
            {
                result = MapWalkXY;
            }
            return result;
        }

        private MapWalkXY FindGoodPathB(MapWalkXY[] WalkStep, int nType)
        {
            MapWalkXY result = default;
            MapWalkXY MapWalkXY = default;
            int n10 = int.MaxValue;
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
            if (MapWalkXY.WalkStep > 0 && Master != null)
            {
                int nMonCount = MapWalkXY.MonCount;
                int nMastrRange = MapWalkXY.MastrRange;
                n10 = int.MaxValue;
                MapWalkXY MapWalkXYA = MapWalkXY;
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
                if (MapWalkXY.WalkStep == 0)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY.WalkStep > 0)
            {
                result = MapWalkXY;
            }
            return result;
        }

        private MapWalkXY FindMinRange(MapWalkXY[] WalkStep)
        {
            int n10 = int.MaxValue;
            MapWalkXY result = default;
            MapWalkXY MapWalkXY = default;
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (WalkStep[i].WalkStep > 0)
                {
                    int n1C = Math.Abs(WalkStep[i].X - TargetCret.CurrX) + Math.Abs(WalkStep[i].Y - TargetCret.CurrY);
                    if (n1C < n10)
                    {
                        n10 = n1C;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY.WalkStep > 0)
            {
                int nMonCount = MapWalkXY.MonCount;
                MapWalkXY MapWalkXYA = MapWalkXY;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
                {
                    if (WalkStep[i].WalkStep > 0 && WalkStep[i].MonCount <= nMonCount)
                    {
                        int n1C = Math.Abs(WalkStep[i].X - TargetCret.CurrX) + Math.Abs(WalkStep[i].Y - TargetCret.CurrY);
                        if (n1C <= n10)
                        {
                            n10 = n1C;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY.WalkStep == 0)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY.WalkStep > 0)
            {
                result = MapWalkXY;
            }
            return result;
        }

        /// <summary>
        /// 检测下一步在不在攻击位
        /// </summary>
        /// <returns></returns>
        private bool CanWalkNextPosition(short nX, short nY, int nRange, byte btDir, bool boFlag)
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

        private bool FindPosOfSelf(ref MapWalkXY[] walkStep, int nRange, bool boFlag)
        {
            if (walkStep == null)
            {
                walkStep = new MapWalkXY[8];
            }
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir))
                    {
                        walkStep[i] = new MapWalkXY();
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        walkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool ActThinkFindPosOfSelf(ref MapWalkXY[] walkStep, int nRange, bool boFlag)
        {
            if (walkStep == null)
            {
                walkStep = new MapWalkXY[8];
            }
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
                        walkStep[i] = new MapWalkXY();
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        walkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool FindPosOfTarget(ref MapWalkXY[] walkStep, short nTargetX, short nTargetY, int nRange, bool boFlag)
        {
            if (walkStep == null)
            {
                walkStep = new MapWalkXY[8];
            }
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
            {
                if (Envir.GetNextPosition(nTargetX, nTargetY, i, nRange, ref nCurrX, ref nCurrY) && Envir.CanWalkEx(nCurrX, nCurrY, false))
                {
                    if ((!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) && IsGotoXy(CurrX, CurrY, nCurrX, nCurrY))
                    {
                        walkStep[i] = new MapWalkXY();
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        walkStep[i].MonCount = GetRangeTargetCount(nCurrX, nCurrY, 2);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool FindPos(ref MapWalkXY[] walkStep, int nRange, bool boFlag)
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
                        walkStep[i] = new MapWalkXY();
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        walkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
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
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        walkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPos(MapWalkXY[] walkStep, int nRange, bool boFlag)
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
                        walkStep[i] = new MapWalkXY();
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        walkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
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
                        walkStep[i].WalkStep = nRange;
                        walkStep[i].X = nCurrX;
                        walkStep[i].Y = nCurrY;
                        walkStep[i].MonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        walkStep[i].MonCount = GetNearTargetCount(nCurrX, nCurrY);
                        walkStep[i].MastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool WalkToRightPos(int wMagicID)
        {
            MapWalkXY[] walkStep = null;
            try
            {
                bool boFlag = Race == 108 || new ArrayList(new int[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID) || Job == 0;
                MapWalkXY mapWalkXy;
                int nRange;
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
                    if (wMagicID <= 62 && wMagicID >= 60)
                    {
                        nRange = 6;
                    }
                    for (int i = nRange; i >= 1; i--)
                    {
                        if (FindPosOfTarget(ref walkStep, TargetCret.CurrX, TargetCret.CurrY, i, boFlag))
                        {
                            mapWalkXy = FindGoodPathB(walkStep, 0);
                            if (mapWalkXy.WalkStep > 0)
                            {
                                if (GotoNext(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    for (int i = 2; i >= 1; i--)
                    {
                        if (FindPosOfSelf(ref walkStep, i, boFlag))
                        {
                            if (Master != null)
                            {
                                mapWalkXy = FindGoodPathB(walkStep, 1);
                            }
                            else
                            {
                                mapWalkXy = FindGoodPathB(walkStep, 0);
                            }
                            if (mapWalkXy.WalkStep > 0)
                            {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    return true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (wMagicID > 0)
                    {
                        nRange = HUtil32._MAX(SystemShare.RandomNumber.Random(3), 2);
                    }
                    else
                    {
                        nRange = 1;
                    }
                    boFlag = Race == 108 || new ArrayList(new int[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID) || nRange == 1;
                    for (int i = 2; i >= 1; i--)
                    {
                        if (FindPosOfSelf(ref walkStep, i, boFlag))
                        {
                            mapWalkXy = FindGoodPathA(walkStep, nRange, 0);
                            if (mapWalkXy.WalkStep > 0)
                            {
                                if (GotoNextOne(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    for (int i = 2; i >= 1; i--)
                    {
                        if (ActThinkFindPosOfSelf(ref walkStep, i, boFlag))
                        {
                            mapWalkXy = FindMinRange(walkStep);
                            if (mapWalkXy.WalkStep > 0)
                            {
                                if (GotoNextOne(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    for (int i = nRange; i >= 1; i--)
                    {
                        if (FindPosOfTarget(ref walkStep, TargetCret.CurrX, TargetCret.CurrY, i, boFlag))
                        {
                            mapWalkXy = FindGoodPathB(walkStep, 0);
                            if (mapWalkXy.WalkStep > 0)
                            {
                                if (GotoNextOne(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                                {
                                    MRunPos.btDirection = 0;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                LogService.Error("WalkToRightPos:" + ChrName);
            }
            return false;
        }

        private bool AvoidTarget(short wMagicID)
        {
            short nX = 0;
            short nY = 0;
            MapWalkXY[] WalkStep = null;
            int nRange = HUtil32._MAX(SystemShare.RandomNumber.Random(3), 2);
            bool boFlag = Race == 108 || new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID);
            byte btDir;
            MapWalkXY mapWalkXy;
            for (int i = nRange; i >= 1; i--)
            {
                if (FindPosOfSelf(ref WalkStep, i, boFlag))
                {
                    mapWalkXy = FindGoodPathB(WalkStep, 0);
                    if (mapWalkXy.WalkStep > 0)
                    {
                        btDir = SystemShare.GetNextDirection(CurrX, CurrY, mapWalkXy.X, mapWalkXy.Y);
                        if (GotoNextOne(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                        {
                            if (Race != 108)
                            {
                                for (int j = nRange; j >= 1; j--)// 再跑1次
                                {
                                    if (Envir.GetNextPosition(mapWalkXy.X, mapWalkXy.Y, btDir, j, ref nX, ref nY) && Envir.CanWalkEx(nX, nY, true) && GetNearTargetCount(nX, nY) <= mapWalkXy.MonCount)
                                    {
                                        GotoNextOne(nX, nY, Race != 108);
                                        break;
                                    }
                                }
                            }
                            MRunPos.btDirection = 0;
                            return true;
                        }
                    }
                }
            }
            for (int i = nRange; i >= 1; i--)
            {
                if (ActThinkFindPosOfSelf(ref WalkStep, i, boFlag))
                {
                    mapWalkXy = FindGoodPathB(WalkStep, 0);
                    if (mapWalkXy.WalkStep > 0)
                    {
                        btDir = SystemShare.GetNextDirection(CurrX, CurrY, mapWalkXy.X, mapWalkXy.Y);
                        if (GotoNextOne(mapWalkXy.X, mapWalkXy.Y, Race != 108))
                        {
                            for (int j = nRange; j >= 1; j--)
                            {
                                // 再跑1次
                                if (Envir.GetNextPosition(mapWalkXy.X, mapWalkXy.Y, btDir, j, ref nX, ref nY) && Envir.CanWalkEx(nX, nY, true) && GetNearTargetCount(nX, nY) <= mapWalkXy.MonCount)
                                {
                                    mapWalkXy.X = nX;
                                    mapWalkXy.Y = nY;
                                    GotoNextOne(mapWalkXy.X, mapWalkXy.Y, Race != 108);
                                    break;
                                }
                            }
                            MRunPos.btDirection = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool FollowTarget(short wMagicID)
        {
            int nRange = 2;
            MapWalkXY[] WalkStep = null;
            MapWalkXY MapWalkXY;
            bool boFlag = Race == 108 || new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID);
            for (int i = nRange; i >= 1; i--)
            {
                if (FindPosOfSelf(ref WalkStep, i, boFlag))
                {
                    MapWalkXY = FindMinRange(WalkStep);
                    if (MapWalkXY.WalkStep > 0)
                    {
                        if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                        {
                            MRunPos.btDirection = 0;
                            return true;
                        }
                    }
                }
            }
            for (int i = nRange; i >= 1; i--)
            {
                if (ActThinkFindPosOfSelf(ref WalkStep, i, boFlag))
                {
                    MapWalkXY = FindMinRange(WalkStep);
                    if (MapWalkXY.WalkStep > 0)
                    {
                        if (GotoNextOne(MapWalkXY.X, MapWalkXY.Y, Race != 108))
                        {
                            MRunPos.btDirection = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool MotaeboPos(short wMagicID)
        {
            short nTargetX = 0;
            short nTargetY = 0;
            if (TargetCret == null || Master == null)
            {
                return false;
            }
            if (GetPoseCreate() == TargetCret || TargetCret.GetPoseCreate() == this)
            {
                byte btNewDir = SystemShare.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
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

        private MapWalkXY FindPosOfDir(byte nDir, int nRange, bool boFlag)
        {
            MapWalkXY result = default;
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

        private static byte RunPosAttackGetNextRunPos(byte btDir, bool boTurn)
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

        private bool RunPosAttack(int magicId)
        {
            MapWalkXY[] walkStep = new MapWalkXY[2];
            int nRange;
            bool result = false;
            byte btDir = SystemShare.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
            byte btNewDir1 = RunPosAttackGetNextRunPos(btDir, true);
            byte btNewDir2 = RunPosAttackGetNextRunPos(btDir, false);
            if (Job == 0)
            {
                nRange = 1;
                if (magicId == 43)
                {
                    nRange = 2;
                }
                if (magicId == 12)
                {
                    nRange = 2;
                }
                if (magicId <= 62 && magicId >= 60)
                {
                    nRange = 6;
                }
                walkStep[0] = FindPosOfDir(btNewDir1, nRange, true);
                walkStep[1] = FindPosOfDir(btNewDir2, nRange, true);
            }
            else
            {
                nRange = 2;
                walkStep[0] = FindPosOfDir(btNewDir1, nRange, false);
                walkStep[1] = FindPosOfDir(btNewDir2, nRange, false);
            }
            int nNearTargetCount = GetNearTargetCount(CurrX, CurrY);
            MapWalkXY mapWalkXY = default;
            if (walkStep[0].WalkStep > 0 && walkStep[1].WalkStep > 0)
            {
                if (MRunPos.btDirection > 0)
                {
                    mapWalkXY = walkStep[1];
                }
                else
                {
                    mapWalkXY = walkStep[0];
                }
                if (nNearTargetCount < walkStep[0].MonCount && nNearTargetCount < walkStep[1].MonCount)
                {
                    mapWalkXY = default(MapWalkXY);
                }
                else if (MRunPos.btDirection > 0 && nNearTargetCount < walkStep[1].MonCount)
                {
                    mapWalkXY = default(MapWalkXY);
                }
                else if (MRunPos.btDirection <= 0 && nNearTargetCount < walkStep[0].MonCount)
                {
                    mapWalkXY = default(MapWalkXY);
                }
                if (nNearTargetCount > 0 && mapWalkXY.WalkStep > 0 && mapWalkXY.MonCount > nNearTargetCount)
                {
                    mapWalkXY = default(MapWalkXY);
                }
            }
            else if (walkStep[0].WalkStep > 0)
            {
                mapWalkXY = walkStep[0];
                if (nNearTargetCount < walkStep[0].MonCount)
                {
                    mapWalkXY = default(MapWalkXY);
                }
                MRunPos.btDirection = 0;
            }
            else if (walkStep[1].WalkStep > 0)
            {
                mapWalkXY = walkStep[1];
                if (nNearTargetCount < walkStep[1].MonCount)
                {
                    mapWalkXY = default(MapWalkXY);
                }
                MRunPos.btDirection = 1;
            }
            if (mapWalkXY.WalkStep > 0)
            {
                if (GotoNextOne(mapWalkXY.X, mapWalkXY.Y, Race != 108))
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

        private bool ActThink(short magicId)
        {
            bool result = false;
            int nCode = 0;
            int nThinkCount = 0;
            try
            {
                while (true)
                {
                    if (TargetCret == null || magicId > 255)
                    {
                        break;
                    }
                    nThinkCount = nThinkCount + 1;
                    nCode = DoThink(magicId);
                    switch (Job)
                    {
                        case 0:
                            switch (nCode)
                            {
                                case 2:
                                    if (WalkToRightPos(magicId))
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
                                    if (RunPosAttack(magicId))
                                    {
                                        result = true;
                                    }

                                    break;
                            }
                            break;
                        case PlayerJob.Wizard:
                        case PlayerJob.Taoist:
                            switch (nCode)
                            {
                                case 1:
                                    result = AvoidTarget(magicId);
                                    break;
                                case 2:
                                    if (FollowTarget(magicId))
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
                                    if (WalkToRightPos(magicId))
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
                                    result = RunPosAttack(magicId);
                                    break;
                            }
                            break;
                    }
                    break;
                }
            }
            catch
            {
                LogService.Error(Format("RobotPlayObject::ActThink Name:{0} Code:{1} ", ChrName, nCode));
            }
            return result;
        }
    }
}