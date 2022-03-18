using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    class Program
    {
        private static PeriodicTimer _timer;
        private static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            ThreadPool.SetMaxThreads(200, 200);
            ThreadPool.GetMinThreads(out var workThreads, out var completionPortThreads);
            Console.WriteLine(new StringBuilder()
                .Append($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}, ")
                .Append($"Minimum work threads: {workThreads}, ")
                .Append($"Minimum completion port threads: {completionPortThreads})").ToString());
            
            PrintUsage();
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ServerApp>();
                    services.AddHostedService<TimedService>();
                    services.AddHostedService<AppService>();
                });
            Console.CancelKeyPress += Console_CancelKeyPress;
            await builder.StartAsync(cts.Token);
            ProcessLoopAsync();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            GateShare.ShowLog = true;
            if (_timer != null)
            {
                _timer.Dispose();
            }
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
                    return;
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

        private static async Task ShowServerStatus()
        {
            GateShare.ShowLog = false;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            var serverList = ServerManager.Instance.GetServerList();
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
                 .Cropping(VerticalOverflowCropping.Bottom)
                 .StartAsync(async ctx =>
                 {
                     foreach (var _ in Enumerable.Range(0, 10))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync(cts.Token))
                     {
                         for (int i = 0; i < serverList.Count; i++)
                         {
                             var (serverIp, serverPort, Status, playCount, reviceTotal, sendTotal) = serverList[i].GetStatus();

                             table.UpdateCell(i, 0, $"[bold]{serverIp}[/]");
                             table.UpdateCell(i, 1, ($"[bold]{serverPort}[/]"));
                             table.UpdateCell(i, 2, ($"[bold]{Status}[/]"));
                             table.UpdateCell(i, 3, ($"[bold]{playCount}[/]"));
                             table.UpdateCell(i, 4, ($"[bold]{sendTotal}[/]"));
                             table.UpdateCell(i, 5, ($"[bold]{reviceTotal}[/]"));
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