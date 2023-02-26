using GameSrv.Magic;
using System.Collections;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.RobotPlay {
    public partial class RobotPlayObject {
        public MapWalkXY ActThink_FindGoodPathA(MapWalkXY[] WalkStep, int nRange, int nType) {
            MapWalkXY result = null;
            int n10 = int.MaxValue;
            int nMastrRange;
            int nMonCount;
            MapWalkXY MapWalkXY = null;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (WalkStep[i].nWalkStep > 0 && Math.Abs(WalkStep[i].nX - TargetCret.CurrX) >= nRange && Math.Abs(WalkStep[i].nY - TargetCret.CurrY) >= nRange) {
                    if (WalkStep[i].nMonCount < n10) {
                        n10 = WalkStep[i].nMonCount;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null && Master != null) {
                nMonCount = MapWalkXY.nMonCount;
                nMastrRange = MapWalkXY.nMastrRange;
                n10 = int.MaxValue;
                MapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                    if (WalkStep[i].nWalkStep > 0 && WalkStep[i].nMonCount <= nMonCount && Math.Abs(WalkStep[i].nX - TargetCret.CurrX) >= nRange && Math.Abs(WalkStep[i].nY - TargetCret.CurrY) >= nRange) {
                        if (WalkStep[i].nMastrRange < n10 && WalkStep[i].nMastrRange < nMastrRange) {
                            n10 = WalkStep[i].nMastrRange;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null) {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null) {
                result = MapWalkXY;
            }
            return result;
        }

        public MapWalkXY ActThink_FindGoodPathB(MapWalkXY[] WalkStep, int nType) {
            MapWalkXY result = null;
            int nMastrRange;
            int nMonCount;
            MapWalkXY MapWalkXY = null;
            int n10 = int.MaxValue;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (WalkStep[i].nWalkStep > 0) {
                    if (WalkStep[i].nMonCount < n10) {
                        n10 = WalkStep[i].nMonCount;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null && Master != null) {
                nMonCount = MapWalkXY.nMonCount;
                nMastrRange = MapWalkXY.nMastrRange;
                n10 = int.MaxValue;
                MapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                    if (WalkStep[i].nWalkStep > 0 && WalkStep[i].nMonCount <= nMonCount) {
                        if (WalkStep[i].nMastrRange < n10 && WalkStep[i].nMastrRange < nMastrRange) {
                            n10 = WalkStep[i].nMastrRange;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null) {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null) {
                result = MapWalkXY;
            }
            return result;
        }

        public MapWalkXY ActThink_FindMinRange(MapWalkXY[] WalkStep) {
            MapWalkXY result = null;
            int n10 = int.MaxValue;
            int n1C;
            int nMonCount;
            MapWalkXY MapWalkXY = null;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (WalkStep[i].nWalkStep > 0) {
                    n1C = Math.Abs(WalkStep[i].nX - TargetCret.CurrX) + Math.Abs(WalkStep[i].nY - TargetCret.CurrY);
                    if (n1C < n10) {
                        n10 = n1C;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null) {
                nMonCount = MapWalkXY.nMonCount;
                MapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                    if (WalkStep[i].nWalkStep > 0 && WalkStep[i].nMonCount <= nMonCount) {
                        n1C = Math.Abs(WalkStep[i].nX - TargetCret.CurrX) + Math.Abs(WalkStep[i].nY - TargetCret.CurrY);
                        if (n1C <= n10) {
                            n10 = n1C;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null) {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null) {
                result = MapWalkXY;
            }
            return result;
        }

        public bool ActThink_CanWalkNextPosition(short nX, short nY, int nRange, byte btDir, bool boFlag) {
            // 检测下一步在不在攻击位
            short nCurrX = 0;
            short nCurrY = 0;
            bool result = false;
            if (Envir.GetNextPosition(nX, nY, btDir, 1, ref nCurrX, ref nCurrY) && CanMove(nX, nY, nCurrX, nCurrY, false) && !boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) {
                result = true;
                return result;
            }
            if (Envir.GetNextPosition(nX, nY, btDir, 2, ref nCurrX, ref nCurrY) && CanMove(nX, nY, nCurrX, nCurrY, false) && !boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) {
                result = true;
                return result;
            }
            return result;
        }

        public bool ActThink_FindPosOfSelf(MapWalkXY[] WalkStep, int nRange, bool boFlag) {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(CurrX, CurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false)) {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPosOfSelf(MapWalkXY[] WalkStep, int nRange, bool boFlag) {
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            bool result = false;
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(CurrX, CurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false)) {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir) || ActThink_CanWalkNextPosition(nCurrX, nCurrY, nRange, i, boFlag)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool ActThink_FindPosOfTarget(MapWalkXY[] WalkStep, short nTargetX, short nTargetY, int nRange, bool boFlag) {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(nTargetX, nTargetY, i, nRange, ref nCurrX, ref nCurrY) && Envir.CanWalkEx(nCurrX, nCurrY, false)) {
                    if ((!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) && IsGotoXY(CurrX, CurrY, nCurrX, nCurrY)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        WalkStep[i].nMonCount = GetRangeTargetCount(nCurrX, nCurrY, 2);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink_FindPos(MapWalkXY[] WalkStep, int nRange, bool boFlag) {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 2, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false)) {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            if (result) {
                return result;
            }
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 1, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false)) {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPos(MapWalkXY[] WalkStep, int nRange, bool boFlag) {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 1, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false)) {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir) || ActThink_CanWalkNextPosition(nCurrX, nCurrY, nRange, i, boFlag)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            if (result) {
                return result;
            }
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (byte i = Direction.Up; i <= Direction.UpLeft; i++) {
                if (Envir.GetNextPosition(CurrX, CurrY, i, 2, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false)) {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, TargetCret, nRange, ref btDir) || ActThink_CanWalkNextPosition(nCurrX, nCurrY, nRange, i, boFlag)) {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink_WalkToRightPos(int wMagicID) {
            bool boFlag;
            int nRange;
            MapWalkXY[] WalkStep = null;
            MapWalkXY MapWalkXY;
            bool result = false;
            try {
                boFlag = Race == 108 || new ArrayList(new int[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID) || Job == 0;
                if (Job == 0 || wMagicID <= 0) {
                    nRange = 1;
                    if (wMagicID == 43) {
                        nRange = 4;
                    }
                    if (wMagicID == 12) {
                        nRange = 2;
                    }
                    if (new ArrayList(new[] { 60, 61, 62 }).Contains(wMagicID)) {
                        nRange = 6;
                    }
                    for (int i = nRange; i >= 1; i--) {
                        if (ActThink_FindPosOfTarget(WalkStep, TargetCret.CurrX, TargetCret.CurrY, i, boFlag)) {
                            MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                            if (MapWalkXY.nWalkStep > 0) {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (int i = 2; i >= 1; i--) {
                        if (ActThink_FindPosOfSelf(WalkStep, i, boFlag)) {
                            if (Master != null) {
                                MapWalkXY = ActThink_FindGoodPathB(WalkStep, 1);
                            }
                            else {
                                MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                            }
                            if (MapWalkXY.nWalkStep > 0) {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                }
                else {
                    if (wMagicID > 0) {
                        nRange = HUtil32._MAX(M2Share.RandomNumber.Random(3), 2);
                    }
                    else {
                        nRange = 1;
                    }
                    boFlag = Race == 108 || new ArrayList(new int[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID) || nRange == 1;
                    for (int i = 2; i >= 1; i--) {
                        if (ActThink_FindPosOfSelf(WalkStep, i, boFlag)) {
                            MapWalkXY = ActThink_FindGoodPathA(WalkStep, nRange, 0);
                            if (MapWalkXY.nWalkStep > 0) {
                                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (int i = 2; i >= 1; i--) {
                        if (ActThink__FindPosOfSelf(WalkStep, i, boFlag)) {
                            MapWalkXY = ActThink_FindMinRange(WalkStep);
                            if (MapWalkXY.nWalkStep > 0) {
                                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (int i = nRange; i >= 1; i--) {
                        if (ActThink_FindPosOfTarget(WalkStep, TargetCret.CurrX, TargetCret.CurrY, i, boFlag)) {
                            MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                            if (MapWalkXY.nWalkStep > 0) {
                                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            catch {
                M2Share.Logger.Error("WalkToRightPos:" + ChrName);
            }
            return result;
        }

        public bool ActThink_AvoidTarget(short wMagicID) {
            short nX = 0;
            short nY = 0;
            bool boFlag;
            MapWalkXY[] WalkStep = null;
            bool result = false;
            int nRange = HUtil32._MAX(M2Share.RandomNumber.Random(3), 2);
            boFlag = Race == 108 || new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID);
            byte btDir;
            MapWalkXY MapWalkXY;
            for (int i = nRange; i >= 1; i--) {
                if (ActThink_FindPosOfSelf(WalkStep, i, boFlag)) {
                    MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                    if (MapWalkXY.nWalkStep > 0) {
                        btDir = M2Share.GetNextDirection(CurrX, CurrY, MapWalkXY.nX, MapWalkXY.nY);
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                            if (Race != 108) {
                                for (int j = nRange; j >= 1; j--) {
                                    // 再跑1次
                                    if (Envir.GetNextPosition(MapWalkXY.nX, MapWalkXY.nY, btDir, j, ref nX, ref nY) && Envir.CanWalkEx(nX, nY, true) && GetNearTargetCount(nX, nY) <= MapWalkXY.nMonCount) {
                                        GotoNextOne(nX, nY, Race != 108);
                                        break;
                                    }
                                }
                            }
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            for (int i = nRange; i >= 1; i--) {
                if (ActThink__FindPosOfSelf(WalkStep, i, boFlag)) {
                    MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                    if (MapWalkXY.nWalkStep > 0) {
                        btDir = M2Share.GetNextDirection(CurrX, CurrY, MapWalkXY.nX, MapWalkXY.nY);
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                            for (int j = nRange; j >= 1; j--) {
                                // 再跑1次
                                if (Envir.GetNextPosition(MapWalkXY.nX, MapWalkXY.nY, btDir, j, ref nX, ref nY) && Envir.CanWalkEx(nX, nY, true) && GetNearTargetCount(nX, nY) <= MapWalkXY.nMonCount) {
                                    MapWalkXY.nX = nX;
                                    MapWalkXY.nY = nY;
                                    GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108);
                                    break;
                                }
                            }
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public bool ActThink_FollowTarget(short wMagicID) {
            int nRange = 2;
            MapWalkXY[] WalkStep = null;
            MapWalkXY MapWalkXY;
            bool result = false;
            bool boFlag = Race == 108 || new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2, MagicConst.SKILL_FIRECHARM }).Contains(wMagicID);
            for (int i = nRange; i >= 1; i--) {
                if (ActThink_FindPosOfSelf(WalkStep, i, boFlag)) {
                    MapWalkXY = ActThink_FindMinRange(WalkStep);
                    if (MapWalkXY.nWalkStep > 0) {
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            for (int i = nRange; i >= 1; i--) {
                if (ActThink__FindPosOfSelf(WalkStep, i, boFlag)) {
                    MapWalkXY = ActThink_FindMinRange(WalkStep);
                    if (MapWalkXY.nWalkStep > 0) {
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public bool ActThink_MotaeboPos(short wMagicID) {
            bool result = false;
            short nTargetX = 0;
            short nTargetY = 0;
            byte btNewDir;
            if (TargetCret == null || Master == null) {
                return result;
            }
            if (GetPoseCreate() == TargetCret || TargetCret.GetPoseCreate() == this) {
                btNewDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, btNewDir, 1, ref nTargetX, ref nTargetY)) {
                    if (Envir.CanWalk(nTargetX, nTargetY, true)) {
                        result = true;
                        return result;
                    }
                }
            }
            result = ActThink_WalkToRightPos(wMagicID);
            return result;
        }

        public MapWalkXY ActThink_FindPosOfDir(byte nDir, int nRange, bool boFlag) {
            MapWalkXY result = null;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, nDir, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false) && (boFlag && CanLineAttack(nCurrX, nCurrY) || !boFlag) && IsGotoXY(CurrX, CurrY, nCurrX, nCurrY)) {
                result = new MapWalkXY();
                result.nWalkStep = nRange;
                result.nX = nCurrX;
                result.nY = nCurrY;
                result.nMonRange = Math.Abs(nCurrX - TargetCret.CurrX) + Math.Abs(nCurrY - TargetCret.CurrY);
                result.nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                result.nMastrRange = GetMasterRange(nCurrX, nCurrY);
            }
            return result;
        }

        public static byte ActThink_RunPosAttack_GetNextRunPos(byte btDir, bool boTurn) {
            byte result = 0;
            if (boTurn) {
                switch (btDir) {
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
            else {
                switch (btDir) {
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

        public bool ActThink_RunPosAttack(int wMagicID) {
            MapWalkXY[] WalkStep = new MapWalkXY[2];
            MapWalkXY MapWalkXY;
            int nRange;
            bool boFlag;
            int nNearTargetCount;
            bool result = false;
            byte btDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
            byte btNewDir1 = ActThink_RunPosAttack_GetNextRunPos(btDir, true);
            byte btNewDir2 = ActThink_RunPosAttack_GetNextRunPos(btDir, false);
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 2, 0);
            if (Job == 0) {
                nRange = 1;
                if (wMagicID == 43) {
                    nRange = 2;
                }
                if (wMagicID == 12) {
                    nRange = 2;
                }
                if (new ArrayList(new[] { 60, 61, 62 }).Contains(wMagicID)) {
                    nRange = 6;
                }
                WalkStep[0] = ActThink_FindPosOfDir(btNewDir1, nRange, true);
                WalkStep[1] = ActThink_FindPosOfDir(btNewDir2, nRange, true);
            }
            else {
                nRange = 2;
                boFlag = false;
                WalkStep[0] = ActThink_FindPosOfDir(btNewDir1, nRange, boFlag);
                WalkStep[1] = ActThink_FindPosOfDir(btNewDir2, nRange, boFlag);
            }
            nNearTargetCount = GetNearTargetCount(CurrX, CurrY);
            MapWalkXY = null;
            if (WalkStep[0].nWalkStep > 0 && WalkStep[1].nWalkStep > 0) {
                if (m_RunPos.btDirection > 0) {
                    MapWalkXY = WalkStep[1];
                }
                else {
                    MapWalkXY = WalkStep[0];
                }
                if (nNearTargetCount < WalkStep[0].nMonCount && nNearTargetCount < WalkStep[1].nMonCount) {
                    MapWalkXY = null;
                }
                else if (m_RunPos.btDirection > 0 && nNearTargetCount < WalkStep[1].nMonCount) {
                    MapWalkXY = null;
                }
                else if (m_RunPos.btDirection <= 0 && nNearTargetCount < WalkStep[0].nMonCount) {
                    MapWalkXY = null;
                }
                if (nNearTargetCount > 0 && MapWalkXY != null && MapWalkXY.nMonCount > nNearTargetCount) {
                    MapWalkXY = null;
                }
            }
            else if (WalkStep[0].nWalkStep > 0) {
                MapWalkXY = WalkStep[0];
                if (nNearTargetCount < WalkStep[0].nMonCount) {
                    MapWalkXY = null;
                }
                m_RunPos.btDirection = 0;
            }
            else if (WalkStep[1].nWalkStep > 0) {
                MapWalkXY = WalkStep[1];
                if (nNearTargetCount < WalkStep[1].nMonCount) {
                    MapWalkXY = null;
                }
                m_RunPos.btDirection = 1;
            }
            if (MapWalkXY != null) {
                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, Race != 108)) {
                    result = true;
                }
            }
            if (!result) {
                m_RunPos.nAttackCount = 0;
            }
            return result;
        }

        private bool ActThink(short wMagicID) {
            bool result = false;
            int nCode = 0;
            int nThinkCount = 0;
            try {
                while (true) {
                    if (TargetCret == null || wMagicID > 255) {
                        break;
                    }
                    nThinkCount = nThinkCount + 1;
                    nCode = DoThink(wMagicID);
                    switch (Job) {
                        case 0:
                            switch (nCode) {
                                case 2:
                                    if (ActThink_WalkToRightPos(wMagicID)) {
                                        result = true;
                                    }
                                    else {
                                        DelTargetCreat();
                                        if (nThinkCount < 2) {
                                            SearchTarget();
                                            continue;
                                        }
                                    }
                                    break;
                                case 5:
                                    if (ActThink_RunPosAttack(wMagicID)) {
                                        result = true;
                                    }

                                    break;
                            }
                            break;
                        case PlayJob.Wizard:
                        case PlayJob.Taoist:
                            switch (nCode) {
                                case 1:
                                    result = ActThink_AvoidTarget(wMagicID);
                                    break;
                                case 2:
                                    if (ActThink_FollowTarget(wMagicID)) {
                                        result = true;
                                    }
                                    else {
                                        DelTargetCreat();
                                        if (nThinkCount < 2) {
                                            SearchTarget();
                                            continue;
                                        }
                                    }
                                    break;
                                case 3:
                                case 4:
                                    if (ActThink_WalkToRightPos(wMagicID)) {
                                        result = true;
                                    }
                                    else {
                                        DelTargetCreat();
                                        if (nThinkCount < 2) {
                                            SearchTarget();
                                            continue;
                                        }
                                    }

                                    break;
                                case 5:
                                    result = ActThink_RunPosAttack(wMagicID);
                                    break;
                            }
                            break;
                    }
                    break;
                }
            }
            catch {
                M2Share.Logger.Error(Format("RobotPlayObject::ActThink Name:{0} Code:{1} ", new object[] { ChrName, nCode }));
            }
            return result;
        }
    }
}