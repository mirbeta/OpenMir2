using GameSvr.Actor;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Maps
{
    public class MapItem : EntityId
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 物品外观
        /// </summary>
        public ushort Looks;
        public byte AniCount;
        public int Reserved;
        /// <summary>
        /// 数量
        /// </summary>
        public int Count;
        /// <summary>
        /// 谁掉落的
        /// </summary>
        public int DropBaseObject;
        /// <summary>
        /// 物品谁可以捡起
        /// </summary>
        public int OfBaseObject;
        /// <summary>
        /// 可以拾取的时间
        /// </summary>
        public int CanPickUpTick;
        public UserItem UserItem;
    }
    
    /// <summary>
    /// 地图连接
    /// </summary>
    public class GateObject : EntityId
    {
        public Envirnoment DEnvir;
        public short nDMapX;
        public short nDMapY;
        public bool boFlag;
    }
    
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
    /// 地图物品
    /// </summary>
    public class VisibleMapItem
    {
        public int nX;
        public int nY;
        public MapItem MapItem;
        public string sName;
        public ushort wLooks;
        public VisibleFlag VisibleFlag;
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