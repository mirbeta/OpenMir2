using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage.Impl
{
    public class MemoryStorageServive : IMemoryStorageServive
    {
        private readonly ConcurrentDictionary<string, HumDataInfo> _cacheMap = new ConcurrentDictionary<string, HumDataInfo>(StringComparer.OrdinalIgnoreCase);

        public void Add(string sChrName, HumDataInfo humDataInfo)
        {
            if (_cacheMap.ContainsKey(sChrName)) //缓存存在则直接直接替换
            {
                _cacheMap[sChrName] = humDataInfo;
            }
        }

        public HumDataInfo Get(string sChrName)
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

        public IEnumerable<HumDataInfo> GetAll()
        {
            return _cacheMap.Values.ToList();
        }
    }
}