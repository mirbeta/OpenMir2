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

        public void Add(string name, UserClientService userClientService)
        {
            if (_gateList.ContainsKey(name))
            {
                return;
            }
            _gateList.TryAdd(name, userClientService);
        }

        public UserClientService GetClientService()
        {
            //TODO 根据配置文件有三种模式  默认随机
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关

            List<UserClientService> userList = new List<UserClientService>(_gateList.Values);
            var random = new System.Random().Next(userList.Count);
            return userList[random];
        }
    }
}