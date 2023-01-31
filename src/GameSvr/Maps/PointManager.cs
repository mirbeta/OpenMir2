using GameSvr.Actor;
using GameSvr.Player;
using System.Collections;
using SystemModule.Enums;

namespace GameSvr.Maps
{
    public class PointManager
    {
        public int Count
        {
            get
            {
                return GetCount();
            }
        }
        public FindPathType PathType
        {
            get
            {
                return FPathType;
            }
            set
            {
                FPathType = value;
            }
        }
        public short m_nCurrX;
        public short m_nCurrY;
        public int m_nPostion;
        public byte m_btDirection;
        public int m_nTurnCount;
        public Envirnoment m_PEnvir;
        private readonly ArrayList FPointList;
        private readonly BaseObject FBaseObject;
        private FindPathType FPathType;

        public PointManager(BaseObject ABaseObject)
        {
            m_nCurrX = -1;
            m_nCurrY = -1;
            m_nPostion = -1;
            FBaseObject = ABaseObject;
            FPointList = new ArrayList();
            FPathType = FindPathType.t_Dynamic;
            m_PEnvir = null;
        }

        private int GetCount()
        {
            return FPointList.Count;
        }

        public void Initialize(Envirnoment Envir)
        {
            m_PEnvir = Envir;
            m_nPostion = 0;
        }

        public static byte GetNextDir(byte btDir)
        {
            byte result = 0;
            switch (btDir)
            {
                case Grobal2.DR_UP:
                    result = Grobal2.DR_UPRIGHT;
                    break;
                case Grobal2.DR_UPRIGHT:
                    result = Grobal2.DR_RIGHT;
                    break;
                case Grobal2.DR_RIGHT:
                    result = Grobal2.DR_DOWNRIGHT;
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    result = Grobal2.DR_DOWN;
                    break;
                case Grobal2.DR_DOWN:
                    result = Grobal2.DR_DOWNLEFT;
                    break;
                case Grobal2.DR_DOWNLEFT:
                    result = Grobal2.DR_LEFT;
                    break;
                case Grobal2.DR_LEFT:
                    result = Grobal2.DR_UPLEFT;
                    break;
                case Grobal2.DR_UPLEFT:
                    result = Grobal2.DR_UP;
                    break;
            }
            return result;
        }

