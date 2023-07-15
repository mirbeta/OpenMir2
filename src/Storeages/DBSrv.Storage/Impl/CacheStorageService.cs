using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemModule.Packets.ServerPackets;

namespace DBSrv.Storage.Impl
{
    public class CacheStorageService : ICacheStorage
    {
        private readonly ConcurrentDictionary<string, CharacterDataInfo> _cacheMap = new ConcurrentDictionary<string, CharacterDataInfo>(StringComparer.OrdinalIgnoreCase);

        public void Add(string sChrName, CharacterDataInfo humDataInfo)
        {
            if (_cacheMap.ContainsKey(sChrName)) //缓存存在则直接直接替换
            {
                _cacheMap[sChrName] = humDataInfo;
            }
        }

        public CharacterDataInfo Get(string sChrName, out bool exist)
        {
            exist = false;
            if (_cacheMap.TryGetValue(sChrName, out var humDataInfo))
            {
                exist = true;
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

        public IEnumerator<CharacterDataInfo> QueryCacheData()
        {
            return _cacheMap.Values.GetEnumerator();
        }
    }
}