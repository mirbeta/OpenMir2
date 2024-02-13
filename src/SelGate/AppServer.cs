using Microsoft.Extensions.DependencyInjection;
using SelGate.Conf;
using SelGate.Services;
using Spectre.Console;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenMir2;
using SystemModule;

namespace SelGate
{
    public class AppServer 
    {
        private readonly ServerHost _serverHost;
        private static readonly PeriodicTimer _timer;

        public AppServer()
        {
            PrintUsage();
            Console.CancelKeyPress += delegate
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                AnsiConsole.Reset();
            };
            _serverHost = new ServerHost();
            _serverHost.ConfigureServices(service =>
            {
                service.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "config.conf")));
                service.AddSingleton<ServerService>();
                service.AddSingleton<SessionManager>();
                service.AddSingleton<ClientManager>();
                service.AddHostedService<AppService>();
                service.AddHostedService<TimedService>();
            });
        }

        private void PrintUsage()
        {
            Console.WriteLine(@"                                                                      ");
            Console.WriteLine(@"   ___                           __  __   _          ____             ");
            Console.WriteLine(@"  / _ \   _ __     ___   _ __   |  \/  | (_)  _ __  |___ \            ");
            Console.WriteLine(@" | | | | | '_ \   / _ \ | '_ \  | |\/| | | | | '__|   __) |           ");
            Console.WriteLine(@" | |_| | | |_) | |  __/ | | | | | |  | | | | | |     / __/            ");
            Console.WriteLine(@"  \___/  | .__/   \___| |_| |_| |_|  |_| |_| |_|    |_____|           ");
            Console.WriteLine(@"         |_|                                                          ");
            Console.WriteLine(@"  ____           _    ____           _                                ");
            Console.WriteLine(@" / ___|    ___  | |  / ___|   __ _  | |_    ___                       ");
            Console.WriteLine(@" \___ \   / _ \ | | | |  _   / _` | | __|  / _ \                      ");
            Console.WriteLine(@"  ___) | |  __/ | | | |_| | | (_| | | |_  |  __/                      ");
            Console.WriteLine(@" |____/   \___| |_|  \____|  \__,_|  \__|  \___|                      ");
            Console.WriteLine(@"                                                                      ");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _serverHost.BuildHost();
            await _serverHost.StartAsync(cancellationToken);
            await ProcessLoopAsync();
            Stop();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _serverHost.StopAsync(cancellationToken);
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
                    "/r" => ReLoadConfig(),
                    "/q" => Exit(),
                    _ => null
                } is Task task)
                {
                    await task;
                    continue;
                }

            } while (input is not "/exit");
        }

        private Task ReLoadConfig()
        {
            ConfigManager config = _serverHost.ServiceProvider.GetService<ConfigManager>();
            config?.ReLoadConfig();
            LogService.Info("重新读取配置文件完成...");
            return Task.CompletedTask;
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

        private static Task ShowServerStatus()
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
    }
}