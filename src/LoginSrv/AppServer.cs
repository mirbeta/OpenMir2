using LoginSvr.Conf;
using LoginSvr.Services;
using LoginSvr.Storage;
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
using SystemModule;
using SystemModule.Hosts;
using SystemModule.Logger;

namespace LoginSvr
{
    public class AppServer : ServiceHost
    {
        private PeriodicTimer _timer;

        public AppServer()
        {
            PrintUsage();
            Console.CancelKeyPress += delegate
            {
                LsShare.ShowLog = true;
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                AnsiConsole.Reset();
            };

            Builder.ConfigureLogging(ConfigureLogging);
            Builder.ConfigureServices(ConfigureServices);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MirLogger>();
            services.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "logsrv.conf")));
            services.AddSingleton<SessionServer>();
            services.AddSingleton<ClientSession>();
            services.AddSingleton<SessionManager>();
            services.AddSingleton<LoginServer>();
            services.AddSingleton<ClientManager>();
            services.AddSingleton<AccountStorage>();
            services.AddHostedService<TimedService>();
            services.AddHostedService<AppService>();
        }

        private void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddNLog(Configuration);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.Info("正在启动服务器...");
            Host = await Builder.StartAsync(cancellationToken);
            await ProcessLoopAsync();
            Stop();
            Logger.Info("正在等待服务器连接...");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {

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
                    await Exit();
                    return;
                }
                if (input.Length < 2)
                {
                    continue;
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
            LsShare.ShowLog = false;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            var masSocService = (SessionServer)Host.Services.GetService(typeof(SessionServer));
            var serverList = masSocService?.ServerList;
            var table = new Table().Expand().BorderColor(Color.Grey);
            table.AddColumn("[yellow]Server[/]");
            table.AddColumn("[yellow]EndPoint[/]");
            table.AddColumn("[yellow]Status[/]");
            table.AddColumn("[yellow]Online[/]");

            await AnsiConsole.Live(table)
                 .AutoClear(false)
                 .Overflow(VerticalOverflow.Ellipsis)
                 .Cropping(VerticalOverflowCropping.Top)
                 .StartAsync(async ctx =>
                 {
                     foreach (var _ in Enumerable.Range(0, 10))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await _timer.WaitForNextTickAsync())
                     {
                         for (int i = 0; i < serverList.Count; i++)
                         {
                             var msgServer = serverList[i];
                             if (!string.IsNullOrEmpty(msgServer.ServerName))
                             {
                                 var serverType = msgServer.ServerIndex == 99 ? " (DB)" : " (GameSrv)";
                                 table.UpdateCell(i, 0, $"[bold]{msgServer.ServerName}{serverType}[/]");
                                 table.UpdateCell(i, 1, ($"[bold]{msgServer.EndPoint}[/]"));
                                 if (!msgServer.Socket.Connected)
                                 {
                                     table.UpdateCell(i, 2, ($"[red]Not Connected[/]"));
                                 }
                                 else if ((HUtil32.GetTickCount() - msgServer.KeepAliveTick) < 30000)
                                 {
                                     table.UpdateCell(i, 2, ($"[green]Connected[/]"));
                                 }
                                 else
                                 {
                                     table.UpdateCell(i, 2, ($"[red]Timeout[/]"));
                                 }
                             }
                             table.UpdateCell(i, 3, ($"[bold]{msgServer.OnlineCount}[/]"));
                         }
                         ctx.Refresh();
                     }
                 });
        }

        private void PrintUsage()
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
            var header2 = new FigletText("LoginSrv")
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