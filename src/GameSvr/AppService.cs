#nullable enable
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using SystemModule.Data;

namespace GameSvr
{
    public class AppService : IHostedService, IDisposable
    {
        private readonly ILogger<AppService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly GameApp _mirApp;
        private Task? _applicationTask;
        private int? _exitCode;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly CommandLineApplication _application = new CommandLineApplication();

        public AppService(ILogger<AppService> logger, IHostApplicationLifetime lifetime, GameApp serverApp)
        {
            _logger = logger;
            _appLifetime = lifetime;
            _mirApp = serverApp;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            _application.HelpOption("-?|-h|-help");
            _application.OnExecute(() =>
            {
                _application.ShowHelp();
                return 0;
            });
            _application.Command("s", command =>
            {
                command.OnExecuteAsync(async (cancellationToken) =>
                {
                    await ShowServerStatus(cancellationToken);
                });
            });
            _application.Command("exit", command =>
            {
                command.OnExecute(() =>
                {
                    _appLifetime.StopApplication();
                    return 0;
                });
            });
            _application.Command("q", command =>
            {
                command.OnExecute(() =>
                {
                    Exit();
                    return 0;
                });
            });

            _appLifetime.ApplicationStarted.Register(() =>
            {
                _logger.LogDebug("Application has started");
                _applicationTask = Task.Run(() =>
                {
                    try
                    {
                        _logger.LogInformation("正在读取配置信息...");
                        _mirApp.Initialize();
                        _logger.LogInformation("读取配置信息完成...");
                        _mirApp.StartEngine(stoppingToken);
                        _mirApp.StartService();
                        if (M2Share.StartReady)
                        {
                            _mirApp.Start(stoppingToken);
                            M2Share.UserEngine.Start(stoppingToken);
                            M2Share.GateMgr.Start(stoppingToken);
                        }
                        _exitCode = 0;
                    }
                    catch (TaskCanceledException)
                    {
                        // This means the application is shutting down, so just swallow this exception
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                        _exitCode = 1;
                    }
                }, stoppingToken);
                ProcessLoopAsync();
            });

            _appLifetime.ApplicationStopping.Register(OnShutdown);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            // Wait for the application logic to fully complete any cleanup tasks.
            // Note that this relies on the cancellation token to be properly used in the application.
            if (_applicationTask != null)
            {
                await _applicationTask;
            }

            _logger.LogDebug($"Exiting with return code: {_exitCode}");

            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        }

        private void OnShutdown()
        {
            _logger.LogDebug("Application is stopping");
            M2Share.StartReady = false;
            if (M2Share.UserEngine.PlayObjectCount > 0)
            {
                _logger.LogInformation("保存玩家数据");
                foreach (var item in M2Share.UserEngine.PlayObjects)
                {
                    M2Share.UserEngine.SaveHumanRcd(item);
                }
                _logger.LogInformation("数据保存完毕.");
            }

            _logger.LogInformation("检查是否有其他可用服务器.");
            //如果有多机负载转移在线玩家到新服务器
            var sIPaddr = string.Empty;
            var nPort = 0;
            var isMultiServer = M2Share.GetMultiServerAddrPort(M2Share.ServerIndex, ref sIPaddr, ref nPort);//如果有可用服务器，那就切换过去
            if (isMultiServer)
            {
                //todo 通知网关断开链接.停止新玩家进入游戏
                _logger.LogInformation($"转移到新服务器[{sIPaddr}:{nPort}]");
                var playerCount = M2Share.UserEngine.PlayObjects.Count();
                if (playerCount > 0)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        var shutdownSeconds = 120;
                        while (true)
                        {
                            if (playerCount <= 0)
                            {
                                break;
                            }
                            foreach (var playObject in M2Share.UserEngine.PlayObjects)
                            {
                                var closeStr = $"服务器关闭倒计时 [{shutdownSeconds}].";
                                playObject.SysMsg(closeStr, MsgColor.Red, MsgType.Notice);
                                _logger.LogInformation(closeStr);
                                shutdownSeconds--;
                            }
                            if (shutdownSeconds > 0)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(1));
                            }
                            else
                            {
                                foreach (var playObject in M2Share.UserEngine.PlayObjects)
                                {
                                    if (playObject.Ghost || playObject.Death)//死亡或者下线的玩家不进行转移
                                    {
                                        playerCount--;
                                        continue;
                                    }
                                    playObject.ChangeSnapsServer(sIPaddr, nPort);
                                    playerCount--;
                                }
                                break;
                            }
                        }
                        _logger.LogInformation("玩家转移完毕，关闭游戏服务器.");
                        _mirApp.Stop();
                    }, _cancellationTokenSource.Token);
                }
            }
            else
            {
                _logger.LogInformation("没有可用服务器，即将关闭游戏服务器.");
                _mirApp.Stop();
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
                    _application.Execute(new string[] { cmdline });
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void Exit()
        {
            if (AnsiConsole.Confirm("Do you really want to exit?"))
            {
                _cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(1));//延时5分钟关闭游戏服务.
            }
        }

        private static Task ClearConsole()
        {
            Console.Clear();
            AnsiConsole.Clear();
            return Task.CompletedTask;
        }

        private static Task ShowServerStatus(CancellationToken cancellationToken)
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
    }
}