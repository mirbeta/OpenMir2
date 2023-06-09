using System.Runtime.InteropServices;

namespace SystemModule
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