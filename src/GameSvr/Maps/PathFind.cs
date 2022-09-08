// (******************************************************************************
// 关于TLegendMap(位于PathFind.pas)的用法
// 1、FLegendMap:=TLegendMap.Create;
// FLegendMap.LoadMap('mapfile')
// --成功返回后生成地图数据FLegendMap.MapData[i, j]:TMapData
// FLegendMap.SetStartPos(StartX, StartY,PathSpace)
// Path:=FLegendMap.FindPath(StopX, StopY)
// 2、FLegendMap:=TLegendMap.Create;
// FLegendMap.LoadMap('mapfile')
// Path:=FLegendMap.FindPath(StartX,StartY,StopX, StopY,PathSpace)
// 
// 其中
// Path为TPath = array of TPoint 为nil时表示不能到达
// 第一个值为起点，最后一个值为终点
// High(Path)即路径需要的步数
// 
// PathSpace为离开障碍物多少个象素
// ******************************************************************************)
// (*****************************************************************************
// 关于TPathMap的特点
// 1、不需要传递地图数据，节省内存的频繁拷贝
// 2、可自定义估价函数，根据自己需要产生不同路径
// 
// 关于TPathMap的用法
// 1、定义估价函数MovingCost(X, Y, Direction: Integer)
// 只需根据自定义的地图格式编写)
// 2、FPathMap:=TPathMap.Create;
// FPathMap.MakePathMap(MapHeader.width, MapHeader.height, StartX, StartY,MovingCost);
// Path:=FPathMap.FindPathOnMap( EndX, EndY)
// 其中Path为TPath = array of TPoint;
// 
// 如果不喜欢在TPathMap外部定义估价函数，可继承TPathMap，
// 将地图数据的读取和估价函数封装成一个类使用。
// *******************************************************************************)

namespace GameSvr.Maps
{
    // 路径数组
    public class Pathcellsuccess
    {
        // 路径图元
        public int Distance;
        // 离起点的距离
        public int Direction;
    }

    public class PathMap
    {
        protected Pathcellsuccess[,] PathMapArray;
        /// <summary>
        /// 地图高(X最大值)
        /// </summary>
        protected int Height;
        /// <summary>
        /// 地图宽(Y最大值)
        /// </summary>        
        protected int Width;
        private readonly GetCostFunc _getCostFunc;
        protected TRect ClientRect;
        /// <summary>
        /// 寻找范围
        /// </summary>
        private int _scopeValue;
        /// <summary>
        /// 开始寻路
        /// </summary>
        protected bool StartFind;

        public PathMap() : base()
        {
            _scopeValue = 1000; // 寻路范围
            _getCostFunc = null;
        }

