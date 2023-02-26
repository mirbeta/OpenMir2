using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemModule.Packets.ServerPackets;

namespace DBSrv.Storage.Impl
{
    public class CacheStorageService : ICacheStorage
    {
        private readonly ConcurrentDictionary<string, PlayerDataInfo> _cacheMap = new ConcurrentDictionary<string, PlayerDataInfo>(StringComparer.OrdinalIgnoreCase);

        public void Add(string sChrName, PlayerDataInfo humDataInfo)
        {
            if (_cacheMap.ContainsKey(sChrName)) //缓存存在则直接直接替换
            {
                _cacheMap[sChrName] = humDataInfo;
            }
        }

        public PlayerDataInfo Get(string sChrName)
        {
            if (_cacheMap.TryGetValue(sChrName, out var humDataInfo))
            {
                return humDataInfo;
            }
            return null;
        }

        public void Delete(string ChrName)
        {
            if (_cacheMap.TryRemove(ChrName, out var _))
            {

            }
        }

        public IEnumerator<PlayerDataInfo> QueryCacheData()
        {
            return _cacheMap.Values.GetEnumerator();
        }
    }
}