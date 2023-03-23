using System;
using System.Threading;
using System.Threading.Tasks;
using DBSrv.Services;
using DBSrv.Services.Impl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace DBSrv
{
    public class AppService : IHostedService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IHost _host;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly UserService _userService;
        private readonly ClientSession _sessionService;
        private readonly DataService _dataService;
        private readonly MarketService _marketService;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _applicationTask;
        private int _exitCode;

        public AppService(IHostApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
            _appLifetime = lifetime;
            _userService = serviceProvider.GetService<UserService>();
            _dataService = serviceProvider.GetService<DataService>();
            _marketService =serviceProvider.GetService<MarketService>();
            _sessionService = serviceProvider.GetService<ClientSession>();
            _host = serviceProvider.GetService<IHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationToken.Register(() => _logger.Debug("DBSrv is stopping."));
            _logger.Debug("DBSrv is starting.");
            _appLifetime.ApplicationStarted.Register(() =>
            {
                _logger.Debug("Application has started");
                _applicationTask = Task.Run(() =>
                {
                    try
                    {
                        _userService.Start();
                        _sessionService.Start();
                        _dataService.Start();
                        _marketService.Start();
                        _exitCode = 0;
                    }
                    catch (TaskCanceledException)
                    {
                        // This means the application is shutting down, so just swallow this exception
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Unhandled exception!");
                        _logger.Error(ex.StackTrace);
                        _exitCode = 1;
                    }
                }, cancellationToken);
                ProcessLoopAsync();
            });
            _appLifetime.ApplicationStopping.Register(OnShutdown);
            
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _userService.Stop();
            if (_applicationTask != null)
            {
                await _applicationTask;
            }
            _logger.Debug("DBSrv is stopping.");
            _logger.Debug($"Exiting with return code: {_exitCode}");
            _appLifetime.StopApplication();
            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.Exit(Environment.ExitCode);
        }

        private async void OnShutdown()
        {
            _logger.Debug("Application is stopping");
            _logger.Info("数据引擎世界服务已停止...");
            _logger.Info("数据服务已停止...");
            _logger.Info("goodbye!");
            _cancellationTokenSource.CancelAfter(3000);
            await _host.StopAsync(_cancellationTokenSource.Token);
        }
        
        private static void ProcessLoopAsync()
        {
            while (true)
            {
                var cmdline = Console.ReadLine();
                if (string.IsNullOrEmpty(cmdline))
                {
                    continue;
                }
                try
                {
                    //_application.Execute(cmdline);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}