using GameSvr.Magic;
using GameSvr.Npc;
using McMaster.Extensions.CommandLineUtils;
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


            var aaa = new Ability();
            aaa.Level = 1;
            aaa.Reserved1 = 2;
            aaa.AC = 3;
            aaa.MAC = 4;
            aaa.DC = 5;
            aaa.MC = 6;
            aaa.SC = 7;
            aaa.HP = 8;
            aaa.MP = 9;
            aaa.MaxHP = 10;
            aaa.MaxMP = 11;
            aaa.ExpCount = 12;
            aaa.ExpMaxCount = 13;
            aaa.Exp = 14;
            aaa.MaxExp = 15;
            aaa.Weight = 16;
            aaa.MaxWeight = 17;
            aaa.WearWeight = 18;
            aaa.MaxWearWeight = 19;
            aaa.HandWeight = 20;
            aaa.MaxHandWeight = 21;
            var sss = EDCode.EncodeBuffer(aaa);

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