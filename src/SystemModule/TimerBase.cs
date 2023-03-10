using NLog;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SystemModule
{
    public abstract class TimerBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private CancellationToken m_cancellationToken = CancellationToken.None;
        private Timer m_timer;
        private string m_name;
        protected int m_interval = 1000;

        protected TimerBase(int intervalMs, string name)
        {
            m_name = name;
            m_interval = intervalMs;

            m_timer = new Timer
            {
                Interval = intervalMs,
                AutoReset = false
            };
            m_timer.Elapsed += TimerOnElapse;
            m_timer.Disposed += TimerOnDisposed;
        }

        public string Name => m_name;

        public long ElapsedMilliseconds { get; private set; }

        public bool StopOnException { get; set; }

        public bool CloseRequest = false;

        public async Task StartAsync()
        {
            await OnStartAsync();

            m_timer.Start();
        }

        public Task CloseAsync()
        {
            m_timer.Stop();
            m_cancellationToken = new CancellationToken(true);
            return Task.CompletedTask;
        }

        private async void TimerOnDisposed(object sender, EventArgs e)
        {
            await OnCloseAsync();
        }

        private async void TimerOnElapse(object sender, ElapsedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                await OnElapseAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception thrown on [{Name}] thread!!!");
                _logger.Error(ex);
            }
            finally
            {
                m_timer.Enabled = !m_cancellationToken.IsCancellationRequested;
                sw.Stop();
                ElapsedMilliseconds = sw.ElapsedMilliseconds;
            }
        }

        protected virtual async Task OnStartAsync()
        {
            _logger.Info($"Timer [{m_name}] has started");
        }

        protected virtual async Task<bool> OnElapseAsync()
        {
            _logger.Info($"Timer [{m_name}] has elapsed at {DateTime.Now}");
            return true;
        }

        protected virtual async Task OnCloseAsync()
        {
            _logger.Info($"Timer [{m_name}] has finished");
        }
    }
}