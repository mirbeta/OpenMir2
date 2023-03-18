using System;
using System.Threading;
using System.Threading.Tasks;
using DBSrv.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace DBSrv
{
    public class AppService : IHostedService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IHost Host;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly UserService _userSocService;
        private readonly SessionService _loginSvrService;
        private readonly DataService _dataService;
        private readonly MarketService _marketService;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _applicationTask;
        private int _exitCode;

        public AppService(IHostApplicationLifetime lifetime, IServiceProvider serviceProvider, UserService userService, SessionService loginSession,
            DataService dataService, MarketService marketService)
        {
            _appLifetime = lifetime;
            _userSocService = userService;
            _loginSvrService = loginSession;
            _dataService = dataService;
            _marketService = marketService;
            Host = serviceProvider.GetService<IHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationToken.Register(() => logger.Debug("DBSrv is stopping."));
            logger.Debug("DBSrv is starting.");
            _appLifetime.ApplicationStarted.Register(() =>
            {
                logger.Debug("Application has started");
                _applicationTask = Task.Run(() =>
                {
                    try
                    {
                        _userSocService.Start(cancellationToken);
                        _loginSvrService.Start();
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
                        logger.Error(ex, "Unhandled exception!");
                        logger.Error(ex.StackTrace);
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
            _userSocService.Stop();
            if (_applicationTask != null)
            {
                await _applicationTask;
            }
            logger.Debug("DBSrv is stopping.");
            logger.Debug($"Exiting with return code: {_exitCode}");
            _appLifetime.StopApplication();
            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.Exit(Environment.ExitCode);
        }

        private async void OnShutdown()
        {
            logger.Debug("Application is stopping");
            logger.Info("数据引擎世界服务已停止...");
            logger.Info("数据服务已停止...");
            logger.Info("goodbye!");
            _cancellationTokenSource.CancelAfter(3000);
            await Host.StopAsync(_cancellationTokenSource.Token);
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