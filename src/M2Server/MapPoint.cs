using System;
using System.Collections;

namespace M2Server
{
    public class TMapPoint
    {
        public int X
        {
          get {
            return FX;
          }
          set {
            FX = value;
          }
        }
        public int Y
        {
          get {
            return FY;
          }
          set {
            FY = value;
          }
        }
        public bool Through
        {
          get {
            return FThrough;
          }
          set {
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
          get {
            return GetCount();
          }
        }
        public TPathType PathType
        {
          get {
            return FPathType;
          }
          set {
            FPathType = value;
          }
        }
        public int m_nCurrX = 0;
        public int m_nCurrY = 0;
        public int m_nPostion = 0;
        public byte m_btDirection = 0;
        public int m_nTurnCount = 0;
        public TEnvirnoment m_PEnvir = null;
        private ArrayList FPointList = null;
        private TBaseObject FBaseObject = null;
        private TPathType FPathType;
     
        public TPointManager(TBaseObject ABaseObject)
        {
            m_nCurrX =  -1;
            m_nCurrY =  -1;
            m_nPostion =  -1;
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

        public void Initialize(TEnvirnoment Envir)
        {
            m_PEnvir = Envir;
            m_nPostion = 0;
        }

        public byte GetPoint_GetNextDir(byte btDir)
        {
            byte result;
            switch(btDir)
            {
                case grobal2.DR_UP:
                    // 0
                    // 1
                    result = grobal2.DR_UPRIGHT;
                    break;
                case grobal2.DR_UPRIGHT:
                    // 1
                    // 2
                    result = grobal2.DR_RIGHT;
                    break;
                case grobal2.DR_RIGHT:
                    // 2
                    // 3
                    result = grobal2.DR_DOWNRIGHT;
                    break;
                case grobal2.DR_DOWNRIGHT:
                    // 3
                    // 4
                    result = grobal2.DR_DOWN;
                    break;
                case grobal2.DR_DOWN:
                    // 4
                    // 5
                    result = grobal2.DR_DOWNLEFT;
                    break;
                case grobal2.DR_DOWNLEFT:
                    // 5
                    // 6
                    result = grobal2.DR_LEFT;
                    break;
                case grobal2.DR_LEFT:
                    // 6
                    // 7
                    result = grobal2.DR_UPLEFT;
                    break;
                case grobal2.DR_UPLEFT:
                    // 7
                    // 0
                    result = grobal2.DR_UP;
                    break;
            }
            return result;
        }

        public bool GetPoint(ref int nX, ref int nY)
        {
            bool result;
            int I;
            int nMX;
            int nMY;
            int nC;
            int n10;
            int nIndex;
            int nPostion;
            TMapPoint MapPoint;
            TMapPoint MapPoint10;
            bool boFind;
            int nCurrX;
            int nCurrY;
            byte btDir;
            int Pt;
            int nX1;
            int nY1;
            int nX2;
            int nY2;
            int nStep;
            result = false;
            if (FPathType == TPathType.t_Dynamic)
            {
                m_nCurrX = nX;
                m_nCurrY = nY;
                //@ Unsupported property or method(C): 'm_btDirection'
                m_btDirection = ((object)FBaseObject).m_btDirection;
                for (I = 2; I >= 1; I-- )
                {
                    //@ Unsupported property or method(C): 'm_PEnvir'
                    //@ Unsupported property or method(B): 'GetNextPosition'
                    if (((object)FBaseObject).m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, I, nMX, nMY))
                    {
                        //@ Unsupported property or method(A): 'CanMove'
                        if (((object)FBaseObject).CanMove(nMX, nMY, false))
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
                    for (I = 2; I >= 1; I-- )
                    {
                        if (((object)FBaseObject).m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, I, nMX, nMY))
                        {
                            if (((object)FBaseObject).CanMove(nMX, nMY, false))
                            {
                                nX = nMX;
                                nY = nMY;
                                result = true;
                                return result;
                            }
                        }
                    }
                    nC ++;
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
                for (I = m_nPostion; I < m_PEnvir.m_PointList.Count; I ++ )
                {
                    Pt = (int)m_PEnvir.m_PointList[I];
                    nCurrX = LoWord(Pt);
                    nCurrY = HiWord(Pt);
                    nC = Math.Abs(nX - nCurrX) + Math.Abs(nY - nCurrY);
                    if (nC < n10)
                    {
                        n10 = nC;
                        nMX = nCurrX;
                        nMY = nCurrY;
                        nIndex = I;
                        m_nPostion = I;
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
                        for (I = m_nPostion + 1; I < m_PEnvir.m_PointList.Count; I ++ )
                        {
                            Pt = (int)m_PEnvir.m_PointList[I];
                            //@ Undeclared identifier(3): 'LoWord'
                            nCurrX = LoWord(Pt);
                            //@ Unsupported function or procedure: 'HiWord'
                            nCurrY = HiWord(Pt);
                            if (nStep == 0)
                            {
                                //@ Undeclared identifier(3): 'GetNextDirection'
                                btDir = GetNextDirection(nX, nY, nCurrX, nCurrY);
                                nMX = nCurrX;
                                nMY = nCurrY;
                                m_nPostion = I;
                            }
                            else
                            {
                                //@ Undeclared identifier(3): 'GetNextDirection'
                                if (GetNextDirection(nMX, nMY, nCurrX, nCurrY) == btDir)
                                {
                                    nMX = nCurrX;
                                    nMY = nCurrY;
                                    m_nPostion = I;
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
                    //@ Unsupported property or method(A): 'CanRun'
                    if (!((object)FBaseObject).CanRun(nX, nY, nMX, nMY, false))
                    {
                        for (I = m_nPostion + 1; I < m_PEnvir.m_PointList.Count; I ++ )
                        {
                            Pt = (int)m_PEnvir.m_PointList[I];
                            //@ Undeclared identifier(3): 'LoWord'
                            nCurrX = LoWord(Pt);
                            //@ Unsupported function or procedure: 'HiWord'
                            nCurrY = HiWord(Pt);
                            m_nPostion = I;
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
            byte result;
            switch(btDir)
            {
                case grobal2.DR_UP:
                    // 0
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 1
                            result = grobal2.DR_UPRIGHT;
                            break;
                        case 1:
                            // 2
                            result = grobal2.DR_RIGHT;
                            break;
                        case 2:
                            // 7
                            result = grobal2.DR_UPLEFT;
                            break;
                        case 3:
                            // 6
                            result = grobal2.DR_LEFT;
                            break;
                    }
                    break;
                case grobal2.DR_UPRIGHT:
                    // 1
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 2
                            result = grobal2.DR_RIGHT;
                            break;
                        case 1:
                            // 3
                            result = grobal2.DR_DOWNRIGHT;
                            break;
                        case 2:
                            // 0
                            result = grobal2.DR_UP;
                            break;
                        case 3:
                            // 7
                            result = grobal2.DR_UPLEFT;
                            break;
                    }
                    break;
                case grobal2.DR_RIGHT:
                    // 2
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 3
                            result = grobal2.DR_DOWNRIGHT;
                            break;
                        case 1:
                            // 4
                            result = grobal2.DR_DOWN;
                            break;
                        case 2:
                            // 1
                            result = grobal2.DR_UPRIGHT;
                            break;
                        case 3:
                            // 0
                            result = grobal2.DR_UP;
                            break;
                    }
                    break;
                case grobal2.DR_DOWNRIGHT:
                    // 3
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 4
                            result = grobal2.DR_DOWN;
                            break;
                        case 1:
                            // 5
                            result = grobal2.DR_DOWNLEFT;
                            break;
                        case 2:
                            // 2
                            result = grobal2.DR_RIGHT;
                            break;
                        case 3:
                            // 1
                            result = grobal2.DR_UPRIGHT;
                            break;
                    }
                    break;
                case grobal2.DR_DOWN:
                    // 4
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 5
                            result = grobal2.DR_DOWNLEFT;
                            break;
                        case 1:
                            // 6
                            result = grobal2.DR_LEFT;
                            break;
                        case 2:
                            // 3
                            result = grobal2.DR_DOWNRIGHT;
                            break;
                        case 3:
                            // 2
                            result = grobal2.DR_RIGHT;
                            break;
                    }
                    break;
                case grobal2.DR_DOWNLEFT:
                    // 5
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 6
                            result = grobal2.DR_LEFT;
                            break;
                        case 1:
                            // 7
                            result = grobal2.DR_UPLEFT;
                            break;
                        case 2:
                            // 4
                            result = grobal2.DR_DOWN;
                            break;
                        case 3:
                            // 3
                            result = grobal2.DR_DOWNRIGHT;
                            break;
                    }
                    break;
                case grobal2.DR_LEFT:
                    // 6
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 7
                            result = grobal2.DR_UPLEFT;
                            break;
                        case 1:
                            // 0
                            result = grobal2.DR_UP;
                            break;
                        case 2:
                            // 5
                            result = grobal2.DR_DOWNLEFT;
                            break;
                        case 3:
                            // 4
                            result = grobal2.DR_DOWN;
                            break;
                    }
                    break;
                case grobal2.DR_UPLEFT:
                    // 7
                    switch(new System.Random(4).Next())
                    {
                        case 0:
                            // 0
                            result = grobal2.DR_UP;
                            break;
                        case 1:
                            // 1
                            result = grobal2.DR_UPRIGHT;
                            break;
                        case 2:
                            // 6
                            result = grobal2.DR_LEFT;
                            break;
                        case 3:
                            // 5
                            result = grobal2.DR_DOWNLEFT;
                            break;
                    }
                    break;
            }
            return result;
        }

        public bool GetPoint1(ref int nX, ref int nY)
        {
            bool result;
            int I;
            int nMX;
            int nMY;
            int nC;
            int n10;
            int nIndex;
            int nPostion;
            TMapPoint MapPoint;
            TMapPoint MapPoint10;
            bool boFind;
            int nCurrX;
            int nCurrY;
            byte btDir;
            int Pt;
            int nX1;
            int nY1;
            int nX2;
            int nY2;
            int nStep;
            result = false;
            if (FPathType == TPathType.t_Dynamic)
            {
                m_nCurrX = nX;
                m_nCurrY = nY;
                nC = 0;
                btDir = ((TPlayObject)FBaseObject).m_btDirection;
                while (true)
                {
                    btDir = GetPoint1_GetNextDir(btDir);
                    for (I = 2; I >= 1; I-- )
                    {
                        if (((TPlayObject)FBaseObject).m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, I, ref nMX,ref nMY))
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
                    nC ++;
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
                //@ Unsupported property or method(C): 'm_PEnvir'
                if (((TPlayObject)FBaseObject).m_PEnvir != m_PEnvir)
                {
                    //@ Unsupported property or method(C): 'm_PEnvir'
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
                for (I = m_nPostion; I < m_PEnvir.m_PointList.Count; I ++ )
                {
                    Pt = (int)m_PEnvir.m_PointList[I];
                    //@ Undeclared identifier(3): 'LoWord'
                    nCurrX = HUtil32.LoWord(Pt);
                    //@ Unsupported function or procedure: 'HiWord'
                    nCurrY = HUtil32.HiWord(Pt);
                    nC = Math.Abs(nX - nCurrX) + Math.Abs(nY - nCurrY);
                    if (nC < n10)
                    {
                        n10 = nC;
                        nMX = nCurrX;
                        nMY = nCurrY;
                        nIndex = I;
                        m_nPostion = I;
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
                        for (I = m_nPostion + 1; I < m_PEnvir.m_PointList.Count; I ++ )
                        {
                            Pt = (int)m_PEnvir.m_PointList[I];
                            nCurrX = HUtil32.LoWord(Pt);
                            nCurrY =HUtil32. HiWord(Pt);
                            if (nStep == 0)
                            {
                                btDir = GetNextDirection(nX, nY, nCurrX, nCurrY);
                                nMX = nCurrX;
                                nMY = nCurrY;
                                m_nPostion = I;
                            }
                            else
                            {
                                if (GetNextDirection(nMX, nMY, nCurrX, nCurrY) == btDir)
                                {
                                    nMX = nCurrX;
                                    nMY = nCurrY;
                                    m_nPostion = I;
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
                        for (I = m_nPostion + 1; I < m_PEnvir.m_PointList.Count; I ++ )
                        {
                            Pt = (int)m_PEnvir.m_PointList[I];
                            nCurrX = HUtil32.LoWord(Pt);
                            nCurrY = HUtil32.HiWord(Pt);
                            m_nPostion = I;
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

