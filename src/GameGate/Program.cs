using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    class Program
    {
        static IServiceProvider ServiceProvider { get; set; }
        private static PeriodicTimer _timer;
        private static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PrintUsage();
            var host = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "config.conf")));
                    services.AddSingleton<ServerApp>();
                    services.AddSingleton<ServerService>();
                    services.AddSingleton<SessionManager>();
                    services.AddSingleton<ClientManager>();
                    services.AddSingleton<LogQueue>();
                    services.AddHostedService<TimedService>();
                    services.AddHostedService<AppService>();
                }).UseConsoleLifetime().Build();
            Console.CancelKeyPress += Console_CancelKeyPress;

            await host.StartAsync();
            
            ServiceProvider = host.Services;
            ProcessLoopAsync();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            GateShare.ShowLog = true;
            //e.Cancel = true;
        }

        static async void ProcessLoopAsync()
        {
            string? input = null;
            do
            {
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input.StartsWith("/exit") && AnsiConsole.Confirm("Do you really want to exit?"))
                {
                    break;
                }

                var firstTwoCharacters = input[..2];

                if (firstTwoCharacters switch
                {
                    "/m" => ShowServerStatus(),
                    _ => null
                } is Task task)
                {
                    await task;
                    continue;
                }

            } while (input is not "/exit");
        }

        static async Task ShowServerStatus()
        {
            GateShare.ShowLog = false;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            var clientManager = ServiceProvider.GetRequiredService<ClientManager>();
            var clientList = clientManager.GetAllClient();
            var table = new Table().Expand().BorderColor(Color.Grey);
            table.AddColumn("[yellow]Address[/]");
            table.AddColumn("[yellow]Port[/]");
            table.AddColumn("[yellow]Status[/]");
            table.AddColumn("[yellow]Count[/]");
            table.AddColumn("[yellow]Send[/]");
            table.AddColumn("[yellow]Revice[/]");

            await AnsiConsole.Live(table)
                 .AutoClear(true)
                 .Overflow(VerticalOverflow.Crop)
                 .Cropping(VerticalOverflowCropping.Top)
                 .Start(async ctx =>
                 {
                     foreach (var _ in Enumerable.Range(0, 10))
                     {
                         //table.AddRow(new Text("-"), "-", "-", "-", "-", "-");
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync(cts.Token))
                     {
                         for (int i = 0; i < clientList.Count; i++)
                         {
                             var (serverIp, serverPort, Status, playCount, reviceTotal, sendTotal) = clientList[i].GetStatus();

                             table.UpdateCell(0, 0, $"[bold]{serverIp}[/]");
                             table.UpdateCell(0, 1, ($"[bold]{serverPort}[/]"));
                             table.UpdateCell(0, 2, ($"[bold]{Status}[/]"));
                             table.UpdateCell(0, 3, ($"[bold]{playCount}[/]"));
                             table.UpdateCell(0, 4, ($"[bold]{sendTotal}[/]"));
                             table.UpdateCell(0, 5, ($"[bold]{reviceTotal}[/]"));
                         }
                         ctx.Refresh();
                     }

                 });
        }

        static void PrintUsage()
        {
            AnsiConsole.WriteLine();
            using var logoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GameGate.logo.png");
            var logo = new CanvasImage(logoStream!)
            {
                MaxWidth = 25
            };

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

            var markup = new Markup(
                "[bold fuchsia]/s[/] [aqua]查看[/] 网关状况\n");
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
            table.AddRow(logo, rightTable);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

    }
}