        // *************************************************************
        // 方向编号转为X方向符号
        // 7  0  1
        // 6  X  2
        // 5  4  3
        // *************************************************************
        private short DirToDx(int direction)
        {
            short result;
            switch (direction)
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

        private short DirToDy(int direction)
        {
            short result;
            switch (direction)
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

        // *************************************************************
        // 从TPathMap中找出 TPath
        // *************************************************************
        protected PointInfo[] FindPathOnMap(short x, short y, bool run)
        {
            int nCount = 0;
            short nX = LoaclX(x);
            short nY = LoaclY(y);
            if ((nX < 0) || (nY < 0) || (nX >= ClientRect.Right - ClientRect.Left) || (nY >= ClientRect.Bottom - ClientRect.Top))
            {
                return null;
            }
            if ((PathMapArray.Length <= 0) || (PathMapArray[nY, nX].Distance < 0))
            {
                return null;
            }
            var result = new PointInfo[PathMapArray[nY, nX].Distance + 1];
            while (PathMapArray[nY, nX].Distance > 0)
            {
                if (!StartFind)
                {
                    break;
                }
                result[PathMapArray[nY, nX].Distance] = new PointInfo(nX, nY);
                var direction = PathMapArray[nY, nX].Direction;
                nX = (short)(nX - DirToDx(direction));
                nY = (short)(nY - DirToDy(direction));
                nCount++;
            }
            result[0] = new PointInfo(nX, nY);
            if (run)
            {
                result = WalkToRun(result);
            }
            return result;
        }

        // 把WALK合并成RUN
        public byte WalkToRun_GetNextDirection(int sx, int sy, int dx, int dy)
        {
            byte result;
            int flagx;
            int flagy;
            const int drUp = 0;
            const int drUpright = 1;
            const int drRight = 2;
            const int drDownright = 3;
            const int drDown = 4;
            const int drDownleft = 5;
            const int drLeft = 6;
            const int drUpleft = 7;
            result = drDown;
            if (sx < dx)
            {
                flagx = 1;
            }
            else if (sx == dx)
            {
                flagx = 0;
            }
            else
            {
                flagx = -1;
            }
            if (Math.Abs(sy - dy) > 2)
            {
                if ((sx >= dx - 1) && (sx <= dx + 1))
                {
                    flagx = 0;
                }
            }
            if (sy < dy)
            {
                flagy = 1;
            }
            else if (sy == dy)
            {
                flagy = 0;
            }
            else
            {
                flagy = -1;
            }
            if (Math.Abs(sx - dx) > 2)
            {
                if ((sy > dy - 1) && (sy <= dy + 1))
                {
                    flagy = 0;
                }
            }
            if ((flagx == 0) && (flagy == -1))
            {
                result = drUp;
            }
            if ((flagx == 1) && (flagy == -1))
            {
                result = drUpright;
            }
            if ((flagx == 1) && (flagy == 0))
            {
                result = drRight;
            }
            if ((flagx == 1) && (flagy == 1))
            {
                result = drDownright;
            }
            if ((flagx == 0) && (flagy == 1))
            {
                result = drDown;
            }
            if ((flagx == -1) && (flagy == 1))
            {
                result = drDownleft;
            }
            if ((flagx == -1) && (flagy == 0))
            {
                result = drLeft;
            }
            if ((flagx == -1) && (flagy == -1))
            {
                result = drUpleft;
            }
            return result;
        }

        private PointInfo[] WalkToRun(PointInfo[] path)
        {
            int I;
            PointInfo[] result = null;
            if ((path != null) && (path.Length > 1))
            {
                PointInfo[] walkPath = path;
                var nStep = 0;
                I = 0;
                while (true)
                {
                    if (!StartFind)
                    {
                        break;
                    }
                    if (I >= walkPath.Length)
                    {
                        break;
                    }
                    if (nStep >= 2)
                    {
                        int nDir1 = WalkToRun_GetNextDirection(walkPath[I - 2].nX, walkPath[I - 2].nX, walkPath[I - 1].nX, walkPath[I - 1].nX);
                        int nDir2 = WalkToRun_GetNextDirection(walkPath[I - 1].nX, walkPath[I - 1].nX, walkPath[I].nX, walkPath[I].nX);
                        if (nDir1 == nDir2)
                        {
                            walkPath[I - 1].nX = -1;
                            walkPath[I - 1].nX = -1;
                            nStep = 0;
                        }
                        else
                        {
                            // 需要转向不能合并
                            I -= 1;
                            nStep = 0;
                            continue;
                        }
                    }
                    nStep++;
                    I++;
                }
                var n01 = 0;
                for (I = 0; I < walkPath.Length; I++)
                {
                    if ((walkPath[I].nX != -1) && (walkPath[I].nX != -1))
                    {
                        n01++;
                        result = new PointInfo[n01];
                        result[n01 - 1] = walkPath[I];
                    }
                }
                return result;
            }
            if ((path != null) && (path.Length > 0))
            {
                result = new PointInfo[path.Length - 1];
                for (I = 0; I < path.Length; I++)
                {
                    result[I - 1] = path[I];
                }
            }
            else
            {
                result = null;
            }
            return result;
        }

        // 把WALK合并成RUN
        // *************************************************************
        // 寻路算法
        // X1,Y1为路径运算起点，X2，Y2为路径运算终点
        // *************************************************************
        public int MapX(int x)
        {
            return x + ClientRect.Left;
        }

        public int MapY(int y)
        {
            return y + ClientRect.Top;
        }

        public short LoaclX(short x)
        {
            return (short)(x - ClientRect.Left);
        }

        public short LoaclY(short y)
        {
            return (short)(y - ClientRect.Top);
        }

        public void GetClientRect(int x, int y)
        {
            ClientRect = new TRect(0, 0, Width, Height);
            // Bounds定义一个矩形
            if (Width > _scopeValue)
            {
                ClientRect.Left = Math.Max(0, x - _scopeValue / 2);
                ClientRect.Right = ClientRect.Left + Math.Min(Width, x + _scopeValue / 2);
            }
            if (Height > _scopeValue)
            {
                ClientRect.Top = Math.Max(0, y - _scopeValue / 2);
                ClientRect.Bottom = ClientRect.Top + Math.Min(Height, y + _scopeValue / 2);
            }
        }

        public void GetClientRect(int x1, int y1, int x2, int y2)
        {
            int x;
            int y;
            if (x1 > x2)
            {
                x = x2 + (x1 - x2) / 2;
            }
            else if (x1 < x2)
            {
                x = x1 + (x2 - x1) / 2;
            }
            else
            {
                x = x1;
            }
            if (y1 > y2)
            {
                y = y2 + (y1 - y2) / 2;
            }
            else if (y1 < y2)
            {
                y = y1 + (y2 - y1) / 2;
            }
            else
            {
                y = y1;
            }
            ClientRect = new TRect(0, 0, Width, Height);
            if (Width > _scopeValue)
            {
                ClientRect.Left = Math.Max(0, x - _scopeValue / 2);
                ClientRect.Right = ClientRect.Left + Math.Min(Width, x + _scopeValue / 2);
            }
            if (Height > _scopeValue)
            {
                ClientRect.Top = Math.Max(0, y - _scopeValue / 2);
                ClientRect.Bottom = ClientRect.Top + Math.Min(Height, y + _scopeValue / 2);
            }
        }

        /// <summary>
        /// 初始化PathMapArray
        /// </summary>
        /// <param name="result"></param>
        private void FillPathMap_PreparePathMap(ref Pathcellsuccess[,] result)
        {
            int nWidth = ClientRect.Right - ClientRect.Left;
            int nHeight = ClientRect.Bottom - ClientRect.Top;
            result = new Pathcellsuccess[nHeight, nWidth];
            for (var y = 0; y < nHeight; y++)
            {
                for (var x = 0; x < nWidth; x++)
                {
                    result[y, x] = new Pathcellsuccess();
                    result[y, x].Distance = -1;
                }
            }
        }

        // 计算相邻8个节点的权cost，并合法点加入NewWave(),并更新最小cost
        // 合法点是指非障碍物且Result[X，Y]中未访问的点
        private void FillPathMap_TestNeighbours(Wave oldWave, Wave newWave, ref Pathcellsuccess[,] result)
        {
            for (var i = 0; i < 8; i++)
            {
                var x = oldWave.Item.X + DirToDx(i);
                var y = oldWave.Item.Y + DirToDy(i);
                var c = GetCost(x, y, i);
                if ((c >= 0) && (result[y, x].Distance < 0))
                {
                    newWave.Add(x, y, c, i);
                }
            }
        }

        private void FillPathMap_ExchangeWaves(Wave oldWave, Wave newWave)
        {
            var w = oldWave;
            oldWave = newWave;
            newWave = w;
            newWave.Clear();
        }

        // *************************************************************
        // 寻路算法
        // X1,Y1为路径运算起点，X2，Y2为路径运算终点
        // *************************************************************
        protected Pathcellsuccess[,] FillPathMap(short x1, short y1, short x2, short y2)
        {
            Pathcellsuccess[,] result = null;
            Wave oldWave;
            Wave newWave;
            bool finished;
            WaveCell I;
            GetClientRect(x1, y1);
            short nX1 = LoaclX(x1);
            short nY1 = LoaclY(y1);
            short nX2 = LoaclX(x2);
            short nY2 = LoaclY(y2);
            if (x2 < 0)
            {
                nX2 = x2;
            }
            if (y2 < 0)
            {
                nY2 = y2;
            }
            if ((x2 >= 0) && (y2 >= 0))
            {
                if ((Math.Abs(nX1 - nX2) > (ClientRect.Right - ClientRect.Left)) || (Math.Abs(nY1 - nY2) > (ClientRect.Bottom - ClientRect.Top)))
                {
                    result = new Pathcellsuccess[0, 0];
                    return result;
                }
            }
            FillPathMap_PreparePathMap(ref result);
            // 初始化PathMapArray ,Distance:=-1
            oldWave = new Wave();
            newWave = new Wave();
            try
            {
                result[nY1, nX1].Distance = 0;// 起点Distance:=0
                oldWave.Add(nX1, nY1, 0, 0);// 将起点加入OldWave
                FillPathMap_TestNeighbours(oldWave, newWave, ref result);
                finished = (nX1 == nX2) && (nY1 == nY2);// 检验是否到达终点
                while (!finished)
                {
                    FillPathMap_ExchangeWaves(oldWave, newWave);
                    if (!StartFind)
                    {
                        break;
                    }
                    if (!oldWave.Start())
                    {
                        break;
                    }
                    do
                    {
                        if (!StartFind)
                        {
                            break;
                        }
                        I = oldWave.Item;
                        I.Cost = I.Cost - oldWave.MinCost;
                        // 如果大于MinCost
                        if (I.Cost > 0)
                        {
                            // 加入NewWave
                            // 更新Cost= cost-MinCost
                            newWave.Add(I.X, I.Y, I.Cost, I.Direction);
                        }
                        else
                        {
                            // 处理最小COST的点
                            if (result[I.Y, I.X].Distance >= 0)
                            {
                                continue;
                            }
                            result[I.Y, I.X].Distance = result[I.Y - DirToDy(I.Direction), I.X - DirToDx(I.Direction)].Distance + 1;
                            // 此点 Distance:=上一个点Distance+1
                            result[I.Y, I.X].Direction = I.Direction;
                            finished = (I.X == nX2) && (I.Y == nY2);
                            // 检验是否到达终点
                            if (finished)
                            {
                                break;
                            }
                            FillPathMap_TestNeighbours(oldWave, newWave, ref result);
                        }
                    } while (!!oldWave.Next());
                }
            }
            finally
            {

            }
            return result;
        }

        protected virtual int GetCost(int x, int y, int direction)
        {
            int result;
            direction = direction & 7;
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
            {
                result = -1;
            }
            else
            {
                result = _getCostFunc(x, y, direction, 0);
            }
            return result;
        }
    }

    public class FindPath : PathMap
    {
        public PointInfo[] Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }
        
        private PointInfo[] _path;
        private Envirnoment _pathEnvir;
        public int BeginX;
        public int BeginY;
        public int EndX;
        public int EndY;

        public FindPath() : base()
        {
            this.StartFind = false;
        }

        public void Stop()
        {
            this.StartFind = false;
            BeginX = -1;
            BeginY = -1;
            EndX = -1;
            EndY = -1;
            this.PathMapArray = new Pathcellsuccess[0, 0];
            this.PathMapArray = null;
        }

        public PointInfo[] Find(short stopX, short stopY, bool run)
        {
            EndX = stopX;
            EndY = stopY;
            return this.FindPathOnMap(stopX, stopY, run);
        }

        public PointInfo[] Find(Envirnoment envir, short startX, short startY, short stopX, short stopY, bool run)
        {
            this.Width = envir.Width;
            this.Height = envir.Height;
            BeginX = startX;
            BeginY = startY;
            EndX = stopX;
            EndY = stopY;
            _path = null;
            _pathEnvir = envir;
            this.StartFind = true;
            this.PathMapArray = this.FillPathMap(startX, startY, stopX, stopY);
            return this.FindPathOnMap(stopX, stopY, run);
        }

        public void SetStartPos(short startX, short startY)
        {
            BeginX = startX;
            BeginY = startY;
            this.PathMapArray = this.FillPathMap(startX, startY, -1, -1);
        }

        public int GetCost(short x, short y, int direction)
        {
            int result;
            int nX;
            int nY;
            if (_pathEnvir != null)
            {
                direction = direction & 7;
                if ((x < 0) || (x >= this.ClientRect.Right - this.ClientRect.Left) || (y < 0) || (y >= this.ClientRect.Bottom - this.ClientRect.Top))
                {
                    result = -1;
                }
                else
                {
                    nX = this.MapX(x);
                    nY = this.MapY(y);
                    if (_pathEnvir.CanWalkEx(nX, nY, false))
                    {
                        result = 4;
                    }
                    else
                    {
                        result = -1;
                    }
                    // 如果是斜方向,则COST增加
                    if (((direction & 1) == 1) && (result > 0))
                    {
                        result = result + (result >> 1); // 应为Result*sqt(2),此处近似为1.5
                    }
                }
            }
            else
            {
                result = -1;
            }
            return result;
        }
    }

