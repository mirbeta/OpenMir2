using System;

namespace RunGate
{
    public class RunGateClient
    {
        /// <summary>
        /// 点击最多链接10个客户端(RunGate->M2)
        /// </summary>
        private readonly UserClientService[] _gateClient = new UserClientService[10];

        private readonly GateService _gateService;

        public RunGateClient(GateService gateService)
        {
            _gateService = gateService;
        }

        public void LoadConfig()
        {
            if (GateShare.Conf == null)
            {
                Console.WriteLine("获取网关配置文件错误.");
                return;
            }
            GateShare.ServerCount = GateShare.Conf.ReadInteger<int>("Servers", "ServerCount", GateShare.ServerAddr);
            if (GateShare.ServerCount > _gateClient.Length)
            {
                GateShare.ServerCount = _gateClient.Length;//最大不能超过10个网关
            }
            var serverAddr = string.Empty;
            var serverPort = 0;
            for (int i = 0; i < _gateClient.Length; i++)
            {
                serverAddr = GateShare.Conf.GetString("Servers", $"ServerAddr{i}");
                serverPort = GateShare.Conf.ReadInteger<int>("Servers", $"ServerPort{i}", 80);
                if (string.IsNullOrEmpty(serverAddr) || serverPort==80)
                {
                    Console.WriteLine($"网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _gateClient[i] = new UserClientService(serverAddr, serverPort);

                _gateService.Add(serverAddr, _gateClient[i]);
            }
        }

        public void Start()
        {
            for (int i = 0; i < _gateClient.Length; i++)
            {
                _gateClient[i].Start();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < _gateClient.Length; i++)
            {
                _gateClient[i].Stop();
            }
        }
    }
}