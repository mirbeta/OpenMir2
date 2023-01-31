using GameSvr.Actor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Spectre.Console;
using System.Reflection;
using System.Text;
using SystemModule.Hosts;

namespace GameSvr
{
    public class AppServer : ServiceHost
    {
        private static readonly PeriodicTimer _timer;

        public AppServer()
        {
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GameApp>();
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
            M2Share.ActorMgr = new ActorMgr();
            await Builder.RunConsoleAsync(cancellationToken);
            //await ProcessLoopAsync();
            //Stop();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static void Stop()
        {
            AnsiConsole.Status().Start("Disconnecting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
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
                .AddEmptyRow();
            table.AddRow(rightTable);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

        public override void Dispose()
        {

        }
    }
}