    public struct WaveCell
    {
        // 路线点
        public int X;
        public int Y;
        public int Cost;
        public int Direction;
    }

    public class Wave
    {
        public WaveCell Item => GetItem();
        public int MinCost => _fMinCost;

        private WaveCell[] _fData = new WaveCell[0];
        private int _fPos;
        private int _fCount;
        private int _fMinCost;

        public Wave()
        {
            Clear();
        }

        ~Wave()
        {
            _fData = null;
        }

        private WaveCell GetItem()
        {
            return _fData[_fPos];
        }

        public void Add(int newX, int newY, int newCost, int newDirection)
        {
            if (_fCount >= _fData.Length)
            {
                _fData = new WaveCell[_fData.Length + 30];
            }
            _fData[_fCount].X = newX;
            _fData[_fCount].Y = newY;
            _fData[_fCount].Cost = newCost;
            _fData[_fCount].Direction = newDirection;
            if (newCost < _fMinCost)
            {
                _fMinCost = newCost;
            }
            _fCount++;
        }

        public void Clear()
        {
            _fPos = 0;
            _fCount = 0;
            _fMinCost = int.MaxValue;
        }

        public bool Start()
        {
            _fPos = 0;
            return _fCount > 0; ;
        }

        public bool Next()
        {
            _fPos++;
            return _fPos < _fCount;
        }
    }

    public delegate int GetCostFunc(int x, int y, int direction, int pathWidth);
}