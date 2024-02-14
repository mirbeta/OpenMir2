using LoginSrv.Conf;
using LoginSrv.Services;
using LoginSrv.Storage;
using SystemModule;

namespace LoginSrv
{
    public class AppServer
    {
        private readonly ServerHost _serverHost;
        private readonly PeriodicTimer _timer;

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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _serverHost.StopAsync(cancellationToken);
        }

        private async Task ShowServerStatus()
        {
            LsShare.ShowLog = false;
            PeriodicTimer periodicTimer = _timer ?? new PeriodicTimer(TimeSpan.FromSeconds(5));
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
            Console.WriteLine(@"    ___                           __  __   _          ____          ");
            Console.WriteLine(@"   / _ \   _ __     ___   _ __   |  \/  | (_)  _ __  |___ \         ");
            Console.WriteLine(@"  | | | | | '_ \   / _ \ | '_ \  | |\/| | | | | '__|   __) |        ");
            Console.WriteLine(@"  | |_| | | |_) | |  __/ | | | | | |  | | | | | |     / __/         ");
            Console.WriteLine(@"   \___/  | .__/   \___| |_| |_| |_|  |_| |_| |_|    |_____|        ");
            Console.WriteLine(@"          |_|                                                       ");
            Console.WriteLine(@"   _                       _           ____                         ");
            Console.WriteLine(@"  | |       ___     __ _  (_)  _ __   / ___|   _ __  __   __        ");
            Console.WriteLine(@"  | |      / _ \   / _` | | | | '_ \  \___ \  | '__| \ \ / /        ");
            Console.WriteLine(@"  | |___  | (_) | | (_| | | | | | | |  ___) | | |     \ V /         ");
            Console.WriteLine(@"  |_____|  \___/   \__, | |_| |_| |_| |____/  |_|      \_/          ");
            Console.WriteLine(@"                   |___/                                            ");
            Console.WriteLine(@"                                                                    ");
        }
    }
}