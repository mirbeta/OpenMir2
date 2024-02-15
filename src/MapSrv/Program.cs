using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapSrv
{
    internal class Program
    {
        private static readonly PeriodicTimer _timer;
        private static IHost _host;
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PrintUsage();
            Console.CancelKeyPress += delegate
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                AnsiConsole.Reset();
            };

            IConfigurationRoot config = new ConfigurationBuilder().Build();

            IHostBuilder builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<AppService>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                });

            _host = await builder.StartAsync(cts.Token);
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

                string firstTwoCharacters = input[..2];

                if (firstTwoCharacters switch
                {
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

        private static void PrintUsage()
        {
            AnsiConsole.WriteLine();

            Table table = new Table()
            {
                Border = TableBorder.None,
                Expand = true,
            }.HideHeaders();
            table.AddColumn(new TableColumn("One"));

            FigletText header = new FigletText("OpenMir2")
            {
                Color = Color.Fuchsia
            };
            FigletText header2 = new FigletText("LogDataSrv")
            {
                Color = Color.Aqua
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("[bold fuchsia]/s[/] [aqua]查看[/] 网关状况\n");
            sb.Append("[bold fuchsia]/r[/] [aqua]重读[/] 配置文件\n");
            sb.Append("[bold fuchsia]/c[/] [aqua]清空[/] 清除屏幕\n");
            sb.Append("[bold fuchsia]/q[/] [aqua]退出[/] 退出程序\n");
            Markup markup = new Markup(sb.ToString());

            table.AddColumn(new TableColumn("Two"));

            Table rightTable = new Table()
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