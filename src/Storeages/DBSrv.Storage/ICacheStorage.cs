using System.Collections.Generic;
using SystemModule.Packets.ServerPackets;

namespace DBSrv.Storage
{
    /// <summary>
    /// 本地内存存储接口
    /// </summary>
    public interface ICacheStorage
    {
        /// <summary>
        /// 添加角色数据到内存缓存
        /// </summary>
        void Add(string ChrName, PlayerDataInfo playerData);

        /// <summary>
        /// 从缓存取出角色数据
        /// </summary>
        /// <param name="ChrName"></param>
        /// <returns></returns>
        PlayerDataInfo Get(string ChrName);

        /// <summary>
        /// 从缓存删除角色数据
        /// </summary>
        /// <param name="ChrName"></param>
        void Delete(string ChrName);

        /// <summary>
        /// 从缓存取出所有角色数据
        /// </summary>
        /// <returns></returns>
        IList<PlayerDataInfo> QueryCacheData();
    }
}