using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.Maps {
    public class PointManager 
    {
        public short m_nCurrX;
        public short m_nCurrY;
        public int m_nPostion;
        public byte m_btDirection;
        public int m_nTurnCount;
        public Envirnoment m_PEnvir;
        public FindPathType PathType;
        private readonly BaseObject FBaseObject;

        public PointManager(BaseObject baseObject) {
            m_nCurrX = -1;
            m_nCurrY = -1;
            m_nPostion = -1;
            FBaseObject = baseObject;
            PathType = FindPathType.Dynamic;
            m_PEnvir = null;
        }

        public void Initialize(Envirnoment Envir) {
            m_PEnvir = Envir;
            m_nPostion = 0;
        }

        private static byte GetNextDir(byte btDir) {
            byte result = 0;
            switch (btDir) {
                case Direction.Up:
                    result = Direction.UpRight;
                    break;
                case Direction.UpRight:
                    result = Direction.Right;
                    break;
                case Direction.Right:
                    result = Direction.DownRight;
                    break;
                case Direction.DownRight:
                    result = Direction.Down;
                    break;
                case Direction.Down:
                    result = Direction.DownLeft;
                    break;
                case Direction.DownLeft:
                    result = Direction.Left;
                    break;
                case Direction.Left:
                    result = Direction.UpLeft;
                    break;
                case Direction.UpLeft:
                    result = Direction.Up;
                    break;
            }
            return result;
        }

        public bool GetPoint(ref short nX, ref short nY) {
            short nMX = 0;
            short nMY = 0;
            int nC;
            int n10;
            int nIndex;
            short nCurrX;
            short nCurrY;
            byte btDir = 0;
            int nStep;
            bool result = false;
            if (PathType == FindPathType.Dynamic) {
                m_nCurrX = nX;
                m_nCurrY = nY;
                if (FBaseObject.Direction > 8) {
                    FBaseObject.Direction = 4;
                }
                m_btDirection = FBaseObject.Direction;
                for (int i = 2; i >= 1; i--) {
                    if (FBaseObject.Envir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, i, ref nMX, ref nMY)) {
                        if (FBaseObject.CanMove(nMX, nMY, false)) {
                            m_nTurnCount = 0;
                            nX = nMX;
                            nY = nMY;
                            result = true;
                            return result;
                        }
                    }
                }
                nC = 0;
                btDir = m_btDirection;
                while (true) {
                    btDir = GetNextDir(btDir);
                    for (int i = 2; i >= 1; i--) {
                        if (FBaseObject.Envir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nMX, ref nMY)) {
                            if (FBaseObject.CanMove(nMX, nMY, false)) {
                                nX = nMX;
                                nY = nMY;
                                result = true;
                                return result;
                            }
                        }
                    }
                    nC++;
                    if (nC >= 8) {
                        break;
                    }
                }
            }
            else {
                nMX = 0;
                nMY = 0;
                if (((PlayObject)FBaseObject).Envir != m_PEnvir) {
                    m_PEnvir = ((PlayObject)FBaseObject).Envir;
                    m_nPostion = 0;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
                nIndex = m_PEnvir.PointList.Count;
                n10 = 99999;
                if (!(m_nPostion >= 0 && m_nPostion < m_PEnvir.PointList.Count && m_nCurrX == nX && m_nCurrY == nY)) {
                    m_nPostion = 0;
                }
                PointInfo Pt;
                for (int i = m_nPostion; i < m_PEnvir.PointList.Count; i++) {
                    Pt = m_PEnvir.PointList[i];
                    nCurrX = Pt.nX;
                    nCurrY = Pt.nY;
                    nC = Math.Abs(nX - nCurrX) + Math.Abs(nY - nCurrY);
                    if (nC < n10) {
                        n10 = nC;
                        nMX = nCurrX;
                        nMY = nCurrY;
                        nIndex = i;
                        m_nPostion = i;
                        result = true;
                        if (n10 <= 0) {
                            break;
                        }
                    }
                }
                if (nIndex >= m_PEnvir.PointList.Count - 1) {
                    result = false;
                }
                else {
                    if (n10 <= 0 && nIndex >= 0) {
                        nStep = 0;
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++) {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            if (nStep == 0) {
                                btDir = M2Share.GetNextDirection(nX, nY, nCurrX, nCurrY);
                                nMX = nCurrX;
                                nMY = nCurrY;
                                m_nPostion = i;
                            }
                            else {
                                if (M2Share.GetNextDirection(nMX, nMY, nCurrX, nCurrY) == btDir) {
                                    nMX = nCurrX;
                                    nMY = nCurrY;
                                    m_nPostion = i;
                                }
                                else {
                                    break;
                                }
                            }
                            nStep = nStep + 1;
                            if (nStep >= 2) {
                                break;
                            }
                        }
                    }
                    if (!FBaseObject.CanRun(nX, nY, nMX, nMY, false)) {
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++) {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            m_nPostion = i;
                            if (m_PEnvir.CanWalkEx(nCurrX, nCurrY, false)) {
                                nMX = nCurrX;
                                nMY = nCurrY;
                                break;
                            }
                        }
                    }
                    nX = nMX;
                    nY = nMY;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
            }
            return result;
        }

        private static byte GetPoint1_GetNextDir(byte btDir) {
            byte result = 0;
            switch (btDir) {
                case Direction.Up:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.UpRight;
                            break;
                        case 1:
                            result = Direction.Right;
                            break;
                        case 2:
                            result = Direction.UpLeft;
                            break;
                        case 3:
                            result = Direction.Left;
                            break;
                    }
                    break;
                case Direction.UpRight:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.Right;
                            break;
                        case 1:
                            result = Direction.DownRight;
                            break;
                        case 2:
                            result = Direction.Up;
                            break;
                        case 3:
                            result = Direction.UpLeft;
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.DownRight;
                            break;
                        case 1:
                            result = Direction.Down;
                            break;
                        case 2:
                            result = Direction.UpRight;
                            break;
                        case 3:
                            result = Direction.Up;
                            break;
                    }
                    break;
                case Direction.DownRight:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.Down;
                            break;
                        case 1:
                            result = Direction.DownLeft;
                            break;
                        case 2:
                            result = Direction.Right;
                            break;
                        case 3:
                            result = Direction.UpRight;
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.DownLeft;
                            break;
                        case 1:
                            result = Direction.Left;
                            break;
                        case 2:
                            result = Direction.DownRight;
                            break;
                        case 3:
                            result = Direction.Right;
                            break;
                    }
                    break;
                case Direction.DownLeft:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.Left;
                            break;
                        case 1:
                            result = Direction.UpLeft;
                            break;
                        case 2:
                            result = Direction.Down;
                            break;
                        case 3:
                            result = Direction.DownRight;
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.UpLeft;
                            break;
                        case 1:
                            result = Direction.Up;
                            break;
                        case 2:
                            result = Direction.DownLeft;
                            break;
                        case 3:
                            result = Direction.Down;
                            break;
                    }
                    break;
                case Direction.UpLeft:
                    switch (M2Share.RandomNumber.Random(4)) {
                        case 0:
                            result = Direction.Up;
                            break;
                        case 1:
                            result = Direction.UpRight;
                            break;
                        case 2:
                            result = Direction.Left;
                            break;
                        case 3:
                            result = Direction.DownLeft;
                            break;
                    }
                    break;
            }
            return result;
        }

        public bool GetPoint1(ref short nX, ref short nY) {
            bool result = false;
            short nMX = 0;
            short nMY = 0;
            int nC;
            int n10;
            int nIndex;
            short nCurrX;
            short nCurrY;
            byte btDir = 0;
            int nStep;
            if (PathType == FindPathType.Dynamic) {
                m_nCurrX = nX;
                m_nCurrY = nY;
                nC = 0;
                btDir = ((PlayObject)FBaseObject).Direction;
                while (true) {
                    btDir = GetPoint1_GetNextDir(btDir);
                    for (int i = 2; i >= 1; i--) {
                        if (((PlayObject)FBaseObject).Envir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nMX, ref nMY)) {
                            if (((PlayObject)FBaseObject).CanMove(nMX, nMY, false)) {
                                nX = nMX;
                                nY = nMY;
                                result = true;
                                return result;
                            }
                        }
                    }
                    nC++;
                    if (nC >= 8) {
                        break;
                    }
                }
            }
            else {
                nMX = 0;
                nMY = 0;
                if (((PlayObject)FBaseObject).Envir != m_PEnvir) {
                    m_PEnvir = ((PlayObject)FBaseObject).Envir;
                    m_nPostion = 0;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
                nIndex = m_PEnvir.PointList.Count;
                n10 = 99999;
                if (!(m_nPostion >= 0 && m_nPostion < m_PEnvir.PointList.Count && m_nCurrX == nX && m_nCurrY == nY)) {
                    m_nPostion = 0;
                }
                PointInfo Pt;
                for (int i = m_nPostion; i < m_PEnvir.PointList.Count; i++) {
                    Pt = m_PEnvir.PointList[i];
                    nCurrX = Pt.nX;
                    nCurrY = Pt.nY;
                    nC = Math.Abs(nX - nCurrX) + Math.Abs(nY - nCurrY);
                    if (nC < n10) {
                        n10 = nC;
                        nMX = nCurrX;
                        nMY = nCurrY;
                        nIndex = i;
                        m_nPostion = i;
                        result = true;
                        if (n10 <= 0) {
                            break;
                        }
                    }
                }
                if (nIndex >= m_PEnvir.PointList.Count - 1) {
                    result = false;
                }
                else {
                    if (n10 <= 0 && nIndex >= 0) {
                        nStep = 0;
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++) {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            if (nStep == 0) {
                                btDir = M2Share.GetNextDirection(nX, nY, nCurrX, nCurrY);
                                nMX = nCurrX;
                                nMY = nCurrY;
                                m_nPostion = i;
                            }
                            else {
                                if (M2Share.GetNextDirection(nMX, nMY, nCurrX, nCurrY) == btDir) {
                                    nMX = nCurrX;
                                    nMY = nCurrY;
                                    m_nPostion = i;
                                }
                                else {
                                    break;
                                }
                            }
                            nStep = nStep + 1;
                            if (nStep >= 2) {
                                break;
                            }
                        }
                    }
                    if (!((PlayObject)FBaseObject).CanRun(nX, nY, nMX, nMY, false)) {
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++) {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            m_nPostion = i;
                            if (m_PEnvir.CanWalkEx(nCurrX, nCurrY, false)) {
                                nMX = nCurrX;
                                nMY = nCurrY;
                                break;
                            }
                        }
                    }
                    nX = nMX;
                    nY = nMY;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
            }
            return result;
        }
    }
}