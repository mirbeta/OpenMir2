using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NLog;

namespace SystemModule;

public abstract class TimerScheduledService : BackgroundService
{
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly PeriodicTimer _timer;
    private readonly Stopwatch _stopwatch;

    protected TimerScheduledService(TimeSpan timeSpan, string name)
    {
        Name = name;
        _stopwatch = new Stopwatch();
        _timer = new PeriodicTimer(timeSpan);
    }

    public string Name { get; }

    public long ElapsedMilliseconds { get; private set; }

    public bool StopOnException { get; set; }

    public bool CloseRequest = false;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Debug($"Thread [{Name}] has started");
        Startup(cancellationToken);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Stopping(cancellationToken);
        _timer.Dispose();
        _logger.Debug($"Thread [{Name}] has finished");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
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
                    _stopwatch.Reset();
                }
            }
        }
        catch (OperationCanceledException operationCancelledException)
        {
            _logger.Warn(operationCancelledException, "service stopped");
        }
    }

    public abstract void Initialize(CancellationToken cancellationToken);
    protected abstract Task ExecuteInternal(CancellationToken cancellationToken);
    protected abstract void Startup(CancellationToken cancellationToken);
    protected abstract void Stopping(CancellationToken cancellationToken);

    public override void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}