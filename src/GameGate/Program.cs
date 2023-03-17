using GameGate.Conf;
using GameGate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Spectre.Console;
using System;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = NLog.LogLevel;

namespace GameGate
{
    internal class Program
    {
        private static Logger _logger;
        private static PeriodicTimer _timer;
        private static readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();

        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            ThreadPool.SetMaxThreads(200, 200);
            ThreadPool.GetMinThreads(out var workThreads, out var completionPortThreads);
            Console.WriteLine(new StringBuilder()
                .Append($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}, ")
                .Append($"Minimum work threads: {workThreads}, ")
                .Append($"Minimum completion port threads: {completionPortThreads})").ToString());

            PrintUsage();
            Console.CancelKeyPress += delegate
            {
                ChanggeLogLevel(LogLevel.Info);
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                AnsiConsole.Reset();
            };

            var config = new ConfigurationBuilder().Build();

            _logger = LogManager.Setup()
                .SetupExtensions(ext => ext.RegisterConfigSettings(config))
                .GetCurrentClassLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddSingleton<CloudClient>();
                    services.AddSingleton<ServerApp>();
                    services.AddHostedService<TimedService>();
                    services.AddHostedService<AppService>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    logging.AddNLog(config);
                });
            await builder.StartAsync(CancellationToken.Token);
            await ProcessLoopAsync();
            Stop();
        }

        private static void Stop()
        {
            AnsiConsole.Status().Start("Disconnecting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                LogManager.Shutdown();
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

                var firstTwoCharacters = input[..2];

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
            ServerManager.Instance.StartMessageWorkThread(CancellationToken.Token);
            Console.WriteLine("重新读取配置文件完成...");
            return Task.CompletedTask;
        }

        private static void ChanggeLogLevel(LogLevel logLevel)
        {
            LogManager.Configuration.Variables["logLevel"] = logLevel.ToString();
            LogManager.ReconfigExistingLoggers();
        }
        
        private static async Task ShowServerStatus()
        {
            //GateShare.ShowLog = false;
            ChanggeLogLevel(LogLevel.Off);
            
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            var serverList = ServerManager.Instance.GetServerList();
            var table = new Table().Expand().BorderColor(Color.Grey);
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
                     foreach (var _ in Enumerable.Range(0, serverList.Length))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync(CancellationToken.Token))
                     {
                         for (var i = 0; i < serverList.Length; i++)
                         {
                             var (endPoint, status, playCount, reviceTotal, sendTotal, totalrevice, totalSend, queueCount, threads) = serverList[i].GetStatus();

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
            AnsiConsole.WriteLine();

            var table = new Table()
            {
                Border = TableBorder.None,
                Expand = true,
            }.HideHeaders();
            table.AddColumn(new TableColumn("One"));

            var header = new FigletText("OpenMir2")
            {
                Color = Color.Fuchsia
            };
            var header2 = new FigletText("Game Gate")
            {
                Color = Color.Aqua
            };

            var sb = new StringBuilder();
            sb.Append("[bold fuchsia]/s[/] [aqua]查看[/] 网关状况\n");
            sb.Append("[bold fuchsia]/r[/] [aqua]重读[/] 配置文件\n");
            sb.Append("[bold fuchsia]/c[/] [aqua]清空[/] 清除屏幕\n");
            sb.Append("[bold fuchsia]/q[/] [aqua]退出[/] 退出程序\n");
            var markup = new Markup(sb.ToString());

            table.AddColumn(new TableColumn("Two"));

            var rightTable = new Table()
                .HideHeaders()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn("Content"));

            rightTable.AddRow(header)
                .AddRow(header2)
                .AddEmptyRow()
                .AddEmptyRow()
                .AddRow(markup);
            table.AddRow(rightTable);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

    }
}