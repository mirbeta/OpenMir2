using System.Collections.Generic;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage
{
    /// <summary>
    /// 本地内存存储接口
    /// </summary>
    public interface ICacheStorage
    {
        /// <summary>
        /// 添加角色数据到内存缓存
        /// </summary>
        void Add(string sChrName, HumDataInfo humDataInfo);

        /// <summary>
        /// 从缓存取出角色数据
        /// </summary>
        /// <param name="sChrName"></param>
        /// <returns></returns>
        HumDataInfo Get(string sChrName);

        /// <summary>
        /// 从缓存删除角色数据
        /// </summary>
        /// <param name="ChrName"></param>
        void Delete(string ChrName);

        /// <summary>
        /// 从缓存取出所有角色数据
        /// </summary>
        /// <returns></returns>
        IList<HumDataInfo> QueryCacheData();
    }
}