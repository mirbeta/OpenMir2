using System.Drawing;
using System.Runtime.InteropServices;

namespace BotSrv.Maps
{
    public class PathMap
    {
        public static Point[] g_MapPath;
        public const int SCALE = 4;
        protected static string MAP_BASEPATH = GetMapPath();
        public static int[] TerrainParams = new int[2] { 4, -1 };
        public MapHeader m_MapHeader;
        public CellParams[,] m_MapData;
        public MapInfo[] m_MapBuf;
        protected int m_nPathWidth = 0;
        protected PathcellSuccess[,] m_PathMapArray;

        public PathMap()
        {
            m_MapBuf = null;
            m_nPathWidth = 0;
        }

        public static string GetMapPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "/Volumes/Data/Mirserver/Mir200/Map/";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return "/opt/MirServer/Mir200/Map/";
            }
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "D:/Mirserver/Mir200/Map/" : string.Empty;
        }

        /// <summary>
        /// 从TPathMap中找出 TPath
        /// </summary>
        protected Point[] FindPathOnMap(int X, int Y)
        {
            if ((X >= m_MapHeader.wWidth) || (Y >= m_MapHeader.wHeight))
            {
                return null;
            }
            if (m_PathMapArray[Y, X].Distance < 0)
            {
                return null;
            }
            var result = new Point[m_PathMapArray[Y, X].Distance + 1];
            while (m_PathMapArray[Y, X].Distance > 0)
            {
                result[m_PathMapArray[Y, X].Distance] = new Point(X, Y);
                var direction = m_PathMapArray[Y, X].Direction;
                X = X - DirToDX(direction);
                Y = Y - DirToDY(direction);
            }
            result[0] = new Point(X, Y);
            return result;
        }

        private int DirToDX(int Direction)
        {
            int result;
            switch (Direction)
            {
                case 0:
                case 4:
                    result = 0;
                    break;
                case 1:
                case 2:
                case 3:
                    result = 1;
                    break;
                default:
                    result = -1;
                    break;
            }
            return result;
        }

        public int DirToDY(int Direction)
        {
            int result;
            switch (Direction)
            {
                case 2:
                case 6:
                    result = 0;
                    break;
                case 3:
                case 4:
                case 5:
                    result = 1;
                    break;
                default:
                    result = -1;
                    break;
            }
            return result;
        }

        protected int GetCost(int X, int Y, int Direction)
        {
            int result = 0;
            Direction = Direction & 7;
            //if ((X < 0) || (X >= m_MapHeader.wWidth) || (Y < 0) || (Y >= m_MapHeader.wHeight))
            //{
            //    result = -1;
            //}
            //else
            //{
            //    result = TerrainParams[m_MapData[X, Y].TerrainType || m_MapData[X, Y].TCellActor];
            //    if ((X < m_MapHeader.wWidth - m_nPathWidth) && (X > m_nPathWidth) && (Y < m_MapHeader.wHeight - m_nPathWidth) && (Y > m_nPathWidth))
            //    {
            //        Cost = TerrainParams[m_MapData[X - m_nPathWidth, Y].TerrainType || m_MapData[X - m_nPathWidth, Y].TCellActor] + TerrainParams[m_MapData[X + m_nPathWidth, Y].TerrainType || m_MapData[X + m_nPathWidth, Y].TCellActor] + TerrainParams[m_MapData[X, Y - m_nPathWidth].TerrainType || m_MapData[X, Y - m_nPathWidth].TCellActor] + TerrainParams[m_MapData[X, Y + m_nPathWidth].TerrainType || m_MapData[X, Y + m_nPathWidth].TCellActor];
            //        if (Cost < 4 * TerrainParams[false])
            //        {
            //            result = -1;
            //        }
            //    }
            //    if (((Direction & 1) == 1) && (result > 0))
            //    {
            //        result = result + (result >> 1);
            //    }
            //}
            return result;
        }

        public void FillPathMap_TestNeighbours()
        {
            //for (d = 0; d <= 7; d++)
            //{
            //    X = OldWave.item.X + DirToDX(d);
            //    Y = OldWave.item.Y + DirToDY(d);
            //    c = GetCost(X, Y, d);
            //    if ((c >= 0) && (result[Y, X].Distance < 0))
            //    {
            //        NewWave.Add(X, Y, c, d);
            //    }
            //}
        }

        public void FillPathMap_ExchangeWaves()
        {
            //TWave w = OldWave;
            //OldWave = NewWave;
            //NewWave = w;
            //NewWave.Clear();
        }

        protected PathcellSuccess[,] FillPathMap(int X1, int Y1, int X2, int Y2)
        {
            WaveCell i;
            var Finished = (X1 == X2) && (Y1 == Y2);
            if (Finished)
            {
                return null;
            }
            var result = new PathcellSuccess[m_MapHeader.wHeight, m_MapHeader.wWidth];
            for (var Y = 0; Y < (m_MapHeader.wHeight - 1); Y++)
            {
                for (var X = 0; X < (m_MapHeader.wWidth - 1); X++)
                {
                    result[Y, X].Distance = -1;
                }
            }
            var OldWave = new TWave();
            var NewWave = new TWave();
            result[Y1, X1].Distance = 0;
            OldWave.Add(X1, Y1, 0, 0);
            FillPathMap_TestNeighbours();
            while (!Finished)
            {
                FillPathMap_ExchangeWaves();
                if (!OldWave.start())
                {
                    break;
                }
                do
                {
                    i = OldWave.item;
                    i.Cost = i.Cost - OldWave.MinCost;
                    if (i.Cost > 0)
                    {
                        NewWave.Add(i.X, i.Y, i.Cost, i.Direction);
                    }
                    else
                    {
                        if (result[i.Y, i.X].Distance >= 0)
                        {
                            continue;
                        }
                        result[i.Y, i.X].Distance = result[i.Y - DirToDY(i.Direction), i.X - DirToDX(i.Direction)].Distance + 1;
                        result[i.Y, i.X].Direction = i.Direction;
                        Finished = (i.X == X2) && (i.Y == Y2);
                        if (Finished)
                        {
                            break;
                        }
                        FillPathMap_TestNeighbours();
                    }
                } while (!!OldWave.Next());
            }

            return result;
        }
    }
}

