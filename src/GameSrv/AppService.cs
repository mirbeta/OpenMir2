using GameSrv.Services;
using GameSrv.Word;
using McMaster.Extensions.CommandLineUtils;
using SystemModule.Enums;

namespace GameSrv
{
    public class AppService : IHostedLifecycleService, IDisposable
    {
        private readonly GameApp _mirApp;
        private int? _exitCode;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CommandLineApplication _application;
        private PeriodicTimer _timer;

        public AppService(GameApp serverApp, IServiceProvider serviceProvider)
        {
            _mirApp = serverApp;
            _application = new CommandLineApplication();
            LogService.Debug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _cancellationTokenSource = new CancellationTokenSource();

            _application.HelpOption("-?|-h|-help");
            _application.OnExecute(() =>
            {
                _application.ShowHelp();
                return 0;
            });
            _application.Command("reloadconf", command =>
            {
                command.Description = "重新读取配置文件";
                command.OnExecute(() =>
                {
                    Console.WriteLine("重新读取所有配置文件");
                });
            });
            _application.Command("save", command =>
            {
                command.Description = "立即保存游戏数据";
                command.OnExecute(SavePlayer);
            });
            _application.Command("gamestatus", command =>
            {
                command.Description = "查看游戏网关状况";
                command.OnExecuteAsync(async (cancellationToken) =>
                {
                    await ShowGateStatus(cancellationToken);
                });
            });
            _application.Command("exit", command =>
            {
                command.Description = "停止游戏服务";
                command.OnExecute(() =>
                {
                    StoppingAsync(_cancellationTokenSource.Token);
                    return 0;
                });
            });
            _application.Command("quit", command =>
            {
                command.Description = "退出程序";
                command.OnExecute(() =>
                {
                    Exit();
                    return 0;
                });
            });
            _application.Command("status", command =>
            {
                command.Description = "查看系统状态";
                command.OnExecute(() =>
                {
                    ShowWordStatus(_cancellationTokenSource.Token);
                });
            });
        }

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            try
            {
                LogService.Info("正在读取配置信息...");
                LogService.Info("读取游戏引擎数据配置文件...");
                GameShare.GeneratorProcessor.Initialize(cancellationToken);
                M2Share.FrontEngine = new FrontEngine();
                GameShare.LoadConfig();
                _mirApp.LoadServerTable();
                LogService.Info("初始化游戏引擎数据配置文件完成...");
                LogService.Info("初始化游戏基础数据...");
                _mirApp.Initialize(cancellationToken);
                LogService.Info("初始化游戏基础数据完成...");
            }
            catch (Exception ex)
            {
                _exitCode = 1;
                LogService.Error("初始化游戏基础数据失败...", ex);
            }
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            try
            {
                LogService.Info("初始化游戏世界服务...");
                _mirApp.InitializeWorld(_cancellationTokenSource.Token);
                LogService.Info("初始化游戏世界服务完成...");
            }
            catch (Exception ex)
            {
                LogService.Error("初始化游戏世界服务失败...", ex);
            }
            return Task.CompletedTask;
        }

        public async Task StartedAsync(CancellationToken cancellationToken)
        {
            _exitCode = 0;
            await _mirApp.StartUp(cancellationToken);
            LogService.Info("初始化游戏世界服务线程完成...");
            LogService.Info("欢迎使用翎风系列游戏软件...");
            LogService.Info("网站:http://www.gameofmir.com");
            LogService.Info("论坛:http://bbs.gameofmir.com");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            LogService.Debug($"Exiting with return code: {_exitCode}");
            return Task.CompletedTask;
        }

        private void SavePlayer()
        {
            if (SystemShare.WorldEngine.PlayObjectCount > 0) //服务器关闭，强制保存玩家数据
            {
                LogService.Debug("保存玩家数据");
                IEnumerable<SystemModule.Actors.IPlayerActor> playObjectList = SystemShare.WorldEngine.GetPlayObjects();
                foreach (SystemModule.Actors.IPlayerActor play in playObjectList)
                {
                    WorldServer.SaveHumanRcd(play);
                }
                LogService.Debug("数据保存完毕.");
            }
        }

        private const string CloseTransferMessgae = "服务器关闭倒计时[{0}],无需下线或退出游戏,稍后自动回到安全区.";
        private const string CloseServerMessage = "服务器关闭倒计时[{0}].";

