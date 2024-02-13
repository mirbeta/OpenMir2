using LoginSrv.Conf;
using LoginSrv.Services;
using LoginSrv.Storage;
using Microsoft.Extensions.DependencyInjection;
using OpenMir2;
using Spectre.Console;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginSrv
{
    public class AppServer
    {
        private readonly ServerHost _serverHost;
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

            _serverHost = new ServerHost();
            _serverHost.ConfigureServices(service =>
            {
                service.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "logsrv.conf")));
                service.AddSingleton<SessionServer>();
                service.AddSingleton<ClientSession>();
                service.AddSingleton<SessionManager>();
                service.AddSingleton<LoginServer>();
                service.AddSingleton<ClientManager>();
                service.AddSingleton<AccountStorage>();
                service.AddHostedService<TimedService>();
                service.AddHostedService<AppService>();
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LogService.Info("正在启动服务器...");
            _serverHost.BuildHost();
            await _serverHost.StartAsync(cancellationToken);
            await ProcessLoopAsync();
            Stop();
            LogService.Info("正在等待服务器连接...");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           return _serverHost.StopAsync(cancellationToken);
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
                string firstTwoCharacters = input[..2];

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
            var periodicTimer = _timer ?? new PeriodicTimer(TimeSpan.FromSeconds(5));
            SessionServer masSocService = (SessionServer)_serverHost.ServiceProvider.GetService(typeof(SessionServer));
            System.Collections.Generic.IList<ServerSessionInfo> serverList = masSocService?.ServerList;
            Table table = new Table().Expand().BorderColor(Color.Grey);
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
                     foreach (int _ in Enumerable.Range(0, 10))
                     {
                         table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
                     }

                     while (await periodicTimer.WaitForNextTickAsync())
                     {
                         for (int i = 0; i < serverList.Count; i++)
                         {
                             ServerSessionInfo msgServer = serverList[i];
                             if (!string.IsNullOrEmpty(msgServer.ServerName))
                             {
                                 string serverType = msgServer.ServerIndex == 99 ? " (DB)" : " (GameSrv)";
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
            FigletText header2 = new FigletText("LoginSrv")
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