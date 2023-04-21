using System.Runtime.InteropServices;

namespace M2Server
{
    /// <summary>
    /// 地图上的对象
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CellObject
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int CellObjId { get; set; }
        /// <summary>
        /// Cell类型
        /// </summary>
        public CellType CellType { get; set; }
        /// <summary>
        /// 精灵对象（玩家 怪物 商人）
        /// </summary>
        public bool ActorObject { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public int AddTime { get; set; }
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
}