        private async Task StopService(string sIPaddr, int nPort, bool isTransfer)
        {
            int playerCount = SystemShare.WorldEngine.PlayObjectCount;
            if (playerCount == 0)
            {
                LogService.Info("没有玩家在线，游戏引擎服务已停止...Bye!");
                await _mirApp.Stopping(_cancellationTokenSource.Token);
                return;
            }
            // 通知游戏网关暂停接收新的连接,发送消息后停止5秒,防止玩家在倒计时结束前进入游戏
            await Task.Factory.StartNew(async () =>
            {
                int shutdownSeconds = SystemShare.Config.ShutdownSeconds;
                LogService.Debug("网关停止新玩家连接");
                M2Share.NetChannel.SendServerStopMsg();//通知网关停止分配新的玩家连接
                await Task.Delay(5000);//强制5秒延迟，防止玩家在倒计时结束前进入游戏
                while (true)
                {
                    IEnumerable<SystemModule.Actors.IPlayerActor> playObjectList = SystemShare.WorldEngine.GetPlayObjects();
                    if (shutdownSeconds <= 0)
                    {
                        if (isTransfer)
                        {
                            foreach (SystemModule.Actors.IPlayerActor playObject in playObjectList)
                            {
                                if (playObject.Ghost || playObject.Death)//死亡或者下线的玩家不进行转移
                                {
                                    continue;
                                }
                                // playObject.TransferPlanesServer(sIPaddr, nPort);
                            }
                        }
                        break;//转移结束后跳出循环
                    }
                    foreach (SystemModule.Actors.IPlayerActor playObject in playObjectList)
                    {
                        string closeMsg = isTransfer ? string.Format(CloseTransferMessgae, shutdownSeconds) : string.Format(CloseServerMessage, shutdownSeconds);
                        playObject.SysMsg(closeMsg, MsgColor.Red, MsgType.Notice);
                        LogService.Info(closeMsg);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    shutdownSeconds--;
                }
                LogService.Info("5秒后关闭网关服务...");
                await Task.Delay(5000);//延时1秒，等待网关服务停止
                await M2Share.NetChannel.StopAsync();//停止网关服务
                LogService.Info("网关服务已停止...");
                LogService.Info("即将停止游戏引擎世界服务...");
                await Task.Delay(500);//延时1秒，等待网关服务停止
                await _mirApp.Stopping(_cancellationTokenSource.Token);
                LogService.Info("游戏引擎世界服务已停止...");
                LogService.Info("游戏服务已停止...");
                LogService.Info("goodbye!");
            }, _cancellationTokenSource.Token);
        }

        private async void OnShutdown()
        {
            LogService.Debug("Application is stopping");
            M2Share.StartReady = false;
            SavePlayer();
            if (SystemShare.ServerIndex == 0)
            {
                await StopService(string.Empty, 0, false);
            }
            else if (SystemShare.ServerIndex > 0)
            {
                LogService.Info("检查是否有其他可用服务器.");
                //如果有多机负载转移在线玩家到新服务器
                string sIPaddr = string.Empty;
                int nPort = 0;
                bool isMultiServer = GameShare.GetMultiServerAddrPort(SystemShare.ServerIndex, ref sIPaddr, ref nPort);//如果有可用服务器，那就切换过去
                if (isMultiServer)
                {
                    LogService.Info($"玩家转移目标服务器[{sIPaddr}:{nPort}].");
                    await StopService(sIPaddr, nPort, true);
                }
            }
            else
            {
                LogService.Info("没有可用服务器，即将关闭游戏服务器.");
                await StopService(string.Empty, 0, false);
            }
            _cancellationTokenSource?.CancelAfter(3000);
        }

        private void ProcessLoopAsync()
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
                    _application.Execute(cmdline);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static void Exit()
        {
            if (AnsiConsole.Confirm("Do you really want to exit?"))
            {
                // _cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(1));//延时5分钟关闭游戏服务.
                Environment.Exit(Environment.ExitCode);
            }
        }

        private static Task ClearConsole()
        {
            Console.Clear();
            AnsiConsole.Clear();
            return Task.CompletedTask;
        }

        private void ShowWordStatus(CancellationToken cancellationToken)
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("Thinking...", async ctx =>
                {
                    while (await _timer.WaitForNextTickAsync(cancellationToken))
                    {
                        int monsterCount = 0;
                        /*for (var i = 0; i < SystemShare.WorldEngine.MobThreads.Length; i++)
                        {
                            monsterCount += SystemShare.WorldEngine.MobThreads[i].MonsterCount;
                        }*/
                        AnsiConsole.MarkupLine($"Monsters:{monsterCount}");
                        GameShare.Statistics.ShowServerStatus();
                        ctx.Refresh();
                    }
                });
        }

        private static Task ShowGateStatus(CancellationToken cancellationToken)
        {
            //GateShare.ShowLog = false;
            //_timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            //var serverList = ServerManager.Instance.GetServerList();
            //var table = new Table().Expand().BorderColor(Color.Grey);
            //table.AddColumn("[yellow]Address[/]");
            //table.AddColumn("[yellow]Port[/]");
            //table.AddColumn("[yellow]Status[/]");
            //table.AddColumn("[yellow]Online[/]");
            //table.AddColumn("[yellow]Send[/]");
            //table.AddColumn("[yellow]Revice[/]");
            //table.AddColumn("[yellow]Queue[/]");

            //await AnsiConsole.Live(table)
            //     .AutoClear(true)
            //     .Overflow(VerticalOverflow.Crop)
            //     .Cropping(VerticalOverflowCropping.Bottom)
            //     .StartAsync(async ctx =>
            //     {
            //         foreach (var _ in Enumerable.Range(0, 10))
            //         {
            //             table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
            //         }

            //         while (await _timer.WaitForNextTickAsync(cts.Token))
            //         {
            //             for (int i = 0; i < serverList.Count; i++)
            //             {
            //                 var (serverIp, serverPort, Status, playCount, reviceTotal, sendTotal, queueCount) = serverList[i].GetStatus();

            //                 table.UpdateCell(i, 0, $"[bold]{serverIp}[/]");
            //                 table.UpdateCell(i, 1, ($"[bold]{serverPort}[/]"));
            //                 table.UpdateCell(i, 2, ($"[bold]{Status}[/]"));
            //                 table.UpdateCell(i, 3, ($"[bold]{playCount}[/]"));
            //                 table.UpdateCell(i, 4, ($"[bold]{sendTotal}[/]"));
            //                 table.UpdateCell(i, 5, ($"[bold]{reviceTotal}[/]"));
            //                 table.UpdateCell(i, 6, ($"[bold]{queueCount}[/]"));
            //             }
            //             ctx.Refresh();
            //         }
            //     });
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            OnShutdown();
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}