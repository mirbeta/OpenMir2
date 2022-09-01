namespace GameSvr.Maps
{
    public class MapCellInfoConst
    {
        public static MapCellInfo LowWall = new MapCellInfo
        {
            Attribute = CellAttribute.LowWall
        };

        public static MapCellInfo HighWall = new MapCellInfo
        {
            Attribute = CellAttribute.HighWall
        };
    }

    /// <summary>
    /// 可见的地图物品
    /// </summary>
    public class VisibleMapItem
    {
        public int nX;
        public int nY;
        public MapItem MapItem;
        public string sName;
        public ushort wLooks;
        public int nVisibleFlag;
    }

    /// <summary>
    /// 地图上的对象
    /// </summary>
    public class CellObject
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int CellObjId;
        /// <summary>
        /// Cell类型
        /// </summary>
        public CellType CellType;
        /// <summary>
        /// 添加时间
        /// </summary>
        public int AddTime;
        /// <summary>
        /// 对象释放已释放
        /// </summary>
        public bool ObjectDispose;
    }

    public enum CellType : byte
    {
        EventObject = 1,
        MovingObject = 2,
        ItemObject = 3,
        GateObject = 4,
        MapEvent = 5,
        Door = 6,
        Roon = 7
    }

    public enum CellAttribute : byte
    {
        /// <summary>
        /// 可以走动
        /// </summary>
        Walk = 0,
        HighWall = 1,
        LowWall = 2
    }

    public class PointInfo
    {
        public short nX;
        public short nY;

        public PointInfo(short x, short y)
        {
            nX = x;
            nY = y;
        }
    }

    public class TRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public TRect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}