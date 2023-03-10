using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace SystemModule
{
    public abstract class TimerScheduledService : BackgroundService, IDisposable
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private PeriodicTimer m_timer;
        private readonly string m_name;
        private readonly Stopwatch _stopwatch;

        protected TimerScheduledService(TimeSpan timeSpan, string name)
        {
            m_name = name;
            _stopwatch = new Stopwatch();
            m_timer = new PeriodicTimer(timeSpan);
        }

        public string Name => m_name;

        public long ElapsedMilliseconds { get; private set; }

        public bool StopOnException { get; set; }

        public bool CloseRequest = false;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info($"Timer [{m_name}] has started");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            m_timer.Dispose();
            _logger.Info($"Timer [{m_name}] has finished");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await m_timer.WaitForNextTickAsync(stoppingToken))
                {
                    _stopwatch.Start();
                    try
                    {
                        await ExecuteInternal(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Execute exception");
                    }
                    finally
                    {
                        _stopwatch.Stop();
                        ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
                    }
                }
            }
            catch (OperationCanceledException operationCancelledException)
            {
                _logger.Warn(operationCancelledException, "service stopped");
            }
        }

        protected abstract Task ExecuteInternal(CancellationToken stoppingToken);
        
        public void Dispose()
        {
            m_timer?.Dispose();
        }
    }
}