﻿namespace GameSrv.Maps {
    /// <summary>
    /// 地图上的对象
    /// </summary>
    public record struct CellObject {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int CellObjId;
        /// <summary>
        /// Cell类型
        /// </summary>
        public CellType CellType;
        /// <summary>
        /// 精灵对象（玩家 怪物 商人）
        /// </summary>
        public bool ActorObject;
        /// <summary>
        /// 添加时间
        /// </summary>
        public int AddTime;
    }
}