using System.Collections.Generic;
using OpenMir2.Packets.ServerPackets;

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
        void Add(string chrName, CharacterDataInfo playerData);

        /// <summary>
        /// 从缓存取出角色数据
        /// </summary>
        /// <returns></returns>
        CharacterDataInfo Get(string chrName, out bool exist);

        /// <summary>
        /// 从缓存删除角色数据
        /// </summary>
        /// <param name="chrName"></param>
        void Delete(string chrName);

        /// <summary>
        /// 从缓存取出所有角色数据
        /// </summary>
        /// <returns></returns>
        IEnumerator<CharacterDataInfo> QueryCacheData();
    }
}