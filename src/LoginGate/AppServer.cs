using SystemModule;

public class AppServer
{
    private readonly ServerHost _serverHost;
    private PeriodicTimer _timer;

    public AppServer()
    {
        _serverHost = new ServerHost();
        _serverHost.ConfigureServices(service =>
        {
            service.AddSingleton(new ConfigManager(Path.Combine(AppContext.BaseDirectory, "config.conf")));
            service.AddSingleton<ServerService>();
            service.AddSingleton<ClientManager>();
            service.AddSingleton<ClientThread>();
            service.AddSingleton<ServerManager>();
            service.AddSingleton<SessionManager>();
            service.AddHostedService<AppService>();
            service.AddHostedService<TimedService>();
        });
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        LogService.Debug("LoginGate is starting.");
        LogService.Info("正在启动服务...", 2);
        await _serverHost.StartAsync(cancellationToken);
        GateShare.ServiceProvider = _serverHost.ServiceProvider;
        await ProcessLoopAsync();
        Stop();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _serverHost.StopAsync(cancellationToken);
    }

    private void Stop()
    {
        AnsiConsole.Status().Start("Disconnecting...", ctx => { ctx.Spinner(Spinner.Known.Dots); });
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
        //GateShare.ShowLog = false;
        var periodicTimer = _timer ?? new PeriodicTimer(TimeSpan.FromSeconds(5));
        ClientManager clientManager = (ClientManager)GateShare.ServiceProvider.GetService(typeof(ClientManager));
        if (clientManager == null)
        {
            return;
        }

        ServerManager serverManager = (ServerManager)GateShare.ServiceProvider.GetService(typeof(ServerManager));
        if (serverManager == null)
        {
            return;
        }

        IList<ClientThread> clientList = clientManager.Clients;
        IList<ServerService> serverList = serverManager.GetServerList();
        Table table = new Table().Expand().BorderColor(Color.Grey);
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
                foreach (int _ in Enumerable.Range(0, 10))
                {
                    table.AddRow(new[]
                    {
                        new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"),
                        new Markup("-"), new Markup("-")
                    });
                }

                while (await periodicTimer.WaitForNextTickAsync())
                {
                    AnsiConsole.Clear();
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        (string remoteendpoint, string status, string playCount, string reviceTotal, string sendTotal, string threadCount) =
                            clientList[i].GetStatus();

                        table.UpdateCell(i, 0, $"[bold]{serverList[i].GetEndPoint()}[/]");
                        table.UpdateCell(i, 1, $"[bold]{remoteendpoint}[/]");
                        table.UpdateCell(i, 2, $"[bold]{status}[/]");
                        table.UpdateCell(i, 3, $"[bold]{playCount}[/]");
                        table.UpdateCell(i, 4, $"[bold]{sendTotal}[/]");
                        table.UpdateCell(i, 5, $"[bold]{reviceTotal}[/]");
                        table.UpdateCell(i, 6, $"[bold]{clientManager.GetQueueCount()}[/]");
                        table.UpdateCell(i, 7, $"[bold]{threadCount}[/]");
                    }

                    ctx.Refresh();
                }
            });
    }
}