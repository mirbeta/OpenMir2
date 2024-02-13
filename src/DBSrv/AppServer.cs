using DBSrv.Conf;
using DBSrv.Services.Impl;
using DBSrv.Storage;
using DBSrv.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;
using OpenMir2;
using Spectre.Console;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace DBSrv
{
    public class AppServer
    {
        private static PeriodicTimer _timer;
        private readonly ServerHost _serverHost;
        
        public AppServer()
        {
            ConfigManager configManager = new ConfigManager();
            configManager.LoadConfig();
            _serverHost = new ServerHost();
            _serverHost.ConfigureServices(service =>
            {
                service.AddSingleton(configManager.Setting);
                service.AddSingleton<ClientSession>();
                service.AddSingleton<UserService>();
                service.AddSingleton<DataService>();
                service.AddSingleton<MarketService>();
                service.AddSingleton<ICacheStorage, CacheStorageService>();
                service.AddHostedService<TimedService>();
                service.AddHostedService<AppService>();
                LoadAssembly(service, "MySQL");
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _serverHost.BuildHost();
            await _serverHost.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serverHost.StopAsync(cancellationToken);
        }

        private void LoadAssembly(IServiceCollection services, string storageName)
        {
            string storageFileName = $"DBSrv.Storage.{storageName}.dll";
            string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, storageFileName);
            if (!File.Exists(storagePath))
            {
                throw new Exception($"{storageFileName} 存储策略文件不存在,服务启动失败.");
            }
            AssemblyLoadContext context = new AssemblyLoadContext(storagePath);
            context.Resolving += ContextResolving;
            Assembly assembly = context.LoadFromAssemblyPath(storagePath);
            if (assembly == null)
            {
                throw new Exception($"获取{storageName}数据存储实例失败，请确认文件是否正确.");
            }
            Type playDataStorageType = assembly.GetType($"DBSrv.Storage.{storageName}.PlayDataStorage", true);
            Type playRecordStorageType = assembly.GetType($"DBSrv.Storage.{storageName}.PlayRecordStorage", true);
            Type marketStorageType = assembly.GetType($"DBSrv.Storage.{storageName}.MarketStoageService", true);
            if (playDataStorageType == null)
            {
                throw new ArgumentNullException(nameof(storageName), "获取数据存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            if (playRecordStorageType == null)
            {
                throw new ArgumentNullException(nameof(storageName), "获取数据索引存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            if (marketStorageType == null)
            {
                throw new ArgumentNullException(nameof(storageName), "获取拍卖行存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            StorageOption storageOption = new StorageOption();
            storageOption.ConnectionString = "server=10.10.0.199;uid=root;pwd=123456;database=mir2_db;";
            IPlayDataStorage playDataStorage = (IPlayDataStorage)Activator.CreateInstance(playDataStorageType, storageOption);
            IPlayRecordStorage playRecordStorage = (IPlayRecordStorage)Activator.CreateInstance(playRecordStorageType, storageOption);
            IMarketStorage marketStorage = (IMarketStorage)Activator.CreateInstance(marketStorageType, storageOption);
            if (playDataStorage == null)
            {
                throw new ArgumentNullException(nameof(storageName), "创建数据存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            if (playRecordStorage == null)
            {
                throw new ArgumentNullException(nameof(storageName), "创建数据索引存储实例失败，请确认文件是否正确或程序版本是否正确.");
            }
            if (marketStorage == null)
            {
                throw new ArgumentNullException(nameof(storageName), "创建拍卖行数据存储实力失败，请确认文件是否正确或程序版本是否正确.");
            }
            services.AddSingleton(playDataStorage);
            services.AddSingleton(playRecordStorage);
            services.AddSingleton(marketStorage);
            LogService.Info($"[{storageName}]数据存储引擎初始化成功.");
        }

        /// <summary>
        /// 加载依赖项
        /// </summary>
        /// <returns></returns>
        private Assembly ContextResolving(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            string expectedPath = Path.Combine(AppContext.BaseDirectory, assemblyName.Name + ".dll");
            if (File.Exists(expectedPath))
            {
                try
                {
                    using FileStream stream = File.OpenRead(expectedPath);
                    return context.LoadFromStream(stream);
                }
                catch (Exception ex)
                {
                    LogService.Error($"加载依赖项{expectedPath}发生异常：{ex.Message},{ex.StackTrace}");
                }
            }
            else
            {
                LogService.Error($"依赖项不存在：{expectedPath}");
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

                string firstTwoCharacters = input[..2];

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
            UserService userService = _serverHost.ServiceProvider.GetService<UserService>();
            if (userService == null)
            {
                return;
            }
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            SelGateInfo[] serverList = userService.GetGates.ToArray();
            Table table = new Table().Expand().BorderColor(Color.Grey);
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
                     foreach (int _ in Enumerable.Range(0, serverList.Length))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync())
                     {
                         for (int i = 0; i < serverList.Length; i++)
                         {
                             (string serverIp, string status, string sessionCount, string reviceTotal, string sendTotal, string queueCount) = serverList[i].GetStatus();

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
    }
}