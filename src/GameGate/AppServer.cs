using GameGate;
using SystemModule;

public class AppServer
{
    private readonly ServerHost _serverHost;
    private PeriodicTimer _timer;

    public AppServer()
    {
        PrintUsage();
        _serverHost = new ServerHost();
        _serverHost.ConfigureServices(service =>
        {
            service.AddHostedService<AppService>();
            service.AddHostedService<TimedService>();
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

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        LogService.Info("正在启动服务...");
        _serverHost.BuildHost();
        GateShare.ServiceProvider = _serverHost.ServiceProvider;
        await _serverHost.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _serverHost.StopAsync(cancellationToken);
    }

    private static async Task ShowServerStatus()
    {
        //GateShare.ShowLog = false;
        //_timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
        //ServerService[] serverList = ServerManager.Instance.GetServerList();
        //Table table = new Table().Expand().BorderColor(Color.Grey);
        //table.AddColumn("[yellow]Id[/]");
        //table.AddColumn("[yellow]EndPoint[/]");
        //table.AddColumn("[yellow]Status[/]");
        //table.AddColumn("[yellow]Online[/]");
        //table.AddColumn("[yellow]Send[/]");
        //table.AddColumn("[yellow]Revice[/]");
        //table.AddColumn("[yellow]Total Send[/]");
        //table.AddColumn("[yellow]Total Revice[/]");
        //table.AddColumn("[yellow]Queue[/]");
        //table.AddColumn("[yellow]WorkThread[/]");

        //await AnsiConsole.Live(table)
        //     .AutoClear(true)
        //     .Overflow(VerticalOverflow.Crop)
        //     .Cropping(VerticalOverflowCropping.Bottom)
        //     .StartAsync(async ctx =>
        //     {
        //         foreach (int _ in Enumerable.Range(0, serverList.Length))
        //         {
        //             table.AddRow(new[] { new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-"), new Markup("-") });
        //         }

        //         while (await _timer.WaitForNextTickAsync(CancellationToken.Token))
        //         {
        //             for (int i = 0; i < serverList.Length; i++)
        //             {
        //                 (string endPoint, string status, string playCount, string reviceTotal, string sendTotal, string totalrevice, string totalSend, string queueCount, int threads) = serverList[i].GetStatus();

        //                 table.UpdateCell(i, 0, $"[bold]{endPoint}[/]");
        //                 table.UpdateCell(i, 1, $"[bold]{status}[/]");
        //                 table.UpdateCell(i, 2, $"[bold]{playCount}[/]");
        //                 table.UpdateCell(i, 3, $"[bold]{sendTotal}[/]");
        //                 table.UpdateCell(i, 4, $"[bold]{reviceTotal}[/]");
        //                 table.UpdateCell(i, 5, $"[bold]{totalSend}[/]");
        //                 table.UpdateCell(i, 6, $"[bold]{totalrevice}[/]");
        //                 table.UpdateCell(i, 7, $"[bold]{queueCount}[/]");
        //                 table.UpdateCell(i, 8, $"[bold]{threads}[/]");
        //             }
        //             ctx.Refresh();
        //         }
        //     });
    }
}