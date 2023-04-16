using GameSrv.Actor;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Maps
{
    public record struct MapDoor
    {
        public int DoorId;
        public short nX;
        public short nY;
        public DoorStatus Status;
        public int n08;

        public MapDoor()
        {
            DoorId = M2Share.ActorMgr.GetNextIdentity();
        }
    }

    public record struct MapItem {
        /// <summary>
        /// 物品唯一ID
        /// </summary>
        public int ItemId;
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
        /// 掉落的时间
        /// </summary>
        public int CanPickUpTick;
        public UserItem UserItem;

        public MapItem() {
            this.ItemId = M2Share.ActorMgr.GetNextIdentity();
        }
    }

    /// <summary>
    /// 地图连接对象
    /// </summary>
    public record struct MapRouteItem {
        public int RouteId;
        public Envirnoment Envir;
        public short X;
        public short Y;
        public bool Flag;
    }

    /// <summary>
    /// 地图物品
    /// </summary>
    public class VisibleMapItem {
        public short nX;
        public short nY;
        public MapItem MapItem;
        public string sName;
        public ushort wLooks;
        public VisibleFlag VisibleFlag;
    }

    public enum CellType : byte {
        /// <summary>
        /// 事件
        /// 挖坑 烟花 等
        /// </summary>
        Event = 1,
        /// <summary>
        /// 玩家
        /// </summary>
        Play = 2,
        /// <summary>
        /// 物品
        /// </summary>
        Item = 3,
        /// <summary>
        /// 地图链接
        /// </summary>
        MapRoute = 4,
        /// <summary>
        /// 地图事件
        /// </summary>
        MapEvent = 5,
        /// <summary>
        /// 门
        /// </summary>
        Door = 6,
        /// <summary>
        /// 未知
        /// </summary>
        Roon = 7,
        /// <summary>
        /// 游戏商人
        /// </summary>
        Merchant = 8,
        /// <summary>
        /// 怪物
        /// </summary>
        Monster = 9,
        /// <summary>
        /// 下属
        /// </summary>
        SavleMonster = 10,
        /// <summary>
        /// 沙巴克城门
        /// </summary>
        CastleDoor = 11
    }

    public enum CellAttribute : byte {
        /// <summary>
        /// 可以走动
        /// </summary>
        Walk = 0,
        HighWall = 1,
        /// <summary>
        /// 不能走动
        /// </summary>
        LowWall = 2
    }

    public record struct PointInfo {
        public short nX;
        public short nY;

        public PointInfo(short x, short y) {
            nX = x;
            nY = y;
        }
    }

    public record struct TRect {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public TRect(int left, int top, int right, int bottom) {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}