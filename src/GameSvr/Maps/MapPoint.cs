using System;
using System.Collections;
using SystemModule;

namespace GameSvr
{
    public class TMapPoint
    {
        public int X
        {
            get
            {
                return FX;
            }
            set
            {
                FX = value;
            }
        }
        public int Y
        {
            get
            {
                return FY;
            }
            set
            {
                FY = value;
            }
        }
        public bool Through
        {
            get
            {
                return FThrough;
            }
            set
            {
                FThrough = value;
            }
        }
        private int FX = 0;
        private int FY = 0;
        private bool FThrough = false;

        public TMapPoint(int nX, int nY)
        {
            FX = nX;
            FY = nY;
            FThrough = false;
        }
    }

    public class TPointManager
    {
        public int Count
        {
            get
            {
                return GetCount();
            }
        }
        public TPathType PathType
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
        public short m_nCurrX = 0;
        public short m_nCurrY = 0;
        public int m_nPostion = 0;
        public byte m_btDirection = 0;
        public int m_nTurnCount = 0;
        public Envirnoment m_PEnvir = null;
        private ArrayList FPointList = null;
        private TBaseObject FBaseObject = null;
        private TPathType FPathType;

        public TPointManager(TBaseObject ABaseObject)
        {
            m_nCurrX = -1;
            m_nCurrY = -1;
            m_nPostion = -1;
            FBaseObject = ABaseObject;
            FPointList = new ArrayList();
            FPathType = TPathType.t_Dynamic;
            // t_Fixed; //t_Dynamic;
            m_PEnvir = null;
        }

        private int GetCount()
        {
            int result;
            result = FPointList.Count;
            return result;
        }

        public void Initialize(Envirnoment Envir)
        {
            m_PEnvir = Envir;
            m_nPostion = 0;
        }

        public byte GetPoint_GetNextDir(byte btDir)
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
            bool result;
            short nMX = 0;
            short nMY = 0;
            int nC;
            int n10;
            int nIndex;
            short nCurrX;
            short nCurrY;
            byte btDir = 0;
            PointInfo Pt;
            int nStep;
            result = false;
            if (FPathType == TPathType.t_Dynamic)
            {
                m_nCurrX = nX;
                m_nCurrY = nY;
                if (FBaseObject.m_btDirection > 8)
                {
                    FBaseObject.m_btDirection = 4;
                }
                m_btDirection = FBaseObject.m_btDirection;
                for (var i = 2; i >= 1; i--)
                {
                    if (FBaseObject.m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, i, ref nMX, ref nMY))
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
                    btDir = GetPoint_GetNextDir(btDir);
                    for (var i = 2; i >= 1; i--)
                    {
                        if (FBaseObject.m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nMX, ref nMY))
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
                if (((TPlayObject)FBaseObject).m_PEnvir != m_PEnvir)
                {
                    m_PEnvir = ((TPlayObject)FBaseObject).m_PEnvir;
                    m_nPostion = 0;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
                nIndex = m_PEnvir.m_PointList.Count;
                n10 = 99999;
                if (!(m_nPostion >= 0 && m_nPostion < m_PEnvir.m_PointList.Count && m_nCurrX == nX && m_nCurrY == nY))
                {
                    m_nPostion = 0;
                }
                for (var i = m_nPostion; i < m_PEnvir.m_PointList.Count; i++)
                {
                    Pt = m_PEnvir.m_PointList[i];
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
                if (nIndex >= m_PEnvir.m_PointList.Count - 1)
                {
                    result = false;
                }
                else
                {
                    if (n10 <= 0 && nIndex >= 0)
                    {
                        nStep = 0;
                        for (var i = m_nPostion + 1; i < m_PEnvir.m_PointList.Count; i++)
                        {
                            Pt = m_PEnvir.m_PointList[i];
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
                        for (var i = m_nPostion + 1; i < m_PEnvir.m_PointList.Count; i++)
                        {
                            Pt = m_PEnvir.m_PointList[i];
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

        public byte GetPoint1_GetNextDir(byte btDir)
        {
            byte result = 0;
            switch (btDir)
            {
                case Grobal2.DR_UP:
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
                    switch (new System.Random(4).Next())
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
            PointInfo Pt;
            int nStep;
            if (FPathType == TPathType.t_Dynamic)
            {
                m_nCurrX = nX;
                m_nCurrY = nY;
                nC = 0;
                btDir = ((TPlayObject)FBaseObject).m_btDirection;
                while (true)
                {
                    btDir = GetPoint1_GetNextDir(btDir);
                    for (var i = 2; i >= 1; i--)
                    {
                        if (((TPlayObject)FBaseObject).m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nMX, ref nMY))
                        {
                            if (((TPlayObject)FBaseObject).CanMove(nMX, nMY, false))
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
                if (((TPlayObject)FBaseObject).m_PEnvir != m_PEnvir)
                {
                    m_PEnvir = ((TPlayObject)FBaseObject).m_PEnvir;
                    m_nPostion = 0;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                }
                nIndex = m_PEnvir.m_PointList.Count;
                n10 = 99999;
                if (!(m_nPostion >= 0 && m_nPostion < m_PEnvir.m_PointList.Count && m_nCurrX == nX && m_nCurrY == nY))
                {
                    m_nPostion = 0;
                }
                for (var i = m_nPostion; i < m_PEnvir.m_PointList.Count; i++)
                {
                    Pt = m_PEnvir.m_PointList[i];
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
                if (nIndex >= m_PEnvir.m_PointList.Count - 1)
                {
                    result = false;
                }
                else
                {
                    if (n10 <= 0 && nIndex >= 0)
                    {
                        nStep = 0;
                        for (var i = m_nPostion + 1; i < m_PEnvir.m_PointList.Count; i++)
                        {
                            Pt = m_PEnvir.m_PointList[i];
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
                    if (!((TPlayObject)FBaseObject).CanRun(nX, nY, nMX, nMY, false))
                    {
                        for (var i = m_nPostion + 1; i < m_PEnvir.m_PointList.Count; i++)
                        {
                            Pt = m_PEnvir.m_PointList[i];
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