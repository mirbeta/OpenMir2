using GameGate.Conf;
using GameGate.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Runtime;

namespace GameGate
{
    internal class Program
    {
        private static PeriodicTimer _timer;
        private static readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();

        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCLatencyMode.Batch;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            ThreadPool.SetMaxThreads(200, 200);
            ThreadPool.GetMinThreads(out int workThreads, out int completionPortThreads);

            PrintUsage();
            Console.CancelKeyPress += delegate
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }

                AnsiConsole.Reset();
            };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            LogService.Logger = Log.Logger;

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<TimedService>();
            builder.Services.AddHostedService<AppService>();
            builder.Logging.AddConfiguration(configuration.GetSection("Logging"));
            builder.Logging.AddSerilog(dispose: true);

            IHost host = builder.Build();
            await host.StartAsync(CancellationToken.Token);
            LogService.Info(new StringBuilder()
               .Append($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}, ")
               .Append($"Minimum work threads: {workThreads}, ")
               .Append($"Minimum completion port threads: {completionPortThreads})").ToString());
            //启动后台服务
            await ProcessLoopAsync();
            Stop();
        }

        private static void Stop()
        {
            AnsiConsole.Status().Start("Disconnecting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
            });
        }

        private static async Task ProcessLoopAsync()
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

        private static Task Exit()
        {
            CancellationToken.CancelAfter(3000);
            Environment.Exit(Environment.ExitCode);
            return Task.CompletedTask;
        }

        private static Task ClearConsole()
        {
            Console.Clear();
            AnsiConsole.Clear();
            return Task.CompletedTask;
        }

        private static Task ReLoadConfig()
        {
            ConfigManager.Instance.ReLoadConfig();
            ServerManager.Instance.StartClientMessageWork(CancellationToken.Token);
            Console.WriteLine("重新读取配置文件完成...");
            return Task.CompletedTask;
        }

        private static async Task ShowServerStatus()
        {
            //GateShare.ShowLog = false;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            ServerService[] serverList = ServerManager.Instance.GetServerList();
            Table table = new Table().Expand().BorderColor(Color.Grey);
            table.AddColumn("[yellow]Id[/]");
            table.AddColumn("[yellow]EndPoint[/]");
            table.AddColumn("[yellow]Status[/]");
            table.AddColumn("[yellow]Online[/]");
            table.AddColumn("[yellow]Send[/]");
            table.AddColumn("[yellow]Revice[/]");
            table.AddColumn("[yellow]Total Send[/]");
            table.AddColumn("[yellow]Total Revice[/]");
            table.AddColumn("[yellow]Queue[/]");
            table.AddColumn("[yellow]WorkThread[/]");

            await AnsiConsole.Live(table)
                 .AutoClear(true)
                 .Overflow(VerticalOverflow.Crop)
                 .Cropping(VerticalOverflowCropping.Bottom)
                 .StartAsync(async ctx =>
                 {
                     foreach (int _ in Enumerable.Range(0, serverList.Length))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync(CancellationToken.Token))
                     {
                         for (int i = 0; i < serverList.Length; i++)
                         {
                             (string endPoint, string status, string playCount, string reviceTotal, string sendTotal, string totalrevice, string totalSend, string queueCount, int threads) = serverList[i].GetStatus();

                             table.UpdateCell(i, 0, $"[bold]{endPoint}[/]");
                             table.UpdateCell(i, 1, $"[bold]{status}[/]");
                             table.UpdateCell(i, 2, $"[bold]{playCount}[/]");
                             table.UpdateCell(i, 3, $"[bold]{sendTotal}[/]");
                             table.UpdateCell(i, 4, $"[bold]{reviceTotal}[/]");
                             table.UpdateCell(i, 5, $"[bold]{totalSend}[/]");
                             table.UpdateCell(i, 6, $"[bold]{totalrevice}[/]");
                             table.UpdateCell(i, 7, $"[bold]{queueCount}[/]");
                             table.UpdateCell(i, 8, $"[bold]{threads}[/]");
                         }
                         ctx.Refresh();
                     }
                 });
        }

        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine(@"   ___                           __  __   _          ____                  ");
            Console.WriteLine(@"  / _ \   _ __     ___   _ __   |  \/  | (_)  _ __  |___ \                 ");
            Console.WriteLine(@" | | | | | '_ \   / _ \ | '_ \  | |\/| | | | | '__|   __) |                ");
            Console.WriteLine(@" | |_| | | |_) | |  __/ | | | | | |  | | | | | |     / __/                 ");
            Console.WriteLine(@"  \___/  | .__/   \___| |_| |_| |_|  |_| |_| |_|    |_____|                ");
            Console.WriteLine(@"         |_|                                                               ");
            Console.WriteLine(@"   ____                               ____           _                     ");
            Console.WriteLine(@"  / ___|   __ _   _ __ ___     ___   / ___|   __ _  | |_    ___            ");
            Console.WriteLine(@" | |  _   / _` | | '_ ` _ \   / _ \ | |  _   / _` | | __|  / _ \           ");
            Console.WriteLine(@" | |_| | | (_| | | | | | | | |  __/ | |_| | | (_| | | |_  |  __/           ");
            Console.WriteLine(@"  \____|  \__,_| |_| |_| |_|  \___|  \____|  \__,_|  \__|  \___|           ");
            Console.WriteLine(@"                                                                           ");
        }
    }
}