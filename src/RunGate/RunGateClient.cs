using System;
using System.Collections.Generic;
using System.Threading;
using SystemModule;

namespace RunGate
{
    public class RunGateClient
    {
        /// <summary>
        /// 点击最多链接10个客户端(RunGate->M2)
        /// </summary>
        private readonly ForwardClientService[] _gateClient = new ForwardClientService[10];
        private Timer clientTimer = null;
        
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
                serverPort = GateShare.Conf.ReadInteger<int>("Servers", $"ServerPort{i}", -1);
                if (string.IsNullOrEmpty(serverAddr) || serverPort == -1)
                {
                    Console.WriteLine($"网关配置文件服务器节点[ServerAddr{i}]配置获取失败.");
                    return;
                }
                _gateClient[i] = new ForwardClientService(serverAddr, serverPort);
                _gateClient[i].GateIdx = i;
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
            clientTimer = new Timer(CloseAllUser, null, 10000, 5000);
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

        public IList<ForwardClientService> GetAllClient()
        {
            return _gateClient;
        }

        private void CloseAllUser(object obj)
        {
            for (int i = 0; i < _gateClient.Length; i++)
            {
                if (_gateClient[i] == null)
                {
                    continue;
                }
                if (_gateClient[i].SessionArray == null)
                {
                    continue;
                }
                for (int j = 0; j < _gateClient[i].SessionArray.Length; j++)
                {
                    var session = _gateClient[i].SessionArray[j];
                    if (session == null)
                    {
                        continue;
                    }
                    if (session.Socket == null)
                    {
                        continue;
                    }
                    if (GateShare.UserSessions[session.SocketId] == null)
                    {
                        continue;
                    }
                    var userSession = GateShare.UserSessions[session.SocketId].GetGameSpeed();
                    if ((HUtil32.GetTickCount() - userSession.dwGameTick) > 600000)
                    {
                        userSession.dwWalkTick =HUtil32. GetTickCount();      //走路间隔
                        userSession.dwRunTick = HUtil32.GetTickCount();       //跑步间隔
                        userSession.dwHitTick = HUtil32.GetTickCount();       //攻击间隔
                        userSession.dwSpellTick = HUtil32.GetTickCount();     //魔法间隔
                        userSession.dwTurnTick = HUtil32.GetTickCount();      //转身间隔
                        userSession.dwPickupTick =HUtil32. GetTickCount();    //捡起间隔
                        userSession.dwButchTick = HUtil32.GetTickCount();     //挖肉间隔
                        userSession.dwEatTick = HUtil32.GetTickCount();       //吃药间隔
                        userSession.dwRunWalkTick = HUtil32.GetTickCount();       //移动时间
                        userSession.dwFeiDnItemsTick = HUtil32.GetTickCount();    //传送时间
                        userSession.dwSuperNeverTick = HUtil32.GetTickCount();    //超级加速时间
                        userSession.dwGameTick = HUtil32.GetTickCount();    //在线时间
                    }
                }
            }
        }
    }
}