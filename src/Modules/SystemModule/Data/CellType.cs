namespace SystemModule.Data
{
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
}
