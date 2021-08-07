using System;
using System.Collections.Generic;

namespace RunGate
{
    public class RunGateClient
    {
        /// <summary>
        /// 点击最多链接10个客户端(RunGate->M2)
        /// </summary>
        private readonly UserClientService[] _gateClient = new UserClientService[10];

        public RunGateClient( )
        {
        }

        public void LoadConfig()
        {
            if (GateShare.Conf == null)
            {
                Console.WriteLine("获取网关配置文件错误.");
                return;
            }
            GateShare.ServerCount = GateShare.Conf.ReadInteger<int>("Servers", "ServerCount", GateShare.ServerCount);
            if (GateShare.ServerCount > _gateClient.Length)
            {
                GateShare.ServerCount = _gateClient.Length;//最大不能超过10个网关
            }
            var serverAddr = string.Empty;
            var serverPort = 0;
            for (var i = 0; i < GateShare.ServerCount; i++)
            {
                serverAddr = GateShare.Conf.GetString("Servers", $"ServerAddr{i}");
                serverPort = GateShare.Conf.ReadInteger<int>("Servers", $"ServerPort{i}", 80);
                if (string.IsNullOrEmpty(serverAddr) || serverPort == 80)
                {
                    Console.WriteLine($"网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _gateClient[i] = new UserClientService(serverAddr, serverPort);
            }
        }

        public void Start()
        {
            for (var i = 0; i < _gateClient.Length; i++)
            {
                if (_gateClient[i] == null)
                {
                    continue;
                }
                _gateClient[i].Start();
                _gateClient[i].RestSessionArray();
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _gateClient.Length; i++)
            {
                if (_gateClient[i] == null)
                {
                    continue;
                }
                _gateClient[i].Stop();
            }
        }

        public IList<UserClientService> GetAllClient()
        {
            return _gateClient;
        }
    }
}