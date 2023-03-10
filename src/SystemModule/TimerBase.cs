using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace SystemModule
{
    public abstract class TimerBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private CancellationToken m_cancellationToken = CancellationToken.None;
        //private readonly PeriodicTimer m_timer;
        private readonly Thread _thread;
        private readonly string m_name;
        protected int m_interval = 1000;
        protected Stopwatch sw;


        protected TimerBase(int intervalMs, string name)
        {
            m_name = name;
            m_interval = intervalMs;
            sw = new Stopwatch();
            //m_timer = new PeriodicTimer(TimeSpan.FromMilliseconds(intervalMs));
            _thread = new Thread(TimerOnElapse) { IsBackground = true };
        }

        public string Name => m_name;

        public long ElapsedMilliseconds { get; private set; }

        public bool StopOnException { get; set; }

        public bool CloseRequest = false;

        public void StartAsync()
        {
            //OnStartAsync();
            _thread.Start();
            /*while (await m_timer.WaitForNextTickAsync())
            {
                TimerOnElapse();
            }*/
        }

        public Task CloseAsync()
        {
            //m_timer.Dispose();
            m_cancellationToken = new CancellationToken(true);
            return Task.CompletedTask;
        }

        private void TimerOnDisposed(object sender, EventArgs e)
        {
            OnCloseAsync();
        }

        private void TimerOnElapse()
        {
            while (true)
            {
                sw.Start();
                try
                {
                    OnElapseAsync();
                }
                catch (Exception ex)
                {
                    _logger.Error($"Exception thrown on [{Name}] thread!!!");
                    _logger.Error(ex);
                }
                finally
                {
                    sw.Stop();
                    ElapsedMilliseconds = sw.ElapsedMilliseconds;
                }
                Thread.SpinWait(m_interval);
            }
        }

        protected virtual void OnStartAsync()
        {
            _logger.Info($"Timer [{m_name}] has started");
        }

        protected virtual bool OnElapseAsync()
        {
            _logger.Info($"Timer [{m_name}] has elapsed at {DateTime.Now}");
            return true;
        }

        protected virtual void OnCloseAsync()
        {
            _logger.Info($"Timer [{m_name}] has finished");
        }
    }
}