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

namespace M2Server.Maps.AutoPath {
    // 路径数组
    public class PathcellSuccess {
        // 路径图元
        public int Distance;
        // 离起点的距离
        public int Direction;
    }

    public struct WaveCell {
        // 路线点
        public short X;
        public short Y;
        public int Cost;
        public int Direction;
    }

    public class Wave {
        public WaveCell Item => GetItem();
        public int MinCost => _fMinCost;

        private WaveCell[] _fData = new WaveCell[0];
        private int _fPos;
        private int _fCount;
        private int _fMinCost;

        public Wave() {
            Clear();
        }

        ~Wave() {
            _fData = null;
        }

        private WaveCell GetItem() {
            return _fData[_fPos];
        }

        public void Add(short newX, short newY, int newCost, int newDirection) {
            if (_fCount >= _fData.Length) {
                _fData = new WaveCell[_fData.Length + 30];
            }
            _fData[_fCount].X = newX;
            _fData[_fCount].Y = newY;
            _fData[_fCount].Cost = newCost;
            _fData[_fCount].Direction = newDirection;
            if (newCost < _fMinCost) {
                _fMinCost = newCost;
            }
            _fCount++;
        }

        public void Clear() {
            _fPos = 0;
            _fCount = 0;
            _fMinCost = int.MaxValue;
        }

        public bool Start() {
            _fPos = 0;
            return _fCount > 0; ;
        }

        public bool Next() {
            _fPos++;
            return _fPos < _fCount;
        }
    }

    public delegate int GetCostFunc(int x, int y, int direction, int pathWidth);
}