using LoginGate.Conf;
using LoginGate.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Spectre.Console;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemModule.Hosts;
using SystemModule.Logger;

namespace LoginGate
{
    public class AppServer : ServiceHost
    {
        private static PeriodicTimer _timer;

        public AppServer()
        {
            PrintUsage();
            Console.CancelKeyPress += delegate
            {
                //GateShare.ShowLog = true;
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
                AnsiConsole.Reset();
            };
            Builder.ConfigureLogging(ConfigureLogging);
            Builder.ConfigureServices(ConfigureServices);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "config.conf")));
            services.AddSingleton<ServerService>();
            services.AddSingleton<ClientManager>();
            services.AddSingleton<ClientThread>();
            services.AddSingleton<ServerManager>();
            services.AddSingleton<SessionManager>();
            services.AddHostedService<AppService>();
            services.AddHostedService<TimedService>();
        }

        private void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddNLog(Configuration);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.Debug("LoginGate is starting.");
            Logger.Info("正在启动服务...", 2);
            Host = await Builder.StartAsync(cancellationToken);
            await ProcessLoopAsync();
            Stop();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void Stop()
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
                if (input.Length < 2)
                {
                    return;
                }
                var firstTwoCharacters = input[..2];
                if (firstTwoCharacters switch
                {
                    "/s" => ShowServerStatus(),
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

        private async Task ShowServerStatus()
        {
            //GateShare.ShowLog = false;
            if (_timer == null)
            {
                _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            }
            var clientManager = (ClientManager)Host.Services.GetService(typeof(ClientManager));
            if (clientManager == null)
            {
                return;
            }
            var serverManager = (ServerManager)Host.Services.GetService(typeof(ServerManager));
            if (serverManager == null)
            {
                return;
            }
            var clientList = clientManager.Clients;
            var serverList = serverManager.GetServerList();
            var table = new Table().Expand().BorderColor(Color.Grey);
            table.AddColumn("[yellow]LocalEndPoint[/]");
            table.AddColumn("[yellow]RemoteEndPoint[/]");
            table.AddColumn("[yellow]Status[/]");
            table.AddColumn("[yellow]Online[/]");
            table.AddColumn("[yellow]Send[/]");
            table.AddColumn("[yellow]Revice[/]");
            table.AddColumn("[yellow]Queue[/]");
            table.AddColumn("[yellow]Thread[/]");

            await AnsiConsole.Live(table)
                .AutoClear(true)
                .Overflow(VerticalOverflow.Crop)
                .Cropping(VerticalOverflowCropping.Bottom)
                .StartAsync(async ctx =>
                {
                    foreach (var _ in Enumerable.Range(0, 10))
                    {
                        table.AddRow(new[]
                        {
                            new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-")
                        });
                    }

                    while (await _timer.WaitForNextTickAsync())
                    {
                        AnsiConsole.Clear();
                        for (int i = 0; i < clientList.Count; i++)
                        {
                            var (remoteendpoint, status, playCount, reviceTotal, sendTotal, threadCount) = clientList[i].GetStatus();

                            table.UpdateCell(i, 0, $"[bold]{serverList[i].EndPoint}[/]");
                            table.UpdateCell(i, 1, $"[bold]{remoteendpoint}[/]");
                            table.UpdateCell(i, 2, ($"[bold]{status}[/]"));
                            table.UpdateCell(i, 3, ($"[bold]{playCount}[/]"));
                            table.UpdateCell(i, 4, ($"[bold]{sendTotal}[/]"));
                            table.UpdateCell(i, 5, ($"[bold]{reviceTotal}[/]"));
                            table.UpdateCell(i, 6, ($"[bold]{clientManager.GetQueueCount()}[/]"));
                            table.UpdateCell(i, 7, ($"[bold]{threadCount}[/]"));
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
            var header2 = new FigletText("Login Gate")
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

        public override void Dispose()
        {

        }
    }
}