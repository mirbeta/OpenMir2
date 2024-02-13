using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using OpenMir2;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule
{
    public class ServerHost
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly IConfiguration _configuration;

        public ServerHost()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            _hostBuilder = CreateHost();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
            LogService.Logger = Log.Logger;
        }

        public void ConfigureServices(Action<IServiceCollection> configureServices)
        {
            _hostBuilder.ConfigureServices(configureServices);
        }

        private IHostBuilder CreateHost()
        {
            return Host.CreateDefaultBuilder()
                .UseConsoleLifetime()
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddSerilog();
                    logging.AddConsole();
                });
        }

        private IHost AppHost { get; set; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get { return _configuration; } }

        public void BuildHost()
        {
            AppHost = _hostBuilder.Build();
            ServiceProvider = AppHost.Services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            PrintUsage();
            await AppHost.StartAsync(cancellationToken);
            await AppHost.WaitForShutdownAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await AppHost.StopAsync(cancellationToken);
        }

        private static void PrintUsage()
        {
            //AnsiConsole.WriteLine();

            //Table table = new Table()
            //{
            //    Border = TableBorder.None,
            //    Expand = true,
            //}.HideHeaders();
            //table.AddColumn(new TableColumn("One"));

            //FigletText header = new FigletText("OpenMir2")
            //{
            //    Color = Color.Fuchsia
            //};
            //FigletText header2 = new FigletText("Game Server")
            //{
            //    Color = Color.Aqua
            //};

            //table.AddColumn(new TableColumn("Two"));

            //Table rightTable = new Table()
            //    .HideHeaders()
            //    .Border(TableBorder.None)
            //    .AddColumn(new TableColumn("Content"));

            //rightTable.AddRow(header)
            //    .AddRow(header2)
            //    .AddEmptyRow()
            //    .AddEmptyRow();
            //table.AddRow(rightTable);

            //AnsiConsole.Write(table);

            //AnsiConsole.Write(new Rule($"[green3] Free open source, OpenMir2 creates unlimited possibilities.[/]").RuleStyle("grey").LeftJustified());
            //AnsiConsole.Write(new Rule($"[green3] Version:{MessageSettings.Version} UpdateTime:{MessageSettings.UpDateTime}[/]").RuleStyle("grey").LeftJustified());
        }

    }
}