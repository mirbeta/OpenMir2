using DBSrv.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using OpenMir2;

namespace DBSrv
{
    public class AppService : IHostedService
    {
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
            _marketService = serviceProvider.GetService<MarketService>();
            _sessionService = serviceProvider.GetService<ClientSession>();
            _host = serviceProvider.GetService<IHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationToken.Register(() => LogService.Debug("DBSrv is stopping."));
            LogService.Debug("DBSrv is starting.");
            _appLifetime.ApplicationStarted.Register(() =>
            {
                LogService.Debug("Application has started");
                _applicationTask = Task.Run(() =>
                {
                    try
                    {
                        _userService.Initialize();
                        _dataService.Initialize();
                        _marketService.Initialize();
                        _userService.Start();
                        _dataService.Start();
                        _marketService.Start();
                        _sessionService.Start();
                        _exitCode = 0;
                    }
                    catch (TaskCanceledException)
                    {
                        // This means the application is shutting down, so just swallow this exception
                    }
                    catch (Exception ex)
                    {
                        LogService.Error(ex);
                        LogService.Error(ex.StackTrace);
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
            _dataService.Stop();
            _marketService.Stop();
            _sessionService.Stop();
            if (_applicationTask != null)
            {
                await _applicationTask;
            }
            LogService.Debug("DBSrv is stopping.");
            LogService.Debug($"Exiting with return code: {_exitCode}");
            _appLifetime.StopApplication();
            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.Exit(Environment.ExitCode);
        }

        private async void OnShutdown()
        {
            LogService.Debug("Application is stopping");
            LogService.Info("数据引擎世界服务已停止...");
            LogService.Info("数据服务已停止...");
            LogService.Info("goodbye!");
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