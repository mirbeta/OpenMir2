using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Spectre.Console;
using SystemModule.Hosts;

namespace GameSrv {
    public class AppServer : ServiceHost {
        private static readonly PeriodicTimer _timer;

        public AppServer() {
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

            Builder.ConfigureLogging(ConfigureLogging);
            Builder.ConfigureServices(ConfigureServices);
        }

        public static void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<GameApp>();
            services.AddHostedService<AppService>();
            services.AddHostedService<TimedService>();
        }

        private void ConfigureLogging(ILoggingBuilder logging) {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddNLog(Configuration);
        }

        public override async Task StartAsync(CancellationToken cancellationToken) {
            await Builder.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken) {
            Host.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }

        private static void Stop() {
            AnsiConsole.Status().Start("Disconnecting...", ctx => {
                ctx.Spinner(Spinner.Known.Dots);
            });
        }

        private static void PrintUsage() {
            AnsiConsole.WriteLine();

            Table table = new Table() {
                Border = TableBorder.None,
                Expand = true,
            }.HideHeaders();
            table.AddColumn(new TableColumn("One"));

            FigletText header = new FigletText("OpenMir2") {
                Color = Color.Fuchsia
            };
            FigletText header2 = new FigletText("Game Svrver") {
                Color = Color.Aqua
            };

            table.AddColumn(new TableColumn("Two"));

            Table rightTable = new Table()
                .HideHeaders()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn("Content"));

            rightTable.AddRow(header)
                .AddRow(header2)
                .AddEmptyRow()
                .AddEmptyRow();
            table.AddRow(rightTable);

            AnsiConsole.Write(table);

            AnsiConsole.Write(new Rule($"[green3] Free open source, OpenMir2 creates unlimited possibilities.[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.Write(new Rule($"[green3] Version:{Settings.Version} UpdateTime:{Settings.UpDateTime}[/]").RuleStyle("grey").LeftJustified());
        }

        public override void Dispose() {

        }
    }
}