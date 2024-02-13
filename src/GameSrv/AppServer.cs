using GameSrv.Module;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;
using IModuleInitializer = SystemModule.IModuleInitializer;

namespace GameSrv
{
    public class AppServer
    {
        private readonly IHost _host;

        public AppServer()
        {
            PrintUsage();

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddModules();
            builder.Services.AddSingleton<GameApp>();
            builder.Services.AddHostedService<AppService>();
            builder.Services.AddHostedService<TimedService>();
            builder.Services.AddScoped<IMediator, Mediator>();
            foreach (ModuleInfo module in GameShare.Modules)
            {
                Type moduleInitializerType = module.Assembly.GetTypes().FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                {
                    IModuleInitializer moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    builder.Services.AddSingleton(typeof(IModuleInitializer), moduleInitializer);
                    moduleInitializer.ConfigureServices(builder.Services);
                }
            }

            builder.Logging.ClearProviders();
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddSerilog();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            LogService.Logger = Log.Logger;

            _host = builder.Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _host.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _host.StopAsync(cancellationToken);
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
            FigletText header2 = new FigletText("Game Server")
            {
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
            AnsiConsole.Write(new Rule($"[green3] Version:{MessageSettings.Version} UpdateTime:{MessageSettings.UpDateTime}[/]").RuleStyle("grey").LeftJustified());
        }
    }
}