        public bool GetPoint(ref short nX, ref short nY)
        {
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
            if (FPathType == FindPathType.t_Dynamic)
            {
                m_nCurrX = nX;
                m_nCurrY = nY;
                if (FBaseObject.Direction > 8)
                {
                    FBaseObject.Direction = 4;
                }
                m_btDirection = FBaseObject.Direction;
                for (int i = 2; i >= 1; i--)
                {
                    if (FBaseObject.Envir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, i, ref nMX, ref nMY))
                    {
                        if (FBaseObject.CanMove(nMX, nMY, false))
                        {
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
                while (true)
                {
                    btDir = GetNextDir(btDir);
                    for (int i = 2; i >= 1; i--)
                    {
                        if (FBaseObject.Envir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nMX, ref nMY))
                        {
                            if (FBaseObject.CanMove(nMX, nMY, false))
                            {
                                nX = nMX;
                                nY = nMY;
                                result = true;
                                return result;
                            }
                        }
                    }
                    nC++;
                    if (nC >= 8)
                    {
                        break;
                    }
                }
            }
            else
            {
                nMX = 0;
                nMY = 0;
                if (((PlayObject)FBaseObject).Envir != m_PEnvir)
                {
                    m_PEnvir = ((PlayObject)FBaseObject).Envir;
                    m_nPostion = 0;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
                nIndex = m_PEnvir.PointList.Count;
                n10 = 99999;
                if (!(m_nPostion >= 0 && m_nPostion < m_PEnvir.PointList.Count && m_nCurrX == nX && m_nCurrY == nY))
                {
                    m_nPostion = 0;
                }
                PointInfo Pt;
                for (int i = m_nPostion; i < m_PEnvir.PointList.Count; i++)
                {
                    Pt = m_PEnvir.PointList[i];
                    nCurrX = Pt.nX;
                    nCurrY = Pt.nY;
                    nC = Math.Abs(nX - nCurrX) + Math.Abs(nY - nCurrY);
                    if (nC < n10)
                    {
                        n10 = nC;
                        nMX = nCurrX;
                        nMY = nCurrY;
                        nIndex = i;
                        m_nPostion = i;
                        result = true;
                        if (n10 <= 0)
                        {
                            break;
                        }
                    }
                }
                if (nIndex >= m_PEnvir.PointList.Count - 1)
                {
                    result = false;
                }
                else
                {
                    if (n10 <= 0 && nIndex >= 0)
                    {
                        nStep = 0;
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++)
                        {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            if (nStep == 0)
                            {
                                btDir = M2Share.GetNextDirection(nX, nY, nCurrX, nCurrY);
                                nMX = nCurrX;
                                nMY = nCurrY;
                                m_nPostion = i;
                            }
                            else
                            {
                                if (M2Share.GetNextDirection(nMX, nMY, nCurrX, nCurrY) == btDir)
                                {
                                    nMX = nCurrX;
                                    nMY = nCurrY;
                                    m_nPostion = i;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            nStep = nStep + 1;
                            if (nStep >= 2)
                            {
                                break;
                            }
                        }
                    }
                    if (!FBaseObject.CanRun(nX, nY, nMX, nMY, false))
                    {
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++)
                        {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            m_nPostion = i;
                            if (m_PEnvir.CanWalkEx(nCurrX, nCurrY, false))
                            {
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

        public static byte GetPoint1_GetNextDir(byte btDir)
        {
            byte result = 0;
            switch (btDir)
            {
                case Grobal2.DR_UP:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_UPRIGHT;
                            break;
                        case 1:
                            result = Grobal2.DR_RIGHT;
                            break;
                        case 2:
                            result = Grobal2.DR_UPLEFT;
                            break;
                        case 3:
                            result = Grobal2.DR_LEFT;
                            break;
                    }
                    break;
                case Grobal2.DR_UPRIGHT:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_RIGHT;
                            break;
                        case 1:
                            result = Grobal2.DR_DOWNRIGHT;
                            break;
                        case 2:
                            result = Grobal2.DR_UP;
                            break;
                        case 3:
                            result = Grobal2.DR_UPLEFT;
                            break;
                    }
                    break;
                case Grobal2.DR_RIGHT:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_DOWNRIGHT;
                            break;
                        case 1:
                            result = Grobal2.DR_DOWN;
                            break;
                        case 2:
                            result = Grobal2.DR_UPRIGHT;
                            break;
                        case 3:
                            result = Grobal2.DR_UP;
                            break;
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_DOWN;
                            break;
                        case 1:
                            result = Grobal2.DR_DOWNLEFT;
                            break;
                        case 2:
                            result = Grobal2.DR_RIGHT;
                            break;
                        case 3:
                            result = Grobal2.DR_UPRIGHT;
                            break;
                    }
                    break;
                case Grobal2.DR_DOWN:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_DOWNLEFT;
                            break;
                        case 1:
                            result = Grobal2.DR_LEFT;
                            break;
                        case 2:
                            result = Grobal2.DR_DOWNRIGHT;
                            break;
                        case 3:
                            result = Grobal2.DR_RIGHT;
                            break;
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_LEFT;
                            break;
                        case 1:
                            result = Grobal2.DR_UPLEFT;
                            break;
                        case 2:
                            result = Grobal2.DR_DOWN;
                            break;
                        case 3:
                            result = Grobal2.DR_DOWNRIGHT;
                            break;
                    }
                    break;
                case Grobal2.DR_LEFT:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_UPLEFT;
                            break;
                        case 1:
                            result = Grobal2.DR_UP;
                            break;
                        case 2:
                            result = Grobal2.DR_DOWNLEFT;
                            break;
                        case 3:
                            result = Grobal2.DR_DOWN;
                            break;
                    }
                    break;
                case Grobal2.DR_UPLEFT:
                    switch (M2Share.RandomNumber.Random(4))
                    {
                        case 0:
                            result = Grobal2.DR_UP;
                            break;
                        case 1:
                            result = Grobal2.DR_UPRIGHT;
                            break;
                        case 2:
                            result = Grobal2.DR_LEFT;
                            break;
                        case 3:
                            result = Grobal2.DR_DOWNLEFT;
                            break;
                    }
                    break;
            }
            return result;
        }

        public bool GetPoint1(ref short nX, ref short nY)
        {
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
            if (FPathType == FindPathType.t_Dynamic)
            {
                m_nCurrX = nX;
                m_nCurrY = nY;
                nC = 0;
                btDir = ((PlayObject)FBaseObject).Direction;
                while (true)
                {
                    btDir = GetPoint1_GetNextDir(btDir);
                    for (int i = 2; i >= 1; i--)
                    {
                        if (((PlayObject)FBaseObject).Envir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nMX, ref nMY))
                        {
                            if (((PlayObject)FBaseObject).CanMove(nMX, nMY, false))
                            {
                                nX = nMX;
                                nY = nMY;
                                result = true;
                                return result;
                            }
                        }
                    }
                    nC++;
                    if (nC >= 8)
                    {
                        break;
                    }
                }
            }
            else
            {
                nMX = 0;
                nMY = 0;
                if (((PlayObject)FBaseObject).Envir != m_PEnvir)
                {
                    m_PEnvir = ((PlayObject)FBaseObject).Envir;
                    m_nPostion = 0;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
                nIndex = m_PEnvir.PointList.Count;
                n10 = 99999;
                if (!(m_nPostion >= 0 && m_nPostion < m_PEnvir.PointList.Count && m_nCurrX == nX && m_nCurrY == nY))
                {
                    m_nPostion = 0;
                }
                PointInfo Pt;
                for (int i = m_nPostion; i < m_PEnvir.PointList.Count; i++)
                {
                    Pt = m_PEnvir.PointList[i];
                    nCurrX = Pt.nX;
                    nCurrY = Pt.nY;
                    nC = Math.Abs(nX - nCurrX) + Math.Abs(nY - nCurrY);
                    if (nC < n10)
                    {
                        n10 = nC;
                        nMX = nCurrX;
                        nMY = nCurrY;
                        nIndex = i;
                        m_nPostion = i;
                        result = true;
                        if (n10 <= 0)
                        {
                            break;
                        }
                    }
                }
                if (nIndex >= m_PEnvir.PointList.Count - 1)
                {
                    result = false;
                }
                else
                {
                    if (n10 <= 0 && nIndex >= 0)
                    {
                        nStep = 0;
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++)
                        {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            if (nStep == 0)
                            {
                                btDir = M2Share.GetNextDirection(nX, nY, nCurrX, nCurrY);
                                nMX = nCurrX;
                                nMY = nCurrY;
                                m_nPostion = i;
                            }
                            else
                            {
                                if (M2Share.GetNextDirection(nMX, nMY, nCurrX, nCurrY) == btDir)
                                {
                                    nMX = nCurrX;
                                    nMY = nCurrY;
                                    m_nPostion = i;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            nStep = nStep + 1;
                            if (nStep >= 2)
                            {
                                break;
                            }
                        }
                    }
                    if (!((PlayObject)FBaseObject).CanRun(nX, nY, nMX, nMY, false))
                    {
                        for (int i = m_nPostion + 1; i < m_PEnvir.PointList.Count; i++)
                        {
                            Pt = m_PEnvir.PointList[i];
                            nCurrX = Pt.nX;
                            nCurrY = Pt.nY;
                            m_nPostion = i;
                            if (m_PEnvir.CanWalkEx(nCurrX, nCurrY, false))
                            {
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