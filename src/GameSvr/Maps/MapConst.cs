using GameSvr.Actor;
using System.Diagnostics;
using SystemModule;
using SystemModule.Packets.ClientPackets;

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
        public ClientUserItem UserItem;

        public MapItem()
        {
            ActorId = M2Share.ActorMgr.Dequeue();
        }
    }

    /// <summary>
    /// 地图连接
    /// </summary>
    public class GateObject : EntityId
    {
        public Envirnoment Envir;
        public short nX;
        public short nY;
        public bool boFlag;
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

    /// <summary>
    /// 地图对象类型
    /// </summary>
    public enum CellObjectType : byte
    {
        /// <summary>
        /// 可以移动的对象
        /// </summary>
        MoveObject = 0
    }

    public enum CellType : byte
    {
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
        Route = 4,
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
        SavleMonster = 10
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