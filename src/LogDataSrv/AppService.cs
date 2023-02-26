using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogDataSrv
{
    public class AppService : IHostedService, IDisposable
    {
        private readonly ILogger<AppService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private Task? _applicationTask;
        private int? _exitCode;
        private CancellationTokenSource? _cancellationTokenSource;
        private PeriodicTimer _timer;

        public AppService(ILogger<AppService> logger, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _appLifetime = lifetime;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            _appLifetime.ApplicationStarted.Register(() =>
            {
                _logger.LogDebug("Application has started");
                _applicationTask = Task.Run(async () =>
                {
                    try
                    {
                        Process[]? processes = null;
                        while (processes?.Length != 1)
                        {
                            processes = Process.GetProcessesByName("GameSrv");
                            await Task.Delay(100);
                        }

                        int counter = 0;

                        DiagnosticsClient client = new DiagnosticsClient(processes.First().Id);

                        Console.WriteLine($"GameSvr进程监听成功.PID[{processes.First().Id}]");

                        var providers = new List<EventPipeProvider>()
                        {
                            new EventPipeProvider("LogProvider", System.Diagnostics.Tracing.EventLevel.Informational, (long)ClrTraceEventParser.Keywords.All)
                        };
                        var session = client.StartEventPipeSession(providers, false);

                        var source = new EventPipeEventSource(session.EventStream);

                        source.Dynamic.All += (e) =>
                        {
                            if (e.ProviderName == "UserLogProvider")
                            {
                                Console.WriteLine($"{counter++} {e.ID} {e.EventName} {e.PayloadString(0)}");
                            }
                            else
                            {
                                Console.WriteLine($"{e.ProviderName} {e.EventName}");
                            }
                        };
                        source.Process();
                        _exitCode = 0;
                    }
                    catch (TaskCanceledException)
                    {
                        // This means the application is shutting down, so just swallow this exception
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                        _logger.LogError(ex.StackTrace);
                        _exitCode = 1;
                    }
                }, stoppingToken);
            });

            _appLifetime.ApplicationStopping.Register(OnShutdown);
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
            _cancellationTokenSource?.CancelAfter(3000);
        }

        private void Exit()
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
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
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