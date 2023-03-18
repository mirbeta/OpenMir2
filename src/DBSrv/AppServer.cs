using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using DBSrv.Conf;
using DBSrv.Services;
using DBSrv.Storage;
using DBSrv.Storage.Impl;
using DBSrv.Storage.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Spectre.Console;
using SystemModule;
using SystemModule.Common;
using SystemModule.Hosts;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace DBSrv
{
    public class AppServer : ServiceHost
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static PeriodicTimer _timer;
        private static DbSrvConf _config;

        public AppServer()
        {
            Builder.ConfigureLogging(ConfigureLogging);
            Builder.ConfigureServices(ConfigureServices);
        }

        public override void Initialize()
        {
            _logger.Info("初始化配置文件...");
            var configManager = new ConfigManager();
            configManager.LoadConfig();
            _config = configManager.GetConfig;
            _logger.Info("配置文件读取完成...");
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            DBShare.Initialization();
            LoadServerInfo();
            LoadChrNameList("DenyChrName.txt");
            LoadClearMakeIndexList("ClearMakeIndex.txt");
            Host = await Builder.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Host.StopAsync(cancellationToken);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (!Enum.TryParse<StoragePolicy>(_config.StoreageType, true, out var storagePolicy))
            {
                throw new Exception("数据存储配置文件错误或者不支持该存储类型");
            }
            switch (storagePolicy)
            {
                case StoragePolicy.MySQL:
                    LoadAssembly(services, "MySQL");
                    _logger.Info("[MySQL]数据存储引擎初始化成功...");
                    break;
                case StoragePolicy.MongoDB:
                    LoadAssembly(services, "MongoDB");
                    _logger.Info("[MongoDB]数据存储引擎初始化成功.");
                    break;
                case StoragePolicy.Sqlite:
                    LoadAssembly(services, "Sqlite");
                    _logger.Info("[Sqlite]数据存储引擎初始化成功.");
                    break;
                case StoragePolicy.Local:
                    LoadAssembly(services, "Local");
                    _logger.Info("[Local]数据存储引擎初始化成功.");
                    break;
            }
            services.AddSingleton(_config);
            services.AddSingleton<LoginSessionService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<PlayerDataService>();
            services.AddSingleton<ICacheStorage, CacheStorageService>();
            services.AddHostedService<TimedService>();
            services.AddHostedService<AppService>();
        }

        private void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddNLog(Configuration);
        }

        private void LoadAssembly(IServiceCollection services, string storageName)
        {
            var storageFileName = $"DBSrv.Storage.{storageName}.dll";
            var storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, storageFileName);
            if (!File.Exists(storagePath))
            {
                throw new Exception($"{storageFileName} 存储策略文件不存在,服务启动失败.");
            }
            var context = new AssemblyLoadContext(storagePath);
            context.Resolving += ContextResolving;
            var assembly = context.LoadFromAssemblyPath(storagePath);
            if (assembly == null)
            {
                throw new Exception($"获取{storageName}数据存储实例失败，请确认文件是否正确.");
            }
            var playDataStorageType = assembly.GetType($"DBSrv.Storage.{storageName}.PlayDataStorage", true);
            var playRecordStorageType = assembly.GetType($"DBSrv.Storage.{storageName}.PlayRecordStorage", true);
            if (playDataStorageType == null)
            {
                throw new ArgumentNullException(nameof(storageName), "获取数据存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            if (playRecordStorageType == null)
            {
                throw new ArgumentNullException(nameof(storageName), "获取数据索引存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            var storageOption = new StorageOption(_config.ConnctionString);
            var playDataStorage = (IPlayDataStorage)Activator.CreateInstance(playDataStorageType, storageOption);
            var playRecordStorage = (IPlayRecordStorage)Activator.CreateInstance(playRecordStorageType, storageOption);
            if (playDataStorage == null)
            {
                throw new ArgumentNullException(nameof(storageName), "创建数据存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            if (playRecordStorage == null)
            {
                throw new ArgumentNullException(nameof(storageName), "创建数据索引存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            services.AddSingleton(playDataStorage);
            services.AddSingleton(playRecordStorage);
        }

        /// <summary>
        /// 加载依赖项
        /// </summary>
        /// <returns></returns>
        private Assembly ContextResolving(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            var expectedPath = Path.Combine(AppContext.BaseDirectory, assemblyName.Name + ".dll");
            if (File.Exists(expectedPath))
            {
                try
                {
                    using var stream = File.OpenRead(expectedPath);
                    return context.LoadFromStream(stream);
                }
                catch (Exception ex)
                {
                    _logger.Error($"加载依赖项{expectedPath}发生异常：{ex.Message},{ex.StackTrace}");
                }
            }
            else
            {
                _logger.Error($"依赖项不存在：{expectedPath}");
            }
            return null;
        }

        private static void Stop()
        {
            AnsiConsole.Status().Start("Disconnecting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
            });
        }

        private async Task ProcessLoopAsync()
        {
            string input = null;
            do
            {
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input.StartsWith("/exit") && AnsiConsole.Confirm("Do you really want to exit?"))
                {
                    return;
                }

                var firstTwoCharacters = input[..2];

                if (firstTwoCharacters switch
                {
                    "/s" => ShowServerStatus(),
                    "/c" => ClearConsole(),
                    "/q" => Exit(),
                    _ => null
                } is Task task)
                {
                    await task;
                    continue;
                }

            } while (input is not "/exit");
        }

        private static Task Exit()
        {
            Environment.Exit(Environment.ExitCode);
            return Task.CompletedTask;
        }

        private static Task ClearConsole()
        {
            Console.Clear();
            AnsiConsole.Clear();
            return Task.CompletedTask;
        }

        private async Task ShowServerStatus()
        {
            DBShare.ShowLog = false;
            var userService = Host.Services.GetService<UserService>();
            if (userService == null)
            {
                return;
            }
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            var serverList = userService.GetGates.ToArray();
            var table = new Table().Expand().BorderColor(Color.Grey);
            table.AddColumn("[yellow]ServerName[/]");
            table.AddColumn("[yellow]EndPoint[/]");
            table.AddColumn("[yellow]Status[/]");
            table.AddColumn("[yellow]Sessions[/]");
            table.AddColumn("[yellow]Send[/]");
            table.AddColumn("[yellow]Revice[/]");
            table.AddColumn("[yellow]Queue[/]");

            await AnsiConsole.Live(table)
                 .AutoClear(true)
                 .Overflow(VerticalOverflow.Crop)
                 .Cropping(VerticalOverflowCropping.Bottom)
                 .StartAsync(async ctx =>
                 {
                     foreach (var _ in Enumerable.Range(0, serverList.Length))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync())
                     {
                         for (int i = 0; i < serverList.Length; i++)
                         {
                             var (serverIp, status, sessionCount, reviceTotal, sendTotal, queueCount) = serverList[i].GetStatus();

                             table.UpdateCell(i, 0, "[bold][blue]SelGate[/][/]");
                             table.UpdateCell(i, 1, ($"[bold]{serverIp}[/]"));
                             table.UpdateCell(i, 2, ($"[bold]{status}[/]"));
                             table.UpdateCell(i, 3, ($"[bold]{sessionCount}[/]"));
                             table.UpdateCell(i, 4, ($"[bold]{sendTotal}[/]"));
                             table.UpdateCell(i, 5, ($"[bold]{reviceTotal}[/]"));
                             table.UpdateCell(i, 6, ($"[bold]{queueCount}[/]"));
                         }
                         ctx.Refresh();
                     }
                 });
        }

        private void LoadServerInfo()
        {
            var sSelGateIPaddr = string.Empty;
            var sGameGateIPaddr = string.Empty;
            var sGameGate = string.Empty;
            var sGameGatePort = string.Empty;
            var sMapName = string.Empty;
            var sMapInfo = string.Empty;
            var sServerIndex = string.Empty;
            var loadList = new StringList();
            if (!File.Exists(DBShare.GateConfFileName))
            {
                return;
            }
            loadList.LoadFromFile(DBShare.GateConfFileName);
            if (loadList.Count <= 0)
            {
                _logger.Error("加载游戏服务配置文件ServerInfo.txt失败.");
                return;
            }
            var nRouteIdx = 0;
            var nGateIdx = 0;
            for (var i = 0; i < loadList.Count; i++)
            {
                var sLineText = loadList[i].Trim();
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
                    while ((sGameGate != ""))
                    {
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new[] { " ", "\09" });
                        sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new[] { " ", "\09" });
                        DBShare.RouteInfo[nRouteIdx].GameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                        DBShare.RouteInfo[nRouteIdx].GameGatePort[nGateIdx] = HUtil32.StrToInt(sGameGatePort, 0);
                        nGateIdx++;
                    }
                    DBShare.RouteInfo[nRouteIdx].GateCount = nGateIdx;
                    nRouteIdx++;
                    _logger.Info($"读取网关配置信息.GameGateIP:[{sGameGateIPaddr}:{sGameGatePort}]");
                }
            }
            _logger.Info($"读取网关配置信息成功.[{loadList.Count}]");
            DBShare.MapList.Clear();
            if (File.Exists(_config.MapFile))
            {
                loadList.Clear();
                loadList.LoadFromFile(_config.MapFile);
                for (var i = 0; i < loadList.Count; i++)
                {
                    var sLineText = loadList[i];
                    if ((!string.IsNullOrEmpty(sLineText)) && (sLineText[0] == '['))
                    {
                        sLineText = HUtil32.ArrestStringEx(sLineText, "[", "]", ref sMapName);
                        sMapInfo = HUtil32.GetValidStr3(sMapName, ref sMapName, new[] { " ", "\09" });
                        sServerIndex = HUtil32.GetValidStr3(sMapInfo, ref sMapInfo, new[] { " ", "\09" });
                        var nServerIndex = HUtil32.StrToInt(sServerIndex, 0);
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
                var i = 0;
                while (true)
                {
                    if (DBShare.ClearMakeIndex.Count <= i)
                    {
                        break;
                    }
                    var sLineText = DBShare.ClearMakeIndex[i];
                    var nIndex = HUtil32.StrToInt(sLineText, -1);
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