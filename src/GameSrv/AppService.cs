using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Spectre.Console;
using SystemModule.Enums;

namespace GameSrv {
    public class AppService : IHostedService, IDisposable {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IHost Host;
        private readonly GameApp _mirApp;
        private Task _applicationTask;
        private int? _exitCode;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly CommandLineApplication _application = new CommandLineApplication();
        private PeriodicTimer _timer;

        public AppService(IHostApplicationLifetime lifetime, GameApp serverApp, IServiceProvider serviceProvider) {
            _appLifetime = lifetime;
            _mirApp = serverApp;
            Host = serviceProvider.GetService<IHost>();
        }

        public Task StartAsync(CancellationToken stoppingToken) {
            _logger.Debug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            _application.HelpOption("-?|-h|-help");
            _application.OnExecute(() => {
                _application.ShowHelp();
                return 0;
            });
            _application.Command("reloadconf", command => {
                command.Description = "重新读取配置文件";
                command.OnExecute(() => {
                    Console.WriteLine("重新读取所有配置文件");
                });
            });
            _application.Command("save", command => {
                command.Description = "立即保存游戏数据";
                command.OnExecute(SavePlayer);
            });
            _application.Command("gamestatus", command => {
                command.Description = "查看游戏网关状况";
                command.OnExecuteAsync(async (cancellationToken) => {
                    await ShowGateStatus(cancellationToken);
                });
            });
            _application.Command("exit", command => {
                command.Description = "停止游戏服务";
                command.OnExecute(() => {
                    _appLifetime.StopApplication();
                    return 0;
                });
            });
            _application.Command("quit", command => {
                command.Description = "退出程序";
                command.OnExecute(() => {
                    Exit();
                    return 0;
                });
            });
            _application.Command("status", command => {
                command.Description = "查看系统状态";
                command.OnExecute(() => {
                    ShowWordStatus(stoppingToken);
                });
            });

            _appLifetime.ApplicationStarted.Register(() => {
                _logger.Debug("Application has started");
                _applicationTask = Task.Run(() => {
                    try {
                        _logger.Info("正在读取配置信息...");
                        _mirApp.Initialize();
                        _logger.Info("读取配置信息完成...");
                        _mirApp.StartServer(stoppingToken);
                        _mirApp.StartWorld(stoppingToken);
                        _exitCode = 0;
                    }
                    catch (TaskCanceledException) {
                        // This means the application is shutting down, so just swallow this exception
                    }
                    catch (Exception ex) {
                        _logger.Error(ex, "Unhandled exception!");
                        _logger.Error(ex.StackTrace);
                        _exitCode = 1;
                    }
                }, stoppingToken);
                ProcessLoopAsync();
            });

            _appLifetime.ApplicationStopping.Register(OnShutdown);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken) {
            // Wait for the application logic to fully complete any cleanup tasks.
            // Note that this relies on the cancellation token to be properly used in the application.
            if (_applicationTask != null) {
                await _applicationTask;
            }

            _logger.Debug($"Exiting with return code: {_exitCode}");

            _appLifetime.StopApplication();
            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.Exit(Environment.ExitCode);
        }

        private void SavePlayer() {
            if (M2Share.WorldEngine.PlayObjectCount > 0) //服务器关闭，强制保存玩家数据
            {
                _logger.Info("保存玩家数据");
                foreach (Player.PlayObject play in M2Share.WorldEngine.PlayObjects) {
                    M2Share.WorldEngine.SaveHumanRcd(play);
                }
                _logger.Info("数据保存完毕.");
            }
        }

        private void StopService(string sIPaddr, int nPort, bool isTransfer) {
            Task.Factory.StartNew(async () => {
                int shutdownSeconds = M2Share.Config.CloseCountdown;
                int playerCount = M2Share.WorldEngine.PlayObjectCount;
                while (true) {
                    if (playerCount <= 0) {
                        break;
                    }
                    foreach (Player.PlayObject playObject in M2Share.WorldEngine.PlayObjects) {
                        string closeMsg = isTransfer ? $"服务器关闭倒计时[{shutdownSeconds}]. 关闭后自动转移到其他大区，请勿退出游戏。" : $"服务器关闭倒计时[{shutdownSeconds}].";
                        playObject.SysMsg(closeMsg, MsgColor.Red, MsgType.Notice);
                        _logger.Info(closeMsg);
                        shutdownSeconds--;
                    }
                    if (shutdownSeconds > 0) {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    else {
                        if (isTransfer) {
                            foreach (Player.PlayObject playObject in M2Share.WorldEngine.PlayObjects) {
                                if (playObject.Ghost || playObject.Death)//死亡或者下线的玩家不进行转移
                                {
                                    playerCount--;
                                    continue;
                                }
                                playObject.TransferPlanesServer(sIPaddr, nPort);
                                playerCount--;
                            }
                            break;
                        }
                    }
                }
                ServerBase.Stop();
                await Host.StopAsync(_cancellationTokenSource.Token);
                _logger.Info("游戏服务已停止...");
            }, _cancellationTokenSource.Token);
        }

        private void OnShutdown() {
            _logger.Debug("Application is stopping");
            M2Share.StartReady = false;
            SavePlayer();
            if (M2Share.ServerIndex == 0) {
                StopService("", 0, false);
            }
            else if (M2Share.ServerIndex > 0) {
                _logger.Info("检查是否有其他可用服务器.");
                //如果有多机负载转移在线玩家到新服务器
                string sIPaddr = string.Empty;
                int nPort = 0;
                bool isMultiServer = M2Share.GetMultiServerAddrPort(M2Share.ServerIndex, ref sIPaddr, ref nPort);//如果有可用服务器，那就切换过去
                if (isMultiServer) {
                    //todo 通知网关断开链接.停止新玩家进入游戏
                    _logger.Info($"转移到新服务器[{sIPaddr}:{nPort}]");
                    StopService(sIPaddr, nPort, true);
                }
            }
            else {
                _logger.Info("没有可用服务器，即将关闭游戏服务器.");
                StopService("", 0, false);
            }
            _cancellationTokenSource?.CancelAfter(3000);
        }

        private void ProcessLoopAsync() {
            while (true) {
                string cmdline = Console.ReadLine();
                if (string.IsNullOrEmpty(cmdline)) {
                    continue;
                }
                try {
                    _application.Execute(new[] { cmdline });
                }
                catch {
                    // ignored
                }
            }
        }

        private static void Exit() {
            if (AnsiConsole.Confirm("Do you really want to exit?")) {
                // _cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(1));//延时5分钟关闭游戏服务.
                Environment.Exit(Environment.ExitCode);
            }
        }

        private static Task ClearConsole() {
            Console.Clear();
            AnsiConsole.Clear();
            return Task.CompletedTask;
        }

        private void ShowWordStatus(CancellationToken cancellationToken) {
            if (_timer != null) {
                _timer.Dispose();
                _timer = null;
            }
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("Thinking...", async ctx => {
                    while (await _timer.WaitForNextTickAsync(cancellationToken)) {
                        int monsterCount = 0;
                        for (int i = 0; i < M2Share.WorldEngine.MobThreads.Length; i++) {
                            monsterCount += M2Share.WorldEngine.MobThreads[i].MonsterCount;
                        }
                        AnsiConsole.MarkupLine($"Monsters:{monsterCount}");
                        M2Share.Statistics.ShowServerStatus();
                        ctx.Refresh();
                    }
                });
        }

        private static Task ShowGateStatus(CancellationToken cancellationToken) {
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

        public void Dispose() {

        }
    }
}