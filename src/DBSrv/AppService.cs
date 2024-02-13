using DBSrv.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMir2;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DBSrv.Conf;
using OpenMir2.Common;

namespace DBSrv
{
    public class AppService : IHostedLifecycleService
    {
        private readonly UserService _userService;
        private readonly ClientSession _sessionService;
        private readonly DataService _dataService;
        private readonly MarketService _marketService;
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _applicationTask;
        private int _exitCode;

        public AppService(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetService<UserService>();
            _dataService = serviceProvider.GetService<DataService>();
            _marketService = serviceProvider.GetService<MarketService>();
            _sessionService = serviceProvider.GetService<ClientSession>();
            _serviceProvider = serviceProvider;
        }

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            DBShare.Initialization();
            LogService.Info("正在读取基础配置信息...");
            DBShare.LoadConfig();
            LogService.Info($"加载IP授权文件列表成功...[{DBShare.ServerIpCount}]");
            LogService.Info("读取基础配置信息完成...");
            LoadServerInfo();
            LoadChrNameList("DenyChrName.txt");
            LoadClearMakeIndexList("ClearMakeIndex.txt");
            LogService.Info("配置文件读取完成...");
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _userService.Initialize();
            _dataService.Initialize();
            _marketService.Initialize();
            _userService.Start();
            _dataService.Start();
            _marketService.Start();
            _sessionService.Start();
            _exitCode = 0;
            return Task.CompletedTask;
        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            LogService.Info("服务已启动成功...");
            LogService.Info("欢迎使用翎风系列游戏软件...");
            LogService.Info("网站:http://www.gameofmir.com");
            LogService.Info("论坛:http://bbs.gameofmir.com");
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _userService.Stop();
            _dataService.Stop();
            _marketService.Stop();
            _sessionService.Stop();
            if (_applicationTask != null)
            {
                await _applicationTask;
            }
            LogService.Debug("DBSrv is stopping.");
            LogService.Debug($"Exiting with return code: {_exitCode}");
            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.Exit(Environment.ExitCode);
        }

        private async void OnShutdown()
        {
            LogService.Debug("Application is stopping");
            LogService.Info("数据引擎世界服务已停止...");
            LogService.Info("数据服务已停止...");
            LogService.Info("goodbye!");
        }

        private static void ProcessLoopAsync()
        {
            while (true)
            {
                string cmdline = Console.ReadLine();
                if (string.IsNullOrEmpty(cmdline))
                {
                    continue;
                }
                try
                {
                    //_application.Execute(cmdline);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void LoadServerInfo()
        {
            string sSelGateIPaddr = string.Empty;
            string sGameGateIPaddr = string.Empty;
            string sGameGate = string.Empty;
            string sGameGatePort = string.Empty;
            string sMapName = string.Empty;
            string sMapInfo = string.Empty;
            string sServerIndex = string.Empty;
            StringList loadList = new StringList();
            if (!File.Exists(DBShare.GateConfFileName))
            {
                return;
            }
            loadList.LoadFromFile(DBShare.GateConfFileName);
            if (loadList.Count <= 0)
            {
                LogService.Error("加载游戏服务配置文件ServerInfo.txt失败.");
                return;
            }
            int nRouteIdx = 0;
            int nGateIdx = 0;
            for (int i = 0; i < loadList.Count; i++)
            {
                string sLineText = loadList[i].Trim();
                if (!string.IsNullOrEmpty(sLineText) && !sLineText.StartsWith(";"))
                {
                    sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new[] { " ", "\09" });
                    if ((string.IsNullOrEmpty(sGameGate)) || (string.IsNullOrEmpty(sSelGateIPaddr)))
                    {
                        continue;
                    }
                    DBShare.RouteInfo[nRouteIdx] = new GateRouteInfo();
                    DBShare.RouteInfo[nRouteIdx].SelGateIP = sSelGateIPaddr.Trim();
                    DBShare.RouteInfo[nRouteIdx].GateCount = 0;
                    nGateIdx = 0;
                    while (!string.IsNullOrEmpty(sGameGate))
                    {
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                        string[] gamrGates = sGameGate.Split(",");
                        if (gamrGates.Length == 0)
                        {
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                            DBShare.RouteInfo[nRouteIdx].GameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                            DBShare.RouteInfo[nRouteIdx].GameGatePort[nGateIdx] = HUtil32.StrToInt(sGameGatePort, 0);
                            nGateIdx++;
                        }
                        else
                        {
                            for (int j = 0; j < gamrGates.Length; j++)
                            {
                                DBShare.RouteInfo[nRouteIdx].GameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                                DBShare.RouteInfo[nRouteIdx].GameGatePort[nGateIdx] = HUtil32.StrToInt(gamrGates[j], 0);
                                nGateIdx++;
                            }
                            sGameGate = string.Empty;
                        }
                    }
                    DBShare.RouteInfo[nRouteIdx].GateCount = nGateIdx;
                    nRouteIdx++;
                }
            }
            LogService.Info($"读取网关配置信息成功.[{DBShare.RouteInfo.Where(x => x != null).Sum(x => x.GateCount)}]");
            DBShare.MapList.Clear();
            var settings = _serviceProvider.GetService<SettingConf>();
            if (File.Exists(settings.MapFile))
            {
                loadList.Clear();
                loadList.LoadFromFile(settings.MapFile);
                for (int i = 0; i < loadList.Count; i++)
                {
                    string sLineText = loadList[i];
                    if ((!string.IsNullOrEmpty(sLineText)) && (sLineText[0] == '['))
                    {
                        sLineText = HUtil32.ArrestStringEx(sLineText, "[", "]", ref sMapName);
                        sMapInfo = HUtil32.GetValidStr3(sMapName, ref sMapName, new[] { " ", "\09" });
                        sServerIndex = HUtil32.GetValidStr3(sMapInfo, ref sMapInfo, new[] { " ", "\09" });
                        int nServerIndex = HUtil32.StrToInt(sServerIndex, 0);
                        DBShare.MapList.Add(sMapName, nServerIndex);
                    }
                }
            }
            loadList = null;
        }

        private static void LoadChrNameList(string sFileName)
        {
            int i;
            if (File.Exists(sFileName))
            {
                DBShare.DenyChrNameList.LoadFromFile(sFileName);
                i = 0;
                while (true)
                {
                    if (DBShare.DenyChrNameList.Count <= i)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(DBShare.DenyChrNameList[i].Trim()))
                    {
                        DBShare.DenyChrNameList.RemoveAt(i);
                        continue;
                    }
                    i++;
                }
            }
        }

        private static void LoadClearMakeIndexList(string sFileName)
        {
            if (File.Exists(sFileName))
            {
                DBShare.ClearMakeIndex.LoadFromFile(sFileName);
                int i = 0;
                while (true)
                {
                    if (DBShare.ClearMakeIndex.Count <= i)
                    {
                        break;
                    }
                    string sLineText = DBShare.ClearMakeIndex[i];
                    int nIndex = HUtil32.StrToInt(sLineText, -1);
                    if (nIndex < 0)
                    {
                        DBShare.ClearMakeIndex.RemoveAt(i);
                        continue;
                    }
                    DBShare.ClearMakeIndex[i] = nIndex.ToString();
                    i++;
                }
            }
        }
    }
}