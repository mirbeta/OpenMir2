using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Spectre.Console;
using System.Reflection;
using System.Runtime;
using System.Text;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace GameSvr
{
    class Program
    {
        private static PeriodicTimer _timer;
        private static Logger _logger;
        private static IHost _host;
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            var ClientItem = new ClientStdItem();
            ClientItem.Name="召唤神兽召唤神兽召唤神兽";
            ClientItem.StdMode     =1;
            ClientItem. Shape 	   =2;
            ClientItem. Weight       =3;
            ClientItem. AniCount    =4;
            ClientItem.SpecialPwr  =5;
            ClientItem. ItemDesc     =6;
            ClientItem.Looks     =7;
            ClientItem.      DuraMax      =8;
            ClientItem. AC     =9;
            ClientItem.MAC     =10;
            ClientItem.DC    =11;
            ClientItem.MC     =12;
            ClientItem.SC     =13;
            ClientItem.Need      =14;
            ClientItem.NeedLevel      =15;
            ClientItem.NeedIdentify     =16;
            ClientItem.Price      =17;
            ClientItem.Stock     =18;
            ClientItem.AtkSpd       =19;
            ClientItem.Agility    =20;
            ClientItem.Accurate   =21;
            ClientItem.MgAvoid   =22;
            ClientItem.Strong   =23;
            ClientItem.Undead    =24;
            ClientItem.HpAdd     =25;
            ClientItem.MpAdd     =26;
            ClientItem.ExpAdd     =27;
            ClientItem.EffType1    =28;
            ClientItem. EffRate1    =29;
            ClientItem.EffValue1    =30;
            ClientItem. EffType2     =31;
            ClientItem. EffRate2     =32;
            ClientItem. EffValue2      =33;
            ClientItem. Slowdown   =34;
            ClientItem.Tox          =35;
            ClientItem.ToxAvoid       =36;
            ClientItem.UniqueItem    =37;
            ClientItem.OverlapItem   =38;
            ClientItem.Light     =39;
            ClientItem.ItemType  =40;
            ClientItem.ItemSet  =41;
            ClientItem.Reference = "test";
            var str = EDCode.EncodeBuffer(ClientItem);

            var config = new ConfigurationBuilder().Build();

            _logger = LogManager.Setup()
                .SetupExtensions(ext => ext.RegisterConfigSettings(config))
                .GetCurrentClassLogger();

            PrintUsage();

            //Console.CancelKeyPress += delegate
            //{
            //    //GateShare.ShowLog = true;
            //    if (_timer != null)
            //    {
            //        _timer.Dispose();
            //    }
            //    AnsiConsole.Reset();
            //};
            
            var builder = new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureHostOptions(options =>
                 {
                      options.ShutdownTimeout = TimeSpan.FromSeconds(30);
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<GameApp>();
                    services.AddSingleton<MirLog>();
                    services.AddHostedService<AppService>();
                    services.AddHostedService<TimedService>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    logging.AddNLog(config);
                });


            await builder.RunConsoleAsync(cts.Token);
            //await ProcessLoopAsync();
            //Stop();
        }

        static void Stop()
        {
            AnsiConsole.Status().Start("Disconnecting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
            });
        }
        
        static void PrintUsage()
        {
            AnsiConsole.WriteLine();
            using var logoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GameSvr.logo.png");
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
            var header2 = new FigletText("Game Svrver")
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
            table.AddRow(logo, rightTable);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

    }
}