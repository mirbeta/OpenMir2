using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RunGate
{
    /// <summary>
    /// 网关服务
    /// </summary>
    public class GateService
    {
        private readonly ConcurrentDictionary<string, UserClientService> _gateList;

        public GateService()
        {
            _gateList = new ConcurrentDictionary<string, UserClientService>();
        }

        public void Add(string name,UserClientService userClientService)
        {
            _gateList.TryAdd(name, userClientService);
        }

        public TSessionInfo[] GetGateSessionInfo(string name)
        {
            if (_gateList.ContainsKey(name))
            {
                return _gateList[name].GetSession();
            }
        }
    }
}